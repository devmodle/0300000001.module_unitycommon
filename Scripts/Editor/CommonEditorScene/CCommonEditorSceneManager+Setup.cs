using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

#if INPUT_SYSTEM_MODULE_ENABLE
using UnityEngine.InputSystem;
#endif			// #if INPUT_SYSTEM_MODULE_ENABLE

#if UNIVERSAL_RENDER_PIPELINE_MODULE_ENABLE
using UnityEngine.Rendering.Universal;
#endif			// #if UNIVERSAL_RENDER_PIPELINE_MODULE_ENABLE

/** 공용 에디터 씬 관리자 - 설정 */
public static partial class CCommonEditorSceneManager {
	#region 클래스 함수
	/** 콜백을 설정한다 */
	private static void SetupCallbacks() {
		EditorApplication.update -= CCommonEditorSceneManager.Update;
		EditorApplication.update += CCommonEditorSceneManager.Update;

		EditorApplication.hierarchyWindowItemOnGUI -= CCommonEditorSceneManager.UpdateHierarchyUIState;
		EditorApplication.hierarchyWindowItemOnGUI += CCommonEditorSceneManager.UpdateHierarchyUIState;
	}

	/** 씬을 설정한다 */
	private static void SetupScene() {
		var oLights = Resources.FindObjectsOfTypeAll<Light>();
		var oCameras = Resources.FindObjectsOfTypeAll<Camera>();
		var oSceneManagers = Resources.FindObjectsOfTypeAll<CSceneManager>();

		for(int i = 0; i < oLights.Length; ++i) {
			// 메인 광원 일 경우
			if(oLights[i].name.Equals(KCDefine.U_OBJ_N_SCENE_MAIN_LIGHT)) {
				oLights[i].type = LightType.Directional;
				oLights[i].lightmapBakeType = KCDefine.U_LIGHTMAP_BAKE_TYPE_DIRECTIONAL;

				oLights[i].ExSetTag(KCDefine.U_TAG_MAIN_LIGHT);
			}
		}

		for(int i = 0; i < oSceneManagers.Length; ++i) {
			oSceneManagers[i].ExSetTag(KCDefine.U_TAG_SCENE_MANAGER);

			for(int j = 0; j < oCameras.Length; ++j) {
				// 에디터 카메라가 아닐 경우
				if(!oCameras[j].name.Equals(KCEditorDefine.B_OBJ_N_SCENE_EDITOR_CAMERA)) {
					bool bIsUIsCamera = oCameras[j].name.Equals(KCDefine.U_OBJ_N_SCENE_UIS_CAMERA);
					bool bIsMainCamera = oCameras[j].name.Equals(KCDefine.U_OBJ_N_SCENE_MAIN_CAMERA);

					// 태그 설정이 가능 할 경우
					if(bIsUIsCamera || bIsMainCamera) {
						oCameras[j].ExSetTag(bIsUIsCamera ? KCDefine.U_TAG_UIS_CAMERA : KCDefine.U_TAG_MAIN_CAMERA);

#if UNIVERSAL_RENDER_PIPELINE_MODULE_ENABLE
						oCameras[j].gameObject.ExAddComponent<UniversalAdditionalCameraData>();
#endif			// #if UNIVERSAL_RENDER_PIPELINE_MODULE_ENABLE
					}

					// 메인 씬 일 경우
					if(oSceneManagers[i].SceneName.Equals(oSceneManagers[i].gameObject.scene.name)) {
#if CAMERA_STACKING_ENABLE
						oCameras[j].gameObject.SetActive(bIsUIsCamera || bIsMainCamera);
#else
						oCameras[j].gameObject.SetActive(bIsMainCamera);
#endif			// #if CAMERA_STACKING_ENABLE
					}
				}
			}
		}

		// FPS 카운터를 설정한다 {
		string oFPSCounterFilter = Path.GetFileNameWithoutExtension(KCDefine.U_OBJ_P_FPS_COUNTER);

		var oFPSCounterList = CEditorFunc.FindAssets<GameObject>(oFPSCounterFilter, new List<string>() {
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
	
	/** 광원 옵션을 설정한다 */
	private static void SetupLightOpts() {
		var stScene = EditorSceneManager.GetActiveScene();

		// 광원 설정이 가능 할 경우
		if(stScene.name.ExIsValid()) {
			bool bIsValid = Lightmapping.TryGetLightingSettings(out LightingSettings oLightingSettings);
			bIsValid = bIsValid && !oLightingSettings.name.Contains(KCDefine.U_ASSET_N_LIGHTING_SETTINGS);

#if LIGHTMAP_BAKE_ENABLE
			bool bIsBakeGI = true;
#else
			bool bIsBakeGI = false;
#endif			// #if LIGHTMAP_BAKE_ENABLE

#if REALTIME_GI_ENABLE
			bool bIsRealtimeGI = true;
#else
			bool bIsRealtimeGI = false;
#endif			// #if REALTIME_GI_ENABLE

#if REALTIME_ENVIRONMENT_LIGHTING_ENABLE
			bool bIsRealtimeEnvironmentLighting = true;
#else
			bool bIsRealtimeEnvironmentLighting = false;
#endif			// #if REALTIME_ENVIRONMENT_LIGHTING_ENABLE

			// 광원 설정이 유효하지 않을 경우
			if(!bIsValid || !oLightingSettings.name.Contains(stScene.name)) {
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

			// GI 설정이 필요 할 경우
			if(oLightingSettings.bakedGI != bIsBakeGI || oLightingSettings.realtimeGI != bIsRealtimeGI || oLightingSettings.realtimeEnvironmentLighting != bIsRealtimeEnvironmentLighting) {
				oLightingSettings.bakedGI = bIsBakeGI;
				oLightingSettings.realtimeGI = bIsRealtimeGI;
				oLightingSettings.realtimeEnvironmentLighting = bIsRealtimeEnvironmentLighting;
			}

			// 광원 맵 설정이 필요 할 경우
			if(oLightingSettings.lightmapper != KCEditorDefine.B_EDITOR_OPTS_LIGHTMAPPER || oLightingSettings.mixedBakeMode != KCEditorDefine.B_EDITOR_OPTS_LIGHTMAP_BAKE_MODE) {
				oLightingSettings.lightmapper = KCEditorDefine.B_EDITOR_OPTS_LIGHTMAPPER;
				oLightingSettings.mixedBakeMode = KCEditorDefine.B_EDITOR_OPTS_LIGHTMAP_BAKE_MODE;
			}
		}
	}
	#endregion			// 클래스 함수

	#region 클래스 조건부 함수
#if INPUT_SYSTEM_MODULE_ENABLE
	/** 입력 시스템을 설정한다 */
	private static void SetupInputSystem() {
		// 입력 시스템 설정이 없을 경우
		if(!EditorBuildSettings.TryGetConfigObject<InputSettings>(KCEditorDefine.B_MODULE_N_INPUT_SYSTEM, out InputSettings oInputSettings)) {
			var oAsset = AssetDatabase.LoadAssetAtPath<InputSettings>(KCEditorDefine.B_ASSET_P_INPUT_SETTINGS);
			oAsset = oAsset ?? CEditorFactory.CreateScriptableObj<InputSettings>(KCEditorDefine.B_ASSET_P_INPUT_SETTINGS);
			
			EditorBuildSettings.AddConfigObject(KCEditorDefine.B_MODULE_N_INPUT_SYSTEM, oAsset, true);
		}

		oInputSettings.filterNoiseOnCurrent = false;
		oInputSettings.compensateForScreenOrientation = true;

		oInputSettings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
	}
#endif			// #if INPUT_SYSTEM_MODULE_ENABLE

#if UNIVERSAL_RENDER_PIPELINE_MODULE_ENABLE
	/** 렌더링 파이프라인을 설정한다 */
	private static void SetupRenderPipeline() {
		var oAsset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(KCEditorDefine.B_ASSET_P_UNIVERSAL_RP_SETTINGS);
		oAsset = oAsset ?? CEditorFactory.CreateScriptableObj<ScriptableObject>(KCEditorDefine.B_ASSET_P_UNIVERSAL_RP_SETTINGS);

		var oSerializeObj = new SerializedObject(oAsset);

		oSerializeObj.ExSetPropertyVal(KCEditorDefine.B_PROPERTY_N_STRIP_DEBUG_VARIANTS, (a_oProperty) => a_oProperty.boolValue = true);
		oSerializeObj.ExSetPropertyVal(KCEditorDefine.B_PROPERTY_N_STRIP_UNUSED_VARIANTS, (a_oProperty) => a_oProperty.boolValue = true);
		oSerializeObj.ExSetPropertyVal(KCEditorDefine.B_PROPERTY_N_STRIP_UNUSED_POST_PROCESSING_VARIANTS, (a_oProperty) => a_oProperty.boolValue = true);
	}	
#endif			// #if UNIVERSAL_RENDER_PIPELINE_MODULE_ENABLE

#if BURST_COMPILER_MODULE_ENABLE
	/** 버스트 컴파일러를 설정한다 */
	private static void SetupBurstCompiler() {
		// Do Something
	}
#endif			// #if BURST_COMPILER_MODULE_ENABLE
	#endregion			// 클래스 조건부 함수
}
#endif			// #if UNITY_EDITOR
