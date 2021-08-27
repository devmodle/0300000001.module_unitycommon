using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using EnhancedUI.EnhancedScroller;

#if NEVER_USE_THIS
#if UNITY_EDITOR || UNITY_STANDALONE
//! 서브 레벨 에디터 씬 관리자
public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
	#region 변수
	private STIDInfo m_stSelIDInfo;
	private SampleEngineName.STGridInfo m_stSelGridInfo;

	private CLevelInfo m_oSelLevelInfo = null;
	private SpriteRenderer m_oSelBlockSprite = null;

	private CTouchDispatcher m_oBGTouchDispatcher = null;
	private Dictionary<SampleEngineName.EBlockKinds, SpriteRenderer>[,] m_oBlockSpriteDicts = null;

	// UI {
	private EnhancedScroller m_oSelScroller = null;

	private EnhancedScrollerCellView m_oOriginLevelScrollerCellView = null;
	private EnhancedScrollerCellView m_oOriginStageScrollerCellView = null;
	private EnhancedScrollerCellView m_oOriginChapterScrollerCellView = null;

	// 왼쪽 에디터 UI
	private EnhancedScroller m_oLEUIsLevelScroller = null;
	private EnhancedScroller m_oLEUIsStageScroller = null;
	private EnhancedScroller m_oLEUIsChapterScroller = null;

	// 오른쪽 에디터 UI {
	private Text m_oREUIsTitleText = null;
	private Toggle m_oREUIsRewardToggle = null;

	private InputField m_oREUIsLevelInput = null;
	private InputField m_oREUIsNumCellsXInput = null;
	private InputField m_oREUIsNumCellsYInput = null;
	// 오른쪽 에디터 UI }

	// 중앙 에디터 UI {
	private Text m_oMEUIsLevelText = null;

	private Button m_oMEUIsPrevLevelBtn = null;
	private Button m_oMEUIsNextLevelBtn = null;

	private Button m_oMEUIsMoveLevelBtn = null;
	private Button m_oMEUIsRemoveLevelBtn = null;
	// 중앙 에디터 UI }
	// UI }
	#endregion			// 변수

	#region 인터페이스
	//! 셀 개수를 반환한다
	public int GetNumberOfCells(EnhancedScroller a_oSender) {
		// 챕터 스크롤러 일 경우
		if(m_oLEUIsChapterScroller == a_oSender) {
			return CLevelInfoTable.Inst.NumChapterInfos;
		}

		return (m_oLEUIsStageScroller == a_oSender) ? CLevelInfoTable.Inst.GetNumStageInfos(m_oSelLevelInfo.m_stIDInfo.m_nChapterID) : CLevelInfoTable.Inst.GetNumLevelInfos(m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);
	}

	//! 셀 뷰 크기를 반환한다
	public float GetCellViewSize(EnhancedScroller a_oSender, int a_nDataIdx) {
		// 챕터 스크롤러 일 경우
		if(m_oLEUIsChapterScroller == a_oSender) {
			return (m_oOriginChapterScrollerCellView.transform as RectTransform).sizeDelta.y;
		}
		
		return (m_oLEUIsStageScroller == a_oSender) ? (m_oOriginStageScrollerCellView.transform as RectTransform).sizeDelta.y : (m_oOriginLevelScrollerCellView.transform as RectTransform).sizeDelta.y;
	}

	//! 셀 뷰를 반환한다
	public EnhancedScrollerCellView GetCellView(EnhancedScroller a_oSender, int a_nDataIdx, int a_nCellIdx) {
		long nNumInfos = CLevelInfoTable.Inst.NumChapterInfos;

		Color stColor = (m_oSelLevelInfo.m_stIDInfo.m_nChapterID == a_nDataIdx) ? KCDefine.U_COLOR_NORM : KCDefine.U_COLOR_DISABLE;
		STIDInfo stIDInfo = CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, a_nDataIdx);

		string oNameFmt = KCDefine.B_TEXT_FMT_CHAPTER;
		string oNumInfosStr = string.Empty;

		EnhancedScrollerCellView oOriginScrollerCellView = m_oOriginChapterScrollerCellView;

		// 챕터 스크롤러 일 경우
		if(m_oLEUIsChapterScroller == a_oSender) {
			int nNumStageInfos = CLevelInfoTable.Inst.GetNumStageInfos(a_nDataIdx);
			oNumInfosStr = string.Format(KCDefine.B_TEXT_FMT_BRACKET, nNumStageInfos);
		}
		// 스테이지 스크롤러 일 경우 
		else if(m_oLEUIsStageScroller == a_oSender) {
			int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(a_nDataIdx, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);
			nNumInfos = CLevelInfoTable.Inst.GetNumStageInfos(m_oSelLevelInfo.m_stIDInfo.m_nChapterID);

			stColor = (m_oSelLevelInfo.m_stIDInfo.m_nStageID == a_nDataIdx) ? KCDefine.U_COLOR_NORM : KCDefine.U_COLOR_DISABLE;
			stIDInfo = CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, a_nDataIdx, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);

			oNameFmt = KCDefine.B_TEXT_FMT_STAGE;
			oNumInfosStr = string.Format(KCDefine.B_TEXT_FMT_BRACKET, nNumLevelInfos);
			oOriginScrollerCellView = m_oOriginStageScrollerCellView;
		} else {
			nNumInfos = CLevelInfoTable.Inst.GetNumLevelInfos(m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);

			stColor = (m_oSelLevelInfo.m_stIDInfo.m_nID == a_nDataIdx) ? KCDefine.U_COLOR_NORM : KCDefine.U_COLOR_DISABLE;
			stIDInfo = CFactory.MakeIDInfo(a_nDataIdx, m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);

			oNameFmt = KCDefine.B_TEXT_FMT_LEVEL;
			oOriginScrollerCellView = m_oOriginLevelScrollerCellView;
		}

		var stParams = new CEditorScrollerCellView.STParams() {
			m_stBaseParams = new CScrollerCellView.STParams() {
				m_oScroller = a_oSender	
			},

			m_nLevelID = CFactory.MakeUniqueLevelID(stIDInfo.m_nID, stIDInfo.m_nStageID, stIDInfo.m_nChapterID)
		};

		var stCallbackParams = new CEditorScrollerCellView.STCallbackParams() {
			m_stBaseCallbackParams = new CScrollerCellView.STCallbackParams() {
#if UNITY_STANDALONE
				m_oSelCallback = this.OnTouchESCVSelBtn
#endif			// #if UNITY_STANDALONE
			},

#if UNITY_STANDALONE
			m_oCopyCallback = this.OnTouchESCVCopyBtn,
			m_oMoveCallback = this.OnTouchESCVMoveBtn,
			m_oRemoveCallback = this.OnTouchESCVRemoveBtn
#endif			// #if UNITY_STANDALONE
		};

		string oName = string.Format(oNameFmt, a_nDataIdx + KCDefine.B_VAL_1_INT);
		string oScrollerCellViewName = string.Format(KCDefine.B_TEXT_FMT_2_SPACE_COMBINE, oName, oNumInfosStr);

		var oScrollerCellView = a_oSender.GetCellView(oOriginScrollerCellView) as CEditorScrollerCellView;
		oScrollerCellView.Init(stParams, stCallbackParams);
		oScrollerCellView.transform.localScale = Vector3.one;

		oScrollerCellView.MoveBtn?.ExSetInteractable(nNumInfos > KCDefine.B_VAL_1_INT, false);
		oScrollerCellView.RemoveBtn?.ExSetInteractable(nNumInfos > KCDefine.B_VAL_1_INT, false);

		oScrollerCellView.NameText?.ExSetText<Text>(oScrollerCellViewName, false);
		oScrollerCellView.SelBtn?.image.ExSetColor<Image>(stColor, false);

		return oScrollerCellView;
	}
	#endregion			// 인터페이스

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		
		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
#if UNITY_STANDALONE
			// 레벨 정보가 없을 경우
			if(!CLevelInfoTable.Inst.LevelInfoDictContainer.ExIsValid()) {
				var oLevelInfo = Factory.MakeLevelInfo(KCDefine.B_VAL_0_INT);

				Func.EditorSetupLevelInfo(oLevelInfo, new CSubEditorLevelCreateInfo() {
					m_nNumLevels = KCDefine.B_VAL_0_INT,

					m_stMinNumCells = SampleEngineName.KDefine.E_MIN_NUM_CELLS,
					m_stMaxNumCells = SampleEngineName.KDefine.E_MIN_NUM_CELLS
				});

				CLevelInfoTable.Inst.AddLevelInfo(oLevelInfo);
				CLevelInfoTable.Inst.SaveLevelInfos();
			}
#endif			// #if UNITY_STANDALONE

			this.SetupAwake();
		}
	}
	
	//! 초기화
	public override void Start() {
		base.Start();

		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
#if UNITY_STANDALONE
			this.ExLateCallFunc((a_oSender, a_oParams) => {
				this.UpdateUIsState();
			}, KCDefine.U_DELAY_INIT);
#endif			// #if UNITY_STANDALONE

			this.SetupStart();
		}
	}

	//! 제거 되었을 경우
	public override void OnDestroy() {
		base.OnDestroy();

		// 앱이 실행 중 일 경우
		if(CSceneManager.IsAwake || CSceneManager.IsAppRunning) {
			// Do Something
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
		// 스크롤러 셀 뷰를 설정한다 {
		var oLevelScrollerCellView = CResManager.Inst.GetRes<GameObject>(KCDefine.E_OBJ_P_LEVEL_EDITOR_SCROLLER_CELL_VIEW);
		var oStageScrollerCellView = CResManager.Inst.GetRes<GameObject>(KCDefine.E_OBJ_P_STAGE_EDITOR_SCROLLER_CELL_VIEW);
		var oChapterScrollerCellView = CResManager.Inst.GetRes<GameObject>(KCDefine.E_OBJ_P_CHAPTER_EDITOR_SCROLLER_CELL_VIEW);

		m_oOriginLevelScrollerCellView = oLevelScrollerCellView?.GetComponentInChildren<EnhancedScrollerCellView>();
		m_oOriginStageScrollerCellView = oStageScrollerCellView?.GetComponentInChildren<EnhancedScrollerCellView>();
		m_oOriginChapterScrollerCellView = oChapterScrollerCellView?.GetComponentInChildren<EnhancedScrollerCellView>();
		// 스크롤러 셀 뷰를 설정한다 }

#if UNITY_STANDALONE
		this.SetupLeftEditorUIs();
		this.SetupRightEditorUIs();
		this.SetupMidEditorUIs();

		// 레벨 정보를 설정한다
		m_oSelLevelInfo = CGameInfoStorage.Inst.PlayLevelInfo ?? CLevelInfoTable.Inst.GetLevelInfo(KCDefine.B_VAL_0_INT);

		// 터치 전달자를 설정한다
		m_oBGTouchDispatcher = m_oBGTouchResponder?.GetComponentInChildren<CTouchDispatcher>();
		m_oBGTouchDispatcher?.ExSetBeginCallback(this.OnTouchBegin, false);
		m_oBGTouchDispatcher?.ExSetMoveCallback(this.OnTouchMove, false);
		m_oBGTouchDispatcher?.ExSetEndCallback(this.OnTouchEnd, false);
#endif			// #if UNITY_STANDALONE
	}

	//! 씬을 설정한다
	private void SetupStart() {
		// Do Something
	}

	//! 에디터 종료 팝업 결과를 수신했을 경우
	private void OnReceiveEditorQuitPopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
		// 확인 버튼을 눌렀을 경우
		if(a_bIsOK) {
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_TITLE);
		}
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_STANDALONE
	//! 블럭 스프라이트를 리셋한다
	private void ResetBlockSprites() {
		// 블럭 스프라이트가 존재 할 경우
		if(m_oBlockSpriteDicts.ExIsValid()) {
			for(int i = 0; i < m_oBlockSpriteDicts.GetLength(KCDefine.B_VAL_0_INT); ++i) {
				for(int j = 0; j < m_oBlockSpriteDicts.GetLength(KCDefine.B_VAL_1_INT); ++j) {
					foreach(var stKeyVal in m_oBlockSpriteDicts[i, j]) {
						CFactory.RemoveObj(stKeyVal.Value.gameObject);
					}
				}
			}
		}

		m_stSelGridInfo = SampleEngineName.Factory.MakeGridInfo(m_oSelLevelInfo);

		// 비율을 설정한다 {
		bool bIsValidA = !float.IsNaN(m_stSelGridInfo.m_stGridScale.x) && !float.IsInfinity(m_stSelGridInfo.m_stGridScale.x);
		bool bIsValidB = !float.IsNaN(m_stSelGridInfo.m_stGridScale.y) && !float.IsInfinity(m_stSelGridInfo.m_stGridScale.y);
		bool bIsValidC = !float.IsNaN(m_stSelGridInfo.m_stGridScale.z) && !float.IsInfinity(m_stSelGridInfo.m_stGridScale.z);

		m_oBlockObjs.transform.localScale = (bIsValidA && bIsValidB && bIsValidC) ? m_stSelGridInfo.m_stGridScale : Vector3.one;
		// 비율을 설정한다 }

		// 블럭 스프라이트를 설정한다 {
		m_oBlockSpriteDicts = new Dictionary<SampleEngineName.EBlockKinds, SpriteRenderer>[m_oSelLevelInfo.NumCells.y, m_oSelLevelInfo.NumCells.x];

		for(int i = 0; i < m_oSelLevelInfo.m_oCellInfoDictContainer.Count; ++i) {
			for(int j = 0; j < m_oSelLevelInfo.m_oCellInfoDictContainer[i].Count; ++j) {
				var stIdx = m_oSelLevelInfo.m_oCellInfoDictContainer[i][j].m_stIdxInfo.ExToIdx();
				var oBlockSpriteDict = new Dictionary<SampleEngineName.EBlockKinds, SpriteRenderer>();

				for(int k = 0; k < m_oSelLevelInfo.m_oCellInfoDictContainer[i][j].m_oBlockKindsList.Count; ++k) {
					var oBlockSprite = Factory.CreateBlockSprite(m_oSelLevelInfo.m_oCellInfoDictContainer[i][j].m_oBlockKindsList[k], m_oBlockObjs);
					oBlockSprite.transform.localPosition = m_stSelGridInfo.m_stGridPivotPos + stIdx.ExToPos(SampleEngineName.KDefine.E_OFFSET_CELL, SampleEngineName.KDefine.E_SIZE_CELL);

					oBlockSpriteDict.Add(m_oSelLevelInfo.m_oCellInfoDictContainer[i][j].m_oBlockKindsList[k], oBlockSprite);
				}
				
				m_oBlockSpriteDicts[stIdx.y, stIdx.x] = oBlockSpriteDict;
			}
		}
		// 블럭 스프라이트를 설정한다 }
	}

	//! 왼쪽 에디터 UI 를 설정한다
	private void SetupLeftEditorUIs() {
		// 버튼을 설정한다 {
		var oAddLevelBtn = m_oLeftEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_LE_UIS_ADD_LEVEL_BTN);
		oAddLevelBtn?.onClick.AddListener(this.OnTouchLEUIsAddLevelBtn);

		var oAddStageBtn = m_oLeftEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_LE_UIS_ADD_STAGE_BTN);
		oAddStageBtn?.onClick.AddListener(this.OnTouchLEUIsAddStageBtn);

		var oAddChapterBtn = m_oLeftEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_LE_UIS_ADD_CHAPTER_BTN);
		oAddChapterBtn?.onClick.AddListener(this.OnTouchLEUIsAddChapterBtn);
		// 버튼을 설정한다 }

		// 스크롤러를 설정한다 {
		m_oLEUIsLevelScroller = m_oLeftEditorUIs.ExFindComponent<EnhancedScroller>(KCDefine.LES_OBJ_N_LEVEL_SCROLL_VIEW);
		m_oLEUIsLevelScroller?.ExSetActive(false, false);
		m_oLEUIsLevelScroller?.ExSetDelegate(this, false);

		m_oLEUIsStageScroller = m_oLeftEditorUIs.ExFindComponent<EnhancedScroller>(KCDefine.LES_OBJ_N_STAGE_SCROLL_VIEW);
		m_oLEUIsStageScroller?.ExSetActive(true, false);
		m_oLEUIsStageScroller?.ExSetDelegate(this, false);

		m_oLEUIsChapterScroller = m_oLeftEditorUIs.ExFindComponent<EnhancedScroller>(KCDefine.LES_OBJ_N_CHAPTER_SCROLL_VIEW);
		m_oLEUIsChapterScroller?.ExSetActive(true, false);
		m_oLEUIsChapterScroller?.ExSetDelegate(this, false);
		// 스크롤러를 설정한다 }
	}

	//! 오른족 에디터 UI 를 설정한다
	private void SetupRightEditorUIs() {
		// 텍스트를 설정한다
		m_oREUIsTitleText = m_oRightEditorUIs.ExFindComponent<Text>(KCDefine.U_OBJ_N_TITLE_TEXT);

		// 토글을 설정한다
		m_oREUIsRewardToggle = m_oRightEditorUIs.ExFindComponent<Toggle>(KCDefine.U_OBJ_N_RE_UIS_REWARD_TOGGLE);
		m_oREUIsRewardToggle?.onValueChanged.AddListener(this.OnTouchREUIsRewardToggle);

		// 입력 필드를 설정한다
		m_oREUIsLevelInput = m_oRightEditorUIs.ExFindComponent<InputField>(KCDefine.LES_OBJ_N_RE_UIS_LEVEL_INPUT);
		m_oREUIsNumCellsXInput = m_oRightEditorUIs.ExFindComponent<InputField>(KCDefine.LES_OBJ_N_RE_UIS_NUM_CELLS_X_INPUT);
		m_oREUIsNumCellsYInput = m_oRightEditorUIs.ExFindComponent<InputField>(KCDefine.LES_OBJ_N_RE_UIS_NUM_CELLS_Y_INPUT);

		// 버튼을 설정한다 {
		var oApplyBtn = m_oRightEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_RE_UIS_APPLY_BTN);
		oApplyBtn?.onClick.AddListener(this.OnTouchREUIsApplyBtn);

		var oLoadLevelBtn = m_oRightEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_RE_UIS_LOAD_LEVEL_BTN);
		oLoadLevelBtn?.onClick.AddListener(this.OnTouchREUIsLoadLevelBtn);
		// 버튼을 설정한다 }
	}

	//! 중앙 에디터 UI 를 설정한다
	private void SetupMidEditorUIs() {
		// 텍스트를 설정한다
		m_oMEUIsLevelText = m_oMidEditorUIs.ExFindComponent<Text>(KCDefine.U_OBJ_N_LEVEL_TEXT);

		// 버튼을 설정한다 {
		m_oMEUIsPrevLevelBtn = m_oMidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_PREV_LEVEL_BTN);
		m_oMEUIsPrevLevelBtn?.onClick.AddListener(this.OnTouchMEUIsPrevLevelBtn);

		m_oMEUIsNextLevelBtn = m_oMidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_NEXT_LEVEL_BTN);
		m_oMEUIsNextLevelBtn?.onClick.AddListener(this.OnTouchMEUIsNextLevelBtn);

		m_oMEUIsMoveLevelBtn = m_oMidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_MOVE_LEVEL_BTN);
		m_oMEUIsMoveLevelBtn?.onClick.AddListener(this.OnTouchMEUIsMoveLevelBtn);

		m_oMEUIsRemoveLevelBtn = m_oMidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_REMOVE_LEVEL_BTN);
		m_oMEUIsRemoveLevelBtn?.onClick.AddListener(this.OnTouchMEUIsRemoveLevelBtn);

		var oCopyLevelBtn = m_oMidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_COPY_LEVEL_BTN);
		oCopyLevelBtn?.onClick.AddListener(this.OnTouchMEUIsCopyLevelBtn);

		var oSaveBtn = m_oMidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_SAVE_BTN);
		oSaveBtn?.onClick.AddListener(this.OnTouchMEUIsSaveBtn);

		var oResetBtn = m_oMidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_RESET_BTN);
		oResetBtn?.onClick.AddListener(this.OnTouchMEUIsResetBtn);

		var oTestBtn = m_oMidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_TEST_BTN);
		oTestBtn?.onClick.AddListener(this.OnTouchMEUIsTestBtn);
		// 버튼을 설정한다 }
	}

	//! UI 상태를 갱신한다
	private void UpdateUIsState() {
		this.ResetBlockSprites();

		this.UpdateLeftEditorUIsState();
		this.UpdateRightEditorUIsState();
		this.UpdateMidEditorUIsState();
	}

	//! 왼쪽 에디터 UI 상태를 갱신한다
	private void UpdateLeftEditorUIsState() {
		// 스크롤러를 갱신한다
		m_oLEUIsLevelScroller?.ExReloadData(m_oSelLevelInfo.m_stIDInfo.m_nID - KCDefine.B_VAL_1_INT, false);
		m_oLEUIsStageScroller?.ExReloadData(m_oSelLevelInfo.m_stIDInfo.m_nStageID - KCDefine.B_VAL_1_INT, false);
		m_oLEUIsChapterScroller?.ExReloadData(m_oSelLevelInfo.m_stIDInfo.m_nChapterID - KCDefine.B_VAL_1_INT, false);
	}

	//! 오른쪽 에디터 UI 상태를 갱신한다
	private void UpdateRightEditorUIsState() {
		// 텍스트를 설정한다 {
		int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);
		string oLevelPageFmt = CStrTable.Inst.GetStr(KCDefine.ST_KEY_COMMON_LEVEL_PAGE_TEXT_FMT);

		m_oREUIsTitleText?.ExSetText<Text>(string.Format(oLevelPageFmt, m_oSelLevelInfo.m_stIDInfo.m_nID + KCDefine.B_VAL_1_INT, nNumLevelInfos), false);
		// 텍스트를 설정한다 }
		
		// 입력 필드를 갱신한다 {
		string oLevelStr = string.Format(KCDefine.B_TEXT_FMT_1_DIGITS, m_oSelLevelInfo.m_stIDInfo.m_nID + KCDefine.B_VAL_1_INT);
		m_oREUIsLevelInput?.ExSetText<InputField>(oLevelStr, false);

		string oNumCellsXStr = string.Format(KCDefine.B_TEXT_FMT_1_DIGITS, m_oSelLevelInfo.NumCells.x);
		m_oREUIsNumCellsXInput?.ExSetText<InputField>((m_oSelLevelInfo.NumCells.x <= KCDefine.B_VAL_0_INT) ? string.Empty : oNumCellsXStr, false);

		string oNumCellsYStr = string.Format(KCDefine.B_TEXT_FMT_1_DIGITS, m_oSelLevelInfo.NumCells.y);
		m_oREUIsNumCellsYInput?.ExSetText<InputField>((m_oSelLevelInfo.NumCells.y <= KCDefine.B_VAL_0_INT) ? string.Empty : oNumCellsYStr, false);
		// 입력 필드를 갱신한다 }
	}

	//! 중앙 에디터 UI 상태를 갱신한다
	private void UpdateMidEditorUIsState() {
		// 텍스트를 갱신한다
		string oStr = string.Format(KCDefine.B_TEXT_FMT_LEVEL, m_oSelLevelInfo.m_stIDInfo.m_nID + KCDefine.B_VAL_1_INT);
		m_oMEUIsLevelText?.ExSetText<Text>(oStr, false);

		// 버튼을 갱신한다 {
		int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);

		m_oMEUIsPrevLevelBtn?.ExSetInteractable(CLevelInfoTable.Inst.TryGetLevelInfo(m_oSelLevelInfo.m_stIDInfo.m_nID - KCDefine.B_VAL_1_INT, out CLevelInfo oPrevLevelInfo, m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID), false);
		m_oMEUIsNextLevelBtn?.ExSetInteractable(CLevelInfoTable.Inst.TryGetLevelInfo(m_oSelLevelInfo.m_stIDInfo.m_nID + KCDefine.B_VAL_1_INT, out CLevelInfo oNextLevelInfo, m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID), false);

		m_oMEUIsMoveLevelBtn?.ExSetInteractable(nNumLevelInfos > KCDefine.B_VAL_1_INT);
		m_oMEUIsRemoveLevelBtn?.ExSetInteractable(nNumLevelInfos > KCDefine.B_VAL_1_INT);
		// 버튼을 갱신한다 }
	}

	//! 터치를 시작했을 경우
	private void OnTouchBegin(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
		// 배경 터치 전달자 일 경우
		if(m_oBGTouchDispatcher == a_oSender) {
			// Do Something
		}
	}

	//! 터치를 움직였을 경우
	private void OnTouchMove(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
		// 배경 터치 전달자 일 경우
		if(m_oBGTouchDispatcher == a_oSender) {
			// Do Something
		}
	}

	//! 터치를 종료했을 경우
	private void OnTouchEnd(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
		// 배경 터치 전달자 일 경우
		if(m_oBGTouchDispatcher == a_oSender) {
			// Do Something
		}
	}

	//! 왼쪽 에디터 UI 레벨 추가 버튼을 눌렀을 경우
	private void OnTouchLEUIsAddLevelBtn() {
		Func.ShowEditorLevelCreatePopup(this.SubPopupUIs, (a_oSender) => {
			var oEditorLevelCreatePopup = a_oSender as CEditorLevelCreatePopup;
			oEditorLevelCreatePopup.Init(this.OnReceiveEditorLevelCreatePopupResult);
		});
	}

	//! 왼쪽 에디터 UI 스테이지 추가 버튼을 눌렀을 경우
	private void OnTouchLEUIsAddStageBtn() {
		int nNumStageInfos = CLevelInfoTable.Inst.GetNumStageInfos(m_oSelLevelInfo.m_stIDInfo.m_nChapterID);

		// 스테이지 추가가 가능 할 경우
		if(nNumStageInfos < KCDefine.U_MAX_NUM_STAGE_INFOS) {
			this.AddLevelInfo(KCDefine.B_VAL_0_INT, nNumStageInfos, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);
		}
	}

	//! 왼쪽 에디터 UI 챕터 추가 버튼을 눌렀을 경우
	private void OnTouchLEUIsAddChapterBtn() {
		int nNumChapterInfos = CLevelInfoTable.Inst.NumChapterInfos;

		// 챕터 추가가 가능 할 경우
		if(nNumChapterInfos < KCDefine.U_MAX_NUM_CHAPTER_INFOS) {
			this.AddLevelInfo(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, nNumChapterInfos);
		}
	}

	//! 오른쪽 에디터 UI 보상 토글을 눌렀을 경우
	private void OnTouchREUIsRewardToggle(bool a_bIsOn) {
		// Do Something
	}

	//! 오른쪽 에디터 UI 적용 버튼을 눌렀을 경우
	private void OnTouchREUIsApplyBtn() {
		bool bIsValidA = int.TryParse(m_oREUIsNumCellsXInput?.text, out int nNumCellsX);
		bool bIsValidB = int.TryParse(m_oREUIsNumCellsYInput?.text, out int nNumCellsY);

		bool bIsValidNumCellsX = Mathf.Max(nNumCellsX, SampleEngineName.KDefine.E_MIN_NUM_CELLS.x) != m_oSelLevelInfo.NumCells.x;
		bool bIsValidNumCellsY = Mathf.Max(nNumCellsY, SampleEngineName.KDefine.E_MIN_NUM_CELLS.y) != m_oSelLevelInfo.NumCells.y;

		// 셀 개수가 유효 할 경우
		if(bIsValidA && bIsValidB && (bIsValidNumCellsX || bIsValidNumCellsY)) {
			Func.EditorSetupLevelInfo(m_oSelLevelInfo, new CSubEditorLevelCreateInfo() {
				m_nNumLevels = KCDefine.B_VAL_0_INT,

				m_stMinNumCells = new Vector3Int(nNumCellsX, nNumCellsY, KCDefine.B_VAL_0_INT),
				m_stMaxNumCells = new Vector3Int(nNumCellsX, nNumCellsY, KCDefine.B_VAL_0_INT)
			});
			
			this.UpdateUIsState();
		}
	}

	//! 오른쪽 에디터 UI 레벨 로드 버튼을 눌렀을 경우
	private void OnTouchREUIsLoadLevelBtn() {
		// 식별자가 유효 할 경우
		if(int.TryParse(m_oREUIsLevelInput?.text, out int nID)) {
			int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);
			m_oSelLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(Mathf.Clamp(nID, KCDefine.B_VAL_1_INT, nNumLevelInfos) - KCDefine.B_VAL_1_INT, m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);
			
			this.UpdateUIsState();
		}
	}

	//! 중앙 에디터 UI 이전 레벨 버튼을 눌렀을 경우
	private void OnTouchMEUIsPrevLevelBtn() {
		// 이전 레벨 정보가 존재 할 경우
		if(CLevelInfoTable.Inst.TryGetLevelInfo(m_oSelLevelInfo.m_stIDInfo.m_nID - KCDefine.B_VAL_1_INT, out CLevelInfo oPrevLevelInfo, m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID)) {
			m_oSelLevelInfo = oPrevLevelInfo;
			this.UpdateUIsState();
		}
	}

	//! 중앙 에디터 UI 다음 레벨 버튼을 눌렀을 경우
	private void OnTouchMEUIsNextLevelBtn() {
		// 다음 레벨 정보가 존재 할 경우
		if(CLevelInfoTable.Inst.TryGetLevelInfo(m_oSelLevelInfo.m_stIDInfo.m_nID + KCDefine.B_VAL_1_INT, out CLevelInfo oNextLevelInfo, m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID)) {
			m_oSelLevelInfo = oNextLevelInfo;
			this.UpdateUIsState();
		}
	}

	//! 중앙 에디터 UI 레벨 복사 버튼을 눌렀을 경우
	private void OnTouchMEUIsCopyLevelBtn() {
		int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);

		// 레벨 추가가 가능 할 경우
		if(nNumLevelInfos < KCDefine.U_MAX_NUM_LEVEL_INFOS) {
			var stIDInfo = CFactory.MakeIDInfo(m_oSelLevelInfo.m_stIDInfo.m_nID, m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);
			this.CopyLevelInfos(m_oLEUIsLevelScroller, stIDInfo);
		}
	}

	//! 중앙 에디터 UI 레벨 이동 버튼을 눌렀을 경우
	private void OnTouchMEUIsMoveLevelBtn() {
		m_stSelIDInfo = CFactory.MakeIDInfo(m_oSelLevelInfo.m_stIDInfo.m_nID, m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);
		m_oSelScroller = m_oLEUIsLevelScroller;

		Func.ShowEditorInputPopup(this.SubPopupUIs, (a_oSender) => {
			var oEditorInputPopup = a_oSender as CEditorInputPopup;
			oEditorInputPopup.Init(this.OnReceiveEditorInputPopupResult);
		});
	}

	//! 중앙 에디터 UI 레벨 제거 버튼을 눌렀을 경우
	private void OnTouchMEUIsRemoveLevelBtn() {
		m_stSelIDInfo = CFactory.MakeIDInfo(m_oSelLevelInfo.m_stIDInfo.m_nID, m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);
		m_oSelScroller = m_oLEUIsLevelScroller;

		this.OnReceiveEditorRemovePopupResult(null, true);
	}

	//! 중앙 에디터 UI 저장 버튼을 눌렀을 경우
	private void OnTouchMEUIsSaveBtn() {
		CLevelInfoTable.Inst.SaveLevelInfos();
	}

	//! 중앙 에디터 UI 리셋 버튼을 눌렀을 경우
	private void OnTouchMEUIsResetBtn() {
		CLevelInfoTable.Inst.LevelInfoDictContainer.Clear();
		CLevelInfoTable.Inst.LoadLevelInfos();
		
		// 레벨 정보가 없을 경우
		if(!CLevelInfoTable.Inst.LevelInfoDictContainer.ExIsValid()) {
			var oLevelInfo = Factory.MakeLevelInfo(KCDefine.B_VAL_0_INT);
			CLevelInfoTable.Inst.AddLevelInfo(oLevelInfo);
		}

		m_oSelLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(KCDefine.B_VAL_0_INT);
		this.UpdateUIsState();
	}

	//! 중앙 에디터 UI 테스트 버튼을 눌렀을 경우
	private void OnTouchMEUIsTestBtn() {
		CGameInfoStorage.Inst.SetupPlayLevelInfo(m_oSelLevelInfo.m_stIDInfo.m_nID, EPlayMode.TEST, m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);
		CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME);
	}

	//! 에디터 스크롤러 셀 뷰 선택 버튼을 눌렀을 경우
	private void OnTouchESCVSelBtn(CScrollerCellView a_oSender, long a_nLevelID) {
		m_oSelLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(a_nLevelID.ExUniqueLevelIDToID(), a_nLevelID.ExUniqueLevelIDToStageID(), a_nLevelID.ExUniqueLevelIDToChapterID());
		this.UpdateUIsState();
	}

	//! 에디터 스크롤러 셀 뷰 복사 버튼을 눌렀을 경우
	private void OnTouchESCVCopyBtn(CScrollerCellView a_oSender, long a_nLevelID) {
		int nNumInfos = CLevelInfoTable.Inst.NumChapterInfos;
		int nMaxNumInfos = KCDefine.U_MAX_NUM_CHAPTER_INFOS;

		// 챕터 스크롤러가 아닐 경우
		if(m_oLEUIsChapterScroller != a_oSender.Scroller) {
			nNumInfos = (m_oLEUIsStageScroller == a_oSender.Scroller) ? CLevelInfoTable.Inst.GetNumStageInfos(a_nLevelID.ExUniqueLevelIDToChapterID()) : CLevelInfoTable.Inst.GetNumLevelInfos(a_nLevelID.ExUniqueLevelIDToStageID(), a_nLevelID.ExUniqueLevelIDToChapterID());
			nMaxNumInfos = (m_oLEUIsStageScroller == a_oSender.Scroller) ? KCDefine.U_MAX_NUM_STAGE_INFOS : KCDefine.U_MAX_NUM_LEVEL_INFOS;
		}

		// 복사가 가능 할 경우
		if(nNumInfos < nMaxNumInfos) {
			this.CopyLevelInfos(a_oSender.Scroller, CFactory.MakeIDInfo(a_nLevelID.ExUniqueLevelIDToID(), a_nLevelID.ExUniqueLevelIDToStageID(), a_nLevelID.ExUniqueLevelIDToChapterID()));
		}
	}

	//! 에디터 스크롤러 셀 뷰 이동 버튼을 눌렀을 경우
	private void OnTouchESCVMoveBtn(CScrollerCellView a_oSender, long a_nLevelID) {
		m_stSelIDInfo = CFactory.MakeIDInfo(a_nLevelID.ExUniqueLevelIDToID(), a_nLevelID.ExUniqueLevelIDToStageID(), a_nLevelID.ExUniqueLevelIDToChapterID());
		m_oSelScroller = a_oSender.Scroller;

		Func.ShowEditorInputPopup(this.SubPopupUIs, (a_oSender) => {
			var oEditorInputPopup = a_oSender as CEditorInputPopup;
			oEditorInputPopup.Init(this.OnReceiveEditorInputPopupResult);
		});
	}

	//! 에디터 스크롤러 셀 뷰 제거 버튼을 눌렀을 경우
	private void OnTouchESCVRemoveBtn(CScrollerCellView a_oSender, long a_nLevelID) {
		m_stSelIDInfo = CFactory.MakeIDInfo(a_nLevelID.ExUniqueLevelIDToID(), a_nLevelID.ExUniqueLevelIDToStageID(), a_nLevelID.ExUniqueLevelIDToChapterID());
		m_oSelScroller = a_oSender.Scroller;

		// 레벨 스크롤러 일 경우
		if(m_oLEUIsLevelScroller == a_oSender.Scroller) {
			Func.ShowEditorLevelRemovePopup(this.OnReceiveEditorRemovePopupResult);
		}
		// 스테이지 스크롤러 일 경우
		else if(m_oLEUIsStageScroller == a_oSender.Scroller) {
			Func.ShowEditorStageRemovePopup(this.OnReceiveEditorRemovePopupResult);
		} else {
			Func.ShowEditorChapterRemovePopup(this.OnReceiveEditorRemovePopupResult);
		}
	}

	//! 에디터 제거 팝업 결과를 수신했을 경우
	private void OnReceiveEditorRemovePopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
		// 확인 버튼을 눌렀을 경우
		if(a_bIsOK) {
			this.RemoveLevelInfos(m_oSelScroller, m_stSelIDInfo);
			this.UpdateUIsState();
		}
	}

	//! 에디터 입력 팝업 결과를 수신했을 경우
	private void OnReceiveEditorInputPopupResult(CEditorInputPopup a_oSender, string a_oStr) {
		// 식별자가 유효 할 경우
		if(int.TryParse(a_oStr, out int nID)) {
			this.MoveLevelInfos(m_oSelScroller, m_stSelIDInfo, nID);
			this.UpdateUIsState();
		}
	}

	//! 에디터 레벨 생성 팝업 결과를 수신했을 경우
	private void OnReceiveEditorLevelCreatePopupResult(CEditorLevelCreatePopup a_oSender, CEditorLevelCreateInfo a_oCreateInfo) {
		int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);
		int nNumCreateLevelInfos = (nNumLevelInfos + a_oCreateInfo.m_nNumLevels < KCDefine.U_MAX_NUM_LEVEL_INFOS) ? a_oCreateInfo.m_nNumLevels : KCDefine.U_MAX_NUM_LEVEL_INFOS - nNumLevelInfos;

		for(int i = 0; i < nNumCreateLevelInfos; ++i) {
			var oLevelInfo = Factory.MakeLevelInfo(i + nNumLevelInfos, m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);
			m_oSelLevelInfo = oLevelInfo;

			CLevelInfoTable.Inst.AddLevelInfo(oLevelInfo);
			Func.EditorSetupLevelInfo(oLevelInfo, a_oCreateInfo);
		}

		this.UpdateUIsState();
	}

	//! 레벨 정보를 반환한다
	private bool TryGetLevelInfo(STIDInfo a_stPrevIDInfo, STIDInfo a_stNextIDInfo, out CLevelInfo a_oOutLevelInfo) {
		CLevelInfoTable.Inst.TryGetLevelInfo(a_stPrevIDInfo.m_nID, out CLevelInfo oPrevLevelInfo, a_stPrevIDInfo.m_nStageID, a_stPrevIDInfo.m_nChapterID);
		CLevelInfoTable.Inst.TryGetLevelInfo(a_stNextIDInfo.m_nID, out CLevelInfo oNextLevelInfo, a_stNextIDInfo.m_nStageID, a_stNextIDInfo.m_nChapterID);

		a_oOutLevelInfo = oPrevLevelInfo ?? oNextLevelInfo;
		return oPrevLevelInfo != null || oNextLevelInfo != null;
	}

	//! 레벨 정보를 추가한다
	private void AddLevelInfo(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		m_oSelLevelInfo = Factory.MakeLevelInfo(a_nID, a_nStageID, a_nChapterID);
		CLevelInfoTable.Inst.AddLevelInfo(m_oSelLevelInfo);

		Func.EditorSetupLevelInfo(m_oSelLevelInfo, new CSubEditorLevelCreateInfo() {
			m_nNumLevels = KCDefine.B_VAL_0_INT,

			m_stMinNumCells = SampleEngineName.KDefine.E_MIN_NUM_CELLS,
			m_stMaxNumCells = SampleEngineName.KDefine.E_MIN_NUM_CELLS
		});

		this.UpdateUIsState();
	}
	
	//! 레벨 정보를 제거한다
	private void RemoveLevelInfos(EnhancedScroller a_oScroller, STIDInfo a_stIDInfo) {
		var oLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(a_stIDInfo.m_nID, a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);

		// 레벨 스크롤러 일 경우
		if(m_oLEUIsLevelScroller == a_oScroller) {
			CLevelInfoTable.Inst.RemoveLevelInfo(a_stIDInfo.m_nID, a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);
		}
		// 스테이지 스크롤러 일 경우
		else if(m_oLEUIsStageScroller == a_oScroller) {
			CLevelInfoTable.Inst.RemoveStageLevelInfos(a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);
		} else {
			CLevelInfoTable.Inst.RemoveChapterLevelInfos(a_stIDInfo.m_nChapterID);
		}
		
		// 레벨 정보가 없을 경우
		if(!CLevelInfoTable.Inst.LevelInfoDictContainer.ExIsValid()) {
			m_oSelLevelInfo = Factory.MakeLevelInfo(KCDefine.B_VAL_0_INT);
			CLevelInfoTable.Inst.AddLevelInfo(m_oSelLevelInfo);
		} else {
			CLevelInfo oSelLevelInfo = null;

			// 레벨 스크롤러 일 경우
			if(m_oLEUIsLevelScroller == a_oScroller) {
				var stPrevIDInfo = CFactory.MakeIDInfo(a_stIDInfo.m_nID - KCDefine.B_VAL_1_INT, a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);
				var stNextIDInfo = CFactory.MakeIDInfo(a_stIDInfo.m_nID, a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);

				this.TryGetLevelInfo(stPrevIDInfo, stNextIDInfo, out oSelLevelInfo);
			}

			// 스테이지 스크롤러 일 경우
			if(oSelLevelInfo == null || m_oLEUIsStageScroller == a_oScroller) {
				var stPrevIDInfo = CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, a_stIDInfo.m_nStageID - KCDefine.B_VAL_1_INT, a_stIDInfo.m_nChapterID);
				var stNextIDInfo = CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);

				this.TryGetLevelInfo(stPrevIDInfo, stNextIDInfo, out oSelLevelInfo);
			}

			// 챕터 스크롤러 일 경우
			if(oSelLevelInfo == null || m_oLEUIsChapterScroller == a_oScroller) {
				var stPrevIDInfo = CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, a_stIDInfo.m_nChapterID - KCDefine.B_VAL_1_INT);
				var stNextIDInfo = CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, a_stIDInfo.m_nChapterID);

				this.TryGetLevelInfo(stPrevIDInfo, stNextIDInfo, out oSelLevelInfo);
			}

			m_oSelLevelInfo = oSelLevelInfo;
		}

		this.UpdateUIsState();
	}

	//! 레벨 정보를 복사한다
	private void CopyLevelInfos(EnhancedScroller a_oScroller, STIDInfo a_stIDInfo) {
		// 레벨 스크롤러 일 경우
		if(m_oLEUIsLevelScroller == a_oScroller) {
			var oLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(a_stIDInfo.m_nID, a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);

			var oCloneLevelInfo = oLevelInfo.Clone() as CLevelInfo;
			oCloneLevelInfo.m_stIDInfo.m_nID = CLevelInfoTable.Inst.GetNumLevelInfos(a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);
			
			m_oSelLevelInfo = oCloneLevelInfo;
			CLevelInfoTable.Inst.AddLevelInfo(oCloneLevelInfo);
		} else {
			// 스테이지 스크롤러 일 경우
			if(m_oLEUIsStageScroller == a_oScroller) {
				int nNumStageInfos = CLevelInfoTable.Inst.GetNumStageInfos(a_stIDInfo.m_nChapterID);
				var oStageLevelInfoDict = CLevelInfoTable.Inst.GetStageLevelInfos(a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);

				for(int i = 0; i < oStageLevelInfoDict.Count; ++i) {
					var oCloneLevelInfo = oStageLevelInfoDict[i].Clone() as CLevelInfo;
					oCloneLevelInfo.m_stIDInfo.m_nStageID = nNumStageInfos;

					CLevelInfoTable.Inst.AddLevelInfo(oCloneLevelInfo);
				}
			} else {
				int nNumChapterInfos = CLevelInfoTable.Inst.NumChapterInfos;
				var oChapterLevelInfoDictContainer = CLevelInfoTable.Inst.GetChapterLevelInfos(a_stIDInfo.m_nChapterID);

				for(int i = 0; i < oChapterLevelInfoDictContainer.Count; ++i) {
					for(int j = 0; j < oChapterLevelInfoDictContainer[i].Count; ++j) {
						var oCloneLevelInfo = oChapterLevelInfoDictContainer[i][j].Clone() as CLevelInfo;
						oCloneLevelInfo.m_stIDInfo.m_nChapterID = nNumChapterInfos;

						CLevelInfoTable.Inst.AddLevelInfo(oCloneLevelInfo);
					}
				}
			}

			int nID = KCDefine.B_VAL_0_INT;
			int nStageID = (m_oLEUIsStageScroller == a_oScroller) ? CLevelInfoTable.Inst.GetNumStageInfos(a_stIDInfo.m_nChapterID) - KCDefine.B_VAL_1_INT : KCDefine.B_VAL_0_INT;
			int nChapterID = (m_oLEUIsChapterScroller == a_oScroller) ? CLevelInfoTable.Inst.NumChapterInfos - KCDefine.B_VAL_1_INT : a_stIDInfo.m_nChapterID;

			m_oSelLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(nID, nStageID, nChapterID);
		}

		CAccess.AssignVal<CLevelInfo>(ref m_oSelLevelInfo, m_oSelLevelInfo, CLevelInfoTable.Inst.GetLevelInfo(KCDefine.B_VAL_0_INT));
		this.UpdateUIsState();
	}

	//! 레벨 정보를 이동한다
	private void MoveLevelInfos(EnhancedScroller a_oScroller, STIDInfo a_stIDInfo, int a_nToID) {
		// 레벨 스크롤러 일 경우
		if(m_oLEUIsLevelScroller == a_oScroller) {
			int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);
			int nToID = Mathf.Clamp(a_nToID, KCDefine.B_VAL_1_INT, nNumLevelInfos) - KCDefine.B_VAL_1_INT;
			
			CLevelInfoTable.Inst.MoveLevelInfo(a_stIDInfo.m_nID, nToID, a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);
		} 
		// 스테이지 스크롤러 일 경우
		else if(m_oLEUIsStageScroller == a_oScroller) {
			int nNumStageInfos = CLevelInfoTable.Inst.GetNumStageInfos(a_stIDInfo.m_nChapterID);
			int nToID = Mathf.Clamp(a_nToID, KCDefine.B_VAL_1_INT, nNumStageInfos) - KCDefine.B_VAL_1_INT;

			CLevelInfoTable.Inst.MoveStageLevelInfos(a_stIDInfo.m_nStageID, nToID, a_stIDInfo.m_nChapterID);
		} else {
			int nNumChapterInfos = CLevelInfoTable.Inst.NumChapterInfos;
			int nToID = Mathf.Clamp(a_nToID, KCDefine.B_VAL_1_INT, nNumChapterInfos) - KCDefine.B_VAL_1_INT;

			CLevelInfoTable.Inst.MoveChapterLevelInfos(a_stIDInfo.m_nChapterID, nToID);
		}
	}
#endif			// #if UNITY_STANDALONE
	#endregion			// 조건부 함수

	#region 추가 변수

	#endregion			// 추가 변수

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
#endif			// #if NEVER_USE_THIS
