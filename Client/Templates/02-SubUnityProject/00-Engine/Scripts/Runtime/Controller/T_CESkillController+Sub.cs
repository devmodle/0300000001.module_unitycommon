#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 서브 스킬 제어자 */
	public partial class CESkillController : CEController {
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
		/** 스킬을 적용한다 */
		public void Apply() {
			this.SetState(EState.IDLE);
			this.Owner.GetOwner<CEObj>()?.GetController<CEObjController>().ApplySkillTimeDict.ExReplaceVal(this.GetOwner<CESkill>().Params.m_stSkillInfo.m_eSkillKinds, System.DateTime.Now);
		}

		/** 효과를 취소한다 */
		public void Cancel() {
			this.SetSubState(ESubState.COMPLETE);
		}

		/** 대기 상태를 처리한다 */
		protected override void HandleIdleState(float a_fDeltaTime) {
			base.HandleIdleState(a_fDeltaTime);
			m_oRealDict[EKey.UPDATE_SKIP_TIME] += a_fDeltaTime;

			// 딜레이 시간이 지났을 경우
			if(m_oRealDict[EKey.UPDATE_SKIP_TIME].ExIsGreateEquals(this.GetOwner<CESkill>().Params.m_stSkillInfo.m_stTimeInfo.m_fDelay)) {
				m_oRealDict[EKey.UPDATE_SKIP_TIME] = KCDefine.B_VAL_0_REAL;

				this.SetState(EState.SKILL);
				this.SetSubState(ESubState.APPLY);
			}
		}

		/** 초기화 */
		private void SubAwake() {
			// Do Something
		}

		/** 초기화 */
		private void SubInit() {
			// Do Something
		}

		/** 스킬 정보를 리셋한다 */
		private void SubResetSkillInfo(STSkillInfo a_stSkillInfo) {
			// Do Something
		}

		/** 제거 되었을 경우 */
		private void SubOnDestroy() {
			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsAppRunning) {
					// Do Something
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CESkillController.SubOnDestroy Exception: {oException.Message}");
			}
		}

		/** 상태를 갱신한다 */
		private void SubOnUpdate(float a_fDeltaTime) {
			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				// Do Something
			}
		}

		/** 다중 스킬을 적용한다 */
		private void ApplyMultiSkill() {
			// Do Something
		}

		/** 단일 스킬을 적용한다 */
		private void ApplySingleSkill() {
			// Do Something
		}

		/** 적용 서브 상태를 처리한다 */
		private void HandleApplySubState(float a_fDeltaTime) {
			m_oRealDict[EKey.UPDATE_SKIP_TIME] += a_fDeltaTime;

			// 적용 간격이 지났을 경우
			if(m_oIntDict[EKey.APPLY_TIMES] < this.GetOwner<CESkill>().Params.m_stSkillInfo.m_nMaxApplyTimes && m_oRealDict[EKey.UPDATE_SKIP_TIME].ExIsGreateEquals(this.GetOwner<CESkill>().Params.m_stSkillInfo.m_stTimeInfo.m_fDeltaTime * (m_oIntDict[EKey.APPLY_TIMES] - KCDefine.B_VAL_1_INT))) {
				m_oIntDict[EKey.APPLY_TIMES] += KCDefine.B_VAL_1_INT;
				
				switch((EApplyType)((int)this.GetOwner<CESkill>().Params.m_stSkillInfo.m_eSkillApplyKinds).ExKindsToType()) {
					case EApplyType.MULTI: this.ApplyMultiSkill(); break;
					case EApplyType.SINGLE: this.ApplySingleSkill(); break;
				}
			}

			// 적용 시간이 지났을 경우
			if(m_oRealDict[EKey.UPDATE_SKIP_TIME].ExIsGreateEquals(this.GetOwner<CESkill>().Params.m_stSkillInfo.m_stTimeInfo.m_fDuration)) {
				this.SetSubState(ESubState.COMPLETE);
			}
		}

		/** 완료 서브 상태를 처리한다 */
		private void HandleCompleteSubState(float a_fDeltaTime) {
			// 소유자가 존재 할 경우
			if(this.GetOwner<CESkill>() != null && this.GetOwner<CESkill>().gameObject.activeSelf) {
				this.SetState(EState.NONE);
				this.Owner.GetOwner<CEObj>()?.GetController<CEObjController>()?.SetState(EState.IDLE);

				this.Engine.RemoveEObjComponent(this.GetOwner<CESkill>());
			}
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
