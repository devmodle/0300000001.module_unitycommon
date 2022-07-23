using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
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
	public static bool IsEnableTrade(STTargetInfo a_stTargetInfo) {
		switch(a_stTargetInfo.TargetType) {
			case ETargetType.ITEM: return Access.IsEnableItemTargetTrade(null, a_stTargetInfo);
			case ETargetType.SKILL: return Access.IsEnableSkillTargetTrade(null, a_stTargetInfo);
			case ETargetType.OBJ: return Access.IsEnableObjTargetTrade(null, a_stTargetInfo);
		}

		return false;
	}

	/** 교환 가능 여부를 검사한다 */
	public static bool IsEnableTrade(CTargetInfo a_oTargetInfo, STTargetInfo a_stTargetInfo) {
		switch(a_stTargetInfo.TargetType) {
			case ETargetType.ITEM: return Access.IsEnableItemTargetTrade(a_oTargetInfo as CItemInfo, a_stTargetInfo);
			case ETargetType.SKILL: return Access.IsEnableSkillTargetTrade(a_oTargetInfo as CSkillInfo, a_stTargetInfo);
			case ETargetType.OBJ: return Access.IsEnableObjTargetTrade(a_oTargetInfo as CObjInfo, a_stTargetInfo);
		}

		return false;
	}

	/** 교환 가능 여부를 검사한다 */
	public static bool IsEnableTrade(List<(CTargetInfo, STTargetInfo)> a_oTradeInfoList) {
		return a_oTradeInfoList.All((a_stTradeInfo) => Access.IsEnableTrade(a_stTradeInfo.Item1, a_stTradeInfo.Item2));
	}

	/** 교환 가능 여부를 검사한다 */
	public static bool IsEnableTrade(Dictionary<ulong, STTargetInfo> a_oTargetInfoDict) {
		CAccess.Assert(a_oTargetInfoDict != null);
		return a_oTargetInfoDict.All((a_stKeyVal) => Access.IsEnableTrade(a_stKeyVal.Value));
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

	/** 아이템 정보를 반환한다 */
	public static CItemInfo GetItemInfo(EItemKinds a_eItemKinds, bool a_bIsAutoCreate = false) {
		// 자동 생성 모드 일 경우
		if(a_bIsAutoCreate && !CUserInfoStorage.Inst.TryGetItemInfo(a_eItemKinds, out CItemInfo oItemInfo)) {
			CUserInfoStorage.Inst.AddItemInfo(Factory.MakeItemInfo(a_eItemKinds), CItemInfoTable.Inst.GetItemInfo(a_eItemKinds).m_stCommonInfo.m_bIsRepeat);
		}

		return CUserInfoStorage.Inst.TryGetItemInfo(a_eItemKinds, out oItemInfo) ? oItemInfo : null;
	}

	/** 스킬 정보를 반환한다 */
	public static CSkillInfo GetSkillInfo(ESkillKinds a_eSkillKinds, bool a_bIsAutoCreate = false) {
		// 자동 생성 모드 일 경우
		if(a_bIsAutoCreate && !CUserInfoStorage.Inst.TryGetSkillInfo(a_eSkillKinds, out CSkillInfo oSkillInfo)) {
			CUserInfoStorage.Inst.AddSkillInfo(Factory.MakeSkillInfo(a_eSkillKinds), CSkillInfoTable.Inst.GetSkillInfo(a_eSkillKinds).m_stCommonInfo.m_bIsRepeat);
		}

		return CUserInfoStorage.Inst.TryGetSkillInfo(a_eSkillKinds, out oSkillInfo) ? oSkillInfo : null;
	}

	/** 객체 정보를 반환한다 */
	public static CObjInfo GetObjInfo(EObjKinds a_eObjKinds, bool a_bIsAutoCreate = false) {
		// 자동 생성 모드 일 경우
		if(a_bIsAutoCreate && !CUserInfoStorage.Inst.TryGetObjInfo(a_eObjKinds, out CObjInfo oObjInfo)) {
			CUserInfoStorage.Inst.AddObjInfo(Factory.MakeObjInfo(a_eObjKinds), CObjInfoTable.Inst.GetObjInfo(a_eObjKinds).m_stCommonInfo.m_bIsRepeat);
		}

		return CUserInfoStorage.Inst.TryGetObjInfo(a_eObjKinds, out oObjInfo) ? oObjInfo : null;
	}
	
	/** 교환 가능 여부를 검사한다 */
	private static bool DoIsEnableTrade(CTargetInfo a_oTargetInfo, STTargetInfo a_stTargetInfo) {
		CAccess.Assert(a_oTargetInfo != null);

		switch(((int)a_stTargetInfo.m_eTargetKinds).ExKindsToSubKindsTypeVal()) {
			case KEnumVal.LV_TARGET_SUB_KINDS_TYPE: return a_oTargetInfo.m_oAbilityTargetInfoDict.GetValueOrDefault(Factory.MakeUniqueTargetInfoID(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_LV), STTargetInfo.INVALID).m_stValInfo.m_nVal >= a_stTargetInfo.m_stValInfo.m_nVal;
			case KEnumVal.EXP_TARGET_SUB_KINDS_TYPE: return a_oTargetInfo.m_oAbilityTargetInfoDict.GetValueOrDefault(Factory.MakeUniqueTargetInfoID(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_EXP), STTargetInfo.INVALID).m_stValInfo.m_nVal >= a_stTargetInfo.m_stValInfo.m_nVal;
			case KEnumVal.NUMS_TARGET_SUB_KINDS_TYPE: return a_oTargetInfo.m_oAbilityTargetInfoDict.GetValueOrDefault(Factory.MakeUniqueTargetInfoID(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS), STTargetInfo.INVALID).m_stValInfo.m_nVal >= a_stTargetInfo.m_stValInfo.m_nVal;
			case KEnumVal.ENHANCE_TARGET_SUB_KINDS_TYPE: return a_oTargetInfo.m_oAbilityTargetInfoDict.GetValueOrDefault(Factory.MakeUniqueTargetInfoID(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_ENHANCE), STTargetInfo.INVALID).m_stValInfo.m_nVal >= a_stTargetInfo.m_stValInfo.m_nVal;
		}

		return false;
	}

	/** 아이템 타겟 교환 가능 여부를 검사한다 */
	private static bool IsEnableItemTargetTrade(CItemInfo a_oItemInfo, STTargetInfo a_stTargetInfo) {
		var stItemInfo = CItemInfoTable.Inst.GetItemInfo((EItemKinds)a_stTargetInfo.Kinds);
		CAccess.Assert(a_oItemInfo != null || !CItemInfoTable.Inst.GetItemInfo((EItemKinds)a_stTargetInfo.Kinds).m_stCommonInfo.m_bIsRepeat);

		return Access.DoIsEnableTrade(a_oItemInfo ?? CUserInfoStorage.Inst.GetItemInfo((EItemKinds)a_stTargetInfo.Kinds), a_stTargetInfo);
	}

	/** 스킬 타겟 교환 가능 여부를 검사한다 */
	private static bool IsEnableSkillTargetTrade(CSkillInfo a_oSkillInfo, STTargetInfo a_stTargetInfo) {
		var stSkillInfo = CSkillInfoTable.Inst.GetSkillInfo((ESkillKinds)a_stTargetInfo.Kinds);
		CAccess.Assert(a_oSkillInfo != null || !CSkillInfoTable.Inst.GetSkillInfo((ESkillKinds)a_stTargetInfo.Kinds).m_stCommonInfo.m_bIsRepeat);

		return Access.DoIsEnableTrade(a_oSkillInfo ?? CUserInfoStorage.Inst.GetSkillInfo((ESkillKinds)a_stTargetInfo.Kinds), a_stTargetInfo);
	}

	/** 객체 타겟 교환 가능 여부를 검사한다 */
	private static bool IsEnableObjTargetTrade(CObjInfo a_oObjInfo, STTargetInfo a_stTargetInfo) {
		var stObjInfo = CObjInfoTable.Inst.GetObjInfo((EObjKinds)a_stTargetInfo.Kinds);
		CAccess.Assert(a_oObjInfo != null || !CObjInfoTable.Inst.GetObjInfo((EObjKinds)a_stTargetInfo.Kinds).m_stCommonInfo.m_bIsRepeat);

		return Access.DoIsEnableTrade(a_oObjInfo ?? CUserInfoStorage.Inst.GetObjInfo((EObjKinds)a_stTargetInfo.Kinds), a_stTargetInfo);
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
