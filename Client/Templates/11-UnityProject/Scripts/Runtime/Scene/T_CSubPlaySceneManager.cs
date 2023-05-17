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
			SEL_REWARD_ADS_UIS,

			CONTINUE_TIMES,
			ADS_CONTINUE_TIMES,

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
			PREV,
			NEXT,
			RETRY,
			RESUME,
			CONTINUE,
			FINISH,
			LEAVE,
			[HideInInspector] MAX_VAL
		}

		/** 보상 광고 UI */
		private enum ERewardAdsUIs {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		private Dictionary<EKey, int> m_oIntDict = new Dictionary<EKey, int>() {
			[EKey.CONTINUE_TIMES] = KCDefine.B_VAL_0_INT,
			[EKey.ADS_CONTINUE_TIMES] = KCDefine.B_VAL_0_INT
		};

		private Dictionary<EKey, ERewardAdsUIs> m_oRewardAdsUIsDict = new Dictionary<EKey, ERewardAdsUIs>() {
			[EKey.SEL_REWARD_ADS_UIS] = ERewardAdsUIs.NONE
		};

		private NSEngine.CEngine m_oEngine = null;
		private Dictionary<EKey, SpriteRenderer> m_oSpriteDict = new Dictionary<EKey, SpriteRenderer>();

		/** =====> 객체 <===== */
		private Dictionary<EKey, GameObject> m_oObjDict = new Dictionary<EKey, GameObject>();

		[Header("=====> 객체 <=====")]
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
					CGameInfoStorage.Inst.SetPlayStartingTime(System.DateTime.Now);
					Func.SetupPlayEpisodeInfo(CGameInfoStorage.Inst.PlayCharacterID, KCDefine.B_VAL_0_INT, EPlayMode.NORM);
				}
#endif // #if DEBUG || DEVELOPMENT_BUILD

				this.SetupEngine();
				this.SetupRewardAdsUIs();

				// 아이템 종류를 설정한다
				CGameInfoStorage.Inst.SelItemKindsList.Sort((a_eLhs, a_eRhs) => a_eLhs.CompareTo(a_eRhs));
				CGameInfoStorage.Inst.FreeSelItemKindsList.Sort((a_eLhs, a_eRhs) => a_eLhs.CompareTo(a_eRhs));

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
				this.UpdateUIsState();

				this.ExLateCallFunc((a_oSender) => {
					m_oEngine.SetEnableRunning(true);
					m_oEngine.SetState(NSEngine.CEngine.EState.IDLE);
					m_oEngine.SetSubState(NSEngine.CEngine.ESubState.PLAY);

					this.ApplySelItems();
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

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
				// 단축키를 눌렀을 경우
				if(Input.GetKey(KeyCode.LeftShift)) {
					this.HandleHotKeys();
				}
#endif // #if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
			}
		}

		/** 상태를 갱신한다 */
		public override void OnLateUpdate(float a_fDeltaTime) {
			base.OnLateUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				this.SubOnLateUpdate(a_fDeltaTime);
				m_oEngine.OnLateUpdate(a_fDeltaTime);
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

		/** 정보를 저장한다 */
		protected override void SaveInfo() {
			base.SaveInfo();
			Func.SaveInfoStorages();
		}

		/** 상태를 갱신한다 */
		protected override void UpdateState() {
			base.UpdateState();
			this.UpdateUIsState();
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
				m_oRewardAdsUIsList[i]?.SetActive(CGameInfoStorage.Inst.PlayEpisodeInfo.ULevelID + KCDefine.B_VAL_1_INT >= KDefine.PS_MIN_LEVEL_ENABLE_WATCH_ADS);
			}
		}

		/** 그만두기 팝업 결과를 수신했을 경우 */
		private void OnReceiveLeavePopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				this.HandleLeavePopupCallback(a_oSender);
			}
		}

		/** 팝업 콜백을 수신했을 경우 */
		private void OnReceivePopupCallback(CPopup a_oSender, EPopupCallback a_eCallback) {
			switch(a_eCallback) {
				case EPopupCallback.PREV: this.HandlePrevPopupCallback(a_oSender); break;
				case EPopupCallback.NEXT: this.HandleNextPopupCallback(a_oSender); break;

				case EPopupCallback.RETRY: this.HandleRetryPopupCallback(a_oSender); break;
				case EPopupCallback.RESUME: this.HandleResumePopupCallback(a_oSender); break;
				case EPopupCallback.CONTINUE: this.HandleContinuePopupCallback(a_oSender); break;

				case EPopupCallback.FINISH: this.HandleFinishPopupCallback(a_oSender); break;
				case EPopupCallback.LEAVE: this.HandleLeavePopupCallback(a_oSender); break;
			}

			this.SetEnableSaveInfo(true);
			this.SetEnableUpdateState(true);

			m_oEngine.SetEnableRunning(a_eCallback == EPopupCallback.RESUME || a_eCallback == EPopupCallback.CONTINUE);

			a_oSender?.SetIgnoreAni(a_eCallback != EPopupCallback.RESUME && a_eCallback != EPopupCallback.CONTINUE);
			a_oSender?.Close();
		}

		/** 클리어 콜백을 수신했을 경우 */
		private void OnReceiveClearCallback(NSEngine.CEngine a_oSender) {
			// 테스트 모드가 아닐 경우
			if(CGameInfoStorage.Inst.PlayMode != EPlayMode.TEST) {
				var oLevelClearInfo = Access.GetLevelClearInfo(CGameInfoStorage.Inst.PlayCharacterID, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03, true);
				oLevelClearInfo.m_stRecordInfo.m_nIntRecord = a_oSender.RecordInfo.m_nIntRecord;
				oLevelClearInfo.m_stRecordInfo.m_dblRealRecord = a_oSender.RecordInfo.m_dblRealRecord;
				oLevelClearInfo.m_stBestRecordInfo.m_nIntRecord = System.Math.Max(a_oSender.RecordInfo.m_nIntRecord, oLevelClearInfo.m_stBestRecordInfo.m_nIntRecord);
				oLevelClearInfo.m_stBestRecordInfo.m_dblRealRecord = a_oSender.RecordInfo.m_dblRealRecord.ExIsGreate(oLevelClearInfo.m_stBestRecordInfo.m_dblRealRecord) ? a_oSender.RecordInfo.m_dblRealRecord : oLevelClearInfo.m_stBestRecordInfo.m_dblRealRecord;
			}

			this.ShowResultPopup(true);
		}

		/** 클리어 실패 콜백을 수신했을 경우 */
		private void OnReceiveClearFailCallback(NSEngine.CEngine a_oSender) {
			// 이어하기가 가능 할 경우
			if(m_oIntDict[EKey.CONTINUE_TIMES] < KDefine.PS_MAX_TIMES_CONTINUE) {
				this.ShowContinuePopup();
			} else {
				this.ShowResultPopup(false);
			}
		}

		/** 정지 버튼을 눌렀을 경우 */
		private void OnTouchPauseBtn() {
			Func.ShowPausePopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CPausePopup).Init(CPausePopup.MakeParams(new Dictionary<CPausePopup.ECallback, System.Action<CPausePopup>>() {
					[CPausePopup.ECallback.RETRY] = (a_oPopupSender) => this.OnReceivePopupCallback(a_oPopupSender, EPopupCallback.RETRY),
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
		private void ApplySelItem(EItemKinds a_eItemKinds) {
			m_oEngine.ApplyItem(CItemInfoTable.Inst.GetItemInfo(a_eItemKinds), Access.GetItemTargetInfo(CGameInfoStorage.Inst.PlayCharacterID, a_eItemKinds));
			global::Func.Pay(CGameInfoStorage.Inst.PlayCharacterID, new STTargetInfo(ETargetKinds.ITEM_TARGET_NUMS, (int)a_eItemKinds, new STValInfo(EValType.INT, KCDefine.B_VAL_1_INT)));
		}

		/** 선택 아이템을 적용한다 */
		private void ApplySelItems() {
			for(int i = 0; i < CGameInfoStorage.Inst.SelItemKindsList.Count; ++i) {
				this.ApplySelItem(CGameInfoStorage.Inst.SelItemKindsList[i]);
			}

			for(int i = 0; i < CGameInfoStorage.Inst.FreeSelItemKindsList.Count; ++i) {
				this.ApplySelItem(CGameInfoStorage.Inst.FreeSelItemKindsList[i]);
			}

			Func.SaveInfoStorages();
			CGameInfoStorage.Inst.ResetSelItems();
		}

		/** 레벨을 로드한다 */
		private void LoadLevel(CPopup a_oPopup, STEpisodeInfo a_stEpisodeInfo) {
			// 일반 모드 일 경우
			if(CGameInfoStorage.Inst.PlayMode == EPlayMode.NORM) {
				bool bIsValid01 = a_stEpisodeInfo.m_stIDInfo.m_nID01 < CLevelInfoTable.Inst.GetNumLevelInfos(a_stEpisodeInfo.m_stIDInfo.m_nID02, a_stEpisodeInfo.m_stIDInfo.m_nID03);
				bool bIsValid02 = a_stEpisodeInfo.m_stIDInfo.m_nID01 <= Access.GetNumLevelClearInfos(CGameInfoStorage.Inst.PlayCharacterID, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03);

				// 레벨 에피소드 정보가 존재 할 경우
				if(bIsValid01 && bIsValid02 && a_stEpisodeInfo.m_stIDInfo.m_nID01 > KCDefine.B_IDX_INVALID) {
					CGameInfoStorage.Inst.SetPlayStartingTime(System.DateTime.Now);
					Func.SetupPlayEpisodeInfo(CGameInfoStorage.Inst.PlayCharacterID, a_stEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayMode, a_stEpisodeInfo.m_stIDInfo.m_nID02, a_stEpisodeInfo.m_stIDInfo.m_nID03);

#if ADS_MODULE_ENABLE
					Func.ShowFullscreenAds(null, KCDefine.B_VAL_0_1_REAL);
#endif // #if ADS_MODULE_ENABLE

					CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_PLAY);
				} else {
					this.HandleLeavePopupCallback(a_oPopup);
				}
			} else {
				this.HandleLeavePopupCallback(a_oPopup);
			}
		}

		/** 이어하기 팝업을 출력한다 */
		private void ShowContinuePopup() {
			Func.ShowContinuePopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CContinuePopup).Init(CContinuePopup.MakeParams(m_oIntDict[EKey.CONTINUE_TIMES], m_oIntDict[EKey.ADS_CONTINUE_TIMES], new Dictionary<CContinuePopup.ECallback, System.Action<CContinuePopup>>() {
					[CContinuePopup.ECallback.RETRY] = (a_oPopupSender) => this.OnReceivePopupCallback(a_oPopupSender, EPopupCallback.RETRY),
					[CContinuePopup.ECallback.CONTINUE] = (a_oPopupSender) => this.OnReceivePopupCallback(a_oPopupSender, EPopupCallback.CONTINUE),
					[CContinuePopup.ECallback.FINISH] = (a_oPopupSender) => this.OnReceivePopupCallback(a_oPopupSender, EPopupCallback.FINISH)
				}));
			});
		}

		/** 결과 팝업을 출력한다 */
		private void ShowResultPopup(bool a_bIsClear) {
			Func.ShowResultPopup(this.PopupUIs, (a_oSender) => {
				var stIDInfo = CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo;
				var stRecordInfo = new STRecordInfo(m_oEngine.RecordInfo.m_nIntRecord, m_oEngine.RecordInfo.m_dblRealRecord, a_bIsClear);

				(a_oSender as CResultPopup).Init(CResultPopup.MakeParams(stIDInfo, stRecordInfo, new Dictionary<CResultPopup.ECallback, System.Action<CResultPopup>>() {
					[CResultPopup.ECallback.NEXT] = (a_oPopupSender) => this.OnReceivePopupCallback(a_oPopupSender, EPopupCallback.NEXT),
					[CResultPopup.ECallback.RETRY] = (a_oPopupSender) => this.OnReceivePopupCallback(a_oPopupSender, EPopupCallback.RETRY),
					[CResultPopup.ECallback.LEAVE] = (a_oPopupSender) => this.OnReceivePopupCallback(a_oPopupSender, EPopupCallback.LEAVE)
				}));
			});

			m_oEngine.SetEnableRunning(false);
			CSceneLoader.Inst.LoadAdditiveScene(KCDefine.B_SCENE_N_RESULT);
		}
		#endregion // 함수

		#region 조건부 함수
#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		/** 단축키를 처리한다 */
		private void HandleHotKeys() {
			// 클리어 버튼을 눌렀을 경우
			if(Input.GetKeyDown(KeyCode.C)) {
				this.OnReceiveClearCallback(m_oEngine);
			}
			// 클리어 실패 버튼을 눌렀을 경우
			else if(Input.GetKeyDown(KeyCode.F)) {
				this.OnReceiveClearFailCallback(m_oEngine);
			}
		}
#endif // #if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		#endregion // 조건부 함수
	}

	/** 서브 플레이 씬 관리자 - 접근 */
	public partial class CSubPlaySceneManager : CPlaySceneManager {
		#region 함수
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
