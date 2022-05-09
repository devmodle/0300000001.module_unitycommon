using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif			// #if PURCHASE_MODULE_ENABLE

/** 기본 로그 함수 */
public static partial class LogFunc {
	#region 클래스 함수
	/** 로그를 전송한다 */
	public static void SendLog(string a_oName, Dictionary<string, object> a_oDataDict) {
#if NEWTON_SOFT_JSON_MODULE_ENABLE && ANALYTICS_TEST_ENABLE
		var oDataDict = LogFunc.MakeLogDatas(a_oDataDict);

#if FLURRY_MODULE_ENABLE
		// 플러리 분석이 가능 할 경우
		if(KDefine.G_ANALYTICS_LOG_ENABLE_LIST.Contains(EAnalytics.FLURRY)) {
			CFlurryManager.Inst.SendLog(a_oName, oDataDict.ExToTypes<string, object, string, string>());
		}
#endif			// #if FLURRY_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
		// 파이어 베이스 분석이 가능 할 경우
		if(KDefine.G_ANALYTICS_LOG_ENABLE_LIST.Contains(EAnalytics.FIREBASE)) {
			CFirebaseManager.Inst.SendLog(a_oName, oDataDict.ExToTypes<string, object, string, string>());
		}
#endif			// #if FIREBASE_MODULE_ENABLE

#if APPS_FLYER_MODULE_ENABLE
		// 앱스 플라이어 분석이 가능 할 경우
		if(KDefine.G_ANALYTICS_LOG_ENABLE_LIST.Contains(EAnalytics.APPS_FLYER)) {
			CAppsFlyerManager.Inst.SendLog(a_oName, oDataDict.ExToTypes<string, object, string, string>());
		}
#endif			// #if APPS_FLYER_MODULE_ENABLE
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE && ANALYTICS_TEST_ENABLE
	}

	/** 로그 데이터를 생성한다 */
	private static Dictionary<string, object> MakeLogDatas(Dictionary<string, object> a_oDataDict) {
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

		return oDataDict;
	}
	#endregion			// 클래스 함수

	#region 조건부 클래스 함수
#if PURCHASE_MODULE_ENABLE
	/** 결제 로그를 전송한다 */
	public static void SendPurchaseLog(Product a_oProduct, int a_nNumProducts = KCDefine.B_VAL_1_INT) {
#if NEWTON_SOFT_JSON_MODULE_ENABLE && ANALYTICS_TEST_ENABLE
		var oDataDict = LogFunc.MakeLogDatas(null);

#if FLURRY_MODULE_ENABLE
		// 플러리 분석이 가능 할 경우
		if(KDefine.G_ANALYTICS_PURCHASE_LOG_ENABLE_LIST.Contains(EAnalytics.FLURRY)) {
			CFlurryManager.Inst.SendPurchaseLog(a_oProduct, a_nNumProducts, oDataDict.ExToTypes<string, object, string, string>());
		}
#endif			// #if FLURRY_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
		// 파이어 베이스 분석이 가능 할 경우
		if(KDefine.G_ANALYTICS_PURCHASE_LOG_ENABLE_LIST.Contains(EAnalytics.FIREBASE)) {
			CFirebaseManager.Inst.SendPurchaseLog(a_oProduct, a_nNumProducts, oDataDict.ExToTypes<string, object, string, string>());
		}
#endif			// #if FIREBASE_MODULE_ENABLE

#if APPS_FLYER_MODULE_ENABLE
		// 앱스 플라이어 분석이 가능 할 경우
		if(KDefine.G_ANALYTICS_PURCHASE_LOG_ENABLE_LIST.Contains(EAnalytics.APPS_FLYER)) {
			CAppsFlyerManager.Inst.SendPurchaseLog(a_oProduct, a_nNumProducts, oDataDict.ExToTypes<string, object, string, string>());
		}
#endif			// #if APPS_FLYER_MODULE_ENABLE
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE && ANALYTICS_TEST_ENABLE
	}
#endif			// #if PURCHASE_MODULE_ENABLE
	#endregion			// 조건부 클래스 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
