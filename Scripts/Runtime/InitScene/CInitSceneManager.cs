using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_IOS
using UnityEngine.iOS;
#endif			// #if UNITY_IOS

//! 초기화 씬 관리자
public abstract partial class CInitSceneManager : CSceneManager {
	#region 클래스 객체
	private static GameObject m_oBlindUIs = null;
	#endregion			// 클래스 객체

	#region 프로퍼티
	public override string SceneName => KCDefine.B_SCENE_N_INIT;

#if UNITY_EDITOR
	public override int ScriptOrder => KCDefine.U_SCRIPT_O_INIT_SCENE_MANAGER;
#endif			// #if UNITY_EDITOR
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public sealed override void Start() {
		base.Start();

		// 초기화가 필요 할 경우
		if(!CSceneManager.IsInit) {
			StartCoroutine(this.OnStart());
		}
	}

	//! 씬을 설정한다
	protected virtual void Setup() {
		this.SetupBlindUIs();

		// 테이블을 로드한다
		CValTable.Inst.LoadValsFromRes(KCDefine.U_TABLE_P_G_COMMON_VAL);
		CStrTable.Inst.LoadStrsFromRes(KCDefine.U_TABLE_P_G_COMMON_STR);

		// 저장소를 로드한다
		CCommonAppInfoStorage.Inst.LoadAppInfo();
		CCommonUserInfoStorage.Inst.LoadUserInfo();
		CCommonGameInfoStorage.Inst.LoadGameInfo();

		// 공용 앱 정보 저장소를 설정한다
		CCommonAppInfoStorage.Inst.AppInfo.LastPlayTime = System.DateTime.Now;
		CCommonAppInfoStorage.Inst.SaveAppInfo();

		// 사운드 관리자를 설정한다 {
		CSndManager.Inst.BGSndVolume = CCommonGameInfoStorage.Inst.GameInfo.BGSndVolume;
		CSndManager.Inst.FXSndsVolume = CCommonGameInfoStorage.Inst.GameInfo.FXSndsVolume;
		
		CSndManager.Inst.IsMuteBGSnd = CCommonGameInfoStorage.Inst.GameInfo.IsMuteBGSnd;
		CSndManager.Inst.IsMuteFXSnds = CCommonGameInfoStorage.Inst.GameInfo.IsMuteFXSnds;
		CSndManager.Inst.IsDisableVibrate = CCommonGameInfoStorage.Inst.GameInfo.IsDisableVibrate;
		// 사운드 관리자를 설정한다 }
	}

	//! 초기화
	private IEnumerator OnStart() {
		CIndicatorManager.Create();
		CIndicatorManager.Inst.Show(true, false);
		
		yield return CFactory.CreateWaitForSecs(KCDefine.U_DELAY_INIT);

		this.SetupOffsets();
		yield return CFactory.CreateWaitForSecs(KCDefine.U_DELAY_INIT);

		// iOS 를 설정한다 {
#if UNITY_IOS
		Device.SetNoBackupFlag(KCDefine.B_DIR_P_WRITABLE);
		Device.SetNoBackupFlag(KCDefine.U_IMG_P_SCREENSHOT);

		Device.hideHomeButton = false;
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
		// 관리자를 생성한다 }

		// 로더를 생성한다
		CSceneLoader.Create();

		// 디바이스 연동 객체를 생성한다
		CUnityMsgSender.Create();
		CDeviceMsgReceiver.Create();

		// 테이블을 생성한다 {
		CValTable.Create();
		CStrTable.Create();

		CBuildInfoTable.Create(KCDefine.U_ASSET_P_G_BUILD_INFO_TABLE);
		CBuildOptsTable.Create(KCDefine.U_ASSET_P_G_BUILD_OPTS_TABLE);
		CDefineSymbolTable.Create(KCDefine.U_ASSET_P_G_DEFINE_SYMBOL_TABLE);
		CProjInfoTable.Create(KCDefine.U_ASSET_P_G_PROJ_INFO_TABLE);
		CDeviceInfoTable.Create(KCDefine.U_ASSET_P_G_DEVICE_INFO_TABLE);

#if ADS_MODULE_ENABLE || FLURRY_MODULE_ENABLE || TENJIN_MODULE_ENABLE || FIREBASE_MODULE_ENABLE || SINGULAR_MODULE_ENABLE
		CPluginInfoTable.Create(KCDefine.U_ASSET_P_G_PLUGIN_INFO_TABLE);
#endif			// #if ADS_MODULE_ENABLE || FLURRY_MODULE_ENABLE || TENJIN_MODULE_ENABLE || FIREBASE_MODULE_ENABLE || SINGULAR_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
		CProductInfoTable.Create(KCDefine.U_ASSET_P_G_PRODUCT_INFO_TABLE);
#endif			// #if PURCHASE_MODULE_ENABLE
		// 테이블을 생성한다 }

		// 저장소를 생성한다
		CCommonAppInfoStorage.Create();
		CCommonUserInfoStorage.Create();
		CCommonGameInfoStorage.Create();

		this.Setup();
		yield return CFactory.CreateWaitForSecs(KCDefine.U_DELAY_INIT);
		
		CSceneManager.IsInit = true;
		CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_SPLASH, false, false);
	}

	//! 블라인드 이미지를 생성한다
	protected virtual Image CreateBlindImg(string a_oName, GameObject a_oParent) {
		return CFactory.CreateCloneObj<Image>(a_oName, KCDefine.IS_OBJ_P_SCREEN_BLIND_IMG, a_oParent);
	}
	#endregion			// 함수
}
