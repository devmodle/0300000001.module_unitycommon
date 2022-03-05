using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;

#if SCRIPT_TEMPLATE_ONLY
#if RUNTIME_TEMPLATES_MODULE_ENABLE
/** 서브 메인 씬 관리자 */
public class CSubMainSceneManager : CMainSceneManager, IEnhancedScrollerDelegate {
	/** 스크롤러 */
	private enum EScroller {
		NONE = -1,
		LEVEL,
		STAGE,
		CHAPTER,
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
	private Dictionary<EScroller, EnhancedScroller> m_oScrollerDict = new Dictionary<EScroller, EnhancedScroller>();
	private Dictionary<EScroller, EnhancedScrollerCellView> m_oOriginScrollerCellViewDict = new Dictionary<EScroller, EnhancedScrollerCellView>();

#if DEBUG || DEVELOPMENT_BUILD
	[SerializeField] private STTestUIs m_stTestUIs;
#endif			// #if DEBUG || DEVELOPMENT_BUILD
	#endregion			// 변수

	#region 추가 변수

	#endregion			// 추가 변수

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region IEnhancedScrollerDelegate
	/** 셀 개수를 반환한다 */
	public int GetNumberOfCells(EnhancedScroller a_oSender) {
		// 레벨 스크롤러 일 경우
		if(m_oScrollerDict[EScroller.LEVEL] == a_oSender) {
			return CLevelInfoTable.Inst.GetNumLevelInfos(m_stSelIDInfo.m_nStageID, m_stSelIDInfo.m_nChapterID) / KDefine.MS_MAX_NUM_LEVELS_IN_ROW;
		}

		return (m_oScrollerDict[EScroller.STAGE] == a_oSender) ? CLevelInfoTable.Inst.GetNumStageInfos(m_stSelIDInfo.m_nChapterID) / KDefine.MS_MAX_NUM_STAGES_IN_ROW : CLevelInfoTable.Inst.NumChapterInfos / KDefine.MS_MAX_NUM_CHAPTERS_IN_ROW;
	}

	/** 셀 뷰 크기를 반환한다 */
	public float GetCellViewSize(EnhancedScroller a_oSender, int a_nDataIdx) {
		// 레벨 스크롤러 일 경우
		if(m_oScrollerDict[EScroller.LEVEL] == a_oSender) {
			return (m_oOriginScrollerCellViewDict[EScroller.LEVEL].transform as RectTransform).sizeDelta.y;
		}

		return (m_oScrollerDict[EScroller.STAGE] == a_oSender) ? (m_oOriginScrollerCellViewDict[EScroller.STAGE].transform as RectTransform).sizeDelta.y : (m_oOriginScrollerCellViewDict[EScroller.CHAPTER].transform as RectTransform).sizeDelta.y;
	}

	/** 셀 뷰를 반환한다 */
	public EnhancedScrollerCellView GetCellView(EnhancedScroller a_oSender, int a_nDataIdx, int a_nCellIdx) {
		var stIDInfo = CFactory.MakeIDInfo(a_nDataIdx * KDefine.MS_MAX_NUM_LEVELS_IN_ROW, m_stSelIDInfo.m_nStageID, m_stSelIDInfo.m_nChapterID);
		var oOriginScrollerCellView = m_oOriginScrollerCellViewDict[EScroller.LEVEL];

		// 레벨 스크롤러가 아닐 경우
		if(m_oScrollerDict[EScroller.LEVEL] != a_oSender) {
			stIDInfo = (m_oScrollerDict[EScroller.STAGE] == a_oSender) ? CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, a_nDataIdx * KDefine.MS_MAX_NUM_STAGES_IN_ROW, m_stSelIDInfo.m_nChapterID) : CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, a_nDataIdx * KDefine.MS_MAX_NUM_CHAPTERS_IN_ROW);
			oOriginScrollerCellView = (m_oScrollerDict[EScroller.STAGE] == a_oSender) ? m_oOriginScrollerCellViewDict[EScroller.STAGE] : m_oOriginScrollerCellViewDict[EScroller.CHAPTER];
		}
		
		var stParams = new CScrollerCellView.STParams() {
			m_nID = CFactory.MakeUniqueLevelID(stIDInfo.m_nID, stIDInfo.m_nStageID, stIDInfo.m_nChapterID),

			m_oCallbackDict = new Dictionary<CScrollerCellView.ECallback, System.Action<CScrollerCellView, long>>() {
				[CScrollerCellView.ECallback.SEL] = this.OnTouchSCVSelBtn
			}
		};

		var oScrollerCellView = a_oSender.GetCellView(oOriginScrollerCellView) as CScrollerCellView;
		oScrollerCellView.Init(stParams);

		return oScrollerCellView;
	}
	#endregion			// IEnhancedScrollerDelegate
	
	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
