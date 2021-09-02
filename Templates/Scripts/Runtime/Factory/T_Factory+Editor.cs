using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
#if UNITY_EDITOR || UNITY_STANDALONE
//! 에디터 팩토리
public static partial class Factory {
	#region 클래스 함수
	//! 셀 정보를 생성한다
	public static CCellInfo MakeCellInfo(Vector3Int a_stIdx) {
		return new CCellInfo() {
			m_stIdxInfo = CFactory.MakeIdxInfo(a_stIdx.x, a_stIdx.y, a_stIdx.z)
		};
	}

	//! 레벨 정보를 생성한다
	public static CLevelInfo MakeLevelInfo(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		return new CLevelInfo() {
			m_stIDInfo = CFactory.MakeIDInfo(a_nID, a_nStageID, a_nChapterID)
		};
	}

	//! 블럭 스프라이트를 생성한다
	public static SpriteRenderer CreateBlockSprite(SampleEngineName.EBlockKinds a_eBlockKinds, GameObject a_oParent) {
		var oBlockSprite = CFactory.CreateCloneObj<SpriteRenderer>(KCDefine.E_OBJ_N_BLOCK_SPRITE, KCDefine.U_OBJ_P_SPRITE, a_oParent);
		oBlockSprite.sprite = SampleEngineName.Access.GetBlockSprite(a_eBlockKinds);
		oBlockSprite.ExSetSortingOrder(SampleEngineName.Access.GetSortingOrder(a_eBlockKinds));

		return oBlockSprite;
	}
	#endregion			// 클래스 함수

	#region 추가 클래스 함수

	#endregion			// 추가 클래스 함수
}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
#endif			// #if NEVER_USE_THIS
