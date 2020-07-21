using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//! 자동 씬 추가자
[InitializeOnLoad]
public static class CAutoSceneImporter {
	#region 클래스 함수
	//! 클래스 생성자
	static CAutoSceneImporter() {
		EditorApplication.projectChanged -= CAutoSceneImporter.ImportAllScenes;
		EditorApplication.projectChanged += CAutoSceneImporter.ImportAllScenes;
	}

	//! 모든 씬을 추가한다
	private static void ImportAllScenes() {
		var oAssetGUIDs = AssetDatabase.FindAssets(KCEditorDefine.SCENE_NAME_PATTERN, new string[] {
			KCEditorDefine.DIR_PATH_AUTO_SCENES,
			KCEditorDefine.DIR_PATH_SCENES,

#if EDITOR_ENABLE
			KCEditorDefine.DIR_PATH_EDITOR_SCENES
#endif			// #if EDITOR_ENABLE
		});

		var oEditorBuildSettingsScenes = new EditorBuildSettingsScene[oAssetGUIDs.Length];

		for(int i = 0; i < oAssetGUIDs.Length; ++i) {
			string oFilepath = AssetDatabase.GUIDToAssetPath(oAssetGUIDs[i]);
			oEditorBuildSettingsScenes[i] = new EditorBuildSettingsScene(oFilepath, true);
		}

		EditorBuildSettings.scenes = oEditorBuildSettingsScenes;
	}
	#endregion			// 클래스 함수
}
