#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
/** 에디터 씬 접근자 */
public static partial class Access {
	#region 클래스 함수

	#endregion // 클래스 함수

	#region 조건부 클래스 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
	/** 에디터 스프라이트를 반환한다 */
	public static Sprite GetEditorSprite(EObjKinds a_eObjKinds, string a_oPrefix, bool a_bIsIgnoreEditor = false, EKindsGroupType a_eGroupType = EKindsGroupType.SUB_KINDS_TYPE) {
		var oImgPathDict = CCollectionPoolManager.Inst.SpawnDict<EObjKinds, string>();

		try {
			// 에디터 무시 모드가 아닐 경우
			if(!a_bIsIgnoreEditor) {
				NSEngine.KDefine.E_IMG_P_OBJ_DICT.ExCopyTo(oImgPathDict, (a_oImgPath) => a_oImgPath);
			}

			string oStr = CStrTable.Inst.GetEnumStr(typeof(EObjKinds), ((int)a_eObjKinds).ExKindsToDetailSubKindsType());
			string oImgPath = oImgPathDict.GetValueOrDefault((EObjKinds)((int)a_eObjKinds).ExKindsToDetailSubKindsType(), oStr);
			string oEditorImgPath = string.Format(KCDefine.B_TEXT_FMT_2_COMBINE, a_oPrefix, oImgPath);

			return CResManager.Inst.GetRes<Sprite>(oEditorImgPath) ?? NSEngine.Access.GetSprite(a_eObjKinds, a_eGroupType);
		} finally {
			CCollectionPoolManager.Inst.DespawnDict(oImgPathDict);
		}
	}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endregion // 조건부 클래스 함수
}

/** 레벨 에디터 씬 접근자 */
public static partial class Access {
	#region 클래스 함수

	#endregion // 클래스 함수
}
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
#endif // #if SCRIPT_TEMPLATE_ONLY
