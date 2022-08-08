#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 전역 팩토리 */
public static partial class Factory {
	#region 클래스 함수
	/** 타겟 정보 고유 식별자를 생성한다 */
	public static ulong MakeUniqueTargetInfoID(ETargetKinds a_eTargetKinds, int a_nKinds) {
		return ((ulong)a_eTargetKinds << (sizeof(int) * KCDefine.B_UNIT_BITS_PER_BYTE)) | (uint)a_nKinds;
	}

	/** 타겟 정보를 생성한다 */
	public static STTargetInfo MakeTargetInfo(ETargetKinds a_eTargetKinds, int a_nKinds, STValInfo a_stValInfo, EKindsGroupType a_eKindsGroupType = EKindsGroupType.NONE) {
		return Factory.MakeTargetInfo(a_eTargetKinds, a_nKinds, a_stValInfo, STValInfo.INVALID, a_eKindsGroupType);
	}

	/** 타겟 정보를 생성한다 */
	public static STTargetInfo MakeTargetInfo(ETargetKinds a_eTargetKinds, int a_nKinds, STValInfo a_stValInfo01, STValInfo a_stValInfo02, EKindsGroupType a_eKindsGroupType = EKindsGroupType.NONE) {
		return Factory.MakeTargetInfo(a_eTargetKinds, a_nKinds, a_stValInfo01, a_stValInfo02, STValInfo.INVALID, a_eKindsGroupType);
	}

	/** 타겟 정보를 생성한다 */
	public static STTargetInfo MakeTargetInfo(ETargetKinds a_eTargetKinds, int a_nKinds, STValInfo a_stValInfo01, STValInfo a_stValInfo02, STValInfo a_stValInfo03, EKindsGroupType a_eKindsGroupType = EKindsGroupType.NONE) {
		return new STTargetInfo() {
			m_nKinds = a_nKinds, m_eTargetKinds = a_eTargetKinds, m_eKindsGroupType = a_eKindsGroupType, m_stValInfo01 = a_stValInfo01, m_stValInfo02 = a_stValInfo02, m_stValInfo03 = a_stValInfo03
		};
	}

	/** 클리어 정보를 생성한다 */
	public static CClearInfo MakeClearInfo(int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		var oClearInfo = new CClearInfo() {
			m_stIDInfo = CFactory.MakeIDInfo(a_nLevelID, a_nStageID, a_nChapterID)
		};

		oClearInfo.OnAfterDeserialize();
		return oClearInfo;
	}

	/** 아이템 타겟 정보를 생성한다 */
	public static CItemTargetInfo MakeItemTargetInfo(EItemKinds a_eItemKinds, CTargetInfo a_oOwnerTargetInfo = null) {
		var oItemTargetInfo = new CItemTargetInfo() {
			ItemKinds = a_eItemKinds, m_oOwnerTargetInfo = a_oOwnerTargetInfo, m_stIdxInfo = STIdxInfo.INVALID
		};

		Factory.MakeDefAbilityTargetInfos().ExCopyTo(oItemTargetInfo.m_oAbilityTargetInfoDict, (a_stTargetInfo) => a_stTargetInfo, false);
		oItemTargetInfo.OnAfterDeserialize();

		return oItemTargetInfo;
	}

	/** 스킬 타겟 정보를 생성한다 */
	public static CSkillTargetInfo MakeSkillTargetInfo(ESkillKinds a_eSkillKinds, CTargetInfo a_oOwnerTargetInfo = null) {
		var oSkillTargetInfo = new CSkillTargetInfo() {
			SkillKinds = a_eSkillKinds, m_oOwnerTargetInfo = a_oOwnerTargetInfo, m_stIdxInfo = STIdxInfo.INVALID
		};

		Factory.MakeDefAbilityTargetInfos().ExCopyTo(oSkillTargetInfo.m_oAbilityTargetInfoDict, (a_stTargetInfo) => a_stTargetInfo, false);
		oSkillTargetInfo.OnAfterDeserialize();

		return oSkillTargetInfo;
	}

	/** 객체 타겟 정보를 생성한다 */
	public static CObjTargetInfo MakeObjTargetInfo(EObjKinds a_eObjKinds, CTargetInfo a_oOwnerTargetInfo = null) {
		var oObjTargetInfo = new CObjTargetInfo() {
			ObjKinds = a_eObjKinds, m_oOwnerTargetInfo = a_oOwnerTargetInfo, m_stIdxInfo = STIdxInfo.INVALID
		};

		Factory.MakeDefAbilityTargetInfos().ExCopyTo(oObjTargetInfo.m_oAbilityTargetInfoDict, (a_stTargetInfo) => a_stTargetInfo, false);
		oObjTargetInfo.OnAfterDeserialize();

		return oObjTargetInfo;
	}

	/** 캐릭터 유저 정보를 생성한다 */
	public static CCharacterUserInfo MakeCharacterUserInfo(EObjKinds a_eObjKinds, STIdxInfo a_stIdxInfo) {
		var oCharacterUserInfo = new CCharacterUserInfo() {
			ObjKinds = a_eObjKinds, m_stIdxInfo = a_stIdxInfo
		};

		Factory.MakeDefAbilityTargetInfos().ExCopyTo(oCharacterUserInfo.m_oAbilityTargetInfoDict, (a_stTargetInfo) => a_stTargetInfo, false);
		oCharacterUserInfo.OnAfterDeserialize();

		return oCharacterUserInfo;
	}

	/** 기본 어빌리티 타겟 정보를 생성한다 */
	private static Dictionary<ulong, STTargetInfo> MakeDefAbilityTargetInfos() {
		return new Dictionary<ulong, STTargetInfo>() {
			[Factory.MakeUniqueTargetInfoID(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_LV)] = Factory.MakeTargetInfo(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_LV, new STValInfo() { m_nVal = KCDefine.B_VAL_0_INT, m_eValType = EValType.INT }),
			[Factory.MakeUniqueTargetInfoID(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_EXP)] = Factory.MakeTargetInfo(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_EXP, new STValInfo() { m_nVal = KCDefine.B_VAL_0_INT, m_eValType = EValType.INT }),
			[Factory.MakeUniqueTargetInfoID(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS)] = Factory.MakeTargetInfo(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS, new STValInfo() { m_nVal = KCDefine.B_VAL_0_INT, m_eValType = EValType.INT }),
			[Factory.MakeUniqueTargetInfoID(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_ENHANCE)] = Factory.MakeTargetInfo(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_ENHANCE, new STValInfo() { m_nVal = KCDefine.B_VAL_0_INT, m_eValType = EValType.INT })
		};
	}
	#endregion			// 클래스 함수
}

/** 초기화 씬 팩토리 */
public static partial class Factory {
	#region 클래스 함수
	
	#endregion			// 클래스 함수
}

/** 시작 씬 팩토리 */
public static partial class Factory {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 설정 씬 팩토리 */
public static partial class Factory {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 약관 동의 씬 팩토리 */
public static partial class Factory {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 지연 설정 씬 팩토리 */
public static partial class Factory {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 타이틀 씬 팩토리 */
public static partial class Factory {
	#region 클래스 함수
	
	#endregion			// 클래스 함수
}

/** 메인 씬 팩토리 */
public static partial class Factory {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 게임 씬 팩토리 */
public static partial class Factory {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 로딩 씬 팩토리 */
public static partial class Factory {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 중첩 씬 팩토리 */
public static partial class Factory {
	#region 클래스 함수
	
	#endregion			// 클래스 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
