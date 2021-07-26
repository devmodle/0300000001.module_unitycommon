using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 판매 상품 정보
[System.Serializable]
public struct STSaleProductInfo {
	public string m_oName;
	public string m_oDesc;

	public EPriceKinds m_ePriceKinds;
	public ESaleProductKinds m_eSaleProductKinds;

	public List<STItemInfo> m_oItemInfoList;

	#region 함수
	//! 생성자
	public STSaleProductInfo(SimpleJSON.JSONNode a_oSaleProductInfo) {
		m_oName = a_oSaleProductInfo[KCDefine.U_KEY_NAME];
		m_oDesc = a_oSaleProductInfo[KCDefine.U_KEY_DESC];

		m_ePriceKinds = (EPriceKinds)a_oSaleProductInfo[KDefine.G_KEY_SALE_PIT_PRICE_KINDS].AsInt;
		m_eSaleProductKinds = (ESaleProductKinds)a_oSaleProductInfo[KDefine.G_KEY_SALE_PIT_SALE_PRODUCT_KINDS].AsInt;
		
		m_oItemInfoList = new List<STItemInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_SALE_ITEM_INFOS; ++i) {
			string oNumItemsKey = string.Format(KDefine.G_KEY_FMT_SALE_PIT_NUM_ITEMS, i + KCDefine.B_VAL_1_INT);
			string oItemKindsKey = string.Format(KDefine.G_KEY_FMT_SALE_PIT_ITEM_KINDS, i + KCDefine.B_VAL_1_INT);

			var stItemInfo = new STItemInfo() {
				m_nNumItems = a_oSaleProductInfo[oNumItemsKey].AsInt,
				m_eItemKinds = (EItemKinds)a_oSaleProductInfo[oItemKindsKey].AsInt
			};

			m_oItemInfoList.Add(stItemInfo);
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
	public Dictionary<ESaleProductKinds, STSaleProductInfo> SaleProductInfoDict { get; private set; } = new Dictionary<ESaleProductKinds, STSaleProductInfo>();
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		for(int i = 0; i < m_oSaleProductInfoList.Count; ++i) {
			this.SaleProductInfoDict.ExAddVal(m_oSaleProductInfoList[i].m_eSaleProductKinds, m_oSaleProductInfoList[i]);
		}
	}

	//! 아이템 정보 포함 여부를 검사한다
	public bool IsContainsItemInfo(ESaleProductKinds a_eSaleProductKinds, EItemKinds a_eItemKinds) {
		return this.TryGetItemInfo(a_eSaleProductKinds, a_eItemKinds, out STItemInfo stItemInfo);
	}
	
	//! 판매 코인 개수를 반환한다
	public int GetNumSaleCoins(ESaleProductKinds a_eSaleProductKinds) {
		bool bIsValid = this.TryGetItemInfo(a_eSaleProductKinds, EItemKinds.GOODS_COIN, out STItemInfo stItemInfo);
		return bIsValid ? stItemInfo.m_nNumItems : KCDefine.B_VAL_0_INT;
	}

	//! 판매 상품 정보를 반환한다
	public STSaleProductInfo GetSaleProductInfo(ESaleProductKinds a_eSaleProductKinds) {
		bool bIsValid = this.TryGetSaleProductInfo(a_eSaleProductKinds, out STSaleProductInfo stSaleProductInfo);
		CAccess.Assert(bIsValid);

		return stSaleProductInfo;
	}

	//! 아이템 정보를 반환한다
	public STItemInfo GetItemInfo(ESaleProductKinds a_eSaleProductKinds, EItemKinds a_eItemKinds) {
		bool bIsValid = this.TryGetItemInfo(a_eSaleProductKinds, a_eItemKinds, out STItemInfo stItemInfo);
		CAccess.Assert(bIsValid);

		return stItemInfo;
	}

	//! 판매 상품 정보를 반환한다
	public bool TryGetSaleProductInfo(ESaleProductKinds a_eSaleProductKinds, out STSaleProductInfo a_stOutSaleProductInfo) {
		a_stOutSaleProductInfo = this.SaleProductInfoDict.ExGetVal(a_eSaleProductKinds, KDefine.G_INVALID_SALE_PRODUCT_INFO);
		return this.SaleProductInfoDict.ContainsKey(a_eSaleProductKinds);
	}

	//! 아이템 정보를 반환한다
	public bool TryGetItemInfo(ESaleProductKinds a_eSaleProductKinds, EItemKinds a_eItemKinds, out STItemInfo a_stOutItemInfo) {
		// 판매 상품 정보가 존재 할 경우
		if(this.TryGetSaleProductInfo(a_eSaleProductKinds, out STSaleProductInfo stSaleProductInfo)) {
			int nIdx = stSaleProductInfo.m_oItemInfoList.ExFindVal((a_stItemInfo) => a_stItemInfo.m_eItemKinds == a_eItemKinds);
			a_stOutItemInfo = stSaleProductInfo.m_oItemInfoList.ExIsValidIdx(nIdx) ? stSaleProductInfo.m_oItemInfoList[nIdx] : KDefine.G_INVALID_ITEM_INFO;

			return stSaleProductInfo.m_oItemInfoList.ExIsValidIdx(nIdx);
		}

		a_stOutItemInfo = KDefine.G_INVALID_ITEM_INFO;
		return false;
	}

	//! 판매 상품 정보를 로드한다
	public Dictionary<ESaleProductKinds, STSaleProductInfo> LoadSaleProductInfos() {
#if UNITY_EDITOR || UNITY_STANDALONE
		return this.LoadSaleProductInfos(KDefine.G_RUNTIME_TABLE_P_SALE_PRODUCT_INFO);
#else
		return this.LoadSaleProductInfos(KCDefine.U_TABLE_P_G_SALE_PRODUCT_INFO);
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	}

	//! 판매 상품 정보를 로드한다
	public Dictionary<ESaleProductKinds, STSaleProductInfo> LoadSaleProductInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if UNITY_EDITOR || UNITY_STANDALONE
		string oJSONStr = CFunc.ReadStr(a_oFilePath);
		return this.DoLoadSaleProductInfos(oJSONStr);
#else
		try {
			var oTextAsset = CResManager.Inst.GetRes<TextAsset>(a_oFilePath);
			return this.DoLoadSaleProductInfos(oTextAsset.text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	}

	//! 판매 상품 정보를 로드한다
	private Dictionary<ESaleProductKinds, STSaleProductInfo> DoLoadSaleProductInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());

		var oJSONNode = SimpleJSON.JSONNode.Parse(a_oJSONStr);
		var oSaleProductInfos = oJSONNode[KCDefine.B_KEY_JSON_COMMON_DATA];

		for(int i = 0; i < oSaleProductInfos.Count; ++i) {
			var stSaleProductInfo = new STSaleProductInfo(oSaleProductInfos[i]);
			bool bIsReplace = oSaleProductInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT;

			// 판매 상품 정보가 추가 가능 할 경우
			if(bIsReplace || !this.SaleProductInfoDict.ContainsKey(stSaleProductInfo.m_eSaleProductKinds)) {
				this.SaleProductInfoDict.ExReplaceVal(stSaleProductInfo.m_eSaleProductKinds, stSaleProductInfo);
			}
		}

		return this.SaleProductInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
