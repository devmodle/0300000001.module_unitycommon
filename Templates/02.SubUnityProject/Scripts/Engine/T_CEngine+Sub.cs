#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
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
			m_oPlayerObjDict[EKey.SEL_PLAYER_OBJ].Move(a_stDirection);
			CSceneManager.ActiveSceneMainCamera.transform.position = m_oPlayerObjDict[EKey.SEL_PLAYER_OBJ].transform.position;
		}
		
		/** 플레이어 객체 스킬을 적용한다 */
		public void ApplyPlayerObjSkill(CSkillTargetInfo a_oSkillTargetInfo) {
			m_oPlayerObjDict[EKey.SEL_PLAYER_OBJ].ApplySkill(a_oSkillTargetInfo);
		}

		/** 초기화한다 */
		private void SubInit() {
			// Do Something
		}

		/** 상태를 리셋한다 */
		private void SubReset() {
			this.SetEngineState(EEngineState.NONE);
		}
		
		/** 구동 상태를 처리한다 */
		private void HandleRunState() {
			// Do Something
		}

		/** 중지 상태를 처리한다 */
		private void HandleStopState() {
			// Do Something
		}

		/** 플레이 엔진 상태를 처리한다 */
		private void HandlePlayEngineState(float a_fDeltaTime) {
			for(int i = 0; i < m_oSkillList.Count; ++i) {
				m_oSkillList[i].OnUpdate(a_fDeltaTime);
			}

			for(int i = 0; i < m_oEnemyObjList.Count; ++i) {
				m_oEnemyObjList[i].OnUpdate(a_fDeltaTime);
			}
		}

		/** 정지 엔진 상태를 처리한다 */
		private void HandlePauseEngineState(float a_fDeltaTime) {
			// Do Something
		}

		/** 터치 시작 이벤트를 처리한다 */
		private void HandleTouchBeginEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var stIdx = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot).ExToIdx(m_oGridInfoDict[EKey.SEL_GRID_INFO].m_stPivotPos, KDefine.E_SIZE_CELL);
		}

		/** 터치 이동 이벤트를 처리한다 */
		private void HandleTouchMoveEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var stIdx = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot).ExToIdx(m_oGridInfoDict[EKey.SEL_GRID_INFO].m_stPivotPos, KDefine.E_SIZE_CELL);
		}

		/** 터치 종료 이벤트를 처리한다 */
		private void HandleTouchEndEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var stIdx = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot).ExToIdx(m_oGridInfoDict[EKey.SEL_GRID_INFO].m_stPivotPos, KDefine.E_SIZE_CELL);
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
