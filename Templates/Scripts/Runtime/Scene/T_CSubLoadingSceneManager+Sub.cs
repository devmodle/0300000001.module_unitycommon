using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
namespace LoadingScene {
	/** 서브 로딩 씬 관리자 */
	public partial class CSubLoadingSceneManager : CLoadingSceneManager {
		#region 함수

		#endregion			// 함수
	}

	/** 서브 로딩 씬 관리자 - 서브 */
	public partial class CSubLoadingSceneManager : CLoadingSceneManager {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

		#region 변수

		#endregion			// 변수

		#region 함수

		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
