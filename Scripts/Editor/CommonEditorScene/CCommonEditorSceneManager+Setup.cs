using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneTemplate;
using UnityEditor.SceneManagement;

#if INPUT_SYSTEM_MODULE_ENABLE
using UnityEngine.InputSystem;

#if UNITY_IOS
using UnityEngine.InputSystem.iOS;
#endif			// #if UNITY_IOS
#endif			// #if INPUT_SYSTEM_MODULE_ENABLE

/** 공용 에디터 씬 관리자 - 설정 */
public static partial class CCommonEditorSceneManager {
	#region 클래스 함수
	/** 태그를 설정한다 */
	private static void SetupTags() {
		var oLights = Resources.FindObjectsOfTypeAll<Light>();
		var oCameras = Resources.FindObjectsOfTypeAll<Camera>();
		var oSceneManagers = Resources.FindObjectsOfTypeAll<CSceneManager>();

		for(int i = 0; i < oLights.Length; ++i) {
			// 메인 광원 일 경우
			if(oLights[i].name.Equals(KCDefine.U_OBJ_N_SCENE_MAIN_LIGHT)) {
				oLights[i].type = LightType.Directional;
				oLights[i].lightmapBakeType = LightmapBakeType.Mixed;

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
					}
				}
			}
		}
	}

	/** 콜백을 설정한다 */
	private static void SetupCallbacks() {
		EditorApplication.update -= CCommonEditorSceneManager.Update;
		EditorApplication.update += CCommonEditorSceneManager.Update;

		EditorApplication.hierarchyWindowItemOnGUI -= CCommonEditorSceneManager.UpdateHierarchyUIState;
		EditorApplication.hierarchyWindowItemOnGUI += CCommonEditorSceneManager.UpdateHierarchyUIState;
	}

	/** 광원 옵션을 설정한다 */
	private static void SetupLightOpts() {
		var oSampleSceneNameList = new List<string>() {
			KCDefine.B_SCENE_N_SAMPLE, KCDefine.B_SCENE_N_EDITOR_SAMPLE, KCDefine.B_SCENE_N_STUDY_SAMPLE
		};

		var stScene = EditorSceneManager.GetActiveScene();

		// 광원 설정이 가능 할 경우
		if(stScene.IsValid() && !oSampleSceneNameList.Contains(stScene.name)) {
			bool bIsValid = Lightmapping.TryGetLightingSettings(out LightingSettings oLightingSettings);
			
			// 광원 설정이 없을 경우
			if((!bIsValid || oLightingSettings.name.Contains(KCEditorDefine.B_ASSET_N_LIGHTING_SETTINGS_TEMPLATE)) && CAccess.IsExistsRes<LightingSettings>(KCDefine.U_ASSET_P_G_LIGHTING_SETTINGS, true)) {
				EditorSceneManager.MarkSceneDirty(stScene);
				Lightmapping.SetLightingSettingsForScene(stScene, Resources.Load<LightingSettings>(KCDefine.U_ASSET_P_G_LIGHTING_SETTINGS));
			}

			// 광원 설정이 존재 할 경우
			if(oLightingSettings != null) {
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
	}

	/** 씬 템플릿을 설정한다 */
	private static void SetupSceneTemplates() {
		// 샘플 씬 템플릿이 존재 할 경우
		if(CEditorAccess.IsExistsAsset(KCEditorDefine.B_ASSET_P_SAMPLE_SCENE_TEMPLATE)) {
			CCommonEditorSceneManager.DoSetupSceneTemplates(CEditorFunc.FindAsset<SceneTemplateAsset>(KCEditorDefine.B_ASSET_P_SAMPLE_SCENE_TEMPLATE));
		}

		// 에디터 샘플 씬 템플릿이 존재 할 경우
		if(CEditorAccess.IsExistsAsset(KCEditorDefine.B_ASSET_P_EDITOR_SAMPLE_SCENE_TEMPLATE)) {
			CCommonEditorSceneManager.DoSetupSceneTemplates(CEditorFunc.FindAsset<SceneTemplateAsset>(KCEditorDefine.B_ASSET_P_EDITOR_SAMPLE_SCENE_TEMPLATE));
		}

#if STUDY_MODULE_ENABLE
		// 스터디 샘플 씬 템플릿이 존재 할 경우
		if(CEditorAccess.IsExistsAsset(KCEditorDefine.B_ASSET_P_STUDY_SAMPLE_SCENE_TEMPLATE)) {
			CCommonEditorSceneManager.DoSetupSceneTemplates(CEditorFunc.FindAsset<SceneTemplateAsset>(KCEditorDefine.B_ASSET_P_STUDY_SAMPLE_SCENE_TEMPLATE));
		}
#endif			// #if STUDY_MODULE_ENABLE
	}

	/** 씬 템플릿을 설정한다 */
	private static void DoSetupSceneTemplates(SceneTemplateAsset a_oSceneTemplate) {
		for(int i = 0; i < a_oSceneTemplate.dependencies.Length; ++i) {
			a_oSceneTemplate.dependencies[i].instantiationMode = TemplateInstantiationMode.Reference;
		}
	}
	#endregion			// 클래스 함수

	#region 클래스 조건부 함수
#if INPUT_SYSTEM_MODULE_ENABLE
	/** 입력 시스템을 설정한다 */
	private static void SetupInputSystem() {
		// 입력 시스템 설정이 없을 경우
		if(!EditorBuildSettings.TryGetConfigObject<InputSettings>(KCEditorDefine.B_MODULE_N_INPUT_SYSTEM, out InputSettings oInputSettings)) {
			oInputSettings = AssetDatabase.LoadAssetAtPath<InputSettings>(KCEditorDefine.B_ASSET_P_INPUT_SETTINGS);
			oInputSettings = oInputSettings ?? CEditorFactory.CreateScriptableObj<InputSettings>(KCEditorDefine.B_ASSET_P_INPUT_SETTINGS);

			InputSystem.settings = oInputSettings;
			EditorBuildSettings.AddConfigObject(KCEditorDefine.B_MODULE_N_INPUT_SYSTEM, oInputSettings, true);
		}

		oInputSettings.filterNoiseOnCurrent = false;
		oInputSettings.compensateForScreenOrientation = true;

		oInputSettings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
		oInputSettings.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.PointersAndKeyboardsRespectGameViewFocus;

		oInputSettings.iOS.motionUsage = new UnityEngine.InputSystem.iOS.PrivacyDataUsage() {
			enabled = false, usageDescription = (CPlatformOptsSetter.OptsInfoTable != null) ? CPlatformOptsSetter.OptsInfoTable.BuildOptsInfo.m_stiOSBuildOptsInfo.m_oMotionDescription : string.Empty
		};
	}
#endif			// #if INPUT_SYSTEM_MODULE_ENABLE

#if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE
	/** 렌더링 파이프라인을 설정한다 */
	private static void SetupRenderingPipeline() {
		// 렌더링 파이프라인 설정이 존재 할 경우
		if(CEditorAccess.IsExistsAsset(KCEditorDefine.B_ASSET_P_UNIVERSAL_RP_SETTINGS)) {
			var oSerializeObj = CEditorFactory.CreateSerializeObj(KCEditorDefine.B_ASSET_P_UNIVERSAL_RP_SETTINGS);
			oSerializeObj.ExSetPropertyVal(KCEditorDefine.B_PROPERTY_N_STRIP_DEBUG_VARIANTS, (a_oProperty) => a_oProperty.boolValue = true);
			oSerializeObj.ExSetPropertyVal(KCEditorDefine.B_PROPERTY_N_STRIP_UNUSED_VARIANTS, (a_oProperty) => a_oProperty.boolValue = true);
			oSerializeObj.ExSetPropertyVal(KCEditorDefine.B_PROPERTY_N_STRIP_UNUSED_POST_PROCESSING_VARIANTS, (a_oProperty) => a_oProperty.boolValue = true);
		}
	}	
#endif			// #if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE

#if BURST_COMPILER_MODULE_ENABLE
	/** 버스트 컴파일러를 설정한다 */
	private static void SetupBurstCompiler() {
		// Do Something
	}
#endif			// #if BURST_COMPILER_MODULE_ENABLE
	#endregion			// 클래스 조건부 함수
}
#endif			// #if UNITY_EDITOR
