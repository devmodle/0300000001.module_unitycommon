using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using IngameDebugConsole;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

#if INPUT_SYSTEM_MODULE_ENABLE
using UnityEngine.InputSystem;
#endif			// #if INPUT_SYSTEM_MODULE_ENABLE

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
					bool bIsUIsCamera = oCameras[j].name.ExIsEquals(KCDefine.U_OBJ_N_SCENE_UIS_CAMERA);
					bool bIsMainCamera = oCameras[j].name.ExIsEquals(KCDefine.U_OBJ_N_SCENE_MAIN_CAMERA);

					// UI 카메라 태그 설정이 가능 할 경우
					if(bIsUIsCamera && !oCameras[j].CompareTag(KCDefine.U_TAG_UIS_CAMERA)) {
						oCameras[j].tag = KCDefine.U_TAG_UIS_CAMERA;
					}
					// 메인 카메라 태그 설정이 가능 할 경우
					else if(bIsMainCamera && !oCameras[j].CompareTag(KCDefine.U_TAG_MAIN_CAMERA)) {
						oCameras[j].tag = KCDefine.U_TAG_MAIN_CAMERA;
					}

#if UNIVERSAL_PIPELINE_MODULE_ENABLE
					// UI, 메인 카메라가 존재 할 경우
					if(bIsUIsCamera || bIsMainCamera) {
						oCameras[j].gameObject.ExAddComponent<UniversalAdditionalCameraData>();
					}
