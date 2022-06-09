using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MessagePack;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#region 기본
/** 가격 정보 */
[System.Serializable]
public struct STPriceInfo {
	public string m_oPrice;
	public EPriceKinds m_ePriceKinds;

	#region 프로퍼티
	public long IntPrice => long.TryParse(m_oPrice, out long nPrice) ? nPrice : KCDefine.B_VAL_0_LONG;
	public double RealPrice => double.TryParse(m_oPrice, out double dblPrice) ? dblPrice : KCDefine.B_VAL_0_DBL;
	public EPriceType PriceType => (EPriceType)((int)m_ePriceKinds).ExKindsToType();
	#endregion			// 프로퍼티
}

/** 아이템 개수 정보 */
[System.Serializable]
public struct STNumItemsInfo {
	public long m_nNumItems;
	public EItemKinds m_eItemKinds;
}

/** 어빌리티 값 정보 */
[System.Serializable]
public struct STAbilityValInfo {
	public long m_nLV;
	public EAbilityKinds m_eAbilityKinds;
}

/** 타입 랩퍼 */
[MessagePackObject]
public struct STTypeWrapper {
	[Key(51)] public List<long> m_oUniqueLevelIDList;

	[Key(161)] public Dictionary<int, Dictionary<int, int>> m_oNumLevelInfosDictContainer;
	[Key(162)] public Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>> m_oLevelInfoDictContainer;
}
#endregion			// 기본
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
