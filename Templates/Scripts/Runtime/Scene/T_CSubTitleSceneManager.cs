using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
#if RUNTIME_TEMPLATES_MODULE_ENABLE
/** 서브 타이틀 씬 관리자 */
public partial class CSubTitleSceneManager : CTitleSceneManager {
	#region 추가 변수

	#endregion			// 추가 변수

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			this.SetupAwake();
		}
	}

	/** 초기화 */
	public override void Start() {
		base.Start();

		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			this.SetupStart();
			this.UpdateUIsState();

			// 최초 시작 일 경우
			if(CCommonAppInfoStorage.Inst.IsFirstStart) {
				this.UpdateFirstStartState();
			}
		}
	}

	/** 씬을 설정한다 */
	private void SetupAwake() {
		// Do Something
	}

	/** 씬을 설정한다 */
	private void SetupStart() {
		// 최초 플레이 일 경우
		if(CCommonAppInfoStorage.Inst.AppInfo.IsFirstPlay) {
			this.UpdateFirstPlayState();
		}
		// 업데이트가 가능 할 경우
		else if(!CAppInfoStorage.Inst.IsIgnoreUpdate && CCommonAppInfoStorage.Inst.IsEnableUpdate()) {
			CAppInfoStorage.Inst.IsIgnoreUpdate = true;
			this.ExLateCallFunc((a_oSender) => Func.ShowUpdatePopup(this.OnReceiveUpdatePopupResult));
		}
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		// Do Something
	}

	/** 최초 시작 상태를 갱신한다 */
	private void UpdateFirstStartState() {
		LogFunc.SendLaunchLog();
		LogFunc.SendSplashLog();
		
		CCommonAppInfoStorage.Inst.IsFirstStart = false;
		
#if (!UNITY_EDITOR && UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_LEVEL_EDITOR);
#else
		CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MAIN);
#endif			// #if (!UNITY_EDITOR && UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 최초 플레이 상태를 갱신한다 */
	private void UpdateFirstPlayState() {
		CCommonAppInfoStorage.Inst.AppInfo.IsFirstPlay = false;
		CCommonAppInfoStorage.Inst.SaveAppInfo();

		// 약관 동의 팝업이 닫혔을 경우
		if(CAppInfoStorage.Inst.IsCloseAgreePopup) {
			LogFunc.SendAgreeLog();
		}
	}

	/** 업데이트 팝업 결과를 수신했을 경우 */
	private void OnReceiveUpdatePopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
		// 확인 버튼을 눌렀을 경우
		if(a_bIsOK) {
			Application.OpenURL(Access.StoreURL);
		}
	}
	#endregion			// 함수

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if NEVER_USE_THIS
