using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

//! 객체 이름 에디터 윈도우
public class CObjNameEditorWindow : CEditorWindow<CObjNameEditorWindow> {
	#region 변수
	private string m_oOriginObjName = string.Empty;
	private string m_oReplaceObjName = string.Empty;
	#endregion			// 변수

	#region 함수
	//! GUI 를 그린다
	public void OnGUI() {
#if NGUI_ENABLE
		NGUIEditorTools.SetLabelWidth(KEditorDefine.B_WIDTH_EDITOR_W_NAME_TEXT_FIELD);
#endif			// #if NGUI_ENABLE

		m_oOriginObjName = EditorGUILayout.TextField("검색 문자열", m_oOriginObjName, GUILayout.Width(KEditorDefine.B_TOTAL_WIDTH_EDITOR_W_NAME_TEXT_FIELD));
		m_oReplaceObjName = EditorGUILayout.TextField("변경 문자열", m_oReplaceObjName, GUILayout.Width(KEditorDefine.B_TOTAL_WIDTH_EDITOR_W_NAME_TEXT_FIELD));

		if(GUILayout.Button("적용", GUILayout.Width(KEditorDefine.B_WIDTH_EDITOR_W_APPLY_BUTTON))) {
			if(m_oOriginObjName.ExIsValid() && m_oReplaceObjName.ExIsValid()) {
				Func.EnumerateScenes((a_stScene) => {
					this.ReplaceSceneObjsName(a_stScene, m_oOriginObjName, m_oReplaceObjName);
					EditorSceneManager.MarkSceneDirty(a_stScene);
				});
			}
		}
	}

	//! 씬 객체 이름을 변경한다
	private void ReplaceSceneObjsName(Scene a_stScene, string a_oOriginName, string a_oReplaceName) {
		var oObjList = a_stScene.ExGetChildren();

		for(int i = 0; i < oObjList.Count; ++i) {
			oObjList[i].name = oObjList[i].name.ExGetReplaceString(a_oOriginName, a_oReplaceName);
		}
	}
	#endregion			// 함수

	#region 클래스 함수
	//! 객체 이름 에디터 윈도우를 출력한다
	[MenuItem("Utility/Editor Window/Show ObjNameEditorWindow")]
	public static void ShowObjNameEditorWindow() {
		CObjNameEditorWindow.ShowEditorWindow(KEditorDefine.B_OBJ_NAME_OBJ_NAME_EDITOR_POPUP, 
			KEditorDefine.B_MIN_SIZE_EDITOR_WINDOW);
	}
	#endregion			// 클래스 함수
}
