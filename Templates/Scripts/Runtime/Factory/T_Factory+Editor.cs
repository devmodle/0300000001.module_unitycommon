using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if UNITY_STANDALONE && EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
/** 에디터 팩토리 */
public static partial class Factory {
	#region 클래스 함수
	
	#endregion			// 클래스 함수

	#region 조건부 클래스 함수
#if ENGINE_TEMPLATES_MODULE_ENABLE
	/** 블럭 스프라이트를 생성한다 */
	public static SpriteRenderer CreateBlockSprite(EBlockKinds a_eBlockKinds, GameObject a_oParent) {
		var oBlockSprite = CFactory.CreateCloneObj<SpriteRenderer>(KCDefine.E_OBJ_N_BLOCK_SPRITE, KCDefine.U_OBJ_P_SPRITE, a_oParent);
		oBlockSprite.sprite = SampleEngineName.Access.GetBlockSprite(a_eBlockKinds);
		oBlockSprite.ExSetSortingOrder(SampleEngineName.Access.GetSortingOrder(a_eBlockKinds));

		return oBlockSprite;
	}
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE

#if RUNTIME_TEMPLATES_MODULE_ENABLE
	/** 셀 정보를 생성한다 */
	public static CCellInfo MakeCellInfo(Vector3Int a_stIdx) {
		return new CCellInfo() {
			m_stIdx = a_stIdx
		};
	}

	/** 레벨 정보를 생성한다 */
	public static CLevelInfo MakeLevelInfo(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		return new CLevelInfo() {
			m_stIDInfo = CFactory.MakeIDInfo(a_nID, a_nStageID, a_nChapterID)
		};
	}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
	#endregion			// 조건부 클래스 함수

	#region 추가 클래스 함수

	#endregion			// 추가 클래스 함수
}
#endif			// #if UNITY_STANDALONE && EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
#endif			// #if SCRIPT_TEMPLATE_ONLY
