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
	public override string SceneName => KBDefine.SCENE_NAME_INIT;

#if UNITY_EDITOR
	public override int ScriptOrder => KUDefine.SCRIPT_ORDER_INIT_SCENE_MANAGER;
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
		// 디바이스 정보를 설정한다 {
		int nTargetFrameRate = KAppDefine.G_DESKTOP_TARGET_FRAME_RATE;

		if(CBAccess.IsDesktopPlatform()) {
			Screen.SetResolution(KBDefine.DESKTOP_WINDOW_WIDTH, 
				KBDefine.DESKTOP_WINDOW_HEIGHT, FullScreenMode.Windowed);
		} else {
			if(CBAccess.IsMobilePlatform()) {
				nTargetFrameRate = KAppDefine.G_MOBILE_TARGET_FRAME_RATE;
			} else if(CBAccess.IsConsolePlatform()) {
				nTargetFrameRate = KAppDefine.G_CONSOLE_TARGET_FRAME_RATE;
			} else {
				nTargetFrameRate = KAppDefine.G_HANDHELD_CONSOLE_TARGET_FRAME_RATE;
			}

			var stScreenSize = CUAccess.GetDeviceScreenSize(Application.isPlaying);
			Screen.SetResolution((int)stScreenSize.x, (int)stScreenSize.y, true);
		}

		Func.SetupQuality(nTargetFrameRate, 
			KAppDefine.G_MULTI_TOUCH_ENABLE, KAppDefine.G_DEF_QUALITY_LEVEL);
		// 디바이스 정보를 설정한다 }

		this.SetupBlindUI();

#if !UNITY_EDITOR
		GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
#endif			// #if !UNITY_EDITOR

		// 저장소를 로드한다 {
#if MESSAGE_PACK_ENABLE
		CUserInfoStorage.Instance.LoadUserInfo(KBDefine.DATA_PATH_USER_INFO);
#endif			// #if MESSAGE_PACK_ENABLE
		// 저장소를 로드한다 }

		// 사운드를 설정한다 {
		CSndManager.Instance.BGSndVolume = 1.0f;
		CSndManager.Instance.FXSndsVolume = 1.0f;

#if MESSAGE_PACK_ENABLE
		CSndManager.Instance.IsMuteBGSnd = CUserInfoStorage.Instance.UserInfo.IsMuteBGSnd;
		CSndManager.Instance.IsMuteFXSnds = CUserInfoStorage.Instance.UserInfo.IsMuteFXSnds;
		CSndManager.Instance.IsDisableVibrate = CUserInfoStorage.Instance.UserInfo.IsDisableVibrate;
#endif			// #if MESSAGE_PACK_ENABLE
		// 사운드를 설정한다 }
	}

	//! 초기화
	private IEnumerator OnStart() {
		if(!CSceneManager.IsInit) {
			yield return CBFactory.CreateWaitForSeconds(KUDefine.DELAY_INIT);
			CActivityIndicatorManager.Instance.StartActivityIndicator(true, false);

			// 간격을 설정한다
			this.SetupOffsets();
			yield return CBFactory.CreateWaitForSeconds(KUDefine.DELAY_INIT);

			// iOS 플랫폼을 설정한다 {
#if UNITY_IOS
			Device.SetNoBackupFlag(KBDefine.DIR_PATH_WRITABLE);
			Device.SetNoBackupFlag(KUDefine.IMG_PATH_SCREENSHOT);

			Device.hideHomeButton = false;
			yield return CBFactory.CreateWaitForSeconds(KUDefine.DELAY_INIT);
#endif			// #if UNITY_IOS
			// iOS 플랫폼을 설정한다 }

			// 관리자를 생성한다 {
			CLogManager.Create();
			CSndManager.Create();
			CScheduleManager.Create();
			CResManager.Create();
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

#if PURCHASE_ENABLE && MESSAGE_PACK_ENABLE
			CPurchaseManager.Create();
#endif			// #if PURCHASE_ENABLE && MESSAGE_PACK_ENABLE

#if UNITY_SERVICE_ENABLE
			CUnityServiceManager.Create();
#endif			// #if UNITY_SERVICE_ENABLE

			yield return CBFactory.CreateWaitForSeconds(KUDefine.DELAY_INIT);
			// 관리자를 생성한다 }

			// 디바이스 연동 객체를 생성한다 {
			CUnityMsgSender.Create();
			CDeviceMsgReceiver.Create();

			yield return CBFactory.CreateWaitForSeconds(KUDefine.DELAY_INIT);
			// 디바이스 연동 객체를 생성한다 }

			// 테이블을 생성한다 {
			CValueTable.Create();
			CStringTable.Create();

			CBuildInfoTable.Create(KUDefine.SCRIPTABLE_PATH_G_BUILD_INFO_TABLE);
			CBuildOptionTable.Create(KUDefine.SCRIPTABLE_PATH_G_BUILD_OPTION_TABLE);
			CDefineSymbolTable.Create(KUDefine.SCRIPTABLE_PATH_G_DEFINE_SYMBOL_TABLE);
			CProjectInfoTable.Create(KUDefine.SCRIPTABLE_PATH_G_PROJECT_INFO_TABLE);
			CDeviceInfoTable.Create(KUDefine.SCRIPTABLE_PATH_G_DEVICE_INFO_TABLE);

#if ADS_ENABLE || TENJIN_ENABLE || FLURRY_ENABLE || FIREBASE_ENABLE
			CPluginInfoTable.Create(KUDefine.SCRIPTABLE_PATH_G_PLUGIN_INFO_TABLE);
#endif			// #if ADS_ENABLE || TENJIN_ENABLE || FLURRY_ENABLE || FIREBASE_ENABLE

#if PURCHASE_ENABLE
			CProductInfoTable.Create(KUDefine.SCRIPTABLE_PATH_G_PRODUCT_INFO_TABLE);
#endif			// #if PURCHASE_ENABLE

			yield return CBFactory.CreateWaitForSeconds(KUDefine.DELAY_INIT);
			// 테이블을 생성한다 }

			// 저장소를 생성한다 {
#if MESSAGE_PACK_ENABLE
			CAppInfoStorage.Create();
			CUserInfoStorage.Create();
#endif			// #if MESSAGE_PACK_ENABLE

			yield return CBFactory.CreateWaitForSeconds(KUDefine.DELAY_INIT);
			// 저장소를 생성한다 }
		}

		this.Setup();
		yield return CBFactory.CreateWaitForSeconds(KUDefine.DELAY_INIT);
		
		CSceneManager.IsInit = true;
		CSceneLoader.Instance.LoadScene(KBDefine.SCENE_NAME_SPLASH, false, false);
	}
	#endregion			// 함수
}
