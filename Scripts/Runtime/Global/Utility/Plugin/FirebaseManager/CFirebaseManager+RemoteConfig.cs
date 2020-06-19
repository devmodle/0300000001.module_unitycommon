using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if FIREBASE_ENABLE && FIREBASE_REMOTE_CONFIG_ENABLE
using Firebase.RemoteConfig;

//! 파이어 베이스 관리자 - 원격 속성
public partial class CFirebaseManager : CSingleton<CFirebaseManager> {
	#region 함수
	//! 속성 데이터를 반환한다
	public string GetConfigData(string a_oKey) {
		Function.Assert(a_oKey.ExIsValid());
		Function.ShowLog("CFirebaseManager.GetConfigData: {0}", Color.yellow, a_oKey);

		return this.IsInit ? FirebaseRemoteConfig.GetValue(a_oKey).StringValue : string.Empty;
	}

	//! 설정 데이터를 로드한다
	public void LoadConfigData(System.Action<CFirebaseManager, bool> a_oCallback) {
		Function.ShowLog("CFirebaseManager.LoadConfigData", Color.yellow);

		if(!this.IsInit) {
			a_oCallback?.Invoke(this, false);
		} else {
			Function.WaitAsyncTask(FirebaseRemoteConfig.FetchAsync(KDefine.U_DEF_TIMEOUT_FIREBASE_FETCH_CONFIG_DATA), (a_oTask) => {
				Function.ShowLog("CFirebaseManager.OnLoadConfigData: {0}", Color.yellow, a_oTask.Exception?.Message);

				if(!Function.IsCompleteAsyncTask(a_oTask)) {
					a_oCallback?.Invoke(this, false);
				} else {
					a_oCallback?.Invoke(this, FirebaseRemoteConfig.ActivateFetched());
				}
			});
		}
	}
	#endregion			// 함수
}
#endif			// #if FIREBASE_ENABLE && FIREBASE_REMOTE_CONFIG_ENABLE
