using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if UNITY_EDITOR
using UnityEditor;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif			// #if UNITY_IOS

/** 에디터 상수 */
public static partial class KEditorDefine {
	#region 기본
	// 시간
	public const float B_DELAY_DEFINE_S_UPDATE = 1.0f;
	
	// 유니티 패키지 {
	public const string B_UNITY_PKGS_N_KEY = "name";
	public const string B_UNITY_PKGS_SCOPED_REGISTRY_DICT_KEY = "scopedRegistries";

	public const string B_UNITY_PKGS_ID_FMT = "{0}@{1}";
	// 유니티 패키지 }
	#endregion			// 기본
	
	#region 런타임 상수
	// 스크립트 순서
	public static Dictionary<System.Type, int> B_SCRIPT_ORDER_DICT = new Dictionary<System.Type, int>() {
		[typeof(CValTable)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CStrTable)] = KCDefine.U_SCRIPT_O_SINGLETON,

		[typeof(CSndManager)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CResManager)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CTaskManager)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CScheduleManager)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CNavStackManager)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CIndicatorManager)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CCollectionManager)] = KCDefine.U_SCRIPT_O_SINGLETON,

		[typeof(CUnityMsgSender)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CDeviceMsgReceiver)] = KCDefine.U_SCRIPT_O_SINGLETON,

#if NEWTON_SOFT_JSON_MODULE_ENABLE
		[typeof(CCommonAppInfoStorage)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CCommonUserInfoStorage)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CCommonGameInfoStorage)] = KCDefine.U_SCRIPT_O_SINGLETON,
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE

#if SCENE_TEMPLATES_MODULE_ENABLE
		[typeof(CSubInitSceneManager)] = KCDefine.U_SCRIPT_O_INIT_SCENE_MANAGER,
		[typeof(CSubSetupSceneManager)] = KCDefine.U_SCRIPT_O_SETUP_SCENE_MANAGER,
		[typeof(CSubAgreeSceneManager)] = KCDefine.U_SCRIPT_O_AGREE_SCENE_MANAGER,
		[typeof(CSubLateSetupSceneManager)] = KCDefine.U_SCRIPT_O_LATE_SETUP_SCENE_MANAGER,
		[typeof(CSubStartSceneManager)] = KCDefine.U_SCRIPT_O_START_SCENE_MANAGER,
		[typeof(CSubSplashSceneManager)] = KCDefine.U_SCRIPT_O_SPLASH_SCENE_MANAGER,
		[typeof(CSubIntroSceneManager)] = KCDefine.U_SCRIPT_O_SCENE_MANAGER,

#if STUDY_MODULE_ENABLE
		[typeof(CSubMenuSceneManager)] = KCDefine.U_SCRIPT_O_SCENE_MANAGER,
#endif			// #if STUDY_MODULE_ENABLE
#endif			// #if SCENE_TEMPLATES_MODULE_ENABLE

#if RUNTIME_TEMPLATES_MODULE_ENABLE
		[typeof(CLevelInfoTable)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CSaleItemInfoTable)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CSaleProductInfoTable)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CMissionInfoTable)] = KCDefine.U_SCRIPT_O_SINGLETON,
		
		[typeof(CRewardInfoTable)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CEpisodeInfoTable)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CTutorialInfoTable)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CFXInfoTable)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CBlockInfoTable)] = KCDefine.U_SCRIPT_O_SINGLETON,

		[typeof(CAppInfoStorage)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CUserInfoStorage)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CGameInfoStorage)] = KCDefine.U_SCRIPT_O_SINGLETON,
		
		[typeof(CSubTitleSceneManager)] = KCDefine.U_SCRIPT_O_SCENE_MANAGER,
		[typeof(CSubMainSceneManager)] = KCDefine.U_SCRIPT_O_SCENE_MANAGER,
		[typeof(CSubGameSceneManager)] = KCDefine.U_SCRIPT_O_SCENE_MANAGER,
		[typeof(CSubLoadingSceneManager)] = KCDefine.U_SCRIPT_O_LOADING_SCENE_MANAGER,
		[typeof(CSubOverlaySceneManager)] = KCDefine.U_SCRIPT_O_OVERLAY_SCENE_MANAGER,
		[typeof(CSubTestSceneManager)] = KCDefine.U_SCRIPT_O_SCENE_MANAGER,
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE

#if UNITY_STANDALONE && EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
		[typeof(CSubLevelEditorSceneManager)] = KCDefine.U_SCRIPT_O_SCENE_MANAGER,
#endif			// #if UNITY_STANDALONE && EDITOR_SCENE_TEMPLATES_MODULE_ENABLE

#if ADS_MODULE_ENABLE
		[typeof(CAdsManager)] = KCDefine.U_SCRIPT_O_SINGLETON,
		[typeof(CBannerAdsPosCorrector)] = KCDefine.U_SCRIPT_O_ADS_CORRECTOR,
		[typeof(CBannerAdsSizeCorrector)] = KCDefine.U_SCRIPT_O_ADS_CORRECTOR,
		[typeof(CRewardAdsTouchInteractable)] = KCDefine.U_SCRIPT_O_ADS_INTERACTABLE,
#endif			// #if ADS_MODULE_ENABLE

#if FLURRY_MODULE_ENABLE
		[typeof(CFlurryManager)] = KCDefine.U_SCRIPT_O_SINGLETON,
#endif			// #if FLURRY_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
		[typeof(CFacebookManager)] = KCDefine.U_SCRIPT_O_SINGLETON,
#endif			// #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
		[typeof(CFirebaseManager)] = KCDefine.U_SCRIPT_O_SINGLETON,
#endif			// #if FIREBASE_MODULE_ENABLE

#if APPS_FLYER_MODULE_ENABLE
		[typeof(CAppsFlyerManager)] = KCDefine.U_SCRIPT_O_SINGLETON,
#endif			// #if APPS_FLYER_MODULE_ENABLE

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
	public static readonly Dictionary<string, System.Type> B_SCENE_MANAGER_TYPE_DICT = new Dictionary<string, System.Type>() {
#if SCENE_TEMPLATES_MODULE_ENABLE
		[KCDefine.B_SCENE_N_INIT] = typeof(CSubInitSceneManager),
		[KCDefine.B_SCENE_N_SETUP] = typeof(CSubSetupSceneManager),
		[KCDefine.B_SCENE_N_AGREE] = typeof(CSubAgreeSceneManager),
		[KCDefine.B_SCENE_N_LATE_SETUP] = typeof(CSubLateSetupSceneManager),

		[KCDefine.B_SCENE_N_START] = typeof(CSubStartSceneManager),
		[KCDefine.B_SCENE_N_SPLASH] = typeof(CSubSplashSceneManager),
		[KCDefine.B_SCENE_N_INTRO] = typeof(CSubIntroSceneManager),

#if STUDY_MODULE_ENABLE
		[KCDefine.B_SCENE_N_MENU] = typeof(CSubMenuSceneManager),
#endif			// #if STUDY_MODULE_ENABLE
#endif			// #if SCENE_TEMPLATES_MODULE_ENABLE
		
#if RUNTIME_TEMPLATES_MODULE_ENABLE
		[KCDefine.B_SCENE_N_TITLE] = typeof(CSubTitleSceneManager),
		[KCDefine.B_SCENE_N_MAIN] = typeof(CSubMainSceneManager),
		[KCDefine.B_SCENE_N_GAME] = typeof(CSubGameSceneManager),
		[KCDefine.B_SCENE_N_LOADING] = typeof(CSubLoadingSceneManager),
		[KCDefine.B_SCENE_N_OVERLAY] = typeof(CSubOverlaySceneManager),
		[KCDefine.B_SCENE_N_TEST] = typeof(CSubTestSceneManager),
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE

#if UNITY_STANDALONE && EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
		[KCDefine.B_SCENE_N_LEVEL_EDITOR] = typeof(CSubLevelEditorSceneManager)
#endif			// #if UNITY_STANDALONE && EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
	};

	// 유니티 패키지 {
	public static readonly Dictionary<string, string> B_UNITY_PKGS_DEPENDENCY_DICT = new Dictionary<string, string>() {
		// 기본
		["com.unity.2d.sprite"] = "1.0.0",
		["com.unity.localization"] = "1.0.5",
		["com.unity.terrain-tools"] = "4.0.3",
		["com.unity.2d.spriteshape"] = "7.0.4",
		["com.unity.2d.tilemap.extras"] = "2.2.1",
		["com.unity.animation.rigging"] = "1.1.1",

#if SAMPLE_PROJ || DEVELOPMENT_PROJ
		// 분석
		["com.unity.mobile.android-logcat"] = "1.2.3",
		["com.unity.performance.profile-analyzer"] = "1.1.1",

		// 기타
		["com.unity.sequences"] = "1.0.4",
		["com.unity.polybrush"] = "1.1.2",
		["com.unity.probuilder"] = "5.0.4",
		["com.unity.formats.fbx"] = "4.1.2",
		["com.unity.ads.ios-support"] = "1.0.0",

#if ML_AGENTS_ENABLE || ML_AGENTS_MODULE_ENABLE
		["com.unity.ml-agents"] = "2.0.1",
#endif			// #if ML_AGENTS_ENABLE || ML_AGENTS_MODULE_ENABLE

#if CINEMACHINE_ENABLE || CINEMACHINE_MODULE_ENABLE
		["com.unity.cinemachine"] = "2.8.4",
#endif			// #if CINEMACHINE_ENABLE || CINEMACHINE_MODULE_ENABLE

#if INPUT_SYSTEM_ENABLE || INPUT_SYSTEM_MODULE_ENABLE
		["com.unity.inputsystem"] = "1.3.0",
#endif			// #if INPUT_SYSTEM_ENABLE || INPUT_SYSTEM_MODULE_ENABLE

#if POST_PROCESSING_ENABLE || POST_PROCESSING_MODULE_ENABLE
    	["com.unity.postprocessing"] = "3.2.1",
#endif			// #if POST_PROCESSING_ENABLE || POST_PROCESSING_MODULE_ENABLE

#if UNIVERSAL_RENDERING_PIPELINE_ENABLE || UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE
		["com.unity.visualeffectgraph"] = "12.1.5",
		["com.unity.render-pipelines.universal"] = "12.1.5",
#endif			// #if UNIVERSAL_RENDERING_PIPELINE_ENABLE || UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE

#if ANIMATION_2D_ENABLE || ANIMATION_2D_MODULE_ENABLE
		["com.unity.2d.psdimporter"] = "6.0.3",
#endif			// #if ANIMATION_2D_ENABLE || ANIMATION_2D_MODULE_ENABLE

#if ADS_ENABLE || ADS_MODULE_ENABLE
		["module.unitycommonads"] = "https://9tap:NT9studio!@gitlab.com/9tapmodule.repository/03000001.module_unitycommonads_client.git#2.1.6",
#endif			// #if ADS_ENABLE || ADS_MODULE_ENABLE

#if FLURRY_ENABLE || FLURRY_MODULE_ENABLE
		["module.unitycommonflurry"] = "https://9tap:NT9studio!@gitlab.com/9tapmodule.repository/03000001.module_unitycommonflurry_client.git#2.1.6",
#endif			// #if FLURRY_ENABLE || FLURRY_MODULE_ENABLE

#if FACEBOOK_ENABLE || FACEBOOK_MODULE_ENABLE
		["module.unitycommonfacebook"] = "https://9tap:NT9studio!@gitlab.com/9tapmodule.repository/03000001.module_unitycommonfacebook_client.git#2.1.6",
#endif			// #if FACEBOOK_ENABLE || FACEBOOK_MODULE_ENABLE

#if FIREBASE_ENABLE || FIREBASE_MODULE_ENABLE
		["module.unitycommonfirebase"] = "https://9tap:NT9studio!@gitlab.com/9tapmodule.repository/03000001.module_unitycommonfirebase_client.git#2.1.6",
#endif			// #if FIREBASE_ENABLE || FIREBASE_MODULE_ENABLE

#if APPS_FLYER_ENABLE || APPS_FLYER_MODULE_ENABLE
		["module.unitycommonappsflyer"] = "https://9tap:NT9studio!@gitlab.com/9tapmodule.repository/03000001.module_unitycommonappsflyer_client.git#2.1.6",
#endif			// #if APPS_FLYER_ENABLE || APPS_FLYER_MODULE_ENABLE

#if GAME_CENTER_ENABLE || GAME_CENTER_MODULE_ENABLE
		["module.unitycommongamecenter"] = "https://9tap:NT9studio!@gitlab.com/9tapmodule.repository/03000001.module_unitycommongamecenter_client.git#2.1.6",
#endif			// #if GAME_CENTER_ENABLE || GAME_CENTER_MODULE_ENABLE

#if PURCHASE_ENABLE || PURCHASE_MODULE_ENABLE
		["com.unity.purchasing"] = "4.1.3",
		["module.unitycommonpurchase"] = "https://9tap:NT9studio!@gitlab.com/9tapmodule.repository/03000001.module_unitycommonpurchase_client.git#2.1.6",
#endif			// #if PURCHASE_ENABLE || PURCHASE_MODULE_ENABLE

#if NOTI_ENABLE || NOTI_MODULE_ENABLE
		["com.unity.mobile.notifications"] = "2.0.0",
		["module.unitycommon.Noti"] = "https://9tap:NT9studio!@gitlab.com/9tapmodule.repository/03000001.module_unitycommon_Noti_client.git#2.1.6"
#endif			// #if NOTI_ENABLE || NOTI_MODULE_ENABLE
#endif			// #if SAMPLE_PROJ || DEVELOPMENT_PROJ
	};

	public static readonly Dictionary<string, string> B_UNITY_PKGS_SCOPED_REGISTRY_DICT = new Dictionary<string, string>() {
		// Do Something
	};
	// 유니티 패키지 }
	#endregion			// 런타임 상수

	#region 조건부 상수
