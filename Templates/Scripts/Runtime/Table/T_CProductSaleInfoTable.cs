using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif			// #if PURCHASE_MODULE_ENABLE

/** 상품 판매 정보 */
[System.Serializable]
public struct STProductSaleInfo {
	public STDescInfo m_stDescInfo;

	public int m_nID;
	public EProductSaleKinds m_eProductSaleKinds;

	public List<STPriceInfo> m_oPriceInfoList;
	public List<STNumItemsInfo> m_oNumItemsInfoList;

	#region 프로퍼티
	public EProductSaleType ProductSaleType => (EProductSaleType)((int)m_eProductSaleKinds).ExKindsToType();
	public EProductSaleKinds BaseProductSaleKinds => (EProductSaleKinds)((int)m_eProductSaleKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STProductSaleInfo(SimpleJSON.JSONNode a_oProductSaleInfo) {
		m_stDescInfo = new STDescInfo(a_oProductSaleInfo);

		m_nID = a_oProductSaleInfo[KCDefine.U_KEY_ID].AsInt;
		m_eProductSaleKinds = a_oProductSaleInfo[KCDefine.U_KEY_PRODUCT_SALE_KINDS].ExIsValid() ? (EProductSaleKinds)a_oProductSaleInfo[KCDefine.U_KEY_PRODUCT_SALE_KINDS].AsInt : EProductSaleKinds.NONE;

		m_oPriceInfoList = new List<STPriceInfo>();
		m_oNumItemsInfoList = new List<STNumItemsInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_PRICE_INFOS; ++i) {
			string oPriceKey = string.Format(KCDefine.U_KEY_FMT_PRICE, i + KCDefine.B_VAL_1_INT);
			string oPriceKindsKey = string.Format(KCDefine.U_KEY_FMT_PRICE_KINDS, i + KCDefine.B_VAL_1_INT);

			m_oPriceInfoList.Add(new STPriceInfo() {
				m_oPrice = a_oProductSaleInfo[oPriceKey].ExIsValid() ? a_oProductSaleInfo[oPriceKey] : KCDefine.B_STR_0_INT, m_ePriceKinds = a_oProductSaleInfo[oPriceKindsKey].ExIsValid() ? (EPriceKinds)a_oProductSaleInfo[oPriceKindsKey].AsInt : EPriceKinds.NONE
			});
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_ITEMS_INFOS; ++i) {
			string oNumItemsKey = string.Format(KCDefine.U_KEY_FMT_NUM_ITEMS, i + KCDefine.B_VAL_1_INT);
			string oItemKindsKey = string.Format(KCDefine.U_KEY_FMT_ITEM_KINDS, i + KCDefine.B_VAL_1_INT);

			m_oNumItemsInfoList.Add(new STNumItemsInfo() {
				m_nNumItems = long.TryParse(a_oProductSaleInfo[oNumItemsKey], out long nNumItems) ? nNumItems : KCDefine.B_VAL_0_LONG, m_eItemKinds = a_oProductSaleInfo[oItemKindsKey].ExIsValid() ? (EItemKinds)a_oProductSaleInfo[oItemKindsKey].AsInt : EItemKinds.NONE
			});
		}
	}
	#endregion			// 함수
}

/** 상품 판매 정보 테이블 */
public partial class CProductSaleInfoTable : CScriptableObj<CProductSaleInfoTable> {
	#region 변수
	[Header("=====> Pkgs Product Sale Info <=====")]
	[SerializeField] private List<STProductSaleInfo> m_oPkgsProductSaleInfoList = new List<STProductSaleInfo>();

	[Header("=====> Single Product Sale Info <=====")]
	[SerializeField] private List<STProductSaleInfo> m_oSingleProductSaleInfoList = new List<STProductSaleInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<EProductSaleKinds, STProductSaleInfo> ProductSaleInfoDict { get; private set; } = new Dictionary<EProductSaleKinds, STProductSaleInfo>();

