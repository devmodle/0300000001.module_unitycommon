using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

//! 공용 에디터 씬 관리자 - 설정
public static partial class CCommonEditorSceneManager {
	#region 클래스 함수
	//! 콜백을 설정한다
	private static void SetupCallbacks() {
		EditorApplication.update -= CCommonEditorSceneManager.Update;
		EditorApplication.update += CCommonEditorSceneManager.Update;

		EditorApplication.playModeStateChanged -= CCommonEditorSceneManager.OnChangePlayMode;
		EditorApplication.playModeStateChanged += CCommonEditorSceneManager.OnChangePlayMode;

		EditorApplication.hierarchyWindowItemOnGUI -= CCommonEditorSceneManager.UpdateHierarchyUIState;
		EditorApplication.hierarchyWindowItemOnGUI += CCommonEditorSceneManager.UpdateHierarchyUIState;
		
		EditorSceneManager.sceneOpened -= CCommonEditorSceneManager.OnSceneOpen;
		EditorSceneManager.sceneOpened += CCommonEditorSceneManager.OnSceneOpen;
	}

	//! 씬을 설정한다
	private static void SetupScene() {
		bool bIsExistsUICamera = false;
		bool bIsExistsMainCamera = false;
		bool bIsExistsMainLight = false;

		var oLights = Resources.FindObjectsOfTypeAll<Light>();
		var oCameras = Resources.FindObjectsOfTypeAll<Camera>();
		var oSceneManagers = Resources.FindObjectsOfTypeAll<CSceneManager>();

		for(int i = 0; i < oLights.Length; ++i) {
			// 메인 광원 일 경우
			if(oLights[i].name.ExIsEquals(KCDefine.U_OBJ_NAME_SCENE_MAIN_LIGHT)) {
				bIsExistsMainLight = true;

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
				if(!oCameras[j].name.ExIsEquals(KCEditorDefine.B_OBJ_NAME_SCENE_EDITOR_CAMERA)) {
					bool bIsUICamera = oCameras[j].name.ExIsEquals(KCDefine.U_OBJ_NAME_SCENE_UI_CAMERA);
					bool bIsMainCamera = oCameras[j].name.ExIsEquals(KCDefine.U_OBJ_NAME_SCENE_MAIN_CAMERA);

					bIsExistsUICamera = bIsUICamera ? true : bIsExistsUICamera;
					bIsExistsMainCamera = bIsMainCamera ? true : bIsExistsMainCamera;

					// UI 카메라 태그 설정이 가능 할 경우
					if(bIsUICamera && !oCameras[j].CompareTag(KCDefine.U_TAG_UI_CAMERA)) {
						oCameras[j].tag = KCDefine.U_TAG_UI_CAMERA;
					}
					// 메인 카메라 태그 설정이 가능 할 경우
					else if(bIsMainCamera && !oCameras[j].CompareTag(KCDefine.U_TAG_MAIN_CAMERA)) {
						oCameras[j].tag = KCDefine.U_TAG_MAIN_CAMERA;
					}

					// 현재 씬 관리자 일 경우
					if(oSceneManagers[i].SceneName.ExIsEquals(oSceneManagers[i].gameObject.scene.name)) {
#if CAMERA_STACK_ENABLE
						oCameras[j].gameObject.SetActive(bIsUICamera || bIsMainCamera);
#else
						oCameras[j].gameObject.SetActive(bIsMainCamera);
#endif			// #if CAMERA_STACK_ENABLE
					}
				}
			}
		}

		// UI 카메라가 없을 경우
		if(!bIsExistsUICamera) {
			CFunc.ShowLogWarning(string.Format("{0} 객체가 없습니다.", KCDefine.U_OBJ_NAME_SCENE_UI_CAMERA));
		}
		// 메인 카메라가 없을 경우
		else if(!bIsExistsMainCamera) {
			CFunc.ShowLogWarning(string.Format("{0} 객체가 없습니다.", KCDefine.U_OBJ_NAME_SCENE_MAIN_CAMERA));
		}
		// 메인 광원이 없을 경우
		else if(!bIsExistsMainLight) {
			CFunc.ShowLogWarning(string.Format("{0} 객체가 없습니다.", KCDefine.U_OBJ_NAME_SCENE_MAIN_LIGHT));
		}

		// FPS 카운터를 설정한다 {
#if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
		string oFilter = Path.GetFileNameWithoutExtension(KCDefine.U_OBJ_PATH_FPS_COUNTER);

		var oFPSCounterList = CEditorFunc.FindAssets<GameObject>(oFilter, new string[] {
			KCEditorDefine.B_DIR_PATH_FILTER_FPS_COUNTER
		});

		// FPS 카운터가 존재 할 경우
		if(oFPSCounterList.ExIsValid()) {
			for(int i = 0; i < oFPSCounterList.Count; ++i) {
				var oStaticText = oFPSCounterList[i].ExFindComponent<Text>(KCEditorDefine.B_OBJ_NAME_STATIC_TEXT);
				oStaticText.fontSize = KCEditorDefine.B_FONT_SIZE_STATIC_TEXT;

				var oDynamicText = oFPSCounterList[i].ExFindComponent<Text>(KCEditorDefine.B_OBJ_NAME_DYNAMIC_TEXT);
				oDynamicText.fontSize = KCEditorDefine.B_FONT_SIZE_DYNAMIC_TEXT;

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
				oStaticText.rectTransform.anchoredPosition = KCEditorDefine.B_POSITION_STATIC_TEXT;

				oDynamicText.rectTransform.pivot = KCDefine.B_ANCHOR_BOTTOM_RIGHT;
				oDynamicText.rectTransform.anchorMin = KCDefine.B_ANCHOR_MIDDLE_RIGHT;
				oDynamicText.rectTransform.anchorMax = KCDefine.B_ANCHOR_MIDDLE_RIGHT;
				oDynamicText.rectTransform.anchoredPosition = KCEditorDefine.B_POSITION_DYNAMIC_TEXT;
				// 위치를 설정한다 }
			}
		}
#endif			// #if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
		// FPS 카운터를 설정한다 }
	}
	
	//! 광원 옵션을 설정한다
	private static void SetupLightOpts() {
		// 광원 맵 설정이 존재 할 경우
		if(Lightmapping.TryGetLightingSettings(out LightingSettings oLightingSettings)) {
			oLightingSettings.realtimeGI = false;
			oLightingSettings.realtimeEnvironmentLighting = false;
			
			oLightingSettings.lightmapper = KCEditorDefine.B_EDITOR_OPT_LIGHTMAPPER;
			oLightingSettings.mixedBakeMode = KCEditorDefine.B_EDITOR_OPT_LIGHTMAP_BAKE_MODE;

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
			var stScene = EditorSceneManager.GetActiveScene();

			// 경로가 유효 할 경우
			if(stScene.path.ExIsValid()) {
				var oScenePath = Path.GetDirectoryName(stScene.path);
				var oSceneName = Path.GetFileNameWithoutExtension(stScene.path);

				string oFilepath = string.Format(KCEditorDefine.B_ASSET_PATH_FORMAT_LIGHTING_SETTINGS, 
					oScenePath, oSceneName);
				
				var oLightingSettingsAsset = CEditorFunc.FindAsset<LightingSettings>(oFilepath);

				// 광원 맵 설정 에셋이 존재 할 경우
				if(oLightingSettingsAsset != null) {
					Lightmapping.lightingSettings = oLightingSettingsAsset;
				} else {
					var oSettings = Resources.Load<LightingSettings>(KCDefine.U_ASSET_PATH_LIGHTING_SETTINGS);
					var oPropertyInfos = oSettings.GetType().GetProperties(KCDefine.B_BINDING_FLAG_PUBLIC_INSTANCE);

					oLightingSettingsAsset = new LightingSettings();

					for(int i = 0; i < oPropertyInfos.Length; ++i) {
						var oPropertyInfo = oPropertyInfos[i];

						oLightingSettings.ExSetPropertyValue<LightingSettings>(oPropertyInfo.Name, 
							KCDefine.B_BINDING_FLAG_PUBLIC_INSTANCE, oPropertyInfo.GetValue(oSettings));
					}

					CEditorFactory.CreateAsset(oLightingSettingsAsset, oFilepath, false);
				}
			}
		}
	}
	#endregion			// 클래스 함수

	#region 조건부 클래스 함수
#if FILE_BROWSER_ENABLE
	//! 파일 브라우저 UI 를 설정한다
	private static void SetupFileBrowserUI() {
		var oFileBrowserUI = Resources.Load<GameObject>(KCEditorDefine.B_OBJ_PATH_FILE_BROWSER_UI);

		// 파일 브라우저 UI 가 존재 할 경우
		if(oFileBrowserUI != null) {
			var oCanvas = oFileBrowserUI.GetComponentInChildren<Canvas>();
			oCanvas.sortingOrder = KCDefine.U_SORTING_ORDER_FILE_BROWSER_UI;

			var oCanvasScaler = oFileBrowserUI.GetComponentInChildren<CanvasScaler>();
			oCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			oCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
			oCanvasScaler.referenceResolution = new Vector2(KCDefine.B_SCREEN_WIDTH, KCDefine.B_SCREEN_HEIGHT);
			oCanvasScaler.referencePixelsPerUnit = KCDefine.B_REF_PIXELS_UNIT;

			var oFileBrowserWindow = oFileBrowserUI.ExFindChild(KCEditorDefine.B_OBJ_NAME_FILE_BROWSER_WINDOW);
			oFileBrowserWindow.transform.localScale = KCDefine.B_SCALE_NORMAL * KCEditorDefine.B_SCALE_FILE_BROWSER_WINDOW;
		}
	}
#endif			// #if FILE_BROWSER_ENABLE
	#endregion			// 조건부 클래스 함수
}
#endif			// #if UNITY_EDITOR