#if UNITY_IOS
	// 프로퍼티 속성
	public const bool B_IOS_ENCRYPTION_ENABLE = false;
	public const string B_IOS_USER_TRACKING_USAGE_DESC = "∙ Special offers and promotions just for you\n∙ Advertisements that match your interests\n∙ An improved personalized experience over time";
#endif			// #if UNITY_IOS
	#endregion			// 조건부 상수

	#region 조건부 런타임 상수
#if UNITY_IOS
	// 광고 네트워크 식별자
	public static readonly List<string> B_IOS_ADS_NETWORK_ID_LIST = new List<string>() {
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
		"7rz58n8ntl.skadnetwork",
		"7ug5zh24hu.skadnetwork",
		"8s468mfl3y.skadnetwork",
		"9rd848q2bz.skadnetwork",
		"9t245vhmpl.skadnetwork",
		"av6w8kgt66.skadnetwork",
		"bvpn9ufa9b.skadnetwork",
		"c6k4g5qg8m.skadnetwork",
		"cstr6suwn9.skadnetwork",
		"ejvt5qm6ak.skadnetwork",
		"f38h382jlk.skadnetwork",
		"f73kdq92p3.skadnetwork",
		"g28c52eehv.skadnetwork",
		"glqzh8vgby.skadnetwork",
		"gta9lk7p23.skadnetwork",
		"hs6bdukanm.skadnetwork",
		"kbd757ywx3.skadnetwork",
		"klf5c3l5u5.skadnetwork",
		"lr83yxwka7.skadnetwork",
		"ludvb6z3bs.skadnetwork",
		"m8dbw4sv7c.skadnetwork",
		"mlmmfzh3r3.skadnetwork",
		"mtkv5xtk9e.skadnetwork",
		"n38lu8286q.skadnetwork",
		"n9x2a789qt.skadnetwork",
		"ppxm28t8ap.skadnetwork",
		"prcb7njmu6.skadnetwork",
		"s39g8k73mm.skadnetwork",
		"su67r6k2v3.skadnetwork",
		"t38b2kh725.skadnetwork",
		"tl55sbb4fm.skadnetwork",
		"v72qych5uu.skadnetwork",
		"v79kvwwj4g.skadnetwork",
		"v9wttpbfk9.skadnetwork",
		"wg4vff78zm.skadnetwork",
		"wzmmz9fp6w.skadnetwork",
		"yclnxrl5pm.skadnetwork",
		"ydx93a7ass.skadnetwork",
		"zmvfpc5aq8.skadnetwork",
		"mp6xlyr22a.skadnetwork",
		"275upjj5gd.skadnetwork",
		"6g9af3uyq4.skadnetwork",
		"9nlqeag3gk.skadnetwork",
		"cg4yq2srnc.skadnetwork",
		"qqp299437r.skadnetwork",
		"rx5hdcabgc.skadnetwork",
		"u679fj5vs4.skadnetwork",
		"uw77j35x4d.skadnetwork",
		"2fnua5tdw4.skadnetwork",
		"3qcr597p9d.skadnetwork",
		"e5fvkxwrpn.skadnetwork",
		"ecpz2srf59.skadnetwork",
		"hjevpa356n.skadnetwork",
		"k674qkevps.skadnetwork",
		"n6fk4nfna4.skadnetwork",
		"p78axxw29g.skadnetwork",
		"y2ed4ez56y.skadnetwork",
		"zq492l623r.skadnetwork",
		"32z4fx6l9h.skadnetwork",
		"523jb4fst2.skadnetwork",
		"54nzkqm89y.skadnetwork",
		"5l3tpt7t6e.skadnetwork",
		"6xzpu9s2p8.skadnetwork",
		"79pbpufp6p.skadnetwork",
		"9b89h5y424.skadnetwork",
		"cj5566h2ga.skadnetwork",
		"feyaarzu9v.skadnetwork",
		"ggvn48r87g.skadnetwork",
		"pwa73g5rt2.skadnetwork",
		"xy9t38ct57.skadnetwork",
		"4w7y6s5ca2.skadnetwork",
		"737z793b9f.skadnetwork",
		"dzg6xy7pwj.skadnetwork",
		"hdw39hrw9y.skadnetwork",
		"mls7yz5dvl.skadnetwork",
		"w9q455wk68.skadnetwork",
		"x44k69ngh6.skadnetwork",
		"y45688jllp.skadnetwork",
		"252b5q8x7y.skadnetwork",
		"9g2aggbj52.skadnetwork",
		"nu4557a4je.skadnetwork",
		"v4nxqhlyqp.skadnetwork",
		"r26jy69rpl.skadnetwork",
		"eh6m2bh4zr.skadnetwork",
		"8m87ys6875.skadnetwork",
		"97r2b46745.skadnetwork",
		"52fl2v3hgk.skadnetwork",
		"9yg77x724h.skadnetwork",
		"gvmwg8q7h5.skadnetwork",
		"n66cz3y3bx.skadnetwork",
		"nzq8sh4pbs.skadnetwork",
		"pu4na253f3.skadnetwork",
		"yrqqpx2mcb.skadnetwork",
		"z4gj7hsk7h.skadnetwork",
		"f7s53z58qe.skadnetwork",
		"7953jerfzd.skadnetwork"
	};

	// 프레임워크 {
	public static readonly List<string> B_IOS_EXTRA_FRAMEWORK_LIST = new List<string>() {
		"libz.tbd",
		"libsqlite3.0.tbd",
		
		"iAd.framework",
		"WebKit.framework",
		"GameKit.framework",
		"Security.framework",
		"StoreKit.framework",
		"MessageUI.framework",
		"AdSupport.framework",
		"UserNotifications.framework",
		"SystemConfiguration.framework",
		"AuthenticationServices.framework"
	};

	public static readonly List<string> B_IOS_REMOVE_FRAMEWORK_LIST = new List<string>() {
		"AppTrackingTransparency.framework"
	};
	// 프레임워크 }

	// 호환성 타입
	public static readonly List<PBXCapabilityType> B_IOS_EXTRA_CAPABILITY_TYPE_LIST = new List<PBXCapabilityType>() {
#if APPLE_LOGIN_ENABLE
		PBXCapabilityType.SignInWithApple,
#endif			// #if APPLE_LOGIN_ENABLE

#if FIREBASE_MODULE_ENABLE && FIREBASE_CLOUD_MSG_ENABLE
		PBXCapabilityType.BackgroundModes, PBXCapabilityType.PushNotifications,
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

	#region 추가 상수

	#endregion			// 추가 상수

	#region 추가 런타임 상수

	#endregion			// 추가 런타임 상수
}
#endif			// #if UNITY_EDITOR
#endif			// #if SCRIPT_TEMPLATE_ONLY
