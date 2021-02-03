using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 판매 상품 정보
[System.Serializable]
public struct STSaleProductInfoTable {
	public int m_nID;
	public string m_oName;
}

//! 판매 상품 정보 테이블
public class CSaleProductInfoTable : CScriptableObj<CSaleProductInfoTable> {
	#region 변수
	[Header("Sale Product Info")]
	[SerializeField] private List<STSaleProductInfoTable> m_oSaleProductInfoList = new List<STSaleProductInfoTable>();
	#endregion			// 변수

	#region 프로퍼티
	public List<STSaleProductInfoTable> SaleProductInfoList => m_oSaleProductInfoList;
	#endregion			// 프로퍼티
}
#endif			// #if NEVER_USE_THIS
