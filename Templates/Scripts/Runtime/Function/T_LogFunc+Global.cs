using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 전역 로그 함수 */
public static partial class LogFunc {
	#region 클래스 함수
	/** 앱 구동 로그를 전송한다 */
	public static void SendLaunchLog() {
		var oDataDict = LogFunc.MakeDefDatas();
		LogFunc.SendLog(KDefine.L_LOG_N_LAUNCH, oDataDict);
	}

	/** 약관 동의 로그를 전송한다 */
	public static void SendAgreeLog() {
		var oDataDict = LogFunc.MakeDefDatas();
		LogFunc.SendLog(KDefine.L_LOG_N_AGREE, oDataDict);
	}
	
	/** 스플래시 로그를 전송한다 */
	public static void SendSplashLog() {
		var oDataDict = LogFunc.MakeDefDatas();
		LogFunc.SendLog(KDefine.L_LOG_N_SPLASH, oDataDict);
	}

	/** 기본 데이터를 생성한다 */
	private static Dictionary<string, object> MakeDefDatas() {
		return new Dictionary<string, object>() {
			[KDefine.L_LOG_KEY_LOG_TIME] = System.DateTime.Now.ExToPSTTime().ExToLongStr(),

#if NEWTON_SOFT_JSON_MODULE_ENABLE
			[KDefine.L_LOG_KEY_INSTALL_TIME] = CCommonAppInfoStorage.Inst.AppInfo.PSTInstallTime.ExToLongStr()
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
		};
	}
	#endregion			// 클래스 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
