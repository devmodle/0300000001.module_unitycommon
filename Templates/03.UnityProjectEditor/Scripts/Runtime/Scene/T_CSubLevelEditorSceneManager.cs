#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#if UNITY_STANDALONE && EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
using GoogleSheetsToUnity;
using EnhancedUI.EnhancedScroller;
using DanielLochner.Assets.SimpleScrollSnap;

#if INPUT_SYSTEM_MODULE_ENABLE
using UnityEngine.InputSystem;
#endif			// #if INPUT_SYSTEM_MODULE_ENABLE

namespace LevelEditorScene {
	/** 서브 레벨 에디터 씬 관리자 */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			SEL_GRID_INFO,
			SEL_USER_TYPE,
			SEL_TABLE_SRC,
			SEL_INPUT_POPUP,

			SEL_SCROLLER,
			SEL_OBJ_SPRITE,
			
			ME_UIS_MSG_TEXT,
			ME_UIS_LEVEL_TEXT,

			ME_UIS_PREV_BTN,
			ME_UIS_NEXT_BTN,
			ME_UIS_MOVE_LEVEL_BTN,
			ME_UIS_REMOVE_LEVEL_BTN,

			LE_UIS_A_SET_BTN,
			LE_UIS_B_SET_BTN,
			LE_UIS_ADD_STAGE_BTN,
			LE_UIS_ADD_CHAPTER_BTN,

			LE_UIS_LEVEL_SCROLLER_INFO,
			LE_UIS_STAGE_SCROLLER_INFO,
			LE_UIS_CHAPTER_SCROLLER_INFO,

			LE_UIS_STAGE_SCROLLER_INFO_01,
			LE_UIS_STAGE_SCROLLER_INFO_02,

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

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			SEL_LEVEL_INFO,
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

			[HideInInspector] MAX_VAL
		}

		/** 테이블 소스 */
		private enum ETableSrc {
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
		private Dictionary<EKey, EUserType> m_oUserTypeDict = new Dictionary<EKey, EUserType>();
		private Dictionary<EKey, ETableSrc> m_oTableSrcDict = new Dictionary<EKey, ETableSrc>();
		private Dictionary<EKey, EInputPopup> m_oInputPopupDict = new Dictionary<EKey, EInputPopup>();
		private Dictionary<EKey, SpriteRenderer> m_oSpriteDict = new Dictionary<EKey, SpriteRenderer>();
		private Dictionary<ECallback, System.Reflection.MethodInfo> m_oMethodInfoDict = new Dictionary<ECallback, System.Reflection.MethodInfo>();

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		private Dictionary<EObjType, List<(EObjKinds, SpriteRenderer)>>[,] m_oObjSpriteInfoDictContainers = null;
		private Dictionary<EKey, SampleEngineName.STGridInfo> m_oGridInfoDict = new Dictionary<EKey, SampleEngineName.STGridInfo>();
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		private Dictionary<EKey, CLevelInfo> m_oLevelInfoDict = new Dictionary<EKey, CLevelInfo>();
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

		/** =====> UI <===== */
		private Dictionary<EKey, Text> m_oTextDict = new Dictionary<EKey, Text>();
		private Dictionary<EKey, InputField> m_oInputDict = new Dictionary<EKey, InputField>();
		private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>();
		private Dictionary<EKey, EnhancedScroller> m_oScrollerDict = new Dictionary<EKey, EnhancedScroller>();
		private Dictionary<EKey, SimpleScrollSnap> m_oScrollSnapDict = new Dictionary<EKey, SimpleScrollSnap>();
		private Dictionary<EKey, (EnhancedScroller, EnhancedScrollerCellView)> m_oScrollerInfoDict = new Dictionary<EKey, (EnhancedScroller, EnhancedScrollerCellView)>();

		/** =====> 객체 <===== */
		private Dictionary<EKey, GameObject> m_oUIsDict = new Dictionary<EKey, GameObject>();
		#endregion			// 변수

		#region 상수
		private const string ID_ETC_INFO_TABLE_GOOGLE_SHEET = "1YKF5m1_8zvZe5ZEJ-A_nqQq7qQq5nW8vX2OW5iMp9AA";
		private const string ID_OBJ_INFO_TABLE_GOOGLE_SHEET = "12pVPEnja4xIzCa-Bffl72HpJPZTDA4wlZOCdWgR9NhQ";
		#endregion			// 상수
		
		#region IEnhancedScrollerDelegate
		/** 셀 개수를 반환한다 */
		public virtual int GetNumberOfCells(EnhancedScroller a_oSender) {
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_LEVEL_SCROLLER_INFO).Item1 == a_oSender) {
				return CLevelInfoTable.Inst.GetNumLevelInfos(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID02, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID03);
			}

			return (m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_STAGE_SCROLLER_INFO).Item1 == a_oSender) ? CLevelInfoTable.Inst.GetNumStageInfos(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID03) : CLevelInfoTable.Inst.NumChapterInfos;
#else
			return KCDefine.B_VAL_0_INT;
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		}

		/** 셀 뷰 크기를 반환한다 */
		public virtual float GetCellViewSize(EnhancedScroller a_oSender, int a_nDataIdx) {
			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_LEVEL_SCROLLER_INFO).Item1 == a_oSender) {
				return (m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_LEVEL_SCROLLER_INFO).Item2.transform as RectTransform).sizeDelta.y;
			}

			return (m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_STAGE_SCROLLER_INFO).Item1 == a_oSender) ? (m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_STAGE_SCROLLER_INFO).Item2.transform as RectTransform).sizeDelta.y : (m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_CHAPTER_SCROLLER_INFO).Item2.transform as RectTransform).sizeDelta.y;
		}

		/** 셀 뷰를 반환한다 */
		public virtual EnhancedScrollerCellView GetCellView(EnhancedScroller a_oSender, int a_nDataIdx, int a_nCellIdx) {
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			var stColor = (m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID01 == a_nDataIdx) ? KCDefine.U_COLOR_NORM : KCDefine.U_COLOR_DISABLE;
			var stIDInfo = CFactory.MakeIDInfo(a_nDataIdx, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID02, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID03);
			var oOriginScrollerCellView = m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_LEVEL_SCROLLER_INFO).Item2;

			int nNumInfos = CLevelInfoTable.Inst.GetNumLevelInfos(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID02, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID03);

			string oNameFmt = KCDefine.LES_TEXT_FMT_LEVEL;
			string oNumInfosStr = string.Empty;

			// 스테이지 스크롤러 일 경우
			if(m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_STAGE_SCROLLER_INFO).Item1 == a_oSender) {
				nNumInfos = CLevelInfoTable.Inst.GetNumStageInfos(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID03);
				oNameFmt = KCDefine.LES_TEXT_FMT_STAGE;
				oNumInfosStr = string.Format(KCDefine.B_TEXT_FMT_BRACKET, CLevelInfoTable.Inst.GetNumLevelInfos(a_nDataIdx, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID03));

				stColor = (m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID02 == a_nDataIdx) ? KCDefine.U_COLOR_NORM : KCDefine.U_COLOR_DISABLE;
				stIDInfo = CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, a_nDataIdx, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID03);
				oOriginScrollerCellView = m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_STAGE_SCROLLER_INFO).Item2;
			}
			// 챕터 스크롤러 일 경우
			else if(m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_CHAPTER_SCROLLER_INFO).Item1 == a_oSender) {
				nNumInfos = CLevelInfoTable.Inst.NumChapterInfos;
				oNameFmt = KCDefine.LES_TEXT_FMT_CHAPTER;
				oNumInfosStr = string.Format(KCDefine.B_TEXT_FMT_BRACKET, CLevelInfoTable.Inst.GetNumStageInfos(a_nDataIdx));

				stColor = (m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID03 == a_nDataIdx) ? KCDefine.U_COLOR_NORM : KCDefine.U_COLOR_DISABLE;
				stIDInfo = CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, a_nDataIdx);
				oOriginScrollerCellView = m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_CHAPTER_SCROLLER_INFO).Item2;
			}

			var stParams = new CEditorScrollerCellView.STParams() {
				m_stBaseParams = new CScrollerCellView.STParams() {
					m_nID = CFactory.MakeULevelID(stIDInfo.m_nID01, stIDInfo.m_nID02, stIDInfo.m_nID03),
					m_oScroller = a_oSender,

					m_oCallbackDict = new Dictionary<CScrollerCellView.ECallback, System.Action<CScrollerCellView, ulong>>() {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
						[CScrollerCellView.ECallback.SEL] = this.OnTouchSCVSelBtn
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
					}
				},

				m_oCallbackDict = new Dictionary<CEditorScrollerCellView.ECallback, System.Action<CEditorScrollerCellView, ulong>>() {
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
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		}
		#endregion			// IEnhancedScrollerDelegate

		#region 함수
		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
#if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE)
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
					m_oScrollSnapDict.GetValueOrDefault(EKey.RE_UIS_PAGE_SCROLL_SNAP)?.GoToPreviousPanel();
				}
				// 다음 페이지 키를 눌렀을 경우
				else if(Keyboard.current.leftShiftKey.isPressed && Keyboard.current.rightArrowKey.wasPressedThisFrame) {
					m_oScrollSnapDict.GetValueOrDefault(EKey.RE_UIS_PAGE_SCROLL_SNAP)?.GoToNextPanel();
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
					m_oScrollSnapDict.GetValueOrDefault(EKey.RE_UIS_PAGE_SCROLL_SNAP)?.GoToPreviousPanel();
				}
				// 다음 페이지 키를 눌렀을 경우
				else if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.RightArrow)) {
					m_oScrollSnapDict.GetValueOrDefault(EKey.RE_UIS_PAGE_SCROLL_SNAP)?.GoToNextPanel();
				}
#endif			// #if INPUT_SYSTEM_MODULE_ENABLE
#endif			// #if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE)
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
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
				Func.ShowEditorQuitPopup(this.OnReceiveEditorQuitPopupResult);
#else
				this.OnReceiveEditorQuitPopupResult(null, true);
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			}
		}

		/** 에디터 종료 팝업 결과를 수신했을 경우 */
		private void OnReceiveEditorQuitPopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
#if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE)
				CLevelInfoTable.Inst.SaveLevelInfos();
#endif			// #if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE)

#if STUDY_MODULE_ENABLE
				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MENU);
#else
				CSceneLoader.Inst.LoadScene(COptsInfoTable.Inst.EtcOptsInfo.m_bIsEnableTitleScene ? KCDefine.B_SCENE_N_TITLE : KCDefine.B_SCENE_N_MAIN);
#endif			// #if STUDY_MODULE_ENABLE
			}
		}
		#endregion			// 함수

		#region 조건부 함수