	private string ProductSaleInfoTablePath {
		get {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_PRODUCT_SALE_INFO;
#else
			return KCDefine.U_TABLE_P_G_PRODUCT_SALE_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		var oProductSaleInfoList = new List<STProductSaleInfo>(m_oPkgsProductSaleInfoList);
		oProductSaleInfoList.ExAddVals(m_oSingleProductSaleInfoList);

		for(int i = 0; i < oProductSaleInfoList.Count; ++i) {
			this.ProductSaleInfoDict.TryAdd(oProductSaleInfoList[i].m_eProductSaleKinds, oProductSaleInfoList[i]);
		}
	}

	/** 상품 판매 정보를 반환한다 */
	public STProductSaleInfo GetProductSaleInfo(int a_nID) {
		bool bIsValid = this.TryGetProductSaleInfo(a_nID, out STProductSaleInfo stProductSaleInfo);
		CAccess.Assert(bIsValid);

		return stProductSaleInfo;
	}
	
	/** 상품 판매 정보를 반환한다 */
	public STProductSaleInfo GetProductSaleInfo(EProductSaleKinds a_eProductSaleKinds) {
		bool bIsValid = this.TryGetProductSaleInfo(a_eProductSaleKinds, out STProductSaleInfo stProductSaleInfo);
		CAccess.Assert(bIsValid);

		return stProductSaleInfo;
	}

	/** 가격 정보를 반환한다 */
	public STPriceInfo GetPriceInfo(EProductSaleKinds a_eProductSaleKinds, EPriceKinds a_ePriceKinds) {
		bool bIsValid = this.TryGetPriceInfo(a_eProductSaleKinds, a_ePriceKinds, out STPriceInfo stPriceInfo);
		CAccess.Assert(bIsValid);

		return stPriceInfo;
	}

	/** 아이템 개수 정보를 반환한다 */
	public STNumItemsInfo GetNumItemsInfo(EProductSaleKinds a_eProductSaleKinds, EItemKinds a_eItemKinds) {
		bool bIsValid = this.TryGetNumItemsInfo(a_eProductSaleKinds, a_eItemKinds, out STNumItemsInfo stNumItemsInfo);
		CAccess.Assert(bIsValid);

		return stNumItemsInfo;
	}

	/** 상품 판매 정보를 반환한다 */
	public bool TryGetProductSaleInfo(int a_nID, out STProductSaleInfo a_stOutProductSaleInfo) {
		var stResult = this.ProductSaleInfoDict.ExFindVal((a_stProductSaleInfo) => a_stProductSaleInfo.m_nID == a_nID);
		a_stOutProductSaleInfo = stResult.Item1 ? this.ProductSaleInfoDict[stResult.Item2] : default(STProductSaleInfo);

		return stResult.Item1;
	}

	/** 상품 판매 정보를 반환한다 */
	public bool TryGetProductSaleInfo(EProductSaleKinds a_eProductSaleKinds, out STProductSaleInfo a_stOutProductSaleInfo) {
		a_stOutProductSaleInfo = this.ProductSaleInfoDict.GetValueOrDefault(a_eProductSaleKinds, default(STProductSaleInfo));
		return this.ProductSaleInfoDict.ContainsKey(a_eProductSaleKinds);
	}

	/** 가격 정보를 반환한다 */
	public bool TryGetPriceInfo(EProductSaleKinds a_eProductSaleKinds, EPriceKinds a_ePriceKinds, out STPriceInfo a_stOutPriceInfo) {
		// 아이템 판매 정보가 존재 할 경우
		if(this.TryGetProductSaleInfo(a_eProductSaleKinds, out STProductSaleInfo stProductSaleInfo)) {
			return stProductSaleInfo.m_oPriceInfoList.ExTryGetPriceInfo(a_ePriceKinds, out a_stOutPriceInfo);
		}

		a_stOutPriceInfo = default(STPriceInfo);
		return false;
	}

	/** 아이템 개수 정보를 반환한다 */
	public bool TryGetNumItemsInfo(EProductSaleKinds a_eProductSaleKinds, EItemKinds a_eItemKinds, out STNumItemsInfo a_stOutNumItemsInfo) {
		// 상품 판매 정보가 존재 할 경우
		if(this.TryGetProductSaleInfo(a_eProductSaleKinds, out STProductSaleInfo stProductSaleInfo)) {
			return stProductSaleInfo.m_oNumItemsInfoList.ExTryGetNumItemsInfo(a_eItemKinds, out a_stOutNumItemsInfo);
		}

		a_stOutNumItemsInfo = default(STNumItemsInfo);
		return false;
	}

	/** 상품 판매 정보를 로드한다 */
	public Dictionary<EProductSaleKinds, STProductSaleInfo> LoadProductSaleInfos() {
		return this.LoadProductSaleInfos(this.ProductSaleInfoTablePath);
	}

	/** 상품 판매 정보를 로드한다 */
	private Dictionary<EProductSaleKinds, STProductSaleInfo> LoadProductSaleInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadProductSaleInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			var oTextAsset = CResManager.Inst.GetRes<TextAsset>(a_oFilePath);
			return this.DoLoadProductSaleInfos(oTextAsset.text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 상품 판매 정보를 로드한다 */
	private Dictionary<EProductSaleKinds, STProductSaleInfo> DoLoadProductSaleInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		var oJSONNode = SimpleJSON.JSONNode.Parse(a_oJSONStr);

		var oProductSaleInfosList = new List<SimpleJSON.JSONNode>() {
			oJSONNode[KCDefine.U_KEY_PKGS], oJSONNode[KCDefine.U_KEY_SINGLE]
		};

		for(int i = 0; i < oProductSaleInfosList.Count; ++i) {
			for(int j = 0; j < oProductSaleInfosList[i].Count; ++j) {
				var stProductSaleInfo = new STProductSaleInfo(oProductSaleInfosList[i][j]);

				// 상품 판매 정보가 추가 가능 할 경우
				if(!this.ProductSaleInfoDict.ContainsKey(stProductSaleInfo.m_eProductSaleKinds) || oProductSaleInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT) {
					this.ProductSaleInfoDict.ExReplaceVal(stProductSaleInfo.m_eProductSaleKinds, stProductSaleInfo);
				}
			}
		}

		return this.ProductSaleInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
