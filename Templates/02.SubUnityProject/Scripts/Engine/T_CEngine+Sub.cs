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
			this.SetState(EState.STOP);

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
			RUN,
			STOP,
			[HideInInspector] MAX_VAL
		}

		/** 엔진 상태 */
		public enum EEngineState {
			NONE = -1,
			PLAY,
			PAUSE,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		
		#endregion			// 변수

		#region 프로퍼티
		public EState State { get; private set; } = EState.NONE;
		public EEngineState EngineState { get; private set; } = EEngineState.NONE;

		public bool IsPlaying => this.State == EState.RUN && this.EngineState == EEngineState.PLAY;
		#endregion			// 프로퍼티
		
		#region 함수
		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(this.IsPlaying && CSceneManager.IsAppRunning) {
				switch(this.EngineState) {
					case EEngineState.PLAY: this.HandlePlayEngineState(a_fDeltaTime); break;
					case EEngineState.PAUSE: this.HandlePauseEngineState(a_fDeltaTime); break;
				}

				var stEpisodeInfo = global::Access.GetEpisodeInfo(this.Params.m_oLevelInfo.m_stIDInfo.m_nID01, this.Params.m_oLevelInfo.m_stIDInfo.m_nID02, this.Params.m_oLevelInfo.m_stIDInfo.m_nID03);
				var stEpisodeSize = new Vector3(Mathf.Clamp(stEpisodeInfo.m_stSize.x, KCDefine.B_VAL_0_REAL, stEpisodeInfo.m_stSize.x - KCDefine.B_SCREEN_SIZE.x), Mathf.Clamp(stEpisodeInfo.m_stSize.y, KCDefine.B_VAL_0_REAL, stEpisodeInfo.m_stSize.y - KCDefine.B_SCREEN_SIZE.y), stEpisodeInfo.m_stSize.z) * (KCDefine.B_UNIT_SCALE * CAccess.ResolutionScale);
				var stMainCameraPos = new Vector3(Mathf.Clamp(m_oPlayerObjDict.GetValueOrDefault(EKey.SEL_PLAYER_OBJ).transform.position.x, stEpisodeSize.x / -KCDefine.B_VAL_2_REAL, stEpisodeSize.x / KCDefine.B_VAL_2_REAL), Mathf.Clamp(m_oPlayerObjDict.GetValueOrDefault(EKey.SEL_PLAYER_OBJ).transform.position.y + KDefine.E_OFFSET_MAIN_CAMERA, stEpisodeSize.y / -KCDefine.B_VAL_2_REAL, stEpisodeSize.y / KCDefine.B_VAL_2_REAL), CSceneManager.ActiveSceneMainCamera.transform.position.z);
				
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

		/** 플레이어 객체 이동을 처리한다 */
		public void MovePlayerObj(Vector3 a_stDirection) {
			m_oPlayerObjDict.GetValueOrDefault(EKey.SEL_PLAYER_OBJ).GetController<CEPlayerObjController>().Move(a_stDirection);
		}
		
		/** 플레이어 객체 스킬을 적용한다 */
		public void ApplyPlayerObjSkill(CSkillTargetInfo a_oSkillTargetInfo) {
			m_oPlayerObjDict.GetValueOrDefault(EKey.SEL_PLAYER_OBJ).GetController<CEPlayerObjController>().ApplySkill(a_oSkillTargetInfo);
		}

		/** 초기화한다 */
		private void SubInit() {
			var stObjInfo = CObjInfoTable.Inst.GetObjInfo(EObjKinds.PLAYABLE_COMMON_CHARACTER_01);
			m_oPlayerObjDict.ExReplaceVal(EKey.SEL_PLAYER_OBJ, this.CreatePlayerObj(stObjInfo, CUserInfoStorage.Inst.GetCharacterUserInfo(CGameInfoStorage.Inst.PlayCharacterID), null, true));

			CSceneManager.ActiveSceneMainCamera.transform.position = new Vector3(m_oPlayerObjDict.GetValueOrDefault(EKey.SEL_PLAYER_OBJ).transform.position.x, m_oPlayerObjDict.GetValueOrDefault(EKey.SEL_PLAYER_OBJ).transform.position.y + (KDefine.E_OFFSET_MAIN_CAMERA * KCDefine.B_UNIT_SCALE * CAccess.ResolutionScale), CSceneManager.ActiveSceneMainCamera.transform.position.z);
		}

		/** 상태를 리셋한다 */
		private void SubReset() {
			this.SetEngineState(EEngineState.NONE);
		}
		
		/** 구동 상태를 처리한다 */
		private void HandleRunState() {
			this.SetEngineState(EEngineState.PLAY);
		}

		/** 중지 상태를 처리한다 */
		private void HandleStopState() {
			this.SetEngineState(EEngineState.PAUSE);
		}

		/** 플레이 엔진 상태를 처리한다 */
		private void HandlePlayEngineState(float a_fDeltaTime) {
			CFunc.UpdateComponents(m_oItemList, a_fDeltaTime);
			CFunc.UpdateComponents(m_oSkillList, a_fDeltaTime);
			CFunc.UpdateComponents(m_oFXList, a_fDeltaTime);
			CFunc.UpdateComponents(m_oEnemyObjList, a_fDeltaTime);
			CFunc.UpdateComponents(m_oPlayerObjDict, a_fDeltaTime);

			/* FIXME: 임시 주석 처리
			var stEpisodeInfo = global::Access.GetEpisodeInfo(this.Params.m_oLevelInfo.m_stIDInfo.m_nID01, this.Params.m_oLevelInfo.m_stIDInfo.m_nID02, this.Params.m_oLevelInfo.m_stIDInfo.m_nID03);
			var oNumEnemyObjsDict = CCollectionManager.Inst.SpawnDict<EObjKinds, int>();

			try {
				for(int i = 0; i < m_oEnemyObjList.Count; ++i) {
					int nNumEnemyObjs = oNumEnemyObjsDict.GetValueOrDefault(m_oEnemyObjList[i].Params.m_stObjInfo.m_eObjKinds);
					oNumEnemyObjsDict.ExReplaceVal(m_oEnemyObjList[i].Params.m_stObjInfo.m_eObjKinds, nNumEnemyObjs + KCDefine.B_VAL_1_INT);
				}

				foreach(var stKeyVal in stEpisodeInfo.m_oEnemyObjTargetInfoDict) {
					// 적 객체 생성이 가능 할 경우
					if(oNumEnemyObjsDict.GetValueOrDefault((EObjKinds)stKeyVal.Value.m_nKinds) < stKeyVal.Value.m_stValInfo01.m_nVal && m_oEnemyObjList.Count < stEpisodeInfo.m_nMaxNumEnemyObjs) {
						var oEnemyObj = this.CreateEnemyObj(CObjInfoTable.Inst.GetObjInfo((EObjKinds)stKeyVal.Value.m_nKinds), null);
						oEnemyObj.transform.localPosition = new Vector3(Random.Range(stEpisodeInfo.m_stSize.x / -KCDefine.B_VAL_2_REAL, stEpisodeInfo.m_stSize.x / KCDefine.B_VAL_2_REAL), Random.Range(stEpisodeInfo.m_stSize.y / -KCDefine.B_VAL_2_REAL, stEpisodeInfo.m_stSize.y / KCDefine.B_VAL_2_REAL), KCDefine.B_VAL_0_REAL);

						m_oEnemyObjList.ExAddVal(oEnemyObj);
					}
				}
			} finally {
				CCollectionManager.Inst.DespawnDict(oNumEnemyObjsDict);
			}
			*/
		}

		/** 정지 엔진 상태를 처리한다 */
		private void HandlePauseEngineState(float a_fDeltaTime) {
			// Do Something
		}

		/** 터치 시작 이벤트를 처리한다 */
		private void HandleTouchBeginEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 구동 상태 일 경우
			if(this.State == EState.RUN) {
				var stIdx = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot).ExToIdx(m_oGridInfoDict.GetValueOrDefault(EKey.SEL_GRID_INFO).m_stPivotPos, KDefine.E_SIZE_CELL);
			}
		}

		/** 터치 이동 이벤트를 처리한다 */
		private void HandleTouchMoveEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 구동 상태 일 경우
			if(this.State == EState.RUN) {
				var stIdx = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot).ExToIdx(m_oGridInfoDict.GetValueOrDefault(EKey.SEL_GRID_INFO).m_stPivotPos, KDefine.E_SIZE_CELL);
			}
		}

		/** 터치 종료 이벤트를 처리한다 */
		private void HandleTouchEndEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 구동 상태 일 경우
			if(this.State == EState.RUN) {
				var stIdx = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot).ExToIdx(m_oGridInfoDict.GetValueOrDefault(EKey.SEL_GRID_INFO).m_stPivotPos, KDefine.E_SIZE_CELL);
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
