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

/** 상품 판매 팝업 */
public partial class CProductSalePopup : CSubPopup {
	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();
		m_stParams = a_stParams;
	}

	/** UI 상태를 갱신한다 */
	private new void UpdateUIsState() {
		for(int i = 0; i < m_oProductSaleUIsDict.Count; ++i) {
			this.UpdateProductSaleUIsState(m_oProductSaleUIsDict[i], CProductSaleInfoTable.Inst.GetProductSaleInfo(KDefine.G_PRODUCT_KINDS_SPECIAL_PKGS_LIST[i]));
		}
	}

	/** 상품 판매 UI 상태를 갱신한다 */
	private void UpdateProductSaleUIsState(GameObject a_oSpecialPkgsUIs, STProductSaleInfo a_stProductSaleInfo) {
		// 텍스트를 설정한다 {
		var oPriceText = a_oSpecialPkgsUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_PRICE_TEXT);
		oPriceText?.ExSetText(string.Format(KCDefine.B_TEXT_FMT_USD_PRICE, a_stProductSaleInfo.m_oPayTargetInfoDict.First().Value.m_stValInfo01.m_nVal), EFontSet._1, false);

		a_oSpecialPkgsUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_NAME_TEXT)?.ExSetText(CStrTable.Inst.GetStr(a_stProductSaleInfo.m_stCommonInfo.m_oName), EFontSet._1, false);
		a_oSpecialPkgsUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_PRICE_TEXT)?.ExSetText(string.Format(KCDefine.B_TEXT_FMT_USD_PRICE, a_stProductSaleInfo.m_oPayTargetInfoDict.First().Value.m_stValInfo01.m_nVal), EFontSet._1, false);

#if !UNITY_EDITOR && PURCHASE_MODULE_ENABLE
		// 상품이 존재 할 경우
		if(Access.GetProduct(a_stProductSaleInfo.m_nTableIdx) != null) {
			oPriceText?.ExSetText(Access.GetPriceStr(a_stProductSaleInfo.m_nTableIdx), EFontSet._1, false);
		}
#endif			// #if !UNITY_EDITOR && PURCHASE_MODULE_ENABLE
		// 텍스트를 설정한다 }

		// 버튼을 설정한다 {
		var oPurchaseBtn = a_oSpecialPkgsUIs?.ExFindComponent<Button>(KCDefine.U_OBJ_N_PURCHASE_BTN);
		oPurchaseBtn?.ExAddListener(() => this.OnTouchPurchaseBtn(a_stProductSaleInfo));

#if PURCHASE_MODULE_ENABLE
		var stProductInfo = CProductInfoTable.Inst.GetProductInfo(a_stProductSaleInfo.m_nTableIdx);

		// 비소모 상품 일 경우
		if(stProductInfo.m_eProductType == ProductType.NonConsumable) {
			oPurchaseBtn?.ExSetInteractable(!CPurchaseManager.Inst.IsPurchaseNonConsumableProduct(stProductInfo.m_oID));
		}
#endif			// #if PURCHASE_MODULE_ENABLE
		// 버튼을 설정한다 }
	}
	#endregion			// 함수
}

/** 서브 상품 판매 팝업 */
public partial class CProductSalePopup : CSubPopup {
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

	#endregion			// 함수

	#region 조건부 함수
#if PURCHASE_MODULE_ENABLE
	/** 상품이 결제 되었을 경우 */
	private void OnPurchaseProduct(CPurchaseManager a_oSender, string a_oProductID, bool a_bIsSuccess) {
		// 결제 되었을 경우
		if(a_bIsSuccess) {
			// Do Something
		}

		this.UpdateUIsState();
		m_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.PURCHASE)?.Invoke(a_oSender, a_oProductID, a_bIsSuccess);
	}
#endif			// #if PURCHASE_MODULE_ENABLE
	#endregion			// 조건부 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
