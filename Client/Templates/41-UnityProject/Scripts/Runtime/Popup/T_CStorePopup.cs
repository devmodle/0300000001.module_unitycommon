#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.Linq;
using TMPro;

#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif // #if PURCHASE_MODULE_ENABLE

/** 상점 팝업 */
public partial class CStorePopup : CSubPopup {
	/** 콜백 */
	public enum ECallback {
		NONE = -1,
		ADS,
		RESTORE,
		PURCHASE,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public List<STProductTradeInfo> m_oProductTradeInfoList;

#if ADS_MODULE_ENABLE
		public Dictionary<ECallback, System.Action<CAdsManager, STAdsRewardInfo, bool>> m_oAdsCallbackDict;
#endif // #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
		public Dictionary<ECallback, System.Action<CPurchaseManager, string, bool>> m_oPurchaseCallbackDictA;
		public Dictionary<ECallback, System.Action<CPurchaseManager, List<Product>, bool>> m_oPurchaseCallbackDictB;
#endif // #if PURCHASE_MODULE_ENABLE
	}

	#region 변수
	private EProductKinds m_eSelProductKinds = EProductKinds.NONE;
	private CStoreUIsHandler m_oStoreUIsHandler = null;

#if PURCHASE_MODULE_ENABLE
	private List<Product> m_oRestoreProductList = new List<Product>();
#endif // #if PURCHASE_MODULE_ENABLE

	[Header("=====> Game Objects <=====")]
	[SerializeField] private List<GameObject> m_oProductBuyUIsList = new List<GameObject>();
	#endregion // 변수

	#region 프로퍼티
	public STParams Params { get; private set; }
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		m_oStoreUIsHandler = this.ContentsUIs.GetComponentInChildren<CStoreUIsHandler>();

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
			(KCDefine.U_OBJ_N_RESTORE_BTN, this.ContentsUIs, this.OnTouchRestoreBtn)
		});

		this.SubAwake();
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();
		this.Params = a_stParams;

		// 상품 교환 정보를 설정한다
		a_stParams.m_oProductTradeInfoList.ExStableSort((a_stLhs, a_stRhs) => a_stLhs.m_nProductIdx.CompareTo(a_stRhs.m_nProductIdx));

		// 상점 UI 처리자가 존재 할 경우
		if(m_oStoreUIsHandler != null) {
			var stHandlerParams = CStoreUIsHandler.MakeParams(a_stParams.m_oProductTradeInfoList);

#if ADS_MODULE_ENABLE
			stHandlerParams.m_oAdsCallbackDict.TryAdd(CStoreUIsHandler.ECallback.ADS, (a_oSender, a_stAdsRewardInfo, a_bIsSuccess) => {
				a_stParams.m_oAdsCallbackDict?.ExGetVal(ECallback.ADS)?.Invoke(a_oSender, a_stAdsRewardInfo, a_bIsSuccess);
			});
#endif // #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
			stHandlerParams.m_oPurchaseCallbackDictA.TryAdd(CStoreUIsHandler.ECallback.PURCHASE, (a_oSender, a_oProductID, a_bIsSuccess) => {
				a_stParams.m_oPurchaseCallbackDictA?.GetValueOrDefault(ECallback.PURCHASE)?.Invoke(a_oSender, a_oProductID, a_bIsSuccess);
			});

			stHandlerParams.m_oPurchaseCallbackDictB.TryAdd(CStoreUIsHandler.ECallback.RESTORE, (a_oSender, a_oProductList, a_bIsSuccess) => {
				a_stParams.m_oPurchaseCallbackDictB?.GetValueOrDefault(ECallback.RESTORE)?.Invoke(a_oSender, a_oProductList, a_bIsSuccess);
			});
#endif // #if PURCHASE_MODULE_ENABLE

			m_oStoreUIsHandler.Init(stHandlerParams);
		}

		this.SubInit();
	}

	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		// 상품 UI 상태를 갱신한다
		for(int i = 0; i < m_oProductBuyUIsList.Count; ++i) {
			this.UpdateProductBuyUIsState(m_oProductBuyUIsList[i], this.Params.m_oProductTradeInfoList[i]);
		}

		this.SubUpdateUIsState();
	}

	/** 상품 구입 UI 상태를 갱신한다 */
	private void UpdateProductBuyUIsState(GameObject a_oProductBuyUI, STProductTradeInfo a_stProductTradeInfo) {
		var oPriceUIsDict = CCollectionPoolManager.Inst.SpawnDict<EPurchaseType, GameObject>();

		try {
			// 객체를 갱신한다 {
			CFunc.SetupGameObjs(new List<(EPurchaseType, string, GameObject)>() {
				(EPurchaseType.ADS, KCDefine.U_OBJ_N_ADS_PRICE_UIS, a_oProductBuyUI),
				(EPurchaseType.IN_APP_PURCHASE, KCDefine.U_OBJ_N_PURCHASE_PRICE_UIS, a_oProductBuyUI)
			}, oPriceUIsDict);

			foreach(var stKeyVal in oPriceUIsDict) {
				stKeyVal.Value?.SetActive(a_stProductTradeInfo.m_ePurchaseType == stKeyVal.Key);
			}
			// 객체를 갱신한다 }

			// 텍스트를 갱신한다 {
			var oPriceText = a_oProductBuyUI.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_PRICE_TEXT);
			oPriceText?.ExSetText(string.Format(KCDefine.B_TEXT_FMT_USD_PRICE, a_stProductTradeInfo.m_oPayTargetInfoDict.FirstOrDefault().Value.m_stValInfo01.m_dmVal), EFontSet._1, false);

			var oAcquireTargetInfoKeyList = a_stProductTradeInfo.m_oAcquireTargetInfoDict.Keys.ToList();
			a_oProductBuyUI.ExFindComponent<Text>(KCDefine.U_OBJ_N_NAME_TEXT)?.ExSetText(a_stProductTradeInfo.m_stCommonInfo.m_oName, EFontSet._1, false);

			for(int i = 0; i < oAcquireTargetInfoKeyList.Count; ++i) {
				var nUniqueTargetInfoID = oAcquireTargetInfoKeyList[i];
				var oNumText = a_oProductBuyUI.ExFindComponent<TMP_Text>(string.Format(KCDefine.U_OBJ_N_FMT_NUM_TEXT, i + KCDefine.B_VAL_1_INT));

				// 코인 일 경우
				if(a_stProductTradeInfo.m_oAcquireTargetInfoDict.ExGetVal(nUniqueTargetInfoID).Kinds == (int)EItemKinds.GOODS_ITEM_COINS_01) {
					oNumText?.ExSetText($"{a_stProductTradeInfo.m_oAcquireTargetInfoDict.ExGetVal(nUniqueTargetInfoID).m_stValInfo01.m_dmVal:0}", EFontSet._1, false);
				} else {
					oNumText?.ExSetText(string.Format(KCDefine.B_TEXT_FMT_CROSS, $"{a_stProductTradeInfo.m_oAcquireTargetInfoDict.ExGetVal(nUniqueTargetInfoID).m_stValInfo01.m_dmVal:0}"), EFontSet._1, false);
				}
			}

#if !UNITY_EDITOR && PURCHASE_MODULE_ENABLE
			// 인앱 결제 상품 일 경우
			if(a_stProductTradeInfo.m_ePurchaseType == EPurchaseType.IN_APP_PURCHASE && Access.GetProduct(a_stProductTradeInfo.m_nProductIdx) != null) {
				oPriceText?.ExSetText(Access.GetPriceStr(a_stProductTradeInfo.m_nProductIdx), EFontSet._1, false);
			}
#endif // #if !UNITY_EDITOR && PURCHASE_MODULE_ENABLE
			// 텍스트를 갱신한다 }

			// 버튼을 갱신한다 {
			var oPurchaseBtn = oPriceUIsDict[EPurchaseType.IN_APP_PURCHASE]?.ExFindComponent<Button>(KCDefine.U_OBJ_N_PURCHASE_BTN);
			oPurchaseBtn?.ExAddListener(() => this.OnTouchPurchaseBtn(a_stProductTradeInfo));

#if PURCHASE_MODULE_ENABLE
			var stProductInfo = CProductInfoTable.Inst.GetProductInfo(a_stProductTradeInfo.m_nProductIdx);

			// 비소모 상품 일 경우
			if(stProductInfo.m_eProductType == ProductType.NonConsumable) {
				oPurchaseBtn?.ExSetInteractable(!CPurchaseManager.Inst.IsPurchaseNonConsumableProduct(stProductInfo.m_oID));
			}
#endif // #if PURCHASE_MODULE_ENABLE
			// 버튼을 갱신한다 }

			// 패키지 상품 일 경우
			if(a_stProductTradeInfo.ProductType == EProductType.PKGS) {
				this.UpdatePkgsProductBuyUIsState(a_oProductBuyUI, a_stProductTradeInfo);
			} else {
				this.UpdateSingleProductBuyUIsState(a_oProductBuyUI, a_stProductTradeInfo);
			}
		} finally {
			CCollectionPoolManager.Inst.DespawnDict(oPriceUIsDict);
		}
	}

	/** 복원 버튼을 눌렀을 경우 */
	private void OnTouchRestoreBtn() {
#if PURCHASE_MODULE_ENABLE
		m_oRestoreProductList.Clear();
		Func.RestoreProducts(this.OnRestoreProducts);
#endif // #if PURCHASE_MODULE_ENABLE
	}

	/** 결제 버튼을 눌렀을 경우 */
	private void OnTouchPurchaseBtn(STProductTradeInfo a_stProductTradeInfo) {
		switch(a_stProductTradeInfo.m_ePurchaseType) {
			case EPurchaseType.ADS: {
#if ADS_MODULE_ENABLE
				m_eSelProductKinds = a_stProductTradeInfo.m_eProductKinds;
				Func.ShowRewardAds(this.OnCloseRewardAds);
#endif // #if ADS_MODULE_ENABLE

				break;
			}
			case EPurchaseType.TARGET: {
				Func.Trade(CGameInfoStorage.Inst.PlayCharacterID, a_stProductTradeInfo);
				break;
			}
			case EPurchaseType.IN_APP_PURCHASE: {
#if PURCHASE_MODULE_ENABLE
				CSceneManager.GetSceneManager<OverlayScene.CSubOverlaySceneManager>()?.PurchaseProduct(a_stProductTradeInfo.m_eProductKinds, this.OnPurchaseProduct);
#endif // #if PURCHASE_MODULE_ENABLE

				break;
			}
		}
	}
	#endregion // 함수

	#region 조건부 함수
#if ADS_MODULE_ENABLE
	/** 보상 광고가 닫혔을 경우 */
	private void OnCloseRewardAds(CAdsManager a_oSender, STAdsRewardInfo a_stAdsRewardInfo, bool a_bIsSuccess) {
		// 광고를 시청했을 경우
		if(a_bIsSuccess) {
			Func.Trade(CGameInfoStorage.Inst.PlayCharacterID, CProductTradeInfoTable.Inst.GetBuyProductTradeTradeInfo(m_eSelProductKinds));
		}

		this.UpdateUIsState();
		this.Params.m_oAdsCallbackDict?.ExGetVal(ECallback.ADS)?.Invoke(a_oSender, a_stAdsRewardInfo, a_bIsSuccess);
	}
#endif // #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	/** 상품이 복원되었을 경우 */
	public void OnRestoreProducts(CPurchaseManager a_oSender, List<Product> a_oProductList, bool a_bIsSuccess) {
		// 복원되었을 경우
		if(a_bIsSuccess) {
			m_oRestoreProductList = a_oProductList;
			Func.AcquireRestoreProducts(a_oProductList);
		}

#if FIREBASE_MODULE_ENABLE
		this.ExLateCallFunc((a_oFuncSender) => Func.LoadTargetInfos(this.OnLoadTargetInfos));
#else
		Func.OnRestoreProducts(a_oSender, a_oProductList, a_bIsSuccess, null);
#endif // #if FIREBASE_MODULE_ENABLE

		this.UpdateUIsState();
		this.Params.m_oPurchaseCallbackDictB?.GetValueOrDefault(ECallback.RESTORE)?.Invoke(a_oSender, a_oProductList, a_bIsSuccess);
	}

	/** 상품이 결제되었을 경우 */
	private void OnPurchaseProduct(CPurchaseManager a_oSender, string a_oProductID, bool a_bIsSuccess) {
		// 결제되었을 경우
		if(a_bIsSuccess) {
			// Do Something
		}

		this.UpdateUIsState();
		this.Params.m_oPurchaseCallbackDictA?.GetValueOrDefault(ECallback.PURCHASE)?.Invoke(a_oSender, a_oProductID, a_bIsSuccess);
	}

#if FIREBASE_MODULE_ENABLE
	/** 타겟 정보를 로드했을 경우 */
	private void OnLoadTargetInfos(CFirebaseManager a_oSender, string a_oJSONStr, bool a_bIsSuccess) {
		// 로드되었을 경우
		if(a_bIsSuccess && a_oJSONStr.ExIsValid()) {
			var oTargetInfoDict = a_oJSONStr.ExJSONStrToTargetInfos();
			Func.Acquire(CGameInfoStorage.Inst.PlayCharacterID, oTargetInfoDict, true);

			this.ExLateCallFunc((a_oFuncSender) => { oTargetInfoDict.Clear(); Func.SaveTargetInfos(oTargetInfoDict, this.OnSaveTargetInfos); });
		} else {
			Func.OnRestoreProducts(CPurchaseManager.Inst, m_oRestoreProductList, m_oRestoreProductList.ExIsValid(), null);
		}

		this.UpdateUIsState();
	}

	/** 타겟 정보를 저장했을 경우 */
	private void OnSaveTargetInfos(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		Func.OnRestoreProducts(CPurchaseManager.Inst, m_oRestoreProductList, m_oRestoreProductList.ExIsValid(), null);
	}
#endif // #if FIREBASE_MODULE_ENABLE
#endif // #if PURCHASE_MODULE_ENABLE
	#endregion // 조건부 함수
}

/** 상점 팝업 - 팩토리 */
public partial class CStorePopup : CSubPopup {
	#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(List<STProductTradeInfo> a_oProductTradeInfoList) {
		return new STParams() {
			m_oProductTradeInfoList = a_oProductTradeInfoList,

#if ADS_MODULE_ENABLE
			m_oAdsCallbackDict = new Dictionary<ECallback, System.Action<CAdsManager, STAdsRewardInfo, bool>>(),
#endif // #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
			m_oPurchaseCallbackDictA = new Dictionary<ECallback, System.Action<CPurchaseManager, string, bool>>(),
			m_oPurchaseCallbackDictB = new Dictionary<ECallback, System.Action<CPurchaseManager, List<Product>, bool>>()
#endif // #if PURCHASE_MODULE_ENABLE
		};
	}
	#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
