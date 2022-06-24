using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 어빌리티 정보 */
[System.Serializable]
public partial struct STAbilityInfo {
	public STDescInfo m_stDescInfo;
	public STValInfo m_stValInfo;

	public EAbilityKinds m_eAbilityKinds;
	public EAbilityKinds m_ePrevAbilityKinds;
	public EAbilityKinds m_eNextAbilityKinds;

	public List<EAbilityKinds> m_oExtraAbilityKindsList;

	#region 프로퍼티
	public EAbilityType AbilityType => (EAbilityType)((int)m_eAbilityKinds).ExKindsToType();
	public EAbilityKinds BaseAbilityKinds => (EAbilityKinds)((int)m_eAbilityKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STAbilityInfo(SimpleJSON.JSONNode a_oAbilityInfo) {
		m_stDescInfo = new STDescInfo(a_oAbilityInfo);
		m_stValInfo = new STValInfo(a_oAbilityInfo);

		m_eAbilityKinds = a_oAbilityInfo[KCDefine.U_KEY_ABILITY_KINDS].ExIsValid() ? (EAbilityKinds)a_oAbilityInfo[KCDefine.U_KEY_ABILITY_KINDS].AsInt : EAbilityKinds.NONE;
		m_ePrevAbilityKinds = a_oAbilityInfo[KCDefine.U_KEY_PREV_ABILITY_KINDS].ExIsValid() ? (EAbilityKinds)a_oAbilityInfo[KCDefine.U_KEY_PREV_ABILITY_KINDS].AsInt : EAbilityKinds.NONE;
		m_eNextAbilityKinds = a_oAbilityInfo[KCDefine.U_KEY_NEXT_ABILITY_KINDS].ExIsValid() ? (EAbilityKinds)a_oAbilityInfo[KCDefine.U_KEY_NEXT_ABILITY_KINDS].AsInt : EAbilityKinds.NONE;

		m_oExtraAbilityKindsList = new List<EAbilityKinds>();

		for(int i = 0; i < KDefine.G_MAX_NUM_ABILITY_KINDS; ++i) {
			string oExtraAbilityKindsKey = string.Format(KCDefine.U_KEY_FMT_ABILITY_KINDS, i + KCDefine.B_VAL_1_INT);
			m_oExtraAbilityKindsList.Add(a_oAbilityInfo[oExtraAbilityKindsKey].ExIsValid() ? (EAbilityKinds)a_oAbilityInfo[oExtraAbilityKindsKey].AsInt : EAbilityKinds.NONE);
		}
	}
	#endregion			// 함수
}

/** 어빌리티 정보 테이블 */
public partial class CAbilityInfoTable : CScriptableObj<CAbilityInfoTable> {
	#region 변수
	[Header("=====> Stat Ability Info <=====")]
	[SerializeField] private List<STAbilityInfo> m_oStatAbilityInfoList = new List<STAbilityInfo>();

	[Header("=====> Buff Ability Info <=====")]
	[SerializeField] private List<STAbilityInfo> m_oBuffAbilityInfoList = new List<STAbilityInfo>();

	[Header("=====> Debuff Ability Info <=====")]
	[SerializeField] private List<STAbilityInfo> m_oDebuffAbilityInfoList = new List<STAbilityInfo>();

	[Header("=====> Upgrade Ability Info <=====")]
	[SerializeField] private List<STAbilityInfo> m_oUpgradeAbilityInfoList = new List<STAbilityInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<EAbilityKinds, STAbilityInfo> AbilityInfoDict { get; private set; } = new Dictionary<EAbilityKinds, STAbilityInfo>();

	private string AbilityInfoTablePath {
		get {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_ABILITY_INFO;
#else
			return KCDefine.U_TABLE_P_G_ABILITY_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		var oAbilityInfoList = new List<STAbilityInfo>(m_oStatAbilityInfoList);
		oAbilityInfoList.ExAddVals(m_oBuffAbilityInfoList);
		oAbilityInfoList.ExAddVals(m_oDebuffAbilityInfoList);
		oAbilityInfoList.ExAddVals(m_oUpgradeAbilityInfoList);

		for(int i = 0; i < oAbilityInfoList.Count; ++i) {
			this.AbilityInfoDict.TryAdd(oAbilityInfoList[i].m_eAbilityKinds, oAbilityInfoList[i]);
		}
	}

	/** 어빌리티 정보를 반환한다 */
	public STAbilityInfo GetAbilityInfo(EAbilityKinds a_EAbilityKinds) {
		bool bIsValid = this.TryGetAbilityInfo(a_EAbilityKinds, out STAbilityInfo stAbilityInfo);
		CAccess.Assert(bIsValid);

		return stAbilityInfo;
	}

	/** 어빌리티 정보를 반환한다 */
	public bool TryGetAbilityInfo(EAbilityKinds a_EAbilityKinds, out STAbilityInfo a_stOutAbilityInfo) {
		a_stOutAbilityInfo = this.AbilityInfoDict.GetValueOrDefault(a_EAbilityKinds, default(STAbilityInfo));
		return this.AbilityInfoDict.ContainsKey(a_EAbilityKinds);
	}

	/** 어빌리티 정보를 로드한다 */
	public Dictionary<EAbilityKinds, STAbilityInfo> LoadAbilityInfos() {
		return this.LoadAbilityInfos(this.AbilityInfoTablePath);
	}

	/** 어빌리티 정보를 로드한다 */
	private Dictionary<EAbilityKinds, STAbilityInfo> LoadAbilityInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadAbilityInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			return this.DoLoadAbilityInfos(CResManager.Inst.GetRes<TextAsset>(a_oFilePath).text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 어빌리티 정보를 로드한다 */
	private Dictionary<EAbilityKinds, STAbilityInfo> DoLoadAbilityInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		var oJSONNode = SimpleJSON.JSONNode.Parse(a_oJSONStr);

		var oAbilityInfosList = new List<SimpleJSON.JSONNode>() {
			oJSONNode[KCDefine.U_KEY_STAT], oJSONNode[KCDefine.U_KEY_BUFF], oJSONNode[KCDefine.U_KEY_DEBUFF], oJSONNode[KCDefine.U_KEY_UPGRADE]
		};

		for(int i = 0; i < oAbilityInfosList.Count; ++i) {
			for(int j = 0; j < oAbilityInfosList[i].Count; ++j) {
				var stAbilityInfo = new STAbilityInfo(oAbilityInfosList[i][j]);

				// 어빌리티 정보가 추가 가능 할 경우
				if(stAbilityInfo.m_eAbilityKinds.ExIsValid() && (!this.AbilityInfoDict.ContainsKey(stAbilityInfo.m_eAbilityKinds) || oAbilityInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.AbilityInfoDict.ExReplaceVal(stAbilityInfo.m_eAbilityKinds, stAbilityInfo);
				}
			}
		}

		return this.AbilityInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
