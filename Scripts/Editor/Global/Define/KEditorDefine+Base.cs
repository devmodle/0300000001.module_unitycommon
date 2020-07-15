using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

//! 에디터 기본 상수
public static partial class KEditorDefine {
	#region 기본
	// 시간
	public const float B_DELTA_TIME_HIERARCHY_UPDATE = 1.0f;
	public const float B_DELTA_TIME_EDITOR_SM_SCENE_UPDATE = 0.25f;

	// 계층 뷰
	public const float B_HIERARCHY_WIDTH = 250.0f;
	public const float B_HIERARCHY_OFFSET_X = 125.0f;

	// 크기 {
	public const float B_WIDTH_EDITOR_W_NAME_TEXT_FIELD = 100.0f;
	public const float B_TOTAL_WIDTH_EDITOR_W_NAME_TEXT_FIELD = 200.0f;

	public const float B_WIDTH_EDITOR_W_APPLY_BUTTON = 100.0f;
	// 크기 }

	// 토큰
	public const string B_TOKEN_EXTENSION = ";";
	public const string B_TOKEN_DEFINE_SYMBOL = ";";

	// 형식
	public const string U_SORTING_ORDER_INFO_FORMAT = "[{0}:{1}]";

	// 커맨드 라인
	public const string B_CMDLINE_PARAMETER_FORMAT_SHELL = "-c \"{0}\"";
	public const string B_CMDLINE_PARAMETER_FORMAT_CMD_PROMPT = "/c \"{0}\"";

	// 알림 팝업 {
	public const string B_ALERT_P_TITLE = "알림";
	public const string B_ALERT_P_OK_BTN_TEXT = "확인";
	public const string B_ALERT_P_CANCEL_BTN_TEXT = "취소";

	public const string B_ALERT_P_EXPORT_IMG_SUCCESS_MSG = "이미지를 추출했습니다.";

	public const string B_ALERT_P_EXPORT_TEXTURE_FAIL_MSG = "텍스처를 선택해주세요.";
	public const string B_ALERT_P_EXPORT_SPRITE_FAIL_MSG = "스프라이트를 선택해주세요.";
	// 알림 팝업 }

	// 이름 {
	public const string B_OBJ_NAME_OBJ_NAME_EDITOR_POPUP = "ObjNameEditorPopup";

	public const string B_OBJ_NAME_TEXT = "Text";
	public const string B_OBJ_NAME_LOCALIZE_TEXT = "LocalizeText";

	public const string B_OBJ_NAME_TEXT_BTN = "TextBtn";
	public const string B_OBJ_NAME_TEXT_SCALE_BTN = "TextScaleBtn";

	public const string B_OBJ_NAME_LOCALIZE_TEXT_BTN = "LocalizeTextBtn";
	public const string B_OBJ_NAME_LOCALIZE_TEXT_SCALE_BTN = "LocalizeTextScaleBtn";

	public const string B_OBJ_NAME_IMG_BTN = "ImgBtn";
	public const string B_OBJ_NAME_IMG_SCALE_BTN = "ImgScaleBtn";

	public const string B_OBJ_NAME_IMG_TEXT_BTN = "ImgTextBtn";
	public const string B_OBJ_NAME_IMG_TEXT_SCALE_BTN = "ImgTextScaleBtn";

	public const string B_OBJ_NAME_IMG_LOCALIZE_TEXT_BTN = "ImgLocalizeTextBtn";
	public const string B_OBJ_NAME_IMG_LOCALIZE_TEXT_SCALE_BTN = "ImgLocalizeTextScaleBtn";

	public const string B_OBJ_NAME_SCROLL_VIEW = "ScrollView";
	public const string B_OBJ_NAME_PAGE_SCROLL_VIEW = "PageScrollView";

	public const string B_OBJ_NAME_TOUCH_RESPONDER = "TouchResponder";
	public const string B_OBJ_NAME_DRAG_RESPONDER = "DragResponder";

	public const string B_OBJ_NAME_SCENE_EDITOR_CAMERA = "SceneCamera";
	public const string B_OBJ_NAME_SCENE_EDITOR_LIGHT = "SceneLight";
	
	public const string B_PROPERTY_NAME_CATEGORY = "applicationCategoryType";
	public const string B_PROPERTY_NAME_OPTIMIZE_FRAME_PACING = "optimizedFramePacing";
	public const string B_PROPERTY_NAME_REQUIRE_AR_KIT_SUPPORT = "requiresARKitSupport";
	public const string B_PROPERTY_NAME_APPLE_ENABLE_PRO_MOTION = "appleEnableProMotion";
	public const string B_PROPERTY_NAME_AUTO_ADD_CAPABILITIES = "automaticallyDetectAndAddCapabilities";
	public const string B_PROPERTY_NAME_VALIDATE_APP_BUNDLE_SIZE = "validateAppBundleSize";
	public const string B_PROPERTY_NAME_APP_BUNDLE_SIZE_TO_VALIDATE = "appBundleSizeToValidate";
	public const string B_PROPERTY_NAME_SUPPORTED_ASPECT_RATIO_MODE = "supportedAspectRatioMode";

	public const string B_PROPERTY_NAME_SORTING_LAYER = "sortingLayerName";
	public const string B_PROPERTY_NAME_SORTING_ORDER = "sortingOrder";

	public const string B_PROPERTY_NAME_TAG_M_TAG = "tags";
	public const string B_PROPERTY_NAME_TAG_M_NAME = "name";
	public const string B_PROPERTY_NAME_TAG_M_UNIQUE_ID = "uniqueID";
	public const string B_PROPERTY_NAME_TAG_M_SORTING_LAYER = "m_SortingLayers";

	public const string B_PROPERTY_NAME_SND_M_GLOBAL_VOLUME = "m_Volume";
	public const string B_PROPERTY_NAME_SND_M_ROLLOFF_SCALE = "Rolloff Scale";
	public const string B_PROPERTY_NAME_SND_M_DOPPLER_FACTOR = "Doppler Factor";
	public const string B_PROPERTY_NAME_SND_M_DISABLE_AUDIO = "m_DisableAudio";
	public const string B_PROPERTY_NAME_SND_M_VIRTUALIZE_EFFECT = "m_VirtualizeEffects";

	public const string B_PROPERTY_NAME_ENABLE_BAKE_LIGHTMAPS = "m_GISettings.m_EnableBakedLightmaps";
	public const string B_PROPERTY_NAME_ENABLE_REALTIME_LIGHTMAPS = "m_GISettings.m_EnableRealtimeLightmaps";

	public const string B_FUNC_NAME_GET_LIGHTMAP_SETTINGS = "GetLightmapSettings";
	public const string B_FUNC_NAME_SET_COMPRESSION_TYPE = "SetCompressionType";
	public const string B_FUNC_NAME_SET_LIGHTMAP_ENCODING_QUALITY = "SetLightmapEncodingQualityForPlatformGroup";
	public const string B_FUNC_NAME_SET_LIGHTMAP_STREAMING_ENABLE = "SetLightmapStreamingEnabledForPlatformGroup";
	public const string B_FUNC_NAME_SET_LIGHTMAP_STREAMING_PRIORITY = "SetLightmapStreamingPriorityForPlatformGroup";

	public const string B_SCENE_NAME_PATTERN = "t:Example t:Scene";

	public const string B_SCENE_NAME_PATTERN_EDITOR_A = "EditorMenu";
	public const string B_SCENE_NAME_PATTERN_EDITOR_B = "EditorScene";
	// 이름 }

	// 경로 {
	public const string B_TOOL_PATH_SHELL = "/bin/zsh";
	public const string B_TOOL_PATH_CMD_PROMPT = "cmd.exe";

	public const string B_DIR_PATH_ASSETS = "Assets/";
	public const string B_DIR_PATH_AUTO_CREATE = "00.AutoCreate/";
	public const string B_DIR_PATH_PROJECT_SETTINGS = "ProjectSettings/";

	public const string B_DIR_PATH_EXPORT_IMG_BASE = "Export/Images/";
	// 경로 }
	
	// 에디터 옵션 {
	public const string B_EDITOR_OPTION_IOS_REMOTE_DEVICE = "Any iOS Device";
	public const string B_EDITOR_OPTION_ANDROID_REMOTE_DEVICE = "Any Android Device";
	public const string B_EDITOR_OPTION_DISABLE_REMOTE_DEVICE = "None";

	public const string B_EDITOR_OPTION_REMOTE_COMPRESSION = "JPEG";
	public const string B_EDITOR_OPTION_REMOTE_RESOLUTION = "Downsize";
	public const string B_EDITOR_OPTION_VERSION_CONTROL = "Visible Meta Files";
	public const string B_EDITOR_OPTION_JOYSTIC_SOURCE = "Remote";
	// 에디터 옵션 }

	// 젠킨스 {
	public const string B_JENKINS_KEY_BRANCH = "Branch";
	public const string B_JENKINS_KEY_SOURCE = "Source";
	public const string B_JENKINS_KEY_PROJECT_NAME = "ProjectName";
	public const string B_JENKINS_KEY_PROJECT_PATH = "ProjectPath";
	public const string B_JENKINS_KEY_DISTRIBUTION_PATH = "DistributionPath";
	public const string B_JENKINS_KEY_BUNDLE_ID = "BundleID";
	public const string B_JENKINS_KEY_PROFILE_ID = "ProfileID";
	public const string B_JENKINS_KEY_PLATFORM = "Platform";
	public const string B_JENKINS_KEY_ANALYTICS = "Analytics";
	public const string B_JENKINS_KEY_BUILD_MODE = "BuildMode";
	public const string B_JENKINS_KEY_BUILD_FUNC = "BuildFunc";
	public const string B_JENKINS_KEY_PIPELINE_NAME = "PipelineName";
	public const string B_JENKINS_KEY_IPA_EXPORT_METHOD = "IPAExportMethod";
	public const string B_JENKINS_KEY_BUILD_FILE_EXTENSION = "BuildFileExtension";

	public const string B_JENKINS_PIPELINE_GROUP_NAME = "job/00001.Common/job/01.Pipelines/job";
	public const string B_JENKINS_BUILD_PARAMETER_TOKEN = " ";

	public const string B_JENKINS_BUILD_DATA_FORMAT = "--data {0}={1}";
	public const string B_JENKINS_BUILD_CMD_FORMAT = "curl -X POST {0} --user {1}:{2} --data token={3}";

	public const string B_JENKINS_SOURCE_FORMAT = "{0}/{1}";
	public const string B_JENKINS_PROJECT_PATH_FORMAT = "{0}/{1}/{2}";
	public const string B_JENKINS_ANALYTICS_FORMAT = "{0}/00.Analytics";

	public const string B_JENKINS_STANDALONE_BUILD_PROJECT_NAME = "41.Standalone";
	public const string B_JENKINS_IOS_BUILD_PROJECT_NAME = "01.iOS";
	public const string B_JENKINS_ANDROID_BUILD_PROJECT_NAME = "11.Android";

	public const string B_JENKINS_DEBUG_BUILD_FUNC = "Debug";
	public const string B_JENKINS_RELEASE_BUILD_FUNC = "Release";
	public const string B_JENKINS_ADHOC_BUILD_FUNC = "Adhoc";
	public const string B_JENKINS_STORE_BUILD_FUNC = "Store";

	public const string B_JENKINS_STANDALONE_DEBUG_PIPELINE_NAME = "41.StandaloneDebug";
	public const string B_JENKINS_STANDALONE_RELEASE_PIPELINE_NAME = "42.StandaloneRelease";

	public const string B_JENKINS_IOS_DEBUG_PIPELINE_NAME = "01.iOSDebug";
	public const string B_JENKINS_IOS_RELEASE_PIPELINE_NAME = "02.iOSRelease";
	public const string B_JENKINS_IOS_ADHOC_PIPELINE_NAME = "03.iOSAdhoc";
	public const string B_JENKINS_IOS_STORE_PIPELINE_NAME = "04.iOSStore";

	public const string B_JENKINS_ANDROID_DEBUG_PIPELINE_NAME = "11.iOSDebug";
	public const string B_JENKINS_ANDROID_RELEASE_PIPELINE_NAME = "12.iOSRelease";
	public const string B_JENKINS_ANDROID_ADHOC_PIPELINE_NAME = "13.iOSAdhoc";
	public const string B_JENKINS_ANDROID_STORE_PIPELINE_NAME = "14.iOSStore";
	// 젠킨스 }

	// 맥
	public const string B_MAC_BUILD_PATH = "Builds/Standalone/Mac/MacBuildOutput.app";

	// 윈도우즈
	public const string B_WINDOWS_BUILD_PATH = "Builds/Standalone/Windows";

	// iOS {
	public const string B_IOS_DEV_IPA_EXPORT_METHOD = "development";
	public const string B_IOS_ADHOC_IPA_EXPORT_METHOD = "ad-hoc";
	public const string B_IOS_STORE_IPA_EXPORT_METHOD = "app-store";

	public const string B_IOS_BUILD_PATH = "Builds/iOS";
	public const string B_IOS_INFO_PLIST_PATH_FORMAT = "{0}/Info.plist";

	public const string B_IOS_ENCRYPTION_ENABLE_KEY = "ITSAppUsesNonExemptEncryption";
	// iOS }

	// 안드로이드 {
	public const string B_ANDROID_APK_BUILD_FILE_EXTENSION = "apk";
	public const string B_ANDROID_AAB_BUILD_FILE_EXTENSION = "aab";

	public const string B_ANDROID_BUILD_PATH_FORMAT = "Builds/Android/{0}/{1}.{2}";
	public const string B_ANDROID_BUILD_FILENAME_FORMAT = "{0}BuildOutput";
	// 안드로이드 }
	#endregion			// 기본

	#region 런타임 상수
	// 계층 뷰
	public static readonly Color B_HIERARCHY_TEXT_COLOR = new Color(1.0f, 0.27f, 0.0f, 1.0f);

	// 색상
	public static readonly Color B_COLOR_UNITY_LOGO_BG = new Color(0.137f, 0.121f, 0.125f, 1.0f);

	// 크기
	public static readonly Vector2 B_MIN_SIZE_EDITOR_WINDOW = new Vector2(350.0f, 350.0f);

	// 경로 {
	public static readonly string B_DIR_PATH_AUTO_CREATE_RESES = string.Format("{0}{1}Resources", KEditorDefine.B_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE);

	public static readonly string B_ABSOLUTE_DIR_PATH_ASSETS = string.Format("{0}/", Application.dataPath);
	public static readonly string B_ABSOLUTE_DIR_PATH_UNITY_ENGINE = string.Format("{0}/", EditorApplication.applicationPath);
	public static readonly string B_ABSOLUTE_DIR_PATH_SAMPLE_SCENE = string.Format("{0}00.Application/Common/Scenes/SampleScene.unity", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS);

	public static readonly string B_ABSOLUTE_DIR_PATH_TEMPLATES = string.Format("{0}00.Application/Common/Templates/", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS);
	public static readonly string B_ABSOLUTE_DIR_PATH_DATA_TEMPLATES = string.Format("{0}Datas/", KEditorDefine.B_ABSOLUTE_DIR_PATH_TEMPLATES);
	public static readonly string B_ABSOLUTE_DIR_PATH_PREFAB_TEMPLATES = string.Format("{0}Prefabs/", KEditorDefine.B_ABSOLUTE_DIR_PATH_TEMPLATES);
	public static readonly string B_ABSOLUTE_DIR_PATH_SCRIPT_TEMPLATES = string.Format("{0}Scripts/", KEditorDefine.B_ABSOLUTE_DIR_PATH_TEMPLATES);
	public static readonly string B_ABSOLUTE_DIR_PATH_SCRIPTABLE_TEMPLATES = string.Format("{0}Scriptables/", KEditorDefine.B_ABSOLUTE_DIR_PATH_TEMPLATES);
	public static readonly string B_ABSOLUTE_DIR_PATH_TABLE_TEMPLATES = string.Format("{0}Tables/", KEditorDefine.B_ABSOLUTE_DIR_PATH_TEMPLATES);

	public static readonly string B_ASSET_PATH_TAG_MANAGER = string.Format("{0}TagManager.asset", KEditorDefine.B_DIR_PATH_PROJECT_SETTINGS);
	public static readonly string B_ASSET_PATH_SND_MANAGER = string.Format("{0}AudioManager.asset", KEditorDefine.B_DIR_PATH_PROJECT_SETTINGS);

	public static readonly string B_DIR_PATH_SCENES = string.Format("{0}{1}/Scenes", KEditorDefine.B_DIR_PATH_ASSETS, KAppDefine.G_NAME_PROJECT_ROOT);
	public static readonly string B_DIR_PATH_AUTO_SCENES = string.Format("{0}{1}Scenes", KEditorDefine.B_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE);
	public static readonly string B_DIR_PATH_EDITOR_SCENES = string.Format("{0}{1}_Editor/Scenes", KEditorDefine.B_DIR_PATH_ASSETS, KAppDefine.G_NAME_PROJECT_ROOT);

	public static readonly string B_IMG_PATH_FORMAT_TEXTURE_TO_IMG = string.Format("{0}{1}", KEditorDefine.B_DIR_PATH_EXPORT_IMG_BASE, "Textures/{0}.png");
	public static readonly string B_IMG_PATH_FORMAT_SPRITE_TO_IMG = string.Format("{0}{1}", KEditorDefine.B_DIR_PATH_EXPORT_IMG_BASE, "Sprites/{0}.png");

	public static readonly string B_PATH_FORMAT_SCRIPTABLE_OBJ = string.Format("{0}{1}", KEditorDefine.B_DIR_PATH_ASSETS, "{0}.asset");
	public static readonly string B_ASSET_PATH_FORMAT_DEFINE_SYMBOL_OUTPUT = string.Format("{0}/BuildOutput/{1}", KDefine.B_DIR_PATH_WRITABLE, "{0}DefineSymbol.txt");

	// 스크립트 순서
	public static readonly Dictionary<System.Type, int> G_SCRIPT_ORDERS = new Dictionary<System.Type, int>() {
		[typeof(CStringTable)] = KDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CValueTable)] = KDefine.U_SCRIPT_ORDER_SINGLETON,

		[typeof(CUnityMsgSender)] = KDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CDeviceMsgReceiver)] = KDefine.U_SCRIPT_ORDER_SINGLETON,

		[typeof(CLogManager)] = KDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CSndManager)] = KDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CScheduleManager)] = KDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CResourceManager)] = KDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CNavigationManager)] = KDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CToastPopupManager)] = KDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CActivityIndicatorManager)] = KDefine.U_SCRIPT_ORDER_SINGLETON,

#if USE_CUSTOM_PROJECT_OPTION
		[typeof(CSubInitSceneManager)] = KDefine.U_SCRIPT_ORDER_INIT_SCENE_MANAGER,
		[typeof(CSubSetupSceneManager)] = KDefine.U_SCRIPT_ORDER_SETUP_SCENE_MANAGER,
		[typeof(CSubStartSceneManager)] = KDefine.U_SCRIPT_ORDER_START_SCENE_MANAGER,
		[typeof(CSubLoadingSceneManager)] = KDefine.U_SCRIPT_ORDER_LOADING_SCENE_MANAGER,
		[typeof(CSubSplashSceneManager)] = KDefine.U_SCRIPT_ORDER_SPLASH_SCENE_MANAGER,
		[typeof(CSubAgreeSceneManager)] = KDefine.U_SCRIPT_ORDER_AGREE_SCENE_MANAGER,
		[typeof(CSubIntroSceneManager)] = KDefine.U_SCRIPT_ORDER_SCENE_MANAGER,

#if MESSAGE_PACK_ENABLE
		[typeof(CAppInfoStorage)] = KDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CUserInfoStorage)] = KDefine.U_SCRIPT_ORDER_SINGLETON,
#endif			// #if MESSAGE_PACK_ENABLE
#endif			// #if USE_CUSTOM_PROJECT_OPTION

#if UNITY_ANDROID
		[typeof(CPermissionManager)] = KDefine.U_SCRIPT_ORDER_SINGLETON,
#endif			// #if UNITY_ANDROID

#if ADS_ENABLE
		[typeof(CAdsManager)] = KDefine.U_SCRIPT_ORDER_SINGLETON,
#endif			// #if ADS_ENABLE

#if TENJIN_ENABLE
		[typeof(CTenjinManager)] = KDefine.U_SCRIPT_ORDER_SINGLETON,
#endif			// #if TENJIN_ENABLE

#if FLURRY_ENABLE
		[typeof(CFlurryManager)] = KDefine.U_SCRIPT_ORDER_SINGLETON,
#endif			// #if FLURRY_ENABLE

#if FACEBOOK_ENABLE
		[typeof(CFacebookManager)] = KDefine.U_SCRIPT_ORDER_SINGLETON,
#endif			// #if FACEBOOK_ENABLE

#if FIREBASE_ENABLE
		[typeof(CFirebaseManager)] = KDefine.U_SCRIPT_ORDER_SINGLETON,
#endif			// #if FIREBASE_ENABLE

#if PURCHASE_ENABLE && MESSAGE_PACK_ENABLE
		[typeof(CPurchaseManager)] = KDefine.U_SCRIPT_ORDER_SINGLETON,
#endif			// #if PURCHASE_ENABLE && MESSAGE_PACK_ENABLE

#if UNITY_SERVICE_ENABLE
		[typeof(CUnityServiceManager)] = KDefine.U_SCRIPT_ORDER_SINGLETON
#endif			// #if UNITY_SERVICE_ENABLE
	};
	
	public static readonly KeyValuePair<string, string>[] B_PATH_DATA_FILEPATH_INFOS = new KeyValuePair<string, string>[] {
		new KeyValuePair<string, string>(string.Format("{0}T_Service_KO.txt", KEditorDefine.B_ABSOLUTE_DIR_PATH_DATA_TEMPLATES),
			string.Format("{0}{1}Resources/{2}.txt", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.AS_DATA_PATH_KOREAN_SERVICE_TEXT)),

		new KeyValuePair<string, string>(string.Format("{0}T_Personal_KO.txt", KEditorDefine.B_ABSOLUTE_DIR_PATH_DATA_TEMPLATES),
			string.Format("{0}{1}Resources/{2}.txt", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.AS_DATA_PATH_KOREAN_PERSONAL_TEXT)),

		new KeyValuePair<string, string>(string.Format("{0}T_Service_EN.txt", KEditorDefine.B_ABSOLUTE_DIR_PATH_DATA_TEMPLATES),
			string.Format("{0}{1}Resources/{2}.txt", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.AS_DATA_PATH_ENGLISH_SERVICE_TEXT)),

		new KeyValuePair<string, string>(string.Format("{0}T_Personal_EN.txt", KEditorDefine.B_ABSOLUTE_DIR_PATH_DATA_TEMPLATES),
			string.Format("{0}{1}Resources/{2}.txt", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.AS_DATA_PATH_ENGLISH_PERSONAL_TEXT))
	};
	
	public static readonly KeyValuePair<string, string>[] B_PATH_SCRIPT_FILEPATH_INFOS = new KeyValuePair<string, string>[] {
		new KeyValuePair<string, string>(string.Format("{0}Define/T_KAppDefine+Global.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_SCRIPT_TEMPLATES),
			string.Format("{0}{1}Scripts/Runtime/Global/Define/KAppDefine+Global.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE)),

		new KeyValuePair<string, string>(string.Format("{0}Define/T_KDefine+InitScene.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_SCRIPT_TEMPLATES),
			string.Format("{0}{1}Scripts/Runtime/Global/Define/KDefine+InitScene.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE)),

		new KeyValuePair<string, string>(string.Format("{0}Define/T_KDefine+SetupScene.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_SCRIPT_TEMPLATES),
			string.Format("{0}{1}Scripts/Runtime/Global/Define/KDefine+SetupScene.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE)),

		new KeyValuePair<string, string>(string.Format("{0}Define/T_KDefine+StartScene.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_SCRIPT_TEMPLATES),
			string.Format("{0}{1}Scripts/Runtime/Global/Define/KDefine+StartScene.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE)),

		new KeyValuePair<string, string>(string.Format("{0}Define/T_KDefine+LoadingScene.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_SCRIPT_TEMPLATES),
			string.Format("{0}{1}Scripts/Runtime/Global/Define/KDefine+LoadingScene.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE)),

		new KeyValuePair<string, string>(string.Format("{0}Define/T_KDefine+SplashScene.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_SCRIPT_TEMPLATES),
			string.Format("{0}{1}Scripts/Runtime/Global/Define/KDefine+SplashScene.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE)),

		new KeyValuePair<string, string>(string.Format("{0}Define/T_KDefine+AgreeScene.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_SCRIPT_TEMPLATES),
			string.Format("{0}{1}Scripts/Runtime/Global/Define/KDefine+AgreeScene.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE)),

		new KeyValuePair<string, string>(string.Format("{0}Define/T_KDefine+StringTable.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_SCRIPT_TEMPLATES),
			string.Format("{0}{1}Scripts/Runtime/Global/Define/KDefine+StringTable.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE)),

		new KeyValuePair<string, string>(string.Format("{0}Define/T_KDefine+ValueTable.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_SCRIPT_TEMPLATES),
			string.Format("{0}{1}Scripts/Runtime/Global/Define/KDefine+ValueTable.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE)),

		new KeyValuePair<string, string>(string.Format("{0}Define/T_KDefine+GameCenter.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_SCRIPT_TEMPLATES),
			string.Format("{0}{1}Scripts/Runtime/Global/Define/KDefine+GameCenter.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE)),

		new KeyValuePair<string, string>(string.Format("{0}Storage/T_CAppInfoStorage.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_SCRIPT_TEMPLATES),
			string.Format("{0}{1}Scripts/Runtime/Utility/Storage/CAppInfoStorage.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE)),

		new KeyValuePair<string, string>(string.Format("{0}Storage/T_CUserInfoStorage.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_SCRIPT_TEMPLATES),
			string.Format("{0}{1}Scripts/Runtime/Utility/Storage/CUserInfoStorage.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE)),

		new KeyValuePair<string, string>(string.Format("{0}Scene/T_CSubInitSceneManager.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_SCRIPT_TEMPLATES),
			string.Format("{0}{1}Scripts/Runtime/SubInitScene/CSubInitSceneManager.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE)),

		new KeyValuePair<string, string>(string.Format("{0}Scene/T_CSubSetupSceneManager.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_SCRIPT_TEMPLATES),
			string.Format("{0}{1}Scripts/Runtime/SubSetupScene/CSubSetupSceneManager.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE)),

		new KeyValuePair<string, string>(string.Format("{0}Scene/T_CSubStartSceneManager.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_SCRIPT_TEMPLATES),
			string.Format("{0}{1}Scripts/Runtime/SubStartScene/CSubStartSceneManager.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE)),

		new KeyValuePair<string, string>(string.Format("{0}Scene/T_CSubLoadingSceneManager.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_SCRIPT_TEMPLATES),
			string.Format("{0}{1}Scripts/Runtime/SubLoadingScene/CSubLoadingSceneManager.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE)),

		new KeyValuePair<string, string>(string.Format("{0}Scene/T_CSubSplashSceneManager.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_SCRIPT_TEMPLATES),
			string.Format("{0}{1}Scripts/Runtime/SubSplashScene/CSubSplashSceneManager.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE)),

		new KeyValuePair<string, string>(string.Format("{0}Scene/T_CSubAgreeSceneManager.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_SCRIPT_TEMPLATES),
			string.Format("{0}{1}Scripts/Runtime/SubAgreeScene/CSubAgreeSceneManager.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE)),

		new KeyValuePair<string, string>(string.Format("{0}Scene/T_CSubIntroSceneManager.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_SCRIPT_TEMPLATES),
			string.Format("{0}{1}Scripts/Runtime/SubIntroScene/CSubIntroSceneManager.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE))
	};

	public static readonly KeyValuePair<string, string>[] B_PATH_COMPARE_SCRIPT_FILEPATH_INFOS = new KeyValuePair<string, string>[] {
		new KeyValuePair<string, string>(string.Format("{0}../Packages/Scripts/TotalJSON/JSON.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS),
			string.Format("{0}00.Application/Common/External/TotalJSON/Scripts/JSON.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS)),

		new KeyValuePair<string, string>(string.Format("{0}../Packages/Scripts/TotalJSON/InternalTools.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS),
			string.Format("{0}00.Application/Common/External/TotalJSON/Internal/InternalTools.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS)),

		new KeyValuePair<string, string>(string.Format("{0}../Packages/Scripts/SmartTimer/Timer.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS),
			string.Format("{0}00.Application/Common/External/SmartTimersManager/TimerManager/Timer.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS)),

		new KeyValuePair<string, string>(string.Format("{0}../Packages/Scripts/ObjectPool/ObjectPool.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS),
			string.Format("{0}00.Application/Common/External/AdvanceObjectpool/Scripts/ObjectPool.cs", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS))
	};

	public static readonly KeyValuePair<string, string>[] B_PATH_PREFAB_FILEPATH_INFOS = new KeyValuePair<string, string>[] {
		new KeyValuePair<string, string>(string.Format("{0}{1}T_Text.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_PREFAB_TEMPLATES, KDefine.B_DIR_PATH_TEXT_BASE),
			string.Format("{0}{1}Resources/{2}.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_OBJ_PATH_TEXT)),

		new KeyValuePair<string, string>(string.Format("{0}{1}T_TextBtn.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_PREFAB_TEMPLATES, KDefine.B_DIR_PATH_BUTTON_BASE),
			string.Format("{0}{1}Resources/{2}.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_OBJ_PATH_TEXT_BTN)),

		new KeyValuePair<string, string>(string.Format("{0}{1}T_TextScaleBtn.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_PREFAB_TEMPLATES, KDefine.B_DIR_PATH_BUTTON_BASE),
			string.Format("{0}{1}Resources/{2}.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_OBJ_PATH_TEXT_SCALE_BTN)),

		new KeyValuePair<string, string>(string.Format("{0}{1}T_LocalizeText.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_PREFAB_TEMPLATES, KDefine.B_DIR_PATH_TEXT_BASE),
			string.Format("{0}{1}Resources/{2}.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_OBJ_PATH_LOCALIZE_TEXT)),

		new KeyValuePair<string, string>(string.Format("{0}{1}T_LocalizeTextBtn.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_PREFAB_TEMPLATES, KDefine.B_DIR_PATH_BUTTON_BASE),
			string.Format("{0}{1}Resources/{2}.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_OBJ_PATH_LOCALIZE_TEXT_BTN)),

		new KeyValuePair<string, string>(string.Format("{0}{1}T_LocalizeTextScaleBtn.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_PREFAB_TEMPLATES, KDefine.B_DIR_PATH_BUTTON_BASE),
			string.Format("{0}{1}Resources/{2}.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_OBJ_PATH_LOCALIZE_TEXT_SCALE_BTN)),

		new KeyValuePair<string, string>(string.Format("{0}{1}T_ImgBtn.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_PREFAB_TEMPLATES, KDefine.B_DIR_PATH_BUTTON_BASE),
			string.Format("{0}{1}Resources/{2}.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_OBJ_PATH_IMG_BTN)),

		new KeyValuePair<string, string>(string.Format("{0}{1}T_ImgScaleBtn.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_PREFAB_TEMPLATES, KDefine.B_DIR_PATH_BUTTON_BASE),
			string.Format("{0}{1}Resources/{2}.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_OBJ_PATH_IMG_SCALE_BTN)),

		new KeyValuePair<string, string>(string.Format("{0}{1}T_ImgTextBtn.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_PREFAB_TEMPLATES, KDefine.B_DIR_PATH_BUTTON_BASE),
			string.Format("{0}{1}Resources/{2}.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_OBJ_PATH_IMG_TEXT_BTN)),

		new KeyValuePair<string, string>(string.Format("{0}{1}T_ImgTextScaleBtn.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_PREFAB_TEMPLATES, KDefine.B_DIR_PATH_BUTTON_BASE),
			string.Format("{0}{1}Resources/{2}.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_OBJ_PATH_IMG_TEXT_SCALE_BTN)),

		new KeyValuePair<string, string>(string.Format("{0}{1}T_ImgLocalizeTextBtn.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_PREFAB_TEMPLATES, KDefine.B_DIR_PATH_BUTTON_BASE),
			string.Format("{0}{1}Resources/{2}.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_OBJ_PATH_IMG_LOCALIZE_TEXT_BTN)),

		new KeyValuePair<string, string>(string.Format("{0}{1}T_ImgLocalizeTextScaleBtn.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_PREFAB_TEMPLATES, KDefine.B_DIR_PATH_BUTTON_BASE),
			string.Format("{0}{1}Resources/{2}.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_OBJ_PATH_IMG_LOCALIZE_TEXT_SCALE_BTN)),

		new KeyValuePair<string, string>(string.Format("{0}{1}T_ScrollView.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_PREFAB_TEMPLATES, KDefine.B_DIR_PATH_SCROLL_VIEW_BASE),
			string.Format("{0}{1}Resources/{2}.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_OBJ_PATH_SCROLL_VIEW)),

		new KeyValuePair<string, string>(string.Format("{0}{1}T_PageScrollView.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_PREFAB_TEMPLATES, KDefine.B_DIR_PATH_SCROLL_VIEW_BASE),
			string.Format("{0}{1}Resources/{2}.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_OBJ_PATH_PAGE_SCROLL_VIEW)),

		new KeyValuePair<string, string>(string.Format("{0}{1}T_DragResponder.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_PREFAB_TEMPLATES, KDefine.B_DIR_PATH_RESPONDER_BASE),
			string.Format("{0}{1}Resources/{2}.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_OBJ_PATH_DRAG_RESPONDER)),

		new KeyValuePair<string, string>(string.Format("{0}{1}T_TouchResponder.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_PREFAB_TEMPLATES, KDefine.B_DIR_PATH_RESPONDER_BASE),
			string.Format("{0}{1}Resources/{2}.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_OBJ_PATH_TOUCH_RESPONDER)),

		new KeyValuePair<string, string>(string.Format("{0}{1}T_AlertPopup.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_PREFAB_TEMPLATES, KDefine.B_DIR_PATH_POPUP_BASE),
			string.Format("{0}{1}Resources/{2}.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_OBJ_PATH_ALERT_POPUP)),
			
		new KeyValuePair<string, string>(string.Format("{0}{1}T_ToastPopup.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_PREFAB_TEMPLATES, KDefine.B_DIR_PATH_POPUP_BASE),
			string.Format("{0}{1}Resources/{2}.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_OBJ_PATH_TOAST_POPUP)),

		new KeyValuePair<string, string>(string.Format("{0}{1}T_BGSnd.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_PREFAB_TEMPLATES, KDefine.B_DIR_PATH_SOUND_BASE),
			string.Format("{0}{1}Resources/{2}.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_OBJ_PATH_BG_SND)),

		new KeyValuePair<string, string>(string.Format("{0}{1}T_FXSnd.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_PREFAB_TEMPLATES, KDefine.B_DIR_PATH_SOUND_BASE),
			string.Format("{0}{1}Resources/{2}.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_OBJ_PATH_FX_SND)),

		new KeyValuePair<string, string>(string.Format("{0}00.Application/Common/External/SmartTimersManager/TimerManager/TimersManager.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS),
			string.Format("{0}{1}Resources/{2}.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_OBJ_PATH_SS_TIMER_MANAGER)),

#if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
		new KeyValuePair<string, string>(string.Format("{0}00.Application/Common/External/OmniSARTechnologies/LiteFPSCounter/Prefabs/LiteFPSCounter.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS),
			string.Format("{0}{1}Resources/{2}.prefab", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_OBJ_PATH_SS_FPS_COUNTER)),
#endif			// #if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	};

	public static readonly KeyValuePair<string, string>[] B_PATH_TABLE_FILEPATH_INFOS = new KeyValuePair<string, string>[] {
		new KeyValuePair<string, string>(string.Format("{0}{1}T_ValueTable_Common.csv", KEditorDefine.B_ABSOLUTE_DIR_PATH_TABLE_TEMPLATES, KDefine.B_DIR_PATH_VALUE_INFO_BASE),
			string.Format("{0}{1}Resources/{2}.csv", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_TABLE_PATH_G_COMMON_VALUE_TABLE)),
			
		new KeyValuePair<string, string>(string.Format("{0}{1}T_StringTable_Common.csv", KEditorDefine.B_ABSOLUTE_DIR_PATH_TABLE_TEMPLATES, KDefine.B_DIR_PATH_STRING_INFO_BASE),
			string.Format("{0}{1}Resources/{2}.csv", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_TABLE_PATH_G_COMMON_STRING_TABLE)),

		new KeyValuePair<string, string>(string.Format("{0}{1}T_StringTable_Common_KO.csv", KEditorDefine.B_ABSOLUTE_DIR_PATH_TABLE_TEMPLATES, KDefine.B_DIR_PATH_STRING_INFO_BASE),
			string.Format("{0}{1}Resources/{2}.csv", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_TABLE_PATH_G_KOREAN_COMMON_STRING_TABLE)),

		new KeyValuePair<string, string>(string.Format("{0}{1}T_StringTable_Common_EN.csv", KEditorDefine.B_ABSOLUTE_DIR_PATH_TABLE_TEMPLATES, KDefine.B_DIR_PATH_STRING_INFO_BASE),
			string.Format("{0}{1}Resources/{2}.csv", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_TABLE_PATH_G_ENGLISH_COMMON_STRING_TABLE))
	};

	public static readonly KeyValuePair<string, string>[] B_PATH_SCRIPTABLE_FILEPATH_INFOS = new KeyValuePair<string, string>[] {
		new KeyValuePair<string, string>(string.Format("{0}T_BuildInfoTable.asset", KEditorDefine.B_ABSOLUTE_DIR_PATH_SCRIPTABLE_TEMPLATES),
			string.Format("{0}{1}Resources/{2}.asset", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_SCRIPTABLE_PATH_G_BUILD_INFO_TABLE)),
			
		new KeyValuePair<string, string>(string.Format("{0}T_BuildOptionTable.asset", KEditorDefine.B_ABSOLUTE_DIR_PATH_SCRIPTABLE_TEMPLATES),
			string.Format("{0}{1}Resources/{2}.asset", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_SCRIPTABLE_PATH_G_BUILD_OPTION_TABLE)),

		new KeyValuePair<string, string>(string.Format("{0}T_DefineSymbolTable.asset", KEditorDefine.B_ABSOLUTE_DIR_PATH_SCRIPTABLE_TEMPLATES),
			string.Format("{0}{1}Resources/{2}.asset", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_SCRIPTABLE_PATH_G_DEFINE_SYMBOL_TABLE)),
			
		new KeyValuePair<string, string>(string.Format("{0}T_DeviceInfoTable.asset", KEditorDefine.B_ABSOLUTE_DIR_PATH_SCRIPTABLE_TEMPLATES),
			string.Format("{0}{1}Resources/{2}.asset", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_SCRIPTABLE_PATH_G_DEVICE_INFO_TABLE)),
		
		new KeyValuePair<string, string>(string.Format("{0}T_ProjectInfoTable.asset", KEditorDefine.B_ABSOLUTE_DIR_PATH_SCRIPTABLE_TEMPLATES),
			string.Format("{0}{1}Resources/{2}.asset", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_SCRIPTABLE_PATH_G_PROJECT_INFO_TABLE)),

#if ADS_ENABLE || TENJIN_ENABLE || FLURRY_ENABLE || FIREBASE_ENABLE
		new KeyValuePair<string, string>(string.Format("{0}T_PluginInfoTable.asset", KEditorDefine.B_ABSOLUTE_DIR_PATH_SCRIPTABLE_TEMPLATES),
			string.Format("{0}{1}Resources/{2}.asset", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_SCRIPTABLE_PATH_G_PLUGIN_INFO_TABLE)),
#endif			// #if ADS_ENABLE || TENJIN_ENABLE || FLURRY_ENABLE || FIREBASE_ENABLE

#if PURCHASE_ENABLE			
		new KeyValuePair<string, string>(string.Format("{0}T_ProductInfoTable.asset", KEditorDefine.B_ABSOLUTE_DIR_PATH_SCRIPTABLE_TEMPLATES),
			string.Format("{0}{1}Resources/{2}.asset", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.U_SCRIPTABLE_PATH_G_PRODUCT_INFO_TABLE))
#endif			// #if PURCHASE_ENABLE
	};

	public static readonly KeyValuePair<string, string>[] B_PATH_SCENE_FILEPATH_INFOS = new KeyValuePair<string, string>[] {
		new KeyValuePair<string, string>(KEditorDefine.B_ABSOLUTE_DIR_PATH_SAMPLE_SCENE,
			string.Format("{0}{1}Scenes/{2}.unity", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.B_SCENE_NAME_INIT)),

		new KeyValuePair<string, string>(KEditorDefine.B_ABSOLUTE_DIR_PATH_SAMPLE_SCENE,
			string.Format("{0}{1}Scenes/{2}.unity", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.B_SCENE_NAME_SETUP)),

		new KeyValuePair<string, string>(KEditorDefine.B_ABSOLUTE_DIR_PATH_SAMPLE_SCENE,
			string.Format("{0}{1}Scenes/{2}.unity", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.B_SCENE_NAME_START)),

		new KeyValuePair<string, string>(KEditorDefine.B_ABSOLUTE_DIR_PATH_SAMPLE_SCENE,
			string.Format("{0}{1}Scenes/{2}.unity", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.B_SCENE_NAME_LOADING)),

		new KeyValuePair<string, string>(KEditorDefine.B_ABSOLUTE_DIR_PATH_SAMPLE_SCENE,
			string.Format("{0}{1}Scenes/{2}.unity", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.B_SCENE_NAME_SPLASH)),

		new KeyValuePair<string, string>(KEditorDefine.B_ABSOLUTE_DIR_PATH_SAMPLE_SCENE,
			string.Format("{0}{1}Scenes/{2}.unity", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.B_SCENE_NAME_AGREE)),

		new KeyValuePair<string, string>(KEditorDefine.B_ABSOLUTE_DIR_PATH_SAMPLE_SCENE,
			string.Format("{0}{1}Scenes/{2}.unity", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, KEditorDefine.B_DIR_PATH_AUTO_CREATE, KDefine.B_SCENE_NAME_INTRO))
	};
	// 경로 }

	// 에디터 옵션
	public static readonly string[] B_EDITOR_OPTION_EXTENSIONS = new string[] {
		"txt", "xml", "fnt", "cd", "asmdef", "rsp", "asmref"
	};


	public static readonly Dictionary<string, System.Type> B_SCENE_MANAGER_TYPE_LIST = new Dictionary<string, System.Type>() {
		[KDefine.B_SCENE_NAME_INIT] = typeof(CSubInitSceneManager),
		[KDefine.B_SCENE_NAME_SETUP] = typeof(CSubSetupSceneManager),
		[KDefine.B_SCENE_NAME_START] = typeof(CSubStartSceneManager),
		[KDefine.B_SCENE_NAME_LOADING] = typeof(CSubLoadingSceneManager),
		[KDefine.B_SCENE_NAME_SPLASH] = typeof(CSubSplashSceneManager),
		[KDefine.B_SCENE_NAME_AGREE] = typeof(CSubAgreeSceneManager),
		[KDefine.B_SCENE_NAME_INTRO] = typeof(CSubIntroSceneManager)
	};

	// 젠킨스
	public static readonly string B_JENKINS_STANDALONE_PIPELINE = string.Format("{0}/41.Standalone", KEditorDefine.B_JENKINS_PIPELINE_GROUP_NAME);
	public static readonly string B_JENKINS_IOS_PIPELINE = string.Format("{0}/01.iOS", KEditorDefine.B_JENKINS_PIPELINE_GROUP_NAME);
	public static readonly string B_JENKINS_ANDROID_PIPELINE = string.Format("{0}/11.Android", KEditorDefine.B_JENKINS_PIPELINE_GROUP_NAME);

	// 독립 플랫폼
	public static readonly string B_STANDALONE_ABSOLUTE_BUILD_PATH_FORMAT = string.Format("{0}../Builds/Standalone/{1}", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, "{0}");

	// 맥
	public static readonly GraphicsDeviceType[] B_MAC_GRAPHICS_DEVICE_TYPES = new GraphicsDeviceType[] {
		GraphicsDeviceType.Metal, GraphicsDeviceType.OpenGLCore
	};

	// 윈도우즈
	public static readonly GraphicsDeviceType[] B_WINDOWS_GRAPHICS_DEVICE_TYPES = new GraphicsDeviceType[] {
#if DIRECT_3D_12_ENABLE
		GraphicsDeviceType.Direct3D12, 
#endif			// #if DIRECT_3D_12_ENABLE
		
		GraphicsDeviceType.Direct3D11
	};

	// iOS {
	public static readonly string B_IOS_ABSOLUTE_BUILD_PATH = string.Format("{0}../Builds/iOS", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS);

	public static readonly string B_IOS_SRC_PLUGIN_PATH = string.Format("{0}../PluginProjects/iOS/Classes/Plugin/", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS);
	public static readonly string B_IOS_DEST_PLUGIN_PATH = string.Format("{0}Plugins/iOS/CustomiOSPlugin/", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS);

	public static readonly GraphicsDeviceType[] B_IOS_DEVICE_GRAPHICS_DEVICE_TYPES = new GraphicsDeviceType[] {
		GraphicsDeviceType.Metal
	};

	public static readonly GraphicsDeviceType[] B_IOS_SIMULATOR_GRAPHICS_DEVICE_TYPES = new GraphicsDeviceType[] {
		GraphicsDeviceType.OpenGLES2
	};
	// iOS }

	// 안드로이드 {
	public static readonly string B_ANDROID_ABSOLUTE_BUILD_PATH_FORMAT = string.Format("{0}../Builds/Android/{1}", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS, "{0}");

	public static readonly string B_ANDROID_SRC_PLUGIN_PATH = string.Format("{0}../PluginProjects/Android/plugin/build/intermediates/packaged-classes/release/classes.jar", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS);
	public static readonly string B_ANDROID_DEST_PLUGIN_PATH = string.Format("{0}Plugins/Android/libs/CustomAndroidPlugin.jar", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS);

	public static readonly string B_ANDROID_SRC_MANIFEST_PATH = string.Format("{0}../Packages/Builds/Options/Android/AndroidManifest.xml", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS);
	public static readonly string B_ANDROID_DEST_MANIFEST_PATH = string.Format("{0}Plugins/Android/AndroidManifest.xml", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS);

	public static readonly string B_ANDROID_SRC_MAIN_TEMPLATE_PATH = string.Format("{0}../Packages/Builds/Options/Android/mainTemplate.gradle", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS);
	public static readonly string B_ANDROID_DEST_MAIN_TEMPLATE_PATH = string.Format("{0}Plugins/Android/mainTemplate.gradle", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS);

	public static readonly string B_ANDROID_SRC_PROGUARD_PATH = string.Format("{0}../Packages/Builds/Options/Android/proguard-user.txt", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS);
	public static readonly string B_ANDROID_DEST_PROGUARD_PATH = string.Format("{0}Plugins/Android/proguard-user.txt", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS);

	public static readonly string B_ANDROID_SRC_UNITY_PLUGIN_PATH = string.Format("{0}../PlaybackEngines/AndroidPlayer/Variations/il2cpp/Release/Classes/classes.jar", KEditorDefine.B_ABSOLUTE_DIR_PATH_UNITY_ENGINE);
	public static readonly string B_ANDROID_DEST_UNITY_PLUGIN_PATH = string.Format("{0}../PluginProjects/Android/plugin/libs/classes.jar", KEditorDefine.B_ABSOLUTE_DIR_PATH_ASSETS);

	public static readonly GraphicsDeviceType[] B_ANDROID_GRAPHICS_DEVICE_TYPES = new GraphicsDeviceType[] {
		GraphicsDeviceType.Vulkan, GraphicsDeviceType.OpenGLES3, GraphicsDeviceType.OpenGLES2
	};
	// 안드로이드 }
	#endregion			// 런타임 상수

	#region 조건부 상수
#if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	// 크기
	public const int B_FONT_SIZE_STATIC_TEXT = 24;
	public const int B_FONT_SIZE_DYNAMIC_TEXT = 24;

	// 이름
	public const string B_OBJ_NAME_STATIC_TEXT = "StaticInfoText";
	public const string B_OBJ_NAME_DYNAMIC_TEXT = "DynamicInfoText";
#endif			// #if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)

#if FILE_BROWSER_ENABLE
	// 비율
	public const float B_SCALE_FILE_BROWSER_WINDOW = 1.25f;

	// 이름
	public const string B_OBJ_NAME_FILE_BROWSER_WINDOW = "SimpleFileBrowserWindow";

	// 경로
	public const string B_OBJ_PATH_FILE_BROWSER_UI = "SimpleFileBrowserCanvas";
#endif			// #if FILE_BROWSER_ENABLE
	#endregion			// 조건부 상수

	#region 조건부 런타임 상수
#if UNITY_IOS
	// 경로
	public static readonly string B_PATH_CAPABILITY_ENTITLEMENTS_IOS = string.Format("{0}.entitlements", KAppDefine.G_NAME_PROJECT_ROOT);
#endif			// #if UNITY_IOS

#if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	// 위치
	public static readonly Vector2 B_POSITION_STATIC_TEXT = new Vector2(-10.0f, 0.0f);
	public static readonly Vector2 B_POSITION_DYNAMIC_TEXT = new Vector2(-10.0f, 140.0f);
#endif			// #if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	#endregion			// 조건부 런타임 상수
}
