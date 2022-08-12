#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 효과 정보 */
[System.Serializable]
public partial struct STFXInfo {
	public STCommonInfo m_stCommonInfo;
	public STDurationInfo m_stDurationInfo;

	public EFXKinds m_eFXKinds;
	public EFXKinds m_ePrevFXKinds;
	public EFXKinds m_eNextFXKinds;

	List<EResKinds> m_oResKindsList;

	#region 상수
	public static STFXInfo INVALID = new STFXInfo() {
		m_eFXKinds = EFXKinds.NONE, m_ePrevFXKinds = EFXKinds.NONE, m_eNextFXKinds = EFXKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public EFXType FXType => (EFXType)((int)m_eFXKinds).ExKindsToType();
	public EFXKinds BaseFXKinds => (EFXKinds)((int)m_eFXKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STFXInfo(SimpleJSON.JSONNode a_oFXInfo) {
		m_stCommonInfo = new STCommonInfo(a_oFXInfo);
		m_stDurationInfo = new STDurationInfo(a_oFXInfo[KCDefine.U_KEY_DURATION_INFO]);

		m_eFXKinds = a_oFXInfo[KCDefine.U_KEY_FX_KINDS].ExIsValid() ? (EFXKinds)a_oFXInfo[KCDefine.U_KEY_FX_KINDS].AsInt : EFXKinds.NONE;
		m_ePrevFXKinds = a_oFXInfo[KCDefine.U_KEY_PREV_FX_KINDS].ExIsValid() ? (EFXKinds)a_oFXInfo[KCDefine.U_KEY_PREV_FX_KINDS].AsInt : EFXKinds.NONE;
		m_eNextFXKinds = a_oFXInfo[KCDefine.U_KEY_NEXT_FX_KINDS].ExIsValid() ? (EFXKinds)a_oFXInfo[KCDefine.U_KEY_NEXT_FX_KINDS].AsInt : EFXKinds.NONE;

		m_oResKindsList = new List<EResKinds>();

		for(int i = 0; i < KDefine.G_MAX_NUM_RES_KINDS; ++i) {
			string oKey = string.Format(KCDefine.U_KEY_FMT_RES_KINDS, i + KCDefine.B_VAL_1_INT);
			if(a_oFXInfo[oKey].ExIsValid()) { m_oResKindsList.ExAddVal((EResKinds)a_oFXInfo[oKey].AsInt); }
		}
	}
	#endregion			// 함수
}

/** 효과 정보 테이블 */
public partial class CFXInfoTable : CSingleton<CFXInfoTable> {
	#region 프로퍼티
	public Dictionary<EFXKinds, STFXInfo> FXInfoDict { get; private set; } = new Dictionary<EFXKinds, STFXInfo>();

	private string FXInfoTablePath {
		get {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_ETC_INFO;
#else
			return KCDefine.U_TABLE_P_G_ETC_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetFXInfos();
	}

	/** 효과 정보를 리셋한다 */
	public void ResetFXInfos() {
		this.FXInfoDict.Clear();
	}

	/** 효과 정보를 리셋한다 */
	public void ResetFXInfos(string a_oJSONStr) {
		this.ResetFXInfos();
		this.DoLoadFXInfos(a_oJSONStr);
	}

	/** 효과 정보를 반환한다 */
	public STFXInfo GetFXInfo(EFXKinds a_eFXKinds) {
		bool bIsValid = this.TryGetFXInfo(a_eFXKinds, out STFXInfo stFXInfo);
		CAccess.Assert(bIsValid);

		return stFXInfo;
	}

	/** 효과 정보를 반환한다 */
	public bool TryGetFXInfo(EFXKinds a_eFXKinds, out STFXInfo a_stOutFXInfo) {
		a_stOutFXInfo = this.FXInfoDict.GetValueOrDefault(a_eFXKinds, STFXInfo.INVALID);
		return this.FXInfoDict.ContainsKey(a_eFXKinds);
	}

	/** 효과 정보를 로드한다 */
	public Dictionary<EFXKinds, STFXInfo> LoadFXInfos() {
		this.ResetFXInfos();
		return this.LoadFXInfos(this.FXInfoTablePath);
	}

	/** 효과 정보를 로드한다 */
	private Dictionary<EFXKinds, STFXInfo> LoadFXInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadFXInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			return this.DoLoadFXInfos(CResManager.Inst.GetRes<TextAsset>(a_oFilePath).text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 효과 정보를 로드한다 */
	private Dictionary<EFXKinds, STFXInfo> DoLoadFXInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());

		var oJSONNode = SimpleJSON.JSONNode.Parse(a_oJSONStr);
		var oFXInfosList = new List<SimpleJSON.JSONNode>();

		for(int i = 0; i < KDefine.G_KEY_FX_IT_INFOS_LIST.Count; ++i) {
			oFXInfosList.ExAddVal(oJSONNode[KDefine.G_KEY_FX_IT_INFOS_LIST[i]]);
		}

		for(int i = 0; i < oFXInfosList.Count; ++i) {
			for(int j = 0; j < oFXInfosList[i].Count; ++j) {
				var stFXInfo = new STFXInfo(oFXInfosList[i][j]);

				// 효과 정보 추가 가능 할 경우
				if(stFXInfo.m_eFXKinds.ExIsValid() && (!this.FXInfoDict.ContainsKey(stFXInfo.m_eFXKinds) || oFXInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.FXInfoDict.ExReplaceVal(stFXInfo.m_eFXKinds, stFXInfo);
				}
			}
		}

		return this.FXInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
