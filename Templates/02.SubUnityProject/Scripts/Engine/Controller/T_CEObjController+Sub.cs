#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 아이템 제어자 */
	public partial class CEObjController : CEController {
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

	/** 서브 객체 제어자 */
	public partial class CEObjController : CEController {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

		#region 변수

		#endregion			// 변수

		#region 프로퍼티

		#endregion			// 프로퍼티

		#region 함수
		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(this.IsAutoControl && CSceneManager.IsAppRunning) {
				// Do Something
			}
		}

		/** 이동을 처리한다 */
		public virtual void Move(Vector3 a_stDirection) {
			this.SetState(EState.MOVE);
			this.Vec3Dict.ExReplaceVal(EKey.MOVE_DIRECTION, a_stDirection);
		}

		/** 스킬을 적용시킨다 */
		public virtual void ApplySkill(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo) {
			// 스킬 적용이 가능 할 경우
			if(this.IsEnableSkillState() && this.IsEnableApplySkill(a_stSkillInfo, a_oSkillTargetInfo)) {
				this.SetState(EState.SKILL);
				this.ApplySkillTimeDict.ExReplaceVal(a_oSkillTargetInfo.SkillKinds, System.DateTime.Now);

				this.SkillInfoDict.ExReplaceVal(EKey.APPLY_SKILL_INFO, a_stSkillInfo);
				this.SkillTargetInfoDict.ExReplaceVal(EKey.APPLY_SKILL_TARGET_INFO, a_oSkillTargetInfo);
			}
		}

		/** 스킬 적용 가능 여부를 검사한다 */
		protected virtual bool IsEnableApplySkill(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo) {
			// 적용 스킬 타겟 정보가 없을 경우
			if(this.SkillInfoDict.GetValueOrDefault(EKey.APPLY_SKILL_INFO).m_eSkillKinds == ESkillKinds.NONE || this.SkillTargetInfoDict.GetValueOrDefault(EKey.APPLY_SKILL_TARGET_INFO) == null) {
				return true;
			}

			var stApplySkillTime = this.ApplySkillTimeDict.GetValueOrDefault(a_stSkillInfo.m_eSkillKinds, System.DateTime.Today.AddDays(-KCDefine.B_VAL_1_INT));
			return System.DateTime.Now.ExGetDeltaTime(stApplySkillTime).ExIsGreateEquals(a_stSkillInfo.m_fDelay);
		}

		/** 이동 상태를 처리한다 */
		protected override void HandleMoveState(float a_fDeltaTime) {
			base.HandleMoveState(a_fDeltaTime);

			var stNextPos = this.GetOwner<CEObj>().transform.localPosition + ((this.Vec3Dict.GetValueOrDefault(EKey.MOVE_DIRECTION) * (float)this.GetOwner<CEObj>().AbilityValDict.GetValueOrDefault(EAbilityKinds.STAT_MOVE_SPEED_01)) * a_fDeltaTime);
			var stEpisodeInfo = global::Access.GetEpisodeInfo(base.Params.m_oEngine.Params.m_oLevelInfo.m_stIDInfo.m_nID01, base.Params.m_oEngine.Params.m_oLevelInfo.m_stIDInfo.m_nID02, base.Params.m_oEngine.Params.m_oLevelInfo.m_stIDInfo.m_nID03);

			float fNextPosY = Mathf.Clamp(stNextPos.y, (stEpisodeInfo.m_stSize.y / -KCDefine.B_VAL_2_REAL) + KDefine.E_OFFSET_BOTTOM, stEpisodeInfo.m_stSize.y / KCDefine.B_VAL_2_REAL);
			this.GetOwner<CEObj>().transform.localPosition = new Vector3(Mathf.Clamp(stNextPos.x, stEpisodeInfo.m_stSize.x / -KCDefine.B_VAL_2_REAL, stEpisodeInfo.m_stSize.x / KCDefine.B_VAL_2_REAL), fNextPosY, fNextPosY / stEpisodeInfo.m_stSize.y);
		}

		/** 스킬 상태를 처리한다 */
		protected override void HandleSkillState(float a_fDeltaTime) {
			base.HandleSkillState(a_fDeltaTime);
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
