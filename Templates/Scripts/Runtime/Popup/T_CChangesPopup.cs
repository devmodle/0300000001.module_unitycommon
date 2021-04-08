using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 잔돈 팝업
public class CChangesPopup : CSubPopup {
	#region UI 변수
	private Button m_oPurchaseBtn = null;
	#endregion			// UI 변수

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
	}

	//! 초기화
	public virtual void Init() {
		base.Init();
		this.UpdateUIsState();
	}

	//! UI 상태를 변경한다
	private void UpdateUIsState() {
		// Do Nothing
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
#if PURCHASE_MODULE_ENABLE
	//! 결제가 완료 되었을 경우
	private void OnCompletePurchase(CPurchaseManager a_oSender, string a_oProductID, bool a_bIsSuccess) {
		// 결제 되었을 경우
		if(a_bIsSuccess) {
			int nIdx = CProductInfoTable.Inst.GetProductInfoIdx(a_oProductID);
			var stSaleProductInfo = CSaleProductInfoTable.Inst.GetSaleProductInfo(nIdx);

			for(int i = 0; i < stSaleProductInfo.m_oItemInfoList.Count; ++i) {
				Func.AcquireItem(stSaleProductInfo.m_oItemInfoList[i]);
			}
		}
	}
#endif			// #if PURCHASE_MODULE_ENABLE
	#endregion			// 조건부 함수
}
#endif			// #if NEVER_USE_THIS
