using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Unity.Linq;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.U2D;
using UnityEditor.SceneTemplate;
using UnityEditor.SceneManagement;

#if INPUT_SYSTEM_MODULE_ENABLE
using UnityEngine.InputSystem;

#if UNITY_IOS
using UnityEngine.InputSystem.iOS;
#endif			// #if UNITY_IOS
#endif			// #if INPUT_SYSTEM_MODULE_ENABLE

#if NEWTON_SOFT_JSON_MODULE_ENABLE
using Newtonsoft.Json;
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE

/** 공용 에디터 씬 관리자 - 설정 */
public static partial class CCommonEditorSceneManager {
	#region 클래스 함수
	/** 에디터 씬 관리자를 설정한다 */
	private static IEnumerator SetupEditorSceneManager() {
		do {
			yield return null;
		} while(!CEditorAccess.IsEnableUpdateState);
		
		yield return CFactory.CreateWaitForSecs(KCDefine.B_VAL_1_FLT);

		CCommonEditorSceneManager.m_bIsEnableSetup = true;
		CCommonEditorSceneManager.m_bIsEnableBuild = true;
	}

	/** 태그를 설정한다 */
	private static void SetupTags() {
		var oLights = Resources.FindObjectsOfTypeAll<Light>();
		var oCameras = Resources.FindObjectsOfTypeAll<Camera>();
		var oSceneManagers = Resources.FindObjectsOfTypeAll<CSceneManager>();

		for(int i = 0; i < oLights.Length; ++i) {
			// 에디터 광원이 아닐 경우
			if(!KCEditorDefine.B_OBJ_N_SCENE_EDITOR_LIGHT_LIST.Contains(oLights[i].name)) {
				oLights[i].type = oLights[i].name.Equals(KCDefine.U_OBJ_N_SCENE_MAIN_LIGHT) ? LightType.Directional : oLights[i].type;

				// 메인 광원 일 경우
				if(oLights[i].name.Equals(KCDefine.U_OBJ_N_SCENE_MAIN_LIGHT)) {
					oLights[i].ExSetTag(KCDefine.U_TAG_MAIN_LIGHT);
				} else {
					oLights[i].ExSetTag((oLights[i].CompareTag(KCDefine.U_TAG_UNTAGGED) || oLights[i].CompareTag(KCDefine.U_TAG_MAIN_LIGHT)) ? KCDefine.U_TAG_ADDITIONAL_LIGHT : oLights[i].tag);
				}
			}
		}

		for(int i = 0; i < oCameras.Length; ++i) {
			// 에디터 카메라가 아닐 경우
			if(!KCEditorDefine.B_OBJ_N_SCENE_EDITOR_CAMERA_LIST.Contains(oCameras[i].name)) {
				// 메인 카메라 일 경우
				if(oCameras[i].name.Equals(KCDefine.U_OBJ_N_SCENE_MAIN_CAMERA)) {
					oCameras[i].ExSetTag(KCDefine.U_TAG_MAIN_CAMERA);
				} else {
					oCameras[i].ExSetTag((oCameras[i].CompareTag(KCDefine.U_TAG_UNTAGGED) || oCameras[i].CompareTag(KCDefine.U_TAG_MAIN_CAMERA)) ? KCDefine.U_TAG_ADDITIONAL_CAMERA : oCameras[i].tag);
				}
			}
		}

		for(int i = 0; i < oSceneManagers.Length; ++i) {
			oSceneManagers[i].ExSetTag(KCDefine.U_TAG_SCENE_MANAGER);
		}
	}

	/** 콜백을 설정한다 */
	private static void SetupCallbacks() {
		EditorApplication.update -= CCommonEditorSceneManager.Update;
		EditorApplication.update += CCommonEditorSceneManager.Update;

		EditorApplication.hierarchyWindowItemOnGUI -= CCommonEditorSceneManager.UpdateHierarchyUIState;
		EditorApplication.hierarchyWindowItemOnGUI += CCommonEditorSceneManager.UpdateHierarchyUIState;

		EditorApplication.playModeStateChanged -= CCommonEditorSceneManager.OnUpdatePlayModeState;
		EditorApplication.playModeStateChanged += CCommonEditorSceneManager.OnUpdatePlayModeState;

		EditorApplication.projectChanged -= CCommonEditorSceneManager.OnUpdateProjectState;
		EditorApplication.projectChanged += CCommonEditorSceneManager.OnUpdateProjectState;
	}
	
