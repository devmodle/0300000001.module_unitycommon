using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

/** 공용 에디터 씬 관리자 */
[InitializeOnLoad]
public static partial class CCommonEditorSceneManager
{
	#region 클래스 변수
	private static GUIStyle m_oTextGUIStyle = new GUIStyle()
	{
		alignment = TextAnchor.MiddleRight,
		fontStyle = FontStyle.Bold
	};

	private static GUIStyle m_oOutlineGUIStyle = new GUIStyle()
	{
		alignment = TextAnchor.MiddleRight,
		fontStyle = FontStyle.Bold
	};

	private static Dictionary<string, string> m_oSortingLayerDict = new Dictionary<string, string>()
	{
		[KCDefine.B_SORTING_L_ABS] = "A",
		[KCDefine.B_SORTING_L_DEF] = "D",

		[KCDefine.B_SORTING_L_TOP] = "T",
		[KCDefine.B_SORTING_L_TOPMOST] = "TM",

		[KCDefine.B_SORTING_L_FOREGROUND] = "F",
		[KCDefine.B_SORTING_L_BACKGROUND] = "B",

		[KCDefine.B_SORTING_L_OVERGROUND] = "O",
		[KCDefine.B_SORTING_L_UNDERGROUND] = "U",

		[KCDefine.B_SORTING_L_OVERLAY_ABS] = "OA",
		[KCDefine.B_SORTING_L_OVERLAY_DEF] = "OD",

		[KCDefine.B_SORTING_L_OVERLAY_TOP] = "OT",
		[KCDefine.B_SORTING_L_OVERLAY_TOPMOST] = "OTM",

		[KCDefine.B_SORTING_L_OVERLAY_FOREGROUND] = "OF",
		[KCDefine.B_SORTING_L_OVERLAY_BACKGROUND] = "OB",

		[KCDefine.B_SORTING_L_OVERLAY_OVERGROUND] = "OO",
		[KCDefine.B_SORTING_L_OVERLAY_UNDERGROUND] = "OU"
	};

	private static bool m_bIsEnableSetup = false;
	private static bool m_bIsEnableBuild = false;

	private static double m_dblSkipTimeUpdate = 0.0;
	private static System.Text.StringBuilder m_oStrBuilder = new System.Text.StringBuilder();
	private static List<string> m_oListSceneNameSample = new List<string>();

	[Header("=====> Game Objects <=====")]
	private static List<GameObject> m_oPrefabMissingObjList = new List<GameObject>();
	#endregion // 클래스 변수

	#region 클래스 함수
	/** 생성자 */
	static CCommonEditorSceneManager()
	{
		// 플레이 모드가 아닐 경우
		if(!EditorApplication.isPlaying)
		{
			CCommonEditorSceneManager.m_oTextGUIStyle.normal = new GUIStyleState()
			{
				textColor = KCDefineEditor.G_HIERARCHY_COLOR_TEXT
			};

			CCommonEditorSceneManager.m_oOutlineGUIStyle.normal = new GUIStyleState()
			{
				textColor = KCDefineEditor.G_HIERARCHY_COLOR_OUTLINE
			};

			CCommonEditorSceneManager.m_dblSkipTimeUpdate = EditorApplication.timeSinceStartup;

			CCommonEditorSceneManager.m_oListSceneNameSample.ExAddVal(KCDefine.B_SCENE_N_SAMPLE);
			CCommonEditorSceneManager.m_oListSceneNameSample.ExAddVal(KCDefine.B_SCENE_N_SAMPLE_MENU);
			CCommonEditorSceneManager.m_oListSceneNameSample.ExAddVal(KCDefine.B_SCENE_N_SAMPLE_RESEARCH);
			CCommonEditorSceneManager.m_oListSceneNameSample.ExAddVal(KCDefine.B_SCENE_N_SAMPLE_EDITOR);
		}

		CCommonEditorSceneManager.SetupCallbacks();
	}

	/** 스크립트가 로드되었을 경우 */
	[UnityEditor.Callbacks.DidReloadScripts]
	public static void OnLoadScript()
	{
		CCommonEditorSceneManager.m_bIsEnableSetup = true;
		CCommonEditorSceneManager.m_bIsEnableBuild = true;
	}

