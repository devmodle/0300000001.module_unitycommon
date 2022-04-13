using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif			// #if PURCHASE_MODULE_ENABLE

#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
using GoogleSheetsToUnity;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)

/** 기본 함수 */
public static partial class Func {
	/** 콜백 */
	private enum ECallback {
		NONE = -1,

#if ADS_MODULE_ENABLE
		BANNER_ADS,
		REWARD_ADS,
		FULLSCREEN_ADS,
#endif			// #if ADS_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
		FACEBOOK_LOGIN,
		FACEBOOK_LOGOUT,
#endif			// #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
		FIREBASE_LOGIN,
		FIREBASE_LOGOUT,

		LOAD_USER_INFO,
		LOAD_PURCHASE_INFOS,
		LOAD_POST_ITEM_INFOS,

		SAVE_USER_INFO,
		SAVE_PURCHASE_INFOS,
		SAVE_POST_ITEM_INFOS,
#endif			// #if FIREBASE_MODULE_ENABLE

#if GAME_CENTER_MODULE_ENABLE
		GAME_CENTER_LOGIN,
		GAME_CENTER_LOGOUT,

		UPDATE_RECORD,
		UPDATE_ACHIEVEMENT,
#endif			// #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
		PURCHASE,
		RESTORE,
#endif			// #if PURCHASE_MODULE_ENABLE

#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		LOAD_GOOGLE_SHEET,	
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)

		[HideInInspector] MAX_VAL
	}

	#region 클래스 변수
#if ADS_MODULE_ENABLE
	private static bool m_bIsWatchRewardAds = false;
	private static bool m_bIsWatchFullscreenAds = false;

	private static STAdsRewardInfo m_stAdsRewardInfo;
	private static Dictionary<ECallback, System.Action<CAdsManager, bool>> m_oAdsCallbackDictA = new Dictionary<ECallback, System.Action<CAdsManager, bool>>();
	private static Dictionary<ECallback, System.Action<CAdsManager, STAdsRewardInfo, bool>> m_oAdsCallbackDictB = new Dictionary<ECallback, System.Action<CAdsManager, STAdsRewardInfo, bool>>();
#endif			// #if ADS_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
	private static Dictionary<ECallback, System.Action<CFacebookManager>> m_oFacebookCallbackDictA = new Dictionary<ECallback, System.Action<CFacebookManager>>();
	private static Dictionary<ECallback, System.Action<CFacebookManager, bool>> m_oFacebookCallbackDictB = new Dictionary<ECallback, System.Action<CFacebookManager, bool>>();
#endif			// #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
	private static Dictionary<ECallback, System.Action<CFirebaseManager>> m_oFirebaseCallbackDictA = new Dictionary<ECallback, System.Action<CFirebaseManager>>();
	private static Dictionary<ECallback, System.Action<CFirebaseManager, bool>> m_oFirebaseCallbackDictB = new Dictionary<ECallback, System.Action<CFirebaseManager, bool>>();
	private static Dictionary<ECallback, System.Action<CFirebaseManager, string, bool>> m_oFirebaseCallbackDictC = new Dictionary<ECallback, System.Action<CFirebaseManager, string, bool>>();
#endif			// #if FIREBASE_MODULE_ENABLE

#if GAME_CENTER_MODULE_ENABLE
	private static Dictionary<ECallback, System.Action<CGameCenterManager>> m_oGameCenterCallbackDictA = new Dictionary<ECallback, System.Action<CGameCenterManager>>();
	private static Dictionary<ECallback, System.Action<CGameCenterManager, bool>> m_oGameCenterCallbackDictB = new Dictionary<ECallback, System.Action<CGameCenterManager, bool>>();
#endif			// #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	private static Dictionary<ECallback, System.Action<CPurchaseManager, string, bool>> m_oPurchaseCallbackDictA = new Dictionary<ECallback, System.Action<CPurchaseManager, string, bool>>();
	private static Dictionary<ECallback, System.Action<CPurchaseManager, List<Product>, bool>> m_oPurchaseCallbackDictB = new Dictionary<ECallback, System.Action<CPurchaseManager, List<Product>, bool>>();
#endif			// #if PURCHASE_MODULE_ENABLE

#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	private static List<(string, int, int)> m_oGoogleSheetInfoList = new List<(string, int, int)>();
	private static Dictionary<string, (SimpleJSON.JSONNode, bool)> m_oGoogleSheetJSONNodeInfoDict = new Dictionary<string, (SimpleJSON.JSONNode, bool)>();
	private static Dictionary<ECallback, System.Action<CServicesManager, GstuSpreadSheet, string, Dictionary<string, (SimpleJSON.JSONNode, bool)>>> m_oServicesCallbackDict = new Dictionary<ECallback, System.Action<CServicesManager, GstuSpreadSheet, string, Dictionary<string, (SimpleJSON.JSONNode, bool)>>>();
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion			// 클래스 변수

	#region 클래스 함수
	/** 지역화 문자열을 설정한다 */
	public static void SetupLocalizeStrs() {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
		Func.SetupLocalizeStrs(CCommonAppInfoStorage.Inst.CountryCode, CCommonAppInfoStorage.Inst.SystemLanguage);
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
	}

	/** 지역화 문자열을 설정한다 */
	public static void SetupLocalizeStrs(string a_oCountryCode, SystemLanguage a_eSystemLanguage, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oCountryCode.ExIsValid());
		
		// 국가 코드가 존재 할 경우
		if(a_oCountryCode.ExIsValid()) {
			CStrTable.Inst.LoadStrsFromRes(CFactory.MakeLocalizePath(KCDefine.U_BASE_TABLE_P_G_LOCALIZE_COMMON_STR, KCDefine.U_TABLE_P_G_ENGLISH_COMMON_STR, a_oCountryCode, a_eSystemLanguage.ToString()));
		}
	}

	/** 경고 팝업을 출력한다 */
	public static void ShowAlertPopup(string a_oMsg, System.Action<CAlertPopup, bool> a_oCallback, bool a_bIsEnableCancelBtn = true) {
		var stParams = new CAlertPopup.STParams() {
			m_oStrDict = new Dictionary<CAlertPopup.EStr, string>() {
				[CAlertPopup.EStr.TITLE] = CStrTable.Inst.GetStr(KCDefine.ST_KEY_COMMON_NOTI_TEXT),
				[CAlertPopup.EStr.MSG] = a_oMsg,
				[CAlertPopup.EStr.OK_BTN] = CStrTable.Inst.GetStr(KCDefine.ST_KEY_COMMON_OK_TEXT),
			},

			m_oCallbackDict = new Dictionary<CAlertPopup.ECallback, System.Action<CAlertPopup, bool>>() {
				[CAlertPopup.ECallback.OK_CANCEL] = a_oCallback
			}
		};

		// 취소 버튼 가능 모드 일 경우
		if(a_bIsEnableCancelBtn) {
			stParams.m_oStrDict.TryAdd(CAlertPopup.EStr.CANCEL_BTN, CStrTable.Inst.GetStr(KCDefine.ST_KEY_COMMON_CANCEL_TEXT));
		}

		Func.ShowAlertPopup(stParams);
	}

	/** 경고 팝업을 출력한다 */
	public static void ShowAlertPopup(CAlertPopup.STParams a_stParams) {
		// 경고 팝업이 없을 경우
		if(CSceneManager.ScreenPopupUIs.ExFindChild(KCDefine.U_OBJ_N_ALERT_POPUP) == null) {
			var oAlertPopup = CAlertPopup.Create<CAlertPopup>(KCDefine.U_OBJ_N_ALERT_POPUP, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_G_ALERT_POPUP), CSceneManager.ScreenPopupUIs, a_stParams);
			oAlertPopup.Show(null, null);
		}
	}
	
	/** 종료 팝업을 출력한다 */
	public static void ShowQuitPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_QUIT_P_MSG), a_oCallback);
	}
	
	/** 업데이트 팝업을 출력한다 */
	public static void ShowUpdatePopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_UPDATE_P_MSG), a_oCallback);
	}

	/** 그만두기 팝업을 출력한다 */
	public static void ShowLeavePopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_LEAVE_P_MSG), a_oCallback);
	}

	/** 로드 팝업을 출력한다 */
	public static void ShowLoadPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_LOAD_P_MSG), a_oCallback);
	}

	/** 저장 팝업을 출력한다 */
	public static void ShowSavePopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_SAVE_P_MSG), a_oCallback);
	}

	/** 로그인 성공 팝업을 출력한다 */
	public static void ShowLoginSuccessPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_COMMON_LOGIN_SUCCESS_MSG), a_oCallback, false);
	}

	/** 로그인 실패 팝업을 출력한다 */
	public static void ShowLoginFailPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_COMMON_LOGIN_SUCCESS_MSG), a_oCallback, false);
	}

	/** 로그아웃 성공 팝업을 출력한다 */
	public static void ShowLogoutSuccessPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_COMMON_LOGOUT_SUCCESS_MSG), a_oCallback, false);
	}

	/** 로그아웃 실패 팝업을 출력한다 */
	public static void ShowLogoutFailPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_COMMON_LOGOUT_SUCCESS_MSG), a_oCallback, false);
	}

	/** 로드 성공 팝업을 출력한다 */
	public static void ShowLoadSuccessPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_COMMON_LOAD_SUCCESS_MSG), a_oCallback, false);
	}

	/** 로드 실패 팝업을 출력한다 */
	public static void ShowLoadFailPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_COMMON_LOAD_FAIL_MSG), a_oCallback, false);
	}

	/** 저장 성공 팝업을 출력한다 */
	public static void ShowSaveSuccessPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_COMMON_SAVE_SUCCESS_MSG), a_oCallback, false);
	}

	/** 저장 실패 팝업을 출력한다 */
	public static void ShowSaveFailPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_COMMON_SAVE_FAIL_MSG), a_oCallback, false);
	}

	/** 결제 성공 팝업을 출력한다 */
	public static void ShowPurchaseSuccessPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_COMMON_PURCHASE_SUCCESS_MSG), a_oCallback, false);
	}

	/** 결제 실패 팝업을 출력한다 */
	public static void ShowPurchaseFailPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_COMMON_PURCHASE_FAIL_MSG), a_oCallback, false);
	}

	/** 복원 성공 팝업을 출력한다 */
	public static void ShowRestoreSuccessPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_COMMON_RESTORE_SUCCESS_MSG), a_oCallback, false);
	}

	/** 복원 실패 팝업을 출력한다 */
	public static void ShowRestoreFailPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_COMMON_RESTORE_FAIL_MSG), a_oCallback, false);
	}
	#endregion			// 클래스 함수

	#region 제네릭 클래스 함수
	/** 팝업을 출력한다 */
	public static void ShowPopup<T>(string a_oName, string a_oObjPath, GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) where T : CPopup {
		// 팝업이 없을 경우
		if(a_oParent.ExFindChild(a_oName) == null) {
			var oPopup = CPopup.Create<T>(a_oName, a_oObjPath, a_oParent);
			CFunc.Invoke(ref a_oInitCallback, oPopup);

			oPopup.Show(a_oShowCallback, a_oCloseCallback);
		}
	}
	#endregion			// 제네릭 클래스 함수

	#region 조건부 클래스 함수
#if ADS_MODULE_ENABLE
	/** 배너 광고를 출력한다 */
	public static void ShowBannerAds(System.Action<CAdsManager, bool> a_oCallback) {
		Func.ShowBannerAds(CPluginInfoTable.Inst.AdsPlatform, a_oCallback);
	}

	/** 배너 광고를 출력한다 */
	public static void ShowBannerAds(EAdsPlatform a_eAdsPlatform, System.Action<CAdsManager, bool> a_oCallback) {
		// 배너 광고 출력이 가능 할 경우
		if(CAdsManager.Inst.IsLoadBannerAds(a_eAdsPlatform)) {
			Func.m_oAdsCallbackDictA.ExReplaceVal(ECallback.BANNER_ADS, a_oCallback);
			CSceneManager.ActiveSceneManager.ExLateCallFunc((a_oSender) => CAdsManager.Inst.ShowBannerAds(a_eAdsPlatform, Func.OnShowBannerAds));
		} else {
			CFunc.Invoke(ref a_oCallback, CAdsManager.Inst, false);
		}
	}

	/** 배너 광고를 닫는다 */
	public static void CloseBannerAds() {
		Func.CloseBannerAds(CPluginInfoTable.Inst.AdsPlatform);
	}

	/** 배너 광고를 닫는다 */
	public static void CloseBannerAds(EAdsPlatform a_eAdsPlatform) {
		CAdsManager.Inst.CloseBannerAds(a_eAdsPlatform);
	}

	/** 보상 광고를 출력한다 */
	public static void ShowRewardAds(System.Action<CAdsManager, STAdsRewardInfo, bool> a_oCallback) {
		Func.ShowRewardAds(CPluginInfoTable.Inst.AdsPlatform, a_oCallback);
	}
	
	/** 보상 광고를 출력한다 */
	public static void ShowRewardAds(EAdsPlatform a_eAdsPlatform, System.Action<CAdsManager, STAdsRewardInfo, bool> a_oCallback) {
		// 보상 광고 출력이 가능 할 경우
		if(CAdsManager.Inst.IsLoadRewardAds(a_eAdsPlatform)) {
#if UNITY_EDITOR
			CIndicatorManager.Inst.Show(false, a_eAdsPlatform != EAdsPlatform.ADMOB);
#else
			CIndicatorManager.Inst.Show();
#endif			// #if UNITY_EDITOR

			CSceneManager.ActiveSceneManager.ExLateCallFunc((a_oSender) => {
				Func.m_bIsWatchRewardAds = false;
				Func.m_stAdsRewardInfo = KCDefine.U_INVALID_ADS_REWARD_INFO;

				Func.m_oAdsCallbackDictB.ExReplaceVal(ECallback.REWARD_ADS, a_oCallback);
				CAdsManager.Inst.ShowRewardAds(a_eAdsPlatform, Func.OnReceiveAdsReward, Func.OnCloseRewardAds);
			});
		} else {
			CFunc.Invoke(ref a_oCallback, CAdsManager.Inst, KCDefine.U_INVALID_ADS_REWARD_INFO, false);
		}
	}

	/** 전면 광고를 출력한다 */
	public static void ShowFullscreenAds(System.Action<CAdsManager, bool> a_oCallback) {
		Func.ShowFullscreenAds(CPluginInfoTable.Inst.AdsPlatform, a_oCallback);
	}

	/** 전면 광고를 출력한다 */
	public static void ShowFullscreenAds(EAdsPlatform a_eAdsPlatform, System.Action<CAdsManager, bool> a_oCallback) {
		// 전면 광고 출력이 가능 할 경우
		if(CAppInfoStorage.Inst.IsEnableShowFullscreenAds && CAdsManager.Inst.IsLoadFullscreenAds(a_eAdsPlatform)) {
#if UNITY_EDITOR
			CIndicatorManager.Inst.Show(false, a_eAdsPlatform != EAdsPlatform.ADMOB);
#else
			CIndicatorManager.Inst.Show();
#endif			// #if UNITY_EDITOR

			CSceneManager.ActiveSceneManager.ExLateCallFunc((a_oSender) => {
				// 전면 광고 출력이 가능 할 경우
				if(CAppInfoStorage.Inst.IsEnableShowFullscreenAds) {
					Func.m_bIsWatchFullscreenAds = true;
					Func.m_oAdsCallbackDictA.ExReplaceVal(ECallback.FULLSCREEN_ADS, a_oCallback);

					CAdsManager.Inst.ShowFullscreenAds(a_eAdsPlatform, null, Func.OnCloseFullscreenAds);
				} else {
					CIndicatorManager.Inst.Close();
					CFunc.Invoke(ref a_oCallback, CAdsManager.Inst, false);
				}
			}, KCDefine.B_VAL_1_FLT, true);
		} else {
			// 광고 누적 횟수 갱신이 가능 할 경우
			if(CAppInfoStorage.Inst.IsEnableUpdateAdsSkipTimes) {
				CAppInfoStorage.Inst.AddAdsSkipTimes(KCDefine.B_VAL_1_INT);
			}

			CFunc.Invoke(ref a_oCallback, CAdsManager.Inst, false);
		}
	}

	/** 배너 광고가 출력 되었을 경우 */
	private static void OnShowBannerAds(CAdsManager a_oSender, bool a_bIsSuccess) {
		Func.m_oAdsCallbackDictA.GetValueOrDefault(ECallback.BANNER_ADS)?.Invoke(a_oSender, a_bIsSuccess);
	}

	/** 보상 광고가 닫혔을 경우 */
	private static void OnCloseRewardAds(CAdsManager a_oSender) {
		CAppInfoStorage.Inst.PrevRewardAdsTime = System.DateTime.Now;

		CAppInfoStorage.Inst.AddRewardAdsWatchTimes(KCDefine.B_VAL_1_INT);
		CAppInfoStorage.Inst.SaveAppInfo();

		CIndicatorManager.Inst.Close();
		Func.m_oAdsCallbackDictB.GetValueOrDefault(ECallback.REWARD_ADS)?.Invoke(a_oSender, Func.m_stAdsRewardInfo, Func.m_bIsWatchRewardAds);
	}

	/** 광고 보상을 수신했을 경우 */
	private static void OnReceiveAdsReward(CAdsManager a_oSender, STAdsRewardInfo a_stAdsRewardInfo, bool a_bIsSuccess) {
		Func.m_bIsWatchRewardAds = a_bIsSuccess;
		Func.m_stAdsRewardInfo = a_stAdsRewardInfo;
	}

	/** 전면 광고가 닫혔을 경우 */
	private static void OnCloseFullscreenAds(CAdsManager a_oSender) {
		CAppInfoStorage.Inst.AdsSkipTimes = KCDefine.B_VAL_0_INT;
		CAppInfoStorage.Inst.PrevAdsTime = System.DateTime.Now;

		CAppInfoStorage.Inst.AddFullscreenAdsWatchTimes(KCDefine.B_VAL_1_INT);
		CAppInfoStorage.Inst.SaveAppInfo();

		CIndicatorManager.Inst.Close();
		Func.m_oAdsCallbackDictA.GetValueOrDefault(ECallback.FULLSCREEN_ADS)?.Invoke(a_oSender, Func.m_bIsWatchFullscreenAds);
	}
#endif			// #if ADS_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
	/** 페이스 북 로그인을 처리한다 */
	public static void FacebookLogin(System.Action<CFacebookManager, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oFacebookCallbackDictB.ExReplaceVal(ECallback.FACEBOOK_LOGIN, a_oCallback);

		CFacebookManager.Inst.Login(KCDefine.U_PERMISSION_LIST_FACEBOOK, Func.OnFacebookLogin);
	}

	/** 페이스 북 로그아웃을 처리한다 */
	public static void FacebookLogout(System.Action<CFacebookManager> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oFacebookCallbackDictA.ExReplaceVal(ECallback.FACEBOOK_LOGOUT, a_oCallback);

		CFacebookManager.Inst.Logout(a_oCallback);
	}

	/** 페이스 북에 로그인 되었을 경우 */
	private static void OnFacebookLogin(CFacebookManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oFacebookCallbackDictB.GetValueOrDefault(ECallback.FACEBOOK_LOGIN)?.Invoke(a_oSender, a_bIsSuccess);
	}

	/** 페이스 북에서 로그아웃 되었을 경우 */
	private static void OnFacebookLogout(CFacebookManager a_oSender) {
		CIndicatorManager.Inst.Close();
		Func.m_oFacebookCallbackDictA.GetValueOrDefault(ECallback.FACEBOOK_LOGOUT)?.Invoke(a_oSender);
	}
#endif			// #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
	/** 파이어 베이스 로그인을 처리한다 */
	public static void FirebaseLogin(System.Action<CFirebaseManager, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oFirebaseCallbackDictB.ExReplaceVal(ECallback.FIREBASE_LOGIN, a_oCallback);

#if UNITY_IOS && APPLE_LOGIN_ENABLE
		CServicesManager.Inst.LoginWithApple(Func.OnFirebaseAppleLogin);
#elif (UNITY_IOS || UNITY_ANDROID) && FACEBOOK_MODULE_ENABLE
		CFacebookManager.Inst.Login(KCDefine.U_PERMISSION_LIST_FACEBOOK, Func.OnFirebaseFacebookLogin);
#else
		CFirebaseManager.Inst.Login(Func.OnFirebaseLogin);
#endif			// #if UNITY_IOS && APPLE_LOGIN_ENABLE
	}

	/** 파이어 베이스 로그아웃을 처리한다 */
	public static void FirebaseLogout(System.Action<CFirebaseManager> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oFirebaseCallbackDictA.ExReplaceVal(ECallback.FIREBASE_LOGOUT, a_oCallback);

#if UNITY_IOS && APPLE_LOGIN_ENABLE
		CServicesManager.Inst.LogoutWithApple(Func.OnFirebaseAppleLogout);
#elif (UNITY_IOS || UNITY_ANDROID) && FACEBOOK_MODULE_ENABLE
		CFacebookManager.Inst.Logout(Func.OnFirebaseFacebookLogout);
#else
		CFirebaseManager.Inst.Logout(Func.OnFirebaseLogout);
#endif			// #if UNITY_IOS && APPLE_LOGIN_ENABLE
	}

	/** 유저 정보를 로드한다 */
	public static void LoadUserInfo(System.Action<CFirebaseManager, string, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oFirebaseCallbackDictC.ExReplaceVal(ECallback.LOAD_USER_INFO, a_oCallback);

		// 로그인 되었을 경우
		if(CFirebaseManager.Inst.IsLogin) {
			var oNodeList = Factory.MakeUserInfoNodes();
			CFirebaseManager.Inst.LoadDB(oNodeList, Func.OnLoadUserInfo);
		} else {
			Func.OnLoadUserInfo(CFirebaseManager.Inst, string.Empty, false);
		}
	}

	/** 결제 정보를 로드한다 */
	public static void LoadPurchaseInfos(System.Action<CFirebaseManager, string, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oFirebaseCallbackDictC.ExReplaceVal(ECallback.LOAD_PURCHASE_INFOS, a_oCallback);

		// 로그인 되었을 경우
		if(CFirebaseManager.Inst.IsLogin) {
			var oNodeList = Factory.MakePurchaseInfoNodes();
			CFirebaseManager.Inst.LoadDB(oNodeList, Func.OnLoadPurchaseInfos);
		} else {
			Func.OnLoadPurchaseInfos(CFirebaseManager.Inst, string.Empty, false);
		}
	}

	/** 지급 아이템 정보를 로드한다 */
	public static void LoadPostItemInfos(System.Action<CFirebaseManager, string, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oFirebaseCallbackDictC.ExReplaceVal(ECallback.LOAD_POST_ITEM_INFOS, a_oCallback);

		// 로그인 되었을 경우
		if(CFirebaseManager.Inst.IsLogin) {
			var oNodeList = Factory.MakePostItemInfoNodes();
			CFirebaseManager.Inst.LoadDB(oNodeList, Func.OnLoadPostItemInfos);
		} else {
			Func.OnLoadPostItemInfos(CFirebaseManager.Inst, string.Empty, false);
		}
	}

	/** 유저 정보를 저장한다 */
	public static void SaveUserInfo(System.Action<CFirebaseManager, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oFirebaseCallbackDictB.ExReplaceVal(ECallback.SAVE_USER_INFO, a_oCallback);

		// 로그인 되었을 경우
		if(CFirebaseManager.Inst.IsLogin) {
			var oNodeList = Factory.MakeUserInfoNodes();

			var oJSONNode = new SimpleJSON.JSONClass();
			oJSONNode.Add(KCDefine.B_KEY_JSON_USER_INFO_DATA, CUserInfoStorage.Inst.UserInfo.ExToMsgPackJSONStr());
			oJSONNode.Add(KCDefine.B_KEY_JSON_GAME_INFO_DATA, CGameInfoStorage.Inst.GameInfo.ExToMsgPackBase64Str());

#if NEWTON_SOFT_JSON_MODULE_ENABLE
			oJSONNode.Add(KCDefine.B_KEY_JSON_COMMON_APP_INFO_DATA, CCommonAppInfoStorage.Inst.AppInfo.ExToMsgPackJSONStr());
			oJSONNode.Add(KCDefine.B_KEY_JSON_COMMON_USER_INFO_DATA, CCommonUserInfoStorage.Inst.UserInfo.ExToMsgPackJSONStr());
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE

			CFirebaseManager.Inst.SaveDB(oNodeList, oJSONNode.ToString(), Func.OnSaveUserInfo);
		} else {
			Func.OnSaveUserInfo(CFirebaseManager.Inst, false);
		}
	}

	/** 결제 정보를 저장한다 */
	public static void SavePurchaseInfos(List<STPurchaseInfo> a_oPurchaseInfoList, System.Action<CFirebaseManager, bool> a_oCallback, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oPurchaseInfoList != null);

		// 결제 정보가 존재 할 경우
		if(a_oPurchaseInfoList != null) {
			CIndicatorManager.Inst.Show();
			Func.m_oFirebaseCallbackDictB.ExReplaceVal(ECallback.SAVE_PURCHASE_INFOS, a_oCallback);

			// 로그인 되었을 경우
			if(CFirebaseManager.Inst.IsLogin) {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
				CFirebaseManager.Inst.SaveDB(Factory.MakePurchaseInfoNodes(), a_oPurchaseInfoList.ExToJSONStr(true), Func.OnSavePurchaseInfos);
#else
				Func.OnSavePurchaseInfos(CFirebaseManager.Inst, false);
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
			} else {
				Func.OnSavePurchaseInfos(CFirebaseManager.Inst, false);
			}
		}
	}

	/** 지급 아이템 정보를 저장한다 */
	public static void SavePostItemInfos(List<STPostItemInfo> a_oPostItemInfoList, System.Action<CFirebaseManager, bool> a_oCallback, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oPostItemInfoList != null);

		// 지급 아이템 정보가 존재 할 경우
		if(a_oPostItemInfoList != null) {
			CIndicatorManager.Inst.Show();
			Func.m_oFirebaseCallbackDictB.ExReplaceVal(ECallback.SAVE_POST_ITEM_INFOS, a_oCallback);

			// 로그인 되었을 경우
			if(CFirebaseManager.Inst.IsLogin) {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
				CFirebaseManager.Inst.SaveDB(Factory.MakePostItemInfoNodes(), a_oPostItemInfoList.ExToJSONStr(true), Func.OnSavePostItemInfos);
#else
				Func.OnSavePostItemInfos(CFirebaseManager.Inst, false);
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
			} else {
				Func.OnSavePostItemInfos(CFirebaseManager.Inst, false);
			}
		}
	}

	/** 파이어 베이스에 로그인 되었을 경우 */
	private static void OnFirebaseLogin(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oFirebaseCallbackDictB.GetValueOrDefault(ECallback.FIREBASE_LOGIN)?.Invoke(a_oSender, a_bIsSuccess);
	}

	/** 파이어 베이스에서 로그아웃 되었을 경우 */
	private static void OnFirebaseLogout(CFirebaseManager a_oSender) {
		CIndicatorManager.Inst.Close();
		Func.m_oFirebaseCallbackDictA.GetValueOrDefault(ECallback.FIREBASE_LOGOUT)?.Invoke(a_oSender);
	}

	/** 유저 정보가 로드 되었을 경우 */
	private static void OnLoadUserInfo(CFirebaseManager a_oSender, string a_oJSONStr, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oFirebaseCallbackDictC.GetValueOrDefault(ECallback.LOAD_USER_INFO)?.Invoke(a_oSender, a_oJSONStr, a_bIsSuccess);
	}

	/** 결제 정보가 로드 되었을 경우 */
	private static void OnLoadPurchaseInfos(CFirebaseManager a_oSender, string a_oJSONStr, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oFirebaseCallbackDictC.GetValueOrDefault(ECallback.LOAD_PURCHASE_INFOS)?.Invoke(a_oSender, a_oJSONStr, a_bIsSuccess);
	}

	/** 지급 아이템 정보가 로드 되었을 경우 */
	private static void OnLoadPostItemInfos(CFirebaseManager a_oSender, string a_oJSONStr, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oFirebaseCallbackDictC.GetValueOrDefault(ECallback.LOAD_POST_ITEM_INFOS)?.Invoke(a_oSender, a_oJSONStr, a_bIsSuccess);
	}

	/** 유저 정보가 저장 되었을 경우 */
	private static void OnSaveUserInfo(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oFirebaseCallbackDictB.GetValueOrDefault(ECallback.SAVE_USER_INFO)?.Invoke(a_oSender, a_bIsSuccess);
	}

	/** 결제 정보가 저장 되었을 경우 */
	private static void OnSavePurchaseInfos(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oFirebaseCallbackDictB.GetValueOrDefault(ECallback.SAVE_PURCHASE_INFOS)?.Invoke(a_oSender, a_bIsSuccess);
	}

	/** 지급 아이템 정보가 저장 되었을 경우 */
	private static void OnSavePostItemInfos(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oFirebaseCallbackDictB.GetValueOrDefault(ECallback.SAVE_POST_ITEM_INFOS)?.Invoke(a_oSender, a_bIsSuccess);
	}
	
#if UNITY_IOS && APPLE_LOGIN_ENABLE
	/** 애플에 로그인 되었을 경우 */
	private static void OnFirebaseAppleLogin(CServicesManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();

		// 로그인 되었을 경우
		if(a_bIsSuccess) {
			CIndicatorManager.Inst.Show();
			CFirebaseManager.Inst.LoginWithApple(a_oSender.AppleUserID, a_oSender.AppleIDToken, Func.OnFirebaseLogin);
		} else {
			Func.OnFirebaseLogin(CFirebaseManager.Inst, false);
		}
	}

	/** 애플에서 로그아웃 되었을 경우 */
	private static void OnFirebaseAppleLogout(CServicesManager a_oSender) {
		CFirebaseManager.Inst.Logout(Func.OnFirebaseLogout);
	}
#endif			// #if UNITY_IOS && APPLE_LOGIN_ENABLE

#if (UNITY_IOS || UNITY_ANDROID) && FACEBOOK_MODULE_ENABLE
	/** 페이스 북에 로그인 되었을 경우 */
	private static void OnFirebaseFacebookLogin(CFacebookManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();

		// 로그인 되었을 경우
		if(a_bIsSuccess) {
			CIndicatorManager.Inst.Show();
			CFirebaseManager.Inst.LoginWithFacebook(a_oSender.AccessToken, Func.OnFirebaseLogin);
		} else {
			Func.OnFirebaseLogin(CFirebaseManager.Inst, false);
		}
	}

	/** 페이스 북에서 로그아웃 되었을 경우 */
	private static void OnFirebaseFacebookLogout(CFacebookManager a_oSender) {
		CFirebaseManager.Inst.Logout(Func.OnFirebaseLogout);
	}
#endif			// #if (UNITY_IOS || UNITY_ANDROID) && FACEBOOK_MODULE_ENABLE
#endif			// #if FIREBASE_MODULE_ENABLE

#if GAME_CENTER_MODULE_ENABLE
	/** 게임 센터 로그인을 처리한다 */
	public static void GameCenterLogin(System.Action<CGameCenterManager, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oGameCenterCallbackDictB.ExReplaceVal(ECallback.GAME_CENTER_LOGIN, a_oCallback);

		CGameCenterManager.Inst.Login(Func.OnGameCenterLogin);
	}

	/** 게임 센터 로그아웃을 처리한다 */
	public static void GameCenterLogout(System.Action<CGameCenterManager> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oGameCenterCallbackDictA.ExReplaceVal(ECallback.GAME_CENTER_LOGOUT, a_oCallback);

		CGameCenterManager.Inst.Logout(Func.OnGameCenterLogout);
	}

	/** 기록을 갱신한다 */
	public static void UpdateRecord(string a_oLeaderboardID, long a_nRecord, System.Action<CGameCenterManager, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oGameCenterCallbackDictB.ExReplaceVal(ECallback.UPDATE_RECORD, a_oCallback);

		CGameCenterManager.Inst.UpdateRecord(a_oLeaderboardID, a_nRecord, Func.OnUpdateRecord);
	}

	/** 업적을 갱신한다 */
	public static void UpdateAchievement(string a_oAchievementID, double a_dblPercent, System.Action<CGameCenterManager, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oGameCenterCallbackDictB.ExReplaceVal(ECallback.UPDATE_ACHIEVEMENT, a_oCallback);

		CGameCenterManager.Inst.UpdateAchievement(a_oAchievementID, a_dblPercent, Func.OnUpdateAchievement);
	}

	/** 게임 센터에 로그인 되었을 경우 */
	private static void OnGameCenterLogin(CGameCenterManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oGameCenterCallbackDictB.GetValueOrDefault(ECallback.GAME_CENTER_LOGIN)?.Invoke(a_oSender, a_bIsSuccess);
	}

	/** 게임 센터에서 로그아웃 되었을 경우 */
	private static void OnGameCenterLogout(CGameCenterManager a_oSender) {
		CIndicatorManager.Inst.Close();
		Func.m_oGameCenterCallbackDictA.GetValueOrDefault(ECallback.GAME_CENTER_LOGOUT)?.Invoke(a_oSender);
	}
	
	/** 기록이 갱신 되었을 경우 */
	private static void OnUpdateRecord(CGameCenterManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oGameCenterCallbackDictB.GetValueOrDefault(ECallback.UPDATE_RECORD)?.Invoke(a_oSender, a_bIsSuccess);
	}

	/** 업적이 갱신 되었을 경우 */
	private static void OnUpdateAchievement(CGameCenterManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oGameCenterCallbackDictB.GetValueOrDefault(ECallback.UPDATE_ACHIEVEMENT)?.Invoke(a_oSender, a_bIsSuccess);
	}
#endif			// #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	/** 상품을 결제한다 */
	public static void PurchaseProduct(int a_nID, System.Action<CPurchaseManager, string, bool> a_oCallback, bool a_bIsEnableAssert = true) {
		Func.PurchaseProduct(CProductInfoTable.Inst.GetProductInfo(a_nID).m_oID, a_oCallback, a_bIsEnableAssert);
	}

	/** 상품을 결제한다 */
	public static void PurchaseProduct(ESaleProductKinds a_eSaleProductKinds, System.Action<CPurchaseManager, string, bool> a_oCallback, bool a_bIsEnableAssert = true) {
		int nID = Access.GetSaleProductID(a_eSaleProductKinds);
		CAccess.Assert(!a_bIsEnableAssert || KDefine.G_KINDS_SALE_PIT_SALE_PRODUCT_LIST.ExIsValidIdx(nID));

		// 상품이 존재 할 경우
		if(KDefine.G_KINDS_SALE_PIT_SALE_PRODUCT_LIST.ExIsValidIdx(nID)) {
			Func.PurchaseProduct(nID, a_oCallback, a_bIsEnableAssert);
		}
	}
	
	/** 상품을 결제한다 */
	public static void PurchaseProduct(string a_oID, System.Action<CPurchaseManager, string, bool> a_oCallback, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oID.ExIsValid());

		// 식별자가 유효 할 경우
		if(a_oID.ExIsValid()) {
			CIndicatorManager.Inst.Show();
			Func.m_oPurchaseCallbackDictA.ExReplaceVal(ECallback.PURCHASE, a_oCallback);

			CPurchaseManager.Inst.PurchaseProduct(a_oID, Func.OnPurchaseProduct);
		}
	}

	/** 상품을 복원한다 */
	public static void RestoreProducts(System.Action<CPurchaseManager, List<Product>, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oPurchaseCallbackDictB.ExReplaceVal(ECallback.RESTORE, a_oCallback);
		
		CPurchaseManager.Inst.RestoreProducts(Func.OnRestoreProducts);
	}

	/** 상품이 결제 되었을 경우 */
	private static void OnPurchaseProduct(CPurchaseManager a_oSender, string a_oProductID, bool a_bIsSuccess) {
		CPurchaseManager.Inst.ConfirmPurchase(a_oProductID, (a_oSender, a_oConfirmProductID, a_bIsSuccess) => {
			CIndicatorManager.Inst.Close();
			Func.m_oPurchaseCallbackDictA.GetValueOrDefault(ECallback.PURCHASE)?.Invoke(a_oSender, a_oConfirmProductID, a_bIsSuccess);
		});
	}

	/** 상품이 복원 되었을 경우 */
	private static void OnRestoreProducts(CPurchaseManager a_oSender, List<Product> a_oProductList, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oPurchaseCallbackDictB.GetValueOrDefault(ECallback.RESTORE)?.Invoke(a_oSender, a_oProductList, a_bIsSuccess);
	}
#endif			// #if PURCHASE_MODULE_ENABLE

