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

	// 이름 {
	public const string SS_OBJ_NAME_LOADING_TEXT = "LoadingText";
	public const string SS_OBJ_NAME_LOADING_IMG_OBJ = "LoadingImgObj";

	public const string SS_OBJ_NAME_GAUGE_IMG = "GaugeImg";
	// 이름 }
	#endregion			// 기본

	#region 런타임 상수
	// 위치
	public static readonly Vector3 SS_POS_LOADING_TEXT = new Vector3(0.0f, 40.0f, 0.0f);
	public static readonly Vector3 SS_POS_LOADING_IMG_OBJ = new Vector3(0.0f, -40.0f, 0.0f);
	#endregion			// 런타임 상수
}
#endif			// #if NEVER_USE_THIS
