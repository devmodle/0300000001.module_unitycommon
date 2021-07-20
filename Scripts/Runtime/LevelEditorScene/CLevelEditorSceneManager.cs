using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR || UNITY_STANDALONE
//! 레벨 에디터 씬 관리자
public class CLevelEditorSceneManager : CSceneManager {
	#region 프로퍼티
	public GameObject PortraitUIs { get; private set; } = null;
	public GameObject LandscapeUIs { get; private set; } = null;

	public GameObject LeftEditorUIs { get; private set; } = null;
	public GameObject MidEditorUIs { get; private set; } = null;
	public GameObject RightEditorUIs { get; private set; } = null;

	public override bool IsIgnoreBlindV => true;
	public override bool IsIgnoreBlindH => true;
	
	public override string SceneName => KCDefine.B_SCENE_N_LEVEL_EDITOR;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			this.PortraitUIs = this.SubUIs.ExFindChild(KCDefine.E_OBJ_N_PORTRAIT_UIS);
			this.LandscapeUIs = this.SubUIs.ExFindChild(KCDefine.E_OBJ_N_LANDSCAPE_UIS);

			this.LeftEditorUIs = this.SubUIs.ExFindChild(KCDefine.E_OBJ_N_LEFT_EDITOR_UIS);
			this.MidEditorUIs = this.SubUIs.ExFindChild(KCDefine.E_OBJ_N_MID_EDITOR_UIS);
			this.RightEditorUIs = this.SubUIs.ExFindChild(KCDefine.E_OBJ_N_RIGHT_EDITOR_UIS);
		}
	}
	#endregion			// 함수
}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
