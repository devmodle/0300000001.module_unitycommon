using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if UNITY_STANDALONE && EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
/** 레벨 에디터 씬 상수 */
public static partial class KDefine {
	#region 기본
	// 횟수
	public const int LES_MAX_TRY_TIMES_SETUP_CELL_INFOS = byte.MaxValue;
	#endregion			// 기본

	#region 추가 상수

	#endregion			// 추가 상수

	#region 추가 런타임 상수

	#endregion			// 추가 런타임 상수
}
#endif			// #if UNITY_STANDALONE && EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
#endif			// #if SCRIPT_TEMPLATE_ONLY
