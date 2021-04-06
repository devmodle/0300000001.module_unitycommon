using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif			// #if PURCHASE_MODULE_ENABLE

//! 전역 함수
public static partial class Func {
	#region 클래스 함수
	//! 아이템을 구입한다
	public static void AcquireItem(STItemInfo a_stItemInfo) {
		switch(a_stItemInfo.m_eItemKinds) {
			case EItemKinds.GOODS_COIN: {
				CUserInfoStorage.Inst.AddNumCoins(a_stItemInfo.m_nNumItems);
			} break;
			case EItemKinds.NON_CONSUMABLE_REMOVE_ADS: {
				CCommonUserInfoStorage.Inst.UserInfo.IsRemoveAds = true;

#if ADS_MODULE_ENABLE
				CAdsManager.Inst.IsEnableBannerAds = false;
				CAdsManager.Inst.IsEnableFullscreenAds = false;
				CAdsManager.Inst.IsEnableResumeAds = false;
				
				CAdsManager.Inst.CloseBannerAds(CPluginInfoTable.Inst.DefAdsType, true);
#endif			// #if ADS_MODULE_ENABLE
			} break;
			default: {
				CUserInfoStorage.Inst.AddNumItems(a_stItemInfo.m_eItemKinds, a_stItemInfo.m_nNumItems);
			} break;
		}

		CUserInfoStorage.Inst.SaveUserInfo();
		CCommonUserInfoStorage.Inst.SaveUserInfo();
	}

	//! 아이템을 구입한다
	public static void BuyItem(STSaleItemInfo a_stSaleItemInfo, bool a_bIsIgnoreProcess = false) {
		// 구입 처리가 가능 할 경우
		if(!a_bIsIgnoreProcess) {
			Func.AcquireItem(a_stSaleItemInfo.m_stItemInfo);
		}

		// 비용이 존재 할 경우
		if(a_stSaleItemInfo.m_ePriceKinds == EPriceKinds.GOODS_COIN && a_stSaleItemInfo.m_nPrice > KCDefine.B_VALUE_0_INT) {
			CUserInfoStorage.Inst.AddNumCoins(-a_stSaleItemInfo.m_nPrice);
		}

		CCommonUserInfoStorage.Inst.SaveUserInfo();
	}

	//! 상점 팝업을 출력한다
	public static void ShowStorePopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		// 상점 팝업이 없을 경우
		if(a_oParent.ExFindChild(KDefine.G_OBJ_N_STORE_POPUP) == null) {
			Func.ShowPopup<CStorePopup>(KDefine.G_OBJ_N_STORE_POPUP, KCDefine.U_OBJ_P_G_STORE_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
		}
	}

	//! 보상 팝업을 출력한다
	public static void ShowRewardPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		// 보상 팝업이 없을 경우
		if(a_oParent.ExFindChild(KDefine.G_OBJ_N_REWARD_POPUP) == null) {
			Func.ShowPopup<CRewardPopup>(KDefine.G_OBJ_N_REWARD_POPUP, KCDefine.U_OBJ_P_G_REWARD_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
		}
	}
	
	//! 무료 보상 팝업을 출력한다
	public static void ShowFreeRewardPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		// 무료 보상 팝업이 없을 경우
		if(a_oParent.ExFindChild(KDefine.G_OBJ_N_FREE_REWARD_POPUP) == null) {
			Func.ShowPopup<CFreeRewardPopup>(KDefine.G_OBJ_N_STORE_POPUP, KCDefine.U_OBJ_P_G_STORE_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
		}
	}

	//! 일일 보상 팝업을 출력한다
	public static void ShowDailyRewardPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		// 일일 보상 팝업이 없을 경우
		if(a_oParent.ExFindChild(KDefine.G_OBJ_N_DAILY_REWARD_POPUP) == null) {
			Func.ShowPopup<CDailyRewardPopup>(KDefine.G_OBJ_N_STORE_POPUP, KCDefine.U_OBJ_P_G_STORE_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
		}
	}
	#endregion			// 클래스 함수

	#region 조건부 클래스 함수
#if PURCHASE_MODULE_ENABLE
	//! 상품 결제가 완료 되었을 경우
	public static void OnCompletePurchaseProduct(CPurchaseManager a_oSender, string a_oID, bool a_bIsSuccess, System.Action<CAlertPopup, bool> a_oCallback) {
		// 결제 되었을 경우
		if(a_bIsSuccess) {
			Func.ShowPurchaseSuccessPopup(a_oCallback);
		} else {
			Func.ShowPurchaseFailPopup(a_oCallback);
		}
	}
#endif			// #if PURCHASE_MODULE_ENABLE
	#endregion			// 조건부 클래스 함수
}
#endif			// #if NEVER_USE_THIS