#if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE)
		/** 터치 이벤트를 처리한다 */
		protected override void HandleTouchEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData, ETouchEvent a_eTouchEvent) {
			base.HandleTouchEvent(a_oSender, a_oEventData, a_eTouchEvent);
			var stTouchPos = a_oEventData.ExGetLocalPos(this.ObjRoot);
			
			// 배경 터치 전달자 일 경우
			if(this.BGTouchDispatcher == a_oSender && m_oGridInfoDict.GetValueOrDefault(EKey.SEL_GRID_INFO).m_stBounds.Contains(stTouchPos)) {
				switch(a_eTouchEvent) {
					case ETouchEvent.BEGIN: this.HandleTouchBeginEvent(a_oSender, a_oEventData); break;
					case ETouchEvent.MOVE: this.HandleTouchMoveEvent(a_oSender, a_oEventData); break;
					case ETouchEvent.END: this.HandleTouchEndEvent(a_oSender, a_oEventData); break;
				}
			}
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
				
				m_oLevelInfoDict.ExReplaceVal(EKey.SEL_LEVEL_INFO, CLevelInfoTable.Inst.GetLevelInfo(KCDefine.B_VAL_0_INT));
				this.UpdateUIsState();
			}
		}

		/** 에디터 세트 팝업 결과를 수신했을 경우 */
		private void OnReceiveEditorSetPopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
				CCommonUserInfoStorage.Inst.UserInfo.UserType = m_oUserTypeDict.GetValueOrDefault(EKey.SEL_USER_TYPE, EUserType.NONE);
				CCommonUserInfoStorage.Inst.SaveUserInfo();
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE

#if GOOGLE_SHEET_ENABLE
				m_oTableSrcDict.ExReplaceVal(EKey.SEL_TABLE_SRC, ETableSrc.REMOTE);
#else
				m_oTableSrcDict.ExReplaceVal(EKey.SEL_TABLE_SRC, ETableSrc.LOCAL);
