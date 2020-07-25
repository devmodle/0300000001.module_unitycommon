using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

//! 플랫폼 빌더 - 젠킨스
public static partial class CPlatformBuilder {
	#region 클래스 함수
	//! 젠킨스 빌드를 실행한다
	public static void ExecuteJenkinsBuild(string a_oPipeline, 
		string a_oProjName, string a_oBuildMode, string a_oBuildFunc, string a_oPipelineName, Dictionary<string, string> a_oDataList = null) {
		CAccess.Assert(a_oPipeline.ExIsValid() && CPlatformBuildOption.BuildInfoTable != null);

		var oStringBuilder = new System.Text.StringBuilder();
		string oURL = string.Format(CPlatformBuildOption.BuildInfoTable.JenkinsInfo.m_oBuildURLFormat, a_oPipeline);

		oStringBuilder.AppendFormat(KCEditorDefine.B_JENKINS_BUILD_CMD_FORMAT,
			oURL,
			CPlatformBuildOption.BuildInfoTable.JenkinsInfo.m_oUserID,
			CPlatformBuildOption.BuildInfoTable.JenkinsInfo.m_oAccessToken,
			CPlatformBuildOption.BuildInfoTable.JenkinsInfo.m_oBuildToken);
			
		// 매개 변수를 설정한다 {
		string oSource = string.Format(KCEditorDefine.B_JENKINS_SOURCE_FORMAT, 
			CPlatformBuildOption.BuildInfoTable.JenkinsInfo.m_oSourceRoot, a_oProjName);

		string oProjPath = string.Format(KCEditorDefine.B_JENKINS_PROJ_PATH_FORMAT, 
			CPlatformBuildOption.BuildInfoTable.JenkinsInfo.m_oWorkspaceRoot, oSource, CPlatformBuildOption.ProjInfoTable.ProjName);
			
		string oAnalytics = string.Format(KCEditorDefine.B_JENKINS_ANALYTICS_FORMAT,
			CPlatformBuildOption.BuildInfoTable.JenkinsInfo.m_oSourceRoot);
			
		var oDataList = a_oDataList ?? new Dictionary<string, string>();
		oDataList.ExAddValue(KCEditorDefine.B_JENKINS_KEY_SOURCE, oSource);
		oDataList.ExAddValue(KCEditorDefine.B_JENKINS_KEY_PROJ_PATH, oProjPath);
		oDataList.ExAddValue(KCEditorDefine.B_JENKINS_KEY_BRANCH, CPlatformBuildOption.BuildInfoTable.JenkinsInfo.m_oBranch);
		oDataList.ExAddValue(KCEditorDefine.B_JENKINS_KEY_DISTRIBUTION_PATH, CPlatformBuildOption.BuildInfoTable.JenkinsInfo.m_oDistributionPath);
		oDataList.ExAddValue(KCEditorDefine.B_JENKINS_KEY_PROJ_NAME, CPlatformBuildOption.ProjInfoTable.ProjName);
		oDataList.ExAddValue(KCEditorDefine.B_JENKINS_KEY_ANALYTICS, oAnalytics);
		oDataList.ExAddValue(KCEditorDefine.B_JENKINS_KEY_BUILD_MODE, a_oBuildMode);
		oDataList.ExAddValue(KCEditorDefine.B_JENKINS_KEY_BUILD_FUNC, a_oBuildFunc);
		oDataList.ExAddValue(KCEditorDefine.B_JENKINS_KEY_PIPELINE_NAME, a_oPipelineName);

		foreach(var stKeyValue in oDataList) {
			oStringBuilder.Append(KCEditorDefine.B_JENKINS_BUILD_PARAMETER_TOKEN);
			oStringBuilder.AppendFormat(KCEditorDefine.B_JENKINS_BUILD_DATA_FORMAT, stKeyValue.Key, stKeyValue.Value);
		}
		// 매개 변수를 설정한다 }

		CEditorFunc.ExecuteCmdline(oStringBuilder.ToString());
	}

	//! 독립 플랫폼 젠킨스 빌드를 실행한다
	public static void ExecuteStandalonePlatformJenkinsBuild(string a_oBuildMode, 
		string a_oBuildFunc, string a_oPipelineName, EStandalonePlatformType a_ePlatformType) {
		var oDataList = new Dictionary<string, string>() {
			[KCEditorDefine.B_JENKINS_KEY_PLATFORM] = CEditorAccess.GetStandalonePlatformName(a_ePlatformType)
		};

		CPlatformBuilder.ExecuteJenkinsBuild(KCEditorDefine.B_JENKINS_STANDALONE_PIPELINE, 
			KCEditorDefine.B_JENKINS_STANDALONE_BUILD_PROJ_NAME, a_oBuildMode, a_oBuildFunc, a_oPipelineName, oDataList);
	}

	//! iOS 플랫폼 젠킨스 빌드를 실행한다
	public static void ExecuteiOSPlatformJenkinsBuild(string a_oBuildMode, 
		string a_oBuildFunc, string a_oPipelineName, string a_oProfileID, string a_oIPAExportMethod) {
		CAccess.Assert(CPlatformBuildOption.ProjInfoTable != null);

		var oDataList = new Dictionary<string, string>() {
			[KCEditorDefine.B_JENKINS_KEY_BUNDLE_ID] = CPlatformBuildOption.ProjInfoTable.iOSProjInfo.m_oAppID,
			[KCEditorDefine.B_JENKINS_KEY_PROFILE_ID] = a_oProfileID,
			[KCEditorDefine.B_JENKINS_KEY_IPA_EXPORT_METHOD] = a_oIPAExportMethod
		};

		CPlatformBuilder.ExecuteJenkinsBuild(KCEditorDefine.B_JENKINS_IOS_PIPELINE, 
			KCEditorDefine.B_JENKINS_IOS_BUILD_PROJ_NAME, a_oBuildMode, a_oBuildFunc, a_oPipelineName, oDataList);
	}

	//! 안드로이드 플랫폼 젠킨스 빌드를 실행한다
	public static void ExecuteAndroidPlatformJenkinsBuild(string a_oBuildMode, 
		string a_oBuildFunc, string a_oPipelineName, string a_oBuildFileExtension, EAndroidPlatformType a_ePlatformType) {
		var oDataList = new Dictionary<string, string>() {
			[KCEditorDefine.B_JENKINS_KEY_PLATFORM] = CEditorAccess.GetAndroidPlatformName(a_ePlatformType),
			[KCEditorDefine.B_JENKINS_KEY_BUILD_FILE_EXTENSION] = a_oBuildFileExtension
		};

		CPlatformBuilder.ExecuteJenkinsBuild(KCEditorDefine.B_JENKINS_ANDROID_PIPELINE, 
			KCEditorDefine.B_JENKINS_ANDROID_BUILD_PROJ_NAME, a_oBuildMode, a_oBuildFunc, a_oPipelineName, oDataList);
	}
	#endregion			// 클래스 함수
}
#endif			// #if UNITY_EDITOR
