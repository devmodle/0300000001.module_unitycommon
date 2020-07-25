using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

//! 플랫폼 빌더 - iOS
public static partial class CPlatformBuilder {
	#region 클래스 함수
	//! iOS 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/iOS/Device/Debug")]
	public static void BuildiOSDevicePlatformDebug() {
		// 전처리기 심볼을 추가한다
		CPlatformBuildOption.AddDefineSymbol(BuildTargetGroup.iOS, KCEditorDefine.DS_DEFINE_S_ADS_TEST_ENABLE);
		CPlatformBuildOption.AddDefineSymbol(BuildTargetGroup.iOS, KCEditorDefine.DS_DEFINE_S_LOGIC_TEST_ENABLE);

		// 프로비저닝 파일 정보를 설정한다
		PlayerSettings.iOS.iOSManualProvisioningProfileID = CPlatformBuildOption.BuildInfoTable.iOSBuildInfo.m_oDevProfileID;
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
		CPlatformBuildOption.AddDefineSymbol(BuildTargetGroup.iOS, KCEditorDefine.DS_DEFINE_S_ADS_TEST_ENABLE);

		if(CPlatformBuilder.IsAutoPlay) {
			CPlatformBuildOption.AddDefineSymbol(BuildTargetGroup.iOS, KCEditorDefine.DS_DEFINE_S_LOGIC_TEST_ENABLE);
		}
		// 전처리기 심볼을 추가한다 }

		// 프로비저닝 파일 정보를 설정한다
		PlayerSettings.iOS.iOSManualProvisioningProfileID = CPlatformBuildOption.BuildInfoTable.iOSBuildInfo.m_oDevProfileID;
		PlayerSettings.iOS.iOSManualProvisioningProfileType = ProvisioningProfileType.Development;

		// 빌드 옵션을 설정한다
		var oPlayerOptions = new BuildPlayerOptions();
		EditorUserBuildSettings.iOSBuildConfigType = iOSBuildType.Release;

		CPlatformBuilder.BuildiOSPlatform(oPlayerOptions);
	}