#endif			// #if GOOGLE_SHEET_ENABLE

				this.OnReceiveEditorResetPopupResult(null, true);
				this.OnReceiveEditorTableLoadPopupResult(null, true);
			}
		}

		/** 에디터 세트 테이블 로드 팝업 결과를 수신했을 경우 */
		private void OnReceiveEditorTableLoadPopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				switch(m_oTableSrcDict.GetValueOrDefault(EKey.SEL_TABLE_SRC, ETableSrc.NONE)) {
					case ETableSrc.LOCAL: {
						CObjInfoTable.Inst.LoadObjInfos();
						CEpisodeInfoTable.Inst.LoadEpisodeInfos();

						this.UpdateUIsState();
					} break;
					case ETableSrc.REMOTE: {
#if GOOGLE_SHEET_ENABLE
						Func.LoadGoogleSheet(ID_OBJ_INFO_TABLE_GOOGLE_SHEET, new List<(string, int)>() {
							(KCDefine.U_KEY_BG, KCDefine.B_VAL_2_INT),
							(KCDefine.U_KEY_NORM, KCDefine.B_VAL_2_INT),
							(KCDefine.U_KEY_OVERLAY, KCDefine.B_VAL_2_INT),
							(KCDefine.U_KEY_PLAYABLE, KCDefine.B_VAL_2_INT),
							(KCDefine.U_KEY_NON_PLAYABLE, KCDefine.B_VAL_2_INT)
						}, this.OnLoadObjInfosGoogleSheet);
#endif			// #if GOOGLE_SHEET_ENABLE
					} break;
				}
			}
		}

		/** 에디터 제거 팝업 결과를 수신했을 경우 */
		private void OnReceiveEditorRemovePopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				this.RemoveLevelInfos(m_oScrollerDict.GetValueOrDefault(EKey.SEL_SCROLLER), m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo);
				this.UpdateUIsState();
			}
		}

		/** 에디터 입력 팝업 결과를 수신했을 경우 */
		private void OnReceiveEditorInputPopupResult(CEditorInputPopup a_oSender, string a_oStr, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				switch(m_oInputPopupDict.GetValueOrDefault(EKey.SEL_INPUT_POPUP, EInputPopup.NONE)) {
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
				int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID02, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID03);
				int nNumCreateLevelInfos = (nNumLevelInfos + a_oCreateInfo.m_nNumLevels < KCDefine.U_MAX_NUM_LEVEL_INFOS) ? a_oCreateInfo.m_nNumLevels : KCDefine.U_MAX_NUM_LEVEL_INFOS - nNumLevelInfos;

				for(int i = 0; i < nNumCreateLevelInfos; ++i) {
					var oLevelInfo = Factory.MakeLevelInfo(i + nNumLevelInfos, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID02, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID03);
					m_oLevelInfoDict.ExReplaceVal(EKey.SEL_LEVEL_INFO, oLevelInfo);

					CLevelInfoTable.Inst.AddLevelInfo(oLevelInfo);
					Func.SetupEditorLevelInfo(oLevelInfo, a_oCreateInfo);
				}

				this.UpdateUIsState();
			}
		}

		/** 레벨 정보를 반환한다 */
		private bool TryGetLevelInfo(STIDInfo a_stPrevIDInfo, STIDInfo a_stNextIDInfo, out CLevelInfo a_oOutLevelInfo) {
			CLevelInfoTable.Inst.TryGetLevelInfo(a_stPrevIDInfo.m_nID01, out CLevelInfo oPrevLevelInfo, a_stPrevIDInfo.m_nID02, a_stPrevIDInfo.m_nID03);
			CLevelInfoTable.Inst.TryGetLevelInfo(a_stNextIDInfo.m_nID01, out CLevelInfo oNextLevelInfo, a_stNextIDInfo.m_nID02, a_stNextIDInfo.m_nID03);
			
			a_oOutLevelInfo = oPrevLevelInfo ?? oNextLevelInfo;
			return oPrevLevelInfo != null || oNextLevelInfo != null;
		}

		/** 레벨 정보를 추가한다 */
		private void AddLevelInfo(int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
			m_oLevelInfoDict.ExReplaceVal(EKey.SEL_LEVEL_INFO, Factory.MakeLevelInfo(a_nLevelID, a_nStageID, a_nChapterID));
			CLevelInfoTable.Inst.AddLevelInfo(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO));

			Func.SetupEditorLevelInfo(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO), new CSubEditorLevelCreateInfo() {
				m_nNumLevels = KCDefine.B_VAL_0_INT, m_stMinNumCells = SampleEngineName.KDefine.E_MIN_NUM_CELLS, m_stMaxNumCells = SampleEngineName.KDefine.E_MIN_NUM_CELLS
			});

			this.UpdateUIsState();
		}

		/** 레벨 정보를 제거한다 */
		private void RemoveLevelInfos(EnhancedScroller a_oScroller, STIDInfo a_stIDInfo) {
			var oLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(a_stIDInfo.m_nID01, a_stIDInfo.m_nID02, a_stIDInfo.m_nID03);

			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_LEVEL_SCROLLER_INFO).Item1 == a_oScroller) {
				CLevelInfoTable.Inst.RemoveLevelInfo(a_stIDInfo.m_nID01, a_stIDInfo.m_nID02, a_stIDInfo.m_nID03);
			}
			// 스테이지 스크롤러 일 경우
			else if(m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_STAGE_SCROLLER_INFO).Item1 == a_oScroller) {
				CLevelInfoTable.Inst.RemoveStageLevelInfos(a_stIDInfo.m_nID02, a_stIDInfo.m_nID03);
			}
			// 챕터 스크롤러 일 경우
			else if(m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_CHAPTER_SCROLLER_INFO).Item1 == a_oScroller) {
				CLevelInfoTable.Inst.RemoveChapterLevelInfos(a_stIDInfo.m_nID03);
			}

			// 레벨 정보가 존재 할 경우
			if(!CLevelInfoTable.Inst.LevelInfoDictContainer.ExIsValid()) {
				m_oLevelInfoDict.ExReplaceVal(EKey.SEL_LEVEL_INFO, Factory.MakeLevelInfo(KCDefine.B_VAL_0_INT));
				CLevelInfoTable.Inst.AddLevelInfo(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO));

				Func.SetupEditorLevelInfo(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO), new CSubEditorLevelCreateInfo() {
					m_nNumLevels = KCDefine.B_VAL_0_INT, m_stMinNumCells = SampleEngineName.KDefine.E_MIN_NUM_CELLS, m_stMaxNumCells = SampleEngineName.KDefine.E_MIN_NUM_CELLS
				});
			} else {
				CLevelInfo oSelLevelInfo = null;

				// 레벨 스크롤러 일 경우
				if(m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_LEVEL_SCROLLER_INFO).Item1 == a_oScroller) {
					var stPrevIDInfo = CFactory.MakeIDInfo(a_stIDInfo.m_nID01 - KCDefine.B_VAL_1_INT, a_stIDInfo.m_nID02, a_stIDInfo.m_nID03);
					var stNextIDInfo = CFactory.MakeIDInfo(a_stIDInfo.m_nID01, a_stIDInfo.m_nID02, a_stIDInfo.m_nID03);

					this.TryGetLevelInfo(stPrevIDInfo, stNextIDInfo, out oSelLevelInfo);
				}

				// 스테이지 스크롤러 일 경우
				if(oSelLevelInfo == null || m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_STAGE_SCROLLER_INFO).Item1 == a_oScroller) {
					var stPrevIDInfo = CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, a_stIDInfo.m_nID02 - KCDefine.B_VAL_1_INT, a_stIDInfo.m_nID03);
					var stNextIDInfo = CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, a_stIDInfo.m_nID02, a_stIDInfo.m_nID03);

					this.TryGetLevelInfo(stPrevIDInfo, stNextIDInfo, out oSelLevelInfo);
				}

				// 챕터 스크롤러 일 경우
				if(oSelLevelInfo == null || m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_CHAPTER_SCROLLER_INFO).Item1 == a_oScroller) {
					var stPrevIDInfo = CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, a_stIDInfo.m_nID03 - KCDefine.B_VAL_1_INT);
					var stNextIDInfo = CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, a_stIDInfo.m_nID03);

					this.TryGetLevelInfo(stPrevIDInfo, stNextIDInfo, out oSelLevelInfo);
				}

				m_oLevelInfoDict.ExReplaceVal(EKey.SEL_LEVEL_INFO, oSelLevelInfo);
			}

			this.UpdateUIsState();
		}

		/** 레벨 정보를 복사한다 */
		private void CopyLevelInfos(EnhancedScroller a_oScroller, STIDInfo a_stIDInfo) {
			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_LEVEL_SCROLLER_INFO).Item1 == a_oScroller) {
				var oCloneLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(a_stIDInfo.m_nID01, a_stIDInfo.m_nID02, a_stIDInfo.m_nID03).Clone() as CLevelInfo;
				oCloneLevelInfo.m_stIDInfo.m_nID01 = CLevelInfoTable.Inst.GetNumLevelInfos(a_stIDInfo.m_nID02, a_stIDInfo.m_nID03);
				
				m_oLevelInfoDict.ExReplaceVal(EKey.SEL_LEVEL_INFO, oCloneLevelInfo);
				CLevelInfoTable.Inst.AddLevelInfo(oCloneLevelInfo);
			} else {
				// 스테이지 스크롤러 일 경우
				if(m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_STAGE_SCROLLER_INFO).Item1 == a_oScroller) {
					int nNumStageInfos = CLevelInfoTable.Inst.GetNumStageInfos(a_stIDInfo.m_nID03);
					var oStageLevelInfoDict = CLevelInfoTable.Inst.GetStageLevelInfos(a_stIDInfo.m_nID02, a_stIDInfo.m_nID03);

					for(int i = 0; i < oStageLevelInfoDict.Count; ++i) {
						var oCloneLevelInfo = oStageLevelInfoDict[i].Clone() as CLevelInfo;
						oCloneLevelInfo.m_stIDInfo.m_nID02 = nNumStageInfos;

						CLevelInfoTable.Inst.AddLevelInfo(oCloneLevelInfo);
					}
				}
				// 챕터 스크롤러 일 경우
				else if(m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_CHAPTER_SCROLLER_INFO).Item1 == a_oScroller) {
					int nNumChapterInfos = CLevelInfoTable.Inst.NumChapterInfos;
					var oChapterLevelInfoDictContainer = CLevelInfoTable.Inst.GetChapterLevelInfos(a_stIDInfo.m_nID03);

					for(int i = 0; i < oChapterLevelInfoDictContainer.Count; ++i) {
						for(int j = 0; j < oChapterLevelInfoDictContainer[i].Count; ++j) {
							var oCloneLevelInfo = oChapterLevelInfoDictContainer[i][j].Clone() as CLevelInfo;
							oCloneLevelInfo.m_stIDInfo.m_nID03 = nNumChapterInfos;

							CLevelInfoTable.Inst.AddLevelInfo(oCloneLevelInfo);
						}
					}
				}

				int nID = KCDefine.B_VAL_0_INT;
				int nStageID = (m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_STAGE_SCROLLER_INFO).Item1 == a_oScroller) ? CLevelInfoTable.Inst.GetNumStageInfos(a_stIDInfo.m_nID03) - KCDefine.B_VAL_1_INT : KCDefine.B_VAL_0_INT;
				int nChapterID = (m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_CHAPTER_SCROLLER_INFO).Item1 == a_oScroller) ? CLevelInfoTable.Inst.NumChapterInfos - KCDefine.B_VAL_1_INT : a_stIDInfo.m_nID03;

				m_oLevelInfoDict.ExReplaceVal(EKey.SEL_LEVEL_INFO, CLevelInfoTable.Inst.GetLevelInfo(nID, nStageID, nChapterID));
			}

			var oSelLevelInfo = m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO);
			m_oLevelInfoDict.ExReplaceVal(EKey.SEL_LEVEL_INFO, oSelLevelInfo ?? CLevelInfoTable.Inst.GetLevelInfo(KCDefine.B_VAL_0_INT));

			this.UpdateUIsState();
		}

		/** 레벨 정보를 이동한다 */
		private void MoveLevelInfos(EnhancedScroller a_oScroller, STIDInfo a_stIDInfo, int a_nDestID) {
			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_LEVEL_SCROLLER_INFO).Item1 == a_oScroller) {
				int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(a_stIDInfo.m_nID02, a_stIDInfo.m_nID03);
				CLevelInfoTable.Inst.MoveLevelInfo(a_stIDInfo.m_nID01, Mathf.Clamp(a_nDestID, KCDefine.B_VAL_1_INT, nNumLevelInfos) - KCDefine.B_VAL_1_INT, a_stIDInfo.m_nID02, a_stIDInfo.m_nID03);
			}
			// 스테이지 스크롤러 일 경우
			else if(m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_STAGE_SCROLLER_INFO).Item1 == a_oScroller) {
				int nNumStageInfos = CLevelInfoTable.Inst.GetNumStageInfos(a_stIDInfo.m_nID03);
				CLevelInfoTable.Inst.MoveStageLevelInfos(a_stIDInfo.m_nID02, Mathf.Clamp(a_nDestID, KCDefine.B_VAL_1_INT, nNumStageInfos) - KCDefine.B_VAL_1_INT, a_stIDInfo.m_nID03);
			}
			// 챕터 스크롤러 일 경우
			else if(m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_CHAPTER_SCROLLER_INFO).Item1 == a_oScroller) {
				int nNumChapterInfos = CLevelInfoTable.Inst.NumChapterInfos;
				CLevelInfoTable.Inst.MoveChapterLevelInfos(a_stIDInfo.m_nID03, Mathf.Clamp(a_nDestID, KCDefine.B_VAL_1_INT, nNumChapterInfos) - KCDefine.B_VAL_1_INT);
			}
		}

		/** 알림을 출력한다 */
		private void ShowNoti(string a_oMsg) {
			this.MEUIsMsgUIs?.SetActive(true);
			m_oTextDict.GetValueOrDefault(EKey.ME_UIS_MSG_TEXT)?.ExSetText<Text>(a_oMsg, false);

			CScheduleManager.Inst.RemoveTimer(this);
			CScheduleManager.Inst.AddTimer(this, KCDefine.B_VAL_5_REAL, KCDefine.B_VAL_1_INT, () => this.MEUIsMsgUIs?.SetActive(false));
		}

		/** 에디터 레벨 이동 입력 팝업 결과를 처리한다 */
		private void HandleMoveLevelInputPopupResult(string a_oStr) {
			// 식별자가 유효 할 경우
			if(int.TryParse(a_oStr, NumberStyles.Any, null, out int nID)) {
				this.MoveLevelInfos(m_oScrollerDict.GetValueOrDefault(EKey.SEL_SCROLLER), m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo, nID);
			}
		}

		/** 에디터 레벨 제거 입력 팝업 결과를 처리한다 */
		private void HandleRemoveLevelInputPopupResult(string a_oStr) {
			var oTokenList = a_oStr.Split(KCDefine.B_TOKEN_DASH).ToList();
			int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID02, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID03);

			// 식별자가 유효 할 경우
			if(oTokenList.Count > KCDefine.B_VAL_1_INT && (int.TryParse(oTokenList[KCDefine.B_VAL_0_INT], NumberStyles.Any, null, out int nMinID) && int.TryParse(oTokenList[KCDefine.B_VAL_1_INT], NumberStyles.Any, null, out int nMaxID))) {
				nMinID = Mathf.Clamp(nMinID, KCDefine.B_VAL_1_INT, nNumLevelInfos);
				nMaxID = Mathf.Clamp(nMaxID, KCDefine.B_VAL_1_INT, nNumLevelInfos);

				CFunc.LessCorrectSwap(ref nMinID, ref nMaxID);
				var stIDInfo = CFactory.MakeIDInfo(nMinID - KCDefine.B_VAL_1_INT, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID02, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID03);

				for(int i = nMinID; i <= nMaxID; ++i) {
					// 레벨 정보가 존재 할 경우
					if(CLevelInfoTable.Inst.TryGetLevelInfo(stIDInfo.m_nID01, out CLevelInfo oLevelInfo, stIDInfo.m_nID02, stIDInfo.m_nID03)) {
						this.RemoveLevelInfos(m_oScrollerDict.GetValueOrDefault(EKey.SEL_SCROLLER), stIDInfo);
					}
				}
			}
		}

#if GOOGLE_SHEET_ENABLE
		/** 객체 정보 구글 시트를 로드했을 경우 */
		private void OnLoadObjInfosGoogleSheet(CServicesManager a_oSender, GstuSpreadSheet a_oGoogleSheet, string a_oID, Dictionary<string, (SimpleJSON.JSONNode, bool)> a_oJSONNodeInfoDict) {
			var oResult = a_oJSONNodeInfoDict.ExFindVal((a_oJSONInfoDict) => !a_oJSONInfoDict.Item2);

			// 로드 되었을 경우
			if(!oResult.Item1) {
				CObjInfoTable.Inst.ResetObjInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString());

				Func.LoadGoogleSheet(ID_ETC_INFO_TABLE_GOOGLE_SHEET, new List<(string, int)>() {
					(KCDefine.U_KEY_LEVEL_EPISODE, CLevelInfoTable.Inst.TotalNumLevelInfos + KCDefine.B_VAL_1_INT),
					(KCDefine.U_KEY_STAGE_EPISODE, CLevelInfoTable.Inst.TotalNumStageInfos + KCDefine.B_VAL_1_INT),
					(KCDefine.U_KEY_CHAPTER_EPISODE, CLevelInfoTable.Inst.NumChapterInfos + KCDefine.B_VAL_1_INT)
				}, this.OnLoadEpisodeInfosGoogleSheet);
			} else {
				Func.ShowEditorGoogleSheetLoadPopup(null);
			}
		}

		/** 에피소드 정보 구글 시트를 로드했을 경우 */
		private void OnLoadEpisodeInfosGoogleSheet(CServicesManager a_oSender, GstuSpreadSheet a_oGoogleSheet, string a_oID, Dictionary<string, (SimpleJSON.JSONNode, bool)> a_oJSONNodeInfoDict) {
			var oResult = a_oJSONNodeInfoDict.ExFindVal((a_oJSONInfoDict) => !a_oJSONInfoDict.Item2);

			// 로드 되었을 경우
			if(!oResult.Item1) {
				CEpisodeInfoTable.Inst.ResetEpisodeInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString());
				this.UpdateUIsState();
			} else {
				Func.ShowEditorGoogleSheetLoadPopup(null);
			}
		}
#endif			// #if GOOGLE_SHEET_ENABLE
#endif			// #if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE)
		#endregion			// 조건부 함수
	}

	/** 서브 레벨 에디터 씬 관리자 - 중앙 에디터 UI */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
		#region 조건부 함수
#if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE)
		/** 중앙 에디터 UI 이전 레벨 버튼을 눌렀을 경우 */
		private void OnTouchMEUIsPrevBtn() {
			// 이전 레벨 정보가 존재 할 경우
			if(CLevelInfoTable.Inst.TryGetLevelInfo(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID01 - KCDefine.B_VAL_1_INT, out CLevelInfo oPrevLevelInfo, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID02, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID03)) {
				m_oLevelInfoDict.ExReplaceVal(EKey.SEL_LEVEL_INFO, oPrevLevelInfo);
				this.UpdateUIsState();
			}
		}

		/** 중앙 에디터 UI 다음 레벨 버튼을 눌렀을 경우 */
		private void OnTouchMEUIsNextBtn() {
			// 다음 레벨 정보가 존재 할 경우
			if(CLevelInfoTable.Inst.TryGetLevelInfo(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID01 + KCDefine.B_VAL_1_INT, out CLevelInfo oNextLevelInfo, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID02, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID03)) {
				m_oLevelInfoDict.ExReplaceVal(EKey.SEL_LEVEL_INFO, oNextLevelInfo);
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
			Func.SetupPlayLevelInfo(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID01, EPlayMode.TEST, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID02, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID03);
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME);
		}

		/** 중앙 에디터 UI 레벨 복사 버튼을 눌렀을 경우 */
		private void OnTouchMEUIsCopyLevelBtn() {
			int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID02, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID03);

			// 레벨 추가가 가능 할 경우
			if(nNumLevelInfos < KCDefine.U_MAX_NUM_LEVEL_INFOS) {
				var stIDInfo = CFactory.MakeIDInfo(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID01, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID02, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID03);
				this.CopyLevelInfos(m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_LEVEL_SCROLLER_INFO).Item1, stIDInfo);
			}
		}

		/** 중앙 에디터 UI 레벨 이동 버튼을 눌렀을 경우 */
		private void OnTouchMEUIsMoveLevelBtn() {
			m_oInputPopupDict.ExReplaceVal(EKey.SEL_INPUT_POPUP, EInputPopup.MOVE_LEVEL);
			m_oScrollerDict.ExReplaceVal(EKey.SEL_SCROLLER, m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_LEVEL_SCROLLER_INFO).Item1);

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
			m_oScrollerDict.ExReplaceVal(EKey.SEL_SCROLLER, m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_LEVEL_SCROLLER_INFO).Item1);
			Func.ShowEditorLevelRemovePopup(this.OnReceiveEditorRemovePopupResult);
		}
