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

	public Dictionary<EAbilityKinds, STAbilityValInfo> m_oAbilityValInfoDict;

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

		m_oAbilityValInfoDict = new Dictionary<EAbilityKinds, STAbilityValInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_FX_KINDS; ++i) {
			string oFXKindsKey = string.Format(KCDefine.U_KEY_FMT_FX_KINDS, i + KCDefine.B_VAL_1_INT);
			m_oFXKindsList.ExAddVal(a_oSkillInfo[oFXKindsKey].ExIsValid() ? (EFXKinds)a_oSkillInfo[oFXKindsKey].AsInt : EFXKinds.NONE);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_RES_KINDS; ++i) {
			string oResKindsKey = string.Format(KCDefine.U_KEY_FMT_RES_KINDS, i + KCDefine.B_VAL_1_INT);
			m_oResKindsList.ExAddVal(a_oSkillInfo[oResKindsKey].ExIsValid() ? (EResKinds)a_oSkillInfo[oResKindsKey].AsInt : EResKinds.NONE);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_ABILITY_VAL_INFOS; ++i) {
			var stAbilityValInfo = new STAbilityValInfo(a_oSkillInfo[string.Format(KCDefine.U_KEY_FMT_ABILITY_VAL_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oAbilityValInfoDict.TryAdd(stAbilityValInfo.m_eAbilityKinds, stAbilityValInfo);
		}
	}
	#endregion			// 함수
}

/** 스킬 정보 테이블 */
public partial class CSkillInfoTable : CScriptableObj<CSkillInfoTable> {
	#region 변수
	[Header("=====> Active Skill Info <=====")]
	[SerializeField] private List<STSkillInfo> m_oActiveSkillInfoList = new List<STSkillInfo>();

	[Header("=====> Passive Skill Info <=====")]
	[SerializeField] private List<STSkillInfo> m_oPassiveSkillInfoList = new List<STSkillInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<ESkillKinds, STSkillInfo> SkillInfoDict { get; private set; } = new Dictionary<ESkillKinds, STSkillInfo>();

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

		var oSkillInfoList = new List<STSkillInfo>(m_oActiveSkillInfoList);
		oSkillInfoList.ExAddVals(m_oPassiveSkillInfoList);

		for(int i = 0; i < oSkillInfoList.Count; ++i) {
			this.SkillInfoDict.TryAdd(oSkillInfoList[i].m_eSkillKinds, oSkillInfoList[i]);
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

	/** 스킬 정보를 반환한다 */
	public bool TryGetSkillInfo(ESkillKinds a_ESkillKinds, out STSkillInfo a_stOutSkillInfo) {
		a_stOutSkillInfo = this.SkillInfoDict.GetValueOrDefault(a_ESkillKinds, default(STSkillInfo));
		return this.SkillInfoDict.ContainsKey(a_ESkillKinds);
	}

	/** 스킬 정보를 로드한다 */
	public Dictionary<ESkillKinds, STSkillInfo> LoadSkillInfos() {
		this.ResetSkillInfos();
		return this.LoadSkillInfos(this.SkillInfoTablePath);
	}

	/** 스킬 정보를 로드한다 */
	private Dictionary<ESkillKinds, STSkillInfo> LoadSkillInfos(string a_oFilePath) {
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
	private Dictionary<ESkillKinds, STSkillInfo> DoLoadSkillInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		var oJSONNode = SimpleJSON.JSONNode.Parse(a_oJSONStr);

		var oSkillInfosList = new List<SimpleJSON.JSONNode>() {
			oJSONNode[KCDefine.U_KEY_ACTIVE], oJSONNode[KCDefine.U_KEY_PASSIVE]
		};

		for(int i = 0; i < oSkillInfosList.Count; ++i) {
			for(int j = 0; j < oSkillInfosList[i].Count; ++j) {
				var stSkillInfo = new STSkillInfo(oSkillInfosList[i][j]);

				// 스킬 정보가 추가 가능 할 경우
				if(stSkillInfo.m_eSkillKinds.ExIsValid() && (!this.SkillInfoDict.ContainsKey(stSkillInfo.m_eSkillKinds) || oSkillInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.SkillInfoDict.ExReplaceVal(stSkillInfo.m_eSkillKinds, stSkillInfo);
				}
			}
		}

		return this.SkillInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
