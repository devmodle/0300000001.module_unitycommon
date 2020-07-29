using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if !UNITY_EDITOR
using UnityEngine.Scripting;
#endif			// #if !UNITY_EDITOR

#if UNITY_IOS
using UnityEngine.iOS;
#endif			// #if UNITY_IOS

//! 초기화 씬 관리자
public abstract partial class CInitSceneManager : CSceneManager {
	#region 클래스 객체
	private static GameObject m_oBlindUI = null;
	#endregion			// 클래스 객체

	#region 프로퍼티
	public override string SceneName => KCDefine.B_SCENE_NAME_INIT;

#if UNITY_EDITOR
	public override int ScriptOrder => KCDefine.U_SCRIPT_ORDER_INIT_SCENE_MANAGER;
#endif			// #if UNITY_EDITOR
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public sealed override void Start() {
		base.Start();
		StartCoroutine(this.OnStart());
	}

	//! 씬을 설정한다
	protected virtual void Setup() {
		// 테이블을 로드한다
		CValueTable.Instance.LoadValuesFromRes(KCDefine.U_TABLE_PATH_G_COMMON_VALUE_TABLE);
		CStringTable.Instance.LoadStringsFromRes(KCDefine.U_TABLE_PATH_G_COMMON_STRING_TABLE);

		// 디바이스 정보를 설정한다 {
		int nQualityLevel = CValueTable.Instance.GetInt(KCDefine.VT_KEY_QUALITY_LEVEL);
		int nTargetFrameRate = CValueTable.Instance.GetInt(KCDefine.VT_KEY_DESKTOP_TARGET_FRAME_RATE);

		if(CAccess.IsDesktopPlatform()) {
			Screen.SetResolution(KCDefine.B_DESKTOP_WINDOW_WIDTH, 
				KCDefine.B_DESKTOP_WINDOW_HEIGHT, FullScreenMode.Windowed);
		} else {
			if(CAccess.IsMobilePlatform()) {
				nTargetFrameRate = CValueTable.Instance.GetInt(KCDefine.VT_KEY_MOBILE_TARGET_FRAME_RATE);
			} else if(CAccess.IsConsolePlatform()) {
				nTargetFrameRate = CValueTable.Instance.GetInt(KCDefine.VT_KEY_CONSOLE_TARGET_FRAME_RATE);
			} else {
				nTargetFrameRate = CValueTable.Instance.GetInt(KCDefine.VT_KEY_HANDHELD_CONSOLE_TARGET_FRAME_RATE);
			}

			var stScreenSize = CAccess.GetDeviceScreenSize(Application.isPlaying);
			Screen.SetResolution((int)stScreenSize.x, (int)stScreenSize.y, true);
		}

		CFunc.SetupQuality(nTargetFrameRate,
			CValueTable.Instance.GetBool(KCDefine.VT_KEY_MULTI_TOUCH_ENABLE), (EQualityLevel)nQualityLevel);
		// 디바이스 정보를 설정한다 }

		this.SetupBlindUI();

#if !UNITY_EDITOR
		GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
#endif			// #if !UNITY_EDITOR

		// 저장소를 로드한다 {
#if MSG_PACK_ENABLE
		CUserInfoStorage.Instance.LoadUserInfo(KCDefine.B_DATA_PATH_USER_INFO);
#endif			// #if MSG_PACK_ENABLE
		// 저장소를 로드한다 }

		// 사운드를 설정한다 {
		CSndManager.Instance.BGSndVolume = 1.0f;
		CSndManager.Instance.FXSndsVolume = 1.0f;

#if MSG_PACK_ENABLE
		CSndManager.Instance.IsMuteBGSnd = CUserInfoStorage.Instance.UserInfo.IsMuteBGSnd;
		CSndManager.Instance.IsMuteFXSnds = CUserInfoStorage.Instance.UserInfo.IsMuteFXSnds;
		CSndManager.Instance.IsDisableVibrate = CUserInfoStorage.Instance.UserInfo.IsDisableVibrate;
#endif			// #if MSG_PACK_ENABLE
		// 사운드를 설정한다 }
	}

