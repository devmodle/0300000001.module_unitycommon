using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if NEVER_USE_THIS
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEditor.SceneManagement;

//! 에디터 씬 관리자
[InitializeOnLoad]
public static partial class CEditorSceneManager {
	#region 클래스 변수
	private static float m_fSkipTime = 0.0f;
	private static ListRequest m_oListRequest = null;
	#endregion			// 클래스 변수

	#region 클래스 함수
	//! 생성자
	static CEditorSceneManager() {
		if(!Application.isBatchMode) {
			CEditorSceneManager.SetupCallbacks();
		}
	}

	//! 상태를 갱신한다
	public static void Update() {
		if(CEditorAccess.IsEnableUpdateState()) {
			var oMonoScripts = MonoImporter.GetAllRuntimeMonoScripts();
			CEditorSceneManager.m_fSkipTime += Time.unscaledDeltaTime;

			for(int i = 0; i < oMonoScripts.Length; ++i) {
				var oType = oMonoScripts[i].GetClass();

				if(oType != null && KEditorDefine.B_SCRIPT_ORDERS.ContainsKey(oType)) {
					CAccess.SetScriptOrder(oMonoScripts[i], KEditorDefine.B_SCRIPT_ORDERS[oType]);
				}
			}

			if(CEditorSceneManager.m_fSkipTime >= KCEditorDefine.B_DELTA_TIME_SCRIPT_M_SCENE_UPDATE) {
				CEditorSceneManager.m_fSkipTime = 0.0f;
				
				CFunc.EnumerateScenes((a_stScene) => {
					CSampleSceneManager.SetupSceneManager(a_stScene, KEditorDefine.B_SCENE_MANAGER_TYPE_LIST);
				});
			}
		}
	}

	//! 독립 패키지 상태를 갱신한다
	public static void UpdateDependencyState() {
		if(m_oListRequest.ExIsComplete()) {
			try {
				CEditorSceneManager.SetupDependencies();
			} finally {
				m_oListRequest = null;
				EditorApplication.update -= CEditorSceneManager.UpdateDependencyState;
			}
		}
	}

	//! 패키지 레지스트리 상태를 갱신한다
	public static void UpdateScopedRegistryState() {
		CEditorSceneManager.SetupScopedRegistries();
		EditorApplication.update -= CEditorSceneManager.UpdateScopedRegistryState;
	}

	//! 스크립트가 로드 되었을 경우
	[UnityEditor.Callbacks.DidReloadScripts]
	public static void OnLoadScript() {
		if(!Application.isBatchMode && CEditorAccess.IsEnableUpdateState()) {
			CEditorSceneManager.SetupCallbacks();
			CEditorSceneManager.m_oListRequest = Client.List();
		}
	}

	//! 씬이 열렸을 경우
	public static void OnSceneOpen(Scene a_stScene, OpenSceneMode a_eSceneMode) {
		CEditorSceneManager.OnLoadScript();

		// 패키지 레지스트리를 복사한다
		CFunc.CopyFile(KEditorDefine.B_UNITY_PKG_SRC_GOOGLE_SCOPED_REGISTRY_PATH, 
			KEditorDefine.B_UNITY_PKG_DEST_GOOGLE_SCOPED_REGISTRY_PATH, false);

		CEditorFunc.UpdateAssetDatabaseState();
	}
	#endregion			// 클래스 함수
}
#endif			// #if UNITY_EDITOR
#endif			// #if NEVER_USE_THIS
