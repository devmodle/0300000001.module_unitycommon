using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 설정 씬 관리자
public abstract partial class CSetupSceneManager : CSceneManager {
	#region 클래스 객체
	private static GameObject m_oDebugUI = null;
	private static GameObject m_oPopupUI = null;
	private static GameObject m_oTopmostUI = null;
	private static GameObject m_oAbsUI = null;
	private static GameObject m_oTimerManager = null;

#if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	private static GameObject m_oFPSCounter = null;
#endif			// #if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	#endregion			// 클래스 객체

	#region 프로퍼티
	public bool IsAutoLoadTable { get; protected set; } = false;
	public bool IsAutoInitManager { get; protected set; } = false;

	public override string SceneName => KCDefine.B_SCENE_NAME_SETUP;

#if UNITY_EDITOR
	public override int ScriptOrder => KCDefine.U_SCRIPT_ORDER_SETUP_SCENE_MANAGER;
#endif			// #if UNITY_EDITOR
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public sealed override void Start() {
		base.Start();
		StartCoroutine(this.OnStart());
	}

	//! 디바이스 메세지를 수신했을 경우
	public void OnReceiveDeviceMsg(string a_oCmd, string a_oMsg) {
		if(a_oCmd.ExIsEquals(KCDefine.B_CMD_GET_DEVICE_ID)) {
			this.HandleGetDeviceIDMsg(a_oMsg);
		} else if(a_oCmd.ExIsEquals(KCDefine.B_CMD_GET_COUNTRY_CODE)) {
			this.HandleGetCountryCodeMsg(a_oMsg);
		}
	}

	//! 씬을 설정한다
	protected virtual void Setup() {
		this.SetupPopupUI();
		this.SetupTopmostUI();
		this.SetupAbsUI();
		this.SetupTimerManager();

#if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
		this.SetupDebugUI();
#endif			// #if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)

#if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
		this.SetupFPSCounter();
#endif			// #if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	}

	//! 디바이스 식별자 반환 메세지를 처리한다
	private void HandleGetDeviceIDMsg(string a_oMsg) {
#if MSG_PACK_ENABLE
		bool bIsValid = CCommonAppInfoStorage.Instance.AppInfo.DeviceID.ExIsValid();

		if(!bIsValid || CCommonAppInfoStorage.Instance.AppInfo.DeviceID.ExIsEquals(KCDefine.B_UNKNOWN_DEVICE_ID)) {
			CCommonAppInfoStorage.Instance.AppInfo.DeviceID = a_oMsg.ExIsValid() ? a_oMsg : KCDefine.B_UNKNOWN_DEVICE_ID;
		}

		CCommonAppInfoStorage.Instance.SaveAppInfo();
#endif			// #if MSG_PACK_ENABLE

		// 국가 코드 반환 메세지를 전송한다
		CUnityMsgSender.Instance.SendGetCountryCodeMsg(this.OnReceiveDeviceMsg);
	}

	//! 국가 코드 반환 메세지를 처리한다
	private void HandleGetCountryCodeMsg(string a_oMsg) {
		string oCountryCode = a_oMsg;

		// 국가 코드가 유효하지 않을 경우
		if(!CAccess.IsMobilePlatform() || !a_oMsg.ExIsValid()) {
			oCountryCode = !CAccess.IsMobilePlatform() ? KCDefine.B_KOREA_COUNTRY_CODE : KCDefine.B_UNKNOWN_COUNTRY_CODE;
		}

#if MSG_PACK_ENABLE
		CCommonAppInfoStorage.Instance.CountryCode = oCountryCode.ToUpper();
		CCommonAppInfoStorage.Instance.SaveAppInfo();

#if FLURRY_ENABLE && FLURRY_ANALYTICS_ENABLE
		CFlurryManager.Instance.SetAnalyticsUserID(CCommonAppInfoStorage.Instance.AppInfo.DeviceID);
#endif			// #if FLURRY_ENABLE && FLURRY_ANALYTICS_ENABLE

#if FIREBASE_ENABLE
#if FIREBASE_ANALYTICS_ENABLE
		CFirebaseManager.Instance.SetAnalyticsUserID(CCommonAppInfoStorage.Instance.AppInfo.DeviceID);

		CFirebaseManager.Instance.SetAnalyticsDatas(new Dictionary<string, string>() {
			[KCDefine.U_LOG_KEY_COUNTRY_CODE] = CCommonAppInfoStorage.Instance.CountryCode
		});
#endif			// #if FIREBASE_ANALYTICS_ENABLE

#if FIREBASE_CRASHLYTICS_ENABLE
		CFirebaseManager.Instance.SetCrashUserID(CCommonAppInfoStorage.Instance.AppInfo.DeviceID);

		CFirebaseManager.Instance.SetCrashDatas(new Dictionary<string, string>() {
			[KCDefine.U_LOG_KEY_COUNTRY_CODE] = CCommonAppInfoStorage.Instance.CountryCode
		});
#endif			// #if FIREBASE_CRASHLYTICS_ENABLE
#endif			// #if FIREBASE_ENABLE

#if UNITY_SERVICE_ENABLE
#if UNITY_SERVICE_ANALYTICS_ENABLE
		CUnityServiceManager.Instance.SetAnalyticsUserID(CCommonAppInfoStorage.Instance.AppInfo.DeviceID);
#endif			// #if UNITY_SERVICE_ANALYTICS_ENABLE

#if UNITY_SERVICE_CRASHLYTICS_ENABLE
		CUnityServiceManager.Instance.SetCrashDatas(new Dictionary<string, string>() {
			[KCDefine.U_LOG_KEY_USER_ID] = CCommonAppInfoStorage.Instance.AppInfo.DeviceID,
			[KCDefine.U_LOG_KEY_COUNTRY_CODE] = CCommonAppInfoStorage.Instance.CountryCode
		});
#endif			// #if UNITY_SERVICE_CRASHLYTICS_ENABLE
#endif			// #if UNITY_SERVICE_ENABLE

#if SINGULAR_ENABLE && SINGULAR_ANALYTICS_ENABLE
		CSingularManager.Instance.SetAnalyticsUserID(CCommonAppInfoStorage.Instance.AppInfo.DeviceID);
#endif			// #if SINGULAR_ENABLE && SINGULAR_ANALYTICS_ENABLE

		if(this.IsAutoLoadTable) {
			if(CCommonAppInfoStorage.Instance.CountryCode.ExIsEquals(KCDefine.B_KOREA_COUNTRY_CODE)) {
				CStringTable.Instance.LoadStringsFromRes(KCDefine.U_TABLE_PATH_G_KOREAN_COMMON_STRING_TABLE);
			} else {
				CStringTable.Instance.LoadStringsFromRes(KCDefine.U_TABLE_PATH_G_ENGLISH_COMMON_STRING_TABLE);
			}	
		}
#endif			// #if MSG_PACK_ENABLE

		if(this.IsAutoInitManager) {
#if FLURRY_ENABLE
			CFlurryManager.Instance.Init(CPluginInfoTable.Instance.FlurryPluginInfo.m_oAPIKey, null);
#endif			// #if FLURRY_ENABLE

#if TENJIN_ENABLE
			CTenjinManager.Instance.Init(CPluginInfoTable.Instance.TenjinPluginInfo.m_oAPIKey, null);
#endif			// #if TENJIN_ENABLE
		}

		CSceneManager.IsSetup = true;
		this.SendAppLaunchLog();

		CFunc.LateCallFunc(this, KCDefine.U_DELAY_INIT, (a_oComponent, a_oParams) => {
			CSceneLoader.Instance.LoadAdditiveScene(KCDefine.B_SCENE_NAME_AGREE, false);
		});
	}

	//! 초기화
	private IEnumerator OnStart() {
		if(!CSceneManager.IsSetup) {
			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);

#if DEBUG || DEVELOPMENT_BUILD
			CUnityMsgSender.Instance.SendSetBuildModeMsg(true);
#else
			CUnityMsgSender.Instance.SendSetBuildModeMsg(false);
#endif			// #if DEBUG || DEVELOPMENT_BUILD

			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
			
			// 저장소를 설정한다 {
#if MSG_PACK_ENABLE
			CCommonAppInfoStorage.Instance.SetupStoreVersion();
			CCommonAppInfoStorage.Instance.LoadAppInfo();
#endif			// #if MSG_PACK_ENABLE

			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
			// 저장소를 설정한다 }
			
			// 관리자를 초기화한다
			if(this.IsAutoInitManager) {
#if ADS_ENABLE
				var oDeviceIDList = new List<string>();

#if ADMOB_ENABLE
				oDeviceIDList.AddRange(CDeviceInfoTable.Instance.AdmobDeviceIDList);
#endif			// #if ADMOB_ENABLE

				CAdsManager.Instance.Init(new CAdsManager.STParams() {
					m_eBannerAdsType = CPluginInfoTable.Instance.m_eBannerAdsType,
					m_oDeviceIDList = oDeviceIDList,

#if ADMOB_ENABLE
					m_stAdmobParams = new CAdsManager.STAdmobParams() {
						m_oTemplateIDList = new List<string>(CPluginInfoTable.Instance.AdmobPluginInfo.m_oTemplateIDList),

						m_oAdsIDList = new Dictionary<string, string>() {
							[KCDefine.U_KEY_ADS_M_BANNER_ADS_ID] = CPluginInfoTable.Instance.AdmobPluginInfo.m_oBannerAdsID,
							[KCDefine.U_KEY_ADS_M_REWARD_ADS_ID] = CPluginInfoTable.Instance.AdmobPluginInfo.m_oRewardAdsID,
							[KCDefine.U_KEY_ADS_M_NATIVE_ADS_ID] = CPluginInfoTable.Instance.AdmobPluginInfo.m_oNativeAdsID,
							[KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID] = CPluginInfoTable.Instance.AdmobPluginInfo.m_oFullscreenAdsID
						}
					},
#endif			// #if ADMOB_ENABLE

#if UNITY_ADS_ENABLE
					m_stUnityAdsParams = new CAdsManager.STUnityAdsParams() {
						m_oGameID = CPluginInfoTable.Instance.UnityAdsPluginInfo.m_oGameID,

						m_oAdsPlacementList = new Dictionary<string, string>() {
							[KCDefine.U_KEY_ADS_M_BANNER_ADS_PLACEMENT] = CPluginInfoTable.Instance.UnityAdsPluginInfo.m_oBannerAdsPlacement,
							[KCDefine.U_KEY_ADS_M_REWARD_ADS_PLACEMENT] = CPluginInfoTable.Instance.UnityAdsPluginInfo.m_oRewardAdsPlacement,
							[KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_PLACEMENT] = CPluginInfoTable.Instance.UnityAdsPluginInfo.m_oFullscreenAdsPlacement
						}
					},
#endif			// #if UNITY_ADS_ENABLE

#if IRON_SOURCE_ENABLE
					m_stIronSourceParams = new CAdsManager.STIronSourceParams() {
						m_oAppKey = CPluginInfoTable.Instance.IronSourcePluginInfo.m_oAppKey,

						m_oAdsUnitList = new List<string>() {
							CPluginInfoTable.Instance.IronSourcePluginInfo.m_oBannerAdsPlacement.ExIsValid() ? IronSourceAdUnits.BANNER : string.Empty,
							CPluginInfoTable.Instance.IronSourcePluginInfo.m_oRewardAdsPlacement.ExIsValid() ? IronSourceAdUnits.REWARDED_VIDEO : string.Empty,
							CPluginInfoTable.Instance.IronSourcePluginInfo.m_oFullscreenAdsPlacement.ExIsValid() ? IronSourceAdUnits.INTERSTITIAL : string.Empty
						},

						m_oAdsPlacementList = new Dictionary<string, string>() {
							[KCDefine.U_KEY_ADS_M_BANNER_ADS_PLACEMENT] = CPluginInfoTable.Instance.IronSourcePluginInfo.m_oBannerAdsPlacement,
							[KCDefine.U_KEY_ADS_M_REWARD_ADS_PLACEMENT] = CPluginInfoTable.Instance.IronSourcePluginInfo.m_oRewardAdsPlacement,
							[KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_PLACEMENT] = CPluginInfoTable.Instance.IronSourcePluginInfo.m_oFullscreenAdsPlacement
						}
					},
#endif			// #if IRON_SOURCE_ENABLE

#if APP_LOVIN_ENABLE
					m_stAppLovinParams = new CAdsManager.STAppLovinParams() {
						m_oSDKKey = CPluginInfoTable.Instance.AppLoginPluginInfo.m_oSDKKey,

						m_oAdsIDList = new List<string>() {
							[KCDefine.U_KEY_ADS_M_BANNER_ADS_PLACEMENT] = CPluginInfoTable.Instance.AppLoginPluginInfo.m_oBannerAdsID,
							[KCDefine.U_KEY_ADS_M_REWARD_ADS_PLACEMENT] = CPluginInfoTable.Instance.AppLoginPluginInfo.m_oRewardAdsID,
							[KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_PLACEMENT] = CPluginInfoTable.Instance.AppLoginPluginInfo.m_oFullscreenAdsID
						}
					}
#endif			// #if APP_LOVIN_ENABLE
				}, null);

				yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
#endif			// #if ADS_ENABLE

#if FACEBOOK_ENABLE
				CFacebookManager.Instance.Init(null);
				yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
#endif			// #if FACEBOOK_ENABLE

#if FIREBASE_ENABLE
#if FIREBASE_REMOTE_CONFIG_ENABLE
				var oMacVersionInfo = new Dictionary<string, string>() {
					[KCDefine.U_CONFIG_KEY_FIREBASE_BUILD_NUMBER] = CProjInfoTable.Instance.MacProjInfo.m_oBuildNumber,
					[KCDefine.U_CONFIG_KEY_FIREBASE_BUILD_VERSION] = CProjInfoTable.Instance.MacProjInfo.m_oBuildVersion
				};

				var oWindowsVersionInfo = new Dictionary<string, string>() {
					[KCDefine.U_CONFIG_KEY_FIREBASE_BUILD_NUMBER] = CProjInfoTable.Instance.WindowsProjInfo.m_oBuildNumber,
					[KCDefine.U_CONFIG_KEY_FIREBASE_BUILD_VERSION] = CProjInfoTable.Instance.WindowsProjInfo.m_oBuildVersion
				};

				var oiOSVersionInfo = new Dictionary<string, string>() {
					[KCDefine.U_CONFIG_KEY_FIREBASE_BUILD_NUMBER] = CProjInfoTable.Instance.iOSProjInfo.m_oBuildNumber,
					[KCDefine.U_CONFIG_KEY_FIREBASE_BUILD_VERSION] = CProjInfoTable.Instance.iOSProjInfo.m_oBuildVersion
				};

				var oGoogleVersionInfo = new Dictionary<string, string>() {
					[KCDefine.U_CONFIG_KEY_FIREBASE_BUILD_NUMBER] = CProjInfoTable.Instance.GoogleProjInfo.m_oBuildNumber,
					[KCDefine.U_CONFIG_KEY_FIREBASE_BUILD_VERSION] = CProjInfoTable.Instance.GoogleProjInfo.m_oBuildVersion
				};

				var oOneStoreVersionInfo = new Dictionary<string, string>() {
					[KCDefine.U_CONFIG_KEY_FIREBASE_BUILD_NUMBER] = CProjInfoTable.Instance.OneStoreProjInfo.m_oBuildNumber,
					[KCDefine.U_CONFIG_KEY_FIREBASE_BUILD_VERSION] = CProjInfoTable.Instance.OneStoreProjInfo.m_oBuildVersion
				};

				var oGalaxyStoreVersionInfo = new Dictionary<string, string>() {
					[KCDefine.U_CONFIG_KEY_FIREBASE_BUILD_NUMBER] = CProjInfoTable.Instance.GalaxyStoreProjInfo.m_oBuildNumber,
					[KCDefine.U_CONFIG_KEY_FIREBASE_BUILD_VERSION] = CProjInfoTable.Instance.GalaxyStoreProjInfo.m_oBuildVersion
				};

				var oConfigDataList = new Dictionary<string, object>() {
					[KCDefine.U_CONFIG_KEY_FIREBASE_MAC_VERSION_INFO] = oMacVersionInfo.ExToJSONString(),
					[KCDefine.U_CONFIG_KEY_FIREBASE_WINDOWS_VERSION_INFO] = oWindowsVersionInfo.ExToJSONString(),

					[KCDefine.U_CONFIG_KEY_FIREBASE_IOS_VERSION_INFO] = oiOSVersionInfo.ExToJSONString(),

					[KCDefine.U_CONFIG_KEY_FIREBASE_GOOGLE_VERSION_INFO] = oGoogleVersionInfo.ExToJSONString(),
					[KCDefine.U_CONFIG_KEY_FIREBASE_ONE_STORE_VERSION_INFO] = oOneStoreVersionInfo.ExToJSONString(),
					[KCDefine.U_CONFIG_KEY_FIREBASE_GALAXY_STORE_VERSION_INFO] = oGalaxyStoreVersionInfo.ExToJSONString()
				};
#else
				var oConfigDataList = new Dictionary<string, object>();
#endif			// #if FIREBASE_REMOTE_CONFIG_ENABLE

				CFirebaseManager.Instance.Init(oConfigDataList, null);
				yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
#endif			// #if FIREBASE_ENABLE

#if UNITY_SERVICE_ENABLE
				CUnityServiceManager.Instance.Init(null);
				yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
#endif			// #if UNITY_SERVICE_ENABLE

#if SINGULAR_ENABLE
				CSingularManager.Instance.Init(null);
				yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_TIME);
#endif			// #if SINGULAR_ENABLE

#if GAME_CENTER_ENABLE
				CGameCenterManager.Instance.Init(null);
				yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
#endif			// #if GAME_CENTER_ENABLE

#if PURCHASE_ENABLE && MSG_PACK_ENABLE
				CPurchaseManager.Instance.Init(CProductInfoTable.Instance.ProductInfoList, null);
				yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
#endif			// #if PURCHASE_ENABLE && MSG_PACK_ENABLE
			}

			this.Setup();
			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);

