#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 수식 정보 */
[System.Serializable]
public partial struct STCalcInfo {
	public STCommonInfo m_stCommonInfo;
	public string m_oCalc;

	public ECalcKinds m_eCalcKinds;
	public ECalcKinds m_ePrevCalcKinds;
	public ECalcKinds m_eNextCalcKinds;

	#region 상수
	public static STCalcInfo INVALID = new STCalcInfo() {
		m_eCalcKinds = ECalcKinds.NONE, m_ePrevCalcKinds = ECalcKinds.NONE, m_eNextCalcKinds = ECalcKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public ECalcType CalcType => (ECalcType)((int)m_eCalcKinds).ExKindsToType();
	public ECalcKinds BaseCalcKinds => (ECalcKinds)((int)m_eCalcKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STCalcInfo(SimpleJSON.JSONNode a_oCalcInfo) {
		m_stCommonInfo = new STCommonInfo(a_oCalcInfo);
		m_oCalc = a_oCalcInfo[KCDefine.U_KEY_CALC].ExIsValid() ? a_oCalcInfo[KCDefine.U_KEY_CALC].Value.ExInfixToPostfixCalc() : string.Empty;

		m_eCalcKinds = a_oCalcInfo[KCDefine.U_KEY_CALC_KINDS].ExIsValid() ? (ECalcKinds)a_oCalcInfo[KCDefine.U_KEY_CALC_KINDS].AsInt : ECalcKinds.NONE;
		m_ePrevCalcKinds = a_oCalcInfo[KCDefine.U_KEY_PREV_CALC_KINDS].ExIsValid() ? (ECalcKinds)a_oCalcInfo[KCDefine.U_KEY_PREV_CALC_KINDS].AsInt : ECalcKinds.NONE;
		m_eNextCalcKinds = a_oCalcInfo[KCDefine.U_KEY_NEXT_CALC_KINDS].ExIsValid() ? (ECalcKinds)a_oCalcInfo[KCDefine.U_KEY_NEXT_CALC_KINDS].AsInt : ECalcKinds.NONE;
	}
	#endregion			// 함수
}

/** 기타 정보 테이블 */
public partial class CEtcInfoTable : CSingleton<CEtcInfoTable> {
	#region 프로퍼티
	public Dictionary<ECalcKinds, STCalcInfo> CalcInfoDict { get; private set; } = new Dictionary<ECalcKinds, STCalcInfo>();

	private string EtcInfoTablePath {
		get {
#if AB_TEST_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return (CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.A) ? KCDefine.U_RUNTIME_TABLE_P_G_ETC_INFO_SET_A : KCDefine.U_RUNTIME_TABLE_P_G_ETC_INFO_SET_B;
#else
			return (CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.A) ? KCDefine.U_TABLE_P_G_ETC_INFO_SET_A : KCDefine.U_TABLE_P_G_ETC_INFO_SET_B;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
#else
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_ETC_INFO;
#else
			return KCDefine.U_TABLE_P_G_ETC_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
#endif			// #if AB_TEST_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetEtcInfos();
	}

	/** 기타 정보를 리셋한다 */
	public void ResetEtcInfos() {
		this.CalcInfoDict.Clear();
	}

	/** 기타 정보를 리셋한다 */
	public void ResetEtcInfos(string a_oJSONStr) {
		this.ResetEtcInfos();
		this.DoLoadEtcInfos(a_oJSONStr);
	}

	/** 수식 정보를 반환한다 */
	public STCalcInfo GetCalcInfo(ECalcKinds a_eCalcKinds) {
		bool bIsValid = this.TryGetCalcInfo(a_eCalcKinds, out STCalcInfo stCalcInfo);
		CAccess.Assert(bIsValid);

		return stCalcInfo;
	}

	/** 수식 정보를 반환한다 */
	public bool TryGetCalcInfo(ECalcKinds a_eCalcKinds, out STCalcInfo a_stOutCalcInfo) {
		a_stOutCalcInfo = this.CalcInfoDict.GetValueOrDefault(a_eCalcKinds, STCalcInfo.INVALID);
		return this.CalcInfoDict.ContainsKey(a_eCalcKinds);
	}

	/** 기타 정보를 로드한다 */
	public Dictionary<ECalcKinds, STCalcInfo> LoadEtcInfos() {
		this.ResetEtcInfos();
		return this.LoadEtcInfos(this.EtcInfoTablePath);
	}

	/** 기타 정보를 로드한다 */
	private Dictionary<ECalcKinds, STCalcInfo> LoadEtcInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadEtcInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			return this.DoLoadEtcInfos(CResManager.Inst.GetRes<TextAsset>(a_oFilePath).text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 기타 정보를 로드한다 */
	private Dictionary<ECalcKinds, STCalcInfo> DoLoadEtcInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());

		var oJSONNode = SimpleJSON.JSONNode.Parse(a_oJSONStr);
		var oCalcInfosList = new List<SimpleJSON.JSONNode>();

		for(int i = 0; i < KDefine.G_KEY_ETC_IT_CALC_INFOS_LIST.Count; ++i) {
			oCalcInfosList.ExAddVal(oJSONNode[KDefine.G_KEY_ETC_IT_CALC_INFOS_LIST[i]]);
		}

		for(int i = 0; i < oCalcInfosList.Count; ++i) {
			for(int j = 0; j < oCalcInfosList[i].Count; ++j) {
				var stCalcInfo = new STCalcInfo(oCalcInfosList[i][j]);

				// 수식 정보 추가 가능 할 경우
				if(stCalcInfo.m_eCalcKinds.ExIsValid() && (!this.CalcInfoDict.ContainsKey(stCalcInfo.m_eCalcKinds) || oCalcInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.CalcInfoDict.ExReplaceVal(stCalcInfo.m_eCalcKinds, stCalcInfo);
				}
			}
		}

		return this.CalcInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
