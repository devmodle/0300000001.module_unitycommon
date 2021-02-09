using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 아이템 정보
[System.Serializable]
public struct STItemInfo {
	public int m_nPrice;

	public string m_oName;
	public string m_oDesc;

	public EPriceType m_ePriceType;
	public STSaleItemInfo m_stSaleItemInfo;
}

//! 아이템 정보 테이블
public class CItemInfoTable : CScriptableObj<CItemInfoTable> {
	#region 변수
	[Header("Item Info")]
	[SerializeField] private List<STItemInfo> m_oItemInfoList = new List<STItemInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public List<STItemInfo> ItemInfoList => m_oItemInfoList;
	#endregion			// 프로퍼티

	#region 함수
	//! 아이템 정보를 반환한다
	public STItemInfo GetItemInfo(EItemKinds a_eItemKinds) {
		CAccess.Assert(this.TryGetItemInfo(a_eItemKinds, out STItemInfo stItemInfo));
		return stItemInfo;
	}
	
	//! 아이템 정보를 반환한다
	public bool TryGetItemInfo(EItemKinds a_eItemKinds, out STItemInfo a_stOutItemInfo) {
		int nIdx = m_oItemInfoList.ExFindValue((a_stItemInfo) => a_stItemInfo.m_stSaleItemInfo.m_eItemKinds == a_eItemKinds);
		a_stOutItemInfo = m_oItemInfoList.ExIsValidIdx(nIdx) ? m_oItemInfoList[nIdx] : default(STItemInfo);

		return m_oItemInfoList.ExIsValidIdx(nIdx);
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
