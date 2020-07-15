using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_SERVICE_ENABLE && UNITY_SERVICE_CRASHLYTICS_ENABLE
using UnityEngine.CrashReportHandler;

//! 유니티 서비스 관리자 - 크래시 리포트
public partial class CUnityServiceManager : CSingleton<CUnityServiceManager> {
	#region 함수
	//! 크래시 데이터를 변경한다
	public void SetCrashDatas(Dictionary<string, string> a_oDataList) {
		Func.Assert(a_oDataList.ExIsValid());
		Func.ShowLog("CUnityServiceManager.SetCrashDatas: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_oDataList);

		if(this.IsInit) {
			foreach(var stKeyValue in a_oDataList) {
				CrashReportHandler.SetUserMetadata(stKeyValue.Key, stKeyValue.Value);
			}
		}
	}
	#endregion			// 함수
}
#endif			// #if UNITY_SERVICE_ENABLE && UNITY_SERVICE_CRASHLYTICS_ENABLE
