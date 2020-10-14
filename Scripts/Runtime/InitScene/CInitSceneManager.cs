using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		this.SetupBlindUI();

		// 테이블을 로드한다
		CValueTable.Instance.LoadValuesFromRes(KCDefine.U_TABLE_PATH_G_COMMON_VALUE);
		CStringTable.Instance.LoadStringsFromRes(KCDefine.U_TABLE_PATH_G_COMMON_STRING);

		// 저장소를 로드한다
		CCommonUserInfoStorage.Instance.LoadUserInfo();

		// 사운드를 설정한다 {
		CSndManager.Instance.BGSndVolume = KCDefine.B_MAX_VALUE_NORM;
		CSndManager.Instance.FXSndsVolume = KCDefine.B_MAX_VALUE_NORM;
		
		CSndManager.Instance.IsMuteBGSnd = CCommonUserInfoStorage.Instance.UserInfo.IsMuteBGSnd;
		CSndManager.Instance.IsMuteFXSnds = CCommonUserInfoStorage.Instance.UserInfo.IsMuteFXSnds;
		CSndManager.Instance.IsDisableVibrate = CCommonUserInfoStorage.Instance.UserInfo.IsDisableVibrate;
		// 사운드를 설정한다 }
	}

	//! 초기화
	private IEnumerator OnStart() {
		CAccess.Assert(!CSceneManager.IsInit);

		yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
		CActivityIndicatorManager.Instance.StartActivityIndicator(true, false);

		// 간격을 설정한다
		this.SetupOffsets();
		yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);

		// iOS 를 설정한다 {
#if UNITY_IOS
		Device.SetNoBackupFlag(KCDefine.B_DIR_PATH_WRITABLE);
		Device.SetNoBackupFlag(KCDefine.U_IMG_PATH_SCREENSHOT);

		Device.hideHomeButton = false;
		yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
#endif			// #if UNITY_IOS
		// iOS 를 설정한다 }

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

#if ADS_MODULE_ENABLE
		CAdsManager.Create();
#endif			// #if ADS_MODULE_ENABLE

#if FLURRY_MODULE_ENABLE
		CFlurryManager.Create();
#endif			// #if FLURRY_MODULE_ENABLE

#if TENJIN_MODULE_ENABLE
		CTenjinManager.Create();
#endif			// #if TENJIN_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
		CFacebookManager.Create();
#endif			// #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
		CFirebaseManager.Create();
#endif			// #if FIREBASE_MODULE_ENABLE

#if UNITY_SERVICE_MODULE_ENABLE
		CUnityServiceManager.Create();
#endif			// #if UNITY_SERVICE_MODULE_ENABLE

#if SINGULAR_MODULE_ENABLE
		CSingularManager.Create();
#endif			// #if SINGULAR_MODULE_ENABLE

#if GAME_CENTER_MODULE_ENABLE
		CGameCenterManager.Create();
#endif			// #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
		CPurchaseManager.Create();
#endif			// #if PURCHASE_MODULE_ENABLE

#if NOTI_MODULE_ENABLE
		CNotiManager.Create();
#endif			// #if NOTI_MODULE_ENABLE

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

		CBuildInfoTable.Create(KCDefine.U_ASSET_PATH_G_BUILD_INFO_TABLE);
		CBuildOptsTable.Create(KCDefine.U_ASSET_PATH_G_BUILD_OPTS_TABLE);
		CDefineSymbolTable.Create(KCDefine.U_ASSET_PATH_G_DEFINE_SYMBOL_TABLE);
		CProjInfoTable.Create(KCDefine.U_ASSET_PATH_G_PROJ_INFO_TABLE);
		CDeviceInfoTable.Create(KCDefine.U_ASSET_PATH_G_DEVICE_INFO_TABLE);

#if ADS_MODULE_ENABLE || FLURRY_MODULE_ENABLE || TENJIN_MODULE_ENABLE || FIREBASE_MODULE_ENABLE || SINGULAR_MODULE_ENABLE
		CPluginInfoTable.Create(KCDefine.U_ASSET_PATH_G_PLUGIN_INFO_TABLE);
#endif			// #if ADS_MODULE_ENABLE || FLURRY_MODULE_ENABLE || TENJIN_MODULE_ENABLE || FIREBASE_MODULE_ENABLE || SINGULAR_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
		CProductInfoTable.Create(KCDefine.U_ASSET_PATH_G_PRODUCT_INFO_TABLE);
#endif			// #if PURCHASE_MODULE_ENABLE

		yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
		// 테이블을 생성한다 }

		// 저장소를 생성한다 {
		CCommonAppInfoStorage.Create();
		CCommonUserInfoStorage.Create();

		yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
		// 저장소를 생성한다 }

		this.Setup();
		yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
		
		CSceneManager.IsInit = true;
		CSceneLoader.Instance.LoadScene(KCDefine.B_SCENE_NAME_SPLASH, false, false);
	}
	#endregion			// 함수
}
