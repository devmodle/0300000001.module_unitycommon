#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 전역 접근자 */
public static partial class Access {
	#region 클래스 프로퍼티
	public static string StoreURL {
		get {
#if UNITY_IOS
			return string.Format(KCDefine.U_FMT_STORE_URL, CProjInfoTable.Inst.ProjInfo.m_oStoreAppID);
#else
			return string.Format(KCDefine.U_FMT_STORE_URL, CProjInfoTable.Inst.GetAppID(CProjInfoTable.Inst.ProjInfo));
#endif			// #if UNITY_IOS
		}
	}

	public static bool IsPurchaseRemoveAds => Access.GetItemTargetVal(CGameInfoStorage.Inst.PlayCharacterID, EItemKinds.NON_CONSUMABLE_REMOVE_ADS, ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS) > KCDefine.B_VAL_0_INT;
	public static string MoreAppsURL => string.Format(KCDefine.U_FMT_MORE_APPS_URL, CProjInfoTable.Inst.ProjInfo.m_oStoreAppID);
	#endregion			// 클래스 프로퍼티

	#region 클래스 함수
	/** 레벨 클리어 여부를 검사한다 */
	public static bool IsClearLevel(int a_nCharacterID, int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo);
		return oCharacterGameInfo != null && oCharacterGameInfo.m_oLevelClearInfoDict.ContainsKey(CFactory.MakeULevelID(a_nLevelID, a_nStageID, a_nChapterID));
	}

	/** 스테이지 클리어 여부를 검사한다 */
	public static bool IsClearStage(int a_nCharacterID, int a_nStageID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo);
		return oCharacterGameInfo != null && oCharacterGameInfo.m_oStageClearInfoDict.ContainsKey(CFactory.MakeUniqueStageID(a_nStageID, a_nChapterID));
	}

	/** 챕터 클리어 여부를 검사한다 */
	public static bool IsClearChapter(int a_nCharacterID, int a_nChapterID) {
		CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo);
		return oCharacterGameInfo != null && oCharacterGameInfo.m_oChapterClearInfoDict.ContainsKey(CFactory.MakeUniqueChapterID(a_nChapterID));
	}

	/** 레벨 잠금 해제 여부를 검사한다 */
	public static bool IsUnlockLevel(int a_nCharacterID, int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo);
		return oCharacterGameInfo != null && oCharacterGameInfo.m_oUnlockULevelIDList.Contains(CFactory.MakeULevelID(a_nLevelID, a_nStageID, a_nChapterID));
	}

	/** 스테이지 잠금 해제 여부를 검사한다 */
	public static bool IsUnlockStage(int a_nCharacterID, int a_nStageID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo);
		return oCharacterGameInfo != null && oCharacterGameInfo.m_oUnlockUniqueStageIDList.Contains(CFactory.MakeUniqueStageID(a_nStageID, a_nChapterID));
	}

	/** 챕터 잠금 해제 여부를 검사한다 */
	public static bool IsUnlockChapter(int a_nCharacterID, int a_nChapterID) {
		CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo);
		return oCharacterGameInfo != null && oCharacterGameInfo.m_oUnlockUniqueChapterIDList.Contains(CFactory.MakeUniqueChapterID(a_nChapterID));
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

	/** 일일 보상 지속 가능 여부를 검사한다 */
	public static bool IsContinueGetDailyReward(int a_nCharacterID) {
		return CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo) ? System.DateTime.Now.ExGetDeltaTimePerDays(oCharacterGameInfo.PrevDailyRewardTime).ExIsLess(KCDefine.B_VAL_2_REAL) : false;
	}

	/** 교환 가능 여부를 검사한다 */
	public static bool IsEnableTrade(int a_nCharacterID, STTargetInfo a_stTargetInfo) {
		switch(a_stTargetInfo.TargetType) {
			case ETargetType.ITEM: return Access.IsEnableItemTargetTrade(a_stTargetInfo, Access.GetItemTargetInfo(a_nCharacterID, (EItemKinds)a_stTargetInfo.m_nKinds));
			case ETargetType.SKILL: return Access.IsEnableSkillTargetTrade(a_stTargetInfo, Access.GetSkillTargetInfo(a_nCharacterID, (ESkillKinds)a_stTargetInfo.m_nKinds));
			case ETargetType.OBJ: return Access.IsEnableObjTargetTrade(a_stTargetInfo, Access.GetObjTargetInfo(a_nCharacterID, (EObjKinds)a_stTargetInfo.m_nKinds));
		}

		return false;
	}

	/** 교환 가능 여부를 검사한다 */
	public static bool IsEnableTrade(STTargetInfo a_stTargetInfo, CTargetInfo a_oTargetInfo) {
		switch(a_stTargetInfo.TargetType) {
			case ETargetType.ITEM: return Access.IsEnableItemTargetTrade(a_stTargetInfo, a_oTargetInfo as CItemTargetInfo);
			case ETargetType.SKILL: return Access.IsEnableSkillTargetTrade(a_stTargetInfo, a_oTargetInfo as CSkillTargetInfo);
			case ETargetType.OBJ: return Access.IsEnableObjTargetTrade(a_stTargetInfo, a_oTargetInfo as CObjTargetInfo);
		}

		return false;
	}

	/** 교환 가능 여부를 검사한다 */
	public static bool IsEnableTrade(List<(STTargetInfo, CTargetInfo)> a_oTradeTargetInfoList) {
		return a_oTradeTargetInfoList.All((a_stTradeTargetInfo) => Access.IsEnableTrade(a_stTradeTargetInfo.Item1, a_stTradeTargetInfo.Item2));
	}

	/** 교환 가능 여부를 검사한다 */
	public static bool IsEnableTrade(int a_nCharacterID, Dictionary<ulong, STTargetInfo> a_oTargetInfoDict) {
		CAccess.Assert(a_oTargetInfoDict != null);
		return a_oTargetInfoDict.All((a_stKeyVal) => Access.IsEnableTrade(a_nCharacterID, a_stKeyVal.Value));
	}

	/** 튜토리얼 메세지를 반환한다 */
	public static string GetTutorialMsg(ETutorialKinds a_eTutorialKinds, int a_nIdx = KCDefine.B_VAL_0_INT) {
		return CStrTable.Inst.GetStr(string.Format(KCDefine.U_KEY_FMT_TUTORIAL_MSG, a_eTutorialKinds, a_nIdx + KCDefine.B_VAL_1_INT));
	}

	/** 스테이지 서브 에피소드 개수를 반환한다 */
	public static int GetNumStageSubEpisodes(int a_nStageID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		return CEpisodeInfoTable.Inst.TryGetStageEpisodeInfo(a_nStageID, out STEpisodeInfo stStageEpisodeInfo, a_nChapterID) ? stStageEpisodeInfo.m_nNumSubEpisodes : KCDefine.B_VAL_0_INT;
	}

	/** 챕터 서브 에피소드 개수를 반환한다 */
	public static int GetNumChapterSubEpisodes(int a_nChapterID) {
		return CEpisodeInfoTable.Inst.TryGetChapterEpisodeInfo(a_nChapterID, out STEpisodeInfo stChapterEpisodeInfo) ? stChapterEpisodeInfo.m_nNumSubEpisodes : KCDefine.B_VAL_0_INT;
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

	/** 심볼 개수를 반환한다 */
	public static long GetTotalNumSymbols(int a_nCharacterID) {
		return CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo) ? oCharacterGameInfo.m_oLevelClearInfoDict.Sum((a_stKeyVal) => a_stKeyVal.Value.NumSymbols) : KCDefine.B_VAL_0_INT;
	}

	/** 아이템 타겟 값을 반환한다 */
	public static long GetItemTargetVal(int a_nCharacterID, EItemKinds a_eItemKinds, ETargetKinds a_eTargetKinds, int a_nKinds) {
		return Access.GetItemTargetInfo(a_nCharacterID, a_eItemKinds, true).m_oAbilityTargetInfoDict.ExGetTargetVal(a_eTargetKinds, a_nKinds);
	}

	/** 스킬 타겟 값을 반환한다 */
	public static long GetSkillTargetVal(int a_nCharacterID, ESkillKinds a_eSkillKinds, ETargetKinds a_eTargetKinds, int a_nKinds) {
		return Access.GetSkillTargetInfo(a_nCharacterID, a_eSkillKinds, true).m_oAbilityTargetInfoDict.ExGetTargetVal(a_eTargetKinds, a_nKinds);
	}

	/** 객체 타겟 값을 반환한다 */
	public static long GetObjTargetVal(int a_nCharacterID, EObjKinds a_eObjKinds, ETargetKinds a_eTargetKinds, int a_nKinds) {
		return Access.GetObjTargetInfo(a_nCharacterID, a_eObjKinds, true).m_oAbilityTargetInfoDict.ExGetTargetVal(a_eTargetKinds, a_nKinds);
	}

	/** 일일 보상 종류를 반환한다 */
	public static ERewardKinds GetDailyRewardKinds(int a_nCharacterID) {
		var oCharacterGameInfo = CGameInfoStorage.Inst.GetCharacterGameInfo(a_nCharacterID);
		return KDefine.G_REWARDS_KINDS_DAILY_REWARD_LIST[oCharacterGameInfo.DailyRewardID];
	}

	/** 에피소드 정보를 반환한다 */
	public static STEpisodeInfo GetEpisodeInfo(int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		// 레벨 에피소드 정보가 존재 할 경우
		if(CEpisodeInfoTable.Inst.TryGetLevelEpisodeInfo(a_nLevelID, out STEpisodeInfo stLevelEpisodeInfo, a_nStageID, a_nChapterID)) {
			return stLevelEpisodeInfo;
		}
		
		return CEpisodeInfoTable.Inst.TryGetStageEpisodeInfo(a_nStageID, out STEpisodeInfo stStageEpisodeInfo, a_nChapterID) ? stStageEpisodeInfo : CEpisodeInfoTable.Inst.GetChapterEpisodeInfo(a_nChapterID);
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

	/** 아이템 타겟 정보를 반환한다 */
	public static CItemTargetInfo GetItemTargetInfo(int a_nCharacterID, EItemKinds a_eItemKinds, bool a_bIsAutoCreate = false) {
		// 자동 생성 모드 일 경우
		if(a_bIsAutoCreate && !CUserInfoStorage.Inst.TryGetItemTargetInfo(a_nCharacterID, a_eItemKinds, out CItemTargetInfo oItemTargetInfo)) {
			CUserInfoStorage.Inst.AddTargetInfo(a_nCharacterID, Factory.MakeItemTargetInfo(a_eItemKinds), CItemInfoTable.Inst.GetItemInfo(a_eItemKinds).m_stCommonInfo.m_bIsRepeat);
		}

		return CUserInfoStorage.Inst.TryGetItemTargetInfo(a_nCharacterID, a_eItemKinds, out oItemTargetInfo) ? oItemTargetInfo : null;
	}

	/** 스킬 타겟 정보를 반환한다 */
	public static CSkillTargetInfo GetSkillTargetInfo(int a_nCharacterID, ESkillKinds a_eSkillKinds, bool a_bIsAutoCreate = false) {
		// 자동 생성 모드 일 경우
		if(a_bIsAutoCreate && !CUserInfoStorage.Inst.TryGetSkillTargetInfo(a_nCharacterID, a_eSkillKinds, out CSkillTargetInfo oSkillTargetInfo)) {
			CUserInfoStorage.Inst.AddTargetInfo(a_nCharacterID, Factory.MakeSkillTargetInfo(a_eSkillKinds), CSkillInfoTable.Inst.GetSkillInfo(a_eSkillKinds).m_stCommonInfo.m_bIsRepeat);
		}

		return CUserInfoStorage.Inst.TryGetSkillTargetInfo(a_nCharacterID, a_eSkillKinds, out oSkillTargetInfo) ? oSkillTargetInfo : null;
	}

	/** 객체 타겟 정보를 반환한다 */
	public static CObjTargetInfo GetObjTargetInfo(int a_nCharacterID, EObjKinds a_eObjKinds, bool a_bIsAutoCreate = false) {
		// 자동 생성 모드 일 경우
		if(a_bIsAutoCreate && !CUserInfoStorage.Inst.TryGetObjTargetInfo(a_nCharacterID, a_eObjKinds, out CObjTargetInfo oObjTargetInfo)) {
			CUserInfoStorage.Inst.AddTargetInfo(a_nCharacterID, Factory.MakeObjTargetInfo(a_eObjKinds), CObjInfoTable.Inst.GetObjInfo(a_eObjKinds).m_stCommonInfo.m_bIsRepeat);
		}

		return CUserInfoStorage.Inst.TryGetObjTargetInfo(a_nCharacterID, a_eObjKinds, out oObjTargetInfo) ? oObjTargetInfo : null;
	}

	/** 일일 보상 종류를 변경한다 */
	public static void SetDailyRewardID(int a_nCharacterID, int a_nID, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (KDefine.G_REWARDS_KINDS_DAILY_REWARD_LIST.ExIsValidIdx(a_nID) && CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo)));

		// 캐릭터 게임 정보가 존재 할 경우
		if(KDefine.G_REWARDS_KINDS_DAILY_REWARD_LIST.ExIsValidIdx(a_nID) && CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out oCharacterGameInfo)) {
			oCharacterGameInfo.DailyRewardID = a_nID;
		}
	}

	/** 아이템 타겟 값을 변경한다 */
	public static void SetItemTargetVal(int a_nCharacterID, EItemKinds a_eItemKinds, ETargetKinds a_eTargetKinds, int a_nKinds, long a_nVal) {
		Access.GetItemTargetInfo(a_nCharacterID, a_eItemKinds, true).m_oAbilityTargetInfoDict.ExReplaceTargetVal(a_eTargetKinds, a_nKinds, a_nVal);
	}

	/** 스킬 타겟 값을 변경한다 */
	public static void SetSkillTargetVal(int a_nCharacterID, ESkillKinds a_eSkillKinds, ETargetKinds a_eTargetKinds, int a_nKinds, long a_nVal) {
		Access.GetSkillTargetInfo(a_nCharacterID, a_eSkillKinds, true).m_oAbilityTargetInfoDict.ExReplaceTargetVal(a_eTargetKinds, a_nKinds, a_nVal);
	}

	/** 객체 타겟 값을 변경한다 */
	public static void SetObjTargetVal(int a_nCharacterID, EObjKinds a_eObjKinds, ETargetKinds a_eTargetKinds, int a_nKinds, long a_nVal) {
		Access.GetObjTargetInfo(a_nCharacterID, a_eObjKinds, true).m_oAbilityTargetInfoDict.ExReplaceTargetVal(a_eTargetKinds, a_nKinds, a_nVal);
	}

	/** 아이템 타겟 교환 가능 여부를 검사한다 */
	private static bool IsEnableItemTargetTrade(STTargetInfo a_stTargetInfo, CItemTargetInfo a_oItemTargetInfo) {
		CAccess.Assert(CItemInfoTable.Inst.TryGetItemInfo((EItemKinds)a_stTargetInfo.m_nKinds, out STItemInfo stItemInfo));
		return Access.DoIsEnableTrade(a_stTargetInfo, a_oItemTargetInfo);
	}

	/** 스킬 타겟 교환 가능 여부를 검사한다 */
	private static bool IsEnableSkillTargetTrade(STTargetInfo a_stTargetInfo, CSkillTargetInfo a_oSkillTargetInfo) {
		CAccess.Assert(CSkillInfoTable.Inst.TryGetSkillInfo((ESkillKinds)a_stTargetInfo.m_nKinds, out STSkillInfo stSkillInfo));
		return Access.DoIsEnableTrade(a_stTargetInfo, a_oSkillTargetInfo);
	}

	/** 객체 타겟 교환 가능 여부를 검사한다 */
	private static bool IsEnableObjTargetTrade(STTargetInfo a_stTargetInfo, CObjTargetInfo a_oObjTargetInfo) {
		CAccess.Assert(CObjInfoTable.Inst.TryGetObjInfo((EObjKinds)a_stTargetInfo.m_nKinds, out STObjInfo stObjInfo));
		return Access.DoIsEnableTrade(a_stTargetInfo, a_oObjTargetInfo);
	}

	/** 교환 가능 여부를 검사한다 */
	private static bool DoIsEnableTrade(STTargetInfo a_stTargetInfo, CTargetInfo a_oTargetInfo) {
		// 타겟 정보가 존재 할 경우
		if(a_oTargetInfo != null && !a_stTargetInfo.Equals(STTargetInfo.INVALID)) {
			switch(((int)a_stTargetInfo.m_eTargetKinds).ExKindsToSubKindsTypeVal()) {
				case KEnumVal.LV_TARGET_SUB_KINDS_TYPE_VAL: return a_oTargetInfo.m_oAbilityTargetInfoDict.GetValueOrDefault(Factory.MakeUniqueTargetInfoID(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_LV), STTargetInfo.INVALID).m_stValInfo01.m_nVal >= a_stTargetInfo.m_stValInfo01.m_nVal;
				case KEnumVal.EXP_TARGET_SUB_KINDS_TYPE_VAL: return a_oTargetInfo.m_oAbilityTargetInfoDict.GetValueOrDefault(Factory.MakeUniqueTargetInfoID(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_EXP), STTargetInfo.INVALID).m_stValInfo01.m_nVal >= a_stTargetInfo.m_stValInfo01.m_nVal;
				case KEnumVal.NUMS_TARGET_SUB_KINDS_TYPE_VAL: return a_oTargetInfo.m_oAbilityTargetInfoDict.GetValueOrDefault(Factory.MakeUniqueTargetInfoID(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS), STTargetInfo.INVALID).m_stValInfo01.m_nVal >= a_stTargetInfo.m_stValInfo01.m_nVal;
				case KEnumVal.ENHANCE_TARGET_SUB_KINDS_TYPE_VAL: return a_oTargetInfo.m_oAbilityTargetInfoDict.GetValueOrDefault(Factory.MakeUniqueTargetInfoID(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_ENHANCE), STTargetInfo.INVALID).m_stValInfo01.m_nVal >= a_stTargetInfo.m_stValInfo01.m_nVal;
			}
		}

		return false;
	}
	#endregion			// 클래스 함수
}

/** 초기화 씬 접근자 */
public static partial class Access {
	#region 클래스 함수
	
	#endregion			// 클래스 함수
}

/** 시작 씬 접근자 */
public static partial class Access {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 설정 씬 접근자 */
public static partial class Access {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 약관 동의 씬 접근자 */
public static partial class Access {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 지연 설정 씬 접근자 */
public static partial class Access {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 타이틀 씬 접근자 */
public static partial class Access {
	#region 클래스 함수
	
	#endregion			// 클래스 함수
}

/** 메인 씬 접근자 */
public static partial class Access {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 게임 씬 접근자 */
public static partial class Access {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 로딩 씬 접근자 */
public static partial class Access {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 중첩 씬 접근자 */
public static partial class Access {
	#region 클래스 함수
	
	#endregion			// 클래스 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
