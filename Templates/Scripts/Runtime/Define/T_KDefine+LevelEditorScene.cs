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
	
	// 식별자
	public const string LES_KEY_SPRITE_OBJS_POOL = "SpriteObjsPool";

	// 이름 {
	public const string LES_OBJ_N_FMT_PAGE_UIS = "PageUIs_{0:00}";

	public const string LES_FUNC_N_FMT_SETUP_RE_UIS_PAGE_UIS = "SetupREUIsPageUIs{0:00}";
	public const string LES_FUNC_N_FMT_UPDATE_RE_UIS_PAGE_UIS = "UpdateREUIsPageUIs{0:00}";
	// 이름 }
	#endregion			// 기본

	#region 추가 상수

	#endregion			// 추가 상수

	#region 추가 런타임 상수

	#endregion			// 추가 런타임 상수
}
#endif			// #if UNITY_STANDALONE && EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
#endif			// #if SCRIPT_TEMPLATE_ONLY
