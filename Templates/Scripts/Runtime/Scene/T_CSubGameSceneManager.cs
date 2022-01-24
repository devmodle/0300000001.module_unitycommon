using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if NEVER_USE_THIS
#if RUNTIME_TEMPLATES_MODULE_ENABLE
/** 서브 게임 씬 관리자 */
public partial class CSubGameSceneManager : CGameSceneManager {
	/** 팝업 결과 */
	private enum EPopupResult {
		NONE = -1,
		NEXT,
		RETRY,
		CONTINUE,
		LEAVE,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	private bool m_bIsLeave = false;
	private int m_nContinueTimes = 0;

	private CTouchDispatcher m_oBGTouchDispatcher = null;

	[System.NonSerialized] private CLevelInfo m_oLevelInfo = null;
	[System.NonSerialized] private CClearInfo m_oClearInfo = null;
	
#if ENGINE_TEMPLATES_MODULE_ENABLE
	private SampleEngineName.CEngine m_oEngine = null;
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
	#endregion			// 변수

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
#if DEBUG || DEVELOPMENT_BUILD
			// 플레이 레벨 정보가 없을 경우
			if(CGameInfoStorage.Inst.PlayLevelInfo == null) {
#if UNITY_STANDALONE && EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
				// 레벨 정보가 없을 경우
				if(!CLevelInfoTable.Inst.LevelInfoDictContainer.ExIsValid()) {
					var oLevelInfo = Factory.MakeLevelInfo(KCDefine.B_VAL_0_INT);

					Func.EditorSetupLevelInfo(oLevelInfo, new CSubEditorLevelCreateInfo() {
						m_nNumLevels = KCDefine.B_VAL_0_INT, m_stMinNumCells = SampleEngineName.KDefine.E_MIN_NUM_CELLS, m_stMaxNumCells = SampleEngineName.KDefine.E_MIN_NUM_CELLS
					});
					
					CLevelInfoTable.Inst.AddLevelInfo(oLevelInfo);
					CLevelInfoTable.Inst.SaveLevelInfos();
				}
#endif			// #if UNITY_STANDALONE && EDITOR_SCENE_TEMPLATES_MODULE_ENABLE

				CGameInfoStorage.Inst.SetupPlayLevelInfo(KCDefine.B_VAL_0_INT, EPlayMode.NORM);
			}
#endif			// #if DEBUG || DEVELOPMENT_BUILD

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