	/** 광선 추적자를 설정한다 */
	private static void SetupRaycasters() {
		CFunc.EnumerateComponents<GraphicRaycaster>((a_oRaycaster) => {
			a_oRaycaster.ignoreReversedGraphics = true;
			a_oRaycaster.blockingMask = a_oRaycaster.blockingMask.ExGetLayerVal(KCDefine.B_VAL_0_INT);
			a_oRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;

			return true; 
		});

		CFunc.EnumerateComponents<PhysicsRaycaster>((a_oRaycaster) => {
			a_oRaycaster.eventMask = a_oRaycaster.eventMask.ExGetLayerVal(int.MaxValue);
			a_oRaycaster.maxRayIntersections = KCDefine.B_VAL_0_INT;
			
			return true;
		});
	}

	/** 광원 옵션을 설정한다 */
	private static void SetupLightOpts() {
		var oLightingSettingsPathDict = new Dictionary<EQualityLevel, string>() {
			[EQualityLevel.NORM] = KCDefine.U_ASSET_P_G_NORM_QUALITY_LIGHTING_SETTINGS, [EQualityLevel.HIGH] = KCDefine.U_ASSET_P_G_HIGH_QUALITY_LIGHTING_SETTINGS, [EQualityLevel.ULTRA] = KCDefine.U_ASSET_P_G_ULTRA_QUALITY_LIGHTING_SETTINGS
		};

		var stScene = EditorSceneManager.GetActiveScene();

		foreach(var stKeyVal in oLightingSettingsPathDict) {
			CCommonEditorSceneManager.DoSetupLightOpts(stKeyVal.Key, Resources.Load<LightingSettings>(stKeyVal.Value), false);
		}

		// 광원 설정이 가능 할 경우
		if(stScene.IsValid() && !CCommonEditorSceneManager.m_oSampleSceneNameList.Contains(stScene.name) && CPlatformOptsSetter.OptsInfoTable != null) {
			bool bIsValid01 = Lightmapping.TryGetLightingSettings(out LightingSettings oLightingSettings);
			bool bIsValid02 = oLightingSettings != null && oLightingSettings.name.Equals(KCEditorDefine.B_ASSET_N_LIGHTING_SETTINGS_TEMPLATE);
			bool bIsValid03 = oLightingSettings != null && !oLightingSettings.name.Equals(Path.GetFileNameWithoutExtension(oLightingSettingsPathDict[CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_eQualityLevel]));

			var oResult = oLightingSettingsPathDict.ExFindVal((a_oLightingSettingsPath) => oLightingSettings != null && Path.GetFileNameWithoutExtension(a_oLightingSettingsPath).Equals(oLightingSettings.name));

			// 광원 설정이 없을 경우
			if((!bIsValid01 || bIsValid02 || (bIsValid03 && oResult.Item1)) && CAccess.IsExistsRes<LightingSettings>(oLightingSettingsPathDict[CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_eQualityLevel], true)) {
				EditorSceneManager.MarkSceneDirty(stScene);
				Lightmapping.SetLightingSettingsForScene(stScene, Resources.Load<LightingSettings>(oLightingSettingsPathDict[CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_eQualityLevel]));
			}
		}
	}

	/** 지역화 정보를 설정한다 */
	private static void SetupLocalizeInfos() {
		// 지역화 정보 테이블이 존재 할 경우
		if(CPlatformOptsSetter.LocalizeInfoTable != null) {
			for(int i = 0; i < CPlatformOptsSetter.LocalizeInfoTable.LocalizeInfoList.Count; ++i) {
				for(int j = 0; j < CPlatformOptsSetter.LocalizeInfoTable.LocalizeInfoList[i].m_oFontSetInfoList.Count; ++j) {
					var stFontSetInfo = CPlatformOptsSetter.LocalizeInfoTable.LocalizeInfoList[i].m_oFontSetInfoList[j];
					stFontSetInfo.m_eSet = EFontSet._1 + j;

					CPlatformOptsSetter.LocalizeInfoTable.LocalizeInfoList[i].m_oFontSetInfoList[j] = stFontSetInfo;
				}
			}
		}
	}

