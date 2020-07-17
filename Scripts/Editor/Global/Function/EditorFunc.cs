using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

// 에디터 함수
public static partial class EditorFunc {
	#region 클래스 함수
	//! 활성된 객체를 반환한다
	public static GameObject GetActiveObj(bool a_bIsInHierarchy = true) {
		var oObj = Selection.activeGameObject;

		return (oObj == null || (a_bIsInHierarchy && !oObj.activeInHierarchy)) ? null 
			: Selection.activeGameObject;
	}

	//! 독립 플랫폼 이름을 반환한다
	public static string GetStandalonePlatformName(EStandalonePlatformType a_ePlatformType) {
		Func.Assert(a_ePlatformType > EStandalonePlatformType.NONE && a_ePlatformType < EStandalonePlatformType.MAX_VALUE);
		return (a_ePlatformType == EStandalonePlatformType.WINDOWS) ? KDefine.B_PLATFORM_NAME_WINDOWS : KDefine.B_PLATFORM_NAME_MAC;
	}

	//! 안드로이드 플랫폼 이름을 반환한다
	public static string GetAndroidPlatformName(EAndroidPlatformType a_ePlatformType) {
		Func.Assert(a_ePlatformType > EAndroidPlatformType.NONE && a_ePlatformType < EAndroidPlatformType.MAX_VALUE);

		if(a_ePlatformType == EAndroidPlatformType.ONE_STORE) {
			return KDefine.B_PLATFORM_NAME_ONE_STORE;
		} else if(a_ePlatformType == EAndroidPlatformType.GALAXY_STORE) {
			return KDefine.B_PLATFORM_NAME_GALAXY_STORE;
		}

		return KDefine.B_PLATFORM_NAME_GOOGLE;
	}

	//! 그래픽 API 를 변경한다
	public static void SetGraphicAPI(BuildTarget a_eTarget, GraphicsDeviceType[] a_oDeviceTypes, bool a_bIsEnableAuto = true) {
		PlayerSettings.SetGraphicsAPIs(a_eTarget, a_oDeviceTypes);
		PlayerSettings.SetUseDefaultGraphicsAPIs(a_eTarget, a_bIsEnableAuto);
	}

	//! 에셋을 로드한다
	public static Object LoadAsset(string a_oFilepath) {
		Func.Assert(a_oFilepath.ExIsValid());
		var oAssets = AssetDatabase.LoadAllAssetsAtPath(a_oFilepath);

		return oAssets.ExIsValid() ? oAssets.First() : null;
	}

	//! 알림 팝업을 출력한다
	public static bool ShowAlertPopup(string a_oTitle,
		string a_oMsg, string a_oOKBtnText, string a_oCancelBtnText) {
		if(!a_oCancelBtnText.ExIsValid()) {
			return EditorUtility.DisplayDialog(a_oTitle, a_oMsg, a_oOKBtnText);
		}

		return EditorUtility.DisplayDialog(a_oTitle, a_oMsg, a_oOKBtnText, a_oCancelBtnText);
	}

	//! 에셋 데이터 베이스를 갱신한다
	public static void UpdateAssetDatabaseState() {
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}

