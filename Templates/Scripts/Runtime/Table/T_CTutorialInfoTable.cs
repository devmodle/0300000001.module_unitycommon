using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 튜토리얼 정보
[System.Serializable]
public struct STTutorialInfo {
	public ERewardKinds m_eRewardKinds;
	public ETutorialKinds m_eTutorialKinds;
	public ETutorialKinds m_eNextTutorialKinds;

	public List<string> m_oStrList;

	#region 함수
	//! 생성자
	public STTutorialInfo(SimpleJSON.JSONNode a_oTutorialInfo) {
		m_eRewardKinds = (ERewardKinds)a_oTutorialInfo[KCDefine.U_KEY_REWARD_KINDS].AsInt;
		m_eTutorialKinds = (ETutorialKinds)a_oTutorialInfo[KCDefine.U_KEY_TUTORIAL_KINDS].AsInt;
		m_eNextTutorialKinds = (ETutorialKinds)a_oTutorialInfo[KCDefine.U_KEY_NEXT_TUTORIAL_KINDS].AsInt;

		m_oStrList = new List<string>();

		for(int i = 0; i < KDefine.G_MAX_NUM_TUTORIAL_STRS; ++i) {
			string oStrKey = string.Format(KCDefine.U_KEY_FMT_STRS, i + KCDefine.B_VAL_1_INT);
			m_oStrList.Add(a_oTutorialInfo[oStrKey]);
		}
	}
	#endregion			// 함수
}

//! 튜토리얼 정보 테이블
public class CTutorialInfoTable : CScriptableObj<CTutorialInfoTable> {
	#region 변수
	[Header("Play Tutorial Info")]
	[SerializeField] private List<STTutorialInfo> m_oPlayTutorialInfoList = new List<STTutorialInfo>();

	[Header("Help Tutorial Info")]
	[SerializeField] private List<STTutorialInfo> m_oHelpTutorialInfoList = new List<STTutorialInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<ETutorialKinds, STTutorialInfo> TutorialInfoDict { get; private set; } = new Dictionary<ETutorialKinds, STTutorialInfo>();
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		var oTutorialInfoList = new List<STTutorialInfo>(m_oPlayTutorialInfoList);
		oTutorialInfoList.AddRange(m_oHelpTutorialInfoList);

		for(int i = 0; i < oTutorialInfoList.Count; ++i) {
			this.TutorialInfoDict.ExAddVal(oTutorialInfoList[i].m_eTutorialKinds, oTutorialInfoList[i]);
		}
	}

	//! 튜토리얼 정보를 반환한다
	public STTutorialInfo GetTutorialInfo(ETutorialKinds a_eTutorialKinds) {
		bool bIsValid = this.TryGetTutorialInfo(a_eTutorialKinds, out STTutorialInfo stTutorialInfo);
		CAccess.Assert(bIsValid);

		return stTutorialInfo;
	}

	//! 튜토리얼 정보를 반환한다
	public bool TryGetTutorialInfo(ETutorialKinds a_eTutorialKinds, out STTutorialInfo a_stOutTutorialInfo) {
		a_stOutTutorialInfo = this.TutorialInfoDict.ExGetVal(a_eTutorialKinds, KDefine.G_INVALID_TUTORIAL_INFO);
		return this.TutorialInfoDict.ContainsKey(a_eTutorialKinds);
	}

	//! 튜토리얼 정보를 로드한다
	public Dictionary<ETutorialKinds, STTutorialInfo> LoadTutorialInfos() {
#if UNITY_EDITOR || UNITY_STANDALONE
		return this.LoadTutorialInfos(KCDefine.U_RUNTIME_TABLE_P_G_TUTORIAL_INFO);
#else
		return this.LoadTutorialInfos(KCDefine.U_TABLE_P_G_TUTORIAL_INFO);
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	}

	//! 튜토리얼 정보를 로드한다
	public Dictionary<ETutorialKinds, STTutorialInfo> LoadTutorialInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_EDITOR || UNITY_STANDALONE
		string oJSONStr = CFunc.ReadStr(a_oFilePath);
		return this.DoLoadTutorialInfos(oJSONStr);
#else
		try {
			var oTextAsset = CResManager.Inst.GetRes<TextAsset>(a_oFilePath);
			return this.DoLoadTutorialInfos(oTextAsset.text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	}

	//! 튜토리얼 정보를 로드한다
	private Dictionary<ETutorialKinds, STTutorialInfo> DoLoadTutorialInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr) as SimpleJSON.JSONClass;

		var oTutorialInfosList = new List<SimpleJSON.JSONNode>() {
			oJSONNode[KCDefine.U_KEY_PLAY],
			oJSONNode[KCDefine.U_KEY_HELP]
		};

		for(int i = 0; i < oTutorialInfosList.Count; ++i) {
			for(int j = 0; j < oTutorialInfosList[i].Count; ++j) {
				var stTutorialInfo = new STTutorialInfo(oTutorialInfosList[i][j]);
				bool bIsReplace = oTutorialInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT;

				// 튜토리얼 정보가 추가 가능 할 경우
				if(bIsReplace || !this.TutorialInfoDict.ContainsKey(stTutorialInfo.m_eTutorialKinds)) {
					this.TutorialInfoDict.ExReplaceVal(stTutorialInfo.m_eTutorialKinds, stTutorialInfo);
				}
			}
		}
		
		return this.TutorialInfoDict;
	}
	#endregion			// 함수

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if NEVER_USE_THIS