			CSceneLoader.Inst.LoadAdditiveScene(KCDefine.B_SCENE_N_OVERLAY);
		}
	}

	/** 상태를 갱신한다 */
	public override void OnUpdate(float a_fDeltaTime) {
		base.OnUpdate(a_fDeltaTime);

		// 앱이 실행 중 일 경우
		if(CSceneManager.IsAppRunning) {
#if ENGINE_TEMPLATES_MODULE_ENABLE
			m_oEngine.OnUpdate(a_fDeltaTime);
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
		}
	}

	/** 제거 되었을 경우 */
	public override void OnDestroy() {
		base.OnDestroy();

		try {
			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAwake || CSceneManager.IsAppRunning) {
				// Do Something
			}
		} catch(System.Exception oException) {
			CFunc.ShowLogWarning($"CSubGameSceneManager.OnDestroy Exception: {oException.Message}");
		}
	}

	/** 앱이 정지 되었을 경우 */
	public virtual void OnApplicationPause(bool a_bIsPause) {
		// 재개 되었을 경우
		if(!a_bIsPause && (CSceneManager.IsAwake || CSceneManager.IsAppRunning)) {
#if ADS_MODULE_ENABLE
			// 광고 출력이 가능 할 경우
			if(CAppInfoStorage.Inst.IsEnableShowFullscreenAds && CAdsManager.Inst.IsLoadFullscreenAds(CPluginInfoTable.Inst.AdsPlatform)) {
				Func.ShowFullscreenAds(null);
			}
#endif			// #if ADS_MODULE_ENABLE
		}
	}

	/** 내비게이션 스택 이벤트를 수신했을 경우 */
	public override void OnReceiveNavStackEvent(ENavStackEvent a_eEvent) {
		base.OnReceiveNavStackEvent(a_eEvent);

		// 백 키 눌림 이벤트 일 경우
		if(a_eEvent == ENavStackEvent.BACK_KEY_DOWN) {
			// 이전 씬이 레벨 에디터 씬 일 경우
			if(CSceneLoader.Inst.PrevActiveSceneName.Equals(KCDefine.B_SCENE_N_LEVEL_EDITOR)) {
				Func.ShowLeavePopup(this.OnReceiveLeavePopupResult);
			} else {
				this.OnTouchPauseBtn();
			}
		}
	}

	/** 씬을 설정한다 */
	private void SetupAwake() {
		this.SetupEngine();

		m_oLevelInfo = CGameInfoStorage.Inst.PlayLevelInfo;
		m_oClearInfo = CGameInfoStorage.Inst.TryGetClearInfo(CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nID, out CClearInfo oClearInfo, CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nStageID, CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nChapterID) ? oClearInfo : null;

		// 버튼을 설정한다
		var oPauseBtn = this.UIsBase.ExFindComponent<Button>(KCDefine.U_OBJ_N_RESTORE_BTN);
		oPauseBtn?.ExAddListener(this.OnTouchPauseBtn, true, false);

		// 터치 전달자를 설정한다
		m_oBGTouchDispatcher = m_oBGTouchResponder?.GetComponentInChildren<CTouchDispatcher>();
		m_oBGTouchDispatcher?.ExSetBeginCallback(this.OnTouchBegin, false);
		m_oBGTouchDispatcher?.ExSetMoveCallback(this.OnTouchMove, false);
		m_oBGTouchDispatcher?.ExSetEndCallback(this.OnTouchEnd, false);

#if ENGINE_TEMPLATES_MODULE_ENABLE
		// 비율을 설정한다 {
		bool bIsValidA = !float.IsNaN(m_oEngine.GridInfo.m_stGridScale.x) && !float.IsInfinity(m_oEngine.GridInfo.m_stGridScale.x);
		bool bIsValidB = !float.IsNaN(m_oEngine.GridInfo.m_stGridScale.y) && !float.IsInfinity(m_oEngine.GridInfo.m_stGridScale.y);
		bool bIsValidC = !float.IsNaN(m_oEngine.GridInfo.m_stGridScale.z) && !float.IsInfinity(m_oEngine.GridInfo.m_stGridScale.z);

		m_oBlockObjs.transform.localScale = (bIsValidA && bIsValidB && bIsValidC) ? m_oEngine.GridInfo.m_stGridScale : Vector3.one;
		// 비율을 설정한다 }
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE

#if DEBUG || DEVELOPMENT_BUILD
		this.SetupTestUIs();
#endif			// #if DEBUG || DEVELOPMENT_BUILD
	}

	/** 씬을 설정한다 */
	private void SetupStart() {
		// Do Something
	}

	/** 엔진을 설정한다 */
	private void SetupEngine() {
#if ENGINE_TEMPLATES_MODULE_ENABLE
		bool bIsValid = CGameInfoStorage.Inst.TryGetClearInfo(CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nID, out CClearInfo oClearInfo, CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nStageID, CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nChapterID);

		var stParams = new SampleEngineName.CEngine.STParams() {
			m_oLevelInfo = CGameInfoStorage.Inst.PlayLevelInfo, m_oClearInfo = bIsValid ? oClearInfo : null, m_oBlockObjs = this.m_oBlockObjs
		};

		var stCallbackParams = new SampleEngineName.CEngine.STCallbackParams() {
			m_oClearCallback = this.OnClearLevel, m_oClearFailCallback = this.OnClearFailLevel
		};

		m_oEngine = CFactory.CreateObj<SampleEngineName.CEngine>(KDefine.GS_OBJ_N_ENGINE, this.gameObject);
		m_oEngine.Init(stParams, stCallbackParams);
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
#if DEBUG || DEVELOPMENT_BUILD
		this.UpdateTestUIsState();
#endif			// #if DEBUG || DEVELOPMENT_BUILD
	}

	/** 그만두기 팝업 결과를 수신했을 경우 */
	private void OnReceiveLeavePopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
		// 확인 버튼을 눌렀을 경우
		if(a_bIsOK) {
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_LEVEL_EDITOR);
		}
	}

	/** 팝업 결과를 수신했을 경우 */
	private void OnReceivePopupResult(CPopup a_oSender, EPopupResult a_eResult) {
		// 팝업이 존재 할 경우
		if(a_oSender != null) {
			a_oSender.IsIgnoreAni = true;
			a_oSender.Close();
		}

		switch(a_eResult) {
			case EPopupResult.NEXT: this.LoadNextLevel(); break;
			case EPopupResult.RETRY: this.RetryCurLevel(); break;
			case EPopupResult.CONTINUE: this.ContinueCurLevel(); break;
			case EPopupResult.LEAVE: m_bIsLeave = true; this.LoadNextLevel(); break;
		}
	}

	/** 정지 버튼을 눌렀을 경우 */
	private void OnTouchPauseBtn() {
		// Do Something
	}

	/** 터치를 시작했을 경우 */
	private void OnTouchBegin(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
		// 배경 터치 전달자 일 경우
		if(m_oBGTouchDispatcher == a_oSender) {
#if ENGINE_TEMPLATES_MODULE_ENABLE
			m_oEngine.OnTouchBegin(a_oSender, a_oEventData);
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
		}
	}

	/** 터치를 움직였을 경우 */
	private void OnTouchMove(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
		// 배경 터치 전달자 일 경우
		if(m_oBGTouchDispatcher == a_oSender) {
#if ENGINE_TEMPLATES_MODULE_ENABLE
			m_oEngine.OnTouchMove(a_oSender, a_oEventData);
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
		}
	}

	/** 터치를 종료했을 경우 */
	private void OnTouchEnd(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
		// 배경 터치 전달자 일 경우
		if(m_oBGTouchDispatcher == a_oSender) {
#if ENGINE_TEMPLATES_MODULE_ENABLE
			m_oEngine.OnTouchEnd(a_oSender, a_oEventData);
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
		}
	}

	/** 다음 레벨을 로드한다 */
	private void LoadNextLevel() {
		switch(CGameInfoStorage.Inst.PlayMode) {
			case EPlayMode.NORM: {
				int nNextID = m_oLevelInfo.m_stIDInfo.m_nID + KCDefine.B_VAL_1_INT;
				int nNumClearInfos = CGameInfoStorage.Inst.GetNumClearInfos(m_oLevelInfo.m_stIDInfo.m_nStageID, m_oLevelInfo.m_stIDInfo.m_nChapterID);

#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
				bool bIsValid = CLevelInfoTable.Inst.TryGetLevelInfo(nNextID, out CLevelInfo oNextLevelInfo, m_oLevelInfo.m_stIDInfo.m_nStageID, m_oLevelInfo.m_stIDInfo.m_nChapterID) && nNextID <= nNumClearInfos;
#else
				bool bIsValid = CEpisodeInfoTable.Inst.TryGetLevelInfo(nNextID, out STLevelInfo stNextLevelInfo, m_oLevelInfo.m_stIDInfo.m_nStageID, m_oLevelInfo.m_stIDInfo.m_nChapterID) && nNextID <= nNumClearInfos;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)

				// 다음 레벨이 존재 할 경우
				if(bIsValid && !m_bIsLeave) {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
					CGameInfoStorage.Inst.SetupPlayLevelInfo(oNextLevelInfo.m_stIDInfo.m_nID, CGameInfoStorage.Inst.PlayMode, oNextLevelInfo.m_stIDInfo.m_nStageID, oNextLevelInfo.m_stIDInfo.m_nChapterID);
#else
					CGameInfoStorage.Inst.SetupPlayLevelInfo(stNextLevelInfo.m_nID, CGameInfoStorage.Inst.PlayMode, stNextLevelInfo.m_nStageID, stNextLevelInfo.m_nChapterID);
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)

#if ADS_MODULE_ENABLE
					Func.ShowFullscreenAds((a_oSender, a_bIsSuccess) => CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME));
#else
					CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME);
