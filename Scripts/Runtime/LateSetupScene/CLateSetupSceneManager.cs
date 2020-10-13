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
		CAccess.Assert(!CSceneManager.IsLateSetup);
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
						[KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID] = CPluginInfoTable.Instance.AdmobPluginInfo.m_oFullscreenAdsID
					}
				},
#endif			// #if ADMOB_ENABLE

#if IRON_SOURCE_ENABLE
				m_stIronSourceParams = new CAdsManager.STIronSourceParams() {
					m_oAppKey = CPluginInfoTable.Instance.IronSourcePluginInfo.m_oAppKey,

					m_oAdsIDList = new Dictionary<string, string>() {
						[KCDefine.U_KEY_ADS_M_BANNER_ADS_ID] = CPluginInfoTable.Instance.IronSourcePluginInfo.m_oBannerAdsID,
						[KCDefine.U_KEY_ADS_M_REWARD_ADS_ID] = CPluginInfoTable.Instance.IronSourcePluginInfo.m_oRewardAdsID,
						[KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID] = CPluginInfoTable.Instance.IronSourcePluginInfo.m_oFullscreenAdsID
					}
				},
#endif			// #if IRON_SOURCE_ENABLE

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
			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
#endif			// #if ADS_MODULE_ENABLE

#if FLURRY_MODULE_ENABLE
			CFlurryManager.Instance.Init(CPluginInfoTable.Instance.FlurryAPIKey, 
				CLateSetupSceneManager.OnInitFlurryManager);

			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
#endif			// #if FLURRY_MODULE_ENABLE

#if TENJIN_MODULE_ENABLE
			CTenjinManager.Instance.Init(CPluginInfoTable.Instance.TenjinAPIKey, 
				CLateSetupSceneManager.OnInitTenjinManager);

			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
#endif			// #if TENJIN_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
			CFacebookManager.Instance.Init(CLateSetupSceneManager.OnInitFacebookManager);
			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
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
			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
#endif			// #if FIREBASE_MODULE_ENABLE

#if UNITY_SERVICE_MODULE_ENABLE
			CUnityServiceManager.Instance.Init(CLateSetupSceneManager.OnInitUnityServiceManager);
			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
#endif			// #if UNITY_SERVICE_MODULE_ENABLE

#if SINGULAR_MODULE_ENABLE
			CSingularManager.Instance.Init(CPluginInfoTable.Instance.SingularPluginInfo.m_oAPIKey,
				CPluginInfoTable.Instance.SingularPluginInfo.m_oAPISecret, CLateSetupSceneManager.OnInitSingularManager);
				
			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
#endif			// #if SINGULAR_MODULE_ENABLE

#if GAME_CENTER_MODULE_ENABLE
			CGameCenterManager.Instance.Init(CLateSetupSceneManager.OnInitGameCenterManager);
			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
#endif			// #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
			CPurchaseManager.Instance.Init(CProductInfoTable.Instance.ProductInfoList, 
				CLateSetupSceneManager.OnInitPurchaseManager);

			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
#endif			// #if PURCHASE_MODULE_ENABLE

#if LOCAL_NOTI_MODULE_ENABLE
			var stLocalNotiParams = new CLocalNotiManager.STParams() {
#if UNITY_IOS
				m_eAuthOpts = KCDefine.U_AUTH_OPTS_LOCAL_NOTI
#elif UNITY_ANDROID
				m_eImportance = KCDefine.U_IMPORTANCE_LOCAL_NOTI
#endif			// #if UNITY_IOS
			};

			CLocalNotiManager.Instance.Init(stLocalNotiParams, CLateSetupSceneManager.OnInitLocalNotiManager);
			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
#endif			// #if LOCAL_NOTI_MODULE_ENABLE
		}

		this.Setup();
		yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);

		CFunc.BroadcastMsg(KCDefine.SS_FUNC_NAME_START_SCENE_EVENT, 
			EStartSceneEvent.LOAD_INTRO_SCENE);

		this.ExLateCallFunc(KCDefine.U_DELAY_INIT, (a_oSender, a_oParams) => {
			bool bIsInitScene = CSceneManager.AwakeSceneName.ExIsEquals(KCDefine.B_SCENE_NAME_INIT);
			bool bIsSetupScene = CSceneManager.AwakeSceneName.ExIsEquals(KCDefine.B_SCENE_NAME_SETUP);
			bool bIsStartScene = CSceneManager.AwakeSceneName.ExIsEquals(KCDefine.B_SCENE_NAME_START);
			bool bIsSplashScene = CSceneManager.AwakeSceneName.ExIsEquals(KCDefine.B_SCENE_NAME_SPLASH);
			bool bIsAgreeScene = CSceneManager.AwakeSceneName.ExIsEquals(KCDefine.B_SCENE_NAME_AGREE);
			bool bIsLateSetupScene = CSceneManager.AwakeSceneName.ExIsEquals(KCDefine.B_SCENE_NAME_LATE_SETUP);
			
			CSceneManager.IsLateSetup = true;

			// 인트로 씬 로드가 필요 할 경우
			if(bIsInitScene || bIsSetupScene || bIsStartScene || bIsSplashScene || bIsAgreeScene || bIsLateSetupScene) {
				CSceneLoader.Instance.LoadAdditiveScene(KCDefine.B_SCENE_NAME_INTRO);
			} else {
				CSceneLoader.Instance.LoadScene(CSceneManager.AwakeSceneName, false, false);
			}
		});
	}
	#endregion			// 함수

	#region 조건부 클래스 함수
#if ADS_MODULE_ENABLE
	//! 광고 관리자가 초기화 되었을 경우
	private static void OnInitAdsManager(CAdsManager a_oSender, EAdsType a_eAdsType, bool a_bIsSuccess) {
		CFunc.ShowLog("CLateSetupSceneManager.OnInitAdsManager: {0}, {1}", a_eAdsType, a_bIsSuccess);

		// 광고 자동 로드 모드 일 경우
		if(a_bIsSuccess && CLateSetupSceneManager.IsAutoLoadAds) {
			CAdsManager.Instance.LoadRewardAds(a_eAdsType);

			// 전면 광고 로드가 가능 할 경우
			if(!CCommonUserInfoStorage.Instance.UserInfo.IsRemoveAds) {
				CAdsManager.Instance.LoadFullscreenAds(a_eAdsType);
			}
		}
	}
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
			CFirebaseManager.Instance.LoadConfig(CLateSetupSceneManager.OnLoadConfig);
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

#if UNITY_SERVICE_MODULE_ENABLE
	//! 유니티 서비스 관리자가 초기화 되었을 경우
	private static void OnInitUnityServiceManager(CUnityServiceManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog("CLateSetupSceneManager.OnInitUnityServiceManager: {0}", a_bIsSuccess);

		// 초기화 되었을 경우
		if(a_bIsSuccess) {
			CUnityServiceManager.Instance.SetCrashDatas(new Dictionary<string, string>() {
				[KCDefine.U_LOG_KEY_USER_ID] = CCommonAppInfoStorage.Instance.AppInfo.DeviceID,
				[KCDefine.U_LOG_KEY_COUNTRY_CODE] = CCommonAppInfoStorage.Instance.CountryCode
			});

			CUnityServiceManager.Instance.SetAnalyticsUserID(CCommonAppInfoStorage.Instance.AppInfo.DeviceID);
			CUnityServiceManager.Instance.SendLog(KCDefine.U_LOG_NAME_APP_LAUNCH, null);
		}
	}
#endif			// #if UNITY_SERVICE_MODULE_ENABLE

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

#if LOCAL_NOTI_MODULE_ENABLE
	//! 로컬 알림 관리자가 초기화 되었을 경우
	private static void OnInitLocalNotiManager(CLocalNotiManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog("CLateSetupSceneManager.OnInitLocalNotiManager: {0}", a_bIsSuccess);
	}
#endif			// #if LOCAL_NOTI_MODULE_ENABLE
	#endregion			// 조건부 클래스 함수
}
