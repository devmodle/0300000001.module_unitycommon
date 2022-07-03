using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 스킬 판매 정보 */
[System.Serializable]
public partial struct STSkillSaleInfo {
	public STCommonInfo m_stCommonInfo;

	public ESkillSaleKinds m_eSkillSaleKinds;
	public ESkillSaleKinds m_ePrevSkillSaleKinds;
	public ESkillSaleKinds m_eNextSkillSaleKinds;

	public List<STTargetInfo> m_oPayTargetInfoList;
	public List<STTargetInfo> m_oAcquireTargetInfoList;

	#region 프로퍼티
	public ESkillSaleType SkillSaleType => (ESkillSaleType)((int)m_eSkillSaleKinds).ExKindsToType();
	public ESkillSaleKinds BaseSkillSaleKinds => (ESkillSaleKinds)((int)m_eSkillSaleKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STSkillSaleInfo(SimpleJSON.JSONNode a_oSkillSaleInfo) {
		m_stCommonInfo = new STCommonInfo(a_oSkillSaleInfo);

		m_eSkillSaleKinds = a_oSkillSaleInfo[KCDefine.U_KEY_SKILL_SALE_KINDS].ExIsValid() ? (ESkillSaleKinds)a_oSkillSaleInfo[KCDefine.U_KEY_SKILL_SALE_KINDS].AsInt : ESkillSaleKinds.NONE;
		m_ePrevSkillSaleKinds = a_oSkillSaleInfo[KCDefine.U_KEY_PREV_SKILL_SALE_KINDS].ExIsValid() ? (ESkillSaleKinds)a_oSkillSaleInfo[KCDefine.U_KEY_PREV_SKILL_SALE_KINDS].AsInt : ESkillSaleKinds.NONE;
		m_eNextSkillSaleKinds = a_oSkillSaleInfo[KCDefine.U_KEY_NEXT_SKILL_SALE_KINDS].ExIsValid() ? (ESkillSaleKinds)a_oSkillSaleInfo[KCDefine.U_KEY_NEXT_SKILL_SALE_KINDS].AsInt : ESkillSaleKinds.NONE;

		m_oPayTargetInfoList = new List<STTargetInfo>();
		m_oAcquireTargetInfoList = new List<STTargetInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_PRICE_INFOS; ++i) {
			m_oPayTargetInfoList.Add(new STTargetInfo(a_oSkillSaleInfo, KCDefine.U_PREFIX_PAY, i));
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_ITEMS_INFOS; ++i) {
			m_oAcquireTargetInfoList.Add(new STTargetInfo(a_oSkillSaleInfo, KCDefine.U_PREFIX_ACQUIRE, i));
		}
	}
	#endregion			// 함수
}

/** 스킬 판매 정보 테이블 */
public partial class CSkillSaleInfoTable : CScriptableObj<CSkillSaleInfoTable> {
	#region 변수
	[Header("=====> Active Skill Sale Info <=====")]
	[SerializeField] private List<STSkillSaleInfo> m_oActiveSkillSaleInfoList = new List<STSkillSaleInfo>();

	[Header("=====> Passive Skill Sale Info <=====")]
	[SerializeField] private List<STSkillSaleInfo> m_oPassiveSkillSaleInfoList = new List<STSkillSaleInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<ESkillSaleKinds, STSkillSaleInfo> SkillSaleInfoDict { get; private set; } = new Dictionary<ESkillSaleKinds, STSkillSaleInfo>();

