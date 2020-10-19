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
	private static bool m_bIsSetupDependencies = false;

	private static float m_fSkipTime = 0.0f;
	private static float m_fDefineSymbolSkipTime = 0.0f;

	private static ListRequest m_oListRequest = null;
	#endregion			// 클래스 변수

	#region 클래스 함수
	//! 생성자
	static CEditorSceneManager() {
		// 배치 모드가 아닐 경우
		if(!Application.isBatchMode) {
			CEditorSceneManager.SetupCallbacks();
		}
	}

	//! 스크립트가 로드 되었을 경우
	[UnityEditor.Callbacks.DidReloadScripts]
	public static void OnLoadScript() {
		// 상태 갱신이 가능 할 경우
		if(!Application.isBatchMode && CEditorAccess.IsEnableUpdateState()) {
			CEditorSceneManager.SetupCallbacks();
			CEditorSceneManager.m_oListRequest = Client.List();
		}
	}

	//! 상태를 갱신한다
	private static void Update() {
		// 상태 갱신이 가능 할 경우
		if(CEditorAccess.IsEnableUpdateState()) {
			CEditorSceneManager.m_fSkipTime += Time.deltaTime;
			var oMonoScripts = MonoImporter.GetAllRuntimeMonoScripts();

			for(int i = KCDefine.B_VALUE_INT_0; i < oMonoScripts.Length; ++i) {
				var oType = oMonoScripts[i].GetClass();

				// 스크립트 순서 설정이 가능 할 경우
				if(oType != null && KEditorDefine.B_SCRIPT_ORDERS.ContainsKey(oType)) {
					CAccess.SetScriptOrder(oMonoScripts[i], KEditorDefine.B_SCRIPT_ORDERS[oType]);
				}
			}

			// 갱신 주기가 지났을 경우
			if(CEditorSceneManager.m_fSkipTime.ExIsGreateEquals(KCEditorDefine.B_DELTA_TIME_SCRIPT_M_SCENE_UPDATE)) {
				CEditorSceneManager.m_fSkipTime = KCDefine.B_VALUE_FLOAT_0;
				
				CFunc.EnumerateScenes((a_stScene) => 
					CSampleSceneManager.SetupSceneManager(a_stScene, KEditorDefine.B_SCENE_MANAGER_TYPE_LIST));
			}
		}
	}

	//! 독립 패키지 상태를 갱신한다
	private static void UpdateDependencyState() {
		// 리스트 요청이 완료 되었을 경우
		if(m_oListRequest.ExIsComplete()) {
			try {
				CEditorSceneManager.SetupDependencies();
			} finally {
				CEditorSceneManager.m_oListRequest = null;
				CEditorSceneManager.m_bIsSetupDependencies = true;

				EditorApplication.update -= CEditorSceneManager.UpdateDependencyState;
			}
		}
	}

	//! 패키지 레지스트리 상태를 갱신한다
	private static void UpdateScopedRegistryState() {
		CEditorSceneManager.SetupScopedRegistries();
		EditorApplication.update -= CEditorSceneManager.UpdateScopedRegistryState;
	}

	//! 상태를 갱신한다
	private static void LateUpdate() {
		bool bIsEnableUpdate = CEditorSceneManager.m_bIsSetupDependencies && 
			CEditorAccess.IsEnableUpdateState();

		// 상태 갱신이 가능 할 경우
		if(bIsEnableUpdate && m_oListRequest == null) {
			CEditorSceneManager.m_fDefineSymbolSkipTime += Time.deltaTime;

			var oAsset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(KCEditorDefine.B_ASSET_PATH_DEFINE_SYMBOL_TABLE);
			bool bIsEnable = CEditorSceneManager.m_fDefineSymbolSkipTime.ExIsGreateEquals(KEditorDefine.B_DELAY_DEFINE_S_UPDATE);

			// 전처리기 심볼 테이블 갱신이 가능 할 경우
			if(bIsEnable && oAsset != null && CCommonPlatformOptsSetter.DefineSymbolTable != null) {
				bool bIsNeedUpdate = false;

				CEditorSceneManager.m_bIsSetupDependencies = false;
				CEditorSceneManager.m_fDefineSymbolSkipTime = KCDefine.B_VALUE_FLOAT_0;

				var oDefineSymbolListContainer = new List<string>[] {
					CCommonPlatformOptsSetter.DefineSymbolTable.EditorCommonDefineSymbolList,
					CCommonPlatformOptsSetter.DefineSymbolTable.EditorSubCommonDefineSymbolList,
					
					CCommonPlatformOptsSetter.DefineSymbolTable.EditorStandaloneDefineSymbolList,
					CCommonPlatformOptsSetter.DefineSymbolTable.EditorMacDefineSymbolList,
					CCommonPlatformOptsSetter.DefineSymbolTable.EditorWindowsDefineSymbolList,

					CCommonPlatformOptsSetter.DefineSymbolTable.EditoriOSDefineSymbolList,
					
					CCommonPlatformOptsSetter.DefineSymbolTable.EditorAndroidDefineSymbolList,
					CCommonPlatformOptsSetter.DefineSymbolTable.EditorGoogleDefineSymbolList,
					CCommonPlatformOptsSetter.DefineSymbolTable.EditorOneStoreDefineSymbolList,
					CCommonPlatformOptsSetter.DefineSymbolTable.EditorGalaxyStoreDefineSymbolList
				};

				foreach(var stKeyValue in KCEditorDefine.DS_REPLACE_DEFINE_S_MODULE_LIST) {
					for(int i = KCDefine.B_VALUE_INT_0; i < oDefineSymbolListContainer.Length; ++i) {
						var oDefineSymbolList = oDefineSymbolListContainer[i];

						// 전처리기 심볼 갱신이 필요 할 경우
						if(oDefineSymbolList.Contains(stKeyValue.Key)) {
							bIsNeedUpdate = true;
							oDefineSymbolList.ExReplaceValue(stKeyValue.Key, stKeyValue.Value);
						}
					}
				}
				
				// 전처리기 심볼 갱신이 필요 할 경우
				if(bIsNeedUpdate) {
					EditorUtility.SetDirty(oAsset);
					
					CEditorFunc.UpdateAssetDBState();
					CCommonPlatformOptsSetter.SetupDefineSymbols();
				}
			}
		}
	}

	//! 씬이 열렸을 경우
	private static void OnSceneOpen(Scene a_stScene, OpenSceneMode a_eSceneMode) {
		CEditorSceneManager.OnLoadScript();

		// 패키지 레지스트리를 복사한다
		CFunc.CopyFile(KEditorDefine.B_UNITY_PKG_SRC_GOOGLE_SCOPED_REGISTRY_PATH, 
			KEditorDefine.B_UNITY_PKG_DEST_GOOGLE_SCOPED_REGISTRY_PATH, false);
	}
	#endregion			// 클래스 함수
}
#endif			// #if UNITY_EDITOR
#endif			// #if NEVER_USE_THIS
