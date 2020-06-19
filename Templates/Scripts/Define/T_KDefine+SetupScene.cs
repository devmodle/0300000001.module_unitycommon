using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if !USE_CUSTOM_PROJECT_OPTION
//! 설정 씬 상수
public static partial class KDefine {
	#region 기본
	// 이름 {
	public const string SS_NAME_POPUP_UI = "PopupUI";
	public const string SS_NAME_TOPMOST_UI = "TopmostUI";
	public const string SS_NAME_ABSOLUTE_UI = "AbsoluteUI";

	public const string SS_NAME_TIMER_MANAGER = "TimerManager";
	// 이름 }
	#endregion			// 기본

	#region 조건부 상수
#if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	// 이름
	public const string SS_NAME_DEBUG_UI = "DebugUI";
#endif			// #if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)

#if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	// 이름
	public const string SS_NAME_FPS_COUNTER = "FPSCounter";
#endif			// #if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	#endregion			// 조건부 상수
}
#endif			// #if !USE_CUSTOM_PROJECT_OPTION
