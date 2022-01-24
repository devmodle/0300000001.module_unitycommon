using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using EnhancedHierarchy;

#if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE
using UnityEngine.Rendering.Universal;
#endif			// #if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE

/** 에디터 씬 관리자 */
[InitializeOnLoad]
public static partial class CEditorSceneManager {
	#region 클래스 변수
	private static bool m_bIsEnableSetup = false;
	private static bool m_bIsSetupDependencies = false;

	private static float m_fUpdateSkipTime = 0.0f;
	private static float m_fDefineSymbolSkipTime = 0.0f;

	private static ListRequest m_oListRequest = null;
	private static List<AddRequest> m_oAddRequestList = new List<AddRequest>();
	#endregion			// 클래스 변수
	
	#region 클래스 함수
	/** 생성자 */
	static CEditorSceneManager() {
		CEditorSceneManager.SetupCallbacks();
	}

	/** 스크립트가 로드 되었을 경우 */
	[UnityEditor.Callbacks.DidReloadScripts]
	public static void OnLoadScript() {
		CEditorSceneManager.m_bIsEnableSetup = true;
	}

	/** 상태를 갱신한다 */
	private static void Update() {
		// 상태 갱신이 가능 할 경우
		if(CEditorAccess.IsEnableUpdateState) {
			CEditorSceneManager.m_fUpdateSkipTime += Time.deltaTime;
			var oMonoScripts = MonoImporter.GetAllRuntimeMonoScripts();

			// 상태 갱신이 가능 할 경우
			if(CEditorSceneManager.m_bIsEnableSetup) {
				CEditorSceneManager.m_bIsEnableSetup = false;
				CEditorSceneManager.m_oListRequest = Client.List();

				Preferences.Tooltips.Value = false;
				Preferences.SelectOnTree.Value = true;

				CEditorSceneManager.SetupCallbacks();

#if RUNTIME_TEMPLATES_MODULE_ENABLE
				EditorFactory.CreateSaleItemInfoTable();
				EditorFactory.CreateSaleProductInfoTable();
				EditorFactory.CreateMissionInfoTable();
				EditorFactory.CreateRewardInfoTable();
				EditorFactory.CreateEpisodeInfoTable();
				EditorFactory.CreateTutorialInfoTable();
				EditorFactory.CreateBlockInfoTable();
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
			}

			// 갱신 주기가 지났을 경우
			if(CEditorSceneManager.m_fUpdateSkipTime.ExIsGreateEquals(KCEditorDefine.B_DELTA_T_SCENE_M_SCRIPT_UPDATE)) {
				CEditorSceneManager.m_fUpdateSkipTime = KCDefine.B_VAL_0_FLT;
				CFunc.EnumerateScenes((a_stScene) => { CSampleSceneManager.SetupSceneManager(a_stScene, KEditorDefine.B_SCENE_MANAGER_TYPE_DICT); return true; });
			}

			for(int i = 0; i < oMonoScripts.Length; ++i) {
				// 스크립트가 존재 할 경우
				if(oMonoScripts[i] != null) {
					var oType = oMonoScripts[i].GetClass();
					
					// 스크립트 순서 설정이 가능 할 경우
					if(oType != null && KEditorDefine.B_SCRIPT_ORDER_DICT.TryGetValue(oType, out int nOrder)) {
						CAccess.SetScriptOrder(oMonoScripts[i], nOrder);
					}
				}
			}
		}
	}

	/** 독립 패키지 상태를 갱신한다 */
	private static void UpdateDependencyState() {
		// 상태 갱신이 가능 할 경우
		if(CEditorAccess.IsEnableUpdateState && (m_oListRequest != null && m_oListRequest.ExIsComplete())) {
			try {
				CEditorSceneManager.SetupDependencies();
			} finally {
				CEditorSceneManager.m_oListRequest = null;
				CEditorSceneManager.m_bIsSetupDependencies = true;

				EditorApplication.update -= CEditorSceneManager.UpdateDependencyState;
			}
		}
	}

	/** 패키지 레지스트리 상태를 갱신한다 */
	private static void UpdateScopedRegistryState() {
		// 상태 갱신이 가능 할 경우
		if(CEditorAccess.IsEnableUpdateState) {
			CEditorSceneManager.SetupScopedRegistries();
			EditorApplication.update -= CEditorSceneManager.UpdateScopedRegistryState;
		}
	}

	/** 상태를 갱신한다 */
	private static void LateUpdate() {
		bool bIsEnableUpdate = CEditorAccess.IsEnableUpdateState && CEditorSceneManager.m_bIsSetupDependencies && CEditorSceneManager.m_oAddRequestList.Count <= KCDefine.B_VAL_0_INT;
		CEditorSceneManager.m_fDefineSymbolSkipTime = bIsEnableUpdate ? CEditorSceneManager.m_fDefineSymbolSkipTime + Time.deltaTime : KCDefine.B_VAL_0_FLT;

		// 상태 갱신이 가능 할 경우
		if(bIsEnableUpdate && CEditorSceneManager.m_fDefineSymbolSkipTime.ExIsGreateEquals(KEditorDefine.B_DELAY_DEFINE_S_UPDATE)) {
			var oDefineSymbolInfoTable = CEditorFunc.FindAsset<CDefineSymbolInfoTable>(KCEditorDefine.B_ASSET_P_DEFINE_SYMBOL_INFO_TABLE);

			// 전처리기 심볼 정보 테이블이 존재 할 경우
			if(oDefineSymbolInfoTable != null) {
				CEditorSceneManager.m_bIsSetupDependencies = false;
				CEditorSceneManager.m_fDefineSymbolSkipTime = KCDefine.B_VAL_0_FLT;

				var oDefineSymbolLists = new List<List<string>>() {
					oDefineSymbolInfoTable.EditorCommonDefineSymbolList,
					oDefineSymbolInfoTable.EditorSubCommonDefineSymbolList,

					oDefineSymbolInfoTable.EditoriOSAppleDefineSymbolList,

					oDefineSymbolInfoTable.EditorAndroidGoogleDefineSymbolList,
					oDefineSymbolInfoTable.EditorAndroidAmazonDefineSymbolList,
					oDefineSymbolInfoTable.EditorAndroidOneStoreDefineSymbolList,
					
					oDefineSymbolInfoTable.EditorStandaloneMacAppleDefineSymbolList,
					oDefineSymbolInfoTable.EditorStandaloneMacSteamDefineSymbolList,
					oDefineSymbolInfoTable.EditorStandaloneWndsSteamDefineSymbolList
				};

				foreach(var stKeyVal in KCEditorDefine.DS_DEFINE_S_REPLACE_MODULE_DICT) {
					for(int i = 0; i < oDefineSymbolLists.Count; ++i) {
						// 전처리기 심볼 갱신이 필요 할 경우
						if(oDefineSymbolLists[i].Contains(stKeyVal.Key)) {
							EditorUtility.SetDirty(oDefineSymbolInfoTable);
							oDefineSymbolLists[i].ExReplaceVal(stKeyVal.Key, stKeyVal.Value);
						}
					}
				}

				// 전처리기 심볼 갱신이 필요 할 경우
				if(EditorUtility.IsDirty(oDefineSymbolInfoTable)) {
					CEditorFunc.UpdateAssetDBState();
				}
			}
		}
	}
	#endregion			// 클래스 함수

	#region 추가 클래스 함수

	#endregion			// 추가 클래스 함수
}
#endif			// #if UNITY_EDITOR
#endif			// #if NEVER_USE_THIS
