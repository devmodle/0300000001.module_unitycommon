#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 난이도 UI 처리자 */
public partial class CDifficultyUIsHandler : CComponent {
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
	/** 초기화 */
	private void SubAwake() {
		// Do Something
	}

	/** 초기화 */
	private void SubStart() {
		// Do Something
	}
	#endregion // 함수
}

/** 난이도 UI 처리자 - 설정 */
public partial class CDifficultyUIsHandler : CComponent {
	#region 함수
	/** 난이도를 설정한다 */
	private void SubSetupDifficulty() {
		// Do Something
	}
	#endregion // 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
