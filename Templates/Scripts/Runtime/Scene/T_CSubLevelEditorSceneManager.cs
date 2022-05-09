using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using EnhancedUI.EnhancedScroller;
using DanielLochner.Assets.SimpleScrollSnap;

#if SCRIPT_TEMPLATE_ONLY
#if UNITY_STANDALONE && EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
using GoogleSheetsToUnity;

#if INPUT_SYSTEM_MODULE_ENABLE
using UnityEngine.InputSystem;
#endif			// #if INPUT_SYSTEM_MODULE_ENABLE

namespace LevelEditorScene {
	/** 서브 레벨 에디터 씬 관리자 */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
		/** 식별자 */
		private enum EKey {
			NONE = -1,

			ME_UIS_MSG_TEXT,
			ME_UIS_LEVEL_TEXT,

			ME_UIS_PREV_BTN,
			ME_UIS_NEXT_BTN,
			ME_UIS_MOVE_LEVEL_BTN,
			ME_UIS_REMOVE_LEVEL_BTN,

			LE_UIS_A_SET_BTN,
			LE_UIS_B_SET_BTN,

			LE_UIS_LEVEL_SCROLLER_INFO,
			LE_UIS_STAGE_SCROLLER_INFO,
			LE_UIS_CHAPTER_SCROLLER_INFO,

			RE_UIS_PAGE_TEXT,
			RE_UIS_TITLE_TEXT,

			RE_UIS_PREV_BTN,
			RE_UIS_NEXT_BTN,
			RE_UIS_REMOVE_ALL_LEVELS_BTN,
			RE_UIS_LOAD_REMOTE_TABLE_BTN,

			RE_UIS_PAGE_SCROLL_SNAP,
			RE_UIS_PAGE_UIS_01,
			
			RE_UIS_PAGE_UIS_01_LEVEL_INPUT,
			RE_UIS_PAGE_UIS_01_NUM_CELLS_X_INPUT,
			RE_UIS_PAGE_UIS_01_NUM_CELLS_Y_INPUT,

			SEL_SCROLLER,
			SEL_BLOCK_SPRITE,
			BG_TOUCH_DISPATCHER,

#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
			SEL_LEVEL_INFO,
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE

			[HideInInspector] MAX_VAL
		}

		/** 테이블 */
		private enum ETable {
			NONE = -1,
			LOCAL,
			REMOTE,
			[HideInInspector] MAX_VAL
		}

		/** 입력 팝업 */
		private enum EInputPopup {
			NONE = -1,
			MOVE_LEVEL,
			REMOVE_LEVEL,
			[HideInInspector] MAX_VAL
		}

		/** 콜백 */
		private enum ECallback {
			NONE = -1,
			SETUP_RE_UIS_PAGE_UIS_01,
			UPDATE_RE_UIS_PAGE_UIS_01,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		private ETable m_eSelTable = ETable.NONE;
		private EUserType m_eSelUserType = EUserType.NONE;
		private EInputPopup m_eSelInputPopup = EInputPopup.NONE;
		private Dictionary<ECallback, System.Reflection.MethodInfo> m_oMethodInfoDict = new Dictionary<ECallback, System.Reflection.MethodInfo>();

		private Dictionary<EKey, SpriteRenderer> m_oSpriteDict = new Dictionary<EKey, SpriteRenderer>() {
			[EKey.SEL_BLOCK_SPRITE] = null
		};

		private Dictionary<EKey, CTouchDispatcher> m_oTouchDispatcherDict = new Dictionary<EKey, CTouchDispatcher>() {
			[EKey.BG_TOUCH_DISPATCHER] = null
		};

		[SerializeField] private string m_oEpisodeInfoTableGoogleSheetID = string.Empty;

		/** =====> UI <===== */
		private Dictionary<EKey, Text> m_oTextDict = new Dictionary<EKey, Text>() {
			[EKey.ME_UIS_MSG_TEXT] = null,
			[EKey.ME_UIS_LEVEL_TEXT] = null,

			[EKey.RE_UIS_PAGE_TEXT] = null,
			[EKey.RE_UIS_TITLE_TEXT] = null
		};

		private Dictionary<EKey, InputField> m_oInputDict = new Dictionary<EKey, InputField>() {
			[EKey.RE_UIS_PAGE_UIS_01_LEVEL_INPUT] = null,
			[EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_X_INPUT] = null,
			[EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_Y_INPUT] = null
		};

		private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>() {
			[EKey.ME_UIS_PREV_BTN] = null,
			[EKey.ME_UIS_NEXT_BTN] = null,
			[EKey.ME_UIS_MOVE_LEVEL_BTN] = null,
			[EKey.ME_UIS_REMOVE_LEVEL_BTN] = null,

			[EKey.LE_UIS_A_SET_BTN] = null,
			[EKey.LE_UIS_B_SET_BTN] = null,

			[EKey.RE_UIS_PREV_BTN] = null,
			[EKey.RE_UIS_NEXT_BTN] = null,
			[EKey.RE_UIS_REMOVE_ALL_LEVELS_BTN] = null,
			[EKey.RE_UIS_LOAD_REMOTE_TABLE_BTN] = null
		};

		private Dictionary<EKey, EnhancedScroller> m_oScrollerDict = new Dictionary<EKey, EnhancedScroller>() {
			[EKey.SEL_SCROLLER] = null
		};

		private Dictionary<EKey, SimpleScrollSnap> m_oScrollSnapDict = new Dictionary<EKey, SimpleScrollSnap>() {
			[EKey.RE_UIS_PAGE_SCROLL_SNAP] = null
		};

		private Dictionary<EKey, (EnhancedScroller, EnhancedScrollerCellView)> m_oScrollerInfoDict = new Dictionary<EKey, (EnhancedScroller, EnhancedScrollerCellView)>() {
			[EKey.LE_UIS_LEVEL_SCROLLER_INFO] = (null, null),
			[EKey.LE_UIS_STAGE_SCROLLER_INFO] = (null, null),
			[EKey.LE_UIS_CHAPTER_SCROLLER_INFO] = (null, null)
		};

		/** =====> 객체 <===== */
		private Dictionary<EKey, GameObject> m_oUIsDict = new Dictionary<EKey, GameObject>();

#if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
		private SampleEngineName.STGridInfo m_stGridInfo;
		private Dictionary<EBlockType, List<(EBlockKinds, SpriteRenderer)>>[,] m_oBlockSpriteInfoDictContainers = null;
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE

#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
		private Dictionary<EKey, CLevelInfo> m_oLevelInfoDict = new Dictionary<EKey, CLevelInfo>() {
			[EKey.SEL_LEVEL_INFO] = null
		};
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
		#endregion			// 변수

		#region IEnhancedScrollerDelegate
		/** 셀 개수를 반환한다 */
		public virtual int GetNumberOfCells(EnhancedScroller a_oSender) {
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].Item1 == a_oSender) {
				return CLevelInfoTable.Inst.GetNumLevelInfos(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nStageID, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nChapterID);
			}

			return (m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].Item1 == a_oSender) ? CLevelInfoTable.Inst.GetNumStageInfos(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nChapterID) : CLevelInfoTable.Inst.NumChapterInfos;
