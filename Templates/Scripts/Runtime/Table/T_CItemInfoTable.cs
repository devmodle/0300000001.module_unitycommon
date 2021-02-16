using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#if NEVER_USE_THIS
//! 아이템 정보
[System.Serializable]
public struct STItemInfo {
	public int m_nPrice;

	public EPriceType m_ePriceType;
	public EPriceKinds m_ePriceKinds;

	public string m_oName;
	public string m_oDesc;

	public STSaleItemInfo m_stSaleItemInfo;

	#region 함수
	//! 생성자
	public STItemInfo(SimpleJSON.JSONNode a_oNode) {
		m_nPrice = a_oNode[KDefine.G_KEY_ITEM_IT_PRICE].AsInt;
		
		m_ePriceType = (EPriceType)a_oNode[KDefine.G_KEY_ITEM_IT_PRICE_TYPE].AsInt;
		m_ePriceKinds = (EPriceKinds)a_oNode[KDefine.G_KEY_ITEM_IT_PRICE_KINDS].AsInt;

		m_oName = a_oNode[KDefine.G_KEY_ITEM_IT_NAME];
		m_oDesc = a_oNode[KDefine.G_KEY_ITEM_IT_DESC];

		m_stSaleItemInfo = new STSaleItemInfo() {
			m_nNumItems = a_oNode[KDefine.G_KEY_ITEM_IT_NUM_ITEMS].AsInt,
			m_eItemKinds = (EItemKinds)a_oNode[KDefine.G_KEY_ITEM_IT_ITEM_KINDS].AsInt
		};
	}
	#endregion			// 함수
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
	public List<STItemInfo> LoadItemInfos(string a_oJSONString) {
		CAccess.Assert(a_oJSONString.ExIsValid());
		
		var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONString);
		var oItemInfos = oJSONNode[KCDefine.B_KEY_JSON_COMMON_DATA];

		for(int i = 0; i < oItemInfos.Count; ++i) {
			var oItemInfo = oItemInfos[i];
			var stItemInfo = new STItemInfo(oItemInfo);

			int nIdx = m_oItemInfoList.ExFindValue((a_stItemInfo) => a_stItemInfo.m_stSaleItemInfo.m_eItemKinds == stItemInfo.m_stSaleItemInfo.m_eItemKinds);
			int nReplace = int.Parse(oItemInfo[KDefine.G_KEY_ITEM_IT_REPLACE]);

			// 아이템 정보가 없을 경우
			if(!m_oItemInfoList.ExIsValidIdx(nIdx)) {
				m_oItemInfoList.Add(stItemInfo);
			} else if(nReplace != KCDefine.B_VALUE_INT_0) {
				m_oItemInfoList[nIdx] = stItemInfo;
			}
		}

		return m_oItemInfoList;
	}

	//! 아이템 정보를 로드한다
	public List<STItemInfo> LoadItemInfosFromFile(string a_oFilePath) {
		string oString = CFunc.ReadString(a_oFilePath);
		return this.LoadItemInfos(oString);
	}

	//! 아이템 정보를 로드한다
	public List<STItemInfo> LoadItemInfosFromRes(string a_oFilePath) {
		try {
			var oTextAsset = CResManager.Inst.GetRes<TextAsset>(a_oFilePath);
			return this.LoadItemInfos(oTextAsset.text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
	}

	//! 아이템 정보를 반환한다
	public STItemInfo GetItemInfo(EItemKinds a_eItemKinds) {
		bool bIsValid = this.TryGetItemInfo(a_eItemKinds, out STItemInfo stItemInfo);
		CAccess.Assert(bIsValid);

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
