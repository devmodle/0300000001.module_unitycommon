#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif			// #if PURCHASE_MODULE_ENABLE

#if PLAYFAB_MODULE_ENABLE
using PlayFab.SharedModels;
#endif			// #if PLAYFAB_MODULE_ENABLE

#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
using GoogleSheetsToUnity;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)

/** 기본 함수 */
public static partial class Func {
	/** 콜백 */
	private enum ECallback {
		NONE = -1,

#if ADS_MODULE_ENABLE
		SHOW_BANNER_ADS,
		SHOW_REWARD_ADS,
		SHOW_FULLSCREEN_ADS,
#endif			// #if ADS_MODULE_ENABLE

#if UNITY_IOS && APPLE_LOGIN_ENABLE
		APPLE_LOGIN,
		APPLE_LOGOUT,
#endif			// #if UNITY_IOS && APPLE_LOGIN_ENABLE

#if FACEBOOK_MODULE_ENABLE
		FACEBOOK_LOGIN,
		FACEBOOK_LOGOUT,
#endif			// #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
		FIREBASE_LOGIN,
		FIREBASE_LOGOUT,

		LOAD_USER_INFO,
		LOAD_TARGET_INFOS,
		LOAD_PURCHASE_INFOS,

		SAVE_USER_INFO,
		SAVE_TARGET_INFOS,
		SAVE_PURCHASE_INFOS,
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

#if PLAYFAB_MODULE_ENABLE
		PLAYFAB_LOGIN,
		PLAYFAB_LOGOUT,
#endif			// #if PLAYFAB_MODULE_ENABLE

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
	private static Dictionary<ECallback, System.Action<CAdsManager, bool>> m_oAdsCallbackDict01 = new Dictionary<ECallback, System.Action<CAdsManager, bool>>();
	private static Dictionary<ECallback, System.Action<CAdsManager, STAdsRewardInfo, bool>> m_oAdsCallbackDict02 = new Dictionary<ECallback, System.Action<CAdsManager, STAdsRewardInfo, bool>>();
#endif			// #if ADS_MODULE_ENABLE

#if UNITY_IOS && APPLE_LOGIN_ENABLE
	private static Dictionary<ECallback, System.Action<CServicesManager>> m_oServicesCallbackDict01 = new Dictionary<ECallback, System.Action<CServicesManager>>();
	private static Dictionary<ECallback, System.Action<CServicesManager, bool>> m_oServicesCallbackDict02 = new Dictionary<ECallback, System.Action<CServicesManager, bool>>();
#endif			// #if UNITY_IOS && APPLE_LOGIN_ENABLE

#if FACEBOOK_MODULE_ENABLE
	private static Dictionary<ECallback, System.Action<CFacebookManager>> m_oFacebookCallbackDict01 = new Dictionary<ECallback, System.Action<CFacebookManager>>();
	private static Dictionary<ECallback, System.Action<CFacebookManager, bool>> m_oFacebookCallbackDict02 = new Dictionary<ECallback, System.Action<CFacebookManager, bool>>();
#endif			// #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
	private static Dictionary<ECallback, System.Action<CFirebaseManager>> m_oFirebaseCallbackDict01 = new Dictionary<ECallback, System.Action<CFirebaseManager>>();
	private static Dictionary<ECallback, System.Action<CFirebaseManager, bool>> m_oFirebaseCallbackDict02 = new Dictionary<ECallback, System.Action<CFirebaseManager, bool>>();
	private static Dictionary<ECallback, System.Action<CFirebaseManager, string, bool>> m_oFirebaseCallbackDict03 = new Dictionary<ECallback, System.Action<CFirebaseManager, string, bool>>();
#endif			// #if FIREBASE_MODULE_ENABLE

#if GAME_CENTER_MODULE_ENABLE
	private static Dictionary<ECallback, System.Action<CGameCenterManager>> m_oGameCenterCallbackDict01 = new Dictionary<ECallback, System.Action<CGameCenterManager>>();
	private static Dictionary<ECallback, System.Action<CGameCenterManager, bool>> m_oGameCenterCallbackDict02 = new Dictionary<ECallback, System.Action<CGameCenterManager, bool>>();
#endif			// #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	private static Dictionary<ECallback, System.Action<CPurchaseManager, string, bool>> m_oPurchaseCallbackDict01 = new Dictionary<ECallback, System.Action<CPurchaseManager, string, bool>>();
	private static Dictionary<ECallback, System.Action<CPurchaseManager, List<Product>, bool>> m_oPurchaseCallbackDict02 = new Dictionary<ECallback, System.Action<CPurchaseManager, List<Product>, bool>>();
#endif			// #if PURCHASE_MODULE_ENABLE

#if PLAYFAB_MODULE_ENABLE
	private static Dictionary<ECallback, System.Action<CPlayfabManager>> m_oPlayfabCallbackDict01 = new Dictionary<ECallback, System.Action<CPlayfabManager>>();
	private static Dictionary<ECallback, System.Action<CPlayfabManager, bool>> m_oPlayfabCallbackDict02 = new Dictionary<ECallback, System.Action<CPlayfabManager, bool>>();
	private static Dictionary<ECallback, System.Action<CPlayfabManager, PlayFabResultCommon, bool>> m_oPlayfabCallbackDict03 = new Dictionary<ECallback, System.Action<CPlayfabManager, PlayFabResultCommon, bool>>();
#endif			// #if PLAYFAB_MODULE_ENABLE

#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	private static List<(string, int, int)> m_oGoogleSheetInfoList = new List<(string, int, int)>();
	private static Dictionary<string, (SimpleJSON.JSONNode, bool)> m_oGoogleSheetJSONNodeInfoDict = new Dictionary<string, (SimpleJSON.JSONNode, bool)>();
	private static Dictionary<ECallback, System.Action<CServicesManager, GstuSpreadSheet, string, Dictionary<string, (SimpleJSON.JSONNode, bool)>>> m_oServicesCallbackDict = new Dictionary<ECallback, System.Action<CServicesManager, GstuSpreadSheet, string, Dictionary<string, (SimpleJSON.JSONNode, bool)>>>();
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion			// 클래스 변수

	#region 클래스 함수
	/** 문자열 테이블을 설정한다 */
	public static void SetupStrTable() {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
		Func.SetupStrTable(CCommonAppInfoStorage.Inst.CountryCode, CCommonAppInfoStorage.Inst.SystemLanguage);
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
	}

	/** 문자열 테이블을 설정한다 */
	public static void SetupStrTable(string a_oCountryCode, SystemLanguage a_eSystemLanguage, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oCountryCode.ExIsValid());
		
		// 국가 코드가 존재 할 경우
		if(a_oCountryCode.ExIsValid()) {
			CStrTable.Inst.LoadStrsFromRes(CFactory.MakeLocalizePath(KCDefine.U_BASE_TABLE_P_G_LOCALIZE_COMMON_STR, KCDefine.U_TABLE_P_G_ENGLISH_COMMON_STR, a_oCountryCode, a_eSystemLanguage.ToString()));
		}
	}

	/** 터치 전달자를 설정한다 */
	public static void SetupTouchDispatchers(List<(GameObject, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>)> a_oKeyInfoList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oKeyInfoList.ExIsValid());

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid()) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oKeyInfoList[i].Item1?.GetComponentInChildren<CTouchDispatcher>()?.ExSetBeginCallback(a_oKeyInfoList[i].Item2, a_bIsEnableAssert);
				a_oKeyInfoList[i].Item1?.GetComponentInChildren<CTouchDispatcher>()?.ExSetMoveCallback(a_oKeyInfoList[i].Item3, a_bIsEnableAssert);
				a_oKeyInfoList[i].Item1?.GetComponentInChildren<CTouchDispatcher>()?.ExSetEndCallback(a_oKeyInfoList[i].Item4, a_bIsEnableAssert);
			}
		}
	}

	/** 터치 전달자를 설정한다 */
	public static void SetupTouchDispatchers(List<(string, GameObject, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>)> a_oKeyInfoList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oKeyInfoList.ExIsValid());

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid()) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oKeyInfoList[i].Item2?.ExFindComponent<CTouchDispatcher>(a_oKeyInfoList[i].Item1)?.ExSetBeginCallback(a_oKeyInfoList[i].Item3, a_bIsEnableAssert);
				a_oKeyInfoList[i].Item2?.ExFindComponent<CTouchDispatcher>(a_oKeyInfoList[i].Item1)?.ExSetMoveCallback(a_oKeyInfoList[i].Item4, a_bIsEnableAssert);
				a_oKeyInfoList[i].Item2?.ExFindComponent<CTouchDispatcher>(a_oKeyInfoList[i].Item1)?.ExSetEndCallback(a_oKeyInfoList[i].Item5, a_bIsEnableAssert);
			}
		}
	}

	/** 배경음을 재생한다 */
	public static CSnd PlayBGSnd(EResKinds a_eResKinds, float a_fVolume = KCDefine.B_VAL_0_REAL, bool a_bIsLoop = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_eResKinds.ExIsValid());

		try {
			return Func.PlayBGSnd(a_eResKinds, CSceneManager.ActiveSceneMainCamera.transform.position, a_fVolume, a_bIsLoop, a_bIsEnableAssert);
		} catch(System.Exception oException) {
			CFunc.ShowLog($"Func.PlayBGSnd Exception: {oException.Message}");
		}

		return null;
	}

	/** 배경음을 재생한다 */
	public static CSnd PlayBGSnd(EResKinds a_eResKinds, Vector3 a_stPos, float a_fVolume = KCDefine.B_VAL_0_REAL, bool a_bIsLoop = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_eResKinds.ExIsValid());
		return CResInfoTable.Inst.TryGetResInfo(a_eResKinds, out STResInfo stResInfo) ? CSndManager.Inst.PlayBGSnd(stResInfo.m_oResPath, a_stPos, a_fVolume, a_bIsLoop, a_bIsEnableAssert) : null;
	}

	/** 효과음을 재생한다 */
	public static CSnd PlayFXSnds(EResKinds a_eResKinds, float a_fVolume = KCDefine.B_VAL_0_REAL, bool a_bIsLoop = false, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_eResKinds.ExIsValid());

		try {
			return Func.PlayFXSnds(a_eResKinds, CSceneManager.ActiveSceneMainCamera.transform.position, a_fVolume, a_bIsLoop, a_bIsEnableAssert);
		} catch(System.Exception oException) {
			CFunc.ShowLog($"Func.PlayFXSnds Exception: {oException.Message}");
		}

		return null;
	}

	/** 효과음을 재생한다 */
	public static CSnd PlayFXSnds(EResKinds a_eResKinds, Vector3 a_stPos, float a_fVolume = KCDefine.B_VAL_0_REAL, bool a_bIsLoop = false, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_eResKinds.ExIsValid());
		return CResInfoTable.Inst.TryGetResInfo(a_eResKinds, out STResInfo stResInfo) ? CSndManager.Inst.PlayFXSnds(stResInfo.m_oResPath, a_stPos, a_fVolume, a_bIsLoop, a_bIsEnableAssert) : null;
	}

	/** 경고 팝업을 출력한다 */
	public static void ShowAlertPopup(CAlertPopup.STParams a_stParams) {
		// 경고 팝업이 없을 경우
		if(CSceneManager.ScreenPopupUIs.ExFindChild(KCDefine.U_OBJ_N_ALERT_POPUP) == null) {
			var oAlertPopup = CAlertPopup.Create<CAlertPopup>(KCDefine.U_OBJ_N_ALERT_POPUP, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_G_ALERT_POPUP), CSceneManager.ScreenPopupUIs, a_stParams);
			oAlertPopup.Show(null, null);
		}
	}

	/** 경고 팝업을 출력한다 */
	public static void ShowAlertPopup(string a_oMsg, System.Action<CAlertPopup, bool> a_oCallback, bool a_bIsEnableCancelBtn = true) {
		Func.ShowAlertPopup(new CAlertPopup.STParams() {
			m_oTitle = CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_NOTI_TEXT),
			m_oMsg = a_oMsg,
			m_oOKBtnText = CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_OK_TEXT),
			m_oCancelBtnText = a_bIsEnableCancelBtn ? CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_CANCEL_TEXT) : string.Empty,

			m_oCallbackDict = new Dictionary<CAlertPopup.ECallback, System.Action<CAlertPopup, bool>>() {
				[CAlertPopup.ECallback.OK_CANCEL] = a_oCallback
			}
		});
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
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_LOGIN_SUCCESS_MSG), a_oCallback, false);
	}

	/** 로그인 실패 팝업을 출력한다 */
	public static void ShowLoginFailPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_LOGIN_SUCCESS_MSG), a_oCallback, false);
	}

	/** 로그아웃 성공 팝업을 출력한다 */
	public static void ShowLogoutSuccessPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_LOGOUT_SUCCESS_MSG), a_oCallback, false);
	}

	/** 로그아웃 실패 팝업을 출력한다 */
	public static void ShowLogoutFailPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_LOGOUT_SUCCESS_MSG), a_oCallback, false);
	}

	/** 로드 성공 팝업을 출력한다 */
	public static void ShowLoadSuccessPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_LOAD_SUCCESS_MSG), a_oCallback, false);
	}

	/** 로드 실패 팝업을 출력한다 */
	public static void ShowLoadFailPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_LOAD_FAIL_MSG), a_oCallback, false);
	}

	/** 저장 성공 팝업을 출력한다 */
	public static void ShowSaveSuccessPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_SAVE_SUCCESS_MSG), a_oCallback, false);
	}

	/** 저장 실패 팝업을 출력한다 */
	public static void ShowSaveFailPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_SAVE_FAIL_MSG), a_oCallback, false);
	}

	/** 결제 성공 팝업을 출력한다 */
	public static void ShowPurchaseSuccessPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_PURCHASE_SUCCESS_MSG), a_oCallback, false);
	}

	/** 결제 실패 팝업을 출력한다 */
	public static void ShowPurchaseFailPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_PURCHASE_FAIL_MSG), a_oCallback, false);
	}

	/** 복원 성공 팝업을 출력한다 */
	public static void ShowRestoreSuccessPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_RESTORE_SUCCESS_MSG), a_oCallback, false);
	}

	/** 복원 실패 팝업을 출력한다 */
	public static void ShowRestoreFailPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_RESTORE_FAIL_MSG), a_oCallback, false);
	}
	#endregion			// 클래스 함수

	#region 제네릭 클래스 함수
	/** 터치 전달자를 설정한다 */
	public static void SetupTouchDispatchers<K>(List<(K, GameObject, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>)> a_oKeyInfoList, Dictionary<K, CTouchDispatcher> a_oOutTouchDispatcherDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutTouchDispatcherDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutTouchDispatcherDict != null) {
			CFunc.SetupComponents(Factory.MakeKeyInfos(a_oKeyInfoList), a_oOutTouchDispatcherDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutTouchDispatcherDict[a_oKeyInfoList[i].Item1]?.ExSetBeginCallback(a_oKeyInfoList[i].Item3, a_bIsEnableAssert);
				a_oOutTouchDispatcherDict[a_oKeyInfoList[i].Item1]?.ExSetMoveCallback(a_oKeyInfoList[i].Item4, a_bIsEnableAssert);
				a_oOutTouchDispatcherDict[a_oKeyInfoList[i].Item1]?.ExSetEndCallback(a_oKeyInfoList[i].Item5, a_bIsEnableAssert);
			}
		}
	}

	/** 터치 전달자를 설정한다 */
	public static void SetupTouchDispatchers<K>(List<(K, string, GameObject, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>)> a_oKeyInfoList, Dictionary<K, CTouchDispatcher> a_oOutTouchDispatcherDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutTouchDispatcherDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutTouchDispatcherDict != null) {
			CFunc.SetupComponents(Factory.MakeKeyInfos(a_oKeyInfoList), a_oOutTouchDispatcherDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutTouchDispatcherDict[a_oKeyInfoList[i].Item1]?.ExSetBeginCallback(a_oKeyInfoList[i].Item4, a_bIsEnableAssert);
				a_oOutTouchDispatcherDict[a_oKeyInfoList[i].Item1]?.ExSetMoveCallback(a_oKeyInfoList[i].Item5, a_bIsEnableAssert);
				a_oOutTouchDispatcherDict[a_oKeyInfoList[i].Item1]?.ExSetEndCallback(a_oKeyInfoList[i].Item6, a_bIsEnableAssert);
			}
		}
	}

	/** 터치 전달자를 설정한다 */
	public static void SetupTouchDispatchers<K>(List<(K, string, GameObject, GameObject, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>)> a_oKeyInfoList, Dictionary<K, CTouchDispatcher> a_oOutTouchDispatcherDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutTouchDispatcherDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutTouchDispatcherDict != null) {
			CFunc.SetupComponents(Factory.MakeKeyInfos(a_oKeyInfoList), a_oOutTouchDispatcherDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutTouchDispatcherDict[a_oKeyInfoList[i].Item1]?.ExSetBeginCallback(a_oKeyInfoList[i].Item5, a_bIsEnableAssert);
				a_oOutTouchDispatcherDict[a_oKeyInfoList[i].Item1]?.ExSetMoveCallback(a_oKeyInfoList[i].Item6, a_bIsEnableAssert);
				a_oOutTouchDispatcherDict[a_oKeyInfoList[i].Item1]?.ExSetEndCallback(a_oKeyInfoList[i].Item7, a_bIsEnableAssert);
			}
		}
	}

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
			Func.m_oAdsCallbackDict01.ExReplaceVal(ECallback.SHOW_BANNER_ADS, a_oCallback);
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
			CIndicatorManager.Inst.Show();

			CSceneManager.ActiveSceneManager.ExLateCallFunc((a_oSender) => {
				Func.m_bIsWatchRewardAds = false;
				Func.m_stAdsRewardInfo = KCDefine.U_INVALID_ADS_REWARD_INFO;

				Func.m_oAdsCallbackDict02.ExReplaceVal(ECallback.SHOW_REWARD_ADS, a_oCallback);
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
			CIndicatorManager.Inst.Show();

			CSceneManager.ActiveSceneManager.ExLateCallFunc((a_oSender) => {
				CIndicatorManager.Inst.Close();

				// 전면 광고 출력이 가능 할 경우
				if(CAppInfoStorage.Inst.IsEnableShowFullscreenAds) {
					Func.m_bIsWatchFullscreenAds = true;
					Func.m_oAdsCallbackDict01.ExReplaceVal(ECallback.SHOW_FULLSCREEN_ADS, a_oCallback);

					CAdsManager.Inst.ShowFullscreenAds(a_eAdsPlatform, null, Func.OnCloseFullscreenAds);
				} else {
					CFunc.Invoke(ref a_oCallback, CAdsManager.Inst, false);
				}
			}, KCDefine.B_VAL_2_REAL, true);
		} else {
			Func.IncrAdsSkipTimes(KCDefine.B_VAL_1_INT);
			CFunc.Invoke(ref a_oCallback, CAdsManager.Inst, false);
		}
	}

	/** 배너 광고가 출력 되었을 경우 */
	private static void OnShowBannerAds(CAdsManager a_oSender, bool a_bIsSuccess) {
		Func.m_oAdsCallbackDict01.GetValueOrDefault(ECallback.SHOW_BANNER_ADS)?.Invoke(a_oSender, a_bIsSuccess);
	}

	/** 보상 광고가 닫혔을 경우 */
	private static void OnCloseRewardAds(CAdsManager a_oSender) {
		CIndicatorManager.Inst.Close();
		CAppInfoStorage.Inst.PrevRewardAdsTime = System.DateTime.Now;

		Func.IncrRewardAdsWatchTimes(KCDefine.B_VAL_1_INT);
		CAppInfoStorage.Inst.SaveAppInfo();

		Func.m_oAdsCallbackDict02.GetValueOrDefault(ECallback.SHOW_REWARD_ADS)?.Invoke(a_oSender, Func.m_stAdsRewardInfo, Func.m_bIsWatchRewardAds);
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

		Func.IncrFullscreenAdsWatchTimes(KCDefine.B_VAL_1_INT);
		CAppInfoStorage.Inst.SaveAppInfo();

		Func.m_oAdsCallbackDict01.GetValueOrDefault(ECallback.SHOW_FULLSCREEN_ADS)?.Invoke(a_oSender, Func.m_bIsWatchFullscreenAds);
	}
#endif			// #if ADS_MODULE_ENABLE

#if UNITY_IOS && APPLE_LOGIN_ENABLE
	/** 애플 로그인을 처리한다 */
	public void AppleLogin(System.Action<CServicesManager, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oServicesCallbackDict02.ExReplaceVal(ECallback.APPLE_LOGIN, a_oCallback);

		CServicesManager.Inst.LoginWithApple(Func.OnAppleLogin);
	}

	/** 애플 로그아웃을 처리한다 */
	public static void AppleLogout(System.Action<CServicesManager> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oServicesCallbackDict01.ExReplaceVal(ECallback.APPLE_LOGOUT, a_oCallback);

		CServicesManager.Inst.LogoutWithApple(Func.OnAppleLogout);
	}

	/** 애플에 로그인 되었을 경우 */
	private static void OnAppleLogin(CServicesManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oServicesCallbackDict02.GetValueOrDefault(ECallback.APPLE_LOGIN)?.Invoke(a_oSender, a_bIsSuccess);
	}

	/** 애플에서 로그아웃 되었을 경우 */
	private static void OnAppleLogout(CServicesManager a_oSender) {
		CIndicatorManager.Inst.Close();
		Func.m_oServicesCallbackDict01.GetValueOrDefault(ECallback.APPLE_LOGOUT)?.Invoke(a_oSender);
	}
