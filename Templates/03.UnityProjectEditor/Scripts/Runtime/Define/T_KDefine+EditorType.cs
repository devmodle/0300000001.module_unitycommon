#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_STANDALONE && EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
using MessagePack;

#region 기본
/** 에디터 타입 랩퍼 */
[MessagePackObject]
public partial struct STEditorTypeWrapper {
	// Do Something
}
#endregion			// 기본
#endif			// #if UNITY_STANDALONE && EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
#endif			// #if SCRIPT_TEMPLATE_ONLY
