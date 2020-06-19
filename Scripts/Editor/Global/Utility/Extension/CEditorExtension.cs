using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif			// #if UNITY_IOS

//! 에디터 기본 확장 클래스
public static partial class CEditorExtension {
	#region 클래스 함수
	//! 직렬화 속성 값을 변경한다
	public static void ExSetPropertyValue(this SerializedObject a_oSender, string a_oName, System.Action<SerializedProperty> a_oCallback) {
		var oProperty = a_oSender?.FindProperty(a_oName);
		Function.Assert(oProperty != null);

		// 값을 갱신한다 {
		a_oCallback?.Invoke(oProperty);

		a_oSender.ApplyModifiedProperties();
		a_oSender.Update();
		// 값을 갱신한다 }
	}
	#endregion			// 클래스 함수

	#region 조건부 클래스 함수
#if UNITY_IOS
	//! 유효 여부를 검사한다
	public static bool ExIsValid(this PlistDocument a_oSender) {
		return a_oSender != null && a_oSender.root != null;
	}
#endif			// #if UNITY_IOS
	#endregion			// 조건부 클래스 함수
}
