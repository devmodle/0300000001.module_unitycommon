using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Linq;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEditor.SceneManagement;

//! 에디터 씬 관리자
[InitializeOnLoad]
public static partial class CEditorSceneManager {
	#region 변수
	private static float m_fSkipTime = 0.0f;
	private static float m_fHierarchySkipTime = 0.0f;

	private static GUIStyle m_oGUIStyle = null;
	private static ListRequest m_oListRequest = null;
	#endregion			// 변수

	#region 클래스 함수
	//! 생성자
	static CEditorSceneManager() {
		if(!Application.isBatchMode) {
			// GUI 스타일을 설정한다 {
			CEditorSceneManager.m_oGUIStyle = new GUIStyle();
			CEditorSceneManager.m_oGUIStyle.alignment = TextAnchor.MiddleLeft;
			CEditorSceneManager.m_oGUIStyle.fontStyle = FontStyle.BoldAndItalic;

			CEditorSceneManager.m_oGUIStyle.normal = new GUIStyleState() {
				textColor = KCEditorDefine.B_HIERARCHY_TEXT_COLOR
			};
			// GUI 스타일을 설정한다 }

			CEditorSceneManager.SetupCallbacks();
		}
	}
	
	//! 스크립트가 로드 되었을 경우
	[UnityEditor.Callbacks.DidReloadScripts]
	public static void OnLoadScript() {
		if(!Application.isBatchMode && CEditorAccess.IsEnableUpdateState()) {
			CEditorSceneManager.SetupCallbacks();
			CEditorSceneManager.m_oListRequest = Client.List();

			CPlatformOptSetter.SetupPlayerOpts();
			CPlatformOptSetter.SetupEditorOpts();
			CPlatformOptSetter.SetupProjOpts();
			CPlatformOptSetter.SetupPluginProjs();
			CPlatformOptSetter.SetupGraphicAPIs();
		}
	}

	//! 상태를 갱신한다
	public static void Update() {
		if(CEditorAccess.IsEnableUpdateState()) {
			CEditorSceneManager.m_fSkipTime += Time.unscaledDeltaTime;
			CEditorSceneManager.m_fHierarchySkipTime += Time.unscaledDeltaTime;

			// 씬 갱신이 필요 할 경우
			if(CEditorSceneManager.m_fSkipTime >= KCEditorDefine.B_DELTA_TIME_EDITOR_SM_SCENE_UPDATE) {
				CEditorSceneManager.m_fSkipTime = 0.0f;

				CEditorSceneManager.SetupScene();
				CEditorSceneManager.SetupLightOpts();

#if FILE_BROWSER_ENABLE
				CEditorSceneManager.SetupFileBrowserUI();
#endif			// #if FILE_BROWSER_ENABLE

				// 계층 뷰 갱신이 필요 할 경우
				if(CEditorSceneManager.m_fHierarchySkipTime >= KCEditorDefine.B_DELTA_TIME_HIERARCHY_UPDATE) {
					CEditorSceneManager.m_fHierarchySkipTime = 0.0f;

					CFunc.EnumerateScenes((a_stScene) => {
						var oObjs = a_stScene.GetRootGameObjects();

						for(int j = 0; j < oObjs.Length; ++j) {
							var oEnumerator = oObjs[j].DescendantsAndSelf();

							foreach(var oObj in oEnumerator) {
								if(GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(oObj) >= 1) {
									GameObjectUtility.RemoveMonoBehavioursWithMissingScript(oObj);
									EditorSceneManager.MarkSceneDirty(a_stScene);
								}
							}
						}
					});
				}
			}
		}
	}

	//! 독립 패키지 상태를 갱신한다
	private static void UpdateDependencyState() {
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
	private static void UpdateScopedRegistryState() {
		CEditorSceneManager.SetupScopedRegistries();
		EditorApplication.update -= CEditorSceneManager.UpdateScopedRegistryState;
	}
	
	//! 계층 뷰 UI 상태를 갱신한다
	private static void UpdateHierarchyUIState(int a_nInstanceID, Rect a_stRect) {
		var oObj = EditorUtility.InstanceIDToObject(a_nInstanceID) as GameObject;

		if(oObj != null) {
			a_stRect.size = new Vector2(KCEditorDefine.B_HIERARCHY_WIDTH, a_stRect.size.y);
			a_stRect.position += new Vector2(KCEditorDefine.B_HIERARCHY_OFFSET_X, 0.0f);

			var oComponents = oObj.GetComponents<Component>();

			for(int i = 0; i < oComponents.Length; ++i) {
				if(oComponents[i] != null) {
					var oType = oComponents[i].GetType();
					
					var oSortingLayerProperty = oType.GetProperty(KCEditorDefine.B_PROPERTY_NAME_SORTING_LAYER,
						KCDefine.B_BINDING_FLAG_PUBLIC_INSTANCE);

					var oSortingOrderProperty = oType.GetProperty(KCEditorDefine.B_PROPERTY_NAME_SORTING_ORDER,
						KCDefine.B_BINDING_FLAG_PUBLIC_INSTANCE);

					if(oSortingLayerProperty != null && oSortingOrderProperty != null) {
						string oString = string.Format(KCEditorDefine.B_SORTING_ORDER_INFO_FORMAT, 
							oSortingLayerProperty.GetValue(oComponents[i]), oSortingOrderProperty.GetValue(oComponents[i]));

						GUI.Label(a_stRect, oString, m_oGUIStyle);
					}
				}
			}
		}
	}
	
	//! 씬이 열렸을 경우
	public static void OnSceneOpen(Scene a_stScene, OpenSceneMode a_eSceneMode) {
		if(!Application.isBatchMode && CEditorAccess.IsEnableUpdateState()) {
			CEditorSceneManager.SetupCallbacks();
			CPlatformOptSetter.SetupProjOpts();

			CEditorSceneManager.m_oListRequest = Client.List();
		}
	}
	#endregion			// 클래스 함수
}
#endif			// #if UNITY_EDITOR
