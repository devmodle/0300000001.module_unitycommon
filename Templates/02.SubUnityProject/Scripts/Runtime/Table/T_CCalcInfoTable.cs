#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 수식 정보 */
[System.Serializable]
public struct STCalcInfo {
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

/** 수식 정보 테이블 */
public partial class CCalcInfoTable : CSingleton<CCalcInfoTable> {
	#region 프로퍼티
	public Dictionary<ECalcKinds, STCalcInfo> CalcInfoDict { get; } = new Dictionary<ECalcKinds, STCalcInfo>();
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetCalcInfos();
	}

	/** 수식 정보를 리셋한다 */
	public virtual void ResetCalcInfos() {
		this.CalcInfoDict.Clear();
	}

	/** 수식 정보를 리셋한다 */
	public virtual void ResetCalcInfos(string a_oJSONStr) {
		this.ResetCalcInfos();
		this.DoLoadCalcInfos(a_oJSONStr);
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
	public Dictionary<ECalcKinds, STCalcInfo> LoadCalcInfos() {
		this.ResetCalcInfos();
		return this.LoadCalcInfos(Access.CalcInfoTableLoadPath);
	}

	/** 수식 정보를 저장한다 */
	public void SaveCalcInfos(string a_oJSONStr, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oJSONStr != null);

		// JSON 문자열이 존재 할 경우
		if(a_oJSONStr != null) {
			this.ResetCalcInfos(a_oJSONStr);
		}
	}

	/** JSON 노드를 설정한다 */
	private void SetupJSONNodes(SimpleJSON.JSONNode a_oJSONNode, out List<SimpleJSON.JSONNode> a_oOutCalcInfosList) {
		a_oOutCalcInfosList = new List<SimpleJSON.JSONNode>();
		var oTableInfoDictContainer = KDefine.G_TABLE_INFO_DICT_CONTAINER[Path.GetFileNameWithoutExtension(Access.CalcInfoTableLoadPath)];

		for(int i = 0; i < oTableInfoDictContainer.Item2[this.GetType()][KCDefine.B_KEY_COMMON].Count; ++i) {
			a_oOutCalcInfosList.ExAddVal(a_oJSONNode[oTableInfoDictContainer.Item2[this.GetType()][KCDefine.B_KEY_COMMON][i]]);
		}
	}

	/** 기타 정보를 로드한다 */
	private Dictionary<ECalcKinds, STCalcInfo> LoadCalcInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_EDITOR || (UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD))
		return this.DoLoadCalcInfos(File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, false) : CFunc.ReadStrFromRes(a_oFilePath, false));
#else
		return this.DoLoadCalcInfos(File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, true) : CFunc.ReadStrFromRes(a_oFilePath, true));
#endif			// #if UNITY_EDITOR || (UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD))
	}

	/** 기타 정보를 로드한다 */
	private Dictionary<ECalcKinds, STCalcInfo> DoLoadCalcInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		this.SetupJSONNodes(SimpleJSON.JSONNode.Parse(a_oJSONStr), out List<SimpleJSON.JSONNode> oCalcInfosList);

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
