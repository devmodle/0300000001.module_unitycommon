using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System.IO;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using Unity.Linq;

//! 에디터 씬 관리자
[InitializeOnLoad]
public static partial class CEditorSceneManager {
	#region 변수
	private static float m_fSkipTime = 0.0f;
	private static float m_fHierarchySkipTime = 0.0f;

	private static GUIStyle m_oHierarchyGUIStyle = null;
	#endregion			// 변수

	#region 클래스 함수
	//! 생성자
	static CEditorSceneManager() {
		if(!Application.isBatchMode) {
			// GUI 스타일을 생성한다 {
			CEditorSceneManager.m_oHierarchyGUIStyle = new GUIStyle();
			CEditorSceneManager.m_oHierarchyGUIStyle.alignment = TextAnchor.MiddleLeft;
			CEditorSceneManager.m_oHierarchyGUIStyle.fontStyle = FontStyle.BoldAndItalic;

			CEditorSceneManager.m_oHierarchyGUIStyle.normal = new GUIStyleState() {
				textColor = KEditorDefine.B_HIERARCHY_TEXT_COLOR
			};
			// GUI 스타일을 생성한다 }

			EditorApplication.update -= CEditorSceneManager.Update;
			EditorApplication.update += CEditorSceneManager.Update;

			EditorApplication.hierarchyWindowItemOnGUI -= CEditorSceneManager.UpdateHierarchyUIState;
			EditorApplication.hierarchyWindowItemOnGUI += CEditorSceneManager.UpdateHierarchyUIState;

			EditorSceneManager.sceneOpened -= CEditorSceneManager.OnSceneOpen;
			EditorSceneManager.sceneOpened += CEditorSceneManager.OnSceneOpen;
		}
	}

	//! 상태 갱신 가능 여부를 검사한다
	public static bool IsEnableUpdateState() {
		return !Application.isPlaying && !EditorApplication.isCompiling && !BuildPipeline.isBuildingPlayer;
	}
	
	//! 스크립트가 로드 되었을 경우
	[UnityEditor.Callbacks.DidReloadScripts]
	public static void OnLoadScript() {
		if(!Application.isBatchMode && CEditorSceneManager.IsEnableUpdateState()) {
			EditorFunc.SetupPlayerOptions();
			EditorFunc.SetupEditorOptions();
			EditorFunc.SetupProjectOptions();
			EditorFunc.SetupPluginProjects();
			EditorFunc.SetupGraphicAPIs();
		}
	}

	//! 씬이 열렸을 경우
	public static void OnSceneOpen(Scene a_stScene, OpenSceneMode a_eSceneMode) {
		EditorFunc.SetupProjectOptions();
		EditorFunc.SetupScene(a_stScene, KEditorDefine.B_SCENE_MANAGER_TYPE_LIST);
	}

	//! 상태를 갱신한다
	public static void Update() {
		if(CEditorSceneManager.IsEnableUpdateState()) {
			CEditorSceneManager.m_fSkipTime += Time.unscaledDeltaTime;
			CEditorSceneManager.m_fHierarchySkipTime += Time.unscaledDeltaTime;

			// 씬 갱신이 필요 할 경우
			if(CEditorSceneManager.m_fSkipTime >= KEditorDefine.B_DELTA_TIME_EDITOR_SM_SCENE_UPDATE) {
				CEditorSceneManager.m_fSkipTime = 0.0f;

				CEditorSceneManager.SetupScene();
				CEditorSceneManager.SetupLightOptions();

#if FILE_BROWSER_ENABLE
				CEditorSceneManager.SetupFileBrowserUI();
#endif			// #if FILE_BROWSER_ENABLE

				// 계층 뷰 갱신이 필요 할 경우
				if(CEditorSceneManager.m_fHierarchySkipTime >= KEditorDefine.B_DELTA_TIME_HIERARCHY_UPDATE) {
					CEditorSceneManager.m_fHierarchySkipTime = 0.0f;

					for(int i = 0; i < SceneManager.sceneCount; ++i) {
						var stScene = SceneManager.GetSceneAt(i);
						var oGameObjects = stScene.GetRootGameObjects();

						for(int j = 0; j < oGameObjects.Length; ++j) {
							var oEnumerator = oGameObjects[j].DescendantsAndSelf();

							foreach(var oGameObject in oEnumerator) {
								if(GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(oGameObject) >= 1) {
									GameObjectUtility.RemoveMonoBehavioursWithMissingScript(oGameObject);
									EditorSceneManager.MarkSceneDirty(stScene);
								}
							}
						}
					}
				}
			}
		}
	}

