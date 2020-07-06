using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if PURCHASE_ENABLE
using UnityEngine.Purchasing;
#endif			// #if PURCHASE_ENABLE

//! 기본 스크립트 객체 생성자
public static partial class CScriptableObjectCreator {
	#region 클래스 함수
	//! 전처리기 심볼 테이블을 생성한다
	[MenuItem("Utility/Create/DefineSymbolTable")]
	public static void CreateDefineSymbolTable() {
		var oDefineSymbolTable = EditorFunc.CreateScriptableObj<CDefineSymbolTable>();

		oDefineSymbolTable.SetCommonDefineSymbolList(new List<string>() {
			KEditorDefine.DS_DEFINE_SYMBOL_SECURITY_ENABLE,
			KEditorDefine.DS_DEFINE_SYMBOL_DYNAMIC_BATCHING_ENABLE,
			KEditorDefine.DS_DEFINE_SYMBOL_UNIVERSAL_RENDER_PIPELINE_ENABLE
		});
	}

	//! 디바이스 정보 테이블을 생성한다
	[MenuItem("Utility/Create/DeviceInfoTable")]
	public static void CreateDeviceInfoTable() {
		var oDeviceInfoTable = EditorFunc.CreateScriptableObj<CDeviceInfoTable>();

#if ADS_ENABLE && ADMOB_ENABLE
		oDeviceInfoTable.SetiOSAdmobDeviceIDList(new List<string>() {
			"cda4cbaf3ee1d95d96c9316c45ca1163",
			"b18f2566214cc419bac71a7c3df368ad"
		});

		oDeviceInfoTable.SetAndroidAdmobDeviceIDList(new List<string>() {
			"20883ADA1C122EA63419F9AF1FAD52F0",
			"3E2176530B8BACFCD9C26AD844C0B3C3"
		});
#endif			// #if ADS_ENABLE && ADMOB_ENABLE
	}

	//! 빌드 정보 테이블을 생성한다
	[MenuItem("Utility/Create/BuildInfoTable")]
	public static void CreateBuildInfoTable() {
		var oBuildInfoTable = EditorFunc.CreateScriptableObj<CBuildInfoTable>();

		oBuildInfoTable.SetJenkinsInfo(new STJenkinsInfo() {
			m_oUserID = "dante",

			m_oBranch = "master",
			m_oProjectName = "Library",

			m_oSourceRoot = "00011.Library",
			m_oWorkspaceRoot = "/Users/dante/Documents/jenkins/workspace",
			m_oDistributionPath = "/Users/dante/Documents/jenkins/builds",

			m_oBuildToken = "JenkinsBuild",
			m_oAccessToken = "116037dbf725a41cff3fe98b951aa2cd42",

			m_oBuildURLFormat = "http://localhost:8080/{0}/buildWithParameters"
		});

		oBuildInfoTable.SetStandaloneBuildInfo(new STStandaloneBuildInfo() {
			m_oCategory = "public.app-category.games"
		});

		oBuildInfoTable.SetiOSBuildInfo(new STiOSBuildInfo() {
			m_oTeamID = "8XBEE2A299",
			m_oTargetOSVersion = "10.0",

			m_oDevProfileID = string.Empty,
			m_oAdhocProfileID = string.Empty,
			m_oStoreProfileID = string.Empty,

			m_oURLSchemeList = new List<string>() {
				"library"
			},

			m_eiPadLaunchScreenType = iOSLaunchScreenType.Default,
			m_eiPhoneLaunchScreenType = iOSLaunchScreenType.Default
		});

		oBuildInfoTable.SetAndroidBuildInfo(new STAndroidBuildInfo() {
			m_oKeystorePath = "Keystore.keystore",
			m_oKeyaliasName = "Keystore",

			m_oKeystorePassword = "NSString132!",
			m_oKeyaliasPassword = "NSString132!",

			m_eMinSDKVersion = AndroidSdkVersions.AndroidApiLevel21,
			m_eTargetSDKVersion = AndroidSdkVersions.AndroidApiLevel28
		});
	}

	//! 빌드 옵션 테이블을 생성한다
	[MenuItem("Utility/Create/BuildOptionTable")]
	public static void CreateBuildOptionTable() {
		var oBuildOptionTable = EditorFunc.CreateScriptableObj<CBuildOptionTable>();

		oBuildOptionTable.SetEditorOption(new STEditorOption() {
			m_bIsAsyncShaderCompile = true,
			m_bIsUseLegacyProbeSampleCount = true,
			m_bIsEnableTextureStreamingInPlayMode = true,
			m_bIsEnableTextureStreamingInEditMode = true,

			m_eCacheServerMode = CacheServerMode.AsPreferences,
			m_eAssetPipelineMode = AssetPipelineMode.Version2,
			m_eLineEndingMode = LineEndingsMode.Windows,
			m_eTextureCompressionType = ETextureCompressionType.DEFAULT
		});

		oBuildOptionTable.SetSndOption(new STSndOption() {
			m_bIsDisable = false,
			m_bIsVirtualizeEffect = true,

			m_nSampleRate = 0,
			m_nNumRealVoices = 32,
			m_nNumVirtualVoices = 512,

			m_fGlobalVolume = 1.0f,
			m_fRolloffScale = 1.0f,
			m_fDopplerFactor = 1.0f,

			m_eSpeakerMode = AudioSpeakerMode.Mono,
			m_eDSPBufferSize = EDSPBufferSize.BEST_PERFORMANCE
		});

		oBuildOptionTable.SetCommonBuildOption(new STCommonBuildOption() {
			m_bIsGPUSkinning = true,
			m_bIsMTRendering = true,
			m_bIsRunInBackground = false,
			m_bIsPreBakeCollisionMesh = false,
			m_bIsUse32BitDisplayBuffer = true,
			m_bIsMuteOtherAudioSource = false,
			m_bIsEnableFrameTimingStats = false,
			m_bIsEnableInternalProfiler = false,
			m_bIsPreserveFrameBufferAlpha = false,
			m_bIsEnableVulkanSRGBWrite = false,
			m_bIsEnableLightmapStreaming = true,
			
			m_nLightmapStreamingPriority = 0,
			m_nNumVulkanSwapChainBuffers = 3,
			
			m_oAOTCompileOption = string.Empty,
			m_oPreloadAssetList = null,
			
			m_eAccelerometerFrequency = EAccelerometerFrequency.FREQUENCY_60_HZ,
			m_eLightmapEncodingQuality = ELightmapEncodingQuality.LOW
		});

		oBuildOptionTable.SetStandaloneBuildOption(new STStandaloneBuildOption() {
			m_bIsForceSingleInstance = true,
			m_bIsCaptureSingleScreen = false,
			
			m_eFullscreenMode = FullScreenMode.Windowed
		});

		oBuildOptionTable.SetiOSBuildOption(new STiOSBuildOption() {
			m_bIsEnableProMotion = false,
			m_bIsRequreARKitSupport = false,
			m_bIsRequirePersistentWIFI = false,
			m_bIsAutoAddCapabilities = true,
			
			m_oCameraDescription = string.Empty,
			m_oLocationDescription = string.Empty,
			m_oMicrophoneDescription = string.Empty,

			m_eTargetDevice = iOSTargetDevice.iPhoneAndiPad,
			m_eStatusBarStyle = iOSStatusBarStyle.Default,
			m_eBackgroundBehavior = iOSAppInBackgroundBehavior.Suspend
		});

		oBuildOptionTable.SetAndroidBuildOption(new STAndroidBuildOption() {
			m_bIsTVCompatibility = false,
			m_bIsOptimizeFramePacing = false,

			m_nAppBundleSize = 150,

			m_eBlitType = AndroidBlitType.Always,
			m_eAspectRatioMode = EAspectRatioMode.NATIVE_ASPECT_RATIO,
			m_ePreferredInstallLocation = AndroidPreferredInstallLocation.ForceInternal
		});
	}

	//! 프로젝트 정보 테이블을 생성한다
	[MenuItem("Utility/Create/ProjectInfoTable")]
	public static void CreateProjectInfoTable() {
		var oProjectInfoTable = EditorFunc.CreateScriptableObj<CProjectInfoTable>();
		oProjectInfoTable.SetCompanyName("LKStudio");
		oProjectInfoTable.SetProjectName("Library");
		oProjectInfoTable.SetProductName("Library");

		oProjectInfoTable.SetMacProjectInfo(new STProjectInfo() {
			m_oAppID = "dante.distribution.library",

			m_oBuildNumber = "1",
			m_oBuildVersion = "1.0.0",

			m_oStoreURL = string.Empty,
			m_oSupportMail = "are2341@nate.com"
		});

		oProjectInfoTable.SetWindowsProjectInfo(new STProjectInfo() {
			m_oAppID = "dante.distribution.library",

			m_oBuildNumber = "1",
			m_oBuildVersion = "1.0.0",

			m_oStoreURL = string.Empty,
			m_oSupportMail = "are2341@nate.com"
		});

		oProjectInfoTable.SetiOSProjectInfo(new STProjectInfo() {
			m_oAppID = "dante.distribution.library",

			m_oBuildNumber = "1",
			m_oBuildVersion = "1.0.0",

			m_oStoreURL = "https://itunes.apple.com/app/id1197115495",
			m_oSupportMail = "are2341@nate.com"
		});

		oProjectInfoTable.SetGoogleProjectInfo(new STProjectInfo() {
			m_oAppID = "dante.distribution.library",

			m_oBuildNumber = "1",
			m_oBuildVersion = "1.0.0",

			m_oStoreURL = "https://play.google.com/store/apps/details?id=dante.distribution.library",
			m_oSupportMail = "are2341@nate.com"
		});

		oProjectInfoTable.SetOneStoreProjectInfo(new STProjectInfo() {
			m_oAppID = "dante.distribution.library",

			m_oBuildNumber = "1",
			m_oBuildVersion = "1.0.0",

			m_oStoreURL = string.Empty,
			m_oSupportMail = "are2341@nate.com"
		});

		oProjectInfoTable.SetGalaxyStoreProjectInfo(new STProjectInfo() {
			m_oAppID = "dante.distribution.library",

			m_oBuildNumber = "1",
			m_oBuildVersion = "1.0.0",

			m_oStoreURL = string.Empty,
			m_oSupportMail = "are2341@nate.com"
		});
	}

	//! 플러그인 정보 테이블을 생성한다
	[MenuItem("Utility/Create/PluginInfoTable")]
	public static void CreatePluginInfoTable() {
		var oPluginInfoTable = EditorFunc.CreateScriptableObj<CPluginInfoTable>();

#if ADS_ENABLE
#if ADMOB_ENABLE
		oPluginInfoTable.SetiOSAdmobPluginInfo(new STAdmobPluginInfo() {
			m_oBannerAdsID = "ca-app-pub-4429226069711533/9321907041",
			m_oRewardAdsID = "ca-app-pub-4429226069711533/1443417026",
			m_oNativeAdsID = "ca-app-pub-4429226069711533/7205442808",
			m_oFullscreenAdsID = "ca-app-pub-4429226069711533/6695743706",

			m_oTemplateIDList = new List<string>()
		});

		oPluginInfoTable.SetAndroidAdmobPluginInfo(new STAdmobPluginInfo() {
			m_oBannerAdsID = "ca-app-pub-4429226069711533/6026208607",
			m_oRewardAdsID = "ca-app-pub-4429226069711533/1279765492",
			m_oNativeAdsID = "ca-app-pub-4429226069711533/8775112138",
			m_oFullscreenAdsID = "ca-app-pub-4429226069711533/8460800259",

			m_oTemplateIDList = new List<string>()
		});
#endif			// #if ADMOB_ENABLE

#if UNITY_ADS_ENABLE
		oPluginInfoTable.SetiOSUnityAdsPluginInfo(new STUnityAdsPluginInfo() {
			m_oGameID = "3258360",

			m_oBannerAdsPlacement = "banner",
			m_oRewardAdsPlacement = "rewardedVideo",
			m_oFullscreenAdsPlacement = "video"
		});

		oPluginInfoTable.SetAndroidUnityAdsPluginInfo(new STUnityAdsPluginInfo() {
			m_oGameID = "3258361",

			m_oBannerAdsPlacement = "banner",
			m_oRewardAdsPlacement = "rewardedVideo",
			m_oFullscreenAdsPlacement = "video"
		});
#endif			// #if UNITY_ADS_ENABLE

#if IRON_SOURCE_ENABLE
		oPluginInfoTable.SetiOSIronSourcePluginInfo(new STIronSourcePluginInfo() {
			m_oAppKey = "aca5425d",

			m_oBannerAdsPlacement = "DefaultBanner",
			m_oRewardAdsPlacement = "DefaultInterstitial",
			m_oFullscreenAdsPlacement = "DefaultRewardedVideo"
		});

		oPluginInfoTable.SetAndroidIronSourcePluginInfo(new STIronSourcePluginInfo() {
			m_oAppKey = "b8c2c725",

			m_oBannerAdsPlacement = "DefaultBanner",
			m_oRewardAdsPlacement = "DefaultInterstitial",
			m_oFullscreenAdsPlacement = "DefaultRewardedVideo"
		});
#endif			// #if IRON_SOURCE_ENABLE

#if APP_LOVIN_ENABLE
		oPluginInfoTable.SetiOSAppLovinPluginInfo(new STAppLovinPluginInfo() {
			m_oSDKKey = "",
			
			m_oBannerAdsID = "",
			m_oRewardAdsID = "",
			m_oFullscreenAdsID = ""
		});

		oPluginInfoTable.SetAndroidAppLovinPluginInfo(new STAppLovinPluginInfo() {
			m_oSDKKey = "",

			m_oBannerAdsID = "",
			m_oRewardAdsID = "",
			m_oFullscreenAdsID = ""
		});
#endif			// #if APP_LOVIN_ENABLE
#endif			// #if ADS_ENABLE

#if TENJIN_ENABLE
		oPluginInfoTable.SetTenjinPluginInfo(new STTenjinPluginInfo() {
			m_oAPIKey = "ZAI7IG4Y9G6YW1H3DTMNUX7GBFT8FLWS"
		});
#endif			// #if TENJIN_ENABLE

#if FLURRY_ENABLE
		oPluginInfoTable.SetiOSFlurryPluginInfo(new STFlurryPluginInfo() {
			m_oAPIKey = "94J595GYKRYCJ6XSWR29"
		});

		oPluginInfoTable.SetAndroidFlurryPluginInfo(new STFlurryPluginInfo() {
			m_oAPIKey = "FZ3K7T8FB4476688BXD5"
		});
#endif			// #if FLURRY_ENABLE

#if FIREBASE_ENABLE && FIREBASE_DATABASE_ENABLE
		oPluginInfoTable.SetFirebasePluginInfo(new STFirebasePluginInfo() {
			m_oDatabaseURL = "https://library-97927303.firebaseio.com"
		});
#endif			// #if FIREBASE_ENABLE && FIREBASE_DATABASE_ENABLE
	}
	#endregion			// 클래스 함수

	#region 조건부 클래스 함수
#if PURCHASE_ENABLE
	//! 상품 정보 테이블을 생성한다
	[MenuItem("Utility/Create/ProductInfoTable")]
	public static void CreateProductInfoTable() {
		var oProductInfoTable = EditorFunc.CreateScriptableObj<CProductInfoTable>();

		oProductInfoTable.SetCommonProductInfoList(new List<STProductInfo>() {
			new STProductInfo {
				m_oID = "dante.distribution.library.iap.consumable",
				m_eProductType = ProductType.Consumable
			},

			new STProductInfo {
				m_oID = "dante.distribution.library.iap.nonconsumable",
				m_eProductType = ProductType.NonConsumable
			}
		});
	}
#endif			// #if PURCHASE_ENABLE
	#endregion			// 조건부 클래스 함수
}