	/** 미리 로드 할 에셋을 설정한다 */
	private static void SetupPreloadAssets() {
		var oPreloadAssetList = PlayerSettings.GetPreloadedAssets().ToList();

		try {
			var oPreloadAssetInfoListContainer = new List<List<(string, string)>>() {
				KCEditorDefine.B_PREFAB_P_INFO_LIST, KCEditorDefine.B_ASSET_P_INFO_LIST
			};

			for(int i = 0; i < SceneManager.sceneCountInBuildSettings; ++i) {
				string oScenePath = SceneUtility.GetScenePathByBuildIndex(i);

				// 씬 추가가 가능 할 경우
				if(!oScenePath.Contains(KCDefine.B_EDITOR_SCENE_N_PATTERN_01) && !oScenePath.Contains(KCDefine.B_EDITOR_SCENE_N_PATTERN_02)) {
					var oAsset = CEditorFunc.FindAsset<SceneAsset>(oScenePath);
					oPreloadAssetList.ExAddVal(oAsset, (a_oAsset) => (a_oAsset != null && oAsset != null) && oScenePath.Contains(a_oAsset.name));
				}
			}

			for(int i = 0; i < oPreloadAssetInfoListContainer.Count; ++i) {
				for(int j = 0; j < oPreloadAssetInfoListContainer[i].Count; ++j) {
					var oAsset = CEditorFunc.FindAsset<Object>(oPreloadAssetInfoListContainer[i][j].Item2);
					oPreloadAssetList.ExAddVal(oAsset, (a_oAsset) => (a_oAsset != null && oAsset != null) && Path.GetFileNameWithoutExtension(oPreloadAssetInfoListContainer[i][j].Item2).Equals(a_oAsset.name));
				}
			}

			oPreloadAssetList.RemoveAll((a_oAsset) => a_oAsset == null);
			oPreloadAssetList.Sort((a_oLhs, a_oRhs) => a_oLhs.name.CompareTo(a_oRhs.name));
		} finally {
			PlayerSettings.SetPreloadedAssets(oPreloadAssetList.ToArray());
		}
	}

	/** 스프라이트 아틀라스를 설정한다 */
	private static void SetupSpriteAtlases() {
		for(int i = 0; i < KCEditorDefine.B_SEARCH_P_SPRITE_ATLAS_LIST.Count; ++i) {
			string oDirPath = Path.GetDirectoryName(KCEditorDefine.B_SEARCH_P_SPRITE_ATLAS_LIST[i]);
			CCommonEditorSceneManager.DoSetupSpriteAtlases(AssetDatabase.GetSubFolders(oDirPath).ToList());
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

	/** 레이어를 설정한다 */
	private static void SetupLayers(GameObject a_oObj) {
		foreach(var oObj in a_oObj.DescendantsAndSelf()) {
			// 레이어 설정이 필요 할 경우
			if(oObj.layer != KCDefine.U_LAYER_UIS) {
				oObj.ExSetLayer(KCDefine.U_LAYER_UIS, false);
			}
		}
	}

	/** 정적 객체를 설정한다 */
	private static void SetupStaticObjs(GameObject a_oObj) {
		foreach(var oObj in a_oObj.ChildrenAndSelf()) {
			// 정적 객체 일 경우
			if(KCEditorDefine.B_OBJ_N_STATIC_OBJ_LIST.Contains(oObj.name)) {
				oObj.ExSetStaticEditorFlags((StaticEditorFlags)int.MaxValue);
			}
		}
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

				a_oSettings.lightmapper == CPlatformOptsSetter.OptsInfoTable.BuildOptsInfo.m_eLightmapper,
				a_oSettings.filteringMode == LightingSettings.FilterMode.Auto,
				a_oSettings.mixedBakeMode == CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_eMixedLightingMode,
				a_oSettings.directionalityMode == (LightmapsMode)stRenderingOptsInfo.m_stLightOptsInfo.m_eLightmapMode,

				a_oSettings.lightmapPadding == KCDefine.B_VAL_4_INT,
				a_oSettings.lightmapMaxSize == (int)stRenderingOptsInfo.m_stLightOptsInfo.m_eLightmapMaxSize,
				a_oSettings.finalGatherRayCount == (int)EPOT._256,
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

				a_oSettings.lightmapper = CPlatformOptsSetter.OptsInfoTable.BuildOptsInfo.m_eLightmapper;
				a_oSettings.filteringMode = LightingSettings.FilterMode.Auto;
				a_oSettings.mixedBakeMode = CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_eMixedLightingMode;
				a_oSettings.directionalityMode = (LightmapsMode)stRenderingOptsInfo.m_stLightOptsInfo.m_eLightmapMode;

				a_oSettings.lightmapPadding = KCDefine.B_VAL_4_INT;
				a_oSettings.lightmapMaxSize = (int)stRenderingOptsInfo.m_stLightOptsInfo.m_eLightmapMaxSize;
				a_oSettings.finalGatherRayCount = (int)EPOT._256;
				a_oSettings.lightmapCompression = stRenderingOptsInfo.m_stLightOptsInfo.m_eLightmapCompression;

				a_oSettings.albedoBoost = KCDefine.B_VAL_1_INT;
				a_oSettings.indirectScale = KCDefine.B_VAL_1_INT;
				a_oSettings.indirectResolution = KCDefine.B_UNIT_LIGHTMAP_RESOLUTION;
				a_oSettings.lightmapResolution = KCDefine.B_UNIT_LIGHTMAP_RESOLUTION;
			}
		}
	}

	/** 스프라이트 아틀라스를 설정한다 */
	private static void DoSetupSpriteAtlases(List<string> a_oDirPathList) {
		for(int i = 0; i < a_oDirPathList.Count; ++i) {
			int nIdx = KCDefine.U_ASSET_P_SPRITE_ATLAS_LIST.FindIndex((a_oSpriteAtlasPath) => a_oSpriteAtlasPath.Contains(Path.GetFileNameWithoutExtension(a_oDirPathList[i])));

			// 스프라이트 아틀라스 경로가 존재 할 경우
			if(KCDefine.U_ASSET_P_SPRITE_ATLAS_LIST.ExIsValidIdx(nIdx)) {
				string oSpriteAtlasPath01 = string.Format(KCDefine.B_TEXT_FMT_2_COMBINE, KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES, KCDefine.U_ASSET_P_SPRITE_ATLAS_LIST[nIdx]);
				string oSpriteAtlasPath02 = string.Format(KCDefine.B_TEXT_FMT_2_COMBINE, KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR_RESOURCES, KCDefine.U_ASSET_P_SPRITE_ATLAS_LIST[nIdx]);

				CCommonEditorSceneManager.DoSetupSpriteAtlas(CEditorFunc.FindAsset<SpriteAtlas>(string.Format(KCDefine.B_TEXT_FMT_2_COMBINE, oSpriteAtlasPath01, KCDefine.B_FILE_EXTENSION_SPRITE_ATLAS)), a_oDirPathList[i]);
				CCommonEditorSceneManager.DoSetupSpriteAtlas(CEditorFunc.FindAsset<SpriteAtlas>(string.Format(KCDefine.B_TEXT_FMT_2_COMBINE, oSpriteAtlasPath02, KCDefine.B_FILE_EXTENSION_SPRITE_ATLAS)), a_oDirPathList[i]);
			}
		}
	}

	/** 스프라이트 아틀라스를 설정한다 */
	private static void DoSetupSpriteAtlas(SpriteAtlas a_oSpriteAtlas, string a_oDirPath) {
		// 스프라이트 아틀라스가 존재 할 경우
		if(a_oSpriteAtlas != null) {
			var oDirAsset = CEditorFunc.FindAsset<Object>(a_oDirPath);
			var oPackables = a_oSpriteAtlas.GetPackables();

			// 디렉토리가 없을 경우
			if(!oPackables.ExFindVal((a_oPackable) => a_oPackable != null && a_oPackable.name.Equals(oDirAsset.name)).ExIsValidIdx()) {
				a_oSpriteAtlas.Add(new Object[] { oDirAsset });
			}
		}
	}

	/** 씬 템플릿을 설정한다 */
	private static void DoSetupSceneTemplates(SceneTemplateAsset a_oSceneTemplate) {
		for(int i = 0; i < a_oSceneTemplate.dependencies.Length; ++i) {
			a_oSceneTemplate.dependencies[i].instantiationMode = TemplateInstantiationMode.Reference;
		}
	}

