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
	private Button m_oAddLevelBtn = null;
	private Button m_oAddStageBtn = null;
	private Button m_oAddChapterBtn = null;

	private InputField m_oNumCellsXInput = null;
	private InputField m_oNumCellsYInput = null;

	private EnhancedScroller m_oLevelScroller = null;
	private EnhancedScroller m_oStageScroller = null;
	private EnhancedScroller m_oChapterScroller = null;
	#endregion			// UI 변수

	#region 객체
	private EnhancedScrollerCellView m_oOriginLevelScrollerCellView = null;
	private EnhancedScrollerCellView m_oOriginStageScrollerCellView = null;
	private EnhancedScrollerCellView m_oOriginChapterScrollerCellView = null;
	#endregion			// 객체

	#region 인터페이스
	//! 셀 개수를 반환한다
	public int GetNumberOfCells(EnhancedScroller a_oSender) {
		// 챕터 스크롤러 일 경우
		if(m_oChapterScroller == a_oSender) {
			return CLevelInfoTable.Inst.NumChapterInfos;
		}

		return (m_oStageScroller == a_oSender) ? CLevelInfoTable.Inst.GetNumStageInfos(m_oSelLevelInfo.ChapterID) : CLevelInfoTable.Inst.GetNumLevelInfos(m_oSelLevelInfo.StageID, m_oSelLevelInfo.ChapterID);
	}

	//! 셀 뷰 크기를 반환한다
	public float GetCellViewSize(EnhancedScroller a_oSender, int a_nDataIdx) {
		// 챕터 스크롤러 일 경우
		if(m_oChapterScroller == a_oSender) {
			return (m_oOriginChapterScrollerCellView.transform as RectTransform).sizeDelta.y;
		}

		return (m_oStageScroller == a_oSender) ? (m_oOriginStageScrollerCellView.transform as RectTransform).sizeDelta.y : (m_oOriginLevelScrollerCellView.transform as RectTransform).sizeDelta.y;
	}

	//! 셀 뷰를 반환한다
	public EnhancedScrollerCellView GetCellView(EnhancedScroller a_oSender, int a_nDataIdx, int a_nCellIdx) {
		long nID = Factory.MakeUniqueChapterID(a_nDataIdx);
		string oNameFmt = KCDefine.B_TEXT_FMT_CHAPTER;
		EnhancedScrollerCellView oOriginScrollerCellView = m_oOriginChapterScrollerCellView;

		// 챕터 스크롤러가 아닐 경우
		if(m_oChapterScroller != a_oSender) {
			nID = (m_oStageScroller == a_oSender) ? Factory.MakeUniqueStageID(a_nDataIdx, m_oSelLevelInfo.ChapterID) : Factory.MakeUniqueLevelID(a_nDataIdx, m_oSelLevelInfo.StageID, m_oSelLevelInfo.ChapterID);
			oNameFmt = (m_oStageScroller == a_oSender) ? KCDefine.B_TEXT_FMT_STAGE : KCDefine.B_TEXT_FMT_LEVEL;
			oOriginScrollerCellView = (m_oStageScroller == a_oSender) ? m_oOriginStageScrollerCellView : m_oOriginLevelScrollerCellView;
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

		var oScrollerCellView = a_oSender.GetCellView(oOriginScrollerCellView) as CEditorScrollerCellView;
		oScrollerCellView.Init(stParams, stCallbackParams);
		oScrollerCellView.NameText.text = string.Format(oNameFmt, a_nDataIdx);

		return oOriginScrollerCellView;
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
		var oLevelScrollerCellView = CResManager.Inst.GetRes<GameObject>(KCDefine.E_OBJ_P_EDITOR_SCROLLER_CELL_VIEW);
		m_oOriginLevelScrollerCellView = oLevelScrollerCellView?.GetComponentInChildren<EnhancedScrollerCellView>();

		var oStageScrollerCellView = CResManager.Inst.GetRes<GameObject>(KCDefine.E_OBJ_P_EDITOR_SCROLLER_CELL_VIEW);
		m_oOriginStageScrollerCellView = oStageScrollerCellView?.GetComponentInChildren<EnhancedScrollerCellView>();

		var oChapterScrollerCellView = CResManager.Inst.GetRes<GameObject>(KCDefine.E_OBJ_P_EDITOR_SCROLLER_CELL_VIEW);
		m_oOriginChapterScrollerCellView = oChapterScrollerCellView?.GetComponentInChildren<EnhancedScrollerCellView>();

		// 레벨 정보를 설정한다 {
		long nLevelID = Factory.MakeUniqueLevelID(KCDefine.B_VAL_0_INT);

		m_oSelLevelInfo = (CGameInfoStorage.Inst.PlayLevelInfo != null) ? CGameInfoStorage.Inst.PlayLevelInfo : CLevelInfoTable.Inst.GetLevelInfo(nLevelID);
		m_stSelGridInfo = SampleEngineName.Factory.MakeGridInfo(m_oSelLevelInfo);
		// 레벨 정보를 설정한다 }

		// 버튼을 설정한다 {
		m_oAddLevelBtn = this.SubUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ADD_LEVEL_BTN);
		m_oAddLevelBtn?.onClick.AddListener(() => this.OnTouchAddBtn(m_oAddLevelBtn));

		m_oAddStageBtn = this.SubUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ADD_STAGE_BTN);
		m_oAddStageBtn?.onClick.AddListener(() => this.OnTouchAddBtn(m_oAddStageBtn));

		m_oAddChapterBtn = this.SubUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ADD_CHAPTER_BTN);
		m_oAddChapterBtn?.onClick.AddListener(() => this.OnTouchAddBtn(m_oAddChapterBtn));

		var oApplyNumCellsBtn = this.SubUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_APPLY_NUM_CELLS_BTN);
		oApplyNumCellsBtn?.onClick.AddListener(this.OnTouchApplyNumCellsBtn);
		// 버튼을 설정한다 }

		// 입력 필드를 설정한다
		m_oNumCellsXInput = this.SubUIs.ExFindComponent<InputField>(KCDefine.LES_OBJ_N_NUM_CELLS_X_INPUT);
		m_oNumCellsYInput = this.SubUIs.ExFindComponent<InputField>(KCDefine.LES_OBJ_N_NUM_CELLS_Y_INPUT);

		// 스크롤러를 설정한다 {
		m_oLevelScroller = this.SubUIs.ExFindComponent<EnhancedScroller>(KCDefine.LES_OBJ_N_LEVEL_SCROLL_VIEW);
		CAccess.AssignDelegate(m_oLevelScroller, this);

		m_oStageScroller = this.SubUIs.ExFindComponent<EnhancedScroller>(KCDefine.LES_OBJ_N_STAGE_SCROLL_VIEW);
		CAccess.AssignDelegate(m_oStageScroller, this);

		m_oChapterScroller = this.SubUIs.ExFindComponent<EnhancedScroller>(KCDefine.LES_OBJ_N_CHAPTER_SCROLL_VIEW);
		CAccess.AssignDelegate(m_oChapterScroller, this);
		// 스크롤러를 설정한다 }
	}

	//! 씬을 설정한다
	private void SetupStart() {
		// Do Nothing
	}

	//! UI 상태를 갱신한다
	private void UpdateUIsState() {
		m_oLevelScroller?.ReloadData();
		m_oStageScroller?.ReloadData();
		m_oChapterScroller?.ReloadData();
	}

	//! 추가 버튼을 눌렀을 경우
	private void OnTouchAddBtn(Button a_oSender) {
		int nID = (m_oAddLevelBtn == a_oSender) ? CLevelInfoTable.Inst.GetNumLevelInfos(m_oSelLevelInfo.StageID, m_oSelLevelInfo.ChapterID) : KCDefine.B_VAL_0_INT;
		int nStageID = (m_oAddStageBtn == a_oSender) ? CLevelInfoTable.Inst.GetNumStageInfos(m_oSelLevelInfo.ChapterID) : KCDefine.B_VAL_0_INT;
		int nChapterID = (m_oAddChapterBtn = a_oSender) ? CLevelInfoTable.Inst.NumChapterInfos : KCDefine.B_VAL_0_INT;

		m_oSelLevelInfo = Factory.MakeLevelInfo(nID, nStageID, nChapterID);
		CLevelInfoTable.Inst.AddLevelInfo(m_oSelLevelInfo);

		this.UpdateUIsState();
	}

	//! 셀 개수 적용 버튼을 눌렀을 경우
	private void OnTouchApplyNumCellsBtn() {
		bool bIsValid = int.TryParse(m_oNumCellsXInput?.text, out int nNumCellsX);

		// 셀 개수가 유효 할 경우
		if(bIsValid && int.TryParse(m_oNumCellsYInput?.text, out int nNumCellsY)) {
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

	//! 에디터 스크롤러 셀 뷰 선택 버튼을 눌렀을 경우
	private void OnTouchESCVSelBtn(CEditorScrollerCellView a_oSender, long a_nID, long a_nDataIdx) {
		m_oSelLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(a_nID);
		this.UpdateUIsState();
	}

	//! 에디터 스크롤러 셀 뷰 복사 버튼을 눌렀을 경우
	private void OnTouchESCVCopyBtn(CEditorScrollerCellView a_oSender, long a_nID, long a_nDataIdx) {
		var oLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(a_nID);

		// 레벨 스크롤러 일 경우
		if(m_oLevelScroller == a_oSender.Scroller) {
			var oCloneLevelInfo = oLevelInfo.Clone() as CLevelInfo;
			oCloneLevelInfo.ID = CLevelInfoTable.Inst.GetNumLevelInfos(oLevelInfo.StageID, oLevelInfo.ChapterID);

			CLevelInfoTable.Inst.AddLevelInfo(oCloneLevelInfo);
		} else {
			int nNumLevelInfos = (m_oStageScroller == a_oSender.Scroller) ? CLevelInfoTable.Inst.GetNumStageInfos(oLevelInfo.ChapterID) : CLevelInfoTable.Inst.NumChapterInfos;
			var oLevelInfoList = (m_oStageScroller == a_oSender.Scroller) ? CLevelInfoTable.Inst.GetStageLevelInfos(oLevelInfo.StageID, oLevelInfo.ChapterID) : CLevelInfoTable.Inst.GetChapterLevelInfos(oLevelInfo.ChapterID);

			for(int i = 0; i < oLevelInfoList.Count; ++i) {
				var oCloneLevelInfo = oLevelInfoList[i].Clone() as CLevelInfo;

				// 스테이지 스크롤러 일 경우
				if(m_oStageScroller == a_oSender.Scroller) {
					oCloneLevelInfo.StageID = nNumLevelInfos;
				} else {
					oCloneLevelInfo.ChapterID = nNumLevelInfos;
				}

				CLevelInfoTable.Inst.AddLevelInfo(oCloneLevelInfo);
			}
		}

		this.UpdateUIsState();
	}

	//! 에디터 스크롤러 셀 뷰 이동 버튼을 눌렀을 경우
	private void OnTouchESCVMoveBtn(CEditorScrollerCellView a_oSender, long a_nID, long a_nDataIdx) {
		// Do Nothing
	}

	//! 에디터 스크롤러 셀 뷰 제거 버튼을 눌렀을 경우
	private void OnTouchESCVRemoveBtn(CEditorScrollerCellView a_oSender, long a_nID, long a_nDataIdx) {
		var oLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(a_nID);

		// 레벨 스크롤러 일 경우
		if(m_oLevelScroller == a_oSender.Scroller) {
			CLevelInfoTable.Inst.RemoveLevelInfo(oLevelInfo);
		} else {
			var oLevelInfoList = (m_oStageScroller == a_oSender.Scroller) ? CLevelInfoTable.Inst.GetStageLevelInfos(oLevelInfo.StageID, oLevelInfo.ChapterID) : CLevelInfoTable.Inst.GetChapterLevelInfos(oLevelInfo.ChapterID);

			for(int i = 0; i < oLevelInfoList.Count; ++i) {
				CLevelInfoTable.Inst.RemoveLevelInfo(oLevelInfoList[i]);
			}
		}

		this.UpdateUIsState();
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
