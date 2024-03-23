using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif // #if UNITY_EDITOR

/** 샘플 씬 관리자 - 변수 */
public partial class CSampleSceneManager : CSceneManager
{
	#region 프로퍼티
	public override float ScreenWidth => this.IsEditorSampleScene ? 
		KCDefine.B_DESIGN_P_SCREEN_WIDTH : base.ScreenWidth;

	public override float ScreenHeight => this.IsEditorSampleScene ?
		KCDefine.B_DESIGN_P_SCREEN_HEIGHT : base.ScreenHeight;

	private bool IsEditorSampleScene => CSceneManager.ActiveSceneName.Equals(KCDefine.B_EDITOR_SCENE_N_SAMPLE);
	#endregion // 프로퍼티
}
