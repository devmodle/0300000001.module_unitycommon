using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;
using TMPro;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
namespace MainScene {
	/** 서브 메인 씬 관리자 */
	public partial class CSubMainSceneManager : CMainSceneManager, IEnhancedScrollerDelegate {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			LEVEL_SCROLLER_INFO,
			STAGE_SCROLLER_INFO,
			CHAPTER_SCROLLER_INFO,
			[HideInInspector] MAX_VAL
		}

#if DEBUG || DEVELOPMENT_BUILD
		/** 테스트 UI */
		[System.Serializable]
		private struct STTestUIs {
			// Do Something
		}
#endif			// #if DEBUG || DEVELOPMENT_BUILD

		#region 변수
		private STIDInfo m_stSelIDInfo;

		/** =====> UI <===== */
		private Dictionary<EKey, (EnhancedScroller, EnhancedScrollerCellView)> m_oScrollerInfoDict = new Dictionary<EKey, (EnhancedScroller, EnhancedScrollerCellView)>() {
			[EKey.LEVEL_SCROLLER_INFO] = (null, null),
			[EKey.STAGE_SCROLLER_INFO] = (null, null),
			[EKey.CHAPTER_SCROLLER_INFO] = (null, null)
		};

#if DEBUG || DEVELOPMENT_BUILD
		[SerializeField] private STTestUIs m_stTestUIs;
#endif			// #if DEBUG || DEVELOPMENT_BUILD
		#endregion			// 변수

		#region 프로퍼티
		public override bool IsIgnoreOverlayScene => false;
		#endregion			// 프로퍼티

		#region IEnhancedScrollerDelegate
		/** 셀 개수를 반환한다 */
		public int GetNumberOfCells(EnhancedScroller a_oSender) {
			int nNumCells = (CLevelInfoTable.Inst.GetNumLevelInfos(m_stSelIDInfo.m_nStageID, m_stSelIDInfo.m_nChapterID) / KDefine.MS_MAX_NUM_LEVELS_IN_ROW);
			int nNumExtraCells = (CLevelInfoTable.Inst.GetNumLevelInfos(m_stSelIDInfo.m_nStageID, m_stSelIDInfo.m_nChapterID) % KDefine.MS_MAX_NUM_LEVELS_IN_ROW != KCDefine.B_VAL_0_INT) ? KCDefine.B_VAL_1_INT : KCDefine.B_VAL_0_INT;

			// 스테이지 스크롤러 일 경우
			if(m_oScrollerInfoDict[EKey.STAGE_SCROLLER_INFO].Item1 == a_oSender) {
				nNumCells = (CLevelInfoTable.Inst.GetNumStageInfos(m_stSelIDInfo.m_nChapterID) / KDefine.MS_MAX_NUM_STAGES_IN_ROW);
				nNumExtraCells = (CLevelInfoTable.Inst.GetNumStageInfos(m_stSelIDInfo.m_nChapterID) % KDefine.MS_MAX_NUM_STAGES_IN_ROW != KCDefine.B_VAL_0_INT) ? KCDefine.B_VAL_1_INT : KCDefine.B_VAL_0_INT;
			}
			// 챕터 스크롤러 일 경우
			else if(m_oScrollerInfoDict[EKey.CHAPTER_SCROLLER_INFO].Item1 == a_oSender) {
				nNumCells = (CLevelInfoTable.Inst.NumChapterInfos / KDefine.MS_MAX_NUM_CHAPTERS_IN_ROW);
				nNumExtraCells = (CLevelInfoTable.Inst.NumChapterInfos % KDefine.MS_MAX_NUM_CHAPTERS_IN_ROW != KCDefine.B_VAL_0_INT) ? KCDefine.B_VAL_1_INT : KCDefine.B_VAL_0_INT;
			}
			
			return nNumCells + nNumExtraCells;
		}

		/** 셀 뷰 크기를 반환한다 */
		public float GetCellViewSize(EnhancedScroller a_oSender, int a_nDataIdx) {
			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict[EKey.LEVEL_SCROLLER_INFO].Item1 == a_oSender) {
				return (m_oScrollerInfoDict[EKey.LEVEL_SCROLLER_INFO].Item2.transform as RectTransform).sizeDelta.y;
			}

			return (m_oScrollerInfoDict[EKey.STAGE_SCROLLER_INFO].Item1 == a_oSender) ? (m_oScrollerInfoDict[EKey.STAGE_SCROLLER_INFO].Item2.transform as RectTransform).sizeDelta.y : (m_oScrollerInfoDict[EKey.CHAPTER_SCROLLER_INFO].Item2.transform as RectTransform).sizeDelta.y;
		}

		/** 셀 뷰를 반환한다 */
		public EnhancedScrollerCellView GetCellView(EnhancedScroller a_oSender, int a_nDataIdx, int a_nCellIdx) {
			var stIDInfo = CFactory.MakeIDInfo(a_nDataIdx * KDefine.MS_MAX_NUM_LEVELS_IN_ROW, m_stSelIDInfo.m_nStageID, m_stSelIDInfo.m_nChapterID);
			var oOriginScrollerCellView = m_oScrollerInfoDict[EKey.LEVEL_SCROLLER_INFO].Item2;

			// 레벨 스크롤러가 아닐 경우
			if(m_oScrollerInfoDict[EKey.LEVEL_SCROLLER_INFO].Item1 != a_oSender) {
				stIDInfo = (m_oScrollerInfoDict[EKey.STAGE_SCROLLER_INFO].Item1 == a_oSender) ? CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, a_nDataIdx * KDefine.MS_MAX_NUM_STAGES_IN_ROW, m_stSelIDInfo.m_nChapterID) : CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, a_nDataIdx * KDefine.MS_MAX_NUM_CHAPTERS_IN_ROW);
				oOriginScrollerCellView = (m_oScrollerInfoDict[EKey.STAGE_SCROLLER_INFO].Item1 == a_oSender) ? m_oScrollerInfoDict[EKey.STAGE_SCROLLER_INFO].Item2 : m_oScrollerInfoDict[EKey.CHAPTER_SCROLLER_INFO].Item2;
			}

			var stParams = new CScrollerCellView.STParams() {
				m_nID = CFactory.MakeUniqueLevelID(stIDInfo.m_nID, stIDInfo.m_nStageID, stIDInfo.m_nChapterID),

				m_oCallbackDict = new Dictionary<CScrollerCellView.ECallback, System.Action<CScrollerCellView, long>>() {
					[CScrollerCellView.ECallback.SEL] = this.OnTouchSCVSelBtn
				}
			};

			var oScrollerCellView = a_oSender.GetCellView(oOriginScrollerCellView) as CScrollerCellView;

			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict[EKey.LEVEL_SCROLLER_INFO].Item1 == a_oSender) {
				(oScrollerCellView as CLevelScrollerCellView)?.Init(new CLevelScrollerCellView.STParams() {
					m_stBaseParams = stParams
				});
			}
			// 스테이지 스크롤러 일 경우
			else if(m_oScrollerInfoDict[EKey.STAGE_SCROLLER_INFO].Item1 == a_oSender) {
				(oScrollerCellView as CStageScrollerCellView)?.Init(new CStageScrollerCellView.STParams() {
					m_stBaseParams = stParams
				});
			} else {
				(oScrollerCellView as CChapterScrollerCellView)?.Init(new CChapterScrollerCellView.STParams() {
					m_stBaseParams = stParams
				});
			}

			return oScrollerCellView;
		}
		#endregion			// IEnhancedScrollerDelegate
		
		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
