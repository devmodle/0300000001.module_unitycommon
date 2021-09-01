using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif			// #if UNITY_IOS

//! 지연 설정 씬 관리자
public abstract partial class CLateSetupSceneManager : CSceneManager {
	#region 프로퍼티
	public bool IsAutoInitManager { get; protected set; } = false;
	public override string SceneName => KCDefine.B_SCENE_N_LATE_SETUP;

#if UNITY_EDITOR
	public override int ScriptOrder => KCDefine.U_SCRIPT_O_LATE_SETUP_SCENE_MANAGER;
#endif			// #if UNITY_EDITOR
	#endregion			// 프로퍼티

	#region 클래스 프로퍼티
#if ADS_MODULE_ENABLE
	public static bool IsAutoLoadBannerAds { get; protected set; } = false;
	public static bool IsAutoLoadRewardAds { get; protected set; } = false;
	public static bool IsAutoLoadFullscreenAds { get; protected set; } = false;
#endif			// #if ADS_MODULE_ENABLE
	#endregion			// 클래스 프로퍼티

	#region 추상 함수
	//! 추적 설명 팝업을 출력한다
	protected abstract void ShowTrackingDescPopup();
	#endregion			// 추상 함수

	#region 함수
	//! 초기화
	public sealed override void Start() {
		base.Start();

		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			StartCoroutine(this.OnStart());
		}
	}

	//! 씬을 설정한다
	protected virtual void Setup() {
		// Do Something
	}

	//! 동의 뷰를 출력한다
	protected void ShowConsentView() {
#if UNITY_IOS
		ATTrackingStatusBinding.RequestAuthorizationTracking();

		this.ExRepeatCallFunc((a_oSender, a_oParams, a_bIsComplete) => {
			// 완료 되었을 경우
			if(a_bIsComplete) {
				var eStatus = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
				this.OnCloseConsentView(eStatus != ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED && eStatus != ATTrackingStatusBinding.AuthorizationTrackingStatus.DENIED);
			}
			
			return ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED;
		}, KCDefine.U_DELAY_INIT, KCDefine.B_MAX_DELTA_T_CONSENT_VIEW);
#else
		this.ExLateCallFunc((a_oSender, a_oParams) => this.OnCloseConsentView(true));
#endif			// #if UNITY_IOS
	}

	//! 초기화
	private IEnumerator OnStart() {
		yield return CFactory.CreateWaitForSecs(KCDefine.U_DELAY_INIT);
		CSceneLoader.Inst.UnloadSceneAsync(KCDefine.B_SCENE_N_AGREE, null);
		
		// 동의 뷰 출력이 가능 할 경우
		if(CAccess.IsEnableShowConsentView) {
			this.ShowTrackingDescPopup();
		} else {
			this.OnCloseConsentView(true);
		}
	}

	//! 서비스 관리자가 초기화 되었을 경우
	private static void OnInitServicesManager(CServicesManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitServicesManager: {a_bIsSuccess}");
		CFunc.BroadcastMsg(KCDefine.U_FUNC_N_ON_INIT_SERVICES_MANAGER, a_oSender);

		// 초기화 되었을 경우
		if(a_bIsSuccess) {
			CServicesManager.Inst.SetAnalyticsUserID(CCommonAppInfoStorage.Inst.AppInfo.DeviceID);
			CServicesManager.Inst.SendLog(KCDefine.L_LOG_N_APP_LAUNCH, null);

#if UNITY_IOS && APPLE_LOGIN_ENABLE
			CServicesManager.Inst.UpdateAppleLoginState(CLateSetupSceneManager.OnUpdateAppleLoginState);
#endif			// #if UNITY_IOS && APPLE_LOGIN_ENABLE
		}
	}

	//! 동의 뷰가 닫혔을 경우
	private void OnCloseConsentView(bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnCloseConsentView: {a_bIsSuccess}");

		CCommonAppInfoStorage.Inst.AppInfo.IsAgreeTracking = a_bIsSuccess;
		CCommonAppInfoStorage.Inst.AppInfo.IsEnableShowTrackingDescPopup = false;

		CCommonAppInfoStorage.Inst.SaveAppInfo();

		// 관리자 자동 초기화 모드 일 경우
		if(this.IsAutoInitManager) {
			CServicesManager.Inst.Init(new CServicesManager.STCallbackParams() {
				m_oCallback = CLateSetupSceneManager.OnInitServicesManager
			});
			
#if ADS_MODULE_ENABLE
			var stAdsParams = new CAdsManager.STParams() {
				m_eDefAdsType = CPluginInfoTable.Inst.DefAdsType,
				m_eBannerAdsPos = CPluginInfoTable.Inst.BannerAdsPos,

#if ADMOB_ENABLE
				m_stAdmobParams = new CAdsManager.STAdmobParams() {
#if UNITY_IOS
					m_oAdmobIDList = CDeviceInfoTable.Inst.DeviceInfo.m_oiOSAdmobIDList,
#elif UNITY_ANDROID
					m_oAdmobIDList = CDeviceInfoTable.Inst.DeviceInfo.m_oAndroidAdmobIDList,
#else
					m_oAdmobIDList = new List<string>(),
#endif			// #if UNITY_IOS

					m_oAdsIDDict = new Dictionary<string, string>() {
						[KCDefine.U_KEY_ADS_M_BANNER_ADS_ID] = CPluginInfoTable.Inst.AdmobPluginInfo.m_oBannerAdsID,
						[KCDefine.U_KEY_ADS_M_REWARD_ADS_ID] = CPluginInfoTable.Inst.AdmobPluginInfo.m_oRewardAdsID,
						[KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID] = CPluginInfoTable.Inst.AdmobPluginInfo.m_oFullscreenAdsID
					}
				},
#endif			// #if ADMOB_ENABLE

#if IRON_SRC_ENABLE
				m_stIronSrcParams = new CAdsManager.STIronSrcParams() {
					m_oAppKey = CPluginInfoTable.Inst.IronSrcPluginInfo.m_oAppKey,

					m_oAdsIDDict = new Dictionary<string, string>() {
						[KCDefine.U_KEY_ADS_M_BANNER_ADS_ID] = CPluginInfoTable.Inst.IronSrcPluginInfo.m_oBannerAdsID,
						[KCDefine.U_KEY_ADS_M_REWARD_ADS_ID] = CPluginInfoTable.Inst.IronSrcPluginInfo.m_oRewardAdsID,
						[KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID] = CPluginInfoTable.Inst.IronSrcPluginInfo.m_oFullscreenAdsID
					}
				},
#endif			// #if IRON_SRC_ENABLE

#if APP_LOVIN_ENABLE
				m_stAppLovinParams = new CAdsManager.STAppLovinParams() {
					m_oSDKKey = CPluginInfoTable.Inst.AppLovinSDKKey,

					m_oAdsIDDict = new Dictionary<string, string>() {
						[KCDefine.U_KEY_ADS_M_BANNER_ADS_ID] = CPluginInfoTable.Inst.AppLovinPluginInfo.m_oBannerAdsID,
						[KCDefine.U_KEY_ADS_M_REWARD_ADS_ID] = CPluginInfoTable.Inst.AppLovinPluginInfo.m_oRewardAdsID,
						[KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID] = CPluginInfoTable.Inst.AppLovinPluginInfo.m_oFullscreenAdsID
					}
				}
#endif			// #if APP_LOVIN_ENABLE
			};

			var stAdsCallbackParams = new CAdsManager.STCallbackParams() {
				m_oCallback = CLateSetupSceneManager.OnInitAdsManager
			};

			CUnityMsgSender.Inst.SendSetEnableAdsTrackingMsg(true);
			CAdsManager.Inst.Init(stAdsParams, stAdsCallbackParams);
#endif			// #if ADS_MODULE_ENABLE

#if FLURRY_MODULE_ENABLE
			var stFlurryParams = new CFlurryManager.STParams() {
				m_oAPIKey = CPluginInfoTable.Inst.FlurryAPIKey
			};

			var stFlurryCallbackParams = new CFlurryManager.STCallbackParams() {
				m_oCallback = CLateSetupSceneManager.OnInitFlurryManager
			};

			CFlurryManager.Inst.Init(stFlurryParams, stFlurryCallbackParams);
#endif			// #if FLURRY_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
			var stFacebookCallbackParams = new CFacebookManager.STCallbackParams() {
				m_oCallback = CLateSetupSceneManager.OnInitFacebookManager
			};

			CFacebookManager.Inst.Init(stFacebookCallbackParams);
#endif			// #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
#if UNITY_EDITOR || UNITY_STANDALONE
			string oGameConfigStr = CFunc.ReadStr(KCDefine.U_RUNTIME_DATA_P_G_GAME_CONFIG);
			string oBuildVerConfigStr = CFunc.ReadStr(KCDefine.U_RUNTIME_DATA_P_G_BUILD_VER_CONFIG);
#else
			var oGameConfig = CResManager.Inst.GetRes<TextAsset>(KCDefine.U_DATA_P_G_GAME_CONFIG);
			var oBuildVerConfig = CResManager.Inst.GetRes<TextAsset>(KCDefine.U_DATA_P_G_BUILD_VER_CONFIG);
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE

			var stFirebaseParams = new CFirebaseManager.STParams() {
				m_oConfigDict = new Dictionary<string, object>() {
					[KCDefine.U_CONFIG_KEY_FIREBASE_M_DEVICE] = CDeviceInfoTable.Inst.DeviceConfig.ExToJSONStr(),

#if UNITY_EDITOR || UNITY_STANDALONE
					[KCDefine.U_CONFIG_KEY_FIREBASE_M_GAME] = oGameConfigStr,
					[KCDefine.U_CONFIG_KEY_FIREBASE_M_BUILD_VER] = oBuildVerConfigStr
#else
					[KCDefine.U_CONFIG_KEY_FIREBASE_M_GAME] = oGameConfig.text,
					[KCDefine.U_CONFIG_KEY_FIREBASE_M_BUILD_VER] = oBuildVerConfig.text
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
				}
			};

			var stFirebaseCallbackParams = new CFirebaseManager.STCallbackParams() {
				m_oCallback = CLateSetupSceneManager.OnInitFirebaseManager
			};

			CResManager.Inst.RemoveRes<TextAsset>(KCDefine.U_DATA_P_G_GAME_CONFIG, true);
			CResManager.Inst.RemoveRes<TextAsset>(KCDefine.U_DATA_P_G_BUILD_VER_CONFIG, true);

			CFirebaseManager.Inst.Init(stFirebaseParams, stFirebaseCallbackParams);
#endif			// #if FIREBASE_MODULE_ENABLE

#if APPS_FLYER_MODULE_ENABLE
			var stAppsFlyerParams = new CAppsFlyerManager.STParams() {
				m_oDevKey = CPluginInfoTable.Inst.AppsFlyerPluginInfo.m_oDevKey,

#if UNITY_IOS
				m_oAppID = CPluginInfoTable.Inst.AppsFlyerPluginInfo.m_oAppID
#else
				m_oAppID = string.Empty
#endif			// #if UNITY_IOS
			};

			var stAppsFlyerCallbackParams = new CAppsFlyerManager.STCallbackParams() {
				m_oCallback = CLateSetupSceneManager.OnInitAppsFlyerManager
			};

			CAppsFlyerManager.Inst.Init(stAppsFlyerParams, stAppsFlyerCallbackParams);
#endif			// #if APPS_FLYER_MODULE_ENABLE

#if GAME_ANALYTICS_MODULE_ENABLE
			var stGameAnalyticsCallbackParams = new CGameAnalyticsManager.STCallbackParams() {
				m_oCallback = CLateSetupSceneManager.OnInitGameAnalyticsManager
			};

			CGameAnalyticsManager.Inst.Init(stGameAnalyticsCallbackParams);
#endif			// #if GAME_ANALYTICS_MODULE_ENABLE

#if SINGULAR_MODULE_ENABLE
			var stSingularParams = new CSingularManager.STParams() {
				m_oAPIKey = CPluginInfoTable.Inst.SingularPluginInfo.m_oAPIKey,
				m_oAPISecret = CPluginInfoTable.Inst.SingularPluginInfo.m_oAPISecret
			};

			var stSingularCallbackParams = new CSingularManager.STCallbackParams() {
				m_oCallback = CLateSetupSceneManager.OnInitSingularManager
			};
			
			CSingularManager.Inst.Init(stSingularParams, stSingularCallbackParams);
#endif			// #if SINGULAR_MODULE_ENABLE

#if GAME_CENTER_MODULE_ENABLE
			var stGameCenterCallbackParams = new CGameCenterManager.STCallbackParams() {
				m_oCallback = CLateSetupSceneManager.OnInitGameCenterManager
			};

			CGameCenterManager.Inst.Init(stGameCenterCallbackParams);
#endif			// #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
			var stPurchaseParams = new CPurchaseManager.STParams() {
				m_oProductInfoList = CProductInfoTable.Inst.ProductInfoList
			};

			var stPurchaseCallbackParams = new CPurchaseManager.STCallbackParams() {
				m_oCallback = CLateSetupSceneManager.OnInitPurchaseManager
			};
			
			CPurchaseManager.Inst.Init(stPurchaseParams, stPurchaseCallbackParams);
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

			var stNotiCallbackParams = new CNotiManager.STCallbackParams() {
				m_oCallback = CLateSetupSceneManager.OnInitNotiManager
			};

			CNotiManager.Inst.Init(stNotiParams, stNotiCallbackParams);
#endif			// #if NOTI_MODULE_ENABLE
		}

		this.Setup();
		CFunc.BroadcastMsg(KCDefine.SS_FUNC_N_START_SCENE_EVENT, EStartSceneEvent.LOAD_PERMISSION_SCENE, false);

		CSceneManager.IsLateSetup = true;
		CSceneLoader.Inst.LoadAdditiveScene(KCDefine.B_SCENE_N_PERMISSION);
	}
	#endregion			// 함수

	#region 조건부 클래스 함수
