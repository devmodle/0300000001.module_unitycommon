using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
using MessagePack;
using Newtonsoft.Json;

#region 기본
/** 가격 정보 */
[System.Serializable]
public partial struct STPriceInfo {
	public int m_nKinds;
	public string m_oPrice;
	public EPriceType m_ePriceType;

	#region 프로퍼티
	public long IntPrice => long.TryParse(m_oPrice, out long nPrice) ? nPrice : KCDefine.B_VAL_0_LONG;
	public double RealPrice => double.TryParse(m_oPrice, out double dblPrice) ? dblPrice : KCDefine.B_VAL_0_DBL;
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STPriceInfo(SimpleJSON.JSONNode a_oPriceInfo, int a_nIdx) {
		string oPriceKey = string.Format(KCDefine.U_KEY_FMT_PRICE, a_nIdx + KCDefine.B_VAL_1_INT);
		string oPriceTypeKey = string.Format(KCDefine.U_KEY_FMT_PRICE_TYPE, a_nIdx + KCDefine.B_VAL_1_INT);
		string oPriceKindsKey = string.Format(KCDefine.U_KEY_FMT_PRICE_KINDS, a_nIdx + KCDefine.B_VAL_1_INT);

		m_nKinds = a_oPriceInfo[oPriceKindsKey].ExIsValid() ? a_oPriceInfo[oPriceKindsKey].AsInt : KCDefine.B_IDX_INVALID;
		m_oPrice = a_oPriceInfo[oPriceKey].ExIsValid() ? a_oPriceInfo[oPriceKey] : KCDefine.B_STR_0_INT;
		m_ePriceType = a_oPriceInfo[oPriceTypeKey].ExIsValid() ? (EPriceType)a_oPriceInfo[oPriceTypeKey].AsInt : EPriceType.NONE;
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR || UNITY_STANDALONE
	/** 가격 정보를 생성한다 */
	public void MakePriceInfo(SimpleJSON.JSONClass a_oOutPriceInfo, int a_nIdx) {
		a_oOutPriceInfo.Add(string.Format(KCDefine.U_KEY_FMT_PRICE_TYPE, a_nIdx + KCDefine.B_VAL_1_INT), $"{(int)m_ePriceType}");
		a_oOutPriceInfo.Add(string.Format(KCDefine.U_KEY_FMT_PRICE_KINDS, a_nIdx + KCDefine.B_VAL_1_INT), $"{m_nKinds}");
		a_oOutPriceInfo.Add(string.Format(KCDefine.U_KEY_FMT_PRICE, a_nIdx + KCDefine.B_VAL_1_INT), $"{m_oPrice}");
	}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	#endregion			// 조건부 함수
}

/** 타겟 정보 */
[System.Serializable]
public partial struct STTargetInfo {
	public int m_nKinds;
	public int m_nNumTargets;
	public ETargetType m_eTargetType;

	#region 함수
	/** 생성자 */
	public STTargetInfo(SimpleJSON.JSONNode a_oTargetInfo, int a_nIdx) {
		string oNumTargetsKey = string.Format(KCDefine.U_KEY_FMT_NUM_TARGETS, a_nIdx + KCDefine.B_VAL_1_INT);
		string oTargetTypeKey = string.Format(KCDefine.U_KEY_FMT_TARGET_TYPE, a_nIdx + KCDefine.B_VAL_1_INT);
		string oTargetKindsKey = string.Format(KCDefine.U_KEY_FMT_TARGET_KINDS, a_nIdx + KCDefine.B_VAL_1_INT);

		m_nKinds = a_oTargetInfo[oTargetKindsKey].ExIsValid() ? a_oTargetInfo[oTargetKindsKey].AsInt : KCDefine.B_IDX_INVALID;
		m_nNumTargets = a_oTargetInfo[oNumTargetsKey].ExIsValid() ? a_oTargetInfo[oNumTargetsKey].AsInt : KCDefine.B_VAL_0_INT;
		m_eTargetType = a_oTargetInfo[oTargetTypeKey].ExIsValid() ? (ETargetType)a_oTargetInfo[oTargetTypeKey].AsInt : ETargetType.NONE;
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR || UNITY_STANDALONE
	/** 타겟 정보를 생성한다 */
	public void MakeTargetInfo(SimpleJSON.JSONClass a_oOutTargetInfo, int a_nIdx) {
		a_oOutTargetInfo.Add(string.Format(KCDefine.U_KEY_FMT_TARGET_TYPE, a_nIdx + KCDefine.B_VAL_1_INT), $"{(int)m_eTargetType}");
		a_oOutTargetInfo.Add(string.Format(KCDefine.U_KEY_FMT_TARGET_KINDS, a_nIdx + KCDefine.B_VAL_1_INT), $"{m_nKinds}");
		a_oOutTargetInfo.Add(string.Format(KCDefine.U_KEY_FMT_NUM_TARGETS, a_nIdx + KCDefine.B_VAL_1_INT), $"{m_nNumTargets}");
	}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	#endregion			// 조건부 함수
}

/** 아이템 개수 정보 */
[System.Serializable]
public partial struct STNumItemsInfo {
	public long m_nNumItems;
	public EItemKinds m_eItemKinds;

