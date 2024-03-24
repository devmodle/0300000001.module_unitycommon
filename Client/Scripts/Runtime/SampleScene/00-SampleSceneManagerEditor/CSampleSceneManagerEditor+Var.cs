using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 에디터 샘플 씬 관리자 - 변수 */
public partial class CSampleSceneManagerEditor : CSampleSceneManager
{
	#region 프로퍼티
#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	public override float ScreenHeight => this.IsSampleSceneEditor ? 
		base.ScreenHeight * KCDefine.ES_SCALE_DESIGN_SCREEN_HEIGHT : base.ScreenHeight;

	private bool IsSampleSceneEditor => CSceneManager.ActiveSceneName.Equals(KCDefine.B_SCENE_N_SAMPLE_EDITOR);
#endif // #if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	#endregion // 프로퍼티
}
