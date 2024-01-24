#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR && EXTRA_SCRIPT_MODULE_ENABLE
using UnityEditor;

/** 추가 에디터 씬 관리자 */
[InitializeOnLoad]
public static partial class CExtraEditorSceneManager {
	#region 클래스 변수

	#endregion // 클래스 변수

	#region 클래스 함수
	/** 생성자 */
	static CExtraEditorSceneManager() {
		// 플레이 모드 일 경우
		if(EditorApplication.isPlaying) {
			return;
		}
		
		CExtraEditorSceneManager.SetupCallbacks();
	}

	/** 상태를 갱신한다 */
	private static void Update() {
		// 상태 갱신이 불가능 할 경우
		if(CEditorAccess.IsEnableUpdateState) {
			return;
		}
	}

	/** 상태를 갱신한다 */
	private static void LateUpdate() {
		// 상태 갱신이 불가능 할 경우
		if(CEditorAccess.IsEnableUpdateState) {
			return;
		}
	}

	/** 콜백을 설정한다 */
	private static void SetupCallbacks() {
		EditorApplication.update -= CExtraEditorSceneManager.Update;
		EditorApplication.update += CExtraEditorSceneManager.Update;

		EditorApplication.update -= CExtraEditorSceneManager.LateUpdate;
		EditorApplication.update += CExtraEditorSceneManager.LateUpdate;
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR && EXTRA_SCRIPT_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
