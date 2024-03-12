using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using System.IO;
using System.Linq;
using UnityEngine.U2D;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.U2D;
using UnityEditor.SceneTemplate;
using UnityEditor.SceneManagement;
using Unity.Linq;

#if ADAPTIVE_PERFORMANCE_ENABLE
using UnityEngine.AdaptivePerformance;
using UnityEditor.AdaptivePerformance.Editor;
#endif // #if ADAPTIVE_PERFORMANCE_ENABLE

#if INPUT_SYSTEM_MODULE_ENABLE
using UnityEngine.InputSystem;

#if UNITY_IOS
using UnityEngine.InputSystem.iOS;
#endif // #if UNITY_IOS
#endif // #if INPUT_SYSTEM_MODULE_ENABLE

#if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#endif // #if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE

/** 공용 에디터 씬 관리자 - 설정 */
public static partial class CCommonEditorSceneManager
{
	#region 클래스 함수
	/** 태그를 설정한다 */
	private static void SetupTags()
	{
		var oLights = Resources.FindObjectsOfTypeAll<Light>();
		var oCameras = Resources.FindObjectsOfTypeAll<Camera>();
		var oSceneManagers = Resources.FindObjectsOfTypeAll<CSceneManager>();

		for(int i = 0; i < oLights.Length; ++i)
		{
			// 에디터 광원이 아닐 경우
			if(!KCEditorDefine.B_OBJ_N_SCENE_EDITOR_LIGHT_LIST.Contains(oLights[i].name))
			{
				oLights[i].type = oLights[i].name.Equals(KCDefine.U_OBJ_N_SCENE_MAIN_LIGHT) ? LightType.Directional : oLights[i].type;

				// 메인 광원 일 경우
				if(oLights[i].name.Equals(KCDefine.U_OBJ_N_SCENE_MAIN_LIGHT))
				{
					oLights[i].ExSetTag(KCDefine.B_TAG_MAIN_LIGHT);
				}
				else
				{
					oLights[i].ExSetTag((oLights[i].CompareTag(KCDefine.B_TAG_UNTAGGED) || oLights[i].CompareTag(KCDefine.B_TAG_MAIN_LIGHT)) ? KCDefine.B_TAG_ADDITIONAL_LIGHT : oLights[i].tag);
				}
			}
		}

		for(int i = 0; i < oCameras.Length; ++i)
		{
			// 에디터 카메라가 아닐 경우
			if(!KCEditorDefine.B_OBJ_N_SCENE_EDITOR_CAMERA_LIST.Contains(oCameras[i].name))
			{
				// 메인 카메라 일 경우
				if(oCameras[i].name.Equals(KCDefine.U_OBJ_N_SCENE_MAIN_CAMERA))
				{
					oCameras[i].ExSetTag(KCDefine.B_TAG_MAIN_CAMERA);
				}
				else
				{
					oCameras[i].ExSetTag((oCameras[i].CompareTag(KCDefine.B_TAG_UNTAGGED) || oCameras[i].CompareTag(KCDefine.B_TAG_MAIN_CAMERA)) ? KCDefine.B_TAG_ADDITIONAL_CAMERA : oCameras[i].tag);
				}
			}
		}

		for(int i = 0; i < oSceneManagers.Length; ++i)
		{
			oSceneManagers[i].ExSetTag(KCDefine.B_TAG_SCENE_MANAGER);
		}
	}

	/** 콜백을 설정한다 */
	private static void SetupCallbacks()
	{
		EditorApplication.update -= CCommonEditorSceneManager.Update;
		EditorApplication.update += CCommonEditorSceneManager.Update;

		EditorApplication.hierarchyWindowItemOnGUI -= CCommonEditorSceneManager.UpdateHierarchyUIState;
		EditorApplication.hierarchyWindowItemOnGUI += CCommonEditorSceneManager.UpdateHierarchyUIState;

		EditorApplication.playModeStateChanged -= CCommonEditorSceneManager.OnUpdatePlayModeState;
		EditorApplication.playModeStateChanged += CCommonEditorSceneManager.OnUpdatePlayModeState;

		EditorApplication.projectChanged -= CCommonEditorSceneManager.OnUpdateProjectState;
		EditorApplication.projectChanged += CCommonEditorSceneManager.OnUpdateProjectState;

		EditorSceneManager.sceneOpened -= CCommonEditorSceneManager.OnOpenScene;
		EditorSceneManager.sceneOpened += CCommonEditorSceneManager.OnOpenScene;
	}

