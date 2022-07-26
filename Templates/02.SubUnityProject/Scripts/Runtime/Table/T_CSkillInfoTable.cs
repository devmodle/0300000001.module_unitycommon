using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 스킬 정보 */
[System.Serializable]
public partial struct STSkillInfo {
	public STCommonInfo m_stCommonInfo;
	public STDurationInfo m_stDurationInfo;

	public ESkillKinds m_eSkillKinds;
	public ESkillKinds m_ePrevSkillKinds;
	public ESkillKinds m_eNextSkillKinds;

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
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STSkillInfo(SimpleJSON.JSONNode a_oSkillInfo) {
		m_stCommonInfo = new STCommonInfo(a_oSkillInfo);
		m_stDurationInfo = new STDurationInfo(a_oSkillInfo[KCDefine.U_KEY_DURATION_INFO]);

		m_eSkillKinds = a_oSkillInfo[KCDefine.U_KEY_SKILL_KINDS].ExIsValid() ? (ESkillKinds)a_oSkillInfo[KCDefine.U_KEY_SKILL_KINDS].AsInt : ESkillKinds.NONE;
		m_ePrevSkillKinds = a_oSkillInfo[KCDefine.U_KEY_PREV_SKILL_KINDS].ExIsValid() ? (ESkillKinds)a_oSkillInfo[KCDefine.U_KEY_PREV_SKILL_KINDS].AsInt : ESkillKinds.NONE;
		m_eNextSkillKinds = a_oSkillInfo[KCDefine.U_KEY_NEXT_SKILL_KINDS].ExIsValid() ? (ESkillKinds)a_oSkillInfo[KCDefine.U_KEY_NEXT_SKILL_KINDS].AsInt : ESkillKinds.NONE;

		m_oFXKindsList = new List<EFXKinds>();
		m_oResKindsList = new List<EResKinds>();

		m_oAbilityTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_FX_KINDS; ++i) {
			string oKey = string.Format(KCDefine.U_KEY_FMT_FX_KINDS, i + KCDefine.B_VAL_1_INT);
			m_oFXKindsList.ExAddVal(a_oSkillInfo[oKey].ExIsValid() ? (EFXKinds)a_oSkillInfo[oKey].AsInt : EFXKinds.NONE);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_RES_KINDS; ++i) {
			string oKey = string.Format(KCDefine.U_KEY_FMT_RES_KINDS, i + KCDefine.B_VAL_1_INT);
			m_oResKindsList.ExAddVal(a_oSkillInfo[oKey].ExIsValid() ? (EResKinds)a_oSkillInfo[oKey].AsInt : EResKinds.NONE);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oSkillInfo[string.Format(KCDefine.U_KEY_FMT_ABILITY_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oAbilityTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}
	}
	#endregion			// 함수
}

/** 스킬 판매 정보 */
[System.Serializable]
public partial struct STSkillSaleInfo {
	public STCommonInfo m_stCommonInfo;

	public ESkillKinds m_eSkillKinds;
	public ESkillKinds m_ePrevSkillKinds;
	public ESkillKinds m_eNextSkillKinds;

	public Dictionary<ulong, STTargetInfo> m_oPayTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAcquireTargetInfoDict;

