using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

//! 디바이스 메세지 수신자
public class CDeviceMessageReceiver : CSingleton<CDeviceMessageReceiver> {
	#region 변수
	private Dictionary<string, KeyValuePair<bool, System.Action<string, string>>> m_oCallbackInfoList = new Dictionary<string, KeyValuePair<bool, System.Action<string, string>>>();
	#endregion			// 변수

	#region 함수
	//! 콜백 정보를 추가한다
	public void AddCallbackInfo(string a_oKey, KeyValuePair<bool, System.Action<string, string>> a_stCallbackInfo) {
		Func.Assert(a_oKey.ExIsValid());
		m_oCallbackInfoList.ExAddValue(a_oKey, a_stCallbackInfo);
	}

	//! 콜백 정보를 제거한다
	public void RemoveCallbackInfo(string a_oKey) {
		Func.Assert(a_oKey.ExIsValid());
		m_oCallbackInfoList.ExRemoveValue(a_oKey);
	}

	//! 디바이스 메세지를 처리한다
	private void HandleDeviceMessage(string a_oMessage) {
		Func.Assert(a_oMessage.ExIsValid());
		Func.ShowLog("CDeviceMessageReceiver.HandleDeviceMessage: {0}", Color.yellow, a_oMessage);

		var oJSONNode = JSON.Parse(a_oMessage);
		string oCommand = oJSONNode[KDefine.U_KEY_DEVICE_COMMAND];

		if(m_oCallbackInfoList.ContainsKey(oCommand)) {
			string oMessage = oJSONNode[KDefine.U_KEY_DEVICE_MESSAGE];
			m_oCallbackInfoList[oCommand].Value?.Invoke(oCommand, oMessage);

			if(m_oCallbackInfoList[oCommand].Key) {
				this.RemoveCallbackInfo(oCommand);
			}
		}
	}
	#endregion			// 함수
}