#if UNITY_IOS && APPLE_LOGIN_ENABLE
	//! 애플 로그인 상태가 갱신 되었을 경우
	private static void OnUpdateAppleLoginState(CServicesManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnUpdateAppleLoginState: {a_bIsSuccess}");
	}
#endif			// #if UNITY_IOS && APPLE_LOGIN_ENABLE

#if ADS_MODULE_ENABLE
	//! 광고 관리자가 초기화 되었을 경우
	private static void OnInitAdsManager(CAdsManager a_oSender, EAdsType a_eAdsType, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitAdsManager: {a_eAdsType}, {a_bIsSuccess}");
		CFunc.BroadcastMsg(KCDefine.U_FUNC_N_ON_INIT_ADS_MANAGER, a_oSender);
		
		// 초기화 되었을 경우
		if(a_bIsSuccess) {
			bool bIsEnableLoadBannerAds = CPluginInfoTable.Inst.GetBannerAdsID(a_eAdsType).ExIsValid() && !CCommonUserInfoStorage.Inst.UserInfo.IsRemoveAds;
			bIsEnableLoadBannerAds = bIsEnableLoadBannerAds && CPluginInfoTable.Inst.DefAdsType == a_eAdsType;

			bool bIsEnableLoadRewardAds = CPluginInfoTable.Inst.GetRewardAdsID(a_eAdsType).ExIsValid();
			bool bIsEnableLoadFullscreenAds = CPluginInfoTable.Inst.GetFullscreenAdsID(a_eAdsType).ExIsValid() && !CCommonUserInfoStorage.Inst.UserInfo.IsRemoveAds;

			// 배너 광고 로드가 가능 할 경우
			if(bIsEnableLoadBannerAds && CLateSetupSceneManager.IsAutoLoadBannerAds) {
				CAdsManager.Inst.LoadBannerAds(a_eAdsType);
			}

			// 보상 광고 로드가 가능 할 경우
			if(bIsEnableLoadRewardAds && CLateSetupSceneManager.IsAutoLoadRewardAds) {
				CAdsManager.Inst.LoadRewardAds(a_eAdsType);
			}

			// 전면 광고 로드가 가능 할 경우
			if(bIsEnableLoadFullscreenAds && CLateSetupSceneManager.IsAutoLoadFullscreenAds) {
				CAdsManager.Inst.LoadFullscreenAds(a_eAdsType);
			}
		}
	}
#endif			// #if ADS_MODULE_ENABLE

#if FLURRY_MODULE_ENABLE
	//! 플러리 관리자가 초기화 되었을 경우
	private static void OnInitFlurryManager(CFlurryManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitFlurryManager: {a_bIsSuccess}");
		CFunc.BroadcastMsg(KCDefine.U_FUNC_N_ON_INIT_FLURRY_MANAGER, a_oSender);

		// 초기화 되었을 경우
		if(a_bIsSuccess) {
			CFlurryManager.Inst.SetAnalyticsUserID(CCommonAppInfoStorage.Inst.AppInfo.DeviceID);
			CFlurryManager.Inst.SendLog(KCDefine.L_LOG_N_APP_LAUNCH, null);
		}
	}
#endif			// #if FLURRY_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
	//! 페이스 북 관리자가 초기화 되었을 경우
	private static void OnInitFacebookManager(CFacebookManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitFacebookManager: {a_bIsSuccess}");
		CFunc.BroadcastMsg(KCDefine.U_FUNC_N_ON_INIT_FACEBOOK_MANAGER, null);
	}
#endif			// #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
	//! 파이어 베이스 관리자가 초기화 되었을 경우
	private static void OnInitFirebaseManager(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitFirebaseManager: {a_bIsSuccess}");
		CFunc.BroadcastMsg(KCDefine.U_FUNC_N_ON_INIT_FIREBASE_MANAGER, null);

		// 초기화 되었을 경우
		if(a_bIsSuccess) {
			CFirebaseManager.Inst.SetCrashDatas(new Dictionary<string, string>() {
				[KCDefine.L_LOG_KEY_COUNTRY_CODE] = CCommonAppInfoStorage.Inst.CountryCode
			});
			
			CFirebaseManager.Inst.SetAnalyticsUserID(CCommonAppInfoStorage.Inst.AppInfo.DeviceID);
			CFirebaseManager.Inst.SetCrashUserID(CCommonAppInfoStorage.Inst.AppInfo.DeviceID);

			CFirebaseManager.Inst.SendLog(KCDefine.L_LOG_N_APP_LAUNCH, null);

#if FIREBASE_REMOTE_CONFIG_ENABLE
			CFirebaseManager.Inst.LoadConfig(CLateSetupSceneManager.OnLoadConfig);
#endif			// #if FIREBASE_REMOTE_CONFIG_ENABLE
		}
	}