#else
			return KCDefine.B_VAL_0_INT;
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
		}

		/** 셀 뷰 크기를 반환한다 */
		public virtual float GetCellViewSize(EnhancedScroller a_oSender, int a_nDataIdx) {
			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].Item1 == a_oSender) {
				return (m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].Item2.transform as RectTransform).sizeDelta.y;
			}

			return (m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].Item1 == a_oSender) ? (m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].Item2.transform as RectTransform).sizeDelta.y : (m_oScrollerInfoDict[EKey.LE_UIS_CHAPTER_SCROLLER_INFO].Item2.transform as RectTransform).sizeDelta.y;
		}

		/** 셀 뷰를 반환한다 */
		public virtual EnhancedScrollerCellView GetCellView(EnhancedScroller a_oSender, int a_nDataIdx, int a_nCellIdx) {
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
			var stColor = (m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nID == a_nDataIdx) ? KCDefine.U_COLOR_NORM : KCDefine.U_COLOR_DISABLE;
			var stIDInfo = CFactory.MakeIDInfo(a_nDataIdx, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nStageID, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nChapterID);
			var oOriginScrollerCellView = m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].Item2;

			int nNumInfos = CLevelInfoTable.Inst.GetNumLevelInfos(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nStageID, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nChapterID);

			string oNameFmt = KCDefine.LES_TEXT_FMT_LEVEL;
			string oNumInfosStr = string.Empty;

			// 스테이지 스크롤러 일 경우
			if(m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].Item1 == a_oSender) {
				nNumInfos = CLevelInfoTable.Inst.GetNumStageInfos(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nChapterID);
				oNameFmt = KCDefine.LES_TEXT_FMT_STAGE;
				oNumInfosStr = string.Format(KCDefine.B_TEXT_FMT_BRACKET, CLevelInfoTable.Inst.GetNumLevelInfos(a_nDataIdx, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nChapterID));

				stColor = (m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nStageID == a_nDataIdx) ? KCDefine.U_COLOR_NORM : KCDefine.U_COLOR_DISABLE;
				stIDInfo = CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, a_nDataIdx, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nChapterID);
				oOriginScrollerCellView = m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].Item2;
			}
			// 챕터 스크롤러 일 경우
			else if(m_oScrollerInfoDict[EKey.LE_UIS_CHAPTER_SCROLLER_INFO].Item1 == a_oSender) {
				nNumInfos = CLevelInfoTable.Inst.NumChapterInfos;
				oNameFmt = KCDefine.LES_TEXT_FMT_CHAPTER;
				oNumInfosStr = string.Format(KCDefine.B_TEXT_FMT_BRACKET, CLevelInfoTable.Inst.GetNumStageInfos(a_nDataIdx));

				stColor = (m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nChapterID == a_nDataIdx) ? KCDefine.U_COLOR_NORM : KCDefine.U_COLOR_DISABLE;
				stIDInfo = CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, a_nDataIdx);
				oOriginScrollerCellView = m_oScrollerInfoDict[EKey.LE_UIS_CHAPTER_SCROLLER_INFO].Item2;
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
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
		}
		#endregion			// IEnhancedScrollerDelegate

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();
			
			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
#if (UNITY_STANDALONE && EXTRA_SCRIPT_MODULE_ENABLE) && (ENGINE_TEMPLATES_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE)
				// 레벨 정보가 없을 경우
				if(!CLevelInfoTable.Inst.LevelInfoDictContainer.ExIsValid()) {
					var oLevelInfo = Factory.MakeLevelInfo(KCDefine.B_VAL_0_INT);

					Func.SetupEditorLevelInfo(oLevelInfo, new CSubEditorLevelCreateInfo() {
						m_nNumLevels = KCDefine.B_VAL_0_INT, m_stMinNumCells = SampleEngineName.KDefine.E_MIN_NUM_CELLS, m_stMaxNumCells = SampleEngineName.KDefine.E_MIN_NUM_CELLS
					});

					CLevelInfoTable.Inst.AddLevelInfo(oLevelInfo);
					CLevelInfoTable.Inst.SaveLevelInfos();
				}
#endif			// #if (UNITY_STANDALONE && EXTRA_SCRIPT_MODULE_ENABLE) && (ENGINE_TEMPLATES_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE)

				this.SetupAwake();
			}
		}
		
		/** 초기화 */
		public override void Start() {
			base.Start();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
#if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE)
				this.ExLateCallFunc((a_oSender) => this.UpdateUIsState(), KCDefine.U_DELAY_INIT);
#endif			// #if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE)

				this.SetupStart();
				CSndManager.Inst.StopBGSnd();
			}
		}

		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
#if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE)
#if INPUT_SYSTEM_MODULE_ENABLE
				// 이전 레벨 키를 눌렀을 경우
				if(Keyboard.current.leftShiftKey.isPressed && Keyboard.current.upArrowKey.wasPressedThisFrame) {
					this.OnTouchMEUIsPrevBtn();
				}
				// 다음 레벨 키를 눌렀을 경우
				else if(Keyboard.current.leftShiftKey.isPressed && Keyboard.current.downArrowKey.wasPressedThisFrame) {
					this.OnTouchMEUIsNextBtn();
				}

				// 이전 페이지 키를 눌렀을 경우
				if(Keyboard.current.leftShiftKey.isPressed && Keyboard.current.leftArrowKey.wasPressedThisFrame) {
					m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP]?.GoToPreviousPanel();
				}
				// 다음 페이지 키를 눌렀을 경우
				else if(Keyboard.current.leftShiftKey.isPressed && Keyboard.current.rightArrowKey.wasPressedThisFrame) {
					m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP]?.GoToNextPanel();
				}
#else
				// 이전 레벨 키를 눌렀을 경우
				if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.UpArrow)) {
					this.OnTouchMEUIsPrevBtn();
				}
				// 다음 레벨 키를 눌렀을 경우
				else if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.DownArrow)) {
					this.OnTouchMEUIsNextBtn();
				}

				// 이전 페이지 키를 눌렀을 경우
				if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.LeftArrow)) {
					m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP]?.GoToPreviousPanel();
				}
				// 다음 페이지 키를 눌렀을 경우
				else if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.RightArrow)) {
					m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP]?.GoToNextPanel();
				}
#endif			// #if INPUT_SYSTEM_MODULE_ENABLE
#endif			// #if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE)
			}
		}

		/** 제거 되었을 경우 */
		public override void OnDestroy() {
			base.OnDestroy();

			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsAppRunning) {
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
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
				Func.ShowEditorQuitPopup(this.OnReceiveEditorQuitPopupResult);
#else
				this.OnReceiveEditorQuitPopupResult(null, true);
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
			}
		}

		/** 씬을 설정한다 */
		private void SetupAwake() {
			this.AddObjsPool(KDefine.LES_KEY_SPRITE_OBJS_POOL, CFactory.CreateObjsPool(KCDefine.U_OBJ_P_SPRITE, this.BlockObjs));

#if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE)
			this.SetupMidEditorUIs();
			this.SetupLeftEditorUIs();
			this.SetupRightEditorUIs();

			// 레벨 정보를 설정한다
			m_oLevelInfoDict[EKey.SEL_LEVEL_INFO] = CGameInfoStorage.Inst.PlayLevelInfo ?? CLevelInfoTable.Inst.GetLevelInfo(KCDefine.B_VAL_0_INT);

			// 터치 전달자를 설정한다
			m_oTouchDispatcherDict[EKey.BG_TOUCH_DISPATCHER] = this.BGTouchResponder?.GetComponentInChildren<CTouchDispatcher>();
			m_oTouchDispatcherDict[EKey.BG_TOUCH_DISPATCHER]?.ExSetBeginCallback(this.OnTouchBegin, false);
			m_oTouchDispatcherDict[EKey.BG_TOUCH_DISPATCHER]?.ExSetMoveCallback(this.OnTouchMove, false);
			m_oTouchDispatcherDict[EKey.BG_TOUCH_DISPATCHER]?.ExSetEndCallback(this.OnTouchEnd, false);
#endif			// #if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE)
		}

		/** 씬을 설정한다 */
		private void SetupStart() {
			// Do Something
		}

		/** 에디터 종료 팝업 결과를 수신했을 경우 */
		private void OnReceiveEditorQuitPopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
#if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE)
				CLevelInfoTable.Inst.SaveLevelInfos();
#endif			// #if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE)

#if STUDY_MODULE_ENABLE
				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MENU);
#else
				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_TITLE);
#endif			// #if STUDY_MODULE_ENABLE
			}
		}
		#endregion			// 함수

		#region 조건부 함수
