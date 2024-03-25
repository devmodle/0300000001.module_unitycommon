using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEngine.InputSystem;
using UnityEditor;

#if UNITY_IOS
using UnityEngine.InputSystem.iOS;
#endif // #if UNITY_IOS

/** 공용 에디터 씬 관리자 - 입력 시스템 */
public static partial class CCommonEditorSceneManager
{
	#region 클래스 함수
	/** 입력 시스템을 설정한다 */
	private static void SetupInputSystem()
	{
#if INPUT_SYSTEM_MODULE_ENABLE
		// 옵션 정보 테이블이 없을 경우
		if(CPlatformOptsSetter.OptsInfoTable == null)
		{
			return;
		}

		// 입력 시스템 설정이 없을 경우
		if(!EditorBuildSettings.TryGetConfigObject(KCEditorDefine.B_MODULE_N_INPUT_SYSTEM_SETTINGS, out InputSettings oInputSettings))
		{
			oInputSettings = AssetDatabase.LoadAssetAtPath<InputSettings>(KCEditorDefine.B_ASSET_P_INPUT_SETTINGS);
			oInputSettings = oInputSettings ?? CEditorFactory.CreateScriptableObj<InputSettings>(KCEditorDefine.B_ASSET_P_INPUT_SETTINGS);

			InputSystem.settings = oInputSettings;
			EditorBuildSettings.AddConfigObject(KCEditorDefine.B_MODULE_N_INPUT_SYSTEM_SETTINGS, oInputSettings, true);
		}

		var oListIsSetupOpts = new List<bool>() {
			oInputSettings.compensateForScreenOrientation,
			oInputSettings.updateMode == InputSettings.UpdateMode.ProcessEventsInDynamicUpdate,
			oInputSettings.editorInputBehaviorInPlayMode == InputSettings.EditorInputBehaviorInPlayMode.PointersAndKeyboardsRespectGameViewFocus,

#if UNITY_IOS
			oInputSettings.iOS.motionUsage.enabled == CPlatformOptsSetter.OptsInfoTable.BuildOptsInfo.m_stInfoOptsBuildiOS.m_bIsEnableInputSystemMotion,
			oInputSettings.iOS.motionUsage.usageDescription.ExIsEquals(CPlatformOptsSetter.OptsInfoTable.BuildOptsInfo.m_oInputSystemMotionDesc)
#endif // #if UNITY_IOS
		};

		// 설정 갱신이 필요 없을 경우
		if(!oListIsSetupOpts.Contains(false))
		{
			return;
		}

		oInputSettings.compensateForScreenOrientation = true;
		oInputSettings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
		oInputSettings.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.PointersAndKeyboardsRespectGameViewFocus;

#if UNITY_IOS
		oInputSettings.iOS.motionUsage.enabled = CPlatformOptsSetter.OptsInfoTable.BuildOptsInfo.m_stInfoOptsBuildiOS.m_bIsEnableInputSystemMotion;
		oInputSettings.iOS.motionUsage.usageDescription = CPlatformOptsSetter.OptsInfoTable.BuildOptsInfo.m_oInputSystemMotionDesc;
#endif // #if UNITY_IOS

		CEditorFunc.SaveAsset(oInputSettings);
#endif // #if INPUT_SYSTEM_MODULE_ENABLE
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR
