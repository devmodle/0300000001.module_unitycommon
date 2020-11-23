using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 지연 설정 씬 관리자
public abstract partial class CLateSetupSceneManager : CSceneManager {
	#region 프로퍼티
	public bool IsAutoInitManager { get; protected set; } = false;
	public override string SceneName => KCDefine.B_SCENE_NAME_LATE_SETUP;

#if UNITY_EDITOR
	public override int ScriptOrder => KCDefine.U_SCRIPT_ORDER_LATE_SETUP_SCENE_MANAGER;
#endif			// #if UNITY_EDITOR
	#endregion			// 프로퍼티

	#region 클래스 프로퍼티
#if ADS_MODULE_ENABLE
	public static bool IsAutoLoadAds { get; protected set; } = false;
#endif			// #if ADS_MODULE_ENABLE
	#endregion			// 클래스 프로퍼티

	#region 함수
	//! 초기화
	public sealed override void Start() {
		base.Start();
		StartCoroutine(this.OnStart());
	}

	//! 씬을 설정한다
	protected virtual void Setup() {
		// Do Nothing
	}

	//! 초기화
	private IEnumerator OnStart() {
		CSceneLoader.Instance.UnloadSceneAsync(KCDefine.B_SCENE_NAME_AGREE, null);
		yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);

		// 관리자 자동 초기화 모드 일 경우
		if(this.IsAutoInitManager) {
#if ADS_MODULE_ENABLE
			var stAdsParams = new CAdsManager.STParams() {
				m_eBannerAdsType = CPluginInfoTable.Instance.BannerAdsType,

#if ADMOB_ENABLE
				m_stAdmobParams = new CAdsManager.STAdmobParams() {
#if UNITY_IOS
					m_oAdmobIDList = CDeviceInfoTable.Instance.DeviceInfo.m_oiOSAdmobIDList,
#elif UNITY_ANDROID
					m_oAdmobIDList = CDeviceInfoTable.Instance.DeviceInfo.m_oAndroidAdmobIDList,
#else
					m_oAdmobIDList = new List<string>(),
#endif			// #if UNITY_IOS

					m_oAdsIDList = new Dictionary<string, string>() {
						[KCDefine.U_KEY_ADS_M_BANNER_ADS_ID] = CPluginInfoTable.Instance.AdmobPluginInfo.m_oBannerAdsID,
						[KCDefine.U_KEY_ADS_M_REWARD_ADS_ID] = CPluginInfoTable.Instance.AdmobPluginInfo.m_oRewardAdsID,
						[KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID] = CPluginInfoTable.Instance.AdmobPluginInfo.m_oFullscreenAdsID,
						[KCDefine.U_KEY_ADS_M_RESUME_ADS_ID] = CPluginInfoTable.Instance.AdmobPluginInfo.m_oResumeAdsID
					}
				},
#endif			// #if ADMOB_ENABLE

#if IRON_SRC_ENABLE
				m_stIronSrcParams = new CAdsManager.STIronSrcParams() {
					m_oAppKey = CPluginInfoTable.Instance.IronSrcPluginInfo.m_oAppKey,

					m_oAdsIDList = new Dictionary<string, string>() {
						[KCDefine.U_KEY_ADS_M_BANNER_ADS_ID] = CPluginInfoTable.Instance.IronSrcPluginInfo.m_oBannerAdsID,
						[KCDefine.U_KEY_ADS_M_REWARD_ADS_ID] = CPluginInfoTable.Instance.IronSrcPluginInfo.m_oRewardAdsID,
						[KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID] = CPluginInfoTable.Instance.IronSrcPluginInfo.m_oFullscreenAdsID
					}
				},
#endif			// #if IRON_SRC_ENABLE

#if APP_LOVIN_ENABLE
				m_stAppLovinParams = new CAdsManager.STAppLovinParams() {
					m_oSDKKey = CPluginInfoTable.Instance.AppLovinSDKKey,

					m_oAdsIDList = new Dictionary<string, string>() {
						[KCDefine.U_KEY_ADS_M_BANNER_ADS_ID] = CPluginInfoTable.Instance.AppLovinPluginInfo.m_oBannerAdsID,
						[KCDefine.U_KEY_ADS_M_REWARD_ADS_ID] = CPluginInfoTable.Instance.AppLovinPluginInfo.m_oRewardAdsID,
						[KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID] = CPluginInfoTable.Instance.AppLovinPluginInfo.m_oFullscreenAdsID
					}
				}
#endif			// #if APP_LOVIN_ENABLE
			};

			CAdsManager.Instance.Init(stAdsParams, CLateSetupSceneManager.OnInitAdsManager);
#endif			// #if ADS_MODULE_ENABLE

#if FLURRY_MODULE_ENABLE
			CFlurryManager.Instance.Init(CPluginInfoTable.Instance.FlurryAPIKey, 
				CLateSetupSceneManager.OnInitFlurryManager);
#endif			// #if FLURRY_MODULE_ENABLE

#if TENJIN_MODULE_ENABLE
			CTenjinManager.Instance.Init(CPluginInfoTable.Instance.TenjinAPIKey, 
				CLateSetupSceneManager.OnInitTenjinManager);
#endif			// #if TENJIN_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
			CFacebookManager.Instance.Init(CLateSetupSceneManager.OnInitFacebookManager);
#endif			// #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
			var oGameConfig = CResManager.Instance.GetTextAsset(KCDefine.U_DATA_PATH_G_GAME_CONFIG);
			var oBuildVersionConfig = CResManager.Instance.GetTextAsset(KCDefine.U_DATA_PATH_G_BUILD_VERSION_CONFIG);

			string oDeviceConfig = CDeviceInfoTable.Instance.DeviceConfig.ExToJSONString();
			
			CAccess.Assert(oGameConfig.ExIsValid() && 
				oBuildVersionConfig.ExIsValid() && oDeviceConfig.ExIsValid());

			var oConfigList = new Dictionary<string, object>() {
				[KCDefine.U_CONFIG_KEY_FIREBASE_M_GAME] = oGameConfig.text,
				[KCDefine.U_CONFIG_KEY_FIREBASE_M_DEVICE] = oDeviceConfig,
				[KCDefine.U_CONFIG_KEY_FIREBASE_M_BUILD_VERSION] = oBuildVersionConfig.text
			};

			CResManager.Instance.RemoveTextAsset(KCDefine.U_DATA_PATH_G_GAME_CONFIG, true);
			CResManager.Instance.RemoveTextAsset(KCDefine.U_DATA_PATH_G_BUILD_VERSION_CONFIG, true);

			CFirebaseManager.Instance.Init(oConfigList, CLateSetupSceneManager.OnInitFirebaseManager);
#endif			// #if FIREBASE_MODULE_ENABLE

#if UNITY_SERVICES_MODULE_ENABLE
			CUnityServicesManager.Instance.Init(CLateSetupSceneManager.OnInitUnityServicesManager);
#endif			// #if UNITY_SERVICES_MODULE_ENABLE

#if SINGULAR_MODULE_ENABLE
			CSingularManager.Instance.Init(CPluginInfoTable.Instance.SingularPluginInfo.m_oAPIKey,
				CPluginInfoTable.Instance.SingularPluginInfo.m_oAPISecret, 
				CLateSetupSceneManager.OnInitSingularManager);
#endif			// #if SINGULAR_MODULE_ENABLE

#if GAME_CENTER_MODULE_ENABLE
			CGameCenterManager.Instance.Init(CLateSetupSceneManager.OnInitGameCenterManager);
#endif			// #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
			CPurchaseManager.Instance.Init(CProductInfoTable.Instance.ProductInfoList, 
				CLateSetupSceneManager.OnInitPurchaseManager);
#endif			// #if PURCHASE_MODULE_ENABLE

#if NOTI_MODULE_ENABLE
			var stNotiParams = new CNotiManager.STParams() {
#if UNITY_IOS
				m_eAuthOpts = KCDefine.U_AUTH_OPTS_NOTI,
				m_ePresentOpts = KCDefine.U_PRESENT_OPTS_NOTI
#elif UNITY_ANDROID
				m_eImportance = KCDefine.U_IMPORTANCE_NOTI
#endif			// #if UNITY_IOS
			};

			CNotiManager.Instance.Init(stNotiParams, CLateSetupSceneManager.OnInitNotiManager);
#endif			// #if NOTI_MODULE_ENABLE
		}

		this.Setup();
		yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);

		CFunc.BroadcastMsg(KCDefine.SS_FUNC_NAME_START_SCENE_EVENT, 
			EStartSceneEvent.LOAD_PERMISSION_SCENE);

		CSceneManager.IsLateSetup = true;
		CSceneLoader.Instance.LoadAdditiveScene(KCDefine.B_SCENE_NAME_PERMISSION);
	}
	#endregion			// 함수

	#region 조건부 클래스 함수
