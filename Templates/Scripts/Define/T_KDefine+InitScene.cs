using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if !USE_CUSTOM_PROJECT_OPTION
//! 초기화 씬 상수
public static partial class KDefine {
	#region 기본
	// 이름
	public const string IS_NAME_BLIND_UI = "BlindUI";
	#endregion			// 기본
	
	#region 런타임 상수
	// 경로 {
	public static readonly string IS_PATH_SCREEN_DEBUG_UI = string.Format("{0}{1}IS_ScreenDebugUI", KCDefine.DIR_PATH_PREFABS, KCDefine.DIR_PATH_INIT_SCENE_BASE);
	public static readonly string IS_PATH_SCREEN_BLIND_UI = string.Format("{0}{1}IS_ScreenBlindUI", KCDefine.DIR_PATH_PREFABS, KCDefine.DIR_PATH_INIT_SCENE_BASE);
	public static readonly string IS_PATH_SCREEN_POPUP_UI = string.Format("{0}{1}IS_ScreenPopupUI", KCDefine.DIR_PATH_PREFABS, KCDefine.DIR_PATH_INIT_SCENE_BASE);
	public static readonly string IS_PATH_SCREEN_TOPMOST_UI = string.Format("{0}{1}IS_ScreenTopmostUI", KCDefine.DIR_PATH_PREFABS, KCDefine.DIR_PATH_INIT_SCENE_BASE);
	public static readonly string IS_PATH_SCREEN_ABSOLUTE_UI = string.Format("{0}{1}IS_ScreenAbsoluteUI", KCDefine.DIR_PATH_PREFABS, KCDefine.DIR_PATH_INIT_SCENE_BASE);

	public static readonly string IS_PATH_SCREEN_BLIND_IMG = string.Format("{0}{1}IS_ScreenBlindImg", KCDefine.DIR_PATH_PREFABS, KCDefine.DIR_PATH_INIT_SCENE_BASE);
	// 경로 }
	#endregion			// 런타임 상수
}
#endif			// #if !USE_CUSTOM_PROJECT_OPTION