#if FIREBASE_REMOTE_CONFIG_ENABLE
	//! 속성이 로드 되었을 경우
	private static void OnLoadConfig(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnLoadConfig: {a_bIsSuccess}");

		// 속성이 로드 되었을 경우
		if(a_bIsSuccess) {
			string oDeviceConfig = CFirebaseManager.Inst.GetConfig(KCDefine.U_CONFIG_KEY_FIREBASE_M_DEVICE);
			CCommonAppInfoStorage.Inst.DeviceConfig = oDeviceConfig.ExJSONStrToObj<STDeviceConfig>();
		}
	}
#endif			// #if FIREBASE_REMOTE_CONFIG_ENABLE
#endif			// #if FIREBASE_MODULE_ENABLE

#if APPS_FLYER_MODULE_ENABLE
	//! 앱스 플라이어 관리자가 초기화 되었을 경우
	private static void OnInitAppsFlyerManager(CAppsFlyerManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitAppsFlyerManager: {a_bIsSuccess}");
		CFunc.BroadcastMsg(KCDefine.U_FUNC_N_ON_INIT_APPS_FLYER_MANAGER, null);

		// 초기화 되었을 경우
		if(a_bIsSuccess) {
			CAppsFlyerManager.Inst.SetAnalyticsUserID(CCommonAppInfoStorage.Inst.AppInfo.DeviceID);
			CAppsFlyerManager.Inst.SendLog(KCDefine.L_LOG_N_APP_LAUNCH, null);
		}
	}
