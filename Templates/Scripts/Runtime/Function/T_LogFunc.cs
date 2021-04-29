using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif			// #if PURCHASE_MODULE_ENABLE

//! 기본 로그 함수
public static partial class LogFunc {
	#region 클래스 함수
	//! 로그를 전송한다
	public static void SendLog(string a_oName, Dictionary<string, object> a_oDataList, float? a_oVal = null) {
#if ANALYTICS_TEST_ENABLE
		bool bIsEnableSendLog = true;
#else
		bool bIsEnableSendLog = false;
#endif			// #if ANALYTICS_TEST_ENABLE

		// 로그 전송이 가능 할 경우
		if(bIsEnableSendLog || !CCommonAppInfoStorage.Inst.IsTestDevice()) {
			CServicesManager.Inst.SendLog(a_oName, a_oDataList);

#if FLURRY_MODULE_ENABLE
			var oFlurryDataList = (a_oDataList != null) ? a_oDataList.ExToTypes<string, object, string, string>() : null;
			CFlurryManager.Inst.SendLog(a_oName, oFlurryDataList);
#endif			// #if FLURRY_MODULE_ENABLE

#if TENJIN_MODULE_ENABLE
			var oTenjinDataList = (a_oDataList != null) ? a_oDataList.ExToListTypes<string, object, string>() : null;
			CTenjinManager.Inst.SendLog(a_oName, oTenjinDataList);
#endif			// #if TENJIN_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
			var oFirebaseDataList = (a_oDataList != null) ? a_oDataList.ExToTypes<string, object, string, string>() : null;
			CFirebaseManager.Inst.SendLog(a_oName, oFirebaseDataList);
#endif			// #if FIREBASE_MODULE_ENABLE

#if SINGULAR_MODULE_ENABLE
			CSingularManager.Inst.SendLog(a_oName, a_oDataList);
#endif			// #if SINGULAR_MODULE_ENABLE
		}
	}
	#endregion			// 클래스 함수

	#region 조건부 클래스 함수
#if PURCHASE_MODULE_ENABLE
	//! 결제 로그를 전송한다
	public static void SendPurchaseLog(Product a_oProduct, int a_nNumProducts = KCDefine.B_VAL_1_INT) {
#if ANALYTICS_TEST_ENABLE
		bool bIsEnableSendLog = true;
#else
		bool bIsEnableSendLog = false;
#endif			// #if ANALYTICS_TEST_ENABLE

		// 로그 전송이 가능 할 경우
		if(bIsEnableSendLog || !CCommonAppInfoStorage.Inst.IsTestDevice()) {
			CServicesManager.Inst.SendPurchaseLog(a_oProduct, a_nNumProducts);

#if FLURRY_MODULE_ENABLE
			CFlurryManager.Inst.SendPurchaseLog(a_oProduct, a_nNumProducts);
#endif			// #if FLURRY_MODULE_ENABLE

#if TENJIN_MODULE_ENABLE
			CTenjinManager.Inst.SendPurchaseLog(a_oProduct, a_nNumProducts);
#endif			// #if TENJIN_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
			CFirebaseManager.Inst.SendPurchaseLog(a_oProduct);
#endif			// #if FIREBASE_MODULE_ENABLE

#if SINGULAR_MODULE_ENABLE
			CSingularManager.Inst.SendPurchaseLog(a_oProduct);
#endif			// #if SINGULAR_MODULE_ENABLE
		}
	}
#endif			// #if PURCHASE_MODULE_ENABLE
	#endregion			// 조건부 클래스 함수
}
#endif			// #if NEVER_USE_THIS