#endif			// #if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE)
		#endregion			// 조건부 함수
	}

	/** 서브 레벨 에디터 씬 관리자 - 왼쪽 에디터 UI */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
		#region 조건부 함수
#if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE)
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
			int nNumStageInfos = CLevelInfoTable.Inst.GetNumStageInfos(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID03);

			// 스테이지 추가가 가능 할 경우
			if(nNumStageInfos < KCDefine.U_MAX_NUM_STAGE_INFOS) {
				this.AddLevelInfo(KCDefine.B_VAL_0_INT, nNumStageInfos, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID03);
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

#if AB_TEST_ENABLE
		/** 왼쪽 에디터 UI 세트 버튼을 눌렀을 경우 */
		private void OnTouchLEUIsSetBtn(Button a_oSender) {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
			var eUserType = (a_oSender == m_oBtnDict.GetValueOrDefault(EKey.LE_UIS_A_SET_BTN)) ? EUserType.A : EUserType.B;

			// 유저 타입이 다를 경우
			if(CCommonUserInfoStorage.Inst.UserInfo.UserType != eUserType) {
				m_oUserTypeDict.ExReplaceVal(EKey.SEL_USER_TYPE, eUserType);

				// A 세트 버튼 일 경우
				if(a_oSender == m_oBtnDict.GetValueOrDefault(EKey.LE_UIS_A_SET_BTN)) {
					Func.ShowEditorASetPopup(this.OnReceiveEditorSetPopupResult);
				} else {
					Func.ShowEditorBSetPopup(this.OnReceiveEditorSetPopupResult);
				}
			}
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
		}
#endif			// #if AB_TEST_ENABLE
#endif			// #if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE)
		#endregion			// 조건부 함수
	}

	/** 서브 레벨 에디터 씬 관리자 - 오른쪽 에디터 UI */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
		#region 조건부 함수
#if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE)
		/** 오른쪽 에디터 UI 적용 버튼을 눌렀을 경우 */
		private void OnTouchREUIsApplyBtn() {
			bool bIsValid01 = int.TryParse(m_oInputDict.GetValueOrDefault(EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_X_INPUT)?.text, NumberStyles.Any, null, out int nNumCellsX);
			bool bIsValid02 = int.TryParse(m_oInputDict.GetValueOrDefault(EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_Y_INPUT)?.text, NumberStyles.Any, null, out int nNumCellsY);

			bool bIsValidNumCellsX = Mathf.Max(nNumCellsX, SampleEngineName.KDefine.E_MIN_NUM_CELLS.x) != m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).NumCells.x;
			bool bIsValidNumCellsY = Mathf.Max(nNumCellsY, SampleEngineName.KDefine.E_MIN_NUM_CELLS.y) != m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).NumCells.y;

			// 셀 개수가 유효 할 경우
			if(bIsValid01 && bIsValid02 && (bIsValidNumCellsX || bIsValidNumCellsY)) {
				Func.SetupEditorLevelInfo(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO), new CSubEditorLevelCreateInfo() {
					m_nNumLevels = KCDefine.B_VAL_0_INT, m_stMinNumCells = new Vector3Int(nNumCellsX, nNumCellsY, KCDefine.B_VAL_0_INT), m_stMaxNumCells = new Vector3Int(nNumCellsX, nNumCellsY, KCDefine.B_VAL_0_INT)
				});
				
				this.UpdateUIsState();
			}
		}

		/** 오른쪽 에디터 UI 레벨 로드 버튼을 눌렀을 경우 */
		private void OnTouchREUIsLoadLevelBtn() {
			// 식별자가 유효 할 경우
			if(int.TryParse(m_oInputDict.GetValueOrDefault(EKey.RE_UIS_PAGE_UIS_01_LEVEL_INPUT)?.text, NumberStyles.Any, null, out int nID)) {
				int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID02, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID03);
				m_oLevelInfoDict.ExReplaceVal(EKey.SEL_LEVEL_INFO, CLevelInfoTable.Inst.GetLevelInfo(Mathf.Clamp(nID, KCDefine.B_VAL_1_INT, nNumLevelInfos) - KCDefine.B_VAL_1_INT, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID02, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID03));
				
				this.UpdateUIsState();
			}
		}

		/** 오른쪽 에디터 UI 모든 레벨 제거 버튼을 눌렀을 경우 */
		private void OnTouchREUIsRemoveAllLevelsBtn() {
			m_oInputPopupDict.ExReplaceVal(EKey.SEL_INPUT_POPUP, EInputPopup.REMOVE_LEVEL);
			m_oScrollerDict.ExReplaceVal(EKey.SEL_SCROLLER, m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_LEVEL_SCROLLER_INFO).Item1);

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
		private void OnTouchREUIsLoadTableBtn(ETableSrc a_eTableSrc) {
			m_oTableSrcDict.ExReplaceVal(EKey.SEL_TABLE_SRC, a_eTableSrc);
			Func.ShowEditorTableLoadPopup(this.OnReceiveEditorTableLoadPopupResult);
		}
