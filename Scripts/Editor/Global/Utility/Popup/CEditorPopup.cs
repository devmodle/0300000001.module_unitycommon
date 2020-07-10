using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

//! 편집 팝업
public class CEditorPopup : EditorWindow {
	#region 변수
	private string m_oOriginObjName = string.Empty;
	private string m_oReplaceObjName = string.Empty;
	#endregion			// 변수

	#region 함수
	//! GUI 를 그린다
	public void OnGUI() {
		NGUIEditorTools.SetLabelWidth(KEditorDefine.B_WIDTH_EDITOR_W_NAME_TEXT_FIELD);

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
	[MenuItem("Utility/Popup/Show EditorPopup")]
	//! 편집 윈도우를 출력한다
	public static void ShowEditorPopup() {
		EditorFunc.ShowPopup<CEditorPopup>(KDefine.B_SCREEN_SIZE / 2.0f);
	}
	#endregion			// 클래스 함수	
}
