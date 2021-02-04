using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 판매 상품 정보
[System.Serializable]
public struct STSaleProductInfo {
	public string m_oName;
	public string m_oDesc;
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
	//! 판매 상품 정보를 반환한다
	public STSaleProductInfo GetSaleProductInfo(int a_nIdx) {
		CAccess.Assert(m_oSaleProductInfoList.ExIsValidIdx(a_nIdx));
		return m_oSaleProductInfoList[a_nIdx];
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
