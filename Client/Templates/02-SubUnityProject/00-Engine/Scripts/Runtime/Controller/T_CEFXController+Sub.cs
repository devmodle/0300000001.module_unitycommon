#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 서브 효과 제어자 */
	public partial class CEFXController : CEController {
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
		/** 효과를 적용한다 */
		public void Apply() {
			this.SetState(EState.IDLE);

			switch(this.GetOwner<CEFX>().Params.m_stFXInfo.FXApplyType) {
				case EFXApplyType.ANI: this.ApplyAni(); break;
				case EFXApplyType.TWEEN: this.ApplyTween(); break;
				case EFXApplyType.ANIMATOR: this.ApplyAnimator(); break;
				case EFXApplyType.PARTICLE_FX: this.ApplyParticleFX(); break;
			}
		}

		/** 대기 상태를 처리한다 */
		protected override void HandleIdleState(float a_fDeltaTime) {
			base.HandleIdleState(a_fDeltaTime);
			m_oRealDict[EKey.UPDATE_SKIP_TIME] += a_fDeltaTime;

			// 딜레이 시간이 지났을 경우
			if(m_oRealDict[EKey.UPDATE_SKIP_TIME].ExIsGreateEquals(this.GetOwner<CESkill>().Params.m_stSkillInfo.m_stTimeInfo.m_fDelay)) {
				m_oRealDict[EKey.UPDATE_SKIP_TIME] = KCDefine.B_VAL_0_REAL;

				this.SetState(EState.FX);
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

		/** 효과 정보를 리셋한다 */
		private void SubResetFXInfo(STFXInfo a_stFXInfo) {
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
				CFunc.ShowLogWarning($"CEFXController.SubOnDestroy Exception: {oException.Message}");
			}
		}

		/** 상태를 갱신한다 */
		private void SubOnUpdate(float a_fDeltaTime) {
			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				// Do Something
			}
		}

		/** 애니메이션을 적용한다 */
		private void ApplyAni() {
			// Do Something
		}

		/** 트윈 애니메이션을 적용한다 */
		private void ApplyTween() {
			// Do Something
		}

		/** 메카님 애니메이터를 적용한다 */
		private void ApplyAnimator() {
			// Do Something
		}

		/** 파티클 효과를 적용한다 */
		private void ApplyParticleFX() {
			// Do Something
		}

		/** 적용 서브 상태를 처리한다 */
		private void HandleApplySubState(float a_fDeltaTime) {
			m_oRealDict[EKey.UPDATE_SKIP_TIME] += a_fDeltaTime;

			// 적용 간격이 지났을 경우
			if(m_oIntDict[EKey.APPLY_TIMES] < this.GetOwner<CESkill>().Params.m_stSkillInfo.m_nMaxApplyTimes && m_oRealDict[EKey.UPDATE_SKIP_TIME].ExIsGreateEquals(this.GetOwner<CESkill>().Params.m_stSkillInfo.m_stTimeInfo.m_fDeltaTime * (m_oIntDict[EKey.APPLY_TIMES] - KCDefine.B_VAL_1_INT))) {
				m_oIntDict[EKey.APPLY_TIMES] += KCDefine.B_VAL_1_INT;
			}

			// 적용 시간이 지났을 경우
			if(m_oRealDict[EKey.UPDATE_SKIP_TIME].ExIsGreateEquals(this.GetOwner<CEFX>().Params.m_stFXInfo.m_stTimeInfo.m_fDuration)) {
				this.Engine.RemoveEObjComponent(this.GetOwner<CEFX>());
			}
		}

		/** 완료 서브 상태를 처리한다 */
		private void HandleCompleteSubState(float a_fDeltaTime) {
			// Do Something
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
