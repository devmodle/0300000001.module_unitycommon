using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 판매 상품 정보
[System.Serializable]
public struct STSaleProductInfo {
	[HideInInspector] public int m_nID;

	public string m_oName;
	public string m_oDesc;

	public EPriceType m_ePriceType;
	public List<STSaleItemInfo> m_oSaleItemInfoList;
}

//! 판매 상품 정보 테이블
public class CSaleProductInfoTable : CScriptableObj<CSaleProductInfoTable> {
	#region 변수
	[Header("Sale Product Info")]
	[SerializeField] private List<STSaleProductInfo> m_oSaleProductInfoList = new List<STSaleProductInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public List<STSaleProductInfo> SaleProductInfoList => m_oSaleProductInfoList;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		for(int i = 0; i < m_oSaleProductInfoList.Count; ++i) {
			var stSaleProductInfo = m_oSaleProductInfoList[i];
			stSaleProductInfo.m_nID = i;

			m_oSaleProductInfoList[i] = stSaleProductInfo;
		}
	}

	//! 판매 아이템 정보 포함 여부를 검사한다
	public bool IsContainsSaleItemInfo(int a_nID, EItemKinds a_eItemKinds) {
		return this.TryGetSaleItemInfo(a_nID, a_eItemKinds, out STSaleItemInfo stSaleItemInfo);
	}
	
	//! 판매 코인 개수를 반환한다
	public int GetNumSaleCoins(int a_nID) {
		bool bIsValid = this.TryGetSaleItemInfo(a_nID, EItemKinds.GOODS_COIN, out STSaleItemInfo stSaleItemInfo);
		return bIsValid ? stSaleItemInfo.m_nNumItems : KCDefine.B_VALUE_INT_0;
	}

	//! 판매 상품 정보를 반환한다
	public STSaleProductInfo GetSaleProductInfo(int a_nID) {
		CAccess.Assert(this.TryGetSaleProductInfo(a_nID, out STSaleProductInfo stSaleProductInfo));
		return stSaleProductInfo;
	}

	//! 판매 아이템 정보를 반환한다
	public STSaleItemInfo GetSaleItemInfo(int a_nID, EItemKinds a_eItemKinds) {
		CAccess.Assert(this.TryGetSaleItemInfo(a_nID, a_eItemKinds, out STSaleItemInfo stSaleItemInfo));
		return stSaleItemInfo;
	}

	//! 판매 상품 정보를 반환한다
	public bool TryGetSaleProductInfo(int a_nID, out STSaleProductInfo a_stOutSaleProductInfo) {
		a_stOutSaleProductInfo = m_oSaleProductInfoList.ExIsValidIdx(a_nID) ? m_oSaleProductInfoList[a_nID] : default(STSaleProductInfo);
		return m_oSaleProductInfoList.ExIsValidIdx(a_nID);
	}

	//! 판매 아이템 정보를 반환한다
	public bool TryGetSaleItemInfo(int a_nID, EItemKinds a_eItemKinds, out STSaleItemInfo a_stOutSaleItemInfo) {
		CAccess.Assert(this.TryGetSaleProductInfo(a_nID, out STSaleProductInfo stSaleProductInfo));

		int nIdx = stSaleProductInfo.m_oSaleItemInfoList.ExFindValue((a_stSaleItemInfo) => a_stSaleItemInfo.m_eItemKinds == a_eItemKinds);
		a_stOutSaleItemInfo = stSaleProductInfo.m_oSaleItemInfoList.ExIsValidIdx(nIdx) ? stSaleProductInfo.m_oSaleItemInfoList[nIdx] : default(STSaleItemInfo);

		return stSaleProductInfo.m_oSaleItemInfoList.ExIsValidIdx(nIdx);
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
