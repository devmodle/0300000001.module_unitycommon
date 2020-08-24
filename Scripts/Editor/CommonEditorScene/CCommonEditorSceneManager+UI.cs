using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

//! 공용 에디터 씬 관리자 - UI
public static partial class CCommonEditorSceneManager {
	#region 클래스 함수
	//! 기즈모를 그린다
	[DrawGizmo(GizmoType.Active | GizmoType.Selected | GizmoType.NonSelected | GizmoType.Pickable | GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
	public static void DrawGizmos(CSceneManager a_oSceneManager, GizmoType a_oGizmoType) {
		// 기즈모를 그릴 수 있을 경우
		if(CEditorAccess.IsEnableDrawGizmos()) {
			CFunc.EnumerateScenes((a_stScene) => {
				var oSceneManager = a_stScene.ExFindComponent<CSceneManager>(KCDefine.U_OBJ_NAME_SCENE_SCENE_MANAGER);

				// 씬 관리자가 존재 할 경우
				if(oSceneManager != null) {
					// 상태 갱신이 가능 할 경우
					if(CEditorAccess.IsEnableUpdateState()) {
						oSceneManager.EditorSetupScene();
					}

					// 메인 카메라가 존재 할 경우
					if(CSceneManager.MainCamera != null) {
						oSceneManager.EditorDrawGuideline();
					}
				}
			});
		}
	}
	#endregion			// 클래스 함수
}
#endif			// #if UNITY_EDITOR