	#region 상수
	public static STSkillSaleInfo INVALID = new STSkillSaleInfo() {
		m_eSkillKinds = ESkillKinds.NONE, m_ePrevSkillKinds = ESkillKinds.NONE, m_eNextSkillKinds = ESkillKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public ESkillType SkillType => (ESkillType)((int)m_eSkillKinds).ExKindsToType();
	public ESkillKinds BaseSkillKinds => (ESkillKinds)((int)m_eSkillKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STSkillSaleInfo(SimpleJSON.JSONNode a_oSkillSaleInfo) {
		m_stCommonInfo = new STCommonInfo(a_oSkillSaleInfo);

		m_eSkillKinds = a_oSkillSaleInfo[KCDefine.U_KEY_SKILL_KINDS].ExIsValid() ? (ESkillKinds)a_oSkillSaleInfo[KCDefine.U_KEY_SKILL_KINDS].AsInt : ESkillKinds.NONE;
		m_ePrevSkillKinds = a_oSkillSaleInfo[KCDefine.U_KEY_PREV_SKILL_KINDS].ExIsValid() ? (ESkillKinds)a_oSkillSaleInfo[KCDefine.U_KEY_PREV_SKILL_KINDS].AsInt : ESkillKinds.NONE;
		m_eNextSkillKinds = a_oSkillSaleInfo[KCDefine.U_KEY_NEXT_SKILL_KINDS].ExIsValid() ? (ESkillKinds)a_oSkillSaleInfo[KCDefine.U_KEY_NEXT_SKILL_KINDS].AsInt : ESkillKinds.NONE;

		m_oPayTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oAcquireTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oSkillSaleInfo[string.Format(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oPayTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oSkillSaleInfo[string.Format(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oAcquireTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}
	}
	#endregion			// 함수
}

/** 스킬 강화 정보 */
[System.Serializable]
public partial struct STSkillEnhanceInfo {
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
			m_oPayTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oSkillEnhanceInfo[string.Format(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oAcquireTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}
	}
	#endregion			// 함수
}

/** 스킬 정보 테이블 */
public partial class CSkillInfoTable : CSingleton<CSkillInfoTable> {
	#region 변수
	[Header("=====> Action Skill Info <=====")]
	[SerializeField] private List<STSkillInfo> m_oActionSkillInfoList = new List<STSkillInfo>();
	[SerializeField] private List<STSkillSaleInfo> m_oActionSkillSaleInfoList = new List<STSkillSaleInfo>();
	[SerializeField] private List<STSkillEnhanceInfo> m_oActionSkillEnhanceInfoList = new List<STSkillEnhanceInfo>();

	[Header("=====> Active Skill Info <=====")]
	[SerializeField] private List<STSkillInfo> m_oActiveSkillInfoList = new List<STSkillInfo>();
	[SerializeField] private List<STSkillSaleInfo> m_oActiveSkillSaleInfoList = new List<STSkillSaleInfo>();
	[SerializeField] private List<STSkillEnhanceInfo> m_oActiveSkillEnhanceInfoList = new List<STSkillEnhanceInfo>();

	[Header("=====> Passive Skill Info <=====")]
	[SerializeField] private List<STSkillInfo> m_oPassiveSkillInfoList = new List<STSkillInfo>();
	[SerializeField] private List<STSkillSaleInfo> m_oPassiveSkillSaleInfoList = new List<STSkillSaleInfo>();
	[SerializeField] private List<STSkillEnhanceInfo> m_oPassiveSkillEnhanceInfoList = new List<STSkillEnhanceInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<ESkillKinds, STSkillInfo> SkillInfoDict { get; private set; } = new Dictionary<ESkillKinds, STSkillInfo>();
	public Dictionary<ESkillKinds, STSkillSaleInfo> SkillSaleInfoDict { get; private set; } = new Dictionary<ESkillKinds, STSkillSaleInfo>();
	public Dictionary<ESkillKinds, STSkillEnhanceInfo> SkillEnhanceInfoDict { get; private set; } = new Dictionary<ESkillKinds, STSkillEnhanceInfo>();

	private string SkillInfoTablePath {
		get {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_SKILL_INFO;
#else
			return KCDefine.U_TABLE_P_G_SKILL_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetSkillInfos();
	}

	/** 스킬 정보를 리셋한다 */
	public void ResetSkillInfos() {
		this.SkillInfoDict.Clear();
		this.SkillSaleInfoDict.Clear();
		this.SkillEnhanceInfoDict.Clear();

		var oSkillInfoList = new List<STSkillInfo>(m_oActionSkillInfoList);
		oSkillInfoList.ExAddVals(m_oActiveSkillInfoList);
		oSkillInfoList.ExAddVals(m_oPassiveSkillInfoList);

		var oSkillSaleInfoList = new List<STSkillSaleInfo>(m_oActionSkillSaleInfoList);
		oSkillSaleInfoList.ExAddVals(m_oActiveSkillSaleInfoList);
		oSkillSaleInfoList.ExAddVals(m_oPassiveSkillSaleInfoList);

		var oSkillEnhanceInfoList = new List<STSkillEnhanceInfo>(m_oActionSkillEnhanceInfoList);
		oSkillEnhanceInfoList.ExAddVals(m_oActiveSkillEnhanceInfoList);
		oSkillEnhanceInfoList.ExAddVals(m_oPassiveSkillEnhanceInfoList);

		for(int i = 0; i < oSkillInfoList.Count; ++i) {
			this.SkillInfoDict.TryAdd(oSkillInfoList[i].m_eSkillKinds, oSkillInfoList[i]);
		}

		for(int i = 0; i < oSkillSaleInfoList.Count; ++i) {
			this.SkillSaleInfoDict.TryAdd(oSkillSaleInfoList[i].m_eSkillKinds, oSkillSaleInfoList[i]);
		}

		for(int i = 0; i < oSkillEnhanceInfoList.Count; ++i) {
			this.SkillEnhanceInfoDict.TryAdd(oSkillEnhanceInfoList[i].m_eSkillKinds, oSkillEnhanceInfoList[i]);
		}
	}

	/** 스킬 정보를 리셋한다 */
	public void ResetSkillInfos(string a_oJSONStr) {
		this.ResetSkillInfos();
		this.DoLoadSkillInfos(a_oJSONStr);
	}

	/** 스킬 정보를 반환한다 */
	public STSkillInfo GetSkillInfo(ESkillKinds a_ESkillKinds) {
		bool bIsValid = this.TryGetSkillInfo(a_ESkillKinds, out STSkillInfo stSkillInfo);
		CAccess.Assert(bIsValid);

		return stSkillInfo;
	}

	/** 스킬 판매 정보를 반환한다 */
	public STSkillSaleInfo GetSkillSaleInfo(ESkillKinds a_eSkillKinds) {
		bool bIsValid = this.TryGetSkillSaleInfo(a_eSkillKinds, out STSkillSaleInfo stSkillSaleInfo);
		CAccess.Assert(bIsValid);

		return stSkillSaleInfo;
	}

	/** 스킬 강화 정보를 반환한다 */
	public STSkillEnhanceInfo GetSkillEnhanceInfo(ESkillKinds a_eSkillKinds) {
		bool bIsValid = this.TryGetSkillEnhanceInfo(a_eSkillKinds, out STSkillEnhanceInfo stSkillEnhanceInfo);
		CAccess.Assert(bIsValid);

		return stSkillEnhanceInfo;
	}

	/** 스킬 정보를 반환한다 */
	public bool TryGetSkillInfo(ESkillKinds a_ESkillKinds, out STSkillInfo a_stOutSkillInfo) {
		a_stOutSkillInfo = this.SkillInfoDict.GetValueOrDefault(a_ESkillKinds, STSkillInfo.INVALID);
		return this.SkillInfoDict.ContainsKey(a_ESkillKinds);
	}

	/** 스킬 판매 정보를 반환한다 */
	public bool TryGetSkillSaleInfo(ESkillKinds a_eSkillKinds, out STSkillSaleInfo a_stOutSkillSaleInfo) {
		a_stOutSkillSaleInfo = this.SkillSaleInfoDict.GetValueOrDefault(a_eSkillKinds, STSkillSaleInfo.INVALID);
		return this.SkillSaleInfoDict.ContainsKey(a_eSkillKinds);
	}

	/** 스킬 강화 정보를 반환한다 */
	public bool TryGetSkillEnhanceInfo(ESkillKinds a_eSkillKinds, out STSkillEnhanceInfo a_stOutSkillEnhanceInfo) {
		a_stOutSkillEnhanceInfo = this.SkillEnhanceInfoDict.GetValueOrDefault(a_eSkillKinds, STSkillEnhanceInfo.INVALID);
		return this.SkillEnhanceInfoDict.ContainsKey(a_eSkillKinds);
	}

	/** 스킬 정보를 로드한다 */
	public (Dictionary<ESkillKinds, STSkillInfo>, Dictionary<ESkillKinds, STSkillSaleInfo>, Dictionary<ESkillKinds, STSkillEnhanceInfo>) LoadSkillInfos() {
		this.ResetSkillInfos();
		return this.LoadSkillInfos(this.SkillInfoTablePath);
	}

	/** 스킬 정보를 로드한다 */
	private (Dictionary<ESkillKinds, STSkillInfo>, Dictionary<ESkillKinds, STSkillSaleInfo>, Dictionary<ESkillKinds, STSkillEnhanceInfo>) LoadSkillInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadSkillInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			return this.DoLoadSkillInfos(CResManager.Inst.GetRes<TextAsset>(a_oFilePath).text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 스킬 정보를 로드한다 */
	private (Dictionary<ESkillKinds, STSkillInfo>, Dictionary<ESkillKinds, STSkillSaleInfo>, Dictionary<ESkillKinds, STSkillEnhanceInfo>) DoLoadSkillInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());

		var oJSONNode = SimpleJSON.JSONNode.Parse(a_oJSONStr);
		var oSkillInfosList = new List<SimpleJSON.JSONNode>();
		var oSkillSaleInfosList = new List<SimpleJSON.JSONNode>();
		var oSkillEnhanceInfosList = new List<SimpleJSON.JSONNode>();

		for(int i = 0; i < KDefine.G_KEY_SKILL_INFOS.Count; ++i) {
			oSkillInfosList.ExAddVal(oJSONNode[KDefine.G_KEY_SKILL_INFOS[i]]);
		}

		for(int i = 0; i < KDefine.G_KEY_SKILL_SALE_INFOS.Count; ++i) {
			oSkillSaleInfosList.ExAddVal(oJSONNode[KDefine.G_KEY_SKILL_SALE_INFOS[i]]);
		}

		for(int i = 0; i < KDefine.G_KEY_SKILL_ENHANCE_INFOS.Count; ++i) {
			oSkillEnhanceInfosList.ExAddVal(oJSONNode[KDefine.G_KEY_SKILL_ENHANCE_INFOS[i]]);
		}

		for(int i = 0; i < oSkillInfosList.Count; ++i) {
			for(int j = 0; j < oSkillInfosList[i].Count; ++j) {
				var stSkillInfo = new STSkillInfo(oSkillInfosList[i][j]);

				// 스킬 정보 추가 가능 할 경우
				if(stSkillInfo.m_eSkillKinds.ExIsValid() && (!this.SkillInfoDict.ContainsKey(stSkillInfo.m_eSkillKinds) || oSkillInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.SkillInfoDict.ExReplaceVal(stSkillInfo.m_eSkillKinds, stSkillInfo);
				}
			}
		}

		for(int i = 0; i < oSkillSaleInfosList.Count; ++i) {
			for(int j = 0; j < oSkillSaleInfosList[i].Count; ++j) {
				var stSkillSaleInfo = new STSkillSaleInfo(oSkillSaleInfosList[i][j]);

				// 스킬 판매 정보 추가 가능 할 경우
				if(stSkillSaleInfo.m_eSkillKinds.ExIsValid() && (!this.SkillSaleInfoDict.ContainsKey(stSkillSaleInfo.m_eSkillKinds) || oSkillSaleInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.SkillSaleInfoDict.ExReplaceVal(stSkillSaleInfo.m_eSkillKinds, stSkillSaleInfo);
				}
			}
		}

		for(int i = 0; i < oSkillEnhanceInfosList.Count; ++i) {
			for(int j = 0; j < oSkillEnhanceInfosList[i].Count; ++j) {
				var stSkillEnhanceInfo = new STSkillEnhanceInfo(oSkillEnhanceInfosList[i][j]);

				// 스킬 강화 정보 추가 가능 할 경우
				if(stSkillEnhanceInfo.m_eSkillKinds.ExIsValid() && (!this.SkillSaleInfoDict.ContainsKey(stSkillEnhanceInfo.m_eSkillKinds) || oSkillEnhanceInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.SkillEnhanceInfoDict.ExReplaceVal(stSkillEnhanceInfo.m_eSkillKinds, stSkillEnhanceInfo);
				}
			}
		}

		return (this.SkillInfoDict, this.SkillSaleInfoDict, this.SkillEnhanceInfoDict);
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
