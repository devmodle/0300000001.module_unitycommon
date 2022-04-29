using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif			// #if PURCHASE_MODULE_ENABLE

/** 상점 팝업 */
public partial class CStorePopup : CSubPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		PURCHASE_PRODUCT_ID,
		[HideInInspector] MAX_VAL
	}

	/** 콜백 */
	public enum ECallback {
		NONE = -1,
		ADS,
		PURCHASE,
		RESTORE,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public List<STProductSaleInfo> m_oProductSaleInfoList;

#if ADS_MODULE_ENABLE
		public Dictionary<ECallback, System.Action<CAdsManager, STAdsRewardInfo, bool>> m_oAdsCallbackDict;
#endif			// #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
		public Dictionary<ECallback, System.Action<CPurchaseManager, string, bool>> m_oPurchaseCallbackDict01;
		public Dictionary<ECallback, System.Action<CPurchaseManager, List<Product>, bool>> m_oPurchaseCallbackDict02;
#endif			// #if PURCHASE_MODULE_ENABLE
	}

	#region 변수
	private STParams m_stParams;
	private EProductSaleKinds m_eSelProductSaleKinds = EProductSaleKinds.NONE;

	private Dictionary<EKey, string> m_oStrDict = new Dictionary<EKey, string>() {
		[EKey.PURCHASE_PRODUCT_ID] = string.Empty
	};

	/** =====> 객체 <===== */
	[SerializeField] private List<GameObject> m_oProductSaleUIsList = new List<GameObject>();

#if PURCHASE_MODULE_ENABLE
	private List<Product> m_oRestoreProductList = new List<Product>();
#endif			// #if PURCHASE_MODULE_ENABLE
	#endregion			// 변수

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 버튼을 설정한다
		this.Contents.ExFindComponent<Button>(KCDefine.U_OBJ_N_RESTORE_BTN)?.onClick.AddListener(this.OnTouchRestoreBtn);
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
		for(int i = 0; i < m_oProductSaleUIsList.Count; ++i) {
			this.UpdateProductSaleUIsState(m_oProductSaleUIsList[i], m_stParams.m_oProductSaleInfoList[i]);
		}
	}

	/** 상품 판매 UI 상태를 갱신한다 */
	private void UpdateProductSaleUIsState(GameObject a_oProductSaleUIs, STProductSaleInfo a_stProductSaleInfo) {
		var ePriceType = (EPriceType)((int)a_stProductSaleInfo.m_ePriceKinds).ExKindsToType();
		var eProductSaleType = (EProductSaleType)((int)a_stProductSaleInfo.m_eProductSaleKinds).ExKindsToType();

		var oAdsPriceUIs = a_oProductSaleUIs.ExFindChild(KCDefine.U_OBJ_N_ADS_PRICE_UIS);
		oAdsPriceUIs?.SetActive(ePriceType == EPriceType.ADS);

		var oGoodsPriceUIs = a_oProductSaleUIs.ExFindChild(KCDefine.U_OBJ_N_GOODS_PRICE_UIS);
		oGoodsPriceUIs?.SetActive(ePriceType == EPriceType.GOODS);

		var oPurchasePriceUIs = a_oProductSaleUIs.ExFindChild(KCDefine.U_OBJ_N_PURCHASE_PRICE_UIS);
		oPurchasePriceUIs?.SetActive(ePriceType == EPriceType.PURCHASE);

		var oPriceUIs = (ePriceType == EPriceType.GOODS) ? oGoodsPriceUIs : oPurchasePriceUIs;

		// 텍스트를 설정한다 {
		var oPriceText = a_oProductSaleUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_PRICE_TEXT);
		oPriceText?.ExSetText(string.Format(KCDefine.B_TEXT_FMT_USD_PRICE, a_stProductSaleInfo.m_oPrice), EFontSet._1, false);
		
		a_oProductSaleUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_NAME_TEXT)?.ExSetText(a_stProductSaleInfo.m_stDescInfo.m_oName, EFontSet._1, false);

		for(int i = 0; i < a_stProductSaleInfo.m_oNumItemsInfoList.Count; ++i) {
			var oNumText = a_oProductSaleUIs.ExFindComponent<TMP_Text>(string.Format(KCDefine.U_OBJ_N_FMT_NUM_TEXT, i + KCDefine.B_VAL_1_INT));
			oNumText?.ExSetText($"{a_stProductSaleInfo.m_oNumItemsInfoList[i].m_nNumItems}", EFontSet._1, false);
		}

