using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using IngameDebugConsole;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

#if UNIVERSAL_PIPELINE_MODULE_ENABLE
using UnityEngine.Rendering.Universal;
#endif			// #if UNIVERSAL_PIPELINE_MODULE_ENABLE

//! 공용 에디터 씬 관리자 - 설정
public static partial class CCommonEditorSceneManager {
	#region 클래스 함수
	//! 콜백을 설정한다
	private static void SetupCallbacks() {
		EditorApplication.update -= CCommonEditorSceneManager.Update;
		EditorApplication.update += CCommonEditorSceneManager.Update;

		EditorApplication.hierarchyWindowItemOnGUI -= CCommonEditorSceneManager.UpdateHierarchyUIState;
		EditorApplication.hierarchyWindowItemOnGUI += CCommonEditorSceneManager.UpdateHierarchyUIState;

		EditorApplication.playModeStateChanged -= CCommonEditorSceneManager.OnChangePlayMode;
		EditorApplication.playModeStateChanged += CCommonEditorSceneManager.OnChangePlayMode;
		
		EditorSceneManager.sceneOpened -= CCommonEditorSceneManager.OnSceneOpen;
		EditorSceneManager.sceneOpened += CCommonEditorSceneManager.OnSceneOpen;
	}

	//! 씬을 설정한다
	private static void SetupScene() {
		var oLights = Resources.FindObjectsOfTypeAll<Light>();
		var oCameras = Resources.FindObjectsOfTypeAll<Camera>();
		var oSceneManagers = Resources.FindObjectsOfTypeAll<CSceneManager>();

		for(int i = 0; i < oLights.Length; ++i) {
			// 메인 광원 일 경우
			if(oLights[i].name.ExIsEquals(KCDefine.U_OBJ_N_SCENE_MAIN_LIGHT)) {
				oLights[i].type = LightType.Directional;
				oLights[i].lightmapBakeType = KCDefine.U_LIGHTMAP_BAKE_TYPE_DIRECTIONAL;

				// 태그 설정이 필요 할 경우
				if(!oLights[i].CompareTag(KCDefine.U_TAG_MAIN_LIGHT)) {
					oLights[i].tag = KCDefine.U_TAG_MAIN_LIGHT;
				}
			}
		}

		for(int i = 0; i < oSceneManagers.Length; ++i) {
			// 태그 설정이 필요 할 경우
			if(!oSceneManagers[i].CompareTag(KCDefine.U_TAG_SCENE_MANAGER)) {
				oSceneManagers[i].tag = KCDefine.U_TAG_SCENE_MANAGER;
			}
			
			for(int j = 0; j < oCameras.Length; ++j) {
				// 에디터 카메라가 아닐 경우
				if(!oCameras[j].name.ExIsEquals(KCEditorDefine.B_OBJ_N_SCENE_EDITOR_CAMERA)) {
					bool bIsUICamera = oCameras[j].name.ExIsEquals(KCDefine.U_OBJ_N_SCENE_UI_CAMERA);
					bool bIsMainCamera = oCameras[j].name.ExIsEquals(KCDefine.U_OBJ_N_SCENE_MAIN_CAMERA);

					// UI 카메라 태그 설정이 가능 할 경우
					if(bIsUICamera && !oCameras[j].CompareTag(KCDefine.U_TAG_UI_CAMERA)) {
						oCameras[j].tag = KCDefine.U_TAG_UI_CAMERA;
					}
					// 메인 카메라 태그 설정이 가능 할 경우
					else if(bIsMainCamera && !oCameras[j].CompareTag(KCDefine.U_TAG_MAIN_CAMERA)) {
						oCameras[j].tag = KCDefine.U_TAG_MAIN_CAMERA;
					}

#if UNIVERSAL_PIPELINE_MODULE_ENABLE
					// UI, 메인 카메라가 존재 할 경우
					if(bIsUICamera || bIsMainCamera) {
						oCameras[j].gameObject.ExAddComponent<UniversalAdditionalCameraData>();
					}
#endif			// #if UNIVERSAL_PIPELINE_MODULE_ENABLE

					// 현재 씬 관리자 일 경우
					if(oSceneManagers[i].SceneName.ExIsEquals(oSceneManagers[i].gameObject.scene.name)) {
#if !CAMERA_STACK_ENABLE || UNIVERSAL_PIPELINE_MODULE_ENABLE
						oCameras[j].gameObject.SetActive(bIsMainCamera);
#else
						oCameras[j].gameObject.SetActive(bIsUICamera || bIsMainCamera);
#endif			// #if !CAMERA_STACK_ENABLE || UNIVERSAL_PIPELINE_MODULE_ENABLE
					}
				}
			}
		}
		
		// 디버그 콘솔을 설정한다 {
		string oDebugConsoleFilter = Path.GetFileNameWithoutExtension(KCDefine.U_OBJ_P_DEBUG_CONSOLE);
		string oDebugLogItemFilter = Path.GetFileNameWithoutExtension(KCDefine.U_OBJ_P_DEBUG_LOG_ITEM);

		var oDebugConsoleList = CEditorFunc.FindAssets<GameObject>(oDebugConsoleFilter, new string[] {
			KCEditorDefine.B_DIR_P_FILTER_DEBUG_CONSOLE
		});

		var oDebugLogItemList = CEditorFunc.FindAssets<GameObject>(oDebugLogItemFilter, new string[] {
			KCEditorDefine.B_DIR_P_FILTER_DEBUG_LOG_ITEM
		});

		// 디버그 콘솔이 존재 할 경우
		if(oDebugConsoleList.ExIsValid()) {
			for(int i = 0; i < oDebugConsoleList.Count; ++i) {
				var oLogWindow = oDebugConsoleList[i].ExFindChild(KCDefine.U_OBJ_N_DEBUG_C_LOG_WINDOW);
				var oEventSystem = oDebugConsoleList[i].ExFindChild(KCDefine.U_OBJ_N_SCENE_EVENT_SYSTEM);
				var oLogManager = oDebugConsoleList[i].GetComponentInChildren<DebugLogManager>();

				var oScrollView = oDebugConsoleList[i].GetComponentInChildren<ScrollRect>();
				oScrollView.movementType = ScrollRect.MovementType.Clamped;

				var oWindowTrans = oLogWindow.transform as RectTransform;
				oWindowTrans.pivot = KCDefine.B_ANCHOR_MIDDLE_CENTER;
				oWindowTrans.anchorMin = KCDefine.B_ANCHOR_BOTTOM_LEFT;
				oWindowTrans.anchorMax = KCDefine.B_ANCHOR_TOP_RIGHT;
				oWindowTrans.anchoredPosition = Vector2.zero;

				// 이벤트 시스템이 존재 할 경우
				if(oEventSystem != null) {
					CFactory.RemoveObj(oEventSystem, true);
				}

				// 로그 관리자가 존재 할 경우
				if(oLogManager != null && oDebugLogItemList.ExIsValid()) {
					var oSerializeObj = new SerializedObject(oLogManager);

					oSerializeObj.ExSetPropertyValue(KCEditorDefine.B_PROPERTY_N_DEBUG_C_LOG_ITEM_PREFAB, (a_oProperty) => 
						a_oProperty.objectReferenceValue = oDebugLogItemList[KCDefine.B_VALUE_INT_0]);
				}
			}
		}

		// 디버그 로그 아이템이 존재 할 경우
		if(oDebugLogItemList.ExIsValid()) {
			for(int i = 0; i < oDebugLogItemList.Count; ++i) {
				var oText = oDebugLogItemList[i].GetComponentInChildren<Text>();
				oText.fontSize = KCEditorDefine.B_FONT_SIZE_DEBUG_C_TEXT;

				var oTrans = oDebugLogItemList[i].transform as RectTransform;
				oTrans.pivot = KCDefine.B_ANCHOR_TOP_LEFT;
				oTrans.anchorMin = KCDefine.B_ANCHOR_TOP_LEFT;
				oTrans.anchorMax = KCDefine.B_ANCHOR_TOP_RIGHT;
				oTrans.sizeDelta = KCEditorDefine.B_SIZE_DEBUG_C_LOG_ITEM;
			}
		}
		// 디버그 콘솔을 설정한다 }

		// FPS 카운터를 설정한다 {
		string oFPSCounterFilter = Path.GetFileNameWithoutExtension(KCDefine.U_OBJ_P_FPS_COUNTER);

		var oFPSCounterList = CEditorFunc.FindAssets<GameObject>(oFPSCounterFilter, new string[] {
			KCEditorDefine.B_DIR_P_FILTER_FPS_COUNTER
		});

		// FPS 카운터가 존재 할 경우
		if(oFPSCounterList.ExIsValid()) {
			for(int i = 0; i < oFPSCounterList.Count; ++i) {
				var oStaticText = oFPSCounterList[i].ExFindComponent<Text>(KCDefine.U_OBJ_N_FPS_C_STATIC_TEXT);
				oStaticText.fontSize = KCEditorDefine.B_FONT_SIZE_FPS_C_STATIC_TEXT;

				var oDynamicText = oFPSCounterList[i].ExFindComponent<Text>(KCDefine.U_OBJ_N_FPS_C_DYNAMIC_TEXT);
				oDynamicText.fontSize = KCEditorDefine.B_FONT_SIZE_FPS_C_DYNAMIC_TEXT;

				// 크기를 설정한다 {
				var oStaticSizeFitter = oStaticText.gameObject.ExAddComponent<ContentSizeFitter>();
				oStaticSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
				oStaticSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

				var oDynamicSizeFitter = oDynamicText.gameObject.ExAddComponent<ContentSizeFitter>();
				oDynamicSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
				oDynamicSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
				// 크기를 설정한다 }

				// 위치를 설정한다 {
				oStaticText.rectTransform.pivot = KCDefine.B_ANCHOR_BOTTOM_RIGHT;
				oStaticText.rectTransform.anchorMin = KCDefine.B_ANCHOR_MIDDLE_RIGHT;
				oStaticText.rectTransform.anchorMax = KCDefine.B_ANCHOR_MIDDLE_RIGHT;
				oStaticText.rectTransform.anchoredPosition = KCEditorDefine.B_POS_FPS_C_STATIC_TEXT;

				oDynamicText.rectTransform.pivot = KCDefine.B_ANCHOR_BOTTOM_RIGHT;
				oDynamicText.rectTransform.anchorMin = KCDefine.B_ANCHOR_MIDDLE_RIGHT;
				oDynamicText.rectTransform.anchorMax = KCDefine.B_ANCHOR_MIDDLE_RIGHT;
				oDynamicText.rectTransform.anchoredPosition = KCEditorDefine.B_POS_FPS_C_DYNAMIC_TEXT;
				// 위치를 설정한다 }
			}
		}
		// FPS 카운터를 설정한다 }
	}
	