#endif			// #if UNITY_IOS && APPLE_LOGIN_ENABLE

#if FACEBOOK_MODULE_ENABLE
	/** 페이스 북 로그인을 처리한다 */
	public static void FacebookLogin(System.Action<CFacebookManager, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oFacebookCallbackDict02.ExReplaceVal(ECallback.FACEBOOK_LOGIN, a_oCallback);

		CFacebookManager.Inst.Login(KCDefine.U_KEY_FACEBOOK_PERMISSION_LIST, Func.OnFacebookLogin);
	}

	/** 페이스 북 로그아웃을 처리한다 */
	public static void FacebookLogout(System.Action<CFacebookManager> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oFacebookCallbackDict01.ExReplaceVal(ECallback.FACEBOOK_LOGOUT, a_oCallback);

		CFacebookManager.Inst.Logout(Func.OnFacebookLogout);
	}

	/** 페이스 북에 로그인 되었을 경우 */
	private static void OnFacebookLogin(CFacebookManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oFacebookCallbackDict02.GetValueOrDefault(ECallback.FACEBOOK_LOGIN)?.Invoke(a_oSender, a_bIsSuccess);
	}

	/** 페이스 북에서 로그아웃 되었을 경우 */
	private static void OnFacebookLogout(CFacebookManager a_oSender) {
		CIndicatorManager.Inst.Close();
		Func.m_oFacebookCallbackDict01.GetValueOrDefault(ECallback.FACEBOOK_LOGOUT)?.Invoke(a_oSender);
	}