#if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE)
		/** UI 상태를 갱신한다 */
		private void UpdateUIsState() {
#if ENGINE_TEMPLATES_MODULE_ENABLE
			this.ResetBlockSprites();
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE

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
				
				m_oLevelInfoDict[EKey.SEL_LEVEL_INFO] = CLevelInfoTable.Inst.GetLevelInfo(KCDefine.B_VAL_0_INT);
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

#if GOOGLE_SHEET_ENABLE
				m_eSelTable = ETable.REMOTE;
#else
				m_eSelTable = ETable.LOCAL;
#endif			// #if GOOGLE_SHEET_ENABLE

				this.OnReceiveEditorResetPopupResult(null, true);
				this.OnReceiveEditorTableLoadPopupResult(null, true);
			}
		}

		/** 에디터 세트 테이블 로드 팝업 결과를 수신했을 경우 */
		private void OnReceiveEditorTableLoadPopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				switch(m_eSelTable) {
					case ETable.LOCAL: {
						CEpisodeInfoTable.Inst.LevelInfoDict.Clear();
						CEpisodeInfoTable.Inst.StageInfoDict.Clear();
						CEpisodeInfoTable.Inst.ChapterInfoDict.Clear();

						CEpisodeInfoTable.Inst.LoadEpisodeInfos();
						this.UpdateUIsState();
					} break;
					case ETable.REMOTE: {
#if GOOGLE_SHEET_ENABLE
						Func.LoadGoogleSheet(m_oEpisodeInfoTableGoogleSheetID, new List<(string, int)>() {
							(KCDefine.U_KEY_LEVEL, CLevelInfoTable.Inst.TotalNumLevelInfos + KCDefine.B_VAL_1_INT), (KCDefine.U_KEY_STAGE, CLevelInfoTable.Inst.TotalNumStageInfos + KCDefine.B_VAL_1_INT), (KCDefine.U_KEY_CHAPTER, CLevelInfoTable.Inst.NumChapterInfos + KCDefine.B_VAL_1_INT)
						}, this.OnLoadGoogleSheetEpisodeInfos);
#endif			// #if GOOGLE_SHEET_ENABLE
					} break;
				}
			}
		}

		/** 에디터 제거 팝업 결과를 수신했을 경우 */
		private void OnReceiveEditorRemovePopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				this.RemoveLevelInfos(m_oScrollerDict[EKey.SEL_SCROLLER], m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo);
				this.UpdateUIsState();
			}
		}

		/** 에디터 입력 팝업 결과를 수신했을 경우 */
		private void OnReceiveEditorInputPopupResult(CEditorInputPopup a_oSender, string a_oStr, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				switch(m_eSelInputPopup) {
					case EInputPopup.MOVE_LEVEL: this.HandleMoveLevelInputPopupResult(a_oStr); break;
					case EInputPopup.REMOVE_LEVEL: this.HandleRemoveLevelInputPopupResult(a_oStr); break;
				}
			}

			this.UpdateUIsState();
		}

		/** 에디터 레벨 생성 팝업 결과를 수신했을 경우 */
		private void OnReceiveEditorLevelCreatePopupResult(CEditorLevelCreatePopup a_oSender, CEditorLevelCreateInfo a_oCreateInfo, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
#if ENGINE_TEMPLATES_MODULE_ENABLE
				int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nStageID, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nChapterID);
				int nNumCreateLevelInfos = (nNumLevelInfos + a_oCreateInfo.m_nNumLevels < KCDefine.U_MAX_NUM_LEVEL_INFOS) ? a_oCreateInfo.m_nNumLevels : KCDefine.U_MAX_NUM_LEVEL_INFOS - nNumLevelInfos;

				for(int i = 0; i < nNumCreateLevelInfos; ++i) {
					var oLevelInfo = Factory.MakeLevelInfo(i + nNumLevelInfos, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nStageID, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nChapterID);
					m_oLevelInfoDict[EKey.SEL_LEVEL_INFO] = oLevelInfo;

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
			if(m_oTouchDispatcherDict[EKey.BG_TOUCH_DISPATCHER] == a_oSender) {
				// Do Something
			}
		}

		/** 터치를 움직였을 경우 */
		private void OnTouchMove(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 배경 터치 전달자 일 경우
			if(m_oTouchDispatcherDict[EKey.BG_TOUCH_DISPATCHER] == a_oSender) {
				// Do Something
			}
		}

		/** 터치를 종료했을 경우 */
		private void OnTouchEnd(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 배경 터치 전달자 일 경우
			if(m_oTouchDispatcherDict[EKey.BG_TOUCH_DISPATCHER] == a_oSender) {
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

		/** 레벨 정보를 제거한다 */
		private void RemoveLevelInfos(EnhancedScroller a_oScroller, STIDInfo a_stIDInfo) {
			var oLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(a_stIDInfo.m_nID, a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);

			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].Item1 == a_oScroller) {
				CLevelInfoTable.Inst.RemoveLevelInfo(a_stIDInfo.m_nID, a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);
			}
			// 스테이지 스크롤러 일 경우
			else if(m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].Item1 == a_oScroller) {
				CLevelInfoTable.Inst.RemoveStageLevelInfos(a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);
			}
			// 챕터 스크롤러 일 경우
			else if(m_oScrollerInfoDict[EKey.LE_UIS_CHAPTER_SCROLLER_INFO].Item1 == a_oScroller) {
				CLevelInfoTable.Inst.RemoveChapterLevelInfos(a_stIDInfo.m_nChapterID);
			}

			// 레벨 정보가 존재 할 경우
			if(!CLevelInfoTable.Inst.LevelInfoDictContainer.ExIsValid()) {
				m_oLevelInfoDict[EKey.SEL_LEVEL_INFO] = Factory.MakeLevelInfo(KCDefine.B_VAL_0_INT);
				CLevelInfoTable.Inst.AddLevelInfo(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO]);

				Func.SetupEditorLevelInfo(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO], new CSubEditorLevelCreateInfo() {
					m_nNumLevels = KCDefine.B_VAL_0_INT, m_stMinNumCells = SampleEngineName.KDefine.E_MIN_NUM_CELLS, m_stMaxNumCells = SampleEngineName.KDefine.E_MIN_NUM_CELLS
				});
			} else {
				CLevelInfo oSelLevelInfo = null;

				// 레벨 스크롤러 일 경우
				if(m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].Item1 == a_oScroller) {
					var stPrevIDInfo = CFactory.MakeIDInfo(a_stIDInfo.m_nID - KCDefine.B_VAL_1_INT, a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);
					var stNextIDInfo = CFactory.MakeIDInfo(a_stIDInfo.m_nID, a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);

					this.TryGetLevelInfo(stPrevIDInfo, stNextIDInfo, out oSelLevelInfo);
				}

				// 스테이지 스크롤러 일 경우
				if(oSelLevelInfo == null || m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].Item1 == a_oScroller) {
					var stPrevIDInfo = CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, a_stIDInfo.m_nStageID - KCDefine.B_VAL_1_INT, a_stIDInfo.m_nChapterID);
					var stNextIDInfo = CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);

					this.TryGetLevelInfo(stPrevIDInfo, stNextIDInfo, out oSelLevelInfo);
				}

				// 챕터 스크롤러 일 경우
				if(oSelLevelInfo == null || m_oScrollerInfoDict[EKey.LE_UIS_CHAPTER_SCROLLER_INFO].Item1 == a_oScroller) {
					var stPrevIDInfo = CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, a_stIDInfo.m_nChapterID - KCDefine.B_VAL_1_INT);
					var stNextIDInfo = CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, a_stIDInfo.m_nChapterID);

					this.TryGetLevelInfo(stPrevIDInfo, stNextIDInfo, out oSelLevelInfo);
				}

				m_oLevelInfoDict[EKey.SEL_LEVEL_INFO] = oSelLevelInfo;
			}

			this.UpdateUIsState();
		}

		/** 레벨 정보를 복사한다 */
		private void CopyLevelInfos(EnhancedScroller a_oScroller, STIDInfo a_stIDInfo) {
			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].Item1 == a_oScroller) {
				var oLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(a_stIDInfo.m_nID, a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);

				var oCloneLevelInfo = oLevelInfo.Clone() as CLevelInfo;
				oCloneLevelInfo.m_stIDInfo.m_nID = CLevelInfoTable.Inst.GetNumLevelInfos(a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);
				
				m_oLevelInfoDict[EKey.SEL_LEVEL_INFO] = oCloneLevelInfo;
				CLevelInfoTable.Inst.AddLevelInfo(oCloneLevelInfo);
			} else {
				// 스테이지 스크롤러 일 경우
				if(m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].Item1 == a_oScroller) {
					int nNumStageInfos = CLevelInfoTable.Inst.GetNumStageInfos(a_stIDInfo.m_nChapterID);
					var oStageLevelInfoDict = CLevelInfoTable.Inst.GetStageLevelInfos(a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);

					for(int i = 0; i < oStageLevelInfoDict.Count; ++i) {
						var oCloneLevelInfo = oStageLevelInfoDict[i].Clone() as CLevelInfo;
						oCloneLevelInfo.m_stIDInfo.m_nStageID = nNumStageInfos;

						CLevelInfoTable.Inst.AddLevelInfo(oCloneLevelInfo);
					}
				}
				// 챕터 스크롤러 일 경우
				else if(m_oScrollerInfoDict[EKey.LE_UIS_CHAPTER_SCROLLER_INFO].Item1 == a_oScroller) {
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
				int nStageID = (m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].Item1 == a_oScroller) ? CLevelInfoTable.Inst.GetNumStageInfos(a_stIDInfo.m_nChapterID) - KCDefine.B_VAL_1_INT : KCDefine.B_VAL_0_INT;
				int nChapterID = (m_oScrollerInfoDict[EKey.LE_UIS_CHAPTER_SCROLLER_INFO].Item1 == a_oScroller) ? CLevelInfoTable.Inst.NumChapterInfos - KCDefine.B_VAL_1_INT : a_stIDInfo.m_nChapterID;

				m_oLevelInfoDict[EKey.SEL_LEVEL_INFO] = CLevelInfoTable.Inst.GetLevelInfo(nID, nStageID, nChapterID);
			}

			m_oLevelInfoDict[EKey.SEL_LEVEL_INFO] = m_oLevelInfoDict[EKey.SEL_LEVEL_INFO] ?? CLevelInfoTable.Inst.GetLevelInfo(KCDefine.B_VAL_0_INT);
			this.UpdateUIsState();
		}

		/** 레벨 정보를 이동한다 */
		private void MoveLevelInfos(EnhancedScroller a_oScroller, STIDInfo a_stIDInfo, int a_nDestID) {
			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].Item1 == a_oScroller) {
				int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);
				CLevelInfoTable.Inst.MoveLevelInfo(a_stIDInfo.m_nID, Mathf.Clamp(a_nDestID, KCDefine.B_VAL_1_INT, nNumLevelInfos) - KCDefine.B_VAL_1_INT, a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID);
			}
			// 스테이지 스크롤러 일 경우
			else if(m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].Item1 == a_oScroller) {
				int nNumStageInfos = CLevelInfoTable.Inst.GetNumStageInfos(a_stIDInfo.m_nChapterID);
				CLevelInfoTable.Inst.MoveStageLevelInfos(a_stIDInfo.m_nStageID, Mathf.Clamp(a_nDestID, KCDefine.B_VAL_1_INT, nNumStageInfos) - KCDefine.B_VAL_1_INT, a_stIDInfo.m_nChapterID);
			}
			// 챕터 스크롤러 일 경우
			else if(m_oScrollerInfoDict[EKey.LE_UIS_CHAPTER_SCROLLER_INFO].Item1 == a_oScroller) {
				int nNumChapterInfos = CLevelInfoTable.Inst.NumChapterInfos;
				CLevelInfoTable.Inst.MoveChapterLevelInfos(a_stIDInfo.m_nChapterID, Mathf.Clamp(a_nDestID, KCDefine.B_VAL_1_INT, nNumChapterInfos) - KCDefine.B_VAL_1_INT);
			}
		}

		/** 알림을 출력한다 */
		private void ShowNoti(string a_oMsg) {
			this.MEUIsMsgUIs?.SetActive(true);
			m_oTextDict[EKey.ME_UIS_MSG_TEXT]?.ExSetText<Text>(a_oMsg, false);

			CScheduleManager.Inst.RemoveTimer(this);
			CScheduleManager.Inst.AddTimer(this, KCDefine.B_VAL_5_FLT, KCDefine.B_VAL_1_INT, () => this.MEUIsMsgUIs?.SetActive(false));
		}

		/** 에디터 레벨 이동 입력 팝업 결과를 처리한다 */
		private void HandleMoveLevelInputPopupResult(string a_oStr) {
			// 식별자가 유효 할 경우
			if(int.TryParse(a_oStr, out int nID)) {
				this.MoveLevelInfos(m_oScrollerDict[EKey.SEL_SCROLLER], m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo, nID);
			}
		}

		/** 에디터 레벨 제거 입력 팝업 결과를 처리한다 */
		private void HandleRemoveLevelInputPopupResult(string a_oStr) {
			var oTokenList = a_oStr.Split(KCDefine.B_TOKEN_DASH).ToList();
			int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nStageID, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nChapterID);

			// 식별자가 유효 할 경우
			if(oTokenList.Count > KCDefine.B_VAL_1_INT && (int.TryParse(oTokenList[KCDefine.B_VAL_0_INT], out int nMinID) && int.TryParse(oTokenList[KCDefine.B_VAL_1_INT], out int nMaxID))) {
				nMinID = Mathf.Clamp(nMinID, KCDefine.B_VAL_1_INT, nNumLevelInfos);
				nMaxID = Mathf.Clamp(nMaxID, KCDefine.B_VAL_1_INT, nNumLevelInfos);

				CFunc.LessCorrectSwap(ref nMinID, ref nMaxID);
				var stIDInfo = CFactory.MakeIDInfo(nMinID - KCDefine.B_VAL_1_INT, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nStageID, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nChapterID);

				for(int i = nMinID; i <= nMaxID; ++i) {
					// 레벨 정보가 존재 할 경우
					if(CLevelInfoTable.Inst.TryGetLevelInfo(stIDInfo.m_nID, out CLevelInfo oLevelInfo, stIDInfo.m_nStageID, stIDInfo.m_nChapterID)) {
						this.RemoveLevelInfos(m_oScrollerDict[EKey.SEL_SCROLLER], stIDInfo);
					}
				}
			}
		}