#if CREATIVE_DIST_BUILD
			for(int i = 0; i < CLevelInfoTable.Inst.NumLevelInfosDictContainer.Count; ++i) {
				for(int j = 0; j < CLevelInfoTable.Inst.NumLevelInfosDictContainer[i].Count; ++j) {
					for(int k = 0; k < CLevelInfoTable.Inst.NumLevelInfosDictContainer[i][j]; ++k) {
						// 클리어 정보가 없을 경우
						if(!CGameInfoStorage.Inst.IsClearLevel(k, j, i)) {
							CGameInfoStorage.Inst.AddClearInfo(Factory.MakeClearInfo(k, j, i));
						}

						var oClearInfo = CGameInfoStorage.Inst.GetClearInfo(k, j, i);
						oClearInfo.NumClearMarks = KDefine.G_MAX_NUM_LEVEL_CLEAR_MARKS;
					}
				}
			}

			CUserInfoStorage.Inst.UserInfo.NumCoins = KCDefine.B_UNIT_DIGITS_PER_HUNDRED_THOUSAND;
			CGameInfoStorage.Inst.SaveGameInfo();
#endif			// #if CREATIVE_DIST_BUILD

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
			Func.ShowQuitPopup(this.OnReceiveQuitPopupResult);
		}
	}

	/** 씬을 설정한다 */
	private void SetupAwake() {
		var ePlayMode = CGameInfoStorage.Inst.PlayMode;
		m_stSelIDInfo = (ePlayMode == EPlayMode.NORM && CGameInfoStorage.Inst.PlayLevelInfo != null) ? CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo : CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT);

		// 버튼을 설정한다
		var oPlayBtn = this.UIsBase.ExFindComponent<Button>(KCDefine.U_OBJ_N_PLAY_BTN);
		oPlayBtn?.ExAddListener(this.OnTouchPlayBtn, true, false);

		// 스크롤 뷰를 설정한다 {
		var oLevelScrollerCellView = CResManager.Inst.GetRes<GameObject>(KCDefine.MS_OBJ_P_LEVEL_SCROLLER_CELL_VIEW);
		var oStageScrollerCellView = CResManager.Inst.GetRes<GameObject>(KCDefine.MS_OBJ_P_STAGE_SCROLLER_CELL_VIEW);
		var oChapterScrollerCellView = CResManager.Inst.GetRes<GameObject>(KCDefine.MS_OBJ_P_CHAPTER_SCROLLER_CELL_VIEW);

		m_oScrollerDict.TryAdd(EScroller.LEVEL, this.UIsBase.ExFindComponent<EnhancedScroller>(KCDefine.U_OBJ_N_LEVEL_SCROLL_VIEW));
		m_oScrollerDict.TryAdd(EScroller.STAGE, this.UIsBase.ExFindComponent<EnhancedScroller>(KCDefine.U_OBJ_N_STAGE_SCROLL_VIEW));
		m_oScrollerDict.TryAdd(EScroller.CHAPTER, this.UIsBase.ExFindComponent<EnhancedScroller>(KCDefine.U_OBJ_N_CHAPTER_SCROLL_VIEW));

		m_oOriginScrollerCellViewDict.TryAdd(EScroller.LEVEL, oLevelScrollerCellView?.GetComponentInChildren<EnhancedScrollerCellView>());
		m_oOriginScrollerCellViewDict.TryAdd(EScroller.STAGE, oStageScrollerCellView?.GetComponentInChildren<EnhancedScrollerCellView>());
		m_oOriginScrollerCellViewDict.TryAdd(EScroller.CHAPTER, oChapterScrollerCellView?.GetComponentInChildren<EnhancedScrollerCellView>());
		
		foreach(var stKeyVal in m_oScrollerDict) {
			stKeyVal.Value?.ExSetDelegate(this, false);
		}
		// 스크롤 뷰를 설정한다 }

#if DEBUG || DEVELOPMENT_BUILD
		this.SetupTestUIs();
#endif			// #if DEBUG || DEVELOPMENT_BUILD
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

#if !TITLE_SCENE_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE
		// 업데이트가 가능 할 경우
		if(!CAppInfoStorage.Inst.IsIgnoreUpdate && CCommonAppInfoStorage.Inst.IsEnableUpdate()) {
			CAppInfoStorage.Inst.IsIgnoreUpdate = true;
			this.ExLateCallFunc((a_oSender) => Func.ShowUpdatePopup(this.OnReceiveUpdatePopupResult));
		}
#endif			// #if !TITLE_SCENE_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE
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
		// Do Something
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
	#endregion			// 조건부 함수

	#region 추가 함수
#if DEBUG || DEVELOPMENT_BUILD

#endif			// #if DEBUG || DEVELOPMENT_BUILD
	#endregion			// 추가 함수
}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