	[DrawGizmo(GizmoType.Active | GizmoType.Selected | GizmoType.NonSelected | GizmoType.Pickable | GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
	public static void DrawGizmos(CSceneManager a_oSceneManager, GizmoType a_oGizmoType) {
		if(CEditorSceneManager.IsEnableUpdateState()) {
			for(int i = 0; i < SceneManager.sceneCount; ++i) {
				var stScene = SceneManager.GetSceneAt(i);
				var oSceneManager = stScene.ExFindComponent<CSceneManager>(KDefine.U_OBJ_NAME_SCENE_SCENE_MANAGER);

				if(oSceneManager != null) {
					oSceneManager.EditorSetupScene();

					if(Camera.main != null) {
						oSceneManager.EditorDrawGuideline();
					}
				}
			}
		}
	}

	//! 씬을 설정한다
	private static void SetupScene() {
		bool bIsExistsUICamera = false;
		bool bIsExistsMainCamera = false;
		bool bIsExistsMainLight = false;

		var oLights = Resources.FindObjectsOfTypeAll<Light>();
		var oCameras = Resources.FindObjectsOfTypeAll<Camera>();
		var oSceneManagers = Resources.FindObjectsOfTypeAll<CSceneManager>();

		// 광원을 설정한다
		for(int j = 0; j < oLights.Length; ++j) {
			if(oLights[j].name.ExIsEquals(KDefine.U_OBJ_NAME_SCENE_MAIN_LIGHT)) {
				bIsExistsMainLight = true;

				oLights[j].type = LightType.Directional;
				oLights[j].lightmapBakeType = KAppDefine.G_DEF_LIGHTMAP_BAKE_TYPE_DIRECTIONAL;

				if(!oLights[j].CompareTag(KDefine.U_TAG_MAIN_LIGHT)) {
					oLights[j].tag = KDefine.U_TAG_MAIN_LIGHT;
				}
			}
		}

		// 카메라를 설정한다
		for(int j = 0; j < oCameras.Length; ++j) {
			if(!oCameras[j].name.ExIsEquals(KEditorDefine.B_OBJ_NAME_SCENE_EDITOR_CAMERA)) {
				bool bIsUICamera = oCameras[j].name.ExIsEquals(KDefine.U_OBJ_NAME_SCENE_UI_CAMERA);
				bool bIsMainCamera = oCameras[j].name.ExIsEquals(KDefine.U_OBJ_NAME_SCENE_MAIN_CAMERA);

				bIsExistsUICamera = bIsUICamera ? true : bIsExistsUICamera;
				bIsExistsMainCamera = bIsMainCamera ? true : bIsExistsMainCamera;
				
				if(bIsUICamera && !oCameras[j].CompareTag(KDefine.U_TAG_UI_CAMERA)) {
					oCameras[j].tag = KDefine.U_TAG_UI_CAMERA;
				} else if(bIsMainCamera && !oCameras[j].CompareTag(KDefine.U_TAG_MAIN_CAMERA)) {
					oCameras[j].tag = KDefine.U_TAG_MAIN_CAMERA;
				}
				
#if CAMERA_STACK_ENABLE
				oCameras[j].gameObject.SetActive(bIsUICamera || bIsMainCamera);
#else
				oCameras[j].gameObject.SetActive(bIsMainCamera);
#endif			// #if CAMERA_STACK_ENABLE
			}
		}

		// 씬 관리자를 설정한다
		for(int j = 0; j < oSceneManagers.Length; ++j) {
			if(!oSceneManagers[j].CompareTag(KDefine.U_TAG_SCENE_MANAGER)) {
				oSceneManagers[j].tag = KDefine.U_TAG_SCENE_MANAGER;
			}
		}

		if(!bIsExistsUICamera) {
			Func.ShowLogWarning(string.Format("{0} 객체가 없습니다.", KDefine.U_OBJ_NAME_SCENE_UI_CAMERA));
		} else if(!bIsExistsMainCamera) {
			Func.ShowLogWarning(string.Format("{0} 객체가 없습니다.", KDefine.U_OBJ_NAME_SCENE_MAIN_CAMERA));
		} else if(!bIsExistsMainLight) {
			Func.ShowLogWarning(string.Format("{0} 객체가 없습니다.", KDefine.U_OBJ_NAME_SCENE_MAIN_LIGHT));
		}

		// FPS 카운터를 설정한다 {
#if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
		string oFilter = Path.GetFileNameWithoutExtension(KDefine.U_OBJ_PATH_SS_FPS_COUNTER);

		var oFPSCounterList = EditorFunc.FindAssets<GameObject>(oFilter, new string[] {
			KEditorDefine.B_DIR_PATH_AUTO_CREATE_RESOURCES
		});

		for(int i = 0; i < oFPSCounterList?.Count; ++i) {
			var oStaticText = oFPSCounterList[i].ExFindComponent<Text>(KEditorDefine.B_OBJ_NAME_STATIC_TEXT);
			oStaticText.fontSize = KEditorDefine.B_FONT_SIZE_STATIC_TEXT;

			var oDynamicText = oFPSCounterList[i].ExFindComponent<Text>(KEditorDefine.B_OBJ_NAME_DYNAMIC_TEXT);
			oDynamicText.fontSize = KEditorDefine.B_FONT_SIZE_DYNAMIC_TEXT;

			// 크기를 설정한다 {
			var oStaticSizeFitter = oStaticText.gameObject.ExAddComponent<ContentSizeFitter>();
			oStaticSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			oStaticSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

			var oDynamicSizeFitter = oDynamicText.gameObject.ExAddComponent<ContentSizeFitter>();
			oDynamicSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			oDynamicSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			// 크기를 설정한다 }

			// 위치를 설정한다 {
			oStaticText.rectTransform.pivot = KDefine.B_ANCHOR_BOTTOM_RIGHT;
			oStaticText.rectTransform.anchorMin = KDefine.B_ANCHOR_MIDDLE_RIGHT;
			oStaticText.rectTransform.anchorMax = KDefine.B_ANCHOR_MIDDLE_RIGHT;
			oStaticText.rectTransform.anchoredPosition = KEditorDefine.B_POSITION_STATIC_TEXT;

			oDynamicText.rectTransform.pivot = KDefine.B_ANCHOR_BOTTOM_RIGHT;
			oDynamicText.rectTransform.anchorMin = KDefine.B_ANCHOR_MIDDLE_RIGHT;
			oDynamicText.rectTransform.anchorMax = KDefine.B_ANCHOR_MIDDLE_RIGHT;
			oDynamicText.rectTransform.anchoredPosition = KEditorDefine.B_POSITION_DYNAMIC_TEXT;
			// 위치를 설정한다 }
		}
#endif			// #if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
		// FPS 카운터를 설정한다 }
	}

	//! 광원 옵션을 설정한다
	private static void SetupLightOptions() {
		var oType = typeof(LightmapEditorSettings);
		var oMethodInfo = oType.GetMethod(KEditorDefine.B_FUNC_NAME_GET_LIGHTMAP_SETTINGS, BindingFlags.Static | BindingFlags.NonPublic);
		var oLightmapSettings = oMethodInfo?.Invoke(null, null) as LightmapSettings;

		if(oLightmapSettings != null) {
			var oSerializeObject = new SerializedObject(oLightmapSettings);

			oSerializeObject.ExSetPropertyValue(KEditorDefine.B_PROPERTY_NAME_ENABLE_BAKE_LIGHTMAPS, (a_oProperty) => {
#if LIGHTMAP_BAKE_ENABLE
				a_oProperty.boolValue = true;
#else
				a_oProperty.boolValue = false;
#endif			// #if LIGHTMAP_BAKE_ENABLE
			});

			oSerializeObject.ExSetPropertyValue(KEditorDefine.B_PROPERTY_NAME_ENABLE_REALTIME_LIGHTMAPS, (a_oProperty) => {
#if REALTIME_LIGHTMAP_ENABLE
				a_oProperty.boolValue = true;
#else
				a_oProperty.boolValue = false;
#endif			// #if REALTIME_LIGHTMAP_ENABLE
			});
		}
	}

	//! 계층 뷰 UI 상태를 갱신한다
	private static void UpdateHierarchyUIState(int a_nInstanceID, Rect a_stRect) {
		var oGameObject = EditorUtility.InstanceIDToObject(a_nInstanceID) as GameObject;

		if(oGameObject != null) {
			a_stRect.size = new Vector2(KEditorDefine.B_HIERARCHY_WIDTH, a_stRect.size.y);
			a_stRect.position += new Vector2(KEditorDefine.B_HIERARCHY_OFFSET_X, 0.0f);

			var oComponents = oGameObject.GetComponents<Component>();

			for(int i = 0; i < oComponents.Length; ++i) {
				if(oComponents[i] != null) {
					var oType = oComponents[i].GetType();
					
					var oSortingLayerProperty = oType.GetProperty(KEditorDefine.B_PROPERTY_NAME_SORTING_LAYER,
						BindingFlags.Public | BindingFlags.Instance);

					var oSortingOrderProperty = oType.GetProperty(KEditorDefine.B_PROPERTY_NAME_SORTING_ORDER,
						BindingFlags.Public | BindingFlags.Instance);

					if(oSortingLayerProperty != null && oSortingOrderProperty != null) {
						string oString = string.Format(KEditorDefine.U_SORTING_ORDER_INFO_FORMAT, oSortingLayerProperty.GetValue(oComponents[i]),
							oSortingOrderProperty.GetValue(oComponents[i]));

						GUI.Label(a_stRect, oString, m_oHierarchyGUIStyle);
					}
				}
			}
		}
	}
	#endregion			// 클래스 함수

	#region 조건부 클래스 함수
#if FILE_BROWSER_ENABLE
	//! 파일 브라우저 UI 를 설정한다
	private static void SetupFileBrowserUI() {
		var oFileBrowserUI = Resources.Load<GameObject>(KEditorDefine.B_OBJ_PATH_FILE_BROWSER_UI);

		if(oFileBrowserUI != null) {
			var oCanvas = oFileBrowserUI.GetComponentInChildren<Canvas>();
			oCanvas.sortingOrder = KDefine.U_SORTING_ORDER_FILE_BROWSER_UI;

			var oCanvasScaler = oFileBrowserUI.GetComponentInChildren<CanvasScaler>();
			oCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			oCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
			oCanvasScaler.referenceResolution = new Vector2(KDefine.B_SCREEN_WIDTH, KDefine.B_SCREEN_HEIGHT);
			oCanvasScaler.referencePixelsPerUnit = KDefine.B_REF_PIXELS_UNIT;

			var oFileBrowserWindow = oFileBrowserUI.ExFindChild(KEditorDefine.B_OBJ_NAME_FILE_BROWSER_WINDOW);
			oFileBrowserWindow.transform.localScale = KDefine.B_SCALE_NORMAL * KEditorDefine.B_SCALE_FILE_BROWSER_WINDOW;
		}
	}
#endif			// #if FILE_BROWSER_ENABLE
	#endregion			// 조건부 클래스 함수
}
