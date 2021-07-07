using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 기본 팩토리
public static partial class Factory {
	#region 클래스 함수
	//! 레벨 식별자를 생성한다
	public static int MakeLevelID(int a_nID) {
		return Factory.MakeLevelID(a_nID, KCDefine.B_VAL_0_INT);
	}

	//! 레벨 식별자를 생성한다
	public static int MakeLevelID(int a_nID, int a_nStageID) {
		return Factory.MakeLevelID(a_nID, a_nStageID, KCDefine.B_VAL_0_INT);
	}

	//! 레벨 식별자를 생성한다
	public static int MakeLevelID(int a_nID, int a_nStageID, int a_nChapterID) {
		return a_nID + (a_nStageID * KCDefine.B_UNIT_IDS_PER_STAGE) + (a_nChapterID * KCDefine.B_UNIT_IDS_PER_CHAPTER);
	}

	//! 레벨 정보를 생성한다
	public static CLevelInfo MakeLevelInfo(ELevelMode a_eLevelMode) {
		return Factory.MakeLevelInfo(KCDefine.B_VAL_0_INT, a_eLevelMode);
	}

	//! 레벨 정보를 생성한다
	public static CLevelInfo MakeLevelInfo(int a_nStageID, ELevelMode a_eLevelMode) {
		return Factory.MakeLevelInfo(a_nStageID, KCDefine.B_VAL_0_INT, a_eLevelMode);
	}

	//! 레벨 정보를 생성한다
	public static CLevelInfo MakeLevelInfo(int a_nStageID, int a_nChapterID, ELevelMode a_eLevelMode) {
		return new CLevelInfo() {
			ID = Factory.MakeLevelID(CLevelInfoTable.Inst.LevelInfoList.Count, a_nStageID, a_nChapterID),
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