	/** 분실 된 스크립트 상태를 갱신한다 */
	private static void UpdateMissingScriptState(GameObject a_oObj) {
		CCommonEditorSceneManager.m_oPrefabMissingObjList.Clear();

		try {
			foreach(var oObj in a_oObj.DescendantsAndSelf()) {
				// 객체 제거가 필요 할 경우
				if(PrefabUtility.IsPrefabAssetMissing(oObj)) {
					EditorSceneManager.MarkSceneDirty(a_oObj.scene);
					CCommonEditorSceneManager.m_oPrefabMissingObjList.ExAddVal(PrefabUtility.GetOutermostPrefabInstanceRoot(oObj));
				}

				// 스크립트 제거가 필요 할 경우
				if(GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(oObj) > KCDefine.B_VAL_0_INT) {
					EditorSceneManager.MarkSceneDirty(a_oObj.scene);
					GameObjectUtility.RemoveMonoBehavioursWithMissingScript(oObj);
				}
			}
		} finally {
			for(int i = 0; i < CCommonEditorSceneManager.m_oPrefabMissingObjList.Count; ++i) {
				string oMsg = string.Format(KCEditorDefine.B_MSG_FMT_ALERT_P_MISSING_PREFAB, CCommonEditorSceneManager.m_oPrefabMissingObjList[i].name);

				// 확인 버튼을 눌렀을 경우
				if(CEditorFunc.ShowOKCancelAlertPopup(KCEditorDefine.B_TEXT_ALERT_P_TITLE, oMsg)) {
					CFactory.RemoveObj(CCommonEditorSceneManager.m_oPrefabMissingObjList[i]);
				} else {
					PrefabUtility.UnpackPrefabInstance(CCommonEditorSceneManager.m_oPrefabMissingObjList[i], PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
				}
			}
		}
	}
	#endregion			// 클래스 함수

	#region 클래스 조건부 함수
#if INPUT_SYSTEM_MODULE_ENABLE
	/** 입력 시스템을 설정한다 */
	private static void SetupInputSystem() {
		// 입력 시스템 설정이 없을 경우
		if(!EditorBuildSettings.TryGetConfigObject<InputSettings>(KCEditorDefine.B_MODULE_N_INPUT_SYSTEM_SETTINGS, out InputSettings oInputSettings)) {
			oInputSettings = AssetDatabase.LoadAssetAtPath<InputSettings>(KCEditorDefine.B_ASSET_P_INPUT_SETTINGS);
			oInputSettings = oInputSettings ?? CEditorFactory.CreateScriptableObj<InputSettings>(KCEditorDefine.B_ASSET_P_INPUT_SETTINGS);

			InputSystem.settings = oInputSettings;
			EditorBuildSettings.AddConfigObject(KCEditorDefine.B_MODULE_N_INPUT_SYSTEM_SETTINGS, oInputSettings, true);
		}

		var oIsSetupOptsList = new List<bool>() {
			oInputSettings.compensateForScreenOrientation,

			oInputSettings.updateMode == InputSettings.UpdateMode.ProcessEventsInDynamicUpdate,
			oInputSettings.editorInputBehaviorInPlayMode == InputSettings.EditorInputBehaviorInPlayMode.PointersAndKeyboardsRespectGameViewFocus,

#if UNITY_IOS
			oInputSettings.iOS.motionUsage.enabled == CPlatformOptsSetter.OptsInfoTable.BuildOptsInfo.m_stiOSBuildOptsInfo.m_bIsEnableInputSystemMotion,
			oInputSettings.iOS.motionUsage.usageDescription.Equals((CPlatformOptsSetter.OptsInfoTable != null) ? CPlatformOptsSetter.OptsInfoTable.BuildOptsInfo.m_stiOSBuildOptsInfo.m_oInputSystemMotionDesc : string.Empty)
#endif			// #if UNITY_IOS
		};
		
		// 설정 갱신이 필요 할 경우
		if(oIsSetupOptsList.Contains(false)) {
			oInputSettings.compensateForScreenOrientation = true;
			
			oInputSettings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
			oInputSettings.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.PointersAndKeyboardsRespectGameViewFocus;

#if UNITY_IOS
			oInputSettings.iOS.motionUsage.enabled = CPlatformOptsSetter.OptsInfoTable.BuildOptsInfo.m_stiOSBuildOptsInfo.m_bIsEnableInputSystemMotion;
			oInputSettings.iOS.motionUsage.usageDescription = (CPlatformOptsSetter.OptsInfoTable != null) ? CPlatformOptsSetter.OptsInfoTable.BuildOptsInfo.m_stiOSBuildOptsInfo.m_oInputSystemMotionDesc : string.Empty;
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

#if BURST_COMPILER_MODULE_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE
	/** 버스트 컴파일러를 설정한다 */
	private static void SetupBurstCompiler() {
		var oSettingsPathList = new List<string>() {
			KCEditorDefine.B_DATA_P_IOS_BURST_AOT_SETTINGS, KCEditorDefine.B_DATA_P_ANDROID_BURST_AOT_SETTINGS, KCEditorDefine.B_DATA_P_MAC_BURST_AOT_SETTINGS, KCEditorDefine.B_DATA_P_WNDS_BURST_AOT_SETTINGS
		};

		for(int i = 0; i < oSettingsPathList.Count; ++i) {
			// 설정 파일이 존재 할 경우
			if(File.Exists(oSettingsPathList[i])) {
				// 모바일 일 경우
				if(oSettingsPathList[i].Equals(KCEditorDefine.B_DATA_P_IOS_BURST_AOT_SETTINGS) || oSettingsPathList[i].Equals(KCEditorDefine.B_DATA_P_ANDROID_BURST_AOT_SETTINGS)) {
					var stAOTSettingsWrapper = JsonConvert.DeserializeObject<STMobileBurstAOTSettingsWrapper>(CFunc.ReadStr(oSettingsPathList[i]));

					var oIsSetupOptsList = new List<bool>() {
						stAOTSettingsWrapper.MonoBehaviour.EnableOptimisations,
						stAOTSettingsWrapper.MonoBehaviour.EnableBurstCompilation,

						stAOTSettingsWrapper.MonoBehaviour.EnableSafetyChecks == false,
						stAOTSettingsWrapper.MonoBehaviour.EnableDebugInAllBuilds == false,

						stAOTSettingsWrapper.MonoBehaviour.OptimizeFor == (int)EBurstCompilerOptimization.PERFORMANCE
					};

					// 설정 갱신이 필요 할 경우
					if(oIsSetupOptsList.Contains(false)) {
						stAOTSettingsWrapper.MonoBehaviour.EnableOptimisations = true;
						stAOTSettingsWrapper.MonoBehaviour.EnableBurstCompilation = true;

						stAOTSettingsWrapper.MonoBehaviour.EnableSafetyChecks = false;
						stAOTSettingsWrapper.MonoBehaviour.EnableDebugInAllBuilds = false;

						stAOTSettingsWrapper.MonoBehaviour.OptimizeFor = (int)EBurstCompilerOptimization.PERFORMANCE;
						CFunc.WriteStr(oSettingsPathList[i], JsonConvert.SerializeObject(stAOTSettingsWrapper, Formatting.Indented));
					}
				} else {
					var stAOTSettingsWrapper = JsonConvert.DeserializeObject<STStandaloneBurstAOTSettingsWrapper>(CFunc.ReadStr(oSettingsPathList[i]));

					var oIsSetupOptsList = new List<bool>() {
						stAOTSettingsWrapper.MonoBehaviour.EnableOptimisations,
						stAOTSettingsWrapper.MonoBehaviour.EnableBurstCompilation,

						stAOTSettingsWrapper.MonoBehaviour.EnableSafetyChecks == false,
						stAOTSettingsWrapper.MonoBehaviour.UsePlatformSDKLinker == false,
						stAOTSettingsWrapper.MonoBehaviour.EnableDebugInAllBuilds == false,

						stAOTSettingsWrapper.MonoBehaviour.OptimizeFor == (int)EBurstCompilerOptimization.PERFORMANCE
					};

					// 설정 갱신이 필요 할 경우
					if(oIsSetupOptsList.Contains(false)) {
						stAOTSettingsWrapper.MonoBehaviour.EnableOptimisations = true;
						stAOTSettingsWrapper.MonoBehaviour.EnableBurstCompilation = true;

						stAOTSettingsWrapper.MonoBehaviour.EnableSafetyChecks = false;
						stAOTSettingsWrapper.MonoBehaviour.UsePlatformSDKLinker = false;
						stAOTSettingsWrapper.MonoBehaviour.EnableDebugInAllBuilds = false;

						stAOTSettingsWrapper.MonoBehaviour.OptimizeFor = (int)EBurstCompilerOptimization.PERFORMANCE;
						CFunc.WriteStr(oSettingsPathList[i], JsonConvert.SerializeObject(stAOTSettingsWrapper, Formatting.Indented));
					}
				}
			}
		}
	}
#endif			// #if BURST_COMPILER_MODULE_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE
	#endregion			// 클래스 조건부 함수
}
#endif			// #if UNITY_EDITOR
