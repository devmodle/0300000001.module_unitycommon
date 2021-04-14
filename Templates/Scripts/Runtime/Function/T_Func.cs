using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 기본 함수
public static partial class Func {
	#region 클래스 변수
#if ADS_MODULE_ENABLE
	private static bool m_bIsWatchRewardAds = false;
	private static bool m_bIsWatchFullscreenAds = false;
	private static bool m_bIsWatchResumeAds = false;

	private static STAdsRewardItemInfo m_stRewardItemInfo;

	private static System.Action<CAdsManager, STAdsRewardItemInfo, bool> m_oRewardAdsCallback = null;
	private static System.Action<CAdsManager, bool> m_oFullscreenAdsCallback = null;
	private static System.Action<CAdsManager, bool> m_oResumeAdsCallback = null;
#endif			// #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	private static System.Action<CPurchaseManager, string, bool> m_oPurchaseCallback = null;
#endif			// #if PURCHASE_MODULE_ENABLE
	#endregion			// 클래스 변수
	
	#region 클래스 함수
	//! 경고 팝업을 출력한다
	public static void ShowAlertPopup(CAlertPopup.STParams a_stParams, System.Action<CAlertPopup, bool> a_oCallback) {
		// 경고 팝업이 없을 경우
		if(CSceneManager.ScreenPopupUIs.ExFindChild(KCDefine.U_OBJ_N_ALERT_POPUP) == null) {
			var oAlertPopup = CAlertPopup.Create<CAlertPopup>(KCDefine.U_OBJ_N_ALERT_POPUP, KCDefine.U_OBJ_P_G_ALERT_POPUP, CSceneManager.ScreenPopupUIs, a_stParams, a_oCallback);
			oAlertPopup.Show(null, null);
		}
	}

	//! 종료 팝업을 출력한다
	public static void ShowQuitPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		var stParams = new CAlertPopup.STParams() {
			m_oTitle = CStrTable.Inst.GetStr(KCDefine.ST_KEY_ALERT_P_TITLE),
			m_oMsg = CStrTable.Inst.GetStr(KCDefine.ST_KEY_QUIT_P_MSG),
			m_oOKBtnText = CStrTable.Inst.GetStr(KCDefine.ST_KEY_ALERT_P_OK_BTN_TEXT),
			m_oCancelBtnText = CStrTable.Inst.GetStr(KCDefine.ST_KEY_ALERT_P_CANCEL_BTN_TEXT)
		};

		Func.ShowAlertPopup(stParams, a_oCallback);
	}

	//! 업데이트 팝업을 출력한다
	public static void ShowUpdatePopup(System.Action<CAlertPopup, bool> a_oCallback) {
		var stParams = new CAlertPopup.STParams() {
			m_oTitle = CStrTable.Inst.GetStr(KCDefine.ST_KEY_ALERT_P_TITLE),
			m_oMsg = CStrTable.Inst.GetStr(KCDefine.ST_KEY_UPDATE_P_MSG),
			m_oOKBtnText = CStrTable.Inst.GetStr(KCDefine.ST_KEY_ALERT_P_OK_BTN_TEXT),
			m_oCancelBtnText = CStrTable.Inst.GetStr(KCDefine.ST_KEY_ALERT_P_CANCEL_BTN_TEXT)
		};

		Func.ShowAlertPopup(stParams, a_oCallback);
	}

	//! 지역화 문자열을 설정한다
	public static void SetupLocalizeStrs() {
		string oLanguage = CCommonAppInfoStorage.Inst.AppInfo.Language.ToString();
		string oCountryCode = CCommonAppInfoStorage.Inst.CountryCode;

		Func.SetupLocalizeStrs(oLanguage, oCountryCode);
	}

	//! 지역화 문자열을 설정한다
	public static void SetupLocalizeStrs(string a_oLanguage, string a_oCountryCode) {
		CAccess.Assert(a_oCountryCode.ExIsValid());

		string oBasePath = KCDefine.U_BASE_TABLE_P_G_LOCALIZE_COMMON_STR;
		string oFilePath = CFactory.MakeLocalizePath(oBasePath, KCDefine.U_TABLE_P_G_ENGLISH_COMMON_STR, a_oLanguage, a_oCountryCode);

		CStrTable.Inst.LoadStrsFromRes(oFilePath);		
	}
	#endregion			// 클래스 함수

	#region 제네릭 클래스 함수
	//! 팝업을 출력한다
	public static void ShowPopup<T>(string a_oName, string a_oObjPath, GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) where T : CPopup {
		// 팝업이 없을 경우
		if(a_oParent.ExFindChild(a_oName) == null) {
			var oPopup = CPopup.Create<T>(a_oName, a_oObjPath, a_oParent);
			a_oInitCallback?.Invoke(oPopup);

			oPopup.Show(a_oShowCallback, a_oCloseCallback);
		}
	}
	#endregion			// 제네릭 클래스 함수

	#region 조건부 클래스 함수
#if ADS_MODULE_ENABLE
	//! 보상 광고를 출력한다
	public static void ShowRewardAds(System.Action<CAdsManager, STAdsRewardItemInfo, bool> a_oCallback) {
		Func.ShowRewardAds(CPluginInfoTable.Inst.DefAdsType, a_oCallback);
	}
	
	//! 보상 광고를 출력한다
	public static void ShowRewardAds(EAdsType a_eAdsType, System.Action<CAdsManager, STAdsRewardItemInfo, bool> a_oCallback) {
		// 보상 광고 출력이 가능 할 경우
		if(CAdsManager.Inst.IsLoadRewardAds(a_eAdsType)) {
			Func.m_bIsWatchRewardAds = false;
			Func.m_stRewardItemInfo = default(STAdsRewardItemInfo);

			Func.m_oRewardAdsCallback = a_oCallback;
			CAdsManager.Inst.ShowRewardAds(a_eAdsType, Func.OnReceiveUserReward, Func.OnCloseRewardAds);
		} else {
			a_oCallback?.Invoke(CAdsManager.Inst, default(STAdsRewardItemInfo), false);
		}
	}

	//! 전면 광고를 출력한다
	public static void ShowFullscreenAds(System.Action<CAdsManager, bool> a_oCallback) {
		Func.ShowFullscreenAds(CPluginInfoTable.Inst.DefAdsType, a_oCallback);
	}

	//! 전면 광고를 출력한다
	public static void ShowFullscreenAds(EAdsType a_eAdsType, System.Action<CAdsManager, bool> a_oCallback) {
		float fDelay = CValueTable.Inst.GetFlt(KCDefine.VT_KEY_DEF_DELAY_FULLSCREEN_ADS);
		double dblDeltaTime = System.DateTime.Now.ExGetDeltaTime(CGameInfoStorage.Inst.PrevFullscreenAdsTime);

		// 전면 광고 출력이 가능 할 경우
		if(dblDeltaTime.ExIsGreateEquals(fDelay) && CAdsManager.Inst.IsLoadFullscreenAds(a_eAdsType)) {
			Func.m_bIsWatchFullscreenAds = true;
			Func.m_oFullscreenAdsCallback = a_oCallback;
			
			CAdsManager.Inst.ShowFullscreenAds(a_eAdsType, null, Func.OnCloseFullscreenAds);
		} else {
			a_oCallback?.Invoke(CAdsManager.Inst, false);
		}
	}

	//! 재개 광고를 출력한다
	public static void ShowResumeAds(System.Action<CAdsManager, bool> a_oCallback) {
		Func.ShowResumeAds(CPluginInfoTable.Inst.DefAdsType, a_oCallback);
	}

	//! 재개 광고를 출력한다
	public static void ShowResumeAds(EAdsType a_eAdsType, System.Action<CAdsManager, bool> a_oCallback) {
		float fDelay = CValueTable.Inst.GetFlt(KCDefine.VT_KEY_DEF_DELAY_RESUME_ADS);
		double dblDeltaTime = System.DateTime.Now.ExGetDeltaTime(CGameInfoStorage.Inst.PrevResumeAdsTime);

		// 재개 광고 출력이 가능 할 경우
		if(dblDeltaTime.ExIsGreateEquals(fDelay) && CAdsManager.Inst.IsLoadResumeAds(a_eAdsType)) {
			Func.m_bIsWatchResumeAds = true;
			Func.m_oResumeAdsCallback = a_oCallback;
			
			CAdsManager.Inst.ShowResumeAds(a_eAdsType, null, Func.OnCloseResumeAds);
		} else {
			a_oCallback?.Invoke(CAdsManager.Inst, false);
		}
	}

	//! 보상 광고가 닫혔을 경우
	private static void OnCloseRewardAds(CAdsManager a_oSender) {
		CFunc.Invoke(ref Func.m_oRewardAdsCallback, a_oSender, Func.m_stRewardItemInfo, Func.m_bIsWatchRewardAds);
	}

	//! 유저 보상을 수신했을 경우
	private static void OnReceiveUserReward(CAdsManager a_oSender, STAdsRewardItemInfo a_stRewardItemInfo, bool a_bIsSuccess) {
		Func.m_bIsWatchRewardAds = a_bIsSuccess;
		Func.m_stRewardItemInfo = a_stRewardItemInfo;
	}

	//! 전면 광고가 닫혔을 경우
	private static void OnCloseFullscreenAds(CAdsManager a_oSender) {
		CGameInfoStorage.Inst.PrevFullscreenAdsTime = System.DateTime.Now;
		CFunc.Invoke(ref Func.m_oFullscreenAdsCallback, a_oSender, Func.m_bIsWatchFullscreenAds);
	}

	//! 재개 광고가 닫혔을 경우
	private static void OnCloseResumeAds(CAdsManager a_oSender) {
		CGameInfoStorage.Inst.PrevResumeAdsTime = System.DateTime.Now;
		CFunc.Invoke(ref Func.m_oResumeAdsCallback, a_oSender, Func.m_bIsWatchResumeAds);
	}