#endif			// #if APPS_FLYER_MODULE_ENABLE

#if GAME_ANALYTICS_MODULE_ENABLE
	//! 게임 분석 관리자가 초기화 되었을 경우
	private static void OnInitGameAnalyticsManager(CGameAnalyticsManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitGameAnalyticsManager: {a_bIsSuccess}");
		CFunc.BroadcastMsg(KCDefine.U_FUNC_N_ON_INIT_GAME_ANALYTICS_MANAGER, null);

		// 초기화 되었을 경우
		if(a_bIsSuccess) {
			CGameAnalyticsManager.Inst.SetAnalyticsUserID(CCommonAppInfoStorage.Inst.AppInfo.DeviceID);
			CGameAnalyticsManager.Inst.SendLog(KCDefine.L_LOG_N_APP_LAUNCH, null);
		}
	}
#endif			// #if GAME_ANALYTICS_MODULE_ENABLE

#if SINGULAR_MODULE_ENABLE
	//! 싱귤러 관리자가 초기화 되었을 경우
	private static void OnInitSingularManager(CSingularManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitSingularManager: {a_bIsSuccess}");
		CFunc.BroadcastMsg(KCDefine.U_FUNC_N_ON_INIT_SINGULAR_MANAGER, null);

		// 초기화 되었을 경우
		if(a_bIsSuccess) {
			CSingularManager.Inst.SetAnalyticsUserID(CCommonAppInfoStorage.Inst.AppInfo.DeviceID);
			CSingularManager.Inst.SendLog(KCDefine.L_LOG_N_APP_LAUNCH, null);
		}
	}
#endif			// #if SINGULAR_MODULE_ENABLE

#if GAME_CENTER_MODULE_ENABLE
	//! 게임 센터 관리자가 초기화 되었을 경우
	private static void OnInitGameCenterManager(CGameCenterManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitGameCenterManager: {a_bIsSuccess}");
		CFunc.BroadcastMsg(KCDefine.U_FUNC_N_ON_INIT_GAME_CENTER_MANAGER, null);
	}
#endif			// #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	//! 결제 관리자가 초기화 되었을 경우
	private static void OnInitPurchaseManager(CPurchaseManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitPurchaseManager: {a_bIsSuccess}");
		CFunc.BroadcastMsg(KCDefine.U_FUNC_N_ON_INIT_PURCHASE_MANAGER, null);
	}
#endif			// #if PURCHASE_MODULE_ENABLE

#if NOTI_MODULE_ENABLE
	//! 알림 관리자가 초기화 되었을 경우
	private static void OnInitNotiManager(CNotiManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitNotiManager: {a_bIsSuccess}");
		CFunc.BroadcastMsg(KCDefine.U_FUNC_N_ON_INIT_NOTI_MANAGER, null);
	}
#endif			// #if NOTI_MODULE_ENABLE
	#endregion			// 조건부 클래스 함수
}
