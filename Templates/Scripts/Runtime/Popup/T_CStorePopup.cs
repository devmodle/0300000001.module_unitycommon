using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 상점 팝업
public class CStorePopup : CSubPopup {
	#region 변수
	private int m_nSelProductID = 0;
	#endregion			// 변수
	
	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
	}
	
	//! 초기화
	public override void Init() {
		base.Init();
	}

	//! 광고 버튼을 눌렀을 경우
	private void OnTouchAdsBtn(int a_nID) {
#if ADS_MODULE_ENABLE
		m_nSelProductID = a_nID;
		Func.ShowRewardAds(this.OnCloseRewardAds);
#endif			// #if ADS_MODULE_ENABLE
	}

	//! 결제 버튼을 눌렀을 경우
	private void OnTouchPurchaseBtn(int a_nID) {
#if PURCHASE_MODULE_ENABLE
		m_nSelProductID = a_nID;
		Func.PurchaseProduct(a_nID, this.OnCompletePurchase);
#endif			// #if PURCHASE_MODULE_ENABLE
	}
	#endregion			// 함수

	#region 조건부 함수
#if ADS_MODULE_ENABLE
	//! 보상 광고가 닫혔을 경우
	private void OnCloseRewardAds(CAdsManager a_oSender, STAdsRewardItemInfo a_stRewardItemInfo, bool a_bIsSuccess) {
		// 광고를 시청했을 경우
		if(a_bIsSuccess) {
			var stSaleProductInfo = CSaleProductInfoTable.Inst.GetSaleProductInfo(m_nSelProductID);

			for(int i = 0; i < stSaleProductInfo.m_oItemInfoList.Count; ++i) {
				Func.BuyItem(stSaleProductInfo.m_oItemInfoList[i]);
			}
		}
	}
#endif			// #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	//! 결제가 완료 되었을 경우
	private void OnCompletePurchase(CPurchaseManager a_oSender, string a_oProductID, bool a_bIsSuccess) {
		// 결제 되었을 경우
		if(a_bIsSuccess) {
			int nID = CProductInfoTable.Inst.GetProductInfoIdx(a_oProductID);
			var stSaleProductInfo = CSaleProductInfoTable.Inst.GetSaleProductInfo(nID);

			for(int i = 0; i < stSaleProductInfo.m_oItemInfoList.Count; ++i) {
				Func.BuyItem(stSaleProductInfo.m_oItemInfoList[i]);
			}
		}
	}
#endif			// #if PURCHASE_MODULE_ENABLE
	#endregion			// 조건부 함수
}
#endif			// #if NEVER_USE_THIS
