using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

//! 플랫폼 빌더 - 안드로이드
public static partial class CPlatformBuilder {
	#region 클래스 함수
	//! 구글 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/Google/Debug")]
	public static void BuildGooglePlatformDebug() {
		CPlatformBuilder.BuildAndroidPlatformDebug(EAndroidPlatformType.GOOGLE);
	}

	//! 원 스토어 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/OneStore/Debug")]
	public static void BuildOneStorePlatformDebug() {
		CPlatformBuilder.BuildAndroidPlatformDebug(EAndroidPlatformType.ONE_STORE);
	}

	//! 갤럭시 스토어 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/GalaxyStore/Debug")]
	public static void BuildGalaxyStorePlatformDebug() {
		CPlatformBuilder.BuildAndroidPlatformDebug(EAndroidPlatformType.GALAXY_STORE);
	}

	//! 구글 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/Google/Debug with AutoPlay")]
	public static void BuildGooglePlatformWithAutoPlayDebug() {
		CPlatformBuilder.BuildAndroidPlatformWithAutoPlayDebug(EAndroidPlatformType.GOOGLE);
	}

	//! 원 스토어 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/OneStore/Debug with AutoPlay")]
	public static void BuildOneStorePlatformWithAutoPlayDebug() {
		CPlatformBuilder.BuildAndroidPlatformWithAutoPlayDebug(EAndroidPlatformType.ONE_STORE);
	}

	//! 갤럭시 스토어 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/GalaxyStore/Debug with AutoPlay")]
	public static void BuildGalaxyStorePlatformWithAutoPlayDebug() {
		CPlatformBuilder.BuildAndroidPlatformWithAutoPlayDebug(EAndroidPlatformType.GALAXY_STORE);
	}

	//! 구글 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/Google/Release")]
	public static void BuildGooglePlatformRelease() {
		CPlatformBuilder.BuildAndroidPlatformRelease(EAndroidPlatformType.GOOGLE);
	}

	//! 원 스토어 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/OneStore/Release")]
	public static void BuildOneStorePlatformRelease() {
		CPlatformBuilder.BuildAndroidPlatformRelease(EAndroidPlatformType.ONE_STORE);
	}

	//! 갤럭시 스토어를 빌드한다
	[MenuItem("Utility/Build/Local/Android/GalaxyStore/Release")]
	public static void BuildGalaxyStorePlatformRelease() {
		CPlatformBuilder.BuildAndroidPlatformRelease(EAndroidPlatformType.GALAXY_STORE);
	}

	//! 구글 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/Google/Release with AutoPlay")]
	public static void BuildGooglePlatformWithAutoPlayRelease() {
		CPlatformBuilder.BuildAndroidPlatformWithAutoPlayRelease(EAndroidPlatformType.GOOGLE);
	}

	//! 원 스토어 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/OneStore/Release with AutoPlay")]
	public static void BuildOneStorePlatformWithAutoPlayRelease() {
		CPlatformBuilder.BuildAndroidPlatformWithAutoPlayRelease(EAndroidPlatformType.ONE_STORE);
	}

	//! 갤럭시 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/GalaxyStore/Release with AutoPlay")]
	public static void BuildGalaxyStorePlatformWithAutoPlayRelease() {
		CPlatformBuilder.BuildAndroidPlatformWithAutoPlayRelease(EAndroidPlatformType.GALAXY_STORE);
	}

	//! 구글 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/Google/Release with AutoPlay (Disable FPS)")]
	public static void BuildGooglePlatformWithAutoPlayDisableFPSRelease() {
		CPlatformBuilder.BuildAndroidPlatformWithAutoPlayDisableFPSRelease(EAndroidPlatformType.GOOGLE);
	}

	//! 원 스토어 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/OneStore/Release with AutoPlay (Disable FPS)")]
	public static void BuildOneStorePlatformWithAutoPlayDisableFPSRelease() {
		CPlatformBuilder.BuildAndroidPlatformWithAutoPlayDisableFPSRelease(EAndroidPlatformType.ONE_STORE);
	}

