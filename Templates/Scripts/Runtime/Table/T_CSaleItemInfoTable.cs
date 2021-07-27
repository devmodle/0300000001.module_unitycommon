using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 판매 아이템 정보
[System.Serializable]
public struct STSaleItemInfo {
	public int m_nPrice;

	public string m_oName;
	public string m_oDesc;

	public EPriceKinds m_ePriceKinds;
	public ESaleItemKinds m_eSaleItemKinds;

	public List<STItemInfo> m_oItemInfoList;

	#region 함수
	//! 생성자
	public STSaleItemInfo(SimpleJSON.JSONNode a_oSaleItemInfo) {
		m_nPrice = a_oSaleItemInfo[KDefine.G_KEY_SALE_IIT_PRICE].AsInt;
		
		m_oName = a_oSaleItemInfo[KCDefine.U_KEY_NAME];
		m_oDesc = a_oSaleItemInfo[KCDefine.U_KEY_DESC];

		m_ePriceKinds = (EPriceKinds)a_oSaleItemInfo[KDefine.G_KEY_SALE_IIT_PRICE_KINDS].AsInt;
		m_eSaleItemKinds = (ESaleItemKinds)a_oSaleItemInfo[KDefine.G_KEY_SALE_IIT_SALE_ITEM_KINDS].AsInt;

		m_oItemInfoList = new List<STItemInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_SALE_ITEM_INFOS; ++i) {
			string oNumItemsKey = string.Format(KDefine.G_KEY_FMT_SALE_IIT_NUM_ITEMS, i + KCDefine.B_VAL_1_INT);
			string oItemKindsKey = string.Format(KDefine.G_KEY_FMT_SALE_IIT_ITEM_KINDS, i + KCDefine.B_VAL_1_INT);

			var stItemInfo = new STItemInfo() {
				m_nNumItems = a_oSaleItemInfo[oNumItemsKey].AsInt,
				m_eItemKinds = (EItemKinds)a_oSaleItemInfo[oItemKindsKey].AsInt
			};

			m_oItemInfoList.Add(stItemInfo);
		}
	}
	#endregion			// 함수
}

//! 판매 아이템 정보 테이블
public class CSaleItemInfoTable : CScriptableObj<CSaleItemInfoTable> {
	#region 변수
	[Header("Sale Item Info")]
	[SerializeField] private List<STSaleItemInfo> m_oSaleItemInfoList = new List<STSaleItemInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<ESaleItemKinds, STSaleItemInfo> SaleItemInfoDict { get; private set; } = new Dictionary<ESaleItemKinds, STSaleItemInfo>();
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		for(int i = 0; i < m_oSaleItemInfoList.Count; ++i) {
			this.SaleItemInfoDict.Add(m_oSaleItemInfoList[i].m_eSaleItemKinds, m_oSaleItemInfoList[i]);
		}
	}

	//! 아이템 정보 포함 여부를 검사한다
	public bool IsContainsItemInfo(ESaleItemKinds a_eSaleItemKinds, EItemKinds a_eItemKinds) {
		return this.TryGetItemInfo(a_eSaleItemKinds, a_eItemKinds, out STItemInfo stItemInfo);
	}

	//! 판매 아이템 정보를 반환한다
	public STSaleItemInfo GetSaleItemInfo(ESaleItemKinds a_eSaleItemKinds) {
		bool bIsValid = this.TryGetSaleItemInfo(a_eSaleItemKinds, out STSaleItemInfo stSaleItemInfo);
		CAccess.Assert(bIsValid);

		return stSaleItemInfo;
	}

	//! 아이템 정보를 반환한다
	public STItemInfo GetItemInfo(ESaleItemKinds a_eSaleItemKinds, EItemKinds a_eItemKinds) {
		bool bIsValid = this.TryGetItemInfo(a_eSaleItemKinds, a_eItemKinds, out STItemInfo stItemInfo);
		CAccess.Assert(bIsValid);

		return stItemInfo;
	}
	
	//! 판매 아이템 정보를 반환한다
	public bool TryGetSaleItemInfo(ESaleItemKinds a_eSaleItemKinds, out STSaleItemInfo a_stOutSaleItemInfo) {
		a_stOutSaleItemInfo = this.SaleItemInfoDict.ExGetVal(a_eSaleItemKinds, KDefine.G_INVALID_SALE_ITEM_INFO);
		return this.SaleItemInfoDict.ContainsKey(a_eSaleItemKinds);
	}

	//! 아이템 정보를 반환한다
	public bool TryGetItemInfo(ESaleItemKinds a_eSaleItemKinds, EItemKinds a_eItemKinds, out STItemInfo a_stOutItemInfo) {
		// 판매 아이템 정보가 존재 할 경우
		if(this.TryGetSaleItemInfo(a_eSaleItemKinds, out STSaleItemInfo stSaleItemInfo)) {
			int nIdx = stSaleItemInfo.m_oItemInfoList.ExFindVal((a_stItemInfo) => a_stItemInfo.m_eItemKinds == a_eItemKinds);
			a_stOutItemInfo = stSaleItemInfo.m_oItemInfoList.ExIsValidIdx(nIdx) ? stSaleItemInfo.m_oItemInfoList[nIdx] : KDefine.G_INVALID_ITEM_INFO;

			return stSaleItemInfo.m_oItemInfoList.ExIsValidIdx(nIdx);
		}

		a_stOutItemInfo = KDefine.G_INVALID_ITEM_INFO;
		return false;
	}

	//! 판매 아이템 정보를 로드한다
	public Dictionary<ESaleItemKinds, STSaleItemInfo> LoadSaleItemInfos() {
#if UNITY_EDITOR || UNITY_STANDALONE
		return this.LoadSaleItemInfos(KDefine.G_RUNTIME_TABLE_P_SALE_ITEM_INFO);
#else
		return this.LoadSaleItemInfos(KCDefine.U_TABLE_P_G_SALE_ITEM_INFO);
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	}

	//! 판매 아이템 정보를 로드한다
	public Dictionary<ESaleItemKinds, STSaleItemInfo> LoadSaleItemInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if UNITY_EDITOR || UNITY_STANDALONE
		string oJSONStr = CFunc.ReadStr(a_oFilePath);
		return this.DoLoadSaleItemInfos(oJSONStr);
#else
		try {
			var oTextAsset = CResManager.Inst.GetRes<TextAsset>(a_oFilePath);
			return this.DoLoadSaleItemInfos(oTextAsset.text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	}

	//! 판매 아이템 정보를 로드한다
	private Dictionary<ESaleItemKinds, STSaleItemInfo> DoLoadSaleItemInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());

		var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr) as SimpleJSON.JSONClass;
		var oSaleItemInfos = oJSONNode[KCDefine.B_KEY_JSON_COMMON_DATA];

		for(int i = 0; i < oSaleItemInfos.Count; ++i) {
			var stSaleItemInfo = new STSaleItemInfo(oSaleItemInfos[i]);
			bool bIsReplace = oSaleItemInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT;

			// 판매 아이템 정보가 추가 가능 할 경우
			if(bIsReplace || !this.SaleItemInfoDict.ContainsKey(stSaleItemInfo.m_eSaleItemKinds)) {
				this.SaleItemInfoDict.ExReplaceVal(stSaleItemInfo.m_eSaleItemKinds, stSaleItemInfo);
			}
		}

		return this.SaleItemInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
