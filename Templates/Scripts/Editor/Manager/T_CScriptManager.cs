using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if NEVER_USE_THIS
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

//! 스크립트 관리자
[InitializeOnLoad]
public static partial class CScriptManager {
	#region 클래스 변수
	private static float m_fSkipTime = 0.0f;
	#endregion			// 클래스 변수

	#region 클래스 함수
	//! 생성자
	static CScriptManager() {
		if(!Application.isBatchMode) {
			EditorApplication.update -= CScriptManager.Update;
			EditorApplication.update += CScriptManager.Update;
		}
	}

	//! 상태를 갱신한다
	public static void Update() {
		if(CEditorAccess.IsEnableUpdateState()) {
			var oMonoScripts = MonoImporter.GetAllRuntimeMonoScripts();
			CScriptManager.m_fSkipTime += Time.unscaledDeltaTime;

			for(int i = 0; i < oMonoScripts.Length; ++i) {
				var oType = oMonoScripts[i].GetClass();

				if(oType != null && KEditorDefine.B_SCRIPT_ORDERS.ContainsKey(oType)) {
					CAccess.SetScriptOrder(oMonoScripts[i], KEditorDefine.B_SCRIPT_ORDERS[oType]);
				}
			}

			if(CScriptManager.m_fSkipTime >= KCEditorDefine.B_DELTA_TIME_SCRIPT_M_SCENE_UPDATE) {
				CScriptManager.m_fSkipTime = 0.0f;
				
				CFunc.EnumerateScenes((a_stScene) => {
					CSampleSceneManager.SetupSceneManager(a_stScene, KEditorDefine.B_SCENE_MANAGER_TYPE_LIST);
				});
			}
		}
	}
	#endregion			// 클래스 함수
}
#endif			// #if UNITY_EDITOR
#endif			// #if NEVER_USE_THIS
