using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

#if UNITY_IOS
using UnityEngine.SignInWithApple;
#endif			// #if UNITY_IOS

//! 유니티 메세지 전송자
public class CUnityMsgSender : CSingleton<CUnityMsgSender> {
	#region 변수
#if UNITY_IOS
	private System.Action<SignInWithApple.CallbackArgs, bool> m_oLoginCallback = null;
	private System.Action<SignInWithApple.CallbackArgs, bool> m_oCredentialCallback = null;
#endif			// #if UNITY_IOS

#if !UNITY_EDITOR && UNITY_ANDROID
	private AndroidJavaClass m_oAndroidPlugin = new AndroidJavaClass(KUDefine.CLS_NAME_UNITY_MS_MSG_RECEIVER);
#endif			// #if !UNITY_EDITOR && UNITY_ANDROID
	#endregion			// 변수

	#region 컴포넌트
#if UNITY_IOS
	private SignInWithApple m_oLoginWithApple = null;
#endif			// #if UNITY_IOS
	#endregion			// 컴포넌트

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

#if UNITY_IOS
		m_oLoginWithApple = CUFactory.CreateCloneObj<SignInWithApple>(KUDefine.OBJ_NAME_LOGIN_WITH_APPLE,
			CResManager.Instance.GetPrefab(KUDefine.OBJ_PATH_LOGIN_WITH_APPLE),
			this.gameObject);
#endif			// #if UNITY_IOS
	}

	//! 디바이스 식별자 반환 메세지를 전송한다
	public void SendGetDeviceIDMsg(System.Action<string, string> a_oCallback) {
		this.SendMsg(KBDefine.CMD_GET_DEVICE_ID, string.Empty, a_oCallback, true);
	}

	//! 국가 코드 반환 메세지를 전송한다
	public void SendGetCountryCodeMsg(System.Action<string, string> a_oCallback) {
		this.SendMsg(KBDefine.CMD_GET_COUNTRY_CODE, string.Empty, a_oCallback, true);
	}

	//! 스토어 버전 반환 메세지를 전송한다
	public void SendGetStoreVersionMsg(string a_oAppID,
		  string a_oVersion, float a_fTimeout, System.Action<string, string> a_oCallback) {
		CBAccess.Assert(a_oAppID.ExIsValid() && a_oVersion.ExIsValid() && a_fTimeout.ExIsGreate(0.0f));

		var oDataList = new Dictionary<string, string>() {
			[KUDefine.KEY_UNITY_MS_APP_ID] = a_oAppID,
			[KUDefine.KEY_UNITY_MS_VERSION] = a_oVersion,
			[KUDefine.KEY_UNITY_MS_TIMEOUT] = a_fTimeout.ToString()
		};

		this.SendMsg(KBDefine.CMD_GET_STORE_VERSION,
			oDataList.ExToJSONString(), a_oCallback, true);
	}

	//! 빌드 모드 변경 메세지를 전송한다
	public void SendSetBuildModeMsg(bool a_bIsDebug) {
		string oBuildMode = a_bIsDebug ? KBDefine.BUILD_MODE_DEBUG : KBDefine.BUILD_MODE_RELEASE;
		this.SendMsg(KBDefine.CMD_SET_BUILD_MODE, oBuildMode, null, true);
	}

	//! 토스트 출력 메세지를 전송한다
	public void SendShowToastMsg(string a_oMsg) {
		this.SendMsg(KBDefine.CMD_SHOW_TOAST, a_oMsg, null, true);
	}
	
	//! 알림 창 출력 메세지를 전송한다
	public void SendShowAlertMsg(string a_oTitle,
		string a_oMsg, string a_oOKBtnText, string a_oCancelBtnText, System.Action<string, string> a_oCallback) {
		CBAccess.Assert(a_oTitle != null && a_oCancelBtnText != null);
		CBAccess.Assert(a_oMsg.ExIsValid() && a_oOKBtnText.ExIsValid());

		var oDataList = new Dictionary<string, string>() {
			[KUDefine.KEY_UNITY_MS_ALERT_TITLE] = a_oTitle,
			[KUDefine.KEY_UNITY_MS_ALERT_MSG] = a_oMsg,
			[KUDefine.KEY_UNITY_MS_ALERT_OK_BTN_TEXT] = a_oOKBtnText,
			[KUDefine.KEY_UNITY_MS_ALERT_CANCEL_BTN_TEXT] = a_oCancelBtnText
		};

		this.SendMsg(KBDefine.CMD_SHOW_ALERT, oDataList.ExToJSONString(), a_oCallback, true);
	}

	//! 진동 메세지를 전송한다
	public void SendVibrateMsg(EVibrateType a_eType, EVibrateStyle a_eStyle, float a_fDuration, float a_fIntensity) {
		bool bIsEnableType = a_eType > EVibrateType.NONE && a_eType < EVibrateType.MAX_VALUE;
		bool bIsEnableStyle = a_eStyle > EVibrateStyle.NONE && a_eStyle < EVibrateStyle.MAX_VALUE;

		CBAccess.Assert(bIsEnableType && bIsEnableStyle && a_fDuration.ExIsGreate(0.0f));

		var oDataList = new Dictionary<string, string>() {
			[KUDefine.KEY_UNITY_MS_VIBRATE_TYPE] = ((int)a_eType).ToString(),
			[KUDefine.KEY_UNITY_MS_VIBRATE_STYLE] = ((int)a_eStyle).ToString(),
			[KUDefine.KEY_UNITY_MS_VIBRATE_DURATION] = a_fDuration.ToString(),
			[KUDefine.KEY_UNITY_MS_VIBRATE_INTENSITY] = a_fIntensity.ToString()
		};

		this.SendMsg(KBDefine.CMD_VIBRATE, oDataList.ExToJSONString(), null, true);
	}

	//! 액티비티 인디게이터 메세지를 전송한다
	public void SendActivityIndicatorMsg(bool a_bIsStart) {
		this.SendMsg(KBDefine.CMD_ACTIVITY_INDICATOR, a_bIsStart.ToString(), null, true);
	}

	//! 공유 메세지를 전송한다
	public void SendShareMsg(string a_oMsg, 
		System.Action<NativeShare.ShareResult, string> a_oCallback, string a_oFilepath = KBDefine.EMPTY_STRING) {
		CBAccess.Assert(a_oMsg.ExIsValid());
		Func.ShowLog("CUnityMsgSender.SendShareMsg", KBDefine.LOG_COLOR_PLUGIN, a_oMsg);

		var oShare = new NativeShare();
		oShare.SetText(a_oMsg);

		if(a_oFilepath.ExIsValid()) {
			oShare.AddFile(a_oFilepath);
		}
		
		oShare.SetCallback((a_eResult, a_oTarget) => {
			a_oCallback?.Invoke(a_eResult, a_oTarget);	
		});

		oShare.Share();
	}

	//! 메세지를 전송한다
	private void SendMsg(string a_oCmd, string a_oMsg, System.Action<string, string> a_oCallback, bool a_bIsAutoRemove) {
		CBAccess.Assert(a_oMsg != null && a_oCmd.ExIsValid());
		Func.ShowLog("CUnityMsgSender.SendMsg: {0}, {1}", KBDefine.LOG_COLOR_PLUGIN, a_oCmd, a_oMsg);

		if(!CBAccess.IsMobilePlatform()) {
			a_oCallback?.Invoke(a_oCmd, string.Empty);
		} else {
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
			if(a_oCallback != null) {
				var stCallbackInfo = new KeyValuePair<bool, System.Action<string, string>>(a_bIsAutoRemove, a_oCallback);
				CDeviceMsgReceiver.Instance.AddCallbackInfo(a_oCmd, stCallbackInfo);
			}

#if UNITY_IOS
			CUnityMsgSender.HandleUnityMsg(a_oCmd, a_oMsg);
#else
			m_oAndroidPlugin.CallStatic(KUDefine.FUNC_NAME_UNITY_MS_MSG_HANDLER, a_oCmd, a_oMsg);
#endif			// #if UNITY_IOS
#endif			// #if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
		}
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_IOS
	//! 애플 로그인 메세지를 처리한다
	public void HandleLoginWithAppleMsg(SignInWithApple.CallbackArgs a_stArgs) {
		CScheduleManager.Instance.AddCallback(KUDefine.KEY_UNITY_MS_LOGIN_WITH_APPLE_CALLBACK, () => {
			Func.ShowLog("CUnityMsgSender.HandleLoginWithAppleMsg: {0}", KBDefine.LOG_COLOR_PLUGIN, a_stArgs.ExIsValidUserInfo());
			m_oLoginCallback?.Invoke(a_stArgs, a_stArgs.ExIsValidUserInfo());
		});
	}

	//! 인증 상태 반환 메세지를 처리한다
	public void HandleGetCredentialStateMsg(SignInWithApple.CallbackArgs a_stArgs) {
		CScheduleManager.Instance.AddCallback(KUDefine.KEY_UNITY_MS_GET_CREDENTIAL_STATE_CALLBACK, () => {
			Func.ShowLog("CUnityMsgSender.HandleGetCredentialStateMsg: {0}", KBDefine.LOG_COLOR_PLUGIN, a_stArgs.ExIsValidCredentialState());
			m_oCredentialCallback?.Invoke(a_stArgs, a_stArgs.ExIsValidCredentialState());
		});
	}

	//! 애플 로그인 메세지를 전송한다
	public void SendLoginWithAppleMsg(System.Action<SignInWithApple.CallbackArgs, bool> a_oCallback) {
		try {
			Func.ShowLog("CUnityMsgSender.SendLoginWithAppleMsg", KBDefine.LOG_COLOR_PLUGIN);
			m_oLoginCallback = a_oCallback;

			if(!Func.IsSupportLoginWithApple()) {
				this.HandleLoginWithAppleMsg(this.MakeErrorCallbackArgs(KBDefine.UNKNOWN_ERROR_MSG));
			} else {
				m_oLoginWithApple.Login(this.HandleLoginWithAppleMsg);
			}
		} catch(System.Exception oException) {
			Func.ShowLogWarning("CUnityMsgSender.SendLoginWithAppleMsg Exception: {0}", oException.Message);
			this.HandleLoginWithAppleMsg(this.MakeErrorCallbackArgs(oException.Message));
		}
	}

	//! 인증 상태 반환 메세지를 전송한다
	public void SendGetCredentialStateMsg(string a_oUserID, System.Action<SignInWithApple.CallbackArgs, bool> a_oCallback) {
		try {
			Func.ShowLog("CUnityMsgSender.SendGetCredentialStateMsg: {0}", KBDefine.LOG_COLOR_PLUGIN, a_oUserID);
			m_oCredentialCallback = a_oCallback;

			if(!Func.IsSupportLoginWithApple()) {
				this.HandleGetCredentialStateMsg(this.MakeErrorCallbackArgs(KBDefine.UNKNOWN_ERROR_MSG));
			} else {
				m_oLoginWithApple.GetCredentialState(a_oUserID, this.HandleGetCredentialStateMsg);
			}
		} catch(System.Exception oException) {
			Func.ShowLogWarning("CUnityMsgSender.SendGetCredentialStateMsg Exception: {0}", oException.Message);
			this.HandleGetCredentialStateMsg(this.MakeErrorCallbackArgs(oException.Message));
		}
	}

	//! 에러 콜백 인자를 생성한다
	private SignInWithApple.CallbackArgs MakeErrorCallbackArgs(string a_oMsg) {
		return new SignInWithApple.CallbackArgs() {
			error = a_oMsg
		};
	}
#endif			// #if UNITY_IOS
	#endregion			// 조건부 함수

	#region 조건부 클래스 함수
#if !UNITY_EDITOR && UNITY_IOS
	[DllImport("__Internal")]
	private static extern void HandleUnityMsg(string a_oCmd, string a_oMsg);
#endif			// #if !UNITY_EDITOR && UNITY_IOS
	#endregion			// 조건부 클래스 함수
}
