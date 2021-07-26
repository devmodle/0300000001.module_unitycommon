using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if NEVER_USE_THIS
//! 서브 게임 씬 관리자
public partial class CSubGameSceneManager : CGameSceneManager {
	#region 변수
	private CLevelInfo m_oLevelInfo = null;
	private CClearInfo m_oClearInfo = null;
	
	private SampleEngineName.CEngine m_oEngine = null;
	#endregion			// 변수

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		
		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
#if DEBUG || DEVELOPMENT_BUILD
			// 플레이 레벨 정보가 없을 경우
			if(CGameInfoStorage.Inst.PlayLevelInfo == null) {
#if UNITY_EDITOR
				// 레벨 정보가 없을 경우
				if(!CLevelInfoTable.Inst.LevelInfoDictContainer.ExIsValid()) {
					var oLevelInfo = Factory.MakeLevelInfo(KCDefine.B_VAL_0_INT);
					CLevelInfoTable.Inst.AddLevelInfo(oLevelInfo);
				}
#endif			// #if UNITY_EDITOR

				CGameInfoStorage.Inst.SetupPlayLevelInfo(KCDefine.B_VAL_0_INT, EPlayMode.NORM);
			}
#endif			// #if DEBUG || DEVELOPMENT_BUILD

			this.SetupAwake();
			CSceneLoader.Inst.LoadAdditiveScene(KCDefine.B_SCENE_N_OVERLAY);
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

	//! 상태를 갱신한다
	public override void OnUpdate(float a_fDeltaTime) {
		base.OnUpdate(a_fDeltaTime);

		// 앱이 실행 중 일 경우
		if(CSceneManager.IsAppRunning) {
			m_oEngine.OnUpdate(a_fDeltaTime);
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
			// Do Nothing
		}
	}

	//! 씬을 설정한다
	private void SetupAwake() {
		this.SetupEngine();

		m_oLevelInfo = CGameInfoStorage.Inst.PlayLevelInfo;
		m_oClearInfo = CGameInfoStorage.Inst.TryGetClearInfo(CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nID, out CClearInfo oClearInfo, CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nStageID, CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nChapterID) ? oClearInfo : null;

#if DEBUG || DEVELOPMENT_BUILD
		this.SetupTestUIs();
#endif			// #if DEBUG || DEVELOPMENT_BUILD
	}

	//! 씬을 설정한다
	private void SetupStart() {
		// Do Nothing
	}

	//! 엔진을 설정한다
	private void SetupEngine() {
		var stParams = new SampleEngineName.CEngine.STParams {
			m_oLevelInfo = CGameInfoStorage.Inst.PlayLevelInfo,
			m_oClearInfo = CGameInfoStorage.Inst.TryGetClearInfo(CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nID, out CClearInfo oClearInfo, CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nStageID, CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nChapterID) ? oClearInfo : null
		};

		m_oEngine = CFactory.CreateObj<SampleEngineName.CEngine>(KDefine.GS_OBJ_N_ENGINE, this.gameObject);
		m_oEngine.Init(stParams);
	}

	//! UI 상태를 갱신한다
	private void UpdateUIsState() {
#if DEBUG || DEVELOPMENT_BUILD
		this.UpdateTestUIsState();
#endif			// #if DEBUG || DEVELOPMENT_BUILD
	}

	//! 터치를 시작했을 경우
	private void OnTouchBegin(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
		m_oEngine.OnTouchBegin(a_oSender, a_oEventData);	
	}

	//! 터치를 움직였을 경우
	private void OnTouchMove(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
		m_oEngine.OnTouchMove(a_oSender, a_oEventData);
	}

	//! 터치를 종료했을 경우
	private void OnTouchEnd(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
		m_oEngine.OnTouchEnd(a_oSender, a_oEventData);
	}

	//! 레벨을 클리어한다
	private void ClearLevel() {
		// 클리어 정보가 없을 경우
		if(!CGameInfoStorage.Inst.IsClear(m_oLevelInfo.m_stIDInfo.m_nID, m_oLevelInfo.m_stIDInfo.m_nStageID, m_oLevelInfo.m_stIDInfo.m_nChapterID)) {
			var oClearInfo = Factory.MakeClearInfo(m_oLevelInfo.m_stIDInfo.m_nID, m_oLevelInfo.m_stIDInfo.m_nStageID, m_oLevelInfo.m_stIDInfo.m_nChapterID);
			CGameInfoStorage.Inst.AddClearInfo(oClearInfo);
		}
		
		var oCurClearInfo = CGameInfoStorage.Inst.GetClearInfo(m_oLevelInfo.m_stIDInfo.m_nID, m_oLevelInfo.m_stIDInfo.m_nStageID, m_oLevelInfo.m_stIDInfo.m_nChapterID);
		CGameInfoStorage.Inst.SaveGameInfo();
	}

	//! 다음 씬을 로드한다
	private void LoadNextScene() {
		// 테스트 모드 일 경우
		if(CGameInfoStorage.Inst.PlayMode == EPlayMode.TEST) {
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_LEVEL_EDITOR);
		} else {
			// Do Nothing
		}
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR
	//! 가이드 라인을 그린다
	public override void OnDrawGizmos() {
		base.OnDrawGizmos();
	}
#endif			// #if UNITY_EDITOR

#if DEBUG || DEVELOPMENT_BUILD
	//! 테스트 UI 를 설정한다
	private void SetupTestUIs() {
		// Do Nothing
	}

	//! 테스트 UI 상태를 갱신한다
	private void UpdateTestUIsState() {
		// Do Nothing
	}
#endif			// #if DEBUG || DEVELOPMENT_BUILD
	#endregion			// 조건부 함수
}
#endif			// #if NEVER_USE_THIS
