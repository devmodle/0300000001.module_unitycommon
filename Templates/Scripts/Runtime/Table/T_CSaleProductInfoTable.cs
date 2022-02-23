using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if RUNTIME_TEMPLATES_MODULE_ENABLE
#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif			// #if PURCHASE_MODULE_ENABLE

/** 판매 상품 정보 */
[System.Serializable]
public struct STSaleProductInfo {
	public string m_oName;
	public string m_oDesc;
	public string m_oPrice;

	public EPriceKinds m_ePriceKinds;
	public ESaleProductKinds m_eSaleProductKinds;

	public List<STItemInfo> m_oItemInfoList;

#if PURCHASE_MODULE_ENABLE
	public ProductType m_eProductType;
#endif			// #if PURCHASE_MODULE_ENABLE

	#region 프로퍼티
	public long IntPrice => long.TryParse(m_oPrice, out long nPrice) ? nPrice : KCDefine.B_VAL_0_INT;
	public double RealPrice => double.TryParse(m_oPrice, out double dblPrice) ? dblPrice : KCDefine.B_VAL_0_DBL;
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STSaleProductInfo(SimpleJSON.JSONNode a_oSaleProductInfo) {
		m_oName = a_oSaleProductInfo[KCDefine.U_KEY_NAME];
		m_oDesc = a_oSaleProductInfo[KCDefine.U_KEY_DESC];
		m_oPrice = a_oSaleProductInfo[KCDefine.U_KEY_PRICE];

		m_ePriceKinds = (EPriceKinds)a_oSaleProductInfo[KCDefine.U_KEY_PRICE_KINDS].AsInt;
		m_eSaleProductKinds = (ESaleProductKinds)a_oSaleProductInfo[KCDefine.U_KEY_SALE_PRODUCT_KINDS].AsInt;
		
		m_oItemInfoList = new List<STItemInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_SALE_ITEM_INFOS; ++i) {
			string oNumItemsKey = string.Format(KCDefine.U_KEY_FMT_NUM_ITEMS, i + KCDefine.B_VAL_1_INT);
			string oItemKindsKey = string.Format(KCDefine.U_KEY_FMT_ITEM_KINDS, i + KCDefine.B_VAL_1_INT);

			// 아이템 정보가 존재 할 경우
			if(a_oSaleProductInfo[oNumItemsKey] != null && a_oSaleProductInfo[oItemKindsKey] != null) {
				m_oItemInfoList.Add(new STItemInfo() {
					m_nNumItems = long.Parse(a_oSaleProductInfo[oNumItemsKey]), m_eItemKinds = (EItemKinds)a_oSaleProductInfo[oItemKindsKey].AsInt
				});
			}
		}

#if PURCHASE_MODULE_ENABLE
		m_eProductType = (ProductType)a_oSaleProductInfo[KCDefine.U_KEY_PRODUCT_KINDS].AsInt;
		CAccess.Assert(m_eProductType != ProductType.Subscription);
#endif			// #if PURCHASE_MODULE_ENABLE
	}
	#endregion			// 함수
}

/** 판매 상품 정보 테이블 */
public class CSaleProductInfoTable : CScriptableObj<CSaleProductInfoTable> {
	#region 변수
	[Header("=====> Sale Product Info <=====")]
	[SerializeField] private List<STSaleProductInfo> m_oSaleProductInfoList = new List<STSaleProductInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<ESaleProductKinds, STSaleProductInfo> SaleProductInfoDict { get; private set; } = new Dictionary<ESaleProductKinds, STSaleProductInfo>();

	private string SaleProductInfoTablePath {
		get {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_SALE_PRODUCT_INFO;
#else
			return KCDefine.U_TABLE_P_G_SALE_PRODUCT_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		}
	}
	#endregion			// 프로퍼티

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		for(int i = 0; i < m_oSaleProductInfoList.Count; ++i) {
			this.SaleProductInfoDict.TryAdd(m_oSaleProductInfoList[i].m_eSaleProductKinds, m_oSaleProductInfoList[i]);
		}
	}

	/** 아이템 정보 포함 여부를 검사한다 */
	public bool IsContainsItemInfo(ESaleProductKinds a_eSaleProductKinds, EItemKinds a_eItemKinds) {
		return this.TryGetItemInfo(a_eSaleProductKinds, a_eItemKinds, out STItemInfo stItemInfo);
	}
	
	/** 판매 상품 정보를 반환한다 */
	public STSaleProductInfo GetSaleProductInfo(ESaleProductKinds a_eSaleProductKinds) {
		bool bIsValid = this.TryGetSaleProductInfo(a_eSaleProductKinds, out STSaleProductInfo stSaleProductInfo);
		CAccess.Assert(bIsValid);

		return stSaleProductInfo;
	}

	/** 아이템 정보를 반환한다 */
	public STItemInfo GetItemInfo(ESaleProductKinds a_eSaleProductKinds, EItemKinds a_eItemKinds) {
		bool bIsValid = this.TryGetItemInfo(a_eSaleProductKinds, a_eItemKinds, out STItemInfo stItemInfo);
		CAccess.Assert(bIsValid);

		return stItemInfo;
	}

	/** 판매 상품 정보를 반환한다 */
	public bool TryGetSaleProductInfo(ESaleProductKinds a_eSaleProductKinds, out STSaleProductInfo a_stOutSaleProductInfo) {
		a_stOutSaleProductInfo = this.SaleProductInfoDict.GetValueOrDefault(a_eSaleProductKinds, KDefine.G_INVALID_SALE_PRODUCT_INFO);
		return this.SaleProductInfoDict.ContainsKey(a_eSaleProductKinds);
	}

	/** 아이템 정보를 반환한다 */
	public bool TryGetItemInfo(ESaleProductKinds a_eSaleProductKinds, EItemKinds a_eItemKinds, out STItemInfo a_stOutItemInfo) {
		// 판매 상품 정보가 존재 할 경우
		if(this.TryGetSaleProductInfo(a_eSaleProductKinds, out STSaleProductInfo stSaleProductInfo)) {
			int nIdx = stSaleProductInfo.m_oItemInfoList.FindIndex((a_stItemInfo) => a_stItemInfo.m_eItemKinds == a_eItemKinds);
			a_stOutItemInfo = stSaleProductInfo.m_oItemInfoList.ExIsValidIdx(nIdx) ? stSaleProductInfo.m_oItemInfoList[nIdx] : KDefine.G_INVALID_ITEM_INFO;

			return stSaleProductInfo.m_oItemInfoList.ExIsValidIdx(nIdx);
		}

		a_stOutItemInfo = KDefine.G_INVALID_ITEM_INFO;
		return false;
	}

	/** 판매 상품 정보를 로드한다 */
	public Dictionary<ESaleProductKinds, STSaleProductInfo> LoadSaleProductInfos() {
		return this.LoadSaleProductInfos(this.SaleProductInfoTablePath);
	}

	/** 판매 상품 정보를 로드한다 */
	private Dictionary<ESaleProductKinds, STSaleProductInfo> LoadSaleProductInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadSaleProductInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			var oTextAsset = CResManager.Inst.GetRes<TextAsset>(a_oFilePath);
			return this.DoLoadSaleProductInfos(oTextAsset.text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 판매 상품 정보를 로드한다 */
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

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
