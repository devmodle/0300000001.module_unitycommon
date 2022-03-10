using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Unity.Linq;

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
			if(oLights[i].name.Equals(KCDefine.U_OBJ_N_SCENE_MAIN_DIRECTIONAL_LIGHT)) {
				oLights[i].type = LightType.Directional;
				oLights[i].ExSetTag(KCDefine.U_TAG_MAIN_DIRECTIONAL_LIGHT);
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

	/** 레이어를 설정한다 */
	private static void SetupLayers() {
		CFunc.EnumerateRootObjs((a_oObj) => {
			// 최상단 UI 일 경우
			if(a_oObj.name.Equals(KCDefine.U_OBJ_N_SCENE_UIS_TOP)) {
				var oEnumerator = a_oObj.DescendantsAndSelf();

				foreach(var oObj in oEnumerator) {
					// 레이어 설정이 필요 할 경우
					if(oObj.layer != KCDefine.U_LAYER_UIS) {
						oObj.ExSetLayer(KCDefine.U_LAYER_UIS, false);
					}
				}
			}

			return true;
		});
	}

	/** 콜백을 설정한다 */
	private static void SetupCallbacks() {
		EditorApplication.update -= CCommonEditorSceneManager.Update;
		EditorApplication.update += CCommonEditorSceneManager.Update;

		EditorApplication.hierarchyWindowItemOnGUI -= CCommonEditorSceneManager.UpdateHierarchyUIState;
		EditorApplication.hierarchyWindowItemOnGUI += CCommonEditorSceneManager.UpdateHierarchyUIState;

		EditorApplication.playModeStateChanged -= CCommonEditorSceneManager.OnUpdatePlayModeState;
		EditorApplication.playModeStateChanged += CCommonEditorSceneManager.OnUpdatePlayModeState;
	}

	/** 광원 옵션을 설정한다 */
	private static void SetupLightOpts() {
		var oSampleSceneNameList = new List<string>() {
			KCDefine.B_SCENE_N_SAMPLE, KCDefine.B_SCENE_N_EDITOR_SAMPLE, KCDefine.B_SCENE_N_STUDY_SAMPLE
		};

		var oLightingSettingsPathDict = new Dictionary<EQualityLevel, string>() {
			[EQualityLevel.NORM] = KCDefine.U_ASSET_P_G_NORM_QUALITY_LIGHTING_SETTINGS, [EQualityLevel.HIGH] = KCDefine.U_ASSET_P_G_HIGH_QUALITY_LIGHTING_SETTINGS, [EQualityLevel.ULTRA] = KCDefine.U_ASSET_P_G_ULTRA_QUALITY_LIGHTING_SETTINGS
		};

		var stScene = EditorSceneManager.GetActiveScene();

		foreach(var stKeyVal in oLightingSettingsPathDict) {
			CCommonEditorSceneManager.DoSetupLightOpts(stKeyVal.Key, Resources.Load<LightingSettings>(stKeyVal.Value), false);
		}

		// 광원 설정이 가능 할 경우
		if(stScene.IsValid() && !oSampleSceneNameList.Contains(stScene.name) && CPlatformOptsSetter.OptsInfoTable != null) {
			bool bIsValidA = Lightmapping.TryGetLightingSettings(out LightingSettings oLightingSettings);
			bool bIsValidB = oLightingSettings != null && oLightingSettings.name.Equals(KCEditorDefine.B_ASSET_N_LIGHTING_SETTINGS_TEMPLATE);
			bool bIsValidC = oLightingSettings != null && !oLightingSettings.name.Equals(Path.GetFileNameWithoutExtension(oLightingSettingsPathDict[CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_eQualityLevel]));

			var oResult = oLightingSettingsPathDict.ExFindVal((a_oLightingSettingsPath) => oLightingSettings != null && Path.GetFileNameWithoutExtension(a_oLightingSettingsPath).Equals(oLightingSettings.name));

			// 광원 설정이 없을 경우
			if((!bIsValidA || bIsValidB || (bIsValidC && oResult.Item1)) && CAccess.IsExistsRes<LightingSettings>(oLightingSettingsPathDict[CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_eQualityLevel], true)) {
				EditorSceneManager.MarkSceneDirty(stScene);
				Lightmapping.SetLightingSettingsForScene(stScene, Resources.Load<LightingSettings>(oLightingSettingsPathDict[CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_eQualityLevel]));
			}
		}
	}

	/** 정적 객체를 설정한다 */
	private static void SetupStaticObjs() {
		CFunc.EnumerateRootObjs((a_oObj) => {
			// 최상단 객체 일 경우
			if(a_oObj.name.Equals(KCDefine.U_OBJ_N_SCENE_BASE) || a_oObj.name.Equals(KCDefine.U_OBJ_N_SCENE_OBJS_BASE)) {
				var oEnumerator = a_oObj.ChildrenAndSelf();

				foreach(var oObj in oEnumerator) {
					// 플래그 설정이 필요 할 경우
					if(oObj.name.Equals(KCDefine.U_OBJ_N_SCENE_STATIC_OBJS) || oObj.name.Equals(KCDefine.U_OBJ_N_SCENE_ADDITIONAL_LIGHTS)) {
						oObj.ExSetStaticEditorFlags(KCEditorDefine.B_STATIC_EF_DEF);
					}
				}
			}

			return true;
		});
	}

	/** 지역화 정보를 설정한다 */
	private static void SetupLocalizeInfos() {
		// 지역화 정보 테이블이 존재 할 경우
		if(CPlatformOptsSetter.LocalizeInfoTable != null) {
			for(int i = 0; i < CPlatformOptsSetter.LocalizeInfoTable.LocalizeInfoList.Count; ++i) {
				for(int j = 0; j < CPlatformOptsSetter.LocalizeInfoTable.LocalizeInfoList[i].m_oFontSetInfoList.Count; ++j) {
					var stFontSetInfo = CPlatformOptsSetter.LocalizeInfoTable.LocalizeInfoList[i].m_oFontSetInfoList[j];
					stFontSetInfo.m_eSet = EFontSet.A + j;

					CPlatformOptsSetter.LocalizeInfoTable.LocalizeInfoList[i].m_oFontSetInfoList[j] = stFontSetInfo;
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

	/** 광원 옵션을 설정한다 */
	private static void DoSetupLightOpts(EQualityLevel a_eQualityLevel, LightingSettings a_oSettings, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oSettings != null);

		// 광원 설정이 존재 할 경우
		if(a_oSettings != null && CPlatformOptsSetter.OptsInfoTable != null) {
			var stRenderingOptsInfo = CPlatformOptsSetter.OptsInfoTable.GetRenderingOptsInfo(a_eQualityLevel);

			var oIsSetupOptsList = new List<bool>() {
				a_oSettings.ao,
				a_oSettings.bakedGI,
				a_oSettings.finalGather,
				a_oSettings.finalGatherFiltering,

				a_oSettings.realtimeGI == CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_bIsEnableRealtimeGI,
				a_oSettings.realtimeEnvironmentLighting == CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_bIsEnableRealtimeEnvironmentLighting,

				a_oSettings.lightmapper == CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_eLightmapper,
				a_oSettings.filteringMode == LightingSettings.FilterMode.Auto,
				a_oSettings.mixedBakeMode == CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_eMixedLightingMode,
				a_oSettings.directionalityMode == (LightmapsMode)stRenderingOptsInfo.m_stLightOptsInfo.m_eLightmapMode,

				a_oSettings.lightmapPadding == KCDefine.B_VAL_4_INT,
				a_oSettings.lightmapMaxSize == (int)stRenderingOptsInfo.m_stLightOptsInfo.m_eLightmapMaxSize,
				a_oSettings.finalGatherRayCount == (int)EPowOfTwo._512,
				a_oSettings.lightmapCompression == stRenderingOptsInfo.m_stLightOptsInfo.m_eLightmapCompression,

				a_oSettings.albedoBoost.Equals(KCDefine.B_VAL_1_FLT),
				a_oSettings.indirectScale.Equals(KCDefine.B_VAL_1_FLT),
				a_oSettings.indirectResolution.Equals(KCDefine.B_UNIT_LIGHTMAP_RESOLUTION),
				a_oSettings.lightmapResolution.Equals(KCDefine.B_UNIT_LIGHTMAP_RESOLUTION)
			};

			// 설정 갱신이 필요 할 경우
			if(oIsSetupOptsList.Contains(false)) {
				a_oSettings.ao = true;
				a_oSettings.bakedGI = true;
				a_oSettings.finalGather = true;
				a_oSettings.finalGatherFiltering = true;

				a_oSettings.realtimeGI = CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_bIsEnableRealtimeGI;
				a_oSettings.realtimeEnvironmentLighting = CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_bIsEnableRealtimeEnvironmentLighting;

				a_oSettings.lightmapper = CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_eLightmapper;
				a_oSettings.filteringMode = LightingSettings.FilterMode.Auto;
				a_oSettings.mixedBakeMode = CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_eMixedLightingMode;
				a_oSettings.directionalityMode = (LightmapsMode)stRenderingOptsInfo.m_stLightOptsInfo.m_eLightmapMode;

				a_oSettings.lightmapPadding = KCDefine.B_VAL_4_INT;
				a_oSettings.lightmapMaxSize = (int)stRenderingOptsInfo.m_stLightOptsInfo.m_eLightmapMaxSize;
				a_oSettings.finalGatherRayCount = (int)EPowOfTwo._512;
				a_oSettings.lightmapCompression = stRenderingOptsInfo.m_stLightOptsInfo.m_eLightmapCompression;

				a_oSettings.albedoBoost = KCDefine.B_VAL_1_INT;
				a_oSettings.indirectScale = KCDefine.B_VAL_1_INT;
				a_oSettings.indirectResolution = KCDefine.B_UNIT_LIGHTMAP_RESOLUTION;
				a_oSettings.lightmapResolution = KCDefine.B_UNIT_LIGHTMAP_RESOLUTION;
			}
		}
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

		var oIsSetupOptsList = new List<bool>() {
			oInputSettings.compensateForScreenOrientation,

			oInputSettings.updateMode == InputSettings.UpdateMode.ProcessEventsInDynamicUpdate,
			oInputSettings.editorInputBehaviorInPlayMode == InputSettings.EditorInputBehaviorInPlayMode.PointersAndKeyboardsRespectGameViewFocus,

#if UNITY_IOS
			oInputSettings.iOS.motionUsage.enabled == CPlatformOptsSetter.OptsInfoTable.BuildOptsInfo.m_stiOSBuildOptsInfo.m_bIsEnableMotion,
			oInputSettings.iOS.motionUsage.usageDescription.Equals((CPlatformOptsSetter.OptsInfoTable != null) ? CPlatformOptsSetter.OptsInfoTable.BuildOptsInfo.m_stiOSBuildOptsInfo.m_oMotionDescription : string.Empty)
#endif			// #if UNITY_IOS
		};
		
		// 설정 갱신이 필요 할 경우
		if(oIsSetupOptsList.Contains(false)) {
			oInputSettings.compensateForScreenOrientation = true;
			
			oInputSettings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
			oInputSettings.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.PointersAndKeyboardsRespectGameViewFocus;

#if UNITY_IOS
			oInputSettings.iOS.motionUsage.enabled = CPlatformOptsSetter.OptsInfoTable.BuildOptsInfo.m_stiOSBuildOptsInfo.m_bIsEnableMotion;
			oInputSettings.iOS.motionUsage.usageDescription = (CPlatformOptsSetter.OptsInfoTable != null) ? CPlatformOptsSetter.OptsInfoTable.BuildOptsInfo.m_stiOSBuildOptsInfo.m_oMotionDescription : string.Empty;
#endif			// #if UNITY_IOS
		}
	}
#endif			// #if INPUT_SYSTEM_MODULE_ENABLE

#if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE
	/** 렌더링 파이프라인을 설정한다 */
	private static void SetupRenderingPipeline() {
		// 렌더링 파이프라인 설정이 존재 할 경우
		if(CEditorAccess.IsExistsAsset(KCEditorDefine.B_ASSET_P_UNIVERSAL_RP_SETTINGS)) {
			var oSerializeObj = CEditorFactory.CreateSerializeObj(KCEditorDefine.B_ASSET_P_UNIVERSAL_RP_SETTINGS);

			var oIsSetupOptsList = new List<bool>() {
				oSerializeObj.FindProperty(KCEditorDefine.B_PROPERTY_N_STRIP_DEBUG_VARIANTS).boolValue,
				oSerializeObj.FindProperty(KCEditorDefine.B_PROPERTY_N_STRIP_UNUSED_VARIANTS).boolValue,
				oSerializeObj.FindProperty(KCEditorDefine.B_PROPERTY_N_STRIP_UNUSED_POST_PROCESSING_VARIANTS).boolValue
			};

			// 설정 갱신이 필요 할 경우
			if(oIsSetupOptsList.Contains(false)) {
				oSerializeObj.ExSetPropertyVal(KCEditorDefine.B_PROPERTY_N_STRIP_DEBUG_VARIANTS, (a_oProperty) => a_oProperty.boolValue = true);
				oSerializeObj.ExSetPropertyVal(KCEditorDefine.B_PROPERTY_N_STRIP_UNUSED_VARIANTS, (a_oProperty) => a_oProperty.boolValue = true);
				oSerializeObj.ExSetPropertyVal(KCEditorDefine.B_PROPERTY_N_STRIP_UNUSED_POST_PROCESSING_VARIANTS, (a_oProperty) => a_oProperty.boolValue = true);
			}
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
