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

	public static CBuildInfoTable BuildInfoTable { get; private set; } = null;
	public static CBuildOptionTable BuildOptionTable { get; private set; } = null;
	public static CProjectInfoTable ProjectInfoTable { get; private set; } = null;
	public static CDefineSymbolTable DefineSymbolTable { get; private set; } = null;
	public static Dictionary<BuildTargetGroup, List<string>> DefineSymbolListContainer { get; private set; } = new Dictionary<BuildTargetGroup, List<string>>();
	#endregion			// 클래스 프로퍼티

	#region 클래스 함수
	//! 생성자
	static CPlatformBuilder() {
		CPlatformBuilder.EditorInitialize();
	}

	//! 초기화
	public static void EditorInitialize() {
		// 테이블을 로드한다 {
		CPlatformBuilder.BuildInfoTable = Resources.Load<CBuildInfoTable>(KDefine.U_SCRIPTABLE_PATH_G_BUILD_INFO_TABLE);
		CPlatformBuilder.BuildInfoTable?.Awake();

		CPlatformBuilder.BuildOptionTable = Resources.Load<CBuildOptionTable>(KDefine.U_SCRIPTABLE_PATH_G_BUILD_OPTION_TABLE);
		CPlatformBuilder.BuildOptionTable?.Awake();

		CPlatformBuilder.ProjectInfoTable = Resources.Load<CProjectInfoTable>(KDefine.U_SCRIPTABLE_PATH_G_PROJECT_INFO_TABLE);
		CPlatformBuilder.ProjectInfoTable?.Awake();

		CPlatformBuilder.DefineSymbolTable = Resources.Load<CDefineSymbolTable>(KDefine.U_SCRIPTABLE_PATH_G_DEFINE_SYMBOL_TABLE);
		CPlatformBuilder.DefineSymbolTable?.Awake();
		// 테이블을 로드한다 }

		// 전처리기 심볼을 설정한다
		if(CPlatformBuilder.DefineSymbolTable != null) {
			CPlatformBuilder.DefineSymbolListContainer = CPlatformBuilder.DefineSymbolListContainer ?? new Dictionary<BuildTargetGroup, List<string>>();
			CPlatformBuilder.DefineSymbolListContainer.Clear();

			CPlatformBuilder.DefineSymbolListContainer.ExAddValue(BuildTargetGroup.Standalone, 
				CPlatformBuilder.DefineSymbolTable.StandaloneDefineSymbolList);

			CPlatformBuilder.DefineSymbolListContainer.ExAddValue(BuildTargetGroup.iOS, 
				CPlatformBuilder.DefineSymbolTable.iOSDefineSymbolList);

			CPlatformBuilder.DefineSymbolListContainer.ExAddValue(BuildTargetGroup.Android, 
				CPlatformBuilder.DefineSymbolTable.AndroidDefineSymbolList);

			if(Application.isBatchMode) {
				if(CPlatformBuilder.StandalonePlatformType == EStandalonePlatformType.WINDOWS) {
					CPlatformBuilder.DefineSymbolListContainer.ExAddValue(BuildTargetGroup.Standalone, 
						CPlatformBuilder.DefineSymbolTable.WindowsDefineSymbolList);
				} else {
					CPlatformBuilder.DefineSymbolListContainer.ExAddValue(BuildTargetGroup.Standalone, 
						CPlatformBuilder.DefineSymbolTable.MacDefineSymbolList);
				}

				if(CPlatformBuilder.AndroidPlatformType == EAndroidPlatformType.ONE_STORE) {
					CPlatformBuilder.DefineSymbolListContainer.ExAddValue(BuildTargetGroup.Android, 
						CPlatformBuilder.DefineSymbolTable.OneStoreDefineSymbolList);
				} else if(CPlatformBuilder.AndroidPlatformType == EAndroidPlatformType.GALAXY_STORE) {
					CPlatformBuilder.DefineSymbolListContainer.ExAddValue(BuildTargetGroup.Android, 
						CPlatformBuilder.DefineSymbolTable.GalaxyStoreDefineSymbolList);
				} else {
					CPlatformBuilder.DefineSymbolListContainer.ExAddValue(BuildTargetGroup.Android, 
						CPlatformBuilder.DefineSymbolTable.GoogleDefineSymbolList);
				}
			}

			foreach(var stKeyValue in CPlatformBuilder.DefineSymbolListContainer) {
				CPlatformBuilder.AddDefineSymbol(stKeyValue.Key, KEditorDefine.DS_DEFINE_SYMBOL_IL2CPP_ENABLE);
				CPlatformBuilder.AddDefineSymbol(stKeyValue.Key, KEditorDefine.DS_DEFINE_SYMBOL_USE_CUSTOM_PROJECT_OPTION);
			}
		}
	}

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

	//! 전처리기 심볼을 추가한다
	public static void AddDefineSymbol(BuildTargetGroup a_eTargetGroup, string a_oDefineSymbol) {
		if(CPlatformBuilder.DefineSymbolListContainer.ContainsKey(a_eTargetGroup)) {
			CPlatformBuilder.DefineSymbolListContainer[a_eTargetGroup].ExAddValue(a_oDefineSymbol);
		}
	}

	//! 전처리기 심볼을 제거한다
	public static void RemoveDefineSymbol(BuildTargetGroup a_eTargetGroup, string a_oDefineSymbol) {
		if(CPlatformBuilder.DefineSymbolListContainer.ContainsKey(a_eTargetGroup)) {
			CPlatformBuilder.DefineSymbolListContainer[a_eTargetGroup].ExRemoveValue(a_oDefineSymbol);
		}
	}

	//! 플랫폼을 빌드한다
	private static void BuildPlatform(BuildPlayerOptions a_oPlayerOptions) {
		try {
			// 전처리기 심볼을 설정한다 {
			if(CPlatformBuilder.IsDistributionBuild) {
				CPlatformBuilder.RemoveDefineSymbol(a_oPlayerOptions.targetGroup, KEditorDefine.DS_DEFINE_SYMBOL_ANALYTICS_TEST_ENABLE);
			}

			if(a_oPlayerOptions.targetGroup == BuildTargetGroup.Standalone) {
				if(CPlatformBuilder.StandalonePlatformType == EStandalonePlatformType.WINDOWS) {
					CPlatformBuilder.AddDefineSymbol(a_oPlayerOptions.targetGroup, KEditorDefine.DS_DEFINE_SYMBOL_WINDOWS_PLATFORM);
				} else {
					CPlatformBuilder.AddDefineSymbol(a_oPlayerOptions.targetGroup, KEditorDefine.DS_DEFINE_SYMBOL_MAC_PLATFORM);
				}
			} if(a_oPlayerOptions.targetGroup == BuildTargetGroup.iOS || a_oPlayerOptions.targetGroup == BuildTargetGroup.Android) {
				CPlatformBuilder.AddDefineSymbol(a_oPlayerOptions.targetGroup, KEditorDefine.DS_DEFINE_SYMBOL_RECEIPT_CHECK_ENABLE);

				if(a_oPlayerOptions.targetGroup == BuildTargetGroup.Android) {
					if(CPlatformBuilder.AndroidPlatformType == EAndroidPlatformType.ONE_STORE) {
						CPlatformBuilder.AddDefineSymbol(a_oPlayerOptions.targetGroup, KEditorDefine.DS_DEFINE_SYMBOL_ONE_STORE_PLATFORM);
					} else if(CPlatformBuilder.AndroidPlatformType == EAndroidPlatformType.GALAXY_STORE) {
						CPlatformBuilder.AddDefineSymbol(a_oPlayerOptions.targetGroup, KEditorDefine.DS_DEFINE_SYMBOL_GALAXY_STORE_PLATFORM);
					} else {
						CPlatformBuilder.AddDefineSymbol(a_oPlayerOptions.targetGroup, KEditorDefine.DS_DEFINE_SYMBOL_GOOGLE_PLATFORM);
					}
				}
			}

			EditorFunc.SetupDefineSymbols(CPlatformBuilder.DefineSymbolListContainer);
			// 전처리기 심볼을 설정한다 }

			// 전처리기 심볼을 저장한다
			string oFilepath = string.Format(KEditorDefine.B_ASSET_PATH_FORMAT_DEFINE_SYMBOL_OUTPUT, a_oPlayerOptions.targetGroup);
			Func.WriteString(oFilepath, PlayerSettings.GetScriptingDefineSymbolsForGroup(a_oPlayerOptions.targetGroup), System.Text.Encoding.Default);

			// 플랫폼을 설정한다
			EditorFunc.SetupPlayerOptions();
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
				bool bIsContainsA = EditorBuildSettings.scenes[i].path.Contains(KEditorDefine.B_SCENE_NAME_PATTERN_EDITOR_A);
				bool bIsContainsB = EditorBuildSettings.scenes[i].path.Contains(KEditorDefine.B_SCENE_NAME_PATTERN_EDITOR_B);

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
			EditorFunc.SetupGraphicAPIs();

			// 전처리기 심볼을 리셋한다 {
			CPlatformBuilder.RemoveDefineSymbol(a_oPlayerOptions.targetGroup, KEditorDefine.DS_DEFINE_SYMBOL_FPS_ENABLE);
			CPlatformBuilder.RemoveDefineSymbol(a_oPlayerOptions.targetGroup, KEditorDefine.DS_DEFINE_SYMBOL_ADS_TEST_ENABLE);
			CPlatformBuilder.RemoveDefineSymbol(a_oPlayerOptions.targetGroup, KEditorDefine.DS_DEFINE_SYMBOL_ROBO_TEST_ENABLE);
			CPlatformBuilder.RemoveDefineSymbol(a_oPlayerOptions.targetGroup, KEditorDefine.DS_DEFINE_SYMBOL_LOGIC_TEST_ENABLE);
			CPlatformBuilder.RemoveDefineSymbol(a_oPlayerOptions.targetGroup, KEditorDefine.DS_DEFINE_SYMBOL_RECEIPT_CHECK_ENABLE);

			CPlatformBuilder.RemoveDefineSymbol(a_oPlayerOptions.targetGroup, KEditorDefine.DS_DEFINE_SYMBOL_ADHOC_BUILD);
			CPlatformBuilder.RemoveDefineSymbol(a_oPlayerOptions.targetGroup, KEditorDefine.DS_DEFINE_SYMBOL_STORE_BUILD);

			if(a_oPlayerOptions.targetGroup == BuildTargetGroup.Standalone) {
				CPlatformBuilder.RemoveDefineSymbol(a_oPlayerOptions.targetGroup, KEditorDefine.DS_DEFINE_SYMBOL_MAC_PLATFORM);
				CPlatformBuilder.RemoveDefineSymbol(a_oPlayerOptions.targetGroup, KEditorDefine.DS_DEFINE_SYMBOL_WINDOWS_PLATFORM);
			} else if(a_oPlayerOptions.targetGroup == BuildTargetGroup.Android) {
				CPlatformBuilder.RemoveDefineSymbol(a_oPlayerOptions.targetGroup, KEditorDefine.DS_DEFINE_SYMBOL_GOOGLE_PLATFORM);
				CPlatformBuilder.RemoveDefineSymbol(a_oPlayerOptions.targetGroup, KEditorDefine.DS_DEFINE_SYMBOL_ONE_STORE_PLATFORM);
				CPlatformBuilder.RemoveDefineSymbol(a_oPlayerOptions.targetGroup, KEditorDefine.DS_DEFINE_SYMBOL_GALAXY_STORE_PLATFORM);
			}

			EditorFunc.SetupDefineSymbols(CPlatformBuilder.DefineSymbolListContainer);
			// 전처리기 심볼을 리셋한다 }
		}
	}
	#endregion			// 클래스 함수
}
