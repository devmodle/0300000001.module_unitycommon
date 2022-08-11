#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using EnhancedUI.EnhancedScroller;

namespace MainScene {
	/** 서브 메인 씬 관리자 */
	public partial class CSubMainSceneManager : CMainSceneManager, IEnhancedScrollerDelegate {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			SEL_ID_INFO,
			SEL_SCROLLER,
			LEVEL_SCROLLER_INFO,
			STAGE_SCROLLER_INFO,
			CHAPTER_SCROLLER_INFO,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		private Dictionary<EKey, STIDInfo> m_oIDInfoDict = new Dictionary<EKey, STIDInfo>();

		/** =====> UI <===== */
		private Dictionary<EKey, EnhancedScroller> m_oScrollerDict = new Dictionary<EKey, EnhancedScroller>();
		private Dictionary<EKey, (EnhancedScroller, EnhancedScrollerCellView)> m_oScrollerInfoDict = new Dictionary<EKey, (EnhancedScroller, EnhancedScrollerCellView)>();
		#endregion			// 변수
		
		#region IEnhancedScrollerDelegate
		/** 셀 개수를 반환한다 */
		public int GetNumberOfCells(EnhancedScroller a_oSender) {
			int nNumCells = (CLevelInfoTable.Inst.GetNumLevelInfos(m_oIDInfoDict.GetValueOrDefault(EKey.SEL_ID_INFO).m_nID02, m_oIDInfoDict.GetValueOrDefault(EKey.SEL_ID_INFO).m_nID03) / KDefine.MS_MAX_NUM_LEVELS_IN_ROW);
			int nNumExtraCells = (CLevelInfoTable.Inst.GetNumLevelInfos(m_oIDInfoDict.GetValueOrDefault(EKey.SEL_ID_INFO).m_nID02, m_oIDInfoDict.GetValueOrDefault(EKey.SEL_ID_INFO).m_nID03) % KDefine.MS_MAX_NUM_LEVELS_IN_ROW != KCDefine.B_VAL_0_INT) ? KCDefine.B_VAL_1_INT : KCDefine.B_VAL_0_INT;

			// 스테이지 스크롤러 일 경우
			if(m_oScrollerInfoDict.GetValueOrDefault(EKey.STAGE_SCROLLER_INFO).Item1 == a_oSender) {
				nNumCells = (CLevelInfoTable.Inst.GetNumStageInfos(m_oIDInfoDict.GetValueOrDefault(EKey.SEL_ID_INFO).m_nID03) / KDefine.MS_MAX_NUM_STAGES_IN_ROW);
				nNumExtraCells = (CLevelInfoTable.Inst.GetNumStageInfos(m_oIDInfoDict.GetValueOrDefault(EKey.SEL_ID_INFO).m_nID03) % KDefine.MS_MAX_NUM_STAGES_IN_ROW != KCDefine.B_VAL_0_INT) ? KCDefine.B_VAL_1_INT : KCDefine.B_VAL_0_INT;
			}
			// 챕터 스크롤러 일 경우
			else if(m_oScrollerInfoDict.GetValueOrDefault(EKey.CHAPTER_SCROLLER_INFO).Item1 == a_oSender) {
				nNumCells = (CLevelInfoTable.Inst.NumChapterInfos / KDefine.MS_MAX_NUM_CHAPTERS_IN_ROW);
				nNumExtraCells = (CLevelInfoTable.Inst.NumChapterInfos % KDefine.MS_MAX_NUM_CHAPTERS_IN_ROW != KCDefine.B_VAL_0_INT) ? KCDefine.B_VAL_1_INT : KCDefine.B_VAL_0_INT;
			}
			
			return nNumCells + nNumExtraCells;
		}

		/** 셀 뷰 크기를 반환한다 */
		public float GetCellViewSize(EnhancedScroller a_oSender, int a_nDataIdx) {
			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict.GetValueOrDefault(EKey.LEVEL_SCROLLER_INFO).Item1 == a_oSender) {
				return (m_oScrollerInfoDict.GetValueOrDefault(EKey.LEVEL_SCROLLER_INFO).Item2.transform as RectTransform).sizeDelta.y;
			}

			return (m_oScrollerInfoDict.GetValueOrDefault(EKey.STAGE_SCROLLER_INFO).Item1 == a_oSender) ? (m_oScrollerInfoDict.GetValueOrDefault(EKey.STAGE_SCROLLER_INFO).Item2.transform as RectTransform).sizeDelta.y : (m_oScrollerInfoDict.GetValueOrDefault(EKey.CHAPTER_SCROLLER_INFO).Item2.transform as RectTransform).sizeDelta.y;
		}

		/** 셀 뷰를 반환한다 */
		public EnhancedScrollerCellView GetCellView(EnhancedScroller a_oSender, int a_nDataIdx, int a_nCellIdx) {
			var stIDInfo = CFactory.MakeIDInfo(a_nDataIdx * KDefine.MS_MAX_NUM_LEVELS_IN_ROW, m_oIDInfoDict.GetValueOrDefault(EKey.SEL_ID_INFO).m_nID02, m_oIDInfoDict.GetValueOrDefault(EKey.SEL_ID_INFO).m_nID03);
			var oOriginScrollerCellView = m_oScrollerInfoDict.GetValueOrDefault(EKey.LEVEL_SCROLLER_INFO).Item2;

			// 레벨 스크롤러가 아닐 경우
			if(m_oScrollerInfoDict.GetValueOrDefault(EKey.LEVEL_SCROLLER_INFO).Item1 != a_oSender) {
				stIDInfo = (m_oScrollerInfoDict.GetValueOrDefault(EKey.STAGE_SCROLLER_INFO).Item1 == a_oSender) ? CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, a_nDataIdx * KDefine.MS_MAX_NUM_STAGES_IN_ROW, m_oIDInfoDict.GetValueOrDefault(EKey.SEL_ID_INFO).m_nID03) : CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, a_nDataIdx * KDefine.MS_MAX_NUM_CHAPTERS_IN_ROW);
				oOriginScrollerCellView = (m_oScrollerInfoDict.GetValueOrDefault(EKey.STAGE_SCROLLER_INFO).Item1 == a_oSender) ? m_oScrollerInfoDict.GetValueOrDefault(EKey.STAGE_SCROLLER_INFO).Item2 : m_oScrollerInfoDict.GetValueOrDefault(EKey.CHAPTER_SCROLLER_INFO).Item2;
			}

			var stParams = new CScrollerCellView.STParams() {
				m_nID = CFactory.MakeULevelID(stIDInfo.m_nID01, stIDInfo.m_nID02, stIDInfo.m_nID03),

				m_oCallbackDict = new Dictionary<CScrollerCellView.ECallback, System.Action<CScrollerCellView, ulong>>() {
					[CScrollerCellView.ECallback.SEL] = this.OnTouchSCVSelBtn
				}
			};

			var oScrollerCellView = a_oSender.GetCellView(oOriginScrollerCellView) as CScrollerCellView;

			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict.GetValueOrDefault(EKey.LEVEL_SCROLLER_INFO).Item1 == a_oSender) {
				(oScrollerCellView as CLevelScrollerCellView)?.Init(new CLevelScrollerCellView.STParams() {
					m_stBaseParams = stParams
				});
			}
			// 스테이지 스크롤러 일 경우
			else if(m_oScrollerInfoDict.GetValueOrDefault(EKey.STAGE_SCROLLER_INFO).Item1 == a_oSender) {
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
		#endregion			// 함수

		#region 조건부 함수
#if AB_TEST_ENABLE && (DEBUG || DEVELOPMENT_BUILD || PLAY_TEST_ENABLE)
		/** AB 테스트 UI 를 설정한다 */
		private void SetupABTestUIs() {
			var oTextDict = CCollectionManager.Inst.SpawnDict<string, TMP_Text>();
			var oHLayoutGroupDict = CCollectionManager.Inst.SpawnDict<string, HorizontalLayoutGroup>();

			try {
				// 레이아웃을 설정한다 {
				CFunc.SetupLayoutGroups(new List<(string, string, GameObject, GameObject)>() {
					(KCDefine.MS_OBJ_N_AB_T_UIS_SET_UIS, KCDefine.MS_OBJ_N_AB_T_UIS_SET_UIS, this.UpUIs, null)
				}, oHLayoutGroupDict, false);

				oHLayoutGroupDict[KCDefine.MS_OBJ_N_AB_T_UIS_SET_UIS].spacing = KCDefine.B_VAL_4_REAL * KCDefine.B_VAL_5_REAL;

				(oHLayoutGroupDict[KCDefine.MS_OBJ_N_AB_T_UIS_SET_UIS].transform as RectTransform).pivot = KCDefine.B_ANCHOR_UP_CENTER;
				(oHLayoutGroupDict[KCDefine.MS_OBJ_N_AB_T_UIS_SET_UIS].transform as RectTransform).anchorMin = KCDefine.B_ANCHOR_UP_CENTER;
				(oHLayoutGroupDict[KCDefine.MS_OBJ_N_AB_T_UIS_SET_UIS].transform as RectTransform).anchorMax = KCDefine.B_ANCHOR_UP_CENTER;
				(oHLayoutGroupDict[KCDefine.MS_OBJ_N_AB_T_UIS_SET_UIS].transform as RectTransform).anchoredPosition = Vector3.zero;

				var oContentsSizeFitter = oHLayoutGroupDict[KCDefine.MS_OBJ_N_AB_T_UIS_SET_UIS].gameObject.ExAddComponent<ContentSizeFitter>();
				oContentsSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
				oContentsSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
				// 레이아웃을 설정한다 }

				// 텍스트를 설정한다 {
				CFunc.SetupComponents(new List<(string, string, GameObject, GameObject)>() {
					(KCDefine.U_OBJ_N_A_SET_BTN, KCDefine.U_OBJ_N_A_SET_BTN, oHLayoutGroupDict[KCDefine.MS_OBJ_N_AB_T_UIS_SET_UIS].gameObject, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_TMP_TEXT_BTN)),
					(KCDefine.U_OBJ_N_B_SET_BTN, KCDefine.U_OBJ_N_B_SET_BTN, oHLayoutGroupDict[KCDefine.MS_OBJ_N_AB_T_UIS_SET_UIS].gameObject, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_TMP_TEXT_BTN))
				}, oTextDict, false);

				oTextDict[KCDefine.U_OBJ_N_A_SET_BTN].fontSize = KCDefine.U_DEF_SIZE_FONT;
				oTextDict[KCDefine.U_OBJ_N_A_SET_BTN].ExSetText(CStrTable.Inst.GetStr(KCDefine.ST_KEY_MAIN_SM_A_SET_TEXT));

				oTextDict[KCDefine.U_OBJ_N_B_SET_BTN].fontSize = KCDefine.U_DEF_SIZE_FONT;
				oTextDict[KCDefine.U_OBJ_N_B_SET_BTN].ExSetText(CStrTable.Inst.GetStr(KCDefine.ST_KEY_MAIN_SM_B_SET_TEXT));
				// 텍스트를 설정한다 }

				// 버튼을 설정한다
				CFunc.SetupButtons(new List<(GameObject, UnityAction)>() {
					(oTextDict[KCDefine.U_OBJ_N_A_SET_BTN].gameObject, () => this.OnTouchABTUIsSetBtn(EUserType.A)),
					(oTextDict[KCDefine.U_OBJ_N_B_SET_BTN].gameObject, () => this.OnTouchABTUIsSetBtn(EUserType.B))
				}, false);
			} finally {
				CCollectionManager.Inst.DespawnDict(oTextDict);
				CCollectionManager.Inst.DespawnDict(oHLayoutGroupDict);
			}
		}

		/** AB 테스트 UI 세트 버튼을 눌렀을 경우 */
		private void OnTouchABTUIsSetBtn(EUserType a_eUserType) {
			// 유저 타입이 다를 경우
			if(CCommonUserInfoStorage.Inst.UserInfo.UserType != a_eUserType) {
				Func.ShowAlertPopup(CStrTable.Inst.GetStr((a_eUserType == EUserType.A) ? KCDefine.ST_KEY_EDITOR_A_SET_P_MSG : KCDefine.ST_KEY_EDITOR_B_SET_P_MSG), (a_oSender, a_bIsOK) => this.OnReceiveABSetPopupResult(a_oSender, a_bIsOK, a_eUserType));
			}
		}

		/** AB 세트 팝업 결과를 수신했을 경우 */
		private void OnReceiveABSetPopupResult(CAlertPopup a_oSender, bool a_bIsOK, EUserType a_eUserType) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				CCommonUserInfoStorage.Inst.UserInfo.UserType = a_eUserType;
				CCommonUserInfoStorage.Inst.SaveUserInfo();

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
#endif			// #if AB_TEST_ENABLE && (DEBUG || DEVELOPMENT_BUILD || PLAY_TEST_ENABLE)
		#endregion			// 조건부 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