#if GOOGLE_SHEET_ENABLE
		/** 에피소드 정보 구글 시트를 로드했을 경우 */
		private void OnLoadGoogleSheetEpisodeInfos(CServicesManager a_oSender, GstuSpreadSheet a_oGoogleSheet, string a_oID, Dictionary<string, (SimpleJSON.JSONNode, bool)> a_oJSONNodeInfoDict) {
			var oResult = a_oJSONNodeInfoDict.ExFindVal((a_oJSONInfoDict) => !a_oJSONInfoDict.Item2);

			// 로드 되었을 경우
			if(!oResult.Item1) {
				var oJSONNode = new SimpleJSON.JSONClass();

				foreach(var stKeyVal in a_oJSONNodeInfoDict) {
					oJSONNode.Add(stKeyVal.Key, stKeyVal.Value.Item1);
				}

				CEpisodeInfoTable.Inst.ResetEpisodeInfos(oJSONNode.ToString());
				this.UpdateUIsState();
			} else {
				Func.ShowEditorGoogleSheetLoadPopup(null);
			}
		}
#endif			// #if GOOGLE_SHEET_ENABLE

#if ENGINE_TEMPLATES_MODULE_ENABLE
		/** 블럭 스프라이트를 리셋한다 */
		private void ResetBlockSprites() {
			// 블럭 스프라이트가 존재 할 경우
			if(m_oBlockSpriteInfoDictContainers.ExIsValid()) {
				for(int i = 0; i < m_oBlockSpriteInfoDictContainers.GetLength(KCDefine.B_VAL_0_INT); ++i) {
					for(int j = 0; j < m_oBlockSpriteInfoDictContainers.GetLength(KCDefine.B_VAL_1_INT); ++j) {
						this.ResetBlockSprites(m_oBlockSpriteInfoDictContainers[i, j]);
					}
				}
			}

			m_stGridInfo = SampleEngineName.Factory.MakeGridInfo(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO], Vector3.zero);

			// 비율을 설정한다 {
			bool bIsValid01 = !float.IsNaN(m_stGridInfo.m_stScale.x) && !float.IsInfinity(m_stGridInfo.m_stScale.x);
			bool bIsValid02 = !float.IsNaN(m_stGridInfo.m_stScale.y) && !float.IsInfinity(m_stGridInfo.m_stScale.y);
			bool bIsValid03 = !float.IsNaN(m_stGridInfo.m_stScale.z) && !float.IsInfinity(m_stGridInfo.m_stScale.z);

			this.BlockObjs.transform.localScale = (bIsValid01 && bIsValid02 && bIsValid03) ? m_stGridInfo.m_stScale : Vector3.one;
			this.BlockObjs.transform.localPosition = Vector3.zero.ExToWorld(this.MidEditorUIs).ExToLocal(this.UIs);
			// 비율을 설정한다 }

			// 블럭 스프라이트를 설정한다 {
			m_oBlockSpriteInfoDictContainers = new Dictionary<EBlockType, List<(EBlockKinds, SpriteRenderer)>>[m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].NumCells.y, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].NumCells.x];

			for(int i = 0; i < m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_oCellInfoDictContainer.Count; ++i) {
				for(int j = 0; j < m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_oCellInfoDictContainer[i].Count; ++j) {
					this.SetupBlockSprites(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_oCellInfoDictContainer[i][j], out Dictionary<EBlockType, List<(EBlockKinds, SpriteRenderer)>> oBlockSpriteInfoDictContainer);
					m_oBlockSpriteInfoDictContainers[m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_oCellInfoDictContainer[i][j].m_stIdx.y, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_oCellInfoDictContainer[i][j].m_stIdx.x] = oBlockSpriteInfoDictContainer;
				}
			}
			// 블럭 스프라이트를 설정한다 }
		}

		/** 블럭 스프라이트를 리셋한다 */
		private void ResetBlockSprites(Dictionary<EBlockType, List<(EBlockKinds, SpriteRenderer)>> a_oBlockSpriteInfoDictContainer) {
			foreach(var stKeyVal in a_oBlockSpriteInfoDictContainer) {
				for(int i = 0; i < stKeyVal.Value.Count; ++i) {
					this.DespawnObj(KDefine.LES_KEY_SPRITE_OBJS_POOL, stKeyVal.Value[i].Item2.gameObject);
				}
			}
		}

		/** 블럭 스프라이트를 설정한다 */
		private void SetupBlockSprites(CCellInfo a_oCellInfo, out Dictionary<EBlockType, List<(EBlockKinds, SpriteRenderer)>> a_oOutBlockSpriteInfoDictContainer) {
			a_oOutBlockSpriteInfoDictContainer = new Dictionary<EBlockType, List<(EBlockKinds, SpriteRenderer)>>();

			foreach(var stKeyVal in a_oCellInfo.m_oBlockKindsDictContainer) {
				var oBlockSpriteInfoList = new List<(EBlockKinds, SpriteRenderer)>();

				for(int i = 0; i < stKeyVal.Value.Count; ++i) {
					var oBlockSprite = this.SpawnObj<SpriteRenderer>(KDefine.LES_KEY_SPRITE_OBJS_POOL, KDefine.LES_OBJ_N_BLOCK_SPRITE);
					oBlockSprite.sprite = SampleEngineName.Access.GetBlockSprite(stKeyVal.Value[i]);
					oBlockSprite.transform.localPosition = m_stGridInfo.m_stPivotPos + a_oCellInfo.m_stIdx.ExToPos(SampleEngineName.KDefine.E_OFFSET_CELL, SampleEngineName.KDefine.E_SIZE_CELL);

					oBlockSprite.ExSetSortingOrder(SampleEngineName.Access.GetSortingOrderInfo(stKeyVal.Value[i]));
					oBlockSpriteInfoList.ExAddVal((stKeyVal.Value[i], oBlockSprite));
				}

				a_oOutBlockSpriteInfoDictContainer.TryAdd(stKeyVal.Key, oBlockSpriteInfoList);
			}
		}

		/** 레벨 정보를 추가한다 */
		private void AddLevelInfo(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
			m_oLevelInfoDict[EKey.SEL_LEVEL_INFO] = Factory.MakeLevelInfo(a_nID, a_nStageID, a_nChapterID);
			CLevelInfoTable.Inst.AddLevelInfo(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO]);

			Func.SetupEditorLevelInfo(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO], new CSubEditorLevelCreateInfo() {
				m_nNumLevels = KCDefine.B_VAL_0_INT, m_stMinNumCells = SampleEngineName.KDefine.E_MIN_NUM_CELLS, m_stMaxNumCells = SampleEngineName.KDefine.E_MIN_NUM_CELLS
			});

			this.UpdateUIsState();
		}
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE)
		#endregion			// 조건부 함수
	}

	/** 서브 레벨 에디터 씬 관리자 - 중앙 에디터 UI */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
		#region 조건부 함수