#endif			// #if UNIVERSAL_PIPELINE_MODULE_ENABLE

					// 현재 씬 관리자 일 경우
					if(oSceneManagers[i].SceneName.ExIsEquals(oSceneManagers[i].gameObject.scene.name)) {
#if !CAMERA_STACK_ENABLE || UNIVERSAL_PIPELINE_MODULE_ENABLE
						oCameras[j].gameObject.SetActive(bIsMainCamera);
#else
						oCameras[j].gameObject.SetActive(bIsUIsCamera || bIsMainCamera);
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
				var oLogWnd = oDebugConsoleList[i].ExFindChild(KCDefine.U_OBJ_N_DEBUG_C_LOG_WND);
				var oEventSystem = oDebugConsoleList[i].ExFindChild(KCDefine.U_OBJ_N_SCENE_EVENT_SYSTEM);
				var oLogManager = oDebugConsoleList[i].GetComponentInChildren<DebugLogManager>();

				var oScrollView = oDebugConsoleList[i].GetComponentInChildren<ScrollRect>();
				oScrollView.movementType = ScrollRect.MovementType.Clamped;

				var oWndTrans = oLogWnd.transform as RectTransform;
				oWndTrans.pivot = KCDefine.B_ANCHOR_MID_CENTER;
				oWndTrans.anchorMin = KCDefine.B_ANCHOR_DOWN_LEFT;
				oWndTrans.anchorMax = KCDefine.B_ANCHOR_UP_RIGHT;
				oWndTrans.anchoredPosition = Vector2.zero;

				// 이벤트 시스템이 존재 할 경우
				if(oEventSystem != null) {
					CFactory.RemoveObj(oEventSystem, true);
				}

				// 로그 관리자가 존재 할 경우
				if(oLogManager != null && oDebugLogItemList.ExIsValid()) {
					var oSerializeObj = new SerializedObject(oLogManager);

					oSerializeObj.ExSetPropertyVal(KCEditorDefine.B_PROPERTY_N_DEBUG_C_LOG_ITEM_PREFAB, (a_oProperty) => {
						var oDebugLogItem = oDebugLogItemList[KCDefine.B_VAL_0_INT];
						a_oProperty.objectReferenceValue = oDebugLogItem;
					});
				}
			}
		}

		// 디버그 로그 아이템이 존재 할 경우
		if(oDebugLogItemList.ExIsValid()) {
			for(int i = 0; i < oDebugLogItemList.Count; ++i) {
				var oText = oDebugLogItemList[i].GetComponentInChildren<Text>();
				oText.fontSize = KCEditorDefine.B_FONT_SIZE_DEBUG_C_TEXT;

				var oTrans = oDebugLogItemList[i].transform as RectTransform;
				oTrans.pivot = KCDefine.B_ANCHOR_UP_LEFT;
				oTrans.anchorMin = KCDefine.B_ANCHOR_UP_LEFT;
				oTrans.anchorMax = KCDefine.B_ANCHOR_UP_RIGHT;
				oTrans.sizeDelta = KCEditorDefine.B_SIZE_DEBUG_C_LOG_ITEM.ExTo2D();
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
				oStaticText.rectTransform.pivot = KCDefine.B_ANCHOR_DOWN_RIGHT;
				oStaticText.rectTransform.anchorMin = KCDefine.B_ANCHOR_MID_RIGHT;
				oStaticText.rectTransform.anchorMax = KCDefine.B_ANCHOR_MID_RIGHT;
				oStaticText.rectTransform.anchoredPosition = KCEditorDefine.B_POS_FPS_C_STATIC_TEXT.ExTo2D();

				oDynamicText.rectTransform.pivot = KCDefine.B_ANCHOR_DOWN_RIGHT;
				oDynamicText.rectTransform.anchorMin = KCDefine.B_ANCHOR_MID_RIGHT;
				oDynamicText.rectTransform.anchorMax = KCDefine.B_ANCHOR_MID_RIGHT;
				oDynamicText.rectTransform.anchoredPosition = KCEditorDefine.B_POS_FPS_C_DYNAMIC_TEXT.ExTo2D();
				// 위치를 설정한다 }
			}
		}
		// FPS 카운터를 설정한다 }
	}
	
	//! 광원 옵션을 설정한다
	private static void SetupLightOpts() {
		var stScene = EditorSceneManager.GetActiveScene();

		// 광원 설정이 가능 할 경우
		if(stScene.name.ExIsValid()) {
			bool bIsValid = Lightmapping.TryGetLightingSettings(out LightingSettings oLightingSettings);
			bIsValid = bIsValid && !oLightingSettings.name.ExIsContains(KCDefine.U_ASSET_N_LIGHTING_SETTINGS);

			// 광원 설정이 유효하지 않을 경우
			if(!bIsValid || !oLightingSettings.name.ExIsContains(stScene.name)) {
				var oScenePath = Path.GetDirectoryName(stScene.path);
				var oSceneName = Path.GetFileNameWithoutExtension(stScene.path);

				var oSettings = Resources.Load<LightingSettings>(KCDefine.U_ASSET_P_LIGHTING_SETTINGS);
				EditorSceneManager.MarkSceneDirty(stScene);
				
				var oLightingSettingsAsset = new LightingSettings();
				oLightingSettingsAsset.name = oSceneName;
				
				var oType = oSettings.GetType();
				var oPropertyInfos = oType.GetProperties(KCDefine.B_BINDING_F_PUBLIC_INSTANCE);

				for(int i = 0; i < oPropertyInfos.Length; ++i) {
					var oPropertyInfo = oPropertyInfos[i];
					oLightingSettingsAsset.ExSetPropertyVal<LightingSettings>(oPropertyInfo.Name, KCDefine.B_BINDING_F_PUBLIC_INSTANCE, oPropertyInfo.GetValue(oSettings));
				}

				oLightingSettings = oLightingSettingsAsset;
				string oFilePath = string.Format(KCEditorDefine.B_ASSET_P_FMT_LIGHTING_SETTINGS, oScenePath, oSceneName);

				CEditorFactory.RemoveAsset(oFilePath);
				CEditorFactory.CreateAsset(oLightingSettingsAsset, oFilePath, false);

				Lightmapping.SetLightingSettingsForScene(stScene, oLightingSettingsAsset);
			}

			// 라이트맵 설정이 필요 할 경우
			if(oLightingSettings.lightmapper != KCEditorDefine.B_EDITOR_OPTS_LIGHTMAPPER || oLightingSettings.mixedBakeMode != KCEditorDefine.B_EDITOR_OPTS_LIGHTMAP_BAKE_MODE) {
				oLightingSettings.lightmapper = KCEditorDefine.B_EDITOR_OPTS_LIGHTMAPPER;
				oLightingSettings.mixedBakeMode = KCEditorDefine.B_EDITOR_OPTS_LIGHTMAP_BAKE_MODE;
			}

#if LIGHTMAP_BAKE_ENABLE
			bool bIsBakedGI = true;
#else
			bool bIsBakedGI = false;
#endif			// #if LIGHTMAP_BAKE_ENABLE

#if LIGHTMAP_AUTO_BAKE_ENABLE
			bool bIsAutoGenerate = true;
#else
			bool bIsAutoGenerate = false;
#endif			// #if LIGHTMAP_AUTO_BAKE_ENABLE

			// GI 설정이 필요 할 경우
			if(oLightingSettings.bakedGI != bIsBakedGI || oLightingSettings.autoGenerate != bIsAutoGenerate) {
				oLightingSettings.bakedGI = bIsBakedGI;
				oLightingSettings.autoGenerate = bIsAutoGenerate;
			}
		}
	}
	#endregion			// 클래스 함수

	#region 클래스 조건부 함수
#if INPUT_SYSTEM_MODULE_ENABLE
	//! 입력 시스템을 설정한다
	private static void SetupInputSystem() {
		// 입력 시스템 설정이 없을 경우
		if(!EditorBuildSettings.TryGetConfigObject<InputSettings>(KCEditorDefine.B_MODULE_N_INPUT_SYSTEM, out InputSettings oInputSettings)) {
			var oAsset = AssetDatabase.LoadAssetAtPath<InputSettings>(KCEditorDefine.B_ASSET_P_INPUT_SETTINGS);
			oAsset = oAsset ?? CEditorFactory.CreateScriptableObj<InputSettings>(KCEditorDefine.B_ASSET_P_INPUT_SETTINGS);
			
			EditorBuildSettings.AddConfigObject(KCEditorDefine.B_MODULE_N_INPUT_SYSTEM, oAsset, true);
		}
	}
#endif			// #if INPUT_SYSTEM_MODULE_ENABLE
	#endregion			// 클래스 조건부 함수
}
#endif			// #if UNITY_EDITOR