	//! 갤럭시 스토어 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/GalaxyStore/Release with AutoPlay (Disable FPS)")]
	public static void BuildGalaxyStorePlatformWithAutoPlayDisableFPSRelease() {
		CPlatformBuilder.BuildAndroidPlatformWithAutoPlayDisableFPSRelease(EAndroidPlatformType.GALAXY_STORE);
	}

	//! 구글 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/Google/Distribution (Adhoc)")]
	public static void BuildGooglePlatformAdhoc() {
		CPlatformBuilder.BuildAndroidPlatformAdhoc(EAndroidPlatformType.GOOGLE);
	}

	//! 원 스토어 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/OneStore/Distribution (Adhoc)")]
	public static void BuildOneStorePlatformAdhoc() {
		CPlatformBuilder.BuildAndroidPlatformAdhoc(EAndroidPlatformType.ONE_STORE);
	}

	//! 갤럭시 스토어 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/GalaxyStore/Distribution (Adhoc)")]
	public static void BuildGalaxyStorePlatformAdhoc() {
		CPlatformBuilder.BuildAndroidPlatformAdhoc(EAndroidPlatformType.GALAXY_STORE);
	}

	//! 구글 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/Google/Distribution (Adhoc with Robo Test)")]
	public static void BuildGooglePlatformWithRoboTestAdhoc() {
		CPlatformBuilder.BuildAndroidPlatformWithRoboTestAdhoc(EAndroidPlatformType.GOOGLE);
	}

	//! 원 스토어 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/OneStore/Distribution (Adhoc with Robo Test)")]
	public static void BuildOneStorePlatformWithRoboTestAdhoc() {
		CPlatformBuilder.BuildAndroidPlatformWithRoboTestAdhoc(EAndroidPlatformType.ONE_STORE);
	}

	//! 갤럭시 스토어 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/GalaxyStore/Distribution (Adhoc with Robo Test)")]
	public static void BuildGalaxyStorePlatformWithRoboTestAdhoc() {
		CPlatformBuilder.BuildAndroidPlatformWithRoboTestAdhoc(EAndroidPlatformType.GALAXY_STORE);
	}

	//! 구글 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/Google/Distribution (Store)")]
	public static void BuildGooglePlatformStore() {
		CPlatformBuilder.BuildAndroidPlatformStore(EAndroidPlatformType.GOOGLE);
	}

	//! 원 스토어 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/OneStore/Distribution (Store)")]
	public static void BuildOneStorePlatformStore() {
		CPlatformBuilder.BuildAndroidPlatformStore(EAndroidPlatformType.ONE_STORE);
	}

	//! 갤럭시 스토어 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/GalaxyStore/Distribution (Store)")]
	public static void BuildGalaxyStorePlatformStore() {
		CPlatformBuilder.BuildAndroidPlatformStore(EAndroidPlatformType.GALAXY_STORE);
	}

	//! 구글 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/Android/Google/Debug")]
	public static void RemoteBuildGooglePlatformDebug() {
		CPlatformBuilder.RemoteBuildAndroidPlatformDebug(EAndroidPlatformType.GOOGLE);
	}

	//! 원 스토어 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/Android/OneStore/Debug")]
	public static void RemoteBuildOneStorePlatformDebug() {
		CPlatformBuilder.RemoteBuildAndroidPlatformDebug(EAndroidPlatformType.ONE_STORE);
	}

	//! 갤럭시 스토어 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/Android/GalaxyStore/Debug")]
	public static void RemoteBuildGalaxyStorePlatformDebug() {
		CPlatformBuilder.RemoteBuildAndroidPlatformDebug(EAndroidPlatformType.GALAXY_STORE);
	}

	//! 구글 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/Android/Google/Release")]
	public static void RemoteBuildGooglePlatformRelease() {
		CPlatformBuilder.RemoteBuildAndroidPlatformRelease(EAndroidPlatformType.GOOGLE);
	}

