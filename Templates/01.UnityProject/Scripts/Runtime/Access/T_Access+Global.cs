#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
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

	public static string MoreAppsURL => string.Format(KCDefine.U_FMT_MORE_APPS_URL, CProjInfoTable.Inst.ProjInfo.m_oStoreAppID);
	#endregion			// 클래스 프로퍼티

	#region 클래스 함수
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

	/** 레벨 클리어 정보를 반환한다 */
	public static CClearInfo GetLevelClearInfo(int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT, bool a_bIsAutoCreate = false) {
		// 자동 생성 모드 일 경우
		if(a_bIsAutoCreate && !CGameInfoStorage.Inst.TryGetLevelClearInfo(a_nLevelID, out CClearInfo oLevelClearInfo, a_nStageID, a_nChapterID)) {
			CGameInfoStorage.Inst.AddLevelClearInfo(Factory.MakeClearInfo(a_nLevelID, a_nStageID, a_nChapterID));
		}

		return CGameInfoStorage.Inst.TryGetLevelClearInfo(a_nLevelID, out oLevelClearInfo, a_nStageID, a_nChapterID) ? oLevelClearInfo : null;
	}

	/** 아이템 타겟 정보를 반환한다 */
	public static CItemTargetInfo GetItemTargetInfo(int a_nCharacterID, EItemKinds a_eItemKinds, long a_nLV = KCDefine.B_VAL_1_INT, long a_nNums = KCDefine.B_VAL_1_INT, bool a_bIsAutoCreate = false) {
		// 자동 생성 모드 일 경우
		if(a_bIsAutoCreate && !CUserInfoStorage.Inst.TryGetItemTargetInfo(a_nCharacterID, a_eItemKinds, out CItemTargetInfo oItemTargetInfo)) {
			CUserInfoStorage.Inst.AddTargetInfo(a_nCharacterID, Factory.MakeItemTargetInfo(a_eItemKinds, a_nLV, a_nNums), CItemInfoTable.Inst.GetItemInfo(a_eItemKinds).m_stCommonInfo.m_bIsRepeat);
		}

		return CUserInfoStorage.Inst.TryGetItemTargetInfo(a_nCharacterID, a_eItemKinds, out oItemTargetInfo) ? oItemTargetInfo : null;
	}

	/** 스킬 타겟 정보를 반환한다 */
	public static CSkillTargetInfo GetSkillTargetInfo(int a_nCharacterID, ESkillKinds a_eSkillKinds, long a_nLV = KCDefine.B_VAL_1_INT, long a_nNums = KCDefine.B_VAL_1_INT, bool a_bIsAutoCreate = false) {
		// 자동 생성 모드 일 경우
		if(a_bIsAutoCreate && !CUserInfoStorage.Inst.TryGetSkillTargetInfo(a_nCharacterID, a_eSkillKinds, out CSkillTargetInfo oSkillTargetInfo)) {
			CUserInfoStorage.Inst.AddTargetInfo(a_nCharacterID, Factory.MakeSkillTargetInfo(a_eSkillKinds, a_nLV, a_nNums), CSkillInfoTable.Inst.GetSkillInfo(a_eSkillKinds).m_stCommonInfo.m_bIsRepeat);
		}

		return CUserInfoStorage.Inst.TryGetSkillTargetInfo(a_nCharacterID, a_eSkillKinds, out oSkillTargetInfo) ? oSkillTargetInfo : null;
	}

	/** 객체 타겟 정보를 반환한다 */
	public static CObjTargetInfo GetObjTargetInfo(int a_nCharacterID, EObjKinds a_eObjKinds, long a_nLV = KCDefine.B_VAL_1_INT, long a_nNums = KCDefine.B_VAL_1_INT, bool a_bIsAutoCreate = false) {
		// 자동 생성 모드 일 경우
		if(a_bIsAutoCreate && !CUserInfoStorage.Inst.TryGetObjTargetInfo(a_nCharacterID, a_eObjKinds, out CObjTargetInfo oObjTargetInfo)) {
			CUserInfoStorage.Inst.AddTargetInfo(a_nCharacterID, Factory.MakeObjTargetInfo(a_eObjKinds, a_nLV, a_nNums), CObjInfoTable.Inst.GetObjInfo(a_eObjKinds).m_stCommonInfo.m_bIsRepeat);
		}

		return CUserInfoStorage.Inst.TryGetObjTargetInfo(a_nCharacterID, a_eObjKinds, out oObjTargetInfo) ? oObjTargetInfo : null;
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
	#endregion			// 클래스 함수
}

/** 스플래시 씬 접근자 */
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
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
