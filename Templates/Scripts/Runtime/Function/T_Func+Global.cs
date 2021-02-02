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

	#endregion			// 클래스 함수

	#region 조건부 클래스 함수
#if PURCHASE_MODULE_ENABLE
	//! 상품 결제가 완료 되었을 경우
	public static void OnCompletePurchaseProduct(CPurchaseManager a_oSender, string a_oID, bool a_bIsSuccess, System.Action<CAlertPopup, bool> a_oCallback) {
		// 상품 결제에 성공했을 경우
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
