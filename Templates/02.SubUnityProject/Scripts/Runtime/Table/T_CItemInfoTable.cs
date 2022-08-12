#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
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
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oAttachItemTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oItemInfo[string.Format(KCDefine.U_KEY_FMT_SKILL_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oSkillTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oItemInfo[string.Format(KCDefine.U_KEY_FMT_ABILITY_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oAbilityTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
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
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oPayTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oItemEnhanceInfo[string.Format(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oAcquireTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}
	}
	#endregion			// 함수
}

/** 아이템 교환 정보 */
[System.Serializable]
public partial struct STItemTradeInfo {
	public STCommonInfo m_stCommonInfo;

	public EItemKinds m_eItemKinds;
	public EItemKinds m_ePrevItemKinds;
	public EItemKinds m_eNextItemKinds;
	
	public Dictionary<ulong, STTargetInfo> m_oPayTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAcquireTargetInfoDict;

	#region 상수
	public static STItemTradeInfo INVALID = new STItemTradeInfo() {
		m_eItemKinds = EItemKinds.NONE, m_ePrevItemKinds = EItemKinds.NONE, m_eNextItemKinds = EItemKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public EItemType ItemType => (EItemType)((int)m_eItemKinds).ExKindsToType();
	public EItemKinds BaseItemKinds => (EItemKinds)((int)m_eItemKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STItemTradeInfo(SimpleJSON.JSONNode a_oItemTradeInfo) {
		m_stCommonInfo = new STCommonInfo(a_oItemTradeInfo);

		m_eItemKinds = a_oItemTradeInfo[KCDefine.U_KEY_ITEM_KINDS].ExIsValid() ? (EItemKinds)a_oItemTradeInfo[KCDefine.U_KEY_ITEM_KINDS].AsInt : EItemKinds.NONE;
		m_ePrevItemKinds = a_oItemTradeInfo[KCDefine.U_KEY_PREV_ITEM_KINDS].ExIsValid() ? (EItemKinds)a_oItemTradeInfo[KCDefine.U_KEY_PREV_ITEM_KINDS].AsInt : EItemKinds.NONE;
		m_eNextItemKinds = a_oItemTradeInfo[KCDefine.U_KEY_NEXT_ITEM_KINDS].ExIsValid() ? (EItemKinds)a_oItemTradeInfo[KCDefine.U_KEY_NEXT_ITEM_KINDS].AsInt : EItemKinds.NONE;

		m_oPayTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oAcquireTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oItemTradeInfo[string.Format(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oPayTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oItemTradeInfo[string.Format(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oAcquireTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}
	}
	#endregion			// 함수
}

/** 아이템 정보 테이블 */
public partial class CItemInfoTable : CSingleton<CItemInfoTable> {
	#region 프로퍼티
	public Dictionary<EItemKinds, STItemInfo> ItemInfoDict { get; private set; } = new Dictionary<EItemKinds, STItemInfo>();
	public Dictionary<EItemKinds, STItemEnhanceInfo> ItemEnhanceInfoDict { get; private set; } = new Dictionary<EItemKinds, STItemEnhanceInfo>();
	public Dictionary<EItemKinds, STItemTradeInfo> BuyItemTradeInfoDict { get; private set; } = new Dictionary<EItemKinds, STItemTradeInfo>();
	public Dictionary<EItemKinds, STItemTradeInfo> SaleItemTradeInfoDict { get; private set; } = new Dictionary<EItemKinds, STItemTradeInfo>();

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
		this.ItemEnhanceInfoDict.Clear();
		this.BuyItemTradeInfoDict.Clear();
		this.SaleItemTradeInfoDict.Clear();
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

	/** 아이템 강화 정보를 반환한다 */
	public STItemEnhanceInfo GetItemEnhanceInfo(EItemKinds a_eItemKinds) {
		bool bIsValid = this.TryGetItemEnhanceInfo(a_eItemKinds, out STItemEnhanceInfo stItemEnhanceInfo);
		CAccess.Assert(bIsValid);

		return stItemEnhanceInfo;
	}

	/** 구입 아이템 교환 정보를 반환한다 */
	public STItemTradeInfo GetBuyItemTradeInfo(EItemKinds a_eItemKinds) {
		bool bIsValid = this.TryGetBuyItemTradeInfo(a_eItemKinds, out STItemTradeInfo stItemTradeInfo);
		CAccess.Assert(bIsValid);

		return stItemTradeInfo;
	}

	/** 판매 아이템 교환 정보를 반환한다 */
	public STItemTradeInfo GetSaleItemTradeInfo(EItemKinds a_eItemKinds) {
		bool bIsValid = this.TryGetSaleItemTradeInfo(a_eItemKinds, out STItemTradeInfo stItemTradeInfo);
		CAccess.Assert(bIsValid);

		return stItemTradeInfo;
	}

	/** 아이템 정보를 반환한다 */
	public bool TryGetItemInfo(EItemKinds a_EItemKinds, out STItemInfo a_stOutItemInfo) {
		a_stOutItemInfo = this.ItemInfoDict.GetValueOrDefault(a_EItemKinds, STItemInfo.INVALID);
		return this.ItemInfoDict.ContainsKey(a_EItemKinds);
	}

	/** 아이템 강화 정보를 반환한다 */
	public bool TryGetItemEnhanceInfo(EItemKinds a_eItemKinds, out STItemEnhanceInfo a_stOutItemEnhanceInfo) {
		a_stOutItemEnhanceInfo = this.ItemEnhanceInfoDict.GetValueOrDefault(a_eItemKinds, STItemEnhanceInfo.INVALID);
		return this.ItemEnhanceInfoDict.ContainsKey(a_eItemKinds);
	}

	/** 구입 아이템 교환 정보를 반환한다 */
	public bool TryGetBuyItemTradeInfo(EItemKinds a_eItemKinds, out STItemTradeInfo a_stOutItemTradeInfo) {
		a_stOutItemTradeInfo = this.BuyItemTradeInfoDict.GetValueOrDefault(a_eItemKinds, STItemTradeInfo.INVALID);
		return this.BuyItemTradeInfoDict.ContainsKey(a_eItemKinds);
	}

	/** 판매 아이템 교환 정보를 반환한다 */
	public bool TryGetSaleItemTradeInfo(EItemKinds a_eItemKinds, out STItemTradeInfo a_stOutItemTradeInfo) {
		a_stOutItemTradeInfo = this.SaleItemTradeInfoDict.GetValueOrDefault(a_eItemKinds, STItemTradeInfo.INVALID);
		return this.SaleItemTradeInfoDict.ContainsKey(a_eItemKinds);
	}

	/** 아이템 정보를 로드한다 */
	public (Dictionary<EItemKinds, STItemInfo>, Dictionary<EItemKinds, STItemEnhanceInfo>, Dictionary<EItemKinds, STItemTradeInfo>, Dictionary<EItemKinds, STItemTradeInfo>) LoadItemInfos() {
		this.ResetItemInfos();
		return this.LoadItemInfos(this.ItemInfoTablePath);
	}

	/** 아이템 정보를 로드한다 */
	private (Dictionary<EItemKinds, STItemInfo>, Dictionary<EItemKinds, STItemEnhanceInfo>, Dictionary<EItemKinds, STItemTradeInfo>, Dictionary<EItemKinds, STItemTradeInfo>) LoadItemInfos(string a_oFilePath) {
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
	private (Dictionary<EItemKinds, STItemInfo>, Dictionary<EItemKinds, STItemEnhanceInfo>, Dictionary<EItemKinds, STItemTradeInfo>, Dictionary<EItemKinds, STItemTradeInfo>) DoLoadItemInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());

		var oJSONNode = SimpleJSON.JSONNode.Parse(a_oJSONStr);
		var oItemInfosList = new List<SimpleJSON.JSONNode>();
		var oItemEnhanceInfosList = new List<SimpleJSON.JSONNode>();
		var oBuyItemTradeInfosList = new List<SimpleJSON.JSONNode>();
		var oSaleItemTradeInfosList = new List<SimpleJSON.JSONNode>();

		for(int i = 0; i < KDefine.G_KEY_ITEM_IT_INFOS_LIST.Count; ++i) {
			oItemInfosList.ExAddVal(oJSONNode[KDefine.G_KEY_ITEM_IT_INFOS_LIST[i]]);
		}

		for(int i = 0; i < KDefine.G_KEY_ITEM_IT_ENHANCE_INFOS_LIST.Count; ++i) {
			oItemEnhanceInfosList.ExAddVal(oJSONNode[KDefine.G_KEY_ITEM_IT_ENHANCE_INFOS_LIST[i]]);
		}

		for(int i = 0; i < KDefine.G_KEY_ITEM_IT_BUY_TRADE_INFOS_LIST.Count; ++i) {
			oBuyItemTradeInfosList.ExAddVal(oJSONNode[KDefine.G_KEY_ITEM_IT_BUY_TRADE_INFOS_LIST[i]]);
		}

		for(int i = 0; i < KDefine.G_KEY_ITEM_IT_SALE_TRADE_INFOS_LIST.Count; ++i) {
			oSaleItemTradeInfosList.ExAddVal(oJSONNode[KDefine.G_KEY_ITEM_IT_SALE_TRADE_INFOS_LIST[i]]);
		}

		for(int i = 0; i < oItemInfosList.Count; ++i) {
			for(int j = 0; j < oItemInfosList[i].Count; ++j) {
				var stItemInfo = new STItemInfo(oItemInfosList[i][j]);

				// 아이템 정보 추가 가능 할 경우
				if(stItemInfo.m_eItemKinds.ExIsValid() && (!this.ItemInfoDict.ContainsKey(stItemInfo.m_eItemKinds) || oItemInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.ItemInfoDict.ExReplaceVal(stItemInfo.m_eItemKinds, stItemInfo);
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

		for(int i = 0; i < oBuyItemTradeInfosList.Count; ++i) {
			for(int j = 0; j < oBuyItemTradeInfosList[i].Count; ++j) {
				var stItemTradeInfo = new STItemTradeInfo(oBuyItemTradeInfosList[i][j]);

				// 구입 아이템 교환 정보 추가 가능 할 경우
				if(stItemTradeInfo.m_eItemKinds.ExIsValid() && (!this.BuyItemTradeInfoDict.ContainsKey(stItemTradeInfo.m_eItemKinds) || oBuyItemTradeInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.BuyItemTradeInfoDict.ExReplaceVal(stItemTradeInfo.m_eItemKinds, stItemTradeInfo);
				}
			}
		}

		for(int i = 0; i < oSaleItemTradeInfosList.Count; ++i) {
			for(int j = 0; j < oSaleItemTradeInfosList[i].Count; ++j) {
				var stItemTradeInfo = new STItemTradeInfo(oSaleItemTradeInfosList[i][j]);

				// 판매 아이템 교환 정보 추가 가능 할 경우
				if(stItemTradeInfo.m_eItemKinds.ExIsValid() && (!this.SaleItemTradeInfoDict.ContainsKey(stItemTradeInfo.m_eItemKinds) || oSaleItemTradeInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.BuyItemTradeInfoDict.ExReplaceVal(stItemTradeInfo.m_eItemKinds, stItemTradeInfo);
				}
			}
		}

		return (this.ItemInfoDict, this.ItemEnhanceInfoDict, this.BuyItemTradeInfoDict, this.SaleItemTradeInfoDict);
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
