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
