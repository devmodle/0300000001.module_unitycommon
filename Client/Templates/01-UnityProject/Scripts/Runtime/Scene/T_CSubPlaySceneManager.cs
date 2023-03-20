#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using UnityEngine.EventSystems;

namespace PlayScene {
	/** 서브 플레이 씬 관리자 */
	public partial class CSubPlaySceneManager : CPlaySceneManager {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			IS_UPDATE_UIS_STATE,
			SEL_REWARD_ADS_UIS,
			CONTINUE_TIMES,

			BG_SPRITE,
			UP_BG_SPRITE,
			DOWN_BG_SPRITE,
			LEFT_BG_SPRITE,
			RIGHT_BG_SPRITE,

			BG_SPRITE_ROOT,
			[HideInInspector] MAX_VAL
		}

		/** 팝업 콜백 */
		private enum EPopupCallback {
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
			[EKey.IS_UPDATE_UIS_STATE] = false
		};

		private Dictionary<EKey, int> m_oIntDict = new Dictionary<EKey, int>() {
			[EKey.CONTINUE_TIMES] = KCDefine.B_VAL_0_INT
		};

		private Dictionary<EKey, ERewardAdsUIs> m_oRewardAdsUIsDict = new Dictionary<EKey, ERewardAdsUIs>() {
			[EKey.SEL_REWARD_ADS_UIS] = ERewardAdsUIs.NONE
		};

		private NSEngine.CEngine m_oEngine = null;
		private Dictionary<EKey, SpriteRenderer> m_oSpriteDict = new Dictionary<EKey, SpriteRenderer>();

		/** =====> 객체 <===== */
		private Dictionary<EKey, GameObject> m_oObjDict = new Dictionary<EKey, GameObject>();
		[SerializeField] private List<GameObject> m_oRewardAdsUIsList = new List<GameObject>();
		#endregion // 변수

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
#if DEBUG || DEVELOPMENT_BUILD
				// 플레이 레벨 정보가 없을 경우
				if(CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01 <= KCDefine.B_IDX_INVALID) {
					Func.SetupPlayEpisodeInfo(CGameInfoStorage.Inst.PlayCharacterID, KCDefine.B_VAL_0_INT, EPlayMode.NORM);
				}
#endif // #if DEBUG || DEVELOPMENT_BUILD

				this.SetupEngine();
				this.SetupRewardAdsUIs();

				// 객체를 설정한다
				CFunc.SetupObjs(new List<(EKey, string, GameObject, GameObject)>() {
					(EKey.BG_SPRITE_ROOT, $"{EKey.BG_SPRITE_ROOT}", this.Objs, null)
				}, m_oObjDict);

				// 버튼을 설정한다
				CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
					(KCDefine.U_OBJ_N_PAUSE_BTN, this.UIsBase, this.OnTouchPauseBtn),
					(KCDefine.U_OBJ_N_SETTINGS_BTN, this.UIsBase, this.OnTouchSettingsBtn)
				});

				// 비율을 설정한다
				var stSize = new Vector3(Mathf.Max(this.ScreenWidth, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.x), Mathf.Max(this.ScreenHeight, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.y), KCDefine.B_VAL_0_REAL);
				this.SetRootObjsScale(m_oEngine.SelGridInfo.m_stScale.ExIsValid() ? m_oEngine.SelGridInfo.m_stScale : Vector3.one);

				// 스프라이트를 설정한다 {
				CFunc.SetupComponents(new List<(EKey, string, GameObject, GameObject)>() {
					(EKey.BG_SPRITE, $"{EKey.BG_SPRITE}", m_oObjDict[EKey.BG_SPRITE_ROOT], CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_SPRITE)),
					(EKey.UP_BG_SPRITE, $"{EKey.UP_BG_SPRITE}", m_oObjDict[EKey.BG_SPRITE_ROOT], CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_SPRITE)),
					(EKey.DOWN_BG_SPRITE, $"{EKey.DOWN_BG_SPRITE}", m_oObjDict[EKey.BG_SPRITE_ROOT], CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_SPRITE)),
					(EKey.LEFT_BG_SPRITE, $"{EKey.LEFT_BG_SPRITE}", m_oObjDict[EKey.BG_SPRITE_ROOT], CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_SPRITE)),
					(EKey.RIGHT_BG_SPRITE, $"{EKey.RIGHT_BG_SPRITE}", m_oObjDict[EKey.BG_SPRITE_ROOT], CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_SPRITE))
				}, m_oSpriteDict);

				var oSpriteInfoDict = new Dictionary<EKey, (Sprite, STSortingOrderInfo)>() {
					[EKey.BG_SPRITE] = (Access.GetBGSprite(KDefine.PS_TEX_P_FMT_BG, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03), KDefine.PS_SORTING_OI_BG),
					[EKey.UP_BG_SPRITE] = (Access.GetBGSprite(KDefine.PS_TEX_P_FMT_UP_BG, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03), KDefine.PS_SORTING_OI_UP_BG),
					[EKey.DOWN_BG_SPRITE] = (Access.GetBGSprite(KDefine.PS_TEX_P_FMT_DOWN_BG, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03), KDefine.PS_SORTING_OI_DOWN_BG),
					[EKey.LEFT_BG_SPRITE] = (Access.GetBGSprite(KDefine.PS_TEX_P_FMT_LEFT_BG, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03), KDefine.PS_SORTING_OI_LEFT_BG),
					[EKey.RIGHT_BG_SPRITE] = (Access.GetBGSprite(KDefine.PS_TEX_P_FMT_RIGHT_BG, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03), KDefine.PS_SORTING_OI_RIGHT_BG)
				};

				foreach(var stKeyVal in m_oSpriteDict) {
					stKeyVal.Value.sprite = oSpriteInfoDict[stKeyVal.Key].Item1;
					stKeyVal.Value.ExSetSortingOrder(oSpriteInfoDict[stKeyVal.Key].Item2);
				}

				bool bIsValid01 = m_oSpriteDict[EKey.BG_SPRITE] != null && m_oSpriteDict[EKey.BG_SPRITE].sprite != null;
				bool bIsValid02 = m_oSpriteDict[EKey.UP_BG_SPRITE] != null && m_oSpriteDict[EKey.UP_BG_SPRITE].sprite != null;
				bool bIsValid03 = m_oSpriteDict[EKey.DOWN_BG_SPRITE] != null && m_oSpriteDict[EKey.DOWN_BG_SPRITE].sprite != null;
				bool bIsValid04 = m_oSpriteDict[EKey.LEFT_BG_SPRITE] != null && m_oSpriteDict[EKey.LEFT_BG_SPRITE].sprite != null;
				bool bIsValid05 = m_oSpriteDict[EKey.RIGHT_BG_SPRITE] != null && m_oSpriteDict[EKey.RIGHT_BG_SPRITE].sprite != null;

				var oTransInfoDict = new Dictionary<EKey, (Vector3, Vector3)>() {
					[EKey.BG_SPRITE] = bIsValid01 ? (stSize, Vector3.zero) : (Vector3.one, Vector3.zero),
					[EKey.UP_BG_SPRITE] = bIsValid02 ? (new Vector3(stSize.x + (this.ScreenWidth * KCDefine.B_VAL_2_REAL), m_oSpriteDict[EKey.UP_BG_SPRITE].sprite.rect.height, KCDefine.B_VAL_0_REAL), new Vector3(KCDefine.B_VAL_0_REAL, (stSize.y / KCDefine.B_VAL_2_REAL) + (m_oSpriteDict[EKey.UP_BG_SPRITE].sprite.rect.height / KCDefine.B_VAL_2_REAL), KCDefine.B_VAL_0_REAL)) : (Vector3.one, Vector3.zero),
					[EKey.DOWN_BG_SPRITE] = bIsValid03 ? (new Vector3(stSize.x + (this.ScreenWidth * KCDefine.B_VAL_2_REAL), m_oSpriteDict[EKey.DOWN_BG_SPRITE].sprite.rect.height, KCDefine.B_VAL_0_REAL), new Vector3(KCDefine.B_VAL_0_REAL, -((stSize.y / KCDefine.B_VAL_2_REAL) + (m_oSpriteDict[EKey.DOWN_BG_SPRITE].sprite.rect.height / KCDefine.B_VAL_2_REAL) - NSEngine.KDefine.E_OFFSET_BOTTOM.y), KCDefine.B_VAL_0_REAL)) : (Vector3.one, Vector3.zero),
					[EKey.LEFT_BG_SPRITE] = bIsValid04 ? (new Vector3(m_oSpriteDict[EKey.LEFT_BG_SPRITE].sprite.rect.width, Mathf.Max(this.ScreenHeight, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.y), KCDefine.B_VAL_0_REAL), new Vector3(-((stSize.y / KCDefine.B_VAL_2_REAL) + (m_oSpriteDict[EKey.LEFT_BG_SPRITE].sprite.rect.height / KCDefine.B_VAL_2_REAL)), KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL)) : (Vector3.one, Vector3.zero),
					[EKey.RIGHT_BG_SPRITE] = bIsValid05 ? (new Vector3(m_oSpriteDict[EKey.RIGHT_BG_SPRITE].sprite.rect.width, Mathf.Max(this.ScreenHeight, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.y), KCDefine.B_VAL_0_REAL), new Vector3((stSize.y / KCDefine.B_VAL_2_REAL) + (m_oSpriteDict[EKey.RIGHT_BG_SPRITE].sprite.rect.height / KCDefine.B_VAL_2_REAL), KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL)) : (Vector3.one, Vector3.zero)
				};

				foreach(var stKeyVal in m_oSpriteDict) {
					stKeyVal.Value.size = oTransInfoDict[stKeyVal.Key].Item1;
					stKeyVal.Value.drawMode = SpriteDrawMode.Tiled;
					stKeyVal.Value.tileMode = SpriteTileMode.Continuous;

					stKeyVal.Value.transform.localScale = Vector3.one;
					stKeyVal.Value.transform.localPosition = oTransInfoDict[stKeyVal.Key].Item2;
				}
				// 스프라이트를 설정한다 }

				this.SubAwake();
			}
		}

		/** 초기화 */
		public override void Start() {
			base.Start();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				this.SubStart();
				this.ApplySelItems();
				this.UpdateUIsState();

				this.ExLateCallFunc((a_oSender) => {
					m_oEngine.SetEnableRunning(true);
					m_oEngine.SetState(NSEngine.CEngine.EState.IDLE);
					m_oEngine.SetSubState(NSEngine.CEngine.ESubState.PLAY);

					m_oEngine.SelPlayerObj?.GetController<NSEngine.CEController>().SetState(NSEngine.CEController.EState.IDLE, true);
				}, KCDefine.B_VAL_0_5_REAL);

				Func.PlayBGSnd(EResKinds.SND_RES_BG_SCENE_GAME_01);
				CCommonAppInfoStorage.Inst.SetEnableEditor(false);
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
				CFunc.ShowLogWarning($"CSubPlaySceneManager.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 앱이 정지 되었을 경우 */
		public override void OnApplicationPause(bool a_bIsPause) {
			base.OnApplicationPause(a_bIsPause);

			// 재개 되었을 경우
			if(CSceneManager.IsAppRunning && !a_bIsPause) {
#if ADS_MODULE_ENABLE
				// 광고 출력이 가능 할 경우
				if(CAppInfoStorage.Inst.IsEnableShowFullscreenAds && CAdsManager.Inst.IsLoadFullscreenAds(CPluginInfoTable.Inst.AdsPlatform)) {
					Func.ShowFullscreenAds(null);
				}
#endif // #if ADS_MODULE_ENABLE

				Func.ShowResumePopup(this.PopupUIs, (a_oSender) => {
					(a_oSender as CResumePopup).Init(CResumePopup.MakeParams(new Dictionary<CResumePopup.ECallback, System.Action<CResumePopup>>() {
						[CResumePopup.ECallback.RESUME] = (a_oPopupSender) => this.OnReceivePopupCallback(a_oPopupSender, EPopupCallback.RESUME),
						[CResumePopup.ECallback.LEAVE] = (a_oPopupSender) => this.OnReceivePopupCallback(a_oPopupSender, EPopupCallback.LEAVE)
					}));
				});
			}
		}

		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				this.SubOnUpdate(a_fDeltaTime);
				m_oEngine.OnUpdate(a_fDeltaTime);
			}
		}

		/** 상태를 갱신한다 */
		public override void OnLateUpdate(float a_fDeltaTime) {
			base.OnLateUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				this.SubOnLateUpdate(a_fDeltaTime);
				m_oEngine.OnLateUpdate(a_fDeltaTime);

				// UI 갱신이 필요 할 경우
				if(m_oBoolDict[EKey.IS_UPDATE_UIS_STATE]) {
					this.UpdateUIsState();
					m_oBoolDict[EKey.IS_UPDATE_UIS_STATE] = false;
				}
			}
		}

		/** 내비게이션 스택 이벤트를 수신했을 경우 */
		public override void OnReceiveNavStackEvent(ENavStackEvent a_eEvent) {
			base.OnReceiveNavStackEvent(a_eEvent);

			// 백 키 눌림 이벤트 일 경우
			if(a_eEvent == ENavStackEvent.BACK_KEY_DOWN) {
				// 이전 씬이 레벨 에디터 씬 일 경우
				if(CSceneLoader.Inst.PrevActiveSceneName.Equals(KCDefine.B_SCENE_N_LEVEL_EDITOR)) {
					Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_LEAVE_P_MSG), this.OnReceiveLeavePopupResult);
				} else {
					this.OnTouchPauseBtn();
				}
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

				m_oEngine.HandleTouchEvent(a_oSender, a_oEventData, a_eTouchEvent);
			}
		}

		/** 엔진을 설정한다 */
		private void SetupEngine() {
			var oCallbackDict01 = new Dictionary<NSEngine.CEngine.ECallback, System.Action<NSEngine.CEngine>>() {
				[NSEngine.CEngine.ECallback.CLEAR] = this.OnReceiveClearCallback,
				[NSEngine.CEngine.ECallback.CLEAR_FAIL] = this.OnReceiveClearFailCallback
			};

			var oCallbackDict02 = new Dictionary<NSEngine.CEngine.ECallback, System.Action<NSEngine.CEngine, Dictionary<ulong, STTargetInfo>>>() {
				[NSEngine.CEngine.ECallback.ACQUIRE] = this.OnReceiveAcquireCallback
			};

			var oCallbackDict03 = new Dictionary<NSEngine.CEngine.ECallback, System.Action<NSEngine.CEngine, NSEngine.CEObjComponent, NSEngine.EEngineObjEvent, string>>() {
				[NSEngine.CEngine.ECallback.E_OBJ_EVENT] = this.OnReceiveEObjEventCallback
			};

			m_oEngine = CFactory.CreateObj<NSEngine.CEngine>(KDefine.PS_OBJ_N_ENGINE, this.gameObject);
			m_oEngine.Init(NSEngine.CEngine.MakeParams(this.ItemRoot, this.SkillRoot, this.ObjRoot, this.FXRoot, oCallbackDict01, oCallbackDict02, oCallbackDict03));
		}

		/** 보상 광고 UI 를 설정한다 */
		private void SetupRewardAdsUIs() {
			for(int i = 0; i < m_oRewardAdsUIsList.Count; ++i) {
				var eRewardAdsUIs = (ERewardAdsUIs)i;
				m_oRewardAdsUIsList[i]?.GetComponentInChildren<Button>()?.onClick.AddListener(() => this.OnTouchAdsBtn(eRewardAdsUIs));
			}
		}

		/** UI 상태를 갱신한다 */
		private void UpdateUIsState() {
			this.UpdateRewardAdsUIsState();
			this.SubUpdateUIsState();
		}

		/** 보상 광고 UI 상태를 갱신한다 */
		private void UpdateRewardAdsUIsState() {
			for(int i = 0; i < m_oRewardAdsUIsList.Count; ++i) {
				m_oRewardAdsUIsList[i]?.SetActive(CGameInfoStorage.Inst.PlayEpisodeInfo.ULevelID + KCDefine.B_VAL_1_INT >= KDefine.PS_MIN_LEVEL_ENABLE_REWARD_ADS_WATCH);
			}
		}

		/** 그만두기 팝업 결과를 수신했을 경우 */
		private void OnReceiveLeavePopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_LEVEL_EDITOR);
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
			}
		}

		/** 팝업 콜백을 수신했을 경우 */
		private void OnReceivePopupCallback(CPopup a_oSender, EPopupCallback a_eCallback) {
			// 팝업이 존재 할 경우
			if(a_oSender != null) {
				a_oSender.SetIgnoreAni(a_eCallback != EPopupCallback.RESUME && a_eCallback != EPopupCallback.CONTINUE);
				a_oSender.Close();
			}

			switch(a_eCallback) {
				case EPopupCallback.NEXT: this.HandleNextPopupCallback(a_oSender); break;
				case EPopupCallback.RETRY: this.HandleRetryPopupCallback(a_oSender); break;
				case EPopupCallback.RESUME: this.HandleResumePopupCallback(a_oSender); break;
				case EPopupCallback.CONTINUE: this.HandleContinuePopupCallback(a_oSender); break;
				case EPopupCallback.LEAVE: this.HandleLeavePopupCallback(a_oSender); break;
			}
		}

		/** 클리어 콜백을 수신했을 경우 */
		private void OnReceiveClearCallback(NSEngine.CEngine a_oSender) {
			var oLevelClearInfo = Access.GetLevelClearInfo(CGameInfoStorage.Inst.PlayCharacterID, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03, true);
			oLevelClearInfo.m_stRecordInfo.m_nIntRecord = a_oSender.RecordInfo.m_nIntRecord;
			oLevelClearInfo.m_stRecordInfo.m_dblRealRecord = a_oSender.RecordInfo.m_dblRealRecord;
			oLevelClearInfo.m_stBestRecordInfo.m_nIntRecord = System.Math.Max(a_oSender.RecordInfo.m_nIntRecord, oLevelClearInfo.m_stBestRecordInfo.m_nIntRecord);
			oLevelClearInfo.m_stBestRecordInfo.m_dblRealRecord = a_oSender.RecordInfo.m_dblRealRecord.ExIsGreate(oLevelClearInfo.m_stBestRecordInfo.m_dblRealRecord) ? a_oSender.RecordInfo.m_dblRealRecord : oLevelClearInfo.m_stBestRecordInfo.m_dblRealRecord;

			CGameInfoStorage.Inst.SaveGameInfo();
			this.ShowResultPopup(true);
		}

		/** 클리어 실패 콜백을 수신했을 경우 */
		private void OnReceiveClearFailCallback(NSEngine.CEngine a_oSender) {
			this.ShowResultPopup(false);
		}

		/** 정지 버튼을 눌렀을 경우 */
		private void OnTouchPauseBtn() {
			Func.ShowPausePopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CPausePopup).Init(CPausePopup.MakeParams(new Dictionary<CPausePopup.ECallback, System.Action<CPausePopup>>() {
					[CPausePopup.ECallback.LEAVE] = (a_oPopupSender) => this.OnReceivePopupCallback(a_oPopupSender, EPopupCallback.LEAVE)
				}));
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
#endif // #if ADS_MODULE_ENABLE
		}

		/** 선택 아이템을 적용한다 */
		private void ApplySelItems() {
			for(int i = 0; i < CGameInfoStorage.Inst.FreeSelItemKindsList.Count; ++i) {
				this.ApplySelItem(CGameInfoStorage.Inst.FreeSelItemKindsList[i]);
				CGameInfoStorage.Inst.FreeSelItemKindsList.ExRemoveVal(CGameInfoStorage.Inst.FreeSelItemKindsList[i]);
			}

			for(int i = 0; i < CGameInfoStorage.Inst.SelItemKindsList.Count; ++i) {
				var stValInfo = new STValInfo(EValType.INT, KCDefine.B_VAL_1_INT);
				var stTargetInfo = new STTargetInfo(ETargetKinds.ITEM_TARGET_NUMS, (int)CGameInfoStorage.Inst.SelItemKindsList[i], stValInfo);

				this.ApplySelItem(CGameInfoStorage.Inst.SelItemKindsList[i]);
				Func.Pay(CGameInfoStorage.Inst.PlayCharacterID, stTargetInfo);
			}

			CGameInfoStorage.Inst.ResetSelItems();
		}

		/** 레벨을 로드한다 */
		private void LoadLevel(CPopup a_oPopup, STEpisodeInfo a_stEpisodeInfo) {
			switch(CGameInfoStorage.Inst.PlayMode) {
				case EPlayMode.NORM: {
					// 레벨 로드가 가능 할 경우
					if(a_stEpisodeInfo.m_stIDInfo.m_nID01 > KCDefine.B_IDX_INVALID && a_stEpisodeInfo.m_stIDInfo.m_nID01 <= Access.GetNumLevelClearInfos(CGameInfoStorage.Inst.PlayCharacterID, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03)) {
						Func.SetupPlayEpisodeInfo(CGameInfoStorage.Inst.PlayCharacterID, a_stEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayMode, a_stEpisodeInfo.m_stIDInfo.m_nID02, a_stEpisodeInfo.m_stIDInfo.m_nID03);

#if ADS_MODULE_ENABLE
						Func.ShowFullscreenAds((a_oSender, a_bIsSuccess) => CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_PLAY));
#else
						CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_PLAY);
#endif // #if ADS_MODULE_ENABLE
					} else {
						this.HandleLeavePopupCallback(a_oPopup);
					}

					break;
				}
				case EPlayMode.TUTORIAL: {
					break;
				}
				case EPlayMode.TEST: {
#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
					CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_LEVEL_EDITOR);
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
					break;
				}
			}
		}

		/** 이어하기 팝업을 출력한다 */
		private void ShowContinuePopup() {
			Func.ShowContinuePopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CContinuePopup).Init(CContinuePopup.MakeParams(m_oIntDict[EKey.CONTINUE_TIMES], new Dictionary<CContinuePopup.ECallback, System.Action<CContinuePopup>>() {
					[CContinuePopup.ECallback.RETRY] = (a_oPopupSender) => this.OnReceivePopupCallback(a_oPopupSender, EPopupCallback.RETRY),
					[CContinuePopup.ECallback.CONTINUE] = (a_oPopupSender) => this.OnReceivePopupCallback(a_oPopupSender, EPopupCallback.CONTINUE),
					[CContinuePopup.ECallback.LEAVE] = (a_oPopupSender) => this.OnReceivePopupCallback(a_oPopupSender, EPopupCallback.LEAVE)
				}));
			});
		}

		/** 결과 팝업을 출력한다 */
		private void ShowResultPopup(bool a_bIsClear) {
			Func.ShowResultPopup(this.PopupUIs, (a_oSender) => {
				var stRecordInfo = new STRecordInfo {
					m_bIsSuccess = a_bIsClear,
					m_nIntRecord = m_oEngine.RecordInfo.m_nIntRecord,
					m_dblRealRecord = m_oEngine.RecordInfo.m_dblRealRecord
				};

				(a_oSender as CResultPopup).Init(CResultPopup.MakeParams(stRecordInfo, new Dictionary<CResultPopup.ECallback, System.Action<CResultPopup>>() {
					[CResultPopup.ECallback.NEXT] = (a_oPopupSender) => this.OnReceivePopupCallback(a_oPopupSender, EPopupCallback.NEXT),
					[CResultPopup.ECallback.RETRY] = (a_oPopupSender) => this.OnReceivePopupCallback(a_oPopupSender, EPopupCallback.RETRY),
					[CResultPopup.ECallback.LEAVE] = (a_oPopupSender) => this.OnReceivePopupCallback(a_oPopupSender, EPopupCallback.LEAVE)
				}));
			});

			CSceneLoader.Inst.LoadAdditiveScene(KCDefine.B_SCENE_N_RESULT);
		}

		/** 이전 팝업 콜백을 처리한다 */
		private void HandlePrevPopupCallback(CPopup a_oPopup) {
			this.LoadLevel(a_oPopup, Access.GetPrevLevelEpisodeInfo(CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03));
		}

		/** 다음 팝업 콜백을 처리한다 */
		private void HandleNextPopupCallback(CPopup a_oPopup) {
			this.LoadLevel(a_oPopup, Access.GetNextLevelEpisodeInfo(CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03));
		}

		/** 재시도 팝업 콜백을 처리한다 */
		private void HandleRetryPopupCallback(CPopup a_oPopup) {
#if ADS_MODULE_ENABLE
			Func.ShowFullscreenAds((a_oSender, a_bIsSuccess) => CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_PLAY));
#else
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_PLAY);
#endif // #if ADS_MODULE_ENABLE
		}

		/** 재개 팝업 콜백을 처리한다 */
		private void HandleResumePopupCallback(CPopup a_oPopup) {
			a_oPopup?.Close();
		}

		/** 이어하기 팝업 콜백을 처리한다 */
		private void HandleContinuePopupCallback(CPopup a_oPopup) {
			m_oIntDict[EKey.CONTINUE_TIMES] += KCDefine.B_VAL_1_INT;
		}

		/** 떠나기 팝업 콜백을 처리한다 */
		private void HandleLeavePopupCallback(CPopup a_oPopup) {
#if ADS_MODULE_ENABLE
			Func.ShowFullscreenAds((a_oSender, a_bIsSuccess) => CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MAIN));
#else
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MAIN);
#endif // #if ADS_MODULE_ENABLE
		}
		#endregion // 함수
	}

	/** 서브 플레이 씬 관리자 - 접근 */
	public partial class CSubPlaySceneManager : CPlaySceneManager {
		#region 함수
		/** UI 상태 갱신 여부를 변경한다 */
		public void SetEnableUpdateUIsState(bool a_bIsEnable) {
			m_oBoolDict[EKey.IS_UPDATE_UIS_STATE] = a_bIsEnable;
		}

		/** 최상단 객체 비율을 변경한다 */
		public void SetRootObjsScale(Vector3 a_stScale) {
			this.ItemRoot.transform.localScale = a_stScale;
			this.SkillRoot.transform.localScale = a_stScale;
			this.ObjRoot.transform.localScale = a_stScale;
			this.FXRoot.transform.localScale = a_stScale;
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
