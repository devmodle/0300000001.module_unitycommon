using System.Collections;
using System.Collections.Generic;
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
	public static void BuildGooglePlatformAdhocDistribution() {
		CPlatformBuilder.BuildAndroidPlatformAdhocDistribution(EAndroidPlatformType.GOOGLE);
	}

	//! 원 스토어 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/OneStore/Distribution (Adhoc)")]
	public static void BuildOneStorePlatformAdhocDistribution() {
		CPlatformBuilder.BuildAndroidPlatformAdhocDistribution(EAndroidPlatformType.ONE_STORE);
	}

	//! 갤럭시 스토어 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/GalaxyStore/Distribution (Adhoc)")]
	public static void BuildGalaxyStorePlatformAdhocDistribution() {
		CPlatformBuilder.BuildAndroidPlatformAdhocDistribution(EAndroidPlatformType.GALAXY_STORE);
	}

	//! 구글 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/Google/Distribution (Adhoc with Robo Test)")]
	public static void BuildGooglePlatformWithRoboTestAdhocDistribution() {
		CPlatformBuilder.BuildAndroidPlatformWithRoboTestAdhocDistribution(EAndroidPlatformType.GOOGLE);
	}

	//! 원 스토어 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/OneStore/Distribution (Adhoc with Robo Test)")]
	public static void BuildOneStorePlatformWithRoboTestAdhocDistribution() {
		CPlatformBuilder.BuildAndroidPlatformWithRoboTestAdhocDistribution(EAndroidPlatformType.ONE_STORE);
	}

	//! 갤럭시 스토어 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/GalaxyStore/Distribution (Adhoc with Robo Test)")]
	public static void BuildGalaxyStorePlatformWithRoboTestAdhocDistribution() {
		CPlatformBuilder.BuildAndroidPlatformWithRoboTestAdhocDistribution(EAndroidPlatformType.GALAXY_STORE);
	}

	//! 구글 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/Google/Distribution (Store)")]
	public static void BuildGooglePlatformStoreDistribution() {
		CPlatformBuilder.BuildAndroidPlatformStoreDistribution(EAndroidPlatformType.GOOGLE);
	}

	//! 원 스토어 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/OneStore/Distribution (Store)")]
	public static void BuildOneStorePlatformStoreDistribution() {
		CPlatformBuilder.BuildAndroidPlatformStoreDistribution(EAndroidPlatformType.ONE_STORE);
	}

	//! 갤럭시 스토어 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Android/GalaxyStore/Distribution (Store)")]
	public static void BuildGalaxyStorePlatformStoreDistribution() {
		CPlatformBuilder.BuildAndroidPlatformStoreDistribution(EAndroidPlatformType.GALAXY_STORE);
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
	public static void RemoteBuildAndroidPlatformAdhocDistribution() {
		CPlatformBuilder.RemoteBuildAndroidPlatformAdhocDistribution(EAndroidPlatformType.GOOGLE);
	}

	//! 원 스토어 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/Android/OneStore/Distribution (Adhoc)")]
	public static void RemoteBuildOneStorePlatformAdhocDistribution() {
		CPlatformBuilder.RemoteBuildAndroidPlatformAdhocDistribution(EAndroidPlatformType.ONE_STORE);
	}

	//! 갤럭시 스토어 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/Android/GalaxyStore/Distribution (Adhoc)")]
	public static void RemoteBuildGalaxyStorePlatformAdhocDistribution() {
		CPlatformBuilder.RemoteBuildAndroidPlatformAdhocDistribution(EAndroidPlatformType.GALAXY_STORE);
	}

	//! 구글 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/Android/Google/Distribution (Store)")]
	public static void RemoteBuildAndroidPlatformStoreDistribution() {
		CPlatformBuilder.RemoteBuildAndroidPlatformStoreDistribution(EAndroidPlatformType.GOOGLE);
	}

	//! 원 스토어 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/Android/OneStore/Distribution (Store)")]
	public static void RemoteBuildOneStorePlatformStoreDistribution() {
		CPlatformBuilder.RemoteBuildAndroidPlatformStoreDistribution(EAndroidPlatformType.ONE_STORE);
	}

	//! 갤럭시 스토어 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/Android/GalaxyStore/Distribution (Store)")]
	public static void RemoteBuildGalaxyStorePlatformStoreDistribution() {
		CPlatformBuilder.RemoteBuildAndroidPlatformStoreDistribution(EAndroidPlatformType.GALAXY_STORE);
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
	private static void BuildAndroidPlatformAdhocDistribution(EAndroidPlatformType a_ePlatformType) {
		// 전처리기 심볼을 추가한다
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.Android, KEditorDefine.DS_DEFINE_SYMBOL_ADS_TEST_ENABLE);
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.Android, KEditorDefine.DS_DEFINE_SYMBOL_ADHOC_DISTRIBUTION_BUILD);

		EditorUserBuildSettings.buildAppBundle = false;
		CPlatformBuilder.BuildAndroidPlatform(new BuildPlayerOptions(), a_ePlatformType);
	}

	//! 안드로이드 플랫폼을 빌드한다
	private static void BuildAndroidPlatformWithRoboTestAdhocDistribution(EAndroidPlatformType a_ePlatformType) {
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.Android, KEditorDefine.DS_DEFINE_SYMBOL_ROBO_TEST_ENABLE);
		CPlatformBuilder.BuildAndroidPlatformAdhocDistribution(a_ePlatformType);
	}

	//! 안드로이드 플랫폼을 빌드한다
	private static void BuildAndroidPlatformStoreDistribution(EAndroidPlatformType a_ePlatformType) {
		CPlatformBuilder.IsDistributionBuild = true;
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.Android, KEditorDefine.DS_DEFINE_SYMBOL_STORE_DISTRIBUTION_BUILD);

		EditorUserBuildSettings.buildAppBundle = true;
		CPlatformBuilder.BuildAndroidPlatform(new BuildPlayerOptions(), a_ePlatformType);
	}

	//! 안드로이드 플랫폼을 빌드한다
	private static void BuildAndroidPlatform(BuildPlayerOptions a_oPlayerOptions, EAndroidPlatformType a_ePlatformType) {
		CPlatformBuilder.AndroidPlatformType = a_ePlatformType;
		CPlatformBuilder.IsEnableEditorScene = false;

		// 플러그인 파일을 복사한다
		if(!Application.isBatchMode) {
			Function.CopyFile(KEditorDefine.B_ANDROID_SRC_PLUGIN_PATH, KEditorDefine.B_ANDROID_DEST_PLUGIN_PATH, false);
		}

		// 빌드 옵션을 설정한다 {
		a_oPlayerOptions.target = BuildTarget.Android;
		a_oPlayerOptions.targetGroup = BuildTargetGroup.Android;

		string oPlatform = EditorFunction.GetAndroidPlatformName(a_ePlatformType);
		string oFilename = string.Format(KEditorDefine.B_ANDROID_BUILD_FILENAME_FORMAT, oPlatform);

		if(EditorUserBuildSettings.buildAppBundle) {
			a_oPlayerOptions.locationPathName = string.Format(KEditorDefine.B_ANDROID_AAB_BUILD_PATH_FORMAT, oPlatform, oFilename);
		} else {
			a_oPlayerOptions.locationPathName = string.Format(KEditorDefine.B_ANDROID_APK_BUILD_PATH_FORMAT, oPlatform, oFilename);
		}

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

		// 플랫폼을 빌드한다
		CPlatformBuilder.BuildPlatform(a_oPlayerOptions);
	}

	//! 안드로이드 플랫폼을 원격 빌드한다
	private static void RemoteBuildAndroidPlatformDebug(EAndroidPlatformType a_ePlatformType) {
		EditorFunction.ExecuteAndroidPlatformJenkinsBuild(KEditorDefine.B_JENKINS_ANDROID_DEBUG_PIPELINE, a_ePlatformType);
	}

	//! 안드로이드 플랫폼을 원격 빌드한다
	private static void RemoteBuildAndroidPlatformRelease(EAndroidPlatformType a_ePlatformType) {
		EditorFunction.ExecuteAndroidPlatformJenkinsBuild(KEditorDefine.B_JENKINS_ANDROID_RELEASE_PIPELINE, a_ePlatformType);
	}

	//! 안드로이드 플랫폼을 원격 빌드한다
	private static void RemoteBuildAndroidPlatformAdhocDistribution(EAndroidPlatformType a_ePlatformType) {
		EditorFunction.ExecuteAndroidPlatformJenkinsBuild(KEditorDefine.B_JENKINS_ANDROID_ADHOC_DISTRIBUTION_PIPELINE, a_ePlatformType);
	}

	//! 안드로이드 플랫폼을 원격 빌드한다
	private static void RemoteBuildAndroidPlatformStoreDistribution(EAndroidPlatformType a_ePlatformType) {
		EditorFunction.ExecuteAndroidPlatformJenkinsBuild(KEditorDefine.B_JENKINS_ANDROID_STORE_DISTRIBUTION_PIPELINE, a_ePlatformType);
	}
	#endregion			// 클래스 함수
}