#if (UNITY_STANDALONE && GOOGLE_SHEET_ENABLE) && (DEBUG || DEVELOPMENT_BUILD)
	/** 구글 시트를 로드한다 */
	public static void LoadGoogleSheet(string a_oID, List<(string, int)> a_oInfoList, System.Action<CServicesManager, GstuSpreadSheet, string, Dictionary<string, (SimpleJSON.JSONNode, bool)>> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oServicesCallbackDict.ExReplaceVal(ECallback.LOAD_GOOGLE_SHEET, a_oCallback);

		Func.m_oGoogleSheetInfoList.Clear();
		Func.m_oGoogleSheetJSONNodeInfoDict.Clear();

		for(int i = 0; i < a_oInfoList.Count; ++i) {
			Func.m_oGoogleSheetInfoList.ExAddVal((a_oInfoList[i].Item1, a_oInfoList[i].Item2, a_oInfoList[i].Item2));
		}

		CServicesManager.Inst.LoadGoogleSheet(a_oID, a_oInfoList[KCDefine.B_VAL_0_INT].Item1, Func.OnLoadGoogleSheet, KCDefine.B_VAL_0_INT, a_oInfoList[KCDefine.B_VAL_0_INT].Item2);
	}
	
	/** 구글 시트를 로드했을 경우 */
	private static void OnLoadGoogleSheet(CServicesManager a_oSender, GstuSpreadSheet a_oGoogleSheet, STGoogleSheetInfo a_stGoogleSheetInfo, bool a_bIsSuccess) {
		int nIdx = Func.m_oGoogleSheetInfoList.FindIndex((a_oGoogleSheetInfo) =>a_oGoogleSheetInfo.Item1.Equals(a_stGoogleSheetInfo.m_oName));
		CAccess.Assert(Func.m_oGoogleSheetInfoList.ExIsValidIdx(nIdx));

		Func.m_oGoogleSheetInfoList[nIdx] = (Func.m_oGoogleSheetInfoList[nIdx].Item1, Func.m_oGoogleSheetInfoList[nIdx].Item2 - a_stGoogleSheetInfo.m_nNumCells, Func.m_oGoogleSheetInfoList[nIdx].Item3);
		var oJSONNodeInfo = Func.m_oGoogleSheetJSONNodeInfoDict.ContainsKey(a_stGoogleSheetInfo.m_oName) ? Func.m_oGoogleSheetJSONNodeInfoDict[a_stGoogleSheetInfo.m_oName] : (new SimpleJSON.JSONArray(), a_bIsSuccess);

		// 데이터를 로드했을 경우
		if(a_bIsSuccess && a_oGoogleSheet.rows.primaryDictionary.Count > KCDefine.B_VAL_0_INT) {
			int nStartIdx = Func.m_oGoogleSheetJSONNodeInfoDict.ContainsKey(a_stGoogleSheetInfo.m_oName)? KCDefine.B_VAL_0_INT : KCDefine.B_VAL_1_INT;

			// 키 데이터가 없을 경우
			if(oJSONNodeInfo.Item1.Count <= KCDefine.B_VAL_0_INT) {
				var oKeys = new SimpleJSON.JSONArray();

				for(int i = 0; i < a_oGoogleSheet.rows[KCDefine.B_VAL_1_INT].Count; ++i) {
					oKeys.Add(a_oGoogleSheet.rows[KCDefine.B_VAL_1_INT][i].value);
				}

				oJSONNodeInfo.Item1.Add(oKeys);
			}

			for(int i = nStartIdx; i < a_oGoogleSheet.rows.primaryDictionary.Count; ++i) {
				int nSrcIdx = a_stGoogleSheetInfo.m_nSrcIdx + i;
				var oJSONClass = new SimpleJSON.JSONClass();

				for(int j = 0; j < a_oGoogleSheet.rows[nSrcIdx + KCDefine.B_VAL_1_INT].Count; ++j) {
					oJSONClass.Add(oJSONNodeInfo.Item1[KCDefine.B_VAL_0_INT][j], a_oGoogleSheet.rows[nSrcIdx + KCDefine.B_VAL_1_INT][j].value);
				}

				oJSONNodeInfo.Item1.Add(oJSONClass);
			}
		}

		oJSONNodeInfo.Item2 = a_bIsSuccess;
		Func.m_oGoogleSheetJSONNodeInfoDict.ExReplaceVal(a_stGoogleSheetInfo.m_oName, oJSONNodeInfo);

		// 로드 할 데이터가 존재 할 경우
		if(a_bIsSuccess && Func.m_oGoogleSheetInfoList[nIdx].Item2 > KCDefine.B_VAL_0_INT) {
			CServicesManager.Inst.LoadGoogleSheet(a_stGoogleSheetInfo.m_oID, Func.m_oGoogleSheetInfoList[nIdx].Item1, Func.OnLoadGoogleSheet, Func.m_oGoogleSheetInfoList[nIdx].Item3 - Func.m_oGoogleSheetInfoList[nIdx].Item2, Func.m_oGoogleSheetInfoList[nIdx].Item2);
		} else {
			Func.m_oGoogleSheetJSONNodeInfoDict.GetValueOrDefault(Func.m_oGoogleSheetInfoList[nIdx].Item1).Item1.Remove(KCDefine.B_VAL_0_INT);
			Func.m_oGoogleSheetInfoList.ExRemoveValAt(nIdx);

			// 구글 시트 정보가 존재 할 경우
			if(Func.m_oGoogleSheetInfoList.ExIsValid()) {
				CServicesManager.Inst.LoadGoogleSheet(a_stGoogleSheetInfo.m_oID, Func.m_oGoogleSheetInfoList[KCDefine.B_VAL_0_INT].Item1, Func.OnLoadGoogleSheet, KCDefine.B_VAL_0_INT, Func.m_oGoogleSheetInfoList[KCDefine.B_VAL_0_INT].Item2);
			} else {
				CIndicatorManager.Inst.Close();
				Func.m_oServicesCallbackDict.GetValueOrDefault(ECallback.LOAD_GOOGLE_SHEET)?.Invoke(a_oSender, a_oGoogleSheet, a_stGoogleSheetInfo.m_oID, Func.m_oGoogleSheetJSONNodeInfoDict);
			}
		}
	}
#endif			// #if (UNITY_STANDALONE && GOOGLE_SHEET_ENABLE) && (DEBUG || DEVELOPMENT_BUILD)
	#endregion			// 조건부 클래스 함수
}
#endif			// #if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
