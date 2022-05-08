using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.SceneManagement;

/** 에디터 씬 관리자 - 설정 */
public static partial class CEditorSceneManager {
	#region 클래스 함수
	/** 에디터 씬 관리자를 설정한다 */
	private static IEnumerator SetupEditorSceneManager() {
		do {
			yield return null;
		} while(!CEditorAccess.IsEnableUpdateState);

		yield return CFactory.CreateWaitForSecs(KCDefine.B_VAL_1_FLT);
		CEditorSceneManager.m_bIsEnableSetup = true;
	}

	/** 콜백을 설정한다 */
	private static void SetupCallbacks() {
		EditorApplication.update -= CEditorSceneManager.Update;
		EditorApplication.update += CEditorSceneManager.Update;

		EditorApplication.update -= CEditorSceneManager.LateUpdate;
		EditorApplication.update += CEditorSceneManager.LateUpdate;
	}

	/** 종속 패키지를 설정한다 */
	private static void SetupDependencies() {
		var oPkgsInfoList = CEditorSceneManager.m_oListRequest.Result.ToList();

		foreach(var stKeyVal in KEditorDefine.B_UNITY_PKGS_DEPENDENCY_DICT) {
			int nIdx = oPkgsInfoList.FindIndex((a_oPkgsInfo) => a_oPkgsInfo.name.Equals(stKeyVal.Key));

			// 독립 패키지가 없을 경우
			if(!oPkgsInfoList.ExIsValidIdx(nIdx)) {
				// 버전이 유효 할 경우
				if(stKeyVal.Value.ExIsValidBuildVer()) {
					string oID = string.Format(KEditorDefine.B_UNITY_PKGS_ID_FMT, stKeyVal.Key, stKeyVal.Value);
					CEditorSceneManager.m_oAddRequestList.ExAddVal(Client.Add(oID));
				} else {
#if !SAMPLE_PROJ
					CEditorSceneManager.m_oAddRequestList.ExAddVal(Client.Add(stKeyVal.Value));
#endif			// #if !SAMPLE_PROJ
				}
			}
		}
	}

	/** 미리 로드 할 추가 에셋을 설정한다 */
	private static void SetupExtraPreloadAssets() {
#if EXTRA_SCRIPT_MODULE_ENABLE
		var oPreloadAssetList = PlayerSettings.GetPreloadedAssets().ToList();

		try {
			for(int i = 0; i < KEditorDefine.G_EXTRA_DIR_P_PRELOAD_ASSET_LIST.Count; ++i) {
				// 디렉토리가 존재 할 경우
				if(AssetDatabase.IsValidFolder(KEditorDefine.G_EXTRA_DIR_P_PRELOAD_ASSET_LIST[i])) {
					var oAssetList = CEditorFunc.FindAssets<Object>(string.Empty, new List<string>() { KEditorDefine.G_EXTRA_DIR_P_PRELOAD_ASSET_LIST[i] });

					for(int j = 0; j < oAssetList.Count; ++j) {
						// 디렉토리 에셋이 아닐 경우
						if(oAssetList[j].GetType() != typeof(DefaultAsset)) {
							oPreloadAssetList.ExAddVal(oAssetList[j], (a_oAsset) => (a_oAsset != null && oAssetList[j] != null) && oAssetList[j].name.Equals(a_oAsset.name));
						}
					}
				}
			}

			for(int i = 0; i < KEditorDefine.G_EXTRA_ASSET_P_PRELOAD_ASSET_LIST.Count; ++i) {
				var oAsset = CEditorFunc.FindAsset<Object>(KEditorDefine.G_EXTRA_ASSET_P_PRELOAD_ASSET_LIST[i]);
				oPreloadAssetList.ExAddVal(oAsset, (a_oAsset) => (a_oAsset != null && oAsset != null) && oAsset.name.Equals(a_oAsset.name));
			}
		} finally {
			PlayerSettings.SetPreloadedAssets(oPreloadAssetList.ToArray());
		}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE
	}

	/** 프리팹 객체를 설정한다 */
	private static void SetupPrefabObjs(GameObject a_oObj) {
		// 프리팹 설정이 가능 할 경우
		if(!PrefabUtility.IsPrefabAssetMissing(a_oObj) && CFunc.FindComponent<CSampleSceneManager>(a_oObj.scene) == null) {
			string oDirPath = (!a_oObj.scene.name.Contains(KCDefine.B_EDITOR_SCENE_N_PATTERN_01) && !a_oObj.scene.name.Contains(KCDefine.B_EDITOR_SCENE_N_PATTERN_02)) ? KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_PREFABS : KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR_PREFABS;
			string oPrefabPath = string.Format(KCDefine.B_TEXT_FMT_3_SLASH_COMBINE, Path.GetDirectoryName(oDirPath), Path.GetDirectoryName(KCEditorDefine.B_DIR_P_AUTO_CREATE), string.Format(KCDefine.B_TEXT_FMT_2_UNDER_SCORE_COMBINE, a_oObj.scene.name, a_oObj.name));
			string oFinalPrefabPath = string.Format(KCDefine.B_TEXT_FMT_2_COMBINE, oPrefabPath, KCDefine.B_FILE_EXTENSION_PREFAB);

			// 프리팹이 없을 경우
			if(!CEditorAccess.IsExistsAsset(oFinalPrefabPath)) {
				EditorSceneManager.MarkSceneDirty(a_oObj.scene);
				CEditorFactory.MakeDirectories(Path.GetDirectoryName(oPrefabPath));

				PrefabUtility.SaveAsPrefabAssetAndConnect(a_oObj, oFinalPrefabPath, InteractionMode.AutomatedAction);
			}
		}
	}
	#endregion			// 클래스 함수
}
#endif			// #if UNITY_EDITOR
#endif			// #if SCRIPT_TEMPLATE_ONLY
