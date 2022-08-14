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
		
		#endregion			// 함수
	}

	/** 서브 엔진 - 접근 */
	public partial class CEngine : CComponent {
		#region 함수
		/** 상태를 변경한다 */
		public void SetState(EState a_eState) {
			this.State = (m_oStateCheckerDict.TryGetValue(a_eState, out System.Func<bool> oStateChecker) && oStateChecker()) ? a_eState : this.State;
		}

		/** 무효 상태 가능 여부를 검사한다 */
		private bool IsEnableNoneState() {
			return true;
		}

		/** 플레이 상태 가능 여부를 검사한다 */
		private bool IsEnablePlayState() {
			return true;
		}

		/** 정지 상태 가능 여부를 검사한다 */
		private bool IsEnablePauseState() {
			return true;
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
