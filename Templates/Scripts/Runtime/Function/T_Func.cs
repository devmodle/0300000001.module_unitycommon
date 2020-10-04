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

	private static STAdsRewardInfo m_stRewardInfo;

	private static System.Action<CAdsManager, STAdsRewardInfo, bool> m_oRewardAdsCallback = null;
	private static System.Action<CAdsManager, bool> m_oFullscreenAdsCallback = null;
#endif			// #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	private static System.Action<CPurchaseManager, string, bool> m_oPurchaseCallback = null;
#endif			// #if PURCHASE_MODULE_ENABLE
	#endregion			// 클래스 변수

	#region 조건부 클래스 함수
#if MSG_PACK_ENABLE
	//! 지역화 문자열을 설정한다
	public static void SetupLocalizeStrings() {
// 		string oLanguage = CCommonAppInfoStorage.Instance.AppInfo.Language;

// 		// 언어가 유효 할 경우
// 		if(Application.systemLanguage.ExIsValid()) {

// 		} else {

// 		}
// #endif			// #if LOCALIZE_TEST_ENABLE

// 		string oFilepath = string.Empty;

// 		// 언어가 유효 할 경우
// 		if(Application.systemLanguage.ExIsValid()) {
// 			oFilepath = CFunc.MakeLocalizeFilepath(KCDefine.U_BASE_TABLE_PATH_G_LOCALIZE_COMMON_STRING,
// 				Application.systemLanguage.ToString());
// 		} else {
// 			oFilepath = CFunc.MakeLocalizeFilepath(KCDefine.U_BASE_TABLE_PATH_G_LOCALIZE_COMMON_STRING,
// 				CCommonAppInfoStorage.Instance.CountryCode);
// 		}

// 		oFilepath = CAccess.IsExistsRes<TextAsset>(oFilepath) ? oFilepath
// 			: KCDefine.U_TABLE_PATH_G_ENGLISH_COMMON_STRING;
		
// 		CStringTable.Instance.LoadStringsFromRes(oFilepath);
	}
#endif			// #if MSG_PACK_ENABLE

#if ADS_MODULE_ENABLE
	//! 보상 광고를 출력한다
	public static void ShowRewardAds(EAdsType a_eAdsType, 
		System.Action<CAdsManager, STAdsRewardInfo, bool> a_oCallback) 
	{
		// 보상 광고 출력이 가능 할 경우
		if(CAdsManager.Instance.IsLoadRewardAds(a_eAdsType)) {
			Func.m_bIsWatchRewardAds = false;
			Func.m_stRewardInfo = default(STAdsRewardInfo);

			Func.m_oRewardAdsCallback = a_oCallback;

			CAdsManager.Instance.ShowRewardAds(a_eAdsType, 
				Func.OnReceiveUserReward, Func.OnCloseRewardAds);
		} else {
			a_oCallback?.Invoke(CAdsManager.Instance, default(STAdsRewardInfo), false);
		}
	}

	//! 전면 광고를 출력한다
	public static void ShowFullscreenAds(EAdsType a_eAdsType, 
		System.Action<CAdsManager, bool> a_oCallback) 
	{
#if MSG_PACK_ENABLE
		var stDeltaTime = System.DateTime.Now - CGameInfoStorage.Instance.PrevAdsTime;
		float fDelay = CValueTable.Instance.GetFloat(KCDefine.VT_KEY_DEF_DELAY_ADS);

		bool bIsEnable = stDeltaTime.TotalSeconds.ExIsGreateEquals(fDelay);

		// 전면 광고 출력이 가능 할 경우
		if(bIsEnable && CAdsManager.Instance.IsLoadFullscreenAds(a_eAdsType)) {
			Func.m_bIsWatchFullscreenAds = true;
			Func.m_oFullscreenAdsCallback = a_oCallback;
			
			CAdsManager.Instance.ShowFullscreenAds(a_eAdsType, null, Func.OnCloseFullscreenAds);
		} else {
			a_oCallback?.Invoke(CAdsManager.Instance, false);
		}
#else
		a_oCallback?.Invoke(CAdsManager.Instance, false);
#endif			// #if MSG_PACK_ENABLE
	}

	//! 보상 광고가 닫혔을 경우
	private static void OnCloseRewardAds(CAdsManager a_oSender) {
		Func.m_oRewardAdsCallback?.Invoke(a_oSender, Func.m_stRewardInfo, Func.m_bIsWatchRewardAds);
	}

	//! 유저 보상을 수신했을 경우
	private static void OnReceiveUserReward(CAdsManager a_oSender, 
		STAdsRewardInfo a_stRewardInfo, bool a_bIsSuccess) 
	{
		Func.m_bIsWatchRewardAds = a_bIsSuccess;
		Func.m_stRewardInfo = a_stRewardInfo;
	}

	//! 전면 광고가 닫혔을 경우
	private static void OnCloseFullscreenAds(CAdsManager a_oSender) {
		CGameInfoStorage.Instance.PrevAdsTime = System.DateTime.Now;
		Func.m_oFullscreenAdsCallback?.Invoke(a_oSender, Func.m_bIsWatchFullscreenAds);
	}
#endif			// #if ADS_MODULE_ENABLE

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
