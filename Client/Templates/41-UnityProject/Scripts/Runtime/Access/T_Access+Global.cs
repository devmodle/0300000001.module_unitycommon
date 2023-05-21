#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.IO;
using System.Linq;

/** 전역 접근자 */
public static partial class Access {
	#region 클래스 프로퍼티
	public static string StoreURL {
		get {
#if UNITY_IOS
			return string.Format(KCDefine.U_FMT_STORE_URL, CProjInfoTable.Inst.ProjInfo.m_oStoreAppID);
#else
			return string.Format(KCDefine.U_FMT_STORE_URL, CProjInfoTable.Inst.GetAppID(CProjInfoTable.Inst.ProjInfo));
#endif // #if UNITY_IOS
		}
	}

	public static string EtcInfoTableLoadPath {
		get {
#if AB_TEST_ENABLE
			string oTablePath = (CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.B) ? KCDefine.U_RUNTIME_TABLE_P_G_ETC_INFO_SET_B : KCDefine.U_RUNTIME_TABLE_P_G_ETC_INFO_SET_A;
			return File.Exists(oTablePath) ? oTablePath : (CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.B) ? KCDefine.U_TABLE_P_G_ETC_INFO_SET_B : KCDefine.U_TABLE_P_G_ETC_INFO_SET_A;
#else
			return File.Exists(KCDefine.U_RUNTIME_TABLE_P_G_ETC_INFO) ? KCDefine.U_RUNTIME_TABLE_P_G_ETC_INFO : KCDefine.U_TABLE_P_G_ETC_INFO;
#endif // #if AB_TEST_ENABLE
		}
	}

	public static string EtcInfoTableSavePath {
		get {
#if AB_TEST_ENABLE
			return (CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.B) ? KCDefine.U_RUNTIME_TABLE_P_G_ETC_INFO_SET_B : KCDefine.U_RUNTIME_TABLE_P_G_ETC_INFO_SET_A;
#else
			return KCDefine.U_RUNTIME_TABLE_P_G_ETC_INFO;
#endif // #if AB_TEST_ENABLE
		}
	}

	public static string LevelInfoTableLoadPath {
		get {
#if AB_TEST_ENABLE
			string oTablePath = (CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.B) ? KCDefine.U_RUNTIME_TABLE_P_G_LEVEL_INFO_SET_B : KCDefine.U_RUNTIME_TABLE_P_G_LEVEL_INFO_SET_A;
			return File.Exists(oTablePath) ? oTablePath : (CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.B) ? KCDefine.U_TABLE_P_G_LEVEL_INFO_SET_B : KCDefine.U_TABLE_P_G_LEVEL_INFO_SET_A;
#else
			return File.Exists(KCDefine.U_RUNTIME_TABLE_P_G_LEVEL_INFO) ? KCDefine.U_RUNTIME_TABLE_P_G_LEVEL_INFO : KCDefine.U_TABLE_P_G_LEVEL_INFO;
#endif // #if AB_TEST_ENABLE
		}
	}

	public static string LevelInfoTableSavePath {
		get {
#if AB_TEST_ENABLE
			return (CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.B) ? KCDefine.U_RUNTIME_TABLE_P_G_LEVEL_INFO_SET_B : KCDefine.U_RUNTIME_TABLE_P_G_LEVEL_INFO_SET_A;
#else
			return KCDefine.U_RUNTIME_TABLE_P_G_LEVEL_INFO;
#endif // #if AB_TEST_ENABLE
		}
	}

	public static string MoreAppsURL => string.Format(KCDefine.U_FMT_MORE_APPS_URL, CProjInfoTable.Inst.ProjInfo.m_oStoreAppID);

	public static string CalcInfoTableLoadPath => Access.EtcInfoTableLoadPath;
	public static string CalcInfoTableSavePath => Access.EtcInfoTableSavePath;

	public static string MissionInfoTableLoadPath => File.Exists(KCDefine.U_RUNTIME_TABLE_P_G_MISSION_INFO) ? KCDefine.U_RUNTIME_TABLE_P_G_MISSION_INFO : KCDefine.U_TABLE_P_G_MISSION_INFO;
	public static string MissionInfoTableSavePath => KCDefine.U_RUNTIME_TABLE_P_G_MISSION_INFO;

	public static string RewardInfoTableLoadPath => File.Exists(KCDefine.U_RUNTIME_TABLE_P_G_REWARD_INFO) ? KCDefine.U_RUNTIME_TABLE_P_G_REWARD_INFO : KCDefine.U_TABLE_P_G_REWARD_INFO;
	public static string RewardInfoTableSavePath => KCDefine.U_RUNTIME_TABLE_P_G_REWARD_INFO;

	public static string EpisodeInfoTableLoadPath => Access.EtcInfoTableLoadPath;
	public static string EpisodeInfoTableSavePath => Access.EtcInfoTableSavePath;

	public static string TutorialInfoTableLoadPath => Access.EtcInfoTableLoadPath;
	public static string TutorialInfoTableSavePath => Access.EtcInfoTableSavePath;

	public static string ResInfoTableLoadPath => File.Exists(KCDefine.U_RUNTIME_TABLE_P_G_RES_INFO) ? KCDefine.U_RUNTIME_TABLE_P_G_RES_INFO : KCDefine.U_TABLE_P_G_RES_INFO;
	public static string ResInfoTableSavePath => KCDefine.U_RUNTIME_TABLE_P_G_RES_INFO;

	public static string ItemInfoTableLoadPath => File.Exists(KCDefine.U_RUNTIME_TABLE_P_G_ITEM_INFO) ? KCDefine.U_RUNTIME_TABLE_P_G_ITEM_INFO : KCDefine.U_TABLE_P_G_ITEM_INFO;
	public static string ItemInfoTableSavePath => KCDefine.U_RUNTIME_TABLE_P_G_ITEM_INFO;

