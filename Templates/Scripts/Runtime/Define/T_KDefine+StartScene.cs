using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 시작 씬 상수
public static partial class KDefine {
	#region 기본
	// 개수
	public const int SS_MAX_NUM_DOTS = 3;

	// 시간
	public const float SS_DELTA_TIME_UPDATE_STATE = 0.5f;

	// 이름
	public const string SS_OBJ_NAME_LOADING_TEXT = "StateText";
	#endregion			// 기본

	#region 런타임 상수
	// 경로
	public static readonly string SS_OBJ_PATH_LOADING_TEXT = string.Format("{0}{1}SS_StateText", KCDefine.B_DIR_PATH_PREFABS, KCDefine.B_DIR_PATH_START_SCENE_BASE);
	#endregion			// 런타임 상수
}
#endif			// #if NEVER_USE_THIS
