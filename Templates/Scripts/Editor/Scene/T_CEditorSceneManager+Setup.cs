using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if NEVER_USE_THIS
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.SceneManagement;

//! 에디터 씬 관리자 - 설정
public static partial class CEditorSceneManager {
	#region 클래스 함수
	//! 콜백을 설정한다
	private static void SetupCallbacks() {
		EditorApplication.update -= CEditorSceneManager.Update;
		EditorApplication.update += CEditorSceneManager.Update;

		EditorApplication.update -= CEditorSceneManager.UpdateDependencyState;
		EditorApplication.update += CEditorSceneManager.UpdateDependencyState;

		EditorApplication.update -= CEditorSceneManager.UpdateScopedRegistryState;
		EditorApplication.update += CEditorSceneManager.UpdateScopedRegistryState;

		EditorApplication.update -= CEditorSceneManager.LateUpdate;
		EditorApplication.update += CEditorSceneManager.LateUpdate;

		EditorSceneManager.sceneOpened -= CEditorSceneManager.OnSceneOpen;
		EditorSceneManager.sceneOpened += CEditorSceneManager.OnSceneOpen;
	}

	//! 독립 패키지를 설정한다
	private static void SetupDependencies() {
		var oPkgsInfoList = CEditorSceneManager.m_oListRequest.Result.ToList();

		foreach(var stKeyValue in KEditorDefine.B_UNITY_PKGS_DEPENDENCY_LIST) {
			int nIdx = oPkgsInfoList.ExFindValue((a_oPkgsInfo) => a_oPkgsInfo.name.ExIsEquals(stKeyValue.Key));

			// 독립 패키지가 없을 경우
			if(!oPkgsInfoList.ExIsValidIdx(nIdx)) {
				// 버전이 유효 할 경우
				if(stKeyValue.Value.ExIsValidBuildVersion()) {
					var oAddRequest = Client.Add(string.Format(KEditorDefine.B_UNITY_PKGS_ID_FMT, stKeyValue.Key, stKeyValue.Value));
					CEditorSceneManager.m_oAddRequestList.ExAddValue(oAddRequest);
				} else {
#if !SAMPLE_PROJ
					var oAddRequest = Client.Add(stKeyValue.Value);
					CEditorSceneManager.m_oAddRequestList.ExAddValue(oAddRequest);
#endif			// #if !SAMPLE_PROJ
				}
			}
		}
	}

	//! 패키지 레지스트리를 설정한다
	private static void SetupScopedRegistries() {
		string oString = CFunc.ReadString(KCEditorDefine.B_DATA_P_UNITY_PKGS);
		var oJSONNode = SimpleJSON.JSON.Parse(oString);
		
		// JSON 노드가 유효 할 경우
		if(oJSONNode != null) {
			bool bIsNeedUpdate = false;

			var oScopedRegistryList = oJSONNode[KEditorDefine.B_UNITY_PKGS_SCOPED_REGISTRIES_KEY].AsArray;
			oScopedRegistryList = oScopedRegistryList ?? new SimpleJSON.JSONArray();

			foreach(var stKeyValue in KEditorDefine.B_UNITY_PKGS_SCOPED_REGISTRY_LIST) {
				int nIdx = oScopedRegistryList.AsArray.ExFindValue((a_oJSONNode) => {
					string oScopedRegistry = a_oJSONNode[KEditorDefine.B_UNITY_PKGS_N_KEY];
					return stKeyValue.Key.ExIsEquals(oScopedRegistry);
				});

				// 패키지 레지스트리가 없을 경우
				if(!oScopedRegistryList.ExIsValidIdx(nIdx)) {
					string oScopedRegistryString = CFunc.ReadString(stKeyValue.Value);
					var oScopedRegistryNode = SimpleJSON.JSON.Parse(oScopedRegistryString);

					// 패키지 레지스트리 노드가 유효 할 경우
					if(oScopedRegistryNode != null) {
						bIsNeedUpdate = true;
						oScopedRegistryList.Add(SimpleJSON.JSON.Parse(oScopedRegistryString));
					}
				}
			}

			// 패키지 레지스트리 갱신이 필요 할 경우
			if(bIsNeedUpdate && oScopedRegistryList.Count > KCDefine.B_VALUE_INT_0) {
				oJSONNode.Add(KEditorDefine.B_UNITY_PKGS_SCOPED_REGISTRIES_KEY, oScopedRegistryList);
				CFunc.WriteString(KCEditorDefine.B_DATA_P_UNITY_PKGS, oJSONNode.ToString());

				CEditorFunc.UpdateAssetDBState();
			}
		}
	}
	#endregion			// 클래스 함수
}
#endif			// #if UNITY_EDITOR
#endif			// #if NEVER_USE_THIS
