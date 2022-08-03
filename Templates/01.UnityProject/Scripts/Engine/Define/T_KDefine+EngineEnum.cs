#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 열거형 값 */
	public static partial class KEngineEnumVal {
		#region 기본

		#endregion			// 기본
	}

	#region 기본
	/** 컴포넌트 타입 */
	public enum EComponentType {
		NONE = -1,
		ITEM,
		SKILL,
		OBJ,
		FX,
		[HideInInspector] MAX_VAL
	}
	#endregion			// 기본
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