	/** 광원 옵션을 설정한다 */
	private static void SetupLightOpts()
	{
		var oLightingSettingsPathDict = new Dictionary<EQualityLevel, string>()
		{
			[EQualityLevel.NORM] = KCDefine.U_ASSET_P_G_NORM_QUALITY_LIGHTING_SETTINGS,
			[EQualityLevel.HIGH] = KCDefine.U_ASSET_P_G_HIGH_QUALITY_LIGHTING_SETTINGS,
			[EQualityLevel.ULTRA] = KCDefine.U_ASSET_P_G_ULTRA_QUALITY_LIGHTING_SETTINGS
		};

		var stScene = EditorSceneManager.GetActiveScene();

		foreach(var stKeyVal in oLightingSettingsPathDict)
		{
			CCommonEditorSceneManager.DoSetupLightOpts(stKeyVal.Key, Resources.Load<LightingSettings>(stKeyVal.Value), false);
		}

		// 광원 설정이 가능 할 경우
		if(stScene.IsValid() && !CCommonEditorSceneManager.m_oSampleSceneNameList.Contains(stScene.name) && CPlatformOptsSetter.OptsInfoTable != null)
		{
			bool bIsValid01 = Lightmapping.TryGetLightingSettings(out LightingSettings oLightingSettings);
			bool bIsValid02 = oLightingSettings != null && oLightingSettings.name.Equals(KCEditorDefine.B_ASSET_N_LIGHTING_SETTINGS_TEMPLATE);
			bool bIsValid03 = oLightingSettings != null && !oLightingSettings.name.Equals(oLightingSettingsPathDict.ExGetVal(CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_eQualityLevel, string.Empty).ExGetFileName(false));

			var stResult = oLightingSettingsPathDict.ExFindVal((a_stKeyVal) =>
			{
				return oLightingSettings != null && a_stKeyVal.Value.ExGetFileName(false).Equals(oLightingSettings.name);
			});

			// 광원 설정이 없을 경우
			if((!bIsValid01 || bIsValid02 || (bIsValid03 && stResult.Item1)) && CAccess.IsExistsRes<LightingSettings>(oLightingSettingsPathDict.ExGetVal(CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_eQualityLevel, string.Empty), true))
			{
				EditorSceneManager.MarkSceneDirty(stScene);
				Lightmapping.SetLightingSettingsForScene(stScene, Resources.Load<LightingSettings>(oLightingSettingsPathDict.ExGetVal(CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_eQualityLevel, string.Empty)));
			}
		}
	}

	/** 광선 추적자를 설정한다 */
	private static void SetupRaycasters()
	{
		CAccess.EnumerateComponents<GraphicRaycaster>((a_oRaycaster) =>
		{
			a_oRaycaster.ignoreReversedGraphics = true;
			a_oRaycaster.blockingMask = a_oRaycaster.blockingMask.ExGetLayerMask(KCDefine.B_VAL_0_INT);
			a_oRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;

			return true;
		});

		CAccess.EnumerateComponents<PhysicsRaycaster>((a_oRaycaster) =>
		{
			a_oRaycaster.eventMask = a_oRaycaster.eventMask.ExGetLayerMask(int.MaxValue);
			a_oRaycaster.maxRayIntersections = KCDefine.B_VAL_0_INT;

			return true;
		});
	}

