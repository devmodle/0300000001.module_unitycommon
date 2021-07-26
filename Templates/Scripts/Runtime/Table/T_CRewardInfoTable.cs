using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 보상 정보
[System.Serializable]
public struct STRewardInfo {
	public string m_oName;
	public string m_oDesc;

	public ERewardKinds m_eRewardKinds;
	public ERewardQuality m_eRewardQuality;

	public List<STItemInfo> m_oItemInfoList;

	#region 함수
	//! 생성자
	public STRewardInfo(SimpleJSON.JSONNode a_oRewardInfo) {
		m_oName = a_oRewardInfo[KCDefine.U_KEY_NAME];
		m_oDesc = a_oRewardInfo[KCDefine.U_KEY_DESC];

		m_eRewardKinds = (ERewardKinds)a_oRewardInfo[KDefine.G_KEY_REWARD_IT_REWARD_KINDS].AsInt;
		m_eRewardQuality = (ERewardQuality)a_oRewardInfo[KDefine.G_KEY_REWARD_IT_REWARD_QUALITY].AsInt;

		m_oItemInfoList = new List<STItemInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_REWARD_ITEM_INFOS; ++i) {
			string oNumItemsKey = string.Format(KDefine.G_KEY_FMT_REWARD_IT_NUM_ITEMS, i + KCDefine.B_VAL_1_INT);
			string oItemKindsKey = string.Format(KDefine.G_KEY_FMT_REWARD_IT_ITEM_KINDS, i + KCDefine.B_VAL_1_INT);

			var stItemInfo = new STItemInfo() {
				m_nNumItems = a_oRewardInfo[oNumItemsKey].AsInt,
				m_eItemKinds = (EItemKinds)a_oRewardInfo[oItemKindsKey].AsInt
			};

			m_oItemInfoList.Add(stItemInfo);
		}
	}
	#endregion			// 함수
}

//! 보상 정보 테이블
public class CRewardInfoTable : CScriptableObj<CRewardInfoTable> {
	#region 변수
	[Header("Free Reward Info")]
	[SerializeField] private List<STRewardInfo> m_oFreeRewardInfoList = new List<STRewardInfo>();

	[Header("Daily Reward Info")]
	[SerializeField] private List<STRewardInfo> m_oDailyRewardInfoList = new List<STRewardInfo>();

	[Header("Event Reward Info")]
	[SerializeField] private List<STRewardInfo> m_oEventRewardInfoList = new List<STRewardInfo>();

	[Header("Clear Reward Info")]
	[SerializeField] private List<STRewardInfo> m_oClearRewardInfoList = new List<STRewardInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<ERewardKinds, STRewardInfo> RewardInfoDict { get; private set; } = new Dictionary<ERewardKinds, STRewardInfo>();
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		var oRewardInfoList = new List<STRewardInfo>(m_oFreeRewardInfoList);
		oRewardInfoList.AddRange(m_oDailyRewardInfoList);
		oRewardInfoList.AddRange(m_oEventRewardInfoList);
		oRewardInfoList.AddRange(m_oClearRewardInfoList);

		for(int i = 0; i < oRewardInfoList.Count; ++i) {
			this.RewardInfoDict.Add(oRewardInfoList[i].m_eRewardKinds, oRewardInfoList[i]);
		}
	}

	//! 보상 정보를 반환한다
	public STRewardInfo GetRewardInfo(ERewardKinds a_eRewardKinds) {
		bool bIsValid = this.TryGetRewardInfo(a_eRewardKinds, out STRewardInfo stRewardInfo);
		CAccess.Assert(bIsValid);

		return stRewardInfo;
	}

	//! 보상 정보를 반환한다
	public bool TryGetRewardInfo(ERewardKinds a_eRewardKinds, out STRewardInfo a_stOutRewardInfo) {
		a_stOutRewardInfo = this.RewardInfoDict.ExGetVal(a_eRewardKinds, KDefine.G_INVALID_REWARD_INFO);
		return this.RewardInfoDict.ContainsKey(a_eRewardKinds);
	}

	//! 보상 정보를 로드한다
	public Dictionary<ERewardKinds, STRewardInfo> LoadRewardInfos() {
#if UNITY_EDITOR || UNITY_STANDALONE
		return this.LoadRewardInfos(KDefine.G_RUNTIME_TABLE_P_REWARD_INFO);
#else
		return this.LoadRewardInfos(KCDefine.U_TABLE_P_G_REWARD_INFO);
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	}

	//! 보상 정보를 로드한다
	public Dictionary<ERewardKinds, STRewardInfo> LoadRewardInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_EDITOR || UNITY_STANDALONE
		string oJSONStr = CFunc.ReadStr(a_oFilePath);
		return this.DoLoadRewardInfos(oJSONStr);
#else
		try {
			var oTextAsset = CResManager.Inst.GetRes<TextAsset>(a_oFilePath);
			return this.DoLoadRewardInfos(oTextAsset.text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	}

	//! 보상 정보를 로드한다
	private Dictionary<ERewardKinds, STRewardInfo> DoLoadRewardInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr) as SimpleJSON.JSONClass;

		var oRewardInfosList = new List<SimpleJSON.JSONNode>() {
			oJSONNode[KDefine.G_KEY_REWARD_IT_FREE],
			oJSONNode[KDefine.G_KEY_REWARD_IT_DAILY],
			oJSONNode[KDefine.G_KEY_REWARD_IT_EVENT],
			oJSONNode[KDefine.G_KEY_REWARD_IT_CLEAR]
		};

		for(int i = 0; i < oRewardInfosList.Count; ++i) {
			for(int j = 0; j < oRewardInfosList[i].Count; ++j) {
				var stRewardInfo = new STRewardInfo(oRewardInfosList[i][j]);
				bool bIsReplace = oRewardInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT;

				// 보상 정보가 추가 가능 할 경우
				if(bIsReplace || !this.RewardInfoDict.ContainsKey(stRewardInfo.m_eRewardKinds)) {
					this.RewardInfoDict.ExReplaceVal(stRewardInfo.m_eRewardKinds, stRewardInfo);
				}
			}
		}
		
		return this.RewardInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
