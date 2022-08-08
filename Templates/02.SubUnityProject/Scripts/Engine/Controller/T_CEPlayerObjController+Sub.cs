#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 플레이어 객체 제어자 */
	public partial class CEPlayerObjController : CEController {
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
			base.Init(a_stParams.m_stBaseParams);
			this.Params = a_stParams;

			#region 추가
			this.SubInit();
			#endregion			// 추가
		}
		#endregion			// 함수
	}

	/** 서브 플레이어 객체 제어자 */
	public partial class CEPlayerObjController : CEController {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

		#region 변수

		#endregion			// 변수

		#region 프로퍼티
		public bool IsEnableMove => this.IsActive && (this.ControllerState == EControllerState.IDLE || this.ControllerState == EControllerState.MOVE);
		public bool IsEnableApplySkill => this.IsActive && (this.ControllerState == EControllerState.IDLE || this.ControllerState == EControllerState.MOVE);
		#endregion			// 프로퍼티

		#region 함수
		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(this.IsActive && CSceneManager.IsAppRunning) {
				// Do Something
			}
		}

		/** 이동을 처리한다 */
		public override void Move(Vector3 a_stDirection) {
			base.Move(a_stDirection);
			this.SetControllerState(this.IsEnableMove ? EControllerState.MOVE : a_stDirection.ExIsEquals(Vector3.zero) ? EControllerState.IDLE : this.ControllerState);
		}

		/** 스킬을 적용한다 */
		public override void ApplySkill(CSkillTargetInfo a_oSkillTargetInfo) {
			base.ApplySkill(a_oSkillTargetInfo);
			this.SetControllerState(this.IsEnableApplySkill ? EControllerState.SKILL : this.ControllerState);
		}

		/** 대기 제어자 상태를 처리한다 */
		protected override void HandleIdleControllerState(float a_fDeltaTime) {
			base.HandleIdleControllerState(a_fDeltaTime);
		}

		/** 이동 제어자 상태를 처리한다 */
		protected override void HandleMoveControllerState(float a_fDeltaTime) {
			base.HandleMoveControllerState(a_fDeltaTime);
		}

		/** 스킬 제어자 상태를 처리한다 */
		protected override void HandleSkillControllerState(float a_fDeltaTime) {
			base.HandleSkillControllerState(a_fDeltaTime);
		}

		/** 효과를 설정한다 */
		private void SubAwakeSetup() {
			// Do Something
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