	/** 미리 로드 할 에셋을 설정한다 */
	private static void SetupPreloadAssets()
	{
		var oPreloadAssetList = PlayerSettings.GetPreloadedAssets().ToList();

		var oPreloadAssetInfoListContainer = new List<List<(string, string)>>() {
			KCEditorDefine.B_PREFAB_P_INFO_LIST, KCEditorDefine.B_ASSET_P_INFO_LIST
		};

		for(int i = 0; i < SceneManager.sceneCountInBuildSettings; ++i)
		{
			string oScenePath = SceneUtility.GetScenePathByBuildIndex(i);

			// 씬 추가가 가능 할 경우
			if(!oScenePath.Contains(KCEditorDefine.B_EDITOR_SCENE_N_PATTERN_A) && !oScenePath.Contains(KCEditorDefine.B_EDITOR_SCENE_N_PATTERN_B))
			{
				var oAsset = CEditorAccess.FindAsset<SceneAsset>(oScenePath);
				oPreloadAssetList.ExAddVal(oAsset, (a_oAsset) => (a_oAsset != null && oAsset != null) && oScenePath.Contains(a_oAsset.name));
			}
		}

		for(int i = 0; i < oPreloadAssetInfoListContainer.Count; ++i)
		{
			for(int j = 0; j < oPreloadAssetInfoListContainer[i].Count; ++j)
			{
				var oAsset = CEditorAccess.FindAsset<Object>(oPreloadAssetInfoListContainer[i][j].Item2);
				oPreloadAssetList.ExAddVal(oAsset, (a_oAsset) => (a_oAsset != null && oAsset != null) && oPreloadAssetInfoListContainer[i][j].Item2.ExGetFileName(false).Equals(a_oAsset.name));
			}
		}

		oPreloadAssetList.RemoveAll((a_oAsset) => a_oAsset == null);
		oPreloadAssetList.ExStableSort((a_oLhs, a_oRhs) => a_oLhs.name.CompareTo(a_oRhs.name));

		PlayerSettings.SetPreloadedAssets(oPreloadAssetList.ToArray());
	}