	public static string SkillInfoTableLoadPath => File.Exists(KCDefine.U_RUNTIME_TABLE_P_G_SKILL_INFO) ? KCDefine.U_RUNTIME_TABLE_P_G_SKILL_INFO : KCDefine.U_TABLE_P_G_SKILL_INFO;
	public static string SkillInfoTableSavePath => KCDefine.U_RUNTIME_TABLE_P_G_SKILL_INFO;

	public static string ObjInfoTableLoadPath => File.Exists(KCDefine.U_RUNTIME_TABLE_P_G_OBJ_INFO) ? KCDefine.U_RUNTIME_TABLE_P_G_OBJ_INFO : KCDefine.U_TABLE_P_G_OBJ_INFO;
	public static string ObjInfoTableSavePath => KCDefine.U_RUNTIME_TABLE_P_G_OBJ_INFO;

	public static string FXInfoTableLoadPath => Access.EtcInfoTableLoadPath;
	public static string FXInfoTableSavePath => Access.EtcInfoTableSavePath;

	public static string AbilityInfoTableLoadPath => File.Exists(KCDefine.U_RUNTIME_TABLE_P_G_ABILITY_INFO) ? KCDefine.U_RUNTIME_TABLE_P_G_ABILITY_INFO : KCDefine.U_TABLE_P_G_ABILITY_INFO;
	public static string AbilityInfoTableSavePath => KCDefine.U_RUNTIME_TABLE_P_G_ABILITY_INFO;

	public static string ProductTradeInfoTableLoadPath => CAccess.ProductInfoTableLoadPath;
	public static string ProductTradeInfoTableSavePath => CAccess.ProductInfoTableSavePath;

	public static STTableInfo CalcTableInfo => Access.GoogleSheetTableInfo.GetValueOrDefault(Access.CalcInfoTableLoadPath.ExGetFileName(false));
	public static STTableInfo MissionTableInfo => Access.GoogleSheetTableInfo.GetValueOrDefault(Access.MissionInfoTableLoadPath.ExGetFileName(false));
	public static STTableInfo RewardTableInfo => Access.GoogleSheetTableInfo.GetValueOrDefault(Access.RewardInfoTableLoadPath.ExGetFileName(false));
	public static STTableInfo EpisodeTableInfo => Access.GoogleSheetTableInfo.GetValueOrDefault(Access.EpisodeInfoTableLoadPath.ExGetFileName(false));
	public static STTableInfo TutorialTableInfo => Access.GoogleSheetTableInfo.GetValueOrDefault(Access.TutorialInfoTableLoadPath.ExGetFileName(false));
	public static STTableInfo ResTableInfo => Access.GoogleSheetTableInfo.GetValueOrDefault(Access.ResInfoTableLoadPath.ExGetFileName(false));
	public static STTableInfo ItemTableInfo => Access.GoogleSheetTableInfo.GetValueOrDefault(Access.ItemInfoTableLoadPath.ExGetFileName(false));
	public static STTableInfo SkillTableInfo => Access.GoogleSheetTableInfo.GetValueOrDefault(Access.SkillInfoTableLoadPath.ExGetFileName(false));
	public static STTableInfo ObjTableInfo => Access.GoogleSheetTableInfo.GetValueOrDefault(Access.ObjInfoTableLoadPath.ExGetFileName(false));
	public static STTableInfo FXTableInfo => Access.GoogleSheetTableInfo.GetValueOrDefault(Access.FXInfoTableLoadPath.ExGetFileName(false));
	public static STTableInfo AbilityTableInfo => Access.GoogleSheetTableInfo.GetValueOrDefault(Access.AbilityInfoTableLoadPath.ExGetFileName(false));
	public static STTableInfo ProductTradeTableInfo => Access.GoogleSheetTableInfo.GetValueOrDefault(Access.ProductTradeInfoTableLoadPath.ExGetFileName(false));
	#endregion // 클래스 프로퍼티

	#region 클래스 함수
	/** 레벨 클리어 여부를 검사한다 */
	public static bool IsClearLevel(int a_nCharacterID, int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo);
		return oCharacterGameInfo != null && oCharacterGameInfo.m_oLevelClearInfoDict.ContainsKey(CFactory.MakeULevelID(a_nLevelID, a_nStageID, a_nChapterID));
	}

	/** 스테이지 클리어 여부를 검사한다 */
	public static bool IsClearStage(int a_nCharacterID, int a_nStageID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo);
		return oCharacterGameInfo != null && oCharacterGameInfo.m_oStageClearInfoDict.ContainsKey(CFactory.MakeUStageID(a_nStageID, a_nChapterID));
	}

	/** 챕터 클리어 여부를 검사한다 */
	public static bool IsClearChapter(int a_nCharacterID, int a_nChapterID) {
		CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo);
		return oCharacterGameInfo != null && oCharacterGameInfo.m_oChapterClearInfoDict.ContainsKey(CFactory.MakeUChapterID(a_nChapterID));
	}

	/** 레벨 잠금 해제 여부를 검사한다 */
	public static bool IsUnlockLevel(int a_nCharacterID, int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo);
		return oCharacterGameInfo != null && oCharacterGameInfo.m_oUnlockULevelIDList.Contains(CFactory.MakeULevelID(a_nLevelID, a_nStageID, a_nChapterID));
	}

	/** 스테이지 잠금 해제 여부를 검사한다 */
	public static bool IsUnlockStage(int a_nCharacterID, int a_nStageID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo);
		return oCharacterGameInfo != null && oCharacterGameInfo.m_oUnlockUStageIDList.Contains(CFactory.MakeUStageID(a_nStageID, a_nChapterID));
	}

	/** 챕터 잠금 해제 여부를 검사한다 */
	public static bool IsUnlockChapter(int a_nCharacterID, int a_nChapterID) {
		CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo);
		return oCharacterGameInfo != null && oCharacterGameInfo.m_oUnlockUChapterIDList.Contains(CFactory.MakeUChapterID(a_nChapterID));
	}

	/** 레벨 보상 획득 여부를 검사한다 */
	public static bool IsAcquireLevelReward(int a_nCharacterID, int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo);
		return oCharacterGameInfo != null && oCharacterGameInfo.m_oAcquireRewardULevelIDList.Contains(CFactory.MakeULevelID(a_nLevelID, a_nStageID, a_nChapterID));
	}

	/** 스테이지 보상 획득 여부를 검사한다 */
	public static bool IsAcquireStageReward(int a_nCharacterID, int a_nStageID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo);
		return oCharacterGameInfo != null && oCharacterGameInfo.m_oAcquireRewardUStageIDList.Contains(CFactory.MakeUStageID(a_nStageID, a_nChapterID));
	}

	/** 챕터 보상 획득 여부를 검사한다 */
	public static bool IsAcquireChapterReward(int a_nCharacterID, int a_nChapterID) {
		CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo);
		return oCharacterGameInfo != null && oCharacterGameInfo.m_oAcquireRewardUChapterIDList.Contains(CFactory.MakeUChapterID(a_nChapterID));
	}

	/** 일일 보상 지속 획득 여부를 검사한다 */
	public static bool IsContinueGetDailyReward(int a_nCharacterID) {
		return CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo) ? System.DateTime.Now.ExGetDeltaTimePerDays(oCharacterGameInfo.PrevDailyRewardTime).ExIsLess(KCDefine.B_VAL_2_REAL) : false;
	}

	/** 무료 보상 획득 가능 여부를 검사한다 */
	public static bool IsEnableGetFreeReward(int a_nCharacterID) {
		return CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo) ? System.DateTime.Now.ExGetDeltaTimePerDays(oCharacterGameInfo.PrevFreeRewardTime).ExIsGreateEquals(KCDefine.B_VAL_1_REAL) : false;
	}

	/** 일일 보상 획득 가능 여부를 검사한다 */
	public static bool IsEnableGetDailyReward(int a_nCharacterID) {
		return CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo) ? System.DateTime.Now.ExGetDeltaTimePerDays(oCharacterGameInfo.PrevDailyRewardTime).ExIsGreateEquals(KCDefine.B_VAL_1_REAL) : false;
	}

	/** 일일 미션 리셋 가능 여부를 검사한다 */
	public static bool IsEnableResetDailyMission(int a_nCharacterID) {
		return CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo) ? System.DateTime.Now.ExGetDeltaTimePerDays(oCharacterGameInfo.PrevDailyMissionTime).ExIsGreateEquals(KCDefine.B_VAL_1_REAL) : false;
	}

	/** 교환 가능 여부를 검사한다 */
	public static bool IsEnableTrade(int a_nCharacterID, STTargetInfo a_stTargetInfo) {
		switch(a_stTargetInfo.TargetType) {
			case ETargetType.ITEM: return Access.IsEnableItemTargetTrade(a_stTargetInfo, Access.GetItemTargetInfo(a_nCharacterID, (EItemKinds)a_stTargetInfo.Kinds));
			case ETargetType.SKILL: return Access.IsEnableSkillTargetTrade(a_stTargetInfo, Access.GetSkillTargetInfo(a_nCharacterID, (ESkillKinds)a_stTargetInfo.Kinds));
			case ETargetType.OBJ: return Access.IsEnableObjTargetTrade(a_stTargetInfo, Access.GetObjTargetInfo(a_nCharacterID, (EObjKinds)a_stTargetInfo.Kinds));
			case ETargetType.ABILITY: return Access.IsEnableAbilityTargetTrade(a_stTargetInfo, Access.GetAbilityTargetInfo(a_nCharacterID, (EAbilityKinds)a_stTargetInfo.Kinds));
		}

		return false;
	}

	/** 교환 가능 여부를 검사한다 */
	public static bool IsEnableTrade(int a_nCharacterID, STTargetInfo a_stTargetInfo, CTargetInfo a_oTargetInfo) {
		// 어빌리티 타겟 정보가 아닐 경우
		if(a_stTargetInfo.m_eTargetKinds != ETargetKinds.ABILITY_TARGET) {
			return Access.IsEnableTrade(a_nCharacterID, a_stTargetInfo);
		} else {
			CAccess.Assert(CAbilityInfoTable.Inst.TryGetAbilityInfo((EAbilityKinds)a_stTargetInfo.Kinds, out STAbilityInfo stAbilityInfo));
			return a_oTargetInfo.m_oAbilityTargetInfoDict.GetValueOrDefault(Factory.MakeUTargetInfoID(a_stTargetInfo.m_eTargetKinds, a_stTargetInfo.Kinds), STTargetInfo.INVALID).m_stValInfo01.m_dmVal >= a_stTargetInfo.m_stValInfo01.m_dmVal;
		}
	}

	/** 교환 가능 여부를 검사한다 */
	public static bool IsEnableTrade(int a_nCharacterID, Dictionary<ulong, STTargetInfo> a_oTargetInfoDict) {
		CAccess.Assert(a_oTargetInfoDict != null);
		return a_oTargetInfoDict.All((a_stKeyVal) => Access.IsEnableTrade(a_nCharacterID, a_stKeyVal.Value));
	}

	/** 교환 가능 여부를 검사한다 */
	public static bool IsEnableTrade(int a_nCharacterID, Dictionary<ulong, STTargetInfo> a_oTargetInfoDict, CTargetInfo a_oTargetInfo) {
		CAccess.Assert(a_oTargetInfoDict != null);
		return a_oTargetInfoDict.All((a_stKeyVal) => Access.IsEnableTrade(a_nCharacterID, a_stKeyVal.Value, a_oTargetInfo));
	}

	/** 튜토리얼 메세지를 반환한다 */
	public static string GetTutorialMsg(ETutorialKinds a_eTutorialKinds, int a_nIdx = KCDefine.B_VAL_0_INT) {
		return CStrTable.Inst.GetStr(string.Format(KCDefine.U_KEY_FMT_TUTORIAL_MSG, a_eTutorialKinds, a_nIdx + KCDefine.B_VAL_1_INT));
	}

	/** 레벨 클리어 정보 개수를 반환한다 */
	public static int GetNumLevelClearInfos(int a_nCharacterID, int a_nStageID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		var oCharacterGameInfo = CGameInfoStorage.Inst.GetCharacterGameInfo(a_nCharacterID);
		return oCharacterGameInfo.m_oLevelClearInfoDict.Sum((a_stKeyVal) => (a_stKeyVal.Value.m_stIDInfo.m_nID02 == a_nStageID && a_stKeyVal.Value.m_stIDInfo.m_nID03 == a_nChapterID) ? KCDefine.B_VAL_1_INT : KCDefine.B_VAL_0_INT);
	}

	/** 스테이지 클리어 정보 개수를 반환한다 */
	public static int GetNumStageClearInfos(int a_nCharacterID, int a_nChapterID) {
		var oCharacterGameInfo = CGameInfoStorage.Inst.GetCharacterGameInfo(a_nCharacterID);
		return oCharacterGameInfo.m_oStageClearInfoDict.Sum((a_stKeyVal) => (a_stKeyVal.Value.m_stIDInfo.m_nID03 == a_nChapterID) ? KCDefine.B_VAL_1_INT : KCDefine.B_VAL_0_INT);
	}

	/** 챕터 클리어 정보 개수를 반환한다 */
	public static int GetNumChapterClearInfos(int a_nCharacterID) {
		var oCharacterGameInfo = CGameInfoStorage.Inst.GetCharacterGameInfo(a_nCharacterID);
		return oCharacterGameInfo.m_oChapterClearInfoDict.Count;
	}

	/** 클리어 마크 개수를 반환한다 */
	public static long GetTotalNumClearMarks(int a_nCharacterID) {
		return CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo) ? oCharacterGameInfo.m_oLevelClearInfoDict.Sum((a_stKeyVal) => a_stKeyVal.Value.NumMarks) : KCDefine.B_VAL_0_INT;
	}

	/** 아이템 타겟 값을 반환한다 */
	public static decimal GetItemTargetVal(int a_nCharacterID, EItemKinds a_eItemKinds, ETargetKinds a_eTargetKinds, int a_nKinds) {
		return Access.GetItemTargetInfo(a_nCharacterID, a_eItemKinds, true).m_oAbilityTargetInfoDict.ExGetTargetVal(a_eTargetKinds, a_nKinds);
	}

	/** 스킬 타겟 값을 반환한다 */
	public static decimal GetSkillTargetVal(int a_nCharacterID, ESkillKinds a_eSkillKinds, ETargetKinds a_eTargetKinds, int a_nKinds) {
		return Access.GetSkillTargetInfo(a_nCharacterID, a_eSkillKinds, true).m_oAbilityTargetInfoDict.ExGetTargetVal(a_eTargetKinds, a_nKinds);
	}

	/** 객체 타겟 값을 반환한다 */
	public static decimal GetObjTargetVal(int a_nCharacterID, EObjKinds a_eObjKinds, ETargetKinds a_eTargetKinds, int a_nKinds) {
		return Access.GetObjTargetInfo(a_nCharacterID, a_eObjKinds, true).m_oAbilityTargetInfoDict.ExGetTargetVal(a_eTargetKinds, a_nKinds);
	}

	/** 어빌리티 타겟 값을 반환한다 */
	public static decimal GetAbilityTargetVal(int a_nCharacterID, EAbilityKinds a_eAbilityKinds, ETargetKinds a_eTargetKinds, int a_nKinds) {
		return Access.GetAbilityTargetInfo(a_nCharacterID, a_eAbilityKinds, true).m_oAbilityTargetInfoDict.ExGetTargetVal(a_eTargetKinds, a_nKinds);
	}

	/** 일일 보상 식별자를 반환한다 */
	public static int GetDailyRewardID(int a_nCharacterID) {
		return CGameInfoStorage.Inst.GetCharacterGameInfo(a_nCharacterID).DailyRewardID;
	}

	/** 일일 보상 종류를 반환한다 */
	public static ERewardKinds GetDailyRewardKinds(int a_nCharacterID) {
		return KDefine.G_REWARDS_KINDS_DAILY_REWARD_LIST[Access.GetDailyRewardID(a_nCharacterID)];
	}

	/** 에피소드 정보를 반환한다 */
	public static STEpisodeInfo GetEpisodeInfo(int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		// 레벨 에피소드 정보가 존재 할 경우
		if(CEpisodeInfoTable.Inst.TryGetLevelEpisodeInfo(a_nLevelID, out STEpisodeInfo stLevelEpisodeInfo, a_nStageID, a_nChapterID)) {
			return stLevelEpisodeInfo;
		}

		bool bIsValid = CEpisodeInfoTable.Inst.TryGetStageEpisodeInfo(a_nStageID, out STEpisodeInfo stStageEpisodeInfo, a_nChapterID);
		return bIsValid ? stStageEpisodeInfo : CEpisodeInfoTable.Inst.TryGetChapterEpisodeInfo(a_nChapterID, out STEpisodeInfo stChapterEpisodeInfo) ? stChapterEpisodeInfo : STEpisodeInfo.INVALID;
	}

	/** 이전 에피소드 정보를 반환한다 */
	public static STEpisodeInfo GetPrevEpisodeInfo(int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		// 레벨 에피소드 정보가 존재 할 경우
		if(CEpisodeInfoTable.Inst.TryGetLevelEpisodeInfo(a_nLevelID - KCDefine.B_VAL_1_INT, out STEpisodeInfo stLevelEpisodeInfo, a_nStageID, a_nChapterID)) {
			return stLevelEpisodeInfo;
		}

		bool bIsValid = CEpisodeInfoTable.Inst.TryGetStageEpisodeInfo(a_nStageID - KCDefine.B_VAL_1_INT, out STEpisodeInfo stStageEpisodeInfo, a_nChapterID);
		return bIsValid ? stStageEpisodeInfo : CEpisodeInfoTable.Inst.TryGetChapterEpisodeInfo(a_nChapterID - KCDefine.B_VAL_1_INT, out STEpisodeInfo stChapterEpisodeInfo) ? stChapterEpisodeInfo : STEpisodeInfo.INVALID;
	}

	/** 이전 레벨 에피소드 정보를 반환한다 */
	public static STEpisodeInfo GetPrevLevelEpisodeInfo(int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		var stEpisodeInfo = Access.GetPrevEpisodeInfo(a_nLevelID, a_nStageID, a_nChapterID);

		// 레벨 에피소드 정보 일 경우
		if(stEpisodeInfo.EpisodeType == EEpisodeType.LEVEL) {
			return stEpisodeInfo;
		}
		// 스테이지 에피소드 정보 일 경우
		else if(stEpisodeInfo.EpisodeType == EEpisodeType.STAGE && CEpisodeInfoTable.Inst.TryGetLevelEpisodeInfo(stEpisodeInfo.m_nNumSubEpisodes - KCDefine.B_VAL_1_INT, out STEpisodeInfo stStageLevelEpisodeInfo, a_nStageID - KCDefine.B_VAL_1_INT, a_nChapterID)) {
			return stStageLevelEpisodeInfo;
		}

		bool bIsValid = CEpisodeInfoTable.Inst.TryGetStageEpisodeInfo(stEpisodeInfo.m_nNumSubEpisodes - KCDefine.B_VAL_1_INT, out STEpisodeInfo stStageEpisodeInfo, a_nChapterID - KCDefine.B_VAL_1_INT);
		return (bIsValid && stEpisodeInfo.EpisodeType == EEpisodeType.CHAPTER && CEpisodeInfoTable.Inst.TryGetLevelEpisodeInfo(stStageEpisodeInfo.m_nNumSubEpisodes - KCDefine.B_VAL_1_INT, out STEpisodeInfo stChapterLevelEpisodeInfo, stEpisodeInfo.m_nNumSubEpisodes - KCDefine.B_VAL_1_INT, a_nChapterID - KCDefine.B_VAL_1_INT)) ? stChapterLevelEpisodeInfo : STEpisodeInfo.INVALID;
	}

	/** 다음 에피소드 정보를 반환한다 */
	public static STEpisodeInfo GetNextEpisodeInfo(int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		// 레벨 에피소드 정보가 존재 할 경우
		if(CEpisodeInfoTable.Inst.TryGetLevelEpisodeInfo(a_nLevelID + KCDefine.B_VAL_1_INT, out STEpisodeInfo stLevelEpisodeInfo, a_nStageID, a_nChapterID)) {
			return stLevelEpisodeInfo;
		}

		bool bIsValid = CEpisodeInfoTable.Inst.TryGetStageEpisodeInfo(a_nStageID + KCDefine.B_VAL_1_INT, out STEpisodeInfo stStageEpisodeInfo, a_nChapterID);
		return bIsValid ? stStageEpisodeInfo : CEpisodeInfoTable.Inst.TryGetChapterEpisodeInfo(a_nChapterID + KCDefine.B_VAL_1_INT, out STEpisodeInfo stChapterEpisodeInfo) ? stChapterEpisodeInfo : STEpisodeInfo.INVALID;
	}

	/** 다음 레벨 에피소드 정보를 반환한다 */
	public static STEpisodeInfo GetNextLevelEpisodeInfo(int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		var stEpisodeInfo = Access.GetNextEpisodeInfo(a_nLevelID, a_nStageID, a_nChapterID);

		// 레펠 에피소드 정보 일 경우
		if(stEpisodeInfo.EpisodeType == EEpisodeType.LEVEL) {
			return stEpisodeInfo;
		}
		// 스테이지 에피소드 정보 일 경우
		if(stEpisodeInfo.EpisodeType == EEpisodeType.STAGE && CEpisodeInfoTable.Inst.TryGetLevelEpisodeInfo(KCDefine.B_VAL_0_INT, out STEpisodeInfo stStageLevelEpisodeInfo, a_nStageID + KCDefine.B_VAL_1_INT, a_nChapterID)) {
			return stStageLevelEpisodeInfo;
		}

		bool bIsValid = CEpisodeInfoTable.Inst.TryGetStageEpisodeInfo(KCDefine.B_VAL_0_INT, out STEpisodeInfo stStageEpisodeInfo, a_nChapterID + KCDefine.B_VAL_1_INT);
		return (bIsValid && stEpisodeInfo.EpisodeType == EEpisodeType.CHAPTER && CEpisodeInfoTable.Inst.TryGetLevelEpisodeInfo(KCDefine.B_VAL_0_INT, out STEpisodeInfo stChapterLevelEpisodeInfo, KCDefine.B_VAL_0_INT, a_nChapterID + KCDefine.B_VAL_1_INT)) ? stChapterLevelEpisodeInfo : STEpisodeInfo.INVALID;
	}

	/** 레벨 클리어 정보를 반환한다 */
	public static CClearInfo GetLevelClearInfo(int a_nCharacterID, int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT, bool a_bIsAutoCreate = false) {
		// 자동 생성 모드 일 경우
		if(a_bIsAutoCreate && !CGameInfoStorage.Inst.TryGetLevelClearInfo(a_nCharacterID, a_nLevelID, out CClearInfo oLevelClearInfo, a_nStageID, a_nChapterID)) {
			CGameInfoStorage.Inst.AddLevelClearInfo(a_nCharacterID, Factory.MakeClearInfo(a_nLevelID, a_nStageID, a_nChapterID));
		}

		return CGameInfoStorage.Inst.TryGetLevelClearInfo(a_nCharacterID, a_nLevelID, out oLevelClearInfo, a_nStageID, a_nChapterID) ? oLevelClearInfo : null;
	}

	/** 스테이지 클리어 정보를 반환한다 */
	public static CClearInfo GetStageClearInfo(int a_nCharacterID, int a_nStageID, int a_nChapterID = KCDefine.B_VAL_0_INT, bool a_bIsAutoCreate = false) {
		// 자동 생성 모드 일 경우
		if(a_bIsAutoCreate && !CGameInfoStorage.Inst.TryGetStageClearInfo(a_nCharacterID, a_nStageID, out CClearInfo oStageClearInfo, a_nChapterID)) {
			CGameInfoStorage.Inst.AddLevelClearInfo(a_nCharacterID, Factory.MakeClearInfo(KCDefine.B_VAL_0_INT, a_nStageID, a_nChapterID));
		}

		return CGameInfoStorage.Inst.TryGetStageClearInfo(a_nCharacterID, a_nStageID, out oStageClearInfo, a_nChapterID) ? oStageClearInfo : null;
	}

	/** 챕터 클리어 정보를 반환한다 */
	public static CClearInfo GetChapterClearInfo(int a_nCharacterID, int a_nChapterID, bool a_bIsAutoCreate = false) {
		// 자동 생성 모드 일 경우
		if(a_bIsAutoCreate && !CGameInfoStorage.Inst.TryGetChapterClearInfo(a_nCharacterID, a_nChapterID, out CClearInfo oChapterClearInfo)) {
			CGameInfoStorage.Inst.AddLevelClearInfo(a_nCharacterID, Factory.MakeClearInfo(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, a_nChapterID));
		}

		return CGameInfoStorage.Inst.TryGetChapterClearInfo(a_nCharacterID, a_nChapterID, out oChapterClearInfo) ? oChapterClearInfo : null;
	}

	/** 레벨 클리어 보상 식별자 정보를 반환한다 */
	public static (STIDInfo, STIDInfo) GetLevelClearRewardIDInfo(int a_nCharacterID, int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		STIDInfo stMinIDInfo = new STIDInfo(a_nLevelID - KCDefine.B_VAL_1_INT, a_nStageID, a_nChapterID);
		STIDInfo stMaxIDInfo = new STIDInfo(a_nLevelID + KCDefine.B_VAL_0_INT, a_nStageID, a_nChapterID);

		for(int i = 1; i < KCDefine.U_MAX_NUM_LEVEL_INFOS; ++i) {
			bool bIsValid = CEpisodeInfoTable.Inst.TryGetLevelEpisodeInfo(a_nLevelID - i, out STEpisodeInfo stEpisodeInfo);

			// 보상 정보가 존재 할 경우
			if(!bIsValid || stEpisodeInfo.IsExistsReward) {
				stMinIDInfo.m_nID01 = a_nLevelID - i;
				break;
			}
		}

		for(int i = 0; i < KCDefine.U_MAX_NUM_LEVEL_INFOS; ++i) {
			bool bIsValid = CEpisodeInfoTable.Inst.TryGetLevelEpisodeInfo(a_nLevelID + i, out STEpisodeInfo stEpisodeInfo);

			// 보상 정보가 존재 할 경우
			if(!bIsValid || stEpisodeInfo.IsExistsReward) {
				stMaxIDInfo.m_nID01 = a_nLevelID + i;
				break;
			}
		}

		return (stMinIDInfo, stMaxIDInfo);
	}

	/** 아이템 타겟 정보를 반환한다 */
	public static CItemTargetInfo GetItemTargetInfo(int a_nCharacterID, STIdxInfo a_stIdxInfo) {
		return Access.GetTargetInfo(a_nCharacterID, ETargetType.ITEM, a_stIdxInfo) as CItemTargetInfo;
	}

	/** 아이템 타겟 정보를 반환한다 */
	public static CItemTargetInfo GetItemTargetInfo(int a_nCharacterID, EItemKinds a_eItemKinds, bool a_bIsAutoCreate = false) {
		// 자동 생성 모드 일 경우
		if(a_bIsAutoCreate && !CUserInfoStorage.Inst.TryGetItemTargetInfo(a_nCharacterID, a_eItemKinds, out CItemTargetInfo oItemTargetInfo)) {
			CUserInfoStorage.Inst.AddTargetInfo(a_nCharacterID, Factory.MakeItemTargetInfo(a_eItemKinds), CItemInfoTable.Inst.GetItemInfo(a_eItemKinds).m_stCommonInfo.m_bIsFlags01);
		}

		return CUserInfoStorage.Inst.TryGetItemTargetInfo(a_nCharacterID, a_eItemKinds, out oItemTargetInfo) ? oItemTargetInfo : null;
	}

	/** 스킬 타겟 정보를 반환한다 */
	public static CSkillTargetInfo GetSkillTargetInfo(int a_nCharacterID, STIdxInfo a_stIdxInfo) {
		return Access.GetTargetInfo(a_nCharacterID, ETargetType.SKILL, a_stIdxInfo) as CSkillTargetInfo;
	}

	/** 스킬 타겟 정보를 반환한다 */
	public static CSkillTargetInfo GetSkillTargetInfo(int a_nCharacterID, ESkillKinds a_eSkillKinds, bool a_bIsAutoCreate = false) {
		// 자동 생성 모드 일 경우
		if(a_bIsAutoCreate && !CUserInfoStorage.Inst.TryGetSkillTargetInfo(a_nCharacterID, a_eSkillKinds, out CSkillTargetInfo oSkillTargetInfo)) {
			CUserInfoStorage.Inst.AddTargetInfo(a_nCharacterID, Factory.MakeSkillTargetInfo(a_eSkillKinds), CSkillInfoTable.Inst.GetSkillInfo(a_eSkillKinds).m_stCommonInfo.m_bIsFlags01);
		}

		return CUserInfoStorage.Inst.TryGetSkillTargetInfo(a_nCharacterID, a_eSkillKinds, out oSkillTargetInfo) ? oSkillTargetInfo : null;
	}

	/** 객체 타겟 정보를 반환한다 */
	public static CObjTargetInfo GetObjTargetInfo(int a_nCharacterID, STIdxInfo a_stIdxInfo) {
		return Access.GetTargetInfo(a_nCharacterID, ETargetType.OBJ, a_stIdxInfo) as CObjTargetInfo;
	}

	/** 객체 타겟 정보를 반환한다 */
	public static CObjTargetInfo GetObjTargetInfo(int a_nCharacterID, EObjKinds a_eObjKinds, bool a_bIsAutoCreate = false) {
		// 자동 생성 모드 일 경우
		if(a_bIsAutoCreate && !CUserInfoStorage.Inst.TryGetObjTargetInfo(a_nCharacterID, a_eObjKinds, out CObjTargetInfo oObjTargetInfo)) {
			CUserInfoStorage.Inst.AddTargetInfo(a_nCharacterID, Factory.MakeObjTargetInfo(a_eObjKinds), CObjInfoTable.Inst.GetObjInfo(a_eObjKinds).m_stCommonInfo.m_bIsFlags01);
		}

		return CUserInfoStorage.Inst.TryGetObjTargetInfo(a_nCharacterID, a_eObjKinds, out oObjTargetInfo) ? oObjTargetInfo : null;
	}

	/** 어빌리티 타겟 정보를 반환한다 */
	public static CAbilityTargetInfo GetAbilityTargetInfo(int a_nCharacterID, EAbilityKinds a_eAbilityKinds, bool a_bIsAutoCreate = false) {
		// 자동 생성 모드 일 경우
		if(a_bIsAutoCreate && !CUserInfoStorage.Inst.TryGetAbilityTargetInfo(a_nCharacterID, a_eAbilityKinds, out CAbilityTargetInfo oAbilityTargetInfo)) {
			CUserInfoStorage.Inst.AddTargetInfo(a_nCharacterID, Factory.MakeAbilityTargetInfo(a_eAbilityKinds), false);
		}

		return CUserInfoStorage.Inst.TryGetAbilityTargetInfo(a_nCharacterID, a_eAbilityKinds, out oAbilityTargetInfo) ? oAbilityTargetInfo : null;
	}

	/** 시트 이름을 반환한다 */
	public static Dictionary<string, string> GetSheetNames(System.Type a_oType, STTableInfo a_stTableInfo) {
		CAccess.Assert(a_stTableInfo.m_oSheetNameDictContainer.ContainsKey(a_oType));

		return new Dictionary<string, string>() {
			[KCDefine.B_KEY_COMMON] = a_stTableInfo.m_oSheetNameDictContainer[a_oType].GetValueOrDefault(KCDefine.B_KEY_COMMON, string.Empty),
			[KCDefine.B_KEY_BUY_TRADE] = a_stTableInfo.m_oSheetNameDictContainer[a_oType].GetValueOrDefault(KCDefine.B_KEY_BUY_TRADE, string.Empty),
			[KCDefine.B_KEY_SALE_TRADE] = a_stTableInfo.m_oSheetNameDictContainer[a_oType].GetValueOrDefault(KCDefine.B_KEY_SALE_TRADE, string.Empty),
			[KCDefine.B_KEY_ENHANCE_TRADE] = a_stTableInfo.m_oSheetNameDictContainer[a_oType].GetValueOrDefault(KCDefine.B_KEY_ENHANCE_TRADE, string.Empty),
			[KCDefine.U_KEY_LEVEL_EPISODE] = a_stTableInfo.m_oSheetNameDictContainer[a_oType].GetValueOrDefault(KCDefine.U_KEY_LEVEL_EPISODE, string.Empty),
			[KCDefine.U_KEY_STAGE_EPISODE] = a_stTableInfo.m_oSheetNameDictContainer[a_oType].GetValueOrDefault(KCDefine.U_KEY_STAGE_EPISODE, string.Empty),
			[KCDefine.U_KEY_CHAPTER_EPISODE] = a_stTableInfo.m_oSheetNameDictContainer[a_oType].GetValueOrDefault(KCDefine.U_KEY_CHAPTER_EPISODE, string.Empty)
		};
	}

	/** 아이템 타겟 값을 변경한다 */
	public static void SetItemTargetVal(int a_nCharacterID, EItemKinds a_eItemKinds, ETargetKinds a_eTargetKinds, int a_nKinds, decimal a_dmVal) {
		Access.GetItemTargetInfo(a_nCharacterID, a_eItemKinds, true).m_oAbilityTargetInfoDict.ExReplaceTargetVal(a_eTargetKinds, a_nKinds, a_dmVal);
	}

	/** 스킬 타겟 값을 변경한다 */
	public static void SetSkillTargetVal(int a_nCharacterID, ESkillKinds a_eSkillKinds, ETargetKinds a_eTargetKinds, int a_nKinds, decimal a_dmVal) {
		Access.GetSkillTargetInfo(a_nCharacterID, a_eSkillKinds, true).m_oAbilityTargetInfoDict.ExReplaceTargetVal(a_eTargetKinds, a_nKinds, a_dmVal);
	}

	/** 객체 타겟 값을 변경한다 */
	public static void SetObjTargetVal(int a_nCharacterID, EObjKinds a_eObjKinds, ETargetKinds a_eTargetKinds, int a_nKinds, decimal a_dmVal) {
		Access.GetObjTargetInfo(a_nCharacterID, a_eObjKinds, true).m_oAbilityTargetInfoDict.ExReplaceTargetVal(a_eTargetKinds, a_nKinds, a_dmVal);
	}

	/** 어빌리티 타겟 값을 변경한다 */
	public static void SetAbilityTargetVal(int a_nCharacterID, EAbilityKinds a_eAbilityKinds, ETargetKinds a_eTargetKinds, int a_nKinds, decimal a_dmVal) {
		Access.GetAbilityTargetInfo(a_nCharacterID, a_eAbilityKinds, true).m_oAbilityTargetInfoDict.ExReplaceTargetVal(a_eTargetKinds, a_nKinds, a_dmVal);
	}

	/** 타겟 정보를 탐색한다 */
	public static CTargetInfo ExFindTargetInfo(this List<CTargetInfo> a_oSender, ETargetType a_eTargetType, string a_oGUID) {
		return a_oSender.ExTryGetTargetInfo(a_eTargetType, a_oGUID, out CTargetInfo oTargetInfo) ? oTargetInfo : null;
	}

	/** 아이템 타겟 교환 가능 여부를 검사한다 */
	private static bool IsEnableItemTargetTrade(STTargetInfo a_stTargetInfo, CTargetInfo a_oTargetInfo) {
		CAccess.Assert(CItemInfoTable.Inst.TryGetItemInfo((EItemKinds)a_stTargetInfo.Kinds, out STItemInfo stItemInfo));
		return Access.DoIsEnableTrade(a_stTargetInfo, a_oTargetInfo);
	}

	/** 스킬 타겟 교환 가능 여부를 검사한다 */
	private static bool IsEnableSkillTargetTrade(STTargetInfo a_stTargetInfo, CTargetInfo a_oTargetInfo) {
		CAccess.Assert(CSkillInfoTable.Inst.TryGetSkillInfo((ESkillKinds)a_stTargetInfo.Kinds, out STSkillInfo stSkillInfo));
		return Access.DoIsEnableTrade(a_stTargetInfo, a_oTargetInfo);
	}

	/** 객체 타겟 교환 가능 여부를 검사한다 */
	private static bool IsEnableObjTargetTrade(STTargetInfo a_stTargetInfo, CTargetInfo a_oTargetInfo) {
		CAccess.Assert(CObjInfoTable.Inst.TryGetObjInfo((EObjKinds)a_stTargetInfo.Kinds, out STObjInfo stObjInfo));
		return Access.DoIsEnableTrade(a_stTargetInfo, a_oTargetInfo);
	}

	/** 어빌리티 타겟 교환 가능 여부를 검사한다 */
	private static bool IsEnableAbilityTargetTrade(STTargetInfo a_stTargetInfo, CTargetInfo a_oTargetInfo) {
		CAccess.Assert(CAbilityInfoTable.Inst.TryGetAbilityInfo((EAbilityKinds)a_stTargetInfo.Kinds, out STAbilityInfo stAbilityInfo));
		return Access.DoIsEnableTrade(a_stTargetInfo, a_oTargetInfo);
	}

	/** 타겟 정보를 반환한다 */
	private static CTargetInfo GetTargetInfo(int a_nCharacterID, ETargetType a_eTargetType, STIdxInfo a_stIdxInfo) {
		return CUserInfoStorage.Inst.TryGetCharacterUserInfo(a_nCharacterID, out CCharacterUserInfo oCharacterUserInfo) ? oCharacterUserInfo.m_oTargetInfoList.ExGetVal((a_oTargetInfo) => a_oTargetInfo.m_stIdxInfo.Equals(a_stIdxInfo), null) : null;
	}

	/** 교환 가능 여부를 검사한다 */
	private static bool DoIsEnableTrade(STTargetInfo a_stTargetInfo, CTargetInfo a_oTargetInfo) {
		// 타겟 정보가 존재 할 경우
		if(a_stTargetInfo.m_eTargetKinds != ETargetKinds.NONE && a_oTargetInfo != null) {
			switch(((int)a_stTargetInfo.m_eTargetKinds).ExKindsToSubKindsTypeVal()) {
				case KEnumVal.TK_LV_SUB_KINDS_TYPE_VAL: return a_oTargetInfo.m_oAbilityTargetInfoDict.GetValueOrDefault(Factory.MakeUTargetInfoID(ETargetKinds.ABILITY_TARGET, (int)EAbilityKinds.STAT_ABILITY_LV), STTargetInfo.INVALID).m_stValInfo01.m_dmVal >= a_stTargetInfo.m_stValInfo01.m_dmVal;
				case KEnumVal.TK_EXP_SUB_KINDS_TYPE_VAL: return a_oTargetInfo.m_oAbilityTargetInfoDict.GetValueOrDefault(Factory.MakeUTargetInfoID(ETargetKinds.ABILITY_TARGET, (int)EAbilityKinds.STAT_ABILITY_EXP), STTargetInfo.INVALID).m_stValInfo01.m_dmVal >= a_stTargetInfo.m_stValInfo01.m_dmVal;
				case KEnumVal.TK_NUMS_SUB_KINDS_TYPE_VAL: return a_oTargetInfo.m_oAbilityTargetInfoDict.GetValueOrDefault(Factory.MakeUTargetInfoID(ETargetKinds.ABILITY_TARGET, (int)EAbilityKinds.STAT_ABILITY_NUMS), STTargetInfo.INVALID).m_stValInfo01.m_dmVal >= a_stTargetInfo.m_stValInfo01.m_dmVal;
				case KEnumVal.TK_ENHANCE_SUB_KINDS_TYPE_VAL: return a_oTargetInfo.m_oAbilityTargetInfoDict.GetValueOrDefault(Factory.MakeUTargetInfoID(ETargetKinds.ABILITY_TARGET, (int)EAbilityKinds.STAT_ABILITY_ENHANCE), STTargetInfo.INVALID).m_stValInfo01.m_dmVal >= a_stTargetInfo.m_stValInfo01.m_dmVal;
			}
		}

		return false;
	}
	#endregion // 클래스 함수
}

/** 초기화 씬 접근자 */
public static partial class Access {
	#region 클래스 함수

	#endregion // 클래스 함수
}

/** 설정 씬 접근자 */
public static partial class Access {
	#region 클래스 함수

	#endregion // 클래스 함수
}

/** 지연 설정 씬 접근자 */
public static partial class Access {
	#region 클래스 함수

	#endregion // 클래스 함수
}

/** 타이틀 씬 접근자 */
public static partial class Access {
	#region 클래스 함수

	#endregion // 클래스 함수
}

/** 메인 씬 접근자 */
public static partial class Access {
	#region 클래스 함수

	#endregion // 클래스 함수
}

/** 플레이 씬 접근자 */
public static partial class Access {
	#region 클래스 함수

	#endregion // 클래스 함수
}

/** 결과 씬 접근자 */
public static partial class Access {
	#region 클래스 함수

	#endregion // 클래스 함수
}

/** 로딩 씬 접근자 */
public static partial class Access {
	#region 클래스 함수

	#endregion // 클래스 함수
}

/** 중첩 씬 접근자 */
public static partial class Access {
	#region 클래스 함수

	#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
