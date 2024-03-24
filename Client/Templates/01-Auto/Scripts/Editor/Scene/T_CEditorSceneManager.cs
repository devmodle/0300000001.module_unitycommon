#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEditor.SceneManagement;

using EnhancedHierarchy;

/** 에디터 씬 관리자 */
[InitializeOnLoad]
public static partial class CEditorSceneManager {
	#region 클래스 변수
	private static bool m_bIsEnableSetup = false;
	private static bool m_bIsEnableSetupDependencies = false;

	private static double m_dblSkipTimeUpdate = 0.0;
	private static double m_dblSkipTimeDependency = 0.0;
	private static double m_dblSkipTimeDefineSymbol = 0.0;

	private static ListRequest m_oListRequest = null;
	private static List<string> m_oListSceneNameSample = new List<string>();
	private static List<AddRequest> m_oListAddRequest = new List<AddRequest>();
	#endregion // 클래스 변수

	#region 클래스 함수
	/** 생성자 */
	static CEditorSceneManager() {
		// 플레이 모드 일 경우
		if(EditorApplication.isPlaying) {
			goto EDITOR_SCENE_MANAGER_CONSTRUCTOR_EXIT_FINAL;
		}

		CEditorSceneManager.m_dblSkipTimeUpdate = EditorApplication.timeSinceStartup;
		CEditorSceneManager.m_dblSkipTimeDependency = EditorApplication.timeSinceStartup;
		CEditorSceneManager.m_dblSkipTimeDefineSymbol = EditorApplication.timeSinceStartup;

		CEditorSceneManager.m_oListSceneNameSample.ExAddVal(KCDefine.B_SCENE_N_SAMPLE);
		CEditorSceneManager.m_oListSceneNameSample.ExAddVal(KCDefine.B_SCENE_N_SAMPLE_MENU);
		CEditorSceneManager.m_oListSceneNameSample.ExAddVal(KCDefine.B_SCENE_N_SAMPLE_RESEARCH);
		CEditorSceneManager.m_oListSceneNameSample.ExAddVal(KCDefine.B_SCENE_N_SAMPLE_EDITOR);

EDITOR_SCENE_MANAGER_CONSTRUCTOR_EXIT_FINAL:
		CEditorSceneManager.SetupCallbacks();
	}

