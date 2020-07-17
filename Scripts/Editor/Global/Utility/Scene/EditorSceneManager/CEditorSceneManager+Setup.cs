using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

//! 에디터 씬 관리자 - 설정
public static partial class CEditorSceneManager {
	#region 클래스 함수
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
			KEditorDefine.B_DIR_PATH_AUTO_CREATE_RESES
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

	//! 스크립트 순서를 설정한다
	private static void SetupScriptOrders() {
		var oMonoScripts = MonoImporter.GetAllRuntimeMonoScripts();

		for(int i = 0; i < oMonoScripts.Length; ++i) {
			var oType = oMonoScripts[i].GetClass();

			if(oType != null) {
				if(KAppDefine.G_SCRIPT_ORDERS.ContainsKey(oType)) {
					Func.SetScriptOrder(oMonoScripts[i], KAppDefine.G_SCRIPT_ORDERS[oType]);
				} else if(KEditorDefine.G_SCRIPT_ORDERS.ContainsKey(oType)) {
					Func.SetScriptOrder(oMonoScripts[i], KEditorDefine.G_SCRIPT_ORDERS[oType]);
				}
			}
		}
	}

	//! 광원 옵션을 설정한다
	private static void SetupLightOptions() {
		var oType = typeof(LightmapEditorSettings);
		var oMethodInfo = oType.GetMethod(KEditorDefine.B_FUNC_NAME_GET_LIGHTMAP_SETTINGS, KDefine.B_BINDING_FLAG_NON_PUBLIC_STATIC);
		var oLightmapSettings = oMethodInfo?.Invoke(null, null) as LightmapSettings;

		if(oLightmapSettings != null) {
			var oSerializeObj = new SerializedObject(oLightmapSettings);

			oSerializeObj.ExSetPropertyValue(KEditorDefine.B_PROPERTY_NAME_ENABLE_BAKE_LIGHTMAPS, (a_oProperty) => {
#if LIGHTMAP_BAKE_ENABLE
				a_oProperty.boolValue = true;
#else
				a_oProperty.boolValue = false;
#endif			// #if LIGHTMAP_BAKE_ENABLE
			});

			oSerializeObj.ExSetPropertyValue(KEditorDefine.B_PROPERTY_NAME_ENABLE_REALTIME_LIGHTMAPS, (a_oProperty) => {
#if REALTIME_LIGHTMAP_ENABLE
				a_oProperty.boolValue = true;
#else
				a_oProperty.boolValue = false;
#endif			// #if REALTIME_LIGHTMAP_ENABLE
			});
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
