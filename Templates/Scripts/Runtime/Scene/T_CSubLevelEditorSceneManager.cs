using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
#if UNITY_EDITOR || UNITY_STANDALONE
//! 서브 레벨 에디터 씬 관리자
public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager {
	#region 변수
	private SampleEngineName.STGridInfo m_stSelGridInfo;
	private CLevelInfo m_oSelLevelInfo = null;
	#endregion			// 변수
	
	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		
		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
#if UNITY_EDITOR || UNITY_STANDALONE
			// 레벨 정보가 없을 경우
			if(!CLevelInfoTable.Inst.LevelInfoList.ExIsValid()) {
				var oLevelInfo = Factory.MakeLevelInfo(KCDefine.B_VAL_0_INT);
				oLevelInfo.OnAfterDeserialize();

				CLevelInfoTable.Inst.AddLevelInfo(oLevelInfo);
			}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE

			this.SetupAwake();
		}
	}
	
	//! 초기화
	public override void Start() {
		base.Start();

		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			this.SetupStart();
			this.UpdateUIsState();
		}
	}

	//! 제거 되었을 경우
	public override void OnDestroy() {
		base.OnDestroy();

		// 앱이 실행 중 일 경우
		if(CSceneManager.IsAppRunning) {
			// Do Nothing
		}
	}

	//! 내비게이션 스택 이벤트를 수신했을 경우
	public override void OnReceiveNavStackEvent(ENavStackEvent a_eEvent) {
		base.OnReceiveNavStackEvent(a_eEvent);

		// 백 키 눌림 이벤트 일 경우
		if(a_eEvent == ENavStackEvent.BACK_KEY_DOWN) {
			Func.ShowEditorQuitPopup(this.OnReceiveEditorQuitPopupResult);
		}
	}

	//! 씬을 설정한다
	private void SetupAwake() {
		// 레벨 정보를 설정한다 {
		long nLevelID = Factory.MakeUniqueLevelID(KCDefine.B_VAL_0_INT);

		m_oSelLevelInfo = (CGameInfoStorage.Inst.PlayLevelInfo != null) ? CGameInfoStorage.Inst.PlayLevelInfo : CLevelInfoTable.Inst.GetLevelInfo(nLevelID);
		m_stSelGridInfo = SampleEngineName.Factory.MakeGridInfo(m_oSelLevelInfo);
		// 레벨 정보를 설정한다 }
	}

	//! 씬을 설정한다
	private void SetupStart() {
		// Do Nothing
	}

	//! UI 상태를 갱신한다
	private void UpdateUIsState() {
		// Do Nothing
	}

	//! 에디터 종료 팝업 결과를 수신했을 경우
	private void OnReceiveEditorQuitPopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
		// 확인 버튼을 눌렀을 경우
		if(a_bIsOK) {
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_TITLE);
		}
	}
	#endregion			// 함수
}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
#endif			// #if NEVER_USE_THIS
