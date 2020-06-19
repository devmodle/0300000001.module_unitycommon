using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//! 설정 씬 관리자
public abstract class CSetupSceneManager : CSceneManager {
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
	public void OnReceiveDeviceMessage(string a_oCommand, string a_oMessage) {
		if(a_oCommand.ExIsEquals(KDefine.B_COMMAND_GET_DEVICE_ID)) {
			this.HandleGetDeviceIDMessage(a_oMessage);
		} else if(a_oCommand.ExIsEquals(KDefine.B_COMMAND_GET_COUNTRY_CODE)) {
			this.HandleGetCountryCodeMessage(a_oMessage);
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
			yield return Function.CreateWaitForSeconds(KDefine.U_DELAY_INIT);

#if DEBUG || DEVELOPMENT_BUILD
			CUnityMessageSender.Instance.SendSetBuildModeMessage(true);
#else
			CUnityMessageSender.Instance.SendSetBuildModeMessage(false);
#endif			// #if DEBUG || DEVELOPMENT_BUILD

			yield return Function.CreateWaitForSeconds(KDefine.U_DELAY_INIT);
			
			// 저장소를 설정한다 {
#if MESSAGE_PACK_ENABLE
			CAppInfoStorage.Instance.SetupStoreVersion();
			CAppInfoStorage.Instance.LoadAppInfo(KDefine.B_DATA_PATH_APP_INFO);
#endif			// #if MESSAGE_PACK_ENABLE

			yield return Function.CreateWaitForSeconds(KDefine.U_DELAY_INIT);
			// 저장소를 설정한다 }

			// 테이블을 로드한다
			if(this.IsAutoLoadTable) {
				CValueTable.Instance.LoadValuesFromResource(KDefine.U_TABLE_PATH_G_COMMON_VALUE_TABLE);
				CStringTable.Instance.LoadStringsFromResource(KDefine.U_TABLE_PATH_G_COMMON_STRING_TABLE);

				yield return Function.CreateWaitForSeconds(KDefine.U_DELAY_INIT);
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
					}
#endif			// #if IRON_SOURCE_ENABLE
				}, null);

				yield return Function.CreateWaitForSeconds(KDefine.U_DELAY_INIT);
#endif			// #if ADS_ENABLE

#if FACEBOOK_ENABLE
				CFacebookManager.Instance.Init(null);
				yield return Function.CreateWaitForSeconds(KDefine.U_DELAY_INIT);
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
				yield return Function.CreateWaitForSeconds(KDefine.U_DELAY_INIT);
#endif			// #if FIREBASE_ENABLE

#if GAME_CENTER_ENABLE
				CGameCenterManager.Instance.Init(null);
				yield return Function.CreateWaitForSeconds(KDefine.U_DELAY_INIT);
#endif			// #if GAME_CENTER_ENABLE

#if PURCHASE_ENABLE && MESSAGE_PACK_ENABLE
				CPurchaseManager.Instance.Init(CProductInfoTable.Instance.ProductInfoList, null);
				yield return Function.CreateWaitForSeconds(KDefine.U_DELAY_INIT);
#endif			// #if PURCHASE_ENABLE && MESSAGE_PACK_ENABLE

#if UNITY_SERVICE_ENABLE
				CUnityServiceManager.Instance.Init(null);
				yield return Function.CreateWaitForSeconds(KDefine.U_DELAY_INIT);
#endif			// #if UNITY_SERVICE_ENABLE
			}

			this.Setup();
			yield return Function.CreateWaitForSeconds(KDefine.U_DELAY_INIT);

			// 디바이스 식별자 반환 메세지를 전송한다
			CUnityMessageSender.Instance.SendGetDeviceIDMessage(this.OnReceiveDeviceMessage);
		}
	}

	//! 팝업 UI 를 설정한다
	private void SetupPopupUI() {
		if(CSetupSceneManager.m_oPopupUI == null) {
			var oPopupUI = Function.CreateCloneGameObject(KDefine.SS_NAME_POPUP_UI,
				CResourceManager.Instance.GetGameObject(KDefine.IS_PATH_SCREEN_POPUP_UI), null);

			CSetupSceneManager.m_oPopupUI = oPopupUI;
			CSceneManager.ScreenPopupUIRoot = oPopupUI.ExFindChild(KDefine.U_OBJ_NAME_SCREEN_POPUP_UI_ROOT);

			DontDestroyOnLoad(oPopupUI);
			Function.SetupScreenUI(oPopupUI, KDefine.U_SORTING_ORDER_SCREEN_POPUP_UI);
		}
	}

	//! 최상위 UI 를 설정한다
	private void SetupTopmostUI() {
		if(CSetupSceneManager.m_oTopmostUI == null) {
			var oTopmostUI = Function.CreateCloneGameObject(KDefine.SS_NAME_TOPMOST_UI,
				CResourceManager.Instance.GetGameObject(KDefine.IS_PATH_SCREEN_TOPMOST_UI), null);

			CSetupSceneManager.m_oTopmostUI = oTopmostUI;
			CSceneManager.ScreenTopmostUIRoot = oTopmostUI.ExFindChild(KDefine.U_OBJ_NAME_SCREEN_TOPMOST_UI_ROOT);

			DontDestroyOnLoad(oTopmostUI);
			Function.SetupScreenUI(oTopmostUI, KDefine.U_SORTING_ORDER_SCREEN_TOPMOST_UI);
		}
	}

	//! 절대 UI 를 설정한다
	private void SetupAbsoluteUI() {
		if(CSetupSceneManager.m_oAbsoluteUI == null) {
			var oAbsoluteUI = Function.CreateCloneGameObject(KDefine.SS_NAME_ABSOLUTE_UI,
				CResourceManager.Instance.GetGameObject(KDefine.IS_PATH_SCREEN_ABSOLUTE_UI), null);

			CSetupSceneManager.m_oAbsoluteUI = oAbsoluteUI;
			CSceneManager.ScreenAbsoluteUIRoot = oAbsoluteUI.ExFindChild(KDefine.U_OBJ_NAME_SCREEN_ABSOLUTE_UI_ROOT);

			DontDestroyOnLoad(oAbsoluteUI);
			Function.SetupScreenUI(oAbsoluteUI, KDefine.U_SORTING_ORDER_SCREEN_ABSOLUTE_UI);
		}
	}

	//! 타이머 관리자를 설정한다
	private void SetupTimerManager() {
		if(CSetupSceneManager.m_oTimerManager == null) {
			var oTimerManager = Function.CreateCloneGameObject(KDefine.SS_NAME_TIMER_MANAGER,
				CResourceManager.Instance.GetGameObject(KDefine.U_OBJ_PATH_SS_TIMER_MANAGER), null);

			CSetupSceneManager.m_oTimerManager = oTimerManager;
		}
	}

	//! 디바이스 식별자 반환 메세지를 처리한다
	private void HandleGetDeviceIDMessage(string a_oMessage) {
#if MESSAGE_PACK_ENABLE
		bool bIsValid = CAppInfoStorage.Instance.AppInfo.DeviceID.ExIsValid();

		if(!bIsValid || CAppInfoStorage.Instance.AppInfo.DeviceID.ExIsEquals(KDefine.B_UNKNOWN_DEVICE_ID)) {
			CAppInfoStorage.Instance.AppInfo.DeviceID = a_oMessage.ExIsValid() ? a_oMessage : KDefine.B_UNKNOWN_DEVICE_ID;
		}

		CAppInfoStorage.Instance.SaveAppInfo(KDefine.B_DATA_PATH_APP_INFO);
#endif			// #if MESSAGE_PACK_ENABLE

		// 국가 코드 반환 메세지를 전송한다
		CUnityMessageSender.Instance.SendGetCountryCodeMessage(this.OnReceiveDeviceMessage);
	}

	//! 국가 코드 반환 메세지를 처리한다
	private void HandleGetCountryCodeMessage(string a_oMessage) {
		string oCountryCode = a_oMessage;

		// 국가 코드가 유효하지 않을 경우
		if(!Function.IsMobilePlatform() || !a_oMessage.ExIsValid()) {
			oCountryCode = !Function.IsMobilePlatform() ? KDefine.B_KOREA_COUNTRY_CODE : KDefine.B_UNKNOWN_COUNTRY_CODE;
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

		if(this.IsAutoLoadTable) {
			if(CAppInfoStorage.Instance.CountryCode.ExIsEquals(KDefine.B_KOREA_COUNTRY_CODE)) {
				CStringTable.Instance.LoadStringsFromResource(KDefine.U_TABLE_PATH_G_KOREAN_COMMON_STRING_TABLE);
			} else {
				CStringTable.Instance.LoadStringsFromResource(KDefine.U_TABLE_PATH_G_ENGLISH_COMMON_STRING_TABLE);
			}	
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

		LogFunction.SendAppLaunchLog();
		Function.LoadAdditiveScene(KDefine.B_SCENE_NAME_AGREE, false);
	}
	#endregion			// 함수

	#region 조건부 함수
#if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	//! 디버그 UI 를 설정한다
	private void SetupDebugUI() {
		if(CSetupSceneManager.m_oDebugUI == null) {
			var oDebugUI = Function.CreateCloneGameObject(KDefine.SS_NAME_DEBUG_UI,
				CResourceManager.Instance.GetGameObject(KDefine.IS_PATH_SCREEN_DEBUG_UI), null);

			CSetupSceneManager.m_oDebugUI = oDebugUI;

			CSceneManager.ScreenDebugUIRoot = oDebugUI.ExFindChild(KDefine.U_NAME_SCREEN_DEBUG_UI_ROOT);
			CSceneManager.ScreenDebugTextRoot = oDebugUI.ExFindChild(KDefine.U_NAME_SCREEN_DEBUG_TEXT_ROOT);

			CSceneManager.ScreenFPSButton = oDebugUI.ExFindComponent<Button>(KDefine.U_NAME_SCREEN_FPS_BUTTON);
			CSceneManager.ScreenDebugButton = oDebugUI.ExFindComponent<Button>(KDefine.U_NAME_SCREEN_DEBUG_BUTTON);

			CSceneManager.ScreenStaticDebugText = oDebugUI.ExFindComponent<Text>(KDefine.U_NAME_SCREEN_STATIC_DEBUG_TEXT);
			CSceneManager.ScreenStaticDebugText.raycastTarget = false;

			CSceneManager.ScreenDynamicDebugText = oDebugUI.ExFindComponent<Text>(KDefine.U_NAME_SCREEN_DYNAMIC_DEBUG_TEXT);
			CSceneManager.ScreenDynamicDebugText.raycastTarget = false;

			DontDestroyOnLoad(oDebugUI);

			CSceneManager.ScreenDebugTextRoot.SetActive(false);
			Function.SetupScreenUI(oDebugUI, KDefine.U_SORTING_ORDER_SCREEN_DEBUG_UI);
		}
	}
#endif			// #if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)

#if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	//! FPS 카운터를 설정한다
	private void SetupFPSCounter() {
		if(CSetupSceneManager.m_oFPSCounter == null) {
			var oFPSCounter = Function.CreateCloneGameObject(KDefine.SS_NAME_FPS_COUNTER,
				CResourceManager.Instance.GetGameObject(KDefine.U_OBJ_PATH_SS_FPS_COUNTER), null);

			CSetupSceneManager.m_oFPSCounter = oFPSCounter;

			CSetupSceneManager.ScreenStaticFPSText = oFPSCounter.ExFindComponent<Text>(KDefine.U_NAME_SCREEN_STATIC_FPS_TEXT);
			CSetupSceneManager.ScreenStaticFPSText.enabled = false;
			CSetupSceneManager.ScreenStaticFPSText.raycastTarget = false;

			CSetupSceneManager.ScreenDynamicFPSText = oFPSCounter.ExFindComponent<Text>(KDefine.U_NAME_SCREEN_DYNAMIC_FPS_TEXT);
			CSetupSceneManager.ScreenDynamicFPSText.enabled = false;
			CSetupSceneManager.ScreenDynamicFPSText.raycastTarget = false;

			DontDestroyOnLoad(oFPSCounter);
			Function.SetupScreenUI(oFPSCounter, KDefine.U_SORTING_ORDER_FPS_COUNTER);
		}
	}
#endif			// #if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	#endregion			// 조건부 함수
}
