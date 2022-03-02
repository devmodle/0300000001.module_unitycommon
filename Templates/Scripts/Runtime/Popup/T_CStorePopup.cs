using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if SCRIPT_TEMPLATE_ONLY
#if RUNTIME_TEMPLATES_MODULE_ENABLE
#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif			// #if PURCHASE_MODULE_ENABLE

/** 상점 팝업 */
public class CStorePopup : CSubPopup {
	/** 콜백 */
	public enum ECallback {
		NONE = -1,

#if ADS_MODULE_ENABLE
		ADS,
#endif			// #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
		PURCHASE,
		RESTORE,
#endif			// #if PURCHASE_MODULE_ENABLE

		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public List<STSaleProductInfo> m_oSaleProductInfoList;

#if ADS_MODULE_ENABLE
		public Dictionary<ECallback, System.Action<CAdsManager, STAdsRewardInfo, bool>> m_oAdsCallbackDictA;
#endif			// #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
		public Dictionary<ECallback, System.Action<CPurchaseManager, string, bool>> m_oPurchaseCallbackDictA;
		public Dictionary<ECallback, System.Action<CPurchaseManager, List<Product>, bool>> m_oPurchaseCallbackDictB;
#endif			// #if PURCHASE_MODULE_ENABLE
	}

	#region 변수
	private STParams m_stParams;
	private ESaleProductKinds m_eSelSaleProductKinds = ESaleProductKinds.NONE;

	/** =====> 객체 <===== */
	[SerializeField] private List<GameObject> m_oSaleProductUIsList = new List<GameObject>();

#if FIREBASE_MODULE_ENABLE && PURCHASE_MODULE_ENABLE
	private string m_oPurchaseProductID = string.Empty;
	private List<Product> m_oRestoreProductList = new List<Product>();
#endif			// #if FIREBASE_MODULE_ENABLE && PURCHASE_MODULE_ENABLE
	#endregion			// 변수

	#region 추가 변수

	#endregion			// 추가 변수

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 버튼을 설정한다
		m_oContents.ExFindComponent<Button>(KCDefine.U_OBJ_N_RESTORE_BTN)?.onClick.AddListener(this.OnTouchRestoreBtn);
	}
	
	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();
		m_stParams = a_stParams;
	}

	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}
	
	/** UI 상태를 갱신한다 */
	private new void UpdateUIsState() {
		// 상품 UI 상태를 갱신한다
		for(int i = 0; i < m_oSaleProductUIsList.Count; ++i) {
			this.UpdateSaleProductUIsState(m_oSaleProductUIsList[i], m_stParams.m_oSaleProductInfoList[i]);
		}
	}

	/** 판매 상품 UI 상태를 갱신한다 */
	private void UpdateSaleProductUIsState(GameObject a_oSaleProductUIs, STSaleProductInfo a_stSaleProductInfo) {
		var ePriceType = a_stSaleProductInfo.m_ePriceKinds.ExKindsToType();
		var eSaleProductType = a_stSaleProductInfo.m_eSaleProductKinds.ExKindsToType();

		var oAdsPriceUIs = a_oSaleProductUIs.ExFindChild(KCDefine.U_OBJ_N_ADS_PRICE_UIS);
		oAdsPriceUIs?.SetActive(ePriceType == EPriceType.ADS);

		var oGoodsPriceUIs = a_oSaleProductUIs.ExFindChild(KCDefine.U_OBJ_N_GOODS_PRICE_UIS);
		oGoodsPriceUIs?.SetActive(ePriceType == EPriceType.GOODS);

		var oPurchasePriceUIs = a_oSaleProductUIs.ExFindChild(KCDefine.U_OBJ_N_PURCHASE_PRICE_UIS);
		oPurchasePriceUIs?.SetActive(ePriceType == EPriceType.PURCHASE);

		var oPriceUIs = (ePriceType == EPriceType.GOODS) ? oGoodsPriceUIs : oPurchasePriceUIs;

		// 텍스트를 설정한다 {
		var oPriceText = a_oSaleProductUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_PRICE_TEXT);
		oPriceText?.ExSetText(string.Format(KCDefine.B_TEXT_FMT_USD_PRICE, a_stSaleProductInfo.m_oPrice), EFontSet.A, false);
		
		a_oSaleProductUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_NAME_TEXT)?.ExSetText(a_stSaleProductInfo.m_oName, EFontSet.A, false);

#if !UNITY_EDITOR && PURCHASE_MODULE_ENABLE
		// 결제 비용 타입 일 경우
		if(ePriceType == EPriceType.PURCHASE && Access.GetProduct(Access.GetSaleProductID(a_stSaleProductInfo.m_eSaleProductKinds)) != null) {
			int nID = Access.GetSaleProductID(a_stSaleProductInfo.m_eSaleProductKinds);
			oPriceText?.ExSetText(Access.GetPriceStr(nID), EFontSet.A, false);
		}
#endif			// #if !UNITY_EDITOR && PURCHASE_MODULE_ENABLE
		// 텍스트를 설정한다 }

		// 버튼을 설정한다 {
		var oPurchaseBtn = oPriceUIs?.ExFindComponentInParent<Button>(KCDefine.U_OBJ_N_PURCHASE_BTN);
		oPurchaseBtn?.ExAddListener(() => this.OnTouchPurchaseBtn(a_stSaleProductInfo));

#if ADS_MODULE_ENABLE
		// 광고 비용 타입 일 경우
		if(ePriceType == EPriceType.ADS) {
			var oTouchInteractable = oPurchaseBtn?.gameObject.ExAddComponent<CRewardAdsTouchInteractable>();
			oTouchInteractable?.SetAdsPlatform(CPluginInfoTable.Inst.AdsPlatform);
		}
#endif			// #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
		// 비소모 상품 일 경우
		if(a_stSaleProductInfo.m_eProductType == ProductType.NonConsumable) {
			int nID = Access.GetSaleProductID(a_stSaleProductInfo.m_eSaleProductKinds);
			var stProductInfo = CProductInfoTable.Inst.GetProductInfo(nID);

			oPurchaseBtn?.ExSetInteractable(!CPurchaseManager.Inst.IsPurchaseNonConsumableProduct(stProductInfo.m_oID));
		}
#endif			// #if PURCHASE_MODULE_ENABLE
		// 버튼을 설정한다 }

		// 패키지 상품 일 경우
		if(eSaleProductType == ESaleProductType.PKGS) {
			this.UpdatePkgsSaleProductUIsState(a_oSaleProductUIs, a_stSaleProductInfo);
		} else {
			this.UpdateSingleSaleProductUIsState(a_oSaleProductUIs, a_stSaleProductInfo);
		}
	}

	/** 패키지 판매 상품 UI 상태를 갱신한다 */
	private void UpdatePkgsSaleProductUIsState(GameObject a_oSaleProductUIs, STSaleProductInfo a_stSaleProductInfo) {
		// 텍스트를 갱신한다
		for(int i = 0; i < a_stSaleProductInfo.m_oItemInfoList.Count; ++i) {
			string oName = string.Format(KCDefine.U_OBJ_N_FMT_NUM_TEXT, i + KCDefine.B_VAL_1_INT);

			var oNumText = a_oSaleProductUIs.ExFindComponent<TMP_Text>(oName);
			oNumText?.ExSetText($"{a_stSaleProductInfo.m_oItemInfoList[i].m_nNumItems}", EFontSet.A, false);
		}
	}

	/** 단일 판매 상품 UI 상태를 갱신한다 */
	private void UpdateSingleSaleProductUIsState(GameObject a_oSaleProductUIs, STSaleProductInfo a_stSaleProductInfo) {
		// Do Something
	}

	/** 결제 버튼을 눌렀을 경우 */
	private void OnTouchPurchaseBtn(STSaleProductInfo a_stSaleProductInfo) {
		switch(a_stSaleProductInfo.m_ePriceKinds.ExKindsToType()) {
			case EPriceType.ADS: {
#if ADS_MODULE_ENABLE
				m_eSelSaleProductKinds = a_stSaleProductInfo.m_eSaleProductKinds;
				Func.ShowRewardAds(this.OnCloseRewardAds);
#endif			// #if ADS_MODULE_ENABLE
			} break;
			case EPriceType.GOODS: {
				// Do Something
			} break;
			case EPriceType.PURCHASE: {
#if PURCHASE_MODULE_ENABLE
				Func.PurchaseProduct(a_stSaleProductInfo.m_eSaleProductKinds, this.OnPurchaseProduct);
#endif			// #if PURCHASE_MODULE_ENABLE
			} break;
		}
	}

	/** 복원 버튼을 눌렀을 경우 */
	private void OnTouchRestoreBtn() {
#if PURCHASE_MODULE_ENABLE
		Func.RestoreProducts(this.OnRestoreProducts);
#endif			// #if PURCHASE_MODULE_ENABLE
	}
	#endregion			// 함수
	
	#region 조건부 함수
#if ADS_MODULE_ENABLE
	/** 보상 광고가 닫혔을 경우 */
	private void OnCloseRewardAds(CAdsManager a_oSender, STAdsRewardInfo a_stAdsRewardInfo, bool a_bIsSuccess) {
		// 광고를 시청했을 경우
		if(a_bIsSuccess) {
			var stSaleProductInfo = CSaleProductInfoTable.Inst.GetSaleProductInfo(m_eSelSaleProductKinds);

			for(int i = 0; i < stSaleProductInfo.m_oItemInfoList.Count; ++i) {
				Func.AcquireItem(stSaleProductInfo.m_oItemInfoList[i]);
			}
		}

		this.UpdateUIsState();
		m_stParams.m_oAdsCallbackDictA.GetValueOrDefault(ECallback.ADS, null)?.Invoke(a_oSender, a_stAdsRewardInfo, a_bIsSuccess);
	}
