using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 에디터 샘플 씬 관리자 */
public partial class CEditorSampleSceneManager : CSampleSceneManager {
	#region 프로퍼티
#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	public override float ScreenHeight => CSceneManager.ActiveSceneName.Equals(KCDefine.B_SCENE_N_EDITOR_SAMPLE) ? base.ScreenHeight * KCDefine.ES_SCALE_DESIGN_SCREEN_HEIGHT : base.ScreenHeight;
#endif // #if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	#endregion // 프로퍼티
}
