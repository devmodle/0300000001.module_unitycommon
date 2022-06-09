using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 스킬 판매 정보 */
[System.Serializable]
public struct STSkillSaleInfo {
	public STDescInfo m_stDescInfo;
	public ESkillSaleKinds m_eSkillSaleKinds;

	public List<ESkillKinds> m_oSkillKindsList;
	public List<STPriceInfo> m_oPriceInfoList;

	#region 프로퍼티
	public ESkillSaleType SkillSaleType => (ESkillSaleType)((int)m_eSkillSaleKinds).ExKindsToType();
	public ESkillSaleKinds BaseSkillSaleKinds => (ESkillSaleKinds)((int)m_eSkillSaleKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STSkillSaleInfo(SimpleJSON.JSONNode a_oSkillSaleInfo) {
		m_stDescInfo = new STDescInfo(a_oSkillSaleInfo);
		m_eSkillSaleKinds = a_oSkillSaleInfo[KCDefine.U_KEY_SKILL_SALE_KINDS].ExIsValid() ? (ESkillSaleKinds)a_oSkillSaleInfo[KCDefine.U_KEY_SKILL_SALE_KINDS].AsInt : ESkillSaleKinds.NONE;

		m_oSkillKindsList = new List<ESkillKinds>();
		m_oPriceInfoList = new List<STPriceInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_SKILL_KINDS; ++i) {
			string oSkillKindsKey = string.Format(KCDefine.U_KEY_FMT_SKILL_KINDS, i + KCDefine.B_VAL_1_INT);
			m_oSkillKindsList.Add(a_oSkillSaleInfo[oSkillKindsKey].ExIsValid() ? (ESkillKinds)a_oSkillSaleInfo[oSkillKindsKey].AsInt : ESkillKinds.NONE);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_PRICE_INFOS; ++i) {
			string oPriceKey = string.Format(KCDefine.U_KEY_FMT_PRICE, i + KCDefine.B_VAL_1_INT);
			string oPriceKindsKey = string.Format(KCDefine.U_KEY_FMT_PRICE_KINDS, i + KCDefine.B_VAL_1_INT);

			m_oPriceInfoList.Add(new STPriceInfo() {
				m_oPrice = a_oSkillSaleInfo[oPriceKey].ExIsValid() ? a_oSkillSaleInfo[oPriceKey] : KCDefine.B_STR_0_INT, m_ePriceKinds = a_oSkillSaleInfo[oPriceKindsKey].ExIsValid() ? (EPriceKinds)a_oSkillSaleInfo[oPriceKindsKey].AsInt : EPriceKinds.NONE
			});
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

		var oSkillSaleInfoList = new List<STSkillSaleInfo>(m_oActiveSkillSaleInfoList);
		oSkillSaleInfoList.ExAddVals(m_oPassiveSkillSaleInfoList);

		for(int i = 0; i < oSkillSaleInfoList.Count; ++i) {
			this.SkillSaleInfoDict.TryAdd(oSkillSaleInfoList[i].m_eSkillSaleKinds, oSkillSaleInfoList[i]);
		}
	}

	/** 스킬 판매 정보를 반환한다 */
	public STSkillSaleInfo GetSkillSaleInfo(ESkillSaleKinds a_eSkillSaleKinds) {
		bool bIsValid = this.TryGetSkillSaleInfo(a_eSkillSaleKinds, out STSkillSaleInfo stSkillSaleInfo);
		CAccess.Assert(bIsValid);

		return stSkillSaleInfo;
	}

	/** 가격 정보를 반환한다 */
	public STPriceInfo GetPriceInfo(ESkillSaleKinds a_eSkillSaleKinds, EPriceKinds a_ePriceKinds) {
		bool bIsValid = this.TryGetPriceInfo(a_eSkillSaleKinds, a_ePriceKinds, out STPriceInfo stPriceInfo);
		CAccess.Assert(bIsValid);

		return stPriceInfo;
	}

	/** 스킬 판매 정보를 반환한다 */
	public bool TryGetSkillSaleInfo(ESkillSaleKinds a_eSkillSaleKinds, out STSkillSaleInfo a_stOutSkillSaleInfo) {
		a_stOutSkillSaleInfo = this.SkillSaleInfoDict.GetValueOrDefault(a_eSkillSaleKinds, default(STSkillSaleInfo));
		return this.SkillSaleInfoDict.ContainsKey(a_eSkillSaleKinds);
	}

	/** 가격 정보를 반환한다 */
	public bool TryGetPriceInfo(ESkillSaleKinds a_eSkillSaleKinds, EPriceKinds a_ePriceKinds, out STPriceInfo a_stOutPriceInfo) {
		// 아이템 판매 정보가 존재 할 경우
		if(this.TryGetSkillSaleInfo(a_eSkillSaleKinds, out STSkillSaleInfo stSkillSaleInfo)) {
			return stSkillSaleInfo.m_oPriceInfoList.ExTryGetPriceInfo(a_ePriceKinds, out a_stOutPriceInfo);
		}

		a_stOutPriceInfo = default(STPriceInfo);
		return false;
	}

	/** 스킬 판매 정보를 로드한다 */
	public Dictionary<ESkillSaleKinds, STSkillSaleInfo> LoadSkillSaleInfos() {
		return this.LoadSkillSaleInfos(this.SkillSaleInfoTablePath);
	}

	/** 스킬 판매 정보를 로드한다 */
	private Dictionary<ESkillSaleKinds, STSkillSaleInfo> LoadSkillSaleInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadSkillSaleInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			var oTextAsset = CResManager.Inst.GetRes<TextAsset>(a_oFilePath);
			return this.DoLoadSkillSaleInfos(oTextAsset.text);
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
				if(!this.SkillSaleInfoDict.ContainsKey(stSkillSaleInfo.m_eSkillSaleKinds) || oSkillSaleInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT) {
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
