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
	public static void SendLog(string a_oName, Dictionary<string, object> a_oDictData) {
		// 로그 전송이 불가능 할 경우
		if(!LogFunc.IsEnableLogSend(a_oName)) {
			return;
		}

#if ANALYTICS_TEST_ENABLE || STORE_DIST_BUILD
		var oDictData = LogFunc.MakeLogDatas(a_oDictData);
		oDictData.TryAdd(KCDefine.G_LOG_KEY_LOG_NAME, a_oName);

		CStorageInfoAppCommon.Inst.AppInfo.m_oSendLogList.ExAddVal(a_oName);

#if FLURRY_MODULE_ENABLE
		// 플러리 로그 전송이 가능 할 경우
		if(KDefine.G_LIST_ENABLE_LOG_ANALYTICS.Contains(EAnalytics.FLURRY)) {
			CFlurryManager.Inst.SendLog(a_oName, oDictData.ExToTypes<string, object, string, string>());
		}
#endif // #if FLURRY_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
		// 파이어 베이스 로그 전송이 가능 할 경우
		if(KDefine.G_LIST_ENABLE_LOG_ANALYTICS.Contains(EAnalytics.FIREBASE)) {
			CFirebaseManager.Inst.SendLog(a_oName, oDictData.ExToTypes<string, object, string, string>());
		}
#endif // #if FIREBASE_MODULE_ENABLE

#if APPS_FLYER_MODULE_ENABLE
		// 앱스 플라이어 로그 전송이 가능 할 경우
		if(KDefine.G_LIST_ENABLE_LOG_ANALYTICS.Contains(EAnalytics.APPS_FLYER)) {
			CAppsFlyerManager.Inst.SendLog(a_oName, oDictData.ExToTypes<string, object, string, string>());
		}
#endif // #if APPS_FLYER_MODULE_ENABLE

#if PLAYFAB_MODULE_ENABLE
		// 플레이 팹 로그 전송이 가능 할 경우
		if(KDefine.G_LIST_ENABLE_LOG_ANALYTICS.Contains(EAnalytics.PLAYFAB)) {
			CPlayfabManager.Inst.SendLog(a_oName, oDictData);
		}
#endif // #if PLAYFAB_MODULE_ENABLE
#endif // #if ANALYTICS_TEST_ENABLE || STORE_DIST_BUILD

		LogFunc.m_oLogTimeDict.ExReplaceVal(a_oName, System.DateTime.Now.ExToTimePST().ExToStrLong());
	}

	/** 로그 데이터를 생성한다 */
	private static Dictionary<string, object> MakeLogDatas(Dictionary<string, object> a_oDictData) {
		var oDictData = a_oDictData ?? new Dictionary<string, object>();
		oDictData.TryAdd(KCDefine.G_LOG_KEY_PLATFORM, CStorageInfoAppCommon.Inst.Platform);
		oDictData.TryAdd(KCDefine.G_LOG_KEY_DEVICE_ID, CStorageInfoAppCommon.Inst.AppInfo.DeviceID);
		oDictData.TryAdd(KCDefine.G_LOG_KEY_LOG_TIME, System.DateTime.UtcNow.ExToStrLong());
		oDictData.TryAdd(KCDefine.G_LOG_KEY_SHORT_LOG_TIME, System.DateTime.UtcNow.ExToStrShort());
		oDictData.TryAdd(KCDefine.G_LOG_KEY_INSTALL_TIME, CStorageInfoAppCommon.Inst.AppInfo.TimeInstallUTC.ExToStrLong());

#if ANALYTICS_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
		oDictData.TryAdd(KCDefine.G_LOG_KEY_USER_TYPE, KCDefine.B_TEXT_UNKNOWN);
#else
		oDictData.TryAdd(KCDefine.G_LOG_KEY_USER_TYPE, CStorageInfoUserCommon.Inst.UserInfo.UserType.ToString());
#endif // #if ANALYTICS_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)

		return oDictData;
	}

	/** 일회성 로그를 전송한다 */
	public static void SendOnceLog(string a_oName, Dictionary<string, object> a_oDictData) {
		// 로그 전송이 불가능 할 경우
		if(CStorageInfoAppCommon.Inst.AppInfo.m_oSendLogList.Contains(a_oName)) {
			return;
		}

		LogFunc.SendLog(a_oName, a_oDictData);
	}

#if PURCHASE_MODULE_ENABLE
	/** 결제 로그를 전송한다 */
	public static void SendPurchaseLog(Product a_oProduct, int a_nNumProducts = KCDefine.B_VAL_1_INT) {
		// 로그 전송이 불가능 할 경우
		if(!LogFunc.IsEnableLogSend(KDefine.G_LOG_N_PURCHASE)) {
			return;
		}

#if ANALYTICS_TEST_ENABLE || STORE_DIST_BUILD
		var oDictData = LogFunc.MakeLogDatas(null);

#if FLURRY_MODULE_ENABLE
		// 플러리 로그 전송이 가능 할 경우
		if(KDefine.G_LIST_ENABLE_LOG_ANALYTICS.Contains(EAnalytics.FLURRY)) {
			var oFlurryDataDict = oDictData.ExToTypes<string, object, string, string>();
			CFlurryManager.Inst.SendPurchaseLog(a_oProduct, a_nNumProducts, oFlurryDataDict);
		}
#endif // #if FLURRY_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
		// 파이어 베이스 로그 전송이 가능 할 경우
		if(KDefine.G_LIST_ENABLE_LOG_ANALYTICS.Contains(EAnalytics.FIREBASE)) {
			var oFirebaseDataDict = oDictData.ExToTypes<string, object, string, string>();
			CFirebaseManager.Inst.SendPurchaseLog(a_oProduct, a_nNumProducts, oFirebaseDataDict);
		}
#endif // #if FIREBASE_MODULE_ENABLE

#if APPS_FLYER_MODULE_ENABLE
		// 앱스 플라이어 로그 전송이 가능 할 경우
		if(KDefine.G_LIST_ENABLE_LOG_ANALYTICS.Contains(EAnalytics.APPS_FLYER)) {
			var oAppsFlyerDataDict = oDictData.ExToTypes<string, object, string, string>();
			CAppsFlyerManager.Inst.SendPurchaseLog(a_oProduct, a_nNumProducts, oAppsFlyerDataDict);
		}
#endif // #if APPS_FLYER_MODULE_ENABLE

#if PLAYFAB_MODULE_ENABLE
		// 플레이 팹 로그 전송이 가능 할 경우
		if(KDefine.G_LIST_ENABLE_LOG_ANALYTICS.Contains(EAnalytics.PLAYFAB)) {
			// Do Something
		}
#endif // #if PLAYFAB_MODULE_ENABLE
#endif // #if ANALYTICS_TEST_ENABLE || STORE_DIST_BUILD

		LogFunc.m_oLogTimeDict.ExReplaceVal(KDefine.G_LOG_N_PURCHASE, System.DateTime.Now.ExToTimePST().ExToStrLong());
	}
#endif // #if PURCHASE_MODULE_ENABLE
#endregion // 클래스 함수

	#region 클래스 접근 함수
	/** 로그 전송 가능 여부를 검사한다 */
	private static bool IsEnableLogSend(string a_oName) {
		string oLogTime = System.DateTime.Now.ExToTimePST().ExToStrLong();
		return LogFunc.m_oLogTimeDict.ContainsKey(a_oName) ? !LogFunc.m_oLogTimeDict[a_oName].Equals(oLogTime) : true;
	}
	#endregion // 클래스 접근 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