#endif			// #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
	/** 파이어 베이스 로그인을 처리한다 */
	public static void FirebaseLogin(System.Action<CFirebaseManager, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oFirebaseCallbackDict02.ExReplaceVal(ECallback.FIREBASE_LOGIN, a_oCallback);

#if UNITY_IOS && APPLE_LOGIN_ENABLE
		Func.AppleLogin(Func.OnFirebaseAppleLogin);
#elif (UNITY_IOS || UNITY_ANDROID) && FACEBOOK_MODULE_ENABLE
		Func.FacebookLogin(Func.OnFirebaseFacebookLogin);
#else
		CFirebaseManager.Inst.Login(CCommonAppInfoStorage.Inst.AppInfo.DeviceID, Func.OnFirebaseLogin);
#endif			// #if UNITY_IOS && APPLE_LOGIN_ENABLE
	}

	/** 파이어 베이스 로그아웃을 처리한다 */
	public static void FirebaseLogout(System.Action<CFirebaseManager> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oFirebaseCallbackDict01.ExReplaceVal(ECallback.FIREBASE_LOGOUT, a_oCallback);

#if UNITY_IOS && APPLE_LOGIN_ENABLE
		Func.AppleLogout(Func.OnFirebaseAppleLogout);
#elif (UNITY_IOS || UNITY_ANDROID) && FACEBOOK_MODULE_ENABLE
		Func.FacebookLogout(Func.OnFirebaseFacebookLogout);
#else
		CFirebaseManager.Inst.Logout(Func.OnFirebaseLogout);
#endif			// #if UNITY_IOS && APPLE_LOGIN_ENABLE
	}

	/** 유저 정보를 로드한다 */
	public static void LoadUserInfo(System.Action<CFirebaseManager, string, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oFirebaseCallbackDict03.ExReplaceVal(ECallback.LOAD_USER_INFO, a_oCallback);

		// 로그인 되었을 경우
		if(CFirebaseManager.Inst.IsLogin) {
			CFirebaseManager.Inst.LoadDatas(Factory.MakeUserInfoNodes(), Func.OnLoadUserInfo);
		} else {
			Func.OnLoadUserInfo(CFirebaseManager.Inst, string.Empty, false);
		}
	}

	/** 타겟 정보를 로드한다 */
	public static void LoadTargetInfos(System.Action<CFirebaseManager, string, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oFirebaseCallbackDict03.ExReplaceVal(ECallback.LOAD_TARGET_INFOS, a_oCallback);

		// 로그인 되었을 경우
		if(CFirebaseManager.Inst.IsLogin) {
			CFirebaseManager.Inst.LoadDatas(Factory.MakeTargetInfoNodes(), Func.OnLoadTargetInfos);
		} else {
			Func.OnLoadTargetInfos(CFirebaseManager.Inst, string.Empty, false);
		}
	}

	/** 결제 정보를 로드한다 */
	public static void LoadPurchaseInfos(System.Action<CFirebaseManager, string, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oFirebaseCallbackDict03.ExReplaceVal(ECallback.LOAD_PURCHASE_INFOS, a_oCallback);

		// 로그인 되었을 경우
		if(CFirebaseManager.Inst.IsLogin) {
			CFirebaseManager.Inst.LoadDatas(Factory.MakePurchaseInfoNodes(), Func.OnLoadPurchaseInfos);
		} else {
			Func.OnLoadPurchaseInfos(CFirebaseManager.Inst, string.Empty, false);
		}
	}

	/** 유저 정보를 저장한다 */
	public static void SaveUserInfo(System.Action<CFirebaseManager, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oFirebaseCallbackDict02.ExReplaceVal(ECallback.SAVE_USER_INFO, a_oCallback);

		// 로그인 되었을 경우
		if(CFirebaseManager.Inst.IsLogin) {
			var oNodeList = Factory.MakeUserInfoNodes();

			var oJSONNode = new SimpleJSON.JSONClass();
			oJSONNode.Add(KCDefine.B_KEY_JSON_USER_INFO_DATA, CUserInfoStorage.Inst.UserInfo.ExToMsgPackBase64Str());
			oJSONNode.Add(KCDefine.B_KEY_JSON_GAME_INFO_DATA, CGameInfoStorage.Inst.GameInfo.ExToMsgPackBase64Str());

#if NEWTON_SOFT_JSON_MODULE_ENABLE
			oJSONNode.Add(KCDefine.B_KEY_JSON_COMMON_APP_INFO_DATA, CCommonAppInfoStorage.Inst.AppInfo.ExToMsgPackBase64Str());
			oJSONNode.Add(KCDefine.B_KEY_JSON_COMMON_USER_INFO_DATA, CCommonUserInfoStorage.Inst.UserInfo.ExToMsgPackBase64Str());
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE

			CFirebaseManager.Inst.SaveDatas(oNodeList, oJSONNode.ToString(), Func.OnSaveUserInfo);
		} else {
			Func.OnSaveUserInfo(CFirebaseManager.Inst, false);
		}
	}

	/** 타겟 정보를 저장한다 */
	public static void SaveTargetInfos(Dictionary<ulong, STTargetInfo> a_oTargetInfoDict, System.Action<CFirebaseManager, bool> a_oCallback, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oTargetInfoDict != null);

		// 타겟 정보가 존재 할 경우
		if(a_oTargetInfoDict != null) {
			CIndicatorManager.Inst.Show();
			Func.m_oFirebaseCallbackDict02.ExReplaceVal(ECallback.SAVE_TARGET_INFOS, a_oCallback);

			// 로그인 되었을 경우
			if(CFirebaseManager.Inst.IsLogin) {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
				CFirebaseManager.Inst.SaveDatas(Factory.MakeTargetInfoNodes(), a_oTargetInfoDict.ExToJSONStr(true), Func.OnSaveTargetInfos);
#else
				Func.OnSaveTargetInfos(CFirebaseManager.Inst, false);
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
			} else {
				Func.OnSaveTargetInfos(CFirebaseManager.Inst, false);
			}
		}
	}

	/** 결제 정보를 저장한다 */
	public static void SavePurchaseInfos(List<STPurchaseInfo> a_oPurchaseInfoList, System.Action<CFirebaseManager, bool> a_oCallback, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oPurchaseInfoList != null);

		// 결제 정보가 존재 할 경우
		if(a_oPurchaseInfoList != null) {
			CIndicatorManager.Inst.Show();
			Func.m_oFirebaseCallbackDict02.ExReplaceVal(ECallback.SAVE_PURCHASE_INFOS, a_oCallback);

			// 로그인 되었을 경우
			if(CFirebaseManager.Inst.IsLogin) {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
				CFirebaseManager.Inst.SaveDatas(Factory.MakePurchaseInfoNodes(), a_oPurchaseInfoList.ExToJSONStr(true), Func.OnSavePurchaseInfos);
#else
				Func.OnSavePurchaseInfos(CFirebaseManager.Inst, false);
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
			} else {
				Func.OnSavePurchaseInfos(CFirebaseManager.Inst, false);
			}
		}
	}

	/** 파이어 베이스에 로그인 되었을 경우 */
	private static void OnFirebaseLogin(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oFirebaseCallbackDict02.GetValueOrDefault(ECallback.FIREBASE_LOGIN)?.Invoke(a_oSender, a_bIsSuccess);
	}

	/** 파이어 베이스에서 로그아웃 되었을 경우 */
	private static void OnFirebaseLogout(CFirebaseManager a_oSender) {
		CIndicatorManager.Inst.Close();
		Func.m_oFirebaseCallbackDict01.GetValueOrDefault(ECallback.FIREBASE_LOGOUT)?.Invoke(a_oSender);
	}

	/** 유저 정보가 로드 되었을 경우 */
	private static void OnLoadUserInfo(CFirebaseManager a_oSender, string a_oJSONStr, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oFirebaseCallbackDict03.GetValueOrDefault(ECallback.LOAD_USER_INFO)?.Invoke(a_oSender, a_oJSONStr, a_bIsSuccess);
	}

	/** 타겟 정보가 로드 되었을 경우 */
	private static void OnLoadTargetInfos(CFirebaseManager a_oSender, string a_oJSONStr, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oFirebaseCallbackDict03.GetValueOrDefault(ECallback.LOAD_TARGET_INFOS)?.Invoke(a_oSender, a_oJSONStr, a_bIsSuccess);
	}

	/** 결제 정보가 로드 되었을 경우 */
	private static void OnLoadPurchaseInfos(CFirebaseManager a_oSender, string a_oJSONStr, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oFirebaseCallbackDict03.GetValueOrDefault(ECallback.LOAD_PURCHASE_INFOS)?.Invoke(a_oSender, a_oJSONStr, a_bIsSuccess);
	}

	/** 유저 정보가 저장 되었을 경우 */
	private static void OnSaveUserInfo(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oFirebaseCallbackDict02.GetValueOrDefault(ECallback.SAVE_USER_INFO)?.Invoke(a_oSender, a_bIsSuccess);
	}

	/** 타겟 정보가 저장 되었을 경우 */
	private static void OnSaveTargetInfos(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oFirebaseCallbackDict02.GetValueOrDefault(ECallback.SAVE_TARGET_INFOS)?.Invoke(a_oSender, a_bIsSuccess);
	}

	/** 결제 정보가 저장 되었을 경우 */
	private static void OnSavePurchaseInfos(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oFirebaseCallbackDict02.GetValueOrDefault(ECallback.SAVE_PURCHASE_INFOS)?.Invoke(a_oSender, a_bIsSuccess);
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
		Func.m_oGameCenterCallbackDict02.ExReplaceVal(ECallback.GAME_CENTER_LOGIN, a_oCallback);

		CGameCenterManager.Inst.Login(Func.OnGameCenterLogin);
	}

	/** 게임 센터 로그아웃을 처리한다 */
	public static void GameCenterLogout(System.Action<CGameCenterManager> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oGameCenterCallbackDict01.ExReplaceVal(ECallback.GAME_CENTER_LOGOUT, a_oCallback);

		CGameCenterManager.Inst.Logout(Func.OnGameCenterLogout);
	}

	/** 기록을 갱신한다 */
	public static void UpdateRecord(string a_oLeaderboardID, long a_nRecord, System.Action<CGameCenterManager, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oGameCenterCallbackDict02.ExReplaceVal(ECallback.UPDATE_RECORD, a_oCallback);

		CGameCenterManager.Inst.UpdateRecord(a_oLeaderboardID, a_nRecord, Func.OnUpdateRecord);
	}

	/** 업적을 갱신한다 */
	public static void UpdateAchievement(string a_oAchievementID, double a_dblPercent, System.Action<CGameCenterManager, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oGameCenterCallbackDict02.ExReplaceVal(ECallback.UPDATE_ACHIEVEMENT, a_oCallback);

		CGameCenterManager.Inst.UpdateAchievement(a_oAchievementID, a_dblPercent * KCDefine.B_UNIT_NORM_VAL_TO_PERCENT, Func.OnUpdateAchievement);
	}

	/** 게임 센터에 로그인 되었을 경우 */
	private static void OnGameCenterLogin(CGameCenterManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oGameCenterCallbackDict02.GetValueOrDefault(ECallback.GAME_CENTER_LOGIN)?.Invoke(a_oSender, a_bIsSuccess);
	}

	/** 게임 센터에서 로그아웃 되었을 경우 */
	private static void OnGameCenterLogout(CGameCenterManager a_oSender) {
		CIndicatorManager.Inst.Close();
		Func.m_oGameCenterCallbackDict01.GetValueOrDefault(ECallback.GAME_CENTER_LOGOUT)?.Invoke(a_oSender);
	}
	
	/** 기록이 갱신 되었을 경우 */
	private static void OnUpdateRecord(CGameCenterManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oGameCenterCallbackDict02.GetValueOrDefault(ECallback.UPDATE_RECORD)?.Invoke(a_oSender, a_bIsSuccess);
	}

	/** 업적이 갱신 되었을 경우 */
	private static void OnUpdateAchievement(CGameCenterManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oGameCenterCallbackDict02.GetValueOrDefault(ECallback.UPDATE_ACHIEVEMENT)?.Invoke(a_oSender, a_bIsSuccess);
	}
