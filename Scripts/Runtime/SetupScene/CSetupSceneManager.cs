using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 설정 씬 관리자
public abstract partial class CSetupSceneManager : CSceneManager {
	#region 변수
#if LOCALIZE_TEST_ENABLE
	[SerializeField] protected SystemLanguage m_eLanguage = SystemLanguage.Unknown;
#endif			// #if LOCALIZE_TEST_ENABLE
	#endregion			// 변수

	#region 클래스 객체
	private static GameObject m_oDebugUI = null;
	private static GameObject m_oPopupUI = null;
	private static GameObject m_oTopmostUI = null;
	private static GameObject m_oAbsoluteUI = null;
	private static GameObject m_oTimerManager = null;

#if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	private static GameObject m_oFPSCounter = null;
#endif			// #if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	#endregion			// 클래스 객체

	#region 프로퍼티
	public bool IsAutoLoadTable { get; protected set; } = false;
	public bool IsAutoInitManager { get; protected set; } = false;

	public override string SceneName => KDefine.B_SCENE_NAME_SETUP;

#if UNITY_EDITOR
	public override int ScriptOrder => KDefine.U_SCRIPT_ORDER_SETUP_SCENE_MANAGER;
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
		if(a_oCmd.ExIsEquals(KDefine.B_CMD_GET_DEVICE_ID)) {
			this.HandleGetDeviceIDMsg(a_oMsg);
		} else if(a_oCmd.ExIsEquals(KDefine.B_CMD_GET_COUNTRY_CODE)) {
			this.HandleGetCountryCodeMsg(a_oMsg);
		}
	}

	//! 씬을 설정한다
	protected virtual void Setup() {
		this.SetupPopupUI();
		this.SetupTopmostUI();
		this.SetupAbsoluteUI();
		this.SetupTimerManager();

#if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
		this.SetupDebugUI();
#endif			// #if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)

#if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
		this.SetupFPSCounter();
#endif			// #if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	}

	//! 초기화
	private IEnumerator OnStart() {
		if(!CSceneManager.IsSetup) {
			yield return Func.CreateWaitForSeconds(KDefine.U_DELAY_INIT);

#if DEBUG || DEVELOPMENT_BUILD
			CUnityMsgSender.Instance.SendSetBuildModeMsg(true);
#else
			CUnityMsgSender.Instance.SendSetBuildModeMsg(false);
#endif			// #if DEBUG || DEVELOPMENT_BUILD

			yield return Func.CreateWaitForSeconds(KDefine.U_DELAY_INIT);
			
			// 저장소를 설정한다 {
#if MESSAGE_PACK_ENABLE
			// FIXME: 임시 주석 처리
			// CAppInfoStorage.Instance.SetupStoreVersion();
			CAppInfoStorage.Instance.LoadAppInfo(KDefine.B_DATA_PATH_APP_INFO);
#endif			// #if MESSAGE_PACK_ENABLE

			yield return Func.CreateWaitForSeconds(KDefine.U_DELAY_INIT);
			// 저장소를 설정한다 }

			// 테이블을 로드한다
			if(this.IsAutoLoadTable) {
				CValueTable.Instance.LoadValuesFromRes(KDefine.U_TABLE_PATH_G_COMMON_VALUE_TABLE);
				CStringTable.Instance.LoadStringsFromRes(KDefine.U_TABLE_PATH_G_COMMON_STRING_TABLE);

				yield return Func.CreateWaitForSeconds(KDefine.U_DELAY_INIT);
			}

			// 관리자를 초기화한다
			if(this.IsAutoInitManager) {
#if ADS_ENABLE
				var oDeviceIDList = new List<string>();

#if ADMOB_ENABLE
				oDeviceIDList.AddRange(CDeviceInfoTable.Instance.AdmobDeviceIDList);
#endif			// #if ADMOB_ENABLE

				CAdsManager.Instance.Init(new CAdsManager.STParameters() {
					m_eBannerAdsType = CPluginInfoTable.Instance.m_eBannerAdsType,
					m_oDeviceIDList = oDeviceIDList,

#if ADMOB_ENABLE
					m_stAdmobParameters = new CAdsManager.STAdmobParameters() {
						m_oTemplateIDList = new List<string>(CPluginInfoTable.Instance.AdmobPluginInfo.m_oTemplateIDList),

						m_oAdsIDList = new Dictionary<string, string>() {
							[KDefine.U_KEY_ADS_M_BANNER_ADS_ID] = CPluginInfoTable.Instance.AdmobPluginInfo.m_oBannerAdsID,
							[KDefine.U_KEY_ADS_M_REWARD_ADS_ID] = CPluginInfoTable.Instance.AdmobPluginInfo.m_oRewardAdsID,
							[KDefine.U_KEY_ADS_M_NATIVE_ADS_ID] = CPluginInfoTable.Instance.AdmobPluginInfo.m_oNativeAdsID,
							[KDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID] = CPluginInfoTable.Instance.AdmobPluginInfo.m_oFullscreenAdsID
						}
					},
#endif			// #if ADMOB_ENABLE

#if UNITY_ADS_ENABLE
					m_stUnityAdsParameters = new CAdsManager.STUnityAdsParameters() {
						m_oGameID = CPluginInfoTable.Instance.UnityAdsPluginInfo.m_oGameID,

						m_oAdsPlacementList = new Dictionary<string, string>() {
							[KDefine.U_KEY_ADS_M_BANNER_ADS_PLACEMENT] = CPluginInfoTable.Instance.UnityAdsPluginInfo.m_oBannerAdsPlacement,
							[KDefine.U_KEY_ADS_M_REWARD_ADS_PLACEMENT] = CPluginInfoTable.Instance.UnityAdsPluginInfo.m_oRewardAdsPlacement,
							[KDefine.U_KEY_ADS_M_FULLSCREEN_ADS_PLACEMENT] = CPluginInfoTable.Instance.UnityAdsPluginInfo.m_oFullscreenAdsPlacement
						}
					},
#endif			// #if UNITY_ADS_ENABLE

#if IRON_SOURCE_ENABLE
					m_stIronSourceParameters = new CAdsManager.STIronSourceParameters() {
						m_oAppKey = CPluginInfoTable.Instance.IronSourcePluginInfo.m_oAppKey,

						m_oAdsUnitList = new List<string>() {
							CPluginInfoTable.Instance.IronSourcePluginInfo.m_oBannerAdsPlacement.ExIsValid() ? IronSourceAdUnits.BANNER : string.Empty,
							CPluginInfoTable.Instance.IronSourcePluginInfo.m_oRewardAdsPlacement.ExIsValid() ? IronSourceAdUnits.REWARDED_VIDEO : string.Empty,
							CPluginInfoTable.Instance.IronSourcePluginInfo.m_oFullscreenAdsPlacement.ExIsValid() ? IronSourceAdUnits.INTERSTITIAL : string.Empty
						},

						m_oAdsPlacementList = new Dictionary<string, string>() {
							[KDefine.U_KEY_ADS_M_BANNER_ADS_PLACEMENT] = CPluginInfoTable.Instance.IronSourcePluginInfo.m_oBannerAdsPlacement,
							[KDefine.U_KEY_ADS_M_REWARD_ADS_PLACEMENT] = CPluginInfoTable.Instance.IronSourcePluginInfo.m_oRewardAdsPlacement,
							[KDefine.U_KEY_ADS_M_FULLSCREEN_ADS_PLACEMENT] = CPluginInfoTable.Instance.IronSourcePluginInfo.m_oFullscreenAdsPlacement
						}
					},
#endif			// #if IRON_SOURCE_ENABLE

#if APP_LOVIN_ENABLE
					m_stAppLovinParameters = new CAdsManager.STAppLovinParameters() {
						m_oSDKKey = CPluginInfoTable.Instance.AppLoginPluginInfo.m_oSDKKey,

						m_oAdsIDList = new List<string>() {
							[KDefine.U_KEY_ADS_M_BANNER_ADS_PLACEMENT] = CPluginInfoTable.Instance.AppLoginPluginInfo.m_oBannerAdsID,
							[KDefine.U_KEY_ADS_M_REWARD_ADS_PLACEMENT] = CPluginInfoTable.Instance.AppLoginPluginInfo.m_oRewardAdsID,
							[KDefine.U_KEY_ADS_M_FULLSCREEN_ADS_PLACEMENT] = CPluginInfoTable.Instance.AppLoginPluginInfo.m_oFullscreenAdsID
						}
					}
#endif			// #if APP_LOVIN_ENABLE
				}, null);

				yield return Func.CreateWaitForSeconds(KDefine.U_DELAY_INIT);
#endif			// #if ADS_ENABLE

#if FACEBOOK_ENABLE
				CFacebookManager.Instance.Init(null);
				yield return Func.CreateWaitForSeconds(KDefine.U_DELAY_INIT);
#endif			// #if FACEBOOK_ENABLE

#if FIREBASE_ENABLE
#if FIREBASE_REMOTE_CONFIG_ENABLE
				var oMacVersionInfo = new Dictionary<string, string>() {
					[KDefine.U_CONFIG_KEY_FIREBASE_BUILD_NUMBER] = CProjectInfoTable.Instance.MacProjectInfo.m_oBuildNumber,
					[KDefine.U_CONFIG_KEY_FIREBASE_BUILD_VERSION] = CProjectInfoTable.Instance.MacProjectInfo.m_oBuildVersion
				};

				var oWindowsVersionInfo = new Dictionary<string, string>() {
					[KDefine.U_CONFIG_KEY_FIREBASE_BUILD_NUMBER] = CProjectInfoTable.Instance.WindowsProjectInfo.m_oBuildNumber,
					[KDefine.U_CONFIG_KEY_FIREBASE_BUILD_VERSION] = CProjectInfoTable.Instance.WindowsProjectInfo.m_oBuildVersion
				};

				var oiOSVersionInfo = new Dictionary<string, string>() {
					[KDefine.U_CONFIG_KEY_FIREBASE_BUILD_NUMBER] = CProjectInfoTable.Instance.iOSProjectInfo.m_oBuildNumber,
					[KDefine.U_CONFIG_KEY_FIREBASE_BUILD_VERSION] = CProjectInfoTable.Instance.iOSProjectInfo.m_oBuildVersion
				};

				var oGoogleVersionInfo = new Dictionary<string, string>() {
					[KDefine.U_CONFIG_KEY_FIREBASE_BUILD_NUMBER] = CProjectInfoTable.Instance.GoogleProjectInfo.m_oBuildNumber,
					[KDefine.U_CONFIG_KEY_FIREBASE_BUILD_VERSION] = CProjectInfoTable.Instance.GoogleProjectInfo.m_oBuildVersion
				};

				var oOneStoreVersionInfo = new Dictionary<string, string>() {
					[KDefine.U_CONFIG_KEY_FIREBASE_BUILD_NUMBER] = CProjectInfoTable.Instance.OneStoreProjectInfo.m_oBuildNumber,
					[KDefine.U_CONFIG_KEY_FIREBASE_BUILD_VERSION] = CProjectInfoTable.Instance.OneStoreProjectInfo.m_oBuildVersion
				};

				var oGalaxyStoreVersionInfo = new Dictionary<string, string>() {
					[KDefine.U_CONFIG_KEY_FIREBASE_BUILD_NUMBER] = CProjectInfoTable.Instance.GalaxyStoreProjectInfo.m_oBuildNumber,
					[KDefine.U_CONFIG_KEY_FIREBASE_BUILD_VERSION] = CProjectInfoTable.Instance.GalaxyStoreProjectInfo.m_oBuildVersion
				};

				var oConfigDataList = new Dictionary<string, object>() {
					[KDefine.U_CONFIG_KEY_FIREBASE_MAC_VERSION_INFO] = oMacVersionInfo.ExToJSONString(),
					[KDefine.U_CONFIG_KEY_FIREBASE_WINDOWS_VERSION_INFO] = oWindowsVersionInfo.ExToJSONString(),

					[KDefine.U_CONFIG_KEY_FIREBASE_IOS_VERSION_INFO] = oiOSVersionInfo.ExToJSONString(),

					[KDefine.U_CONFIG_KEY_FIREBASE_GOOGLE_VERSION_INFO] = oGoogleVersionInfo.ExToJSONString(),
					[KDefine.U_CONFIG_KEY_FIREBASE_ONE_STORE_VERSION_INFO] = oOneStoreVersionInfo.ExToJSONString(),
					[KDefine.U_CONFIG_KEY_FIREBASE_GALAXY_STORE_VERSION_INFO] = oGalaxyStoreVersionInfo.ExToJSONString()
				};
#else
				var oConfigDataList = new Dictionary<string, object>();
#endif			// #if FIREBASE_REMOTE_CONFIG_ENABLE

				CFirebaseManager.Instance.Init(oConfigDataList, null);
				yield return Func.CreateWaitForSeconds(KDefine.U_DELAY_INIT);
#endif			// #if FIREBASE_ENABLE

#if GAME_CENTER_ENABLE
				CGameCenterManager.Instance.Init(null);
				yield return Func.CreateWaitForSeconds(KDefine.U_DELAY_INIT);
#endif			// #if GAME_CENTER_ENABLE

#if PURCHASE_ENABLE && MESSAGE_PACK_ENABLE
				CPurchaseManager.Instance.Init(CProductInfoTable.Instance.ProductInfoList, null);
				yield return Func.CreateWaitForSeconds(KDefine.U_DELAY_INIT);
#endif			// #if PURCHASE_ENABLE && MESSAGE_PACK_ENABLE

#if UNITY_SERVICE_ENABLE
				CUnityServiceManager.Instance.Init(null);
				yield return Func.CreateWaitForSeconds(KDefine.U_DELAY_INIT);
#endif			// #if UNITY_SERVICE_ENABLE

#if SINGULAR_ENABLE
				CSingularManager.Instance.Init(null);
				yield return Func.CreateWaitForSeconds(KDefine.U_DELAY_INIT);
#endif			// #if SINGULAR_ENABLE
			}

			this.Setup();
			yield return Func.CreateWaitForSeconds(KDefine.U_DELAY_INIT);

			// 디바이스 식별자 반환 메세지를 전송한다
			CUnityMsgSender.Instance.SendGetDeviceIDMsg(this.OnReceiveDeviceMsg);
		}
	}
	
	//! 디바이스 식별자 반환 메세지를 처리한다
	private void HandleGetDeviceIDMsg(string a_oMsg) {
#if MESSAGE_PACK_ENABLE
		bool bIsValid = CAppInfoStorage.Instance.AppInfo.DeviceID.ExIsValid();

		if(!bIsValid || CAppInfoStorage.Instance.AppInfo.DeviceID.ExIsEquals(KDefine.B_UNKNOWN_DEVICE_ID)) {
			CAppInfoStorage.Instance.AppInfo.DeviceID = a_oMsg.ExIsValid() ? a_oMsg : KDefine.B_UNKNOWN_DEVICE_ID;
		}

		CAppInfoStorage.Instance.SaveAppInfo(KDefine.B_DATA_PATH_APP_INFO);
#endif			// #if MESSAGE_PACK_ENABLE

		// 국가 코드 반환 메세지를 전송한다
		CUnityMsgSender.Instance.SendGetCountryCodeMsg(this.OnReceiveDeviceMsg);
	}

	//! 국가 코드 반환 메세지를 처리한다
	private void HandleGetCountryCodeMsg(string a_oMsg) {
		string oCountryCode = a_oMsg;

		// 국가 코드가 유효하지 않을 경우
		if(!Func.IsMobilePlatform() || !a_oMsg.ExIsValid()) {
			oCountryCode = !Func.IsMobilePlatform() ? KDefine.B_KOREA_COUNTRY_CODE : KDefine.B_UNKNOWN_COUNTRY_CODE;
		}

#if MESSAGE_PACK_ENABLE
		CAppInfoStorage.Instance.CountryCode = oCountryCode.ToUpper();
		CAppInfoStorage.Instance.SaveAppInfo(KDefine.B_DATA_PATH_APP_INFO);

#if FLURRY_ENABLE && FLURRY_ANALYTICS_ENABLE
		CFlurryManager.Instance.SetUserID(CAppInfoStorage.Instance.AppInfo.DeviceID);
#endif			// #if FLURRY_ENABLE && FLURRY_ANALYTICS_ENABLE

#if FIREBASE_ENABLE
#if FIREBASE_ANALYTICS_ENABLE
		CFirebaseManager.Instance.SetAnalyticsUserID(CAppInfoStorage.Instance.AppInfo.DeviceID);

		CFirebaseManager.Instance.SetAnalyticsDatas(new Dictionary<string, string>() {
			[KDefine.U_LOG_KEY_COUNTRY_CODE] = CAppInfoStorage.Instance.CountryCode
		});
#endif			// #if FIREBASE_ANALYTICS_ENABLE

#if FIREBASE_CRASHLYTICS_ENABLE
		CFirebaseManager.Instance.SetCrashUserID(CAppInfoStorage.Instance.AppInfo.DeviceID);

		CFirebaseManager.Instance.SetCrashDatas(new Dictionary<string, string>() {
			[KDefine.U_LOG_KEY_COUNTRY_CODE] = CAppInfoStorage.Instance.CountryCode
		});
#endif			// #if FIREBASE_CRASHLYTICS_ENABLE
#endif			// #if FIREBASE_ENABLE

#if UNITY_SERVICE_ENABLE
#if UNITY_SERVICE_ANALYTICS_ENABLE
		CUnityServiceManager.Instance.SetAnalyticsUserID(CAppInfoStorage.Instance.AppInfo.DeviceID);
#endif			// #if UNITY_SERVICE_ANALYTICS_ENABLE

#if UNITY_SERVICE_CRASHLYTICS_ENABLE
		CUnityServiceManager.Instance.SetCrashDatas(new Dictionary<string, string>() {
			[KDefine.U_LOG_KEY_USER_ID] = CAppInfoStorage.Instance.AppInfo.DeviceID,
			[KDefine.U_LOG_KEY_COUNTRY_CODE] = CAppInfoStorage.Instance.CountryCode
		});
#endif			// #if UNITY_SERVICE_CRASHLYTICS_ENABLE
#endif			// #if UNITY_SERVICE_ENABLE

#if SINGULAR_ENABLE && SINGULAR_ANALYTICS_ENABLE
		CSingularManager.Instance.SetAnalyticsUserID(CAppInfoStorage.Instance.AppInfo.DeviceID);
#endif			// #if SINGULAR_ENABLE && SINGULAR_ANALYTICS_ENABLE

		if(this.IsAutoLoadTable) {
			string oLanguage = CAppInfoStorage.Instance.AppInfo.Language;

			// 언어가 유효하지 않을 경우
			if(!oLanguage.ExIsValid() || oLanguage.ExIsEquals(KDefine.B_UNKNOWN_COUNTRY_CODE)) {
#if LOCALIZE_TEST_ENABLE
				var eSystemLanguage = m_eLanguage;
#else
				var eSystemLanguage = Application.systemLanguage;
#endif			// #if LOCALIZE_TEST_ENABLE

				oLanguage = eSystemLanguage.ExIsValidLanguage() ? eSystemLanguage.ToString() 
					: oCountryCode;
				
				string oFilepath = KDefine.U_TABLE_PATH_G_COMMON_STRING_TABLE.ExPathToLocalizePath(oLanguage);
				oLanguage = Func.IsExistsRes<TextAsset>(oFilepath) ? oLanguage : SystemLanguage.English.ToString();
			}

			string oLocalizeFilepath = KDefine.U_TABLE_PATH_G_COMMON_STRING_TABLE.ExPathToLocalizePath(oLanguage);

			CAppInfoStorage.Instance.AppInfo.Language = oLanguage;
			CAppInfoStorage.Instance.SaveAppInfo(KDefine.B_DATA_PATH_APP_INFO);

			Func.BroadcastMsg(KDefine.U_FUNC_NAME_RESET_LOCALIZE, null);
			CStringTable.Instance.LoadStringsFromRes(oLocalizeFilepath);

			// FIXME: 임시 주석 처리 (불필요시 제거)
			// if(CAppInfoStorage.Instance.CountryCode.ExIsEquals(KDefine.B_KOREA_COUNTRY_CODE)) {
			// 	CStringTable.Instance.LoadStringsFromRes(KDefine.U_TABLE_PATH_G_KOREAN_COMMON_STRING_TABLE);
			// } else {
			// 	CStringTable.Instance.LoadStringsFromRes(KDefine.U_TABLE_PATH_G_ENGLISH_COMMON_STRING_TABLE);
			// }	
		}
#endif			// #if MESSAGE_PACK_ENABLE

		if(this.IsAutoInitManager) {
#if FLURRY_ENABLE
			CFlurryManager.Instance.Init(CPluginInfoTable.Instance.FlurryPluginInfo.m_oAPIKey, null);
#endif			// #if FLURRY_ENABLE

#if TENJIN_ENABLE
			CTenjinManager.Instance.Init(CPluginInfoTable.Instance.TenjinPluginInfo.m_oAPIKey, null);
#endif			// #if TENJIN_ENABLE
		}

		CSceneManager.IsSetup = true;
		LogFunc.SendAppLaunchLog();

		Func.LateCallFunc(this, KDefine.U_DELAY_INIT, (a_oComponent, a_oParams) => {
			Func.LoadAdditiveScene(KDefine.B_SCENE_NAME_AGREE, false);
		});
	}
	#endregion			// 함수
}
