#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 서브 객체 제어자 */
	public partial class CEObjController : CEController {
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
		/** 이동 상태를 처리한다 */
		protected override void HandleMoveState(float a_fTimeDelta) {
			base.HandleMoveState(a_fTimeDelta);
			this.GetOwner<CEObj>().transform.localPosition = this.GetNextPos((this.MoveDirection * (float)this.GetOwner<CEObj>().GetAbilityVal(EAbilityKinds.STAT_ABILITY_MOVE_SPEED_01)) * a_fTimeDelta);
		}

		/** 스킬 상태를 처리한다 */
		protected override void HandleSkillState(float a_fTimeDelta) {
			base.HandleSkillState(a_fTimeDelta);
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
				CFunc.ShowLogWarning($"CEObjController.SubOnDestroy Exception: {oException.Message}");
			}
		}

		/** 상태를 갱신한다 */
		private void SubOnUpdate(float a_fTimeDelta) {
			// 앱이 실행 중 일 경우
			if(CSceneManager.IsRunningApp) {
				// Do Something
			}
		}
		#endregion // 함수
	}

	/** 서브 객체 제어자 - 접근 */
	public partial class CEObjController : CEController {
		#region 함수
		/** 다음 위치를 반환한다 */
		protected virtual Vector3 GetNextPos(Vector3 a_stVelocity) {
			var stPos = this.GetOwner<CEObj>().transform.localPosition + a_stVelocity;
			var stSize = this.Engine.EpisodeSize.ExToLocal(this.Engine.Params.m_oObjRoot, false);
			var stOffset = KDefine.E_OFFSET_BOTTOM.ExToLocal(this.Engine.Params.m_oObjRoot, false);

			float fPosY = Mathf.Clamp(stPos.y, (stSize.y / -KCDefine.B_VAL_2_REAL) + stOffset.y, stSize.y / KCDefine.B_VAL_2_REAL);
			return new Vector3(Mathf.Clamp(stPos.x, stSize.x / -KCDefine.B_VAL_2_REAL, stSize.x / KCDefine.B_VAL_2_REAL), fPosY, fPosY / stSize.y);
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
