using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if GAME_CENTER_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 게임 센터 상수 */
public static partial class KDefine {
	#region 기본
	// 식별자 {
#if UNITY_IOS

#else

#endif			// #if UNITY_IOS
	// 식별자 }
	#endregion			// 기본

	#region 추가 상수
#if UNITY_IOS

#else

#endif			// #if UNITY_IOS
	#endregion			// 추가 상수

	#region 추가 런타임 상수

	#endregion			// 추가 런타임 상수
}
#endif			// #if GAME_CENTER_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
