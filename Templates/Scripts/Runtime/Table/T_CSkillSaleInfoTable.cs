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

		m_eSkillKinds = a_oSkillSaleInfo[KCDefine.U_KEY_SKILL_SALE_KINDS].ExIsValid() ? (ESkillKinds)a_oSkillSaleInfo[KCDefine.U_KEY_SKILL_SALE_KINDS].AsInt : ESkillKinds.NONE;
		m_ePrevSkillKinds = a_oSkillSaleInfo[KCDefine.U_KEY_PREV_SKILL_SALE_KINDS].ExIsValid() ? (ESkillKinds)a_oSkillSaleInfo[KCDefine.U_KEY_PREV_SKILL_SALE_KINDS].AsInt : ESkillKinds.NONE;
		m_eNextSkillKinds = a_oSkillSaleInfo[KCDefine.U_KEY_NEXT_SKILL_SALE_KINDS].ExIsValid() ? (ESkillKinds)a_oSkillSaleInfo[KCDefine.U_KEY_NEXT_SKILL_SALE_KINDS].AsInt : ESkillKinds.NONE;

		m_oPayTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oAcquireTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oSkillSaleInfo[string.Format(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oPayTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_ABILITY_VAL_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oSkillSaleInfo[string.Format(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oAcquireTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
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
	public Dictionary<ESkillKinds, STSkillSaleInfo> SkillSaleInfoDict { get; private set; } = new Dictionary<ESkillKinds, STSkillSaleInfo>();

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
			this.SkillSaleInfoDict.TryAdd(oSkillSaleInfoList[i].m_eSkillKinds, oSkillSaleInfoList[i]);
		}
	}

	/** 스킬 판매 정보를 리셋한다 */
	public void ResetSkillSaleInfos(string a_oJSONStr) {
		this.ResetSkillSaleInfos();
		this.DoLoadSkillSaleInfos(a_oJSONStr);
	}

	/** 스킬 판매 정보를 반환한다 */
	public STSkillSaleInfo GetSkillSaleInfo(ESkillKinds a_eSkillKinds) {
		bool bIsValid = this.TryGetSkillSaleInfo(a_eSkillKinds, out STSkillSaleInfo stSkillSaleInfo);
		CAccess.Assert(bIsValid);

		return stSkillSaleInfo;
	}

	/** 지불 타겟 정보를 반환한다 */
	public STTargetInfo GetPayTargetInfo(ESkillKinds a_eSkillKinds, ETargetKinds a_eTargetKinds, int a_nKinds) {
		bool bIsValid = this.TryGetPayTargetInfo(a_eSkillKinds, a_eTargetKinds, a_nKinds, out STTargetInfo stPayTargetInfo);
		CAccess.Assert(bIsValid);

		return stPayTargetInfo;
	}

	/** 획득 타겟 정보를 반환한다 */
	public STTargetInfo GetAcquireTargetInfo(ESkillKinds a_eSkillKinds, ETargetKinds a_eTargetKinds, int a_nKinds) {
		bool bIsValid = this.TryGetAcquireTargetInfo(a_eSkillKinds, a_eTargetKinds, a_nKinds, out STTargetInfo stAcquireTargetInfo);
		CAccess.Assert(bIsValid);

		return stAcquireTargetInfo;
	}

	/** 스킬 판매 정보를 반환한다 */
	public bool TryGetSkillSaleInfo(ESkillKinds a_eSkillKinds, out STSkillSaleInfo a_stOutSkillSaleInfo) {
		a_stOutSkillSaleInfo = this.SkillSaleInfoDict.GetValueOrDefault(a_eSkillKinds, default(STSkillSaleInfo));
		return this.SkillSaleInfoDict.ContainsKey(a_eSkillKinds);
	}

	/** 지불 타겟 정보를 반환한다 */
	public bool TryGetPayTargetInfo(ESkillKinds a_eSkillKinds, ETargetKinds a_eTargetKinds, int a_nKinds, out STTargetInfo a_stOutPayTargetInfo) {
		a_stOutPayTargetInfo = this.TryGetSkillSaleInfo(a_eSkillKinds, out STSkillSaleInfo stSkillSaleInfo) ? stSkillSaleInfo.m_oPayTargetInfoDict.GetValueOrDefault(Factory.MakeUniqueTargetInfoID(a_eTargetKinds, a_nKinds), STTargetInfo.INVALID) : STTargetInfo.INVALID;
		return !a_stOutPayTargetInfo.Equals(STTargetInfo.INVALID);
	}

	/** 획득 타겟 정보를 반환한다 */
	public bool TryGetAcquireTargetInfo(ESkillKinds a_eSkillKinds, ETargetKinds a_eTargetKinds, int a_nKinds, out STTargetInfo a_stOutAcquireTargetInfo) {
		a_stOutAcquireTargetInfo = this.TryGetSkillSaleInfo(a_eSkillKinds, out STSkillSaleInfo stSkillSaleInfo) ? stSkillSaleInfo.m_oAcquireTargetInfoDict.GetValueOrDefault(Factory.MakeUniqueTargetInfoID(a_eTargetKinds, a_nKinds), STTargetInfo.INVALID) : STTargetInfo.INVALID;
		return !a_stOutAcquireTargetInfo.Equals(STTargetInfo.INVALID);
	}

	/** 스킬 판매 정보를 로드한다 */
	public Dictionary<ESkillKinds, STSkillSaleInfo> LoadSkillSaleInfos() {
		this.ResetSkillSaleInfos();
		return this.LoadSkillSaleInfos(this.SkillSaleInfoTablePath);
	}

	/** 스킬 판매 정보를 로드한다 */
	private Dictionary<ESkillKinds, STSkillSaleInfo> LoadSkillSaleInfos(string a_oFilePath) {
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
	private Dictionary<ESkillKinds, STSkillSaleInfo> DoLoadSkillSaleInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr) as SimpleJSON.JSONClass;

		var oSkillSaleInfosList = new List<SimpleJSON.JSONNode>() {
			oJSONNode[KCDefine.U_KEY_GOODS], oJSONNode[KCDefine.U_KEY_CONSUMABLE], oJSONNode[KCDefine.U_KEY_NON_CONSUMABLE], oJSONNode[KCDefine.U_KEY_WEAPON], oJSONNode[KCDefine.U_KEY_ARMOR], oJSONNode[KCDefine.U_KEY_ACCESSORY]
		};

		for(int i = 0; i < oSkillSaleInfosList.Count; ++i) {
			for(int j = 0; j < oSkillSaleInfosList[i].Count; ++j) {
				var stSkillSaleInfo = new STSkillSaleInfo(oSkillSaleInfosList[i][j]);

				// 스킬 판매 정보가 추가 가능 할 경우
				if(stSkillSaleInfo.m_eSkillKinds.ExIsValid() && (!this.SkillSaleInfoDict.ContainsKey(stSkillSaleInfo.m_eSkillKinds) || oSkillSaleInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.SkillSaleInfoDict.ExReplaceVal(stSkillSaleInfo.m_eSkillKinds, stSkillSaleInfo);
				}
			}
		}

		return this.SkillSaleInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
