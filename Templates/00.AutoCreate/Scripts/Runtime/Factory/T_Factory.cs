#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 기본 팩토리 */
public static partial class Factory {
	#region 클래스 함수
	/** 타겟 정보 고유 식별자를 생성한다 */
	public static ulong MakeUniqueTargetInfoID(ETargetKinds a_eTargetKinds, int a_nKinds) {
		return ((ulong)a_eTargetKinds << (sizeof(int) * KCDefine.B_UNIT_BITS_PER_BYTE)) & (ulong)a_nKinds;
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
	public static CItemTargetInfo MakeItemTargetInfo(EItemKinds a_eItemKinds, long a_nLV = KCDefine.B_VAL_1_INT, long a_nNums = KCDefine.B_VAL_1_INT, CTargetInfo a_oOwnerTargetInfo = null) {
		var oItemTargetInfo = new CItemTargetInfo() {
			ItemKinds = a_eItemKinds, m_oOwnerTargetInfo = a_oOwnerTargetInfo
		};

		var stItemInfo = CItemInfoTable.Inst.GetItemInfo(a_eItemKinds);
		stItemInfo.m_oAbilityTargetInfoDict.ExCopyTo(oItemTargetInfo.m_oAbilityTargetInfoDict, (a_stTargetInfo) => a_stTargetInfo);
		Factory.MakeDefAbilityTargetInfos(a_nLV, a_nNums).ExCopyTo(oItemTargetInfo.m_oAbilityTargetInfoDict, (a_stTargetInfo) => a_stTargetInfo);

		oItemTargetInfo.OnAfterDeserialize();
		return oItemTargetInfo;
	}

	/** 스킬 타겟 정보를 생성한다 */
	public static CSkillTargetInfo MakeSkillTargetInfo(ESkillKinds a_eSkillKinds, long a_nLV = KCDefine.B_VAL_1_INT, long a_nNums = KCDefine.B_VAL_1_INT, CTargetInfo a_oOwnerTargetInfo = null) {
		var oSkillTargetInfo = new CSkillTargetInfo() {
			SkillKinds = a_eSkillKinds, m_oOwnerTargetInfo = a_oOwnerTargetInfo
		};

		var stSkillInfo = CSkillInfoTable.Inst.GetSkillInfo(a_eSkillKinds);
		stSkillInfo.m_oAbilityTargetInfoDict.ExCopyTo(oSkillTargetInfo.m_oAbilityTargetInfoDict, (a_stTargetInfo) => a_stTargetInfo);
		Factory.MakeDefAbilityTargetInfos(a_nLV, a_nNums).ExCopyTo(oSkillTargetInfo.m_oAbilityTargetInfoDict, (a_stTargetInfo) => a_stTargetInfo);

		oSkillTargetInfo.OnAfterDeserialize();
		return oSkillTargetInfo;
	}

	/** 객체 타겟 정보를 생성한다 */
	public static CObjTargetInfo MakeObjTargetInfo(EObjKinds a_eObjKinds, long a_nLV = KCDefine.B_VAL_1_INT, long a_nNums = KCDefine.B_VAL_1_INT, CTargetInfo a_oOwnerTargetInfo = null) {
		var oObjTargetInfo = new CObjTargetInfo() {
			ObjKinds = a_eObjKinds, m_oOwnerTargetInfo = a_oOwnerTargetInfo
		};

		var stObjInfo = CObjInfoTable.Inst.GetObjInfo(a_eObjKinds);
		stObjInfo.m_oAbilityTargetInfoDict.ExCopyTo(oObjTargetInfo.m_oAbilityTargetInfoDict, (a_stTargetInfo) => a_stTargetInfo);
		Factory.MakeDefAbilityTargetInfos(a_nLV, a_nNums).ExCopyTo(oObjTargetInfo.m_oAbilityTargetInfoDict, (a_stTargetInfo) => a_stTargetInfo);
		
		oObjTargetInfo.OnAfterDeserialize();
		return oObjTargetInfo;
	}

	/** 기본 어빌리티 타겟 정보를 생성한다 */
	private static Dictionary<ulong, STTargetInfo> MakeDefAbilityTargetInfos(long a_nLV, long a_nNums) {
		return new Dictionary<ulong, STTargetInfo>() {
			[Factory.MakeUniqueTargetInfoID(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_LV)] = Factory.MakeTargetInfo(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_LV, new STValInfo() { m_nVal = a_nLV, m_eValType = EValType.INT }),
			[Factory.MakeUniqueTargetInfoID(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_EXP)] = Factory.MakeTargetInfo(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_EXP, new STValInfo() { m_nVal = KCDefine.B_VAL_0_INT, m_eValType = EValType.INT }),
			[Factory.MakeUniqueTargetInfoID(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS)] = Factory.MakeTargetInfo(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS, new STValInfo() { m_nVal = a_nNums, m_eValType = EValType.INT }),
			[Factory.MakeUniqueTargetInfoID(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_ENHANCE)] = Factory.MakeTargetInfo(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_ENHANCE, new STValInfo() { m_nVal = KCDefine.B_VAL_0_INT, m_eValType = EValType.INT })
		};
	}
	#endregion			// 클래스 함수

	#region 제네릭 클래스 함수
	/** 키 정보를 생성한다 */
	public static List<(T, GameObject)> MakeKeyInfos<T>(List<(T, GameObject, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, GameObject)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2));
		}

		return oKeyInfoList;
	}

	/** 키 정보를 생성한다 */
	public static List<(T, string, GameObject)> MakeKeyInfos<T>(List<(T, string, GameObject, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, string, GameObject)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2, a_oKeyInfoList[i].Item3));
		}

		return oKeyInfoList;
	}
	
	/** 키 정보를 생성한다 */
	public static List<(T, string, GameObject, GameObject)> MakeKeyInfos<T>(List<(T, string, GameObject, GameObject, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, string, GameObject, GameObject)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2, a_oKeyInfoList[i].Item3, a_oKeyInfoList[i].Item4));
		}

		return oKeyInfoList;
	}
	#endregion			// 제네릭 클래스 함수

	#region 조건부 클래스 함수
#if FIREBASE_MODULE_ENABLE
	/** 유저 정보 노드를 생성한다 */
	public static List<string> MakeUserInfoNodes() {
		return CFactory.MakeUserInfoNodes();
	}

	/** 타겟 정보 노드를 생성한다 */
	public static List<string> MakeTargetInfoNodes() {
		return CFactory.MakeTargetInfoNodes();
	}

	/** 결제 정보 노드를 생성한다 */
	public static List<string> MakePurchaseInfoNodes() {
		return CFactory.MakePurchaseInfoNodes();
	}
#endif			// #if FIREBASE_MODULE_ENABLE
	#endregion			// 조건부 클래스 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
