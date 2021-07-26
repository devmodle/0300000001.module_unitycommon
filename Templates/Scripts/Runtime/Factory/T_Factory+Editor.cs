using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
#if UNITY_EDITOR || UNITY_STANDALONE
//! 에디터 팩토리
public static partial class Factory {
	#region 클래스 함수
	//! 셀 정보를 생성한다
	public static CCellInfo MakeCellInfo() {
		return new CCellInfo();
	}

	//! 레벨 정보를 생성한다
	public static CLevelInfo MakeLevelInfo(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		return new CLevelInfo() {
			m_stIDInfo = CFactory.MakeIDInfo(a_nID, a_nStageID, a_nChapterID)
		};
	}
	#endregion			// 클래스 함수
}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
#endif			// #if NEVER_USE_THIS
