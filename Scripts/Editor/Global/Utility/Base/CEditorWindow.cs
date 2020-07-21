using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//! 에디터 윈도우
public class CEditorWindow<T> : EditorWindow where T : CEditorWindow<T> {
	#region 클래스 변수
	private static T m_tInstance = null;
	#endregion			// 클래스 변수

	#region 함수
	//! 제거 되었을 경우
	public virtual void OnDestroy() {
		CEditorWindow<T>.m_tInstance = null;
	}
	#endregion			// 함수

	#region 클래스 함수
	//! 에디터 윈도우를 출력한다
	public static void ShowEditorWindow(string a_oName, Vector2 a_stMinSize, bool a_bIsImmediate = true) {
		if(CEditorWindow<T>.m_tInstance == null) {
			CEditorWindow<T>.m_tInstance = CEditorFactory.CreateEditorWindow<T>(a_oName,a_stMinSize);
		}

		CEditorWindow<T>.m_tInstance.Show(a_bIsImmediate);
		CEditorWindow<T>.m_tInstance.Focus();
	}
	#endregion			// 클래스 함수
}
