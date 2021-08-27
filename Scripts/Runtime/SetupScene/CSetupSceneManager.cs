using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//! 설정 씬 관리자
public abstract partial class CSetupSceneManager : CSceneManager {
	#region 변수
	private Dictionary<string, System.Action<string>> m_oDeviceMsgHandlerDict = new Dictionary<string, System.Action<string>>();
	#endregion			// 변수

	#region 클래스 변수
	// 객체 {
	private static GameObject m_oPopupUIs = null;
	private static GameObject m_oTopmostUIs = null;
	private static GameObject m_oAbsUIs = null;
	private static GameObject m_oTimerManager = null;
	private static GameObject m_oDebugUIs = null;

#if DEBUG || DEVELOPMENT_BUILD
	private static GameObject m_oDebugConsole = null;
#endif			// #if DEBUG || DEVELOPMENT_BUILD

#if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	private static GameObject m_oFPSCounter = null;
#endif			// #if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	// 객체 }
	#endregion			// 클래스 변수

	#region 프로퍼티
	public override string SceneName => KCDefine.B_SCENE_N_SETUP;

#if UNITY_EDITOR
	public override int ScriptOrder => KCDefine.U_SCRIPT_O_SETUP_SCENE_MANAGER;
#endif			// #if UNITY_EDITOR
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			m_oDeviceMsgHandlerDict.ExAddVal(KCDefine.B_CMD_GET_DEVICE_ID, this.HandleGetDeviceIDMsg);
			m_oDeviceMsgHandlerDict.ExAddVal(KCDefine.B_CMD_GET_COUNTRY_CODE, this.HandleGetCountryCodeMsg);
		}
	}

	//! 초기화
	public sealed override void Start() {
		base.Start();

		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			StartCoroutine(this.OnStart());
		}
	}

	//! 디바이스 메세지를 수신했을 경우
	private void OnReceiveDeviceMsg(string a_oCmd, string a_oMsg) {
		CFunc.ShowLog($"CSetupSceneManager.OnReceiveDeviceMsg: {a_oCmd}, {a_oMsg}", KCDefine.B_LOG_COLOR_SETUP);
		m_oDeviceMsgHandlerDict[a_oCmd](a_oMsg);
	}

	//! 씬을 설정한다
	protected virtual void Setup() {
		this.SetupPopupUIs();
		this.SetupTopmostUIs();
		this.SetupAbsUIs();
		this.SetupTimerManager();
		this.SetupDebugUIs();
		this.SetupDebugConsole();
		this.SetupFPSCounter();
	}

	//! 디바이스 식별자 반환 메세지를 처리한다
	private void HandleGetDeviceIDMsg(string a_oMsg) {
		string oDeviceID = CCommonAppInfoStorage.Inst.AppInfo.DeviceID;
		
		// 디바이스 식별자 설정이 필요 할 경우
		if(!oDeviceID.ExIsValid() || oDeviceID.ExIsEquals(KCDefine.B_UNKNOWN_DEVICE_ID)) {
			CCommonAppInfoStorage.Inst.AppInfo.DeviceID = a_oMsg.ExIsValid() ? a_oMsg : KCDefine.B_UNKNOWN_DEVICE_ID;
		}
		
		CCommonAppInfoStorage.Inst.SaveAppInfo();
		CUnityMsgSender.Inst.SendGetCountryCodeMsg(this.OnReceiveDeviceMsg);
	}

	//! 국가 코드 반환 메세지를 처리한다
	private void HandleGetCountryCodeMsg(string a_oMsg) {
		string oCountryCode = a_oMsg;

		// 국가 코드 설정이 필요 할 경우
		if(!CAccess.IsMobile || !oCountryCode.ExIsValid()) {
			oCountryCode = !CAccess.IsMobile ? KCDefine.B_KOREA_COUNTRY_CODE : KCDefine.B_UNKNOWN_COUNTRY_CODE;
		}

		CCommonAppInfoStorage.Inst.CountryCode = oCountryCode.ToUpper();
		CCommonAppInfoStorage.Inst.SaveAppInfo();

		CFunc.BroadcastMsg(KCDefine.SS_FUNC_N_START_SCENE_EVENT, EStartSceneEvent.LOAD_AGREE_SCENE, false);

		CSceneManager.IsSetup = true;
		CSceneLoader.Inst.LoadAdditiveScene(KCDefine.B_SCENE_N_AGREE);
	}

	//! 초기화
	private IEnumerator OnStart() {
		yield return CFactory.CreateWaitForSecs(KCDefine.U_DELAY_INIT);

		// 디바이스 정보를 설정한다 {
		int nQualityLevel = CValTable.Inst.GetInt(KCDefine.VT_KEY_QUALITY_LEVEL);
		int nTargetFrameRate = CValTable.Inst.GetInt(KCDefine.VT_KEY_DESKTOP_TARGET_FRAME_RATE);

#if UNITY_IOS || UNITY_ANDROID
		nTargetFrameRate = CValTable.Inst.GetInt(KCDefine.VT_KEY_MOBILE_TARGET_FRAME_RATE);
#else
		// 데스크 탑 일 경우
		if(CAccess.IsDesktop) {
			int nScreenWidth = KCDefine.B_DESKTOP_SCREEN_WIDTH;
			int nScreenHeight = KCDefine.B_DESKTOP_SCREEN_HEIGHT;
			
			Screen.SetResolution(nScreenWidth, nScreenHeight, CBuildOptsTable.Inst.StandaloneBuildOpts.m_eFullscreenMode);
		} else {
			string oKey = CAccess.IsConsole ? KCDefine.VT_KEY_CONSOLE_TARGET_FRAME_RATE : KCDefine.VT_KEY_HANDHELD_CONSOLE_TARGET_FRAME_RATE;	
			nTargetFrameRate = CValTable.Inst.GetInt(oKey);
		}
#endif			// #if UNITY_IOS || UNITY_ANDROID

		Application.targetFrameRate = Mathf.Min(Screen.currentResolution.refreshRate, nTargetFrameRate);
		Input.multiTouchEnabled = CValTable.Inst.GetInt(KCDefine.VT_KEY_MULTI_TOUCH_ENABLE) != KCDefine.B_VAL_0_INT;
		
		CFunc.SetupQuality((EQualityLevel)nQualityLevel, true);
		yield return CFactory.CreateWaitForSecs(KCDefine.U_DELAY_INIT);
		// 디바이스 정보를 설정한다 }
		
		this.Setup();
		yield return CFactory.CreateWaitForSecs(KCDefine.U_DELAY_INIT);
		
		CUnityMsgSender.Inst.SendGetDeviceIDMsg(this.OnReceiveDeviceMsg);
	}
	#endregion			// 함수
}
