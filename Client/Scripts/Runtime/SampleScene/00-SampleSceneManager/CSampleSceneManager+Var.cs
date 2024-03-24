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
	public override float ScreenWidth => this.IsSampleSceneEditor ? 
		KCDefine.B_DESIGN_P_SCREEN_WIDTH : base.ScreenWidth;

	public override float ScreenHeight => this.IsSampleSceneEditor ?
		KCDefine.B_DESIGN_P_SCREEN_HEIGHT : base.ScreenHeight;

	private bool IsSampleSceneEditor => CSceneManager.ActiveSceneName.Equals(KCDefine.B_SCENE_N_SAMPLE_EDITOR);
	#endregion // 프로퍼티
}