			// 디바이스 식별자 반환 메세지를 전송한다
			CUnityMsgSender.Instance.SendGetDeviceIDMsg(this.OnReceiveDeviceMsg);
		}
	}

	//! 어플리케이션 시작 로그를 전송한다
	private void SendAppLaunchLog() {
#if FLURRY_ENABLE && FLURRY_ANALYTICS_ENABLE
		CFlurryManager.Instance.SendLog(KCDefine.U_LOG_NAME_APP_LAUNCH, null);
#endif			// #if FLURRY_ENABLE && FLURRY_ANALYTICS_ENABLE

#if TENJIN_ENABLE && TENJIN_ANALYTICS_ENABLE
		CTenjinManager.Instance.SendLog(KCDefine.U_LOG_NAME_APP_LAUNCH, null);
#endif			// #if FLURRY_ENABLE && FLURRY_ANALYTICS_ENABLE

#if FACEBOOK_ENABLE && FACEBOOK_ANALYTICS_ENABLE
		CFacebookManager.Instance.SendLog(KCDefine.U_LOG_NAME_APP_LAUNCH, null);
#endif			// #if FACEBOOK_ENABLE && FACEBOOK_ANALYTICS_ENABLE

#if FIREBASE_ENABLE && FIREBASE_ANALYTICS_ENABLE
		CFirebaseManager.Instance.SendLog(KCDefine.U_LOG_NAME_APP_LAUNCH, KCDefine.U_LOG_PARAM_USER_INFO, null);
#endif			// #if FIREBASE_ENABLE && FIREBASE_ANALYTICS_ENABLE

#if UNITY_SERVICE_ENABLE && UNITY_SERVICE_ANALYTICS_ENABLE
		CUnityServiceManager.Instance.SendLog(KCDefine.U_LOG_NAME_APP_LAUNCH, null);
#endif			// #if UNITY_SERVICE_ENABLE && UNITY_SERVICE_ANALYTICS_ENABLE

#if SINGULAR_ENABLE && SINGULAR_ANALYTICS_ENABLE
		CSingularManager.Instance.SendLog(KCDefine.U_LOG_NAME_APP_LAUNCH, null);
#endif			// #if SINGULAR_ENABLE && SINGULAR_ANALYTICS_ENABLE
	}
	#endregion			// 함수
}
