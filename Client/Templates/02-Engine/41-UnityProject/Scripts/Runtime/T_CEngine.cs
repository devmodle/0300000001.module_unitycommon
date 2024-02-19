#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.Linq;
using UnityEngine.EventSystems;

namespace NSEngine {
	/** 엔진 */
	public partial class CEngine : CComponent {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			CELL_OBJ_ROOT,
			PLAYER_OBJ_ROOT,
			NON_PLAYER_OBJ_ROOT,
			[HideInInspector] MAX_VAL
		}

		/** 상태 */
		public enum EState {
			NONE = -1,
			IDLE,
			[HideInInspector] MAX_VAL
		}

		/** 서브 상태 */
		public enum ESubState {
			NONE = -1,
			PLAY,
			PAUSE,
			[HideInInspector] MAX_VAL
		}

		/** 콜백 */
		public enum ECallback {
			NONE = -1,
			CLEAR,
			CLEAR_FAIL,
			ACQUIRE,
			E_OBJ_EVENT,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public struct STParams {
			public GameObject m_oItemRoot;
			public GameObject m_oSkillRoot;
			public GameObject m_oObjRoot;
			public GameObject m_oFXRoot;

			public Dictionary<ECallback, System.Action<CEngine>> m_oCallbackDictA;
			public Dictionary<ECallback, System.Action<CEngine, Dictionary<ulong, STTargetInfo>>> m_oCallbackDictB;
			public Dictionary<ECallback, System.Action<CEngine, CEObjComponent, EEngineObjEvent, string>> m_oCallbackDictC;
		}

		#region 변수
		private Dictionary<EState, System.Func<bool>> m_oStateCheckerDict = new Dictionary<EState, System.Func<bool>>();
		private Dictionary<ESubState, System.Func<bool>> m_oSubStateCheckerDict = new Dictionary<ESubState, System.Func<bool>>();

		[Header("=====> Game Objects <=====")]
		private List<GameObject> m_oCellObjRootList = new List<GameObject>();
		private Dictionary<EKey, GameObject> m_oObjDict = new Dictionary<EKey, GameObject>();
		#endregion // 변수

		#region 프로퍼티
		public STParams Params { get; private set; }
		public STRecordInfo RecordInfo { get; private set; }

		public bool IsFinish { get; private set; } = false;
		public bool IsRunning { get; private set; } = false;

		public int SelGridInfoIdx { get; private set; } = 0;
		public int SelPlayerObjIdx { get; private set; } = 0;

		public float DeltaTimeScale { get; private set; } = 1.0f;

		public EState State { get; private set; } = EState.NONE;
		public ESubState SubState { get; private set; } = ESubState.NONE;
		public List<STGridInfo> GridInfoList { get; } = new List<STGridInfo>();

		public List<CEItem> ItemListWrapper { get; } = new List<CEItem>();
		public List<CESkill> SkillListWrapper { get; } = new List<CESkill>();
		public List<CEFX> FXListWrapper { get; } = new List<CEFX>();

		public List<CEObj> ObjListWrapper { get; } = new List<CEObj>();
		public List<CEObj> PlayerObjListWrapper { get; } = new List<CEObj>();
		public List<CEObj> EnemyObjListWrapper { get; } = new List<CEObj>();

		public List<List<CEObj>[,]> CellObjListsContainer { get; } = new List<List<CEObj>[,]>();
		public List<Stack<List<CEObj>>[]> CellObjStacksContainerH { get; } = new List<Stack<List<CEObj>>[]>();
		public List<Stack<List<CEObj>>[]> CellObjStacksContainerV { get; } = new List<Stack<List<CEObj>>[]>();

		public List<(STItemInfo, CItemTargetInfo)> ItemInfoTupleList { get; } = new List<(STItemInfo, CItemTargetInfo)>();
		public List<(STSkillInfo, CSkillTargetInfo)> SkillInfoTupleList { get; } = new List<(STSkillInfo, CSkillTargetInfo)>();

		public Dictionary<ulong, STTargetInfo> ClearTargetInfoDict { get; } = new Dictionary<ulong, STTargetInfo>();
		public bool IsEnableUpdate => Time.timeScale.ExIsGreat(KCDefine.B_VAL_0_REAL);

		public Vector3 EpisodeSize => new Vector3(Mathf.Max(CSceneManager.ActiveSceneManager.ScreenWidth, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.x), Mathf.Max(CSceneManager.ActiveSceneManager.ScreenHeight, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.y), CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.z);
		public Vector3 CameraEpisodeSize => new Vector3(Mathf.Max(CSceneManager.ActiveSceneManager.ScreenWidth, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.x - CSceneManager.ActiveSceneManager.ScreenWidth), Mathf.Max(CSceneManager.ActiveSceneManager.ScreenHeight, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.y - CSceneManager.ActiveSceneManager.ScreenHeight), CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.z);
		public STGridInfo SelGridInfo => this.GridInfoList.ExGetVal(this.SelGridInfoIdx, STGridInfo.INVALID);

		public CEObj SelPlayerObj => this.PlayerObjListWrapper.ExGetVal(this.SelPlayerObjIdx, null);
		public List<CEObj>[,] SelCellObjLists => this.CellObjListsContainer.ExGetVal(this.SelGridInfoIdx, null);
		public Stack<List<CEObj>>[] SelCellObjStacksH => this.CellObjStacksContainerH.ExGetVal(this.SelGridInfoIdx, null);
		public Stack<List<CEObj>>[] SelCellObjStacksV => this.CellObjStacksContainerV.ExGetVal(this.SelGridInfoIdx, null);
		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();
			this.SubAwake();
		}

		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			this.Params = a_stParams;

			this.Setup();
			this.SetupLevel();
			this.SetupGridLine();

			// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능) {
#if DISABLE_THIS
			// 캐릭터 정보가 존재 할 경우
			if(CObjInfoTable.Inst.TryGetObjInfo(EObjKinds.PLAYABLE_OBJ_COMMON_CHARACTER_01, out STObjInfo stObjInfo)) {
				this.PlayerObjListWrapper.ExAddVal(this.CreatePlayerObj(stObjInfo, CUserInfoStorage.Inst.GetCharacterUserInfo(CGameInfoStorage.Inst.PlayCharacterID), null));
				CSceneManager.ActiveSceneMainCamera.transform.position = new Vector3(this.SelPlayerObj.transform.position.x + (KDefine.E_OFFSET_MAIN_CAMERA.x * CAccess.ResolutionUnitScale), this.SelPlayerObj.transform.position.y + (KDefine.E_OFFSET_MAIN_CAMERA.y * CAccess.ResolutionUnitScale), CSceneManager.ActiveSceneMainCamera.transform.position.z);
			}
#endif // #if DISABLE_THIS
			// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능) }

			this.SubInit();
		}

		/** 상태를 리셋한다 */
		public override void Reset() {
			base.Reset();
			this.IsRunning = false;

			this.SetState(EState.NONE);
		}

		/** 제거되었을 경우 */
		public override void OnDestroy() {
			base.OnDestroy();

			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsAppRunning) {
					this.SubOnDestroy();
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CEngine.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning && this.IsEnableUpdate) {
				// 실행 중 일 경우
				if(this.IsRunning) {
					switch(this.SubState) {
						case ESubState.PLAY: this.HandlePlaySubState(a_fDeltaTime * this.DeltaTimeScale); break;
						case ESubState.PAUSE: this.HandlePauseSubState(a_fDeltaTime * this.DeltaTimeScale); break;
					}

					// 플레이어 객체가 존재 할 경우
					if(this.PlayerObjListWrapper.ExIsValid()) {
						var stMainCameraPos = this.GetMainCameraPos();
						CSceneManager.ActiveSceneMainCamera.transform.position = Vector3.Lerp(CSceneManager.ActiveSceneMainCamera.transform.position, stMainCameraPos.ExToWorld(this.Params.m_oObjRoot), a_fDeltaTime * KCDefine.B_VAL_9_REAL);
					}
				}

				this.SubOnUpdate(a_fDeltaTime * this.DeltaTimeScale);
			}
		}

		/** 상태를 갱신한다 */
		public override void OnLateUpdate(float a_fDeltaTime) {
			base.OnLateUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning && this.IsEnableUpdate) {
				this.SubOnLateUpdate(a_fDeltaTime * this.DeltaTimeScale);
			}
		}

		/** 플레이어 객체 이동을 처리한다 */
		public void MovePlayerObj(Vector3 a_stVal, EVecType a_eVecType = EVecType.DIRECTION) {
			this.SelPlayerObj.GetController<CEPlayerObjController>().Move(a_stVal, a_eVecType);
		}

		/** 아이템을 적용한다 */
		public void ApplyItem(STItemInfo a_stItemInfo, CItemTargetInfo a_oItemTargetInfo) {
			// 아이템 적용이 가능 할 경우
			if(!this.ItemInfoTupleList.FindIndex((a_stItemInfoTuple) => a_stItemInfoTuple.Item1.m_eItemKinds == a_stItemInfo.m_eItemKinds).ExIsValidIdx()) {
				this.ItemInfoTupleList.ExAddVal((a_stItemInfo, a_oItemTargetInfo));
				global::Func.Pay(CGameInfoStorage.Inst.PlayCharacterID, new STTargetInfo(ETargetKinds.ITEM_TARGET_NUMS, (int)a_stItemInfo.m_eItemKinds, new STValInfo(EValType.INT, KCDefine.B_VAL_1_INT)));

				switch(((int)a_stItemInfo.m_eItemKinds).ExKindsToSubTypeVal()) {
					case KEnumVal.IK_GAME_ITEM_SUB_TYPE_VAL: this.ApplyGameItem(a_stItemInfo, a_oItemTargetInfo); break;
					case KEnumVal.IK_BOOSTER_ITEM_SUB_TYPE_VAL: this.ApplyBoosterItem(a_stItemInfo, a_oItemTargetInfo); break;
				}
			}
		}

		/** 스킬을 적용한다 */
		public void ApplySkill(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo) {
			// 스킬 적용이 가능 할 경우
			if(!this.SkillInfoTupleList.FindIndex((a_stSkillInfoTuple) => a_stSkillInfoTuple.Item1.m_eSkillKinds == a_stSkillInfo.m_eSkillKinds).ExIsValidIdx()) {
				this.SkillInfoTupleList.ExAddVal((a_stSkillInfo, a_oSkillTargetInfo));
			}
		}

		/** 플레이어 객체 스킬을 적용한다 */
		public void ApplyPlayerObjSkill(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo) {
			this.SelPlayerObj.GetController<CEPlayerObjController>().ApplySkill(a_stSkillInfo, a_oSkillTargetInfo);
		}

		/** 터치 이벤트를 처리한다 */
		public void HandleTouchEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData, ETouchEvent a_eTouchEvent) {
			var stPos = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot, CSceneManager.ActiveSceneManager.ScreenSize);

			// 그리드 영역 일 경우
			if(this.GridInfoList.ExIsValidIdx(this.SelGridInfoIdx) && this.SelGridInfo.m_stViewBounds.Contains(stPos)) {
				switch(a_eTouchEvent) {
					case ETouchEvent.BEGIN: this.HandleTouchBeginEvent(a_oSender, a_oEventData); break;
					case ETouchEvent.MOVE: this.HandleTouchMoveEvent(a_oSender, a_oEventData); break;
					case ETouchEvent.END: this.HandleTouchEndEvent(a_oSender, a_oEventData); break;
				}
			}
		}

		/** 엔진 객체 이벤트를 수신했을 경우 */
		private void OnReceiveEObjEvent(CEObjComponent a_oSender, EEngineObjEvent a_eEvent, string a_oParams) {
			switch(a_eEvent) {
				case EEngineObjEvent.AVOID: this.HandleAvoidEObjEvent(a_oSender, a_oParams); break;
				case EEngineObjEvent.DAMAGE: this.HandleDamageEObjEvent(a_oSender, a_oParams); break;
				case EEngineObjEvent.CRITICAL_DAMAGE: this.HandleCriticalDamageEObjEvent(a_oSender, a_oParams); break;
			}

			// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능) {
#if DISABLE_THIS
			// 체력이 없을 경우
			if(a_oSender.AbilityValDictWrapper.m_oDict01.ExGetAbilityVal(EAbilityKinds.STAT_ABILITY_HP_01) <= KCDefine.B_VAL_0_INT) {
				// 플레이어 객체 일 경우
				if(this.IsClearFail() || a_oSender.Params.m_stBaseParams.m_oGameObjsPoolKey.Equals(KDefine.E_KEY_PLAYER_OBJ_OBJS_POOL)) {
					this.HandleClearFailState();
				} else {
					foreach(var stKeyVal in CGameInfoStorage.Inst.PlayEpisodeInfo.m_oClearTargetInfoDict) {
						bool bIsValid = stKeyVal.Value.TargetType == ETargetType.ITEM && (a_oSender as CEItem != null) && stKeyVal.Value.Kinds == ((int)(a_oSender as CEItem).Params.m_stItemInfo.m_eItemKinds).ExKindsToCorrectKinds(stKeyVal.Value.m_eKindsGroupType);

						// 클리어 타겟 정보가 존재 할 경우
						if(bIsValid || (stKeyVal.Value.TargetType == ETargetType.OBJ && (a_oSender as CEObj != null) && stKeyVal.Value.Kinds == ((int)(a_oSender as CEObj).Params.m_stObjInfo.m_eObjKinds).ExKindsToCorrectKinds(stKeyVal.Value.m_eKindsGroupType))) {
							this.ClearTargetInfoDict.ExIncrTargetVal(stKeyVal.Value.m_eTargetKinds, stKeyVal.Value.m_nKinds, -KCDefine.B_VAL_1_INT);
						}
					}

					// 선택 플레이어가 존재 할 경우
					if(this.SelPlayerObj != null) {
						var oAcquireTargetInfoDict = CCollectionPoolManager.Inst.SpawnDict<ulong, STTargetInfo>();

						try {
							this.SetupAcquireTargetInfos(a_oSender, oAcquireTargetInfoDict);
							this.Params.m_oCallbackDictB.GetValueOrDefault(ECallback.ACQUIRE)?.Invoke(this, oAcquireTargetInfoDict);

							global::Func.Acquire(CGameInfoStorage.Inst.PlayCharacterID, oAcquireTargetInfoDict, this.SelPlayerObj.Params.m_oObjTargetInfo, true);

							var stObjTradeInfo = CObjInfoTable.Inst.GetEnhanceObjTradeInfo(this.SelPlayerObj.Params.m_stObjInfo.m_eObjKinds);
							var stSkipTargetValInfo = this.SelPlayerObj.Params.m_oObjTargetInfo.m_oAbilityTargetInfoDict.ExGetSkipTargetValInfo(ETargetKinds.ABILITY_TARGET, (int)EAbilityKinds.STAT_ABILITY_EXP, (int)this.SelPlayerObj.Params.m_oObjTargetInfo.m_oAbilityTargetInfoDict.ExGetTargetVal(ETargetKinds.ABILITY_TARGET, (int)EAbilityKinds.STAT_ABILITY_LV), stObjTradeInfo.m_oPayTargetInfoDict);

							// 플레이어 객체 레벨 강화가 가능 할 경우
							if(stSkipTargetValInfo.Item1 >= stSkipTargetValInfo.Item3) {
								global::Func.Trade(CGameInfoStorage.Inst.PlayCharacterID, stObjTradeInfo, this.PlayerObjListWrapper[KCDefine.B_VAL_0_INT].Params.m_oObjTargetInfo);
								this.SelPlayerObj.SetupAbilityVals();
							}
						} finally {
							this.RemoveEObjComponent(a_oSender);
							CCollectionPoolManager.Inst.DespawnDict(oAcquireTargetInfoDict);
						}
					}
				}
			}

			// 클리어 상태 일 경우
			if(this.IsClear()) {
				this.HandleClearState();
			}
#endif // #if DISABLE_THIS
			// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능) }

			this.Params.m_oCallbackDictC.GetValueOrDefault(ECallback.E_OBJ_EVENT)?.Invoke(this, a_oSender, a_eEvent, a_oParams);
			CSceneManager.GetSceneManager<PlayScene.CSubPlaySceneManager>().SetIsDirtyUpdateState(true);
		}
		#endregion // 함수
	}

	/** 엔진 - 설정 */
	public partial class CEngine : CComponent {
		#region 함수
		/** 엔진을 설정한다 */
		private void Setup() {
			for(int i = 0; i < KCDefine.B_VAL_1_INT; ++i) {
				this.CellObjListsContainer.ExAddVal(new List<CEObj>[CGameInfoStorage.Inst.PlayLevelInfo.NumCells.y, CGameInfoStorage.Inst.PlayLevelInfo.NumCells.x]);
				this.CellObjStacksContainerH.ExAddVal(new Stack<List<CEObj>>[CGameInfoStorage.Inst.PlayLevelInfo.NumCells.x]);
				this.CellObjStacksContainerV.ExAddVal(new Stack<List<CEObj>>[CGameInfoStorage.Inst.PlayLevelInfo.NumCells.y]);

				for(int j = 0; j < CGameInfoStorage.Inst.PlayLevelInfo.NumCells.x; ++j) {
					this.CellObjStacksContainerH[i][j] = new Stack<List<CEObj>>();
				}

				for(int j = 0; j < CGameInfoStorage.Inst.PlayLevelInfo.NumCells.y; ++j) {
					this.CellObjStacksContainerV[i][j] = new Stack<List<CEObj>>();
				}
			}

			CGameInfoStorage.Inst.PlayEpisodeInfo.m_oClearTargetInfoDict.ExCopyTo(this.ClearTargetInfoDict, (_, a_stTargetInfo) => a_stTargetInfo);

			// 그리드 정보를 설정한다
			this.GridInfoList.Clear();
			Factory.MakeGridInfos(CGameInfoStorage.Inst.PlayLevelInfo, this.GridInfoList);

			// 객체를 설정한다 {
			CFunc.SetupGameObjs(new List<(EKey, string, GameObject, GameObject)>() {
				(EKey.CELL_OBJ_ROOT, $"{EKey.CELL_OBJ_ROOT}", this.Params.m_oObjRoot, null),
				(EKey.PLAYER_OBJ_ROOT, $"{EKey.PLAYER_OBJ_ROOT}", this.Params.m_oObjRoot, null),
				(EKey.NON_PLAYER_OBJ_ROOT, $"{EKey.NON_PLAYER_OBJ_ROOT}", this.Params.m_oObjRoot, null)
			}, m_oObjDict);

			for(int i = 0; i < this.GridInfoList.Count; ++i) {
				string oName = string.Format(KCDefine.PS_OBJ_N_FMT_CELL_OBJ_ROOT, i + KCDefine.B_VAL_1_INT);
				m_oCellObjRootList.ExAddVal(CFactory.CreateGameObj(oName, m_oObjDict[EKey.CELL_OBJ_ROOT]));
			}
			// 객체를 설정한다 }

			// 게임 객체 풀을 설정한다 {
			CSceneManager.ActiveSceneManager.AddGameObjsPool(KDefine.E_KEY_ITEM_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_ITEM), this.Params.m_oItemRoot, KCDefine.U_SIZE_GAME_OBJS_POOL_50, false);
			CSceneManager.ActiveSceneManager.AddGameObjsPool(KDefine.E_KEY_SKILL_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_SKILL), this.Params.m_oSkillRoot, KCDefine.U_SIZE_GAME_OBJS_POOL_50, false);
			CSceneManager.ActiveSceneManager.AddGameObjsPool(KDefine.E_KEY_OBJ_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_OBJ), this.Params.m_oObjRoot, KCDefine.U_SIZE_GAME_OBJS_POOL_50, false);
			CSceneManager.ActiveSceneManager.AddGameObjsPool(KDefine.E_KEY_FX_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_FX), this.Params.m_oFXRoot, KCDefine.U_SIZE_GAME_OBJS_POOL_50, false);

			CSceneManager.ActiveSceneManager.AddGameObjsPool(KDefine.E_KEY_CELL_OBJ_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_CELL_OBJ), m_oObjDict[EKey.CELL_OBJ_ROOT], KCDefine.U_SIZE_GAME_OBJS_POOL_50, false);
			CSceneManager.ActiveSceneManager.AddGameObjsPool(KDefine.E_KEY_PLAYER_OBJ_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_PLAYER_OBJ), m_oObjDict[EKey.PLAYER_OBJ_ROOT], KCDefine.B_VAL_1_INT, false);
			CSceneManager.ActiveSceneManager.AddGameObjsPool(KDefine.E_KEY_ENEMY_OBJ_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_ENEMY_OBJ), m_oObjDict[EKey.NON_PLAYER_OBJ_ROOT], KCDefine.U_SIZE_GAME_OBJS_POOL_50, false);
			// 게임 객체 풀을 설정한다 }

			this.SubSetup();
		}

		/** 레벨을 설정한다 */
		private void SetupLevel() {
			// 레벨 정보가 존재 할 경우
			if(CGameInfoStorage.Inst.PlayLevelInfo != null) {
				for(int i = 0; i < this.GridInfoList.Count; ++i) {
					for(int j = 0; j < CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer.Count; ++j) {
						for(int k = 0; k < CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer[j].Count; ++k) {
							this.SetupCell(CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer[j][k], this.GridInfoList[i]);
						}
					}
				}

				this.SubSetupLevel();
			}
		}

		/** 셀을 설정한다 */
		private void SetupCell(STCellInfo a_stCellInfo, STGridInfo a_stGridInfo) {
			var oCellObjList = new List<CEObj>();

			for(int i = 0; i < a_stCellInfo.m_oCellObjInfoList.Count; ++i) {
				// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능) {
#if DISABLE_THIS
				// 객체 종류가 유효 할 경우
				if(a_stCellInfo.m_oCellObjInfoList[i].ObjKinds.ExIsValid()) {
					var oCellObj = this.CreateCellObj(CObjInfoTable.Inst.GetObjInfo(a_stCellInfo.m_oCellObjInfoList[i].ObjKinds), a_stGridInfo, null);
					oCellObj.transform.localPosition = a_stGridInfo.m_stPivotPos + a_stCellInfo.m_stIdx.ExToPos(Access.CellCenterOffset, Access.CellSize);				
					
					oCellObj.GetController<CECellObjController>().SetCellIdx(a_stCellInfo.m_stIdx);
					oCellObj.GetController<CECellObjController>().SetCellObjInfo((STCellObjInfo)a_stCellInfo.m_oCellObjInfoList[i].Clone());

					oCellObjList.ExAddVal(oCellObj);
				}
#endif // #if DISABLE_THIS
				// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능) }
			}

			this.CellObjListsContainer[a_stCellInfo.m_stIdx.z].ExSetVal(a_stCellInfo.m_stIdx, oCellObjList);
			this.CellObjStacksContainerH[a_stCellInfo.m_stIdx.z][a_stCellInfo.m_stIdx.x].Push(new List<CEObj>(oCellObjList));
			this.CellObjStacksContainerV[a_stCellInfo.m_stIdx.z][a_stCellInfo.m_stIdx.y].Push(new List<CEObj>(oCellObjList));

			this.SubSetupCell(a_stCellInfo, a_stGridInfo);
		}

		/** 그리드 라인을 설정한다 */
		private void SetupGridLine() {
			this.SubSetupGridLine();
		}

		/** 엔진 객체 컴포넌트를 설정한다 */
		private void SetupEObjComponent(CEObjComponent a_oEObjComponent, CEComponent a_oOwner, CEController a_oController) {
			a_oEObjComponent.SetOwner(a_oOwner);
			a_oEObjComponent.Params.m_oCallbackDict.TryAdd(CEObjComponent.ECallback.ENGINE_OBJ_EVENT, this.OnReceiveEObjEvent);

			a_oController?.SetOwner(a_oEObjComponent);
			a_oController?.ExSetEnable(true, false);
		}

		/** 획득 타겟 정보를 설정한다 */
		private void SetupAcquireTargetInfos(CEObjComponent a_oEObjComponent, Dictionary<ulong, STTargetInfo> a_oOutAcquireTargetInfos) {
			// 아이템 일 경우
			if(a_oEObjComponent.Params.m_stBaseParams.m_oGameObjsPoolKey.Equals(KDefine.E_KEY_ITEM_OBJS_POOL)) {
				(a_oEObjComponent as CEItem).Params.m_stItemInfo.m_oAcquireTargetInfoDict.ExCopyTo(a_oOutAcquireTargetInfos, (_, a_stTargetInfo) => a_stTargetInfo);
			}
			// 적 객체 일 경우
			else if(a_oEObjComponent.Params.m_stBaseParams.m_oGameObjsPoolKey.Equals(KDefine.E_KEY_ENEMY_OBJ_OBJS_POOL)) {
				(a_oEObjComponent as CEObj).Params.m_stObjInfo.m_oAcquireTargetInfoDict.ExCopyTo(a_oOutAcquireTargetInfos, (_, a_stTargetInfo) => a_stTargetInfo);
			}
		}
		#endregion // 함수
	}

	/** 엔진 - 접근 */
	public partial class CEngine : CComponent {
		#region 함수
		/** 메인 카메라 위치를 반환한다 */
		public Vector3 GetMainCameraPos() {
			// 선택 플레이어가 존재 할 경우
			if(this.SelPlayerObj != null) {
				var stSize = this.CameraEpisodeSize.ExToLocal(this.Params.m_oObjRoot, false);
				var stOffset = KDefine.E_OFFSET_MAIN_CAMERA.ExToLocal(this.Params.m_oObjRoot, false);
				var stScreenSize = CSceneManager.ActiveSceneManager.ScreenSize.ExToLocal(this.Params.m_oObjRoot, false);

				float fPosX = Mathf.Clamp(this.SelPlayerObj.transform.localPosition.x, stSize.x / -KCDefine.B_VAL_2_REAL, stSize.x / KCDefine.B_VAL_2_REAL);
				float fPosY = Mathf.Clamp(this.SelPlayerObj.transform.localPosition.y + stOffset.y, (stSize.y / -KCDefine.B_VAL_2_REAL) - (stScreenSize.y / KCDefine.B_VAL_3_REAL), stSize.y / KCDefine.B_VAL_2_REAL);

				return new Vector3(fPosX, fPosY, CSceneManager.ActiveSceneMainCamera.transform.position.ExToLocal(this.Params.m_oObjRoot).z);
			}

			return CSceneManager.ActiveSceneMainCamera.transform.localPosition;
		}

		/** 최상단 셀 객체를 반환한다 */
		public CEObj GetTopCellObj(Vector3Int a_stIdx) {
			bool bIsValid = this.TryGetTopCellObj(a_stIdx, out CEObj oCellObj);
			CAccess.Assert(bIsValid);

			return oCellObj;
		}

		/** 최상단 셀 객체를 반환한다 */
		public bool TryGetTopCellObj(Vector3Int a_stIdx, out CEObj a_oOutCellObj) {
			a_oOutCellObj = this.CellObjListsContainer.ExIsValidIdx(a_stIdx.z) ?
				this.CellObjListsContainer[a_stIdx.z].ExGetVal(a_stIdx).LastOrDefault() : null;

			return a_oOutCellObj != null;
		}

		/** 구동 여부를 변경한다 */
		public void SetIsRunning(bool a_bIsRunning) {
			this.IsRunning = a_bIsRunning;
		}

		/** 플레이어 객체 자동 제어 여부를 변경한다 */
		public void SetIsAutoControlPlayerObj(bool a_bIsAutoControl) {
			this.SelPlayerObj.GetController<CEPlayerObjController>().SetIsAutoControl(a_bIsAutoControl);
		}

		/** 시간 간격 비율을 변경한다 */
		public void SetDeltaTimeScale(float a_fTimeScale) {
			this.DeltaTimeScale = a_fTimeScale.ExGetClampVal(KDefine.E_MIN_DELTA_TIME_SCALE, KDefine.E_MAX_DELTA_TIME_SCALE);
		}

		/** 상태를 변경한다 */
		public void SetState(EState a_eState, bool a_bIsForce = false) {
			// 강제 변경 모드 일 경우
			if(a_bIsForce) {
				this.State = a_eState;
			} else {
				this.State = (!m_oStateCheckerDict.TryGetValue(a_eState, out System.Func<bool> oStateChecker) || oStateChecker()) ? a_eState : this.State;
			}
		}

		/** 서브 상태를 변경한다 */
		public void SetSubState(ESubState a_eState, bool a_bIsForce = false) {
			// 강제 변경 모드 일 경우
			if(a_bIsForce) {
				this.SubState = a_eState;
			} else {
				this.SubState = (!m_oSubStateCheckerDict.TryGetValue(a_eState, out System.Func<bool> oStateChecker) || oStateChecker()) ? a_eState : this.SubState;
			}
		}

		/** 셀 객체를 탐색한다 */
		public CEObj FindCellObj(Vector3Int a_stIdx, EObjKinds a_eObjKinds) {
			// 셀 객체가 없을 경우
			if(!this.CellObjListsContainer.ExIsValidIdx(a_stIdx.z)) {
				return null;
			}

			var oCellObjList = this.CellObjListsContainer[a_stIdx.z].ExGetVal(a_stIdx);
			return oCellObjList?.ExGetVal((a_oCellObj) => a_oCellObj.Params.m_stObjInfo.m_eObjKinds == a_eObjKinds, null);
		}

		/** 적 객체를 탐색한다 */
		public CEObj FindEnemyObj(Vector3 a_stPos, float a_fDistance = float.MaxValue) {
			var oEnemyObj = this.EnemyObjListWrapper.ExGetVal(KCDefine.B_VAL_0_INT, null);

			for(int i = 1; i < this.EnemyObjListWrapper.Count; ++i) {
				float fDistance = (a_stPos - oEnemyObj.transform.localPosition).sqrMagnitude;
				oEnemyObj = fDistance.ExIsLessEquals((a_stPos - this.EnemyObjListWrapper[i].transform.localPosition).sqrMagnitude) ? oEnemyObj : this.EnemyObjListWrapper[i];
			}

			return (oEnemyObj != null && (a_stPos - oEnemyObj.transform.localPosition).sqrMagnitude.ExIsLessEquals(Mathf.Pow(a_fDistance, KCDefine.B_VAL_2_REAL))) ? oEnemyObj : null;
		}

		/** 셀 객체를 탐색한다 */
		public List<CEObj> FindCellObjs(Vector3Int a_stIdx, EObjKinds a_eObjKinds, List<CEObj> a_oOutCellObjList) {
			a_oOutCellObjList = a_oOutCellObjList ?? new List<CEObj>();

			// 셀 객체가 없을 경우
			if(!this.CellObjListsContainer.ExIsValidIdx(a_stIdx.z)) {
				return a_oOutCellObjList;
			}

			var oCellObjList = this.CellObjListsContainer[a_stIdx.z].ExGetVal(a_stIdx);
			return oCellObjList.ExGetVals((a_oCellObj) => a_oCellObj.Params.m_stObjInfo.m_eObjKinds == a_eObjKinds, a_oOutCellObjList);
		}

		/** 셀 객체를 탐색한다 */
		public List<CEObj> FindCellObjs(Vector3Int a_stIdx, Vector3Int a_stOffset, out Vector3Int a_stOutIdx) {
			// 인덱스가 유효 할 경우
			if(this.SelCellObjLists.ExIsValidIdx(a_stIdx)) {
				a_stOutIdx = a_stIdx;

				while(this.SelCellObjLists.ExIsValidIdx(a_stOutIdx)) {
					// 셀 객체가 존재 할 경우
					if(this.SelCellObjLists[a_stOutIdx.y, a_stOutIdx.x].ExIsValid()) {
						return this.SelCellObjLists[a_stOutIdx.y, a_stOutIdx.x];
					}

					a_stOutIdx += a_stOffset;
				}
			}

			a_stOutIdx = KCDefine.B_IDX_INVALID_3D;
			return KDefine.E_EMPTY_OBJ_LIST;
		}

		/** 적 객체를 탐색한다 */
		public List<CEObj> FindEnemyObjs(Vector3 a_stPos, List<CEObj> a_oOutEnemyObjList, float a_fDistance = float.MaxValue) {
			a_oOutEnemyObjList = a_oOutEnemyObjList ?? new List<CEObj>();

			for(int i = 0; i < this.EnemyObjListWrapper.Count; ++i) {
				float fDistance = (a_stPos - this.EnemyObjListWrapper[i].transform.localPosition).sqrMagnitude;

				// 범위 안에 존재 할 경우
				if(fDistance.ExIsLessEquals(Mathf.Pow(a_fDistance, KCDefine.B_VAL_2_REAL))) {
					a_oOutEnemyObjList.ExAddVal(this.EnemyObjListWrapper[i]);
				}
			}

			return a_oOutEnemyObjList;
		}

		/** 최상단 셀 객체를 탐색한다 */
		public CEObj FindTopCellObj(Vector3Int a_stIdx) {
			return this.FindTopCellObj(a_stIdx, EObjKinds.NONE);
		}

		/** 최상단 셀 객체를 탐색한다 */
		public CEObj FindTopCellObj(Vector3Int a_stIdx, EObjKinds a_eObjKinds) {
			var oCellObjList = CCollectionPoolManager.Inst.SpawnList<CEObj>();

			try {
				return (a_eObjKinds != EObjKinds.NONE) ? this.FindCellObjs(a_stIdx, a_eObjKinds, oCellObjList)?.LastOrDefault() : this.SelCellObjLists.ExGetVal(a_stIdx, null)?.LastOrDefault();
			} finally {
				CCollectionPoolManager.Inst.DespawnList(oCellObjList);
			}
		}

		/** 주변 셀 객체를 탐색한다 */
		public List<CEObj> FindAroundCellObjs(Vector3Int a_stIdx, Vector3Int a_stRange, EObjKinds a_eObjKinds, List<CEObj> a_oOutCellObjList) {
			a_oOutCellObjList = a_oOutCellObjList ?? new List<CEObj>();

			for(int i = 0; i < this.SelCellObjLists.GetLength(KCDefine.B_VAL_0_INT); ++i) {
				for(int j = 0; j < this.SelCellObjLists.GetLength(KCDefine.B_VAL_1_INT); ++j) {
					var oCellObjList = CCollectionPoolManager.Inst.SpawnList<CEObj>();

					try {
						var stIdx = new Vector3Int(j, i, this.SelGridInfoIdx);

						var oFindCellObjList = (a_eObjKinds != EObjKinds.NONE) ? 
							this.FindCellObjs(stIdx, a_eObjKinds, oCellObjList) : this.CellObjListsContainer.ExGetVal(a_stIdx.z)?.ExGetVal(stIdx);

						bool bIsValid01 = stIdx.x >= a_stIdx.x - a_stRange.x && stIdx.x <= a_stIdx.x + a_stRange.x;
						bool bIsValid02 = stIdx.y >= a_stIdx.y - a_stRange.y && stIdx.y <= a_stIdx.y + a_stRange.y;

						// 셀 객체가 존재 할 경우
						if(bIsValid01 && bIsValid02 && oFindCellObjList.ExIsValid()) {
							a_oOutCellObjList.ExAddVals(oFindCellObjList);
						}
					} finally {
						CCollectionPoolManager.Inst.DespawnList(oCellObjList);
					}
				}
			}

			return a_oOutCellObjList;
		}

		/** 주변 최상단 셀 객체를 탐색한다 */
		public List<CEObj> FindAroundTopCellObjs(Vector3Int a_stIdx, Vector3Int a_stRange, List<CEObj> a_oOutCellObjList) {
			return this.FindAroundTopCellObjs(a_stIdx, a_stRange, EObjKinds.NONE, a_oOutCellObjList);
		}

		/** 주변 최상단 셀 객체를 탐색한다 */
		public List<CEObj> FindAroundTopCellObjs(Vector3Int a_stIdx, Vector3Int a_stRange, EObjKinds a_eObjKinds, List<CEObj> a_oOutCellObjList) {
			a_oOutCellObjList = a_oOutCellObjList ?? new List<CEObj>();

			for(int i = 0; i < this.SelCellObjLists.GetLength(KCDefine.B_VAL_0_INT); ++i) {
				for(int j = 0; j < this.SelCellObjLists.GetLength(KCDefine.B_VAL_1_INT); ++j) {
					var stIdx = new Vector3Int(j, i, this.SelGridInfoIdx);
					var oCellObj = (a_eObjKinds != EObjKinds.NONE) ? this.FindTopCellObj(stIdx, a_eObjKinds) : this.FindTopCellObj(stIdx);

					bool bIsValid01 = stIdx.x >= a_stIdx.x - a_stRange.x && stIdx.x <= a_stIdx.x + a_stRange.x;
					bool bIsValid02 = stIdx.y >= a_stIdx.y - a_stRange.y && stIdx.y <= a_stIdx.y + a_stRange.y;

					// 셀 객체가 존재 할 경우
					if(bIsValid01 && bIsValid02 && oCellObj != null) {
						a_oOutCellObjList.ExAddVal(oCellObj);
					}
				}
			}

			return a_oOutCellObjList;
		}
		#endregion // 함수
	}

	/** 엔진 - 팩토리 */
	public partial class CEngine : CComponent {
		#region 함수
		/** 아이템을 생성한다 */
		public CEItem CreateItem(STItemInfo a_stItemInfo, CItemTargetInfo a_oItemTargetInfo, CEObjComponent a_oOwner = null, bool a_bIsEnableController = true) {
			var oItem = CSceneManager.ActiveSceneGameObjsPoolManager.SpawnGameObj<CEItem>(KDefine.E_OBJ_N_ITEM, KDefine.E_KEY_ITEM_OBJS_POOL);
			var oController = a_bIsEnableController ? oItem.gameObject.ExAddComponent<CEItemController>() : null;

			oItem.Init(CEItem.MakeParams(this, a_stItemInfo, a_oItemTargetInfo, oController, KDefine.E_KEY_ITEM_OBJS_POOL));
			oItem.ExSetTag(KCDefine.B_TAG_ITEM);

			this.SetupEObjComponent(oItem, a_oOwner, oController);
			
			oController?.Init(CEItemController.MakeParams(this));
			return oItem;
		}

		/** 스킬을 생성한다 */
		public CESkill CreateSkill(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo, CEObjComponent a_oOwner = null, bool a_bIsEnableController = true) {
			var oSkill = CSceneManager.ActiveSceneGameObjsPoolManager.SpawnGameObj<CESkill>(KDefine.E_OBJ_N_SKILL, KDefine.E_KEY_SKILL_OBJS_POOL);
			var oController = a_bIsEnableController ? oSkill.gameObject.ExAddComponent<CESkillController>() : null;

			oSkill.Init(CESkill.MakeParams(this, a_stSkillInfo, a_oSkillTargetInfo, oController, KDefine.E_KEY_SKILL_OBJS_POOL));
			oSkill.ExSetTag(KCDefine.B_TAG_SKILL);

			this.SetupEObjComponent(oSkill, a_oOwner, oController);

			oController?.Init(CESkillController.MakeParams(this));
			return oSkill;
		}

		/** 객체를 생성한다 */
		public CEObj CreateObj(STObjInfo a_stObjInfo, CObjTargetInfo a_oObjTargetInfo, CEObjComponent a_oOwner = null, bool a_bIsEnableController = true) {
			var oObj = CSceneManager.ActiveSceneGameObjsPoolManager.SpawnGameObj<CEObj>(KDefine.E_OBJ_N_OBJ, KDefine.E_KEY_OBJ_OBJS_POOL);
			var oController = a_bIsEnableController ? oObj.gameObject.ExAddComponent<CEObjController>() : null;

			oObj.Init(CEObj.MakeParams(this, a_stObjInfo, a_oObjTargetInfo, oController, KDefine.E_KEY_OBJ_OBJS_POOL));
			oObj.ExSetTag(KCDefine.B_TAG_OBJ);

			this.SetupEObjComponent(oObj, a_oOwner, oController);

			oController?.Init(CEObjController.MakeParams(this));
			return oObj;
		}

		/** 효과를 생성한다 */
		public CEFX CreateFX(STFXInfo a_stFXInfo, CEObjComponent a_oOwner = null, bool a_bIsEnableController = true) {
			var oFX = CSceneManager.ActiveSceneGameObjsPoolManager.SpawnGameObj<CEFX>(KDefine.E_OBJ_N_FX, KDefine.E_KEY_FX_OBJS_POOL);
			var oController = a_bIsEnableController ? oFX.gameObject.ExAddComponent<CEFXController>() : null;

			oFX.Init(CEFX.MakeParams(this, a_stFXInfo, oController, KDefine.E_KEY_FX_OBJS_POOL));
			oFX.ExSetTag(KCDefine.B_TAG_FX);

			this.SetupEObjComponent(oFX, a_oOwner, oController);

			oController?.Init(CEFXController.MakeParams(this));
			return oFX;
		}

		/** 셀 객체를 생성한다 */
		public CEObj CreateCellObj(STObjInfo a_stObjInfo, STGridInfo a_stGridInfo, CObjTargetInfo a_oObjTargetInfo, CEObjComponent a_oOwner = null, bool a_bIsEnableController = true) {
			var oObj = CSceneManager.ActiveSceneGameObjsPoolManager.SpawnGameObj<CEObj>(KDefine.E_OBJ_N_CELL_OBJ, KDefine.E_KEY_CELL_OBJ_OBJS_POOL);
			var oController = a_bIsEnableController ? oObj.gameObject.ExAddComponent<CECellObjController>() : null;

			oObj.Init(CEObj.MakeParams(this, a_stObjInfo, a_oObjTargetInfo, oController, KDefine.E_KEY_CELL_OBJ_OBJS_POOL));
			oObj.ExSetTag(KCDefine.B_TAG_CELL);
			oObj.gameObject.ExSetParent(m_oCellObjRootList.ExGetVal(a_stGridInfo.m_nIdx, null));

			this.SetupEObjComponent(oObj, a_oOwner, oController);

			oController?.Init(CECellObjController.MakeParams(this));
			return oObj;
		}

		/** 플레이어 객체를 생성한다 */
		public CEObj CreatePlayerObj(STObjInfo a_stObjInfo, CObjTargetInfo a_oObjTargetInfo, CEObjComponent a_oOwner = null, bool a_bIsEnableController = true) {
			var oObj = CSceneManager.ActiveSceneGameObjsPoolManager.SpawnGameObj<CEObj>(KDefine.E_OBJ_N_PLAYER_OBJ, KDefine.E_KEY_PLAYER_OBJ_OBJS_POOL);
			var oController = a_bIsEnableController ? oObj.gameObject.ExAddComponent<CEPlayerObjController>() : null;

			oObj.Init(CEObj.MakeParams(this, a_stObjInfo, a_oObjTargetInfo, oController, KDefine.E_KEY_PLAYER_OBJ_OBJS_POOL));
			oObj.ExSetTag(KCDefine.B_TAG_PLAYER);

			this.SetupEObjComponent(oObj, a_oOwner, oController);

			oController?.Init(CEPlayerObjController.MakeParams(this));
			return oObj;
		}

		/** 적 객체를 생성한다 */
		public CEObj CreateEnemyObj(STObjInfo a_stObjInfo, CObjTargetInfo a_oObjTargetInfo, CEObjComponent a_oOwner = null, bool a_bIsEnableController = true) {
			var oObj = CSceneManager.ActiveSceneGameObjsPoolManager.SpawnGameObj<CEObj>(KDefine.E_OBJ_N_ENEMY_OBJ, KDefine.E_KEY_ENEMY_OBJ_OBJS_POOL);
			var oController = a_bIsEnableController ? oObj.gameObject.ExAddComponent<CEEnemyObjController>() : null;

			oObj.Init(CEObj.MakeParams(this, a_stObjInfo, a_oObjTargetInfo, oController, KDefine.E_KEY_ENEMY_OBJ_OBJS_POOL));
			oObj.ExSetTag(KCDefine.B_TAG_NON_PLAYER);

			this.SetupEObjComponent(oObj, a_oOwner, oController);

			oController?.Init(CEEnemyObjController.MakeParams(this));
			return oObj;
		}

		/** 엔진 객체 컴포넌트를 제거한다 */
		public void RemoveEObjComponent(CEObjComponent a_oEObjComponent) {
			switch(a_oEObjComponent.Params.m_stBaseParams.m_oGameObjsPoolKey) {
				case KDefine.E_KEY_ITEM_OBJS_POOL: this.RemoveItem(a_oEObjComponent as CEItem); break;
				case KDefine.E_KEY_SKILL_OBJS_POOL: this.RemoveSkill(a_oEObjComponent as CESkill); break;
				case KDefine.E_KEY_FX_OBJS_POOL: this.RemoveFX(a_oEObjComponent as CEFX); break;
				case KDefine.E_KEY_OBJ_OBJS_POOL: this.RemoveObj(a_oEObjComponent as CEObj); break;
				case KDefine.E_KEY_CELL_OBJ_OBJS_POOL: this.RemoveCellObj(a_oEObjComponent as CEObj); break;
				case KDefine.E_KEY_PLAYER_OBJ_OBJS_POOL: this.RemovePlayerObj(a_oEObjComponent as CEObj); break;
				case KDefine.E_KEY_ENEMY_OBJ_OBJS_POOL: this.RemoveEnemyObj(a_oEObjComponent as CEObj); break;
			}
		}

		/** 아이템을 제거한다 */
		private void RemoveItem(CEItem a_oItem, float a_fDelay = KCDefine.B_VAL_0_REAL, bool a_bIsAssert = true) {
			CAccess.Assert(!a_bIsAssert || (a_oItem != null && a_oItem.Params.m_stBaseParams.m_stBaseParams.m_oGameObjsPoolKey.ExIsValid()));

			// 아이템이 존재 할 경우
			if(a_oItem != null && a_oItem.Params.m_stBaseParams.m_stBaseParams.m_oGameObjsPoolKey.ExIsValid()) {
				this.ItemListWrapper.ExRemoveVal(a_oItem);
				CSceneManager.ActiveSceneGameObjsPoolManager.DespawnGameObj(a_oItem.Params.m_stBaseParams.m_stBaseParams.m_oGameObjsPoolKey, a_oItem.gameObject, a_fDelay);
			}
		}

		/** 스킬을 제거한다 */
		private void RemoveSkill(CESkill a_oSkill, float a_fDelay = KCDefine.B_VAL_0_REAL, bool a_bIsAssert = true) {
			CAccess.Assert(!a_bIsAssert || (a_oSkill != null && a_oSkill.Params.m_stBaseParams.m_stBaseParams.m_oGameObjsPoolKey.ExIsValid()));

			// 스킬이 존재 할 경우
			if(a_oSkill != null && a_oSkill.Params.m_stBaseParams.m_stBaseParams.m_oGameObjsPoolKey.ExIsValid()) {
				this.SkillListWrapper.ExRemoveVal(a_oSkill);
				CSceneManager.ActiveSceneGameObjsPoolManager.DespawnGameObj(a_oSkill.Params.m_stBaseParams.m_stBaseParams.m_oGameObjsPoolKey, a_oSkill.gameObject, a_fDelay);
			}
		}

		/** 객체를 제거한다 */
		private void RemoveObj(CEObj a_oObj, float a_fDelay = KCDefine.B_VAL_0_REAL, bool a_bIsAssert = true) {
			CAccess.Assert(!a_bIsAssert || (a_oObj != null && a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oGameObjsPoolKey.ExIsValid()));

			// 객체가 존재 할 경우
			if(a_oObj != null && a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oGameObjsPoolKey.ExIsValid()) {
				this.ObjListWrapper.ExRemoveVal(a_oObj);
				CSceneManager.ActiveSceneGameObjsPoolManager.DespawnGameObj(a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oGameObjsPoolKey, a_oObj.gameObject, a_fDelay);
			}
		}

		/** 효과를 제거한다 */
		private void RemoveFX(CEFX a_oFX, float a_fDelay = KCDefine.B_VAL_0_REAL, bool a_bIsAssert = true) {
			CAccess.Assert(!a_bIsAssert || (a_oFX != null && a_oFX.Params.m_stBaseParams.m_stBaseParams.m_oGameObjsPoolKey.ExIsValid()));

			// 효과가 존재 할 경우
			if(a_oFX != null && a_oFX.Params.m_stBaseParams.m_stBaseParams.m_oGameObjsPoolKey.ExIsValid()) {
				this.FXListWrapper.ExRemoveVal(a_oFX);
				CSceneManager.ActiveSceneGameObjsPoolManager.DespawnGameObj(a_oFX.Params.m_stBaseParams.m_stBaseParams.m_oGameObjsPoolKey, a_oFX.gameObject, a_fDelay);
			}
		}

		/** 셀 객체를 제거한다 */
		private void RemoveCellObj(CEObj a_oObj, float a_fDelay = KCDefine.B_VAL_0_REAL, bool a_bIsAssert = true) {
			var oCellObjList = (a_oObj != null) ? this.CellObjListsContainer.ExGetVal(a_oObj.GetController<CECellObjController>().CellIdx.z)?.ExGetVal(a_oObj.GetController<CECellObjController>().CellIdx) : null;
			CAccess.Assert(!a_bIsAssert || (a_oObj != null && oCellObjList != null && a_oObj.GetController<CECellObjController>().CellIdx.ExIsValidIdx() && a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oGameObjsPoolKey.ExIsValid()));

			// 셀 객체가 존재 할 경우
			if(a_oObj != null && oCellObjList != null && a_oObj.GetController<CECellObjController>().CellIdx.ExIsValidIdx() && a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oGameObjsPoolKey.ExIsValid()) {
				oCellObjList.ExRemoveVal(a_oObj);
				CSceneManager.ActiveSceneGameObjsPoolManager.DespawnGameObj(a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oGameObjsPoolKey, a_oObj.gameObject, a_fDelay);
			}
		}

		/** 플레이어 객체를 제거한다 */
		private void RemovePlayerObj(CEObj a_oObj, float a_fDelay = KCDefine.B_VAL_0_REAL, bool a_bIsAssert = true) {
			CAccess.Assert(!a_bIsAssert || (a_oObj != null && a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oGameObjsPoolKey.ExIsValid()));

			// 플레이어 객체가 존재 할 경우
			if(a_oObj != null && a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oGameObjsPoolKey.ExIsValid()) {
				this.PlayerObjListWrapper.ExRemoveVal(a_oObj);
				CSceneManager.ActiveSceneGameObjsPoolManager.DespawnGameObj(a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oGameObjsPoolKey, a_oObj.gameObject, a_fDelay);
			}
		}

		/** 적 객체를 제거한다 */
		private void RemoveEnemyObj(CEObj a_oObj, float a_fDelay = KCDefine.B_VAL_0_REAL, bool a_bIsAssert = true) {
			CAccess.Assert(!a_bIsAssert || (a_oObj != null && a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oGameObjsPoolKey.ExIsValid()));

			// 적 객체가 존재 할 경우
			if(a_oObj != null && a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oGameObjsPoolKey.ExIsValid()) {
				this.EnemyObjListWrapper.ExRemoveVal(a_oObj);
				CSceneManager.ActiveSceneGameObjsPoolManager.DespawnGameObj(a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oGameObjsPoolKey, a_oObj.gameObject, a_fDelay);
			}
		}
		#endregion // 함수

		#region 클래스 함수
		/** 매개 변수를 생성한다 */
		public static STParams MakeParams(GameObject a_oItemRoot, GameObject a_oSkillRoot, GameObject a_oObjRoot, GameObject a_oFXRoot, Dictionary<ECallback, System.Action<CEngine>> a_oCallbackDict01 = null, Dictionary<ECallback, System.Action<CEngine, Dictionary<ulong, STTargetInfo>>> a_oCallbackDict02 = null, Dictionary<ECallback, System.Action<CEngine, CEObjComponent, EEngineObjEvent, string>> a_oCallbackDict03 = null) {
			return new STParams() {
				m_oItemRoot = a_oItemRoot,
				m_oSkillRoot = a_oSkillRoot,
				m_oObjRoot = a_oObjRoot,
				m_oFXRoot = a_oFXRoot,
				m_oCallbackDictA = a_oCallbackDict01 ?? new Dictionary<ECallback, System.Action<CEngine>>(),
				m_oCallbackDictB = a_oCallbackDict02 ?? new Dictionary<ECallback, System.Action<CEngine, Dictionary<ulong, STTargetInfo>>>(),
				m_oCallbackDictC = a_oCallbackDict03 ?? new Dictionary<ECallback, System.Action<CEngine, CEObjComponent, EEngineObjEvent, string>>()
			};
		}
		#endregion // 클래스 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
