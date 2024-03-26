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
		if(!CAccessEditor.IsExistsAsset(KCDefineEditor.G_URP_ASSET_P_SETTINGS_GLOBAL))
		{
			return;
		}

		var oSerializeObj = CFactoryEditor.CreateSerializeObj(KCDefineEditor.G_URP_ASSET_P_SETTINGS_GLOBAL);

		var oListIsSetupOpts = new List<bool>() {
			oSerializeObj.FindProperty(KCDefineEditor.G_URP_PROPERTY_N_VARIANTS_DEBUG_STRIP).boolValue,
			oSerializeObj.FindProperty(KCDefineEditor.G_URP_PROPERTY_N_VARIANTS_UNUSED_STRIP).boolValue,
			oSerializeObj.FindProperty(KCDefineEditor.G_URP_PROPERTY_N_VARIANTS_PROCESSING_POST_UNUSED_STRIP).boolValue,

			oSerializeObj.FindProperty(KCDefineEditor.G_URP_PROPERTY_N_LEVEL_LOG_VARIANT_SHADER).intValue == (int)ELevelLogVariantShader.ALL_SHADERS
		};

		// 설정 갱신이 필요 없을 경우
		if(!oListIsSetupOpts.Contains(false))
		{
			return;
		}

		oSerializeObj.ExSetPropertyVal(KCDefineEditor.G_URP_PROPERTY_N_VARIANTS_DEBUG_STRIP, (a_oProperty) => a_oProperty.boolValue = true);
		oSerializeObj.ExSetPropertyVal(KCDefineEditor.G_URP_PROPERTY_N_VARIANTS_UNUSED_STRIP, (a_oProperty) => a_oProperty.boolValue = true);
		oSerializeObj.ExSetPropertyVal(KCDefineEditor.G_URP_PROPERTY_N_VARIANTS_PROCESSING_POST_UNUSED_STRIP, (a_oProperty) => a_oProperty.boolValue = true);

		oSerializeObj.ExSetPropertyVal(KCDefineEditor.G_URP_PROPERTY_N_LEVEL_LOG_VARIANT_SHADER, (a_oProperty) => a_oProperty.intValue = (int)ELevelLogVariantShader.ALL_SHADERS);
#endif // #if URP_MODULE_ENABLE
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR
