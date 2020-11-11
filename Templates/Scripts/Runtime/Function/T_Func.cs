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

	private static STPostItem m_stRewardItem;

	private static System.Action<CAdsManager, STPostItem, bool> m_oRewardAdsCallback = null;
	private static System.Action<CAdsManager, bool> m_oFullscreenAdsCallback = null;
	private static System.Action<CAdsManager, bool> m_oResumeAdsCallback = null;
#endif			// #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	private static System.Action<CPurchaseManager, string, bool> m_oPurchaseCallback = null;
#endif			// #if PURCHASE_MODULE_ENABLE
	#endregion			// 클래스 변수

	#region 클래스 함수
	//! 앱 종료 팝업을 출력한다
	public static void ShowAppQuitPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		var oDataList = new Dictionary<string, string>() {
			[KCDefine.U_KEY_ALERT_P_TITLE] = CStringTable.Instance.GetString(KDefine.ST_KEY_ALERT_P_TITLE),
			[KCDefine.U_KEY_ALERT_P_MSG] = CStringTable.Instance.GetString(KDefine.ST_KEY_ALERT_P_MSG),
			[KCDefine.U_KEY_ALERT_P_OK_BTN_TEXT] = CStringTable.Instance.GetString(KDefine.ST_KEY_ALERT_P_OK_BTN_TEXT),
			[KCDefine.U_KEY_ALERT_P_CANCEL_BTN_TEXT] = CStringTable.Instance.GetString(KDefine.ST_KEY_ALERT_P_CANCEL_BTN_TEXT)
		};

		var oAlertPopup = CAlertPopup.CreateAlertPopup<CAlertPopup>(KCDefine.U_OBJ_NAME_ALERT_POPUP,
			CResManager.Instance.GetPrefab(KCDefine.U_OBJ_PATH_G_ALERT_POPUP),
			CSceneManager.ScreenPopupUIRoot,
			oDataList,
			a_oCallback);

		oAlertPopup.Show(null, null);
	}
	
	//! 지역화 문자열을 설정한다
	public static void SetupLocalizeStrings() {
		string oLanguage = CCommonAppInfoStorage.Instance.AppInfo.Language.ToString();
		string oCountryCode = CCommonAppInfoStorage.Instance.CountryCode;

		Func.SetupLocalizeStrings(oLanguage, oCountryCode);
	}

	//! 지역화 문자열을 설정한다
	public static void SetupLocalizeStrings(string a_oLanguage, string a_oCountryCode) {
		string oFilepath = CFunc.MakeLocalizePath(KCDefine.U_BASE_TABLE_PATH_G_LOCALIZE_COMMON_STRING, 
			KCDefine.U_TABLE_PATH_G_ENGLISH_COMMON_STRING, a_oLanguage, a_oCountryCode);

		CStringTable.Instance.LoadStringsFromRes(oFilepath);		
	}
	#endregion			// 클래스 함수

	#region 조건부 클래스 함수