#if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE)
		/** 중앙 에디터 UI 를 설정한다 */
		private void SetupMidEditorUIs() {
			// 텍스트를 설정한다
			m_oTextDict[EKey.ME_UIS_MSG_TEXT] = this.MidEditorUIs.ExFindComponent<Text>(KCDefine.U_OBJ_N_MSG_TEXT);
			m_oTextDict[EKey.ME_UIS_LEVEL_TEXT] = this.MidEditorUIs.ExFindComponent<Text>(KCDefine.U_OBJ_N_LEVEL_TEXT);

			// 버튼을 설정한다 {
			m_oBtnDict[EKey.ME_UIS_PREV_BTN] = this.MidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_PREV_BTN);
			m_oBtnDict[EKey.ME_UIS_PREV_BTN]?.onClick.AddListener(this.OnTouchMEUIsPrevBtn);

			m_oBtnDict[EKey.ME_UIS_NEXT_BTN] = this.MidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_NEXT_BTN);
			m_oBtnDict[EKey.ME_UIS_NEXT_BTN]?.onClick.AddListener(this.OnTouchMEUIsNextBtn);

			m_oBtnDict[EKey.ME_UIS_MOVE_LEVEL_BTN] = this.MidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_MOVE_LEVEL_BTN);
			m_oBtnDict[EKey.ME_UIS_MOVE_LEVEL_BTN]?.onClick.AddListener(this.OnTouchMEUIsMoveLevelBtn);

			m_oBtnDict[EKey.ME_UIS_REMOVE_LEVEL_BTN] = this.MidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_REMOVE_LEVEL_BTN);
			m_oBtnDict[EKey.ME_UIS_REMOVE_LEVEL_BTN]?.onClick.AddListener(this.OnTouchMEUIsRemoveLevelBtn);

			this.MidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_SAVE_BTN)?.onClick.AddListener(this.OnTouchMEUIsSaveBtn);
			this.MidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_RESET_BTN)?.onClick.AddListener(this.OnTouchMEUIsResetBtn);
			this.MidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_TEST_BTN)?.onClick.AddListener(this.OnTouchMEUIsTestBtn);
			this.MidEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_ME_UIS_COPY_LEVEL_BTN)?.onClick.AddListener(this.OnTouchMEUIsCopyLevelBtn);
			// 버튼을 설정한다 }
		}

		/** 중앙 에디터 UI 상태를 갱신한다 */
		private void UpdateMidEditorUIsState() {
			// 텍스트를 갱신한다
			m_oTextDict[EKey.ME_UIS_LEVEL_TEXT]?.ExSetText<Text>(string.Format(KCDefine.LES_TEXT_FMT_LEVEL, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nID + KCDefine.B_VAL_1_INT), false);

			// 버튼을 갱신한다 {
			int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nStageID, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nChapterID);

			m_oBtnDict[EKey.ME_UIS_PREV_BTN]?.ExSetInteractable(CLevelInfoTable.Inst.TryGetLevelInfo(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nID - KCDefine.B_VAL_1_INT, out CLevelInfo oPrevLevelInfo, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nStageID, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nChapterID), false);
			m_oBtnDict[EKey.ME_UIS_NEXT_BTN]?.ExSetInteractable(CLevelInfoTable.Inst.TryGetLevelInfo(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nID + KCDefine.B_VAL_1_INT, out CLevelInfo oNextLevelInfo, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nStageID, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nChapterID), false);

			m_oBtnDict[EKey.ME_UIS_MOVE_LEVEL_BTN]?.ExSetInteractable(nNumLevelInfos > KCDefine.B_VAL_1_INT);
			m_oBtnDict[EKey.ME_UIS_REMOVE_LEVEL_BTN]?.ExSetInteractable(nNumLevelInfos > KCDefine.B_VAL_1_INT);
			// 버튼을 갱신한다 }
		}

		/** 중앙 에디터 UI 이전 레벨 버튼을 눌렀을 경우 */
		private void OnTouchMEUIsPrevBtn() {
			// 이전 레벨 정보가 존재 할 경우
			if(CLevelInfoTable.Inst.TryGetLevelInfo(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nID - KCDefine.B_VAL_1_INT, out CLevelInfo oPrevLevelInfo, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nStageID, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nChapterID)) {
				m_oLevelInfoDict[EKey.SEL_LEVEL_INFO] = oPrevLevelInfo;
				this.UpdateUIsState();
			}
		}

		/** 중앙 에디터 UI 다음 레벨 버튼을 눌렀을 경우 */
		private void OnTouchMEUIsNextBtn() {
			// 다음 레벨 정보가 존재 할 경우
			if(CLevelInfoTable.Inst.TryGetLevelInfo(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nID + KCDefine.B_VAL_1_INT, out CLevelInfo oNextLevelInfo, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nStageID, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nChapterID)) {
				m_oLevelInfoDict[EKey.SEL_LEVEL_INFO] = oNextLevelInfo;
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
			CGameInfoStorage.Inst.SetupPlayLevelInfo(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nID, EPlayMode.TEST, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nStageID, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nChapterID);
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME);
		}

		/** 중앙 에디터 UI 레벨 복사 버튼을 눌렀을 경우 */
		private void OnTouchMEUIsCopyLevelBtn() {
			int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nStageID, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nChapterID);

			// 레벨 추가가 가능 할 경우
			if(nNumLevelInfos < KCDefine.U_MAX_NUM_LEVEL_INFOS) {
				var stIDInfo = CFactory.MakeIDInfo(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nID, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nStageID, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nChapterID);
				this.CopyLevelInfos(m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].Item1, stIDInfo);
			}
		}

		/** 중앙 에디터 UI 레벨 이동 버튼을 눌렀을 경우 */
		private void OnTouchMEUIsMoveLevelBtn() {
			m_oScrollerDict[EKey.SEL_SCROLLER] = m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].Item1;
			m_eSelInputPopup = EInputPopup.MOVE_LEVEL;

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
			m_oScrollerDict[EKey.SEL_SCROLLER] = m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].Item1;
			Func.ShowEditorLevelRemovePopup(this.OnReceiveEditorRemovePopupResult);
		}