#endif			// #if ADS_MODULE_ENABLE
				} else {
#if ADS_MODULE_ENABLE
					Func.ShowFullscreenAds((a_oSender, a_bIsSuccess) => CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MAIN));
#else
					CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MAIN);
#endif			// #if ADS_MODULE_ENABLE
				}
			} break;
			case EPlayMode.TUTORIAL: {
				// Do Something
			} break;
			case EPlayMode.TEST: {
				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_LEVEL_EDITOR);
			} break;
		}
	}

	/** 현재 레벨을 재시도한다 */
	private void RetryCurLevel() {
#if ADS_MODULE_ENABLE
		Func.ShowFullscreenAds((a_oSender, a_bIsSuccess) => CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME));
#else
		CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME);
#endif			// #if ADS_MODULE_ENABLE
	}

	/** 현재 레벨을 이어한다 */
	private void ContinueCurLevel() {
		m_nContinueTimes += KCDefine.B_VAL_1_INT;
	}

	/** 이어하기 팝업을 출력한다 */
	private void ShowContinuePopup() {
#if RUNTIME_TEMPLATES_MODULE_ENABLE
		Func.ShowContinuePopup(this.PopupUIs, (a_oSender) => {
			var stParams = new CContinuePopup.STParams() {
				m_nContinueTimes = this.m_nContinueTimes, m_oLevelInfo = this.m_oLevelInfo
			};

			var stCallbackParams = new CContinuePopup.STCallbackParams() {
				m_oRetryCallback = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.RETRY),
				m_oContinueCallback = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.CONTINUE),
				m_oLeaveCallback = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.LEAVE)
			};

			(a_oSender as CContinuePopup).Init(stParams, stCallbackParams);
		});
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
	}

	/** 결과 팝업을 출력한다 */
	private void ShowResultPopup(bool a_bIsClear) {
#if RUNTIME_TEMPLATES_MODULE_ENABLE
		Func.ShowResultPopup(this.PopupUIs, (a_oSender) => {
			var stParams = new CResultPopup.STParams() {
				m_stRecordInfo = new STRecordInfo {
					m_bIsSuccess = a_bIsClear,

#if ENGINE_TEMPLATES_MODULE_ENABLE
					m_nIntRecord = m_oEngine.IntRecord,
					m_dblRealRecord = m_oEngine.RealRecord
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
				},
				
				m_oLevelInfo = this.m_oLevelInfo, m_oClearInfo = this.m_oClearInfo
			};

			var stCallbackParams = new CResultPopup.STCallbackParams() {
				m_oNextCallback = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.NEXT),
				m_oRetryCallback = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.RETRY),
				m_oLeaveCallback = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.LEAVE)
			};

			(a_oSender as CResultPopup).Init(stParams, stCallbackParams);
		});
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR
	/** 기즈모를 그린다 */
	public override void OnDrawGizmos() {
		base.OnDrawGizmos();

		// 앱 실행 중이 아닐 경우
		if(!Application.isPlaying) {
			// Do Something
		}
	}