	/** 상태를 갱신한다 */
	private static void Update()
	{
		// 상태 갱신이 가능 할 경우
		if(CAccessEditor.IsEnableUpdateState)
		{
			// 설정 가능 할 경우
			if(CCommonEditorSceneManager.m_bIsEnableSetup)
			{
				CCommonEditorSceneManager.m_bIsEnableSetup = false;
				CImporterScene.ImportAllScenes();

				CSetterOptsPlatform.SetupProjOpts();
				CSetterOptsPlatform.SetupPlayerOpts();
				CSetterOptsPlatform.SetupEditorOpts();
				CSetterOptsPlatform.SetupPluginProjs();
			}

			// 갱신 주기가 지났을 경우
			if((EditorApplication.timeSinceStartup - CCommonEditorSceneManager.m_dblSkipTimeUpdate).ExIsGreatEquals(KCDefine.B_VAL_3_REAL))
			{
				CCommonEditorSceneManager.m_dblSkipTimeUpdate = EditorApplication.timeSinceStartup;
				CAccess.EnumerateComponents<CSceneManager>((a_oSceneManager) => { a_oSceneManager.EditorSetupScene(); return true; });

				CCommonEditorSceneManager.SetupTags();
				CCommonEditorSceneManager.SetupLightOpts();
				CCommonEditorSceneManager.SetupRaycasters();
				CCommonEditorSceneManager.SetupLocalizeInfos();

#if ENABLE_ADAPTIVEPERFORMANCE
				CCommonEditorSceneManager.SetupAdaptivePerformance();
#endif // #if ENABLE_ADAPTIVEPERFORMANCE

#if URP_MODULE_ENABLE
				CCommonEditorSceneManager.SetupURP();
#endif // #if URP_MODULE_ENABLE

#if LOCALIZE_MODULE_ENABLE
				CCommonEditorSceneManager.SetupLocalize();
#endif // #if LOCALIZE_MODULE_ENABLE

#if INPUT_SYSTEM_MODULE_ENABLE
				CCommonEditorSceneManager.SetupInputSystem();
#endif // #if INPUT_SYSTEM_MODULE_ENABLE

#if BURST_MODULE_ENABLE
				CCommonEditorSceneManager.SetupBurst();
#endif // #if BURST_MODULE_ENABLE

				CAccess.EnumerateRootObjs((a_oObj) =>
				{
					// 최상단 UI 일 경우
					if(KCDefineEditor.B_OBJ_N_ROOT_UIS_LIST.Contains(a_oObj.name))
					{
						CCommonEditorSceneManager.SetupLayers(a_oObj);
					}

					// 최상단 객체 일 경우
					if(KCDefineEditor.B_OBJ_N_ROOT_OBJ_LIST.Contains(a_oObj.name))
					{
						CCommonEditorSceneManager.SetupStaticObjs(a_oObj);
					}

					CCommonEditorSceneManager.UpdateMissingScriptState(a_oObj);
					return true;
				});

				// 유니티 패키지 정보를 설정한다 {
				CCommonEditorSceneManager.m_oStrBuilder.Clear();

				CFunc.EnumerateDirectories(KCDefineEditor.B_ABS_DIR_P_UNITY_PACKAGES, (a_oFileList, a_oDirList) =>
				{
					for(int i = 0; i < a_oFileList.Count; ++i)
					{
						// DS Store 파일이 아닐 경우
						if(!a_oFileList[i].EndsWith(KCDefine.B_FILE_EXTENSION_DS_STORE))
						{
							string oDirPath = Path.GetRelativePath(KCDefineEditor.B_ABS_DIR_P_UNITY_PACKAGES, a_oFileList[i]);
							CCommonEditorSceneManager.m_oStrBuilder.AppendLine(oDirPath.Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH));
						}
					}

					return true;
				});
				// 유니티 패키지 정보를 설정한다 }

				// 디렉토리가 존재 할 경우
				if(Directory.Exists(KCDefineEditor.B_ABS_DIR_P_UNITY_PACKAGES))
				{
					string oDirPath = Path.GetDirectoryName(KCDefineEditor.B_ABS_DIR_P_UNITY_PACKAGES).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH);
					CFunc.WriteStr(string.Format(KCDefine.B_TEXT_FMT_2_COMBINE, oDirPath, KCDefine.B_FILE_EXTENSION_TXT), CCommonEditorSceneManager.m_oStrBuilder.ToString(), false);
				}

				// 빌드 가능 할 경우
				if(CCommonEditorSceneManager.m_bIsEnableBuild)
				{
					string oBuildMethod = CFunc.ReadStr(KCDefineEditor.B_DATA_P_BUILD_METHOD, false);
					CCommonEditorSceneManager.m_bIsEnableBuild = false;

					// 빌드 메서드가 존재 할 경우
					if(oBuildMethod.ExIsValid())
					{
						typeof(CBuilderPlatform).GetMethod(oBuildMethod, KCDefine.B_BINDING_F_PUBLIC_STATIC)?.Invoke(null, null);
					}
					else
					{
						CFunc.RemoveFile(KCDefineEditor.B_DATA_P_BUILD_METHOD);
					}
				}
			}
		}
	}

	/** 계층 뷰 UI 상태를 갱신한다 */
	private static void UpdateHierarchyUIState(int a_nInstanceID, Rect a_stRect)
	{
		var oObj = EditorUtility.InstanceIDToObject(a_nInstanceID) as GameObject;

		// 객체가 존재 할 경우
		if(oObj != null)
		{
			var oComponents = oObj.GetComponents<Component>();

			for(int i = 0; i < oComponents.Length; ++i)
			{
				// 컴포넌트가 존재 할 경우
				if(oComponents[i] != null)
				{
					var oSortingLayerProperty = oComponents[i].GetType().GetProperty(KCDefineEditor.B_PROPERTY_N_SORTING_LAYER, KCDefine.B_BINDING_F_PUBLIC_INSTANCE);
					var oSortingOrderProperty = oComponents[i].GetType().GetProperty(KCDefineEditor.B_PROPERTY_N_SORTING_ORDER, KCDefine.B_BINDING_F_PUBLIC_INSTANCE);

					string oSortingLayer = (string)oSortingLayerProperty?.GetValue(oComponents[i]);
					oSortingLayer = oSortingLayer.ExIsValid() ? CCommonEditorSceneManager.m_oSortingLayerDict.ExGetVal(oSortingLayer, string.Empty) : string.Empty;

					// 프로퍼티가 존재 할 경우
					if(oSortingOrderProperty != null && oSortingLayer.ExIsValid())
					{
						a_stRect.position += new Vector2((a_stRect.size.x + KCDefineEditor.G_HIERARCHY_OFFSET_TEXT) * -1.0f, KCDefine.B_VAL_0_REAL);
						string oStr = string.Format(KCDefineEditor.B_SORTING_OI_FMT, oSortingLayer, oSortingOrderProperty.GetValue(oComponents[i]));

						var oRectList = new List<Rect>() {
							new Rect(a_stRect.x + KCDefine.B_VAL_1_REAL, a_stRect.y, a_stRect.width, a_stRect.height),
							new Rect(a_stRect.x - KCDefine.B_VAL_1_REAL, a_stRect.y, a_stRect.width, a_stRect.height),
							new Rect(a_stRect.x, a_stRect.y + KCDefine.B_VAL_1_REAL, a_stRect.width, a_stRect.height),
							new Rect(a_stRect.x, a_stRect.y - KCDefine.B_VAL_1_REAL, a_stRect.width, a_stRect.height)
						};

						for(int j = 0; j < oRectList.Count; ++j)
						{
							GUI.Label(oRectList[j], oStr, CCommonEditorSceneManager.m_oOutlineGUIStyle);
						}

						GUI.Label(a_stRect, oStr, CCommonEditorSceneManager.m_oTextGUIStyle);
					}
				}
			}
		}
	}

	/** 플레이 모드 상태가 갱신되었을 경우 */
	private static void OnUpdatePlayModeState(PlayModeStateChange a_ePlayMode)
	{
		// 에디터 모드 일 경우
		if(a_ePlayMode == PlayModeStateChange.EnteredEditMode)
		{
			CAccess.SetTimeScale(KCDefine.B_VAL_1_REAL);
		}
	}

	/** 프로젝트 상태가 갱신되었을 경우 */
	private static void OnUpdateProjectState()
	{
		CCommonEditorSceneManager.SetupPreloadAssets();
		CCommonEditorSceneManager.SetupSpriteAtlases();
		CCommonEditorSceneManager.SetupSceneTemplates();
	}

	/** 씬이 열렸을 경우 */
	private static void OnOpenScene(Scene a_stScene, OpenSceneMode a_eMode)
	{
		// 중첩 모드 일 경우
		if(a_eMode == OpenSceneMode.Additive)
		{
			// Do Something
		}
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR
