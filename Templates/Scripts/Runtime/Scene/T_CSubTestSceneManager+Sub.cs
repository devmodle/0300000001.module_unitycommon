using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
namespace TestScene {
	/** 서브 테스트 씬 관리자 */
	public partial class CSubTestSceneManager : CTestSceneManager {
		#region 함수

		#endregion			// 함수
	}

	/** 서브 테스트 씬 관리자 - 서브 */
	public partial class CSubTestSceneManager : CTestSceneManager {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

		#region 추가 변수

		#endregion			// 추가 변수

		#region 추가 함수

		#endregion			// 추가 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