	//! 젠킨스 빌드를 실행한다
	public static void ExecuteJenkinsBuild(string a_oPipeline, 
		string a_oProjectName, string a_oBuildMode, string a_oBuildFunc, string a_oPipelineName, Dictionary<string, string> a_oDataList = null) {
		Func.Assert(a_oPipeline.ExIsValid() && CPlatformBuilder.BuildInfoTable != null);

		var oStringBuilder = new System.Text.StringBuilder();
		string oURL = string.Format(CPlatformBuilder.BuildInfoTable.JenkinsInfo.m_oBuildURLFormat, a_oPipeline);

		oStringBuilder.AppendFormat(KEditorDefine.B_JENKINS_BUILD_CMD_FORMAT,
			oURL,
			CPlatformBuilder.BuildInfoTable.JenkinsInfo.m_oUserID,
			CPlatformBuilder.BuildInfoTable.JenkinsInfo.m_oAccessToken,
			CPlatformBuilder.BuildInfoTable.JenkinsInfo.m_oBuildToken);
			
		// 매개 변수를 설정한다 {
		string oSource = string.Format(KEditorDefine.B_JENKINS_SOURCE_FORMAT, 
			CPlatformBuilder.BuildInfoTable.JenkinsInfo.m_oSourceRoot, a_oProjectName);

		string oProjectPath = string.Format(KEditorDefine.B_JENKINS_PROJECT_PATH_FORMAT, 
			CPlatformBuilder.BuildInfoTable.JenkinsInfo.m_oWorkspaceRoot, oSource, KAppDefine.G_NAME_UNITY_PROJECT_ROOT);

		string oAnalytics = string.Format(KEditorDefine.B_JENKINS_ANALYTICS_FORMAT,
			CPlatformBuilder.BuildInfoTable.JenkinsInfo.m_oSourceRoot);
			
		var oDataList = a_oDataList ?? new Dictionary<string, string>();
		oDataList.ExAddValue(KEditorDefine.B_JENKINS_KEY_SOURCE, oSource);
		oDataList.ExAddValue(KEditorDefine.B_JENKINS_KEY_PROJECT_PATH, oProjectPath);
		oDataList.ExAddValue(KEditorDefine.B_JENKINS_KEY_BRANCH, CPlatformBuilder.BuildInfoTable.JenkinsInfo.m_oBranch);
		oDataList.ExAddValue(KEditorDefine.B_JENKINS_KEY_DISTRIBUTION_PATH, CPlatformBuilder.BuildInfoTable.JenkinsInfo.m_oDistributionPath);
		oDataList.ExAddValue(KEditorDefine.B_JENKINS_KEY_PROJECT_NAME, CPlatformBuilder.BuildInfoTable.JenkinsInfo.m_oProjectName);
		oDataList.ExAddValue(KEditorDefine.B_JENKINS_KEY_ANALYTICS, oAnalytics);
		oDataList.ExAddValue(KEditorDefine.B_JENKINS_KEY_BUILD_MODE, a_oBuildMode);
		oDataList.ExAddValue(KEditorDefine.B_JENKINS_KEY_BUILD_FUNC, a_oBuildFunc);
		oDataList.ExAddValue(KEditorDefine.B_JENKINS_KEY_PIPELINE_NAME, a_oPipelineName);

		foreach(var stKeyValue in oDataList) {
			oStringBuilder.Append(KEditorDefine.B_JENKINS_BUILD_PARAMETER_TOKEN);
			oStringBuilder.AppendFormat(KEditorDefine.B_JENKINS_BUILD_DATA_FORMAT, stKeyValue.Key, stKeyValue.Value);
		}
		// 매개 변수를 설정한다 }

		EditorFunc.ExecuteCmdline(oStringBuilder.ToString());
	}

	//! 독립 플랫폼 젠킨스 빌드를 실행한다
	public static void ExecuteStandalonePlatformJenkinsBuild(string a_oBuildMode, 
		string a_oBuildFunc, string a_oPipelineName, EStandalonePlatformType a_ePlatformType) {
		var oDataList = new Dictionary<string, string>() {
			[KEditorDefine.B_JENKINS_KEY_PLATFORM] = EditorFunc.GetStandalonePlatformName(a_ePlatformType)
		};

		EditorFunc.ExecuteJenkinsBuild(KEditorDefine.B_JENKINS_STANDALONE_PIPELINE, 
			KEditorDefine.B_JENKINS_STANDALONE_BUILD_PROJECT_NAME, a_oBuildMode, a_oBuildFunc, a_oPipelineName, oDataList);
	}

	//! iOS 플랫폼 젠킨스 빌드를 실행한다
	public static void ExecuteiOSPlatformJenkinsBuild(string a_oBuildMode, 
		string a_oBuildFunc, string a_oPipelineName, string a_oProfileID, string a_oIPAExportMethod) {
		Func.Assert(CPlatformBuilder.ProjectInfoTable != null);

		var oDataList = new Dictionary<string, string>() {
			[KEditorDefine.B_JENKINS_KEY_BUNDLE_ID] = CPlatformBuilder.ProjectInfoTable.iOSProjectInfo.m_oAppID,
			[KEditorDefine.B_JENKINS_KEY_PROFILE_ID] = a_oProfileID,
			[KEditorDefine.B_JENKINS_KEY_IPA_EXPORT_METHOD] = a_oIPAExportMethod
		};

		EditorFunc.ExecuteJenkinsBuild(KEditorDefine.B_JENKINS_IOS_PIPELINE, 
			KEditorDefine.B_JENKINS_IOS_BUILD_PROJECT_NAME, a_oBuildMode, a_oBuildFunc, a_oPipelineName, oDataList);
	}

	//! 안드로이드 플랫폼 젠킨스 빌드를 실행한다
	public static void ExecuteAndroidPlatformJenkinsBuild(string a_oBuildMode, 
		string a_oBuildFunc, string a_oPipelineName, string a_oBuildFileExtension, EAndroidPlatformType a_ePlatformType) {
		var oDataList = new Dictionary<string, string>() {
			[KEditorDefine.B_JENKINS_KEY_PLATFORM] = EditorFunc.GetAndroidPlatformName(a_ePlatformType),
			[KEditorDefine.B_JENKINS_KEY_BUILD_FILE_EXTENSION] = a_oBuildFileExtension
		};

		EditorFunc.ExecuteJenkinsBuild(KEditorDefine.B_JENKINS_ANDROID_PIPELINE, 
			KEditorDefine.B_JENKINS_ANDROID_BUILD_PROJECT_NAME, a_oBuildMode, a_oBuildFunc, a_oPipelineName, oDataList);
	}

	//! 커맨드 라인을 실행한다
	public static void ExecuteCmdline(string a_oParams) {
		Func.Assert(a_oParams.ExIsValid());

		if(Func.IsMacPlatform()) {
			EditorFunc.ExecuteCmdline(KEditorDefine.B_TOOL_PATH_SHELL,
				string.Format(KEditorDefine.B_CMDLINE_PARAMETER_FORMAT_SHELL, a_oParams));
		} else if(Func.IsWindowsPlatform()) {
			EditorFunc.ExecuteCmdline(KEditorDefine.B_TOOL_PATH_CMD_PROMPT,
				string.Format(KEditorDefine.B_CMDLINE_PARAMETER_FORMAT_CMD_PROMPT, a_oParams));
		}
	}

	//! 커맨드 라인을 실행한다
	public static void ExecuteCmdline(string a_oFilepath, string a_oParams) {
		Func.Assert(a_oFilepath.ExIsValid() && a_oParams.ExIsValid());

		var oStartInfo = new ProcessStartInfo(a_oFilepath, a_oParams);
		oStartInfo.UseShellExecute = true;

		Process.Start(oStartInfo);
	}
	
	//! 프리팹 인스턴스를 생성한다
	public static GameObject CreatePrefabInstance(string a_oName, 
		GameObject a_oOrigin, GameObject a_oParent, bool a_bIsStayWorldState = false) {
		Func.Assert(a_oOrigin != null);

		var oObj = PrefabUtility.InstantiatePrefab(a_oOrigin) as GameObject;
		oObj.name = a_oName;
		oObj.transform.localScale = a_oOrigin.transform.localScale;

		oObj.transform.SetParent(a_oParent?.transform, a_bIsStayWorldState);
		oObj.transform.SetAsLastSibling();
		
		return oObj;
	}
	#endregion			// 클래스 함수

	#region 제네릭 클래스 함수
	//! 에셋을 탐색한다
	public static T FindAsset<T>(string a_oFilter, string[] a_oSearchPaths) where T : Object {
		var oAssets = EditorFunc.FindAssets<T>(a_oFilter, a_oSearchPaths);
		return oAssets.ExIsValid() ? oAssets.First() : null;
	}

	//! 에셋을 탐색한다
	public static List<T> FindAssets<T>(string a_oFilter, string[] a_oSearchPaths) where T : Object {
		Func.Assert(a_oFilter.ExIsValid() && a_oSearchPaths.ExIsValid());

		var oAssetList = new List<T>();
		var oAssetGUIDs = AssetDatabase.FindAssets(a_oFilter, a_oSearchPaths);

		for(int i = 0; i < oAssetGUIDs?.Length; ++i) {
			string oPath = AssetDatabase.GUIDToAssetPath(oAssetGUIDs[i]);
			var oAsset = AssetDatabase.LoadAssetAtPath<T>(oPath);

			if(oAsset != null) {
				oAssetList.Add(oAsset);
			}
		}

		return oAssetList;
	}

	//! 에디터 윈도우를 생성한다
	public static T CreateEditorWindow<T>(string a_oName, Vector2 a_stMinSize) where T : EditorWindow {
		Func.Assert(a_oName.ExIsValid());

		var oPopup = EditorWindow.CreateWindow<T>(a_oName);
		oPopup.minSize = a_stMinSize;

		return oPopup;
	}

	//! 에셋을 생성한다
	public static void CreateAsset<T>(T a_tAsset, string a_oFilepath, bool a_bIsFocus = true) where T : Object {
		Func.Assert(a_tAsset != null && a_oFilepath.ExIsValid());

		AssetDatabase.CreateAsset(a_tAsset, a_oFilepath);
		EditorFunc.UpdateAssetDatabaseState();

		if(a_bIsFocus) {
			Selection.activeObject = a_tAsset;
			EditorUtility.FocusProjectWindow();
		}
	}

	//! 스크립트 객체를 생성한다
	public static T CreateScriptableObj<T>() where T : ScriptableObject {
		string oFilepath = string.Format(KEditorDefine.B_PATH_FORMAT_SCRIPTABLE_OBJ, typeof(T).ToString());
		var oScriptableObj = ScriptableObject.CreateInstance<T>();

		EditorFunc.CreateAsset<T>(oScriptableObj, oFilepath);
		return oScriptableObj;
	}
	#endregion			// 제네릭 클래스 함수
}
