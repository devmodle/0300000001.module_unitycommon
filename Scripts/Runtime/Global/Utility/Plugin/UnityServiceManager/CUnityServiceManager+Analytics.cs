using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_SERVICE_ENABLE && UNITY_SERVICE_ANALYTICS_ENABLE
using UnityEngine.Analytics;

//! 유니티 서비스 관리자 - 분석
public partial class CUnityServiceManager : CSingleton<CUnityServiceManager> {
	#region 함수
	//! 분석 유저 식별자를 변경한다
	public void SetAnalyticsUserID(string a_oID) {
		Func.Assert(a_oID.ExIsValid());
		Func.ShowLog("CUnityServiceManager.SetAnalyticsUserID: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_oID);

		if(this.IsInit) {
			Analytics.SetUserId(a_oID);
		}
	}

	//! 로그를 전송한다
	public void SendLog(string a_oName) {
		this.SendLog(a_oName, null);
	}

	//! 로그를 전송한다
	public void SendLog(string a_oName, string a_oParameter, List<string> a_oDataList) {
		Func.Assert(a_oParameter.ExIsValid());

		this.SendLog(a_oName, new Dictionary<string, object>() {
			[a_oParameter] = a_oDataList.ExToString(KDefine.U_TOKEN_UNITY_SERVICE_ANALYTICS_LOG_DATA)
		});
	}

	//! 로그를 전송한다
	public void SendLog(string a_oName, Dictionary<string, object> a_oDataList) {
		Func.Assert(a_oName.ExIsValid());
		Func.ShowLog("CUnityServiceManager.SendLog: {0}, {1}", KDefine.B_LOG_COLOR_PLUGIN, a_oName, a_oDataList);

#if ANALYTICS_TEST_ENABLE || (ADHOC_BUILD || STORE_BUILD)
		if(this.IsInit) {
			var oDataList = a_oDataList ?? new Dictionary<string, object>();

#if MESSAGE_PACK_ENABLE
			oDataList.ExAddValue(KDefine.U_LOG_KEY_DEVICE_ID, CAppInfoStorage.Instance.AppInfo.DeviceID);

#if AUTO_LOG_PARAMETER_ENABLE
			oDataList.ExAddValue(KDefine.U_LOG_KEY_PLATFORM, CAppInfoStorage.Instance.PlatformName);
			oDataList.ExAddValue(KDefine.U_LOG_KEY_USER_TYPE, CUserInfoStorage.Instance.UserInfo.UserType.ToString());
			
			oDataList.ExAddValue(KDefine.U_LOG_KEY_LOG_TIME, System.DateTime.UtcNow.ExToLongString());
			oDataList.ExAddValue(KDefine.U_LOG_KEY_INSTALL_TIME, CAppInfoStorage.Instance.AppInfo.m_stUTCInstallTime.ExToLongString());
#endif			// #if AUTO_LOG_PARAMETER_ENABLE
#endif			// #if MESSAGE_PACK_ENABLE

			Analytics.CustomEvent(a_oName, oDataList);
			Analytics.FlushEvents();
		}
#endif			// #if ANALYTICS_TEST_ENABLE || (ADHOC_BUILD || STORE_BUILD)
	}
	#endregion			// 함수
}
#endif			// #if UNITY_SERVICE_ENABLE && UNITY_SERVICE_ANALYTICS_ENABLE
