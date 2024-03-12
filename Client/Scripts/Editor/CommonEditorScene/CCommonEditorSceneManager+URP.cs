using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
/** 공용 에디터 씬 관리자 - URP */
public static partial class CCommonEditorSceneManager
{
	#region 클래스 함수
	/** 렌더링 파이프라인을 설정한다 */
	private static void SetupURP()
	{
#if URP_MODULE_ENABLE
		// URP 전역 설정이 없을 경우
		if(!CEditorAccess.IsExistsAsset(KCEditorDefine.B_ASSET_P_URP_GLOBAL_SETTINGS))
		{
			return;
		}

		var oSerializeObj = CEditorFactory.CreateSerializeObj(KCEditorDefine.B_ASSET_P_URP_GLOBAL_SETTINGS);

		var oIsSetupOptsList = new List<bool>() {
			oSerializeObj.FindProperty(KCEditorDefine.B_PROPERTY_N_URP_STRIP_DEBUG_VARIANTS).boolValue,
			oSerializeObj.FindProperty(KCEditorDefine.B_PROPERTY_N_URP_STRIP_UNUSED_VARIANTS).boolValue,
			oSerializeObj.FindProperty(KCEditorDefine.B_PROPERTY_N_URP_STRIP_UNUSED_POST_PROCESSING_VARIANTS).boolValue,

			oSerializeObj.FindProperty(KCEditorDefine.B_PROPERTY_N_URP_SHADER_VARIANT_LOG_LEVEL).intValue == (int)EShaderVariantLogLevel.ALL_SHADERS
		};

		// 설정 갱신이 필요 없을 경우
		if(!oIsSetupOptsList.Contains(false))
		{
			return;
		}

		oSerializeObj.ExSetPropertyVal(KCEditorDefine.B_PROPERTY_N_URP_STRIP_DEBUG_VARIANTS, (a_oProperty) => a_oProperty.boolValue = true);
		oSerializeObj.ExSetPropertyVal(KCEditorDefine.B_PROPERTY_N_URP_STRIP_UNUSED_VARIANTS, (a_oProperty) => a_oProperty.boolValue = true);
		oSerializeObj.ExSetPropertyVal(KCEditorDefine.B_PROPERTY_N_URP_STRIP_UNUSED_POST_PROCESSING_VARIANTS, (a_oProperty) => a_oProperty.boolValue = true);

		oSerializeObj.ExSetPropertyVal(KCEditorDefine.B_PROPERTY_N_URP_SHADER_VARIANT_LOG_LEVEL, (a_oProperty) => a_oProperty.intValue = (int)EShaderVariantLogLevel.ALL_SHADERS);
#endif // #if URP_MODULE_ENABLE
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR
