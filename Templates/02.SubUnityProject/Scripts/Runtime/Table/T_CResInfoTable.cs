#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.IO;
using System.Globalization;

/** 리소스 정보 */
[System.Serializable]
public struct STResInfo {
	public STCommonInfo m_stCommonInfo;

	public string m_oRate;
	public string m_oResPath;

	public EResKinds m_eResKinds;
	public EResKinds m_ePrevResKinds;
	public EResKinds m_eNextResKinds;

	#region 상수
	public static STResInfo INVALID = new STResInfo() {
		m_eResKinds = EResKinds.NONE, m_ePrevResKinds = EResKinds.NONE, m_eNextResKinds = EResKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public int IntRate => int.TryParse(m_oRate, NumberStyles.Any, null, out int nRate) ? nRate : KCDefine.B_VAL_0_INT;
	public float RealRate => float.TryParse(m_oRate, NumberStyles.Any, null, out float fRate) ? fRate : KCDefine.B_VAL_0_INT;

	public EResType ResType => (EResType)((int)m_eResKinds).ExKindsToType();
	public EResKinds BaseResKinds => (EResKinds)((int)m_eResKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티
	
	#region 함수
	/** 생성자 */
	public STResInfo(SimpleJSON.JSONNode a_oResInfo) {
		m_stCommonInfo = new STCommonInfo(a_oResInfo);
		
		m_oRate = a_oResInfo[KCDefine.U_KEY_RATE].ExIsValid() ? a_oResInfo[KCDefine.U_KEY_RATE] : KCDefine.B_STR_0_INT;
		m_oResPath = a_oResInfo[KCDefine.U_KEY_RES_PATH].ExIsValid() ? a_oResInfo[KCDefine.U_KEY_RES_PATH].Value.Replace(KCDefine.B_TOKEN_REV_SLASH, KCDefine.B_TOKEN_SLASH) : string.Empty;

		m_eResKinds = a_oResInfo[KCDefine.U_KEY_RES_KINDS].ExIsValid() ? (EResKinds)a_oResInfo[KCDefine.U_KEY_RES_KINDS].AsInt : EResKinds.NONE;
		m_ePrevResKinds = a_oResInfo[KCDefine.U_KEY_PREV_RES_KINDS].ExIsValid() ? (EResKinds)a_oResInfo[KCDefine.U_KEY_PREV_RES_KINDS].AsInt : EResKinds.NONE;
		m_eNextResKinds = a_oResInfo[KCDefine.U_KEY_NEXT_RES_KINDS].ExIsValid() ? (EResKinds)a_oResInfo[KCDefine.U_KEY_NEXT_RES_KINDS].AsInt : EResKinds.NONE;
	}
	#endregion			// 함수
}

/** 리소스 정보 테이블 */
public partial class CResInfoTable : CSingleton<CResInfoTable> {
	#region 프로퍼티
	public Dictionary<EResKinds, STResInfo> ResInfoDict { get; } = new Dictionary<EResKinds, STResInfo>();
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetResInfos();
	}

	/** 리소스 정보를 리셋한다 */
	public virtual void ResetResInfos() {
		this.ResInfoDict.Clear();
	}

	/** 리소스 정보를 리셋한다 */
	public virtual void ResetResInfos(string a_oJSONStr) {
		this.ResetResInfos();
		this.DoLoadResInfos(a_oJSONStr);
	}

	/** 리소스 정보를 반환한다 */
	public STResInfo GetResInfo(EResKinds a_eResKinds) {
		bool bIsValid = this.TryGetResInfo(a_eResKinds, out STResInfo stResInfo);
		CAccess.Assert(bIsValid);

		return stResInfo;
	}

	/** 리소스 정보를 반환한다 */
	public bool TryGetResInfo(EResKinds a_eResKinds, out STResInfo a_stOutResInfo) {
		a_stOutResInfo = this.ResInfoDict.GetValueOrDefault(a_eResKinds, STResInfo.INVALID);
		return this.ResInfoDict.ContainsKey(a_eResKinds);
	}

	/** 리소스 정보를 로드한다 */
	public Dictionary<EResKinds, STResInfo> LoadResInfos() {
		this.ResetResInfos();
		return this.LoadResInfos(Access.ResInfoTableLoadPath);
	}

	/** 리소스 정보를 저장한다 */
	public void SaveResInfos(string a_oJSONStr, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oJSONStr != null);

		// JSON 문자열이 존재 할 경우
		if(a_oJSONStr != null) {
			this.ResetResInfos(a_oJSONStr);
			
#if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
			CFunc.WriteStr(Access.ResInfoTableSavePath, a_oJSONStr, false);
#else
			CFunc.WriteStr(Access.ResInfoTableSavePath, a_oJSONStr, true);
#endif			// #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		}
	}

	/** JSON 노드를 설정한다 */
	private void SetupJSONNodes(SimpleJSON.JSONNode a_oJSONNode, out List<SimpleJSON.JSONNode> a_oOutResInfosList) {
		a_oOutResInfosList = new List<SimpleJSON.JSONNode>();
		var oTableInfoDictContainer = KDefine.G_TABLE_INFO_DICT_CONTAINER.GetValueOrDefault(Path.GetFileNameWithoutExtension(Access.ResInfoTableLoadPath));

		// 공용 정보가 존재 할 경우
		if(oTableInfoDictContainer.Item2[this.GetType()].ContainsKey(KCDefine.B_KEY_COMMON)) {
			for(int i = 0; i < oTableInfoDictContainer.Item2[this.GetType()][KCDefine.B_KEY_COMMON].Count; ++i) {
				a_oOutResInfosList.ExAddVal(a_oJSONNode[oTableInfoDictContainer.Item2[this.GetType()][KCDefine.B_KEY_COMMON][i]]);
			}
		}
	}

	/** 리소스 정보를 로드한다 */
	private Dictionary<EResKinds, STResInfo> LoadResInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadResInfos(File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, false) : CFunc.ReadStrFromRes(a_oFilePath, false));
#else
		return this.DoLoadResInfos(File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, true) : CFunc.ReadStrFromRes(a_oFilePath, false));
#endif			// #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 리소스 정보를 로드한다 */
	private Dictionary<EResKinds, STResInfo> DoLoadResInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		this.SetupJSONNodes(SimpleJSON.JSONNode.Parse(a_oJSONStr), out List<SimpleJSON.JSONNode> oResInfosList);
		
		for(int i = 0; i < oResInfosList.Count; ++i) {
			for(int j = 0; j < oResInfosList[i].Count; ++j) {
				var stResInfo = new STResInfo(oResInfosList[i][j]);

				// 리소스 정보 추가 가능 할 경우
				if(stResInfo.m_eResKinds.ExIsValid() && (!this.ResInfoDict.ContainsKey(stResInfo.m_eResKinds) || oResInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.ResInfoDict.ExReplaceVal(stResInfo.m_eResKinds, stResInfo);
				}
			}
		}

		return this.ResInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
