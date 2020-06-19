using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

//! 유니티 메세지 전송자
public class CUnityMessageSender : CSingleton<CUnityMessageSender> {
	#region 변수
#if !UNITY_EDITOR && UNITY_ANDROID
	private AndroidJavaClass m_oAndroidPlugin = new AndroidJavaClass(KDefine.U_CLS_NAME_UNITY_MS_MESSAGE_RECEIVER);
#endif			// #if !UNITY_EDITOR && UNITY_ANDROID
	#endregion			// 변수

	#region 함수
	//! 디바이스 식별자 반환 메세지를 전송한다
	public void SendGetDeviceIDMessage(System.Action<string, string> a_oCallback) {
		this.SendMessage(KDefine.B_COMMAND_GET_DEVICE_ID, string.Empty, a_oCallback, true);
	}

	//! 국가 코드 반환 메세지를 전송한다
	public void SendGetCountryCodeMessage(System.Action<string, string> a_oCallback) {
		this.SendMessage(KDefine.B_COMMAND_GET_COUNTRY_CODE, string.Empty, a_oCallback, true);
	}

	//! 시스템 버전 반환 메세지를 전송한다
	public void SendGetSystemVersionMessage(System.Action<string, string> a_oCallback) {
		this.SendMessage(KDefine.B_COMMAND_GET_SYSTEM_VERSION, string.Empty, a_oCallback, true);
	}

	//! 스토어 버전 반환 메세지를 전송한다
	public void SendGetStoreVersionMessage(string a_oAppID,
		  string a_oVersion, float a_fTimeout, System.Action<string, string> a_oCallback) {
		Function.Assert(a_oAppID.ExIsValid() && a_oVersion.ExIsValid() && a_fTimeout.ExIsGreate(0.0f));

		var oDataList = new Dictionary<string, string>() {
			[KDefine.U_KEY_UNITY_MS_APP_ID] = a_oAppID,
			[KDefine.U_KEY_UNITY_MS_VERSION] = a_oVersion,
			[KDefine.U_KEY_UNITY_MS_TIMEOUT] = a_fTimeout.ToString()
		};

		this.SendMessage(KDefine.B_COMMAND_GET_STORE_VERSION,
			oDataList.ExToJSONString(), a_oCallback, true);
	}

	//! 빌드 모드 변경 메세지를 전송한다
	public void SendSetBuildModeMessage(bool a_bIsDebug) {
		string oBuildMode = a_bIsDebug ? KDefine.B_BUILD_MODE_DEBUG : KDefine.B_BUILD_MODE_RELEASE;
		this.SendMessage(KDefine.B_COMMAND_SET_BUILD_MODE, oBuildMode, null, true);
	}

	//! 토스트 출력 메세지를 전송한다
	public void SendShowToastMessage(string a_oMessage) {
		this.SendMessage(KDefine.B_COMMAND_SHOW_TOAST, a_oMessage, null, true);
	}

	//! 알림 창 출력 메세지를 전송한다
	public void SendShowAlertMessage(string a_oTitle,
		string a_oMessage, string a_oOKButtonTitle, string a_oCancelButtonTitle, System.Action<string, string> a_oCallback) {
		Function.Assert(a_oTitle != null && a_oCancelButtonTitle != null);
		Function.Assert(a_oMessage.ExIsValid() && a_oOKButtonTitle.ExIsValid());

		var oDataList = new Dictionary<string, string>() {
			[KDefine.U_KEY_UNITY_MS_ALERT_TITLE] = a_oTitle,
			[KDefine.U_KEY_UNITY_MS_ALERT_MESSAGE] = a_oMessage,
			[KDefine.U_KEY_UNITY_MS_ALERT_OK_BUTTON_TEXT] = a_oOKButtonTitle,
			[KDefine.U_KEY_UNITY_MS_ALERT_CANCEL_BUTTON_TEXT] = a_oCancelButtonTitle
		};

		this.SendMessage(KDefine.B_COMMAND_SHOW_ALERT, oDataList.ExToJSONString(), a_oCallback, true);
	}

	//! 진동 메세지를 전송한다
	public void SendVibrateMessage(EVibrateType a_eType, EVibrateStyle a_eStyle, float a_fDuration, float a_fIntensity) {
		bool bIsEnableType = a_eType > EVibrateType.NONE && a_eType < EVibrateType.MAX_VALUE;
		bool bIsEnableStyle = a_eStyle > EVibrateStyle.NONE && a_eStyle < EVibrateStyle.MAX_VALUE;

		Function.Assert(bIsEnableType && bIsEnableStyle && a_fDuration.ExIsGreate(0.0f));

		var oDataList = new Dictionary<string, string>() {
			[KDefine.U_KEY_UNITY_MS_VIBRATE_TYPE] = ((int)a_eType).ToString(),
			[KDefine.U_KEY_UNITY_MS_VIBRATE_STYLE] = ((int)a_eStyle).ToString(),
			[KDefine.U_KEY_UNITY_MS_VIBRATE_DURATION] = a_fDuration.ToString(),
			[KDefine.U_KEY_UNITY_MS_VIBRATE_INTENSITY] = a_fIntensity.ToString()
		};

		this.SendMessage(KDefine.B_COMMAND_VIBRATE, oDataList.ExToJSONString(), null, true);
	}

	//! 액티비티 인디게이터 메세지를 전송한다
	public void SendActivityIndicatorMessage(bool a_bIsStart) {
		this.SendMessage(KDefine.B_COMMAND_ACTIVITY_INDICATOR, a_bIsStart.ToString(), null, true);
	}

	//! 공유 메세지를 전송한다
	public void SendShareMessage(string a_oMessage, string a_oFilepath = KDefine.B_EMPTY_STRING) {
		Function.Assert(a_oMessage.ExIsValid());

		var oNativeShare = new NativeShare();
		oNativeShare.SetText(a_oMessage);

		if(a_oFilepath.ExIsValid()) {
			oNativeShare.AddFile(a_oFilepath);
		}

		oNativeShare.Share();
	}

	//! 메세지를 전송한다
	private void SendMessage(string a_oCommand, string a_oMessage, System.Action<string, string> a_oCallback, bool a_bIsAutoRemove) {
		Function.Assert(a_oMessage != null && a_oCommand.ExIsValid());

		if(!Function.IsMobilePlatform()) {
			a_oCallback?.Invoke(a_oCommand, string.Empty);
		} else {
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
			if(a_oCallback != null) {
				var stCallbackInfo = new KeyValuePair<bool, System.Action<string, string>>(a_bIsAutoRemove, a_oCallback);
				CDeviceMessageReceiver.Instance.AddCallbackInfo(a_oCommand, stCallbackInfo);
			}

#if UNITY_IOS
			CUnityMessageSender.HandleUnityMessage(a_oCommand, a_oMessage);
#else
			m_oAndroidPlugin.CallStatic(KDefine.U_FUNC_NAME_UNITY_MS_MESSAGE_HANDLER, a_oCommand, a_oMessage);
#endif			// #if UNITY_IOS
#endif			// #if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
		}
	}
	#endregion			// 함수

	#region 조건부 클래스 함수
#if !UNITY_EDITOR && UNITY_IOS
	[DllImport("__Internal")]
	private static extern void HandleUnityMessage(string a_oCommand, string a_oMessage);
#endif			// #if !UNITY_EDITOR && UNITY_IOS
	#endregion			// 조건부 클래스 함수
}
