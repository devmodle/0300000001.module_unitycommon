using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 아이템 판매 정보 */
[System.Serializable]
public struct STItemSaleInfo {
	public STDescInfo m_stDescInfo;
	public EItemSaleKinds m_eItemSaleKinds;
	
	public List<STPriceInfo> m_oPriceInfoList;
	public List<STNumItemsInfo> m_oNumItemsInfoList;

	#region 프로퍼티
	public EItemSaleType ItemSaleType => (EItemSaleType)((int)m_eItemSaleKinds).ExKindsToType();
	public EItemSaleKinds BaseItemSaleKinds => (EItemSaleKinds)((int)m_eItemSaleKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STItemSaleInfo(SimpleJSON.JSONNode a_oItemSaleInfo) {
		m_stDescInfo = new STDescInfo(a_oItemSaleInfo);
		m_eItemSaleKinds = a_oItemSaleInfo[KCDefine.U_KEY_ITEM_SALE_KINDS].ExIsValid() ? (EItemSaleKinds)a_oItemSaleInfo[KCDefine.U_KEY_ITEM_SALE_KINDS].AsInt : EItemSaleKinds.NONE;

		m_oPriceInfoList = new List<STPriceInfo>();
		m_oNumItemsInfoList = new List<STNumItemsInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_PRICE_INFOS; ++i) {
			string oPriceKey = string.Format(KCDefine.U_KEY_FMT_PRICE, i + KCDefine.B_VAL_1_INT);
			string oPriceKindsKey = string.Format(KCDefine.U_KEY_FMT_PRICE_KINDS, i + KCDefine.B_VAL_1_INT);

			m_oPriceInfoList.Add(new STPriceInfo() {
				m_oPrice = a_oItemSaleInfo[oPriceKey].ExIsValid() ? a_oItemSaleInfo[oPriceKey] : KCDefine.B_STR_0_INT, m_ePriceKinds = a_oItemSaleInfo[oPriceKindsKey].ExIsValid() ? (EPriceKinds)a_oItemSaleInfo[oPriceKindsKey].AsInt : EPriceKinds.NONE
			});
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_ITEMS_INFOS; ++i) {
			string oNumItemsKey = string.Format(KCDefine.U_KEY_FMT_NUM_ITEMS, i + KCDefine.B_VAL_1_INT);
			string oItemKindsKey = string.Format(KCDefine.U_KEY_FMT_ITEM_KINDS, i + KCDefine.B_VAL_1_INT);

			m_oNumItemsInfoList.Add(new STNumItemsInfo() {
				m_nNumItems = long.TryParse(a_oItemSaleInfo[oNumItemsKey], out long nNumItems) ? nNumItems : KCDefine.B_VAL_0_LONG, m_eItemKinds = a_oItemSaleInfo[oItemKindsKey].ExIsValid() ? (EItemKinds)a_oItemSaleInfo[oItemKindsKey].AsInt : EItemKinds.NONE
			});
		}
	}
	#endregion			// 함수
}

/** 아이템 판매 정보 테이블 */
public partial class CItemSaleInfoTable : CScriptableObj<CItemSaleInfoTable> {
	#region 변수
	[Header("=====> Goods Item Sale Info <=====")]
	[SerializeField] private List<STItemSaleInfo> m_oGoodsItemSaleInfoList = new List<STItemSaleInfo>();

	[Header("=====> Consumable Item Sale Info <=====")]
	[SerializeField] private List<STItemSaleInfo> m_oConsumableItemSaleInfoList = new List<STItemSaleInfo>();

	[Header("=====> Non Consumable Item Sale Info <=====")]
	[SerializeField] private List<STItemSaleInfo> m_oNonConsumableItemSaleInfoList = new List<STItemSaleInfo>();

	[Header("=====> Weapon Item Sale Info <=====")]
	[SerializeField] private List<STItemSaleInfo> m_oWeaponItemSaleInfoList = new List<STItemSaleInfo>();

	[Header("=====> Armor Item Sale Info <=====")]
	[SerializeField] private List<STItemSaleInfo> m_oArmorItemSaleInfoList = new List<STItemSaleInfo>();

	[Header("=====> Accessory Item Sale Info <=====")]
	[SerializeField] private List<STItemSaleInfo> m_oAccessoryItemSaleInfoList = new List<STItemSaleInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<EItemSaleKinds, STItemSaleInfo> ItemSaleInfoDict { get; private set; } = new Dictionary<EItemSaleKinds, STItemSaleInfo>();

