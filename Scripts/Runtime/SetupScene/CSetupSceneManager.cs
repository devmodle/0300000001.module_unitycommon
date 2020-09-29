using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 설정 씬 관리자
public abstract partial class CSetupSceneManager : CSceneManager {
	#region 변수
	private new Dictionary<string, System.Action<string>> m_oDeviceMsgHandlerList = new Dictionary<string, System.Action<string>>();
	#endregion			// 변수

	#region 클래스 객체
	private static GameObject m_oDebugUI = null;
	private static GameObject m_oPopupUI = null;
	private static GameObject m_oTopmostUI = null;
	private static GameObject m_oAbsUI = null;
	private static GameObject m_oTimerManager = null;

#if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	private static GameObject m_oFPSCounter = null;
#endif			// #if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	#endregion			// 클래스 객체

	#region 프로퍼티
	public override string SceneName => KCDefine.B_SCENE_NAME_SETUP;

#if UNITY_EDITOR
	public override int ScriptOrder => KCDefine.U_SCRIPT_ORDER_SETUP_SCENE_MANAGER;
#endif			// #if UNITY_EDITOR
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		m_oDeviceMsgHandlerList.Add(KCDefine.B_CMD_GET_DEVICE_ID, this.HandleGetDeviceIDMsg);
		m_oDeviceMsgHandlerList.Add(KCDefine.B_CMD_GET_COUNTRY_CODE, this.HandleGetCountryCodeMsg);
	}

	//! 초기화
	public sealed override void Start() {
		base.Start();
		StartCoroutine(this.OnStart());
	}

	//! 디바이스 메세지를 수신했을 경우
	public void OnReceiveDeviceMsg(string a_oCmd, string a_oMsg) {
		CAccess.Assert(m_oDeviceMsgHandlerList.ContainsKey(a_oCmd));
		m_oDeviceMsgHandlerList[a_oCmd](a_oMsg);
	}

	//! 씬을 설정한다
	protected virtual void Setup() {
		this.SetupPopupUI();
		this.SetupTopmostUI();
		this.SetupAbsUI();
		this.SetupTimerManager();

#if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
		this.SetupDebugUI();
#endif			// #if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)

#if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
		this.SetupFPSCounter();
#endif			// #if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	}

	//! 디바이스 식별자 반환 메세지를 처리한다
	private void HandleGetDeviceIDMsg(string a_oMsg) {
#if MSG_PACK_ENABLE
		bool bIsValid = CCommonAppInfoStorage.Instance.AppInfo.DeviceID.ExIsValid();

		// 디바이스 식별자 설정이 필요 할 경우
		if(!bIsValid || CCommonAppInfoStorage.Instance.AppInfo.DeviceID.ExIsEquals(KCDefine.B_UNKNOWN_DEVICE_ID)) {
			CCommonAppInfoStorage.Instance.AppInfo.DeviceID = a_oMsg.ExIsValid() ? 
				a_oMsg : KCDefine.B_UNKNOWN_DEVICE_ID;
		}
		
		CCommonAppInfoStorage.Instance.SaveAppInfo();
#endif			// #if MSG_PACK_ENABLE

		// 국가 코드 반환 메세지를 전송한다
		CUnityMsgSender.Instance.SendGetCountryCodeMsg(this.OnReceiveDeviceMsg);
	}

	//! 국가 코드 반환 메세지를 처리한다
	private void HandleGetCountryCodeMsg(string a_oMsg) {
		string oCountryCode = a_oMsg;

		// 국가 코드 설정이 필요 할 경우
		if(!CAccess.IsMobile() || !a_oMsg.ExIsValid()) {
			oCountryCode = !CAccess.IsMobile() ? 
				KCDefine.B_KOREA_COUNTRY_CODE : KCDefine.B_UNKNOWN_COUNTRY_CODE;
		}
		
#if MSG_PACK_ENABLE
		CCommonAppInfoStorage.Instance.CountryCode = oCountryCode.ToUpper();
		CCommonAppInfoStorage.Instance.SaveAppInfo();
#endif			// #if MSG_PACK_ENABLE

		CSceneManager.IsSetup = true;
		CSceneLoader.Instance.LoadAdditiveScene(KCDefine.B_SCENE_NAME_AGREE);
	}

	//! 초기화
	private IEnumerator OnStart() {
		// 설정이 필요 할 경우
		if(!CSceneManager.IsSetup) {
			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);

			// 디바이스 정보를 설정한다 {
			int nQualityLevel = CValueTable.Instance.GetInt(KCDefine.VT_KEY_QUALITY_LEVEL);
			int nTargetFrameRate = CValueTable.Instance.GetInt(KCDefine.VT_KEY_DESKTOP_TARGET_FRAME_RATE);

			// 데스크 탑 일 경우
			if(CAccess.IsDesktop()) {
				Screen.SetResolution(KCDefine.B_DESKTOP_WINDOW_WIDTH, 
					KCDefine.B_DESKTOP_WINDOW_HEIGHT, FullScreenMode.Windowed);
			} else {
				// 모바일 일 경우
				if(CAccess.IsMobile()) {
					nTargetFrameRate = CValueTable.Instance.GetInt(KCDefine.VT_KEY_MOBILE_TARGET_FRAME_RATE);
				} else {
					string oKey = CAccess.IsConsole() ? 
						KCDefine.VT_KEY_CONSOLE_TARGET_FRAME_RATE : KCDefine.VT_KEY_HANDHELD_CONSOLE_TARGET_FRAME_RATE;

					nTargetFrameRate = CValueTable.Instance.GetInt(oKey);
				}
			}

			Input.multiTouchEnabled = CValueTable.Instance.GetBool(KCDefine.VT_KEY_MULTI_TOUCH_ENABLE);
			Application.targetFrameRate = Mathf.Min(nTargetFrameRate, Screen.currentResolution.refreshRate);
			
			CFunc.SetupQuality((EQualityLevel)nQualityLevel, true);
			// 디바이스 정보를 설정한다 }

#if DEBUG || DEVELOPMENT_BUILD
			CUnityMsgSender.Instance.SendSetBuildModeMsg(true);
#else
			CUnityMsgSender.Instance.SendSetBuildModeMsg(false);
#endif			// #if DEBUG || DEVELOPMENT_BUILD

			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
			
			// 저장소를 설정한다 {
#if MSG_PACK_ENABLE
			CCommonAppInfoStorage.Instance.LoadAppInfo();
#endif			// #if MSG_PACK_ENABLE

			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);
			// 저장소를 설정한다 }

			this.Setup();
			yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);

			// 디바이스 식별자 반환 메세지를 전송한다
			CUnityMsgSender.Instance.SendGetDeviceIDMsg(this.OnReceiveDeviceMsg);
		}
	}
	#endregion			// 함수
}
