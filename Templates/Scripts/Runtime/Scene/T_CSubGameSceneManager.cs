using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
namespace GameScene {
	/** 서브 게임 씬 관리자 */
	public partial class CSubGameSceneManager : CGameSceneManager {
		/** 식별자 */
		private enum EKey {
			NONE = -1,

			LEVEL_INFO,
			CLEAR_INFO,
			BG_TOUCH_DISPATCHER,

#if ENGINE_TEMPLATES_MODULE_ENABLE
			ENGINE,
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE

			[HideInInspector] MAX_VAL
		}

		/** 팝업 결과 */
		private enum EPopupResult {
			NONE = -1,
			NEXT,
			RETRY,
			RESUME,
			CONTINUE,
			LEAVE,
			[HideInInspector] MAX_VAL
		}

		/** 보상 광고 UI */
		private enum ERewardAdsUIs {
			NONE = -1,
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
		private bool m_bIsLeave = false;
		private int m_nContinueTimes = 0;
		private ERewardAdsUIs m_eSelRewardAdsUIs = ERewardAdsUIs.NONE;

		private Dictionary<EKey, CLevelInfo> m_oLevelInfoDict = new Dictionary<EKey, CLevelInfo>() {
			[EKey.LEVEL_INFO] = null
		};

		private Dictionary<EKey, CClearInfo> m_oClearInfoDict = new Dictionary<EKey, CClearInfo>() {
			[EKey.CLEAR_INFO] = null
		};

		private Dictionary<EKey, CTouchDispatcher> m_oTouchDispatcherDict = new Dictionary<EKey, CTouchDispatcher>() {
			[EKey.BG_TOUCH_DISPATCHER] = null
		};

#if ENGINE_TEMPLATES_MODULE_ENABLE
		private Dictionary<EKey, SampleEngineName.CEngine> m_oEngineDict = new Dictionary<EKey, SampleEngineName.CEngine>() {
			[EKey.ENGINE] = null
		};
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE

		/** =====> UI <===== */
#if DEBUG || DEVELOPMENT_BUILD
		[SerializeField] private STTestUIs m_stTestUIs;
#endif			// #if DEBUG || DEVELOPMENT_BUILD

		/** =====> 객체 <===== */
		[SerializeField] private List<GameObject> m_oRewardAdsUIsList = new List<GameObject>();
		#endregion			// 변수

		#region 프로퍼티
		public override bool IsIgnoreOverlayScene => false;
		#endregion			// 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();
			
			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
#if DEBUG || DEVELOPMENT_BUILD
				// 플레이 레벨 정보가 없을 경우
				if(CGameInfoStorage.Inst.PlayLevelInfo == null) {
#if UNITY_STANDALONE && EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
					// 레벨 정보가 없을 경우
					if(!CLevelInfoTable.Inst.LevelInfoDictContainer.ExIsValid()) {
						var oLevelInfo = Factory.MakeLevelInfo(KCDefine.B_VAL_0_INT);

						Func.SetupEditorLevelInfo(oLevelInfo, new CSubEditorLevelCreateInfo() {
							m_nNumLevels = KCDefine.B_VAL_0_INT, m_stMinNumCells = SampleEngineName.KDefine.E_MIN_NUM_CELLS, m_stMaxNumCells = SampleEngineName.KDefine.E_MIN_NUM_CELLS
						});
						
						CLevelInfoTable.Inst.AddLevelInfo(oLevelInfo);
						CLevelInfoTable.Inst.SaveLevelInfos();
					}
#endif			// #if UNITY_STANDALONE && EDITOR_SCENE_TEMPLATES_MODULE_ENABLE

					CGameInfoStorage.Inst.SetupPlayLevelInfo(KCDefine.B_VAL_0_INT, EPlayMode.NORM);
				}
#endif			// #if DEBUG || DEVELOPMENT_BUILD

				this.SetupAwake();
			}
		}
		
