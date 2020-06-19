using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//! 플랫폼 빌더 - iOS
public static partial class CPlatformBuilder {
	#region 클래스 함수
	//! iOS 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/iOS/Device/Debug")]
	public static void BuildiOSDevicePlatformDebug() {
		// 전처리기 심볼을 추가한다
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.iOS, KEditorDefine.DS_DEFINE_SYMBOL_ADS_TEST_ENABLE);
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.iOS, KEditorDefine.DS_DEFINE_SYMBOL_LOGIC_TEST_ENABLE);

		// 프로비저닝 파일 정보를 설정한다
		PlayerSettings.iOS.iOSManualProvisioningProfileID = CPlatformBuilder.BuildInfoTable.iOSBuildInfo.m_oDevProfileID;
		PlayerSettings.iOS.iOSManualProvisioningProfileType = ProvisioningProfileType.Development;

		// 빌드 옵션을 설정한다 {
		var oPlayerOptions = new BuildPlayerOptions();
		oPlayerOptions.options = BuildOptions.Development;
		
		EditorUserBuildSettings.iOSBuildConfigType = iOSBuildType.Debug;
		// 빌드 옵션을 설정한다 }

		CPlatformBuilder.BuildiOSPlatform(oPlayerOptions);
	}

	//! iOS 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/iOS/Device/Debug with AutoPlay")]
	public static void BuildiOSDevicePlatformWithAutoPlayDebug() {
		CPlatformBuilder.IsAutoPlay = true;
		CPlatformBuilder.BuildiOSDevicePlatformDebug();
	}

	//! iOS 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/iOS/Device/Release")]
	public static void BuildiOSDevicePlatformRelease() {
		// 전처리기 심볼을 추가한다 {
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.iOS, KEditorDefine.DS_DEFINE_SYMBOL_ADS_TEST_ENABLE);

		if(CPlatformBuilder.IsAutoPlay) {
			CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.iOS, KEditorDefine.DS_DEFINE_SYMBOL_LOGIC_TEST_ENABLE);
		}
		// 전처리기 심볼을 추가한다 }

		// 프로비저닝 파일 정보를 설정한다
		PlayerSettings.iOS.iOSManualProvisioningProfileID = CPlatformBuilder.BuildInfoTable.iOSBuildInfo.m_oDevProfileID;
		PlayerSettings.iOS.iOSManualProvisioningProfileType = ProvisioningProfileType.Development;

		// 빌드 옵션을 설정한다
		var oPlayerOptions = new BuildPlayerOptions();
		EditorUserBuildSettings.iOSBuildConfigType = iOSBuildType.Release;

		CPlatformBuilder.BuildiOSPlatform(oPlayerOptions);
	}

	//! iOS 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/iOS/Device/Release with AutoPlay")]
	public static void BuildiOSDevicePlatformWithAutoPlayRelease() {
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.iOS, KEditorDefine.DS_DEFINE_SYMBOL_FPS_ENABLE);

		CPlatformBuilder.IsAutoPlay = true;
		CPlatformBuilder.BuildiOSDevicePlatformRelease();
	}

	//! iOS 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/iOS/Device/Release with AutoPlay (Disable FPS)")]
	public static void BuildiOSDevicePlatformWithAutoPlayDisableFPSRelease() {
		CPlatformBuilder.IsAutoPlay = true;
		CPlatformBuilder.BuildiOSDevicePlatformRelease();
	}

	//! iOS 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/iOS/Device/Distribution (Adhoc)")]
	public static void BuildiOSDevicePlatformAdhocDistribution() {
		// 전처리기 심볼을 추가한다
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.iOS, KEditorDefine.DS_DEFINE_SYMBOL_ADS_TEST_ENABLE);
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.iOS, KEditorDefine.DS_DEFINE_SYMBOL_ADHOC_DISTRIBUTION_BUILD);

		// 프로비저닝 파일 정보를 설정한다
		PlayerSettings.iOS.iOSManualProvisioningProfileID = CPlatformBuilder.BuildInfoTable.iOSBuildInfo.m_oAdhocProfileID;
		PlayerSettings.iOS.iOSManualProvisioningProfileType = ProvisioningProfileType.Distribution;

		EditorUserBuildSettings.iOSBuildConfigType = iOSBuildType.Release;
		CPlatformBuilder.BuildiOSPlatform(new BuildPlayerOptions());
	}

	//! iOS 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/iOS/Device/Distribution (Adhoc with Robo Test)")]
	public static void BuildiOSDevicePlatformWithRoboTestAdhocDistribution() {
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.iOS, KEditorDefine.DS_DEFINE_SYMBOL_ROBO_TEST_ENABLE);
		CPlatformBuilder.BuildiOSDevicePlatformAdhocDistribution();
	}

	//! iOS 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/iOS/Device/Distribution (Store)")]
	public static void BuildiOSDevicePlatformStoreDistribution() {
		CPlatformBuilder.IsDistributionBuild = true;
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.iOS, KEditorDefine.DS_DEFINE_SYMBOL_STORE_DISTRIBUTION_BUILD);

		// 프로비저닝 파일 정보를 설정한다
		PlayerSettings.iOS.iOSManualProvisioningProfileID = CPlatformBuilder.BuildInfoTable.iOSBuildInfo.m_oStoreProfileID;
		PlayerSettings.iOS.iOSManualProvisioningProfileType = ProvisioningProfileType.Distribution;

		EditorUserBuildSettings.iOSBuildConfigType = iOSBuildType.Release;
		CPlatformBuilder.BuildiOSPlatform(new BuildPlayerOptions());
	}

	//! iOS 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/iOS/Simulator/Debug")]
	public static void BuildiOSSimulatorPlatformDebug() {
		// 전처리기 심볼을 추가한다
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.iOS, KEditorDefine.DS_DEFINE_SYMBOL_ADS_TEST_ENABLE);
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.iOS, KEditorDefine.DS_DEFINE_SYMBOL_LOGIC_TEST_ENABLE);

		// 그래픽 API 를 설정한다
		PlayerSettings.iOS.sdkVersion = iOSSdkVersion.SimulatorSDK;
		EditorFunction.SetGraphicAPI(BuildTarget.iOS, KEditorDefine.B_IOS_SIMULATOR_GRAPHICS_DEVICE_TYPES);

		CPlatformBuilder.BuildiOSDevicePlatformDebug();
	}

	//! iOS 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/iOS/Simulator/Debug with AutoPlay")]
	public static void BuildiOSSimulatorPlatformWithAutoPlayDebug() {
		CPlatformBuilder.IsAutoPlay = true;
		CPlatformBuilder.BuildiOSSimulatorPlatformDebug();
	}

	//! iOS 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/iOS/Simulator/Release")]
	public static void BuildiOSSimulatorPlatformRelease() {
		// 전처리기 심볼을 추가한다
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.iOS, KEditorDefine.DS_DEFINE_SYMBOL_ADS_TEST_ENABLE);
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.iOS, KEditorDefine.DS_DEFINE_SYMBOL_LOGIC_TEST_ENABLE);

		// 그래픽 API 를 설정한다
		PlayerSettings.iOS.sdkVersion = iOSSdkVersion.SimulatorSDK;
		EditorFunction.SetGraphicAPI(BuildTarget.iOS, KEditorDefine.B_IOS_SIMULATOR_GRAPHICS_DEVICE_TYPES);

		CPlatformBuilder.BuildiOSDevicePlatformRelease();
	}

	//! iOS 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/iOS/Simulator/Release with AutoPlay")]
	public static void BuildiOSSimulatorPlatformWithAutoPlayRelease() {
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.iOS, KEditorDefine.DS_DEFINE_SYMBOL_FPS_ENABLE);

		CPlatformBuilder.IsAutoPlay = true;
		CPlatformBuilder.BuildiOSSimulatorPlatformRelease();
	}

	//! iOS 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/iOS/Simulator/Release with AutoPlay (Disable FPS)")]
	public static void BuildiOSSimulatorPlatformWithAutoPlayDisableFPRelease() {
		CPlatformBuilder.IsAutoPlay = true;
		CPlatformBuilder.BuildiOSSimulatorPlatformRelease();
	}

	//! iOS 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/iOS/Debug")]
	public static void RemoteBuildiOSPlatformDebug() {
		EditorFunction.ExecuteiOSPlatformJenkinsBuild(KEditorDefine.B_JENKINS_IOS_DEBUG_PIPELINE,
			CPlatformBuilder.BuildInfoTable.iOSBuildInfo.m_oDevProfileID);
	}

	//! iOS 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/iOS/Release")]
	public static void RemoteBuildiOSPlatformRelease() {
		EditorFunction.ExecuteiOSPlatformJenkinsBuild(KEditorDefine.B_JENKINS_IOS_RELEASE_PIPELINE,
			CPlatformBuilder.BuildInfoTable.iOSBuildInfo.m_oDevProfileID);
	}

	//! iOS 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/iOS/Distribution (Adhoc)")]
	public static void RemoteBuildiOSPlatformAdhocDistribution() {
		EditorFunction.ExecuteiOSPlatformJenkinsBuild(KEditorDefine.B_JENKINS_IOS_ADHOC_DISTRIBUTION_PIPELINE,
			CPlatformBuilder.BuildInfoTable.iOSBuildInfo.m_oAdhocProfileID);
	}

	//! iOS 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/iOS/Distribution (Store)")]
	public static void RemoteBuildiOSPlatformStoreDistribution() {
		EditorFunction.ExecuteiOSPlatformJenkinsBuild(KEditorDefine.B_JENKINS_IOS_STORE_DISTRIBUTION_PIPELINE,
			CPlatformBuilder.BuildInfoTable.iOSBuildInfo.m_oStoreProfileID);
	}

	//! iOS 플랫폼을 빌드한다
	private static void BuildiOSPlatform(BuildPlayerOptions a_oPlayerOptions) {
		CPlatformBuilder.IsEnableEditorScene = false;

		// 플러그인 파일을 복사한다 {
		Function.CopyFile(KEditorDefine.B_IOS_SRC_MONO_MODULES_REGISTER_PATH, KEditorDefine.B_IOS_DEST_MONO_MODULES_REGISTER_PATH);

		if(!Application.isBatchMode) {
			Function.CopyDirectory(KEditorDefine.B_IOS_SRC_PLUGIN_PATH, KEditorDefine.B_IOS_DEST_PLUGIN_PATH, false);
		}
		// 플러그인 파일을 복사한다 }

		// 빌드 옵션을 설정한다 {
		a_oPlayerOptions.target = BuildTarget.iOS;
		a_oPlayerOptions.targetGroup = BuildTargetGroup.iOS;
		a_oPlayerOptions.locationPathName = KEditorDefine.B_IOS_BUILD_PATH;

		if(CPlatformBuilder.ProjectInfoTable != null) {
			PlayerSettings.bundleVersion = CPlatformBuilder.ProjectInfoTable.iOSProjectInfo.m_oBuildVersion;
		}
		// 빌드 옵션을 설정한다 }

		// 플랫폼을 빌드한다
		CPlatformBuilder.BuildPlatform(a_oPlayerOptions);
	}
	#endregion			// 클래스 함수
}
