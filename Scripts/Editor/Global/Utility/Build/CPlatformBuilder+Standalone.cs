using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

//! 플랫폼 빌더 - 독립 플랫폼
public static partial class CPlatformBuilder {
	#region 클래스 함수
	//! 맥 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Standalone/Mac/Debug")]
	public static void BuildMacPlatformDebug() {
		CPlatformBuilder.BuildStandalonePlatformDebug(EStandalonePlatformType.MAC);
	}

	//! 윈도우즈 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Standalone/Windows/Debug")]
	public static void BuildWindowsPlatformDebug() {
		CPlatformBuilder.BuildStandalonePlatformDebug(EStandalonePlatformType.WINDOWS);
	}

	//! 맥 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Standalone/Mac/Debug with AutoPlay")]
	public static void BuildMacPlatformWithAutoPlayDebug() {
		CPlatformBuilder.BuildStandalonePlatformWithAutoPlayDebug(EStandalonePlatformType.MAC);
	}

	//! 윈도우즈 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Standalone/Windows/Debug with AutoPlay")]
	public static void BuildWindowsPlatformWithAutoPlayDebug() {
		CPlatformBuilder.BuildStandalonePlatformWithAutoPlayDebug(EStandalonePlatformType.WINDOWS);
	}

	//! 맥 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Standalone/Mac/Release")]
	public static void BuildMacPlatformRelease() {
		CPlatformBuilder.BuildStandalonePlatformRelease(EStandalonePlatformType.MAC);
	}

	//! 윈도우즈 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Standalone/Windows/Release")]
	public static void BuildWindowsPlatformRelease() {
		CPlatformBuilder.BuildStandalonePlatformRelease(EStandalonePlatformType.WINDOWS);
	}

	//! 맥 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Standalone/Mac/Release with AutoPlay")]
	public static void BuildMacPlatformWithAutoPlayRelease() {
		CPlatformBuilder.BuildStandalonePlatformWithAutoPlayRelease(EStandalonePlatformType.MAC);
	}

	//! 윈도우즈 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Standalone/Windows/Release with AutoPlay")]
	public static void BuildWindowsPlatformWithAutoPlayRelease() {
		CPlatformBuilder.BuildStandalonePlatformWithAutoPlayRelease(EStandalonePlatformType.WINDOWS);
	}

	//! 맥 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Standalone/Mac/Release with AutoPlay (Disable FPS)")]
	public static void BuildMacPlatformWithAutoPlayDisableFPSRelease() {
		CPlatformBuilder.BuildStandalonePlatformWithAutoPlayDisableFPSRelease(EStandalonePlatformType.MAC);
	}

	//! 윈도우즈 플랫폼을 빌드한다
	[MenuItem("Utility/Build/Local/Standalone/Windows/Release with AutoPlay (Disable FPS)")]
	public static void BuildWindowsPlatformWithAutoPlayDisableFPSRelease() {
		CPlatformBuilder.BuildStandalonePlatformWithAutoPlayDisableFPSRelease(EStandalonePlatformType.WINDOWS);
	}

	//! 맥 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/Standalone/Mac/Debug")]
	public static void RemoteBuildMacPlatformDebug() {
		CPlatformBuilder.RemoteBuildStandalonePlatformDebug(EStandalonePlatformType.MAC);
	}

	//! 윈도우즈 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/Standalone/Windows/Debug")]
	public static void RemoteBuildWindowsPlatformDebug() {
		CPlatformBuilder.RemoteBuildStandalonePlatformDebug(EStandalonePlatformType.WINDOWS);
	}

	//! 맥 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/Standalone/Mac/Release")]
	public static void RemoteBuildMacPlatformRelease() {
		CPlatformBuilder.RemoteBuildStandalonePlatformRelease(EStandalonePlatformType.MAC);
	}

	//! 윈도우즈 플랫폼을 원격 빌드한다
	[MenuItem("Utility/Build/Remote (Jenkins)/Standalone/Windows/Release")]
	public static void RemoteBuildWindowsPlatformRelease() {
		CPlatformBuilder.RemoteBuildStandalonePlatformRelease(EStandalonePlatformType.WINDOWS);
	}

	//! 독립 플랫폼을 빌드한다
	private static void BuildStandalonePlatformDebug(EStandalonePlatformType a_ePlatformType) {
		// 전처리기 심볼을 추가한다
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.Standalone, KEditorDefine.DS_DEFINE_SYMBOL_ADS_TEST_ENABLE);
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.Standalone, KEditorDefine.DS_DEFINE_SYMBOL_LOGIC_TEST_ENABLE);

		// 빌드 옵션을 설정한다
		var oPlayerOptions = new BuildPlayerOptions();
		oPlayerOptions.options = BuildOptions.Development;
		
		CPlatformBuilder.BuildStandalonePlatform(oPlayerOptions, a_ePlatformType);
	}

	//! 독립 플랫폼을 빌드한다
	private static void BuildStandalonePlatformWithAutoPlayDebug(EStandalonePlatformType a_ePlatformType) {
		CPlatformBuilder.IsAutoPlay = true;
		CPlatformBuilder.BuildStandalonePlatformDebug(a_ePlatformType);
	}

	//! 독립 플랫폼을 빌드한다
	private static void BuildStandalonePlatformRelease(EStandalonePlatformType a_ePlatformType) {
		// 전처리기 심볼을 추가한다 {
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.Standalone, KEditorDefine.DS_DEFINE_SYMBOL_ADS_TEST_ENABLE);

		if(CPlatformBuilder.IsAutoPlay) {
			CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.Standalone, KEditorDefine.DS_DEFINE_SYMBOL_LOGIC_TEST_ENABLE);
		}
		// 전처리기 심볼을 추가한다 }

		CPlatformBuilder.BuildStandalonePlatform(new BuildPlayerOptions(), a_ePlatformType);
	}

	//! 독립 플랫폼을 빌드한다
	private static void BuildStandalonePlatformWithAutoPlayRelease(EStandalonePlatformType a_ePlatformType) {
		CPlatformBuilder.AddDefineSymbol(BuildTargetGroup.Standalone, KEditorDefine.DS_DEFINE_SYMBOL_FPS_ENABLE);

		CPlatformBuilder.IsAutoPlay = true;
		CPlatformBuilder.BuildStandalonePlatformRelease(a_ePlatformType);
	}

	//! 독립 플랫폼을 빌드한다
	private static void BuildStandalonePlatformWithAutoPlayDisableFPSRelease(EStandalonePlatformType a_ePlatformType) {
		CPlatformBuilder.IsAutoPlay = true;
		CPlatformBuilder.BuildStandalonePlatformRelease(a_ePlatformType);
	}

	//! 독립 플랫폼을 빌드한다
	private static void BuildStandalonePlatform(BuildPlayerOptions a_oPlayerOptions, EStandalonePlatformType a_ePlatformType) {
		CPlatformBuilder.StandalonePlatformType = a_ePlatformType;
		CPlatformBuilder.IsEnableEditorScene = true;

		// 빌드 옵션을 설정한다 {
		a_oPlayerOptions.targetGroup = BuildTargetGroup.Standalone;

		if(a_ePlatformType == EStandalonePlatformType.WINDOWS) {
			a_oPlayerOptions.target = BuildTarget.StandaloneWindows;
			a_oPlayerOptions.locationPathName = KEditorDefine.B_WINDOWS_BUILD_PATH;
		} else {
			a_oPlayerOptions.target = BuildTarget.StandaloneOSX;
			a_oPlayerOptions.locationPathName = KEditorDefine.B_MAC_BUILD_PATH;
		}
		// 빌드 옵션을 설정한다 }

		// 빌드 디렉토리를 생성한다 {
		string oBuildPath = string.Format(KEditorDefine.B_STANDALONE_ABSOLUTE_BUILD_PATH_FORMAT, 
			EditorFunction.GetStandalonePlatformName(a_ePlatformType));

		Function.CreateDirectory(oBuildPath);
		// 빌드 디렉토리를 생성한다 }

		// 플랫폼을 빌드한다
		CPlatformBuilder.BuildPlatform(a_oPlayerOptions);
	}

	//! 독립 플랫폼을 원격 빌드한다
	private static void RemoteBuildStandalonePlatformDebug(EStandalonePlatformType a_ePlatformType) {
		EditorFunction.ExecuteStandalonePlatformJenkinsBuild(KEditorDefine.B_JENKINS_STANDALONE_DEBUG_PIPELINE, a_ePlatformType);
	}

	//! 독립 플랫폼을 원격 빌드한다
	private static void RemoteBuildStandalonePlatformRelease(EStandalonePlatformType a_ePlatformType) {
		EditorFunction.ExecuteStandalonePlatformJenkinsBuild(KEditorDefine.B_JENKINS_STANDALONE_DEBUG_PIPELINE, a_ePlatformType);
	}
	#endregion			// 클래스 함수
}
