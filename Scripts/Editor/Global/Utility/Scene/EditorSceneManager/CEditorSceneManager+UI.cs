using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

//! 에디터 씬 관리자 - UI
public static partial class CEditorSceneManager {
	#region 클래스 함수
	//! 기즈모를 그린다
	[DrawGizmo(GizmoType.Active | GizmoType.Selected | GizmoType.NonSelected | GizmoType.Pickable | GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
	public static void DrawGizmos(CSceneManager a_oSceneManager, GizmoType a_oGizmoType) {
		if(CEditorAccess.IsEnableDrawGizmos()) {
			CFunc.EnumerateScenes((a_stScene) => {
				var oSceneManager = a_stScene.ExFindComponent<CSceneManager>(KCDefine.U_OBJ_NAME_SCENE_SCENE_MANAGER);

				if(oSceneManager != null) {
					if(CEditorAccess.IsEnableUpdateState()) {
						oSceneManager.EditorSetupScene();
					}

					if(Camera.main != null) {
						oSceneManager.EditorDrawGuideline();
					}
				}
			});
		}
	}
	#endregion			// 클래스 함수
}
#endif			// #if UNITY_EDITOR
