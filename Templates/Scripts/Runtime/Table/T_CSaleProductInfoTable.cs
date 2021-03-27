using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 판매 상품 정보
[System.Serializable]
public struct STSaleProductInfo {
	public string m_oName;
	public string m_oDesc;

	public EPriceType m_ePriceType;
	public EPriceKinds m_ePriceKinds;

	public List<STSaleItemInfo> m_oSaleItemInfoList;

	#region 함수
	//! 생성자
	public STSaleProductInfo(SimpleJSON.JSONNode a_oNode) {
		m_oName = a_oNode[KDefine.G_KEY_SALE_PIT_NAME];
		m_oDesc = a_oNode[KDefine.G_KEY_SALE_PIT_DESC];

		m_ePriceType = (EPriceType)a_oNode[KDefine.G_KEY_SALE_PIT_PRICE_TYPE].AsInt;
		m_ePriceKinds = (EPriceKinds)a_oNode[KDefine.G_KEY_SALE_PIT_PRICE_KINDS].AsInt;

		m_oSaleItemInfoList = new List<STSaleItemInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_SALE_ITEM_INFOS; ++i) {
			string oNumItemsKey = string.Format(KDefine.G_KEY_FMT_SALE_PIT_NUM_ITEMS, i + KCDefine.B_VALUE_1_INT);
			string oItemKindsKey = string.Format(KDefine.G_KEY_FMT_SALE_PIT_SALE_ITEM_KINDS, i + KCDefine.B_VALUE_1_INT);

			var stSaleItemInfo = new STSaleItemInfo() {
				m_nNumItems = a_oNode[oNumItemsKey].AsInt,
				m_eItemKinds = (EItemKinds)a_oNode[oItemKindsKey].AsInt
			};

			m_oSaleItemInfoList.Add(stSaleItemInfo);
		}
	}
	#endregion			// 함수
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
	//! 판매 아이템 정보 포함 여부를 검사한다
	public bool IsContainsSaleItemInfo(int a_nID, EItemKinds a_eItemKinds) {
		return this.TryGetSaleItemInfo(a_nID, a_eItemKinds, out STSaleItemInfo stSaleItemInfo);
	}
	
	//! 판매 코인 개수를 반환한다
	public int GetNumSaleCoins(int a_nID) {
		bool bIsValid = this.TryGetSaleItemInfo(a_nID, EItemKinds.GOODS_COIN, out STSaleItemInfo stSaleItemInfo);
		return bIsValid ? stSaleItemInfo.m_nNumItems : KCDefine.B_VALUE_0_INT;
	}

	//! 판매 상품 정보를 반환한다
	public STSaleProductInfo GetSaleProductInfo(int a_nID) {
		bool bIsValid = this.TryGetSaleProductInfo(a_nID, out STSaleProductInfo stSaleProductInfo);
		CAccess.Assert(bIsValid);

		return stSaleProductInfo;
	}

	//! 판매 아이템 정보를 반환한다
	public STSaleItemInfo GetSaleItemInfo(int a_nID, EItemKinds a_eItemKinds) {
		bool bIsValid = this.TryGetSaleItemInfo(a_nID, a_eItemKinds, out STSaleItemInfo stSaleItemInfo);
		CAccess.Assert(bIsValid);

		return stSaleItemInfo;
	}

	//! 판매 상품 정보를 반환한다
	public bool TryGetSaleProductInfo(int a_nID, out STSaleProductInfo a_stOutSaleProductInfo) {
		a_stOutSaleProductInfo = m_oSaleProductInfoList.ExIsValidIdx(a_nID) ? m_oSaleProductInfoList[a_nID] : default(STSaleProductInfo);
		return m_oSaleProductInfoList.ExIsValidIdx(a_nID);
	}

	//! 판매 아이템 정보를 반환한다
	public bool TryGetSaleItemInfo(int a_nID, EItemKinds a_eItemKinds, out STSaleItemInfo a_stOutSaleItemInfo) {
		// 판매 상품 정보가 존재 할 경우
		if(this.TryGetSaleProductInfo(a_nID, out STSaleProductInfo stSaleProductInfo)) {
			int nIdx = stSaleProductInfo.m_oSaleItemInfoList.ExFindValue((a_stSaleItemInfo) => a_stSaleItemInfo.m_eItemKinds == a_eItemKinds);
			a_stOutSaleItemInfo = stSaleProductInfo.m_oSaleItemInfoList.ExIsValidIdx(nIdx) ? stSaleProductInfo.m_oSaleItemInfoList[nIdx] : default(STSaleItemInfo);

			return stSaleProductInfo.m_oSaleItemInfoList.ExIsValidIdx(nIdx);
		}

		a_stOutSaleItemInfo = default(STSaleItemInfo);
		return false;
	}

	//! 판매 아이템 정보를 로드한다
	public List<STSaleProductInfo> LoadSaleProductInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());

		var oJSONNode = SimpleJSON.JSONNode.Parse(a_oJSONStr);
		var oSaleProductInfos = oJSONNode[KCDefine.B_KEY_JSON_COMMON_DATA];

		for(int i = 0; i < oSaleProductInfos.Count; ++i) {
			var stSaleProductInfo = new STSaleProductInfo(oSaleProductInfos[i]);
			m_oSaleProductInfoList.Add(stSaleProductInfo);
		}

		return m_oSaleProductInfoList;
	}

	//! 판매 아이템 정보를 로드한다
	public List<STSaleProductInfo> LoadSaleProductInfosFromFile(string a_oFilePath) {
		string oJSONStr = CFunc.ReadString(a_oFilePath);
		return this.LoadSaleProductInfos(oJSONStr);
	}

	//! 판매 아이템 정보를 로드한다
	public List<STSaleProductInfo> LoadSaleProductInfosFromRes(string a_oFilePath) {
		try {
			var oTextAsset = CResManager.Inst.GetRes<TextAsset>(a_oFilePath);
			return this.LoadSaleProductInfos(oTextAsset.text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
