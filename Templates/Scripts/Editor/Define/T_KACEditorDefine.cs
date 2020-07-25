using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
#if UNITY_EDITOR
using UnityEditor;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif			// #if UNITY_IOS

//! 자동 생성 상수
public static partial class KACEditorDefine {
	#region 런타임 상수
	// 스크립트 순서
	public static Dictionary<System.Type, int> B_SCRIPT_ORDERS = new Dictionary<System.Type, int>() {
		[typeof(CValueTable)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CStringTable)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,

		[typeof(CUnityMsgSender)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CDeviceMsgReceiver)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,

		[typeof(CLogManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CSndManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CScheduleManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CResManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
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
		[typeof(CSubIntroSceneManager)] = KCDefine.U_SCRIPT_ORDER_SCENE_MANAGER,

#if STUDY_MODULE_ENABLE
		[typeof(CMenuSceneManager)] = KCDefine.U_SCRIPT_ORDER_SCENE_MANAGER,
#endif			// #if STUDY_MODULE_ENABLE

#if MESSAGE_PACK_ENABLE
		[typeof(CAppInfoStorage)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CUserInfoStorage)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
#endif			// #if MESSAGE_PACK_ENABLE

#if UNITY_ANDROID
		[typeof(CPermissionManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
#endif			// #if UNITY_ANDROID

#if ADS_ENABLE
		[typeof(CAdsManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
		[typeof(CBannerAdsCorrector)] = KCDefine.U_SCRIPT_ORDER_BANNER_ADS_CORRECTOR,
#endif			// #if ADS_ENABLE

#if TENJIN_ENABLE
		[typeof(CTenjinManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
#endif			// #if TENJIN_ENABLE

#if FLURRY_ENABLE
		[typeof(CFlurryManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
#endif			// #if FLURRY_ENABLE

#if FACEBOOK_ENABLE
		[typeof(CFacebookManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
#endif			// #if FACEBOOK_ENABLE

#if FIREBASE_ENABLE
		[typeof(CFirebaseManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
#endif			// #if FIREBASE_ENABLE

#if PURCHASE_ENABLE && MESSAGE_PACK_ENABLE
		[typeof(CPurchaseManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON,
#endif			// #if PURCHASE_ENABLE && MESSAGE_PACK_ENABLE

#if UNITY_SERVICE_ENABLE
		[typeof(CUnityServiceManager)] = KCDefine.U_SCRIPT_ORDER_SINGLETON
#endif			// #if UNITY_SERVICE_ENABLE
	};

	// 타입
	public static readonly Dictionary<string, System.Type> B_SCENE_MANAGER_TYPE_LIST = new Dictionary<string, System.Type>() {
		[KCDefine.B_SCENE_NAME_INIT] = typeof(CSubInitSceneManager),
		[KCDefine.B_SCENE_NAME_SETUP] = typeof(CSubSetupSceneManager),
		[KCDefine.B_SCENE_NAME_START] = typeof(CSubStartSceneManager),
		[KCDefine.B_SCENE_NAME_LOADING] = typeof(CSubLoadingSceneManager),
		[KCDefine.B_SCENE_NAME_SPLASH] = typeof(CSubSplashSceneManager),
		[KCDefine.B_SCENE_NAME_AGREE] = typeof(CSubAgreeSceneManager),
		[KCDefine.B_SCENE_NAME_INTRO] = typeof(CSubIntroSceneManager),

#if STUDY_MODULE_ENABLE
		[KSDefine.B_SCENE_NAME_MENU] = typeof(CMenuSceneManager)
#endif			// #if STUDY_MODULE_ENABLE
	};
	#endregion			// 런타임 상수

	#region 조건부 런타임 상수
#if UNITY_IOS
	// 프레임워크
	public static readonly string[] B_EXTRA_FRAMEWORKS_IOS = new string[] {
		"AuthenticationServices.framework"
	};

	// 호환성 타입
	public static readonly PBXCapabilityType[] B_EXTRA_CAPABILITY_TYPES_IOS = new PBXCapabilityType[] {
		PBXCapabilityType.SignInWithApple,

#if GAME_CENTER_ENABLE
		PBXCapabilityType.GameCenter,
#endif			// #if GAME_CENTER_ENABLE

#if FIREBASE_ENABLE && FIREBASE_MESSAGING_ENABLE
		PBXCapabilityType.PushNotifications
#endif			// #if FIREBASE_ENABLE && FIREBASE_MESSAGING_ENABLE
	};
#endif			// #if UNITY_IOS
	#endregion			// 조건부 런타임 상수
}
#endif			// #if UNITY_EDITOR
#endif			// #if NEVER_USE_THIS
