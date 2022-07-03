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
	public static bool IsEnableTrade(CUserTargetInfo a_oUserTargetInfo, STTargetInfo a_stTargetInfo) {
		switch(a_stTargetInfo.TargetType) {
			case ETargetType.ITEM: return Access.IsEnableItemTargetTrade(a_oUserTargetInfo as CUserItemInfo, a_stTargetInfo);
			case ETargetType.SKILL: return Access.IsEnableSkillTargetTrade(a_oUserTargetInfo as CUserSkillInfo, a_stTargetInfo);
			case ETargetType.OBJ: return Access.IsEnableObjTargetTrade(a_oUserTargetInfo as CUserObjInfo, a_stTargetInfo);
		}

		return false;
	}

	/** 교환 가능 여부를 검사한다 */
	public static bool IsEnableTrade(List<STTargetInfo> a_oTargetInfoList) {
		CAccess.Assert(a_oTargetInfoList != null);
		return a_oTargetInfoList.All((a_stTargetInfo) => Access.IsEnableTrade(a_stTargetInfo));
	}

	/** 교환 가능 여부를 검사한다 */
	public static bool IsEnableTrade(List<(CUserTargetInfo, STTargetInfo)> a_oTradeInfoList) {
		return a_oTradeInfoList.All((a_stTradeInfo) => Access.IsEnableTrade(a_stTradeInfo.Item1, a_stTradeInfo.Item2));
	}

	/** 튜토리얼 메세지를 반환한다 */
	public static string GetTutorialMsg(ETutorialKinds a_eTutorialKinds, int a_nIdx = KCDefine.B_VAL_0_INT) {
		return CStrTable.Inst.GetStr(string.Format(KCDefine.U_KEY_FMT_TUTORIAL_MSG, a_eTutorialKinds, a_nIdx + KCDefine.B_VAL_1_INT));
	}

	/** 레벨 클리어 정보를 반환한다 */
	public static CClearInfo GetLevelClearInfo(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT, bool a_bIsAutoCreate = false) {
		// 자동 생성 모드 일 경우
		if(a_bIsAutoCreate && !CGameInfoStorage.Inst.TryGetLevelClearInfo(a_nID, out CClearInfo oLevelClearInfo, a_nStageID, a_nChapterID)) {
			CGameInfoStorage.Inst.AddLevelClearInfo(Factory.MakeClearInfo(a_nID, a_nStageID, a_nChapterID));
		}

		return CGameInfoStorage.Inst.TryGetLevelClearInfo(a_nID, out oLevelClearInfo, a_nStageID, a_nChapterID) ? oLevelClearInfo : null;
	}

	/** 유저 아이템 정보를 반환한다 */
	public static CUserItemInfo GetUserItemInfo(EItemKinds a_eItemKinds, bool a_bIsAutoCreate = false) {
		// 자동 생성 모드 일 경우
		if(a_bIsAutoCreate && !CUserInfoStorage.Inst.TryGetUserItemInfo(a_eItemKinds, out CUserItemInfo oUserItemInfo)) {
			CUserInfoStorage.Inst.AddUserItemInfo(Factory.MakeUserItemInfo(a_eItemKinds), CItemInfoTable.Inst.GetItemInfo(a_eItemKinds).m_stCommonInfo.m_bIsDuplicate);
		}

		return CUserInfoStorage.Inst.TryGetUserItemInfo(a_eItemKinds, out oUserItemInfo) ? oUserItemInfo : null;
	}

	/** 유저 스킬 정보를 반환한다 */
	public static CUserSkillInfo GetUserSkillInfo(ESkillKinds a_eSkillKinds, bool a_bIsAutoCreate = false) {
		// 자동 생성 모드 일 경우
		if(a_bIsAutoCreate && !CUserInfoStorage.Inst.TryGetUserSkillInfo(a_eSkillKinds, out CUserSkillInfo oUserSkillInfo)) {
			CUserInfoStorage.Inst.AddUserSkillInfo(Factory.MakeUserSkillInfo(a_eSkillKinds), CSkillInfoTable.Inst.GetSkillInfo(a_eSkillKinds).m_stCommonInfo.m_bIsDuplicate);
		}

		return CUserInfoStorage.Inst.TryGetUserSkillInfo(a_eSkillKinds, out oUserSkillInfo) ? oUserSkillInfo : null;
	}

	/** 유저 객체 정보를 반환한다 */
	public static CUserObjInfo GetUserObjInfo(EObjKinds a_eObjKinds, bool a_bIsAutoCreate = false) {
		// 자동 생성 모드 일 경우
		if(a_bIsAutoCreate && !CUserInfoStorage.Inst.TryGetUserObjInfo(a_eObjKinds, out CUserObjInfo oUserObjInfo)) {
			CUserInfoStorage.Inst.AddUserObjInfo(Factory.MakeUserObjInfo(a_eObjKinds), CObjInfoTable.Inst.GetObjInfo(a_eObjKinds).m_stCommonInfo.m_bIsDuplicate);
		}

		return CUserInfoStorage.Inst.TryGetUserObjInfo(a_eObjKinds, out oUserObjInfo) ? oUserObjInfo : null;
	}

	/** 교환 가능 여부를 검사한다 */
	private static bool DoIsEnableTrade(CUserTargetInfo a_oUserTargetInfo, STTargetInfo a_stTargetInfo) {
		CAccess.Assert(a_oUserTargetInfo != null);

		switch(a_stTargetInfo.m_eTargetKinds) {
			case ETargetKinds.ITEM_LV: return a_oUserTargetInfo.LV >= a_stTargetInfo.IntTargets;
		}

		return a_oUserTargetInfo.Nums >= a_stTargetInfo.IntTargets;
	}

	/** 아이템 타겟 교환 가능 여부를 검사한다 */
	private static bool IsEnableItemTargetTrade(CUserItemInfo a_oUserItemInfo, STTargetInfo a_stTargetInfo) {
		var stItemInfo = CItemInfoTable.Inst.GetItemInfo((EItemKinds)a_stTargetInfo.m_nKinds);
		CAccess.Assert(a_oUserItemInfo != null || !CItemInfoTable.Inst.GetItemInfo((EItemKinds)a_stTargetInfo.m_nKinds).m_stCommonInfo.m_bIsDuplicate);

		return Access.DoIsEnableTrade(a_oUserItemInfo ?? CUserInfoStorage.Inst.GetUserItemInfo((EItemKinds)a_stTargetInfo.m_nKinds), a_stTargetInfo);
	}

	/** 스킬 타겟 교환 가능 여부를 검사한다 */
	private static bool IsEnableSkillTargetTrade(CUserSkillInfo a_oUserSkillInfo, STTargetInfo a_stTargetInfo) {
		var stSkillInfo = CSkillInfoTable.Inst.GetSkillInfo((ESkillKinds)a_stTargetInfo.m_nKinds);
		CAccess.Assert(a_oUserSkillInfo != null || !CSkillInfoTable.Inst.GetSkillInfo((ESkillKinds)a_stTargetInfo.m_nKinds).m_stCommonInfo.m_bIsDuplicate);

		return Access.DoIsEnableTrade(a_oUserSkillInfo ?? CUserInfoStorage.Inst.GetUserSkillInfo((ESkillKinds)a_stTargetInfo.m_nKinds), a_stTargetInfo);
	}

	/** 객체 타겟 교환 가능 여부를 검사한다 */
	private static bool IsEnableObjTargetTrade(CUserObjInfo a_oUserObjInfo, STTargetInfo a_stTargetInfo) {
		var stObjInfo = CObjInfoTable.Inst.GetObjInfo((EObjKinds)a_stTargetInfo.m_nKinds);
		CAccess.Assert(a_oUserObjInfo != null || !CObjInfoTable.Inst.GetObjInfo((EObjKinds)a_stTargetInfo.m_nKinds).m_stCommonInfo.m_bIsDuplicate);

		return Access.DoIsEnableTrade(a_oUserObjInfo ?? CUserInfoStorage.Inst.GetUserObjInfo((EObjKinds)a_stTargetInfo.m_nKinds), a_stTargetInfo);
	}
	#endregion			// 클래스 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