	/** 상태를 갱신한다 */
	private static void Update() {
		// 상태 갱신이 불가능 할 경우
		if(!CEditorAccess.IsEnableUpdateState) {
			goto EDITOR_SCENE_MANAGER_UPDATE_EXIT_FINAL;
		}

		// 설정이 불가능 할 경우
		if(!CEditorSceneManager.m_bIsEnableSetup) {
			goto EDITOR_SCENE_MANAGER_UPDATE_EXIT_A;
		}

		Preferences.Tooltips.Value = false;
		Preferences.SelectOnTree.Value = false;

		CEditorSceneManager.m_bIsEnableSetup = false;
		CEditorSceneManager.m_bIsEnableSetupDependencies = true;

		CEditorSceneManager.m_oListRequest = Client.List(false, false);

EDITOR_SCENE_MANAGER_UPDATE_EXIT_A:
		double dblUpdateDeltaTime = EditorApplication.timeSinceStartup - CEditorSceneManager.m_dblSkipTimeUpdate;

		// 상태 갱신이 불가능 할 경우
		if(dblUpdateDeltaTime.ExIsLess(KCDefine.B_VAL_3_REAL)) {
			goto EDITOR_SCENE_MANAGER_UPDATE_EXIT_FINAL;
		}

		CEditorSceneManager.m_dblSkipTimeUpdate = EditorApplication.timeSinceStartup;

#if UIS_ROOT_PREFAB_ENABLE
		CAccess.EnumerateRootObjs((a_oObj) => {
			bool bIsRootPrefabObjA = KCEditorDefine.B_OBJ_N_ROOT_PREFAB_OBJ_LIST.Contains(a_oObj.name);
			bool bIsRootPrefabObjB = bIsRootPrefabObjA && !CEditorSceneManager.m_oListSceneNameSample.Contains(a_oObj.scene.name);

			// 최상단 프리팹 객체 일 경우
			if(bIsRootPrefabObjA && bIsRootPrefabObjB) {
				CEditorSceneManager.SetupPrefabObj(a_oObj);
			}

			return true;
		});
#endif // #if UIS_ROOT_PREFAB_ENABLE

#if EXTRA_SCRIPT_MODULE_ENABLE
		CAccess.EnumerateScenes((a_stScene) => {
			CSampleSceneManager.SetupSceneManager(a_stScene, KEditorDefine.G_EXTRA_SCENE_MANAGER_TYPE_DICT);
			return true;
		});
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE

		CAccess.EnumerateScenes((a_stScene) => {
			var oUIsRoot = a_stScene.ExFindChild(KCDefine.U_OBJ_N_SCENE_UIS_ROOT);
			string oPrefabPath = (oUIsRoot == null) ? CEditorAccess.GetRootObjPrefabPath(a_stScene, KCDefine.U_OBJ_N_SCENE_UIS_ROOT) : string.Empty;

			// 최상단 UI 가 없을 경우
			if(oUIsRoot == null && CEditorAccess.IsExistsAsset(oPrefabPath)) {
				EditorSceneManager.MarkSceneDirty(a_stScene);
				CEditorFactory.CreatePrefabInstance(KCDefine.U_OBJ_N_SCENE_UIS_ROOT, CEditorAccess.FindAsset<GameObject>(oPrefabPath), null);
			}

			CSampleSceneManager.SetupSceneManager(a_stScene, KEditorDefine.B_SCENE_MANAGER_TYPE_DICT);
			return true;
		});

		// 스크립트 순서를 설정한다 {
		var oMonoScripts = MonoImporter.GetAllRuntimeMonoScripts();

		for(int i = 0; i < oMonoScripts.Length; ++i) {
			// 순서 설정이 불가능 할 경우
			if(oMonoScripts[i] == null) {
				continue;
			}

			var oType = oMonoScripts[i].GetClass();

			// 스크립트 순서 설정이 가능 할 경우
			if(oType != null && KEditorDefine.B_SCRIPT_ORDER_DICT.TryGetValue(oType, out int nOrder)) {
				CAccess.SetOrderScript(oMonoScripts[i], nOrder);
			}

#if EXTRA_SCRIPT_MODULE_ENABLE
			// 스크립트 순서 설정이 가능 할 경우
			if(oType != null && KEditorDefine.G_EXTRA_SCRIPT_ORDER_DICT.TryGetValue(oType, out int nExtraOrder)) {
				CAccess.SetOrderScript(oMonoScripts[i], nExtraOrder);
			}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE
		}
		// 스크립트 순서를 설정한다 }

EDITOR_SCENE_MANAGER_UPDATE_EXIT_FINAL:
		return;
	}

	/** 상태를 갱신한다 */
	private static void LateUpdate() {
		bool bIsEnableUpdate = CEditorAccess.IsEnableUpdateState && !CEditorSceneManager.m_oListAddRequest.ExIsValid();
		CEditorSceneManager.m_dblSkipTimeDefineSymbol = bIsEnableUpdate ? CEditorSceneManager.m_dblSkipTimeDefineSymbol : EditorApplication.timeSinceStartup;

		for(int i = 0; i < CEditorSceneManager.m_oListAddRequest.Count; ++i) {
			// 에러가 존재 할 경우
			if(CEditorSceneManager.m_oListAddRequest[i].Error != null) {
				CFunc.ShowLogWarning($"CEditorSceneManager.LateUpdate: {CEditorSceneManager.m_oListAddRequest[i].Error.message}");
				CEditorSceneManager.m_oListAddRequest.ExRemoveValAt(i, false);

				break;
			}
		}

		// 상태 갱신이 가능 할 경우
		if(bIsEnableUpdate && (EditorApplication.timeSinceStartup - CEditorSceneManager.m_dblSkipTimeDefineSymbol).ExIsGreatEquals(KCDefine.B_VAL_1_REAL)) {
			var oDefineSymbolInfoTable = CEditorAccess.FindAsset<CDefineSymbolInfoTable>(KCEditorDefine.B_ASSET_P_DEFINE_SYMBOL_INFO_TABLE);

			// 전처리기 심볼 정보 테이블이 존재 할 경우
			if(oDefineSymbolInfoTable != null) {
				CEditorSceneManager.m_dblSkipTimeDefineSymbol = EditorApplication.timeSinceStartup;

				foreach(var stKeyVal in KCEditorDefine.DS_DEFINE_S_REPLACE_MODULE_DICT) {
					var oDefineSymbolLists = new List<List<string>>() {
						oDefineSymbolInfoTable.EditorCommonDefineSymbolList,
						oDefineSymbolInfoTable.EditorSubCommonDefineSymbolList,

						oDefineSymbolInfoTable.EditoriOSAppleDefineSymbolList,

						oDefineSymbolInfoTable.EditorAndroidDefineSymbolList,
						oDefineSymbolInfoTable.EditorAndroidGoogleDefineSymbolList,
						oDefineSymbolInfoTable.EditorAndroidAmazonDefineSymbolList,

						oDefineSymbolInfoTable.EditorStandaloneDefineSymbolList,
						oDefineSymbolInfoTable.EditorStandaloneMacSteamDefineSymbolList,
						oDefineSymbolInfoTable.EditorStandaloneWndsSteamDefineSymbolList
					};

					for(int i = 0; i < oDefineSymbolLists.Count; ++i) {
						// 전처리기 심볼 갱신이 필요 할 경우
						if(oDefineSymbolLists[i].Contains(stKeyVal.Key)) {
							EditorUtility.SetDirty(oDefineSymbolInfoTable);
							oDefineSymbolLists[i].ExReplaceVal(stKeyVal.Key, stKeyVal.Value);
						}
					}
				}

				// 전처리기 심볼 갱신이 필요 할 경우
				if(EditorUtility.IsDirty(oDefineSymbolInfoTable)) {
					CEditorFunc.UpdateAssetDBState();
				}
			}
		}
	}

	/** 독립 패키지 상태를 갱신한다 */
	private static void UpdateDependencyState() {
		// 상태 갱신이 가능 할 경우
		if(CEditorAccess.IsEnableUpdateState) {
			bool bIsEnableSetup = CEditorSceneManager.m_bIsEnableSetupDependencies && (CEditorSceneManager.m_oListRequest != null && CEditorSceneManager.m_oListRequest.Result != null && CEditorSceneManager.m_oListRequest.IsCompleted);

			// 갱신 주기가 지났을 경우
			if(bIsEnableSetup && (EditorApplication.timeSinceStartup - CEditorSceneManager.m_dblSkipTimeDependency).ExIsGreatEquals(KCDefine.B_VAL_3_REAL)) {
				CEditorSceneManager.m_dblSkipTimeDependency = EditorApplication.timeSinceStartup;
				CEditorSceneManager.m_bIsEnableSetupDependencies = false;

				try {
					CEditorSceneManager.SetupDependencies();
				} finally {
					EditorApplication.update -= CEditorSceneManager.UpdateDependencyState;
					CEditorSceneManager.m_oListRequest = null;
				}
			}
		}
	}

	/** 프로젝트 상태가 갱신되었을 경우 */
	private static void OnUpdateProjectState() {
		CEditorSceneManager.SetupExtraPreloadAssets();
	}

	/** 스크립트가 로드되었을 경우 */
	[UnityEditor.Callbacks.DidReloadScripts]
	private static void OnLoadScript() {
		CEditorSceneManager.m_bIsEnableSetup = true;
	}

	/** 씬이 열렸을 경우 */
	private static void OnOpenScene(Scene a_stScene, OpenSceneMode a_eMode) {
		// 중첩 모드 일 경우
		if(a_eMode == OpenSceneMode.Additive) {
			// Do Something
		}
	}
	#endregion // 클래스 함수
}

/** 에디터 씬 관리자 - 설정 */
public static partial class CEditorSceneManager {
	#region 클래스 함수
	/** 콜백을 설정한다 */
	private static void SetupCallbacks() {
		EditorApplication.update -= CEditorSceneManager.Update;
		EditorApplication.update += CEditorSceneManager.Update;

		EditorApplication.update -= CEditorSceneManager.LateUpdate;
		EditorApplication.update += CEditorSceneManager.LateUpdate;

		EditorApplication.update -= CEditorSceneManager.UpdateDependencyState;
		EditorApplication.update += CEditorSceneManager.UpdateDependencyState;

		EditorApplication.projectChanged -= CEditorSceneManager.OnUpdateProjectState;
		EditorApplication.projectChanged += CEditorSceneManager.OnUpdateProjectState;

		EditorSceneManager.sceneOpened -= CEditorSceneManager.OnOpenScene;
		EditorSceneManager.sceneOpened += CEditorSceneManager.OnOpenScene;
	}

	/** 종속 패키지를 설정한다 */
	private static void SetupDependencies() {
		var oPkgsInfoList = CEditorSceneManager.m_oListRequest.Result.ToList();

		foreach(var stKeyVal in KEditorDefine.B_UNITY_PKGS_DEPENDENCY_DICT) {
			int nResult = oPkgsInfoList.FindIndex((a_oPkgsInfo) => a_oPkgsInfo.name.Equals(stKeyVal.Key));

			// 패키지 설정이 불가능 할 경우
			if(oPkgsInfoList.ExIsValidIdx(nResult)) {
				continue;
			}

			// 버전이 유효 할 경우
			if(stKeyVal.Value.ExIsValidBuildVer()) {
				string oUnityPkgsID = string.Format(KCEditorDefine.B_UNITY_PKGS_ID_FMT, stKeyVal.Key, stKeyVal.Value);
				CEditorSceneManager.m_oListAddRequest.ExAddVal(Client.Add(oUnityPkgsID));
			} else {
#if DEVELOPMENT_PROJ
				CEditorSceneManager.m_oListAddRequest.ExAddVal(Client.Add(stKeyVal.Value));
#endif // #if DEVELOPMENT_PROJ
			}
		}
	}

	/** 미리 로드 할 추가 에셋을 설정한다 */
	private static void SetupExtraPreloadAssets() {
#if EXTRA_SCRIPT_MODULE_ENABLE
		var oPreloadAssetList = PlayerSettings.GetPreloadedAssets().ToList();

		for(int i = 0; i < KEditorDefine.G_EXTRA_DIR_P_PRELOAD_ASSET_LIST.Count; ++i) {
			// 에셋 설정이 불가능 할 경우
			if(!AssetDatabase.IsValidFolder(KEditorDefine.G_EXTRA_DIR_P_PRELOAD_ASSET_LIST[i])) {
				continue;
			}

			var oAssetList = CEditorAccess.FindAssets<Object>(string.Empty, new List<string>() {
				KEditorDefine.G_EXTRA_DIR_P_PRELOAD_ASSET_LIST[i]
			});

			for(int j = 0; j < oAssetList.Count; ++j) {
				// 에셋 설정이 불가능 할 경우
				if(oAssetList[j].GetType() == typeof(DefaultAsset)) {
					continue;
				}

				oPreloadAssetList.ExAddVal(oAssetList[j], (a_oAsset) => a_oAsset != null && oAssetList[j] != null && a_oAsset.name.Equals(oAssetList[j].name));
			}
		}

		for(int i = 0; i < KEditorDefine.G_EXTRA_ASSET_P_PRELOAD_ASSET_LIST.Count; ++i) {
			var oAsset = CEditorAccess.FindAsset<Object>(KEditorDefine.G_EXTRA_ASSET_P_PRELOAD_ASSET_LIST[i]);
			oPreloadAssetList.ExAddVal(oAsset, (a_oAsset) => a_oAsset != null && oAsset != null && a_oAsset.name.Equals(oAsset.name));
		}

		PlayerSettings.SetPreloadedAssets(oPreloadAssetList.ToArray());
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE
	}

#if UIS_ROOT_PREFAB_ENABLE
	/** 프리팹 객체를 설정한다 */
	private static void SetupPrefabObj(GameObject a_oObj) {
		// 객체 설정이 불가능 할 경우
		if(PrefabUtility.IsPrefabAssetMissing(a_oObj) || CFunc.FindComponent<CSampleSceneManager>(a_oObj.scene) != null) {
			return;
		}

		string oPrefabPath = CEditorAccess.GetRootObjPrefabPath(a_oObj.scene, a_oObj.name);

		// 프리팹이 없을 경우
		if(!CEditorAccess.IsExistsAsset(oPrefabPath)) {
			CEditorFactory.MakeDirectories(Path.GetDirectoryName(oPrefabPath).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH));
			PrefabUtility.SaveAsPrefabAssetAndConnect(a_oObj, oPrefabPath, InteractionMode.AutomatedAction);

			EditorSceneManager.MarkSceneDirty(a_oObj.scene);
		}

		// 베리언트 프리팹 일 경우
		if(PrefabUtility.GetPrefabAssetType(a_oObj) == PrefabAssetType.Variant) {
			do {
				PrefabUtility.UnpackPrefabInstance(a_oObj, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
			} while(PrefabUtility.GetPrefabAssetType(a_oObj) != PrefabAssetType.NotAPrefab);

			CEditorFactory.RemoveAsset(oPrefabPath);
		}
	}
#endif // #if UIS_ROOT_PREFAB_ENABLE
#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR
#endif // #if SCRIPT_TEMPLATE_ONLY
