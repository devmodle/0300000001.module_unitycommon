#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif // #if PURCHASE_MODULE_ENABLE

/** 기본 로그 함수 */
public static partial class LogFunc {
	#region 클래스 변수
	private static Dictionary<string, string> m_oLogTimeDict = new Dictionary<string, string>();
	#endregion // 클래스 변수

	#region 클래스 함수
	/** 로그를 전송한다 */
	public static void SendLog(string a_oName, Dictionary<string, object> a_oDataDict) {
		// 로그 전송이 불가능 할 경우
		if(!LogFunc.IsEnableSendLog(a_oName)) {
			return;
		}

#if ANALYTICS_TEST_ENABLE || STORE_DIST_BUILD
		var oDataDict = LogFunc.MakeLogDatas(a_oDataDict);
		oDataDict.TryAdd(KCDefine.G_LOG_KEY_LOG_NAME, a_oName);

		CCommonAppInfoStorage.Inst.AppInfo.m_oSendLogList.ExAddVal(a_oName);

#if FLURRY_MODULE_ENABLE
		// 플러리 로그 전송이 가능 할 경우
		if(KDefine.G_ANALYTICS_LOG_ENABLE_LIST.Contains(EAnalytics.FLURRY)) {
			CFlurryManager.Inst.SendLog(a_oName, oDataDict.ExToTypes<string, object, string, string>());
		}
#endif // #if FLURRY_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
		// 파이어 베이스 로그 전송이 가능 할 경우
		if(KDefine.G_ANALYTICS_LOG_ENABLE_LIST.Contains(EAnalytics.FIREBASE)) {
			CFirebaseManager.Inst.SendLog(a_oName, oDataDict.ExToTypes<string, object, string, string>());
		}
#endif // #if FIREBASE_MODULE_ENABLE

#if APPS_FLYER_MODULE_ENABLE
		// 앱스 플라이어 로그 전송이 가능 할 경우
		if(KDefine.G_ANALYTICS_LOG_ENABLE_LIST.Contains(EAnalytics.APPS_FLYER)) {
			CAppsFlyerManager.Inst.SendLog(a_oName, oDataDict.ExToTypes<string, object, string, string>());
		}
#endif // #if APPS_FLYER_MODULE_ENABLE

#if PLAYFAB_MODULE_ENABLE
		// 플레이 팹 로그 전송이 가능 할 경우
		if(KDefine.G_ANALYTICS_LOG_ENABLE_LIST.Contains(EAnalytics.PLAYFAB)) {
			CPlayfabManager.Inst.SendLog(a_oName, oDataDict);
		}
#endif // #if PLAYFAB_MODULE_ENABLE
#endif // #if ANALYTICS_TEST_ENABLE || STORE_DIST_BUILD

		LogFunc.m_oLogTimeDict.ExReplaceVal(a_oName, System.DateTime.Now.ExToPSTTime().ExToLongStr());
	}

	/** 로그 데이터를 생성한다 */
	private static Dictionary<string, object> MakeLogDatas(Dictionary<string, object> a_oDataDict) {
		var oDataDict = a_oDataDict ?? new Dictionary<string, object>();
		oDataDict.TryAdd(KCDefine.G_LOG_KEY_PLATFORM, CCommonAppInfoStorage.Inst.Platform);
		oDataDict.TryAdd(KCDefine.G_LOG_KEY_DEVICE_ID, CCommonAppInfoStorage.Inst.AppInfo.DeviceID);
		oDataDict.TryAdd(KCDefine.G_LOG_KEY_LOG_TIME, System.DateTime.UtcNow.ExToLongStr());
		oDataDict.TryAdd(KCDefine.G_LOG_KEY_SHORT_LOG_TIME, System.DateTime.UtcNow.ExToShortStr());
		oDataDict.TryAdd(KCDefine.G_LOG_KEY_INSTALL_TIME, CCommonAppInfoStorage.Inst.AppInfo.UTCInstallTime.ExToLongStr());

#if ANALYTICS_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
		oDataDict.TryAdd(KCDefine.G_LOG_KEY_USER_TYPE, KCDefine.B_TEXT_UNKNOWN);
#else
		oDataDict.TryAdd(KCDefine.G_LOG_KEY_USER_TYPE, CCommonUserInfoStorage.Inst.UserInfo.UserType.ToString());
#endif // #if ANALYTICS_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)

		return oDataDict;
	}

	/** 일회성 로그를 전송한다 */
	public static void SendOnceLog(string a_oName, Dictionary<string, object> a_oDataDict) {
		// 로그 전송이 불가능 할 경우
		if(CCommonAppInfoStorage.Inst.AppInfo.m_oSendLogList.Contains(a_oName)) {
			return;
		}

		LogFunc.SendLog(a_oName, a_oDataDict);
	}

#if PURCHASE_MODULE_ENABLE
	/** 결제 로그를 전송한다 */
	public static void SendPurchaseLog(Product a_oProduct, int a_nNumProducts = KCDefine.B_VAL_1_INT) {
		// 로그 전송이 불가능 할 경우
		if(!LogFunc.IsEnableSendLog(KDefine.G_LOG_N_PURCHASE)) {
			return;
		}

#if ANALYTICS_TEST_ENABLE || STORE_DIST_BUILD
		var oDataDict = LogFunc.MakeLogDatas(null);

#if FLURRY_MODULE_ENABLE
		// 플러리 로그 전송이 가능 할 경우
		if(KDefine.G_ANALYTICS_LOG_ENABLE_LIST.Contains(EAnalytics.FLURRY)) {
			var oFlurryDataDict = oDataDict.ExToTypes<string, object, string, string>();
			CFlurryManager.Inst.SendPurchaseLog(a_oProduct, a_nNumProducts, oFlurryDataDict);
		}
#endif // #if FLURRY_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
		// 파이어 베이스 로그 전송이 가능 할 경우
		if(KDefine.G_ANALYTICS_LOG_ENABLE_LIST.Contains(EAnalytics.FIREBASE)) {
			var oFirebaseDataDict = oDataDict.ExToTypes<string, object, string, string>();
			CFirebaseManager.Inst.SendPurchaseLog(a_oProduct, a_nNumProducts, oFirebaseDataDict);
		}
#endif // #if FIREBASE_MODULE_ENABLE

#if APPS_FLYER_MODULE_ENABLE
		// 앱스 플라이어 로그 전송이 가능 할 경우
		if(KDefine.G_ANALYTICS_LOG_ENABLE_LIST.Contains(EAnalytics.APPS_FLYER)) {
			var oAppsFlyerDataDict = oDataDict.ExToTypes<string, object, string, string>();
			CAppsFlyerManager.Inst.SendPurchaseLog(a_oProduct, a_nNumProducts, oAppsFlyerDataDict);
		}
#endif // #if APPS_FLYER_MODULE_ENABLE

#if PLAYFAB_MODULE_ENABLE
		// 플레이 팹 로그 전송이 가능 할 경우
		if(KDefine.G_ANALYTICS_LOG_ENABLE_LIST.Contains(EAnalytics.PLAYFAB)) {
			// Do Something
		}
#endif // #if PLAYFAB_MODULE_ENABLE
#endif // #if ANALYTICS_TEST_ENABLE || STORE_DIST_BUILD

		LogFunc.m_oLogTimeDict.ExReplaceVal(KDefine.G_LOG_N_PURCHASE, System.DateTime.Now.ExToPSTTime().ExToLongStr());
	}
#endif // #if PURCHASE_MODULE_ENABLE
#endregion // 클래스 함수

	#region 클래스 접근 함수
	/** 로그 전송 가능 여부를 검사한다 */
	private static bool IsEnableSendLog(string a_oName) {
		string oLogTime = System.DateTime.Now.ExToPSTTime().ExToLongStr();
		return LogFunc.m_oLogTimeDict.ContainsKey(a_oName) ? !LogFunc.m_oLogTimeDict[a_oName].Equals(oLogTime) : true;
	}
	#endregion // 클래스 접근 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
