using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 기본 팩토리
public static partial class Factory {
	#region 클래스 함수
	//! 레벨 식별자를 생성한다
	public static long MakeLevelID(int a_nID) {
		return Factory.MakeLevelID(a_nID, EStageKinds.NONE);
	}

	//! 레벨 식별자를 생성한다
	public static long MakeLevelID(int a_nID, EStageKinds a_eStageKinds) {
		return Factory.MakeLevelID(a_nID, EStageKinds.NONE, EChapterKinds.NONE);
	}

	//! 레벨 식별자를 생성한다
	public static long MakeLevelID(int a_nID, EStageKinds a_eStageKinds, EChapterKinds a_eChapterKinds) {
		int nStageID = Mathf.Max((int)a_eStageKinds, KCDefine.B_VAL_0_INT);
		int nChapterID = Mathf.Max((int)a_eChapterKinds, KCDefine.B_VAL_0_INT);

		return a_nID + (nStageID * (long)KCDefine.B_UNIT_IDS_PER_STAGE) + (nChapterID * (long)KCDefine.B_UNIT_IDS_PER_CHAPTER);
	}

	//! 레벨 정보를 생성한다
	public static CLevelInfo MakeLevelInfo(ELevelMode a_eLevelMode) {
		return Factory.MakeLevelInfo(EStageKinds.NONE, a_eLevelMode);
	}

	//! 레벨 정보를 생성한다
	public static CLevelInfo MakeLevelInfo(EStageKinds a_eStageKinds, ELevelMode a_eLevelMode) {
		return Factory.MakeLevelInfo(a_eStageKinds, EChapterKinds.NONE, a_eLevelMode);
	}

	//! 레벨 정보를 생성한다
	public static CLevelInfo MakeLevelInfo(EStageKinds a_eStageKinds, EChapterKinds a_eChapterKinds, ELevelMode a_eLevelMode) {
		return new CLevelInfo() {
			ID = Factory.MakeLevelID(CLevelInfoTable.Inst.LevelInfoList.Count, a_eStageKinds, a_eChapterKinds),

			StageKinds = a_eStageKinds,
			ChapterKinds = a_eChapterKinds,
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
