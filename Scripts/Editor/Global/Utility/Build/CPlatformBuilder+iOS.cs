using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif			// #if UNITY_IOS

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
	public static void BuildiOSDevicePlatformAdhoc() {
		// 전처리기 심볼을 추가한다
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.iOS, KEditorDefine.DS_DEFINE_SYMBOL_ADS_TEST_ENABLE);
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.iOS, KEditorDefine.DS_DEFINE_SYMBOL_ADHOC_BUILD);

		// 프로비저닝 파일 정보를 설정한다
		PlayerSettings.iOS.iOSManualProvisioningProfileID = CPlatformBuilder.BuildInfoTable.iOSBuildInfo.m_oAdhocProfileID;
		PlayerSettings.iOS.iOSManualProvisioningProfileType = ProvisioningProfileType.Distribution;

		EditorUserBuildSettings.iOSBuildConfigType = iOSBuildType.Release;
		CPlatformBuilder.BuildiOSPlatform(new BuildPlayerOptions());
	}

	//! iOS 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/iOS/Device/Distribution (Adhoc with Robo Test)")]
	public static void BuildiOSDevicePlatformWithRoboTestAdhoc() {
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.iOS, KEditorDefine.DS_DEFINE_SYMBOL_ROBO_TEST_ENABLE);
		CPlatformBuilder.BuildiOSDevicePlatformAdhoc();
	}

	//! iOS 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/iOS/Device/Distribution (Store)")]
	public static void BuildiOSDevicePlatformStore() {
		CPlatformBuilder.IsDistributionBuild = true;
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.iOS, KEditorDefine.DS_DEFINE_SYMBOL_STORE_BUILD);

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
		EditorFunc.SetGraphicAPI(BuildTarget.iOS, KEditorDefine.B_IOS_SIMULATOR_GRAPHICS_DEVICE_TYPES);

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
		EditorFunc.SetGraphicAPI(BuildTarget.iOS, KEditorDefine.B_IOS_SIMULATOR_GRAPHICS_DEVICE_TYPES);

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
		EditorFunc.ExecuteiOSPlatformJenkinsBuild(KDefine.B_BUILD_MODE_DEBUG,
			KEditorDefine.B_JENKINS_DEBUG_BUILD_FUNC, KEditorDefine.B_JENKINS_IOS_DEBUG_PIPELINE_NAME, CPlatformBuilder.BuildInfoTable.iOSBuildInfo.m_oDevProfileID, KEditorDefine.B_IOS_DEV_IPA_EXPORT_METHOD);
	}

	//! iOS 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/iOS/Release")]
	public static void RemoteBuildiOSPlatformRelease() {
		EditorFunc.ExecuteiOSPlatformJenkinsBuild(KDefine.B_BUILD_MODE_RELEASE,
			KEditorDefine.B_JENKINS_RELEASE_BUILD_FUNC, KEditorDefine.B_JENKINS_IOS_RELEASE_PIPELINE_NAME, CPlatformBuilder.BuildInfoTable.iOSBuildInfo.m_oDevProfileID, KEditorDefine.B_IOS_DEV_IPA_EXPORT_METHOD);
	}

	//! iOS 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/iOS/Distribution (Adhoc)")]
	public static void RemoteBuildiOSPlatformAdhoc() {
		EditorFunc.ExecuteiOSPlatformJenkinsBuild(KDefine.B_BUILD_MODE_RELEASE,
			KEditorDefine.B_JENKINS_ADHOC_BUILD_FUNC, KEditorDefine.B_JENKINS_IOS_ADHOC_PIPELINE_NAME, CPlatformBuilder.BuildInfoTable.iOSBuildInfo.m_oAdhocProfileID, KEditorDefine.B_IOS_ADHOC_IPA_EXPORT_METHOD);
	}

	//! iOS 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/iOS/Distribution (Store)")]
	public static void RemoteBuildiOSPlatformStore() {
		EditorFunc.ExecuteiOSPlatformJenkinsBuild(KDefine.B_BUILD_MODE_RELEASE,
			KEditorDefine.B_JENKINS_STORE_BUILD_FUNC, KEditorDefine.B_JENKINS_IOS_STORE_PIPELINE_NAME, CPlatformBuilder.BuildInfoTable.iOSBuildInfo.m_oStoreProfileID, KEditorDefine.B_IOS_STORE_IPA_EXPORT_METHOD);
	}

	//! iOS 플랫폼을 빌드한다
	private static void BuildiOSPlatform(BuildPlayerOptions a_oPlayerOptions) {
		CPlatformBuilder.IsEnableEditorScene = false;

		// 플러그인 파일을 복사한다
		if(!Application.isBatchMode) {
			Func.CopyDirectory(KEditorDefine.B_IOS_SRC_PLUGIN_PATH, KEditorDefine.B_IOS_DEST_PLUGIN_PATH, false);
		}

		// 빌드 옵션을 설정한다 {
		a_oPlayerOptions.target = BuildTarget.iOS;
		a_oPlayerOptions.targetGroup = BuildTargetGroup.iOS;
		a_oPlayerOptions.locationPathName = KEditorDefine.B_IOS_BUILD_PATH;

		if(CPlatformBuilder.ProjectInfoTable != null) {
			PlayerSettings.bundleVersion = CPlatformBuilder.ProjectInfoTable.iOSProjectInfo.m_oBuildVersion;
		}
		// 빌드 옵션을 설정한다 }

		// 빌드 디렉토리를 생성한다
		Func.CreateDirectory(KEditorDefine.B_IOS_ABSOLUTE_BUILD_PATH);

		// 플랫폼을 빌드한다
		CPlatformBuilder.BuildPlatform(a_oPlayerOptions);
	}

	//! iOS 빌드가 완료 되었을 경우
	private static void OnPostProcessiOSBuild(BuildTarget a_eTarget, string a_oPath) {
#if UNITY_IOS
		string oPlistFilepath = string.Format(KEditorDefine.B_IOS_INFO_PLIST_PATH_FORMAT, a_oPath);
		string oProjectFilepath = PBXProject.GetPBXProjectPath(a_oPath);

		// Plist 옵션을 설정한다 {
		var oDocument = new PlistDocument();
		oDocument.ReadFromFile(oPlistFilepath);

		if(oDocument.ExIsValid()) {
			oDocument.root.SetBoolean(KEditorDefine.B_IOS_ENCRYPTION_ENABLE_KEY, false);
			oDocument.WriteToFile(oPlistFilepath);
		}
		// Plist 옵션을 설정한다 }

		// 프로젝트 옵션을 설정한다 {
		var oProject = new PBXProject();
		oProject.ReadFromFile(oProjectFilepath);

		if(oProject != null) {
			string oGUID = oProject.GetUnityMainTargetGuid();

			var oCapability = new ProjectCapabilityManager(oProjectFilepath,
				KEditorDefine.B_PATH_CAPABILITY_ENTITLEMENTS_IOS, null, oGUID);

			for(int i = 0; i < KAppDefine.G_EXTRA_FRAMEWORKS_IOS.Length; ++i) {
				oProject.AddFrameworkToProject(oGUID, KAppDefine.G_EXTRA_FRAMEWORKS_IOS[i], false);
			}

			for(int i = 0; i < KAppDefine.G_EXTRA_CAPABILITY_TYPES_IOS.Length; ++i) {
				var oCapabilityType = KAppDefine.G_EXTRA_CAPABILITY_TYPES_IOS[i];

				if(oCapabilityType.Equals(PBXCapabilityType.GameCenter)) {
					oCapability.AddGameCenter();
				} else if(oCapabilityType.Equals(PBXCapabilityType.SignInWithApple)) {
					oCapability.AddSignInWithApple();
				} else if(oCapabilityType.Equals(PBXCapabilityType.PushNotifications)) {
					oCapability.AddPushNotifications(!CPlatformBuilder.IsDistributionBuild);
				} else if(oCapabilityType.Equals(PBXCapabilityType.InAppPurchase)) {
					oCapability.AddInAppPurchase();
				}

				oProject.AddCapability(oGUID, KAppDefine.G_EXTRA_CAPABILITY_TYPES_IOS[i], 
					KEditorDefine.B_PATH_CAPABILITY_ENTITLEMENTS_IOS);
			}

			oCapability.WriteToFile();
			oProject.WriteToFile(oProjectFilepath);
		}
		// 프로젝트 옵션을 설정한다 }
#endif			// #if UNITY_IOS
	}
	#endregion			// 클래스 함수
}
