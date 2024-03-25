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
				case EFXApplyType.ANI: this.SetupAni(); break;
				case EFXApplyType.TWEEN: this.SetupTween(); break;
				case EFXApplyType.ANIMATOR: this.SetupAnimator(); break;
				case EFXApplyType.PARTICLE: this.SetupParticle(); break;
			}
		}

		/** 효과를 취소한다 */
		public void Cancel() {
			this.SetSubState(ESubState.COMPLETE);

			switch(this.GetOwner<CEFX>().Params.m_stFXInfo.FXApplyType) {
				case EFXApplyType.ANI: this.CancelAni(); break;
				case EFXApplyType.TWEEN: this.CancelTween(); break;
				case EFXApplyType.ANIMATOR: this.CancelAnimator(); break;
				case EFXApplyType.PARTICLE: this.CancelParticle(); break;
			}
		}

		/** 대기 상태를 처리한다 */
		protected override void HandleIdleState(float a_fTimeDelta) {
			base.HandleIdleState(a_fTimeDelta);
			m_fUpdateSkipTime += a_fTimeDelta;

			// 딜레이 시간이 지났을 경우
			if(m_fUpdateSkipTime.ExIsGreatEquals(this.GetOwner<CEFX>().Params.m_stFXInfo.m_stTimeInfo.m_fDelay)) {
				this.SetState(EState.FX);
				this.SetSubState(ESubState.APPLY);

				m_fUpdateSkipTime = KCDefine.B_VAL_0_REAL;
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

		/** 제거되었을 경우 */
		private void SubOnDestroy() {
			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsRunningApp) {
					// Do Something
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CEFXController.SubOnDestroy Exception: {oException.Message}");
			}
		}

		/** 상태를 갱신한다 */
		private void SubOnUpdate(float a_fTimeDelta) {
			// 앱이 실행 중 일 경우
			if(CSceneManager.IsRunningApp) {
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

		/** 파티클을 적용한다 */
		private void ApplyParticle() {
			// Do Something
		}

		/** 애니메이션을 취소한다 */
		private void CancelAni() {
			// Do Something
		}

		/** 트윈 애니메이션을 취소한다 */
		private void CancelTween() {
			// Do Something
		}

		/** 메카님 애니메이터를 취소한다 */
		private void CancelAnimator() {
			// Do Something
		}

		/** 파티클을 취소한다 */
		private void CancelParticle() {
			// Do Something
		}

		/** 적용 서브 상태를 처리한다 */
		private void HandleApplySubState(float a_fTimeDelta) {
			m_fUpdateSkipTime += a_fTimeDelta;

			// 적용 간격이 지났을 경우
			if(m_nApplyTimes < this.GetOwner<CEFX>().Params.m_stFXInfo.m_nMaxApplyTimes && m_fUpdateSkipTime.ExIsGreatEquals(this.GetOwner<CEFX>().Params.m_stFXInfo.m_stTimeInfo.m_fDeltaTime * (m_nApplyTimes - KCDefine.B_VAL_1_INT))) {
				m_nApplyTimes += KCDefine.B_VAL_1_INT;

				switch(this.GetOwner<CEFX>().Params.m_stFXInfo.FXApplyType) {
					case EFXApplyType.ANI: this.ApplyAni(); break;
					case EFXApplyType.TWEEN: this.ApplyTween(); break;
					case EFXApplyType.ANIMATOR: this.ApplyAnimator(); break;
					case EFXApplyType.PARTICLE: this.ApplyParticle(); break;
				}
			}

			// 적용 시간이 지났을 경우
			if(m_fUpdateSkipTime.ExIsGreatEquals(this.GetOwner<CEFX>().Params.m_stFXInfo.m_stTimeInfo.m_fDuration)) {
				this.SetSubState(ESubState.COMPLETE);
			}
		}

		/** 완료 서브 상태를 처리한다 */
		private void HandleCompleteSubState(float a_fTimeDelta) {
			// 소유자가 존재 할 경우
			if(this.GetOwner<CEFX>() != null && this.GetOwner<CEFX>().gameObject.activeSelf) {
				this.SetState(EState.NONE);
				this.Engine.RemoveEObjComponent(this.GetOwner<CEFX>());
			}
		}
		#endregion // 함수
	}

	/** 서브 효과 제어자 - 설정 */
	public partial class CEFXController : CEController {
		#region 함수
		/** 애니메이션을 설정한다 */
		private void SetupAni() {
			// Do Something
		}

		/** 트윈 애니메이션을 설정한다 */
		private void SetupTween() {
			// Do Something
		}

		/** 메카님 애니메이터를 설정한다 */
		private void SetupAnimator() {
			// Do Something
		}

		/** 파티클을 설정한다 */
		private void SetupParticle() {
			// Do Something
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