#endif			// #if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE)
		#endregion			// 조건부 함수
	}

	/** 서브 레벨 에디터 씬 관리자 - 스크롤러 셀 뷰 */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
		#region 조건부 함수
#if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE)
		/** 스크롤러 셀 뷰 선택 버튼을 눌렀을 경우 */
		private void OnTouchSCVSelBtn(CScrollerCellView a_oSender, ulong a_nID) {
			m_oLevelInfoDict.ExReplaceVal(EKey.SEL_LEVEL_INFO, CLevelInfoTable.Inst.GetLevelInfo(a_nID.ExULevelIDToLevelID(), a_nID.ExULevelIDToStageID(), a_nID.ExULevelIDToChapterID()));
			this.UpdateUIsState();
		}

		/** 스크롤러 셀 뷰 복사 버튼을 눌렀을 경우 */
		private void OnTouchSCVCopyBtn(CScrollerCellView a_oSender, ulong a_nID) {
			int nNumInfos = CLevelInfoTable.Inst.GetNumLevelInfos(a_nID.ExULevelIDToStageID(), a_nID.ExULevelIDToChapterID());
			int nMaxNumInfos = KCDefine.U_MAX_NUM_LEVEL_INFOS;

			// 레벨 스크롤러가 아닐 경우
			if(m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_LEVEL_SCROLLER_INFO).Item1 != a_oSender.Params.m_oScroller) {
				nNumInfos = (m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_STAGE_SCROLLER_INFO).Item1 == a_oSender.Params.m_oScroller) ? CLevelInfoTable.Inst.GetNumStageInfos(a_nID.ExULevelIDToChapterID()) : CLevelInfoTable.Inst.NumChapterInfos;
				nMaxNumInfos = (m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_STAGE_SCROLLER_INFO).Item1 == a_oSender.Params.m_oScroller) ? KCDefine.U_MAX_NUM_STAGE_INFOS : KCDefine.U_MAX_NUM_CHAPTER_INFOS;
			}

			// 복사가 가능 할 경우
			if(nNumInfos < nMaxNumInfos) {
				this.CopyLevelInfos(a_oSender.Params.m_oScroller, CFactory.MakeIDInfo(a_nID.ExULevelIDToLevelID(), a_nID.ExULevelIDToStageID(), a_nID.ExULevelIDToChapterID()));
			}
		}

		/** 스크롤러 셀 뷰 이동 버튼을 눌렀을 경우 */
		private void OnTouchSCVMoveBtn(CScrollerCellView a_oSender, ulong a_nID) {
			m_oInputPopupDict.ExReplaceVal(EKey.SEL_INPUT_POPUP, EInputPopup.MOVE_LEVEL);
			m_oScrollerDict.ExReplaceVal(EKey.SEL_SCROLLER, a_oSender.Params.m_oScroller);

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
		private void OnTouchSCVRemoveBtn(CScrollerCellView a_oSender, ulong a_nID) {
			m_oScrollerDict.ExReplaceVal(EKey.SEL_SCROLLER, a_oSender.Params.m_oScroller);

			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_LEVEL_SCROLLER_INFO).Item1 == a_oSender.Params.m_oScroller) {
				Func.ShowEditorLevelRemovePopup(this.OnReceiveEditorRemovePopupResult);
			}
			// 스테이지 스크롤러 일 경우
			else if(m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_STAGE_SCROLLER_INFO).Item1 == a_oSender.Params.m_oScroller) {
				Func.ShowEditorStageRemovePopup(this.OnReceiveEditorRemovePopupResult);
			}
			// 챕터 스크롤러 일 경우
			else if(m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_CHAPTER_SCROLLER_INFO).Item1 == a_oSender.Params.m_oScroller) {
				Func.ShowEditorChapterRemovePopup(this.OnReceiveEditorRemovePopupResult);
			}
		}
#endif			// #if UNITY_STANDALONE && (EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE)
		#endregion			// 조건부 함수
	}
}
#endif			// #if UNITY_STANDALONE && EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
#endif			// #if SCRIPT_TEMPLATE_ONLY
