using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 에디터 샘플 씬 관리자 */
public partial class CEditorSampleSceneManager : CSampleSceneManager {
	#region 프로퍼티
	public override float ScreenHeight => CSceneManager.ActiveSceneName.Equals(KCDefine.B_SCENE_N_EDITOR_SAMPLE) ? base.ScreenHeight * KCDefine.LES_SCALE_SCREEN_SIZE : base.ScreenHeight;
	#endregion // 프로퍼티
}
