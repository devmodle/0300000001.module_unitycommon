using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 아이템 정보 */
[System.Serializable]
public partial struct STItemInfo {
	public STCommonInfo m_stCommonInfo;

	public EItemKinds m_eItemKinds;
	public EItemKinds m_ePrevItemKinds;
	public EItemKinds m_eNextItemKinds;

	public Dictionary<ulong, STTargetInfo> m_oAttachItemTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oSkillTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAbilityTargetInfoDict;

	#region 상수
	public static STItemInfo INVALID = new STItemInfo() {
		m_eItemKinds = EItemKinds.NONE, m_ePrevItemKinds = EItemKinds.NONE, m_eNextItemKinds = EItemKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public EItemType ItemType => (EItemType)((int)m_eItemKinds).ExKindsToType();
	public EItemKinds BaseItemKinds => (EItemKinds)((int)m_eItemKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STItemInfo(SimpleJSON.JSONNode a_oItemInfo) {
		m_stCommonInfo = new STCommonInfo(a_oItemInfo);
		
		m_eItemKinds = a_oItemInfo[KCDefine.U_KEY_ITEM_KINDS].ExIsValid() ? (EItemKinds)a_oItemInfo[KCDefine.U_KEY_ITEM_KINDS].AsInt : EItemKinds.NONE;
		m_ePrevItemKinds = a_oItemInfo[KCDefine.U_KEY_PREV_ITEM_KINDS].ExIsValid() ? (EItemKinds)a_oItemInfo[KCDefine.U_KEY_PREV_ITEM_KINDS].AsInt : EItemKinds.NONE;
		m_eNextItemKinds = a_oItemInfo[KCDefine.U_KEY_NEXT_ITEM_KINDS].ExIsValid() ? (EItemKinds)a_oItemInfo[KCDefine.U_KEY_NEXT_ITEM_KINDS].AsInt : EItemKinds.NONE;

		m_oAttachItemTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oSkillTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oAbilityTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oItemInfo[string.Format(KCDefine.U_KEY_FMT_ATTACH_ITEM_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oAttachItemTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oItemInfo[string.Format(KCDefine.U_KEY_FMT_SKILL_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oSkillTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oItemInfo[string.Format(KCDefine.U_KEY_FMT_ABILITY_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oAbilityTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}
	}
	#endregion			// 함수
}

/** 아이템 판매 정보 */
[System.Serializable]
public partial struct STItemSaleInfo {
	public STCommonInfo m_stCommonInfo;

	public EItemKinds m_eItemKinds;
	public EItemKinds m_ePrevItemKinds;
	public EItemKinds m_eNextItemKinds;
	
	public Dictionary<ulong, STTargetInfo> m_oPayTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAcquireTargetInfoDict;

	#region 상수
	public static STItemSaleInfo INVALID = new STItemSaleInfo() {
		m_eItemKinds = EItemKinds.NONE, m_ePrevItemKinds = EItemKinds.NONE, m_eNextItemKinds = EItemKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public EItemType ItemType => (EItemType)((int)m_eItemKinds).ExKindsToType();
	public EItemKinds BaseItemKinds => (EItemKinds)((int)m_eItemKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STItemSaleInfo(SimpleJSON.JSONNode a_oItemSaleInfo) {
		m_stCommonInfo = new STCommonInfo(a_oItemSaleInfo);

		m_eItemKinds = a_oItemSaleInfo[KCDefine.U_KEY_ITEM_KINDS].ExIsValid() ? (EItemKinds)a_oItemSaleInfo[KCDefine.U_KEY_ITEM_KINDS].AsInt : EItemKinds.NONE;
		m_ePrevItemKinds = a_oItemSaleInfo[KCDefine.U_KEY_PREV_ITEM_KINDS].ExIsValid() ? (EItemKinds)a_oItemSaleInfo[KCDefine.U_KEY_PREV_ITEM_KINDS].AsInt : EItemKinds.NONE;
		m_eNextItemKinds = a_oItemSaleInfo[KCDefine.U_KEY_NEXT_ITEM_KINDS].ExIsValid() ? (EItemKinds)a_oItemSaleInfo[KCDefine.U_KEY_NEXT_ITEM_KINDS].AsInt : EItemKinds.NONE;

		m_oPayTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oAcquireTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oItemSaleInfo[string.Format(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oPayTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oItemSaleInfo[string.Format(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oAcquireTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}
	}
	#endregion			// 함수
}

/** 아이템 강화 정보 */
[System.Serializable]
public partial struct STItemEnhanceInfo {
	public STCommonInfo m_stCommonInfo;

	public EItemKinds m_eItemKinds;
	public EItemKinds m_ePrevItemKinds;
	public EItemKinds m_eNextItemKinds;
	
	public Dictionary<ulong, STTargetInfo> m_oPayTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAcquireTargetInfoDict;

	#region 상수
	public static STItemEnhanceInfo INVALID = new STItemEnhanceInfo() {
		m_eItemKinds = EItemKinds.NONE, m_ePrevItemKinds = EItemKinds.NONE, m_eNextItemKinds = EItemKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public EItemType ItemType => (EItemType)((int)m_eItemKinds).ExKindsToType();
	public EItemKinds BaseItemKinds => (EItemKinds)((int)m_eItemKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STItemEnhanceInfo(SimpleJSON.JSONNode a_oItemEnhanceInfo) {
		m_stCommonInfo = new STCommonInfo(a_oItemEnhanceInfo);

		m_eItemKinds = a_oItemEnhanceInfo[KCDefine.U_KEY_ITEM_KINDS].ExIsValid() ? (EItemKinds)a_oItemEnhanceInfo[KCDefine.U_KEY_ITEM_KINDS].AsInt : EItemKinds.NONE;
		m_ePrevItemKinds = a_oItemEnhanceInfo[KCDefine.U_KEY_PREV_ITEM_KINDS].ExIsValid() ? (EItemKinds)a_oItemEnhanceInfo[KCDefine.U_KEY_PREV_ITEM_KINDS].AsInt : EItemKinds.NONE;
		m_eNextItemKinds = a_oItemEnhanceInfo[KCDefine.U_KEY_NEXT_ITEM_KINDS].ExIsValid() ? (EItemKinds)a_oItemEnhanceInfo[KCDefine.U_KEY_NEXT_ITEM_KINDS].AsInt : EItemKinds.NONE;

		m_oPayTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oAcquireTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oItemEnhanceInfo[string.Format(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oPayTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oItemEnhanceInfo[string.Format(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oAcquireTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}
	}
	#endregion			// 함수
}

/** 아이템 정보 테이블 */
public partial class CItemInfoTable : CSingleton<CItemInfoTable> {
	#region 변수
	[Header("=====> Goods Item Info <=====")]
	[SerializeField] private List<STItemInfo> m_oGoodsItemInfoList = new List<STItemInfo>();
	[SerializeField] private List<STItemSaleInfo> m_oGoodsItemSaleInfoList = new List<STItemSaleInfo>();
	[SerializeField] private List<STItemEnhanceInfo> m_oGoodsItemEnhanceInfoList = new List<STItemEnhanceInfo>();

	[Header("=====> Consumable Item Info <=====")]
	[SerializeField] private List<STItemInfo> m_oConsumableItemInfoList = new List<STItemInfo>();
	[SerializeField] private List<STItemSaleInfo> m_oConsumableItemSaleInfoList = new List<STItemSaleInfo>();
	[SerializeField] private List<STItemEnhanceInfo> m_oConsumableItemEnhanceInfoList = new List<STItemEnhanceInfo>();

	[Header("=====> Non Consumable Item Info <=====")]
	[SerializeField] private List<STItemInfo> m_oNonConsumableItemInfoList = new List<STItemInfo>();
	[SerializeField] private List<STItemSaleInfo> m_oNonConsumableItemSaleInfoList = new List<STItemSaleInfo>();
	[SerializeField] private List<STItemEnhanceInfo> m_oNonConsumableItemEnhanceInfoList = new List<STItemEnhanceInfo>();

	[Header("=====> Weapon Item Info <=====")]
	[SerializeField] private List<STItemInfo> m_oWeaponItemInfoList = new List<STItemInfo>();
	[SerializeField] private List<STItemSaleInfo> m_oWeaponItemSaleInfoList = new List<STItemSaleInfo>();
	[SerializeField] private List<STItemEnhanceInfo> m_oWeaponItemEnhanceInfoList = new List<STItemEnhanceInfo>();

	[Header("=====> Armor Item Info <=====")]
	[SerializeField] private List<STItemInfo> m_oArmorItemInfoList = new List<STItemInfo>();
	[SerializeField] private List<STItemSaleInfo> m_oArmorItemSaleInfoList = new List<STItemSaleInfo>();
	[SerializeField] private List<STItemEnhanceInfo> m_oArmorItemEnhanceInfoList = new List<STItemEnhanceInfo>();

	[Header("=====> Accessory Item Info <=====")]
	[SerializeField] private List<STItemInfo> m_oAccessoryItemInfoList = new List<STItemInfo>();
	[SerializeField] private List<STItemSaleInfo> m_oAccessoryItemSaleInfoList = new List<STItemSaleInfo>();
	[SerializeField] private List<STItemEnhanceInfo> m_oAccessoryItemEnhanceInfoList = new List<STItemEnhanceInfo>();

	[Header("=====> Attach Item Info <=====")]
	[SerializeField] private List<STItemInfo> m_oAttachItemInfoList = new List<STItemInfo>();
	[SerializeField] private List<STItemSaleInfo> m_oAttachItemSaleInfoList = new List<STItemSaleInfo>();
	[SerializeField] private List<STItemEnhanceInfo> m_oAttachItemEnhanceInfoList = new List<STItemEnhanceInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<EItemKinds, STItemInfo> ItemInfoDict { get; private set; } = new Dictionary<EItemKinds, STItemInfo>();
	public Dictionary<EItemKinds, STItemSaleInfo> ItemSaleInfoDict { get; private set; } = new Dictionary<EItemKinds, STItemSaleInfo>();
	public Dictionary<EItemKinds, STItemEnhanceInfo> ItemEnhanceInfoDict { get; private set; } = new Dictionary<EItemKinds, STItemEnhanceInfo>();

	private string ItemInfoTablePath {
		get {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_ITEM_INFO;
#else
			return KCDefine.U_TABLE_P_G_ITEM_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetItemInfos();
	}

	/** 아이템 정보를 리셋한다 */
	public void ResetItemInfos() {
		this.ItemInfoDict.Clear();
		this.ItemSaleInfoDict.Clear();
		this.ItemEnhanceInfoDict.Clear();

		var oItemInfoList = new List<STItemInfo>(m_oGoodsItemInfoList);
		oItemInfoList.ExAddVals(m_oConsumableItemInfoList);
		oItemInfoList.ExAddVals(m_oNonConsumableItemInfoList);
		oItemInfoList.ExAddVals(m_oWeaponItemInfoList);
		oItemInfoList.ExAddVals(m_oArmorItemInfoList);
		oItemInfoList.ExAddVals(m_oAccessoryItemInfoList);
		oItemInfoList.ExAddVals(m_oAttachItemInfoList);

		var oItemSaleInfoList = new List<STItemSaleInfo>(m_oGoodsItemSaleInfoList);
		oItemSaleInfoList.ExAddVals(m_oConsumableItemSaleInfoList);
		oItemSaleInfoList.ExAddVals(m_oNonConsumableItemSaleInfoList);
		oItemSaleInfoList.ExAddVals(m_oWeaponItemSaleInfoList);
		oItemSaleInfoList.ExAddVals(m_oArmorItemSaleInfoList);
		oItemSaleInfoList.ExAddVals(m_oAccessoryItemSaleInfoList);
		oItemSaleInfoList.ExAddVals(m_oAttachItemSaleInfoList);

		var oItemEnhanceInfoList = new List<STItemEnhanceInfo>(m_oGoodsItemEnhanceInfoList);
		oItemEnhanceInfoList.ExAddVals(m_oConsumableItemEnhanceInfoList);
		oItemEnhanceInfoList.ExAddVals(m_oNonConsumableItemEnhanceInfoList);
		oItemEnhanceInfoList.ExAddVals(m_oWeaponItemEnhanceInfoList);
		oItemEnhanceInfoList.ExAddVals(m_oArmorItemEnhanceInfoList);
		oItemEnhanceInfoList.ExAddVals(m_oAccessoryItemEnhanceInfoList);
		oItemEnhanceInfoList.ExAddVals(m_oAttachItemEnhanceInfoList);

		for(int i = 0; i < oItemInfoList.Count; ++i) {
			this.ItemInfoDict.TryAdd(oItemInfoList[i].m_eItemKinds, oItemInfoList[i]);
		}

		for(int i = 0; i < oItemSaleInfoList.Count; ++i) {
			this.ItemSaleInfoDict.TryAdd(oItemSaleInfoList[i].m_eItemKinds, oItemSaleInfoList[i]);
		}

		for(int i = 0; i < oItemEnhanceInfoList.Count; ++i) {
			this.ItemEnhanceInfoDict.TryAdd(oItemEnhanceInfoList[i].m_eItemKinds, oItemEnhanceInfoList[i]);
		}
	}

	/** 아이템 정보를 리셋한다 */
	public void ResetItemInfos(string a_oJSONStr) {
		this.ResetItemInfos();
		this.DoLoadItemInfos(a_oJSONStr);
	}

	/** 아이템 정보를 반환한다 */
	public STItemInfo GetItemInfo(EItemKinds a_EItemKinds) {
		bool bIsValid = this.TryGetItemInfo(a_EItemKinds, out STItemInfo stItemInfo);
		CAccess.Assert(bIsValid);

		return stItemInfo;
	}

	/** 아이템 판매 정보를 반환한다 */
	public STItemSaleInfo GetItemSaleInfo(EItemKinds a_eItemKinds) {
		bool bIsValid = this.TryGetItemSaleInfo(a_eItemKinds, out STItemSaleInfo stItemSaleInfo);
		CAccess.Assert(bIsValid);

		return stItemSaleInfo;
	}

	/** 아이템 강화 정보를 반환한다 */
	public STItemEnhanceInfo GetItemEnhanceInfo(EItemKinds a_eItemKinds) {
		bool bIsValid = this.TryGetItemEnhanceInfo(a_eItemKinds, out STItemEnhanceInfo stItemEnhanceInfo);
		CAccess.Assert(bIsValid);

		return stItemEnhanceInfo;
	}

	/** 아이템 정보를 반환한다 */
	public bool TryGetItemInfo(EItemKinds a_EItemKinds, out STItemInfo a_stOutItemInfo) {
		a_stOutItemInfo = this.ItemInfoDict.GetValueOrDefault(a_EItemKinds, STItemInfo.INVALID);
		return this.ItemInfoDict.ContainsKey(a_EItemKinds);
	}

	/** 아이템 판매 정보를 반환한다 */
	public bool TryGetItemSaleInfo(EItemKinds a_eItemKinds, out STItemSaleInfo a_stOutItemSaleInfo) {
		a_stOutItemSaleInfo = this.ItemSaleInfoDict.GetValueOrDefault(a_eItemKinds, STItemSaleInfo.INVALID);
		return this.ItemSaleInfoDict.ContainsKey(a_eItemKinds);
	}

	/** 아이템 강화 정보를 반환한다 */
	public bool TryGetItemEnhanceInfo(EItemKinds a_eItemKinds, out STItemEnhanceInfo a_stOutItemEnhanceInfo) {
		a_stOutItemEnhanceInfo = this.ItemEnhanceInfoDict.GetValueOrDefault(a_eItemKinds, STItemEnhanceInfo.INVALID);
		return this.ItemEnhanceInfoDict.ContainsKey(a_eItemKinds);
	}

	/** 아이템 정보를 로드한다 */
	public (Dictionary<EItemKinds, STItemInfo>, Dictionary<EItemKinds, STItemSaleInfo>, Dictionary<EItemKinds, STItemEnhanceInfo>) LoadItemInfos() {
		this.ResetItemInfos();
		return this.LoadItemInfos(this.ItemInfoTablePath);
	}

	/** 아이템 정보를 로드한다 */
	private (Dictionary<EItemKinds, STItemInfo>, Dictionary<EItemKinds, STItemSaleInfo>, Dictionary<EItemKinds, STItemEnhanceInfo>) LoadItemInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadItemInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			return this.DoLoadItemInfos(CResManager.Inst.GetRes<TextAsset>(a_oFilePath).text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 아이템 정보를 로드한다 */
	private (Dictionary<EItemKinds, STItemInfo>, Dictionary<EItemKinds, STItemSaleInfo>, Dictionary<EItemKinds, STItemEnhanceInfo>) DoLoadItemInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		var oJSONNode = SimpleJSON.JSONNode.Parse(a_oJSONStr);

		var oItemInfosList = new List<SimpleJSON.JSONNode>() {
			oJSONNode[KCDefine.U_KEY_GOODS], oJSONNode[KCDefine.U_KEY_CONSUMABLE], oJSONNode[KCDefine.U_KEY_NON_CONSUMABLE], oJSONNode[KCDefine.U_KEY_WEAPON], oJSONNode[KCDefine.U_KEY_ARMOR], oJSONNode[KCDefine.U_KEY_ACCESSORY], oJSONNode[KCDefine.U_KEY_ATTACH]
		};

		var oItemSaleInfosList = new List<SimpleJSON.JSONNode>() {
			oJSONNode[KCDefine.U_KEY_GOODS_SALE], oJSONNode[KCDefine.U_KEY_CONSUMABLE_SALE], oJSONNode[KCDefine.U_KEY_NON_CONSUMABLE_SALE], oJSONNode[KCDefine.U_KEY_WEAPON_SALE], oJSONNode[KCDefine.U_KEY_ARMOR_SALE], oJSONNode[KCDefine.U_KEY_ACCESSORY_SALE], oJSONNode[KCDefine.U_KEY_ATTACH_SALE]
		};

		var oItemEnhanceInfosList = new List<SimpleJSON.JSONNode>() {
			oJSONNode[KCDefine.U_KEY_GOODS_ENHANCE], oJSONNode[KCDefine.U_KEY_CONSUMABLE_ENHANCE], oJSONNode[KCDefine.U_KEY_NON_CONSUMABLE_ENHANCE], oJSONNode[KCDefine.U_KEY_WEAPON_ENHANCE], oJSONNode[KCDefine.U_KEY_ARMOR_ENHANCE], oJSONNode[KCDefine.U_KEY_ACCESSORY_ENHANCE], oJSONNode[KCDefine.U_KEY_ATTACH_ENHANCE]
		};

		for(int i = 0; i < oItemInfosList.Count; ++i) {
			for(int j = 0; j < oItemInfosList[i].Count; ++j) {
				var stItemInfo = new STItemInfo(oItemInfosList[i][j]);

				// 아이템 정보 추가 가능 할 경우
				if(stItemInfo.m_eItemKinds.ExIsValid() && (!this.ItemInfoDict.ContainsKey(stItemInfo.m_eItemKinds) || oItemInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.ItemInfoDict.ExReplaceVal(stItemInfo.m_eItemKinds, stItemInfo);
				}
			}
		}

		for(int i = 0; i < oItemSaleInfosList.Count; ++i) {
			for(int j = 0; j < oItemSaleInfosList[i].Count; ++j) {
				var stItemSaleInfo = new STItemSaleInfo(oItemSaleInfosList[i][j]);

				// 아이템 판매 정보 추가 가능 할 경우
				if(stItemSaleInfo.m_eItemKinds.ExIsValid() && (!this.ItemSaleInfoDict.ContainsKey(stItemSaleInfo.m_eItemKinds) || oItemSaleInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.ItemSaleInfoDict.ExReplaceVal(stItemSaleInfo.m_eItemKinds, stItemSaleInfo);
				}
			}
		}

		for(int i = 0; i < oItemEnhanceInfosList.Count; ++i) {
			for(int j = 0; j < oItemEnhanceInfosList[i].Count; ++j) {
				var stItemEnhanceInfo = new STItemEnhanceInfo(oItemEnhanceInfosList[i][j]);

				// 아이템 강화 정보 추가 가능 할 경우
				if(stItemEnhanceInfo.m_eItemKinds.ExIsValid() && (!this.ItemEnhanceInfoDict.ContainsKey(stItemEnhanceInfo.m_eItemKinds) || oItemEnhanceInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.ItemEnhanceInfoDict.ExReplaceVal(stItemEnhanceInfo.m_eItemKinds, stItemEnhanceInfo);
				}
			}
		}

		return (this.ItemInfoDict, this.ItemSaleInfoDict, this.ItemEnhanceInfoDict);
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
