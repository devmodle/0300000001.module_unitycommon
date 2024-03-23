using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 에디터 샘플 씬 관리자 - 변수 */
public partial class CEditorSampleSceneManager : CSampleSceneManager
{
	#region 프로퍼티
#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	public override float ScreenHeight => this.IsEditorSampleScene ? 
		base.ScreenHeight * KCDefine.ES_SCALE_DESIGN_SCREEN_HEIGHT : base.ScreenHeight;

	private bool IsEditorSampleScene => CSceneManager.ActiveSceneName.Equals(KCDefine.B_EDITOR_SCENE_N_SAMPLE);
#endif // #if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	#endregion // 프로퍼티
}
