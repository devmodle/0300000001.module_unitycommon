using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_SERVICE_ENABLE
#if UNITY_SERVICE_ANALYTICS_ENABLE
using UnityEngine.Analytics;
#endif			// #if UNITY_SERVICE_ANALYTICS_ENABLE

//! 유니티 서비스 관리자
public partial class CUnityServiceManager : CSingleton<CUnityServiceManager> {
	#region 프로퍼티
	public bool IsInit { get; private set; } = false;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public virtual void Init(System.Action<CUnityServiceManager, bool> a_oCallback) {
		Function.ShowLog("CUnityServiceManager.Init", Color.yellow);

		if(!this.IsInit && Function.IsMobilePlatform()) {
			this.IsInit = true;

#if UNITY_SERVICE_ANALYTICS_ENABLE
#if ANALYTICS_TEST_ENABLE || (ADHOC_BUILD || STORE_BUILD)
			Analytics.enabled = true;
			Analytics.deviceStatsEnabled = true;
			Analytics.initializeOnStartup = true;
#else
			Analytics.enabled = false;
			Analytics.deviceStatsEnabled = false;
			Analytics.initializeOnStartup = false;
#endif			// #if ANALYTICS_TEST_ENABLE || (ADHOC_BUILD || STORE_BUILD)

			Analytics.ResumeInitialization();
#endif			// #if UNITY_SERVICE_ANALYTICS_ENABLE
		}

		a_oCallback?.Invoke(this, this.IsInit);
	}
	#endregion			// 함수
}
#endif			// #if UNITY_SERVICE_ENABLE