#endif			// #if UNITY_EDITOR

#if DEBUG || DEVELOPMENT_BUILD
	/** 테스트 UI 를 설정한다 */
	private void SetupTestUIs() {
		// Do Something
	}

	/** 테스트 UI 상태를 갱신한다 */
	private void UpdateTestUIsState() {
		// Do Something
	}
#endif			// #if DEBUG || DEVELOPMENT_BUILD

#if ENGINE_TEMPLATES_MODULE_ENABLE
	/** 레벨을 클리어했을 경우 */
	private void OnClearLevel(SampleEngineName.CEngine a_oSender) {
		// 클리어 정보가 없을 경우
		if(!CGameInfoStorage.Inst.IsClearLevel(m_oLevelInfo.m_stIDInfo.m_nID, m_oLevelInfo.m_stIDInfo.m_nStageID, m_oLevelInfo.m_stIDInfo.m_nChapterID)) {
			CGameInfoStorage.Inst.AddClearInfo(Factory.MakeClearInfo(m_oLevelInfo.m_stIDInfo.m_nID, m_oLevelInfo.m_stIDInfo.m_nStageID, m_oLevelInfo.m_stIDInfo.m_nChapterID));
		}
		
		var oClearInfo = CGameInfoStorage.Inst.GetClearInfo(m_oLevelInfo.m_stIDInfo.m_nID, m_oLevelInfo.m_stIDInfo.m_nStageID, m_oLevelInfo.m_stIDInfo.m_nChapterID);
		CGameInfoStorage.Inst.SaveGameInfo();

		this.ShowResultPopup(true);
	}

	/** 레벨 클리어에 실패했을 경우 */
	private void OnClearFailLevel(SampleEngineName.CEngine a_oSender) {
		this.ShowResultPopup(false);
	}
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
	#endregion			// 조건부 함수

	#region 추가 함수
#if DEBUG || DEVELOPMENT_BUILD

#endif			// #if DEBUG || DEVELOPMENT_BUILD
	#endregion			// 추가 함수
}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if NEVER_USE_THIS
