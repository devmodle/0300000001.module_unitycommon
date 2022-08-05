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
			[HideInInspector] MAX_VAL
		}

		#region 변수

		#endregion			// 변수

		#region 프로퍼티

		#endregion			// 프로퍼티

		#region 함수
		/** 상태를 변경한다 */
		public void SetState(EState a_eState) {
			this.State = a_eState;

			switch(a_eState) {
				case EState.ACTIVE: this.HandleActiveState(); break;
				case EState.INACTIVE: this.HandleInactiveState(); break;
			}
		}

		/** 활성 상태를 처리한다 */
		protected virtual void HandleActiveState() {
			// Do Something
		}

		/** 비활성 상태를 처리한다 */
		protected virtual void HandleInactiveState() {
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
