using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Linq;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

//! 공용 에디터 씬 관리자
[InitializeOnLoad]
public static partial class CCommonEditorSceneManager {
	#region 클래스 변수
	private static float m_fSkipTime = 0.0f;
	private static float m_fHierarchySkipTime = 0.0f;

	private static GUIStyle m_oGUIStyle = null;
	#endregion			// 클래스 변수

	#region 클래스 함수
	//! 생성자
	static CCommonEditorSceneManager() {
		// 배치 모드가 아닐 경우
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

	//! 스크립트가 로드 되었을 경우
	[UnityEditor.Callbacks.DidReloadScripts]
	public static void OnLoadScript() {
		// 상태 갱신이 가능 할 경우
		if(!Application.isBatchMode && CEditorAccess.IsEnableUpdateState()) {
			CCommonPlatformOptsSetter.SetupPlayerOpts();
			CCommonPlatformOptsSetter.SetupEditorOpts();
			CCommonPlatformOptsSetter.SetupProjOpts();
			CCommonPlatformOptsSetter.SetupPluginProjs();
			CCommonPlatformOptsSetter.SetupGraphicAPIs();
		}
	}

	//! 상태를 갱신한다
	private static void Update() {
		// 상태 갱신이 가능 할 경우
		if(CEditorAccess.IsEnableUpdateState()) {
			CCommonEditorSceneManager.m_fSkipTime += Time.deltaTime;
			CCommonEditorSceneManager.m_fHierarchySkipTime += Time.deltaTime;

			// 갱신 주기가 지났을 경우
			if(CCommonEditorSceneManager.m_fSkipTime.ExIsGreateEquals(KCEditorDefine.B_DELTA_TIME_EDITOR_SM_SCENE_UPDATE)) {
				CCommonEditorSceneManager.m_fSkipTime = KCDefine.B_VALUE_FLOAT_0;

				CCommonEditorSceneManager.SetupScene();
				CCommonEditorSceneManager.SetupLightOpts();
				CCommonEditorSceneManager.SetupFileBrowserUI();

				// 갱신 주기가 지났을 경우
				if(CCommonEditorSceneManager.m_fHierarchySkipTime.ExIsGreateEquals(KCEditorDefine.B_DELTA_TIME_HIERARCHY_UPDATE)) {
					CCommonEditorSceneManager.m_fHierarchySkipTime = KCDefine.B_VALUE_FLOAT_0;

					CFunc.EnumerateScenes((a_stScene) => {
						var oObjs = a_stScene.GetRootGameObjects();

						for(int j = 0; j < oObjs.Length; ++j) {
							var oEnumerator = oObjs[j].DescendantsAndSelf();
							var oRemoveObjList = new List<GameObject>();

							foreach(var oObj in oEnumerator) {
								int nNumMissingScripts = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(oObj);

								// 객체 제거가 필요 할 경우
								if(PrefabUtility.IsPrefabAssetMissing(oObj)) {
									oRemoveObjList.ExAddValue(oObj);
								}

								// 스크립트 제거가 필요 할 경우
								if(nNumMissingScripts > KCDefine.B_VALUE_INT_0) {
									GameObjectUtility.RemoveMonoBehavioursWithMissingScript(oObj);
									EditorSceneManager.MarkSceneDirty(a_stScene);
								}
							}

							for(int i = 0; i < oRemoveObjList.Count; ++i) {
								CAccess.RemoveObj(oRemoveObjList[i]);
							}
						}
					});
				}
			}
		}

		// 기즈모를 그릴 수 있을 경우
		if(CEditorAccess.IsEnableDrawGizmos()) {
			// 상태 갱신이 가능 할 경우
			if(CEditorAccess.IsEnableUpdateState()) {
				CFunc.EnumerateScenes((a_stScene) => {
					var oSceneManager = a_stScene.ExFindComponent<CSceneManager>(KCDefine.U_OBJ_NAME_SCENE_SCENE_MANAGER);
					oSceneManager?.EditorSetupScene();
				});
			}
		}
	}

	//! 계층 뷰 UI 상태를 갱신한다
	private static void UpdateHierarchyUIState(int a_nInstanceID, Rect a_stRect) {
		var oObj = EditorUtility.InstanceIDToObject(a_nInstanceID) as GameObject;

		// 객체가 존재 할 경우
		if(oObj != null) {
			a_stRect.size = new Vector2(KCEditorDefine.B_HIERARCHY_WIDTH, a_stRect.size.y);
			a_stRect.position += new Vector2(KCEditorDefine.B_HIERARCHY_OFFSET_X, KCDefine.B_VALUE_FLOAT_0);

			var oComponents = oObj.GetComponents<Component>();

			for(int i = 0; i < oComponents.Length; ++i) {
				// 컴포넌트가 존재 할 경우
				if(oComponents[i] != null) {
					var oType = oComponents[i].GetType();
					
					var oSortingLayerProperty = oType.GetProperty(KCEditorDefine.B_PROPERTY_NAME_SORTING_LAYER,
						KCDefine.B_BINDING_FLAG_PUBLIC_INSTANCE);

					var oSortingOrderProperty = oType.GetProperty(KCEditorDefine.B_PROPERTY_NAME_SORTING_ORDER,
						KCDefine.B_BINDING_FLAG_PUBLIC_INSTANCE);

					// 프로퍼티가 존재 할 경우
					if(oSortingLayerProperty != null && oSortingOrderProperty != null) {
						string oString = string.Format(KCEditorDefine.B_SORTING_ORDER_INFO_FORMAT, 
							oSortingLayerProperty.GetValue(oComponents[i]), oSortingOrderProperty.GetValue(oComponents[i]));

						GUI.Label(a_stRect, oString, m_oGUIStyle);
					}
				}
			}
		}
	}

	//! 플레이 모드가 변경 되었을 경우
	private static void OnChangePlayMode(PlayModeStateChange a_eStateChange) {
		if(a_eStateChange == PlayModeStateChange.EnteredEditMode) {
			CFunc.EnumerateScenes((a_stScene) => {
				var oSceneManager = a_stScene.ExFindComponent<CSceneManager>(KCDefine.U_OBJ_NAME_SCENE_SCENE_MANAGER);

				if(oSceneManager != null) {
					CFunc.SelectObj(oSceneManager.gameObject);
				}
			});
		}
	}

	//! 씬이 열렸을 경우
	private static void OnSceneOpen(Scene a_stScene, OpenSceneMode a_eSceneMode) {
		// 상태 갱신이 가능 할 경우
		if(!Application.isBatchMode && CEditorAccess.IsEnableUpdateState()) {
			CCommonPlatformOptsSetter.SetupProjOpts();
		}

#if UNITY_ANDROID
		// 플러그인이 없을 경우
		if(!File.Exists(KCEditorDefine.B_ANDROID_DEST_PLUGIN_PATH)) {
			CEditorFunc.ExecuteCmdline(KCEditorDefine.B_ANDROID_PLUGIN_BUILD_CMD);
		}
#endif			// #if UNITY_ANDROID
	}
	#endregion			// 클래스 함수
}
#endif			// #if UNITY_EDITOR
