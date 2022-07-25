using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif			// #if PURCHASE_MODULE_ENABLE

/** 상점 팝업 */
public partial class CStorePopup : CSubPopup {
	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
			(KCDefine.U_OBJ_N_RESTORE_BTN, this.Contents, this.OnTouchRestoreBtn)
		}, false);
	}
	
	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();
		m_stParams = a_stParams;
		
		a_stParams.m_oProductSaleInfoList.Sort((a_stLhs, a_stRhs) => a_stLhs.m_nTableIdx.CompareTo(a_stRhs.m_nTableIdx));
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
		var oPriceUIsDict = CCollectionManager.Inst.SpawnDict<EPurchaseType, GameObject>();

		try {
			// 객체를 갱신한다 {
			CFunc.SetupObjs(new List<(EPurchaseType, string, GameObject)>() {
				(EPurchaseType.ADS, KCDefine.U_OBJ_N_ADS_PRICE_UIS, a_oProductSaleUIs),
				(EPurchaseType.IN_APP_PURCHASE, KCDefine.U_OBJ_N_PURCHASE_PRICE_UIS, a_oProductSaleUIs)
			}, oPriceUIsDict, false);

			foreach(var stKeyVal in oPriceUIsDict) {
				stKeyVal.Value?.SetActive(a_stProductSaleInfo.m_ePurchaseType == stKeyVal.Key);
			}
			// 객체를 갱신한다 }

			// 텍스트를 갱신한다 {
			var oPriceText = a_oProductSaleUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_PRICE_TEXT);
			oPriceText?.ExSetText(string.Format(KCDefine.B_TEXT_FMT_USD_PRICE, a_stProductSaleInfo.m_oPayTargetInfoDict.First().Value.m_stValInfo.m_nVal), EFontSet._1, false);

			var oAcquireTargetInfoKeyList = a_stProductSaleInfo.m_oAcquireTargetInfoDict.Keys.ToList();
			a_oProductSaleUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_NAME_TEXT)?.ExSetText(a_stProductSaleInfo.m_stCommonInfo.m_oName, EFontSet._1, false);

			for(int i = 0; i < oAcquireTargetInfoKeyList.Count; ++i) {
				var oNumText = a_oProductSaleUIs.ExFindComponent<TMP_Text>(string.Format(KCDefine.U_OBJ_N_FMT_NUM_TEXT, i + KCDefine.B_VAL_1_INT));
				var nUniqueTargetInfoID = oAcquireTargetInfoKeyList[i];

				switch(a_stProductSaleInfo.m_oAcquireTargetInfoDict[nUniqueTargetInfoID].m_stValInfo.m_eValType) {
					case EValType.INT: oNumText?.ExSetText($"{a_stProductSaleInfo.m_oAcquireTargetInfoDict[nUniqueTargetInfoID].m_stValInfo.m_nVal}", EFontSet._1, false); break;
					case EValType.REAL: oNumText?.ExSetText($"{a_stProductSaleInfo.m_oAcquireTargetInfoDict[nUniqueTargetInfoID].m_stValInfo.m_dblVal}", EFontSet._1, false); break;
				}
			}

#if !UNITY_EDITOR && PURCHASE_MODULE_ENABLE
			// 인앱 결제 상품 일 경우
			if(a_stProductSaleInfo.m_ePurchaseType == EPurchaseType.IN_APP_PURCHASE && Access.GetProduct(a_stProductSaleInfo.m_nTableIdx) != null) {
				oPriceText?.ExSetText(Access.GetPriceStr(a_stProductSaleInfo.m_nTableIdx), EFontSet._1, false);
			}
#endif			// #if !UNITY_EDITOR && PURCHASE_MODULE_ENABLE
			// 텍스트를 갱신한다 }

			// 버튼을 갱신한다 {
			var oPurchaseBtn = oPriceUIsDict[EPurchaseType.IN_APP_PURCHASE]?.ExFindComponentInParent<Button>(KCDefine.U_OBJ_N_PURCHASE_BTN);
			oPurchaseBtn?.ExAddListener(() => this.OnTouchPurchaseBtn(a_stProductSaleInfo));

#if ADS_MODULE_ENABLE
			// 보상 광고 상품 일 경우
			if(a_stProductSaleInfo.m_ePurchaseType == EPurchaseType.ADS) {
				var oTouchInteractable = oPurchaseBtn?.gameObject.ExAddComponent<CRewardAdsTouchInteractable>();
				oTouchInteractable?.SetAdsPlatform(CPluginInfoTable.Inst.AdsPlatform);
			}
#endif			// #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
			var stProductInfo = CProductInfoTable.Inst.GetProductInfo(a_stProductSaleInfo.m_nTableIdx);

			// 비소모 상품 일 경우
			if(stProductInfo.m_eProductType == ProductType.NonConsumable) {
				oPurchaseBtn?.ExSetInteractable(!CPurchaseManager.Inst.IsPurchaseNonConsumableProduct(stProductInfo.m_oID));
			}
#endif			// #if PURCHASE_MODULE_ENABLE
			// 버튼을 갱신한다 }

			// 패키지 상품 일 경우
			if(a_stProductSaleInfo.ProductSaleType == EProductSaleType.PKGS) {
				this.UpdatePkgsProductSaleUIsState(a_oProductSaleUIs, a_stProductSaleInfo);
			} else {
				this.UpdateSingleProductSaleUIsState(a_oProductSaleUIs, a_stProductSaleInfo);
			}
		} finally {
			CCollectionManager.Inst.DespawnDict(oPriceUIsDict);
		}
	}
	#endregion			// 함수
}

/** 서브 상점 팝업 */
public partial class CStorePopup : CSubPopup {
	/** 서브 식별자 */
	private enum ESubKey {
		NONE = -1,
		[HideInInspector] MAX_VAL
	}

	#region 변수

	#endregion			// 변수

	#region 프로퍼티

	#endregion			// 프로퍼티

	#region 함수
	/** 패키지 상품 판매 UI 상태를 갱신한다 */
	private void UpdatePkgsProductSaleUIsState(GameObject a_oProductSaleUIs, STProductSaleInfo a_stProductSaleInfo) {
		// Do Something
	}

	/** 단일 상품 판매 UI 상태를 갱신한다 */
	private void UpdateSingleProductSaleUIsState(GameObject a_oProductSaleUIs, STProductSaleInfo a_stProductSaleInfo) {
		// Do Something
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
