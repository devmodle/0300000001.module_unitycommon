using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

//! 플랫폼 빌더
public static partial class CPlatformBuilder {
	#region 클래스 프로퍼티
	public static bool IsAutoPlay { get; private set; } = false;
	public static bool IsEnableEditorScene { get; private set; } = false;
	public static bool IsDistributionBuild { get; private set; } = false;

	public static EStandalonePlatformType StandalonePlatformType { get; private set; } = EStandalonePlatformType.NONE;
	public static EAndroidPlatformType AndroidPlatformType { get; private set; } = EAndroidPlatformType.NONE;
	#endregion			// 클래스 프로퍼티

	#region 클래스 함수
	//! 빌드가 완료 되었을 경우
	[PostProcessBuild]
	public static void OnPostProcessBuild(BuildTarget a_eTarget, string a_oPath) {
		bool bIsWindows = a_eTarget == BuildTarget.StandaloneWindows || a_eTarget == BuildTarget.StandaloneWindows64;

		if(bIsWindows || a_eTarget == BuildTarget.StandaloneOSX) {
			CPlatformBuilder.OnPostProcessStandaloneBuild(a_eTarget, a_oPath);
		} else if(a_eTarget == BuildTarget.iOS) {
			CPlatformBuilder.OnPostProcessiOSBuild(a_eTarget, a_oPath);
		} else if(a_eTarget == BuildTarget.Android) {
			CPlatformBuilder.OnPostProcessAndroidBuild(a_eTarget, a_oPath);
		}
	}

	//! 플랫폼을 빌드한다
	private static void BuildPlatform(BuildPlayerOptions a_oPlayerOptions) {
		try {
			// 전처리기 심볼을 설정한다 {
			if(CPlatformBuilder.IsDistributionBuild) {
				CPlatformBuildOption.RemoveDefineSymbol(a_oPlayerOptions.targetGroup, KDSEditorDefine.DEFINE_SYMBOL_ANALYTICS_TEST_ENABLE);
			}

			if(a_oPlayerOptions.targetGroup == BuildTargetGroup.Standalone) {
				if(CPlatformBuilder.StandalonePlatformType == EStandalonePlatformType.WINDOWS) {
					CPlatformBuildOption.AddDefineSymbol(a_oPlayerOptions.targetGroup, KDSEditorDefine.DEFINE_SYMBOL_WINDOWS_PLATFORM);
				} else {
					CPlatformBuildOption.AddDefineSymbol(a_oPlayerOptions.targetGroup, KDSEditorDefine.DEFINE_SYMBOL_MAC_PLATFORM);
				}
			} if(a_oPlayerOptions.targetGroup == BuildTargetGroup.iOS || a_oPlayerOptions.targetGroup == BuildTargetGroup.Android) {
				CPlatformBuildOption.AddDefineSymbol(a_oPlayerOptions.targetGroup, KDSEditorDefine.DEFINE_SYMBOL_RECEIPT_CHECK_ENABLE);

				if(a_oPlayerOptions.targetGroup == BuildTargetGroup.Android) {
					if(CPlatformBuilder.AndroidPlatformType == EAndroidPlatformType.ONE_STORE) {
						CPlatformBuildOption.AddDefineSymbol(a_oPlayerOptions.targetGroup, KDSEditorDefine.DEFINE_SYMBOL_ONE_STORE_PLATFORM);
					} else if(CPlatformBuilder.AndroidPlatformType == EAndroidPlatformType.GALAXY_STORE) {
						CPlatformBuildOption.AddDefineSymbol(a_oPlayerOptions.targetGroup, KDSEditorDefine.DEFINE_SYMBOL_GALAXY_STORE_PLATFORM);
					} else {
						CPlatformBuildOption.AddDefineSymbol(a_oPlayerOptions.targetGroup, KDSEditorDefine.DEFINE_SYMBOL_GOOGLE_PLATFORM);
					}
				}
			}

			EditorFunc.SetupDefineSymbols(CPlatformBuildOption.DefineSymbolListContainer);
			// 전처리기 심볼을 설정한다 }

			// 전처리기 심볼을 저장한다
			string oFilepath = string.Format(KBEditorDefine.ASSET_PATH_FORMAT_DEFINE_SYMBOL_OUTPUT, a_oPlayerOptions.targetGroup);
			CBAccess.WriteString(oFilepath, PlayerSettings.GetScriptingDefineSymbolsForGroup(a_oPlayerOptions.targetGroup), System.Text.Encoding.Default);

			// 플랫폼을 설정한다
			CPlatformBuildOption.SetupPlayerOptions();
			EditorUserBuildSettings.SwitchActiveBuildTarget(a_oPlayerOptions.targetGroup, a_oPlayerOptions.target);

			// 빌드 옵션을 설정한다 {
			if(CPlatformBuilder.IsAutoPlay) {
				a_oPlayerOptions.options |= BuildOptions.AutoRunPlayer;

#if PROFILER_ENABLE
				a_oPlayerOptions.options |= BuildOptions.Development | BuildOptions.ConnectWithProfiler;
#endif			// #if PROFILER_ENABLE
			}

			a_oPlayerOptions.options |= BuildOptions.StrictMode;
			a_oPlayerOptions.options &= ~(BuildOptions.CompressWithLz4 | BuildOptions.CompressWithLz4HC);
			// 빌드 옵션을 설정한다 }

			// 씬 경로를 설정한다 {
			var oScenePathList = new List<string>();

			for(int i = 0; i < EditorBuildSettings.scenes.Length; ++i) {
				bool bIsContainsA = EditorBuildSettings.scenes[i].path.Contains(KBEditorDefine.SCENE_NAME_PATTERN_EDITOR_A);
				bool bIsContainsB = EditorBuildSettings.scenes[i].path.Contains(KBEditorDefine.SCENE_NAME_PATTERN_EDITOR_B);

				if(CPlatformBuilder.IsEnableEditorScene || (!bIsContainsA && !bIsContainsB)) {
					oScenePathList.Add(EditorBuildSettings.scenes[i].path);
				}
			}

			a_oPlayerOptions.scenes = oScenePathList.ToArray();
			// 씬 경로를 설정한다 }

			// 에셋 상태를 갱신한다
			EditorFunc.UpdateAssetDatabaseState();

			// 플랫폼을 빌드한다
			if(!BuildPipeline.isBuildingPlayer) {
				BuildPipeline.BuildPlayer(a_oPlayerOptions);
			}
		} finally {
			CPlatformBuilder.IsAutoPlay = false;
			CPlatformBuilder.IsEnableEditorScene = false;
			CPlatformBuilder.IsDistributionBuild = false;

			// 그래픽 API 를 설정한다
			CPlatformBuildOption.SetupGraphicAPIs();

			// 전처리기 심볼을 리셋한다 {
			CPlatformBuildOption.RemoveDefineSymbol(a_oPlayerOptions.targetGroup, KDSEditorDefine.DEFINE_SYMBOL_FPS_ENABLE);
			CPlatformBuildOption.RemoveDefineSymbol(a_oPlayerOptions.targetGroup, KDSEditorDefine.DEFINE_SYMBOL_ADS_TEST_ENABLE);
			CPlatformBuildOption.RemoveDefineSymbol(a_oPlayerOptions.targetGroup, KDSEditorDefine.DEFINE_SYMBOL_ROBO_TEST_ENABLE);
			CPlatformBuildOption.RemoveDefineSymbol(a_oPlayerOptions.targetGroup, KDSEditorDefine.DEFINE_SYMBOL_LOGIC_TEST_ENABLE);
			CPlatformBuildOption.RemoveDefineSymbol(a_oPlayerOptions.targetGroup, KDSEditorDefine.DEFINE_SYMBOL_RECEIPT_CHECK_ENABLE);

			CPlatformBuildOption.RemoveDefineSymbol(a_oPlayerOptions.targetGroup, KDSEditorDefine.DEFINE_SYMBOL_ADHOC_BUILD);
			CPlatformBuildOption.RemoveDefineSymbol(a_oPlayerOptions.targetGroup, KDSEditorDefine.DEFINE_SYMBOL_STORE_BUILD);

			if(a_oPlayerOptions.targetGroup == BuildTargetGroup.Standalone) {
				CPlatformBuildOption.RemoveDefineSymbol(a_oPlayerOptions.targetGroup, KDSEditorDefine.DEFINE_SYMBOL_MAC_PLATFORM);
				CPlatformBuildOption.RemoveDefineSymbol(a_oPlayerOptions.targetGroup, KDSEditorDefine.DEFINE_SYMBOL_WINDOWS_PLATFORM);
			} else if(a_oPlayerOptions.targetGroup == BuildTargetGroup.Android) {
				CPlatformBuildOption.RemoveDefineSymbol(a_oPlayerOptions.targetGroup, KDSEditorDefine.DEFINE_SYMBOL_GOOGLE_PLATFORM);
				CPlatformBuildOption.RemoveDefineSymbol(a_oPlayerOptions.targetGroup, KDSEditorDefine.DEFINE_SYMBOL_ONE_STORE_PLATFORM);
				CPlatformBuildOption.RemoveDefineSymbol(a_oPlayerOptions.targetGroup, KDSEditorDefine.DEFINE_SYMBOL_GALAXY_STORE_PLATFORM);
			}

			EditorFunc.SetupDefineSymbols(CPlatformBuildOption.DefineSymbolListContainer);
			// 전처리기 심볼을 리셋한다 }
		}
	}
	#endregion			// 클래스 함수
}
