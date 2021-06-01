using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public static bool IsAutoLoadResumeAds { get; protected set; } = false;
#endif			// #if ADS_MODULE_ENABLE
	#endregion			// 클래스 프로퍼티

	#region 추상 함수
	//! 설명 팝업을 출력한다
	protected abstract void ShowDescPopup();
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
		// Do Nothing
	}

	//! 동의 뷰를 출력한다
	protected void ShowConsentView() {
		CUnityMsgSender.Inst.SendShowConsentViewMsg(this.HandleShowConsentViewMsg);
	}

	//! 초기화
	private IEnumerator OnStart() {
		CSceneLoader.Inst.UnloadSceneAsync(KCDefine.B_SCENE_N_AGREE, null);
		yield return CFactory.CreateWaitForSecs(KCDefine.U_DELAY_INIT);
		
		// 동의 뷰 출력이 가능 할 경우
		if(CAccess.IsEnableShowConsentView) {
			this.ShowDescPopup();
		} else {
			this.HandleShowConsentViewMsg(KCDefine.B_CMD_SHOW_CONSENT_VIEW, KCDefine.B_TRUE_STR);
		}
	}

	//! 서비스 관리자가 초기화 되었을 경우
	private static void OnInitServicesManager(CServicesManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitServicesManager: {a_bIsSuccess}");

		// 초기화 되었을 경우
		if(a_bIsSuccess) {
			CServicesManager.Inst.SetAnalyticsUserID(CCommonAppInfoStorage.Inst.AppInfo.DeviceID);
			CServicesManager.Inst.SendLog(KCDefine.L_LOG_N_APP_LAUNCH, null);

#if UNITY_IOS && APPLE_LOGIN_ENABLE
			CServicesManager.Inst.UpdateAppleLoginState(CLateSetupSceneManager.OnUpdateAppleLoginState);
#endif			// #if UNITY_IOS && APPLE_LOGIN_ENABLE
		}
	}

	//! 동의 뷰 출력 메세지를 처리한다
	private void HandleShowConsentViewMsg(string a_oCmd, string a_oMsg) {
		CFunc.ShowLog($"CLateSetupSceneManager.HandleShowConsentViewMsg: {a_oCmd}, {a_oMsg}");
		bool bIsValid = bool.TryParse(a_oMsg, out bool bIsSuccess);

		CCommonAppInfoStorage.Inst.AppInfo.IsAgreeTracking = bIsValid && bIsSuccess;
		CCommonAppInfoStorage.Inst.AppInfo.IsEnableShowDescPopup = false;

		CCommonAppInfoStorage.Inst.SaveAppInfo();

		// 관리자 자동 초기화 모드 일 경우
		if(this.IsAutoInitManager) {
			CServicesManager.Inst.Init(CLateSetupSceneManager.OnInitServicesManager);
			
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

					m_oAdsIDList = new Dictionary<string, string>() {
						[KCDefine.U_KEY_ADS_M_BANNER_ADS_ID] = CPluginInfoTable.Inst.AdmobPluginInfo.m_oBannerAdsID,
						[KCDefine.U_KEY_ADS_M_REWARD_ADS_ID] = CPluginInfoTable.Inst.AdmobPluginInfo.m_oRewardAdsID,
						[KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID] = CPluginInfoTable.Inst.AdmobPluginInfo.m_oFullscreenAdsID,
						[KCDefine.U_KEY_ADS_M_RESUME_ADS_ID] = CPluginInfoTable.Inst.AdmobPluginInfo.m_oResumeAdsID
					}
				},
#endif			// #if ADMOB_ENABLE

#if IRON_SRC_ENABLE
				m_stIronSrcParams = new CAdsManager.STIronSrcParams() {
					m_oAppKey = CPluginInfoTable.Inst.IronSrcPluginInfo.m_oAppKey,

					m_oAdsIDList = new Dictionary<string, string>() {
						[KCDefine.U_KEY_ADS_M_BANNER_ADS_ID] = CPluginInfoTable.Inst.IronSrcPluginInfo.m_oBannerAdsID,
						[KCDefine.U_KEY_ADS_M_REWARD_ADS_ID] = CPluginInfoTable.Inst.IronSrcPluginInfo.m_oRewardAdsID,
						[KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID] = CPluginInfoTable.Inst.IronSrcPluginInfo.m_oFullscreenAdsID
					}
				},
#endif			// #if IRON_SRC_ENABLE

#if APP_LOVIN_ENABLE
				m_stAppLovinParams = new CAdsManager.STAppLovinParams() {
					m_oSDKKey = CPluginInfoTable.Inst.AppLovinSDKKey,

					m_oAdsIDList = new Dictionary<string, string>() {
						[KCDefine.U_KEY_ADS_M_BANNER_ADS_ID] = CPluginInfoTable.Inst.AppLovinPluginInfo.m_oBannerAdsID,
						[KCDefine.U_KEY_ADS_M_REWARD_ADS_ID] = CPluginInfoTable.Inst.AppLovinPluginInfo.m_oRewardAdsID,
						[KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID] = CPluginInfoTable.Inst.AppLovinPluginInfo.m_oFullscreenAdsID
					}
				}
#endif			// #if APP_LOVIN_ENABLE
			};

			CUnityMsgSender.Inst.SendSetEnableAdsTrackingMsg(true);
			CAdsManager.Inst.Init(stAdsParams, CLateSetupSceneManager.OnInitAdsManager);
#endif			// #if ADS_MODULE_ENABLE

#if FLURRY_MODULE_ENABLE
			var stFlurryParams = new CFlurryManager.STParams() {
				m_oAPIKey = CPluginInfoTable.Inst.FlurryAPIKey
			};

			CFlurryManager.Inst.Init(stFlurryParams, CLateSetupSceneManager.OnInitFlurryManager);
#endif			// #if FLURRY_MODULE_ENABLE

#if TENJIN_MODULE_ENABLE
			var stTenjinParams = new CTenjinManager.STParams() {
				m_oAPIKey = CPluginInfoTable.Inst.TenjinAPIKey
			};

			CTenjinManager.Inst.Init(stTenjinParams, CLateSetupSceneManager.OnInitTenjinManager);
#endif			// #if TENJIN_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
			CFacebookManager.Inst.Init(CLateSetupSceneManager.OnInitFacebookManager);
#endif			// #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
			var oGameConfig = CResManager.Inst.GetRes<TextAsset>(KCDefine.U_DATA_P_G_GAME_CONFIG);
			var oBuildVerConfig = CResManager.Inst.GetRes<TextAsset>(KCDefine.U_DATA_P_G_BUILD_VER_CONFIG);

			var stFirebaseParams = new CFirebaseManager.STParams() {
				m_oConfigList = new Dictionary<string, object>() {
					[KCDefine.U_CONFIG_KEY_FIREBASE_M_GAME] = oGameConfig.text,
					[KCDefine.U_CONFIG_KEY_FIREBASE_M_DEVICE] = CDeviceInfoTable.Inst.DeviceConfig.ExToJSONStr(),
					[KCDefine.U_CONFIG_KEY_FIREBASE_M_BUILD_VER] = oBuildVerConfig.text
				}
			};

			CResManager.Inst.RemoveRes<TextAsset>(KCDefine.U_DATA_P_G_GAME_CONFIG, true);
			CResManager.Inst.RemoveRes<TextAsset>(KCDefine.U_DATA_P_G_BUILD_VER_CONFIG, true);

			CFirebaseManager.Inst.Init(stFirebaseParams, CLateSetupSceneManager.OnInitFirebaseManager);
#endif			// #if FIREBASE_MODULE_ENABLE

#if SINGULAR_MODULE_ENABLE
			var stSingularParams = new CSingularManager.STParams() {
				m_oAPIKey = CPluginInfoTable.Inst.SingularPluginInfo.m_oAPIKey,
				m_oAPISecret = CPluginInfoTable.Inst.SingularPluginInfo.m_oAPISecret
			};
			
			CSingularManager.Inst.Init(stSingularParams, CLateSetupSceneManager.OnInitSingularManager);
#endif			// #if SINGULAR_MODULE_ENABLE

#if GAME_CENTER_MODULE_ENABLE
			CGameCenterManager.Inst.Init(CLateSetupSceneManager.OnInitGameCenterManager);
#endif			// #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
			var stPurchaseParams = new CPurchaseManager.STParams() {
				m_oProductInfoList = CProductInfoTable.Inst.ProductInfoList
			};
			
			CPurchaseManager.Inst.Init(stPurchaseParams, CLateSetupSceneManager.OnInitPurchaseManager);
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

			CNotiManager.Inst.Init(stNotiParams, CLateSetupSceneManager.OnInitNotiManager);
#endif			// #if NOTI_MODULE_ENABLE
		}

		this.Setup();
		CFunc.BroadcastMsg(KCDefine.SS_FUNC_N_START_SCENE_EVENT, EStartSceneEvent.LOAD_PERMISSION_SCENE);

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
		
		// 초기화 되었을 경우
		if(a_bIsSuccess) {
#if ADMOB_ENABLE && (UNITY_IOS || UNITY_ANDROID)
			// 애드 몹 일 경우
			if(a_eAdsType == EAdsType.ADMOB) {
#if UNITY_IOS
				var oAdmobIDList = CDeviceInfoTable.Inst.DeviceInfo.m_oiOSAdmobIDList;
#else
				var oAdmobIDList = CDeviceInfoTable.Inst.DeviceInfo.m_oAndroidAdmobIDList;
#endif			// #if UNITY_IOS

				CUnityMsgSender.Inst.SendInitAdsMsg(CPluginInfoTable.Inst.AdmobPluginInfo.m_oResumeAdsID, oAdmobIDList, CLateSetupSceneManager.HandleInitAdsMsg);
			}
#endif			// #if ADMOB_ENABLE && (UNITY_IOS || UNITY_ANDROID)

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

#if ADMOB_ENABLE && (UNITY_IOS || UNITY_ANDROID)
	//! 광고 초기화 메세지를 처리한다
	private static void HandleInitAdsMsg(string a_oCmd, string a_oMsg) {
		CFunc.ShowLog($"CLateSetupSceneManager.HandleInitAdsMsg: {a_oCmd}, {a_oMsg}");

		bool bIsValid = bool.TryParse(a_oMsg, out bool bIsSuccess) && CLateSetupSceneManager.IsAutoLoadResumeAds;
		bool bIsEnableLoadResumeAds = (bIsValid && bIsSuccess) && !CCommonUserInfoStorage.Inst.UserInfo.IsRemoveAds;

		// 재개 광고 로드가 가능 할 경우
		if(bIsEnableLoadResumeAds && CPluginInfoTable.Inst.AdmobPluginInfo.m_oResumeAdsID.ExIsValid()) {
			CAdsManager.Inst.LoadResumeAds(EAdsType.ADMOB);
		}
	}
#endif			// #if ADMOB_ENABLE && (UNITY_IOS || UNITY_ANDROID)
#endif			// #if ADS_MODULE_ENABLE

#if FLURRY_MODULE_ENABLE
	//! 플러리 관리자가 초기화 되었을 경우
	private static void OnInitFlurryManager(CFlurryManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitFlurryManager: {a_bIsSuccess}");

		// 초기화 되었을 경우
		if(a_bIsSuccess) {
			CFlurryManager.Inst.SetAnalyticsUserID(CCommonAppInfoStorage.Inst.AppInfo.DeviceID);
			CFlurryManager.Inst.SendLog(KCDefine.L_LOG_N_APP_LAUNCH, null);
		}
	}
#endif			// #if FLURRY_MODULE_ENABLE

#if TENJIN_MODULE_ENABLE
	//! 텐진 관리자가 초기화 되었을 경우
	private static void OnInitTenjinManager(CTenjinManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitTenjinManager: {a_bIsSuccess}");

		// 초기화 되었을 경우
		if(a_bIsSuccess) {
			CTenjinManager.Inst.SendLog(KCDefine.L_LOG_N_APP_LAUNCH, null);
		}
	}
#endif			// #if TENJIN_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
	//! 페이스 북 관리자가 초기화 되었을 경우
	private static void OnInitFacebookManager(CFacebookManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitFacebookManager: {a_bIsSuccess}");
	}
#endif			// #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
	//! 파이어 베이스 관리자가 초기화 되었을 경우
	private static void OnInitFirebaseManager(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitFirebaseManager: {a_bIsSuccess}");

		// 초기화 되었을 경우
		if(a_bIsSuccess) {
			CFirebaseManager.Inst.SetAnalyticsDatas(new Dictionary<string, string>() {
				[KCDefine.L_LOG_KEY_COUNTRY_CODE] = CCommonAppInfoStorage.Inst.CountryCode
			});

			CFirebaseManager.Inst.SetCrashDatas(new Dictionary<string, string>() {
				[KCDefine.L_LOG_KEY_COUNTRY_CODE] = CCommonAppInfoStorage.Inst.CountryCode
			});
			
			CFirebaseManager.Inst.SetAnalyticsUserID(CCommonAppInfoStorage.Inst.AppInfo.DeviceID);
			CFirebaseManager.Inst.SetCrashUserID(CCommonAppInfoStorage.Inst.AppInfo.DeviceID);

			CFirebaseManager.Inst.SendLog(KCDefine.L_LOG_N_APP_LAUNCH, null);
			CFirebaseManager.Inst.StartTracking(KCDefine.U_TRACKING_N_APP_LAUNCH, null);

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

#if SINGULAR_MODULE_ENABLE
	//! 싱귤러 관리자가 초기화 되었을 경우
	private static void OnInitSingularManager(CSingularManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitSingularManager: {a_bIsSuccess}");

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
	}
#endif			// #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	//! 결제 관리자가 초기화 되었을 경우
	private static void OnInitPurchaseManager(CPurchaseManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitPurchaseManager: {a_bIsSuccess}");
	}
#endif			// #if PURCHASE_MODULE_ENABLE

#if NOTI_MODULE_ENABLE
	//! 알림 관리자가 초기화 되었을 경우
	private static void OnInitNotiManager(CNotiManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitNotiManager: {a_bIsSuccess}");
	}
#endif			// #if NOTI_MODULE_ENABLE
	#endregion			// 조건부 클래스 함수
}
