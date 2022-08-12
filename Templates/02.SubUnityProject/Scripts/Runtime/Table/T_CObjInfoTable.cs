#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 객체 정보 */
[System.Serializable]
public partial struct STObjInfo {
	public STCommonInfo m_stCommonInfo;
	public Vector3 m_stSize;

	public EObjKinds m_eObjKinds;
	public EObjKinds m_ePrevObjKinds;
	public EObjKinds m_eNextObjKinds;

	public List<EResKinds> m_oResKindsList;
	public Dictionary<ulong, STTargetInfo> m_oDropItemTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oEquipItemTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oSkillTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAbilityTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAcquireTargetInfoDict;

	#region 상수
	public static STObjInfo INVALID = new STObjInfo() {
		m_eObjKinds = EObjKinds.NONE, m_ePrevObjKinds = EObjKinds.NONE, m_eNextObjKinds = EObjKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public EObjType ObjType => (EObjType)((int)m_eObjKinds).ExKindsToType();
	public EObjKinds BaseObjKinds => (EObjKinds)((int)m_eObjKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티
	
	#region 함수
	/** 생성자 */
	public STObjInfo(SimpleJSON.JSONNode a_oObjInfo) {
		m_stCommonInfo = new STCommonInfo(a_oObjInfo);
		m_stSize = a_oObjInfo[KCDefine.U_KEY_SIZE].ExIsValid() ? new Vector3(a_oObjInfo[KCDefine.U_KEY_SIZE][KCDefine.B_VAL_0_INT].AsFloat, a_oObjInfo[KCDefine.U_KEY_SIZE][KCDefine.B_VAL_1_INT].AsFloat, a_oObjInfo[KCDefine.U_KEY_SIZE][KCDefine.B_VAL_2_INT].AsFloat) : Vector3.zero;

		m_eObjKinds = a_oObjInfo[KCDefine.U_KEY_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjInfo[KCDefine.U_KEY_OBJ_KINDS].AsInt : EObjKinds.NONE;
		m_ePrevObjKinds = a_oObjInfo[KCDefine.U_KEY_PREV_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjInfo[KCDefine.U_KEY_PREV_OBJ_KINDS].AsInt : EObjKinds.NONE;
		m_eNextObjKinds = a_oObjInfo[KCDefine.U_KEY_NEXT_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjInfo[KCDefine.U_KEY_NEXT_OBJ_KINDS].AsInt : EObjKinds.NONE;

		m_oResKindsList = new List<EResKinds>();
		m_oDropItemTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oEquipItemTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oSkillTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oAbilityTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oAcquireTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_RES_KINDS; ++i) {
			string oKey = string.Format(KCDefine.U_KEY_FMT_RES_KINDS, i + KCDefine.B_VAL_1_INT);
			if(a_oObjInfo[oKey].ExIsValid()) { m_oResKindsList.ExAddVal((EResKinds)a_oObjInfo[oKey].AsInt); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oObjInfo[string.Format(KCDefine.U_KEY_FMT_DROP_ITEM_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oDropItemTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oObjInfo[string.Format(KCDefine.U_KEY_FMT_EQUIP_ITEM_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oEquipItemTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oObjInfo[string.Format(KCDefine.U_KEY_FMT_SKILL_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oSkillTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oObjInfo[string.Format(KCDefine.U_KEY_FMT_ABILITY_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oAbilityTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oObjInfo[string.Format(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oAcquireTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}
	}
	#endregion			// 함수
}

/** 객체 강화 정보 */
[System.Serializable]
public partial struct STObjEnhanceInfo {
	public STCommonInfo m_stCommonInfo;

	public EObjKinds m_eObjKinds;
	public EObjKinds m_ePrevObjKinds;
	public EObjKinds m_eNextObjKinds;

	public Dictionary<ulong, STTargetInfo> m_oPayTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAcquireTargetInfoDict;

	#region 상수
	public static STObjEnhanceInfo INVALID = new STObjEnhanceInfo() {
		m_eObjKinds = EObjKinds.NONE, m_ePrevObjKinds = EObjKinds.NONE, m_eNextObjKinds = EObjKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public EObjType ObjType => (EObjType)((int)m_eObjKinds).ExKindsToType();
	public EObjKinds BaseObjKinds => (EObjKinds)((int)m_eObjKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STObjEnhanceInfo(SimpleJSON.JSONNode a_oObjTradeInfo) {
		m_stCommonInfo = new STCommonInfo(a_oObjTradeInfo);

		m_eObjKinds = a_oObjTradeInfo[KCDefine.U_KEY_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjTradeInfo[KCDefine.U_KEY_OBJ_KINDS].AsInt : EObjKinds.NONE;
		m_ePrevObjKinds = a_oObjTradeInfo[KCDefine.U_KEY_PREV_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjTradeInfo[KCDefine.U_KEY_PREV_OBJ_KINDS].AsInt : EObjKinds.NONE;
		m_eNextObjKinds = a_oObjTradeInfo[KCDefine.U_KEY_NEXT_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjTradeInfo[KCDefine.U_KEY_NEXT_OBJ_KINDS].AsInt : EObjKinds.NONE;

		m_oPayTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oAcquireTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oObjTradeInfo[string.Format(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oPayTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oObjTradeInfo[string.Format(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oAcquireTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}
	}
	#endregion			// 함수
}

/** 객체 교환 정보 */
[System.Serializable]
public partial struct STObjTradeInfo {
	public STCommonInfo m_stCommonInfo;

	public EObjKinds m_eObjKinds;
	public EObjKinds m_ePrevObjKinds;
	public EObjKinds m_eNextObjKinds;

	public Dictionary<ulong, STTargetInfo> m_oPayTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAcquireTargetInfoDict;

	#region 상수
	public static STObjTradeInfo INVALID = new STObjTradeInfo() {
		m_eObjKinds = EObjKinds.NONE, m_ePrevObjKinds = EObjKinds.NONE, m_eNextObjKinds = EObjKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public EObjType ObjType => (EObjType)((int)m_eObjKinds).ExKindsToType();
	public EObjKinds BaseObjKinds => (EObjKinds)((int)m_eObjKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STObjTradeInfo(SimpleJSON.JSONNode a_oObjTradeInfo) {
		m_stCommonInfo = new STCommonInfo(a_oObjTradeInfo);

		m_eObjKinds = a_oObjTradeInfo[KCDefine.U_KEY_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjTradeInfo[KCDefine.U_KEY_OBJ_KINDS].AsInt : EObjKinds.NONE;
		m_ePrevObjKinds = a_oObjTradeInfo[KCDefine.U_KEY_PREV_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjTradeInfo[KCDefine.U_KEY_PREV_OBJ_KINDS].AsInt : EObjKinds.NONE;
		m_eNextObjKinds = a_oObjTradeInfo[KCDefine.U_KEY_NEXT_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjTradeInfo[KCDefine.U_KEY_NEXT_OBJ_KINDS].AsInt : EObjKinds.NONE;

		m_oPayTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oAcquireTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oObjTradeInfo[string.Format(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oPayTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oObjTradeInfo[string.Format(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oAcquireTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}
	}
	#endregion			// 함수
}

/** 객체 정보 테이블 */
public partial class CObjInfoTable : CSingleton<CObjInfoTable> {
	#region 프로퍼티
	public Dictionary<EObjKinds, STObjInfo> ObjInfoDict { get; private set; } = new Dictionary<EObjKinds, STObjInfo>();
	public Dictionary<EObjKinds, STObjEnhanceInfo> ObjEnhanceInfoDict { get; private set; } = new Dictionary<EObjKinds, STObjEnhanceInfo>();
	public Dictionary<EObjKinds, STObjTradeInfo> BuyObjTradeInfoDict { get; private set; } = new Dictionary<EObjKinds, STObjTradeInfo>();
	public Dictionary<EObjKinds, STObjTradeInfo> SaleObjTradeInfoDict { get; private set; } = new Dictionary<EObjKinds, STObjTradeInfo>();

	private string ObjInfoTablePath {
		get {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_OBJ_INFO;
#else
			return KCDefine.U_TABLE_P_G_OBJ_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetObjInfos();
	}

	/** 객체 정보를 리셋한다 */
	public void ResetObjInfos() {
		this.ObjInfoDict.Clear();
		this.ObjEnhanceInfoDict.Clear();
		this.BuyObjTradeInfoDict.Clear();
		this.SaleObjTradeInfoDict.Clear();
	}

	/** 객체 정보를 리셋한다 */
	public void ResetObjInfos(string a_oJSONStr) {
		this.ResetObjInfos();
		this.DoLoadObjInfos(a_oJSONStr);
	}

	/** 객체 정보를 반환한다 */
	public STObjInfo GetObjInfo(EObjKinds a_eObjKinds) {
		bool bIsValid = this.TryGetObjInfo(a_eObjKinds, out STObjInfo stObjInfo);
		CAccess.Assert(bIsValid);

		return stObjInfo;
	}

	/** 객체 강화 정보를 반환한다 */
	public STObjEnhanceInfo GetObjEnhanceInfo(EObjKinds a_eObjKinds) {
		bool bIsValid = this.TryGetObjEnhanceInfo(a_eObjKinds, out STObjEnhanceInfo stObjEnhanceInfo);
		CAccess.Assert(bIsValid);

		return stObjEnhanceInfo;
	}

	/** 구입 객체 교환 정보를 반환한다 */
	public STObjTradeInfo GetBuyObjTradeInfo(EObjKinds a_eObjKinds) {
		bool bIsValid = this.TryGetBuyObjTradeInfo(a_eObjKinds, out STObjTradeInfo stObjTradeInfo);
		CAccess.Assert(bIsValid);

		return stObjTradeInfo;
	}

	/** 판매 객체 교환 정보를 반환한다 */
	public STObjTradeInfo GetSaleObjTradeInfo(EObjKinds a_eObjKinds) {
		bool bIsValid = this.TryGetSaleObjTradeInfo(a_eObjKinds, out STObjTradeInfo stObjTradeInfo);
		CAccess.Assert(bIsValid);

		return stObjTradeInfo;
	}

	/** 객체 정보를 반환한다 */
	public bool TryGetObjInfo(EObjKinds a_eObjKinds, out STObjInfo a_stOutObjInfo) {
		a_stOutObjInfo = this.ObjInfoDict.GetValueOrDefault(a_eObjKinds, STObjInfo.INVALID);
		return this.ObjInfoDict.ContainsKey(a_eObjKinds);
	}

	/** 객체 강화 정보를 반환한다 */
	public bool TryGetObjEnhanceInfo(EObjKinds a_eObjKinds, out STObjEnhanceInfo a_stOutObjEnhanceInfo) {
		a_stOutObjEnhanceInfo = this.ObjEnhanceInfoDict.GetValueOrDefault(a_eObjKinds, STObjEnhanceInfo.INVALID);
		return this.ObjEnhanceInfoDict.ContainsKey(a_eObjKinds);
	}

	/** 구입 객체 교환 정보를 반환한다 */
	public bool TryGetBuyObjTradeInfo(EObjKinds a_eObjKinds, out STObjTradeInfo a_stOutObjTradeInfo) {
		a_stOutObjTradeInfo = this.BuyObjTradeInfoDict.GetValueOrDefault(a_eObjKinds, STObjTradeInfo.INVALID);
		return this.BuyObjTradeInfoDict.ContainsKey(a_eObjKinds);
	}

	/** 판매 객체 교환 정보를 반환한다 */
	public bool TryGetSaleObjTradeInfo(EObjKinds a_eObjKinds, out STObjTradeInfo a_stOutObjTradeInfo) {
		a_stOutObjTradeInfo = this.SaleObjTradeInfoDict.GetValueOrDefault(a_eObjKinds, STObjTradeInfo.INVALID);
		return this.SaleObjTradeInfoDict.ContainsKey(a_eObjKinds);
	}

	/** 객체 정보를 로드한다 */
	public (Dictionary<EObjKinds, STObjInfo>, Dictionary<EObjKinds, STObjEnhanceInfo>, Dictionary<EObjKinds, STObjTradeInfo>, Dictionary<EObjKinds, STObjTradeInfo>) LoadObjInfos() {
		this.ResetObjInfos();
		return this.LoadObjInfos(this.ObjInfoTablePath);
	}

	/** 객체 정보를 로드한다 */
	private (Dictionary<EObjKinds, STObjInfo>, Dictionary<EObjKinds, STObjEnhanceInfo>, Dictionary<EObjKinds, STObjTradeInfo>, Dictionary<EObjKinds, STObjTradeInfo>) LoadObjInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadObjInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			return this.DoLoadObjInfos(CResManager.Inst.GetRes<TextAsset>(a_oFilePath).text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 객체 정보를 로드한다 */
	private (Dictionary<EObjKinds, STObjInfo>, Dictionary<EObjKinds, STObjEnhanceInfo>, Dictionary<EObjKinds, STObjTradeInfo>, Dictionary<EObjKinds, STObjTradeInfo>) DoLoadObjInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());

		var oJSONNode = SimpleJSON.JSONNode.Parse(a_oJSONStr);
		var oObjInfosList = new List<SimpleJSON.JSONNode>();
		var oObjEnhanceInfosList = new List<SimpleJSON.JSONNode>();
		var oBuyObjTradeInfosList = new List<SimpleJSON.JSONNode>();
		var oSaleObjTradeInfosList = new List<SimpleJSON.JSONNode>();

		for(int i = 0; i < KDefine.G_KEY_OBJ_IT_INFOS_LIST.Count; ++i) {
			oObjInfosList.ExAddVal(oJSONNode[KDefine.G_KEY_OBJ_IT_INFOS_LIST[i]]);
		}

		for(int i = 0; i < KDefine.G_KEY_OBJ_IT_ENHANCE_INFOS_LIST.Count; ++i) {
			oObjEnhanceInfosList.ExAddVal(oJSONNode[KDefine.G_KEY_OBJ_IT_ENHANCE_INFOS_LIST[i]]);
		}

		for(int i = 0; i < KDefine.G_KEY_OBJ_IT_BUY_TRADE_INFOS_LIST.Count; ++i) {
			oBuyObjTradeInfosList.ExAddVal(oJSONNode[KDefine.G_KEY_OBJ_IT_BUY_TRADE_INFOS_LIST[i]]);
		}

		for(int i = 0; i < KDefine.G_KEY_OBJ_IT_SALE_TRADE_INFOS_LIST.Count; ++i) {
			oSaleObjTradeInfosList.ExAddVal(oJSONNode[KDefine.G_KEY_OBJ_IT_SALE_TRADE_INFOS_LIST[i]]);
		}

		for(int i = 0; i < oObjInfosList.Count; ++i) {
			for(int j = 0; j < oObjInfosList[i].Count; ++j) {
				var stObjInfo = new STObjInfo(oObjInfosList[i][j]);

				// 객체 정보 추가 가능 할 경우
				if(stObjInfo.m_eObjKinds.ExIsValid() && (!this.ObjInfoDict.ContainsKey(stObjInfo.m_eObjKinds) || oObjInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.ObjInfoDict.ExReplaceVal(stObjInfo.m_eObjKinds, stObjInfo);
				}
			}
		}

		for(int i = 0; i < oObjEnhanceInfosList.Count; ++i) {
			for(int j = 0; j < oObjEnhanceInfosList[i].Count; ++j) {
				var stObjEnhanceInfo = new STObjEnhanceInfo(oObjEnhanceInfosList[i][j]);

				// 객체 강화 정보 추가 가능 할 경우
				if(stObjEnhanceInfo.m_eObjKinds.ExIsValid() && (!this.BuyObjTradeInfoDict.ContainsKey(stObjEnhanceInfo.m_eObjKinds) || oObjEnhanceInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.ObjEnhanceInfoDict.ExReplaceVal(stObjEnhanceInfo.m_eObjKinds, stObjEnhanceInfo);
				}
			}
		}

		for(int i = 0; i < oBuyObjTradeInfosList.Count; ++i) {
			for(int j = 0; j < oBuyObjTradeInfosList[i].Count; ++j) {
				var stObjTradeInfo = new STObjTradeInfo(oBuyObjTradeInfosList[i][j]);

				// 구입 객체 교환 정보 추가 가능 할 경우
				if(stObjTradeInfo.m_eObjKinds.ExIsValid() && (!this.BuyObjTradeInfoDict.ContainsKey(stObjTradeInfo.m_eObjKinds) || oBuyObjTradeInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.BuyObjTradeInfoDict.ExReplaceVal(stObjTradeInfo.m_eObjKinds, stObjTradeInfo);
				}
			}
		}

		for(int i = 0; i < oSaleObjTradeInfosList.Count; ++i) {
			for(int j = 0; j < oSaleObjTradeInfosList[i].Count; ++j) {
				var stObjTradeInfo = new STObjTradeInfo(oSaleObjTradeInfosList[i][j]);

				// 판매 객체 교환 정보 추가 가능 할 경우
				if(stObjTradeInfo.m_eObjKinds.ExIsValid() && (!this.SaleObjTradeInfoDict.ContainsKey(stObjTradeInfo.m_eObjKinds) || oSaleObjTradeInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.SaleObjTradeInfoDict.ExReplaceVal(stObjTradeInfo.m_eObjKinds, stObjTradeInfo);
				}
			}
		}

		return (this.ObjInfoDict, this.ObjEnhanceInfoDict, this.BuyObjTradeInfoDict, this.SaleObjTradeInfoDict);
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
