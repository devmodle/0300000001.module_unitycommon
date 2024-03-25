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
		if(!CEditorAccess.IsExistsAsset(KCEditorDefine.G_ASSET_P_URP_SETTINGS_GLOBAL))
		{
			return;
		}

		var oSerializeObj = CEditorFactory.CreateSerializeObj(KCEditorDefine.G_ASSET_P_URP_SETTINGS_GLOBAL);

		var oIsSetupOptsList = new List<bool>() {
			oSerializeObj.FindProperty(KCEditorDefine.G_PROPERTY_N_URP_VARIANTS_DEBUG_STRIP).boolValue,
			oSerializeObj.FindProperty(KCEditorDefine.G_PROPERTY_N_URP_VARIANTS_UNUSED_STRIP).boolValue,
			oSerializeObj.FindProperty(KCEditorDefine.G_PROPERTY_N_URP_VARIANTS_PROCESSING_POST_UNUSED_STRIP).boolValue,

			oSerializeObj.FindProperty(KCEditorDefine.G_PROPERTY_N_URP_LEVEL_LOG_VARIANT_SHADER).intValue == (int)EShaderVariantLogLevel.ALL_SHADERS
		};

		// 설정 갱신이 필요 없을 경우
		if(!oIsSetupOptsList.Contains(false))
		{
			return;
		}

		oSerializeObj.ExSetPropertyVal(KCEditorDefine.G_PROPERTY_N_URP_VARIANTS_DEBUG_STRIP, (a_oProperty) => a_oProperty.boolValue = true);
		oSerializeObj.ExSetPropertyVal(KCEditorDefine.G_PROPERTY_N_URP_VARIANTS_UNUSED_STRIP, (a_oProperty) => a_oProperty.boolValue = true);
		oSerializeObj.ExSetPropertyVal(KCEditorDefine.G_PROPERTY_N_URP_VARIANTS_PROCESSING_POST_UNUSED_STRIP, (a_oProperty) => a_oProperty.boolValue = true);

		oSerializeObj.ExSetPropertyVal(KCEditorDefine.G_PROPERTY_N_URP_LEVEL_LOG_VARIANT_SHADER, (a_oProperty) => a_oProperty.intValue = (int)EShaderVariantLogLevel.ALL_SHADERS);
#endif // #if URP_MODULE_ENABLE
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR
