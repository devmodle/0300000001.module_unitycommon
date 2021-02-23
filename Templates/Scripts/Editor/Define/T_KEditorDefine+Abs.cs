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
	public const float B_DELAY_DEFINE_S_UPDATE = 1.0f;

	// 유니티 패키지 {
	public const string B_UNITY_PKGS_N_KEY = "name";
	public const string B_UNITY_PKGS_SCOPED_REGISTRIES_KEY = "scopedRegistries";

	public const string B_UNITY_PKGS_ID_FMT = "{0}@{1}";
	public const string B_UNITY_PKGS_GOOGLE_REGISTRY_NAME = "Game Package Registry by Google";
	// 유니티 패키지 }
	#endregion			// 기본
	
	#region 런타임 상수
	// 스크립트 순서
	public static Dictionary<System.Type, int> B_SCRIPT_ORDERS = new Dictionary<System.Type, int>() {
		[typeof(CValueTable)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CStringTable)] = KCDefine.U_SCRIPT_O_SINGLETON,

		[typeof(CAppInfoStorage)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CUserInfoStorage)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CGameInfoStorage)] = KCDefine.U_SCRIPT_O_SINGLETON,

		[typeof(CCommonAppInfoStorage)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CCommonUserInfoStorage)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CCommonGameInfoStorage)] = KCDefine.U_SCRIPT_O_SINGLETON,

		[typeof(CUnityMsgSender)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CDeviceMsgReceiver)] = KCDefine.U_SCRIPT_O_SINGLETON,

		[typeof(CLogManager)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CSndManager)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CResManager)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CTaskManager)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CScheduleManager)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CNavStackManager)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CToastPopupManager)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CIndicatorManager)] = KCDefine.U_SCRIPT_O_SINGLETON,

		[typeof(CSampleSceneManager)] = KCDefine.U_SCRIPT_O_SCENE_MANAGER,
		
		[typeof(CSubInitSceneManager)] = KCDefine.U_SCRIPT_O_INIT_SCENE_MANAGER,
		[typeof(CSubSetupSceneManager)] = KCDefine.U_SCRIPT_O_SETUP_SCENE_MANAGER,
		[typeof(CSubStartSceneManager)] = KCDefine.U_SCRIPT_O_START_SCENE_MANAGER,
		[typeof(CSubLoadingSceneManager)] = KCDefine.U_SCRIPT_O_LOADING_SCENE_MANAGER,
		[typeof(CSubSplashSceneManager)] = KCDefine.U_SCRIPT_O_SPLASH_SCENE_MANAGER,
		[typeof(CSubAgreeSceneManager)] = KCDefine.U_SCRIPT_O_AGREE_SCENE_MANAGER,
		[typeof(CSubLateSetupSceneManager)] = KCDefine.U_SCRIPT_O_LATE_SETUP_SCENE_MANAGER,
		[typeof(CSubPermissionSceneManager)] = KCDefine.U_SCRIPT_O_PERMISSION_SCENE_MANAGER,
		[typeof(CSubIntroSceneManager)] = KCDefine.U_SCRIPT_O_SCENE_MANAGER,
		[typeof(CSubTitleSceneManager)] = KCDefine.U_SCRIPT_O_SCENE_MANAGER,

#if STUDY_MODULE_ENABLE
		[typeof(CSubMenuSceneManager)] = KCDefine.U_SCRIPT_O_SCENE_MANAGER,
#endif			// #if STUDY_MODULE_ENABLE

#if ADS_MODULE_ENABLE
		[typeof(CAdsManager)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CBannerAdsPosCorrector)] = KCDefine.U_SCRIPT_O_BANNER_ADS_CORRECTOR,
#endif			// #if ADS_MODULE_ENABLE

#if FLURRY_MODULE_ENABLE
		[typeof(CFlurryManager)] = KCDefine.U_SCRIPT_O_SINGLETON,
#endif			// #if FLURRY_MODULE_ENABLE

#if TENJIN_MODULE_ENABLE
		[typeof(CTenjinManager)] = KCDefine.U_SCRIPT_O_SINGLETON,
#endif			// #if TENJIN_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
		[typeof(CFacebookManager)] = KCDefine.U_SCRIPT_O_SINGLETON,
#endif			// #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
		[typeof(CFirebaseManager)] = KCDefine.U_SCRIPT_O_SINGLETON,
#endif			// #if FIREBASE_MODULE_ENABLE

#if SINGULAR_MODULE_ENABLE
		[typeof(CSingularManager)] = KCDefine.U_SCRIPT_O_SINGLETON,
#endif			// #if SINGULAR_MODULE_ENABLE

#if GAME_CENTER_MODULE_ENABLE
		[typeof(CGameCenterManager)] = KCDefine.U_SCRIPT_O_SINGLETON,
#endif			// #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
		[typeof(CPurchaseManager)] = KCDefine.U_SCRIPT_O_SINGLETON,
#endif			// #if PURCHASE_MODULE_ENABLE

#if NOTI_MODULE_ENABLE
		[typeof(CNotiManager)] = KCDefine.U_SCRIPT_O_SINGLETON
#endif			// #if NOTI_MODULE_ENABLE
	};

	// 데이터 타입
	public static readonly Dictionary<string, System.Type> B_SCENE_MANAGER_TYPE_LIST = new Dictionary<string, System.Type>() {
		[KCDefine.B_SCENE_N_INIT] = typeof(CSubInitSceneManager),
		[KCDefine.B_SCENE_N_SETUP] = typeof(CSubSetupSceneManager),
		[KCDefine.B_SCENE_N_START] = typeof(CSubStartSceneManager),
		[KCDefine.B_SCENE_N_LOADING] = typeof(CSubLoadingSceneManager),
		[KCDefine.B_SCENE_N_SPLASH] = typeof(CSubSplashSceneManager),
		[KCDefine.B_SCENE_N_AGREE] = typeof(CSubAgreeSceneManager),
		[KCDefine.B_SCENE_N_LATE_SETUP] = typeof(CSubLateSetupSceneManager),
		[KCDefine.B_SCENE_N_PERMISSION] = typeof(CSubPermissionSceneManager),
		[KCDefine.B_SCENE_N_INTRO] = typeof(CSubIntroSceneManager),
		[KCDefine.B_SCENE_N_TITLE] = typeof(CSubTitleSceneManager),

#if STUDY_MODULE_ENABLE
		[KCDefine.B_SCENE_N_MENU] = typeof(CSubMenuSceneManager)
#endif			// #if STUDY_MODULE_ENABLE
	};

	// 유니티 패키지 {
	public static readonly string B_UNITY_PKGS_SRC_GOOGLE_SCOPED_REGISTRY_PATH = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Templates/Options/Project/GoogleScopedRegistry.json";
	public static readonly string B_UNITY_PKGS_DEST_GOOGLE_SCOPED_REGISTRY_PATH = $"{KCEditorDefine.B_ABS_DIR_P_PACKAGES}Options/Project/GoogleScopedRegistry.json";

	public static readonly Dictionary<string, string> B_UNITY_PKGS_DEPENDENCY_LIST = new Dictionary<string, string>() {
		["com.google.external-dependency-manager"] = "1.2.164",
		["com.google.play.review"] = "1.3.0",

		["com.unity.burst"] = "1.4.4",
		["com.unity.2d.sprite"] = "1.0.0",
		["com.unity.mathematics"] = "1.2.1",
		["com.unity.quicksearch"] = "2.0.2",
		["com.unity.mobile.android-logcat"] = "1.2.1",
		["com.unity.render-pipelines.universal"] = "10.3.1",
		["com.unity.performance.profile-analyzer"] = "1.0.3",

#if ML_AGENTS_ENABLE
		["com.unity.ml-agents"] = "1.0.6",
#endif			// #if ML_AGENTS_ENABLE

#if CINEMACHINE_ENABLE
		["com.unity.cinemachine"] = "2.6.3",
#endif			// #if CINEMACHINE_ENABLE

#if VISUAL_FX_GRAPH_ENABLE
		["com.unity.visualeffectgraph"] = "10.3.1",
#endif			// #if VISUAL_FX_GRAPH_ENABLE

#if TILEMAP_2D_ENABLE
		["com.unity.2d.tilemap"] = "1.0.0",
#endif			// #if TILEMAP_2D_ENABLE

#if SPRITE_SHAPES_ENABLE
		["com.unity.2d.spriteshape"] = "5.1.1",
#endif			// #if SPRITE_SHAPES_ENABLE

#if PIXELS_PERFECT_ENABLE
		["com.unity.2d.pixel-perfect"] = "4.0.1",
#endif			// #if PIXELS_PERFECT_ENABLE

#if POLY_BRUSH_ENABLE
		["com.unity.polybrush"] = "1.0.2",
#endif			// #if POLY_BRUSH_ENABLE

#if PRO_BUILDER_ENABLE
		["com.unity.probuilder"] = "4.5.0",
#endif			// #if PRO_BUILDER_ENABLE

#if ASSET_BUNDLE_ENABLE
		["com.unity.addressables"] = "1.16.16",
#endif			// #if ASSET_BUNDLE_ENABLE

#if UNITY_RECORDER_ENABLE
		["com.unity.recorder"] = "2.5.4",
#endif			// #if UNITY_RECORDER_ENABLE

#if ADAPTIVE_PERFORMANCE_ENABLE
		["com.unity.adaptiveperformance"] = "2.0.2",
		["com.unity.adaptiveperformance.samsung.android"] = "2.0.2",
#endif			// #if ADAPTIVE_PERFORMANCE_ENABLE

#if INPUT_SYSTEM_ENABLE || INPUT_SYSTEM_MODULE_ENABLE
		["com.unity.inputsystem"] = "1.0.2",
#endif			// #if INPUT_SYSTEM_ENABLE || INPUT_SYSTEM_MODULE_ENABLE

#if SKELETON_2D_ANI_ENABLE || SKELETON_2D_ANI_MODULE_ENABLE
		["com.unity.2d.animation"] = "5.0.4",
		["com.unity.2d.psdimporter"] = "4.0.2",
		["com.unity.animation.rigging"] = "1.0.3",
#endif			// #if SKELETON_2D_ANI_ENABLE || SKELETON_2D_ANI_MODULE_ENABLE

#if POST_PROCESSING_ENABLE || UNITY_POST_PROCESSING_STACK_V2
    	["com.unity.postprocessing"] = "3.0.1",
#endif			// #if POST_PROCESSING_ENABLE || UNITY_POST_PROCESSING_STACK_V2

#if ADS_ENABLE || ADS_MODULE_ENABLE
		["unitymodule.common.ads"] = "https://9tap:NT9studio!@gitlab.com/9tapmodule.repository/unitymodule_common_ads_client.git#1.2.4",
#endif			// #if ADS_ENABLE || ADS_MODULE_ENABLE

#if FLURRY_ENABLE || FLURRY_MODULE_ENABLE
		["unitymodule.common.flurry"] = "https://9tap:NT9studio!@gitlab.com/9tapmodule.repository/unitymodule_common_flurry_client.git#1.2.4",
#endif			// #if FLURRY_ENABLE || FLURRY_MODULE_ENABLE

#if TENJIN_ENABLE || TENJIN_MODULE_ENABLE
		["unitymodule.common.tenjin"] = "https://9tap:NT9studio!@gitlab.com/9tapmodule.repository/unitymodule_common_tenjin_client.git#1.2.4",
#endif			// #if TENJIN_ENABLE || TENJIN_MODULE_ENABLE

#if FACEBOOK_ENABLE || FACEBOOK_MODULE_ENABLE
		["unitymodule.common.facebook"] = "https://9tap:NT9studio!@gitlab.com/9tapmodule.repository/unitymodule_common_facebook_client.git#1.2.4",
#endif			// #if FACEBOOK_ENABLE || FACEBOOK_MODULE_ENABLE

#if FIREBASE_ENABLE || FIREBASE_MODULE_ENABLE
		["com.google.firebase.auth"] = "7.1.0",
		["com.google.firebase.analytics"] = "7.1.0",
		["com.google.firebase.crashlytics"] = "7.1.0",
		["com.google.firebase.database"] = "7.1.0",
		["com.google.firebase.remote-config"] = "7.1.0",
		["com.google.firebase.messaging"] = "7.1.0",

		["unitymodule.common.firebase"] = "https://9tap:NT9studio!@gitlab.com/9tapmodule.repository/unitymodule_common_firebase_client.git#1.2.4",
#endif			// #if FIREBASE_ENABLE || FIREBASE_MODULE_ENABLE

#if SINGULAR_ENABLE || SINGULAR_MODULE_ENABLE
		["unitymodule.common.singular"] = "https://9tap:NT9studio!@gitlab.com/9tapmodule.repository/unitymodule_common_singular_client.git#1.2.4",
#endif			// #if SINGULAR_ENABLE || SINGULAR_MODULE_ENABLE

#if GAME_CENTER_ENABLE || GAME_CENTER_MODULE_ENABLE
		["unitymodule.common.gamecenter"] = "https://9tap:NT9studio!@gitlab.com/9tapmodule.repository/unitymodule_common_gamecenter_client.git#1.2.4",
#endif			// #if GAME_CENTER_ENABLE || GAME_CENTER_MODULE_ENABLE

#if PURCHASE_ENABLE || PURCHASE_MODULE_ENABLE
		["com.unity.purchasing"] = "2.2.2",
		["unitymodule.common.purchase"] = "https://9tap:NT9studio!@gitlab.com/9tapmodule.repository/unitymodule_common_purchase_client.git#1.2.4",
#endif			// #if PURCHASE_ENABLE || PURCHASE_MODULE_ENABLE

#if NOTI_ENABLE || NOTI_MODULE_ENABLE
		["com.unity.mobile.notifications"] = "1.3.2",
		["unitymodule.common.Noti"] = "https://9tap:NT9studio!@gitlab.com/9tapmodule.repository/unitymodule_common_Noti_client.git#1.2.4"
#endif			// #if NOTI_ENABLE || NOTI_MODULE_ENABLE
	};

	public static readonly Dictionary<string, string> B_UNITY_PKGS_SCOPED_REGISTRY_LIST = new Dictionary<string, string>() {
		[KEditorDefine.B_UNITY_PKGS_GOOGLE_REGISTRY_NAME] = KEditorDefine.B_UNITY_PKGS_DEST_GOOGLE_SCOPED_REGISTRY_PATH
	};
	// 유니티 패키지 }
	#endregion			// 런타임 상수

	#region 조건부 상수