#endif			// #if ADS_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
	//! 유저 정보를 저장한다
	public static void SaveUserInfo(System.Action<CFirebaseManager, bool> a_oCallback) {
		// 로그인 되었을 경우
		if(CFirebaseManager.Inst.IsLogin) {
			// var oNodeList = CFactory.MakeUserInfoNodeList();
			// string oJSONStr = a_oPostItemInfoList.ExToJSONStr();

			// CFirebaseManager.Inst.SaveDB(oNodeList, oJSONStr, a_oCallback);
		} else {
			a_oCallback?.Invoke(CFirebaseManager.Inst, false);
		}
	}

	//! 지급 아이템 정보를 저장한다
	public static void SavePostItemInfo(List<STPostItemInfo> a_oPostItemInfoList, System.Action<CFirebaseManager, bool> a_oCallback) {
		CAccess.Assert(a_oPostItemInfoList != null);

		// 로그인 되었을 경우
		if(CFirebaseManager.Inst.IsLogin) {
			var oNodeList = CFactory.MakePostItemInfoNodeList();
			string oJSONStr = a_oPostItemInfoList.ExToJSONStr();

			CFirebaseManager.Inst.SaveDB(oNodeList, oJSONStr, a_oCallback);
		} else {
			a_oCallback?.Invoke(CFirebaseManager.Inst, false);
		}
	}

	//! 유저 정보를 로드한다
	public static void LoadUserInfo(System.Action<CFirebaseManager, string, bool> a_oCallback) {
		// 로그인 되었을 경우
		if(CFirebaseManager.Inst.IsLogin) {
			var oNodeList = CFactory.MakeUserInfoNodeList();
			CFirebaseManager.Inst.LoadDB(oNodeList, a_oCallback);
		} else {
			a_oCallback?.Invoke(CFirebaseManager.Inst, string.Empty, false);
		}
	}

	//! 지급 아이템 정보를 로드한다
	public static void LoadPostItemInfo(System.Action<CFirebaseManager, string, bool> a_oCallback) {
		// 로그인 되었을 경우
		if(CFirebaseManager.Inst.IsLogin) {
			var oNodeList = CFactory.MakePostItemInfoNodeList();
			CFirebaseManager.Inst.LoadDB(oNodeList, a_oCallback);
		} else {
			a_oCallback?.Invoke(CFirebaseManager.Inst, string.Empty, false);
		}
	}