	private string SkillSaleInfoTablePath {
		get {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_SKILL_SALE_INFO;
#else
			return KCDefine.U_TABLE_P_G_SKILL_SALE_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetSkillSaleInfos();
	}

	/** 스킬 판매 정보를 리셋한다 */
	public void ResetSkillSaleInfos() {
		this.SkillSaleInfoDict.Clear();

		var oSkillSaleInfoList = new List<STSkillSaleInfo>(m_oActiveSkillSaleInfoList);
		oSkillSaleInfoList.ExAddVals(m_oPassiveSkillSaleInfoList);

		for(int i = 0; i < oSkillSaleInfoList.Count; ++i) {
			this.SkillSaleInfoDict.TryAdd(oSkillSaleInfoList[i].m_eSkillSaleKinds, oSkillSaleInfoList[i]);
		}
	}

	/** 스킬 판매 정보를 리셋한다 */
	public void ResetSkillSaleInfos(string a_oJSONStr) {
		this.ResetSkillSaleInfos();
		this.DoLoadSkillSaleInfos(a_oJSONStr);
	}

	/** 스킬 판매 정보를 반환한다 */
	public STSkillSaleInfo GetSkillSaleInfo(ESkillSaleKinds a_eSkillSaleKinds) {
		bool bIsValid = this.TryGetSkillSaleInfo(a_eSkillSaleKinds, out STSkillSaleInfo stSkillSaleInfo);
		CAccess.Assert(bIsValid);

		return stSkillSaleInfo;
	}

	/** 지불 타겟 정보를 반환한다 */
	public STTargetInfo GetPayTargetInfo(ESkillSaleKinds a_eSkillSaleKinds, ETargetKinds a_eTargetKinds, int a_nKinds) {
		bool bIsValid = this.TryGetPayTargetInfo(a_eSkillSaleKinds, a_eTargetKinds, a_nKinds, out STTargetInfo stPayTargetInfo);
		CAccess.Assert(bIsValid);

		return stPayTargetInfo;
	}

	/** 획득 타겟 정보를 반환한다 */
	public STTargetInfo GetAcquireTargetInfo(ESkillSaleKinds a_eSkillSaleKinds, ETargetKinds a_eTargetKinds, int a_nKinds) {
		bool bIsValid = this.TryGetAcquireTargetInfo(a_eSkillSaleKinds, a_eTargetKinds, a_nKinds, out STTargetInfo stAcquireTargetInfo);
		CAccess.Assert(bIsValid);

		return stAcquireTargetInfo;
	}

	/** 스킬 판매 정보를 반환한다 */
	public bool TryGetSkillSaleInfo(ESkillSaleKinds a_eSkillSaleKinds, out STSkillSaleInfo a_stOutSkillSaleInfo) {
		a_stOutSkillSaleInfo = this.SkillSaleInfoDict.GetValueOrDefault(a_eSkillSaleKinds, default(STSkillSaleInfo));
		return this.SkillSaleInfoDict.ContainsKey(a_eSkillSaleKinds);
	}

	/** 지불 타겟 정보를 반환한다 */
	public bool TryGetPayTargetInfo(ESkillSaleKinds a_eSkillSaleKinds, ETargetKinds a_eTargetKinds, int a_nKinds, out STTargetInfo a_stOutPayTargetInfo) {
		// 스킬 판매 정보가 존재 할 경우
		if(this.TryGetSkillSaleInfo(a_eSkillSaleKinds, out STSkillSaleInfo stSkillSaleInfo)) {
			return stSkillSaleInfo.m_oPayTargetInfoList.ExTryGetTargetInfo(a_eTargetKinds, a_nKinds, out a_stOutPayTargetInfo);
		}

		a_stOutPayTargetInfo = default(STTargetInfo);
		return false;
	}

	/** 획득 타겟 정보를 반환한다 */
	public bool TryGetAcquireTargetInfo(ESkillSaleKinds a_eSkillSaleKinds, ETargetKinds a_eTargetKinds, int a_nKinds, out STTargetInfo a_stOutAcquireTargetInfo) {
		// 스킬 판매 정보가 존재 할 경우
		if(this.TryGetSkillSaleInfo(a_eSkillSaleKinds, out STSkillSaleInfo stSkillSaleInfo)) {
			return stSkillSaleInfo.m_oAcquireTargetInfoList.ExTryGetTargetInfo(a_eTargetKinds, a_nKinds, out a_stOutAcquireTargetInfo);
		}

		a_stOutAcquireTargetInfo = default(STTargetInfo);
		return false;
	}

	/** 스킬 판매 정보를 로드한다 */
	public Dictionary<ESkillSaleKinds, STSkillSaleInfo> LoadSkillSaleInfos() {
		this.ResetSkillSaleInfos();
		return this.LoadSkillSaleInfos(this.SkillSaleInfoTablePath);
	}

	/** 스킬 판매 정보를 로드한다 */
	private Dictionary<ESkillSaleKinds, STSkillSaleInfo> LoadSkillSaleInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadSkillSaleInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			return this.DoLoadSkillSaleInfos(CResManager.Inst.GetRes<TextAsset>(a_oFilePath).text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 스킬 판매 정보를 로드한다 */
	private Dictionary<ESkillSaleKinds, STSkillSaleInfo> DoLoadSkillSaleInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr) as SimpleJSON.JSONClass;

		var oSkillSaleInfosList = new List<SimpleJSON.JSONNode>() {
			oJSONNode[KCDefine.U_KEY_GOODS], oJSONNode[KCDefine.U_KEY_CONSUMABLE], oJSONNode[KCDefine.U_KEY_NON_CONSUMABLE], oJSONNode[KCDefine.U_KEY_WEAPON], oJSONNode[KCDefine.U_KEY_ARMOR], oJSONNode[KCDefine.U_KEY_ACCESSORY]
		};

		for(int i = 0; i < oSkillSaleInfosList.Count; ++i) {
			for(int j = 0; j < oSkillSaleInfosList[i].Count; ++j) {
				var stSkillSaleInfo = new STSkillSaleInfo(oSkillSaleInfosList[i][j]);

				// 스킬 판매 정보가 추가 가능 할 경우
				if(stSkillSaleInfo.m_eSkillSaleKinds.ExIsValid() && (!this.SkillSaleInfoDict.ContainsKey(stSkillSaleInfo.m_eSkillSaleKinds) || oSkillSaleInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.SkillSaleInfoDict.ExReplaceVal(stSkillSaleInfo.m_eSkillSaleKinds, stSkillSaleInfo);
				}
			}
		}

		return this.SkillSaleInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