#endif			// #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	/** 상품을 결제한다 */
	public static void PurchaseProduct(int a_nProductIdx, System.Action<CPurchaseManager, string, bool> a_oCallback, bool a_bIsEnableAssert = true) {
		Func.PurchaseProduct(CProductInfoTable.Inst.GetProductInfo(a_nProductIdx).m_oID, a_oCallback, a_bIsEnableAssert);
	}

	/** 상품을 결제한다 */
	public static void PurchaseProduct(EProductKinds a_eProductKinds, System.Action<CPurchaseManager, string, bool> a_oCallback, bool a_bIsEnableAssert = true) {
		bool bIsValid = CProductTradeInfoTable.Inst.TryGetBuyProductTradeInfo(a_eProductKinds, out STProductTradeInfo stProductTradeInfo);
		CAccess.Assert(!a_bIsEnableAssert || bIsValid);
		
		// 상품이 존재 할 경우
		if(bIsValid) {
			Func.PurchaseProduct(stProductTradeInfo.m_nProductIdx, a_oCallback, a_bIsEnableAssert);
		}
	}
	
	/** 상품을 결제한다 */
	public static void PurchaseProduct(string a_oID, System.Action<CPurchaseManager, string, bool> a_oCallback, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oID.ExIsValid());

		// 식별자가 유효 할 경우
		if(a_oID.ExIsValid()) {
			CIndicatorManager.Inst.Show();
			Func.m_oPurchaseCallbackDict01.ExReplaceVal(ECallback.PURCHASE, a_oCallback);
			
			CPurchaseManager.Inst.PurchaseProduct(a_oID, Func.OnPurchaseProduct);
		}
	}

	/** 상품을 복원한다 */
	public static void RestoreProducts(System.Action<CPurchaseManager, List<Product>, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oPurchaseCallbackDict02.ExReplaceVal(ECallback.RESTORE, a_oCallback);
		
		CPurchaseManager.Inst.RestoreProducts(Func.OnRestoreProducts);
	}

	/** 상품이 결제 되었을 경우 */
	private static void OnPurchaseProduct(CPurchaseManager a_oSender, string a_oProductID, bool a_bIsSuccess) {
		CPurchaseManager.Inst.ConfirmPurchase(a_oProductID, (a_oSender, a_oConfirmProductID, a_bIsConfirmSuccess) => {
			CIndicatorManager.Inst.Close();
			Func.m_oPurchaseCallbackDict01.GetValueOrDefault(ECallback.PURCHASE)?.Invoke(a_oSender, a_oConfirmProductID, a_bIsConfirmSuccess);
		});
	}

	/** 상품이 복원 되었을 경우 */
	private static void OnRestoreProducts(CPurchaseManager a_oSender, List<Product> a_oProductList, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oPurchaseCallbackDict02.GetValueOrDefault(ECallback.RESTORE)?.Invoke(a_oSender, a_oProductList, a_bIsSuccess);
	}
#endif			// #if PURCHASE_MODULE_ENABLE

#if PLAYFAB_MODULE_ENABLE
	/** 플레이 팹 로그인을 처리한다 */
	public static void PlayfabLogin(System.Action<CPlayfabManager, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oPlayfabCallbackDict02.ExReplaceVal(ECallback.PLAYFAB_LOGIN, a_oCallback);

#if UNITY_IOS && APPLE_LOGIN_ENABLE
		Func.AppleLogin(Func.OnPlayfabAppleLogin);
#elif (UNITY_IOS || UNITY_ANDROID) && FACEBOOK_MODULE_ENABLE
		Func.FacebookLogin(Func.OnPlayfabFacebookLogin);
#else
		CPlayfabManager.Inst.Login(CCommonAppInfoStorage.Inst.AppInfo.DeviceID, Func.OnPlayfabLogin);
#endif			// #if UNITY_IOS && APPLE_LOGIN_ENABLE
	}

	/** 플레이 팹 로그아웃을 처리한다 */
	public static void PlayfabLogout(System.Action<CPlayfabManager> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oPlayfabCallbackDict01.ExReplaceVal(ECallback.PLAYFAB_LOGOUT, a_oCallback);

#if UNITY_IOS && APPLE_LOGIN_ENABLE
		Func.AppleLogout(Func.OnPlayfabAppleLogout);
#elif (UNITY_IOS || UNITY_ANDROID) && FACEBOOK_MODULE_ENABLE
		Func.FacebookLogout(Func.OnPlayfabFacebookLogout);
#else
		CPlayfabManager.Inst.Logout(Func.OnPlayfabLogout);
#endif			// #if UNITY_IOS && APPLE_LOGIN_ENABLE
	}

	/** 플레이 팹에 로그인 되었을 경우 */
	private static void OnPlayfabLogin(CPlayfabManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oPlayfabCallbackDict02.GetValueOrDefault(ECallback.PLAYFAB_LOGIN)?.Invoke(a_oSender, a_bIsSuccess);
	}

	/** 플레이 팹에서 로그아웃 되었을 경우 */
	private static void OnPlayfabLogout(CPlayfabManager a_oSender) {
		CIndicatorManager.Inst.Close();
		Func.m_oPlayfabCallbackDict01.GetValueOrDefault(ECallback.PLAYFAB_LOGOUT)?.Invoke(a_oSender);
	}

