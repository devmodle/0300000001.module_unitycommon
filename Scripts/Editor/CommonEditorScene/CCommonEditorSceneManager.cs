using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Unity.Linq;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

/** 공용 에디터 씬 관리자 */
public static partial class CCommonEditorSceneManager {
#if BURST_COMPILER_MODULE_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE
	/** 모바일 버스트 AOT 설정 */
	private struct STMobileBurstAOTSettings {
		public bool EnableSafetyChecks;
		public bool EnableOptimisations;
		public bool EnableBurstCompilation;
		public bool EnableDebugInAllBuilds;

		public int Version;
		public int OptimizeFor;
		public int CpuMinTargetX32;
		public int CpuMaxTargetX32;
		public int CpuMinTargetX64;
		public int CpuMaxTargetX64;
	}

	/** 독립 플랫폼 버스트 AOT 설정 */
	private struct STStandaloneBurstAOTSettings {
		public bool EnableSafetyChecks;
		public bool EnableOptimisations;
		public bool UsePlatformSDKLinker;
		public bool EnableBurstCompilation;
		public bool EnableDebugInAllBuilds;

		public int Version;
		public int OptimizeFor;
		public int CpuTargetsX64;
		public int CpuMinTargetX32;
		public int CpuMaxTargetX32;
		public int CpuMinTargetX64;
		public int CpuMaxTargetX64;
	}

	/** 모바일 버스트 AOT 설정 래퍼 */
	private struct STMobileBurstAOTSettingsWrapper {
		public STMobileBurstAOTSettings MonoBehaviour;
	}

	/** 독립 플랫폼 버스트 AOT 설정 래퍼 */
	private struct STStandaloneBurstAOTSettingsWrapper {
		public STStandaloneBurstAOTSettings MonoBehaviour;
	}
#endif			// #if BURST_COMPILER_MODULE_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE

	#region 클래스 변수
	private static bool m_bIsEnableBuild = false;
	private static bool m_bIsEnableSetup = false;

	private static float m_fUpdateSkipTime = 0.0f;
	private static float m_fHierarchySkipTime = 0.0f;
	
	private static GUIStyle m_oTextGUIStyle = new GUIStyle() {
		alignment = TextAnchor.MiddleRight, fontStyle = FontStyle.Bold
	};

	private static GUIStyle m_oOutlineGUIStyle = new GUIStyle() {
		alignment = TextAnchor.MiddleRight, fontStyle = FontStyle.Bold
	};

	private static Dictionary<string, string> m_oSortingLayerDict = new Dictionary<string, string>() {
		[KCDefine.U_SORTING_L_ABS] = "A",
		[KCDefine.U_SORTING_L_DEF] = "D",
		
		[KCDefine.U_SORTING_L_TOP] = "T",
		[KCDefine.U_SORTING_L_TOPMOST] = "TM",
		
		[KCDefine.U_SORTING_L_FOREGROUND] = "F",
		[KCDefine.U_SORTING_L_BACKGROUND] = "B",
		
		[KCDefine.U_SORTING_L_OVERGROUND] = "O",
		[KCDefine.U_SORTING_L_UNDERGROUND] = "U",

		[KCDefine.U_SORTING_L_OVERLAY_ABS] = "OA",
		[KCDefine.U_SORTING_L_OVERLAY_DEF] = "OD",
		
		[KCDefine.U_SORTING_L_OVERLAY_TOP] = "OT",
		[KCDefine.U_SORTING_L_OVERLAY_TOPMOST] = "OTM",
		
		[KCDefine.U_SORTING_L_OVERLAY_FOREGROUND] = "OF",
		[KCDefine.U_SORTING_L_OVERLAY_BACKGROUND] = "OB",
		
		[KCDefine.U_SORTING_L_OVERLAY_OVERGROUND] = "OO",
		[KCDefine.U_SORTING_L_OVERLAY_UNDERGROUND] = "OU"
	};
	#endregion			// 클래스 변수

	#region 클래스 함수
	/** 생성자 */
	static CCommonEditorSceneManager() {
		// GUI 스타일을 설정한다 {
		CCommonEditorSceneManager.m_oTextGUIStyle.normal = new GUIStyleState() {
			textColor = KCEditorDefine.B_COLOR_HIERARCHY_TEXT
		};

		CCommonEditorSceneManager.m_oOutlineGUIStyle.normal = new GUIStyleState() {
			textColor = KCEditorDefine.B_COLOR_HIERARCHY_OUTLINE
		};
		// GUI 스타일을 설정한다 }

		CCommonEditorSceneManager.SetupCallbacks();
	}

