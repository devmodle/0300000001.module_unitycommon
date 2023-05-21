#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using UnityEngine.EventSystems;

using EnhancedUI.EnhancedScroller;
using DanielLochner.Assets.SimpleScrollSnap;

namespace MainScene {
	/** 서브 메인 씬 관리자 */
	public partial class CSubMainSceneManager : CMainSceneManager, IEnhancedScrollerDelegate {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			SEL_ID_INFO,

			LEVEL_SCROLLER_INFO,
			STAGE_SCROLLER_INFO,
			CHAPTER_SCROLLER_INFO,

			CONTENTS_SCROLL_SNAP,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		private Dictionary<EKey, STIDInfo> m_oIDInfoDict = new Dictionary<EKey, STIDInfo>() {
			[EKey.SEL_ID_INFO] = STIDInfo.INVALID
		};

		[Header("=====> UIs <=====")]
		private Dictionary<EKey, STScrollerInfo> m_oScrollerInfoDict = new Dictionary<EKey, STScrollerInfo>();
		private Dictionary<EKey, SimpleScrollSnap> m_oScrollSnapDict = new Dictionary<EKey, SimpleScrollSnap>();

		[Header("=====> Objs <=====")]
		[SerializeField] private List<Button> m_oContentsTapBtnList = new List<Button>();
		#endregion // 변수

		#region IEnhancedScrollerDelegate
		/** 셀 개수를 반환한다 */
		public int GetNumberOfCells(EnhancedScroller a_oSender) {
			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict[EKey.LEVEL_SCROLLER_INFO].m_oScroller == a_oSender) {
				return this.GetNumLevelScrollerCells(a_oSender);
			}

			return (m_oScrollerInfoDict[EKey.STAGE_SCROLLER_INFO].m_oScroller == a_oSender) ? this.GetNumStageScrollerCells(a_oSender) : this.GetNumChapterScrollerCells(a_oSender);
		}

		/** 셀 뷰 크기를 반환한다 */
		public float GetCellViewSize(EnhancedScroller a_oSender, int a_nDataIdx) {
			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict[EKey.LEVEL_SCROLLER_INFO].m_oScroller == a_oSender) {
				return (m_oScrollerInfoDict[EKey.LEVEL_SCROLLER_INFO].m_oScrollerCellView.transform as RectTransform).sizeDelta.y;
			}

			return (m_oScrollerInfoDict[EKey.STAGE_SCROLLER_INFO].m_oScroller == a_oSender) ? (m_oScrollerInfoDict[EKey.STAGE_SCROLLER_INFO].m_oScrollerCellView.transform as RectTransform).sizeDelta.y : (m_oScrollerInfoDict[EKey.CHAPTER_SCROLLER_INFO].m_oScrollerCellView.transform as RectTransform).sizeDelta.y;
		}

		/** 셀 뷰를 반환한다 */
		public EnhancedScrollerCellView GetCellView(EnhancedScroller a_oSender, int a_nDataIdx, int a_nCellIdx) {
			var oCallbackDict = new Dictionary<CScrollerCellView.ECallback, System.Action<CScrollerCellView, ulong>>() {
				[CScrollerCellView.ECallback.SEL] = this.OnReceiveSelCallback
			};

			/** 레벨 스크롤러 일 경우 */
			if(m_oScrollerInfoDict[EKey.LEVEL_SCROLLER_INFO].m_oScroller == a_oSender) {
				return this.CreateLevelScrollerCellView(a_oSender, a_nDataIdx, a_nCellIdx, oCallbackDict);
			}

			return (m_oScrollerInfoDict[EKey.STAGE_SCROLLER_INFO].m_oScroller == a_oSender) ? this.CreateStageScrollerCellView(a_oSender, a_nDataIdx, a_nCellIdx, oCallbackDict) : this.CreateChapterScrollerCellView(a_oSender, a_nDataIdx, a_nCellIdx, oCallbackDict);
		}
		#endregion // IEnhancedScrollerDelegate

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
#if CREATIVE_DIST_BUILD
				for(int i = 0; i < CLevelInfoTable.Inst.NumChapterInfos; ++i) {
					for(int j = 0; j < CLevelInfoTable.Inst.GetNumStageInfos(i); ++j) {
						for(int k = 0; k < CLevelInfoTable.Inst.GetNumLevelInfos(j, i); ++k) {
							// 클리어 정보가 없을 경우
							if(!Access.IsClearLevel(CGameInfoStorage.Inst.PlayCharacterID, k, j, i)) {
								CGameInfoStorage.Inst.AddLevelClearInfo(CGameInfoStorage.Inst.PlayCharacterID, Factory.MakeClearInfo(k, j, i));
							}

							var oLevelClearInfo = CGameInfoStorage.Inst.GetLevelClearInfo(CGameInfoStorage.Inst.PlayCharacterID, k, j, i);
							oLevelClearInfo.NumMarks = KCDefine.B_VAL_1_INT;
						}
					}
				}

				Access.SetItemTargetVal(CGameInfoStorage.Inst.PlayCharacterID, EItemKinds.GOODS_ITEM_COINS_01, ETargetKinds.ABILITY_TARGET, (int)EAbilityKinds.STAT_ABILITY_NUMS, KCDefine.B_UNIT_DIGITS_PER_HUNDRED_THOUSAND);
				CGameInfoStorage.Inst.SaveGameInfo();