	#region 프로퍼티
	public EItemType ItemType => (EItemType)((int)m_eItemKinds).ExKindsToType();
	public EItemKinds BaseItemKinds => (EItemKinds)((int)m_eItemKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STNumItemsInfo(SimpleJSON.JSONNode a_oNumItemsInfo, int a_nIdx) {
		string oNumItemsKey = string.Format(KCDefine.U_KEY_FMT_NUM_ITEMS, a_nIdx + KCDefine.B_VAL_1_INT);
		string oItemKindsKey = string.Format(KCDefine.U_KEY_FMT_ITEM_KINDS, a_nIdx + KCDefine.B_VAL_1_INT);

		m_nNumItems = long.TryParse(a_oNumItemsInfo[oNumItemsKey], out long nNumItems) ? nNumItems : KCDefine.B_VAL_0_LONG;
		m_eItemKinds = a_oNumItemsInfo[oItemKindsKey].ExIsValid() ? (EItemKinds)a_oNumItemsInfo[oItemKindsKey].AsInt : EItemKinds.NONE;
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR || UNITY_STANDALONE
	/** 아이템 개수 정보를 생성한다 */
	public void MakeNumItemsInfo(SimpleJSON.JSONClass a_oOutNumItemsInfo, int a_nIdx) {
		a_oOutNumItemsInfo.Add(string.Format(KCDefine.U_KEY_FMT_ITEM_KINDS, a_nIdx + KCDefine.B_VAL_1_INT), $"{(int)m_eItemKinds}");
		a_oOutNumItemsInfo.Add(string.Format(KCDefine.U_KEY_FMT_NUM_ITEMS, a_nIdx + KCDefine.B_VAL_1_INT), $"{m_nNumItems}");
	}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	#endregion			// 조건부 함수
}

/** 어빌리티 값 정보 */
[System.Serializable]
public partial struct STAbilityValInfo {
	public long m_nLV;
	public EAbilityKinds m_eAbilityKinds;

	#region 프로퍼티
	public EAbilityType AbilityType => (EAbilityType)((int)m_eAbilityKinds).ExKindsToType();
	public EAbilityKinds BaseAbilityKinds => (EAbilityKinds)((int)m_eAbilityKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STAbilityValInfo(SimpleJSON.JSONNode a_oAbilityValInfo, int a_nIdx) {
		string oAbilityLVKey = string.Format(KCDefine.U_KEY_FMT_ABILITY_LV, a_nIdx + KCDefine.B_VAL_1_INT);
		string oAbilityKindsKey = string.Format(KCDefine.U_KEY_FMT_ABILITY_KINDS, a_nIdx + KCDefine.B_VAL_1_INT);

		m_nLV = long.TryParse(a_oAbilityValInfo[oAbilityLVKey], out long nLV) ? nLV : KCDefine.B_VAL_0_LONG;
		m_eAbilityKinds = a_oAbilityValInfo[oAbilityKindsKey].ExIsValid() ? (EAbilityKinds)a_oAbilityValInfo[oAbilityKindsKey].AsInt : EAbilityKinds.NONE;
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR || UNITY_STANDALONE
	/** 어빌리티 값 정보를 생성한다 */
	public void MakeAbilityValInfo(SimpleJSON.JSONClass a_oOutAbilityValInfo, int a_nIdx) {
		a_oOutAbilityValInfo.Add(string.Format(KCDefine.U_KEY_FMT_ABILITY_KINDS, a_nIdx + KCDefine.B_VAL_1_INT), $"{(int)m_eAbilityKinds}");
		a_oOutAbilityValInfo.Add(string.Format(KCDefine.U_KEY_FMT_ABILITY_LV, a_nIdx + KCDefine.B_VAL_1_INT), $"{m_nLV}");
	}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	#endregion			// 조건부 함수
}

/** 타입 랩퍼 */
[MessagePackObject]
public partial struct STTypeWrapper {
	[Key(51)] public List<long> m_oUniqueLevelIDList;

	[Key(161)] public Dictionary<int, Dictionary<int, int>> m_oNumLevelInfosDictContainer;
	[Key(162)] public Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>> m_oLevelInfoDictContainer;
}
#endregion			// 기본
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