#if ADS_MODULE_ENABLE
	//! 보상 광고를 출력한다
	public static void ShowRewardAds(EAdsType a_eAdsType, 
		System.Action<CAdsManager, STPostItem, bool> a_oCallback) 
	{
		// 보상 광고 출력이 가능 할 경우
		if(CAdsManager.Instance.IsLoadRewardAds(a_eAdsType)) {
			Func.m_bIsWatchRewardAds = false;
			Func.m_stRewardItem = default(STPostItem);

			Func.m_oRewardAdsCallback = a_oCallback;

			CAdsManager.Instance.ShowRewardAds(a_eAdsType, 
				Func.OnReceiveUserReward, Func.OnCloseRewardAds);
		} else {
			a_oCallback?.Invoke(CAdsManager.Instance, default(STPostItem), false);
		}
	}

	//! 전면 광고를 출력한다
	public static void ShowFullscreenAds(EAdsType a_eAdsType, 
		System.Action<CAdsManager, bool> a_oCallback) 
	{
		float fDelay = CValueTable.Instance.GetFloat(KCDefine.VT_KEY_DEF_DELAY_FULLSCREEN_ADS);
		double dblDeltaTime = System.DateTime.Now.ExGetDeltaTime(CGameInfoStorage.Instance.PrevFullscreenAdsTime);

		// 전면 광고 출력이 가능 할 경우
		if(dblDeltaTime.ExIsGreateEquals(fDelay) && 
			CAdsManager.Instance.IsLoadFullscreenAds(a_eAdsType)) 
		{
			Func.m_bIsWatchFullscreenAds = true;
			Func.m_oFullscreenAdsCallback = a_oCallback;
			
			CAdsManager.Instance.ShowFullscreenAds(a_eAdsType, null, Func.OnCloseFullscreenAds);
		} else {
			a_oCallback?.Invoke(CAdsManager.Instance, false);
		}
	}

	//! 재개 광고를 출력한다
	public static void ShowResumeAds(EAdsType a_eAdsType, 
		System.Action<CAdsManager, bool> a_oCallback) 
	{
		float fDelay = CValueTable.Instance.GetFloat(KCDefine.VT_KEY_DEF_DELAY_RESUME_ADS);
		double dblDeltaTime = System.DateTime.Now.ExGetDeltaTime(CGameInfoStorage.Instance.PrevResumeAdsTime);

		// 재개 광고 출력이 가능 할 경우
		if(dblDeltaTime.ExIsGreateEquals(fDelay) && 
			CAdsManager.Instance.IsLoadResumeAds(a_eAdsType)) 
		{
			Func.m_bIsWatchResumeAds = true;
			Func.m_oResumeAdsCallback = a_oCallback;
			
			CAdsManager.Instance.ShowResumeAds(a_eAdsType, null, Func.OnCloseResumeAds);
		} else {
			a_oCallback?.Invoke(CAdsManager.Instance, false);
		}
	}

	//! 보상 광고가 닫혔을 경우
	private static void OnCloseRewardAds(CAdsManager a_oSender) {
		Func.m_oRewardAdsCallback?.Invoke(a_oSender, Func.m_stRewardItem, Func.m_bIsWatchRewardAds);
	}

	//! 유저 보상을 수신했을 경우
	private static void OnReceiveUserReward(CAdsManager a_oSender, 
		STPostItem a_stRewardItem, bool a_bIsSuccess) 
	{
		Func.m_bIsWatchRewardAds = a_bIsSuccess;
		Func.m_stRewardItem = a_stRewardItem;
	}

	//! 전면 광고가 닫혔을 경우
	private static void OnCloseFullscreenAds(CAdsManager a_oSender) {
		CGameInfoStorage.Instance.PrevFullscreenAdsTime = System.DateTime.Now;
		Func.m_oFullscreenAdsCallback?.Invoke(a_oSender, Func.m_bIsWatchFullscreenAds);
	}

	//! 재개 광고가 닫혔을 경우
	private static void OnCloseResumeAds(CAdsManager a_oSender) {
		CGameInfoStorage.Instance.PrevResumeAdsTime = System.DateTime.Now;
		Func.m_oResumeAdsCallback?.Invoke(a_oSender, Func.m_bIsWatchResumeAds);
	}
#endif			// #if ADS_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
	//! 지급 아이템을 저장한다
	public static void SavePostItem(List<STPostItem> a_oPostItem, 
		System.Action<CFirebaseManager, bool> a_oCallback) 
	{
		// 로그인 되었을 경우
		if(CFirebaseManager.Instance.IsLogin) {
			var oNodeList = Func.MakePostItemNodeList();
			string oJSONString = a_oPostItem.ExToJSONString();

			CFirebaseManager.Instance.SaveDB(oNodeList, oJSONString, a_oCallback);
		} else {
			a_oCallback?.Invoke(CFirebaseManager.Instance, false);
		}
	}

	//! 지급 아이템을 로드한다
	public static void LoadPostItem(System.Action<CFirebaseManager, string, bool> a_oCallback) {
		// 로그인 되었을 경우
		if(CFirebaseManager.Instance.IsLogin) {
			var oNodeList = Func.MakePostItemNodeList();
			CFirebaseManager.Instance.LoadDB(oNodeList, a_oCallback);
		} else {
			a_oCallback?.Invoke(CFirebaseManager.Instance, string.Empty, false);
		}
	}

	//! 지급 아이템 노드를 생성한다
	public static List<string> MakePostItemNodeList() {
		return new List<string>() {
			KCDefine.U_NODE_FIREBASE_POST_ITEM_LIST
		};
	}

	//! 유저 정보 노드를 생성한다
	public static List<string> MakeUserInfoNodeList() {
		return new List<string>() {
			KCDefine.U_NODE_FIREBASE_USER_INFO_LIST
		};
	}

	//! 결제 정보 노드를 생성한다
	public static List<string> MakePurchaseInfoList() {
		return new List<string>() {
			KCDefine.U_NODE_FIREBASE_PURCHASE_INFO_LIST
		};
	}
#endif			// #if FIREBASE_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	//! 상품을 결제한다
	public static void PurchaseProduct(string a_oID, 
		System.Action<CPurchaseManager, string, bool> a_oCallback) 
	{
		Func.m_oPurchaseCallback = a_oCallback;
		CPurchaseManager.Instance.PurchaseProduct(a_oID, Func.OnCompletePurchase);
	}

	//! 결제가 완료 되었을 경우
	private static void OnCompletePurchase(CPurchaseManager a_oSender,
		string a_oProductID, bool a_bIsSuccess)
	{
		// 결제 되었을 경우
		if(a_bIsSuccess) {
			CPurchaseManager.Instance.ConfirmPurchase(a_oProductID, Func.m_oPurchaseCallback);
		} else {
			Func.m_oPurchaseCallback?.Invoke(a_oSender, a_oProductID, a_bIsSuccess);
		}
	}
#endif			// #if PURCHASE_MODULE_ENABLE
	#endregion			// 조건부 클래스 함수
}
#endif			// #if NEVER_USE_THIS