#if CREATIVE_DIST_BUILD
				for(int i = 0; i < CLevelInfoTable.Inst.NumLevelInfosDictContainer.Count; ++i) {
					for(int j = 0; j < CLevelInfoTable.Inst.NumLevelInfosDictContainer[i].Count; ++j) {
						for(int k = 0; k < CLevelInfoTable.Inst.NumLevelInfosDictContainer[i][j]; ++k) {
							// 클리어 정보가 없을 경우
							if(!CGameInfoStorage.Inst.IsClearLevel(k, j, i)) {
								CGameInfoStorage.Inst.AddLevelClearInfo(Factory.MakeClearInfo(k, j, i));
							}

							var oLevelClearInfo = CGameInfoStorage.Inst.GetLevelClearInfo(k, j, i);
							oLevelClearInfo.NumMarks = KDefine.G_MAX_NUM_LEVEL_MARKS;
						}
					}
				}

				CUserInfoStorage.Inst.UserInfo.NumCoins = KCDefine.B_UNIT_DIGITS_PER_HUNDRED_THOUSAND;
				CGameInfoStorage.Inst.SaveGameInfo();
#endif			// #if CREATIVE_DIST_BUILD

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

				Func.PlayBGSnd(EResKinds.SND_BG_SCENE_MAIN);
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
				CFunc.ShowLogWarning($"CSubMainSceneManager.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 앱이 정지 되었을 경우 */
		public virtual void OnApplicationPause(bool a_bIsPause) {
			// 재개 되었을 경우
			if(!a_bIsPause && CSceneManager.IsAppRunning) {
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
				Func.ShowQuitPopup(this.OnReceiveQuitPopupResult);
			}
		}

		/** 씬을 설정한다 */
		private void SetupAwake() {
			var ePlayMode = CGameInfoStorage.Inst.PlayMode;
			m_stSelIDInfo = (ePlayMode == EPlayMode.NORM && CGameInfoStorage.Inst.PlayLevelInfo != null) ? CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo : CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT);

			// 버튼을 설정한다
			this.UIsBase.ExFindComponent<Button>(KCDefine.U_OBJ_N_PLAY_BTN)?.ExAddListener(this.OnTouchPlayBtn, true, false);
			this.UIsBase.ExFindComponent<Button>(KCDefine.U_OBJ_N_STORE_BTN)?.ExAddListener(this.OnTouchStoreBtn, true, false);
			this.UIsBase.ExFindComponent<Button>(KCDefine.U_OBJ_N_SETTINGS_BTN)?.ExAddListener(this.OnTouchSettingsBtn, true, false);

			// 스크롤 뷰를 설정한다 {
			m_oScrollerInfoDict[EKey.LEVEL_SCROLLER_INFO] = (
				this.UIsBase.ExFindComponent<EnhancedScroller>(KCDefine.U_OBJ_N_LEVEL_SCROLL_VIEW),
				CResManager.Inst.GetRes<GameObject>(KCDefine.MS_OBJ_P_LEVEL_SCROLLER_CELL_VIEW)?.GetComponentInChildren<EnhancedScrollerCellView>()
			);

			m_oScrollerInfoDict[EKey.STAGE_SCROLLER_INFO] = (
				this.UIsBase.ExFindComponent<EnhancedScroller>(KCDefine.U_OBJ_N_STAGE_SCROLL_VIEW),
				CResManager.Inst.GetRes<GameObject>(KCDefine.MS_OBJ_P_STAGE_SCROLLER_CELL_VIEW)?.GetComponentInChildren<EnhancedScrollerCellView>()
			);

			m_oScrollerInfoDict[EKey.CHAPTER_SCROLLER_INFO] = (
				this.UIsBase.ExFindComponent<EnhancedScroller>(KCDefine.U_OBJ_N_CHAPTER_SCROLL_VIEW),
				CResManager.Inst.GetRes<GameObject>(KCDefine.MS_OBJ_P_CHAPTER_SCROLLER_CELL_VIEW)?.GetComponentInChildren<EnhancedScrollerCellView>()
			);

			foreach(var stKeyVal in m_oScrollerInfoDict) {
				stKeyVal.Value.Item1?.ExSetDelegate(this, false);
			}
			// 스크롤 뷰를 설정한다 }

#if DEBUG || DEVELOPMENT_BUILD
			this.SetupTestUIs();
#endif			// #if DEBUG || DEVELOPMENT_BUILD

#if AB_TEST_ENABLE && (DEBUG || DEVELOPMENT_BUILD || PLAY_TEST_ENABLE)
			this.SetupABTestUIs();
#endif			// #if AB_TEST_ENABLE && (DEBUG || DEVELOPMENT_BUILD || PLAY_TEST_ENABLE)
		}

		/** 씬을 설정한다 */
		private void SetupStart() {
			// 일일 미션 리셋이 가능 할 경우
			if(CGameInfoStorage.Inst.IsEnableResetDailyMission) {
				CGameInfoStorage.Inst.GameInfo.PrevDailyMissionTime = System.DateTime.Today;
				CGameInfoStorage.Inst.GameInfo.m_oCompleteDailyMissionKindsList.Clear();

				CGameInfoStorage.Inst.SaveGameInfo();
			}

			// 무료 보상 획득이 가능 할 경우
			if(CGameInfoStorage.Inst.IsEnableGetFreeReward) {
				CGameInfoStorage.Inst.GameInfo.FreeRewardAcquireTimes = KCDefine.B_VAL_0_INT;
				CGameInfoStorage.Inst.GameInfo.PrevFreeRewardTime = System.DateTime.Today;
				
				CGameInfoStorage.Inst.SaveGameInfo();
			}
			
#if DAILY_REWARD_ENABLE
			// 일일 보상 획득이 가능 할 경우
			if(CGameInfoStorage.Inst.IsEnableGetDailyReward) {
				Func.ShowDailyRewardPopup(this.PopupUIs, (a_oSender) => (a_oSender as CDailyRewardPopup).Init());
			}
#endif			// #if DAILY_REWARD_ENABLE

#if NEWTON_SOFT_JSON_MODULE_ENABLE
			// 업데이트가 가능 할 경우
			if(!CAppInfoStorage.Inst.IsIgnoreUpdate && !COptsInfoTable.Inst.EtcOptsInfo.m_bIsEnableTitleScene && CCommonAppInfoStorage.Inst.IsEnableUpdate()) {
				CAppInfoStorage.Inst.IsIgnoreUpdate = true;
				this.ExLateCallFunc((a_oSender) => Func.ShowUpdatePopup(this.OnReceiveUpdatePopupResult));
			}
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
		}

		/** UI 상태를 갱신한다 */
		private void UpdateUIsState() {
#if DEBUG || DEVELOPMENT_BUILD
			this.UpdateTestUIsState();
#endif			// #if DEBUG || DEVELOPMENT_BUILD
		}

		/** 종료 팝업 결과를 수신했을 경우 */
		private void OnReceiveQuitPopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				a_oSender.IsIgnoreAni = true;
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

		/** 플레이 버튼을 눌렀을 경우 */
		private void OnTouchPlayBtn() {
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME);
		}

		/** 상점 버튼을 눌렀을 경우 */
		private void OnTouchStoreBtn() {
			CSceneManager.GetSceneManager<OverlayScene.CSubOverlaySceneManager>(KCDefine.B_SCENE_N_OVERLAY)?.ShowStorePopup();
		}

		/** 설정 버튼을 눌렀을 경우 */
		private void OnTouchSettingsBtn() {
			Func.ShowSettingsPopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CSettingsPopup).Init();
			});
		}

		/** 스크롤러 셀 뷰 선택 버튼을 눌렀을 경우 */
		private void OnTouchSCVSelBtn(CScrollerCellView a_oSender, long a_nID) {
			// Do Something
		}
		#endregion			// 함수

		#region 조건부 함수
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

#if AB_TEST_ENABLE && (DEBUG || DEVELOPMENT_BUILD || PLAY_TEST_ENABLE)
		/** AB 테스트 UI 를 설정한다 */
		private void SetupABTestUIs() {
			var oABTUIsSetUIsLayoutGroup = CFactory.CreateObj<HorizontalLayoutGroup>(KCDefine.MS_OBJ_N_AB_T_UIS_SET_UIS, this.UpUIs);
			oABTUIsSetUIsLayoutGroup.spacing = KCDefine.B_VAL_4_FLT * KCDefine.B_VAL_5_FLT;
			oABTUIsSetUIsLayoutGroup.ExReset();

			(oABTUIsSetUIsLayoutGroup.transform as RectTransform).pivot = KCDefine.B_ANCHOR_UP_CENTER;
			(oABTUIsSetUIsLayoutGroup.transform as RectTransform).anchorMin = KCDefine.B_ANCHOR_UP_CENTER;
			(oABTUIsSetUIsLayoutGroup.transform as RectTransform).anchorMax = KCDefine.B_ANCHOR_UP_CENTER;
			(oABTUIsSetUIsLayoutGroup.transform as RectTransform).anchoredPosition = Vector3.zero;

			var oContentsSizeFitter = oABTUIsSetUIsLayoutGroup.gameObject.ExAddComponent<ContentSizeFitter>();
			oContentsSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			oContentsSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

			// 텍스트를 설정한다 {
			var oASetText = CFactory.CreateCloneObj<TMP_Text>(KCDefine.U_OBJ_N_A_SET_BTN, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_TMP_TEXT_BTN), oABTUIsSetUIsLayoutGroup.gameObject);
			oASetText.fontSize = KCDefine.U_DEF_SIZE_FONT;
			oASetText.ExSetText(CStrTable.Inst.GetStr(KCDefine.ST_KEY_MAIN_SM_A_SET_TEXT), EFontSet._1);

			var oBSetText = CFactory.CreateCloneObj<TMP_Text>(KCDefine.U_OBJ_N_B_SET_BTN, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_TMP_TEXT_BTN), oABTUIsSetUIsLayoutGroup.gameObject);
			oBSetText.fontSize = KCDefine.U_DEF_SIZE_FONT;
			oBSetText.ExSetText(CStrTable.Inst.GetStr(KCDefine.ST_KEY_MAIN_SM_B_SET_TEXT), EFontSet._1);
			// 텍스트를 설정한다 }

			// 버튼을 설정한다
			oASetText.GetComponentInChildren<Button>().onClick.AddListener(() => this.OnTouchABTUIsSetBtn(EUserType.A));
			oBSetText.GetComponentInChildren<Button>().onClick.AddListener(() => this.OnTouchABTUIsSetBtn(EUserType.B));
		}

		/** AB 테스트 UI 세트 버튼을 눌렀을 경우 */
		private void OnTouchABTUIsSetBtn(EUserType a_eUserType) {
			// 유저 타입이 다를 경우
			if(CCommonUserInfoStorage.Inst.UserInfo.UserType != a_eUserType) {
				CCommonUserInfoStorage.Inst.UserInfo.UserType = a_eUserType;
				CCommonUserInfoStorage.Inst.SaveUserInfo();

				// 에피소드 정보 테이블을 리셋한다 {
				CEpisodeInfoTable.Inst.LevelInfoDict.Clear();
				CEpisodeInfoTable.Inst.StageInfoDict.Clear();
				CEpisodeInfoTable.Inst.ChapterInfoDict.Clear();

				CEpisodeInfoTable.Inst.LoadEpisodeInfos();
				// 에피소드 정보 테이블을 리셋한다 }

				// 레벨 정보 테이블을 리셋한다
				CLevelInfoTable.Inst.LevelInfoDictContainer.Clear();
				CLevelInfoTable.Inst.LoadLevelInfos();

				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MAIN);
			}
		}
#endif			// #if AB_TEST_ENABLE && (DEBUG || DEVELOPMENT_BUILD || PLAY_TEST_ENABLE)
		#endregion			// 조건부 함수
	}
}
#endif			// #if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