	//! 원 스토어 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/Android/OneStore/Release")]
	public static void RemoteBuildOneStorePlatformRelease() {
		CPlatformBuilder.RemoteBuildAndroidPlatformRelease(EAndroidPlatformType.ONE_STORE);
	}

	//! 갤럭시 스토어 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/Android/GalaxyStore/Release")]
	public static void RemoteBuildGalaxyStorePlatformRelease() {
		CPlatformBuilder.RemoteBuildAndroidPlatformRelease(EAndroidPlatformType.GALAXY_STORE);
	}

	//! 구글 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/Android/Google/Distribution (Adhoc)")]
	public static void RemoteBuildGooglePlatformAdhoc() {
		CPlatformBuilder.RemoteBuildAndroidPlatformAdhoc(EAndroidPlatformType.GOOGLE);
	}

	//! 원 스토어 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/Android/OneStore/Distribution (Adhoc)")]
	public static void RemoteBuildOneStorePlatformAdhoc() {
		CPlatformBuilder.RemoteBuildAndroidPlatformAdhoc(EAndroidPlatformType.ONE_STORE);
	}

	//! 갤럭시 스토어 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/Android/GalaxyStore/Distribution (Adhoc)")]
	public static void RemoteBuildGalaxyStorePlatformAdhoc() {
		CPlatformBuilder.RemoteBuildAndroidPlatformAdhoc(EAndroidPlatformType.GALAXY_STORE);
	}

	//! 구글 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/Android/Google/Distribution (Store)")]
	public static void RemoteBuildGooglePlatformStore() {
		CPlatformBuilder.RemoteBuildAndroidPlatformStore(EAndroidPlatformType.GOOGLE);
	}

	//! 원 스토어 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/Android/OneStore/Distribution (Store)")]
	public static void RemoteBuildOneStorePlatformStore() {
		CPlatformBuilder.RemoteBuildAndroidPlatformStore(EAndroidPlatformType.ONE_STORE);
	}

	//! 갤럭시 스토어 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/Android/GalaxyStore/Distribution (Store)")]
	public static void RemoteBuildGalaxyStorePlatformStore() {
		CPlatformBuilder.RemoteBuildAndroidPlatformStore(EAndroidPlatformType.GALAXY_STORE);
	}

