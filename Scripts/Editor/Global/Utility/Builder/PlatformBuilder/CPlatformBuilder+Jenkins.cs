using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 플랫폼 빌더 - 젠킨스
public static partial class CPlatformBuilder {
	#region 클래스 함수
	//! 젠킨스 빌드를 실행한다
	public static void ExecuteJenkinsBuild(string a_oPipeline, 
		string a_oProjectName, string a_oBuildMode, string a_oBuildFunc, string a_oPipelineName, Dictionary<string, string> a_oDataList = null) {
		CBAccess.Assert(a_oPipeline.ExIsValid() && CPlatformBuildOption.BuildInfoTable != null);

		var oStringBuilder = new System.Text.StringBuilder();
		string oURL = string.Format(CPlatformBuildOption.BuildInfoTable.JenkinsInfo.m_oBuildURLFormat, a_oPipeline);

		oStringBuilder.AppendFormat(KBEditorDefine.JENKINS_BUILD_CMD_FORMAT,
			oURL,
			CPlatformBuildOption.BuildInfoTable.JenkinsInfo.m_oUserID,
			CPlatformBuildOption.BuildInfoTable.JenkinsInfo.m_oAccessToken,
			CPlatformBuildOption.BuildInfoTable.JenkinsInfo.m_oBuildToken);
			
		// 매개 변수를 설정한다 {
		string oSource = string.Format(KBEditorDefine.JENKINS_SOURCE_FORMAT, 
			CPlatformBuildOption.BuildInfoTable.JenkinsInfo.m_oSourceRoot, a_oProjectName);

		// FIXME: 임시 코드 수정 필요 (현재 앱 번들 식별자로 프로젝트 이름 설정)
		string oProjectPath = string.Format(KBEditorDefine.JENKINS_PROJECT_PATH_FORMAT, 
			CPlatformBuildOption.BuildInfoTable.JenkinsInfo.m_oWorkspaceRoot, oSource, Application.identifier);// KAppDefine.G_NAME_UNITY_PROJECT_ROOT);

		string oAnalytics = string.Format(KBEditorDefine.JENKINS_ANALYTICS_FORMAT,
			CPlatformBuildOption.BuildInfoTable.JenkinsInfo.m_oSourceRoot);
			
		var oDataList = a_oDataList ?? new Dictionary<string, string>();
		oDataList.ExAddValue(KBEditorDefine.JENKINS_KEY_SOURCE, oSource);
		oDataList.ExAddValue(KBEditorDefine.JENKINS_KEY_PROJECT_PATH, oProjectPath);
		oDataList.ExAddValue(KBEditorDefine.JENKINS_KEY_BRANCH, CPlatformBuildOption.BuildInfoTable.JenkinsInfo.m_oBranch);
		oDataList.ExAddValue(KBEditorDefine.JENKINS_KEY_DISTRIBUTION_PATH, CPlatformBuildOption.BuildInfoTable.JenkinsInfo.m_oDistributionPath);
		oDataList.ExAddValue(KBEditorDefine.JENKINS_KEY_PROJECT_NAME, CPlatformBuildOption.BuildInfoTable.JenkinsInfo.m_oProjectName);
		oDataList.ExAddValue(KBEditorDefine.JENKINS_KEY_ANALYTICS, oAnalytics);
		oDataList.ExAddValue(KBEditorDefine.JENKINS_KEY_BUILD_MODE, a_oBuildMode);
		oDataList.ExAddValue(KBEditorDefine.JENKINS_KEY_BUILD_FUNC, a_oBuildFunc);
		oDataList.ExAddValue(KBEditorDefine.JENKINS_KEY_PIPELINE_NAME, a_oPipelineName);

		foreach(var stKeyValue in oDataList) {
			oStringBuilder.Append(KBEditorDefine.JENKINS_BUILD_PARAMETER_TOKEN);
			oStringBuilder.AppendFormat(KBEditorDefine.JENKINS_BUILD_DATA_FORMAT, stKeyValue.Key, stKeyValue.Value);
		}
		// 매개 변수를 설정한다 }

		EditorFunc.ExecuteCmdline(oStringBuilder.ToString());
	}

	//! 독립 플랫폼 젠킨스 빌드를 실행한다
	public static void ExecuteStandalonePlatformJenkinsBuild(string a_oBuildMode, 
		string a_oBuildFunc, string a_oPipelineName, EStandalonePlatformType a_ePlatformType) {
		var oDataList = new Dictionary<string, string>() {
			[KBEditorDefine.JENKINS_KEY_PLATFORM] = CBEditorAccess.GetStandalonePlatformName(a_ePlatformType)
		};

		CPlatformBuilder.ExecuteJenkinsBuild(KBEditorDefine.JENKINS_STANDALONE_PIPELINE, 
			KBEditorDefine.JENKINS_STANDALONE_BUILD_PROJECT_NAME, a_oBuildMode, a_oBuildFunc, a_oPipelineName, oDataList);
	}

	//! iOS 플랫폼 젠킨스 빌드를 실행한다
	public static void ExecuteiOSPlatformJenkinsBuild(string a_oBuildMode, 
		string a_oBuildFunc, string a_oPipelineName, string a_oProfileID, string a_oIPAExportMethod) {
		CBAccess.Assert(CPlatformBuildOption.ProjectInfoTable != null);

		var oDataList = new Dictionary<string, string>() {
			[KBEditorDefine.JENKINS_KEY_BUNDLE_ID] = CPlatformBuildOption.ProjectInfoTable.iOSProjectInfo.m_oAppID,
			[KBEditorDefine.JENKINS_KEY_PROFILE_ID] = a_oProfileID,
			[KBEditorDefine.JENKINS_KEY_IPA_EXPORT_METHOD] = a_oIPAExportMethod
		};

		CPlatformBuilder.ExecuteJenkinsBuild(KBEditorDefine.JENKINS_IOS_PIPELINE, 
			KBEditorDefine.JENKINS_IOS_BUILD_PROJECT_NAME, a_oBuildMode, a_oBuildFunc, a_oPipelineName, oDataList);
	}

	//! 안드로이드 플랫폼 젠킨스 빌드를 실행한다
	public static void ExecuteAndroidPlatformJenkinsBuild(string a_oBuildMode, 
		string a_oBuildFunc, string a_oPipelineName, string a_oBuildFileExtension, EAndroidPlatformType a_ePlatformType) {
		var oDataList = new Dictionary<string, string>() {
			[KBEditorDefine.JENKINS_KEY_PLATFORM] = CBEditorAccess.GetAndroidPlatformName(a_ePlatformType),
			[KBEditorDefine.JENKINS_KEY_BUILD_FILE_EXTENSION] = a_oBuildFileExtension
		};

		CPlatformBuilder.ExecuteJenkinsBuild(KBEditorDefine.JENKINS_ANDROID_PIPELINE, 
			KBEditorDefine.JENKINS_ANDROID_BUILD_PROJECT_NAME, a_oBuildMode, a_oBuildFunc, a_oPipelineName, oDataList);
	}
	#endregion			// 클래스 함수
}
