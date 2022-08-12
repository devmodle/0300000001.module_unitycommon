#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace GameScene {
	/** 서브 게임 씬 관리자 */
	public partial class CSubGameSceneManager : CGameSceneManager {
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

					Func.SetupPlayLevelInfo(KCDefine.B_VAL_0_INT, EPlayMode.NORM);
				}
#endif			// #if DEBUG || DEVELOPMENT_BUILD

				this.AwakeSetup();
			}
		}
		
		/** 초기화 */
		public override void Start() {
			base.Start();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				this.StartSetup();
				this.UpdateUIsState();

				Func.PlayBGSnd(EResKinds.SND_BG_SCENE_GAME_01);
			}
		}

		/** 씬을 설정한다 */
		private void AwakeSetup() {
			this.SetupEngine();
			this.SetupRewardAdsUIs();

			// 버튼을 설정한다
			CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
				(KCDefine.U_OBJ_N_PAUSE_BTN, this.UIsBase, this.OnTouchPauseBtn),
				(KCDefine.U_OBJ_N_SETTINGS_BTN, this.UIsBase, this.OnTouchSettingsBtn)
			}, false);

			// 비율을 설정한다 {
			bool bIsValid01 = !float.IsNaN(m_oEngine.SelGridInfo.m_stScale.x) && !float.IsInfinity(m_oEngine.SelGridInfo.m_stScale.x);
			bool bIsValid02 = !float.IsNaN(m_oEngine.SelGridInfo.m_stScale.y) && !float.IsInfinity(m_oEngine.SelGridInfo.m_stScale.y);
			bool bIsValid03 = !float.IsNaN(m_oEngine.SelGridInfo.m_stScale.z) && !float.IsInfinity(m_oEngine.SelGridInfo.m_stScale.z);

			this.ObjRoot.transform.localScale = (bIsValid01 && bIsValid02 && bIsValid03) ? m_oEngine.SelGridInfo.m_stScale : Vector3.one;
			// 비율을 설정한다 }

			// 스프라이트를 설정한다 {
			var oSpriteInfoDict = new Dictionary<EKey, (Sprite, STSortingOrderInfo)>() {
				[EKey.BG_SPRITE] = (Access.GetBGSprite(KDefine.GS_IMG_P_FMT_BG, m_oEngine.Params.m_oLevelInfo.m_stIDInfo.m_nID01, m_oEngine.Params.m_oLevelInfo.m_stIDInfo.m_nID02, m_oEngine.Params.m_oLevelInfo.m_stIDInfo.m_nID03), KDefine.GS_SORTING_OI_BG),
				[EKey.TOP_BG_SPRITE] = (Access.GetBGSprite(KDefine.GS_IMG_P_FMT_TOP_BG, m_oEngine.Params.m_oLevelInfo.m_stIDInfo.m_nID01, m_oEngine.Params.m_oLevelInfo.m_stIDInfo.m_nID02, m_oEngine.Params.m_oLevelInfo.m_stIDInfo.m_nID03), KDefine.GS_SORTING_OI_TOP_BG),
				[EKey.BOTTOM_BG_SPRITE] = (Access.GetBGSprite(KDefine.GS_IMG_P_FMT_BOTTOM_BG, m_oEngine.Params.m_oLevelInfo.m_stIDInfo.m_nID01, m_oEngine.Params.m_oLevelInfo.m_stIDInfo.m_nID02, m_oEngine.Params.m_oLevelInfo.m_stIDInfo.m_nID03), KDefine.GS_SORTING_OI_BOTTOM_BG)
			};

			CFunc.SetupComponents(new List<(EKey, string, GameObject, GameObject)>() {
				(EKey.BG_SPRITE, $"{EKey.BG_SPRITE}", this.Objs, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_SPRITE)),
				(EKey.TOP_BG_SPRITE, $"{EKey.TOP_BG_SPRITE}", this.Objs, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_SPRITE)),
				(EKey.BOTTOM_BG_SPRITE, $"{EKey.BOTTOM_BG_SPRITE}", this.Objs, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_SPRITE))
			}, m_oSpriteDict, false);

			foreach(var stKeyVal in m_oSpriteDict) {
				stKeyVal.Value.drawMode = SpriteDrawMode.Tiled;
				stKeyVal.Value.tileMode = SpriteTileMode.Continuous;
				stKeyVal.Value.sprite = oSpriteInfoDict[stKeyVal.Key].Item1;
				stKeyVal.Value.ExSetSortingOrder(oSpriteInfoDict[stKeyVal.Key].Item2);
			}

			var stEpisodeInfo = global::Access.GetEpisodeInfo(m_oEngine.Params.m_oLevelInfo.m_stIDInfo.m_nID01, m_oEngine.Params.m_oLevelInfo.m_stIDInfo.m_nID02, m_oEngine.Params.m_oLevelInfo.m_stIDInfo.m_nID03);

			m_oSpriteDict.GetValueOrDefault(EKey.BG_SPRITE).size = new Vector3(Mathf.Max(this.ScreenWidth, stEpisodeInfo.m_stSize.x), Mathf.Max(CSceneManager.CanvasSize.y, stEpisodeInfo.m_stSize.y), KCDefine.B_VAL_0_REAL);
			m_oSpriteDict.GetValueOrDefault(EKey.BG_SPRITE).transform.localScale = Vector3.one;
			m_oSpriteDict.GetValueOrDefault(EKey.BG_SPRITE).transform.localPosition = Vector3.zero;

			m_oSpriteDict.GetValueOrDefault(EKey.TOP_BG_SPRITE).size = new Vector3(Mathf.Max(this.ScreenWidth, stEpisodeInfo.m_stSize.x), m_oSpriteDict.GetValueOrDefault(EKey.TOP_BG_SPRITE).sprite.rect.height, KCDefine.B_VAL_0_REAL);
			m_oSpriteDict.GetValueOrDefault(EKey.TOP_BG_SPRITE).transform.localScale = Vector3.one;
			m_oSpriteDict.GetValueOrDefault(EKey.TOP_BG_SPRITE).transform.localPosition = new Vector3(KCDefine.B_VAL_0_REAL, Mathf.Max(this.ScreenHeight / KCDefine.B_VAL_2_REAL, stEpisodeInfo.m_stSize.y / KCDefine.B_VAL_2_REAL) + (m_oSpriteDict.GetValueOrDefault(EKey.TOP_BG_SPRITE).sprite.rect.height / KCDefine.B_VAL_2_REAL), KCDefine.B_VAL_0_REAL);

			m_oSpriteDict.GetValueOrDefault(EKey.BOTTOM_BG_SPRITE).size = new Vector3(Mathf.Max(this.ScreenWidth, stEpisodeInfo.m_stSize.x), m_oSpriteDict.GetValueOrDefault(EKey.BOTTOM_BG_SPRITE).sprite.rect.height, KCDefine.B_VAL_0_REAL);
			m_oSpriteDict.GetValueOrDefault(EKey.BOTTOM_BG_SPRITE).transform.localScale = Vector3.one;
			m_oSpriteDict.GetValueOrDefault(EKey.BOTTOM_BG_SPRITE).transform.localPosition = new Vector3(KCDefine.B_VAL_0_REAL, -(Mathf.Max((this.ScreenHeight / KCDefine.B_VAL_2_REAL) - SampleEngineName.KDefine.E_OFFSET_BOTTOM, (stEpisodeInfo.m_stSize.y / KCDefine.B_VAL_2_REAL) - SampleEngineName.KDefine.E_OFFSET_BOTTOM) + (m_oSpriteDict.GetValueOrDefault(EKey.BOTTOM_BG_SPRITE).sprite.rect.height / KCDefine.B_VAL_2_REAL)), KCDefine.B_VAL_0_REAL);
			// 스프라이트를 설정한다 }

			#region 추가
			this.SubAwakeSetup();
			#endregion			// 추가
		}

		/** 씬을 설정한다 */
		private void StartSetup() {
			this.ApplySelItems();
			CGameInfoStorage.Inst.ResetSelItems();

			#region 추가
			this.SubStartSetup();
			#endregion			// 추가
		}

		/** 엔진을 설정한다 */
		private void SetupEngine() {
			m_oEngine = CFactory.CreateObj<SampleEngineName.CEngine>(KDefine.GS_OBJ_N_ENGINE, this.gameObject);

			m_oEngine.Init(new SampleEngineName.CEngine.STParams() {
				m_oLevelInfo = CGameInfoStorage.Inst.PlayLevelInfo,
				m_oClearInfo = CGameInfoStorage.Inst.TryGetLevelClearInfo(CGameInfoStorage.Inst.PlayCharacterID, CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nID01, out CClearInfo oLevelClearInfo, CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayLevelInfo.m_stIDInfo.m_nID03) ? oLevelClearInfo : null,

				m_oItemRoot = this.ItemRoot,
				m_oSkillRoot = this.SkillRoot,
				m_oObjRoot = this.ObjRoot,
				m_oFXRoot = this.FXRoot,

				m_oCallbackDict = new Dictionary<SampleEngineName.CEngine.ECallback, System.Action<SampleEngineName.CEngine>>() {
					[SampleEngineName.CEngine.ECallback.CLEAR] = this.OnClearLevel,
					[SampleEngineName.CEngine.ECallback.CLEAR_FAIL] = this.OnClearFailLevel
				}
			});
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

			#region 추가
			this.SubUpdateUIsState();
			#endregion			// 추가
		}

		/** 보상 광고 UI 상태를 갱신한다 */
		private void UpdateRewardAdsUIsState() {
			for(int i = 0; i < m_oRewardAdsUIsList.Count; ++i) {
				m_oRewardAdsUIsList[i]?.SetActive(m_oEngine.Params.m_oLevelInfo.ULevelID + KCDefine.B_VAL_1_INT >= KDefine.GS_MIN_LEVEL_ENABLE_REWARD_ADS_WATCH);
			}
		}
		#endregion			// 함수
	}

	/** 서브 게임 씬 관리자 - 서브 */
	public partial class CSubGameSceneManager : CGameSceneManager {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

#if DEBUG || DEVELOPMENT_BUILD
		/** 서브 테스트 UI */
		[System.Serializable]
		private partial struct STSubTestUIs {
			// Do Something
		}
#endif			// #if DEBUG || DEVELOPMENT_BUILD

		#region 변수
		/** =====> UI <===== */
#if DEBUG || DEVELOPMENT_BUILD
		[SerializeField] private STSubTestUIs m_stSubTestUIs;
#endif			// #if DEBUG || DEVELOPMENT_BUILD
		#endregion			// 변수

		#region 프로퍼티

		#endregion			// 프로퍼티

		#region 함수
		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				m_oEngine.OnUpdate(a_fDeltaTime);

				// UI 상태 갱신 모드 일 경우
				if(m_oBoolDict.GetValueOrDefault(EKey.IS_UPDATE_UIS_STATE)) {
					m_oBoolDict.ExReplaceVal(EKey.IS_UPDATE_UIS_STATE, false);
				}
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
				CFunc.ShowLogWarning($"CSubGameSceneManager.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 씬을 설정한다 */
		private void SubAwakeSetup() {
#if DEBUG || DEVELOPMENT_BUILD
			this.SetupSubTestUIs();
#endif			// #if DEBUG || DEVELOPMENT_BUILD
		}

		/** 씬을 설정한다 */
		private void SubStartSetup() {
			this.ExLateCallFunc((a_oSender) => m_oEngine.SetState(SampleEngineName.CEngine.EState.RUN), KCDefine.B_VAL_1_REAL);
		}

		/** UI 상태를 갱신한다 */
		private void SubUpdateUIsState() {
#if DEBUG || DEVELOPMENT_BUILD
			this.UpdateSubTestUIsState();
#endif			// #if DEBUG || DEVELOPMENT_BUILD
		}
		
		/** 선택 아이템을 적용한다 */
		private void ApplySelItem(EItemKinds a_eItemKinds) {
			// Do Something
		}

		/** 터치 시작 이벤트를 처리한다 */
		private void HandleTouchBeginEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// Do Something
		}

		/** 터치 이동 이벤트를 처리한다 */
		private void HandleTouchMoveEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// Do Something
		}

		/** 터치 종료 이벤트를 처리한다 */
		private void HandleTouchEndEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// Do Something
		}
		#endregion			// 함수

		#region 조건부 함수
#if UNITY_EDITOR
		/** 기즈모를 그린다 */
		public override void OnDrawGizmos() {
			base.OnDrawGizmos();

			// 메인 카메라가 존재 할 경우
			if(CSceneManager.IsExistsMainCamera) {
				// Do Something
			}
		}
#endif			// #if UNITY_EDITOR

#if DEBUG || DEVELOPMENT_BUILD
		/** 서브 테스트 UI 를 설정한다 */
		private void SetupSubTestUIs() {
			// Do Something
		}

		/** 서브 테스트 UI 상태를 갱신한다 */
		private void UpdateSubTestUIsState() {
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
		#endregion			// 조건부 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
