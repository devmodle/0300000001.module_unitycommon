using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 전역 로그 함수
public static partial class LogFunc {
	#region 클래스 함수
	//! 앱 구동 로그를 전송한다
	public static void SendLaunchLog() {
		var oDatas = LogFunc.MakeDefDatas();
		LogFunc.SendLog(KDefine.L_LOG_N_LAUNCH, oDatas);
	}

	//! 약관 동의 로그를 전송한다
	public static void SendAgreeLog() {
		var oDatas = LogFunc.MakeDefDatas();
		LogFunc.SendLog(KDefine.L_LOG_N_AGREE, oDatas);
	}
	
	//! 스플래시 로그를 전송한다
	public static void SendSplashLog() {
		var oDatas = LogFunc.MakeDefDatas();
		LogFunc.SendLog(KDefine.L_LOG_N_SPLASH, oDatas);
	}

	//! 기본 데이터를 생성한다
	private static Dictionary<string, object> MakeDefDatas() {
		return new Dictionary<string, object>() {
			[KDefine.L_LOG_KEY_LOG_TIME] = System.DateTime.Now.ExToPSTTime().ExToLongStr(),
			[KDefine.L_LOG_KEY_INSTALL_TIME] = CCommonAppInfoStorage.Inst.AppInfo.PSTInstallTime.ExToLongStr()
		};
	}
	#endregion			// 클래스 함수
}
#endif			// #if NEVER_USE_THIS