	/** 스크립트가 로드 되었을 경우 */
	[UnityEditor.Callbacks.DidReloadScripts]
	public static void OnLoadScript() {
		CCommonEditorSceneManager.m_bIsEnableBuild = true;
		CCommonEditorSceneManager.m_bIsEnableSetup = true;
	}
	
	/** 상태를 갱신한다 */
	private static void Update() {
		// 상태 갱신이 가능 할 경우
		if(CEditorAccess.IsEnableUpdateState) {
			CCommonEditorSceneManager.m_fUpdateSkipTime += Mathf.Clamp01(Time.deltaTime);
			CCommonEditorSceneManager.m_fHierarchySkipTime += Mathf.Clamp01(Time.deltaTime);

			// 설정 가능 할 경우
			if(CCommonEditorSceneManager.m_bIsEnableSetup) {
				CCommonEditorSceneManager.m_bIsEnableSetup = false;
				CAutoSceneImporter.ImportAllScenes();
				
				CPlatformOptsSetter.SetupPlayerOpts();
				CPlatformOptsSetter.SetupEditorOpts();
				CPlatformOptsSetter.SetupProjOpts();
				CPlatformOptsSetter.SetupPluginProjs();
			}

			// 빌드 가능 할 경우
			if(CCommonEditorSceneManager.m_bIsEnableBuild) {
				CCommonEditorSceneManager.m_bIsEnableBuild = false;
				string oBuildMethod = CFunc.ReadStr(KCEditorDefine.B_DATA_P_BUILD_METHOD);

				// 빌드 메서드가 존재 할 경우
				if(oBuildMethod.ExIsValid()) {
					typeof(CPlatformBuilder).GetMethod(oBuildMethod, KCDefine.B_BINDING_F_PUBLIC_STATIC)?.Invoke(null, null);
				} else {
					CFactory.RemoveFile(KCEditorDefine.B_DATA_P_BUILD_METHOD);
				}
			}

			// 갱신 주기가 지났을 경우
			if(CCommonEditorSceneManager.m_fUpdateSkipTime.ExIsGreateEquals(KCEditorDefine.B_DELTA_T_EDITOR_SM_SCENE_UPDATE)) {
				CCommonEditorSceneManager.m_fUpdateSkipTime = KCDefine.B_VAL_0_FLT;
				CFunc.EnumerateComponents<CSceneManager>((a_oSceneManager) => { a_oSceneManager.EditorSetupScene(); return true; });

				CCommonEditorSceneManager.SetupTags();
				CCommonEditorSceneManager.SetupLayers();
				CCommonEditorSceneManager.SetupCanvases();
				CCommonEditorSceneManager.SetupLightOpts();
				CCommonEditorSceneManager.SetupStaticObjs();
				CCommonEditorSceneManager.SetupPreloadAssets();
				CCommonEditorSceneManager.SetupLocalizeInfos();
				CCommonEditorSceneManager.SetupSceneTemplates();

#if INPUT_SYSTEM_MODULE_ENABLE
				CCommonEditorSceneManager.SetupInputSystem();
#endif			// #if INPUT_SYSTEM_MODULE_ENABLE

#if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE
				CCommonEditorSceneManager.SetupRenderingPipeline();
#endif			// #if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE

#if BURST_COMPILER_MODULE_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE
				CCommonEditorSceneManager.SetupBurstCompiler();
#endif			// #if BURST_COMPILER_MODULE_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE

				// 갱신 주기가 지났을 경우
				if(CCommonEditorSceneManager.m_fHierarchySkipTime.ExIsGreateEquals(KCEditorDefine.B_DELTA_T_HIERARCHY_UPDATE)) {
					CCommonEditorSceneManager.m_fHierarchySkipTime = KCDefine.B_VAL_0_FLT;

					CFunc.EnumerateRootObjs((a_oObj) => {
						var oEnumerator = a_oObj.DescendantsAndSelf();
						var oRemoveObjList = new List<GameObject>();

						foreach(var oObj in oEnumerator) {
							int nNumMissingScripts = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(oObj);

							// 객체 제거가 필요 할 경우
							if(PrefabUtility.IsPrefabAssetMissing(oObj)) {
								oRemoveObjList.ExAddVal(oObj);
							}

							// 스크립트 제거가 필요 할 경우
							if(nNumMissingScripts > KCDefine.B_VAL_0_INT) {
								GameObjectUtility.RemoveMonoBehavioursWithMissingScript(oObj);
								EditorSceneManager.MarkSceneDirty(a_oObj.scene);
							}
						}

						for(int i = 0; i < oRemoveObjList.Count; ++i) {
							CFactory.RemoveObj(oRemoveObjList[i]);
						}

						return true;
					});
				}
			}
		}
	}

	/** 계층 뷰 UI 상태를 갱신한다 */
	private static void UpdateHierarchyUIState(int a_nInstanceID, Rect a_stRect) {
		var oObj = EditorUtility.InstanceIDToObject(a_nInstanceID) as GameObject;

		// 객체가 존재 할 경우
		if(oObj != null) {
			var oComponents = oObj.GetComponents<Component>();

			for(int i = 0; i < oComponents.Length; ++i) {
				// 컴포넌트가 존재 할 경우
				if(oComponents[i] != null) {
					var oType = oComponents[i].GetType();

					var oSortingLayerProperty = oType.GetProperty(KCEditorDefine.B_PROPERTY_N_SORTING_LAYER, KCDefine.B_BINDING_F_PUBLIC_INSTANCE);
					var oSortingOrderProperty = oType.GetProperty(KCEditorDefine.B_PROPERTY_N_SORTING_ORDER, KCDefine.B_BINDING_F_PUBLIC_INSTANCE);

					string oSortingLayer = (string)oSortingLayerProperty?.GetValue(oComponents[i]);
					oSortingLayer = oSortingLayer.ExIsValid() ? CCommonEditorSceneManager.m_oSortingLayerDict.GetValueOrDefault(oSortingLayer, string.Empty) : string.Empty;
					
					// 프로퍼티가 존재 할 경우
					if(oSortingOrderProperty != null && oSortingLayer.ExIsValid()) {
						a_stRect.position += new Vector2((a_stRect.size.x + KCEditorDefine.B_OFFSET_HIERARCHY_TEXT) * -1.0f, KCDefine.B_VAL_0_FLT);
						string oStr = string.Format(KCEditorDefine.B_SORTING_OI_FMT, oSortingLayer, oSortingOrderProperty.GetValue(oComponents[i]));

						var oRectList = new List<Rect>() {
							new Rect(a_stRect.x + KCEditorDefine.B_OFFSET_HIERARCHY_OUTLINE, a_stRect.y, a_stRect.width, a_stRect.height),
							new Rect(a_stRect.x - KCEditorDefine.B_OFFSET_HIERARCHY_OUTLINE, a_stRect.y, a_stRect.width, a_stRect.height),
							new Rect(a_stRect.x, a_stRect.y + KCEditorDefine.B_OFFSET_HIERARCHY_OUTLINE, a_stRect.width, a_stRect.height),
							new Rect(a_stRect.x, a_stRect.y - KCEditorDefine.B_OFFSET_HIERARCHY_OUTLINE, a_stRect.width, a_stRect.height)
						};

						for(int j = 0; j < oRectList.Count; ++j) {
							GUI.Label(oRectList[j], oStr, CCommonEditorSceneManager.m_oOutlineGUIStyle);
						}

						GUI.Label(a_stRect, oStr, CCommonEditorSceneManager.m_oTextGUIStyle);
					}
				}
			}
		}
	}

	/** 플레이 모드 상태가 갱신 되었을 경우 */
	private static void OnUpdatePlayModeState(PlayModeStateChange a_ePlayMode) {
		// 에디터 모드 일 경우
		if(a_ePlayMode == PlayModeStateChange.EnteredEditMode) {
			Time.timeScale = KCDefine.B_VAL_1_FLT;
		}
	}

	/** 프로젝트 상태가 갱신 되었을 경우 */
	private static void OnUpdateProjectState() {
		CCommonEditorSceneManager.SetupSpriteAtlases();
	}
	#endregion			// 클래스 함수
}
#endif			// #if UNITY_EDITOR
