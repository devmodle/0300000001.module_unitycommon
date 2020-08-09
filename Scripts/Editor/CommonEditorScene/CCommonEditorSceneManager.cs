using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Linq;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

//! 공용 에디터 씬 관리자
[InitializeOnLoad]
public static partial class CCommonEditorSceneManager {
	#region 변수
	private static float m_fSkipTime = 0.0f;
	private static float m_fHierarchySkipTime = 0.0f;

	private static GUIStyle m_oGUIStyle = null;
	#endregion			// 변수

	#region 클래스 함수
	//! 생성자
	static CCommonEditorSceneManager() {
		if(!Application.isBatchMode) {
			// GUI 스타일을 설정한다 {
			CCommonEditorSceneManager.m_oGUIStyle = new GUIStyle();
			CCommonEditorSceneManager.m_oGUIStyle.alignment = TextAnchor.MiddleLeft;
			CCommonEditorSceneManager.m_oGUIStyle.fontStyle = FontStyle.BoldAndItalic;

			CCommonEditorSceneManager.m_oGUIStyle.normal = new GUIStyleState() {
				textColor = KCEditorDefine.B_HIERARCHY_TEXT_COLOR
			};
			// GUI 스타일을 설정한다 }

			CCommonEditorSceneManager.SetupCallbacks();
		}
	}

	//! 상태를 갱신한다
	public static void Update() {
		if(CEditorAccess.IsEnableUpdateState()) {
			CCommonEditorSceneManager.m_fSkipTime += Time.unscaledDeltaTime;
			CCommonEditorSceneManager.m_fHierarchySkipTime += Time.unscaledDeltaTime;

			// 씬 갱신이 필요 할 경우
			if(CCommonEditorSceneManager.m_fSkipTime >= KCEditorDefine.B_DELTA_TIME_EDITOR_SM_SCENE_UPDATE) {
				CCommonEditorSceneManager.m_fSkipTime = 0.0f;

				CCommonEditorSceneManager.SetupScene();
				CCommonEditorSceneManager.SetupLightOpts();

#if FILE_BROWSER_ENABLE
				CCommonEditorSceneManager.SetupFileBrowserUI();
#endif			// #if FILE_BROWSER_ENABLE

				// 계층 뷰 갱신이 필요 할 경우
				if(CCommonEditorSceneManager.m_fHierarchySkipTime >= KCEditorDefine.B_DELTA_TIME_HIERARCHY_UPDATE) {
					CCommonEditorSceneManager.m_fHierarchySkipTime = 0.0f;

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

	//! 계층 뷰 UI 상태를 갱신한다
	public static void UpdateHierarchyUIState(int a_nInstanceID, Rect a_stRect) {
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

	//! 스크립트가 로드 되었을 경우
	[UnityEditor.Callbacks.DidReloadScripts]
	public static void OnLoadScript() {
		if(!Application.isBatchMode && CEditorAccess.IsEnableUpdateState()) {
			CCommonPlatformOptSetter.SetupPlayerOpts();
			CCommonPlatformOptSetter.SetupEditorOpts();
			CCommonPlatformOptSetter.SetupProjOpts();
			CCommonPlatformOptSetter.SetupPluginProjs();
			CCommonPlatformOptSetter.SetupGraphicAPIs();
		}
	}
	
	//! 씬이 열렸을 경우
	public static void OnSceneOpen(Scene a_stScene, OpenSceneMode a_eSceneMode) {
		CCommonEditorSceneManager.OnLoadScript();
	}
	#endregion			// 클래스 함수
}
#endif			// #if UNITY_EDITOR