#endif			// #if FIREBASE_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	//! 결제 성공 팝업을 출력한다
	public static void ShowPurchaseSuccessPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		var stParams = new CAlertPopup.STParams() {
			m_oTitle = CStrTable.Inst.GetStr(KCDefine.ST_KEY_ALERT_P_TITLE),
			m_oMsg = CStrTable.Inst.GetStr(KCDefine.ST_KEY_PURCHASE_P_SUCCESS_MSG),
			m_oOKBtnText = CStrTable.Inst.GetStr(KCDefine.ST_KEY_ALERT_P_OK_BTN_TEXT),
			m_oCancelBtnText = string.Empty
		};

		Func.ShowAlertPopup(stParams, a_oCallback);
	}

	//! 결제 실패 팝업을 출력한다
	public static void ShowPurchaseFailPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		var stParams = new CAlertPopup.STParams() {
			m_oTitle = CStrTable.Inst.GetStr(KCDefine.ST_KEY_ALERT_P_TITLE),
			m_oMsg = CStrTable.Inst.GetStr(KCDefine.ST_KEY_PURCHASE_P_FAIL_MSG),
			m_oOKBtnText = CStrTable.Inst.GetStr(KCDefine.ST_KEY_ALERT_P_OK_BTN_TEXT),
			m_oCancelBtnText = string.Empty
		};

		Func.ShowAlertPopup(stParams, a_oCallback);
	}

	//! 상품을 결제한다
	public static void PurchaseProduct(int a_nID, System.Action<CPurchaseManager, string, bool> a_oCallback) {
		var stProductInfo = CProductInfoTable.Inst.GetProductInfo(a_nID);
		Func.PurchaseProduct(stProductInfo.m_oID, a_oCallback);
	}
	
	//! 상품을 결제한다
	public static void PurchaseProduct(string a_oID, System.Action<CPurchaseManager, string, bool> a_oCallback) {
		CAccess.Assert(a_oID.ExIsValid());
		Func.m_oPurchaseCallback = a_oCallback;

		CPurchaseManager.Inst.PurchaseProduct(a_oID, Func.OnCompletePurchase);
	}

	//! 결제가 완료 되었을 경우
	private static void OnCompletePurchase(CPurchaseManager a_oSender, string a_oProductID, bool a_bIsSuccess) {
		// 결제 되었을 경우
		if(a_bIsSuccess) {
			CPurchaseManager.Inst.ConfirmPurchase(a_oProductID, (a_oConfirmSender, a_oConfirmProductID, a_bIsConfirmSuccess) => {
				CFunc.Invoke(ref Func.m_oPurchaseCallback, a_oConfirmSender, a_oConfirmProductID, a_bIsConfirmSuccess);
			});
		} else {
			CFunc.Invoke(ref Func.m_oPurchaseCallback, a_oSender, a_oProductID, a_bIsSuccess);
		}
	}
#endif			// #if PURCHASE_MODULE_ENABLE
	#endregion			// 조건부 클래스 함수
}
#endif			// #if NEVER_USE_THIS
