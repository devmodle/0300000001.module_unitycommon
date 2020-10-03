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

		EditorSceneManager.sceneOpened -= CEditorSceneManager.OnSceneOpen;
		EditorSceneManager.sceneOpened += CEditorSceneManager.OnSceneOpen;
	}

	//! 독립 패키지를 설정한다
	private static void SetupDependencies() {
		var oPkgInfoList = m_oListRequest.Result.ToList();

		foreach(var stKeyValue in KEditorDefine.B_UNITY_PKGS_DEPENDENCY_LIST) {
			int nIndex = oPkgInfoList.ExFindValue((a_oPkgInfo) => 
				a_oPkgInfo.name.ExIsEquals(stKeyValue.Key));

			// 독립 패키지 추가가 가능 할 경우
			if(nIndex <= KCDefine.B_INDEX_INVALID) {
				Client.Add(string.Format(KEditorDefine.B_UNITY_PKGS_ID_FORMAT,
					stKeyValue.Key, stKeyValue.Value));
			}
		}
	}

	//! 패키지 레지스트리를 설정한다
	private static void SetupScopedRegistries() {
		string oString = CFunc.ReadString(KCEditorDefine.B_DATA_PATH_UNITY_PKGS,
			System.Text.Encoding.Default);

		var oJSONNode = SimpleJSON.JSON.Parse(oString);

		// JSON 노드가 유효 할 경우
		if(oJSONNode != null) {
			bool bIsNeedUpdate = false;

			var oScopedRegistryList = oJSONNode[KEditorDefine.B_UNITY_PKGS_SCOPED_REGISTRIES_KEY].AsArray;
			oScopedRegistryList = oScopedRegistryList ?? new SimpleJSON.JSONArray();

			foreach(var stKeyValue in KEditorDefine.B_UNITY_PKGS_SCOPED_REGISTRY_LIST) {
				int nIndex = oScopedRegistryList.AsArray.ExFindValue((a_oJSONNode) => 
					stKeyValue.Key.ExIsEquals(a_oJSONNode[KEditorDefine.B_UNITY_PKGS_NAME_KEY]));

				// 패키지 레지스트리 추가가 가능 할 경우
				if(nIndex <= KCDefine.B_INDEX_INVALID) {
					string oScopedRegistryString = CFunc.ReadString(stKeyValue.Value, 
						System.Text.Encoding.Default);

					var oScopedRegistryNode = SimpleJSON.JSON.Parse(oScopedRegistryString);

					// 패키지 레지스트리 노드가 유효 할 경우
					if(oScopedRegistryNode != null) {
						bIsNeedUpdate = true;
						oScopedRegistryList.Add(SimpleJSON.JSON.Parse(oScopedRegistryString));
					}
				}
			}

			// 패키지 레지스트리 갱신이 필요 할 경우
			if(bIsNeedUpdate && oScopedRegistryList.Count >= 1) {
				oJSONNode.Add(KEditorDefine.B_UNITY_PKGS_SCOPED_REGISTRIES_KEY, oScopedRegistryList);
				CFunc.WriteString(KCEditorDefine.B_DATA_PATH_UNITY_PKGS, oJSONNode.ToString(), System.Text.Encoding.Default);

				CEditorFunc.UpdateAssetDatabaseState();
			}
		}
	}
	#endregion			// 클래스 함수
}
#endif			// #if UNITY_EDITOR
#endif			// #if NEVER_USE_THIS
