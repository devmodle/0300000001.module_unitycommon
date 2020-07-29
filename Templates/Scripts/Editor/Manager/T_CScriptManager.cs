using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if NEVER_USE_THIS
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

//! 스크립트 관리자
[InitializeOnLoad]
public static partial class CScriptManager {
	#region 클래스 함수
	//! 생성자
	static CScriptManager() {
		if(!Application.isBatchMode) {
			EditorApplication.update -= CScriptManager.Update;
			EditorApplication.update += CScriptManager.Update;

			EditorSceneManager.sceneOpened -= CScriptManager.OnSceneOpen;
			EditorSceneManager.sceneOpened += CScriptManager.OnSceneOpen;
		}
	}

	//! 상태를 갱신한다
	public static void Update() {
		if(CEditorAccess.IsEnableUpdateState()) {
			var oMonoScripts = MonoImporter.GetAllRuntimeMonoScripts();

			for(int i = 0; i < oMonoScripts.Length; ++i) {
				var oType = oMonoScripts[i].GetClass();

				if(oType != null && KEditorDefine.B_SCRIPT_ORDERS.ContainsKey(oType)) {
					CAccess.SetScriptOrder(oMonoScripts[i], KEditorDefine.B_SCRIPT_ORDERS[oType]);
				}
			}
		}
	}

	//! 씬이 열렸을 경우
	public static void OnSceneOpen(Scene a_stScene, OpenSceneMode a_eSceneMode) {
		CSampleSceneManager.SetupSceneManager(a_stScene, KEditorDefine.B_SCENE_MANAGER_TYPE_LIST);
	}
	#endregion			// 클래스 함수
}
#endif			// #if UNITY_EDITOR
#endif			// #if NEVER_USE_THIS
