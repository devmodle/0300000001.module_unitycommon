using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if NEVER_USE_THIS
//! 서브 게임 씬 관리자
public partial class CSubGameSceneManager : CGameSceneManager {
	#region 변수
	private bool m_bIsLeave = false;

	private CLevelInfo m_oLevelInfo = null;
	private CClearInfo m_oClearInfo = null;

	private CTouchDispatcher m_oBGTouchDispatcher = null;
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
#if UNITY_STANDALONE
				// 레벨 정보가 없을 경우
				if(!CLevelInfoTable.Inst.LevelInfoDictContainer.ExIsValid()) {
					var oLevelInfo = Factory.MakeLevelInfo(KCDefine.B_VAL_0_INT);
					oLevelInfo.OnAfterDeserialize();
					
					CLevelInfoTable.Inst.AddLevelInfo(oLevelInfo);
					CLevelInfoTable.Inst.SaveLevelInfos();
				}
#endif			// #if UNITY_STANDALONE

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
			// Do Something
		}
	}

	//! 내비게이션 스택 이벤트를 수신했을 경우
	public override void OnReceiveNavStackEvent(ENavStackEvent a_eEvent) {
		base.OnReceiveNavStackEvent(a_eEvent);

		// 백 키 눌림 이벤트 일 경우
		if(a_eEvent == ENavStackEvent.BACK_KEY_DOWN) {
			// Do Something
		}
	}

	//! 씬을 설정한다
	private void SetupAwake() {
		this.SetupEngine();

		m_oLevelInfo = CGameInfoStorage.Inst.PlayLevelInfo;
		m_oClearInfo = CGameInfoStorage.Inst.TryGetClearInfo(CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nID, out CClearInfo oClearInfo, CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nStageID, CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nChapterID) ? oClearInfo : null;

		// 비율을 설정한다 {
		bool bIsValidA = !float.IsNaN(m_oEngine.GridInfo.m_stGridScale.x) && !float.IsInfinity(m_oEngine.GridInfo.m_stGridScale.x);
		bool bIsValidB = !float.IsNaN(m_oEngine.GridInfo.m_stGridScale.y) && !float.IsInfinity(m_oEngine.GridInfo.m_stGridScale.y);
		bool bIsValidC = !float.IsNaN(m_oEngine.GridInfo.m_stGridScale.z) && !float.IsInfinity(m_oEngine.GridInfo.m_stGridScale.z);

		m_oBlockObjs.transform.localScale = (bIsValidA && bIsValidB && bIsValidC) ? m_oEngine.GridInfo.m_stGridScale : Vector3.one;
		// 비율을 설정한다 }

		// 터치 전달자를 설정한다
		m_oBGTouchDispatcher = m_oBGTouchResponder?.GetComponentInChildren<CTouchDispatcher>();
		m_oBGTouchDispatcher?.ExSetBeginCallback(this.OnTouchBegin, false);
		m_oBGTouchDispatcher?.ExSetMoveCallback(this.OnTouchMove, false);
		m_oBGTouchDispatcher?.ExSetEndCallback(this.OnTouchEnd, false);

#if DEBUG || DEVELOPMENT_BUILD
		this.SetupTestUIs();
#endif			// #if DEBUG || DEVELOPMENT_BUILD
	}

	//! 씬을 설정한다
	private void SetupStart() {
		// Do Something
	}

	//! 엔진을 설정한다
	private void SetupEngine() {
		var stParams = new SampleEngineName.CEngine.STParams {
			m_oLevelInfo = CGameInfoStorage.Inst.PlayLevelInfo,
			m_oClearInfo = CGameInfoStorage.Inst.TryGetClearInfo(CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nID, out CClearInfo oClearInfo, CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nStageID, CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nChapterID) ? oClearInfo : null,

			m_oBlockObjs = this.m_oBlockObjs
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
		// 배경 터치 전달자 일 경우
		if(m_oBGTouchDispatcher == a_oSender) {
			m_oEngine.OnTouchBegin(a_oSender, a_oEventData);
		}
	}

	//! 터치를 움직였을 경우
	private void OnTouchMove(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
		// 배경 터치 전달자 일 경우
		if(m_oBGTouchDispatcher == a_oSender) {
			m_oEngine.OnTouchMove(a_oSender, a_oEventData);
		}
	}

	//! 터치를 종료했을 경우
	private void OnTouchEnd(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
		// 배경 터치 전달자 일 경우
		if(m_oBGTouchDispatcher == a_oSender) {
			m_oEngine.OnTouchEnd(a_oSender, a_oEventData);
		}
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

	//! 다음 레벨을 로드한다
	private void LoadNextLevel() {
		// 테스트 모드 일 경우
		if(CGameInfoStorage.Inst.PlayMode == EPlayMode.TEST) {
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_LEVEL_EDITOR);
		}
		// 튜토리얼 모드 일 경우
		else if(CGameInfoStorage.Inst.PlayMode == EPlayMode.TUTORIAL) {
			// Do Nothing
		} else {
			bool bIsValid = CEpisodeInfoTable.Inst.TryGetLevelInfo(m_oLevelInfo.m_stIDInfo.m_nID, out STLevelInfo stNextLevelInfo, m_oLevelInfo.m_stIDInfo.m_nStageID, m_oLevelInfo.m_stIDInfo.m_nChapterID);

			// 다음 레벨이 존재 할 경우
			if(bIsValid && !m_bIsLeave) {
				CGameInfoStorage.Inst.SetupPlayLevelInfo(stNextLevelInfo.m_nID, CGameInfoStorage.Inst.PlayMode, stNextLevelInfo.m_nStageID, stNextLevelInfo.m_nChapterID);
				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME);
			} else {
				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_TITLE);
			}
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
		// Do Something
	}

	//! 테스트 UI 상태를 갱신한다
	private void UpdateTestUIsState() {
		// Do Something
	}
#endif			// #if DEBUG || DEVELOPMENT_BUILD
	#endregion			// 조건부 함수
}
#endif			// #if NEVER_USE_THIS