#endif // #if CREATIVE_DIST_BUILD

				this.SetupAwake();
				CGameInfoStorage.Inst.ResetSelItems();
			}
		}

		/** 초기화 */
		public override void Start() {
			base.Start();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				this.SetupStart();
				this.UpdateUIsState();

				Func.PlayBGSnd(EResKinds.SND_RES_BG_SCENE_MAIN_01);

#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
				// 에디터가 유효 할 경우
				if(CCommonAppInfoStorage.Inst.IsEnableEditor && !CSceneLoader.Inst.AwakeActiveSceneName.Equals(KCDefine.B_SCENE_N_MAIN)) {
					CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_LEVEL_EDITOR);
				}
#endif // #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			}
		}

		/** 제거 되었을 경우 */
		public override void OnDestroy() {
			base.OnDestroy();

			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsAppRunning) {
					this.SubOnDestroy();
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CSubMainSceneManager.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 앱이 정지 되었을 경우 */
		public override void OnApplicationPause(bool a_bIsPause) {
			base.OnApplicationPause(a_bIsPause);

			// 재개 되었을 경우
			if(!a_bIsPause && CSceneManager.IsAppRunning) {
#if ADS_MODULE_ENABLE
				// 광고 출력이 가능 할 경우
				if(CAppInfoStorage.Inst.IsEnableShowFullscreenAds && CAdsManager.Inst.IsLoadFullscreenAds(CPluginInfoTable.Inst.AdsPlatform)) {
					Func.ShowFullscreenAds(null);
				}
#endif // #if ADS_MODULE_ENABLE
			}
		}

		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				this.SubOnUpdate(a_fDeltaTime);

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
				// 단축키를 눌렀을 경우
				if(Input.GetKey(KeyCode.LeftShift)) {
					this.HandleHotKeys();
				}
#endif // #if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
			}
		}

		/** 내비게이션 스택 이벤트를 수신했을 경우 */
		public override void OnReceiveNavStackEvent(ENavStackEvent a_eEvent) {
			base.OnReceiveNavStackEvent(a_eEvent);

			// 백 키 눌림 이벤트 일 경우
			if(a_eEvent == ENavStackEvent.BACK_KEY_DOWN) {
				Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_QUIT_P_MSG), this.OnReceiveQuitPopupResult);
			}
		}

		/** 터치 이벤트를 처리한다 */
		protected override void HandleTouchEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData, ETouchEvent a_eTouchEvent) {
			base.HandleTouchEvent(a_oSender, a_oEventData, a_eTouchEvent);

			// 배경 터치 전달자 일 경우
			if(this.BGTouchDispatcher == a_oSender) {
				switch(a_eTouchEvent) {
					case ETouchEvent.BEGIN: this.HandleTouchBeginEvent(a_oSender, a_oEventData); break;
					case ETouchEvent.MOVE: this.HandleTouchMoveEvent(a_oSender, a_oEventData); break;
					case ETouchEvent.END: this.HandleTouchEndEvent(a_oSender, a_oEventData); break;
				}
			}
		}

		/** 씬을 설정한다 */
		private void SetupAwake() {
			var ePlayMode = CGameInfoStorage.Inst.PlayMode;
			m_oIDInfoDict[EKey.SEL_ID_INFO] = (ePlayMode == EPlayMode.NORM && CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01 > KCDefine.B_IDX_INVALID) ? CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo : new STIDInfo(KCDefine.B_VAL_0_INT);

			// 버튼을 설정한다 {
			CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
				(KCDefine.U_OBJ_N_PLAY_BTN, this.UIsBase, this.OnTouchPlayBtn),
				(KCDefine.U_OBJ_N_STORE_BTN, this.UIsBase, this.OnTouchStoreBtn),
				(KCDefine.U_OBJ_N_REVIEW_BTN, this.UIsBase, this.OnTouchReviewBtn),
				(KCDefine.U_OBJ_N_SETTINGS_BTN, this.UIsBase, this.OnTouchSettingsBtn)
			});

			for(int i = 0; i < m_oContentsTapBtnList.Count; ++i) {
				int nIdx = i;
				m_oContentsTapBtnList[i].onClick.AddListener(() => this.OnTouchContentsTapBtn(nIdx));
			}
			// 버튼을 설정한다 }

			// 스크롤 뷰를 설정한다 {
			CFunc.SetupScrollerInfos(new List<(EKey, string, GameObject, EnhancedScrollerCellView, IEnhancedScrollerDelegate)>() {
				(EKey.LEVEL_SCROLLER_INFO, KCDefine.U_OBJ_N_LEVEL_SCROLL_VIEW, this.UIsBase, CResManager.Inst.GetRes<GameObject>(KCDefine.MS_OBJ_P_LEVEL_SCROLLER_CELL_VIEW)?.GetComponentInChildren<EnhancedScrollerCellView>(), this),
				(EKey.STAGE_SCROLLER_INFO, KCDefine.U_OBJ_N_STAGE_SCROLL_VIEW, this.UIsBase, CResManager.Inst.GetRes<GameObject>(KCDefine.MS_OBJ_P_STAGE_SCROLLER_CELL_VIEW)?.GetComponentInChildren<EnhancedScrollerCellView>(), this),
				(EKey.CHAPTER_SCROLLER_INFO, KCDefine.U_OBJ_N_CHAPTER_SCROLL_VIEW, this.UIsBase, CResManager.Inst.GetRes<GameObject>(KCDefine.MS_OBJ_P_CHAPTER_SCROLLER_CELL_VIEW)?.GetComponentInChildren<EnhancedScrollerCellView>(), this)
			}, m_oScrollerInfoDict);

			CFunc.SetupScrollSnaps(new List<(EKey, string, GameObject, UnityAction<int, int>)>() {
				(EKey.CONTENTS_SCROLL_SNAP, KCDefine.U_OBJ_N_PAGE_VIEW, this.UIs, (a_nCenterIdx, a_nSelIdx) => this.UpdateUIsState())
			}, m_oScrollSnapDict);

			m_oScrollSnapDict[EKey.CONTENTS_SCROLL_SNAP]?.ExAddListener(this.OnChangeScrollSnapPage, a_bIsEnableAssert: false);
			// 스크롤 뷰를 설정한다 }

			this.SubAwake();
		}

		/** 씬을 설정한다 */
		private void SetupStart() {
			// 스크롤 뷰를 갱신한다
			m_oScrollerInfoDict[EKey.LEVEL_SCROLLER_INFO].m_oScroller?.ExReloadData((Access.GetNumLevelClearInfos(CGameInfoStorage.Inst.PlayCharacterID, m_oIDInfoDict[EKey.SEL_ID_INFO].m_nID02, m_oIDInfoDict[EKey.SEL_ID_INFO].m_nID03) / KDefine.MS_MAX_NUM_LEVELS_IN_ROW) - KCDefine.B_VAL_1_INT);
			m_oScrollerInfoDict[EKey.STAGE_SCROLLER_INFO].m_oScroller?.ExReloadData((Access.GetNumStageClearInfos(CGameInfoStorage.Inst.PlayCharacterID, m_oIDInfoDict[EKey.SEL_ID_INFO].m_nID03) / KDefine.MS_MAX_NUM_STAGES_IN_ROW) - KCDefine.B_VAL_1_INT);
			m_oScrollerInfoDict[EKey.CHAPTER_SCROLLER_INFO].m_oScroller?.ExReloadData((Access.GetNumChapterClearInfos(CGameInfoStorage.Inst.PlayCharacterID) / KDefine.MS_MAX_NUM_CHAPTERS_IN_ROW) - KCDefine.B_VAL_1_INT);

			// 캐릭터 게임 정보가 존재 할 경우
			if(CGameInfoStorage.Inst.TryGetCharacterGameInfo(CGameInfoStorage.Inst.PlayCharacterID, out CCharacterGameInfo oCharacterGameInfo)) {
				// 일일 미션 리셋이 가능 할 경우
				if(Access.IsEnableResetDailyMission(CGameInfoStorage.Inst.PlayCharacterID)) {
					oCharacterGameInfo.PrevDailyMissionTime = System.DateTime.Today;
					oCharacterGameInfo.m_oCompleteDailyMissionKindsList.Clear();
				}

				// 무료 보상 획득이 가능 할 경우
				if(Access.IsEnableGetFreeReward(CGameInfoStorage.Inst.PlayCharacterID)) {
					oCharacterGameInfo.FreeRewardAcquireTimes = KCDefine.B_VAL_0_INT;
					oCharacterGameInfo.PrevFreeRewardTime = System.DateTime.Today;
				}

				CGameInfoStorage.Inst.SaveGameInfo();
			}

			// 업데이트가 가능 할 경우
			if(!CAppInfoStorage.Inst.IsIgnoreUpdate && !COptsInfoTable.Inst.EtcOptsInfo.m_bIsEnableTitleScene && CCommonAppInfoStorage.Inst.IsEnableUpdate()) {
				CAppInfoStorage.Inst.SetIgnoreUpdate(true);
				this.ExLateCallFunc((a_oSender) => Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_UPDATE_P_MSG), this.OnReceiveUpdatePopupResult));
			}