#if ADS_MODULE_ENABLE
	//! 광고 관리자가 초기화 되었을 경우
	private static void OnInitAdsManager(CAdsManager a_oSender, 
		EAdsType a_eAdsType, bool a_bIsSuccess) 
	{
		CFunc.ShowLog("CLateSetupSceneManager.OnInitAdsManager: {0}, {1}", a_eAdsType, a_bIsSuccess);

#if ADMOB_ENABLE
		// 초기화 되었을 경우
		if(a_bIsSuccess) {
#if UNITY_IOS || UNITY_ANDROID
			// 애드 몹 일 경우
			if(a_eAdsType == EAdsType.ADMOB) {
#if UNITY_IOS
				var oAdmobIDList = CDeviceInfoTable.Instance.DeviceInfo.m_oiOSAdmobIDList;
#else
				var oAdmobIDList = CDeviceInfoTable.Instance.DeviceInfo.m_oAndroidAdmobIDList;
#endif			// #if UNITY_IOS

				CUnityMsgSender.Instance.SendInitAdsMsg(CPluginInfoTable.Instance.AdmobPluginInfo.m_oResumeAdsID,
					oAdmobIDList, CLateSetupSceneManager.OnInitAds);
			}
#endif			// #if UNITY_IOS || UNITY_ANDROID
#endif			// #if ADMOB_ENABLE

			// 광고 자동 로드 모드 일 경우
			if(CLateSetupSceneManager.IsAutoLoadAds) {
				CAdsManager.Instance.LoadRewardAds(a_eAdsType);

				// 광고 로드가 가능 할 경우
				if(!CCommonUserInfoStorage.Instance.UserInfo.IsRemoveAds) {
					CAdsManager.Instance.LoadFullscreenAds(a_eAdsType);
				}
			}
		}
	}

#if ADMOB_ENABLE && (UNITY_IOS || UNITY_ANDROID)
	//! 광고가 초기화 되었을 경우
	private static void OnInitAds(string a_oCmd, string a_oMsg) {
		CFunc.ShowLog("CLateSetupSceneManager.OnInitAds: {0}", a_oMsg);

		bool bIsValid = bool.TryParse(a_oMsg, out bool bIsSuccess);
		bIsValid = bIsValid && bIsSuccess;

		bool bIsEnableLoadAds = bIsValid && 
			CLateSetupSceneManager.IsAutoLoadAds && 
			!CCommonUserInfoStorage.Instance.UserInfo.IsRemoveAds;

		// 재개 광고 로드가 가능 할 경우
		if(bIsEnableLoadAds && 
			CPluginInfoTable.Instance.AdmobPluginInfo.m_oResumeAdsID.ExIsValid()) 
		{
			CAdsManager.Instance.LoadResumeAds(EAdsType.ADMOB);
		}
	}
#endif			// #if ADMOB_ENABLE && (UNITY_IOS || UNITY_ANDROID)
#endif			// #if ADS_MODULE_ENABLE

#if FLURRY_MODULE_ENABLE
	//! 플러리 관리자가 초기화 되었을 경우
	private static void OnInitFlurryManager(CFlurryManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog("CLateSetupSceneManager.OnInitFlurryManager: {0}", a_bIsSuccess);

		// 초기화 되었을 경우
		if(a_bIsSuccess) {
			CFlurryManager.Instance.SetAnalyticsUserID(CCommonAppInfoStorage.Instance.AppInfo.DeviceID);
			CFlurryManager.Instance.SendLog(KCDefine.U_LOG_NAME_APP_LAUNCH, null);
		}
	}
#endif			// #if FLURRY_MODULE_ENABLE

#if TENJIN_MODULE_ENABLE
	//! 텐진 관리자가 초기화 되었을 경우
	private static void OnInitTenjinManager(CTenjinManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog("CLateSetupSceneManager.OnInitTenjinManager: {0}", a_bIsSuccess);

		// 초기화 되었을 경우
		if(a_bIsSuccess) {
			CTenjinManager.Instance.SendLog(KCDefine.U_LOG_NAME_APP_LAUNCH, null);
		}
	}
#endif			// #if TENJIN_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
	//! 페이스 북 관리자가 초기화 되었을 경우
	private static void OnInitFacebookManager(CFacebookManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog("CLateSetupSceneManager.OnInitFacebookManager: {0}", a_bIsSuccess);

		// 초기화 되었을 경우
		if(a_bIsSuccess) {
			CFacebookManager.Instance.SendLog(KCDefine.U_LOG_NAME_APP_LAUNCH, null);
		}
	}
#endif			// #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
	//! 파이어 베이스 관리자가 초기화 되었을 경우
	private static void OnInitFirebaseManager(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog("CLateSetupSceneManager.OnInitFirebaseManager: {0}", a_bIsSuccess);

		// 초기화 되었을 경우
		if(a_bIsSuccess) {
			CFirebaseManager.Instance.SetAnalyticsDatas(new Dictionary<string, string>() {
				[KCDefine.U_LOG_KEY_COUNTRY_CODE] = CCommonAppInfoStorage.Instance.CountryCode
			});

			CFirebaseManager.Instance.SetCrashDatas(new Dictionary<string, string>() {
				[KCDefine.U_LOG_KEY_COUNTRY_CODE] = CCommonAppInfoStorage.Instance.CountryCode
			});

			CFirebaseManager.Instance.SetAnalyticsUserID(CCommonAppInfoStorage.Instance.AppInfo.DeviceID);
			CFirebaseManager.Instance.SetCrashUserID(CCommonAppInfoStorage.Instance.AppInfo.DeviceID);

			CFirebaseManager.Instance.SendLog(KCDefine.U_LOG_NAME_APP_LAUNCH, null);
			CFirebaseManager.Instance.StartTracking(KCDefine.U_TRACKING_NAME_APP_LAUNCH, null);

#if FIREBASE_REMOTE_CONFIG_ENABLE
			CFirebaseManager.Instance.LoadConfig(CLateSetupSceneManager.OnLoadConfig);
#endif			// #if FIREBASE_REMOTE_CONFIG_ENABLE
		}
	}

#if FIREBASE_REMOTE_CONFIG_ENABLE
	//! 속성을 로드했을 경우
	private static void OnLoadConfig(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog("CLateSetupSceneManager.OnLoadConfig: {0}", a_bIsSuccess);

		// 속성이 로드 되었을 경우
		if(a_bIsSuccess) {
			string oDeviceConfig = CFirebaseManager.Instance.GetConfig(KCDefine.U_CONFIG_KEY_FIREBASE_M_DEVICE);
			CCommonAppInfoStorage.Instance.DeviceConfig = oDeviceConfig.ExJSONStringToObj<STDeviceConfig>();
		}
	}
#endif			// #if FIREBASE_REMOTE_CONFIG_ENABLE
#endif			// #if FIREBASE_MODULE_ENABLE

#if UNITY_SERVICES_MODULE_ENABLE
	//! 유니티 서비스 관리자가 초기화 되었을 경우
	private static void OnInitUnityServicesManager(CUnityServicesManager a_oSender, 
		bool a_bIsSuccess) 
	{
		CFunc.ShowLog("CLateSetupSceneManager.OnInitUnityServicesManager: {0}", a_bIsSuccess);

		// 초기화 되었을 경우
		if(a_bIsSuccess) {
			CUnityServicesManager.Instance.SetCrashDatas(new Dictionary<string, string>() {
				[KCDefine.U_LOG_KEY_USER_ID] = CCommonAppInfoStorage.Instance.AppInfo.DeviceID,
				[KCDefine.U_LOG_KEY_COUNTRY_CODE] = CCommonAppInfoStorage.Instance.CountryCode
			});

			CUnityServicesManager.Instance.SetAnalyticsUserID(CCommonAppInfoStorage.Instance.AppInfo.DeviceID);
			CUnityServicesManager.Instance.SendLog(KCDefine.U_LOG_NAME_APP_LAUNCH, null);
		}
	}
#endif			// #if UNITY_SERVICES_MODULE_ENABLE

#if SINGULAR_MODULE_ENABLE
	//! 싱귤러 관리자가 초기화 되었을 경우
	private static void OnInitSingularManager(CSingularManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog("CLateSetupSceneManager.OnInitSingularManager: {0}", a_bIsSuccess);

		// 초기화 되었을 경우
		if(a_bIsSuccess) {
			CSingularManager.Instance.SetAnalyticsUserID(CCommonAppInfoStorage.Instance.AppInfo.DeviceID);
			CSingularManager.Instance.SendLog(KCDefine.U_LOG_NAME_APP_LAUNCH, null);
		}
	}
#endif			// #if SINGULAR_MODULE_ENABLE

#if GAME_CENTER_MODULE_ENABLE
	//! 게임 센터 관리자가 초기화 되었을 경우
	private static void OnInitGameCenterManager(CGameCenterManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog("CLateSetupSceneManager.OnInitGameCenterManager: {0}", a_bIsSuccess);
	}
#endif			// #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	//! 결제 관리자가 초기화 되었을 경우
	private static void OnInitPurchaseManager(CPurchaseManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog("CLateSetupSceneManager.OnInitPurchaseManager: {0}", a_bIsSuccess);
	}
#endif			// #if PURCHASE_MODULE_ENABLE

#if NOTI_MODULE_ENABLE
	//! 알림 관리자가 초기화 되었을 경우
	private static void OnInitNotiManager(CNotiManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog("CLateSetupSceneManager.OnInitNotiManager: {0}", a_bIsSuccess);
	}
#endif			// #if NOTI_MODULE_ENABLE
	#endregion			// 조건부 클래스 함수
}
