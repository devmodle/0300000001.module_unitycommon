#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 */
	public partial class CEngine : CComponent {
		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			#region 추가
			this.SubAwakeSetup();
			#endregion			// 추가
		}

		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			this.Params = a_stParams;

			this.SetupEngine();
			this.SetupLevel();
			this.SetupGridLine();
			
			#region 추가
			this.SubInit();
			#endregion			// 추가
		}

		/** 상태를 리셋한다 */
		public override void Reset() {
			base.Reset();
			this.BoolDict.ExReplaceVal(EKey.IS_RUNNING, false);

			#region 추가
			this.SubReset();
			#endregion			// 추가
		}
		#endregion			// 함수
	}

	/** 서브 엔진 */
	public partial class CEngine : CComponent {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

		/** 상태 */
		public enum EState {
			NONE = -1,
			PLAY,
			PAUSE,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		private Dictionary<EState, System.Func<bool>> m_oStateCheckerDict = new Dictionary<EState, System.Func<bool>>();
		#endregion			// 변수

		#region 프로퍼티
		public EState State { get; private set; } = EState.NONE;
		#endregion			// 프로퍼티
		
		#region 함수
		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning && this.BoolDict.GetValueOrDefault(EKey.IS_RUNNING)) {
				switch(this.State) {
					case EState.PLAY: this.HandlePlayState(a_fDeltaTime); break;
					case EState.PAUSE: this.HandlePauseState(a_fDeltaTime); break;
				}

				var stEpisodeInfo = global::Access.GetEpisodeInfo(this.Params.m_oLevelInfo.m_stIDInfo.m_nID01, this.Params.m_oLevelInfo.m_stIDInfo.m_nID02, this.Params.m_oLevelInfo.m_stIDInfo.m_nID03);
				var stEpisodeSize = new Vector3(Mathf.Clamp(stEpisodeInfo.m_stSize.x, KCDefine.B_VAL_0_REAL, stEpisodeInfo.m_stSize.x - KCDefine.B_SCREEN_SIZE.x), Mathf.Clamp(stEpisodeInfo.m_stSize.y, KCDefine.B_VAL_0_REAL, stEpisodeInfo.m_stSize.y - KCDefine.B_SCREEN_SIZE.y), stEpisodeInfo.m_stSize.z) * (KCDefine.B_UNIT_SCALE * CAccess.ResolutionScale);
				var stMainCameraPos = new Vector3(Mathf.Clamp(this.PlayerObjList[KCDefine.B_VAL_0_INT].transform.position.x, stEpisodeSize.x / -KCDefine.B_VAL_2_REAL, stEpisodeSize.x / KCDefine.B_VAL_2_REAL), Mathf.Clamp(this.PlayerObjList[KCDefine.B_VAL_0_INT].transform.position.y + KDefine.E_OFFSET_MAIN_CAMERA, stEpisodeSize.y / -KCDefine.B_VAL_2_REAL, stEpisodeSize.y / KCDefine.B_VAL_2_REAL), CSceneManager.ActiveSceneMainCamera.transform.position.z);
				
				CSceneManager.ActiveSceneMainCamera.transform.position = Vector3.Lerp(CSceneManager.ActiveSceneMainCamera.transform.position, stMainCameraPos, a_fDeltaTime * KCDefine.B_VAL_9_REAL);
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
				CFunc.ShowLogWarning($"CEngine.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 플레이어 객체 자동 제어 여부를 변경한다 */
		public void SetEnablePlayerObjAutoControl(bool a_bIsAutoControl) {
			this.PlayerObjList[KCDefine.B_VAL_0_INT].GetController<CEPlayerObjController>().IsAutoControl = a_bIsAutoControl;
		}

		/** 플레이어 객체 이동을 처리한다 */
		public void MovePlayerObj(Vector3 a_stDirection) {
			this.PlayerObjList[KCDefine.B_VAL_0_INT].GetController<CEPlayerObjController>().Move(a_stDirection);
		}
		
		/** 플레이어 객체 스킬을 적용한다 */
		public void ApplyPlayerObjSkill(CSkillTargetInfo a_oSkillTargetInfo) {
			this.PlayerObjList[KCDefine.B_VAL_0_INT].GetController<CEPlayerObjController>().ApplySkill(a_oSkillTargetInfo);
		}

		/** 초기화한다 */
		private void SubInit() {
			var stObjInfo = CObjInfoTable.Inst.GetObjInfo(EObjKinds.PLAYABLE_COMMON_CHARACTER_01);
			this.PlayerObjList.ExAddVal(this.CreatePlayerObj(stObjInfo, CUserInfoStorage.Inst.GetCharacterUserInfo(CGameInfoStorage.Inst.PlayCharacterID), null));

			CSceneManager.ActiveSceneMainCamera.transform.position = new Vector3(this.PlayerObjList[KCDefine.B_VAL_0_INT].transform.position.x, this.PlayerObjList[KCDefine.B_VAL_0_INT].transform.position.y + (KDefine.E_OFFSET_MAIN_CAMERA * KCDefine.B_UNIT_SCALE * CAccess.ResolutionScale), CSceneManager.ActiveSceneMainCamera.transform.position.z);
		}

		/** 상태를 리셋한다 */
		private void SubReset() {
			this.SetState(EState.NONE);
		}

		/** 플레이 상태를 처리한다 */
		private void HandlePlayState(float a_fDeltaTime) {
			CFunc.UpdateComponents(this.ItemList, a_fDeltaTime);
			CFunc.UpdateComponents(this.SkillList, a_fDeltaTime);
			CFunc.UpdateComponents(this.FXList, a_fDeltaTime);
			CFunc.UpdateComponents(this.PlayerObjList, a_fDeltaTime);
			CFunc.UpdateComponents(this.EnemyObjList, a_fDeltaTime);

			/* FIXME: 임시 주석 처리
			var stEpisodeInfo = global::Access.GetEpisodeInfo(this.Params.m_oLevelInfo.m_stIDInfo.m_nID01, this.Params.m_oLevelInfo.m_stIDInfo.m_nID02, this.Params.m_oLevelInfo.m_stIDInfo.m_nID03);
			var oNumEnemyObjsDict = CCollectionManager.Inst.SpawnDict<EObjKinds, int>();

			try {
				for(int i = 0; i < this.EnemyObjList.Count; ++i) {
					int nNumEnemyObjs = oNumEnemyObjsDict.GetValueOrDefault(this.EnemyObjList[i].Params.m_stObjInfo.m_eObjKinds);
					oNumEnemyObjsDict.ExReplaceVal(this.EnemyObjList[i].Params.m_stObjInfo.m_eObjKinds, nNumEnemyObjs + KCDefine.B_VAL_1_INT);
				}

				foreach(var stKeyVal in stEpisodeInfo.m_oEnemyObjTargetInfoDict) {
					// 적 객체 생성이 가능 할 경우
					if(oNumEnemyObjsDict.GetValueOrDefault((EObjKinds)stKeyVal.Value.m_nKinds) < stKeyVal.Value.m_stValInfo01.m_dmVal && this.EnemyObjList.Count < stEpisodeInfo.m_nMaxNumEnemyObjs) {
						float fPosX = Random.Range(stEpisodeInfo.m_stSize.x / -KCDefine.B_VAL_2_REAL, stEpisodeInfo.m_stSize.x / KCDefine.B_VAL_2_REAL);
						float fPosY = Random.Range(stEpisodeInfo.m_stSize.y / -KCDefine.B_VAL_2_REAL, stEpisodeInfo.m_stSize.y / KCDefine.B_VAL_2_REAL);

						var oEnemyObj = this.CreateEnemyObj(CObjInfoTable.Inst.GetObjInfo((EObjKinds)stKeyVal.Value.m_nKinds), null, null);
						oEnemyObj.transform.localPosition = new Vector3(fPosX, fPosY, fPosY / stEpisodeInfo.m_stSize.y);

						this.EnemyObjList.ExAddVal(oEnemyObj);
					}
				}
			} finally {
				CCollectionManager.Inst.DespawnDict(oNumEnemyObjsDict);
			}
			*/
		}

		/** 정지 상태를 처리한다 */
		private void HandlePauseState(float a_fDeltaTime) {
			// Do Something
		}

		/** 터치 시작 이벤트를 처리한다 */
		private void HandleTouchBeginEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 구동 모드 일 경우
			if(this.BoolDict.GetValueOrDefault(EKey.IS_RUNNING)) {
				var stIdx = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot).ExToIdx(this.GridInfoDict.GetValueOrDefault(EKey.SEL_GRID_INFO).m_stPivotPos, KDefine.E_SIZE_CELL);
			}
		}

		/** 터치 이동 이벤트를 처리한다 */
		private void HandleTouchMoveEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 구동 모드 일 경우
			if(this.BoolDict.GetValueOrDefault(EKey.IS_RUNNING)) {
				var stIdx = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot).ExToIdx(this.GridInfoDict.GetValueOrDefault(EKey.SEL_GRID_INFO).m_stPivotPos, KDefine.E_SIZE_CELL);
			}
		}

		/** 터치 종료 이벤트를 처리한다 */
		private void HandleTouchEndEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 구동 모드 일 경우
			if(this.BoolDict.GetValueOrDefault(EKey.IS_RUNNING)) {
				var stIdx = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot).ExToIdx(this.GridInfoDict.GetValueOrDefault(EKey.SEL_GRID_INFO).m_stPivotPos, KDefine.E_SIZE_CELL);
			}
		}
		#endregion			// 함수

		#region 조건부 함수
#if UNITY_EDITOR
		/** 기즈모를 그린다 */
		public virtual void OnDrawGizmos() {
			// 메인 카메라가 존재 할 경우
			if(CSceneManager.IsExistsMainCamera) {
				// Do Something
			}
		}
#endif			// #if UNITY_EDITOR
		#endregion			// 조건부 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
