using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;

#if NEVER_USE_THIS
#if UNITY_EDITOR || UNITY_STANDALONE
//! 서브 레벨 에디터 씬 관리자
public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
	#region 변수
	private SampleEngineName.STGridInfo m_stSelGridInfo;
	private Vector3Int m_stSelNumCells;

	private CLevelInfo m_oSelLevelInfo = null;
	#endregion			// 변수

	#region UI 변수
	private EnhancedScrollerCellView m_oOriginLevelScrollerCellView = null;
	private EnhancedScrollerCellView m_oOriginStageScrollerCellView = null;
	private EnhancedScrollerCellView m_oOriginChapterScrollerCellView = null;

	// 왼쪽 에디터 UI
	private EnhancedScroller m_oLEUIsLevelScroller = null;
	private EnhancedScroller m_oLEUIsStageScroller = null;
	private EnhancedScroller m_oLEUIsChapterScroller = null;

	// 오른쪽 에디터 UI {
	private Text m_oREUIsTitleText = null;

	private InputField m_oREUIsLevelInput = null;
	private InputField m_oREUIsNumCellsXInput = null;
	private InputField m_oREUIsNumCellsYInput = null;
	// 오른쪽 에디터 UI }

	// 중앙 에디터 UI {
	private Text m_oMEUIsLevelText = null;

	private Button m_oMEUIsPrevLevelBtn = null;
	private Button m_oMEUIsNextLevelBtn = null;
	// 중앙 에디터 UI }
	#endregion			// UI 변수

	#region 인터페이스
	//! 셀 개수를 반환한다
	public int GetNumberOfCells(EnhancedScroller a_oSender) {
		// 챕터 스크롤러 일 경우
		if(m_oLEUIsChapterScroller == a_oSender) {
			return CLevelInfoTable.Inst.NumChapterInfos;
		}

		return (m_oLEUIsStageScroller == a_oSender) ? CLevelInfoTable.Inst.GetNumStageInfos(m_oSelLevelInfo.ChapterID) : CLevelInfoTable.Inst.GetNumStageLevelInfos(m_oSelLevelInfo.StageID, m_oSelLevelInfo.ChapterID);
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
		long nID = Factory.MakeUniqueChapterID(a_nDataIdx);
		Color stColor = (m_oSelLevelInfo.ChapterID == a_nDataIdx) ? KCDefine.U_COLOR_NORM : KCDefine.U_COLOR_DISABLE;

		string oNameFmt = KCDefine.B_TEXT_FMT_CHAPTER;
		EnhancedScrollerCellView oOriginScrollerCellView = m_oOriginChapterScrollerCellView;

		// 챕터 스크롤러가 아닐 경우
		if(m_oLEUIsChapterScroller != a_oSender) {
			nID = (m_oLEUIsStageScroller == a_oSender) ? Factory.MakeUniqueStageID(a_nDataIdx, m_oSelLevelInfo.ChapterID) : Factory.MakeUniqueLevelID(a_nDataIdx, m_oSelLevelInfo.StageID, m_oSelLevelInfo.ChapterID);
			oNameFmt = (m_oLEUIsStageScroller == a_oSender) ? KCDefine.B_TEXT_FMT_STAGE : KCDefine.B_TEXT_FMT_LEVEL;
			oOriginScrollerCellView = (m_oLEUIsStageScroller == a_oSender) ? m_oOriginStageScrollerCellView : m_oOriginLevelScrollerCellView;

			// 스테이지 스크롤러 일 경우
			if(m_oLEUIsStageScroller == a_oSender) {
				stColor = (m_oSelLevelInfo.StageID == a_nDataIdx) ? KCDefine.U_COLOR_NORM : KCDefine.U_COLOR_DISABLE;
			} else {
				stColor = (m_oSelLevelInfo.ID == a_nDataIdx) ? KCDefine.U_COLOR_NORM : KCDefine.U_COLOR_DISABLE;
			}
		}

		var stParams = new CEditorScrollerCellView.STParams() {
			m_nID = nID,
			m_nDataIdx = a_nDataIdx,
			m_oScroller = a_oSender
		};

		var stCallbackParams = new CEditorScrollerCellView.STCallbackParams() {
			m_oSelCallback = this.OnTouchESCVSelBtn,
			m_oCopyCallback = this.OnTouchESCVCopyBtn,
			m_oMoveCallback = this.OnTouchESCVMoveBtn,
			m_oRemoveCallback = this.OnTouchESCVRemoveBtn
		};

		string oStr = string.Format(oNameFmt, a_nDataIdx + KCDefine.B_VAL_1_INT);

		var oScrollerCellView = a_oSender.GetCellView(oOriginScrollerCellView) as CEditorScrollerCellView;
		oScrollerCellView.Init(stParams, stCallbackParams);
		oScrollerCellView.transform.localScale = Vector3.one;

		CAccess.AssignText(oScrollerCellView.NameText, oStr, false);
		CAccess.AssignColor(oScrollerCellView.SelBtn?.image, stColor);

		return oScrollerCellView;
	}
	#endregion			// 인터페이스

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		
		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			// 레벨 정보가 없을 경우
			if(!CLevelInfoTable.Inst.LevelInfoList.ExIsValid()) {
				var oLevelInfo = Factory.MakeLevelInfo(KCDefine.B_VAL_0_INT);
				CLevelInfoTable.Inst.AddLevelInfo(oLevelInfo);
			}

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
		this.SetupLeftEditorUIs();
		this.SetupRightEditorUIs();
		this.SetupMidEditorUIs();

		var oLevelScrollerCellView = CResManager.Inst.GetRes<GameObject>(KCDefine.E_OBJ_P_EDITOR_SCROLLER_CELL_VIEW);
		var oStageScrollerCellView = CResManager.Inst.GetRes<GameObject>(KCDefine.E_OBJ_P_EDITOR_SCROLLER_CELL_VIEW);
		var oChapterScrollerCellView = CResManager.Inst.GetRes<GameObject>(KCDefine.E_OBJ_P_EDITOR_SCROLLER_CELL_VIEW);

		// 스크롤러 셀 뷰를 설정한다
		m_oOriginLevelScrollerCellView = oLevelScrollerCellView?.GetComponentInChildren<EnhancedScrollerCellView>();
		m_oOriginStageScrollerCellView = oStageScrollerCellView?.GetComponentInChildren<EnhancedScrollerCellView>();
		m_oOriginChapterScrollerCellView = oChapterScrollerCellView?.GetComponentInChildren<EnhancedScrollerCellView>();

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
		CAccess.AssignDelegate(m_oLEUIsLevelScroller, this, false);

		m_oLEUIsStageScroller = m_oLeftEditorUIs.ExFindComponent<EnhancedScroller>(KCDefine.LES_OBJ_N_STAGE_SCROLL_VIEW);
		CAccess.AssignDelegate(m_oLEUIsStageScroller, this, false);

		m_oLEUIsChapterScroller = m_oLeftEditorUIs.ExFindComponent<EnhancedScroller>(KCDefine.LES_OBJ_N_CHAPTER_SCROLL_VIEW);
		CAccess.AssignDelegate(m_oLEUIsChapterScroller, this, false);
		// 스크롤러를 설정한다 }
	}

	//! 오른족 에디터 UI 를 설정한다
	private void SetupRightEditorUIs() {
		// 텍스트를 설정한다
		m_oREUIsTitleText = m_oRightEditorUIs.ExFindComponent<Text>(KCDefine.LES_OBJ_N_RE_UIS_TITLE_TEXT);

		// 입력 필드를 설정한다
		m_oREUIsLevelInput = m_oRightEditorUIs.ExFindComponent<InputField>(KCDefine.LES_OBJ_N_RE_UIS_LEVEL_INPUT);
		m_oREUIsNumCellsXInput = m_oRightEditorUIs.ExFindComponent<InputField>(KCDefine.LES_OBJ_N_RE_UIS_NUM_CELLS_X_INPUT);
		m_oREUIsNumCellsYInput = m_oRightEditorUIs.ExFindComponent<InputField>(KCDefine.LES_OBJ_N_RE_UIS_NUM_CELLS_Y_INPUT);

		// 버튼을 설정한다 {
		var oLoadLevelBtn = m_oRightEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_RE_UIS_LOAD_LEVEL_BTN);
		oLoadLevelBtn?.onClick.AddListener(this.OnTouchREUIsLoadLevelBtn);

		var oApplyNumCellsBtn = m_oRightEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_RE_UIS_APPLY_NUM_CELLS_BTN);
		oApplyNumCellsBtn?.onClick.AddListener(this.OnTouchREUIsApplyNumCellsBtn);
		// 버튼을 설정한다 }
	}

	//! 중앙 에디터 UI 를 설정한다
	private void SetupMidEditorUIs() {
		// 텍스트를 설정한다
		m_oMEUIsLevelText = m_oMidEditorUIs.ExFindComponent<Text>(KCDefine.LES_OBJ_N_ME_UIS_LEVEL_TEXT);

		// 버튼을 설정한다 {
		m_oMEUIsPrevLevelBtn = m_oMidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_PREV_LEVEL_BTN);
		m_oMEUIsPrevLevelBtn?.onClick.AddListener(this.OnTouchMEUIsPrevLevelBtn);

		m_oMEUIsNextLevelBtn = m_oMidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_NEXT_LEVEL_BTN);
		m_oMEUIsNextLevelBtn?.onClick.AddListener(this.OnTouchMEUIsNextLevelBtn);

		var oRemoveLevelBtn = m_oMidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_REMOVE_LEVEL_BTN);
		oRemoveLevelBtn?.onClick.AddListener(this.OnTouchMEUIsRemoveLevelBtn);

		var oSaveLevelBtn = m_oMidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_SAVE_LEVEL_BTN);
		oSaveLevelBtn?.onClick.AddListener(this.OnTouchMEUIsSaveLevelBtn);

		var oResetLevelBtn = m_oMidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_RESET_LEVEL_BTN);
		oResetLevelBtn?.onClick.AddListener(this.OnTouchMEUIsResetLevelBtn);

		var oTestLevelBtn = m_oMidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_TEST_LEVEL_BTN);
		oTestLevelBtn?.onClick.AddListener(this.OnTouchMEUIsTestLevelBtn);
		// 버튼을 설정한다 }
	}

	//! UI 상태를 갱신한다
	private void UpdateUIsState() {
		this.UpdateLeftEditorUIsState();
		this.UpdateRightEditorUIsState();
		this.UpdateMidEditorUIsState();
	}

	//! 왼쪽 에디터 UI 상태를 갱신한다
	private void UpdateLeftEditorUIsState() {
		// 스크롤러를 갱신한다
		m_oLEUIsLevelScroller?.ExReloadData(m_oSelLevelInfo.ID, false);
		m_oLEUIsStageScroller?.ExReloadData(m_oSelLevelInfo.StageID, false);
		m_oLEUIsChapterScroller?.ExReloadData(m_oSelLevelInfo.ChapterID, false);
	}

	//! 오른쪽 에디터 UI 상태를 갱신한다
	private void UpdateRightEditorUIsState() {
		// 텍스트를 설정한다 {
		int nNumLevelInfos = CLevelInfoTable.Inst.GetNumStageLevelInfos(m_oSelLevelInfo.StageID, m_oSelLevelInfo.ChapterID);
		string oTitleStr = string.Format(KCDefine.B_TEXT_FMT_LEVEL_PAGE, m_oSelLevelInfo.ID + KCDefine.B_VAL_1_INT, nNumLevelInfos);

		CAccess.AssignText(m_oREUIsTitleText, oTitleStr, false);
		// 텍스트를 설정한다 }
		
		// 입력 필드를 갱신한다 {
		string oLevelStr = string.Format(KCDefine.B_TEXT_FMT_1_DIGITS, m_oSelLevelInfo.ID + KCDefine.B_VAL_1_INT);
		CAccess.AssignText(m_oREUIsLevelInput, oLevelStr, false);

		string oNumCellsXStr = string.Format(KCDefine.B_TEXT_FMT_1_DIGITS, m_oSelLevelInfo.NumCells.x);
		CAccess.AssignText(m_oREUIsNumCellsXInput, oNumCellsXStr, false);

		string oNumCellsYStr = string.Format(KCDefine.B_TEXT_FMT_1_DIGITS, m_oSelLevelInfo.NumCells.y);
		CAccess.AssignText(m_oREUIsNumCellsYInput, oNumCellsYStr, false);
		// 입력 필드를 갱신한다 }
	}

	//! 중앙 에디터 UI 상태를 갱신한다
	private void UpdateMidEditorUIsState() {
		// 텍스트를 갱신한다
		string oStr = string.Format(KCDefine.B_TEXT_FMT_LEVEL, m_oSelLevelInfo.ID + KCDefine.B_VAL_1_INT);
		CAccess.AssignText(m_oMEUIsLevelText, oStr, false);

		// 버튼을 갱신한다
		m_oMEUIsPrevLevelBtn?.ExSetInteractable(CLevelInfoTable.Inst.TryGetLevelInfo(m_oSelLevelInfo.LevelID - KCDefine.B_VAL_1_INT, out CLevelInfo oPrevLevelInfo), false);
		m_oMEUIsNextLevelBtn?.ExSetInteractable(CLevelInfoTable.Inst.TryGetLevelInfo(m_oSelLevelInfo.LevelID + KCDefine.B_VAL_1_INT, out CLevelInfo oNextLevelInfo), false);
	}

	//! 왼쪽 에디터 UI 레벨 추가 버튼을 눌렀을 경우
	private void OnTouchLEUIsAddLevelBtn() {
		int nNumLevelInfos = CLevelInfoTable.Inst.GetNumStageLevelInfos(m_oSelLevelInfo.StageID, m_oSelLevelInfo.ChapterID);
		this.AddLevelInfo(nNumLevelInfos, m_oSelLevelInfo.StageID, m_oSelLevelInfo.ChapterID);
	}

	//! 왼쪽 에디터 UI 스테이지 추가 버튼을 눌렀을 경우
	private void OnTouchLEUIsAddStageBtn() {
		int nNumStageInfos = CLevelInfoTable.Inst.GetNumStageInfos(m_oSelLevelInfo.ChapterID);
		this.AddLevelInfo(KCDefine.B_VAL_0_INT, nNumStageInfos, m_oSelLevelInfo.ChapterID);
	}

	//! 왼쪽 에디터 UI 챕터 추가 버튼을 눌렀을 경우
	private void OnTouchLEUIsAddChapterBtn() {
		int nNumChapterInfos = CLevelInfoTable.Inst.NumChapterInfos;
		this.AddLevelInfo(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, nNumChapterInfos);
	}

	//! 오른쪽 에디터 UI 레벨 로드 버튼을 눌렀을 경우
	private void OnTouchREUIsLoadLevelBtn() {
		// 식별자가 유효 할 경우
		if(int.TryParse(m_oREUIsLevelInput?.text, out int nID)) {
			int nNumLevelInfos = CLevelInfoTable.Inst.GetNumStageLevelInfos(m_oSelLevelInfo.StageID, m_oSelLevelInfo.ChapterID);
			long nLevelID = Factory.MakeUniqueLevelID(Mathf.Clamp(nID, KCDefine.B_VAL_1_INT, nNumLevelInfos) - KCDefine.B_VAL_1_INT, m_oSelLevelInfo.StageID, m_oSelLevelInfo.ChapterID);

			m_oSelLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(nLevelID);
			this.UpdateUIsState();
		}
	}

	//! 오른쪽 에디터 UI 셀 개수 적용 버튼을 눌렀을 경우
	private void OnTouchREUIsApplyNumCellsBtn() {
		bool bIsValid = int.TryParse(m_oREUIsNumCellsXInput?.text, out int nNumCellsX);

		// 셀 개수가 유효 할 경우
		if(bIsValid && int.TryParse(m_oREUIsNumCellsYInput?.text, out int nNumCellsY)) {
			m_oSelLevelInfo.NumCells = new Vector3Int(nNumCellsX, nNumCellsY, KCDefine.B_VAL_0_INT);
			m_oSelLevelInfo.m_oCellInfoListContainer.Clear();

			for(int i = 0; i < nNumCellsY; ++i) {
				var oCellInfoList = new List<CCellInfo>();

				for(int j = 0; j < nNumCellsX; ++j) {
					var oCellInfo = Factory.MakeCellInfo();
					oCellInfoList.Add(oCellInfo);	
				}

				m_oSelLevelInfo.m_oCellInfoListContainer.Add(oCellInfoList);
			}

			this.UpdateUIsState();
		}
	}

	//! 중앙 에디터 UI 이전 레벨 버튼을 눌렀을 경우
	private void OnTouchMEUIsPrevLevelBtn() {
		// 이전 레벨 정보가 존재 할 경우
		if(CLevelInfoTable.Inst.TryGetLevelInfo(m_oSelLevelInfo.LevelID - KCDefine.B_VAL_1_INT, out CLevelInfo oPrevLevelInfo)) {
			m_oSelLevelInfo = oPrevLevelInfo;
			this.UpdateUIsState();
		}
	}

	//! 중앙 에디터 UI 다음 레벨 버튼을 눌렀을 경우
	private void OnTouchMEUIsNextLevelBtn() {
		// 다음 레벨 정보가 존재 할 경우
		if(CLevelInfoTable.Inst.TryGetLevelInfo(m_oSelLevelInfo.LevelID + KCDefine.B_VAL_1_INT, out CLevelInfo oNextLevelInfo)) {
			m_oSelLevelInfo = oNextLevelInfo;
			this.UpdateUIsState();
		}
	}

	//! 중앙 에디터 UI 레벨 제거 버튼을 눌렀을 경우
	private void OnTouchMEUIsRemoveLevelBtn() {
		Func.ShowEditorLevelRemovePopup((a_oSender, a_bIsOK) => {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				this.RemoveLevelInfo(m_oLEUIsLevelScroller, m_oSelLevelInfo.LevelID, m_oSelLevelInfo.ID);
				this.UpdateUIsState();
			}
		});
	}

	//! 중앙 에디터 UI 레벨 저장 버튼을 눌렀을 경우
	private void OnTouchMEUIsSaveLevelBtn() {
		CLevelInfoTable.Inst.SaveLevelInfos();
	}

	//! 중앙 에디터 UI 레벨 리셋 버튼을 눌렀을 경우
	private void OnTouchMEUIsResetLevelBtn() {
		CLevelInfoTable.Inst.LevelInfoList.Clear();
		CLevelInfoTable.Inst.LoadLevelInfos();

		m_oSelLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(KCDefine.B_VAL_0_INT);
		this.UpdateUIsState();
	}

	//! 중앙 에디터 UI 레벨 테스트 버튼을 눌렀을 경우
	private void OnTouchMEUIsTestLevelBtn() {
		CGameInfoStorage.Inst.SetupPlayLevelInfo(m_oSelLevelInfo.LevelID, EPlayMode.TEST);
		CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME);
	}

	//! 에디터 스크롤러 셀 뷰 선택 버튼을 눌렀을 경우
	private void OnTouchESCVSelBtn(CEditorScrollerCellView a_oSender, long a_nID, long a_nDataIdx) {
		m_oSelLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(a_nID);
		this.UpdateUIsState();
	}

	//! 에디터 스크롤러 셀 뷰 복사 버튼을 눌렀을 경우
	private void OnTouchESCVCopyBtn(CEditorScrollerCellView a_oSender, long a_nID, long a_nDataIdx) {
		var oLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(a_nID);

		// 레벨 스크롤러 일 경우
		if(m_oLEUIsLevelScroller == a_oSender.Scroller) {
			var oCloneLevelInfo = oLevelInfo.Clone() as CLevelInfo;
			oCloneLevelInfo.ID = CLevelInfoTable.Inst.GetNumStageLevelInfos(oLevelInfo.StageID, oLevelInfo.ChapterID);
			
			m_oSelLevelInfo = oCloneLevelInfo;
			CLevelInfoTable.Inst.AddLevelInfo(oCloneLevelInfo);
		} else {
			int nNumLevelInfos = (m_oLEUIsStageScroller == a_oSender.Scroller) ? CLevelInfoTable.Inst.GetNumStageInfos(oLevelInfo.ChapterID) : CLevelInfoTable.Inst.NumChapterInfos;
			var oLevelInfoList = (m_oLEUIsStageScroller == a_oSender.Scroller) ? CLevelInfoTable.Inst.GetStageLevelInfos(oLevelInfo.StageID, oLevelInfo.ChapterID) : CLevelInfoTable.Inst.GetChapterLevelInfos(oLevelInfo.ChapterID);

			for(int i = 0; i < oLevelInfoList.Count; ++i) {
				var oCloneLevelInfo = oLevelInfoList[i].Clone() as CLevelInfo;

				// 스테이지 스크롤러 일 경우
				if(m_oLEUIsStageScroller == a_oSender.Scroller) {
					oCloneLevelInfo.StageID = nNumLevelInfos;
				} else {
					oCloneLevelInfo.ChapterID = nNumLevelInfos;
				}

				CLevelInfoTable.Inst.AddLevelInfo(oCloneLevelInfo);
			}

			int nID = KCDefine.B_VAL_0_INT;
			int nStageID = (m_oLEUIsStageScroller == a_oSender.Scroller) ? CLevelInfoTable.Inst.GetNumStageInfos(oLevelInfo.ChapterID) - KCDefine.B_VAL_1_INT : KCDefine.B_VAL_0_INT;
			int nChapterID = (m_oLEUIsChapterScroller == a_oSender.Scroller) ? CLevelInfoTable.Inst.NumChapterInfos - KCDefine.B_VAL_1_INT : oLevelInfo.ChapterID;

			long nLevelID = Factory.MakeUniqueLevelID(nID, nStageID, nChapterID);
			m_oSelLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(nLevelID);
		}

		this.UpdateUIsState();
	}

	//! 에디터 스크롤러 셀 뷰 이동 버튼을 눌렀을 경우
	private void OnTouchESCVMoveBtn(CEditorScrollerCellView a_oSender, long a_nID, long a_nDataIdx) {
		// Do Nothing
	}

	//! 에디터 스크롤러 셀 뷰 제거 버튼을 눌렀을 경우
	private void OnTouchESCVRemoveBtn(CEditorScrollerCellView a_oSender, long a_nID, long a_nDataIdx) {
		System.Action<CAlertPopup, bool> oCallback = (a_oPopupSender, a_bIsOK) => {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				this.RemoveLevelInfo(a_oSender.Scroller, a_nID, a_nDataIdx);
				this.UpdateUIsState();
			}
		};

		// 레벨 스크롤러 일 경우
		if(m_oLEUIsLevelScroller == a_oSender.Scroller) {
			Func.ShowEditorLevelRemovePopup(oCallback);
		}
		// 스테이지 스크롤러 일 경우
		else if(m_oLEUIsStageScroller == a_oSender.Scroller) {
			Func.ShowEditorStageRemovePopup(oCallback);
		} else {
			Func.ShowEditorChapterRemovePopup(oCallback);
		}
	}

	//! 에디터 종료 팝업 결과를 수신했을 경우
	private void OnReceiveEditorQuitPopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
		// 확인 버튼을 눌렀을 경우
		if(a_bIsOK) {
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_TITLE);
		}
	}

	//! 레벨 정보를 반환한다
	private bool TryGetLevelInfo(long a_nPrevID, long a_nNextID, out CLevelInfo a_oOutLevelInfo) {
		CLevelInfoTable.Inst.TryGetLevelInfo(a_nPrevID, out CLevelInfo oPrevLevelInfo);
		CLevelInfoTable.Inst.TryGetLevelInfo(a_nNextID, out CLevelInfo oNextLevelInfo);

		a_oOutLevelInfo = (oPrevLevelInfo != null) ? oPrevLevelInfo : oNextLevelInfo;
		return oPrevLevelInfo != null || oNextLevelInfo != null;
	}

	//! 레벨 정보를 추가한다
	private void AddLevelInfo(int a_nID, int a_nStageID, int a_nChapterID) {
		m_oSelLevelInfo = Factory.MakeLevelInfo(a_nID, a_nStageID, a_nChapterID);
		CLevelInfoTable.Inst.AddLevelInfo(m_oSelLevelInfo);

		this.UpdateUIsState();
	}

	//! 레벨 정보를 제거한다
	private void RemoveLevelInfo(EnhancedScroller a_oScroller, long a_nID, long a_nDataIdx) {
		var oLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(a_nID);

		// 레벨 스크롤러 일 경우
		if(m_oLEUIsLevelScroller == a_oScroller) {
			CLevelInfoTable.Inst.RemoveLevelInfo(a_nID);
		} else {
			// 스테이지 스크롤러 일 경우
			if(m_oLEUIsStageScroller == a_oScroller) {
				CLevelInfoTable.Inst.RemoveStageLevelInfos(oLevelInfo.StageID, oLevelInfo.ChapterID);
			} else {
				CLevelInfoTable.Inst.RemoveChapterLevelInfos(oLevelInfo.ChapterID);
			}
		}
		
		// 레벨 정보가 없을 경우
		if(!CLevelInfoTable.Inst.LevelInfoList.ExIsValid()) {
			m_oSelLevelInfo = Factory.MakeLevelInfo(KCDefine.B_VAL_0_INT);
			CLevelInfoTable.Inst.AddLevelInfo(m_oSelLevelInfo);
		} else {
			CLevelInfo oSelLevelInfo = null;

			// 레벨 스크롤러 일 경우
			if(m_oLEUIsLevelScroller == a_oScroller) {
				this.TryGetLevelInfo(oLevelInfo.LevelID - KCDefine.B_VAL_1_INT, oLevelInfo.LevelID, out oSelLevelInfo);
			}

			// 스테이지 스크롤러 일 경우
			if(oSelLevelInfo == null || m_oLEUIsStageScroller == a_oScroller) {
				long nPrevLevelID = Factory.MakeUniqueLevelID(KCDefine.B_VAL_0_INT, oLevelInfo.StageID - KCDefine.B_VAL_1_INT, oLevelInfo.ChapterID);
				long nNextLevelID = Factory.MakeUniqueLevelID(KCDefine.B_VAL_0_INT, oLevelInfo.StageID, oLevelInfo.ChapterID);

				this.TryGetLevelInfo(nPrevLevelID, nNextLevelID, out oSelLevelInfo);
			}

			// 챕터 스크롤러 일 경우
			if(oSelLevelInfo == null || m_oLEUIsChapterScroller == a_oScroller) {
				long nPrevLevelID = Factory.MakeUniqueLevelID(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, oLevelInfo.ChapterID - KCDefine.B_VAL_1_INT);
				long nNextLevelID = Factory.MakeUniqueLevelID(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, oLevelInfo.ChapterID);

				this.TryGetLevelInfo(nPrevLevelID, nNextLevelID, out oSelLevelInfo);
			}

			m_oSelLevelInfo = oSelLevelInfo;
		}
	}
	#endregion			// 함수
}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
#endif			// #if NEVER_USE_THIS