	//! 안드로이드 플랫폼을 빌드한다
	private static void BuildAndroidPlatformDebug(EAndroidPlatformType a_ePlatformType) {
		// 전처리기 심볼을 추가한다
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.Android, KEditorDefine.DS_DEFINE_SYMBOL_ADS_TEST_ENABLE);
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.Android, KEditorDefine.DS_DEFINE_SYMBOL_LOGIC_TEST_ENABLE);

		// 빌드 옵션을 설정한다 {
		var oPlayerOptions = new BuildPlayerOptions();
		oPlayerOptions.options = BuildOptions.Development;
		
		EditorUserBuildSettings.buildAppBundle = false;
		// 빌드 옵션을 설정한다 }

		CPlatformBuilder.BuildAndroidPlatform(oPlayerOptions, a_ePlatformType);
	}

	//! 안드로이드 플랫폼을 빌드한다
	private static void BuildAndroidPlatformWithAutoPlayDebug(EAndroidPlatformType a_ePlatformType) {
		CPlatformBuilder.IsAutoPlay = true;
		CPlatformBuilder.BuildAndroidPlatformDebug(a_ePlatformType);
	}

	//! 안드로이드 플랫폼을 빌드한다
	private static void BuildAndroidPlatformRelease(EAndroidPlatformType a_ePlatformType) {
		// 전처리기 심볼을 추가한다 {
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.Android, KEditorDefine.DS_DEFINE_SYMBOL_ADS_TEST_ENABLE);

		if(CPlatformBuilder.IsAutoPlay) {
			CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.Android, KEditorDefine.DS_DEFINE_SYMBOL_LOGIC_TEST_ENABLE);
		}
		// 전처리기 심볼을 추가한다 }

		EditorUserBuildSettings.buildAppBundle = false;
		CPlatformBuilder.BuildAndroidPlatform(new BuildPlayerOptions(), a_ePlatformType);
	}

	//! 안드로이드 플랫폼을 빌드한다
	private static void BuildAndroidPlatformWithAutoPlayRelease(EAndroidPlatformType a_ePlatformType) {
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.Android, KEditorDefine.DS_DEFINE_SYMBOL_FPS_ENABLE);

		CPlatformBuilder.IsAutoPlay = true;
		CPlatformBuilder.BuildAndroidPlatformRelease(a_ePlatformType);
	}

	//! 안드로이드 플랫폼을 빌드한다
	private static void BuildAndroidPlatformWithAutoPlayDisableFPSRelease(EAndroidPlatformType a_ePlatformType) {
		CPlatformBuilder.IsAutoPlay = true;
		CPlatformBuilder.BuildAndroidPlatformRelease(a_ePlatformType);
	}

	//! 안드로이드 플랫폼을 빌드한다
	private static void BuildAndroidPlatformAdhoc(EAndroidPlatformType a_ePlatformType) {
		// 전처리기 심볼을 추가한다
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.Android, KEditorDefine.DS_DEFINE_SYMBOL_ADS_TEST_ENABLE);
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.Android, KEditorDefine.DS_DEFINE_SYMBOL_ADHOC_BUILD);

		EditorUserBuildSettings.buildAppBundle = false;
		CPlatformBuilder.BuildAndroidPlatform(new BuildPlayerOptions(), a_ePlatformType);
	}

	//! 안드로이드 플랫폼을 빌드한다
	private static void BuildAndroidPlatformWithRoboTestAdhoc(EAndroidPlatformType a_ePlatformType) {
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.Android, KEditorDefine.DS_DEFINE_SYMBOL_ROBO_TEST_ENABLE);
		CPlatformBuilder.BuildAndroidPlatformAdhoc(a_ePlatformType);
	}

	//! 안드로이드 플랫폼을 빌드한다
	private static void BuildAndroidPlatformStore(EAndroidPlatformType a_ePlatformType) {
		CPlatformBuilder.IsDistributionBuild = true;
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.Android, KEditorDefine.DS_DEFINE_SYMBOL_STORE_BUILD);

		EditorUserBuildSettings.buildAppBundle = true;
		CPlatformBuilder.BuildAndroidPlatform(new BuildPlayerOptions(), a_ePlatformType);
	}

	//! 안드로이드 플랫폼을 빌드한다
	private static void BuildAndroidPlatform(BuildPlayerOptions a_oPlayerOptions, EAndroidPlatformType a_ePlatformType) {
		CPlatformBuilder.AndroidPlatformType = a_ePlatformType;
		CPlatformBuilder.IsEnableEditorScene = false;

		// 플러그인 파일을 복사한다
		if(!Application.isBatchMode) {
			Func.CopyFile(KEditorDefine.B_ANDROID_SRC_PLUGIN_PATH, KEditorDefine.B_ANDROID_DEST_PLUGIN_PATH, false);
		}

		// 빌드 옵션을 설정한다 {
		string oPlatform = EditorFunc.GetAndroidPlatformName(a_ePlatformType);
		string oFilename = string.Format(KEditorDefine.B_ANDROID_BUILD_FILENAME_FORMAT, oPlatform);

		string oBuildFileExtension = EditorUserBuildSettings.buildAppBundle ? KEditorDefine.B_ANDROID_AAB_BUILD_FILE_EXTENSION
			: KEditorDefine.B_ANDROID_APK_BUILD_FILE_EXTENSION;

		a_oPlayerOptions.target = BuildTarget.Android;
		a_oPlayerOptions.targetGroup = BuildTargetGroup.Android;
		a_oPlayerOptions.locationPathName = string.Format(KEditorDefine.B_ANDROID_BUILD_PATH_FORMAT, oPlatform, oFilename, oBuildFileExtension);

		if(CPlatformBuilder.ProjectInfoTable != null) {
			if(a_ePlatformType == EAndroidPlatformType.ONE_STORE) {
				PlayerSettings.bundleVersion = CPlatformBuilder.ProjectInfoTable.OneStoreProjectInfo.m_oBuildVersion;
			} else if(a_ePlatformType == EAndroidPlatformType.GALAXY_STORE) {
				PlayerSettings.bundleVersion = CPlatformBuilder.ProjectInfoTable.GalaxyStoreProjectInfo.m_oBuildVersion;
			} else {
				PlayerSettings.bundleVersion = CPlatformBuilder.ProjectInfoTable.GoogleProjectInfo.m_oBuildVersion;
			}
		}
		// 빌드 옵션을 설정한다 }

		// 빌드 디렉토리를 생성한다
		string oBuildPath = string.Format(KEditorDefine.B_ANDROID_ABSOLUTE_BUILD_PATH_FORMAT, oPlatform);
		Func.CreateDirectory(oBuildPath);

		// 플랫폼을 빌드한다
		CPlatformBuilder.BuildPlatform(a_oPlayerOptions);
	}

	//! 안드로이드 플랫폼을 원격 빌드한다
	private static void RemoteBuildAndroidPlatformDebug(EAndroidPlatformType a_ePlatformType) {
		EditorFunc.ExecuteAndroidPlatformJenkinsBuild(KDefine.B_BUILD_MODE_DEBUG, 
			KEditorDefine.B_JENKINS_DEBUG_BUILD_FUNC, KEditorDefine.B_JENKINS_ANDROID_DEBUG_PIPELINE_NAME, KEditorDefine.B_ANDROID_APK_BUILD_FILE_EXTENSION, a_ePlatformType);
	}

	//! 안드로이드 플랫폼을 원격 빌드한다
	private static void RemoteBuildAndroidPlatformRelease(EAndroidPlatformType a_ePlatformType) {
		EditorFunc.ExecuteAndroidPlatformJenkinsBuild(KDefine.B_BUILD_MODE_RELEASE, 
			KEditorDefine.B_JENKINS_RELEASE_BUILD_FUNC, KEditorDefine.B_JENKINS_ANDROID_RELEASE_PIPELINE_NAME, KEditorDefine.B_ANDROID_APK_BUILD_FILE_EXTENSION, a_ePlatformType);
	}

	//! 안드로이드 플랫폼을 원격 빌드한다
	private static void RemoteBuildAndroidPlatformAdhoc(EAndroidPlatformType a_ePlatformType) {
		EditorFunc.ExecuteAndroidPlatformJenkinsBuild(KDefine.B_BUILD_MODE_RELEASE, 
			KEditorDefine.B_JENKINS_ADHOC_BUILD_FUNC, KEditorDefine.B_JENKINS_ANDROID_ADHOC_PIPELINE_NAME, KEditorDefine.B_ANDROID_APK_BUILD_FILE_EXTENSION, a_ePlatformType);
	}

	//! 안드로이드 플랫폼을 원격 빌드한다
	private static void RemoteBuildAndroidPlatformStore(EAndroidPlatformType a_ePlatformType) {
		EditorFunc.ExecuteAndroidPlatformJenkinsBuild(KDefine.B_BUILD_MODE_RELEASE, 
			KEditorDefine.B_JENKINS_STORE_BUILD_FUNC, KEditorDefine.B_JENKINS_ANDROID_STORE_PIPELINE_NAME, KEditorDefine.B_ANDROID_AAB_BUILD_FILE_EXTENSION, a_ePlatformType);
	}

	//! 안드로이드 빌드가 완료 되었을 경우
	private static void OnPostProcessAndroidBuild(BuildTarget a_eTarget, string a_oPath) {
#if UNITY_ANDROID

#endif			// #if UNITY_ANDROID
	}
	#endregion			// 클래스 함수
}
