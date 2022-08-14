#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 제어자 */
	public abstract partial class CEController : CComponent {
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

			#region 추가
			this.SubInit();
			#endregion			// 추가
		}
		#endregion			// 함수
	}

	/** 서브 제어자 */
	public abstract partial class CEController : CComponent {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

		/** 상태 */
		public enum EState {
			NONE = -1,
			IDLE,
			MOVE,
			SKILL,
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
			if(CSceneManager.IsAppRunning) {
				switch(this.State) {
					case EState.IDLE: this.HandleIdleState(a_fDeltaTime); break;
					case EState.MOVE: this.HandleMoveState(a_fDeltaTime); break;
					case EState.SKILL: this.HandleSkillState(a_fDeltaTime); break;
				}
			}
		}

		/** 상태를 변경한다 */
		public void SetState(EState a_eState) {
			this.State = (m_oStateCheckerDict.TryGetValue(a_eState, out System.Func<bool> oStateChecker) && oStateChecker()) ? a_eState : this.State;
		}

		/** 무효 상태 가능 여부를 검사한다 */
		protected virtual bool IsEnableNoneState() {
			return true;
		}

		/** 대기 상태 가능 여부를 검사한다 */
		protected virtual bool IsEnableIdleState() {
			return true;
		}

		/** 이동 상태 가능 여부를 검사한다 */
		protected virtual bool IsEnableMoveState() {
			return this.State == EState.NONE || this.State == EState.IDLE || this.State == EState.MOVE;
		}

		/** 스킬 상태 가능 여부를 검사한다 */
		protected virtual bool IsEnableSkillState() {
			return this.State == EState.NONE || this.State == EState.IDLE || this.State == EState.MOVE;
		}

		/** 대기 상태를 처리한다 */
		protected virtual void HandleIdleState(float a_fDeltaTime) {
			// Do Something
		}

		/** 이동 상태를 처리한다 */
		protected virtual void HandleMoveState(float a_fDeltaTime) {
			// Do Something
		}

		/** 스킬 상태를 처리한다 */
		protected virtual void HandleSkillState(float a_fDeltaTime) {
			// Do Something
		}

		/** 효과를 설정한다 */
		private void SubAwakeSetup() {
			m_oStateCheckerDict.TryAdd(EState.NONE, this.IsEnableNoneState);
			m_oStateCheckerDict.TryAdd(EState.IDLE, this.IsEnableIdleState);
			m_oStateCheckerDict.TryAdd(EState.MOVE, this.IsEnableMoveState);
			m_oStateCheckerDict.TryAdd(EState.SKILL, this.IsEnableSkillState);
		}

		/** 초기화한다 */
		private void SubInit() {
			// Do Something
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
