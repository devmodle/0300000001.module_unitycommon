using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if RUNTIME_TEMPLATES_MODULE_ENABLE
#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif			// #if PURCHASE_MODULE_ENABLE

/** 기본 로그 함수 */
public static partial class LogFunc {
	#region 클래스 함수
	/** 로그를 전송한다 */
	public static void SendLog(string a_oName, Dictionary<string, object> a_oDataDict, float? a_oVal = null) {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
#if ANALYTICS_TEST_ENABLE
		bool bIsEnableSendLog = true;
#else
		bool bIsEnableSendLog = false;
#endif			// #if ANALYTICS_TEST_ENABLE

		// 로그 전송이 가능 할 경우
		if(bIsEnableSendLog || !CCommonAppInfoStorage.Inst.IsTestDevice()) {
			var oDataDict = a_oDataDict ?? new Dictionary<string, object>();

			oDataDict.TryAdd(KCDefine.L_LOG_KEY_PLATFORM, CCommonAppInfoStorage.Inst.Platform);
			oDataDict.TryAdd(KCDefine.L_LOG_KEY_DEVICE_ID, CCommonAppInfoStorage.Inst.AppInfo.DeviceID);

#if AUTO_LOG_PARAMS_ENABLE
#if ANALYTICS_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
			oDataDict.TryAdd(KCDefine.L_LOG_KEY_USER_TYPE, KCDefine.B_TEXT_UNKNOWN);
#else
			oDataDict.TryAdd(KCDefine.L_LOG_KEY_USER_TYPE, CCommonUserInfoStorage.Inst.UserInfo.UserType.ToString());
#endif			// #if ANALYTICS_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)

			oDataDict.TryAdd(KCDefine.L_LOG_KEY_LOG_TIME, System.DateTime.UtcNow.ExToLongStr());

#if NEWTON_SOFT_JSON_MODULE_ENABLE
			oDataDict.TryAdd(KCDefine.L_LOG_KEY_INSTALL_TIME, CCommonAppInfoStorage.Inst.AppInfo.UTCInstallTime.ExToLongStr());
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
#endif			// #if AUTO_LOG_PARAMS_ENABLE

			CServicesManager.Inst.SendLog(a_oName, oDataDict);

#if FLURRY_MODULE_ENABLE
			// 플러리 분석이 가능 할 경우
			if(KDefine.G_ANALYTICS_LOG_ENABLE_LIST.Contains(EAnalytics.FLURRY)) {
				var oFlurryDataDict = (oDataDict != null) ? oDataDict.ExToTypes<string, object, string, string>() : null;
				CFlurryManager.Inst.SendLog(a_oName, oFlurryDataDict);
			}
#endif			// #if FLURRY_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
			// 파이어 베이스 분석이 가능 할 경우
			if(KDefine.G_ANALYTICS_LOG_ENABLE_LIST.Contains(EAnalytics.FIREBASE)) {
				var oFirebaseDataDict = (oDataDict != null) ? oDataDict.ExToTypes<string, object, string, string>() : null;
				CFirebaseManager.Inst.SendLog(a_oName, oFirebaseDataDict);
			}
#endif			// #if FIREBASE_MODULE_ENABLE

#if APPS_FLYER_MODULE_ENABLE
			// 앱스 플라이어 분석이 가능 할 경우
			if(KDefine.G_ANALYTICS_LOG_ENABLE_LIST.Contains(EAnalytics.APPS_FLYER)) {
				var oAppsFlyerDataDict = (oDataDict != null) ? oDataDict.ExToTypes<string, object, string, string>() : null;
				CAppsFlyerManager.Inst.SendLog(a_oName, oAppsFlyerDataDict);
			}
#endif			// #if APPS_FLYER_MODULE_ENABLE
		}
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
	}
	#endregion			// 클래스 함수

	#region 조건부 클래스 함수
#if PURCHASE_MODULE_ENABLE
	/** 결제 로그를 전송한다 */
	public static void SendPurchaseLog(Product a_oProduct, int a_nNumProducts = KCDefine.B_VAL_1_INT) {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
#if ANALYTICS_TEST_ENABLE
		bool bIsEnableSendLog = true;
#else
		bool bIsEnableSendLog = false;
#endif			// #if ANALYTICS_TEST_ENABLE

		// 로그 전송이 가능 할 경우
		if(bIsEnableSendLog || !CCommonAppInfoStorage.Inst.IsTestDevice()) {
			CServicesManager.Inst.SendPurchaseLog(a_oProduct, a_nNumProducts);

#if FLURRY_MODULE_ENABLE
			// 플러리 분석이 가능 할 경우
			if(KDefine.G_ANALYTICS_PURCHASE_LOG_ENABLE_LIST.Contains(EAnalytics.FLURRY)) {
				CFlurryManager.Inst.SendPurchaseLog(a_oProduct, a_nNumProducts);
			}
#endif			// #if FLURRY_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
			// 파이어 베이스 분석이 가능 할 경우
			if(KDefine.G_ANALYTICS_PURCHASE_LOG_ENABLE_LIST.Contains(EAnalytics.FIREBASE)) {
				CFirebaseManager.Inst.SendPurchaseLog(a_oProduct, a_nNumProducts);
			}
#endif			// #if FIREBASE_MODULE_ENABLE

#if APPS_FLYER_MODULE_ENABLE
			// 앱스 플라이어 분석이 가능 할 경우
			if(KDefine.G_ANALYTICS_PURCHASE_LOG_ENABLE_LIST.Contains(EAnalytics.APPS_FLYER)) {
				CAppsFlyerManager.Inst.SendPurchaseLog(a_oProduct, a_nNumProducts);
			}
#endif			// #if APPS_FLYER_MODULE_ENABLE
		}
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
	}
#endif			// #if PURCHASE_MODULE_ENABLE
	#endregion			// 조건부 클래스 함수

	#region 추가 클래스 함수

	#endregion			// 추가 클래스 함수
}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
