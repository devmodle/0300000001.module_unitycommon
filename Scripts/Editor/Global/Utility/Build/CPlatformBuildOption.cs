using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//! 플랫폼 빌드 옵션
public static partial class CPlatformBuildOption {
	#region 클래스 프로퍼티
	public static CBuildInfoTable BuildInfoTable { get; private set; } = null;
	public static CBuildOptionTable BuildOptionTable { get; private set; } = null;
	public static CProjectInfoTable ProjectInfoTable { get; private set; } = null;
	public static CDefineSymbolTable DefineSymbolTable { get; private set; } = null;
	public static Dictionary<BuildTargetGroup, List<string>> DefineSymbolListContainer { get; private set; } = new Dictionary<BuildTargetGroup, List<string>>();
	#endregion			// 클래스 프로퍼티

	#region 클래스 함수
	//! 생성자
	static CPlatformBuildOption() {
		CPlatformBuildOption.EditorInitialize();
	}

	//! 초기화
	public static void EditorInitialize() {
		// 테이블을 로드한다 {
		CPlatformBuildOption.BuildInfoTable = Resources.Load<CBuildInfoTable>(KCDefine.SCRIPTABLE_PATH_G_BUILD_INFO_TABLE);
		CPlatformBuildOption.BuildInfoTable?.Awake();

		CPlatformBuildOption.BuildOptionTable = Resources.Load<CBuildOptionTable>(KCDefine.SCRIPTABLE_PATH_G_BUILD_OPTION_TABLE);
		CPlatformBuildOption.BuildOptionTable?.Awake();

		CPlatformBuildOption.ProjectInfoTable = Resources.Load<CProjectInfoTable>(KCDefine.SCRIPTABLE_PATH_G_PROJECT_INFO_TABLE);
		CPlatformBuildOption.ProjectInfoTable?.Awake();

		CPlatformBuildOption.DefineSymbolTable = Resources.Load<CDefineSymbolTable>(KCDefine.SCRIPTABLE_PATH_G_DEFINE_SYMBOL_TABLE);
		CPlatformBuildOption.DefineSymbolTable?.Awake();
		// 테이블을 로드한다 }

		// 전처리기 심볼을 설정한다
		if(CPlatformBuildOption.DefineSymbolTable != null) {
			CPlatformBuildOption.DefineSymbolListContainer = CPlatformBuildOption.DefineSymbolListContainer ?? new Dictionary<BuildTargetGroup, List<string>>();
			CPlatformBuildOption.DefineSymbolListContainer.Clear();

			CPlatformBuildOption.DefineSymbolListContainer.ExAddValue(BuildTargetGroup.Standalone, 
				CPlatformBuildOption.DefineSymbolTable.StandaloneDefineSymbolList);

			CPlatformBuildOption.DefineSymbolListContainer.ExAddValue(BuildTargetGroup.iOS, 
				CPlatformBuildOption.DefineSymbolTable.iOSDefineSymbolList);

			CPlatformBuildOption.DefineSymbolListContainer.ExAddValue(BuildTargetGroup.Android, 
				CPlatformBuildOption.DefineSymbolTable.AndroidDefineSymbolList);

			if(Application.isBatchMode) {
				if(CPlatformBuilder.StandalonePlatformType == EStandalonePlatformType.WINDOWS) {
					CPlatformBuildOption.DefineSymbolListContainer.ExAddValue(BuildTargetGroup.Standalone, 
						CPlatformBuildOption.DefineSymbolTable.WindowsDefineSymbolList);
				} else {
					CPlatformBuildOption.DefineSymbolListContainer.ExAddValue(BuildTargetGroup.Standalone, 
						CPlatformBuildOption.DefineSymbolTable.MacDefineSymbolList);
				}

				if(CPlatformBuilder.AndroidPlatformType == EAndroidPlatformType.ONE_STORE) {
					CPlatformBuildOption.DefineSymbolListContainer.ExAddValue(BuildTargetGroup.Android, 
						CPlatformBuildOption.DefineSymbolTable.OneStoreDefineSymbolList);
				} else if(CPlatformBuilder.AndroidPlatformType == EAndroidPlatformType.GALAXY_STORE) {
					CPlatformBuildOption.DefineSymbolListContainer.ExAddValue(BuildTargetGroup.Android, 
						CPlatformBuildOption.DefineSymbolTable.GalaxyStoreDefineSymbolList);
				} else {
					CPlatformBuildOption.DefineSymbolListContainer.ExAddValue(BuildTargetGroup.Android, 
						CPlatformBuildOption.DefineSymbolTable.GoogleDefineSymbolList);
				}
			}

			foreach(var stKeyValue in CPlatformBuildOption.DefineSymbolListContainer) {
				CPlatformBuildOption.AddDefineSymbol(stKeyValue.Key, KCEditorDefine.DS_DEFINE_SYMBOL_IL2CPP_ENABLE);
				CPlatformBuildOption.AddDefineSymbol(stKeyValue.Key, KCEditorDefine.DS_DEFINE_SYMBOL_USE_CUSTOM_PROJECT_OPTION);
			}
		}
	}

	//! 전처리기 심볼을 추가한다
	public static void AddDefineSymbol(BuildTargetGroup a_eTargetGroup, string a_oDefineSymbol) {
		if(CPlatformBuildOption.DefineSymbolListContainer.ContainsKey(a_eTargetGroup)) {
			CPlatformBuildOption.DefineSymbolListContainer[a_eTargetGroup].ExAddValue(a_oDefineSymbol);
		}
	}

	//! 전처리기 심볼을 제거한다
	public static void RemoveDefineSymbol(BuildTargetGroup a_eTargetGroup, string a_oDefineSymbol) {
		if(CPlatformBuildOption.DefineSymbolListContainer.ContainsKey(a_eTargetGroup)) {
			CPlatformBuildOption.DefineSymbolListContainer[a_eTargetGroup].ExRemoveValue(a_oDefineSymbol);
		}
	}
	#endregion			// 클래스 함수
}