#if UNITY_IOS
	// 암호화 여부
	public const bool B_IOS_ENCRYPTION_ENABLE = false;

	// 백 그라운드 옵션
	public const BackgroundModesOptions B_IOS_BACKGROUND_MODES_OPTS = BackgroundModesOptions.BackgroundFetch | BackgroundModesOptions.RemoteNotifications;
#endif			// #if UNITY_IOS
	#endregion			// 조건부 상수

	#region 조건부 런타임 상수
#if UNITY_IOS
	// 광고 네트워크 식별자
	public static readonly string[] B_IOS_ADS_NETWORK_IDS = new string[] {
#if ADS_MODULE_ENABLE
#if ADMOB_ENABLE || ADMOB_ADAPTER_ENABLE
		"cstr6suwn9.skadnetwork",
#endif			// #if ADMOB_ENABLE || ADMOB_ADAPTER_ENABLE

#if IRON_SRC_ENABLE || IRON_SRC_ADAPTER_ENABLE
		"su67r6k2v3.skadnetwork",
#endif			// #if IRON_SRC_ENABLE || IRON_SRC_ADAPTER_ENABLE

#if APP_LOVIN_ENABLE || APP_LOVIN_ADAPTER_ENABLE
		"2u9pt9hc89.skadnetwork",
		"4468km3ulz.skadnetwork",
		"4fzdc2evr5.skadnetwork",
		"7ug5zh24hu.skadnetwork",
		"8s468mfl3y.skadnetwork",
		"9rd848q2bz.skadnetwork",
		"9t245vhmpl.skadnetwork",
		"av6w8kgt66.skadnetwork",
		"f38h382jlk.skadnetwork",
		"hs6bdukanm.skadnetwork",
		"kbd757ywx3.skadnetwork",
		"ludvb6z3bs.skadnetwork",
		"m8dbw4sv7c.skadnetwork",
		"mlmmfzh3r3.skadnetwork",
		"prcb7njmu6.skadnetwork",
		"t38b2kh725.skadnetwork",
		"tl55sbb4fm.skadnetwork",
		"wzmmz9fp6w.skadnetwork",
		"yclnxrl5pm.skadnetwork",
		"ydx93a7ass.skadnetwork",
#endif			// #if APP_LOVIN_ENABLE || APP_LOVIN_ADAPTER_ENABLE

#if FACEBOOK_ADS_ADAPTER_ENABLE
		"n38lu8286q.skadnetwork",
		"v9wttpbfk9.skadnetwork",
#endif			// #if FACEBOOK_ADS_ADAPTER_ENABLE

#if UNITY_ADS_ADAPTER_ENABLE
		"22mmun2rn5.skadnetwork",
		"238da6jt44.skadnetwork",
		"24t9a8vw3c.skadnetwork",
		"2u9pt9hc89.skadnetwork",
		"3qy4746246.skadnetwork",
		"3rd42ekr43.skadnetwork",
		"3sh42y64q3.skadnetwork",
		"424m5254lk.skadnetwork",
		"4468km3ulz.skadnetwork",
		"44jx6755aq.skadnetwork",
		"44n7hlldy6.skadnetwork",
		"488r3q3dtq.skadnetwork",
		"4dzt52r2t5.skadnetwork",
		"4fzdc2evr5.skadnetwork",
		"4pfyvq9l8r.skadnetwork",
		"578prtvx9j.skadnetwork",
		"5a6flpkh64.skadnetwork",
		"5lm9lj6jb7.skadnetwork",
		"5tjdwbrq8w.skadnetwork",
		"7ug5zh24hu.skadnetwork",
		"8s468mfl3y.skadnetwork",
		"9rd848q2bz.skadnetwork",
		"9t245vhmpl.skadnetwork",
		"av6w8kgt66.skadnetwork",
		"bvpn9ufa9b.skadnetwork",
		"c6k4g5qg8m.skadnetwork",
		"cstr6suwn9.skadnetwork",
		"f38h382jlk.skadnetwork",
		"f73kdq92p3.skadnetwork",
		"g28c52eehv.skadnetwork",
		"glqzh8vgby.skadnetwork",
		"hs6bdukanm.skadnetwork",
		"kbd757ywx3.skadnetwork",
		"lr83yxwka7.skadnetwork",
		"m8dbw4sv7c.skadnetwork",
		"mlmmfzh3r3.skadnetwork",
		"ppxm28t8ap.skadnetwork",
		"prcb7njmu6.skadnetwork",
		"s39g8k73mm.skadnetwork",
		"t38b2kh725.skadnetwork",
		"tl55sbb4fm.skadnetwork",
		"v72qych5uu.skadnetwork",
		"v79kvwwj4g.skadnetwork",
		"wg4vff78zm.skadnetwork",
		"wzmmz9fp6w.skadnetwork",
		"yclnxrl5pm.skadnetwork",
		"ydx93a7ass.skadnetwork",
		"zmvfpc5aq8.skadnetwork",
#endif			// #if UNITY_ADS_ADAPTER_ENABLE

#if VUNGLE_ADAPTER_ENABLE
		"22mmun2rn5.skadnetwork",
		"2u9pt9hc89.skadnetwork",
		"3rd42ekr43.skadnetwork",
		"4fzdc2evr5.skadnetwork",
		"4pfyvq9l8r.skadnetwork",
		"5lm9lj6jb7.skadnetwork",
		"8s468mfl3y.skadnetwork",
		"c6k4g5qg8m.skadnetwork",
		"glqzh8vgby.skadnetwork",
		"gta9lk7p23.skadnetwork",
		"mlmmfzh3r3.skadnetwork",
		"n9x2a789qt.skadnetwork",
		"tl55sbb4fm.skadnetwork",
		"v72qych5uu.skadnetwork",
		"yclnxrl5pm.skadnetwork",
		"ydx93a7ass.skadnetwork",
#endif			// #if VUNGLE_ADAPTER_ENABLE

#if PANGLE_ADAPTER_ENABLE
		"22mmun2rn5.skadnetwork",
		"238da6jt44.skadnetwork",
#endif			// #if PANGLE_ADAPTER_ENABLE

#if AD_COLONY_ADAPTER_ENABLE
		"2u9pt9hc89.skadnetwork",
		"3rd42ekr43.skadnetwork",
		"4468km3ulz.skadnetwork",
		"44jx6755aq.skadnetwork",
		"4fzdc2evr5.skadnetwork",
		"4pfyvq9l8r.skadnetwork",
		"5lm9lj6jb7.skadnetwork",
		"7rz58n8ntl.skadnetwork",
		"7ug5zh24hu.skadnetwork",
		"8s468mfl3y.skadnetwork",
		"9rd848q2bz.skadnetwork",
		"9t245vhmpl.skadnetwork",
		"c6k4g5qg8m.skadnetwork",
		"ejvt5qm6ak.skadnetwork",
		"hs6bdukanm.skadnetwork",
		"klf5c3l5u5.skadnetwork",
		"m8dbw4sv7c.skadnetwork",
		"mlmmfzh3r3.skadnetwork",
		"mtkv5xtk9e.skadnetwork",
		"ppxm28t8ap.skadnetwork",
		"prcb7njmu6.skadnetwork",
		"t38b2kh725.skadnetwork",
		"tl55sbb4fm.skadnetwork",
		"v72qych5uu.skadnetwork",
		"yclnxrl5pm.skadnetwork"
#endif			// #if AD_COLONY_ADAPTER_ENABLE
#endif			// #if ADS_MODULE_ENABLE
	};

	// 프레임워크
	public static readonly string[] B_IOS_EXTRA_FRAMEWORKS = new string[] {
		"GameKit.framework",
		"AuthenticationServices.framework",

#if TENJIN_MODULE_ENABLE
		"iAd.framework",
		"StoreKit.framework",
		"AdSupport.framework",
#endif			// #if TENJIN_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE && FIREBASE_CLOUD_MSG_ENABLE
		"UserNotifications.framework",
#endif			// #if FIREBASE_MODULE_ENABLE && FIREBASE_CLOUD_MSG_ENABLE

#if SINGULAR_MODULE_ENABLE
		"Security.framework",
		"SystemConfiguration.framework",
		"iAd.framework",
		"AdSupport.framework",
		"WebKit.framework",
		"libsqlite3.0.tbd",
		"libz.tbd",
#endif			// #if SINGULAR_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
		"StoreKit.framework"
#endif			// #if PURCHASE_MODULE_ENABLE
	};

	// 호환성 타입
	public static readonly PBXCapabilityType[] B_IOS_EXTRA_CAPABILITY_TYPES = new PBXCapabilityType[] {
#if APPLE_LOGIN_ENABLE
		PBXCapabilityType.SignInWithApple,
#endif			// #if APPLE_LOGIN_ENABLE

#if FIREBASE_MODULE_ENABLE && FIREBASE_CLOUD_MSG_ENABLE
		PBXCapabilityType.BackgroundModes,
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
