using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
namespace GameScene {
	/** 서브 게임 씬 관리자 */
	public partial class CSubGameSceneManager : CGameSceneManager {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			IS_LEAVE,
			CONTINUE_TIMES,
			SEL_REWARD_ADS_UIS,
			PLAY_LEVEL_INFO,
			PLAY_LEVEL_CLEAR_INFO,
			BG_TOUCH_DISPATCHER,
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

		#region 변수
		private Dictionary<EKey, bool> m_oBoolDict = new Dictionary<EKey, bool>() {
			[EKey.IS_LEAVE] = false
		};

		private Dictionary<EKey, int> m_oIntDict = new Dictionary<EKey, int>() {
			[EKey.CONTINUE_TIMES] = 0
		};

		private Dictionary<EKey, ERewardAdsUIs> m_oRewardAdsUIsDict = new Dictionary<EKey, ERewardAdsUIs>() {
			[EKey.SEL_REWARD_ADS_UIS] = ERewardAdsUIs.NONE
		};
		
		private Dictionary<EKey, CLevelInfo> m_oLevelInfoDict = new Dictionary<EKey, CLevelInfo>() {
			[EKey.PLAY_LEVEL_INFO] = null
		};

		private Dictionary<EKey, CClearInfo> m_oClearInfoDict = new Dictionary<EKey, CClearInfo>() {
			[EKey.PLAY_LEVEL_CLEAR_INFO] = null
		};

		private Dictionary<EKey, CTouchDispatcher> m_oTouchDispatcherDict = new Dictionary<EKey, CTouchDispatcher>();

#if ENGINE_TEMPLATES_MODULE_ENABLE
		private SampleEngineName.CEngine m_oEngine = null;
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE

		/** =====> 객체 <===== */
		[SerializeField] private List<GameObject> m_oRewardAdsUIsList = new List<GameObject>();
		#endregion			// 변수
		
		#region 함수
		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
#if ENGINE_TEMPLATES_MODULE_ENABLE
				m_oEngine.OnUpdate(a_fDeltaTime);
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
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
				case EPopupResult.RETRY: this.RetryPlayLevel(a_oSender); break;
				case EPopupResult.RESUME: this.ResumePlayLevel(a_oSender); break;
				case EPopupResult.CONTINUE: this.ContinuePlayLevel(a_oSender); break;
				case EPopupResult.LEAVE: m_oBoolDict[EKey.IS_LEAVE] = true; this.LoadNextLevel(a_oSender); break;
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
		private void OnTouchAdsBtn(ERewardAdsUIs a_eRewardAdsUIs) {
			m_oRewardAdsUIsDict[EKey.SEL_REWARD_ADS_UIS] = a_eRewardAdsUIs;

#if ADS_MODULE_ENABLE
			Func.ShowRewardAds(this.OnCloseRewardAds);
#endif			// #if ADS_MODULE_ENABLE
		}

		/** 터치를 시작했을 경우 */
		private void OnTouchBegin(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 배경 터치 전달자 일 경우
			if(m_oTouchDispatcherDict[EKey.BG_TOUCH_DISPATCHER] == a_oSender) {
#if ENGINE_TEMPLATES_MODULE_ENABLE
				m_oEngine.OnTouchBegin(a_oSender, a_oEventData);
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
			}
		}

		/** 터치를 이동했을 경우 */
		private void OnTouchMove(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 배경 터치 전달자 일 경우
			if(m_oTouchDispatcherDict[EKey.BG_TOUCH_DISPATCHER] == a_oSender) {
#if ENGINE_TEMPLATES_MODULE_ENABLE
				m_oEngine.OnTouchMove(a_oSender, a_oEventData);
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
			}
		}

		/** 터치를 종료했을 경우 */
		private void OnTouchEnd(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 배경 터치 전달자 일 경우
			if(m_oTouchDispatcherDict[EKey.BG_TOUCH_DISPATCHER] == a_oSender) {
#if ENGINE_TEMPLATES_MODULE_ENABLE
				m_oEngine.OnTouchEnd(a_oSender, a_oEventData);
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

				Func.Pay(Factory.MakeTargetInfo(ETargetKinds.ITEM_NUMS, (int)CGameInfoStorage.Inst.SelItemKindsList[i], new STValInfo() {
					m_nVal = KCDefine.B_VAL_1_INT, m_eValType = EValType.INT
				}));
			}
		}

		/** 다음 레벨을 로드한다 */
		private void LoadNextLevel(CPopup a_oPopup) {
			bool bIsValid = CEpisodeInfoTable.Inst.TryGetLevelEpisodeInfo(m_oLevelInfoDict[EKey.PLAY_LEVEL_INFO].m_stIDInfo.m_nID01, out STEpisodeInfo stNextLevelEpisodeInfo, m_oLevelInfoDict[EKey.PLAY_LEVEL_INFO].m_stIDInfo.m_nID02, m_oLevelInfoDict[EKey.PLAY_LEVEL_INFO].m_stIDInfo.m_nID03);
			bIsValid = bIsValid && stNextLevelEpisodeInfo.m_stIDInfo.m_nID01 <= CGameInfoStorage.Inst.GetNumLevelClearInfos(m_oLevelInfoDict[EKey.PLAY_LEVEL_INFO].m_stIDInfo.m_nID02, m_oLevelInfoDict[EKey.PLAY_LEVEL_INFO].m_stIDInfo.m_nID03);

			switch(CGameInfoStorage.Inst.PlayMode) {
				case EPlayMode.NORM: {
					// 다음 레벨이 존재 할 경우
					if(bIsValid && !m_oBoolDict[EKey.IS_LEAVE]) {
						CGameInfoStorage.Inst.SetupPlayLevelInfo(stNextLevelEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayMode, stNextLevelEpisodeInfo.m_stIDInfo.m_nID02, stNextLevelEpisodeInfo.m_stIDInfo.m_nID03);
						
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

		/** 플레이 레벨을 재시도한다 */
		private void RetryPlayLevel(CPopup a_oPopup) {
#if ADS_MODULE_ENABLE
			Func.ShowFullscreenAds((a_oSender, a_bIsSuccess) => CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME));
#else
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME);
#endif			// #if ADS_MODULE_ENABLE
		}

		/** 플레이 레벨을 제개한다 */
		private void ResumePlayLevel(CPopup a_oPopup) {
			a_oPopup?.Close();
		}

		/** 플레이 레벨을 이어한다 */
		private void ContinuePlayLevel(CPopup a_oPopup) {
			m_oIntDict[EKey.CONTINUE_TIMES] += KCDefine.B_VAL_1_INT;
		}

		/** 이어하기 팝업을 출력한다 */
		private void ShowContinuePopup() {
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
			Func.ShowContinuePopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CContinuePopup).Init(new CContinuePopup.STParams() {
					m_nContinueTimes = m_oIntDict[EKey.CONTINUE_TIMES],
					m_oLevelInfo = m_oLevelInfoDict[EKey.PLAY_LEVEL_INFO],

					m_oCallbackDict = new Dictionary<CContinuePopup.ECallback, System.Action<CContinuePopup>>() {
						[CContinuePopup.ECallback.RETRY] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.RETRY),
						[CContinuePopup.ECallback.CONTINUE] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.CONTINUE),
						[CContinuePopup.ECallback.LEAVE] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.LEAVE)
					}
				});
			});
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
		}

		/** 결과 팝업을 출력한다 */
		private void ShowResultPopup(bool a_bIsClear) {
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
			Func.ShowResultPopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CResultPopup).Init(new CResultPopup.STParams() {
					m_stRecordInfo = new STRecordInfo {
						m_bIsSuccess = a_bIsClear,

#if ENGINE_TEMPLATES_MODULE_ENABLE
						m_nIntRecord = m_oEngine.IntRecord,
						m_dblRealRecord = m_oEngine.RealRecord
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
					},
					
					m_oLevelInfo = m_oLevelInfoDict[EKey.PLAY_LEVEL_INFO],
					m_oClearInfo = m_oClearInfoDict[EKey.PLAY_LEVEL_CLEAR_INFO],

					m_oCallbackDict = new Dictionary<CResultPopup.ECallback, System.Action<CResultPopup>>() {
						[CResultPopup.ECallback.NEXT] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.NEXT),
						[CResultPopup.ECallback.RETRY] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.RETRY),
						[CResultPopup.ECallback.LEAVE] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.LEAVE)
					}
				});
			});
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
		}
		#endregion			// 함수

		#region 조건부 함수
#if ENGINE_TEMPLATES_MODULE_ENABLE
		/** 레벨을 클리어했을 경우 */
		private void OnClearLevel(SampleEngineName.CEngine a_oSender) {			
			var oLevelClearInfo = Access.GetLevelClearInfo(m_oLevelInfoDict[EKey.PLAY_LEVEL_INFO].m_stIDInfo.m_nID01, m_oLevelInfoDict[EKey.PLAY_LEVEL_INFO].m_stIDInfo.m_nID02, m_oLevelInfoDict[EKey.PLAY_LEVEL_INFO].m_stIDInfo.m_nID03, true);
			oLevelClearInfo.IntRecord = a_oSender.IntRecord;
			oLevelClearInfo.IntBestRecord = System.Math.Max(a_oSender.IntRecord, oLevelClearInfo.IntBestRecord);
			
			oLevelClearInfo.RealRecord = a_oSender.RealRecord;
			oLevelClearInfo.RealBestRecord = a_oSender.RealRecord.ExIsGreate(oLevelClearInfo.RealBestRecord) ? a_oSender.RealRecord : oLevelClearInfo.RealBestRecord;

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
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
