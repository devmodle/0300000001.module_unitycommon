using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

//! 에디터 씬 관리자 - UI
public static partial class CEditorSceneManager {
	#region 클래스 함수
	//! 기즈모를 그린다
	[DrawGizmo(GizmoType.Active | GizmoType.Selected | GizmoType.NonSelected | GizmoType.Pickable | GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
	public static void DrawGizmos(CSceneManager a_oSceneManager, GizmoType a_oGizmoType) {
		if(CEditorSceneManager.IsEnableUpdateState()) {
			for(int i = 0; i < SceneManager.sceneCount; ++i) {
				var stScene = SceneManager.GetSceneAt(i);
				var oSceneManager = stScene.ExFindComponent<CSceneManager>(KDefine.U_OBJ_NAME_SCENE_SCENE_MANAGER);

				if(oSceneManager != null) {
					oSceneManager.EditorSetupScene();

					if(Camera.main != null) {
						oSceneManager.EditorDrawGuideline();
					}
				}
			}
		}
	}
	#endregion			// 클래스 함수
}
