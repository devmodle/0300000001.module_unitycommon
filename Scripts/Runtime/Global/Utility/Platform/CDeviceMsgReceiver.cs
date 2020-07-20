using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

//! 디바이스 메세지 수신자
public class CDeviceMsgReceiver : CSingleton<CDeviceMsgReceiver> {
	#region 변수
	private Dictionary<string, KeyValuePair<bool, System.Action<string, string>>> m_oCallbackInfoList = new Dictionary<string, KeyValuePair<bool, System.Action<string, string>>>();
	#endregion			// 변수

	#region 함수
	//! 콜백 정보를 추가한다
	public void AddCallbackInfo(string a_oKey, KeyValuePair<bool, System.Action<string, string>> a_stCallbackInfo) {
		CBAccess.Assert(a_oKey.ExIsValid());
		m_oCallbackInfoList.ExAddValue(a_oKey, a_stCallbackInfo);
	}

	//! 콜백 정보를 제거한다
	public void RemoveCallbackInfo(string a_oKey) {
		CBAccess.Assert(a_oKey.ExIsValid());
		m_oCallbackInfoList.ExRemoveValue(a_oKey);
	}

	//! 디바이스 메세지를 처리한다
	private void HandleDeviceMsg(string a_oMsg) {
		CBAccess.Assert(a_oMsg.ExIsValid());
		Func.ShowLog("CDeviceMsgReceiver.HandleDeviceMsg: {0}", KBDefine.LOG_COLOR_PLUGIN, a_oMsg);

		var oJSONNode = JSON.Parse(a_oMsg);
		string oCmd = oJSONNode[KUDefine.KEY_DEVICE_CMD];

		if(m_oCallbackInfoList.ContainsKey(oCmd)) {
			string oMsg = oJSONNode[KUDefine.KEY_DEVICE_MSG];
			m_oCallbackInfoList[oCmd].Value?.Invoke(oCmd, oMsg);

			if(m_oCallbackInfoList[oCmd].Key) {
				this.RemoveCallbackInfo(oCmd);
			}
		}
	}
	#endregion			// 함수
}