	//! 초기화
	private IEnumerator OnStart() {
		if(!CSceneManager.IsInit) {
			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
			CActivityIndicatorManager.Instance.StartActivityIndicator(true, false);

			// 간격을 설정한다
			this.SetupOffsets();
			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);

			// iOS 플랫폼을 설정한다 {
#if UNITY_IOS
			Device.SetNoBackupFlag(KCDefine.B_DIR_PATH_WRITABLE);
			Device.SetNoBackupFlag(KCDefine.U_IMG_PATH_SCREENSHOT);

			Device.hideHomeButton = false;
			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
#endif			// #if UNITY_IOS
			// iOS 플랫폼을 설정한다 }

			// 관리자를 생성한다 {
			CLogManager.Create();
			CSndManager.Create();
			CResManager.Create();
			CTaskManager.Create();
			CScheduleManager.Create();
			CNavStackManager.Create();
			CToastPopupManager.Create();
			CActivityIndicatorManager.Create();

#if UNITY_ANDROID
			CPermissionManager.Create();
#endif			// #if UNITY_ANDROID

#if ADS_ENABLE
			CAdsManager.Create();
#endif			// #if ADS_ENABLE

#if TENJIN_ENABLE
			CTenjinManager.Create();
#endif			// #if TENJIN_ENABLE

#if FLURRY_ENABLE
			CFlurryManager.Create();
#endif			// #if FLURRY_ENABLE

#if FACEBOOK_ENABLE
			CFacebookManager.Create();
#endif			// #if FACEBOOK_ENABLE

#if FIREBASE_ENABLE
			CFirebaseManager.Create();
#endif			// #if FIREBASE_ENABLE

#if GAME_CENTER_ENABLE
			CGameCenterManager.Create();
#endif			// #if GAME_CENTER_ENABLE

#if PURCHASE_ENABLE && MSG_PACK_ENABLE
			CPurchaseManager.Create();
#endif			// #if PURCHASE_ENABLE && MSG_PACK_ENABLE

#if UNITY_SERVICE_ENABLE
			CUnityServiceManager.Create();
#endif			// #if UNITY_SERVICE_ENABLE

			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
			// 관리자를 생성한다 }

			// 디바이스 연동 객체를 생성한다 {
			CUnityMsgSender.Create();
			CDeviceMsgReceiver.Create();

			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
			// 디바이스 연동 객체를 생성한다 }

			// 테이블을 생성한다 {
			CValueTable.Create();
			CStringTable.Create();

			CBuildInfoTable.Create(KCDefine.U_SCRIPTABLE_PATH_G_BUILD_INFO_TABLE);
			CBuildOptionTable.Create(KCDefine.U_SCRIPTABLE_PATH_G_BUILD_OPTION_TABLE);
			CDefineSymbolTable.Create(KCDefine.U_SCRIPTABLE_PATH_G_DEFINE_SYMBOL_TABLE);
			CProjInfoTable.Create(KCDefine.U_SCRIPTABLE_PATH_G_PROJ_INFO_TABLE);
			CDeviceInfoTable.Create(KCDefine.U_SCRIPTABLE_PATH_G_DEVICE_INFO_TABLE);

#if ADS_ENABLE || FLURRY_ENABLE || TENJIN_ENABLE || FIREBASE_ENABLE
			CPluginInfoTable.Create(KCDefine.U_SCRIPTABLE_PATH_G_PLUGIN_INFO_TABLE);
#endif			// #if ADS_ENABLE || FLURRY_ENABLE || TENJIN_ENABLE || FIREBASE_ENABLE

#if PURCHASE_ENABLE
			CProductInfoTable.Create(KCDefine.U_SCRIPTABLE_PATH_G_PRODUCT_INFO_TABLE);
#endif			// #if PURCHASE_ENABLE

			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
			// 테이블을 생성한다 }

			// 저장소를 생성한다 {
#if MSG_PACK_ENABLE
			CAppInfoStorage.Create();
			CUserInfoStorage.Create();
#endif			// #if MSG_PACK_ENABLE

			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
			// 저장소를 생성한다 }
		}

		this.Setup();
		yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
		
		CSceneManager.IsInit = true;
		CSceneLoader.Instance.LoadScene(KCDefine.B_SCENE_NAME_SPLASH, false, false);
	}
	#endregion			// 함수
}
