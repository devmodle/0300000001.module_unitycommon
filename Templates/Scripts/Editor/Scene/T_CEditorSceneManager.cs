using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

//! 에디터 씬 관리자
[InitializeOnLoad]
public static partial class CEditorSceneManager {
	#region 클래스 변수
	private static bool m_bIsEnableSetup = false;
	private static bool m_bIsSetupDependencies = false;

	private static float m_fSkipTime = 0.0f;
	private static float m_fDefineSymbolSkipTime = 0.0f;

	private static ListRequest m_oListRequest = null;
	private static List<AddRequest> m_oAddRequestList = new List<AddRequest>();
	#endregion			// 클래스 변수

	#region 클래스 함수
	//! 생성자
	static CEditorSceneManager() {
		CEditorSceneManager.SetupCallbacks();
	}

	//! 스크립트가 로드 되었을 경우
	[UnityEditor.Callbacks.DidReloadScripts]
	public static void OnLoadScript() {
		CEditorSceneManager.m_bIsEnableSetup = true;
	}

	//! 상태를 갱신한다
	private static void Update() {
		// 상태 갱신이 가능 할 경우
		if(CEditorAccess.IsEnableUpdateState) {
			CEditorSceneManager.m_fSkipTime += Time.deltaTime;
			var oMonoScripts = MonoImporter.GetAllRuntimeMonoScripts();

			for(int i = 0; i < oMonoScripts.Length; ++i) {
				// 스크립트가 유효하지 않을 경우
				if(oMonoScripts[i] == null) {
					continue;
				}

				var oType = oMonoScripts[i].GetClass();

				// 스크립트 순서 설정이 가능 할 경우
				if(oType != null && KEditorDefine.B_SCRIPT_ORDERS.ContainsKey(oType)) {
					CAccess.SetScriptOrder(oMonoScripts[i], KEditorDefine.B_SCRIPT_ORDERS[oType]);
				}
			}

			// 갱신 주기가 지났을 경우
			if(CEditorSceneManager.m_fSkipTime.ExIsGreateEquals(KCEditorDefine.B_DELTA_T_SCENE_M_SCRIPT_UPDATE)) {
				EditorFactory.CreateSaleItemInfoTable();
				EditorFactory.CreateSaleProductInfoTable();
				EditorFactory.CreateMissionInfoTable();
				EditorFactory.CreateRewardInfoTable();
				EditorFactory.CreateEpisodeInfoTable();

				CEditorSceneManager.m_fSkipTime = KCDefine.B_VAL_0_FLT;

				// 상태 갱신이 가능 할 경우
				if(CEditorSceneManager.m_bIsEnableSetup) {
					CEditorSceneManager.m_bIsEnableSetup = false;
					CEditorSceneManager.m_oListRequest = Client.List();

					CEditorSceneManager.SetupCallbacks();
				}
				
				CFunc.EnumerateScenes((a_stScene) => {
					CSampleSceneManager.SetupSceneManager(a_stScene, KEditorDefine.B_SCENE_MANAGER_TYPES);
					return true;
				});
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
		bool bIsEnableUpdate = CEditorSceneManager.m_bIsSetupDependencies;
		bIsEnableUpdate = bIsEnableUpdate && CEditorSceneManager.m_oAddRequestList.Count <= KCDefine.B_VAL_0_INT;

		// 상태 갱신이 가능 할 경우
		if(bIsEnableUpdate && CEditorAccess.IsEnableUpdateState) {
			CEditorSceneManager.m_fDefineSymbolSkipTime += Time.deltaTime;

			var oAsset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(KCEditorDefine.B_ASSET_P_DEFINE_SYMBOL_TABLE);
			bool bIsEnable = oAsset != null && CEditorSceneManager.m_fDefineSymbolSkipTime.ExIsGreateEquals(KEditorDefine.B_DELAY_DEFINE_S_UPDATE);

			// 전처리기 심볼 테이블 갱신이 가능 할 경우
			if(bIsEnable && CPlatformOptsSetter.DefineSymbolTable != null) {
				bool bIsNeedUpdate = false;

				CEditorSceneManager.m_bIsSetupDependencies = false;
				CEditorSceneManager.m_fDefineSymbolSkipTime = KCDefine.B_VAL_0_FLT;

				var oDefineSymbolLists = new List<string>[] {
					CPlatformOptsSetter.DefineSymbolTable.EditorCommonDefineSymbolList,
					CPlatformOptsSetter.DefineSymbolTable.EditorSubCommonDefineSymbolList,
					
					CPlatformOptsSetter.DefineSymbolTable.EditorStandaloneDefineSymbolList,
					CPlatformOptsSetter.DefineSymbolTable.EditorMacDefineSymbolList,
					CPlatformOptsSetter.DefineSymbolTable.EditorWndsDefineSymbolList,

					CPlatformOptsSetter.DefineSymbolTable.EditoriOSDefineSymbolList,
					
					CPlatformOptsSetter.DefineSymbolTable.EditorAndroidDefineSymbolList,
					CPlatformOptsSetter.DefineSymbolTable.EditorGoogleDefineSymbolList,
					CPlatformOptsSetter.DefineSymbolTable.EditorOneStoreDefineSymbolList,
					CPlatformOptsSetter.DefineSymbolTable.EditorGalaxyStoreDefineSymbolList
				};

				foreach(var stKeyVal in KCEditorDefine.DS_REPLACE_DEFINE_S_MODULES) {
					for(int i = 0; i < oDefineSymbolLists.Length; ++i) {
						var oDefineSymbolList = oDefineSymbolLists[i];
						
						// 전처리기 심볼 갱신이 필요 할 경우
						if(oDefineSymbolList.Contains(stKeyVal.Key)) {
							bIsNeedUpdate = true;
							oDefineSymbolList.ExReplaceVal(stKeyVal.Key, stKeyVal.Value);
						}
					}
				}
				
				// 전처리기 심볼 갱신이 필요 할 경우
				if(bIsNeedUpdate) {
					EditorUtility.SetDirty(oAsset);
					
					CEditorFunc.UpdateAssetDBState();
					CPlatformOptsSetter.SetupDefineSymbols();
				}
			}
		}
	}
	#endregion			// 클래스 함수
}
#endif			// #if UNITY_EDITOR
#endif			// #if NEVER_USE_THIS