		/** 초기화 */
		public override void Start() {
			base.Start();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				this.SetupStart();
				this.UpdateUIsState();
			}
		}

		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
#if ENGINE_TEMPLATES_MODULE_ENABLE
				m_oEngineDict[EKey.ENGINE].OnUpdate(a_fDeltaTime);
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
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
				CFunc.ShowLogWarning($"CSubGameSceneManager.OnDestroy Exception: {oException.Message}");
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

				Func.ShowResumePopup(this.PopupUIs, (a_oSender) => {
					(a_oSender as CResumePopup).Init(new CResumePopup.STParams() {
						m_oCallbackDict = new Dictionary<CResumePopup.ECallback, System.Action<CResumePopup>>() {
							[CResumePopup.ECallback.RESUME] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.RESUME),
							[CResumePopup.ECallback.LEAVE] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.LEAVE)
						}
					});
				});
			}
		}

		/** 내비게이션 스택 이벤트를 수신했을 경우 */
		public override void OnReceiveNavStackEvent(ENavStackEvent a_eEvent) {
			base.OnReceiveNavStackEvent(a_eEvent);

			// 백 키 눌림 이벤트 일 경우
			if(a_eEvent == ENavStackEvent.BACK_KEY_DOWN) {
				// 이전 씬이 레벨 에디터 씬 일 경우
				if(CSceneLoader.Inst.PrevActiveSceneName.Equals(KCDefine.B_SCENE_N_LEVEL_EDITOR)) {
					Func.ShowLeavePopup(this.OnReceiveLeavePopupResult);
				} else {
					this.OnTouchPauseBtn();
				}
			}
		}

		/** 씬을 설정한다 */
		private void SetupAwake() {
			this.SetupEngine();
			this.SetupRewardAdsUIs();

			m_oLevelInfoDict[EKey.LEVEL_INFO] = CGameInfoStorage.Inst.PlayLevelInfo;
			m_oClearInfoDict[EKey.CLEAR_INFO] = CGameInfoStorage.Inst.TryGetClearInfo(CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nID, out CClearInfo oClearInfo, CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nStageID, CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nChapterID) ? oClearInfo : null;

			// 버튼을 설정한다
			this.UIsBase.ExFindComponent<Button>(KCDefine.U_OBJ_N_PAUSE_BTN)?.ExAddListener(this.OnTouchPauseBtn, true, false);
			this.UIsBase.ExFindComponent<Button>(KCDefine.U_OBJ_N_SETTINGS_BTN)?.ExAddListener(this.OnTouchSettingsBtn, true, false);

			// 터치 전달자를 설정한다
			m_oTouchDispatcherDict[EKey.BG_TOUCH_DISPATCHER] = this.BGTouchResponder?.GetComponentInChildren<CTouchDispatcher>();
			m_oTouchDispatcherDict[EKey.BG_TOUCH_DISPATCHER]?.ExSetBeginCallback(this.OnTouchBegin, false);
			m_oTouchDispatcherDict[EKey.BG_TOUCH_DISPATCHER]?.ExSetMoveCallback(this.OnTouchMove, false);
			m_oTouchDispatcherDict[EKey.BG_TOUCH_DISPATCHER]?.ExSetEndCallback(this.OnTouchEnd, false);

#if ENGINE_TEMPLATES_MODULE_ENABLE
			// 비율을 설정한다 {
			bool bIsValidA = !float.IsNaN(m_oEngineDict[EKey.ENGINE].GridInfo.m_stScale.x) && !float.IsInfinity(m_oEngineDict[EKey.ENGINE].GridInfo.m_stScale.x);
			bool bIsValidB = !float.IsNaN(m_oEngineDict[EKey.ENGINE].GridInfo.m_stScale.y) && !float.IsInfinity(m_oEngineDict[EKey.ENGINE].GridInfo.m_stScale.y);
			bool bIsValidC = !float.IsNaN(m_oEngineDict[EKey.ENGINE].GridInfo.m_stScale.z) && !float.IsInfinity(m_oEngineDict[EKey.ENGINE].GridInfo.m_stScale.z);

			this.BlockObjs.transform.localScale = (bIsValidA && bIsValidB && bIsValidC) ? m_oEngineDict[EKey.ENGINE].GridInfo.m_stScale : Vector3.one;
			// 비율을 설정한다 }
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE

#if DEBUG || DEVELOPMENT_BUILD
			this.SetupTestUIs();
#endif			// #if DEBUG || DEVELOPMENT_BUILD
		}

		/** 씬을 설정한다 */
		private void SetupStart() {
			this.ApplySelItems();
			CGameInfoStorage.Inst.ResetSelItems();
		}

		/** 엔진을 설정한다 */
		private void SetupEngine() {
#if ENGINE_TEMPLATES_MODULE_ENABLE
			bool bIsValid = CGameInfoStorage.Inst.TryGetClearInfo(CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nID, out CClearInfo oClearInfo, CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nStageID, CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nChapterID);
			m_oEngineDict[EKey.ENGINE] = CFactory.CreateObj<SampleEngineName.CEngine>(KDefine.GS_OBJ_N_ENGINE, this.gameObject);

			m_oEngineDict[EKey.ENGINE].Init(new SampleEngineName.CEngine.STParams() {
				m_oFXObjs = this.FXObjs,
				m_oBlockObjs = this.BlockObjs,

				m_oCallbackDict = new Dictionary<SampleEngineName.CEngine.ECallback, System.Action<SampleEngineName.CEngine>>() {
					[SampleEngineName.CEngine.ECallback.CLEAR] = this.OnClearLevel,
					[SampleEngineName.CEngine.ECallback.CLEAR_FAIL] = this.OnClearFailLevel
				},

#if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
				m_oLevelInfo = CGameInfoStorage.Inst.PlayLevelInfo,
				m_oClearInfo = bIsValid ? oClearInfo : null
#endif			// #if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
			});
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
		}

		/** 보상 광고 UI 를 설정한다 */
		private void SetupRewardAdsUIs() {
			for(int i = 0; i < m_oRewardAdsUIsList.Count; ++i) {
				var eRewardAdsUIs = (ERewardAdsUIs)i;
				
				var oRewardAdsBtn = m_oRewardAdsUIsList[i]?.GetComponentInChildren<Button>();
				oRewardAdsBtn?.ExAddListener(() => this.OnTouchRewardAdsBtn(eRewardAdsUIs), true, false);
			}
		}

		/** UI 상태를 갱신한다 */
		private void UpdateUIsState() {
			this.UpdateRewardAdsUIsState();

#if DEBUG || DEVELOPMENT_BUILD
			this.UpdateTestUIsState();
#endif			// #if DEBUG || DEVELOPMENT_BUILD
		}

		/** 보상 광고 UI 상태를 갱신한다 */
		private void UpdateRewardAdsUIsState() {
			for(int i = 0; i < m_oRewardAdsUIsList.Count; ++i) {
				m_oRewardAdsUIsList[i]?.SetActive(m_oLevelInfoDict[EKey.LEVEL_INFO].m_stIDInfo.m_nID + KCDefine.B_VAL_1_INT >= KDefine.GS_MIN_LEVEL_ENABLE_REWARD_ADS_WATCH);
			}
		}

		/** 그만두기 팝업 결과를 수신했을 경우 */
		private void OnReceiveLeavePopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_LEVEL_EDITOR);
			}
		}

		/** 팝업 결과를 수신했을 경우 */
		private void OnReceivePopupResult(CPopup a_oSender, EPopupResult a_eResult) {
			// 팝업이 존재 할 경우
			if(a_oSender != null) {
				a_oSender.IsIgnoreAni = a_eResult != EPopupResult.CONTINUE;
				a_oSender.Close();
			}

			switch(a_eResult) {
				case EPopupResult.NEXT: this.LoadNextLevel(a_oSender); break;
				case EPopupResult.RETRY: this.RetryCurLevel(a_oSender); break;
				case EPopupResult.RESUME: this.ResumeCurLevel(a_oSender); break;
				case EPopupResult.CONTINUE: this.ContinueCurLevel(a_oSender); break;
				case EPopupResult.LEAVE: m_bIsLeave = true; this.LoadNextLevel(a_oSender); break;
			}
		}

		/** 정지 버튼을 눌렀을 경우 */
		private void OnTouchPauseBtn() {
			Func.ShowPausePopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CPausePopup).Init(new CPausePopup.STParams() {
					m_oCallbackDict = new Dictionary<CPausePopup.ECallback, System.Action<CPausePopup>>() {
						[CPausePopup.ECallback.LEAVE] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.LEAVE)
					}
				});
			});
		}

		/** 설정 버튼을 눌렀을 경웅 */
		private void OnTouchSettingsBtn() {
			Func.ShowSettingsPopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CSettingsPopup).Init();
			});
		}

		/** 광고 버튼을 눌렀을 경우 */
		private void OnTouchRewardAdsBtn(ERewardAdsUIs a_eRewardAdsUIs) {
			m_eSelRewardAdsUIs = a_eRewardAdsUIs;

#if ADS_MODULE_ENABLE
			Func.ShowRewardAds(this.OnCloseRewardAds);
#endif			// #if ADS_MODULE_ENABLE
		}

		/** 터치를 시작했을 경우 */
		private void OnTouchBegin(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 배경 터치 전달자 일 경우
			if(m_oTouchDispatcherDict[EKey.BG_TOUCH_DISPATCHER] == a_oSender) {
#if ENGINE_TEMPLATES_MODULE_ENABLE
				m_oEngineDict[EKey.ENGINE].OnTouchBegin(a_oSender, a_oEventData);
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
			}
		}

		/** 터치를 움직였을 경우 */
		private void OnTouchMove(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 배경 터치 전달자 일 경우
			if(m_oTouchDispatcherDict[EKey.BG_TOUCH_DISPATCHER] == a_oSender) {
#if ENGINE_TEMPLATES_MODULE_ENABLE
				m_oEngineDict[EKey.ENGINE].OnTouchMove(a_oSender, a_oEventData);
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
			}
		}

		/** 터치를 종료했을 경우 */
		private void OnTouchEnd(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 배경 터치 전달자 일 경우
			if(m_oTouchDispatcherDict[EKey.BG_TOUCH_DISPATCHER] == a_oSender) {
#if ENGINE_TEMPLATES_MODULE_ENABLE
				m_oEngineDict[EKey.ENGINE].OnTouchEnd(a_oSender, a_oEventData);
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
			}
		}

		/** 선택 아이템을 적용한다 */
		private void ApplySelItems() {
			for(int i = 0; i < CGameInfoStorage.Inst.FreeSelItemKindsList.Count; ++i) {
				this.ApplySelItem(CGameInfoStorage.Inst.FreeSelItemKindsList[i]);
				CGameInfoStorage.Inst.RemoveSelItem(CGameInfoStorage.Inst.FreeSelItemKindsList[i]);
			}

			for(int i = 0; i < CGameInfoStorage.Inst.SelItemKindsList.Count; ++i) {
				this.ApplySelItem(CGameInfoStorage.Inst.SelItemKindsList[i]);
				CUserInfoStorage.Inst.AddNumItems(CGameInfoStorage.Inst.SelItemKindsList[i], -KCDefine.B_VAL_1_INT);
			}
		}

		/** 선택 아이템을 적용한다 */
		private void ApplySelItem(EItemKinds a_eItemKinds) {
			// Do Something
		}

		/** 다음 레벨을 로드한다 */
		private void LoadNextLevel(CPopup a_oPopup) {
			switch(CGameInfoStorage.Inst.PlayMode) {
				case EPlayMode.NORM: {
					int nNextID = m_oLevelInfoDict[EKey.LEVEL_INFO].m_stIDInfo.m_nID + KCDefine.B_VAL_1_INT;
					int nNumClearInfos = CGameInfoStorage.Inst.GetNumClearInfos(m_oLevelInfoDict[EKey.LEVEL_INFO].m_stIDInfo.m_nStageID, m_oLevelInfoDict[EKey.LEVEL_INFO].m_stIDInfo.m_nChapterID);

#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
					bool bIsValid = CLevelInfoTable.Inst.TryGetLevelInfo(nNextID, out CLevelInfo oNextLevelInfo, m_oLevelInfoDict[EKey.LEVEL_INFO].m_stIDInfo.m_nStageID, m_oLevelInfoDict[EKey.LEVEL_INFO].m_stIDInfo.m_nChapterID) && nNextID <= nNumClearInfos;
#else
					bool bIsValid = CEpisodeInfoTable.Inst.TryGetLevelInfo(nNextID, out STLevelInfo stNextLevelInfo, m_oLevelInfoDict[EKey.LEVEL_INFO].m_stIDInfo.m_nStageID, m_oLevelInfoDict[EKey.LEVEL_INFO].m_stIDInfo.m_nChapterID) && nNextID <= nNumClearInfos;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)

					// 다음 레벨이 존재 할 경우
					if(bIsValid && !m_bIsLeave) {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
						CGameInfoStorage.Inst.SetupPlayLevelInfo(oNextLevelInfo.m_stIDInfo.m_nID, CGameInfoStorage.Inst.PlayMode, oNextLevelInfo.m_stIDInfo.m_nStageID, oNextLevelInfo.m_stIDInfo.m_nChapterID);
#else
						CGameInfoStorage.Inst.SetupPlayLevelInfo(stNextLevelInfo.m_nID, CGameInfoStorage.Inst.PlayMode, stNextLevelInfo.m_nStageID, stNextLevelInfo.m_nChapterID);
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)

#if ADS_MODULE_ENABLE
						Func.ShowFullscreenAds((a_oSender, a_bIsSuccess) => CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME));
#else
						CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME);
