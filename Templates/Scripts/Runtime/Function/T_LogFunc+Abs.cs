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
	public static void SendLog(string a_oName, 
		Dictionary<string, object> a_oDataList, float? a_oValue = null) 
	{
		// 테스트 디바이스가 아닐 경우
		if(!CCommonAppInfoStorage.Instance.IsTestDevice()) {
#if FLURRY_MODULE_ENABLE
			var oFlurryDataList = (a_oDataList != null) ? 
				a_oDataList.ExToTypes<string, object, string, string>() : null;

			CFlurryManager.Instance.SendLog(a_oName, oFlurryDataList);
#endif			// #if FLURRY_MODULE_ENABLE

#if TENJIN_MODULE_ENABLE
			var oTenjinDataList = (a_oDataList != null) ? 
				a_oDataList.ExToListTypes<string, object, string>() : null;

			CTenjinManager.Instance.SendLog(a_oName, oTenjinDataList);
#endif			// #if TENJIN_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
			CFacebookManager.Instance.SendLog(a_oName, a_oDataList, a_oValue);
#endif			// #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
			var oFirebaseDataList = (a_oDataList != null) ? 
				a_oDataList.ExToTypes<string, object, string, string>() : null;

			CFirebaseManager.Instance.SendLog(a_oName, oFirebaseDataList);
#endif			// #if FIREBASE_MODULE_ENABLE

#if UNITY_SERVICE_MODULE_ENABLE
			CUnityServiceManager.Instance.SendLog(a_oName, a_oDataList);
#endif			// #if UNITY_SERVICE_MODULE_ENABLE

#if SINGULAR_MODULE_ENABLE
			CSingularManager.Instance.SendLog(a_oName, a_oDataList);
#endif			// #if SINGULAR_MODULE_ENABLE
		}
	}
	#endregion			// 클래스 함수

	#region 조건부 클래스 함수
#if PURCHASE_MODULE_ENABLE
	//! 결제 로그를 전송한다
	public static void SendPurchaseLog(Product a_oProduct, 
		int a_nNumProducts, Dictionary<string, object> a_oDataList) 
	{
		// 테스트 디바이스가 아닐 경우
		if(!CCommonAppInfoStorage.Instance.IsTestDevice()) {
#if FLURRY_MODULE_ENABLE
			var oDataList = (a_oDataList != null) ? 
				a_oDataList.ExToTypes<string, object, string, string>() : null;

			CFlurryManager.Instance.SendPurchaseLog(a_oProduct, a_nNumProducts, oDataList);
#endif			// #if FLURRY_MODULE_ENABLE

#if TENJIN_MODULE_ENABLE
			CTenjinManager.Instance.SendPurchaseLog(a_oProduct);
#endif			// #if TENJIN_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
			CFacebookManager.Instance.SendPurchaseLog(a_oProduct, a_oDataList);
#endif			// #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
			CFirebaseManager.Instance.SendPurchaseLog(a_oProduct);
#endif			// #if FIREBASE_MODULE_ENABLE

#if UNITY_SERVICE_MODULE_ENABLE
			CUnityServiceManager.Instance.SendPurchaseLog(a_oProduct, a_oDataList);
#endif			// #if UNITY_SERVICE_MODULE_ENABLE

#if SINGULAR_MODULE_ENABLE
			CSingularManager.Instance.SendPurchaseLog(a_oProduct, a_oDataList);
#endif			// #if SINGULAR_MODULE_ENABLE
		}
	}
#endif			// #if PURCHASE_MODULE_ENABLE
	#endregion			// 조건부 클래스 함수
}
#endif			// #if NEVER_USE_THIS
