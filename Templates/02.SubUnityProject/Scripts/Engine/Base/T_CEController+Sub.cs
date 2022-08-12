#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 제어자 */
	public partial class CEController : CComponent {
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
	public partial class CEController : CComponent {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			MOVE_DIRECTION,
			APPLY_SKILL_TARGET_INFO,
			[HideInInspector] MAX_VAL
		}

		/** 상태 */
		public enum EState {
			NONE = -1,
			ACTIVE,
			INACTIVE,
			[HideInInspector] MAX_VAL
		}

		/** 제어자 상태 */
		public enum EControllerState {
			NONE = -1,
			IDLE,
			MOVE,
			SKILL,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		private Dictionary<ESubKey, Vector3> m_oVec3Dict = new Dictionary<ESubKey, Vector3>() {
			[ESubKey.MOVE_DIRECTION] = Vector3.zero
		};

		private Dictionary<ESubKey, CSkillTargetInfo> m_oSkillTargetInfoDict = new Dictionary<ESubKey, CSkillTargetInfo>() {
			[ESubKey.APPLY_SKILL_TARGET_INFO] = null
		};
		#endregion			// 변수

		#region 프로퍼티
		public EState State { get; private set; } = EState.NONE;
		public EControllerState ControllerState { get; private set; } = EControllerState.NONE;
		
		public bool IsActive => this.State == EState.ACTIVE;
		public Vector3 MoveDirection => m_oVec3Dict[ESubKey.MOVE_DIRECTION];
		public CSkillTargetInfo ApplySkillTargetInfo => m_oSkillTargetInfoDict[ESubKey.APPLY_SKILL_TARGET_INFO];
		#endregion			// 프로퍼티

		#region 함수
		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(this.IsActive && CSceneManager.IsAppRunning) {
				switch(this.ControllerState) {
					case EControllerState.IDLE: this.HandleIdleControllerState(a_fDeltaTime); break;
					case EControllerState.MOVE: this.HandleMoveControllerState(a_fDeltaTime); break;
					case EControllerState.SKILL: this.HandleSkillControllerState(a_fDeltaTime); break;
				}
			}
		}

		/** 활성화 되었을 경우 */
		public virtual void OnEnable() {
			this.SetState(EState.ACTIVE);
		}

		/** 비활성화 되었을 경우 */
		public virtual void OnDisable() {
			this.SetState(EState.INACTIVE);
		}

		/** 상태를 변경한다 */
		public void SetState(EState a_eState) {
			// 상태 변경이 가능 할 경우
			if(this.State != a_eState) {
				this.State = a_eState;

				switch(a_eState) {
					case EState.ACTIVE: this.HandleActiveState(); break;
					case EState.INACTIVE: this.HandleInactiveState(); break;
				}
			}
		}

		/** 제어자 상태를 변경한다 */
		public void SetControllerState(EControllerState a_eControllerState) {
			// 제어자 상태 변경이 가능 할 경우
			if(this.ControllerState != a_eControllerState) {
				this.ControllerState = a_eControllerState;
			}
		}

		/** 이동을 처리한다 */
		public virtual void Move(Vector3 a_stDirection) {
			m_oVec3Dict[ESubKey.MOVE_DIRECTION] = a_stDirection;
		}

		/** 스킬을 적용시킨다 */
		public virtual void ApplySkill(CSkillTargetInfo a_oSkillTargetInfo) {
			m_oSkillTargetInfoDict[ESubKey.APPLY_SKILL_TARGET_INFO] = a_oSkillTargetInfo;
		}

		/** 활성 상태를 처리한다 */
		protected virtual void HandleActiveState() {
			// Do Something
		}

		/** 비활성 상태를 처리한다 */
		protected virtual void HandleInactiveState() {
			// Do Something
		}

		/** 대기 제어자 상태를 처리한다 */
		protected virtual void HandleIdleControllerState(float a_fDeltaTime) {
			// Do Something
		}

		/** 이동 제어자 상태를 처리한다 */
		protected virtual void HandleMoveControllerState(float a_fDeltaTime) {
			var stNextPos = this.GetOwner<CEObj>().transform.localPosition + ((this.MoveDirection * (float)this.GetOwner<CEObj>().AbilityValDict.GetValueOrDefault(EAbilityKinds.STAT_MOVE_SPEED_01)) * a_fDeltaTime);
			var stEpisodeInfo = global::Access.GetEpisodeInfo(this.Params.m_oEngine.Params.m_oLevelInfo.m_stIDInfo.m_nID01, this.Params.m_oEngine.Params.m_oLevelInfo.m_stIDInfo.m_nID02, this.Params.m_oEngine.Params.m_oLevelInfo.m_stIDInfo.m_nID03);

			float fNextPosY = Mathf.Clamp(stNextPos.y, (stEpisodeInfo.m_stSize.y / -KCDefine.B_VAL_2_REAL) + KDefine.E_OFFSET_BOTTOM, stEpisodeInfo.m_stSize.y / KCDefine.B_VAL_2_REAL);
			this.GetOwner<CEObj>().transform.localPosition = new Vector3(Mathf.Clamp(stNextPos.x, stEpisodeInfo.m_stSize.x / -KCDefine.B_VAL_2_REAL, stEpisodeInfo.m_stSize.x / KCDefine.B_VAL_2_REAL), fNextPosY, fNextPosY / stEpisodeInfo.m_stSize.y);
		}

		/** 스킬 제어자 상태를 처리한다 */
		protected virtual void HandleSkillControllerState(float a_fDeltaTime) {
			// Do Something
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