#endif			// #if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE)
		#endregion			// 조건부 함수
	}

	/** 서브 레벨 에디터 씬 관리자 - 왼쪽 에디터 UI */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
		#region 조건부 함수
#if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE)
		/** 왼쪽 에디터 UI 를 설정한다 */
		private void SetupLeftEditorUIs() {
			// 스크롤 뷰를 설정한다 {
			var oStageScroller01 = this.LeftEditorUIs.ExFindComponent<EnhancedScroller>(KCDefine.LES_OBJ_N_LE_UIS_STAGE_SCROLL_VIEW_01);
			oStageScroller01?.gameObject.SetActive(false);

			var oStageScroller02 = this.LeftEditorUIs.ExFindComponent<EnhancedScroller>(KCDefine.LES_OBJ_N_LE_UIS_STAGE_SCROLL_VIEW_02);
			oStageScroller02?.gameObject.SetActive(false);

			m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO] = (
				this.LeftEditorUIs.ExFindComponent<EnhancedScroller>(KCDefine.U_OBJ_N_LEVEL_SCROLL_VIEW),
				CResManager.Inst.GetRes<GameObject>(KCDefine.E_OBJ_P_LEVEL_EDITOR_SCROLLER_CELL_VIEW)?.GetComponentInChildren<EnhancedScrollerCellView>()
			);

			m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO] = (
				oStageScroller01,
				CResManager.Inst.GetRes<GameObject>(KCDefine.E_OBJ_P_STAGE_EDITOR_SCROLLER_CELL_VIEW)?.GetComponentInChildren<EnhancedScrollerCellView>()
			);

			m_oScrollerInfoDict[EKey.LE_UIS_CHAPTER_SCROLLER_INFO] = (
				this.LeftEditorUIs.ExFindComponent<EnhancedScroller>(KCDefine.U_OBJ_N_CHAPTER_SCROLL_VIEW),
				CResManager.Inst.GetRes<GameObject>(KCDefine.E_OBJ_P_CHAPTER_EDITOR_SCROLLER_CELL_VIEW)?.GetComponentInChildren<EnhancedScrollerCellView>()
			);

			m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].Item1?.gameObject.SetActive(true);
			m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].Item1?.gameObject.SetActive(false);
			m_oScrollerInfoDict[EKey.LE_UIS_CHAPTER_SCROLLER_INFO].Item1?.gameObject.SetActive(false);

			foreach(var stKeyVal in m_oScrollerInfoDict) {
				stKeyVal.Value.Item1?.ExSetDelegate(this, false);
			}
			// 스크롤 뷰를 설정한다 }

			// 버튼을 설정한다 {
			var oAddStageBtn = this.LeftEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_LE_UIS_ADD_STAGE_BTN);
			oAddStageBtn?.onClick.AddListener(this.OnTouchLEUIsAddStageBtn);

			var oAddChapterBtn = this.LeftEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_LE_UIS_ADD_CHAPTER_BTN);
			oAddChapterBtn?.onClick.AddListener(this.OnTouchLEUIsAddChapterBtn);

			this.LeftEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_LE_UIS_ADD_LEVEL_BTN)?.onClick.AddListener(this.OnTouchLEUIsAddLevelBtn);

#if AB_TEST_ENABLE
			m_oBtnDict[EKey.LE_UIS_A_SET_BTN] = this.LeftEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_LE_UIS_A_SET_BTN);
			m_oBtnDict[EKey.LE_UIS_A_SET_BTN]?.onClick.AddListener(() => this.OnTouchLEUIsSetBtn(m_oBtnDict[EKey.LE_UIS_A_SET_BTN]));

			m_oBtnDict[EKey.LE_UIS_B_SET_BTN] = this.LeftEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_LE_UIS_B_SET_BTN);
			m_oBtnDict[EKey.LE_UIS_B_SET_BTN]?.onClick.AddListener(() => this.OnTouchLEUIsSetBtn(m_oBtnDict[EKey.LE_UIS_B_SET_BTN]));
#endif			// #if AB_TEST_ENABLE

			this.ExLateCallFunc((a_oSender) => {
#if AB_TEST_ENABLE
				this.LEUIsABSetUIs?.SetActive(true);
#else
				this.LEUIsABSetUIs?.SetActive(false);
#endif			// #if AB_TEST_ENABLE

				oAddStageBtn?.ExSetInteractable((oStageScroller01 != null && oStageScroller01.gameObject.activeSelf) || (oStageScroller02 != null && oStageScroller02.gameObject.activeSelf));
				oAddChapterBtn?.ExSetInteractable(m_oScrollerInfoDict[EKey.LE_UIS_CHAPTER_SCROLLER_INFO].Item1 != null && m_oScrollerInfoDict[EKey.LE_UIS_CHAPTER_SCROLLER_INFO].Item1.gameObject.activeSelf);
			});
			// 버튼을 설정한다 }
		}

		/** 왼쪽 에디터 UI 상태를 갱신한다 */
		private void UpdateLeftEditorUIsState() {
			// 버튼을 설정한다 {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
			m_oBtnDict[EKey.LE_UIS_A_SET_BTN]?.image.ExSetColor<Image>((CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.A) ? Color.yellow : Color.white, false);
			m_oBtnDict[EKey.LE_UIS_B_SET_BTN]?.image.ExSetColor<Image>((CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.B) ? Color.yellow : Color.white, false);
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
			// 버튼을 설정한다 }

			// 스크롤 뷰를 갱신한다
			m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].Item1?.ExReloadData(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nID - KCDefine.B_VAL_1_INT, false);
			m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].Item1?.ExReloadData(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nStageID - KCDefine.B_VAL_1_INT, false);
			m_oScrollerInfoDict[EKey.LE_UIS_CHAPTER_SCROLLER_INFO].Item1?.ExReloadData(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nChapterID - KCDefine.B_VAL_1_INT, false);
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
			int nNumStageInfos = CLevelInfoTable.Inst.GetNumStageInfos(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nChapterID);

			// 스테이지 추가가 가능 할 경우
			if(nNumStageInfos < KCDefine.U_MAX_NUM_STAGE_INFOS) {
#if ENGINE_TEMPLATES_MODULE_ENABLE
				this.AddLevelInfo(KCDefine.B_VAL_0_INT, nNumStageInfos, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nChapterID);
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
			}
		}

		/** 왼쪽 에디터 UI 챕터 추가 버튼을 눌렀을 경우 */
		private void OnTouchLEUIsAddChapterBtn() {
			int nNumChapterInfos = CLevelInfoTable.Inst.NumChapterInfos;

			// 챕터 추가가 가능 할 경우
			if(nNumChapterInfos < KCDefine.U_MAX_NUM_CHAPTER_INFOS) {
#if ENGINE_TEMPLATES_MODULE_ENABLE
				this.AddLevelInfo(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, nNumChapterInfos);
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
			}
		}

#if AB_TEST_ENABLE
		/** 왼쪽 에디터 UI 세트 버튼을 눌렀을 경우 */
		private void OnTouchLEUIsSetBtn(Button a_oSender) {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
			var eUserType = (a_oSender == m_oBtnDict[EKey.LE_UIS_A_SET_BTN]) ? EUserType.A : EUserType.B;

			// 유저 타입이 다를 경우
			if(CCommonUserInfoStorage.Inst.UserInfo.UserType != eUserType) {
				m_eSelUserType = eUserType;

				// A 세트 버튼 일 경우
				if(a_oSender == m_oBtnDict[EKey.LE_UIS_A_SET_BTN]) {
					Func.ShowEditorASetPopup(this.OnReceiveEditorSetPopupResult);
				} else {
					Func.ShowEditorBSetPopup(this.OnReceiveEditorSetPopupResult);
				}
			}
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
		}
#endif			// #if AB_TEST_ENABLE
#endif			// #if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE)
		#endregion			// 조건부 함수
	}

	/** 서브 레벨 에디터 씬 관리자 - 오른쪽 에디터 UI */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
		#region 조건부 함수
#if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE)
		/** 오른족 에디터 UI 를 설정한다 */
		private void SetupRightEditorUIs() {
			// 텍스트를 설정한다
			m_oTextDict[EKey.RE_UIS_PAGE_TEXT] = this.RightEditorUIs.ExFindComponent<Text>(KCDefine.U_OBJ_N_PAGE_TEXT);
			m_oTextDict[EKey.RE_UIS_TITLE_TEXT] = this.RightEditorUIs.ExFindComponent<Text>(KCDefine.U_OBJ_N_TITLE_TEXT);

			// 버튼을 설정한다 {
			m_oBtnDict[EKey.RE_UIS_PREV_BTN] = this.RightEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_RE_UIS_PREV_BTN);
			m_oBtnDict[EKey.RE_UIS_NEXT_BTN] = this.RightEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_RE_UIS_NEXT_BTN);

			m_oBtnDict[EKey.RE_UIS_REMOVE_ALL_LEVELS_BTN] = this.RightEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_RE_UIS_REMOVE_ALL_LEVELS_BTN);
			m_oBtnDict[EKey.RE_UIS_REMOVE_ALL_LEVELS_BTN]?.onClick.AddListener(this.OnTouchREUIsRemoveAllLevelsBtn);

			m_oBtnDict[EKey.RE_UIS_LOAD_REMOTE_TABLE_BTN] = this.RightEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_RE_UIS_LOAD_REMOTE_TABLE_BTN);
			m_oBtnDict[EKey.RE_UIS_LOAD_REMOTE_TABLE_BTN]?.onClick.AddListener(() => this.OnTouchREUIsLoadTableBtn(ETable.REMOTE));

			this.RightEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_RE_UIS_APPLY_BTN)?.onClick.AddListener(this.OnTouchREUIsApplyBtn);
			this.RightEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_RE_UIS_LOAD_LEVEL_BTN)?.onClick.AddListener(this.OnTouchREUIsLoadLevelBtn);
			this.RightEditorUIs.ExFindComponent<Button>(KCDefine.LES_OBJ_N_RE_UIS_LOAD_LOCAL_TABLE_BTN)?.onClick.AddListener(() => this.OnTouchREUIsLoadTableBtn(ETable.LOCAL));
			// 버튼을 설정한다 }

			// 스크롤 뷰를 설정한다 {
			m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP] = this.RightEditorUIs.ExFindComponent<SimpleScrollSnap>(KCDefine.LES_OBJ_N_RE_UIS_PAGE_VIEW);
			m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP]?.OnPanelCentered.AddListener((a_nCenterIdx, a_nSelIdx) => this.UpdateUIsState());

			for(int i = 0; i < m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].NumberOfPanels; ++i) {
				string oSetupFuncName = string.Format(KDefine.LES_FUNC_N_FMT_SETUP_RE_UIS_PAGE_UIS, i + KCDefine.B_VAL_1_INT);
				string oUpdateFuncName = string.Format(KDefine.LES_FUNC_N_FMT_UPDATE_RE_UIS_PAGE_UIS, i + KCDefine.B_VAL_1_INT);

				m_oMethodInfoDict.TryAdd(ECallback.SETUP_RE_UIS_PAGE_UIS_01 + i, this.GetType().GetMethod(oSetupFuncName, KCDefine.B_BINDING_F_NON_PUBLIC_INSTANCE));
				m_oMethodInfoDict.TryAdd(ECallback.UPDATE_RE_UIS_PAGE_UIS_01 + i, this.GetType().GetMethod(oUpdateFuncName, KCDefine.B_BINDING_F_NON_PUBLIC_INSTANCE));
			}

			for(int i = 0; i < m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].NumberOfPanels; ++i) {
				string oPageUIsName = string.Format(KDefine.LES_OBJ_N_FMT_RE_UIS_PAGE_UIS, i + KCDefine.B_VAL_1_INT);
				m_oUIsDict.TryAdd(EKey.RE_UIS_PAGE_UIS_01 + i, m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].gameObject.ExFindChild(oPageUIsName));
			}

			for(int i = 0; i < m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].NumberOfPanels; ++i) {
				m_oMethodInfoDict[ECallback.SETUP_RE_UIS_PAGE_UIS_01 + i]?.Invoke(this, null);
			}
			// 스크롤 뷰를 설정한다 }
		}

		/** 오른쪽 에디터 UI 페이지 UI 1 를 설정한다 */
		private void SetupREUIsPageUIs01() {
			// 입력 필드를 설정한다 {
			m_oInputDict[EKey.RE_UIS_PAGE_UIS_01_LEVEL_INPUT] = this.RightEditorUIs.ExFindComponent<InputField>(KCDefine.LES_OBJ_N_RE_UIS_LEVEL_INPUT);

			m_oInputDict[EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_X_INPUT] = this.RightEditorUIs.ExFindComponent<InputField>(KCDefine.LES_OBJ_N_RE_UIS_NUM_CELLS_X_INPUT);
			m_oInputDict[EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_Y_INPUT] = this.RightEditorUIs.ExFindComponent<InputField>(KCDefine.LES_OBJ_N_RE_UIS_NUM_CELLS_Y_INPUT);
			// 입력 필드를 설정한다 }
		}

		/** 오른쪽 에디터 UI 상태를 갱신한다 */
		private void UpdateRightEditorUIsState() {
			// 텍스트를 설정한다
			int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nStageID, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nChapterID);
			m_oTextDict[EKey.RE_UIS_TITLE_TEXT]?.ExSetText<Text>(string.Format(CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_LEVEL_PAGE_TEXT_FMT), m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nID + KCDefine.B_VAL_1_INT, nNumLevelInfos), false);

			// 버튼을 설정한다 {
			m_oBtnDict[EKey.RE_UIS_REMOVE_ALL_LEVELS_BTN]?.ExSetInteractable(nNumLevelInfos > KCDefine.B_VAL_1_INT, false);

#if GOOGLE_SHEET_ENABLE
			m_oBtnDict[EKey.RE_UIS_LOAD_REMOTE_TABLE_BTN]?.ExSetInteractable(true, false);
#else
			m_oBtnDict[EKey.RE_UIS_LOAD_REMOTE_TABLE_BTN]?.ExSetInteractable(false, false);
#endif			// #if GOOGLE_SHEET_ENABLE
			// 버튼을 설정한다 }

			// 스크롤 스냅이 존재 할 경우
			if(m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP] != null) {
				// 텍스트를 설정한다
				m_oTextDict[EKey.RE_UIS_PAGE_TEXT]?.ExSetText<Text>(string.Format(KCDefine.B_TEXT_FMT_2_SLASH_COMBINE, m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].CenteredPanel + KCDefine.B_VAL_1_INT, m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].NumberOfPanels), false);

				// 버튼 상태를 갱신한다
				m_oBtnDict[EKey.RE_UIS_PREV_BTN]?.ExSetInteractable(m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].CenteredPanel > KCDefine.B_VAL_0_INT, false);
				m_oBtnDict[EKey.RE_UIS_NEXT_BTN]?.ExSetInteractable(m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].CenteredPanel < m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].NumberOfPanels - KCDefine.B_VAL_1_INT, false);
			}

			// 페이지 UI 상태를 갱신한다
			for(int i = 0; i < m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].NumberOfPanels; ++i) {
				m_oMethodInfoDict[ECallback.UPDATE_RE_UIS_PAGE_UIS_01 + i]?.Invoke(this, null);
			}
		}

		/** 오른쪽 에디터 UI 페이지 UI 1 상태를 갱신한다 */
		private void UpdateREUIsPageUIs01() {
			// 입력 필드를 갱신한다 {
			m_oInputDict[EKey.RE_UIS_PAGE_UIS_01_LEVEL_INPUT]?.ExSetText<InputField>($"{m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nID + KCDefine.B_VAL_1_INT}", false);

			m_oInputDict[EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_X_INPUT]?.ExSetText<InputField>((m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].NumCells.x <= KCDefine.B_VAL_0_INT) ? string.Empty : $"{m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].NumCells.x}", false);
			m_oInputDict[EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_Y_INPUT]?.ExSetText<InputField>((m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].NumCells.y <= KCDefine.B_VAL_0_INT) ? string.Empty : $"{m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].NumCells.y}", false);
			// 입력 필드를 갱신한다 }
		}

		/** 오른쪽 에디터 UI 적용 버튼을 눌렀을 경우 */
		private void OnTouchREUIsApplyBtn() {
#if ENGINE_TEMPLATES_MODULE_ENABLE
			bool bIsValid01 = int.TryParse(m_oInputDict[EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_X_INPUT]?.text, out int nNumCellsX);
			bool bIsValid02 = int.TryParse(m_oInputDict[EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_Y_INPUT]?.text, out int nNumCellsY);

			bool bIsValidNumCellsX = Mathf.Max(nNumCellsX, SampleEngineName.KDefine.E_MIN_NUM_CELLS.x) != m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].NumCells.x;
			bool bIsValidNumCellsY = Mathf.Max(nNumCellsY, SampleEngineName.KDefine.E_MIN_NUM_CELLS.y) != m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].NumCells.y;

			// 셀 개수가 유효 할 경우
			if(bIsValid01 && bIsValid02 && (bIsValidNumCellsX || bIsValidNumCellsY)) {
				Func.SetupEditorLevelInfo(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO], new CSubEditorLevelCreateInfo() {
					m_nNumLevels = KCDefine.B_VAL_0_INT, m_stMinNumCells = new Vector3Int(nNumCellsX, nNumCellsY, KCDefine.B_VAL_0_INT), m_stMaxNumCells = new Vector3Int(nNumCellsX, nNumCellsY, KCDefine.B_VAL_0_INT)
				});
				
				this.UpdateUIsState();
			}
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
		}

		/** 오른쪽 에디터 UI 레벨 로드 버튼을 눌렀을 경우 */
		private void OnTouchREUIsLoadLevelBtn() {
			// 식별자가 유효 할 경우
			if(int.TryParse(m_oInputDict[EKey.RE_UIS_PAGE_UIS_01_LEVEL_INPUT]?.text, out int nID)) {
				int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nStageID, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nChapterID);
				m_oLevelInfoDict[EKey.SEL_LEVEL_INFO] = CLevelInfoTable.Inst.GetLevelInfo(Mathf.Clamp(nID, KCDefine.B_VAL_1_INT, nNumLevelInfos) - KCDefine.B_VAL_1_INT, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nStageID, m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nChapterID);
				
				this.UpdateUIsState();
			}
		}

		/** 오른쪽 에디터 UI 모든 레벨 제거 버튼을 눌렀을 경우 */
		private void OnTouchREUIsRemoveAllLevelsBtn() {
			m_eSelInputPopup = EInputPopup.REMOVE_LEVEL;
			m_oScrollerDict[EKey.SEL_SCROLLER] = m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].Item1;

			Func.ShowEditorInputPopup(this.PopupUIs, (a_oSender) => {
				var stParams = new CEditorInputPopup.STParams() {
					m_oCallbackDict = new Dictionary<CEditorInputPopup.ECallback, System.Action<CEditorInputPopup, string, bool>>() {
						[CEditorInputPopup.ECallback.OK_CANCEL] = this.OnReceiveEditorInputPopupResult
					}
				};

				(a_oSender as CEditorInputPopup).Init(stParams);
			});
		}

		/** 오른쪽 에디터 UI 테이블 로드 버튼을 눌렀을 경우 */
		private void OnTouchREUIsLoadTableBtn(ETable a_eTable) {
			m_eSelTable = a_eTable;
			Func.ShowEditorTableLoadPopup(this.OnReceiveEditorTableLoadPopupResult);
		}
#endif			// #if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE)
		#endregion			// 조건부 함수
	}

	/** 서브 레벨 에디터 씬 관리자 - 스크롤러 셀 뷰 */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
		#region 조건부 함수
#if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE)
		/** 스크롤러 셀 뷰 선택 버튼을 눌렀을 경우 */
		private void OnTouchSCVSelBtn(CScrollerCellView a_oSender, long a_nID) {
			m_oLevelInfoDict[EKey.SEL_LEVEL_INFO] = CLevelInfoTable.Inst.GetLevelInfo(a_nID.ExUniqueLevelIDToID(), a_nID.ExUniqueLevelIDToStageID(), a_nID.ExUniqueLevelIDToChapterID());
			this.UpdateUIsState();
		}

		/** 스크롤러 셀 뷰 복사 버튼을 눌렀을 경우 */
		private void OnTouchSCVCopyBtn(CScrollerCellView a_oSender, long a_nID) {
			int nNumInfos = CLevelInfoTable.Inst.GetNumLevelInfos(a_nID.ExUniqueLevelIDToStageID(), a_nID.ExUniqueLevelIDToChapterID());
			int nMaxNumInfos = KCDefine.U_MAX_NUM_LEVEL_INFOS;

			// 레벨 스크롤러가 아닐 경우
			if(m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].Item1 != a_oSender.Scroller) {
				nNumInfos = (m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].Item1 == a_oSender.Scroller) ? CLevelInfoTable.Inst.GetNumStageInfos(a_nID.ExUniqueLevelIDToChapterID()) : CLevelInfoTable.Inst.NumChapterInfos;
				nMaxNumInfos = (m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].Item1 == a_oSender.Scroller) ? KCDefine.U_MAX_NUM_STAGE_INFOS : KCDefine.U_MAX_NUM_CHAPTER_INFOS;
			}

			// 복사가 가능 할 경우
			if(nNumInfos < nMaxNumInfos) {
				this.CopyLevelInfos(a_oSender.Scroller, CFactory.MakeIDInfo(a_nID.ExUniqueLevelIDToID(), a_nID.ExUniqueLevelIDToStageID(), a_nID.ExUniqueLevelIDToChapterID()));
			}
		}

		/** 스크롤러 셀 뷰 이동 버튼을 눌렀을 경우 */
		private void OnTouchSCVMoveBtn(CScrollerCellView a_oSender, long a_nID) {
			m_eSelInputPopup = EInputPopup.MOVE_LEVEL;
			m_oScrollerDict[EKey.SEL_SCROLLER] = a_oSender.Scroller;

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
			m_oScrollerDict[EKey.SEL_SCROLLER] = a_oSender.Scroller;

			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].Item1 == a_oSender.Scroller) {
				Func.ShowEditorLevelRemovePopup(this.OnReceiveEditorRemovePopupResult);
			}
			// 스테이지 스크롤러 일 경우
			else if(m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].Item1 == a_oSender.Scroller) {
				Func.ShowEditorStageRemovePopup(this.OnReceiveEditorRemovePopupResult);
			}
			// 챕터 스크롤러 일 경우
			else if(m_oScrollerInfoDict[EKey.LE_UIS_CHAPTER_SCROLLER_INFO].Item1 == a_oSender.Scroller) {
				Func.ShowEditorChapterRemovePopup(this.OnReceiveEditorRemovePopupResult);
			}
		}
#endif			// #if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE)
		#endregion			// 조건부 함수
	}
}
#endif			// #if UNITY_STANDALONE && EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
#endif			// #if SCRIPT_TEMPLATE_ONLY
