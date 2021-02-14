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
	public static void BuyItem(EItemKinds a_eKinds) {
		var stItemInfo = CItemInfoTable.Inst.GetItemInfo(a_eKinds);

		switch(a_eKinds) {
			case EItemKinds.GOODS_COIN: {
				CCommonUserInfoStorage.Inst.AddNumCoins(stItemInfo.m_stSaleItemInfo.m_nNumItems);
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
				CUserInfoStorage.Inst.AddNumItems(a_eKinds, stItemInfo.m_stSaleItemInfo.m_nNumItems);
			} break;
		}

		// 비용이 존재 할 경우
		if(stItemInfo.m_ePriceType == EPriceType.GOODS && stItemInfo.m_nPrice > KCDefine.B_VALUE_INT_0) {
			switch(stItemInfo.m_ePriceKinds) {
				case EPriceKinds.GOODS_COIN: CCommonUserInfoStorage.Inst.AddNumCoins(-stItemInfo.m_nPrice); break;
			}
		}
	}

	//! 상점 팝업을 출력한다
	public static void ShowStorePopup(string a_oName, string a_oObjPath, GameObject a_oParent) {
		// 상점 팝업이 없을 경우
		if(a_oParent.ExFindChild(a_oName) == null) {
			var oStorePopup = CPopup.Create<CStorePopup>(a_oName, a_oObjPath, a_oParent);
			oStorePopup.Init();
			oStorePopup.Show(null, null);
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
