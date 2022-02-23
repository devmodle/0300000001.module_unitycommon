using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#elif UNITY_ANDROID
using UnityEngine.Android;
#endif			// #if UNITY_IOS

/** 지연 설정 씬 관리자 */
public abstract partial class CLateSetupSceneManager : CSceneManager {
	#region 프로퍼티
	public bool IsAutoInitManager { get; protected set; } = false;
	public override string SceneName => KCDefine.B_SCENE_N_LATE_SETUP;

#if UNITY_EDITOR
	public override int ScriptOrder => KCDefine.U_SCRIPT_O_LATE_SETUP_SCENE_MANAGER;
#endif			// #if UNITY_EDITOR

#if UNITY_ANDROID
	public List<string> m_oPermissionList { get; private set; } = new List<string>();
#endif			// #if UNITY_ANDROID
	#endregion			// 프로퍼티

	#region 클래스 프로퍼티
#if ADS_MODULE_ENABLE
	public static bool IsAutoLoadBannerAds { get; protected set; } = false;
	public static bool IsAutoLoadRewardAds { get; protected set; } = false;
	public static bool IsAutoLoadFullscreenAds { get; protected set; } = false;
#endif			// #if ADS_MODULE_ENABLE
	#endregion			// 클래스 프로퍼티

	#region 추상 함수
	/** 추적 설명 팝업을 출력한다 */
	protected abstract void ShowTrackingDescPopup();

#if UNITY_ANDROID
	/** 권한을 요청한다 */
	protected abstract void RequestPermission(string a_oPermission, System.Action<string, bool> a_oCallback);
#endif			// #if UNITY_ANDROID
	#endregion			// 추상 함수

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			CFunc.BroadcastMsg(KCDefine.SS_FUNC_N_START_SCENE_EVENT, EStartSceneEvent.LOAD_LATE_SETUP_SCENE, false);
		}
	}

	/** 초기화 */
	public sealed override void Start() {
		base.Start();

		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			StartCoroutine(this.OnStart());
		}
	}

	/** 씬을 설정한다 */
	protected virtual void Setup() {
		// Do Something
	}

	/** 동의 뷰를 출력한다 */
	protected void ShowConsentView() {
#if UNITY_IOS
		ATTrackingStatusBinding.RequestAuthorizationTracking();

		this.ExRepeatCallFunc((a_oSender, a_bIsComplete) => {
			// 완료 되었을 경우
			if(a_bIsComplete) {
				var eStatus = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
				this.OnCloseConsentView(eStatus != ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED && eStatus != ATTrackingStatusBinding.AuthorizationTrackingStatus.DENIED);
			}
			
			return ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED;
		}, KCDefine.U_DELAY_INIT, KCDefine.B_MAX_DELTA_T_CONSENT_VIEW);
#else
		this.ExLateCallFunc((a_oSender) => this.OnCloseConsentView(true));
#endif			// #if UNITY_IOS
	}

	/** 초기화 */
	private IEnumerator OnStart() {
		yield return CFactory.CreateWaitForSecs(KCDefine.U_DELAY_INIT);
		
		// 동의 뷰 출력이 가능 할 경우
		if(CAccess.IsEnableShowConsentView) {
			this.ShowTrackingDescPopup();
		} else {
			this.OnCloseConsentView(true);
		}
	}

	/** 서비스 관리자가 초기화 되었을 경우 */
	private static void OnInitServicesManager(CServicesManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitServicesManager: {a_bIsSuccess}");

		// 초기화 되었을 경우
		if(a_bIsSuccess) {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
			CServicesManager.Inst.SetAnalyticsUserID(CCommonAppInfoStorage.Inst.AppInfo.DeviceID);
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE

			CServicesManager.Inst.SendLog(KCDefine.L_LOG_N_APP_LAUNCH, null);
			
#if UNITY_IOS && APPLE_LOGIN_ENABLE
			CServicesManager.Inst.UpdateAppleLoginState(CLateSetupSceneManager.OnUpdateAppleLoginState);
#endif			// #if UNITY_IOS && APPLE_LOGIN_ENABLE
		}
	}

	/** 동의 뷰가 닫혔을 경우 */
	private void OnCloseConsentView(bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnCloseConsentView: {a_bIsSuccess}");

#if NEWTON_SOFT_JSON_MODULE_ENABLE
		CCommonAppInfoStorage.Inst.AppInfo.IsAgreeTracking = a_bIsSuccess;
		CCommonAppInfoStorage.Inst.AppInfo.IsEnableShowTrackingDescPopup = false;

		CCommonAppInfoStorage.Inst.SaveAppInfo();
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE

		// 관리자 자동 초기화 모드 일 경우
		if(this.IsAutoInitManager) {
			CServicesManager.Inst.Init(new CServicesManager.STCallbackParams() {
				m_oCallback = CLateSetupSceneManager.OnInitServicesManager
			});
			
#if ADS_MODULE_ENABLE
			var stAdsParams = new CAdsManager.STParams() {
				m_eAdsPlatform = CPluginInfoTable.Inst.AdsPlatform, m_eBannerAdsPos = CPluginInfoTable.Inst.BannerAdsPos,

#if ADMOB_ADS_ENABLE
				m_stAdmobParams = new CAdsManager.STAdmobParams() {
#if UNITY_IOS
					m_oTestDeviceIDList = CDeviceInfoTable.Inst.DeviceInfo.m_oiOSTestDeviceAdmobIDList,
#elif UNITY_ANDROID
					m_oTestDeviceIDList = CDeviceInfoTable.Inst.DeviceInfo.m_oAndroidTestDeviceAdmobIDList,
#else
					m_oTestDeviceIDList = new List<string>(),
#endif			// #if UNITY_IOS

					m_oAdsIDDict = new Dictionary<string, string>() {
						[KCDefine.U_KEY_ADS_M_BANNER_ADS_ID] = CPluginInfoTable.Inst.GetBannerAdsID(EAdsPlatform.ADMOB), [KCDefine.U_KEY_ADS_M_REWARD_ADS_ID] = CPluginInfoTable.Inst.GetRewardAdsID(EAdsPlatform.ADMOB), [KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID] = CPluginInfoTable.Inst.GetFullscreenAdsID(EAdsPlatform.ADMOB)
					}
				},
#endif			// #if ADMOB_ADS_ENABLE

#if IRON_SRC_ADS_ENABLE
				m_stIronSrcParams = new CAdsManager.STIronSrcParams() {
					m_oAppKey = CPluginInfoTable.Inst.IronSrcPluginInfo.m_oAppKey,

					m_oAdsIDDict = new Dictionary<string, string>() {
						[KCDefine.U_KEY_ADS_M_BANNER_ADS_ID] = CPluginInfoTable.Inst.GetBannerAdsID(EAdsPlatform.IRON_SRC), [KCDefine.U_KEY_ADS_M_REWARD_ADS_ID] = CPluginInfoTable.Inst.GetBannerAdsID(EAdsPlatform.IRON_SRC), [KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID] = CPluginInfoTable.Inst.GetBannerAdsID(EAdsPlatform.IRON_SRC)
					}
				},
#endif			// #if IRON_SRC_ADS_ENABLE

#if APP_LOVIN_ADS_ENABLE
				m_stAppLovinParams = new CAdsManager.STAppLovinParams() {
					m_oSDKKey = CPluginInfoTable.Inst.AppLovinSDKKey,

					m_oAdsIDDict = new Dictionary<string, string>() {
						[KCDefine.U_KEY_ADS_M_BANNER_ADS_ID] = CPluginInfoTable.Inst.GetBannerAdsID(EAdsPlatform.APP_LOVIN), [KCDefine.U_KEY_ADS_M_REWARD_ADS_ID] = CPluginInfoTable.Inst.GetBannerAdsID(EAdsPlatform.APP_LOVIN), [KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID] = CPluginInfoTable.Inst.GetBannerAdsID(EAdsPlatform.APP_LOVIN)
					}
				}
#endif			// #if APP_LOVIN_ADS_ENABLE
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
					[KCDefine.U_KEY_FIREBASE_M_DEVICE_CONFIG] = CDeviceInfoTable.Inst.DeviceConfig.ExToJSONStr(),

#if UNITY_EDITOR || UNITY_STANDALONE
					[KCDefine.U_KEY_FIREBASE_M_GAME_CONFIG] = oGameConfigStr, [KCDefine.U_KEY_FIREBASE_M_BUILD_VER_CONFIG] = oBuildVerConfigStr
#else
					[KCDefine.U_KEY_FIREBASE_M_GAME_CONFIG] = oGameConfig.text, [KCDefine.U_KEY_FIREBASE_M_BUILD_VER_CONFIG] = oBuildVerConfig.text
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
				m_oAppID = CProjInfoTable.Inst.ProjInfo.m_oStoreAppID, m_oDevKey = CPluginInfoTable.Inst.AppsFlyerPluginInfo.m_oDevKey
			};

			var stAppsFlyerCallbackParams = new CAppsFlyerManager.STCallbackParams() {
				m_oCallback = CLateSetupSceneManager.OnInitAppsFlyerManager
			};

			CAppsFlyerManager.Inst.Init(stAppsFlyerParams, stAppsFlyerCallbackParams);
#endif			// #if APPS_FLYER_MODULE_ENABLE

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
				m_eAuthOpts = KCDefine.U_AUTH_OPTS_NOTI, m_ePresentOpts = KCDefine.U_PRESENT_OPTS_NOTI
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

#if UNITY_ANDROID
		this.CheckPermission();
#else
		this.LoadNextScene();
#endif			// #if UNITY_ANDROID
	}

	/** 다음 씬을 로드한다 */
	private void LoadNextScene() {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
		CCommonAppInfoStorage.Inst.SetupAdsID();
		CCommonAppInfoStorage.Inst.SetupStoreVer();
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE

		CSceneManager.IsLateSetup = true;
		CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_INTRO);
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_ANDROID
	/** 권한을 수신했을 경우 */
	private void OnReceivePermission(string a_oPermission, bool a_bIsSuccess) {
		m_oPermissionList.ExRemoveVal(a_oPermission);
		this.CheckPermission();
	}

	/** 권한을 검사한다 */
	private void CheckPermission() {
		// 권한이 필요 할 경우
		if(m_oPermissionList.ExIsValid()) {
			this.RequestPermission(m_oPermissionList[KCDefine.B_VAL_0_INT]);
		} else {
			this.LoadNextScene();
		}
	}

	/** 권한을 요청한다 */
	private void RequestPermission(string a_oPermission) {
		// 권한이 유효 할 경우
		if(CAccess.IsEnablePermission(a_oPermission)) {
			this.OnReceivePermission(a_oPermission, true);
		} else {
			this.RequestPermission(a_oPermission, this.OnReceivePermission);
		}
	}
#endif			// #if UNITY_ANDROID
	#endregion			// 조건부 함수

	#region 조건부 클래스 함수
#if UNITY_IOS && APPLE_LOGIN_ENABLE
	/** 애플 로그인 상태가 갱신 되었을 경우 */
	private static void OnUpdateAppleLoginState(CServicesManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnUpdateAppleLoginState: {a_bIsSuccess}");
	}
#endif			// #if UNITY_IOS && APPLE_LOGIN_ENABLE

#if ADS_MODULE_ENABLE
	/** 광고 관리자가 초기화 되었을 경우 */
	private static void OnInitAdsManager(CAdsManager a_oSender, EAdsPlatform a_eAdsPlatform, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitAdsManager: {a_eAdsPlatform}, {a_bIsSuccess}");
		
		// 초기화 되었을 경우
		if(a_bIsSuccess) {
			bool bIsEnableLoadBannerAds = CPluginInfoTable.Inst.GetBannerAdsID(a_eAdsPlatform).ExIsValid() && !CCommonUserInfoStorage.Inst.UserInfo.IsRemoveAds;
			bIsEnableLoadBannerAds = bIsEnableLoadBannerAds && CPluginInfoTable.Inst.AdsPlatform == a_eAdsPlatform;

			bool bIsEnableLoadRewardAds = CPluginInfoTable.Inst.GetRewardAdsID(a_eAdsPlatform).ExIsValid();
			bool bIsEnableLoadFullscreenAds = CPluginInfoTable.Inst.GetFullscreenAdsID(a_eAdsPlatform).ExIsValid() && !CCommonUserInfoStorage.Inst.UserInfo.IsRemoveAds;

			// 배너 광고 로드가 가능 할 경우
			if(bIsEnableLoadBannerAds && CLateSetupSceneManager.IsAutoLoadBannerAds) {
				CAdsManager.Inst.LoadBannerAds(a_eAdsPlatform);
			}

			// 보상 광고 로드가 가능 할 경우
			if(bIsEnableLoadRewardAds && CLateSetupSceneManager.IsAutoLoadRewardAds) {
				CAdsManager.Inst.LoadRewardAds(a_eAdsPlatform);
			}

			// 전면 광고 로드가 가능 할 경우
			if(bIsEnableLoadFullscreenAds && CLateSetupSceneManager.IsAutoLoadFullscreenAds) {
				CAdsManager.Inst.LoadFullscreenAds(a_eAdsPlatform);
			}
		}
	}
#endif			// #if ADS_MODULE_ENABLE

#if FLURRY_MODULE_ENABLE
	/** 플러리 관리자가 초기화 되었을 경우 */
	private static void OnInitFlurryManager(CFlurryManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitFlurryManager: {a_bIsSuccess}");

		// 초기화 되었을 경우
		if(a_bIsSuccess) {
			CFlurryManager.Inst.SetAnalyticsUserID(CCommonAppInfoStorage.Inst.AppInfo.DeviceID);
			CFlurryManager.Inst.SendLog(KCDefine.L_LOG_N_APP_LAUNCH, null);
		}
	}
#endif			// #if FLURRY_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
	/** 페이스 북 관리자가 초기화 되었을 경우 */
	private static void OnInitFacebookManager(CFacebookManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitFacebookManager: {a_bIsSuccess}");
	}
#endif			// #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
	/** 파이어 베이스 관리자가 초기화 되었을 경우 */
	private static void OnInitFirebaseManager(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitFirebaseManager: {a_bIsSuccess}");

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
	/** 속성이 로드 되었을 경우 */
	private static void OnLoadConfig(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnLoadConfig: {a_bIsSuccess}");

		// 속성이 로드 되었을 경우
		if(a_bIsSuccess) {
			string oDeviceConfig = CFirebaseManager.Inst.GetConfig(KCDefine.U_KEY_FIREBASE_M_DEVICE_CONFIG);
			CCommonAppInfoStorage.Inst.DeviceConfig = oDeviceConfig.ExJSONStrToObj<STDeviceConfig>();
		}
	}
#endif			// #if FIREBASE_REMOTE_CONFIG_ENABLE
#endif			// #if FIREBASE_MODULE_ENABLE

#if APPS_FLYER_MODULE_ENABLE
	/** 앱스 플라이어 관리자가 초기화 되었을 경우 */
	private static void OnInitAppsFlyerManager(CAppsFlyerManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitAppsFlyerManager: {a_bIsSuccess}");

		// 초기화 되었을 경우
		if(a_bIsSuccess) {
			CAppsFlyerManager.Inst.SetAnalyticsUserID(CCommonAppInfoStorage.Inst.AppInfo.DeviceID);
			CAppsFlyerManager.Inst.SendLog(KCDefine.L_LOG_N_APP_LAUNCH, null);
		}
	}
#endif			// #if APPS_FLYER_MODULE_ENABLE

#if GAME_CENTER_MODULE_ENABLE
	/** 게임 센터 관리자가 초기화 되었을 경우 */
	private static void OnInitGameCenterManager(CGameCenterManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitGameCenterManager: {a_bIsSuccess}");
	}
#endif			// #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	/** 결제 관리자가 초기화 되었을 경우 */
	private static void OnInitPurchaseManager(CPurchaseManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitPurchaseManager: {a_bIsSuccess}");
	}
#endif			// #if PURCHASE_MODULE_ENABLE

#if NOTI_MODULE_ENABLE
	/** 알림 관리자가 초기화 되었을 경우 */
	private static void OnInitNotiManager(CNotiManager a_oSender, bool a_bIsSuccess) {
		CFunc.ShowLog($"CLateSetupSceneManager.OnInitNotiManager: {a_bIsSuccess}");
	}
#endif			// #if NOTI_MODULE_ENABLE
	#endregion			// 조건부 클래스 함수
}
