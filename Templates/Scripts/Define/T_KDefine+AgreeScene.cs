using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if !USE_CUSTOM_PROJECT_OPTION
//! 약관 동의 씬 상수
public static partial class KDefine {
	#region 기본
	public const string AS_FUNC_NAME_AGREE_SCENE_MANAGER_EVENT = "OnReceiveAgreeSceneManagerEvent";
	#endregion			// 기본

	#region 런타임 상수
	// 경로 {
	public static readonly string AS_DATA_PATH_KOREAN_SERVICE_TEXT = string.Format("{0}{1}AS_Service_KO", KDefine.B_DIR_PATH_DATAS, KDefine.B_DIR_PATH_AGREE_SCENE_BASE);
	public static readonly string AS_DATA_PATH_KOREAN_PERSONAL_TEXT = string.Format("{0}{1}AS_Personal_KO", KDefine.B_DIR_PATH_DATAS, KDefine.B_DIR_PATH_AGREE_SCENE_BASE);

	public static readonly string AS_DATA_PATH_ENGLISH_SERVICE_TEXT = string.Format("{0}{1}AS_Service_EN", KDefine.B_DIR_PATH_DATAS, KDefine.B_DIR_PATH_AGREE_SCENE_BASE);
	public static readonly string AS_DATA_PATH_ENGLISH_PERSONAL_TEXT = string.Format("{0}{1}AS_Personal_EN", KDefine.B_DIR_PATH_DATAS, KDefine.B_DIR_PATH_AGREE_SCENE_BASE);	
	// 경로 }
	#endregion			// 런타임 상수
}
#endif			// #if !USE_CUSTOM_PROJECT_OPTION
