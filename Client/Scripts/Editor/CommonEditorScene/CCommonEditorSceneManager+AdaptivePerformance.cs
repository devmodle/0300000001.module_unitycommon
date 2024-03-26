using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;

#if ENABLE_ADAPTIVEPERFORMANCE
using UnityEngine.AdaptivePerformance;
using UnityEditor.AdaptivePerformance.Editor;
#endif // #if ENABLE_ADAPTIVEPERFORMANCE

/** 공용 에디터 씬 관리자 - 적응형 퍼포먼스 */
public static partial class CCommonEditorSceneManager
{
	#region 클래스 함수
	/** 적응형 퍼포먼스를 설정한다 */
	private static void SetupAdaptivePerformance()
	{
#if ENABLE_ADAPTIVEPERFORMANCE
		// 적응형 퍼포먼스 설정이 존재 할 경우
		if(EditorBuildSettings.TryGetConfigObject(KCDefineEditor.B_MODULE_N_ADAPTIVE_PERFORMANCE_SETTINGS, out AdaptivePerformanceGeneralSettingsPerBuildTarget oPerformanceSettings))
		{
			var oiOSSettings = oPerformanceSettings.SettingsForBuildTarget(BuildTargetGroup.iOS);
			var oAndroidSettings = oPerformanceSettings.SettingsForBuildTarget(BuildTargetGroup.Android);
			var oStandaloneSettings = oPerformanceSettings.SettingsForBuildTarget(BuildTargetGroup.Standalone);

			// 설정이 존재 할 경우
			if(oiOSSettings != null && oAndroidSettings != null && oStandaloneSettings != null)
			{
				var oListIsSetupOpts = new List<bool>() {
					oiOSSettings.InitManagerOnStart,
					oAndroidSettings.InitManagerOnStart,
					oStandaloneSettings.InitManagerOnStart
				};

				// 설정 갱신이 필요 할 경우
				if(oListIsSetupOpts.Contains(false))
				{
					oiOSSettings.InitManagerOnStart = true;
					oAndroidSettings.InitManagerOnStart = true;
					oStandaloneSettings.InitManagerOnStart = true;

					CFuncEditor.SaveAsset(oPerformanceSettings);
				}
			}
		}

		// 적응형 퍼포먼스 제공자 설정이 존재 할 경우
		if(EditorBuildSettings.TryGetConfigObject(KCDefineEditor.B_MODULE_N_ADAPTIVE_PERFORMANCE_PROVIDER_SETTINGS, out IAdaptivePerformanceSettings oProviderSettings))
		{
			CCommonEditorSceneManager.SetupAdaptivePerformanceProvider(oProviderSettings);
		}

		// 삼성 적응형 퍼포먼스 제공자 설정이 존재 할 경우
		if(EditorBuildSettings.TryGetConfigObject(KCDefineEditor.B_MODULE_N_ADAPTIVE_PERFORMANCE_SAMSUNG_PROVIDER_SETTINGS, out IAdaptivePerformanceSettings oSamsungProviderSettings))
		{
			CCommonEditorSceneManager.SetupAdaptivePerformanceProvider(oSamsungProviderSettings);
		}
#endif // #if ENABLE_ADAPTIVEPERFORMANCE
	}

#if ENABLE_ADAPTIVEPERFORMANCE
	/** 적응형 퍼포먼스 제공자를 설정한다 */
	private static void SetupAdaptivePerformanceProvider(IAdaptivePerformanceSettings a_oProviderSettings)
	{
		var oListIsSetupOpts = new List<bool>() {
			a_oProviderSettings.logging == false,
			a_oProviderSettings.enableBoostOnStartup == false,
			a_oProviderSettings.automaticPerformanceMode == false,
			a_oProviderSettings.indexerSettings.active == false
		};

		// 설정 갱신이 필요 없을 경우
		if(!oListIsSetupOpts.Contains(false))
		{
			return;
		}

		a_oProviderSettings.logging = false;
		a_oProviderSettings.enableBoostOnStartup = false;
		a_oProviderSettings.automaticPerformanceMode = false;
		a_oProviderSettings.indexerSettings.active = false;

		CFuncEditor.SaveAsset(a_oProviderSettings);
	}
#endif // #if ENABLE_ADAPTIVEPERFORMANCE
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR
