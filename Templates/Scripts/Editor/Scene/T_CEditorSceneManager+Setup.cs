using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.PackageManager;

/** 에디터 씬 관리자 - 설정 */
public static partial class CEditorSceneManager {
	#region 클래스 함수
	/** 콜백을 설정한다 */
	private static void SetupCallbacks() {
		EditorApplication.update -= CEditorSceneManager.Update;
		EditorApplication.update += CEditorSceneManager.Update;

		EditorApplication.update -= CEditorSceneManager.UpdateDependencyState;
		EditorApplication.update += CEditorSceneManager.UpdateDependencyState;

		EditorApplication.update -= CEditorSceneManager.UpdateScopedRegistryState;
		EditorApplication.update += CEditorSceneManager.UpdateScopedRegistryState;

		EditorApplication.update -= CEditorSceneManager.LateUpdate;
		EditorApplication.update += CEditorSceneManager.LateUpdate;
	}

	/** 독립 패키지를 설정한다 */
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

	/** 패키지 레지스트리를 설정한다 */
	private static void SetupScopedRegistries() {
		string oStr = CFunc.ReadStr(KCEditorDefine.B_DATA_P_UNITY_PKGS);
		var oJSONNode = SimpleJSON.JSON.Parse(oStr) as SimpleJSON.JSONClass;
		
		// JSON 노드가 존재 할 경우
		if(oJSONNode != null) {
			bool bIsNeedsUpdate = false;

			var oScopedRegistryList = oJSONNode[KEditorDefine.B_UNITY_PKGS_SCOPED_REGISTRY_DICT_KEY].AsArray;
			oScopedRegistryList = oScopedRegistryList ?? new SimpleJSON.JSONArray();

			foreach(var stKeyVal in KEditorDefine.B_UNITY_PKGS_SCOPED_REGISTRY_DICT) {
				int nIdx = oScopedRegistryList.AsArray.ExFindVal((a_oJSONNode) => stKeyVal.Key.Equals(a_oJSONNode[KEditorDefine.B_UNITY_PKGS_N_KEY]));

				// 패키지 레지스트리가 없을 경우
				if(!oScopedRegistryList.ExIsValidIdx(nIdx)) {
					var oScopedRegistryNode = SimpleJSON.JSON.Parse(CFunc.ReadStr(stKeyVal.Value)) as SimpleJSON.JSONClass;

					// 패키지 레지스트리 노드가 존재 할 경우
					if(oScopedRegistryNode != null) {
						bIsNeedsUpdate = true;
						oScopedRegistryList.Add(oScopedRegistryNode);
					}
				}
			}

			// 패키지 레지스트리 갱신이 필요 할 경우
			if(bIsNeedsUpdate && oScopedRegistryList.Count > KCDefine.B_VAL_0_INT) {
				oJSONNode.Add(KEditorDefine.B_UNITY_PKGS_SCOPED_REGISTRY_DICT_KEY, oScopedRegistryList);
				CFunc.WriteStr(KCEditorDefine.B_DATA_P_UNITY_PKGS, oJSONNode.ToString());

				CEditorFunc.UpdateAssetDBState();
			}
		}
	}

	/** 미리 로드 할 추가 에셋을 설정한다 */
	private static void SetupExtraPreloadAssets() {
#if EXTRA_SCRIPT_ENABLE
		var oPreloadAssetList = PlayerSettings.GetPreloadedAssets().ToList();

		try {
			for(int i = 0; i < KEditorDefine.G_EXTRA_DIR_P_PRELOAD_ASSET_LIST.Count; ++i) {
				// 디렉토리가 존재 할 경우
				if(AssetDatabase.IsValidFolder(KEditorDefine.G_EXTRA_DIR_P_PRELOAD_ASSET_LIST[i])) {
					var oAssetList = CEditorFunc.FindAssets<Object>(string.Empty, new List<string>() { KEditorDefine.G_EXTRA_DIR_P_PRELOAD_ASSET_LIST[i] });

					for(int j = 0; j < oAssetList.Count; ++j) {
						// 디렉토리 에셋이 아닐 경우
						if(oAssetList[j].GetType() != typeof(DefaultAsset)) {
							oPreloadAssetList.ExAddVal(oAssetList[j], (a_oAsset) => a_oAsset != null && oAssetList[j].name.Equals(a_oAsset.name));
						}
					}
				}
			}

			for(int i = 0; i < KEditorDefine.G_EXTRA_ASSET_P_PRELOAD_ASSET_LIST.Count; ++i) {
				var oAsset = CEditorFunc.FindAsset<Object>(KEditorDefine.G_EXTRA_ASSET_P_PRELOAD_ASSET_LIST[i]);
				oPreloadAssetList.ExAddVal(oAsset, (a_oAsset) => a_oAsset != null && oAsset.name.Equals(a_oAsset.name));
			}
		} finally {
			PlayerSettings.SetPreloadedAssets(oPreloadAssetList.ToArray());
		}
#endif			// #if EXTRA_SCRIPT_ENABLE
	}
	#endregion			// 클래스 함수
}
#endif			// #if UNITY_EDITOR
#endif			// #if SCRIPT_TEMPLATE_ONLY