#endif			// #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	/** 상품이 결제 되었을 경우 */
	private void OnPurchaseProduct(CPurchaseManager a_oSender, string a_oProductID, bool a_bIsSuccess) {
		// 결제 되었을 경우
		if(a_bIsSuccess) {
			Func.AcquireProduct(a_oProductID);

#if FIREBASE_MODULE_ENABLE
			m_oPurchaseProductID = a_oProductID;
			this.ExLateCallFunc((a_oCallFuncSender) => Func.SaveUserInfo(this.OnSaveUserInfo));
#else
			Func.OnPurchaseProduct(a_oSender, a_oProductID, a_bIsSuccess, null);
#endif			// #if FIREBASE_MODULE_ENABLE
		} else {
			Func.OnPurchaseProduct(a_oSender, a_oProductID, a_bIsSuccess, null);
		}

		this.UpdateUIsState();
		m_stParams.m_oPurchaseCallbackDictA.GetValueOrDefault(ECallback.PURCHASE, null)?.Invoke(a_oSender, a_oProductID, a_bIsSuccess);
	}

	/** 상품이 복원 되었을 경우 */
	public void OnRestoreProducts(CPurchaseManager a_oSender, List<Product> a_oProductList, bool a_bIsSuccess) {
		// 복원 되었을 경우
		if(a_bIsSuccess) {
			Func.AcquireRestoreProducts(a_oProductList);

#if FIREBASE_MODULE_ENABLE
			m_oRestoreProductList = a_oProductList;
			this.ExLateCallFunc((a_oCallFuncSender) => Func.LoadPostItemInfos(this.OnLoadPostItemInfos));
#else
			Func.OnRestoreProducts(a_oSender, a_oProductList, a_bIsSuccess, null);
#endif			// #if FIREBASE_MODULE_ENABLE
		} else {
			Func.OnRestoreProducts(a_oSender, a_oProductList, a_bIsSuccess, null);
		}

		this.UpdateUIsState();
		m_stParams.m_oPurchaseCallbackDictB.GetValueOrDefault(ECallback.RESTORE, null)?.Invoke(a_oSender, a_oProductList, a_bIsSuccess);
	}

#if FIREBASE_MODULE_ENABLE
	/** 결제 정보를 로드했을 경우 */
	private void OnLoadPurchaseInfos(CFirebaseManager a_oSender, string a_oJSONStr, bool a_bIsSuccess) {
		// TODO: 로드 처리 로직 구현 필요
	}

	/** 지급 아이템 정보를 로드했을 경우 */
	private void OnLoadPostItemInfos(CFirebaseManager a_oSender, string a_oJSONStr, bool a_bIsSuccess) {
		// 로드 되었을 경우
		if(a_bIsSuccess && a_oJSONStr.ExIsValid()) {
			var oPostItemInfoList = a_oJSONStr.ExJSONStrToPostItemInfos();

			for(int i = 0; i < oPostItemInfoList.Count; ++i) {
				bool bIsValidA = long.TryParse(oPostItemInfoList[i].m_oNumItems, out long nNumItems);
				bool bIsValidB = oPostItemInfoList[i].m_oItemKinds.ExToTryEnumVal<EItemKinds>(out EItemKinds eItemKinds);

				// 지급 아이템 정보가 유효 할 경우
				if(bIsValidA && bIsValidB && eItemKinds.ExIsValid()) {
					Func.AcquireItem(new STItemInfo() {
						m_nNumItems = nNumItems, m_eItemKinds = eItemKinds
					});
				}
			}

			this.ExLateCallFunc((a_oCallFuncSender) => { oPostItemInfoList.Clear(); Func.SavePostItemInfos(oPostItemInfoList, this.OnSavePostItemInfos); });
		} else {
			Func.OnRestoreProducts(CPurchaseManager.Inst, m_oRestoreProductList, true, null);
		}

		this.UpdateUIsState();
	}

	/** 결제 정보를 저장했을 경우 */
	private void OnSavePurchaseInfos(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		// TODO: 저장 처리 로직 구현 필요
	}

	/** 지급 아이템 정보를 저장했을 경우 */
	private void OnSavePostItemInfos(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		Func.OnRestoreProducts(CPurchaseManager.Inst, m_oRestoreProductList, true, null);
	}

	/** 유저 정보를 저장했을 경우 */
	private void OnSaveUserInfo(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		Func.OnPurchaseProduct(CPurchaseManager.Inst, m_oPurchaseProductID, true, null);
	}
#endif			// #if FIREBASE_MODULE_ENABLE
#endif			// #if PURCHASE_MODULE_ENABLE
	#endregion			// 조건부 함수

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