#if !UNITY_EDITOR && PURCHASE_MODULE_ENABLE
		// 결제 비용 타입 일 경우
		if(ePriceType == EPriceType.PURCHASE && Access.GetProduct(a_stProductSaleInfo.m_nID) != null) {
			oPriceText?.ExSetText(Access.GetPriceStr(a_stProductSaleInfo.m_nID), EFontSet._1, false);
		}
#endif			// #if !UNITY_EDITOR && PURCHASE_MODULE_ENABLE
		// 텍스트를 설정한다 }

		// 버튼을 설정한다 {
		var oPurchaseBtn = oPriceUIs?.ExFindComponentInParent<Button>(KCDefine.U_OBJ_N_PURCHASE_BTN);
		oPurchaseBtn?.ExAddListener(() => this.OnTouchPurchaseBtn(a_stProductSaleInfo));

#if ADS_MODULE_ENABLE
		// 광고 비용 타입 일 경우
		if(ePriceType == EPriceType.ADS) {
			var oTouchInteractable = oPurchaseBtn?.gameObject.ExAddComponent<CRewardAdsTouchInteractable>();
			oTouchInteractable?.SetAdsPlatform(CPluginInfoTable.Inst.AdsPlatform);
		}
#endif			// #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
		var stProductInfo = CProductInfoTable.Inst.GetProductInfo(a_stProductSaleInfo.m_nID);

		// 비소모 상품 일 경우
		if(stProductInfo.m_eProductType == ProductType.NonConsumable) {
			oPurchaseBtn?.ExSetInteractable(!CPurchaseManager.Inst.IsPurchaseNonConsumableProduct(stProductInfo.m_oID));
		}
#endif			// #if PURCHASE_MODULE_ENABLE
		// 버튼을 설정한다 }

		// 패키지 상품 일 경우
		if(eProductSaleType == EProductSaleType.PKGS) {
			this.UpdatePkgsProductSaleUIsState(a_oProductSaleUIs, a_stProductSaleInfo);
		} else {
			this.UpdateSingleProductSaleUIsState(a_oProductSaleUIs, a_stProductSaleInfo);
		}
	}

	/** 패키지 상품 판매 UI 상태를 갱신한다 */
	private void UpdatePkgsProductSaleUIsState(GameObject a_oProductSaleUIs, STProductSaleInfo a_stProductSaleInfo) {
		// Do Something
	}

	/** 단일 상품 판매 UI 상태를 갱신한다 */
	private void UpdateSingleProductSaleUIsState(GameObject a_oProductSaleUIs, STProductSaleInfo a_stProductSaleInfo) {
		// Do Something
	}

	/** 결제 버튼을 눌렀을 경우 */
	private void OnTouchPurchaseBtn(STProductSaleInfo a_stProductSaleInfo) {
		switch((EPriceType)((int)a_stProductSaleInfo.m_ePriceKinds).ExKindsToType()) {
			case EPriceType.ADS: {
#if ADS_MODULE_ENABLE
				m_eSelProductSaleKinds = a_stProductSaleInfo.m_eProductSaleKinds;
				Func.ShowRewardAds(this.OnCloseRewardAds);
#endif			// #if ADS_MODULE_ENABLE
			} break;
			case EPriceType.GOODS: {
				// Do Something
			} break;
			case EPriceType.PURCHASE: {
#if PURCHASE_MODULE_ENABLE
				CSceneManager.GetSceneManager<OverlayScene.CSubOverlaySceneManager>(KCDefine.B_SCENE_N_OVERLAY)?.PurchaseProduct(a_stProductSaleInfo.m_eProductSaleKinds, this.OnPurchaseProduct);
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
			var stProductSaleInfo = CProductSaleInfoTable.Inst.GetProductSaleInfo(m_eSelProductSaleKinds);

			for(int i = 0; i < stProductSaleInfo.m_oNumItemsInfoList.Count; ++i) {
				Func.AcquireItem(stProductSaleInfo.m_oNumItemsInfoList[i]);
			}
		}

		this.UpdateUIsState();
		m_stParams.m_oAdsCallbackDict?.GetValueOrDefault(ECallback.ADS)?.Invoke(a_oSender, a_stAdsRewardInfo, a_bIsSuccess);
	}
#endif			// #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	/** 상품이 결제 되었을 경우 */
	private void OnPurchaseProduct(CPurchaseManager a_oSender, string a_oProductID, bool a_bIsSuccess) {
		// 결제 되었을 경우
		if(a_bIsSuccess) {
			// Do Something
		}

		this.UpdateUIsState();
		m_stParams.m_oPurchaseCallbackDict01?.GetValueOrDefault(ECallback.PURCHASE)?.Invoke(a_oSender, a_oProductID, a_bIsSuccess);
	}

	/** 상품이 복원 되었을 경우 */
	public void OnRestoreProducts(CPurchaseManager a_oSender, List<Product> a_oProductList, bool a_bIsSuccess) {
		// 복원 되었을 경우
		if(a_bIsSuccess) {
			Func.AcquireRestoreProducts(a_oProductList);
			m_oRestoreProductList = a_oProductList;

#if FIREBASE_MODULE_ENABLE
			this.ExLateCallFunc((a_oCallFuncSender) => Func.LoadAcquireItemInfos(this.OnLoadAcquireItemInfos));
#else
			Func.OnRestoreProducts(a_oSender, a_oProductList, a_bIsSuccess, null);
#endif			// #if FIREBASE_MODULE_ENABLE
		} else {
			Func.OnRestoreProducts(a_oSender, a_oProductList, a_bIsSuccess, null);
		}

		this.UpdateUIsState();
		m_stParams.m_oPurchaseCallbackDict02?.GetValueOrDefault(ECallback.RESTORE)?.Invoke(a_oSender, a_oProductList, a_bIsSuccess);
	}

#if FIREBASE_MODULE_ENABLE
	/** 획득 아이템 정보를 로드했을 경우 */
	private void OnLoadAcquireItemInfos(CFirebaseManager a_oSender, string a_oJSONStr, bool a_bIsSuccess) {
		// 로드 되었을 경우
		if(a_bIsSuccess && a_oJSONStr.ExIsValid()) {
			var oAcquireItemInfoList = a_oJSONStr.ExJSONStrToAcquireItemInfos();

			for(int i = 0; i < oAcquireItemInfoList.Count; ++i) {
				Func.AcquireItem(oAcquireItemInfoList[i]);
			}

			this.ExLateCallFunc((a_oCallFuncSender) => { oAcquireItemInfoList.Clear(); Func.SaveAcquireItemInfos(oAcquireItemInfoList, this.OnSaveAcquireItemInfos); });
		} else {
			Func.OnRestoreProducts(CPurchaseManager.Inst, m_oRestoreProductList, true, null);
		}

		this.UpdateUIsState();
	}
	
	/** 획득 아이템 정보를 저장했을 경우 */
	private void OnSaveAcquireItemInfos(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		Func.OnRestoreProducts(CPurchaseManager.Inst, m_oRestoreProductList, true, null);
	}
#endif			// #if FIREBASE_MODULE_ENABLE
#endif			// #if PURCHASE_MODULE_ENABLE
	#endregion			// 조건부 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
