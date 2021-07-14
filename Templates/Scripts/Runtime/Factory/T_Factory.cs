using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 기본 팩토리
public static partial class Factory {
	#region 클래스 함수
	//! 레벨 식별자를 생성한다
	public static long MakeLevelID(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		int nStageID = Mathf.Max(a_nStageID, KCDefine.B_VAL_0_INT);
		int nChapterID = Mathf.Max(a_nChapterID, KCDefine.B_VAL_0_INT);

		return a_nID + (nStageID * (long)KCDefine.B_UNIT_IDS_PER_STAGE) + (nChapterID * (long)KCDefine.B_UNIT_IDS_PER_CHAPTER);
	}

	//! 클리어 정보를 생성한다
	public static CClearInfo MakeClearInfo(long a_nID, int a_nScore = KCDefine.B_VAL_0_INT, int a_nNumStars = KCDefine.B_VAL_0_INT) {
		return new CClearInfo() {
			m_nID = a_nID,
			
			Score = a_nScore,
			NumStars = a_nNumStars
		};
	}

	//! 레벨 정보를 생성한다
	public static CLevelInfo MakeLevelInfo(int a_nID, ELevelMode a_eLevelMode, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		return new CLevelInfo() {
			ID = a_nID,
			StageID = a_nStageID,
			ChapterID = a_nChapterID,

			LevelMode = a_eLevelMode
		};
	}
	#endregion			// 클래스 함수

	#region 조건부 클래스 함수
#if FIREBASE_MODULE_ENABLE
	//! 유저 정보 노드를 생성한다
	public static List<string> MakeUserInfoNodes() {
		return CFactory.MakeUserInfoNodes();
	}

	//! 지급 아이템 정보 노드를 생성한다
	public static List<string> MakePostItemInfoNodes() {
		return CFactory.MakePostItemInfoNodes();
	}
#endif			// #if FIREBASE_MODULE_ENABLE
	#endregion			// 조건부 클래스 함수
}
#endif			// #if NEVER_USE_THIS
