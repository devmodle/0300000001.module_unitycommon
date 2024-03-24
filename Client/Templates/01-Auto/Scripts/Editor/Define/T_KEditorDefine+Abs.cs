#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif // #if UNITY_IOS

/** 에디터 상수 */
public static partial class KEditorDefine
{
	#region 런타임 상수
	// 스크립트 순서
	public static readonly Dictionary<System.Type, int> B_SCRIPT_ORDER_DICT = new Dictionary<System.Type, int>()
	{
		[typeof(CValTable)] = KCDefine.B_SCRIPT_O_LATE,
		[typeof(CStrTable)] = KCDefine.B_SCRIPT_O_LATE,

		[typeof(CSndManager)] = KCDefine.B_SCRIPT_O_LATE,
		[typeof(CResManager)] = KCDefine.B_SCRIPT_O_LATE,
		[typeof(CTaskManager)] = KCDefine.B_SCRIPT_O_LATE,
		[typeof(CNetworkManager)] = KCDefine.B_SCRIPT_O_LATE,

		[typeof(CScheduleManager)] = KCDefine.B_SCRIPT_O_LATE,
		[typeof(CNavStackManager)] = KCDefine.B_SCRIPT_O_LATE,
		[typeof(CIndicatorManager)] = KCDefine.B_SCRIPT_O_LATE,

		[typeof(CObjsPoolManager)] = KCDefine.B_SCRIPT_O_LATE,
		[typeof(CCollectionPoolManager)] = KCDefine.B_SCRIPT_O_LATE,

		[typeof(CUnityMsgSender)] = KCDefine.B_SCRIPT_O_LATE,
		[typeof(CDeviceMsgReceiver)] = KCDefine.B_SCRIPT_O_LATE,

		[typeof(CCommonAppInfoStorage)] = KCDefine.B_SCRIPT_O_LATE,
		[typeof(CCommonUserInfoStorage)] = KCDefine.B_SCRIPT_O_LATE,
		[typeof(CCommonGameInfoStorage)] = KCDefine.B_SCRIPT_O_LATE,

		[typeof(CSampleSceneManager)] = KCDefine.B_SCRIPT_O_SCENE_MANAGER,
		[typeof(CSampleSceneManagerEditor)] = KCDefine.B_SCRIPT_O_SCENE_MANAGER,

#if RESEARCH_MODULE_ENABLE
		[typeof(CRSampleSceneManagerMenu)] = KCDefine.B_SCRIPT_O_SCENE_MANAGER,
		[typeof(CRSampleSceneManagerResearch)] = KCDefine.B_SCRIPT_O_SCENE_MANAGER,
#endif // #if RESEARCH_MODULE_ENABLE

#if SCENE_TEMPLATES_MODULE_ENABLE
		[typeof(InitScene.CSubInitSceneManager)] = KCDefine.B_SCRIPT_O_INIT_SCENE_MANAGER,
		[typeof(SetupScene.CSubSetupSceneManager)] = KCDefine.B_SCRIPT_O_SETUP_SCENE_MANAGER,
		[typeof(LateSetupScene.CSubLateSetupSceneManager)] = KCDefine.B_SCRIPT_O_LATE_SETUP_SCENE_MANAGER,

#if RESEARCH_MODULE_ENABLE
		[typeof(MenuScene.CSSubMenuSceneManager)] = KCDefine.B_SCRIPT_O_SCENE_MANAGER,
#endif // #if RESEARCH_MODULE_ENABLE
#endif // #if SCENE_TEMPLATES_MODULE_ENABLE

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		[typeof(CEtcInfoTable)] = KCDefine.B_SCRIPT_O_LATE,
		[typeof(CLevelInfoTable)] = KCDefine.B_SCRIPT_O_LATE,

		[typeof(CCalcInfoTable)] = KCDefine.B_SCRIPT_O_LATE,
		[typeof(CMissionInfoTable)] = KCDefine.B_SCRIPT_O_LATE,
		[typeof(CRewardInfoTable)] = KCDefine.B_SCRIPT_O_LATE,
		[typeof(CEpisodeInfoTable)] = KCDefine.B_SCRIPT_O_LATE,
		[typeof(CTutorialInfoTable)] = KCDefine.B_SCRIPT_O_LATE,
		[typeof(CResInfoTable)] = KCDefine.B_SCRIPT_O_LATE,

		[typeof(CItemInfoTable)] = KCDefine.B_SCRIPT_O_LATE,
		[typeof(CSkillInfoTable)] = KCDefine.B_SCRIPT_O_LATE,
		[typeof(CObjInfoTable)] = KCDefine.B_SCRIPT_O_LATE,
		[typeof(CFXInfoTable)] = KCDefine.B_SCRIPT_O_LATE,
		[typeof(CAbilityInfoTable)] = KCDefine.B_SCRIPT_O_LATE,
		[typeof(CProductTradeInfoTable)] = KCDefine.B_SCRIPT_O_LATE,

		[typeof(CAppInfoStorage)] = KCDefine.B_SCRIPT_O_LATE,
		[typeof(CUserInfoStorage)] = KCDefine.B_SCRIPT_O_LATE,
		[typeof(CGameInfoStorage)] = KCDefine.B_SCRIPT_O_LATE,

		[typeof(MainScene.CSubMainSceneManager)] = KCDefine.B_SCRIPT_O_SCENE_MANAGER,
		[typeof(PlayScene.CSubPlaySceneManager)] = KCDefine.B_SCRIPT_O_SCENE_MANAGER,
		[typeof(TestScene.CSubTestSceneManager)] = KCDefine.B_SCRIPT_O_SCENE_MANAGER,
		[typeof(TitleScene.CSubTitleSceneManager)] = KCDefine.B_SCRIPT_O_SCENE_MANAGER,

		[typeof(ResultScene.CSubResultSceneManager)] = KCDefine.B_SCRIPT_O_RESULT_SCENE_MANAGER,
		[typeof(LoadingScene.CSubLoadingSceneManager)] = KCDefine.B_SCRIPT_O_LOADING_SCENE_MANAGER,
		[typeof(OverlayScene.CSubOverlaySceneManager)] = KCDefine.B_SCRIPT_O_OVERLAY_SCENE_MANAGER,
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE)
		[typeof(LevelEditorScene.CSubLevelEditorSceneManager)] = KCDefine.B_SCRIPT_O_SCENE_MANAGER,
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE)

#if ADS_MODULE_ENABLE
		[typeof(CAdsManager)] = KCDefine.B_SCRIPT_O_LATE,
		[typeof(CBannerAdsPosAdjuster)] = KCDefine.B_SCRIPT_O_LATEST,
		[typeof(CBannerAdsSizeAdjuster)] = KCDefine.B_SCRIPT_O_LATEST,
		[typeof(CRewardAdsTouchInteractable)] = KCDefine.B_SCRIPT_O_LATEST,
#endif // #if ADS_MODULE_ENABLE

#if FLURRY_MODULE_ENABLE
		[typeof(CFlurryManager)] = KCDefine.B_SCRIPT_O_LATE,
#endif // #if FLURRY_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
		[typeof(CFacebookManager)] = KCDefine.B_SCRIPT_O_LATE,
#endif // #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
		[typeof(CFirebaseManager)] = KCDefine.B_SCRIPT_O_LATE,
#endif // #if FIREBASE_MODULE_ENABLE

#if APPS_FLYER_MODULE_ENABLE
		[typeof(CAppsFlyerManager)] = KCDefine.B_SCRIPT_O_LATE,
#endif // #if APPS_FLYER_MODULE_ENABLE

#if GAME_CENTER_MODULE_ENABLE
		[typeof(CGameCenterManager)] = KCDefine.B_SCRIPT_O_LATE,
#endif // #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
		[typeof(CPurchaseManager)] = KCDefine.B_SCRIPT_O_LATE,
#endif // #if PURCHASE_MODULE_ENABLE

#if NOTI_MODULE_ENABLE
		[typeof(CNotiManager)] = KCDefine.B_SCRIPT_O_LATE,
#endif // #if NOTI_MODULE_ENABLE

#if PLAYFAB_MODULE_ENABLE
		[typeof(CPlayfabManager)] = KCDefine.B_SCRIPT_O_LATE
#endif // #if PLAYFAB_MODULE_ENABLE
	};

	// 클래스 타입
	public static readonly Dictionary<string, System.Type> B_SCENE_MANAGER_TYPE_DICT = new Dictionary<string, System.Type>()
	{
#if SCENE_TEMPLATES_MODULE_ENABLE
		[KCDefine.B_SCENE_N_INIT] = typeof(InitScene.CSubInitSceneManager),
		[KCDefine.B_SCENE_N_SETUP] = typeof(SetupScene.CSubSetupSceneManager),
		[KCDefine.B_SCENE_N_LATE_SETUP] = typeof(LateSetupScene.CSubLateSetupSceneManager),

#if RESEARCH_MODULE_ENABLE
		[KCDefine.B_SCENE_N_MENU] = typeof(MenuScene.CSSubMenuSceneManager),
#endif // #if RESEARCH_MODULE_ENABLE
#endif // #if SCENE_TEMPLATES_MODULE_ENABLE

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		[KCDefine.B_SCENE_N_TITLE] = typeof(TitleScene.CSubTitleSceneManager),
		[KCDefine.B_SCENE_N_MAIN] = typeof(MainScene.CSubMainSceneManager),
		[KCDefine.B_SCENE_N_PLAY] = typeof(PlayScene.CSubPlaySceneManager),

		[KCDefine.B_SCENE_N_RESULT] = typeof(ResultScene.CSubResultSceneManager),
		[KCDefine.B_SCENE_N_LOADING] = typeof(LoadingScene.CSubLoadingSceneManager),
		[KCDefine.B_SCENE_N_OVERLAY] = typeof(OverlayScene.CSubOverlaySceneManager),
		[KCDefine.B_SCENE_N_TEST] = typeof(TestScene.CSubTestSceneManager),
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE)
		[KCDefine.B_SCENE_N_EDITOR_LEVEL] = typeof(LevelEditorScene.CSubLevelEditorSceneManager)
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE)
	};

	// 패키지
	public static readonly Dictionary<string, string> B_UNITY_PKGS_DEPENDENCY_DICT = new Dictionary<string, string>()
	{
		// 기본 {
		["com.unity.feature.2d"] = "2.0.0",
		["com.unity.feature.characters-animation"] = "1.0.0",
		["com.unity.feature.cinematic"] = "1.0.0",
		["com.unity.feature.development"] = "1.0.1",
		["com.unity.feature.gameplay-storytelling"] = "1.0.0",
		["com.unity.feature.mobile"] = "1.0.0",
		["com.unity.feature.worldbuilding"] = "1.0.1",

		["com.unity.inputsystem"] = "1.7.0",
		["com.unity.ads.ios-support"] = "1.0.0",
		["com.unity.localization"] = "1.4.5",
		["com.unity.render-pipelines.universal"] = "14.0.10",
		["com.unity.visualeffectgraph"] = "14.0.10",
		["com.unity.adaptiveperformance.samsung.android"] = "4.0.2",

#if PURCHASE_ENABLE || PURCHASE_MODULE_ENABLE
		["com.unity.purchasing"] = "4.11.0",
		["com.unity.purchasing.udp"] = "2.2.5",
#endif // #if PURCHASE_ENABLE || PURCHASE_MODULE_ENABLE

#if UNI_TASK_ENABLE || UNI_TASK_MODULE_ENABLE
		["com.cysharp.unitask"] = "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask",
#endif // #if UNI_TASK_ENABLE || UNI_TASK_MODULE_ENABLE

#if DEVELOPMENT_PROJ
#if ADS_ENABLE || ADS_MODULE_ENABLE
		["module.unitycommonads"] = "https://gitlab.com/9tapmodule.repository/0300000001.module_unitycommonads.git#4.1.0",
#endif // #if ADS_ENABLE || ADS_MODULE_ENABLE

#if FLURRY_ENABLE || FLURRY_MODULE_ENABLE
		["module.unitycommonflurry"] = "https://gitlab.com/9tapmodule.repository/0300000001.module_unitycommonflurry.git#4.1.0",
#endif // #if FLURRY_ENABLE || FLURRY_MODULE_ENABLE

#if FACEBOOK_ENABLE || FACEBOOK_MODULE_ENABLE
		["module.unitycommonfacebook"] = "https://gitlab.com/9tapmodule.repository/0300000001.module_unitycommonfacebook.git#4.1.0",
#endif // #if FACEBOOK_ENABLE || FACEBOOK_MODULE_ENABLE

#if FIREBASE_ENABLE || FIREBASE_MODULE_ENABLE
		["module.unitycommonfirebase"] = "https://gitlab.com/9tapmodule.repository/0300000001.module_unitycommonfirebase.git#4.1.0",
#endif // #if FIREBASE_ENABLE || FIREBASE_MODULE_ENABLE

#if SINGULAR_ENABLE || SINGULAR_MODULE_ENABLE
		["module.unitycommonsingular"] = "https://gitlab.com/9tapmodule.repository/0300000001.module_unitycommonsingular.git#4.1.0",
#endif // #if SINGULAR_ENABLE || SINGULAR_MODULE_ENABLE

#if APPS_FLYER_ENABLE || APPS_FLYER_MODULE_ENABLE
		["module.unitycommonappsflyer"] = "https://gitlab.com/9tapmodule.repository/0300000001.module_unitycommonappsflyer.git#4.1.0",
#endif // #if APPS_FLYER_ENABLE || APPS_FLYER_MODULE_ENABLE

#if GAME_CENTER_ENABLE || GAME_CENTER_MODULE_ENABLE
		["module.unitycommongamecenter"] = "https://gitlab.com/9tapmodule.repository/0300000001.module_unitycommongamecenter.git#4.1.0",
#endif // #if GAME_CENTER_ENABLE || GAME_CENTER_MODULE_ENABLE

#if PURCHASE_ENABLE || PURCHASE_MODULE_ENABLE
		["module.unitycommonpurchase"] = "https://gitlab.com/9tapmodule.repository/0300000001.module_unitycommonpurchase.git#4.1.0",
#endif // #if PURCHASE_ENABLE || PURCHASE_MODULE_ENABLE

#if NOTI_ENABLE || NOTI_MODULE_ENABLE
		["module.unitycommonnotification"] = "https://gitlab.com/9tapmodule.repository/0300000001.module_unitycommonnotification.git#4.1.0",
#endif // #if NOTI_ENABLE || NOTI_MODULE_ENABLE

#if PLAYFAB_ENABLE || PLAYFAB_MODULE_ENABLE
		["module.unitycommonplayfab"] = "https://gitlab.com/9tapmodule.repository/0300000001.module_unitycommonplayfab.git#4.1.0"
#endif // #if PLAYFAB_ENABLE || PLAYFAB_MODULE_ENABLE
#endif // #if DEVELOPMENT_PROJ
	};
	#endregion // 런타임 상수

	#region 조건부 상수
#if UNITY_IOS
	// 텍스트
	public const string B_IOS_USER_TRACKING_USAGE_DESC = "Special offers and promotions just for you\nAdvertisements that match your interests\nAn improved personalized experience over time";
#endif // #if UNITY_IOS
	#endregion // 조건부 상수

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
	public static readonly List<string> B_IOS_ADD_FRAMEWORK_LIST = new List<string>() {
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
#endif // #if UNITY_IOS
	#endregion // 조건부 런타임 상수
}
#endif // #if UNITY_EDITOR
#endif // #if SCRIPT_TEMPLATE_ONLY