	/** 스프라이트 아틀라스를 설정한다 */
	private static void SetupSpriteAtlases()
	{
		for(int i = 0; i < KCEditorDefine.B_SEARCH_P_SPRITE_ATLAS_LIST.Count; ++i)
		{
			string oDirPath = Path.GetDirectoryName(KCEditorDefine.B_SEARCH_P_SPRITE_ATLAS_LIST[i]).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH);
			CCommonEditorSceneManager.DoSetupSpriteAtlases(AssetDatabase.GetSubFolders(oDirPath).ToList());
		}
	}

	/** 씬 템플릿을 설정한다 */
	private static void SetupSceneTemplates()
	{
		// 샘플 씬 템플릿이 존재 할 경우
		if(CEditorAccess.IsExistsAsset(KCEditorDefine.B_ASSET_P_SAMPLE_SCENE_TEMPLATE))
		{
			CCommonEditorSceneManager.DoSetupSceneTemplates(CEditorAccess.FindAsset<SceneTemplateAsset>(KCEditorDefine.B_ASSET_P_SAMPLE_SCENE_TEMPLATE));
		}

		// 에디터 샘플 씬 템플릿이 존재 할 경우
		if(CEditorAccess.IsExistsAsset(KCEditorDefine.B_ASSET_P_EDITOR_SAMPLE_SCENE_TEMPLATE))
		{
			CCommonEditorSceneManager.DoSetupSceneTemplates(CEditorAccess.FindAsset<SceneTemplateAsset>(KCEditorDefine.B_ASSET_P_EDITOR_SAMPLE_SCENE_TEMPLATE));
		}

#if RESEARCH_MODULE_ENABLE
		// 메뉴 샘플 씬 템플릿이 존재 할 경우
		if(CEditorAccess.IsExistsAsset(KCEditorDefine.B_ASSET_P_MENU_SAMPLE_SCENE_TEMPLATE))
		{
			CCommonEditorSceneManager.DoSetupSceneTemplates(CEditorAccess.FindAsset<SceneTemplateAsset>(KCEditorDefine.B_ASSET_P_MENU_SAMPLE_SCENE_TEMPLATE));
		}

		// 스터디 샘플 씬 템플릿이 존재 할 경우
		if(CEditorAccess.IsExistsAsset(KCEditorDefine.B_ASSET_P_STUDY_SAMPLE_SCENE_TEMPLATE))
		{
			CCommonEditorSceneManager.DoSetupSceneTemplates(CEditorAccess.FindAsset<SceneTemplateAsset>(KCEditorDefine.B_ASSET_P_STUDY_SAMPLE_SCENE_TEMPLATE));
		}
#endif // #if RESEARCH_MODULE_ENABLE
	}

	/** 레이어를 설정한다 */
	private static void SetupLayers(GameObject a_oObj)
	{
		foreach(var oObj in a_oObj.DescendantsAndSelf())
		{
			// 레이어 설정이 가능 할 경우
			if(oObj.layer != KCDefine.U_LAYER_UIS && !oObj.name.Contains(KCDefine.B_NAME_PATTERN_FILTER_SETUP_LAYER))
			{
				oObj.ExSetLayer(KCDefine.U_LAYER_UIS, false, false);
			}
		}
	}

	/** 정적 객체를 설정한다 */
	private static void SetupStaticObjs(GameObject a_oObj)
	{
		foreach(var oObj in a_oObj.DescendantsAndSelf())
		{
			bool bIsValid = KCEditorDefine.B_OBJ_N_STATIC_OBJ_LIST.Contains(oObj.name) && GameObjectUtility.GetStaticEditorFlags(oObj) != (StaticEditorFlags)int.MaxValue;

			// 정적 플래그 설정이 가능 할 경우
			if(bIsValid && !oObj.name.Contains(KCDefine.B_NAME_PATTERN_FILTER_SETUP_STATIC_FLAGS))
			{
				oObj.ExSetStaticEditorFlags((StaticEditorFlags)int.MaxValue, false, false);
			}
		}
	}

	/** 광원 옵션을 설정한다 */
	private static void DoSetupLightOpts(EQualityLevel a_eQualityLevel, LightingSettings a_oSettings, bool a_bIsAssert = true)
	{
		CFunc.Assert(!a_bIsAssert || a_oSettings != null);

		// 광원 설정이 존재 할 경우
		if(a_oSettings != null && CPlatformOptsSetter.OptsInfoTable != null)
		{
			var stRenderingOptsInfo = CPlatformOptsSetter.OptsInfoTable.GetRenderingOptsInfo(a_eQualityLevel);

			var oIsSetupOptsList = new List<bool>() {
				a_oSettings.ao,
				a_oSettings.bakedGI,

				a_oSettings.realtimeGI == CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_bIsEnableRealtimeGI,
				a_oSettings.realtimeEnvironmentLighting == CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_bIsEnableRealtimeEnvironmentLighting,

				a_oSettings.lightmapper == (CEditorAccess.IsAppleMSeries ? LightingSettings.Lightmapper.ProgressiveGPU : LightingSettings.Lightmapper.ProgressiveCPU),
				a_oSettings.filteringMode == LightingSettings.FilterMode.Auto,
				a_oSettings.mixedBakeMode == CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_eMixedLightingMode,
				a_oSettings.directionalityMode == (LightmapsMode)stRenderingOptsInfo.m_stLightOptsInfo.m_eLightmapMode,

				a_oSettings.lightmapPadding == KCDefine.B_VAL_4_INT,
				a_oSettings.lightmapMaxSize == (int)stRenderingOptsInfo.m_stLightOptsInfo.m_eLightmapMaxSize,
				a_oSettings.lightmapCompression == stRenderingOptsInfo.m_stLightOptsInfo.m_eLightmapCompression,

				a_oSettings.albedoBoost.Equals(KCDefine.B_VAL_1_REAL),
				a_oSettings.indirectScale.Equals(KCDefine.B_VAL_1_REAL),
				a_oSettings.indirectResolution.Equals(KCDefine.B_UNIT_LIGHTMAP_RESOLUTION),
				a_oSettings.lightmapResolution.Equals(KCDefine.B_UNIT_LIGHTMAP_RESOLUTION)
			};

			// 설정 갱신이 필요 할 경우
			if(oIsSetupOptsList.Contains(false))
			{
				a_oSettings.ao = true;
				a_oSettings.bakedGI = true;

				a_oSettings.realtimeGI = CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_bIsEnableRealtimeGI;
				a_oSettings.realtimeEnvironmentLighting = CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_bIsEnableRealtimeEnvironmentLighting;

				a_oSettings.lightmapper = CEditorAccess.IsAppleMSeries ? LightingSettings.Lightmapper.ProgressiveGPU : LightingSettings.Lightmapper.ProgressiveCPU;
				a_oSettings.filteringMode = LightingSettings.FilterMode.Auto;
				a_oSettings.mixedBakeMode = CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_eMixedLightingMode;
				a_oSettings.directionalityMode = (LightmapsMode)stRenderingOptsInfo.m_stLightOptsInfo.m_eLightmapMode;

				a_oSettings.lightmapPadding = KCDefine.B_VAL_4_INT;
				a_oSettings.lightmapMaxSize = (int)stRenderingOptsInfo.m_stLightOptsInfo.m_eLightmapMaxSize;
				a_oSettings.lightmapCompression = stRenderingOptsInfo.m_stLightOptsInfo.m_eLightmapCompression;

				a_oSettings.albedoBoost = KCDefine.B_VAL_1_INT;
				a_oSettings.indirectScale = KCDefine.B_VAL_1_INT;
				a_oSettings.indirectResolution = KCDefine.B_UNIT_LIGHTMAP_RESOLUTION;
				a_oSettings.lightmapResolution = KCDefine.B_UNIT_LIGHTMAP_RESOLUTION;
			}
		}
	}

	/** 스프라이트 아틀라스를 설정한다 */
	private static void DoSetupSpriteAtlases(List<string> a_oDirPathList)
	{
		for(int i = 0; i < a_oDirPathList.Count; ++i)
		{
			int nIdx01 = KCEditorDefine.B_ASSET_P_SPRITE_ATLAS_LIST.FindIndex((a_oSpriteAtlasPath) => a_oSpriteAtlasPath.Contains(a_oDirPathList[i].ExGetFileName(false).Replace(KCDefine.B_NAME_PATTERN_FIX_REPEAT_WRAP, string.Empty)));
			int nIdx02 = KCEditorDefine.B_ASSET_P_SPRITE_ATLAS_LIST.FindIndex((a_oSpriteAtlasPath) => a_oSpriteAtlasPath.Contains(a_oDirPathList[i].ExGetFileName(false).Replace(KCDefine.B_NAME_PATTERN_FIX_POINT_FILTER, string.Empty)));
			int nIdx03 = KCEditorDefine.B_ASSET_P_SPRITE_ATLAS_LIST.FindIndex((a_oSpriteAtlasPath) => a_oSpriteAtlasPath.Contains(a_oDirPathList[i].ExGetFileName(false).Replace(KCDefine.B_NAME_PATTERN_FILTER_SETUP_TEX_COMPRESS, string.Empty)));

			// 스프라이트 아틀라스 경로가 존재 할 경우
			if(KCEditorDefine.B_ASSET_P_SPRITE_ATLAS_LIST.ExIsValidIdx(nIdx01) || KCEditorDefine.B_ASSET_P_SPRITE_ATLAS_LIST.ExIsValidIdx(nIdx02) || KCEditorDefine.B_ASSET_P_SPRITE_ATLAS_LIST.ExIsValidIdx(nIdx03))
			{
				int nIdx = KCEditorDefine.B_ASSET_P_SPRITE_ATLAS_LIST.ExIsValidIdx(nIdx01) ? nIdx01 : KCEditorDefine.B_ASSET_P_SPRITE_ATLAS_LIST.ExIsValidIdx(nIdx02) ? nIdx02 : nIdx03;
				string oSpriteAtlasPath01 = string.Format(KCDefine.B_TEXT_FMT_2_COMBINE, KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES, KCEditorDefine.B_ASSET_P_SPRITE_ATLAS_LIST[nIdx]);
				string oSpriteAtlasPath02 = string.Format(KCDefine.B_TEXT_FMT_2_COMBINE, KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR_RESOURCES, KCEditorDefine.B_ASSET_P_SPRITE_ATLAS_LIST[nIdx]);

				CCommonEditorSceneManager.DoSetupSpriteAtlas(CEditorAccess.FindAsset<SpriteAtlas>(string.Format(KCDefine.B_TEXT_FMT_2_COMBINE, oSpriteAtlasPath01, KCDefine.B_FILE_EXTENSION_SPRITE_ATLAS)), a_oDirPathList[i]);
				CCommonEditorSceneManager.DoSetupSpriteAtlas(CEditorAccess.FindAsset<SpriteAtlas>(string.Format(KCDefine.B_TEXT_FMT_2_COMBINE, oSpriteAtlasPath02, KCDefine.B_FILE_EXTENSION_SPRITE_ATLAS)), a_oDirPathList[i]);
			}
		}
	}

	/** 스프라이트 아틀라스를 설정한다 */
	private static void DoSetupSpriteAtlas(SpriteAtlas a_oSpriteAtlas, string a_oDirPath)
	{
		// 스프라이트 아틀라스가 존재 할 경우
		if(a_oSpriteAtlas != null)
		{
			var oDirAsset = CEditorAccess.FindAsset<Object>(a_oDirPath);
			var oPackables = a_oSpriteAtlas.GetPackables();

			// 디렉토리가 없을 경우
			if(!oPackables.ExFindVal((a_oPackable) => a_oPackable != null && a_oPackable.name.Equals(oDirAsset.name)).ExIsValidIdx())
			{
				a_oSpriteAtlas.Add(new Object[] { oDirAsset });
			}

			for(int i = 0; i < oPackables.Length; ++i)
			{
				// 에셋이 유효하지 않을 경우
				if(!oPackables[i].name.Contains(a_oSpriteAtlas.name))
				{
					a_oSpriteAtlas.Remove(new Object[] { oPackables[i] });
				}
			}
		}
	}

	/** 씬 템플릿을 설정한다 */
	private static void DoSetupSceneTemplates(SceneTemplateAsset a_oSceneTemplate)
	{
		for(int i = 0; i < a_oSceneTemplate.dependencies.Length; ++i)
		{
			a_oSceneTemplate.dependencies[i].instantiationMode = TemplateInstantiationMode.Reference;
		}
	}

	/** 분실 된 스크립트 상태를 갱신한다 */
	private static void UpdateMissingScriptState(GameObject a_oObj)
	{
		CCommonEditorSceneManager.m_oPrefabMissingObjsList.Clear();

		foreach(var oObj in a_oObj.DescendantsAndSelf())
		{
			// 객체 제거가 필요 할 경우
			if(PrefabUtility.IsPrefabAssetMissing(oObj))
			{
				EditorSceneManager.MarkSceneDirty(a_oObj.scene);
				CCommonEditorSceneManager.m_oPrefabMissingObjsList.ExAddVal(PrefabUtility.GetOutermostPrefabInstanceRoot(oObj));
			}

			// 스크립트 제거가 필요 할 경우
			if(GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(oObj) > KCDefine.B_VAL_0_INT)
			{
				EditorSceneManager.MarkSceneDirty(a_oObj.scene);
				GameObjectUtility.RemoveMonoBehavioursWithMissingScript(oObj);
			}
		}

		for(int i = 0; i < CCommonEditorSceneManager.m_oPrefabMissingObjsList.Count; ++i)
		{
			string oMsg = string.Format(KCEditorDefine.B_MSG_FMT_ALERT_P_MISSING_PREFAB, CCommonEditorSceneManager.m_oPrefabMissingObjsList[i].name);

			// 확인 버튼을 눌렀을 경우
			if(CEditorFunc.ShowOKCancelAlertPopup(KCEditorDefine.B_TEXT_ALERT, oMsg))
			{
				CFunc.RemoveObj(CCommonEditorSceneManager.m_oPrefabMissingObjsList[i]);
			}
			else
			{
				PrefabUtility.UnpackPrefabInstance(CCommonEditorSceneManager.m_oPrefabMissingObjsList[i], PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
			}
		}
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR
