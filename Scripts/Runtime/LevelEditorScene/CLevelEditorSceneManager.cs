using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 레벨 에디터 씬 관리자
public class CLevelEditorSceneManager : CSceneManager {
	#region 프로퍼티
	public override bool IsIgnoreBlindV => true;
	public override bool IsIgnoreBlindH => true;
	
	public override string SceneName => KCDefine.B_SCENE_N_LEVEL_EDITOR;
	#endregion			// 프로퍼티
}