#if DAILY_REWARD_ENABLE
			// 일일 보상 획득이 가능 할 경우
			if(Access.IsEnableGetDailyReward(CGameInfoStorage.Inst.PlayCharacterID)) {
				Func.ShowDailyRewardPopup(this.PopupUIs, (a_oSender) => (a_oSender as CDailyRewardPopup).Init());
			}
#endif // #if DAILY_REWARD_ENABLE

			this.SubStart();
		}

		/** UI 상태를 갱신한다 */
		private void UpdateUIsState() {
			this.SubUpdateUIsState();
		}

		/** 종료 팝업 결과를 수신했을 경우 */
		private void OnReceiveQuitPopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				a_oSender.SetIgnoreAni(true);
				this.ExLateCallFunc((a_oSender) => this.QuitApp());
			}
		}

		/** 업데이트 팝업 결과를 수신했을 경우 */
		private void OnReceiveUpdatePopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				Application.OpenURL(Access.StoreURL);
			}
		}

		/** 준비 팝업 콜백을 수신했을 경우 */
		private void OnReceiveReadyPopupCallback(CPopup a_oSender) {
			CGameInfoStorage.Inst.SetPlayStartingTime(System.DateTime.Now);
			Func.SetupPlayEpisodeInfo(CGameInfoStorage.Inst.PlayCharacterID, (a_oSender as CReadyPopup).Params.m_stIDInfo.m_nID01, EPlayMode.NORM);

			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_PLAY);
		}

		/** 플레이 버튼을 눌렀을 경우 */
		private void OnTouchPlayBtn() {
			// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능) {
#if NEVER_USE_THIS
			Func.ShowReadyPopup(this.PopupUIs, (a_oSender) => {
				int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(KCDefine.B_VAL_0_INT);
				int nNumLevelClearInfos = Access.GetNumLevelClearInfos(CGameInfoStorage.Inst.PlayCharacterID, KCDefine.B_VAL_0_INT);

				// 레벨 에피소드 정보가 없을 경우
				if(nNumLevelClearInfos >= nNumLevelInfos || !CEpisodeInfoTable.Inst.TryGetLevelEpisodeInfo(nNumLevelClearInfos, out STEpisodeInfo stEpisodeInfo)) {
					stEpisodeInfo = CEpisodeInfoTable.Inst.GetLevelEpisodeInfo(nNumLevelClearInfos - KCDefine.B_VAL_1_INT);
				}

				(a_oSender as CReadyPopup).Init(CReadyPopup.MakeParams(stEpisodeInfo.m_stIDInfo, new Dictionary<CReadyPopup.ECallback, System.Action<CReadyPopup>>() {
					[CReadyPopup.ECallback.PLAY] = this.OnReceiveReadyPopupCallback
				}));
			});
#endif // #if NEVER_USE_THIS
			// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능) }
		}

		/** 상점 버튼을 눌렀을 경우 */
		private void OnTouchStoreBtn() {
			CSceneManager.GetSceneManager<OverlayScene.CSubOverlaySceneManager>(KCDefine.B_SCENE_N_OVERLAY)?.ShowStorePopup();
		}

		/** 평가 버튼을 눌렀을 경우 */
		private void OnTouchReviewBtn() {
			CUnityMsgSender.Inst.SendShowReviewMsg();
		}

		/** 설정 버튼을 눌렀을 경우 */
		private void OnTouchSettingsBtn() {
			Func.ShowSettingsPopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CSettingsPopup).Init();
			});
		}

		/** 컨텐츠 탭 버튼을 눌렀을 경우 */
		private void OnTouchContentsTapBtn(int a_nIdx) {
			m_oScrollSnapDict[EKey.CONTENTS_SCROLL_SNAP].GoToPanel(a_nIdx);
		}

		/** 스크롤 스냅 페이지가 변경 되었을 경우 */
		private void OnChangeScrollSnapPage(int a_nSrcIdx, int a_nDestIdx) {
			this.UpdateUIsState();
		}
		#endregion // 함수

		#region 조건부 함수
