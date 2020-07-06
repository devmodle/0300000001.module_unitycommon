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
		Func.Assert(a_oID.ExIsValid());
		Func.ShowLog("CFirebaseManager.SetCrashUserID: {0}", Color.yellow, a_oID);

		if(this.IsInit) {
			Crashlytics.SetUserId(a_oID);
		}
	}

	//! 크래시 데이터를 변경한다
	public void SetCrashDatas(Dictionary<string, string> a_oDataList) {
		Func.Assert(a_oDataList.ExIsValid());
		Func.ShowLog("CFirebaseManager.SetCrashDatas: {0}", Color.yellow, a_oDataList);

		if(this.IsInit) {
			foreach(var stKeyValue in a_oDataList) {
				Crashlytics.SetCustomKey(stKeyValue.Key, stKeyValue.Value);
			}
		}
	}

	//! 크래시 로그를 전송한다
	public void SendCrashLog(string a_oMsg) {
		Func.Assert(a_oMsg.ExIsValid());
		this.SendCrashLog(new System.Exception(a_oMsg));
	}

	//! 크래시 로그를 전송한다
	public void SendCrashLog(System.Exception a_oException) {
		Func.Assert(a_oException != null);
		Func.ShowLog("CFirebaseManager.SendCrashLog: {0}", Color.yellow, a_oException);

		if(this.IsInit) {
			Crashlytics.LogException(a_oException);
		}
	}
	#endregion			// 함수
}
#endif			// #if FIREBASE_ENABLE && FIREBASE_CRASHLYTICS_ENABLE
