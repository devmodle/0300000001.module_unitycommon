using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//! 에디터 씬 관리자 - 접근
public static partial class CEditorSceneManager {
	#region 클래스 함수
	//! 상태 갱신 가능 여부를 검사한다
	public static bool IsEnableUpdateState() {
		return !Application.isPlaying && !EditorApplication.isCompiling && !BuildPipeline.isBuildingPlayer;
	}

	//! 기즈모 그리기 가능 여부를 검사한다
	public static bool IsEnableDrawGizmos() {
		return !EditorApplication.isCompiling && !BuildPipeline.isBuildingPlayer;
	}
	#endregion			// 클래스 함수
}
