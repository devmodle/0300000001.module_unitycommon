using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using EnhancedUI.EnhancedScroller;
using DanielLochner.Assets.SimpleScrollSnap;

#if SCRIPT_TEMPLATE_ONLY
#if INPUT_SYSTEM_MODULE_ENABLE
using UnityEngine.InputSystem;
#endif			// #if INPUT_SYSTEM_MODULE_ENABLE

#if UNITY_STANDALONE && EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
/** 서브 레벨 에디터 씬 관리자 */
public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
	/** 입력 */
	private enum EInput {
		NONE = -1,
		MOVE_LEVEL,
		REMOVE_LEVEL,
		[HideInInspector] MAX_VAL
	}

	/** 스크롤러 */
	private enum EScroller {
		NONE = -1,
		LEVEL,
		STAGE,
		CHAPTER,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	private EInput m_eInput = EInput.NONE;
	private EUserType m_eSelUserType = EUserType.NONE;

	private SpriteRenderer m_oSelBlockSprite = null;
	private CTouchDispatcher m_oBGTouchDispatcher = null;

#if ENGINE_TEMPLATES_MODULE_ENABLE
	private SampleEngineName.STGridInfo m_stSelGridInfo;
	private Dictionary<EBlockKinds, SpriteRenderer>[,] m_oBlockSpriteDicts = null;
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE

#if RUNTIME_TEMPLATES_MODULE_ENABLE
	[System.NonSerialized] private CLevelInfo m_oSelLevelInfo = null;
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE

	/** =====> UI <===== */
	private EnhancedScroller m_oSelScroller = null;

	// 중앙 에디터 UI {
	private Text m_oMEUIsMsgText = null;
	private Text m_oMEUIsLevelText = null;

	private Button m_oMEUIsPrevBtn = null;
	private Button m_oMEUIsNextBtn = null;

	private Button m_oMEUIsMoveLevelBtn = null;
	private Button m_oMEUIsRemoveLevelBtn = null;
	// 중앙 에디터 UI }

	// 왼쪽 에디터 UI {
	private Button m_oLEUIsASetBtn = null;
	private Button m_oLEUIsBSetBtn = null;

	private Dictionary<EScroller, EnhancedScroller> m_oLEUIsScrollerDict = new Dictionary<EScroller, EnhancedScroller>();
	private Dictionary<EScroller, EnhancedScrollerCellView> m_oLEUIsOriginScrollerCellViewDict = new Dictionary<EScroller, EnhancedScrollerCellView>();
	// 왼쪽 에디터 UI }
	
	// 오른쪽 에디터 UI {
	private Text m_oREUIsPageText = null;
	private Text m_oREUIsTitleText = null;

	private Button m_oREUIsPrevBtn = null;
	private Button m_oREUIsNextBtn = null;
	private Button m_oREUIsRemoveLevelBtn = null;

	private InputField m_oREUIsLevelInput = null;
	private InputField m_oREUIsNumCellsXInput = null;
	private InputField m_oREUIsNumCellsYInput = null;

	private SimpleScrollSnap m_oREUIsScrollSnap = null;
	// 오른쪽 에디터 UI }
	#endregion			// 변수

	#region 추가 변수

	#endregion			// 추가 변수

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region IEnhancedScrollerDelegate
	/** 셀 개수를 반환한다 */
	public virtual int GetNumberOfCells(EnhancedScroller a_oSender) {
#if RUNTIME_TEMPLATES_MODULE_ENABLE
		// 레벨 스크롤러 일 경우
		if(m_oLEUIsScrollerDict[EScroller.LEVEL] == a_oSender) {
			return CLevelInfoTable.Inst.GetNumLevelInfos(m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);
		}

		return (m_oLEUIsScrollerDict[EScroller.STAGE] == a_oSender) ? CLevelInfoTable.Inst.GetNumStageInfos(m_oSelLevelInfo.m_stIDInfo.m_nChapterID) : CLevelInfoTable.Inst.NumChapterInfos;
#else
		return KCDefine.B_VAL_0_INT;
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
	}

	/** 셀 뷰 크기를 반환한다 */
	public virtual float GetCellViewSize(EnhancedScroller a_oSender, int a_nDataIdx) {
		// 레벨 스크롤러 일 경우
		if(m_oLEUIsScrollerDict[EScroller.LEVEL] == a_oSender) {
			return (m_oLEUIsOriginScrollerCellViewDict[EScroller.LEVEL].transform as RectTransform).sizeDelta.y;
		}

		return (m_oLEUIsScrollerDict[EScroller.STAGE] == a_oSender) ? (m_oLEUIsOriginScrollerCellViewDict[EScroller.STAGE].transform as RectTransform).sizeDelta.y : (m_oLEUIsOriginScrollerCellViewDict[EScroller.CHAPTER].transform as RectTransform).sizeDelta.y;
	}

	/** 셀 뷰를 반환한다 */
	public virtual EnhancedScrollerCellView GetCellView(EnhancedScroller a_oSender, int a_nDataIdx, int a_nCellIdx) {
#if RUNTIME_TEMPLATES_MODULE_ENABLE
		int nNumInfos = CLevelInfoTable.Inst.GetNumLevelInfos(m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);

		string oNameFmt = KCDefine.B_TEXT_FMT_LEVEL;
		string oNumInfosStr = string.Empty;

		var stColor = (m_oSelLevelInfo.m_stIDInfo.m_nID == a_nDataIdx) ? KCDefine.U_COLOR_NORM : KCDefine.U_COLOR_DISABLE;
		var stIDInfo = CFactory.MakeIDInfo(a_nDataIdx, m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);
		var oOriginScrollerCellView = m_oLEUIsOriginScrollerCellViewDict[EScroller.LEVEL];

		// 스테이지 스크롤러 일 경우
		if(m_oLEUIsScrollerDict[EScroller.STAGE] == a_oSender) {
			nNumInfos = CLevelInfoTable.Inst.GetNumStageInfos(m_oSelLevelInfo.m_stIDInfo.m_nChapterID);

			oNameFmt = KCDefine.B_TEXT_FMT_STAGE;
			oNumInfosStr = string.Format(KCDefine.B_TEXT_FMT_BRACKET, CLevelInfoTable.Inst.GetNumLevelInfos(a_nDataIdx, m_oSelLevelInfo.m_stIDInfo.m_nChapterID));

			stColor = (m_oSelLevelInfo.m_stIDInfo.m_nStageID == a_nDataIdx) ? KCDefine.U_COLOR_NORM : KCDefine.U_COLOR_DISABLE;
			stIDInfo = CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, a_nDataIdx, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);
			oOriginScrollerCellView = m_oLEUIsOriginScrollerCellViewDict[EScroller.STAGE];
		}
		// 챕터 스크롤러 일 경우
		else if(m_oLEUIsScrollerDict[EScroller.CHAPTER] == a_oSender) {
			nNumInfos = CLevelInfoTable.Inst.NumChapterInfos;

			oNameFmt = KCDefine.B_TEXT_FMT_CHAPTER;
			oNumInfosStr = string.Format(KCDefine.B_TEXT_FMT_BRACKET, CLevelInfoTable.Inst.GetNumStageInfos(a_nDataIdx));

			stColor = (m_oSelLevelInfo.m_stIDInfo.m_nChapterID == a_nDataIdx) ? KCDefine.U_COLOR_NORM : KCDefine.U_COLOR_DISABLE;
			stIDInfo = CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, a_nDataIdx);
			oOriginScrollerCellView = m_oLEUIsOriginScrollerCellViewDict[EScroller.CHAPTER];
		}

		var stParams = new CEditorScrollerCellView.STParams() {
			m_stBaseParams = new CScrollerCellView.STParams() {
				m_nID = CFactory.MakeUniqueLevelID(stIDInfo.m_nID, stIDInfo.m_nStageID, stIDInfo.m_nChapterID),
				m_oScroller = a_oSender,

				m_oCallbackDict = new Dictionary<CScrollerCellView.ECallback, System.Action<CScrollerCellView, long>>() {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
					[CScrollerCellView.ECallback.SEL] = this.OnTouchSCVSelBtn
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
				}
			},

			m_oCallbackDict = new Dictionary<CEditorScrollerCellView.ECallback, System.Action<CEditorScrollerCellView, long>>() {
				[CEditorScrollerCellView.ECallback.COPY] = this.OnTouchSCVCopyBtn,
				[CEditorScrollerCellView.ECallback.MOVE] = this.OnTouchSCVMoveBtn,
				[CEditorScrollerCellView.ECallback.REMOVE] = this.OnTouchSCVRemoveBtn
			}
		};

		string oName = string.Format(oNameFmt, a_nDataIdx + KCDefine.B_VAL_1_INT);
		string oScrollerCellViewName = string.Format(KCDefine.B_TEXT_FMT_2_SPACE_COMBINE, oName, oNumInfosStr);

		var oScrollerCellView = a_oSender.GetCellView(oOriginScrollerCellView) as CEditorScrollerCellView;
		oScrollerCellView.Init(stParams);
		oScrollerCellView.transform.localScale = Vector3.one;

		oScrollerCellView.MoveBtn?.ExSetInteractable(nNumInfos > KCDefine.B_VAL_1_INT, false);
		oScrollerCellView.RemoveBtn?.ExSetInteractable(nNumInfos > KCDefine.B_VAL_1_INT, false);

		oScrollerCellView.NameText?.ExSetText<Text>(oScrollerCellViewName, false);
		oScrollerCellView.SelBtn?.image.ExSetColor<Image>(stColor, false);

		return oScrollerCellView;
#else
		return null;
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
	}
	#endregion			// IEnhancedScrollerDelegate

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		
		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
#if UNITY_STANDALONE && (ENGINE_TEMPLATES_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE)
			// 레벨 정보가 없을 경우
			if(!CLevelInfoTable.Inst.LevelInfoDictContainer.ExIsValid()) {
				var oLevelInfo = Factory.MakeLevelInfo(KCDefine.B_VAL_0_INT);

				Func.SetupEditorLevelInfo(oLevelInfo, new CSubEditorLevelCreateInfo() {
					m_nNumLevels = KCDefine.B_VAL_0_INT, m_stMinNumCells = SampleEngineName.KDefine.E_MIN_NUM_CELLS, m_stMaxNumCells = SampleEngineName.KDefine.E_MIN_NUM_CELLS
				});

				CLevelInfoTable.Inst.AddLevelInfo(oLevelInfo);
				CLevelInfoTable.Inst.SaveLevelInfos();
			}
#endif			// #if UNITY_STANDALONE && (ENGINE_TEMPLATES_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE)

			this.SetupAwake();
		}
	}
	
	/** 초기화 */
	public override void Start() {
		base.Start();

		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
#if UNITY_STANDALONE && RUNTIME_TEMPLATES_MODULE_ENABLE
			this.ExLateCallFunc((a_oSender) => this.UpdateUIsState(), KCDefine.U_DELAY_INIT);
#endif			// #if UNITY_STANDALONE && RUNTIME_TEMPLATES_MODULE_ENABLE

			this.SetupStart();
		}
	}

	/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
#if INPUT_SYSTEM_MODULE_ENABLE
				// 이전 키를 눌렀을 경우
				if(Keyboard.current.leftShiftKey.isPressed && Keyboard.current.leftArrowKey.wasPressedThisFrame) {
					this.OnTouchMEUIsPrevBtn();
				}
				// 다음 키를 눌렀을 경우
				else if(Keyboard.current.leftShiftKey.isPressed && Keyboard.current.rightArrowKey.wasPressedThisFrame) {
					this.OnTouchMEUIsNextBtn();
				}
#else
				// 이전 키를 눌렀을 경우
				if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.LeftArrow)) {
					this.OnTouchMEUIsPrevBtn();
				}
				// 다음 키를 눌렀을 경우
				else if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.RightArrow)) {
					this.OnTouchMEUIsNextBtn();
				}
#endif			// #if INPUT_SYSTEM_MODULE_ENABLE
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
			CFunc.ShowLogWarning($"CSubLevelEditorSceneManager.OnDestroy Exception: {oException.Message}");
		}
	}

	/** 내비게이션 스택 이벤트를 수신했을 경우 */
	public override void OnReceiveNavStackEvent(ENavStackEvent a_eEvent) {
		base.OnReceiveNavStackEvent(a_eEvent);

		// 백 키 눌림 이벤트 일 경우
		if(a_eEvent == ENavStackEvent.BACK_KEY_DOWN) {
#if RUNTIME_TEMPLATES_MODULE_ENABLE
			Func.ShowEditorQuitPopup(this.OnReceiveEditorQuitPopupResult);
#else
			this.OnReceiveEditorQuitPopupResult(null, true);
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
		}
	}

	/** 씬을 설정한다 */
	private void SetupAwake() {
		// 스크롤 뷰를 설정한다 {
		var oLevelScrollerCellView = CResManager.Inst.GetRes<GameObject>(KCDefine.E_OBJ_P_LEVEL_EDITOR_SCROLLER_CELL_VIEW);
		var oStageScrollerCellView = CResManager.Inst.GetRes<GameObject>(KCDefine.E_OBJ_P_STAGE_EDITOR_SCROLLER_CELL_VIEW);
		var oChapterScrollerCellView = CResManager.Inst.GetRes<GameObject>(KCDefine.E_OBJ_P_CHAPTER_EDITOR_SCROLLER_CELL_VIEW);

		m_oLEUIsOriginScrollerCellViewDict.Add(EScroller.LEVEL, oLevelScrollerCellView?.GetComponentInChildren<EnhancedScrollerCellView>());
		m_oLEUIsOriginScrollerCellViewDict.Add(EScroller.STAGE, oStageScrollerCellView?.GetComponentInChildren<EnhancedScrollerCellView>());
		m_oLEUIsOriginScrollerCellViewDict.Add(EScroller.CHAPTER, oChapterScrollerCellView?.GetComponentInChildren<EnhancedScrollerCellView>());
		// 스크롤 뷰를 설정한다 }

#if UNITY_STANDALONE && RUNTIME_TEMPLATES_MODULE_ENABLE
		this.SetupMidEditorUIs();
		this.SetupLeftEditorUIs();
		this.SetupRightEditorUIs();

		// 레벨 정보를 설정한다
		m_oSelLevelInfo = CGameInfoStorage.Inst.PlayLevelInfo ?? CLevelInfoTable.Inst.GetLevelInfo(KCDefine.B_VAL_0_INT);

		// 터치 전달자를 설정한다
		m_oBGTouchDispatcher = m_oBGTouchResponder?.GetComponentInChildren<CTouchDispatcher>();
		m_oBGTouchDispatcher?.ExSetBeginCallback(this.OnTouchBegin, false);
		m_oBGTouchDispatcher?.ExSetMoveCallback(this.OnTouchMove, false);
		m_oBGTouchDispatcher?.ExSetEndCallback(this.OnTouchEnd, false);
#endif			// #if UNITY_STANDALONE && RUNTIME_TEMPLATES_MODULE_ENABLE
	}

	/** 씬을 설정한다 */
	private void SetupStart() {
		// Do Something
	}

	/** 에디터 종료 팝업 결과를 수신했을 경우 */
	private void OnReceiveEditorQuitPopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
		// 확인 버튼을 눌렀을 경우
		if(a_bIsOK) {
#if UNITY_STANDALONE && RUNTIME_TEMPLATES_MODULE_ENABLE
			CLevelInfoTable.Inst.SaveLevelInfos();
#endif			// #if UNITY_STANDALONE && RUNTIME_TEMPLATES_MODULE_ENABLE

#if STUDY_MODULE_ENABLE
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MENU);
#else
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_TITLE);
#endif			// #if STUDY_MODULE_ENABLE
		}
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_STANDALONE && RUNTIME_TEMPLATES_MODULE_ENABLE
	/** 블럭 스프라이트를 리셋한다 */
	private void ResetBlockSprites() {
#if ENGINE_TEMPLATES_MODULE_ENABLE
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
		m_oBlockSpriteDicts = new Dictionary<EBlockKinds, SpriteRenderer>[m_oSelLevelInfo.NumCells.y, m_oSelLevelInfo.NumCells.x];

		for(int i = 0; i < m_oSelLevelInfo.m_oCellInfoDictContainer.Count; ++i) {
			for(int j = 0; j < m_oSelLevelInfo.m_oCellInfoDictContainer[i].Count; ++j) {
				var stIdx = m_oSelLevelInfo.m_oCellInfoDictContainer[i][j].m_stIdx;
				var oBlockSpriteDict = new Dictionary<EBlockKinds, SpriteRenderer>();

				for(int k = 0; k < m_oSelLevelInfo.m_oCellInfoDictContainer[i][j].m_oBlockKindsList.Count; ++k) {
					var oBlockSprite = Factory.CreateBlockSprite(m_oSelLevelInfo.m_oCellInfoDictContainer[i][j].m_oBlockKindsList[k], m_oBlockObjs);
					oBlockSprite.transform.localPosition = m_stSelGridInfo.m_stGridPivotPos + stIdx.ExToPos(SampleEngineName.KDefine.E_OFFSET_CELL, SampleEngineName.KDefine.E_SIZE_CELL);

					oBlockSpriteDict.TryAdd(m_oSelLevelInfo.m_oCellInfoDictContainer[i][j].m_oBlockKindsList[k], oBlockSprite);
				}
				
				m_oBlockSpriteDicts[stIdx.y, stIdx.x] = oBlockSpriteDict;
			}
		}
		// 블럭 스프라이트를 설정한다 }
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		this.ResetBlockSprites();

		this.UpdateMidEditorUIsState();
		this.UpdateLeftEditorUIsState();
		this.UpdateRightEditorUIsState();
	}

	/** 에디터 리셋 팝업 결과를 수신했을 경우 */
	private void OnReceiveEditorResetPopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
		// 확인 버튼을 눌렀을 경우
		if(a_bIsOK) {
			CLevelInfoTable.Inst.LevelInfoDictContainer.Clear();
			CLevelInfoTable.Inst.LoadLevelInfos();

			// 레벨 정보가 없을 경우
			if(!CLevelInfoTable.Inst.LevelInfoDictContainer.ExIsValid()) {
				var oLevelInfo = Factory.MakeLevelInfo(KCDefine.B_VAL_0_INT);
				CLevelInfoTable.Inst.AddLevelInfo(oLevelInfo);

				Func.SetupEditorLevelInfo(oLevelInfo, new CSubEditorLevelCreateInfo() {
					m_nNumLevels = KCDefine.B_VAL_0_INT, m_stMinNumCells = SampleEngineName.KDefine.E_MIN_NUM_CELLS, m_stMaxNumCells = SampleEngineName.KDefine.E_MIN_NUM_CELLS
				});
			}
			
			m_oSelLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(KCDefine.B_VAL_0_INT);
			this.UpdateUIsState();
		}
	}

	/** 에디터 세트 팝업 결과를 수신했을 경우 */
	private void OnReceiveEditorSetPopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
		// 확인 버튼을 눌렀을 경우
		if(a_bIsOK) {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
			CCommonUserInfoStorage.Inst.UserInfo.UserType = m_eSelUserType;
			CCommonUserInfoStorage.Inst.SaveUserInfo();
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE

			this.OnReceiveEditorResetPopupResult(null, true);
			this.OnReceiveEditorTableReloadPopupResult(null, true);
		}
	}

	/** 에디터 세트 테이블 다시 로드 팝업 결과를 수신했을 경우 */
	private void OnReceiveEditorTableReloadPopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
		// 확인 버튼을 눌렀을 경우
		if(a_bIsOK) {
			CEpisodeInfoTable.Inst.LevelInfoDict.Clear();
			CEpisodeInfoTable.Inst.StageInfoDict.Clear();
			CEpisodeInfoTable.Inst.ChapterInfoDict.Clear();

			CEpisodeInfoTable.Inst.LoadEpisodeInfos();
			this.UpdateUIsState();
		}
	}

	/** 에디터 제거 팝업 결과를 수신했을 경우 */
	private void OnReceiveEditorRemovePopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
		// 확인 버튼을 눌렀을 경우
		if(a_bIsOK) {
			this.RemoveLevelInfos(m_oSelScroller, m_oSelLevelInfo.m_stIDInfo);
			this.UpdateUIsState();
		}
	}

	/** 에디터 입력 팝업 결과를 수신했을 경우 */
	private void OnReceiveEditorInputPopupResult(CEditorInputPopup a_oSender, string a_oStr, bool a_bIsOK) {
		// 확인 버튼을 눌렀을 경우
		if(a_bIsOK) {
			switch(m_eInput) {
				case EInput.MOVE_LEVEL: this.HandleMoveLevelInputPopupResult(a_oStr); break;
				case EInput.REMOVE_LEVEL: this.HandleRemoveLevelInputPopupResult(a_oStr); break;
			}
		}

		this.UpdateUIsState();
	}

	/** 에디터 레벨 생성 팝업 결과를 수신했을 경우 */
	private void OnReceiveEditorLevelCreatePopupResult(CEditorLevelCreatePopup a_oSender, CEditorLevelCreateInfo a_oCreateInfo, bool a_bIsOK) {
		// 확인 버튼을 눌렀을 경우
		if(a_bIsOK) {
#if ENGINE_TEMPLATES_MODULE_ENABLE
			int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);
			int nNumCreateLevelInfos = (nNumLevelInfos + a_oCreateInfo.m_nNumLevels < KCDefine.U_MAX_NUM_LEVEL_INFOS) ? a_oCreateInfo.m_nNumLevels : KCDefine.U_MAX_NUM_LEVEL_INFOS - nNumLevelInfos;

			for(int i = 0; i < nNumCreateLevelInfos; ++i) {
				var oLevelInfo = Factory.MakeLevelInfo(i + nNumLevelInfos, m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);
				m_oSelLevelInfo = oLevelInfo;

				CLevelInfoTable.Inst.AddLevelInfo(oLevelInfo);
				Func.SetupEditorLevelInfo(oLevelInfo, a_oCreateInfo);
			}

			this.UpdateUIsState();
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
		}
	}

	/** 터치를 시작했을 경우 */
	private void OnTouchBegin(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
		// 배경 터치 전달자 일 경우
		if(m_oBGTouchDispatcher == a_oSender) {
			// Do Something
		}
	}

	/** 터치를 움직였을 경우 */
	private void OnTouchMove(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
		// 배경 터치 전달자 일 경우
		if(m_oBGTouchDispatcher == a_oSender) {
			// Do Something
		}
	}

	/** 터치를 종료했을 경우 */
	private void OnTouchEnd(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
		// 배경 터치 전달자 일 경우
		if(m_oBGTouchDispatcher == a_oSender) {
			// Do Something
		}
	}

	/** 레벨 정보를 반환한다 */
	private bool TryGetLevelInfo(STIDInfo a_stPrevIDInfo, STIDInfo a_stNextIDInfo, out CLevelInfo a_oOutLevelInfo) {
		CLevelInfoTable.Inst.TryGetLevelInfo(a_stPrevIDInfo.m_nID, out CLevelInfo oPrevLevelInfo, a_stPrevIDInfo.m_nStageID, a_stPrevIDInfo.m_nChapterID);
		CLevelInfoTable.Inst.TryGetLevelInfo(a_stNextIDInfo.m_nID, out CLevelInfo oNextLevelInfo, a_stNextIDInfo.m_nStageID, a_stNextIDInfo.m_nChapterID);

		a_oOutLevelInfo = oPrevLevelInfo ?? oNextLevelInfo;
		return oPrevLevelInfo != null || oNextLevelInfo != null;
	}

	/** 레벨 정보를 추가한다 */
	private void AddLevelInfo(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
#if ENGINE_TEMPLATES_MODULE_ENABLE
		m_oSelLevelInfo = Factory.MakeLevelInfo(a_nID, a_nStageID, a_nChapterID);
		CLevelInfoTable.Inst.AddLevelInfo(m_oSelLevelInfo);

		Func.SetupEditorLevelInfo(m_oSelLevelInfo, new CSubEditorLevelCreateInfo() {
			m_nNumLevels = KCDefine.B_VAL_0_INT, m_stMinNumCells = SampleEngineName.KDefine.E_MIN_NUM_CELLS, m_stMaxNumCells = SampleEngineName.KDefine.E_MIN_NUM_CELLS
		});

		this.UpdateUIsState();
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
	}
	
	/** 레벨 정보를 제거한다 */
	private void RemoveLevelInfos(EnhancedScroller a_oScroller, STIDInfo a_stIDInfo) {
		var oLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(a_stIDInfo.m_nID, a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);

		// 레벨 스크롤러 일 경우
		if(m_oLEUIsScrollerDict[EScroller.LEVEL] == a_oScroller) {
			CLevelInfoTable.Inst.RemoveLevelInfo(a_stIDInfo.m_nID, a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);
		}
		// 스테이지 스크롤러 일 경우
		else if(m_oLEUIsScrollerDict[EScroller.STAGE] == a_oScroller) {
			CLevelInfoTable.Inst.RemoveStageLevelInfos(a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);
		}
		// 챕터 스크롤러 일 경우
		else if(m_oLEUIsScrollerDict[EScroller.CHAPTER] == a_oScroller) {
			CLevelInfoTable.Inst.RemoveChapterLevelInfos(a_stIDInfo.m_nChapterID);
		}
		
		// 레벨 정보가 없을 경우
		if(!CLevelInfoTable.Inst.LevelInfoDictContainer.ExIsValid()) {
			m_oSelLevelInfo = Factory.MakeLevelInfo(KCDefine.B_VAL_0_INT);
			CLevelInfoTable.Inst.AddLevelInfo(m_oSelLevelInfo);

			Func.SetupEditorLevelInfo(m_oSelLevelInfo, new CSubEditorLevelCreateInfo() {
				m_nNumLevels = KCDefine.B_VAL_0_INT, m_stMinNumCells = SampleEngineName.KDefine.E_MIN_NUM_CELLS, m_stMaxNumCells = SampleEngineName.KDefine.E_MIN_NUM_CELLS
			});
		} else {
			CLevelInfo oSelLevelInfo = null;

			// 레벨 스크롤러 일 경우
			if(m_oLEUIsScrollerDict[EScroller.LEVEL] == a_oScroller) {
				var stPrevIDInfo = CFactory.MakeIDInfo(a_stIDInfo.m_nID - KCDefine.B_VAL_1_INT, a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);
				var stNextIDInfo = CFactory.MakeIDInfo(a_stIDInfo.m_nID, a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);

				this.TryGetLevelInfo(stPrevIDInfo, stNextIDInfo, out oSelLevelInfo);
			}

			// 스테이지 스크롤러 일 경우
			if(oSelLevelInfo == null || m_oLEUIsScrollerDict[EScroller.STAGE] == a_oScroller) {
				var stPrevIDInfo = CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, a_stIDInfo.m_nStageID - KCDefine.B_VAL_1_INT, a_stIDInfo.m_nChapterID);
				var stNextIDInfo = CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);

				this.TryGetLevelInfo(stPrevIDInfo, stNextIDInfo, out oSelLevelInfo);
			}

			// 챕터 스크롤러 일 경우
			if(oSelLevelInfo == null || m_oLEUIsScrollerDict[EScroller.CHAPTER] == a_oScroller) {
				var stPrevIDInfo = CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, a_stIDInfo.m_nChapterID - KCDefine.B_VAL_1_INT);
				var stNextIDInfo = CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, a_stIDInfo.m_nChapterID);

				this.TryGetLevelInfo(stPrevIDInfo, stNextIDInfo, out oSelLevelInfo);
			}

			m_oSelLevelInfo = oSelLevelInfo;
		}

		this.UpdateUIsState();
	}

	/** 레벨 정보를 복사한다 */
	private void CopyLevelInfos(EnhancedScroller a_oScroller, STIDInfo a_stIDInfo) {
		// 레벨 스크롤러 일 경우
		if(m_oLEUIsScrollerDict[EScroller.LEVEL] == a_oScroller) {
			var oLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(a_stIDInfo.m_nID, a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);

			var oCloneLevelInfo = oLevelInfo.Clone() as CLevelInfo;
			oCloneLevelInfo.m_stIDInfo.m_nID = CLevelInfoTable.Inst.GetNumLevelInfos(a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);
			
			m_oSelLevelInfo = oCloneLevelInfo;
			CLevelInfoTable.Inst.AddLevelInfo(oCloneLevelInfo);
		} else {
			// 스테이지 스크롤러 일 경우
			if(m_oLEUIsScrollerDict[EScroller.STAGE] == a_oScroller) {
				int nNumStageInfos = CLevelInfoTable.Inst.GetNumStageInfos(a_stIDInfo.m_nChapterID);
				var oStageLevelInfoDict = CLevelInfoTable.Inst.GetStageLevelInfos(a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);

				for(int i = 0; i < oStageLevelInfoDict.Count; ++i) {
					var oCloneLevelInfo = oStageLevelInfoDict[i].Clone() as CLevelInfo;
					oCloneLevelInfo.m_stIDInfo.m_nStageID = nNumStageInfos;

					CLevelInfoTable.Inst.AddLevelInfo(oCloneLevelInfo);
				}
			}
			// 챕터 스크롤러 일 경우
			else if(m_oLEUIsScrollerDict[EScroller.CHAPTER] == a_oScroller) {
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
			int nStageID = (m_oLEUIsScrollerDict[EScroller.STAGE] == a_oScroller) ? CLevelInfoTable.Inst.GetNumStageInfos(a_stIDInfo.m_nChapterID) - KCDefine.B_VAL_1_INT : KCDefine.B_VAL_0_INT;
			int nChapterID = (m_oLEUIsScrollerDict[EScroller.CHAPTER] == a_oScroller) ? CLevelInfoTable.Inst.NumChapterInfos - KCDefine.B_VAL_1_INT : a_stIDInfo.m_nChapterID;

			m_oSelLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(nID, nStageID, nChapterID);
		}

		CAccess.AssignVal<CLevelInfo>(ref m_oSelLevelInfo, m_oSelLevelInfo, CLevelInfoTable.Inst.GetLevelInfo(KCDefine.B_VAL_0_INT));
		this.UpdateUIsState();
	}

	/** 레벨 정보를 이동한다 */
	private void MoveLevelInfos(EnhancedScroller a_oScroller, STIDInfo a_stIDInfo, int a_nToID) {
		// 레벨 스크롤러 일 경우
		if(m_oLEUIsScrollerDict[EScroller.LEVEL] == a_oScroller) {
			int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);
			CLevelInfoTable.Inst.MoveLevelInfo(a_stIDInfo.m_nID, Mathf.Clamp(a_nToID, KCDefine.B_VAL_1_INT, nNumLevelInfos) - KCDefine.B_VAL_1_INT, a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);
		}
		// 스테이지 스크롤러 일 경우
		else if(m_oLEUIsScrollerDict[EScroller.STAGE] == a_oScroller) {
			int nNumStageInfos = CLevelInfoTable.Inst.GetNumStageInfos(a_stIDInfo.m_nChapterID);
			CLevelInfoTable.Inst.MoveStageLevelInfos(a_stIDInfo.m_nStageID, Mathf.Clamp(a_nToID, KCDefine.B_VAL_1_INT, nNumStageInfos) - KCDefine.B_VAL_1_INT, a_stIDInfo.m_nChapterID);
		}
		// 챕터 스크롤러 일 경우
		else if(m_oLEUIsScrollerDict[EScroller.CHAPTER] == a_oScroller) {
			int nNumChapterInfos = CLevelInfoTable.Inst.NumChapterInfos;
			CLevelInfoTable.Inst.MoveChapterLevelInfos(a_stIDInfo.m_nChapterID, Mathf.Clamp(a_nToID, KCDefine.B_VAL_1_INT, nNumChapterInfos) - KCDefine.B_VAL_1_INT);
		}
	}

	/** 메세지를 출려한다 */
	private void ShowMsg(string a_oMsg) {
		m_oMEUIsMsgUIs?.SetActive(true);
		m_oMEUIsMsgText?.ExSetText<Text>(a_oMsg, false);

		CScheduleManager.Inst.RemoveTimer(this);
		CScheduleManager.Inst.AddTimer(this, KCDefine.B_VAL_5_FLT, KCDefine.B_VAL_1_INT, () => m_oMEUIsMsgUIs?.SetActive(false));
	}

	/** 에디터 레벨 이동 입력 팝업 결과를 처리한다 */
	private void HandleMoveLevelInputPopupResult(string a_oStr) {
		// 식별자가 유효 할 경우
		if(int.TryParse(a_oStr, out int nID)) {
			this.MoveLevelInfos(m_oSelScroller, m_oSelLevelInfo.m_stIDInfo, nID);
		}
	}

	/** 에디터 레벨 제거 입력 팝업 결과를 처리한다 */
	private void HandleRemoveLevelInputPopupResult(string a_oStr) {
		var oTokenList = a_oStr.Split(KCDefine.B_TOKEN_DASH).ToList();

		// 식별자가 유효 할 경우
		if(oTokenList.Count >= KCDefine.B_VAL_2_INT && int.TryParse(oTokenList[KCDefine.B_VAL_0_INT], out int nMinID) && int.TryParse(oTokenList[KCDefine.B_VAL_1_INT], out int nMaxID)) {
			CFunc.LessCorrectSwap(ref nMinID, ref nMaxID);
			var stIDInfo = CFactory.MakeIDInfo(nMinID - KCDefine.B_VAL_1_INT, m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);

			for(int i = nMinID; i <= nMaxID; ++i) {
				// 레벨 정보가 존재 할 경우
				if(CLevelInfoTable.Inst.TryGetLevelInfo(stIDInfo.m_nID, out CLevelInfo oLevelInfo, stIDInfo.m_nStageID, stIDInfo.m_nChapterID)) {
					this.RemoveLevelInfos(m_oSelScroller, stIDInfo);
				}
			}
		}
	}