	//! 광원 옵션을 설정한다
	private static void SetupLightOpts() {
		var stScene = EditorSceneManager.GetActiveScene();
		LightingSettings oLightingSettings = null;
		
		// 광원 맵 설정이 존재 할 경우
		if(Lightmapping.TryGetLightingSettings(out oLightingSettings) && 
			oLightingSettings.name.ExIsContains(stScene.name)) 
		{
			oLightingSettings.realtimeGI = false;
			oLightingSettings.realtimeEnvironmentLighting = false;
			
			oLightingSettings.lightmapper = KCEditorDefine.B_EDITOR_OPTS_LIGHTMAPPER;
			oLightingSettings.mixedBakeMode = KCEditorDefine.B_EDITOR_OPTS_LIGHTMAP_BAKE_MODE;

#if LIGHTMAP_BAKE_ENABLE
			oLightingSettings.bakedGI = true;
#else
			oLightingSettings.bakedGI = false;
#endif			// #if LIGHTMAP_BAKE_ENABLE

#if LIGHTMAP_AUTO_BAKE_ENABLE
			oLightingSettings.autoGenerate = true;
#else
			oLightingSettings.autoGenerate = false;
#endif			// #if LIGHTMAP_AUTO_BAKE_ENABLE
		} else {
			var oScenePath = Path.GetDirectoryName(stScene.path);
			var oSceneName = Path.GetFileNameWithoutExtension(stScene.path);

			string oFilePath = string.Format(KCEditorDefine.B_ASSET_P_FMT_LIGHTING_SETTINGS, 
				oScenePath, oSceneName);
			
			var oLightingSettingsAsset = CEditorFunc.FindAsset<LightingSettings>(oFilePath);

			// 광원 맵 설정 에셋이 존재 할 경우
			if(oLightingSettingsAsset != null) {
				Lightmapping.lightingSettings = oLightingSettingsAsset;
			} else {
				oLightingSettingsAsset = new LightingSettings();
				var oSettings = Resources.Load<LightingSettings>(KCDefine.U_ASSET_P_LIGHTING_SETTINGS);

				var oType = oSettings.GetType();
				var oPropertyInfos = oType.GetProperties(KCDefine.B_BINDING_F_PUBLIC_INSTANCE);

				for(int i = 0; i < oPropertyInfos.Length; ++i) {
					var oPropertyInfo = oPropertyInfos[i];

					oLightingSettingsAsset.ExSetPropertyValue<LightingSettings>(oPropertyInfo.Name, 
						KCDefine.B_BINDING_F_PUBLIC_INSTANCE, oPropertyInfo.GetValue(oSettings));
				}

				CEditorFactory.CreateAsset(oLightingSettingsAsset, oFilePath, false);
			}
		}
	}

	//! 파일 브라우저 UI 를 설정한다
	private static void SetupFileBrowserUI() {
		var oFileBrowserUI = Resources.Load<GameObject>(KCEditorDefine.B_OBJ_P_FILE_BROWSER_UI);

		// 파일 브라우저 UI 가 존재 할 경우
		if(oFileBrowserUI != null) {
			var oCanvas = oFileBrowserUI.GetComponentInChildren<Canvas>();
			oCanvas.sortingOrder = KCDefine.U_SORTING_O_FILE_BROWSER_UI;

			var stResolution = new Vector2(KCDefine.B_SCREEN_WIDTH, KCDefine.B_SCREEN_HEIGHT);

			var oCanvasScaler = oFileBrowserUI.GetComponentInChildren<CanvasScaler>();
			oCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			oCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
			oCanvasScaler.referenceResolution = stResolution;
			oCanvasScaler.referencePixelsPerUnit = KCDefine.B_REF_PIXELS_UNIT;

			var oFileBrowserWindow = oFileBrowserUI.ExFindChild(KCEditorDefine.B_OBJ_N_FILE_BROWSER_WINDOW);
			oFileBrowserWindow.transform.localScale = KCDefine.B_SCALE_NORM * KCEditorDefine.B_SCALE_FILE_BROWSER_WINDOW;
		}
	}
	#endregion			// 클래스 함수
}
#endif			// #if UNITY_EDITOR
