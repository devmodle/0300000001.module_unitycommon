#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 - 접근 */
	public partial class CEngine : CComponent {
		#region 함수
		/** 상태를 변경한다 */
		public void SetState(EState a_eState) {
			// 상태 변경이 가능 할 경우
			if(this.State != a_eState) {
				this.State = a_eState;

				switch(a_eState) {
					case EState.RUN: this.HandleRunState(); break;
					case EState.STOP: this.HandleStopState(); break;
				}
			}
		}
		#endregion			// 함수
	}

	/** 서브 엔진 - 접근 */
	public partial class CEngine : CComponent {
		#region 함수
		/** 엔진 상태를 변경한다 */
		public void SetEngineState(EEngineState a_eEngineState) {
			// 엔진 상태 변경이 가능 할 경우
			if(this.EngineState != a_eEngineState) {
				this.EngineState = a_eEngineState;
			}
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