#if UNITY_IOS && APPLE_LOGIN_ENABLE
	/** 애플에 로그인 되었을 경우 */
	private static void OnPlayfabAppleLogin(CServicesManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();

		// 로그인 되었을 경우
		if(a_bIsSuccess) {
			CIndicatorManager.Inst.Show();
			CPlayfabManager.Inst.LoginWithApple(a_oSender.AppleUserID, a_oSender.AppleIDToken, Func.OnPlayfabLogin);
		} else {
			Func.OnPlayfabLogin(CPlayfabManager.Inst, false);
		}
	}

	/** 애플에서 로그아웃 되었을 경우 */
	private static void OnPlayfabAppleLogout(CServicesManager a_oSender) {
		CPlayfabManager.Inst.Logout(Func.OnPlayfabLogout);
	}
#endif			// #if UNITY_IOS && APPLE_LOGIN_ENABLE

#if (UNITY_IOS || UNITY_ANDROID) && FACEBOOK_MODULE_ENABLE
	/** 페이스 북에 로그인 되었을 경우 */
	private static void OnPlayfabFacebookLogin(CFacebookManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();

		// 로그인 되었을 경우
		if(a_bIsSuccess) {
			CIndicatorManager.Inst.Show();
			CPlayfabManager.Inst.LoginWithFacebook(a_oSender.AccessToken, Func.OnPlayfabLogin);
		} else {
			Func.OnPlayfabLogin(CPlayfabManager.Inst, false);
		}
	}

	/** 페이스 북에서 로그아웃 되었을 경우 */
	private static void OnPlayfabFacebookLogout(CFacebookManager a_oSender) {
		CPlayfabManager.Inst.Logout(Func.OnPlayfabLogout);
	}
#endif			// #if (UNITY_IOS || UNITY_ANDROID) && FACEBOOK_MODULE_ENABLE
#endif			// #if PLAYFAB_MODULE_ENABLE

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
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