#endif			// #if ADS_MODULE_ENABLE
					} else {
#if ADS_MODULE_ENABLE
						Func.ShowFullscreenAds((a_oSender, a_bIsSuccess) => CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MAIN));
#else
						CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MAIN);
#endif			// #if ADS_MODULE_ENABLE
					}
				} break;
				case EPlayMode.TUTORIAL: {
					// Do Something
				} break;
				case EPlayMode.TEST: {
					CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_LEVEL_EDITOR);
				} break;
			}
		}

		/** 현재 레벨을 재시도한다 */
		private void RetryCurLevel(CPopup a_oPopup) {
#if ADS_MODULE_ENABLE
			Func.ShowFullscreenAds((a_oSender, a_bIsSuccess) => CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME));
#else
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME);
#endif			// #if ADS_MODULE_ENABLE
		}

		/** 현재 레벨을 제개한다 */
		private void ResumeCurLevel(CPopup a_oPopup) {
			a_oPopup?.Close();
		}

		/** 현재 레벨을 이어한다 */
		private void ContinueCurLevel(CPopup a_oPopup) {
			m_nContinueTimes += KCDefine.B_VAL_1_INT;
		}

		/** 이어하기 팝업을 출력한다 */
		private void ShowContinuePopup() {
#if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
			Func.ShowContinuePopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CContinuePopup).Init(new CContinuePopup.STParams() {
					m_nContinueTimes = this.m_nContinueTimes,
					m_oLevelInfo = m_oLevelInfoDict[EKey.LEVEL_INFO],

					m_oCallbackDict = new Dictionary<CContinuePopup.ECallback, System.Action<CContinuePopup>>() {
						[CContinuePopup.ECallback.RETRY] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.RETRY),
						[CContinuePopup.ECallback.CONTINUE] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.CONTINUE),
						[CContinuePopup.ECallback.LEAVE] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.LEAVE)
					}
				});
			});
#endif			// #if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
		}

		/** 결과 팝업을 출력한다 */
		private void ShowResultPopup(bool a_bIsClear) {
#if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
			Func.ShowResultPopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CResultPopup).Init(new CResultPopup.STParams() {
					m_stRecordInfo = new STRecordInfo {
						m_bIsSuccess = a_bIsClear,

#if ENGINE_TEMPLATES_MODULE_ENABLE
						m_nIntRecord = m_oEngineDict[EKey.ENGINE].IntRecord,
						m_dblRealRecord = m_oEngineDict[EKey.ENGINE].RealRecord
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
					},
					
					m_oLevelInfo = m_oLevelInfoDict[EKey.LEVEL_INFO],
					m_oClearInfo = m_oClearInfoDict[EKey.CLEAR_INFO],

					m_oCallbackDict = new Dictionary<CResultPopup.ECallback, System.Action<CResultPopup>>() {
						[CResultPopup.ECallback.NEXT] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.NEXT),
						[CResultPopup.ECallback.RETRY] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.RETRY),
						[CResultPopup.ECallback.LEAVE] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.LEAVE)
					}
				});
			});
#endif			// #if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
		}
		#endregion			// 함수

		#region 조건부 함수
#if UNITY_EDITOR
		/** 기즈모를 그린다 */
		public override void OnDrawGizmos() {
			base.OnDrawGizmos();

			// 앱 실행 중이 아닐 경우
			if(!Application.isPlaying && CSceneManager.IsExistsMainCamera) {
				// Do Something
			}
		}
#endif			// #if UNITY_EDITOR

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

#if ADS_MODULE_ENABLE
		/** 보상 광고가 닫혔을 경우 */
		private void OnCloseRewardAds(CAdsManager a_oSender, STAdsRewardInfo a_stAdsRewardInfo, bool a_bIsSuccess) {
			// 광고를 시청했을 경우
			if(a_bIsSuccess) {
				// Do Something
			}
		}
#endif			// #if ADS_MODULE_ENABLE

#if ENGINE_TEMPLATES_MODULE_ENABLE
		/** 레벨을 클리어했을 경우 */
		private void OnClearLevel(SampleEngineName.CEngine a_oSender) {
			// 클리어 정보가 없을 경우
			if(!CGameInfoStorage.Inst.IsClearLevel(m_oLevelInfoDict[EKey.LEVEL_INFO].m_stIDInfo.m_nID, m_oLevelInfoDict[EKey.LEVEL_INFO].m_stIDInfo.m_nStageID, m_oLevelInfoDict[EKey.LEVEL_INFO].m_stIDInfo.m_nChapterID)) {
				CGameInfoStorage.Inst.AddClearInfo(Factory.MakeClearInfo(m_oLevelInfoDict[EKey.LEVEL_INFO].m_stIDInfo.m_nID, m_oLevelInfoDict[EKey.LEVEL_INFO].m_stIDInfo.m_nStageID, m_oLevelInfoDict[EKey.LEVEL_INFO].m_stIDInfo.m_nChapterID));
			}
			
			var oClearInfo = CGameInfoStorage.Inst.GetClearInfo(m_oLevelInfoDict[EKey.LEVEL_INFO].m_stIDInfo.m_nID, m_oLevelInfoDict[EKey.LEVEL_INFO].m_stIDInfo.m_nStageID, m_oLevelInfoDict[EKey.LEVEL_INFO].m_stIDInfo.m_nChapterID);
			CGameInfoStorage.Inst.SaveGameInfo();

			this.ShowResultPopup(true);
		}

		/** 레벨 클리어에 실패했을 경우 */
		private void OnClearFailLevel(SampleEngineName.CEngine a_oSender) {
			this.ShowResultPopup(false);
		}
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
		#endregion			// 조건부 함수
	}
}
#endif			// #if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
