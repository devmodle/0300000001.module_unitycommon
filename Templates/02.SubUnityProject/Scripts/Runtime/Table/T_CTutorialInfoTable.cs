#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 튜토리얼 정보 */
[System.Serializable]
public partial struct STTutorialInfo {
	public STCommonInfo m_stCommonInfo;

	public ETutorialKinds m_eTutorialKinds;
	public ETutorialKinds m_ePrevTutorialKinds;
	public ETutorialKinds m_eNextTutorialKinds;

	public List<string> m_oStrList;
	public List<ERewardKinds> m_oRewardKindsList;

	#region 상수
	public static STTutorialInfo INVALID = new STTutorialInfo() {
		m_eTutorialKinds = ETutorialKinds.NONE, m_ePrevTutorialKinds = ETutorialKinds.NONE, m_eNextTutorialKinds = ETutorialKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public ETutorialType TutorialType => (ETutorialType)((int)m_eTutorialKinds).ExKindsToType();
	public ETutorialKinds BaseTutorialKinds => (ETutorialKinds)((int)m_eTutorialKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STTutorialInfo(SimpleJSON.JSONNode a_oTutorialInfo) {
		m_stCommonInfo = new STCommonInfo(a_oTutorialInfo);

		m_eTutorialKinds = a_oTutorialInfo[KCDefine.U_KEY_TUTORIAL_KINDS].ExIsValid() ? (ETutorialKinds)a_oTutorialInfo[KCDefine.U_KEY_TUTORIAL_KINDS].AsInt : ETutorialKinds.NONE;
		m_ePrevTutorialKinds = a_oTutorialInfo[KCDefine.U_KEY_PREV_TUTORIAL_KINDS].ExIsValid() ? (ETutorialKinds)a_oTutorialInfo[KCDefine.U_KEY_PREV_TUTORIAL_KINDS].AsInt : ETutorialKinds.NONE;
		m_eNextTutorialKinds = a_oTutorialInfo[KCDefine.U_KEY_NEXT_TUTORIAL_KINDS].ExIsValid() ? (ETutorialKinds)a_oTutorialInfo[KCDefine.U_KEY_NEXT_TUTORIAL_KINDS].AsInt : ETutorialKinds.NONE;

		m_oStrList = new List<string>();
		m_oRewardKindsList = new List<ERewardKinds>();

		for(int i = 0; i < KDefine.G_MAX_NUM_TUTORIAL_STRS; ++i) {
			string oKey = string.Format(KCDefine.U_KEY_FMT_STRS, i + KCDefine.B_VAL_1_INT);
			if(a_oTutorialInfo[oKey].ExIsValid()) { m_oStrList.Add(a_oTutorialInfo[oKey]); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_REWARD_KINDS; ++i) {
			string oKey = string.Format(KCDefine.U_KEY_FMT_REWARD_KINDS, i + KCDefine.B_VAL_1_INT);
			if(a_oTutorialInfo[oKey].ExIsValid()) { m_oRewardKindsList.ExAddVal((ERewardKinds)a_oTutorialInfo[oKey].AsInt); }
		}
	}
	#endregion			// 함수
}

/** 튜토리얼 정보 테이블 */
public partial class CTutorialInfoTable : CSingleton<CTutorialInfoTable> {
	#region 프로퍼티
	public Dictionary<ETutorialKinds, STTutorialInfo> TutorialInfoDict { get; private set; } = new Dictionary<ETutorialKinds, STTutorialInfo>();

	private string TutorialInfoTablePath {
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
		this.ResetTutorialInfos();
	}

	/** 튜토리얼 정보를 리셋한다 */
	public void ResetTutorialInfos() {
		this.TutorialInfoDict.Clear();
	}

	/** 튜토리얼 정보를 리셋한다 */
	public void ResetTutorialInfos(string a_oJSONStr) {
		this.ResetTutorialInfos();
		this.DoLoadTutorialInfos(a_oJSONStr);
	}

	/** 튜토리얼 정보를 반환한다 */
	public STTutorialInfo GetTutorialInfo(ETutorialKinds a_eTutorialKinds) {
		bool bIsValid = this.TryGetTutorialInfo(a_eTutorialKinds, out STTutorialInfo stTutorialInfo);
		CAccess.Assert(bIsValid);

		return stTutorialInfo;
	}

	/** 튜토리얼 정보를 반환한다 */
	public bool TryGetTutorialInfo(ETutorialKinds a_eTutorialKinds, out STTutorialInfo a_stOutTutorialInfo) {
		a_stOutTutorialInfo = this.TutorialInfoDict.GetValueOrDefault(a_eTutorialKinds, STTutorialInfo.INVALID);
		return this.TutorialInfoDict.ContainsKey(a_eTutorialKinds);
	}

	/** 튜토리얼 정보를 로드한다 */
	public Dictionary<ETutorialKinds, STTutorialInfo> LoadTutorialInfos() {
		this.ResetTutorialInfos();
		return this.LoadTutorialInfos(this.TutorialInfoTablePath);
	}

	/** 튜토리얼 정보를 로드한다 */
	private Dictionary<ETutorialKinds, STTutorialInfo> LoadTutorialInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadTutorialInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			return this.DoLoadTutorialInfos(CResManager.Inst.GetRes<TextAsset>(a_oFilePath).text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 튜토리얼 정보를 로드한다 */
	private Dictionary<ETutorialKinds, STTutorialInfo> DoLoadTutorialInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());

		var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr);
		var oTutorialInfosList = new List<SimpleJSON.JSONNode>();

		for(int i = 0; i < KDefine.G_KEY_TUTORIAL_IT_INFOS_LIST.Count; ++i) {
			oTutorialInfosList.ExAddVal(oJSONNode[KDefine.G_KEY_TUTORIAL_IT_INFOS_LIST[i]]);
		}
		
		for(int i = 0; i < oTutorialInfosList.Count; ++i) {
			for(int j = 0; j < oTutorialInfosList[i].Count; ++j) {
				var stTutorialInfo = new STTutorialInfo(oTutorialInfosList[i][j]);

				// 튜토리얼 정보 추가 가능 할 경우
				if(stTutorialInfo.m_eTutorialKinds.ExIsValid() && (!this.TutorialInfoDict.ContainsKey(stTutorialInfo.m_eTutorialKinds) || oTutorialInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.TutorialInfoDict.ExReplaceVal(stTutorialInfo.m_eTutorialKinds, stTutorialInfo);
				}
			}
		}
		
		return this.TutorialInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
