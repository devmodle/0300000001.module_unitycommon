using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
#if UNITY_EDITOR
using UnityEditor;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif			// #if UNITY_IOS

//! 에디터 상수
public static partial class KEditorDefine {
	#region 기본
	// 시간
	public const float B_DELAY_DEFINE_S_UPDATE = 1.5f;

	// 유니티 패키지 {
	public const string B_UNITY_PKGS_NAME_KEY = "name";
	public const string B_UNITY_PKGS_SCOPED_REGISTRIES_KEY = "scopedRegistries";

	public const string B_UNITY_PKGS_ID_FORMAT = "{0}@{1}";
	public const string B_UNITY_PKGS_GOOGLE_REGISTRY_NAME = "Game Package Registry by Google";
	// 유니티 패키지 }
	#endregion			// 기본
	
	#region 런타임 상수
	// 스크립트 순서
	public static Dictionary<System.Type, int> B_SCRIPT_ORDERS = new Dictionary<System.Type, int>() {
		[typeof(CValueTable)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CStringTable)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,

		[typeof(CAppInfoStorage)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CUserInfoStorage)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CGameInfoStorage)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,

		[typeof(CCommonAppInfoStorage)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CCommonUserInfoStorage)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,

		[typeof(CUnityMsgSender)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CDeviceMsgReceiver)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,

		[typeof(CLogManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CSndManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CResManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CTaskManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CScheduleManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CNavStackManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CToastPopupManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CActivityIndicatorManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,

		[typeof(CSampleSceneManager)] = KCDefine.U_SCRIPT_ORDER_SCENE_MANAGER,
		
		[typeof(CSubInitSceneManager)] = KCDefine.U_SCRIPT_ORDER_INIT_SCENE_MANAGER,
		[typeof(CSubSetupSceneManager)] = KCDefine.U_SCRIPT_ORDER_SETUP_SCENE_MANAGER,
		[typeof(CSubStartSceneManager)] = KCDefine.U_SCRIPT_ORDER_START_SCENE_MANAGER,
		[typeof(CSubLoadingSceneManager)] = KCDefine.U_SCRIPT_ORDER_LOADING_SCENE_MANAGER,
		[typeof(CSubSplashSceneManager)] = KCDefine.U_SCRIPT_ORDER_SPLASH_SCENE_MANAGER,
		[typeof(CSubAgreeSceneManager)] = KCDefine.U_SCRIPT_ORDER_AGREE_SCENE_MANAGER,
		[typeof(CSubLateSetupSceneManager)] = KCDefine.U_SCRIPT_ORDER_LATE_SETUP_SCENE_MANAGER,
		[typeof(CSubIntroSceneManager)] = KCDefine.U_SCRIPT_ORDER_SCENE_MANAGER,

#if STUDY_MODULE_ENABLE
		[typeof(CMenuSceneManager)] = KCDefine.U_SCRIPT_ORDER_SCENE_MANAGER,
#endif			// #if STUDY_MODULE_ENABLE

#if ADS_MODULE_ENABLE
		[typeof(CAdsManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CBannerAdsPosCorrector)] = KCDefine.U_SCRIPT_ORDER_BANNER_ADS_CORRECTOR,
#endif			// #if ADS_MODULE_ENABLE

#if FLURRY_MODULE_ENABLE
		[typeof(CFlurryManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
#endif			// #if FLURRY_MODULE_ENABLE

#if TENJIN_MODULE_ENABLE
		[typeof(CTenjinManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
#endif			// #if TENJIN_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
		[typeof(CFacebookManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
#endif			// #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
		[typeof(CFirebaseManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
#endif			// #if FIREBASE_MODULE_ENABLE

#if UNITY_SERVICE_MODULE_ENABLE
		[typeof(CUnityServiceManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
#endif			// #if UNITY_SERVICE_MODULE_ENABLE

#if SINGULAR_MODULE_ENABLE
		[typeof(CSingularManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
#endif			// #if SINGULAR_MODULE_ENABLE

#if GAME_CENTER_MODULE_ENABLE
		[typeof(CGameCenterManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
#endif			// #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
		[typeof(CPurchaseManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
#endif			// #if PURCHASE_MODULE_ENABLE

#if NOTI_MODULE_ENABLE
		[typeof(CNotiManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON
#endif			// #if NOTI_MODULE_ENABLE
	};

	// 데이터 타입
	public static readonly Dictionary<string, System.Type> B_SCENE_MANAGER_TYPE_LIST = new Dictionary<string, System.Type>() {
		[KCDefine.B_SCENE_NAME_INIT] = typeof(CSubInitSceneManager),
		[KCDefine.B_SCENE_NAME_SETUP] = typeof(CSubSetupSceneManager),
		[KCDefine.B_SCENE_NAME_START] = typeof(CSubStartSceneManager),
		[KCDefine.B_SCENE_NAME_LOADING] = typeof(CSubLoadingSceneManager),
		[KCDefine.B_SCENE_NAME_SPLASH] = typeof(CSubSplashSceneManager),
		[KCDefine.B_SCENE_NAME_AGREE] = typeof(CSubAgreeSceneManager),
		[KCDefine.B_SCENE_NAME_LATE_SETUP] = typeof(CSubLateSetupSceneManager),
		[KCDefine.B_SCENE_NAME_INTRO] = typeof(CSubIntroSceneManager),

#if STUDY_MODULE_ENABLE
		[KSDefine.B_SCENE_NAME_MENU] = typeof(CMenuSceneManager)
#endif			// #if STUDY_MODULE_ENABLE
	};

	// 유니티 패키지 {
	public static readonly string B_UNITY_PKG_SRC_GOOGLE_SCOPED_REGISTRY_PATH = string.Format("{0}../UnityPackages/Options/Project/GoogleScopedRegistry.json", KCEditorDefine.B_ABS_DIR_PATH_ASSETS);
	public static readonly string B_UNITY_PKG_DEST_GOOGLE_SCOPED_REGISTRY_PATH = string.Format("{0}Options/Project/GoogleScopedRegistry.json", KCEditorDefine.B_ABS_DIR_PATH_PACKAGES);

	public static readonly Dictionary<string, string> B_UNITY_PKGS_DEPENDENCY_LIST = new Dictionary<string, string>() {
		["com.google.external-dependency-manager"] = "1.2.161",
		["com.google.play.review"] = "1.3.0",

		["com.unity.2d.sprite"] = "1.0.0",
		["com.unity.2d.tilemap"] = "1.0.0",
		["com.unity.assetbundlebrowser"] = "1.7.0",
		["com.unity.mobile.android-logcat"] = "1.2.0",
		["com.unity.render-pipelines.universal"] = "8.2.0",

#if ML_AGENTS_ENABLE
		["com.unity.ml-agents"] = "1.0.5",
#endif			// #if ML_AGENTS_ENABLE

#if CINEMACHINE_ENABLE
		["com.unity.cinemachine"] = "2.6.3",
#endif			// #if CINEMACHINE_ENABLE

#if VISUAL_FX_GRAPH_ENABLE
		["com.unity.visualeffectgraph"] = "8.2.0",
#endif			// #if VISUAL_FX_GRAPH_ENABLE

#if POST_PROCESSING_ENABLE || UNITY_POST_PROCESSING_STACK_V2
    	["com.unity.postprocessing"] = "2.3.0",
#endif			// #if POST_PROCESSING_ENABLE || UNITY_POST_PROCESSING_STACK_V2

#if ADS_ENABLE || ADS_MODULE_ENABLE
		["unitymodule.common.ads"] = "https://sd.lee:NSString132!@gitlab.com/9tapmodule.repository/unitymodule_common_ads_client.git#1.0.2",
#endif			// #if ADS_ENABLE || ADS_MODULE_ENABLE

#if FLURRY_ENABLE || FLURRY_MODULE_ENABLE
		["unitymodule.common.flurry"] = "https://sd.lee:NSString132!@gitlab.com/9tapmodule.repository/unitymodule_common_flurry_client.git#1.0.2",
#endif			// #if FLURRY_ENABLE || FLURRY_MODULE_ENABLE

#if TENJIN_ENABLE || TENJIN_MODULE_ENABLE
		["unitymodule.common.tenjin"] = "https://sd.lee:NSString132!@gitlab.com/9tapmodule.repository/unitymodule_common_tenjin_client.git#1.0.2",
#endif			// #if TENJIN_ENABLE || TENJIN_MODULE_ENABLE

#if FACEBOOK_ENABLE || FACEBOOK_MODULE_ENABLE
		["unitymodule.common.facebook"] = "https://sd.lee:NSString132!@gitlab.com/9tapmodule.repository/unitymodule_common_facebook_client.git#1.0.2",
#endif			// #if FACEBOOK_ENABLE || FACEBOOK_MODULE_ENABLE

#if FIREBASE_ENABLE || FIREBASE_MODULE_ENABLE
		["com.google.firebase.auth"] = "6.16.0",
		["com.google.firebase.analytics"] = "6.16.0",
		["com.google.firebase.crashlytics"] = "6.16.0",
		["com.google.firebase.database"] = "6.16.0",
		["com.google.firebase.remote-config"] = "6.16.0",
		["com.google.firebase.messaging"] = "6.16.0",

		["unitymodule.common.firebase"] = "https://sd.lee:NSString132!@gitlab.com/9tapmodule.repository/unitymodule_common_firebase_client.git#1.0.2",
#endif			// #if FIREBASE_ENABLE || FIREBASE_MODULE_ENABLE

#if UNITY_SERVICE_ENABLE || UNITY_SERVICE_MODULE_ENABLE
		["unitymodule.common.unityservice"] = "https://sd.lee:NSString132!@gitlab.com/9tapmodule.repository/unitymodule_common_unityservice_client.git#1.0.2",
#endif			// #if UNITY_SERVICE_ENABLE || UNITY_SERVICE_MODULE_ENABLE

#if SINGULAR_ENABLE || SINGULAR_MODULE_ENABLE
		["unitymodule.common.singular"] = "https://sd.lee:NSString132!@gitlab.com/9tapmodule.repository/unitymodule_common_singular_client.git#1.0.2",
#endif			// #if SINGULAR_ENABLE || SINGULAR_MODULE_ENABLE

#if GAME_CENTER_ENABLE || GAME_CENTER_MODULE_ENABLE
		["unitymodule.common.gamecenter"] = "https://sd.lee:NSString132!@gitlab.com/9tapmodule.repository/unitymodule_common_gamecenter_client.git#1.0.2",
#endif			// #if GAME_CENTER_ENABLE || GAME_CENTER_MODULE_ENABLE

#if PURCHASE_ENABLE || PURCHASE_MODULE_ENABLE
		["com.unity.purchasing"] = "2.1.1",
		["com.unity.purchasing.udp"] = "1.2.0",

		["unitymodule.common.purchase"] = "https://sd.lee:NSString132!@gitlab.com/9tapmodule.repository/unitymodule_common_purchase_client.git#1.0.2",
#endif			// #if PURCHASE_ENABLE || PURCHASE_MODULE_ENABLE

#if NOTI_ENABLE || NOTI_MODULE_ENABLE
		["com.unity.mobile.notifications"] = "1.0.3",
		["unitymodule.common.Noti"] = "https://sd.lee:NSString132!@gitlab.com/9tapmodule.repository/unitymodule_common_Noti_client.git#1.0.2"
#endif			// #if NOTI_ENABLE || NOTI_MODULE_ENABLE
	};

	public static readonly Dictionary<string, string> B_UNITY_PKGS_SCOPED_REGISTRY_LIST = new Dictionary<string, string>() {
		[KEditorDefine.B_UNITY_PKGS_GOOGLE_REGISTRY_NAME] = KEditorDefine.B_UNITY_PKG_DEST_GOOGLE_SCOPED_REGISTRY_PATH
	};
	// 유니티 패키지 }
	#endregion			// 런타임 상수

	#region 조건부 런타임 상수
#if UNITY_IOS
	// 프레임워크
	public static readonly string[] B_EXTRA_FRAMEWORKS_IOS = new string[] {
#if TENJIN_MODULE_ENABLE
		"iAd.framework",
		"StoreKit.framework",
		"AdSupport.framework",
#endif			// #if TENJIN_MODULE_ENABLE

#if GAME_CENTER_MODULE_ENABLE
		"GameKit.framework",
#endif			// #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
		"StoreKit.framework"
#endif			// #if PURCHASE_MODULE_ENABLE
	};

	// 호환성 타입
	public static readonly PBXCapabilityType[] B_EXTRA_CAPABILITY_TYPES_IOS = new PBXCapabilityType[] {
#if FIREBASE_MODULE_ENABLE && FIREBASE_CLOUD_MSG_ENABLE
		PBXCapabilityType.PushNotifications,
#endif			// #if FIREBASE_MODULE_ENABLE && FIREBASE_CLOUD_MSG_ENABLE

#if GAME_CENTER_MODULE_ENABLE
		PBXCapabilityType.GameCenter,
#endif			// #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
		PBXCapabilityType.InAppPurchase
#endif			// #if PURCHASE_MODULE_ENABLE
	};
#endif			// #if UNITY_IOS
	#endregion			// 조건부 런타임 상수
}
#endif			// #if UNITY_EDITOR
#endif			// #if NEVER_USE_THIS