	//! iOS 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/iOS/Device/Release with AutoPlay")]
	public static void BuildiOSDevicePlatformWithAutoPlayRelease() {
		CPlatformBuildOption.AddDefineSymbol(BuildTargetGroup.iOS, KCEditorDefine.DS_DEFINE_S_FPS_ENABLE);

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
	public static void BuildiOSDevicePlatformAdhoc() {
		// 전처리기 심볼을 추가한다
		CPlatformBuildOption.AddDefineSymbol(BuildTargetGroup.iOS, KCEditorDefine.DS_DEFINE_S_ADS_TEST_ENABLE);
		CPlatformBuildOption.AddDefineSymbol(BuildTargetGroup.iOS, KCEditorDefine.DS_DEFINE_S_ADHOC_BUILD);

		// 프로비저닝 파일 정보를 설정한다
		PlayerSettings.iOS.iOSManualProvisioningProfileID = CPlatformBuildOption.BuildInfoTable.iOSBuildInfo.m_oAdhocProfileID;
		PlayerSettings.iOS.iOSManualProvisioningProfileType = ProvisioningProfileType.Distribution;

		EditorUserBuildSettings.iOSBuildConfigType = iOSBuildType.Release;
		CPlatformBuilder.BuildiOSPlatform(new BuildPlayerOptions());
	}

	//! iOS 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/iOS/Device/Distribution (Adhoc with Robo Test)")]
	public static void BuildiOSDevicePlatformWithRoboTestAdhoc() {
		CPlatformBuildOption.AddDefineSymbol(BuildTargetGroup.iOS, KCEditorDefine.DS_DEFINE_S_ROBO_TEST_ENABLE);
		CPlatformBuilder.BuildiOSDevicePlatformAdhoc();
	}

	//! iOS 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/iOS/Device/Distribution (Store)")]
	public static void BuildiOSDevicePlatformStore() {
		CPlatformBuilder.IsDistributionBuild = true;
		CPlatformBuildOption.AddDefineSymbol(BuildTargetGroup.iOS, KCEditorDefine.DS_DEFINE_S_STORE_BUILD);

		// 프로비저닝 파일 정보를 설정한다
		PlayerSettings.iOS.iOSManualProvisioningProfileID = CPlatformBuildOption.BuildInfoTable.iOSBuildInfo.m_oStoreProfileID;
		PlayerSettings.iOS.iOSManualProvisioningProfileType = ProvisioningProfileType.Distribution;

		EditorUserBuildSettings.iOSBuildConfigType = iOSBuildType.Release;
		CPlatformBuilder.BuildiOSPlatform(new BuildPlayerOptions());
	}

	//! iOS 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/iOS/Simulator/Debug")]
	public static void BuildiOSSimulatorPlatformDebug() {
		// 전처리기 심볼을 추가한다
		CPlatformBuildOption.AddDefineSymbol(BuildTargetGroup.iOS, KCEditorDefine.DS_DEFINE_S_ADS_TEST_ENABLE);
		CPlatformBuildOption.AddDefineSymbol(BuildTargetGroup.iOS, KCEditorDefine.DS_DEFINE_S_LOGIC_TEST_ENABLE);

		// 그래픽 API 를 설정한다
		PlayerSettings.iOS.sdkVersion = iOSSdkVersion.SimulatorSDK;
		CEditorAccess.SetGraphicAPI(BuildTarget.iOS, KCEditorDefine.B_IOS_SIMULATOR_GRAPHICS_DEVICE_TYPES);

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
		CPlatformBuildOption.AddDefineSymbol(BuildTargetGroup.iOS, KCEditorDefine.DS_DEFINE_S_ADS_TEST_ENABLE);
		CPlatformBuildOption.AddDefineSymbol(BuildTargetGroup.iOS, KCEditorDefine.DS_DEFINE_S_LOGIC_TEST_ENABLE);

		// 그래픽 API 를 설정한다
		PlayerSettings.iOS.sdkVersion = iOSSdkVersion.SimulatorSDK;
		CEditorAccess.SetGraphicAPI(BuildTarget.iOS, KCEditorDefine.B_IOS_SIMULATOR_GRAPHICS_DEVICE_TYPES);

		CPlatformBuilder.BuildiOSDevicePlatformRelease();
	}

	//! iOS 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/iOS/Simulator/Release with AutoPlay")]
	public static void BuildiOSSimulatorPlatformWithAutoPlayRelease() {
		CPlatformBuildOption.AddDefineSymbol(BuildTargetGroup.iOS, KCEditorDefine.DS_DEFINE_S_FPS_ENABLE);

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
		CPlatformBuilder.ExecuteiOSPlatformJenkinsBuild(KCDefine.B_BUILD_MODE_DEBUG,
			KCEditorDefine.B_JENKINS_DEBUG_BUILD_FUNC, KCEditorDefine.B_JENKINS_IOS_DEBUG_PIPELINE_NAME, CPlatformBuildOption.BuildInfoTable.iOSBuildInfo.m_oDevProfileID, KCEditorDefine.B_IOS_DEV_IPA_EXPORT_METHOD);
	}

	//! iOS 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/iOS/Release")]
	public static void RemoteBuildiOSPlatformRelease() {
		CPlatformBuilder.ExecuteiOSPlatformJenkinsBuild(KCDefine.B_BUILD_MODE_RELEASE,
			KCEditorDefine.B_JENKINS_RELEASE_BUILD_FUNC, KCEditorDefine.B_JENKINS_IOS_RELEASE_PIPELINE_NAME, CPlatformBuildOption.BuildInfoTable.iOSBuildInfo.m_oDevProfileID, KCEditorDefine.B_IOS_DEV_IPA_EXPORT_METHOD);
	}

	//! iOS 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/iOS/Distribution (Adhoc)")]
	public static void RemoteBuildiOSPlatformAdhoc() {
		CPlatformBuilder.ExecuteiOSPlatformJenkinsBuild(KCDefine.B_BUILD_MODE_RELEASE,
			KCEditorDefine.B_JENKINS_ADHOC_BUILD_FUNC, KCEditorDefine.B_JENKINS_IOS_ADHOC_PIPELINE_NAME, CPlatformBuildOption.BuildInfoTable.iOSBuildInfo.m_oAdhocProfileID, KCEditorDefine.B_IOS_ADHOC_IPA_EXPORT_METHOD);
	}

	//! iOS 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/iOS/Distribution (Store)")]
	public static void RemoteBuildiOSPlatformStore() {
		CPlatformBuilder.ExecuteiOSPlatformJenkinsBuild(KCDefine.B_BUILD_MODE_RELEASE,
			KCEditorDefine.B_JENKINS_STORE_BUILD_FUNC, KCEditorDefine.B_JENKINS_IOS_STORE_PIPELINE_NAME, CPlatformBuildOption.BuildInfoTable.iOSBuildInfo.m_oStoreProfileID, KCEditorDefine.B_IOS_STORE_IPA_EXPORT_METHOD);
	}

	//! iOS 플랫폼을 빌드한다
	private static void BuildiOSPlatform(BuildPlayerOptions a_oPlayerOptions) {
		CPlatformBuilder.IsEnableEditorScene = false;

		// 플러그인 파일을 복사한다
		if(!Application.isBatchMode) {
			CFunc.CopyDirectory(KCEditorDefine.B_IOS_SRC_PLUGIN_PATH, KCEditorDefine.B_IOS_DEST_PLUGIN_PATH, false);
		}

		// 빌드 옵션을 설정한다 {
		a_oPlayerOptions.target = BuildTarget.iOS;
		a_oPlayerOptions.targetGroup = BuildTargetGroup.iOS;
		a_oPlayerOptions.locationPathName = KCEditorDefine.B_IOS_BUILD_PATH;

		if(CPlatformBuildOption.ProjInfoTable != null) {
			PlayerSettings.bundleVersion = CPlatformBuildOption.ProjInfoTable.iOSProjInfo.m_oBuildVersion;
		}
		// 빌드 옵션을 설정한다 }

		// 빌드 디렉토리를 생성한다
		CAccess.CreateDirectory(KCEditorDefine.B_IOS_ABSOLUTE_BUILD_PATH);

		// 플랫폼을 빌드한다
		CPlatformBuilder.BuildPlatform(a_oPlayerOptions);
	}
	#endregion			// 클래스 함수
}
#endif			// #if UNITY_EDITOR
