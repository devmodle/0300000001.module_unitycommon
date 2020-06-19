using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//! 로그 관리자
public class CLogManager : CSingleton<CLogManager> {
	#region 변수
	private System.Text.StringBuilder m_oStringBuilder = new System.Text.StringBuilder();
	#endregion			// 변수

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		Application.logMessageReceived -= this.OnReceiveLog;
		Application.logMessageReceived += this.OnReceiveLog;

		if(File.Exists(KDefine.B_DATA_PATH_LOG)) {
			var oBytes = Function.ReadBytes(KDefine.B_DATA_PATH_LOG);
			m_oStringBuilder.Append(System.Text.Encoding.Default.GetString(oBytes, 0, oBytes.Length));
		}
	}

	//! 제거 되었을 경우
	public override void OnDestroy() {
		base.OnDestroy();

		if(!CSceneManager.IsAppQuit) {
			Application.logMessageReceived -= this.OnReceiveLog;
		}
	}

	//! 로그를 수신했을 경우
	public void OnReceiveLog(string a_oCondition, string a_oStackTrace, LogType a_eLogType) {
		if(a_eLogType != LogType.Log && a_eLogType != LogType.Warning) {
			m_oStringBuilder.AppendFormat(KDefine.U_FORMAT_LOG_M_LOG, 
				System.DateTime.Now.ToString(), a_eLogType, a_oCondition, a_oStackTrace);

			if(m_oStringBuilder.Length > KDefine.U_MAX_LENGTH_LOG) {
				m_oStringBuilder.Remove(0, m_oStringBuilder.Length - KDefine.U_MAX_LENGTH_LOG);
			}

			using(var oWriteStream = Function.GetWriteStream(KDefine.B_DATA_PATH_LOG)) {
				var oBytes = System.Text.Encoding.Default.GetBytes(m_oStringBuilder.ToString());
				Function.WriteBytes(oWriteStream, oBytes);
			}
		}
	}
	#endregion			// 함수
}