#endif			// #if UNITY_STANDALONE && RUNTIME_TEMPLATES_MODULE_ENABLE
	#endregion			// 조건부 함수

	#region 추가 함수

	#endregion			// 추가 함수
}

/** 서브 레벨 에디터 씬 관리자 - 중앙 에디터 UI */
public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
	#region 조건부 함수
#if UNITY_STANDALONE && RUNTIME_TEMPLATES_MODULE_ENABLE
	/** 중앙 에디터 UI 를 설정한다 */
	private void SetupMidEditorUIs() {
		// 텍스트를 설정한다
		m_oMEUIsMsgText = m_oMidEditorUIs.ExFindComponent<Text>(KCDefine.U_OBJ_N_MSG_TEXT);
		m_oMEUIsLevelText = m_oMidEditorUIs.ExFindComponent<Text>(KCDefine.U_OBJ_N_LEVEL_TEXT);

		// 버튼을 설정한다 {
		m_oMEUIsPrevBtn = m_oMidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_PREV_BTN);
		m_oMEUIsPrevBtn?.onClick.AddListener(this.OnTouchMEUIsPrevBtn);

		m_oMEUIsNextBtn = m_oMidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_NEXT_BTN);
		m_oMEUIsNextBtn?.onClick.AddListener(this.OnTouchMEUIsNextBtn);

		m_oMEUIsMoveLevelBtn = m_oMidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_MOVE_LEVEL_BTN);
		m_oMEUIsMoveLevelBtn?.onClick.AddListener(this.OnTouchMEUIsMoveLevelBtn);

		m_oMEUIsRemoveLevelBtn = m_oMidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_REMOVE_LEVEL_BTN);
		m_oMEUIsRemoveLevelBtn?.onClick.AddListener(this.OnTouchMEUIsRemoveLevelBtn);

		m_oMidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_SAVE_BTN)?.onClick.AddListener(this.OnTouchMEUIsSaveBtn);
		m_oMidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_RESET_BTN)?.onClick.AddListener(this.OnTouchMEUIsResetBtn);
		m_oMidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_TEST_BTN)?.onClick.AddListener(this.OnTouchMEUIsTestBtn);
		m_oMidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_TABLE_RELOAD_BTN)?.onClick.AddListener(this.OnTouchMEUIsTableReloadBtn);
		m_oMidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_COPY_LEVEL_BTN)?.onClick.AddListener(this.OnTouchMEUIsCopyLevelBtn);
		// 버튼을 설정한다 }
	}

	/** 중앙 에디터 UI 상태를 갱신한다 */
	private void UpdateMidEditorUIsState() {
		// 텍스트를 갱신한다
		m_oMEUIsLevelText?.ExSetText<Text>(string.Format(KCDefine.B_TEXT_FMT_LEVEL, m_oSelLevelInfo.m_stIDInfo.m_nID + KCDefine.B_VAL_1_INT), false);

		// 버튼을 갱신한다 {
		int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);

		m_oMEUIsPrevBtn?.ExSetInteractable(CLevelInfoTable.Inst.TryGetLevelInfo(m_oSelLevelInfo.m_stIDInfo.m_nID - KCDefine.B_VAL_1_INT, out CLevelInfo oPrevLevelInfo, m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID), false);
		m_oMEUIsNextBtn?.ExSetInteractable(CLevelInfoTable.Inst.TryGetLevelInfo(m_oSelLevelInfo.m_stIDInfo.m_nID + KCDefine.B_VAL_1_INT, out CLevelInfo oNextLevelInfo, m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID), false);

		m_oMEUIsMoveLevelBtn?.ExSetInteractable(nNumLevelInfos > KCDefine.B_VAL_1_INT);
		m_oMEUIsRemoveLevelBtn?.ExSetInteractable(nNumLevelInfos > KCDefine.B_VAL_1_INT);
		// 버튼을 갱신한다 }
	}

	/** 중앙 에디터 UI 이전 레벨 버튼을 눌렀을 경우 */
	private void OnTouchMEUIsPrevBtn() {
		// 이전 레벨 정보가 존재 할 경우
		if(CLevelInfoTable.Inst.TryGetLevelInfo(m_oSelLevelInfo.m_stIDInfo.m_nID - KCDefine.B_VAL_1_INT, out CLevelInfo oPrevLevelInfo, m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID)) {
			m_oSelLevelInfo = oPrevLevelInfo;
			this.UpdateUIsState();
		}
	}

	/** 중앙 에디터 UI 다음 레벨 버튼을 눌렀을 경우 */
	private void OnTouchMEUIsNextBtn() {
		// 다음 레벨 정보가 존재 할 경우
		if(CLevelInfoTable.Inst.TryGetLevelInfo(m_oSelLevelInfo.m_stIDInfo.m_nID + KCDefine.B_VAL_1_INT, out CLevelInfo oNextLevelInfo, m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID)) {
			m_oSelLevelInfo = oNextLevelInfo;
			this.UpdateUIsState();
		}
	}

	/** 중앙 에디터 UI 저장 버튼을 눌렀을 경우 */
	private void OnTouchMEUIsSaveBtn() {
		CLevelInfoTable.Inst.SaveLevelInfos();
	}

	/** 중앙 에디터 UI 리셋 버튼을 눌렀을 경우 */
	private void OnTouchMEUIsResetBtn() {
		Func.ShowEditorResetPopup(this.OnReceiveEditorResetPopupResult);
	}

	/** 중앙 에디터 UI 테스트 버튼을 눌렀을 경우 */
	private void OnTouchMEUIsTestBtn() {
		CGameInfoStorage.Inst.SetupPlayLevelInfo(m_oSelLevelInfo.m_stIDInfo.m_nID, EPlayMode.TEST, m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);
		CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME);
	}

	/** 중앙 에디터 UI 테이블 다시 로드 버튼을 눌렀을 경우 */
	private void OnTouchMEUIsTableReloadBtn() {
		Func.ShowEditorTableReloadPopup(this.OnReceiveEditorTableReloadPopupResult);
	}

	/** 중앙 에디터 UI 레벨 복사 버튼을 눌렀을 경우 */
	private void OnTouchMEUIsCopyLevelBtn() {
		int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);

		// 레벨 추가가 가능 할 경우
		if(nNumLevelInfos < KCDefine.U_MAX_NUM_LEVEL_INFOS) {
			var stIDInfo = CFactory.MakeIDInfo(m_oSelLevelInfo.m_stIDInfo.m_nID, m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);
			this.CopyLevelInfos(m_oLEUIsScrollerDict[EScroller.LEVEL], stIDInfo);
		}
	}

	/** 중앙 에디터 UI 레벨 이동 버튼을 눌렀을 경우 */
	private void OnTouchMEUIsMoveLevelBtn() {
		m_eInput = EInput.MOVE_LEVEL;
		m_oSelScroller = m_oLEUIsScrollerDict[EScroller.LEVEL];

		Func.ShowEditorInputPopup(this.PopupUIs, (a_oSender) => {
			var stParams = new CEditorInputPopup.STParams() {
				m_oCallbackDict = new Dictionary<CEditorInputPopup.ECallback, System.Action<CEditorInputPopup, string, bool>>() {
					[CEditorInputPopup.ECallback.OK_CANCEL] = this.OnReceiveEditorInputPopupResult
				}
			};

			(a_oSender as CEditorInputPopup).Init(stParams);
		});
	}

	/** 중앙 에디터 UI 레벨 제거 버튼을 눌렀을 경우 */
	private void OnTouchMEUIsRemoveLevelBtn() {
		m_oSelScroller = m_oLEUIsScrollerDict[EScroller.LEVEL];
		Func.ShowEditorLevelRemovePopup(this.OnReceiveEditorRemovePopupResult);
	}
#endif			// #if UNITY_STANDALONE && RUNTIME_TEMPLATES_MODULE_ENABLE
	#endregion			// 조건부 함수

	#region 추가 함수

	#endregion			// 추가 함수
}

/** 서브 레벨 에디터 씬 관리자 - 왼쪽 에디터 UI */
public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
	#region 조건부 함수
#if UNITY_STANDALONE && RUNTIME_TEMPLATES_MODULE_ENABLE
	/** 왼쪽 에디터 UI 를 설정한다 */
	private void SetupLeftEditorUIs() {
		// 스크롤 뷰를 설정한다 {
		var oLevelScroller = m_oLeftEditorUIs.ExFindComponent<EnhancedScroller>(KCDefine.U_OBJ_N_LEVEL_SCROLL_VIEW);
		oLevelScroller?.gameObject.SetActive(true);

		var oStageScrollerA = m_oLeftEditorUIs.ExFindComponent<EnhancedScroller>(KCDefine.LES_OBJ_N_LE_UIS_STAGE_SCROLL_VIEW_A);
		oStageScrollerA?.gameObject.SetActive(false);

		var oStageScrollerB = m_oLeftEditorUIs.ExFindComponent<EnhancedScroller>(KCDefine.LES_OBJ_N_LE_UIS_STAGE_SCROLL_VIEW_B);
		oStageScrollerB?.gameObject.SetActive(false);

		var oChapterScroller = m_oLeftEditorUIs.ExFindComponent<EnhancedScroller>(KCDefine.U_OBJ_N_CHAPTER_SCROLL_VIEW);
		oChapterScroller?.gameObject.SetActive(false);

		m_oLEUIsScrollerDict.Add(EScroller.LEVEL, oLevelScroller);
		m_oLEUIsScrollerDict.Add(EScroller.STAGE, oStageScrollerA);
		m_oLEUIsScrollerDict.Add(EScroller.CHAPTER, oChapterScroller);

		foreach(var stKeyVal in m_oLEUIsScrollerDict) {
			stKeyVal.Value?.ExSetDelegate(this, false);	
		}
		// 스크롤 뷰를 설정한다 }

		// 버튼을 설정한다 {
		m_oLEUIsASetBtn = m_oLeftEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_LE_UIS_A_SET_BTN);
		m_oLEUIsASetBtn?.onClick.AddListener(this.OnTouchLEUIsASetBtn);

		m_oLEUIsBSetBtn = m_oLeftEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_LE_UIS_B_SET_BTN);
		m_oLEUIsBSetBtn?.onClick.AddListener(this.OnTouchLEUIsBSetBtn);

		var oAddStageBtn = m_oLeftEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_LE_UIS_ADD_STAGE_BTN);
		oAddStageBtn?.onClick.AddListener(this.OnTouchLEUIsAddStageBtn);

		var oAddChapterBtn = m_oLeftEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_LE_UIS_ADD_CHAPTER_BTN);
		oAddChapterBtn?.onClick.AddListener(this.OnTouchLEUIsAddChapterBtn);

		m_oLeftEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_LE_UIS_ADD_LEVEL_BTN)?.onClick.AddListener(this.OnTouchLEUIsAddLevelBtn);

		this.ExLateCallFunc((a_oSender) => {
#if AB_TEST_ENABLE
			m_oLEUIsSetUIs?.SetActive(true);
#endif			// #if AB_TEST_ENABLE

			oAddStageBtn?.ExSetInteractable((oStageScrollerA != null && oStageScrollerA.gameObject.activeSelf) || (oStageScrollerB != null && oStageScrollerB.gameObject.activeSelf));
			oAddChapterBtn?.ExSetInteractable(oChapterScroller != null && oChapterScroller.gameObject.activeSelf);
		});
		// 버튼을 설정한다 }
	}

	/** 왼쪽 에디터 UI 상태를 갱신한다 */
	private void UpdateLeftEditorUIsState() {
		// 버튼을 설정한다 {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
		m_oLEUIsASetBtn?.image.ExSetColor<Image>((CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.A) ? Color.cyan : Color.white, false);
		m_oLEUIsBSetBtn?.image.ExSetColor<Image>((CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.B) ? Color.cyan : Color.white, false);
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
		// 버튼을 설정한다 }

		// 스크롤 뷰를 갱신한다
		m_oLEUIsScrollerDict[EScroller.LEVEL]?.ExReloadData(m_oSelLevelInfo.m_stIDInfo.m_nID - KCDefine.B_VAL_1_INT, false);
		m_oLEUIsScrollerDict[EScroller.STAGE]?.ExReloadData(m_oSelLevelInfo.m_stIDInfo.m_nStageID - KCDefine.B_VAL_1_INT, false);
		m_oLEUIsScrollerDict[EScroller.CHAPTER]?.ExReloadData(m_oSelLevelInfo.m_stIDInfo.m_nChapterID - KCDefine.B_VAL_1_INT, false);
	}

	/** 왼쪽 에디터 UI A 세트 버튼을 눌렀을 경우 */
	private void OnTouchLEUIsASetBtn() {
		m_eSelUserType = EUserType.A;
		Func.ShowEditorASetPopup(this.OnReceiveEditorSetPopupResult);
	}

	/** 왼쪽 에디터 UI B 세트 버튼을 눌렀을 경우 */
	private void OnTouchLEUIsBSetBtn() {
		m_eSelUserType = EUserType.B;
		Func.ShowEditorBSetPopup(this.OnReceiveEditorSetPopupResult);
	}

	/** 왼쪽 에디터 UI 레벨 추가 버튼을 눌렀을 경우 */
	private void OnTouchLEUIsAddLevelBtn() {
		Func.ShowEditorLevelCreatePopup(this.PopupUIs, (a_oSender) => {
			var stParams = new CEditorLevelCreatePopup.STParams() {
				m_oCallbackDict = new Dictionary<CEditorLevelCreatePopup.ECallback, System.Action<CEditorLevelCreatePopup, CEditorLevelCreateInfo, bool>>() {
					[CEditorLevelCreatePopup.ECallback.OK_CANCEL] = this.OnReceiveEditorLevelCreatePopupResult
				}
			};

			(a_oSender as CEditorLevelCreatePopup).Init(stParams);
		});
	}

	/** 왼쪽 에디터 UI 스테이지 추가 버튼을 눌렀을 경우 */
	private void OnTouchLEUIsAddStageBtn() {
		int nNumStageInfos = CLevelInfoTable.Inst.GetNumStageInfos(m_oSelLevelInfo.m_stIDInfo.m_nChapterID);

		// 스테이지 추가가 가능 할 경우
		if(nNumStageInfos < KCDefine.U_MAX_NUM_STAGE_INFOS) {
			this.AddLevelInfo(KCDefine.B_VAL_0_INT, nNumStageInfos, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);
		}
	}

	/** 왼쪽 에디터 UI 챕터 추가 버튼을 눌렀을 경우 */
	private void OnTouchLEUIsAddChapterBtn() {
		int nNumChapterInfos = CLevelInfoTable.Inst.NumChapterInfos;

		// 챕터 추가가 가능 할 경우
		if(nNumChapterInfos < KCDefine.U_MAX_NUM_CHAPTER_INFOS) {
			this.AddLevelInfo(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, nNumChapterInfos);
		}
	}
#endif			// #if UNITY_STANDALONE && RUNTIME_TEMPLATES_MODULE_ENABLE
	#endregion			// 조건부 함수

	#region 추가 함수

	#endregion			// 추가 함수
}

/** 서브 레벨 에디터 씬 관리자 - 오른쪽 에디터 UI */
public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
	#region 조건부 함수
#if UNITY_STANDALONE && RUNTIME_TEMPLATES_MODULE_ENABLE
	/** 오른족 에디터 UI 를 설정한다 */
	private void SetupRightEditorUIs() {
		// 텍스트를 설정한다
		m_oREUIsPageText = m_oRightEditorUIs.ExFindComponent<Text>(KCDefine.U_OBJ_N_PAGE_TEXT);
		m_oREUIsTitleText = m_oRightEditorUIs.ExFindComponent<Text>(KCDefine.U_OBJ_N_TITLE_TEXT);

		// 버튼을 설정한다 {
		m_oREUIsPrevBtn = m_oRightEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_RE_UIS_PREV_BTN);
		m_oREUIsNextBtn = m_oRightEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_RE_UIS_NEXT_BTN);

		m_oREUIsRemoveLevelBtn = m_oRightEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_RE_UIS_REMOVE_LEVEL_BTN);
		m_oREUIsRemoveLevelBtn?.onClick.AddListener(this.OnTouchREUIsRemoveLevelBtn);

		m_oRightEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_RE_UIS_APPLY_BTN)?.onClick.AddListener(this.OnTouchREUIsApplyBtn);
		m_oRightEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_RE_UIS_LOAD_LEVEL_BTN)?.onClick.AddListener(this.OnTouchREUIsLoadLevelBtn);
		// 버튼을 설정한다 }

		// 입력 필드를 설정한다 {
		m_oREUIsLevelInput = m_oRightEditorUIs.ExFindComponent<InputField>(KCDefine.LES_OBJ_N_RE_UIS_LEVEL_INPUT);

		m_oREUIsNumCellsXInput = m_oRightEditorUIs.ExFindComponent<InputField>(KCDefine.LES_OBJ_N_RE_UIS_NUM_CELLS_X_INPUT);
		m_oREUIsNumCellsYInput = m_oRightEditorUIs.ExFindComponent<InputField>(KCDefine.LES_OBJ_N_RE_UIS_NUM_CELLS_Y_INPUT);
		// 입력 필드를 설정한다 }

		// 스크롤 뷰를 설정한다
		m_oREUIsScrollSnap = m_oRightEditorUIs.ExFindComponent<SimpleScrollSnap>(KCDefine.LES_OBJ_N_RE_UIS_PAGE_VIEW);
		m_oREUIsScrollSnap?.OnPanelCentered.AddListener((a_nCenterIdx, a_nSelIdx) => this.UpdateUIsState());
	}

	/** 오른쪽 에디터 UI 상태를 갱신한다 */
	private void UpdateRightEditorUIsState() {
		int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);

		// 텍스트를 설정한다
		m_oREUIsTitleText?.ExSetText<Text>(string.Format(CStrTable.Inst.GetStr(KCDefine.ST_KEY_COMMON_LEVEL_PAGE_TEXT_FMT), m_oSelLevelInfo.m_stIDInfo.m_nID + KCDefine.B_VAL_1_INT, nNumLevelInfos), false);

		// 버튼을 설정한다
		m_oREUIsRemoveLevelBtn?.ExSetInteractable(nNumLevelInfos > KCDefine.B_VAL_1_INT, false);
		
		// 입력 필드를 갱신한다 {
		m_oREUIsLevelInput?.ExSetText<InputField>($"{m_oSelLevelInfo.m_stIDInfo.m_nID + KCDefine.B_VAL_1_INT}", false);

		m_oREUIsNumCellsXInput?.ExSetText<InputField>((m_oSelLevelInfo.NumCells.x <= KCDefine.B_VAL_0_INT) ? string.Empty : $"{m_oSelLevelInfo.NumCells.x}", false);
		m_oREUIsNumCellsYInput?.ExSetText<InputField>((m_oSelLevelInfo.NumCells.y <= KCDefine.B_VAL_0_INT) ? string.Empty : $"{m_oSelLevelInfo.NumCells.y}", false);
		// 입력 필드를 갱신한다 }

		// 스크롤 스냅이 존재 할 경우
		if(m_oREUIsScrollSnap != null) {
			// 텍스트를 설정한다
			m_oREUIsPageText?.ExSetText<Text>(string.Format(KCDefine.B_TEXT_FMT_2_SLASH_COMBINE, m_oREUIsScrollSnap.CenteredPanel + KCDefine.B_VAL_1_INT, m_oREUIsScrollSnap.NumberOfPanels), false);

			// 버튼 상태를 갱신한다
			m_oREUIsPrevBtn?.ExSetInteractable(m_oREUIsScrollSnap.CenteredPanel > KCDefine.B_VAL_0_INT, false);
			m_oREUIsNextBtn?.ExSetInteractable(m_oREUIsScrollSnap.CenteredPanel < m_oREUIsScrollSnap.NumberOfPanels - KCDefine.B_VAL_1_INT, false);
		}
	}

	/** 오른쪽 에디터 UI 적용 버튼을 눌렀을 경우 */
	private void OnTouchREUIsApplyBtn() {
#if ENGINE_TEMPLATES_MODULE_ENABLE
		bool bIsValidA = int.TryParse(m_oREUIsNumCellsXInput?.text, out int nNumCellsX);
		bool bIsValidB = int.TryParse(m_oREUIsNumCellsYInput?.text, out int nNumCellsY);

		bool bIsValidNumCellsX = Mathf.Max(nNumCellsX, SampleEngineName.KDefine.E_MIN_NUM_CELLS.x) != m_oSelLevelInfo.NumCells.x;
		bool bIsValidNumCellsY = Mathf.Max(nNumCellsY, SampleEngineName.KDefine.E_MIN_NUM_CELLS.y) != m_oSelLevelInfo.NumCells.y;

		// 셀 개수가 유효 할 경우
		if(bIsValidA && bIsValidB && (bIsValidNumCellsX || bIsValidNumCellsY)) {
			Func.SetupEditorLevelInfo(m_oSelLevelInfo, new CSubEditorLevelCreateInfo() {
				m_nNumLevels = KCDefine.B_VAL_0_INT, m_stMinNumCells = new Vector3Int(nNumCellsX, nNumCellsY, KCDefine.B_VAL_0_INT), m_stMaxNumCells = new Vector3Int(nNumCellsX, nNumCellsY, KCDefine.B_VAL_0_INT)
			});
			
			this.UpdateUIsState();
		}
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
	}

	/** 오른쪽 에디터 UI 레벨 로드 버튼을 눌렀을 경우 */
	private void OnTouchREUIsLoadLevelBtn() {
		// 식별자가 유효 할 경우
		if(int.TryParse(m_oREUIsLevelInput?.text, out int nID)) {
			int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);
			m_oSelLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(Mathf.Clamp(nID, KCDefine.B_VAL_1_INT, nNumLevelInfos) - KCDefine.B_VAL_1_INT, m_oSelLevelInfo.m_stIDInfo.m_nStageID, m_oSelLevelInfo.m_stIDInfo.m_nChapterID);
			
			this.UpdateUIsState();
		}
	}

	/** 오른쪽 에디터 UI 레벨 제거 버튼을 눌렀을 경우 */
	private void OnTouchREUIsRemoveLevelBtn() {
		m_eInput = EInput.REMOVE_LEVEL;
		m_oSelScroller = m_oLEUIsScrollerDict[EScroller.LEVEL];

		Func.ShowEditorInputPopup(this.PopupUIs, (a_oSender) => {
			var stParams = new CEditorInputPopup.STParams() {
				m_oCallbackDict = new Dictionary<CEditorInputPopup.ECallback, System.Action<CEditorInputPopup, string, bool>>() {
					[CEditorInputPopup.ECallback.OK_CANCEL] = this.OnReceiveEditorInputPopupResult
				}
			};

			(a_oSender as CEditorInputPopup).Init(stParams);
		});
	}
#endif			// #if UNITY_STANDALONE && RUNTIME_TEMPLATES_MODULE_ENABLE
	#endregion			// 조건부 함수

	#region 추가 함수

	#endregion			// 추가 함수
}

/** 서브 레벨 에디터 씬 관리자 - 스크롤러 셀 뷰 */
public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
	#region 조건부 함수
#if UNITY_STANDALONE && RUNTIME_TEMPLATES_MODULE_ENABLE
	/** 스크롤러 셀 뷰 선택 버튼을 눌렀을 경우 */
	private void OnTouchSCVSelBtn(CScrollerCellView a_oSender, long a_nID) {
		m_oSelLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(a_nID.ExUniqueLevelIDToID(), a_nID.ExUniqueLevelIDToStageID(), a_nID.ExUniqueLevelIDToChapterID());
		this.UpdateUIsState();
	}

	/** 스크롤러 셀 뷰 복사 버튼을 눌렀을 경우 */
	private void OnTouchSCVCopyBtn(CScrollerCellView a_oSender, long a_nID) {
		int nNumInfos = CLevelInfoTable.Inst.GetNumLevelInfos(a_nID.ExUniqueLevelIDToStageID(), a_nID.ExUniqueLevelIDToChapterID());
		int nMaxNumInfos = KCDefine.U_MAX_NUM_LEVEL_INFOS;

		// 레벨 스크롤러가 아닐 경우
		if(m_oLEUIsScrollerDict[EScroller.LEVEL] != a_oSender.Scroller) {
			nNumInfos = (m_oLEUIsScrollerDict[EScroller.STAGE] == a_oSender.Scroller) ? CLevelInfoTable.Inst.GetNumStageInfos(a_nID.ExUniqueLevelIDToChapterID()) : CLevelInfoTable.Inst.NumChapterInfos;
			nMaxNumInfos = (m_oLEUIsScrollerDict[EScroller.STAGE] == a_oSender.Scroller) ? KCDefine.U_MAX_NUM_STAGE_INFOS : KCDefine.U_MAX_NUM_CHAPTER_INFOS;
		}

		// 복사가 가능 할 경우
		if(nNumInfos < nMaxNumInfos) {
			this.CopyLevelInfos(a_oSender.Scroller, CFactory.MakeIDInfo(a_nID.ExUniqueLevelIDToID(), a_nID.ExUniqueLevelIDToStageID(), a_nID.ExUniqueLevelIDToChapterID()));
		}
	}

	/** 스크롤러 셀 뷰 이동 버튼을 눌렀을 경우 */
	private void OnTouchSCVMoveBtn(CScrollerCellView a_oSender, long a_nID) {
		m_oSelScroller = a_oSender.Scroller;

		Func.ShowEditorInputPopup(this.PopupUIs, (a_oSender) => {
			var stParams = new CEditorInputPopup.STParams() {
				m_oCallbackDict = new Dictionary<CEditorInputPopup.ECallback, System.Action<CEditorInputPopup, string, bool>>() {
					[CEditorInputPopup.ECallback.OK_CANCEL] = this.OnReceiveEditorInputPopupResult
				}
			};
			
			(a_oSender as CEditorInputPopup).Init(stParams);
		});
	}

	/** 스크롤러 셀 뷰 제거 버튼을 눌렀을 경우 */
	private void OnTouchSCVRemoveBtn(CScrollerCellView a_oSender, long a_nID) {
		m_oSelScroller = a_oSender.Scroller;

		// 레벨 스크롤러 일 경우
		if(m_oLEUIsScrollerDict[EScroller.LEVEL] == a_oSender.Scroller) {
			Func.ShowEditorLevelRemovePopup(this.OnReceiveEditorRemovePopupResult);
		}
		// 스테이지 스크롤러 일 경우
		else if(m_oLEUIsScrollerDict[EScroller.STAGE] == a_oSender.Scroller) {
			Func.ShowEditorStageRemovePopup(this.OnReceiveEditorRemovePopupResult);
		}
		// 챕터 스크롤러 일 경우
		else if(m_oLEUIsScrollerDict[EScroller.CHAPTER] == a_oSender.Scroller) {
			Func.ShowEditorChapterRemovePopup(this.OnReceiveEditorRemovePopupResult);
		}
	}
#endif			// #if UNITY_STANDALONE && RUNTIME_TEMPLATES_MODULE_ENABLE
	#endregion			// 조건부 함수

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if UNITY_STANDALONE && EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
#endif			// #if SCRIPT_TEMPLATE_ONLY
