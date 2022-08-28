#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 아이템 정보 */
[System.Serializable]
public struct STItemInfo {
	public STCommonInfo m_stCommonInfo;

	public EItemKinds m_eItemKinds;
	public EItemKinds m_ePrevItemKinds;
	public EItemKinds m_eNextItemKinds;

	public Dictionary<ulong, STTargetInfo> m_oAttachItemTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oSkillTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAbilityTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAcquireTargetInfoDict;

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
		m_oAcquireTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oItemInfo[string.Format(KCDefine.U_KEY_FMT_ATTACH_ITEM_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oAttachItemTargetInfoDict.TryAdd(Factory.MakeUTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oItemInfo[string.Format(KCDefine.U_KEY_FMT_SKILL_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oSkillTargetInfoDict.TryAdd(Factory.MakeUTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oItemInfo[string.Format(KCDefine.U_KEY_FMT_ABILITY_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oAbilityTargetInfoDict.TryAdd(Factory.MakeUTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oItemInfo[string.Format(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oAcquireTargetInfoDict.TryAdd(Factory.MakeUTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}
	}
	#endregion			// 함수
}

/** 아이템 강화 정보 */
[System.Serializable]
public struct STItemEnhanceInfo {
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
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oPayTargetInfoDict.TryAdd(Factory.MakeUTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oItemEnhanceInfo[string.Format(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oAcquireTargetInfoDict.TryAdd(Factory.MakeUTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}
	}
	#endregion			// 함수
}

/** 아이템 교환 정보 */
[System.Serializable]
public struct STItemTradeInfo {
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
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oPayTargetInfoDict.TryAdd(Factory.MakeUTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oItemTradeInfo[string.Format(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oAcquireTargetInfoDict.TryAdd(Factory.MakeUTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}
	}
	#endregion			// 함수
}

/** 아이템 정보 테이블 */
public partial class CItemInfoTable : CSingleton<CItemInfoTable> {
	#region 프로퍼티
	public Dictionary<EItemKinds, STItemInfo> ItemInfoDict { get; } = new Dictionary<EItemKinds, STItemInfo>();
	public Dictionary<EItemKinds, STItemEnhanceInfo> ItemEnhanceInfoDict { get; } = new Dictionary<EItemKinds, STItemEnhanceInfo>();
	public Dictionary<EItemKinds, STItemTradeInfo> BuyItemTradeInfoDict { get; } = new Dictionary<EItemKinds, STItemTradeInfo>();
	public Dictionary<EItemKinds, STItemTradeInfo> SaleItemTradeInfoDict { get; } = new Dictionary<EItemKinds, STItemTradeInfo>();
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetItemInfos();
	}

	/** 아이템 정보를 리셋한다 */
	public virtual void ResetItemInfos() {
		this.ItemInfoDict.Clear();
		this.ItemEnhanceInfoDict.Clear();
		this.BuyItemTradeInfoDict.Clear();
		this.SaleItemTradeInfoDict.Clear();
	}

	/** 아이템 정보를 리셋한다 */
	public virtual void ResetItemInfos(string a_oJSONStr) {
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
		return this.LoadItemInfos(Access.ItemInfoTableLoadPath);
	}

	/** 아이템 정보를 저장한다 */
	public void SaveItemInfos(string a_oJSONStr, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oJSONStr != null);

		// JSON 문자열이 존재 할 경우
		if(a_oJSONStr != null) {
			this.ResetItemInfos(a_oJSONStr);

#if UNITY_EDITOR || (UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD))
			CFunc.WriteStr(Access.ItemInfoTableSavePath, a_oJSONStr, false);
#else
			CFunc.WriteStr(Access.ItemInfoTableSavePath, a_oJSONStr, true);
#endif			// #if UNITY_EDITOR || (UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD))
		}
	}

	/** JSON 노드를 설정한다 */
	private void SetupJSONNodes(SimpleJSON.JSONNode a_oJSONNode, out List<SimpleJSON.JSONNode> a_oOutItemInfosList, out List<SimpleJSON.JSONNode> a_oOutItemEnhanceInfosList, out List<SimpleJSON.JSONNode> a_oOutBuyItemTradeInfosList, out List<SimpleJSON.JSONNode> a_oOutSaleItemTradeInfosList) {
		a_oOutItemInfosList = new List<SimpleJSON.JSONNode>();
		a_oOutItemEnhanceInfosList = new List<SimpleJSON.JSONNode>();
		a_oOutBuyItemTradeInfosList = new List<SimpleJSON.JSONNode>();
		a_oOutSaleItemTradeInfosList = new List<SimpleJSON.JSONNode>();

		var oTableInfoDictContainer = KDefine.G_TABLE_INFO_DICT_CONTAINER.GetValueOrDefault(Path.GetFileNameWithoutExtension(Access.ItemInfoTableLoadPath));

		// 공용 정보가 존재 할 경우
		if(oTableInfoDictContainer.Item2[this.GetType()].ContainsKey(KCDefine.B_KEY_COMMON)) {
			for(int i = 0; i < oTableInfoDictContainer.Item2[this.GetType()][KCDefine.B_KEY_COMMON].Count; ++i) {
				a_oOutItemInfosList.ExAddVal(a_oJSONNode[oTableInfoDictContainer.Item2[this.GetType()][KCDefine.B_KEY_COMMON][i]]);
			}
		}

		// 강화 정보가 존재 할 경우
		if(oTableInfoDictContainer.Item2[this.GetType()].ContainsKey(KCDefine.B_KEY_ENHANCE)) {
			for(int i = 0; i < oTableInfoDictContainer.Item2[this.GetType()][KCDefine.B_KEY_ENHANCE].Count; ++i) {
				a_oOutItemEnhanceInfosList.ExAddVal(a_oJSONNode[oTableInfoDictContainer.Item2[this.GetType()][KCDefine.B_KEY_ENHANCE][i]]);
			}
		}

		// 구입 교환 정보가 존재 할 경우
		if(oTableInfoDictContainer.Item2[this.GetType()].ContainsKey(KCDefine.B_KEY_BUY_TRADE)) {
			for(int i = 0; i < oTableInfoDictContainer.Item2[this.GetType()][KCDefine.B_KEY_BUY_TRADE].Count; ++i) {
				a_oOutBuyItemTradeInfosList.ExAddVal(a_oJSONNode[oTableInfoDictContainer.Item2[this.GetType()][KCDefine.B_KEY_BUY_TRADE][i]]);
			}
		}

		// 판매 교환 정보가 존재 할 경우
		if(oTableInfoDictContainer.Item2[this.GetType()].ContainsKey(KCDefine.B_KEY_SALE_TRADE)) {
			for(int i = 0; i < oTableInfoDictContainer.Item2[this.GetType()][KCDefine.B_KEY_SALE_TRADE].Count; ++i) {
				a_oOutSaleItemTradeInfosList.ExAddVal(a_oJSONNode[oTableInfoDictContainer.Item2[this.GetType()][KCDefine.B_KEY_SALE_TRADE][i]]);
			}
		}
	}

	/** 아이템 정보를 로드한다 */
	private (Dictionary<EItemKinds, STItemInfo>, Dictionary<EItemKinds, STItemEnhanceInfo>, Dictionary<EItemKinds, STItemTradeInfo>, Dictionary<EItemKinds, STItemTradeInfo>) LoadItemInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if UNITY_EDITOR || (UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD))
		return this.DoLoadItemInfos(File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, false) : CFunc.ReadStrFromRes(a_oFilePath, false));
#else
		return this.DoLoadItemInfos(File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, true) : CFunc.ReadStrFromRes(a_oFilePath, true));
#endif			// #if UNITY_EDITOR || (UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD))
	}

	/** 아이템 정보를 로드한다 */
	private (Dictionary<EItemKinds, STItemInfo>, Dictionary<EItemKinds, STItemEnhanceInfo>, Dictionary<EItemKinds, STItemTradeInfo>, Dictionary<EItemKinds, STItemTradeInfo>) DoLoadItemInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		this.SetupJSONNodes(SimpleJSON.JSONNode.Parse(a_oJSONStr), out List<SimpleJSON.JSONNode> oItemInfosList, out List<SimpleJSON.JSONNode> oItemEnhanceInfosList, out List<SimpleJSON.JSONNode> oBuyItemTradeInfosList, out List<SimpleJSON.JSONNode> oSaleItemTradeInfosList);

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
