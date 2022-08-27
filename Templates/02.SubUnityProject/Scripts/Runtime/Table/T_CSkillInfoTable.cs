#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 스킬 정보 */
[System.Serializable]
public struct STSkillInfo {
	public STCommonInfo m_stCommonInfo;
	public STTimeInfo m_stTimeInfo;

	public int m_nMaxApplyTimes;
	public ESkillKinds m_eSkillKinds;
	public ESkillKinds m_ePrevSkillKinds;
	public ESkillKinds m_eNextSkillKinds;
	public ESkillApplyKinds m_eSkillApplyKinds;

	public List<EFXKinds> m_oFXKindsList;
	public List<EResKinds> m_oResKindsList;
	public Dictionary<ulong, STTargetInfo> m_oAbilityTargetInfoDict;

	#region 상수
	public static STSkillInfo INVALID = new STSkillInfo() {
		m_eSkillKinds = ESkillKinds.NONE, m_ePrevSkillKinds = ESkillKinds.NONE, m_eNextSkillKinds = ESkillKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public ESkillType SkillType => (ESkillType)((int)m_eSkillKinds).ExKindsToType();
	public ESkillKinds BaseSkillKinds => (ESkillKinds)((int)m_eSkillKinds).ExKindsToSubKindsType();
	
	public ESkillApplyType SkillApplyType => (ESkillApplyType)((int)m_eSkillApplyKinds).ExKindsToType();
	public ESkillApplyKinds BaseSkillApplyKinds => (ESkillApplyKinds)((int)m_eSkillApplyKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STSkillInfo(SimpleJSON.JSONNode a_oSkillInfo) {
		m_stCommonInfo = new STCommonInfo(a_oSkillInfo);
		m_stTimeInfo = new STTimeInfo(a_oSkillInfo[KCDefine.U_KEY_TIME_INFO]);

		m_nMaxApplyTimes = a_oSkillInfo[KCDefine.U_KEY_MAX_APPLY_TIMES].ExIsValid() ? a_oSkillInfo[KCDefine.U_KEY_MAX_APPLY_TIMES].AsInt : KCDefine.B_VAL_0_INT;
		m_eSkillKinds = a_oSkillInfo[KCDefine.U_KEY_SKILL_KINDS].ExIsValid() ? (ESkillKinds)a_oSkillInfo[KCDefine.U_KEY_SKILL_KINDS].AsInt : ESkillKinds.NONE;
		m_ePrevSkillKinds = a_oSkillInfo[KCDefine.U_KEY_PREV_SKILL_KINDS].ExIsValid() ? (ESkillKinds)a_oSkillInfo[KCDefine.U_KEY_PREV_SKILL_KINDS].AsInt : ESkillKinds.NONE;
		m_eNextSkillKinds = a_oSkillInfo[KCDefine.U_KEY_NEXT_SKILL_KINDS].ExIsValid() ? (ESkillKinds)a_oSkillInfo[KCDefine.U_KEY_NEXT_SKILL_KINDS].AsInt : ESkillKinds.NONE;
		m_eSkillApplyKinds = a_oSkillInfo[KCDefine.U_KEY_SKILL_APPLY_KINDS].ExIsValid() ? (ESkillApplyKinds)a_oSkillInfo[KCDefine.U_KEY_SKILL_APPLY_KINDS].AsInt : ESkillApplyKinds.NONE;

		m_oFXKindsList = new List<EFXKinds>();
		m_oResKindsList = new List<EResKinds>();
		m_oAbilityTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_FX_KINDS; ++i) {
			string oKey = string.Format(KCDefine.U_KEY_FMT_FX_KINDS, i + KCDefine.B_VAL_1_INT);
			if(a_oSkillInfo[oKey].ExIsValid()) { m_oFXKindsList.ExAddVal((EFXKinds)a_oSkillInfo[oKey].AsInt); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_RES_KINDS; ++i) {
			string oKey = string.Format(KCDefine.U_KEY_FMT_RES_KINDS, i + KCDefine.B_VAL_1_INT);
			if(a_oSkillInfo[oKey].ExIsValid()) { m_oResKindsList.ExAddVal((EResKinds)a_oSkillInfo[oKey].AsInt); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oSkillInfo[string.Format(KCDefine.U_KEY_FMT_ABILITY_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oAbilityTargetInfoDict.TryAdd(Factory.MakeUTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}
	}
	#endregion			// 함수
}

/** 스킬 강화 정보 */
[System.Serializable]
public struct STSkillEnhanceInfo {
	public STCommonInfo m_stCommonInfo;

	public ESkillKinds m_eSkillKinds;
	public ESkillKinds m_ePrevSkillKinds;
	public ESkillKinds m_eNextSkillKinds;

	public Dictionary<ulong, STTargetInfo> m_oPayTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAcquireTargetInfoDict;

	#region 상수
	public static STSkillEnhanceInfo INVALID = new STSkillEnhanceInfo() {
		m_eSkillKinds = ESkillKinds.NONE, m_ePrevSkillKinds = ESkillKinds.NONE, m_eNextSkillKinds = ESkillKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public ESkillType SkillType => (ESkillType)((int)m_eSkillKinds).ExKindsToType();
	public ESkillKinds BaseSkillKinds => (ESkillKinds)((int)m_eSkillKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STSkillEnhanceInfo(SimpleJSON.JSONNode a_oSkillEnhanceInfo) {
		m_stCommonInfo = new STCommonInfo(a_oSkillEnhanceInfo);

		m_eSkillKinds = a_oSkillEnhanceInfo[KCDefine.U_KEY_SKILL_KINDS].ExIsValid() ? (ESkillKinds)a_oSkillEnhanceInfo[KCDefine.U_KEY_SKILL_KINDS].AsInt : ESkillKinds.NONE;
		m_ePrevSkillKinds = a_oSkillEnhanceInfo[KCDefine.U_KEY_PREV_SKILL_KINDS].ExIsValid() ? (ESkillKinds)a_oSkillEnhanceInfo[KCDefine.U_KEY_PREV_SKILL_KINDS].AsInt : ESkillKinds.NONE;
		m_eNextSkillKinds = a_oSkillEnhanceInfo[KCDefine.U_KEY_NEXT_SKILL_KINDS].ExIsValid() ? (ESkillKinds)a_oSkillEnhanceInfo[KCDefine.U_KEY_NEXT_SKILL_KINDS].AsInt : ESkillKinds.NONE;

		m_oPayTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oAcquireTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oSkillEnhanceInfo[string.Format(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oPayTargetInfoDict.TryAdd(Factory.MakeUTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oSkillEnhanceInfo[string.Format(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oAcquireTargetInfoDict.TryAdd(Factory.MakeUTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}
	}
	#endregion			// 함수
}

/** 구입 스킬 교환 정보 */
[System.Serializable]
public struct STSkillTradeInfo {
	public STCommonInfo m_stCommonInfo;

	public ESkillKinds m_eSkillKinds;
	public ESkillKinds m_ePrevSkillKinds;
	public ESkillKinds m_eNextSkillKinds;

	public Dictionary<ulong, STTargetInfo> m_oPayTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAcquireTargetInfoDict;

	#region 상수
	public static STSkillTradeInfo INVALID = new STSkillTradeInfo() {
		m_eSkillKinds = ESkillKinds.NONE, m_ePrevSkillKinds = ESkillKinds.NONE, m_eNextSkillKinds = ESkillKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public ESkillType SkillType => (ESkillType)((int)m_eSkillKinds).ExKindsToType();
	public ESkillKinds BaseSkillKinds => (ESkillKinds)((int)m_eSkillKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STSkillTradeInfo(SimpleJSON.JSONNode a_oSkillTradeInfo) {
		m_stCommonInfo = new STCommonInfo(a_oSkillTradeInfo);

		m_eSkillKinds = a_oSkillTradeInfo[KCDefine.U_KEY_SKILL_KINDS].ExIsValid() ? (ESkillKinds)a_oSkillTradeInfo[KCDefine.U_KEY_SKILL_KINDS].AsInt : ESkillKinds.NONE;
		m_ePrevSkillKinds = a_oSkillTradeInfo[KCDefine.U_KEY_PREV_SKILL_KINDS].ExIsValid() ? (ESkillKinds)a_oSkillTradeInfo[KCDefine.U_KEY_PREV_SKILL_KINDS].AsInt : ESkillKinds.NONE;
		m_eNextSkillKinds = a_oSkillTradeInfo[KCDefine.U_KEY_NEXT_SKILL_KINDS].ExIsValid() ? (ESkillKinds)a_oSkillTradeInfo[KCDefine.U_KEY_NEXT_SKILL_KINDS].AsInt : ESkillKinds.NONE;

		m_oPayTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oAcquireTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oSkillTradeInfo[string.Format(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oPayTargetInfoDict.TryAdd(Factory.MakeUTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oSkillTradeInfo[string.Format(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oAcquireTargetInfoDict.TryAdd(Factory.MakeUTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}
	}
	#endregion			// 함수
}

/** 스킬 정보 테이블 */
public partial class CSkillInfoTable : CSingleton<CSkillInfoTable> {
	#region 프로퍼티
	public Dictionary<ESkillKinds, STSkillInfo> SkillInfoDict { get; } = new Dictionary<ESkillKinds, STSkillInfo>();
	public Dictionary<ESkillKinds, STSkillEnhanceInfo> SkillEnhanceInfoDict { get; } = new Dictionary<ESkillKinds, STSkillEnhanceInfo>();
	public Dictionary<ESkillKinds, STSkillTradeInfo> BuySkillTradeInfoDict { get; } = new Dictionary<ESkillKinds, STSkillTradeInfo>();
	public Dictionary<ESkillKinds, STSkillTradeInfo> SaleSkillTradeInfoDict { get; } = new Dictionary<ESkillKinds, STSkillTradeInfo>();
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetSkillInfos();
	}

	/** 스킬 정보를 리셋한다 */
	public virtual void ResetSkillInfos() {
		this.SkillInfoDict.Clear();
		this.SkillEnhanceInfoDict.Clear();
		this.BuySkillTradeInfoDict.Clear();
		this.SaleSkillTradeInfoDict.Clear();
	}

	/** 스킬 정보를 리셋한다 */
	public virtual void ResetSkillInfos(string a_oJSONStr) {
		this.ResetSkillInfos();
		this.DoLoadSkillInfos(a_oJSONStr);
	}

	/** 스킬 정보를 반환한다 */
	public STSkillInfo GetSkillInfo(ESkillKinds a_ESkillKinds) {
		bool bIsValid = this.TryGetSkillInfo(a_ESkillKinds, out STSkillInfo stSkillInfo);
		CAccess.Assert(bIsValid);

		return stSkillInfo;
	}

	/** 스킬 강화 정보를 반환한다 */
	public STSkillEnhanceInfo GetSkillEnhanceInfo(ESkillKinds a_eSkillKinds) {
		bool bIsValid = this.TryGetSkillEnhanceInfo(a_eSkillKinds, out STSkillEnhanceInfo stSkillEnhanceInfo);
		CAccess.Assert(bIsValid);

		return stSkillEnhanceInfo;
	}

	/** 구입 스킬 교환 정보를 반환한다 */
	public STSkillTradeInfo GetBuySkillTradeInfo(ESkillKinds a_eSkillKinds) {
		bool bIsValid = this.TryGetBuySkillTradeInfo(a_eSkillKinds, out STSkillTradeInfo stSkillTradeInfo);
		CAccess.Assert(bIsValid);

		return stSkillTradeInfo;
	}

	/** 판매 스킬 교환 정보를 반환한다 */
	public STSkillTradeInfo GetSaleSkillTradeInfo(ESkillKinds a_eSkillKinds) {
		bool bIsValid = this.TryGetSaleSkillTradeInfo(a_eSkillKinds, out STSkillTradeInfo stSkillTradeInfo);
		CAccess.Assert(bIsValid);

		return stSkillTradeInfo;
	}

	/** 스킬 정보를 반환한다 */
	public bool TryGetSkillInfo(ESkillKinds a_ESkillKinds, out STSkillInfo a_stOutSkillInfo) {
		a_stOutSkillInfo = this.SkillInfoDict.GetValueOrDefault(a_ESkillKinds, STSkillInfo.INVALID);
		return this.SkillInfoDict.ContainsKey(a_ESkillKinds);
	}

	/** 스킬 강화 정보를 반환한다 */
	public bool TryGetSkillEnhanceInfo(ESkillKinds a_eSkillKinds, out STSkillEnhanceInfo a_stOutSkillEnhanceInfo) {
		a_stOutSkillEnhanceInfo = this.SkillEnhanceInfoDict.GetValueOrDefault(a_eSkillKinds, STSkillEnhanceInfo.INVALID);
		return this.SkillEnhanceInfoDict.ContainsKey(a_eSkillKinds);
	}

	/** 구입 스킬 교환 정보를 반환한다 */
	public bool TryGetBuySkillTradeInfo(ESkillKinds a_eSkillKinds, out STSkillTradeInfo a_stOutSkillTradeInfo) {
		a_stOutSkillTradeInfo = this.BuySkillTradeInfoDict.GetValueOrDefault(a_eSkillKinds, STSkillTradeInfo.INVALID);
		return this.BuySkillTradeInfoDict.ContainsKey(a_eSkillKinds);
	}

	/** 판매 스킬 교환 정보를 반환한다 */
	public bool TryGetSaleSkillTradeInfo(ESkillKinds a_eSkillKinds, out STSkillTradeInfo a_stOutSkillTradeInfo) {
		a_stOutSkillTradeInfo = this.SaleSkillTradeInfoDict.GetValueOrDefault(a_eSkillKinds, STSkillTradeInfo.INVALID);
		return this.SaleSkillTradeInfoDict.ContainsKey(a_eSkillKinds);
	}

	/** 스킬 정보를 로드한다 */
	public (Dictionary<ESkillKinds, STSkillInfo>, Dictionary<ESkillKinds, STSkillEnhanceInfo>, Dictionary<ESkillKinds, STSkillTradeInfo>, Dictionary<ESkillKinds, STSkillTradeInfo>) LoadSkillInfos() {
		this.ResetSkillInfos();
		return this.LoadSkillInfos(Access.SkillInfoTableLoadPath);
	}

	/** 스킬 정보를 저장한다 */
	public void SaveSkillInfos(string a_oJSONStr, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oJSONStr != null);

		// JSON 문자열이 존재 할 경우
		if(a_oJSONStr != null) {
			this.ResetSkillInfos(a_oJSONStr);
			
#if UNITY_EDITOR || (UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD))
			CFunc.WriteStr(Access.SkillInfoTableSavePath, a_oJSONStr, false);
#else
			CFunc.WriteStr(Access.SkillInfoTableSavePath, a_oJSONStr, true);
#endif			// #if UNITY_EDITOR || (UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD))
		}
	}

	/** JSON 노드를 설정한다 */
	private void SetupJSONNodes(SimpleJSON.JSONNode a_oJSONNode, out List<SimpleJSON.JSONNode> a_oOutSkillInfosList, out List<SimpleJSON.JSONNode> a_oOutSkillEnhanceInfosList, out List<SimpleJSON.JSONNode> a_oOutBuySkillTradeInfosList, out List<SimpleJSON.JSONNode> a_oOutSaleSkillTradeInfosList) {
		a_oOutSkillInfosList = new List<SimpleJSON.JSONNode>();
		a_oOutSkillEnhanceInfosList = new List<SimpleJSON.JSONNode>();
		a_oOutBuySkillTradeInfosList = new List<SimpleJSON.JSONNode>();
		a_oOutSaleSkillTradeInfosList = new List<SimpleJSON.JSONNode>();

		var oTableInfoDictContainer = KDefine.G_TABLE_INFO_DICT_CONTAINER[Path.GetFileNameWithoutExtension(Access.SkillInfoTableLoadPath)];

		for(int i = 0; i < oTableInfoDictContainer.Item2[this.GetType()][KCDefine.B_KEY_COMMON].Count; ++i) {
			a_oOutSkillInfosList.ExAddVal(a_oJSONNode[oTableInfoDictContainer.Item2[this.GetType()][KCDefine.B_KEY_COMMON][i]]);
		}

		for(int i = 0; i < oTableInfoDictContainer.Item2[this.GetType()][KCDefine.B_KEY_COMMON].Count; ++i) {
			a_oOutSkillEnhanceInfosList.ExAddVal(a_oJSONNode[oTableInfoDictContainer.Item2[this.GetType()][KCDefine.B_KEY_COMMON][i]]);
		}

		for(int i = 0; i < oTableInfoDictContainer.Item2[this.GetType()][KCDefine.B_KEY_COMMON].Count; ++i) {
			a_oOutBuySkillTradeInfosList.ExAddVal(a_oJSONNode[oTableInfoDictContainer.Item2[this.GetType()][KCDefine.B_KEY_COMMON][i]]);
		}

		for(int i = 0; i < oTableInfoDictContainer.Item2[this.GetType()][KCDefine.B_KEY_COMMON].Count; ++i) {
			a_oOutSaleSkillTradeInfosList.ExAddVal(a_oJSONNode[oTableInfoDictContainer.Item2[this.GetType()][KCDefine.B_KEY_COMMON][i]]);
		}
	}

	/** 스킬 정보를 로드한다 */
	private (Dictionary<ESkillKinds, STSkillInfo>, Dictionary<ESkillKinds, STSkillEnhanceInfo>, Dictionary<ESkillKinds, STSkillTradeInfo>, Dictionary<ESkillKinds, STSkillTradeInfo>) LoadSkillInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_EDITOR || (UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD))
		return this.DoLoadSkillInfos(File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, false) : CFunc.ReadStrFromRes(a_oFilePath, false));
#else
		return this.DoLoadSkillInfos(File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, true) : CFunc.ReadStrFromRes(a_oFilePath, true));
#endif			// #if UNITY_EDITOR || (UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD))
	}

	/** 스킬 정보를 로드한다 */
	private (Dictionary<ESkillKinds, STSkillInfo>, Dictionary<ESkillKinds, STSkillEnhanceInfo>, Dictionary<ESkillKinds, STSkillTradeInfo>, Dictionary<ESkillKinds, STSkillTradeInfo>) DoLoadSkillInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		this.SetupJSONNodes(SimpleJSON.JSONNode.Parse(a_oJSONStr), out List<SimpleJSON.JSONNode> oSkillInfosList, out List<SimpleJSON.JSONNode> oSkillEnhanceInfosList, out List<SimpleJSON.JSONNode> oBuySkillTradeInfosList, out List<SimpleJSON.JSONNode> oSaleSkillTradeInfosList);
		
		for(int i = 0; i < oSkillInfosList.Count; ++i) {
			for(int j = 0; j < oSkillInfosList[i].Count; ++j) {
				var stSkillInfo = new STSkillInfo(oSkillInfosList[i][j]);

				// 스킬 정보 추가 가능 할 경우
				if(stSkillInfo.m_eSkillKinds.ExIsValid() && (!this.SkillInfoDict.ContainsKey(stSkillInfo.m_eSkillKinds) || oSkillInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.SkillInfoDict.ExReplaceVal(stSkillInfo.m_eSkillKinds, stSkillInfo);
				}
			}
		}

		for(int i = 0; i < oSkillEnhanceInfosList.Count; ++i) {
			for(int j = 0; j < oSkillEnhanceInfosList[i].Count; ++j) {
				var stSkillEnhanceInfo = new STSkillEnhanceInfo(oSkillEnhanceInfosList[i][j]);

				// 스킬 강화 정보 추가 가능 할 경우
				if(stSkillEnhanceInfo.m_eSkillKinds.ExIsValid() && (!this.BuySkillTradeInfoDict.ContainsKey(stSkillEnhanceInfo.m_eSkillKinds) || oSkillEnhanceInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.SkillEnhanceInfoDict.ExReplaceVal(stSkillEnhanceInfo.m_eSkillKinds, stSkillEnhanceInfo);
				}
			}
		}

		for(int i = 0; i < oBuySkillTradeInfosList.Count; ++i) {
			for(int j = 0; j < oBuySkillTradeInfosList[i].Count; ++j) {
				var stSkillTradeInfo = new STSkillTradeInfo(oBuySkillTradeInfosList[i][j]);

				// 구입 스킬 교환 정보 추가 가능 할 경우
				if(stSkillTradeInfo.m_eSkillKinds.ExIsValid() && (!this.BuySkillTradeInfoDict.ContainsKey(stSkillTradeInfo.m_eSkillKinds) || oBuySkillTradeInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.BuySkillTradeInfoDict.ExReplaceVal(stSkillTradeInfo.m_eSkillKinds, stSkillTradeInfo);
				}
			}
		}

		for(int i = 0; i < oSaleSkillTradeInfosList.Count; ++i) {
			for(int j = 0; j < oSaleSkillTradeInfosList[i].Count; ++j) {
				var stSkillTradeInfo = new STSkillTradeInfo(oSaleSkillTradeInfosList[i][j]);

				// 판매 스킬 교환 정보 추가 가능 할 경우
				if(stSkillTradeInfo.m_eSkillKinds.ExIsValid() && (!this.SaleSkillTradeInfoDict.ContainsKey(stSkillTradeInfo.m_eSkillKinds) || oSaleSkillTradeInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.SaleSkillTradeInfoDict.ExReplaceVal(stSkillTradeInfo.m_eSkillKinds, stSkillTradeInfo);
				}
			}
		}

		return (this.SkillInfoDict, this.SkillEnhanceInfoDict, this.BuySkillTradeInfoDict, this.SaleSkillTradeInfoDict);
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
