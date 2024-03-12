using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;

#if LOCALIZE_MODULE_ENABLE
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
#endif // #if LOCALIZE_MODULE_ENABLE

/** 공용 에디터 씬 관리자 - 지역화 */
public static partial class CCommonEditorSceneManager
{
	#region 클래스 함수
	/** 지역화 정보를 설정한다 */
	private static void SetupLocalizeInfos()
	{
		// 지역화 정보 테이블이 없을 경우
		if(CPlatformOptsSetter.LocalizeInfoTable == null)
		{
			return;
		}

		for(int i = 0; i < CPlatformOptsSetter.LocalizeInfoTable.LocalizeInfoList.Count; ++i)
		{
			for(int j = 0; j < CPlatformOptsSetter.LocalizeInfoTable.LocalizeInfoList[i].m_oFontSetInfoList.Count; ++j)
			{
				var stFontSetInfo = CPlatformOptsSetter.LocalizeInfoTable.LocalizeInfoList[i].m_oFontSetInfoList[j];
				stFontSetInfo.m_eSet = EFontSet._1 + j;

				CPlatformOptsSetter.LocalizeInfoTable.LocalizeInfoList[i].m_oFontSetInfoList[j] = stFontSetInfo;
			}
		}
	}

	/** 지역화를 설정한다 */
	private static void SetupLocalize()
	{
#if LOCALIZE_MODULE_ENABLE
		// 지역화 설정이 없을 경우
		if(!EditorBuildSettings.TryGetConfigObject(KCEditorDefine.B_MODULE_N_LOCALIZE_SETTINGS, out LocalizationSettings oLocalizeSettings))
		{
			oLocalizeSettings = AssetDatabase.LoadAssetAtPath<LocalizationSettings>(KCEditorDefine.B_ASSET_P_LOCALIZE_SETTINGS);
			oLocalizeSettings = oLocalizeSettings ?? CEditorFactory.CreateScriptableObj<LocalizationSettings>(KCEditorDefine.B_ASSET_P_LOCALIZE_SETTINGS);

			EditorBuildSettings.AddConfigObject(KCEditorDefine.B_MODULE_N_LOCALIZE_SETTINGS, oLocalizeSettings, true);
		}

		var oSerializeObj = CEditorFactory.CreateSerializeObj(KCEditorDefine.B_ASSET_P_LOCALIZE_SETTINGS);

		var oIsSetupOptsList = new List<bool>() {
			oSerializeObj.FindProperty(KCEditorDefine.B_PROPERTY_N_LOCALIZE_INITIALIZE_SYNCHRONOUSLY).boolValue
		};

		// 설정 갱신이 필요 없을 경우
		if(!oIsSetupOptsList.Contains(false))
		{
			return;
		}

		oSerializeObj.ExSetPropertyVal(KCEditorDefine.B_PROPERTY_N_LOCALIZE_INITIALIZE_SYNCHRONOUSLY,
			(a_oProperty) => a_oProperty.boolValue = true);

		CEditorFunc.SaveAsset(oLocalizeSettings);
#endif // #if LOCALIZE_MODULE_ENABLE
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR
