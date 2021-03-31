using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#if NEVER_USE_THIS
//! 판매 아이템 정보
[System.Serializable]
public struct STSaleItemInfo {
	public int m_nPrice;

	public string m_oName;
	public string m_oDesc;

	public EPriceType m_ePriceType;
	public EPriceKinds m_ePriceKinds;

	public ESaleItemKinds m_eSaleItemKinds;
	public STItemInfo m_stItemInfo;

	#region 함수
	//! 생성자
	public STSaleItemInfo(SimpleJSON.JSONNode a_oNode) {
		m_nPrice = a_oNode[KDefine.G_KEY_ITEM_IT_PRICE].AsInt;

		m_oName = a_oNode[KDefine.G_KEY_ITEM_IT_NAME];
		m_oDesc = a_oNode[KDefine.G_KEY_ITEM_IT_DESC];

		m_ePriceType = (EPriceType)a_oNode[KDefine.G_KEY_ITEM_IT_PRICE_TYPE].AsInt;
		m_ePriceKinds = (EPriceKinds)a_oNode[KDefine.G_KEY_ITEM_IT_PRICE_KINDS].AsInt;
		m_eSaleItemKinds = (ESaleItemKinds)a_oNode[KDefine.G_KEY_ITEM_IT_SALE_ITEM_KINDS].AsInt;

		m_stItemInfo = new STItemInfo() {
			m_nNumItems = a_oNode[KDefine.G_KEY_ITEM_IT_NUM_ITEMS].AsInt,
			m_eItemKinds = (EItemKinds)a_oNode[KDefine.G_KEY_ITEM_IT_ITEM_KINDS].AsInt
		};
	}
	#endregion			// 함수
}

//! 판매 아이템 정보 테이블
public class CItemInfoTable : CScriptableObj<CItemInfoTable> {
	#region 변수
	[Header("Item Info")]
	[SerializeField] private List<STSaleItemInfo> m_oItemInfoList = new List<STSaleItemInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public List<STSaleItemInfo> ItemInfoList => m_oItemInfoList;
	#endregion			// 프로퍼티

	#region 함수
	//! 판매 아이템 정보를 반환한다
	public STSaleItemInfo GetSaleItemInfo(ESaleItemKinds a_eItemKinds) {
		bool bIsValid = this.TryGetSaleItemInfo(a_eItemKinds, out STSaleItemInfo stItemInfo);
		CAccess.Assert(bIsValid);

		return stItemInfo;
	}
	
	//! 판매 아이템 정보를 반환한다
	public bool TryGetSaleItemInfo(ESaleItemKinds a_eItemKinds, out STSaleItemInfo a_stOutItemInfo) {
		int nIdx = m_oItemInfoList.ExFindValue((a_stItemInfo) => a_stItemInfo.m_eSaleItemKinds == a_eItemKinds);
		a_stOutItemInfo = m_oItemInfoList.ExIsValidIdx(nIdx) ? m_oItemInfoList[nIdx] : default(STSaleItemInfo);

		return m_oItemInfoList.ExIsValidIdx(nIdx);
	}

	public List<STSaleItemInfo> LoadSaleItemInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());

		var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr);
		var oSaleItemInfos = oJSONNode[KCDefine.B_KEY_JSON_COMMON_DATA];

		for(int i = 0; i < oSaleItemInfos.Count; ++i) {
			var stItemInfo = new STSaleItemInfo(oSaleItemInfos[i]);
			m_oItemInfoList.Add(stItemInfo);
		}

		return m_oItemInfoList;
	}

	//! 판매 아이템 정보를 로드한다
	public List<STSaleItemInfo> LoadSaleItemInfosFromFile(string a_oFilePath) {
		string oJSONStr = CFunc.ReadStr(a_oFilePath);
		return this.LoadSaleItemInfos(oJSONStr);
	}

	//! 판매 아이템 정보를 로드한다
	public List<STSaleItemInfo> LoadSaleItemInfosFromRes(string a_oFilePath) {
		try {
			var oTextAsset = CResManager.Inst.GetRes<TextAsset>(a_oFilePath);
			return this.LoadSaleItemInfos(oTextAsset.text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
