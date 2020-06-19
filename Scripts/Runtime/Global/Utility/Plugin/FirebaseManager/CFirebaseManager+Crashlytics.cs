using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if FIREBASE_ENABLE && FIREBASE_CRASHLYTICS_ENABLE
using Firebase.Crashlytics;

//! 파이어 베이스 관리자 - 크래시 리포트
public partial class CFirebaseManager : CSingleton<CFirebaseManager> {
	#region 함수
	//! 크래시 유저 식별자를 변경한다
	public void SetCrashUserID(string a_oID) {
		Function.Assert(a_oID.ExIsValid());
		Function.ShowLog("CFirebaseManager.SetCrashUserID: {0}", Color.yellow, a_oID);

		if(this.IsInit) {
			Crashlytics.SetUserId(a_oID);
		}
	}

	//! 크래시 데이터를 변경한다
	public void SetCrashDatas(Dictionary<string, string> a_oDataList) {
		Function.Assert(a_oDataList.ExIsValid());
		Function.ShowLog("CFirebaseManager.SetCrashDatas: {0}", Color.yellow, a_oDataList);

		if(this.IsInit) {
			foreach(var stKeyValue in a_oDataList) {
				Crashlytics.SetCustomKey(stKeyValue.Key, stKeyValue.Value);
			}
		}
	}

	//! 크래시 로그를 전송한다
	public void SendCrashLog(string a_oMessage) {
		Function.Assert(a_oMessage.ExIsValid());
		this.SendCrashLog(new System.Exception(a_oMessage));
	}

	//! 크래시 로그를 전송한다
	public void SendCrashLog(System.Exception a_oException) {
		Function.Assert(a_oException != null);
		Function.ShowLog("CFirebaseManager.SendCrashLog: {0}", Color.yellow, a_oException);

		if(this.IsInit) {
			Crashlytics.LogException(a_oException);
		}
	}
	#endregion			// 함수
}
#endif			// #if FIREBASE_ENABLE && FIREBASE_CRASHLYTICS_ENABLE
