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
	/** 서브 엔진 */
	public partial class CEngine : CComponent {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

		#region 변수

		#endregion // 변수

		#region 프로퍼티

		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		private void SubAwake() {
			// Do Something
		}

		/** 초기화 */
		private void SubInit() {
			// Do Something
		}

		/** 제거되었을 경우 */
		private void SubOnDestroy() {
			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsRunningApp) {
					// Do Something
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CEngine.SubOnDestroy Exception: {oException.Message}");
			}
		}

		/** 상태를 갱신한다 */
		private void SubOnUpdate(float a_fTimeDelta) {
			// 앱이 실행 중 일 경우
			if(CSceneManager.IsRunningApp) {
				// Do Something
			}
		}

		/** 상태를 갱신한다 */
		private void SubOnLateUpdate(float a_fTimeDelta) {
			// 앱이 실행 중 일 경우
			if(CSceneManager.IsRunningApp) {
				// Do Something
			}
		}

		/** 게임 아이템을 적용한다 */
		private void ApplyGameItem(STItemInfo a_stItemInfo, CItemTargetInfo a_oItemTargetInfo) {
			// Do Something
		}

		/** 부스터 아이템을 적용한다 */
		private void ApplyBoosterItem(STItemInfo a_stItemInfo, CItemTargetInfo a_oItemTargetInfo) {
			// Do Something
		}

		/** 클리어 상태를 처리한다 */
		private void HandleClearState() {
			// 종료 상태가 아닐 경우
			if(!this.IsFinish) {
				this.IsFinish = true;

				// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능) {
#if DISABLE_THIS
				this.Params.m_oCallbackDictA.GetValueOrDefault(ECallback.CLEAR)?.Invoke(this);
#endif // #if DISABLE_THIS
				// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능) }
			}
		}

		/** 클리어 실패 상태를 처리한다 */
		private void HandleClearFailState() {
			// 종료 상태가 아닐 경우
			if(!this.IsFinish) {
				this.IsFinish = true;

				// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능) {
#if DISABLE_THIS
				this.Params.m_oCallbackDictA.GetValueOrDefault(ECallback.CLEAR_FAIL)?.Invoke(this);
#endif // #if DISABLE_THIS
				// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능) }
			}
		}

		/** 플레이 서브 상태를 처리한다 */
		private void HandlePlaySubState(float a_fTimeDelta) {
			global::Func.UpdateComponents(this.ItemListWrapper, a_fTimeDelta);
			global::Func.UpdateComponents(this.SkillListWrapper, a_fTimeDelta);
			global::Func.UpdateComponents(this.FXListWrapper, a_fTimeDelta);
			global::Func.UpdateComponents(this.ObjListWrapper, a_fTimeDelta);
			global::Func.UpdateComponents(this.PlayerObjListWrapper, a_fTimeDelta);
			global::Func.UpdateComponents(this.EnemyObjListWrapper, a_fTimeDelta);

			// 실행 중 일 경우
			if(this.IsRunning) {
				var oNumEnemyObjDict = CManagerPoolCollection.Inst.SpawnDict<EObjKinds, int>();

				try {
					for(int i = 0; i < this.EnemyObjListWrapper.Count; ++i) {
						int nNumEnemyObjs = oNumEnemyObjDict.ExGetVal(this.EnemyObjListWrapper[i].Params.m_stObjInfo.m_eObjKinds);
						oNumEnemyObjDict.ExReplaceVal(this.EnemyObjListWrapper[i].Params.m_stObjInfo.m_eObjKinds, nNumEnemyObjs + KCDefine.B_VAL_1_INT);
					}

					foreach(var stKeyVal in CGameInfoStorage.Inst.PlayEpisodeInfo.m_oEnemyObjTargetInfoDict) {
						// 적 객체 생성이 가능 할 경우
						if(oNumEnemyObjDict.ExGetVal((EObjKinds)stKeyVal.Value.Kinds) < stKeyVal.Value.m_stValInfo01.m_dmVal && this.EnemyObjListWrapper.Count < CGameInfoStorage.Inst.PlayEpisodeInfo.m_nMaxNumEnemyObjs) {
							float fPosX = Random.Range(this.EpisodeSize.x / -KCDefine.B_VAL_2_REAL, this.EpisodeSize.x / KCDefine.B_VAL_2_REAL);
							float fPosY = Random.Range(this.EpisodeSize.y / -KCDefine.B_VAL_2_REAL, this.EpisodeSize.y / KCDefine.B_VAL_2_REAL);

							var oEnemyObj = this.CreateEnemyObj(CObjInfoTable.Inst.GetObjInfo((EObjKinds)stKeyVal.Value.Kinds), null);
							oEnemyObj.transform.localPosition = new Vector3(fPosX, fPosY, fPosY / this.EpisodeSize.y);

							this.EnemyObjListWrapper.ExAddVal(oEnemyObj);
						}
					}
				} finally {
					CManagerPoolCollection.Inst.DespawnDict(oNumEnemyObjDict);
				}
			}
		}

		/** 정지 서브 상태를 처리한다 */
		private void HandlePauseSubState(float a_fTimeDelta) {
			// Do Something
		}

		/** 회피 엔진 객체 이벤트를 처리한다 */
		private void HandleAvoidEObjEvent(CEObjComponent a_oSender, string a_oParams) {
			// Do Something
		}

		/** 피해 엔진 객체 이벤트를 처리한다 */
		private void HandleDamageEObjEvent(CEObjComponent a_oSender, string a_oParams) {
			// Do Something
		}

		/** 치명 피해 엔진 객체 이벤트를 처리한다 */
		private void HandleCriticalDamageEObjEvent(CEObjComponent a_oSender, string a_oParams) {
			// Do Something
		}

		/** 터치 시작 이벤트를 처리한다 */
		private void HandleTouchBeginEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 정지되었을 경우
			if(!this.IsRunning) {
				return;
			}

			var stPos = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot,
				CSceneManager.ActiveSceneManager.ScreenSize);

			var stIdx = stPos.ExToIdx(this.SelGridInfo.m_stPivotPos, Access.CellSize);
			var oCellObjLists = this.CellObjListsContainer.ExGetVal(stIdx.z);

			// 인덱스가 유효하지 않을 경우
			if(oCellObjLists == null || oCellObjLists.ExIsValidIdx(stIdx)) {
				return;
			}
		}

		/** 터치 이동 이벤트를 처리한다 */
		private void HandleTouchMoveEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 정지되었을 경우
			if(!this.IsRunning) {
				return;
			}

			var stPos = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot,
				CSceneManager.ActiveSceneManager.ScreenSize);

			var stIdx = stPos.ExToIdx(this.SelGridInfo.m_stPivotPos, Access.CellSize);
			var oCellObjLists = this.CellObjListsContainer.ExGetVal(stIdx.z);

			// 인덱스가 유효하지 않을 경우
			if(oCellObjLists == null || oCellObjLists.ExIsValidIdx(stIdx)) {
				return;
			}
		}

		/** 터치 종료 이벤트를 처리한다 */
		private void HandleTouchEndEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 정지되었을 경우
			if(!this.IsRunning) {
				return;
			}

			var stPos = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot,
				CSceneManager.ActiveSceneManager.ScreenSize);

			var stIdx = stPos.ExToIdx(this.SelGridInfo.m_stPivotPos, Access.CellSize);
			var oCellObjLists = this.CellObjListsContainer.ExGetVal(stIdx.z);

			// 인덱스가 유효하지 않을 경우
			if(oCellObjLists == null || oCellObjLists.ExIsValidIdx(stIdx)) {
				return;
			}
		}
		#endregion // 함수

		#region 조건부 함수
#if UNITY_EDITOR
		/** 기즈모를 그린다 */
		public virtual void OnDrawGizmos() {
			// 메인 카메라가 존재 할 경우
			if(CSceneManager.IsExistsMainCamera) {
				// Do Something
			}
		}
#endif // #if UNITY_EDITOR
#endregion // 조건부 함수
	}

	/** 서브 엔진 - 설정 */
	public partial class CEngine : CComponent {
		#region 함수
		/** 엔진을 설정한다 */
		private void SubSetup() {
			// Do Something
		}

		/** 레벨을 설정한다 */
		private void SubSetupLevel() {
			// 레벨 정보가 존재 할 경우
			if(CGameInfoStorage.Inst.PlayLevelInfo != null) {
				// Do Something
			}
		}

		/** 셀을 설정한다 */
		private void SubSetupCell(STCellInfo a_stCellInfo, STGridInfo a_stGridInfo) {
			int nIdx = KCDefine.B_VAL_0_INT;

			for(int i = 0; i < a_stCellInfo.m_oCellObjInfoList.Count; ++i) {
				// 셀 설정이 불가능 할 경우
				if(!a_stCellInfo.m_oCellObjInfoList[i].ObjKinds.ExIsValid()) {
					continue;
				}

				var stPos = a_stGridInfo.m_stPivotPos + a_stCellInfo.m_stIdx.ExToPos(Vector3.zero, Access.CellSize);
				var stCenterPos = a_stGridInfo.m_stPivotPos + a_stCellInfo.m_stIdx.ExToPos(Access.CellCenterOffset, Access.CellSize);

				// 셀 객체가 없을 경우
				if(!this.CellObjListsContainer.ExIsValidIdx(a_stCellInfo.m_stIdx.z)) {
					continue;
				}

				var oCellObjList = this.CellObjListsContainer[a_stCellInfo.m_stIdx.z].ExGetVal(a_stCellInfo.m_stIdx);

				// 셀 객체가 없을 경우
				if(!oCellObjList.ExIsValidIdx(nIdx)) {
					continue;
				}

				nIdx += KCDefine.B_VAL_1_INT;
			}
		}

		/** 그리드 라인을 설정한다 */
		private void SubSetupGridLine() {
			// Do Something
		}
		#endregion // 함수
	}

	/** 서브 엔진 - 접근 */
	public partial class CEngine : CComponent {
		#region 함수
		/** 클리어 여부를 검사한다 */
		public bool IsClear() {
			// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능) {
#if DISABLE_THIS
			bool bIsClear01 = this.ClearTargetInfoDict.All((a_stKeyVal) => a_stKeyVal.Value.m_stValInfo01.m_dmVal <= KCDefine.B_VAL_0_INT);
#endif // #if DISABLE_THIS
			// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능) }

			return false;
		}

		/** 클리어 실패 여부를 검사한다 */
		public bool IsClearFail() {
			// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능) {
#if DISABLE_THIS
			bool bIsClearFail01 = this.SelPlayerObj != null && this.SelPlayerObj.AbilityValDictWrapper.m_oDictA.ExGetAbilityVal(EAbilityKinds.STAT_ABILITY_HP_01) <= KCDefine.B_VAL_0_INT;
#endif // #if DISABLE_THIS
			// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능) }

			return false;
		}
		#endregion // 함수
	}

	/** 서브 엔진 - 팩토리 */
	public partial class CEngine : CComponent {
		#region 함수

		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
