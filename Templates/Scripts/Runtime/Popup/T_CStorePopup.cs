using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif			// #if PURCHASE_MODULE_ENABLE

//! 상점 팝업
public class CStorePopup : CSubPopup {
	//! 매개 변수
	public struct STParams {
		public List<STSaleProductInfo> m_oSaleProductInfoList;
	}

	#region 변수
	private STParams m_stParams;
	private ESaleProductKinds m_eAdsProductKinds = ESaleProductKinds.NONE;
	#endregion			// 변수

	#region 객체
	[SerializeField] private List<GameObject> m_oProductUIsList = new List<GameObject>();
	#endregion			// 객체
	
	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
	}
	
	//! 초기화
	public virtual void Init(STParams a_stParams) {
		base.Init();
		m_stParams = a_stParams;
		
		this.UpdateUIsState();
	}

	//! UI 상태를 갱신한다
	private void UpdateUIsState() {
		// 상품 UI 상태를 갱신한다
		for(int i = 0; i < m_oProductUIsList.Count; ++i) {
			var oProductUIs = m_oProductUIsList[i];
			this.UpdateProductUIsState(oProductUIs, m_stParams.m_oSaleProductInfoList[i]);
		}
	}

	//! 상품 UI 상태를 갱신한다
	private void UpdateProductUIsState(GameObject a_oProductUIs, STSaleProductInfo a_stSaleProductInfo) {
		// 패키지 상품 일 경우
		if(a_stSaleProductInfo.m_eSaleProductType == ESaleProductType.PKG) {
			this.UpdatePkgProductUIsState(a_oProductUIs, a_stSaleProductInfo);
		} else {
			this.UpdateSingleProductUIsState(a_oProductUIs, a_stSaleProductInfo);
		}
	}

	//! 패키지 상품 UI 상태를 갱신한다
	private void UpdatePkgProductUIsState(GameObject a_oProductUIs, STSaleProductInfo a_stSaleProductInfo) {
		// Do Nothing
	}

	//! 단일 상품 UI 상태를 갱신한다
	private void UpdateSingleProductUIsState(GameObject a_oProductUIs, STSaleProductInfo a_stSaleProductInfo) {
		// Do Nothing
	}

	//! 광고 버튼을 눌렀을 경우
	private void OnTouchAdsBtn(ESaleProductKinds a_eSaleProductKinds) {
#if ADS_MODULE_ENABLE
		m_eAdsProductKinds = a_eSaleProductKinds;
		Func.ShowRewardAds(this.OnCloseRewardAds);
#endif			// #if ADS_MODULE_ENABLE
	}

	//! 결제 버튼을 눌렀을 경우
	private void OnTouchPurchaseBtn(STSaleProductInfo a_stSaleProductInfo) {
#if PURCHASE_MODULE_ENABLE
		Func.PurchaseProduct(a_stSaleProductInfo.m_eSaleProductKinds, this.OnPurchaseProduct);
#endif			// #if PURCHASE_MODULE_ENABLE
	}

	//! 복원 버튼을 눌렀을 경우
	private void OnTouchRestoreBtn() {
#if PURCHASE_MODULE_ENABLE
		Func.RestoreProducts(this.OnRestoreProducts);
#endif			// #if PURCHASE_MODULE_ENABLE
	}
	#endregion			// 함수
	
	#region 조건부 함수
#if ADS_MODULE_ENABLE
	//! 보상 광고가 닫혔을 경우
	private void OnCloseRewardAds(CAdsManager a_oSender, STAdsRewardItemInfo a_stRewardItemInfo, bool a_bIsSuccess) {
		// 광고를 시청했을 경우
		if(a_bIsSuccess) {
			var stSaleProductInfo = CSaleProductInfoTable.Inst.GetSaleProductInfo(m_eAdsProductKinds);

			for(int i = 0; i < stSaleProductInfo.m_oItemInfoList.Count; ++i) {
				Func.AcquireItem(stSaleProductInfo.m_oItemInfoList[i]);
			}
		}
	}
#endif			// #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	//! 상품을 결제했을 경우
	private void OnPurchaseProduct(CPurchaseManager a_oSender, string a_oProductID, bool a_bIsSuccess) {
		// 결제 되었을 경우
		if(a_bIsSuccess) {
			Func.AcquireProduct(a_oProductID);
			Func.OnPurchaseProduct(a_oSender, a_oProductID, a_bIsSuccess, null);
		}
	}

	//! 상품이 복원 되었을 경우
	public void OnRestoreProducts(CPurchaseManager a_oSender, List<Product> a_oProductList, bool a_bIsSuccess) {
		// 복원 되었을 경우
		if(a_bIsSuccess) {
			Func.AcquireRestoreProducts(a_oProductList);
			Func.OnRestoreProducts(a_oSender, a_oProductList, a_bIsSuccess, null);
		}
	}
#endif			// #if PURCHASE_MODULE_ENABLE
	#endregion			// 조건부 함수
}
#endif			// #if NEVER_USE_THIS
