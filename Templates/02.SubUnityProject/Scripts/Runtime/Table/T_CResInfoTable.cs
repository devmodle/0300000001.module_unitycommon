#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 리소스 정보 */
[System.Serializable]
public partial struct STResInfo {
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
	public Dictionary<EResKinds, STResInfo> ResInfoDict { get; private set; } = new Dictionary<EResKinds, STResInfo>();

	private string ResInfoTablePath {
		get {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_RES_INFO;
#else
			return KCDefine.U_TABLE_P_G_RES_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetResInfos();
	}

	/** 리소스 정보를 리셋한다 */
	public void ResetResInfos() {
		this.ResInfoDict.Clear();
	}

	/** 리소스 정보를 리셋한다 */
	public void ResetResInfos(string a_oJSONStr) {
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
		return this.LoadResInfos(this.ResInfoTablePath);
	}

	/** 리소스 정보를 로드한다 */
	private Dictionary<EResKinds, STResInfo> LoadResInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadResInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			return this.DoLoadResInfos(CResManager.Inst.GetRes<TextAsset>(a_oFilePath).text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 리소스 정보를 로드한다 */
	private Dictionary<EResKinds, STResInfo> DoLoadResInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());

		var oJSONNode = SimpleJSON.JSONNode.Parse(a_oJSONStr);
		var oResInfosList = new List<SimpleJSON.JSONNode>();

		for(int i = 0; i < KDefine.G_KEY_RES_IT_INFOS_LIST.Count; ++i) {
			oResInfosList.ExAddVal(oJSONNode[KDefine.G_KEY_RES_IT_INFOS_LIST[i]]);
		}
		
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
