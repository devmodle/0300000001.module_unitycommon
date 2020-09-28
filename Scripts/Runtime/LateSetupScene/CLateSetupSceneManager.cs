using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if LOCAL_NOTI_MODULE_ENABLE
#if UNITY_IOS
using Unity.Notifications.iOS;
#elif UNITY_ANDROID
using Unity.Notifications.Android;
#endif			// #if UNITY_IOS
#endif			// #if LOCAL_NOTI_MODULE_ENABLE

//! 지연 설정 씬 관리자
public abstract partial class CLateSetupSceneManager : CSceneManager {
	#region 프로퍼티
	public bool IsAutoInitManager { get; protected set; } = false;
	public override string SceneName => KCDefine.B_SCENE_NAME_LATE_SETUP;

#if UNITY_EDITOR
	public override int ScriptOrder => KCDefine.U_SCRIPT_ORDER_LATE_SETUP_SCENE_MANAGER;
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
		// Do Nothing
	}

	//! 초기화
	private IEnumerator OnStart() {
		// 지연 설정이 필요 할 경우
		if(!CSceneManager.IsLateSetup) {
			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);

			// 관리자 자동 초기화 모드 일 경우
			if(this.IsAutoInitManager) {
#if ADS_MODULE_ENABLE
				var stAdsParams = new CAdsManager.STParams() {
					m_eBannerAdsType = CPluginInfoTable.Instance.BannerAdsType,

#if ADMOB_ENABLE
#if UNITY_IOS
					m_oAdmobIDList = CDeviceInfoTable.Instance.DeviceInfo.m_oiOSAdmobIDList,
#elif UNITY_ANDROID
					m_oAdmobIDList = CDeviceInfoTable.Instance.DeviceInfo.m_oAndroidAdmobIDList,
#else
					m_oAdmobIDList = new List<string>(),
#endif			// #if UNITY_IOS

					m_stAdmobParams = new CAdsManager.STAdmobParams() {
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
				CFlurryManager.Instance.Init(CPluginInfoTable.Instance.FlurryPluginInfo.m_oAPIKey, 
					CLateSetupSceneManager.OnInitFlurryManager);

				yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
#endif			// #if FLURRY_MODULE_ENABLE

#if TENJIN_MODULE_ENABLE
				CTenjinManager.Instance.Init(CPluginInfoTable.Instance.TenjinPluginInfo.m_oAPIKey,
					CLateSetupSceneManager.OnInitTenjinManager);

				yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
#endif			// #if TENJIN_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
				CFacebookManager.Instance.Init(CLateSetupSceneManager.OnInitFacebookManager);
				yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
#endif			// #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
#if FIREBASE_REMOTE_CONFIG_ENABLE
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

#if MSG_PACK_ENABLE
				CCommonAppInfoStorage.Instance.DeviceConfig = oDeviceConfig.ExJSONStringToObj<STDeviceConfig>();
#endif			// #if MSG_PACK_ENABLE
#else
				var oConfigList = new Dictionary<string, object>();
#endif			// #if FIREBASE_REMOTE_CONFIG_ENABLE

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

#if PURCHASE_MODULE_ENABLE && MSG_PACK_ENABLE
				CPurchaseManager.Instance.Init(CProductInfoTable.Instance.ProductInfoList, 
					CLateSetupSceneManager.OnInitPurchaseManager);

				yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
#endif			// #if PURCHASE_MODULE_ENABLE && MSG_PACK_ENABLE

#if LOCAL_NOTI_MODULE_ENABLE
				var stLocalNotiParams = new CLocalNotiManager.STParams() {
#if UNITY_IOS
					m_eNotiOptions = KCDefine.U_DEF_NOTI_OPTS_LOCAL_NM
#elif UNITY_ANDROID
					m_eImportance = KCDefine.U_DEF_IMPORTANCE_LOCAL_NM,

					m_oGroupID = KCDefine.U_DEF_GROUP_ID_LOCAL_NM,
					m_oGroupName = KCDefine.U_DEF_GROUP_NAME_LOCAL_NM,
					m_oGroupDesc = KCDefine.U_DEF_GROUP_DESC_LOCAL_NM
#endif			// #if UNITY_IOS
				};

				CLocalNotiManager.Instance.Init(stLocalNotiParams, CLateSetupSceneManager.OnInitLocalNotiManager);
				yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
#endif			// #if LOCAL_NOTI_MODULE_ENABLE
			}

			this.Setup();
			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);

			this.ExLateCallFunc(KCDefine.U_DELAY_NEXT_SCENE_LOAD, (a_oComponent, a_oParams) => {
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
	}
	#endregion			// 함수

	#region 조건부 클래스 함수
#if ADS_MODULE_ENABLE
	//! 광고 관리자가 초기화 되었을 경우
	public static void OnInitAdsManager(CAdsManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog("CLateSetupSceneManager.OnInitAdsManager: {0}", a_bIsSuccess);
	}
#endif			// #if ADS_MODULE_ENABLE

#if FLURRY_MODULE_ENABLE
	//! 플러리 관리자가 초기화 되었을 경우
	public static void OnInitFlurryManager(CFlurryManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog("CLateSetupSceneManager.OnInitFlurryManager: {0}", a_bIsSuccess);

		// 초기화 되었을 경우
		if(a_bIsSuccess) {
#if MSG_PACK_ENABLE && FLURRY_ANALYTICS_ENABLE
			CFlurryManager.Instance.SetAnalyticsUserID(CCommonAppInfoStorage.Instance.AppInfo.DeviceID);
			CFlurryManager.Instance.SendLog(KCDefine.U_LOG_NAME_APP_LAUNCH, null);
#endif			// #if MSG_PACK_ENABLE && FLURRY_ANALYTICS_ENABLE
		}
	}
#endif			// #if FLURRY_MODULE_ENABLE

#if TENJIN_MODULE_ENABLE
	//! 텐진 관리자가 초기화 되었을 경우
	public static void OnInitTenjinManager(CTenjinManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog("CLateSetupSceneManager.OnInitTenjinManager: {0}", a_bIsSuccess);

		// 초기화 되었을 경우
		if(a_bIsSuccess) {
#if FLURRY_ANALYTICS_ENABLE
			CTenjinManager.Instance.SendLog(KCDefine.U_LOG_NAME_APP_LAUNCH, null);
#endif			// #if FLURRY_ANALYTICS_ENABLE
		}
	}
#endif			// #if TENJIN_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
	//! 페이스 북 관리자가 초기화 되었을 경우
	public static void OnInitFacebookManager(CFacebookManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog("CLateSetupSceneManager.OnInitFacebookManager: {0}", a_bIsSuccess);

		// 초기화 되었을 경우
		if(a_bIsSuccess) {
#if FACEBOOK_ANALYTICS_ENABLE
			CFacebookManager.Instance.SendLog(KCDefine.U_LOG_NAME_APP_LAUNCH, null);
#endif			// #if FACEBOOK_ANALYTICS_ENABLE
		}
	}
#endif			// #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
	//! 파이어 베이스 관리자가 초기화 되었을 경우
	public static void OnInitFirebaseManager(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog("CLateSetupSceneManager.OnInitFirebaseManager: {0}", a_bIsSuccess);

		// 초기화 되었을 경우
		if(a_bIsSuccess) {
#if MSG_PACK_ENABLE
#if FIREBASE_ANALYTICS_ENABLE
			CFirebaseManager.Instance.SetAnalyticsDatas(new Dictionary<string, string>() {
				[KCDefine.U_LOG_KEY_COUNTRY_CODE] = CCommonAppInfoStorage.Instance.CountryCode
			});
			
			CFirebaseManager.Instance.SetAnalyticsUserID(CCommonAppInfoStorage.Instance.AppInfo.DeviceID);
			CFirebaseManager.Instance.SendLog(KCDefine.U_LOG_NAME_APP_LAUNCH, KCDefine.U_LOG_PARAM_USER_INFO, null);
#endif			// #if FIREBASE_ANALYTICS_ENABLE

#if FIREBASE_CRASHLYTICS_ENABLE
			CFirebaseManager.Instance.SetCrashDatas(new Dictionary<string, string>() {
				[KCDefine.U_LOG_KEY_COUNTRY_CODE] = CCommonAppInfoStorage.Instance.CountryCode
			});

			CFirebaseManager.Instance.SetCrashUserID(CCommonAppInfoStorage.Instance.AppInfo.DeviceID);
#endif			// #if FIREBASE_CRASHLYTICS_ENABLE
#endif			// #if MSG_PACK_ENABLE

#if FIREBASE_REMOTE_CONFIG_ENABLE
			CFirebaseManager.Instance.LoadConfig(CLateSetupSceneManager.OnLoadConfig);
#endif			// #if FIREBASE_REMOTE_CONFIG_ENABLE
		}
	}

	//! 속성을 로드했을 경우
	public static void OnLoadConfig(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog("CLateSetupSceneManager.OnLoadConfig: {0}", a_bIsSuccess);

		// 속성이 로드 되었을 경우
		if(a_bIsSuccess) {
#if MSG_PACK_ENABLE
			string oDeviceConfig = CFirebaseManager.Instance.GetConfig(KCDefine.U_CONFIG_KEY_FIREBASE_M_DEVICE);
			CCommonAppInfoStorage.Instance.DeviceConfig = oDeviceConfig.ExJSONStringToObj<STDeviceConfig>();
#endif			// #if MSG_PACK_ENABLE
		}
	}
#endif			// #if FIREBASE_MODULE_ENABLE

#if UNITY_SERVICE_MODULE_ENABLE
	//! 유니티 서비스 관리자가 초기화 되었을 경우
	public static void OnInitUnityServiceManager(CUnityServiceManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog("CLateSetupSceneManager.OnInitUnityServiceManager: {0}", a_bIsSuccess);

		// 초기화 되었을 경우
		if(a_bIsSuccess) {
#if MSG_PACK_ENABLE
#if UNITY_SERVICE_ANALYTICS_ENABLE
			CUnityServiceManager.Instance.SetAnalyticsUserID(CCommonAppInfoStorage.Instance.AppInfo.DeviceID);
			CUnityServiceManager.Instance.SendLog(KCDefine.U_LOG_NAME_APP_LAUNCH, null);
#endif			// #if UNITY_SERVICE_ANALYTICS_ENABLE

#if UNITY_SERVICE_CRASHLYTICS_ENABLE
			CUnityServiceManager.Instance.SetCrashDatas(new Dictionary<string, string>() {
				[KCDefine.U_LOG_KEY_USER_ID] = CCommonAppInfoStorage.Instance.AppInfo.DeviceID,
				[KCDefine.U_LOG_KEY_COUNTRY_CODE] = CCommonAppInfoStorage.Instance.CountryCode
			});
#endif			// #if UNITY_SERVICE_CRASHLYTICS_ENABLE
#endif			// #if MSG_PACK_ENABLE
		}
	}
#endif			// #if UNITY_SERVICE_MODULE_ENABLE

#if SINGULAR_MODULE_ENABLE
	//! 싱귤러 관리자가 초기화 되었을 경우
	public static void OnInitSingularManager(CSingularManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog("CLateSetupSceneManager.OnInitSingularManager: {0}", a_bIsSuccess);

		// 초기화 되었을 경우
		if(a_bIsSuccess) {
#if MSG_PACK_ENABLE && SINGULAR_ANALYTICS_ENABLE
			CSingularManager.Instance.SetAnalyticsUserID(CCommonAppInfoStorage.Instance.AppInfo.DeviceID);
			CSingularManager.Instance.SendLog(KCDefine.U_LOG_NAME_APP_LAUNCH, null);
#endif			// #if MSG_PACK_ENABLE && SINGULAR_ANALYTICS_ENABLE
		}
	}
#endif			// #if SINGULAR_MODULE_ENABLE

#if GAME_CENTER_MODULE_ENABLE
	//! 게임 센터 관리자가 초기화 되었을 경우
	public static void OnInitGameCenterManager(CGameCenterManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog("CLateSetupSceneManager.OnInitGameCenterManager: {0}", a_bIsSuccess);
	}
#endif			// #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE && MSG_PACK_ENABLE
	//! 결제 관리자가 초기화 되었을 경우
	public static void OnInitPurchaseManager(CPurchaseManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog("CLateSetupSceneManager.OnInitPurchaseManager: {0}", a_bIsSuccess);
	}
#endif			// #if PURCHASE_MODULE_ENABLE && MSG_PACK_ENABLE

#if LOCAL_NOTI_MODULE_ENABLE
	//! 로컬 알림 관리자가 초기화 되었을 경우
	public static void OnInitLocalNotiManager(CLocalNotiManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog("CLateSetupSceneManager.OnInitLocalNotiManager: {0}", a_bIsSuccess);
	}
#endif			// #if LOCAL_NOTI_MODULE_ENABLE
	#endregion			// 조건부 클래스 함수
}