	private string ItemSaleInfoTablePath {
		get {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_ITEM_SALE_INFO;
#else
			return KCDefine.U_TABLE_P_G_ITEM_SALE_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		var oItemSaleInfoList = new List<STItemSaleInfo>(m_oGoodsItemSaleInfoList);
		oItemSaleInfoList.ExAddVals(m_oConsumableItemSaleInfoList);
		oItemSaleInfoList.ExAddVals(m_oNonConsumableItemSaleInfoList);
		oItemSaleInfoList.ExAddVals(m_oWeaponItemSaleInfoList);
		oItemSaleInfoList.ExAddVals(m_oArmorItemSaleInfoList);
		oItemSaleInfoList.ExAddVals(m_oAccessoryItemSaleInfoList);

		for(int i = 0; i < oItemSaleInfoList.Count; ++i) {
			this.ItemSaleInfoDict.TryAdd(oItemSaleInfoList[i].m_eItemSaleKinds, oItemSaleInfoList[i]);
		}
	}

	/** 아이템 판매 정보를 반환한다 */
	public STItemSaleInfo GetItemSaleInfo(EItemSaleKinds a_eItemSaleKinds) {
		bool bIsValid = this.TryGetItemSaleInfo(a_eItemSaleKinds, out STItemSaleInfo stItemSaleInfo);
		CAccess.Assert(bIsValid);

		return stItemSaleInfo;
	}

	/** 가격 정보를 반환한다 */
	public STPriceInfo GetPriceInfo(EItemSaleKinds a_eItemSaleKinds, EPriceKinds a_ePriceKinds) {
		bool bIsValid = this.TryGetPriceInfo(a_eItemSaleKinds, a_ePriceKinds, out STPriceInfo stPriceInfo);
		CAccess.Assert(bIsValid);

		return stPriceInfo;
	}

	/** 아이템 개수 정보를 반환한다 */
	public STNumItemsInfo GetNumItemsInfo(EItemSaleKinds a_eItemSaleKinds, EItemKinds a_eItemKinds) {
		bool bIsValid = this.TryGetNumItemsInfo(a_eItemSaleKinds, a_eItemKinds, out STNumItemsInfo stNumItemsInfo);
		CAccess.Assert(bIsValid);

		return stNumItemsInfo;
	}
	
	/** 아이템 판매 정보를 반환한다 */
	public bool TryGetItemSaleInfo(EItemSaleKinds a_eItemSaleKinds, out STItemSaleInfo a_stOutItemSaleInfo) {
		a_stOutItemSaleInfo = this.ItemSaleInfoDict.GetValueOrDefault(a_eItemSaleKinds, default(STItemSaleInfo));
		return this.ItemSaleInfoDict.ContainsKey(a_eItemSaleKinds);
	}

	/** 가격 정보를 반환한다 */
	public bool TryGetPriceInfo(EItemSaleKinds a_eItemSaleKinds, EPriceKinds a_ePriceKinds, out STPriceInfo a_stOutPriceInfo) {
		// 아이템 판매 정보가 존재 할 경우
		if(this.TryGetItemSaleInfo(a_eItemSaleKinds, out STItemSaleInfo stItemSaleInfo)) {
			return stItemSaleInfo.m_oPriceInfoList.ExTryGetPriceInfo(a_ePriceKinds, out a_stOutPriceInfo);
		}

		a_stOutPriceInfo = default(STPriceInfo);
		return false;
	}

	/** 아이템 개수 정보를 반환한다 */
	public bool TryGetNumItemsInfo(EItemSaleKinds a_eItemSaleKinds, EItemKinds a_eItemKinds, out STNumItemsInfo a_stOutNumItemsInfo) {
		// 아이템 판매 정보가 존재 할 경우
		if(this.TryGetItemSaleInfo(a_eItemSaleKinds, out STItemSaleInfo stItemSaleInfo)) {
			return stItemSaleInfo.m_oNumItemsInfoList.ExTryGetNumItemsInfo(a_eItemKinds, out a_stOutNumItemsInfo);
		}

		a_stOutNumItemsInfo = default(STNumItemsInfo);
		return false;
	}

	/** 아이템 판매 정보를 로드한다 */
	public Dictionary<EItemSaleKinds, STItemSaleInfo> LoadItemSaleInfos() {
		return this.LoadItemSaleInfos(this.ItemSaleInfoTablePath);
	}

	/** 아이템 판매 정보를 로드한다 */
	private Dictionary<EItemSaleKinds, STItemSaleInfo> LoadItemSaleInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadItemSaleInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			var oTextAsset = CResManager.Inst.GetRes<TextAsset>(a_oFilePath);
			return this.DoLoadItemSaleInfos(oTextAsset.text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 아이템 판매 정보를 로드한다 */
	private Dictionary<EItemSaleKinds, STItemSaleInfo> DoLoadItemSaleInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr) as SimpleJSON.JSONClass;

		var oItemSaleInfosList = new List<SimpleJSON.JSONNode>() {
			oJSONNode[KCDefine.U_KEY_GOODS], oJSONNode[KCDefine.U_KEY_CONSUMABLE], oJSONNode[KCDefine.U_KEY_NON_CONSUMABLE], oJSONNode[KCDefine.U_KEY_WEAPON], oJSONNode[KCDefine.U_KEY_ARMOR], oJSONNode[KCDefine.U_KEY_ACCESSORY]
		};

		for(int i = 0; i < oItemSaleInfosList.Count; ++i) {
			for(int j = 0; j < oItemSaleInfosList[i].Count; ++j) {
				var stItemSaleInfo = new STItemSaleInfo(oItemSaleInfosList[i][j]);

				// 아이템 판매 정보가 추가 가능 할 경우
				if(!this.ItemSaleInfoDict.ContainsKey(stItemSaleInfo.m_eItemSaleKinds) || oItemSaleInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT) {
					this.ItemSaleInfoDict.ExReplaceVal(stItemSaleInfo.m_eItemSaleKinds, stItemSaleInfo);
				}
			}
		}

		return this.ItemSaleInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