#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		/** 단축키를 처리한다 */
		private void HandleHotKeys() {
			// 초기화 키를 눌렀을 경우
			if(Input.GetKeyDown(KeyCode.C)) {
				var oCharacterGameInfo = CGameInfoStorage.Inst.GetCharacterGameInfo(CGameInfoStorage.Inst.PlayCharacterID);
				oCharacterGameInfo.m_oLevelClearInfoDict.Clear();
				oCharacterGameInfo.m_oStageClearInfoDict.Clear();
				oCharacterGameInfo.m_oChapterClearInfoDict.Clear();

				CGameInfoStorage.Inst.SaveGameInfo();
				this.ExLateCallFunc((a_oSender) => CSceneLoader.Inst.LoadScene(this.SceneName));
			}
		}
#endif // #if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)

#if AB_TEST_ENABLE && (DEBUG || DEVELOPMENT_BUILD || PLAY_TEST_ENABLE)
		/** AB 테스트 UI 세트 버튼을 눌렀을 경우 */
		protected override void OnTouchABTUIsSetBtn(EUserType a_eUserType) {
			base.OnTouchABTUIsSetBtn(a_eUserType);

			// 유저 타입이 다를 경우
			if(CCommonUserInfoStorage.Inst.UserInfo.UserType != a_eUserType) {
				string oKey = (a_eUserType == EUserType.A) ? KCDefine.ST_KEY_C_SETUP_A_SET_MSG : KCDefine.ST_KEY_C_SETUP_B_SET_MSG;
				Func.ShowAlertPopup(CStrTable.Inst.GetStr(oKey), (a_oSender, a_bIsOK) => this.OnReceiveABSetPopupResult(a_oSender, a_bIsOK, a_eUserType));
			}
		}

		/** AB 세트 팝업 결과를 수신했을 경우 */
		protected override void OnReceiveABSetPopupResult(CAlertPopup a_oSender, bool a_bIsOK, EUserType a_eUserType) {
			base.OnReceiveABSetPopupResult(a_oSender, a_bIsOK, a_eUserType);

			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				// 에피소드 정보 테이블을 리셋한다 {
				CEpisodeInfoTable.Inst.LevelEpisodeInfoDict.Clear();
				CEpisodeInfoTable.Inst.StageEpisodeInfoDict.Clear();
				CEpisodeInfoTable.Inst.ChapterEpisodeInfoDict.Clear();

				CEpisodeInfoTable.Inst.LoadEpisodeInfos();
				// 에피소드 정보 테이블을 리셋한다 }

				// 레벨 정보 테이블을 리셋한다
				CLevelInfoTable.Inst.LevelInfoDictContainer.Clear();
				CLevelInfoTable.Inst.LoadLevelInfos();

				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MAIN);
			}
		}
#endif // #if AB_TEST_ENABLE && (DEBUG || DEVELOPMENT_BUILD || PLAY_TEST_ENABLE)
		#endregion // 조건부 함수
	}

	/** 서브 메인 씬 관리자 - 스크롤러 셀 뷰 */
	public partial class CSubMainSceneManager : CMainSceneManager, IEnhancedScrollerDelegate {
		#region 함수
		/** 레벨 스크롤러 셀 개수를 반환한다 */
		private int GetNumLevelScrollerCells(EnhancedScroller a_oSender) {
			int nNumExtraCells = (CLevelInfoTable.Inst.GetNumLevelInfos(m_oIDInfoDict[EKey.SEL_ID_INFO].m_nID02, m_oIDInfoDict[EKey.SEL_ID_INFO].m_nID03) % KDefine.MS_MAX_NUM_LEVELS_IN_ROW > KCDefine.B_VAL_0_INT) ? KCDefine.B_VAL_1_INT : KCDefine.B_VAL_0_INT;
			return (CLevelInfoTable.Inst.GetNumLevelInfos(m_oIDInfoDict[EKey.SEL_ID_INFO].m_nID02, m_oIDInfoDict[EKey.SEL_ID_INFO].m_nID03) / KDefine.MS_MAX_NUM_LEVELS_IN_ROW) + nNumExtraCells;
		}

		/** 스테이지 스크롤러 셀 개수를 반환한다 */
		private int GetNumStageScrollerCells(EnhancedScroller a_oSender) {
			int nNumExtraCells = (CLevelInfoTable.Inst.GetNumStageInfos(m_oIDInfoDict[EKey.SEL_ID_INFO].m_nID03) % KDefine.MS_MAX_NUM_STAGES_IN_ROW > KCDefine.B_VAL_0_INT) ? KCDefine.B_VAL_1_INT : KCDefine.B_VAL_0_INT;
			return (CLevelInfoTable.Inst.GetNumStageInfos(m_oIDInfoDict[EKey.SEL_ID_INFO].m_nID03) / KDefine.MS_MAX_NUM_STAGES_IN_ROW) + nNumExtraCells;
		}

		/** 챕터 스크롤러 셀 개수를 반환한다 */
		private int GetNumChapterScrollerCells(EnhancedScroller a_oSender) {
			int nNumExtraCells = (CLevelInfoTable.Inst.NumChapterInfos % KDefine.MS_MAX_NUM_CHAPTERS_IN_ROW > KCDefine.B_VAL_0_INT) ? KCDefine.B_VAL_1_INT : KCDefine.B_VAL_0_INT;
			return (CLevelInfoTable.Inst.NumChapterInfos / KDefine.MS_MAX_NUM_CHAPTERS_IN_ROW) + nNumExtraCells;
		}

		/** 레벨 스크롤러 셀 뷰를 생성한다 */
		private EnhancedScrollerCellView CreateLevelScrollerCellView(EnhancedScroller a_oSender, int a_nDataIdx, int a_nCellIdx, Dictionary<CScrollerCellView.ECallback, System.Action<CScrollerCellView, ulong>> a_oCallbackDict) {
			var oScrollerCellView = a_oSender.GetCellView(m_oScrollerInfoDict[EKey.LEVEL_SCROLLER_INFO].m_oScrollerCellView) as CLevelScrollerCellView;
			oScrollerCellView.Init(CLevelScrollerCellView.MakeParams(a_nDataIdx, CFactory.MakeULevelID(a_nDataIdx * KDefine.MS_MAX_NUM_LEVELS_IN_ROW, m_oIDInfoDict[EKey.SEL_ID_INFO].m_nID02, m_oIDInfoDict[EKey.SEL_ID_INFO].m_nID03), a_oSender, a_oCallbackDict));

			return oScrollerCellView;
		}

		/** 스테이지 스크롤러 셀 뷰를 생성한다 */
		private EnhancedScrollerCellView CreateStageScrollerCellView(EnhancedScroller a_oSender, int a_nDataIdx, int a_nCellIdx, Dictionary<CScrollerCellView.ECallback, System.Action<CScrollerCellView, ulong>> a_oCallbackDict) {
			var oScrollerCellView = a_oSender.GetCellView(m_oScrollerInfoDict[EKey.STAGE_SCROLLER_INFO].m_oScrollerCellView) as CStageScrollerCellView;
			oScrollerCellView.Init(CStageScrollerCellView.MakeParams(a_nDataIdx, CFactory.MakeUStageID(a_nDataIdx * KDefine.MS_MAX_NUM_STAGES_IN_ROW, m_oIDInfoDict[EKey.SEL_ID_INFO].m_nID03), a_oSender, a_oCallbackDict));

			return oScrollerCellView;
		}

		/** 챕터 스크롤러 셀 뷰를 생성한다 */
		private EnhancedScrollerCellView CreateChapterScrollerCellView(EnhancedScroller a_oSender, int a_nDataIdx, int a_nCellIdx, Dictionary<CScrollerCellView.ECallback, System.Action<CScrollerCellView, ulong>> a_oCallbackDict) {
			var oScrollerCellView = a_oSender.GetCellView(m_oScrollerInfoDict[EKey.CHAPTER_SCROLLER_INFO].m_oScrollerCellView) as CChapterScrollerCellView;
			oScrollerCellView.Init(CChapterScrollerCellView.MakeParams(a_nDataIdx, CFactory.MakeUChapterID(a_nDataIdx * KDefine.MS_MAX_NUM_CHAPTERS_IN_ROW), a_oSender, a_oCallbackDict));

			return oScrollerCellView;
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
