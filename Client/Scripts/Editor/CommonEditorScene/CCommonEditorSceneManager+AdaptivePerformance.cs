using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;

#if ADAPTIVE_PERFORMANCE_ENABLE
using UnityEngine.AdaptivePerformance;
using UnityEditor.AdaptivePerformance.Editor;
#endif // #if ADAPTIVE_PERFORMANCE_ENABLE

/** 공용 에디터 씬 관리자 - 적응형 퍼포먼스 */
public static partial class CCommonEditorSceneManager {
	#region 클래스 함수
	/** 적응형 퍼포먼스를 설정한다 */
	private static void SetupAdaptivePerformance() {
#if ADAPTIVE_PERFORMANCE_ENABLE
		// 적응형 퍼포먼스 설정이 존재 할 경우
		if(EditorBuildSettings.TryGetConfigObject(KCEditorDefine.B_MODULE_N_ADAPTIVE_PERFORMANCE_SETTINGS, out AdaptivePerformanceGeneralSettingsPerBuildTarget oPerformanceSettings)) {
			var oiOSSettings = oPerformanceSettings.SettingsForBuildTarget(BuildTargetGroup.iOS);
			var oAndroidSettings = oPerformanceSettings.SettingsForBuildTarget(BuildTargetGroup.Android);
			var oStandaloneSettings = oPerformanceSettings.SettingsForBuildTarget(BuildTargetGroup.Standalone);

			// 설정이 존재 할 경우
			if(oiOSSettings != null && oAndroidSettings != null && oStandaloneSettings != null) {
				var oIsSetupOptsList = new List<bool>() {
					oiOSSettings.InitManagerOnStart,
					oAndroidSettings.InitManagerOnStart,
					oStandaloneSettings.InitManagerOnStart
				};

				// 설정 갱신이 필요 할 경우
				if(oIsSetupOptsList.Contains(false)) {
					oiOSSettings.InitManagerOnStart = true;
					oAndroidSettings.InitManagerOnStart = true;
					oStandaloneSettings.InitManagerOnStart = true;

					CEditorFunc.SaveAsset(oPerformanceSettings);
				}
			}
		}

		// 적응형 퍼포먼스 제공자 설정이 존재 할 경우
		if(EditorBuildSettings.TryGetConfigObject(KCEditorDefine.B_MODULE_N_ADAPTIVE_PERFORMANCE_PROVIDER_SETTINGS, out IAdaptivePerformanceSettings oProviderSettings)) {
			CCommonEditorSceneManager.SetupAdaptivePerformanceProvider(oProviderSettings);
		}

		// 삼성 적응형 퍼포먼스 제공자 설정이 존재 할 경우
		if(EditorBuildSettings.TryGetConfigObject(KCEditorDefine.B_MODULE_N_ADAPTIVE_PERFORMANCE_SAMSUNG_PROVIDER_SETTINGS, out IAdaptivePerformanceSettings oSamsungProviderSettings)) {
			CCommonEditorSceneManager.SetupAdaptivePerformanceProvider(oSamsungProviderSettings);
		}
#endif // #if ADAPTIVE_PERFORMANCE_ENABLE
	}

#if ADAPTIVE_PERFORMANCE_ENABLE
	/** 적응형 퍼포먼스 제공자를 설정한다 */
	private static void SetupAdaptivePerformanceProvider(IAdaptivePerformanceSettings a_oProviderSettings) {
		var oIsSetupOptsList = new List<bool>() {
			a_oProviderSettings.logging == false,
			a_oProviderSettings.enableBoostOnStartup == false,
			a_oProviderSettings.automaticPerformanceMode == false,
			a_oProviderSettings.indexerSettings.active == false
		};

		// 설정 갱신이 필요 없을 경우
		if(!oIsSetupOptsList.Contains(false)) {
			return;
		}

		a_oProviderSettings.logging = false;
		a_oProviderSettings.enableBoostOnStartup = false;
		a_oProviderSettings.automaticPerformanceMode = false;
		a_oProviderSettings.indexerSettings.active = false;

		CEditorFunc.SaveAsset(a_oProviderSettings);
	}
#endif // #if ADAPTIVE_PERFORMANCE_ENABLE
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR
