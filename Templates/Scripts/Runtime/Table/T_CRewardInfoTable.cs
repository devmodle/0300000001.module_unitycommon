using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 보상 정보
[System.Serializable]
public struct STRewardInfo {
	public string m_oName;
	public string m_oDesc;

	public ERewardType m_eRewardType;
	public ERewardKinds m_eRewardKinds;

	public List<STItemInfo> m_oItemInfoList;

	#region 함수
	//! 생성자
	public STRewardInfo(SimpleJSON.JSONNode a_oRewardInfo) {
		m_oName = a_oRewardInfo[KDefine.G_KEY_REWARD_IT_NAME];
		m_oDesc = a_oRewardInfo[KDefine.G_KEY_REWARD_IT_DESC];

		m_eRewardType = (ERewardType)a_oRewardInfo[KDefine.G_KEY_REWARD_IT_REWARD_TYPE].AsInt;
		m_eRewardKinds = (ERewardKinds)a_oRewardInfo[KDefine.G_KEY_REWARD_IT_REWARD_KINDS].AsInt;

		m_oItemInfoList = new List<STItemInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_REWARD_IT_ITEM_INFOS; ++i) {
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

	[Header("Clear Reward Info")]
	[SerializeField] private List<STRewardInfo> m_oClearRewardInfoList = new List<STRewardInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<ERewardKinds, STRewardInfo> FreeRewardInfoList { get; private set; } = new Dictionary<ERewardKinds, STRewardInfo>();
	public Dictionary<ERewardKinds, STRewardInfo> DailyRewardInfoList { get; private set; } = new Dictionary<ERewardKinds, STRewardInfo>();
	public Dictionary<ERewardKinds, STRewardInfo> ClearRewardInfoList { get; private set; } = new Dictionary<ERewardKinds, STRewardInfo>();
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		this.SetupRewardInfos(m_oFreeRewardInfoList, this.FreeRewardInfoList);
		this.SetupRewardInfos(m_oDailyRewardInfoList, this.DailyRewardInfoList);
		this.SetupRewardInfos(m_oClearRewardInfoList, this.ClearRewardInfoList);
	}

	//! 무료 보상 정보를 반환한다
	public STRewardInfo GetFreeRewardInfo(ERewardKinds a_eRewardKinds) {
		return this.GetRewardInfo(a_eRewardKinds, this.FreeRewardInfoList);
	}

	//! 일일 보상 정보를 반환한다
	public STRewardInfo GetDailyRewardInfo(ERewardKinds a_eRewardKinds) {
		return this.GetRewardInfo(a_eRewardKinds, this.DailyRewardInfoList);
	}

	//! 클리어 보상 정보를 반환한다
	public STRewardInfo GetClearRewardInfo(ERewardKinds a_eRewardKinds) {
		return this.GetRewardInfo(a_eRewardKinds, this.ClearRewardInfoList);
	}

	//! 무료 보상 정보를 반환한다
	public bool TryGetFreeRewardInfo(ERewardKinds a_eRewardKinds, out STRewardInfo a_stOutFreeRewardInfo) {
		return this.TryGetRewardInfo(a_eRewardKinds, this.FreeRewardInfoList, out a_stOutFreeRewardInfo);
	}

	//! 일일 보상 정보를 반환한다
	public bool TryGetDailyRewardInfo(ERewardKinds a_eRewardKinds, out STRewardInfo a_stOutDailyRewardInfo) {
		return this.TryGetRewardInfo(a_eRewardKinds, this.DailyRewardInfoList, out a_stOutDailyRewardInfo);
	}

	//! 클리어 보상 정보를 반환한다
	public bool TryGetClearRewardInfo(ERewardKinds a_eRewardKinds, out STRewardInfo a_stOutClearRewardInfo) {
		return this.TryGetRewardInfo(a_eRewardKinds, this.ClearRewardInfoList, out a_stOutClearRewardInfo);
	}

	//! 보상 정보를 로드한다
	public List<object> LoadRewardInfos() {
#if UNITY_EDITOR || UNITY_STANDALONE
		return this.LoadRewardInfos(KDefine.G_RUNTIME_TABLE_P_REWARD_INFO);
#else
		return this.LoadRewardInfos(KCDefine.U_TABLE_P_G_REWARD_INFO);
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	}

	//! 보상 정보를 로드한다
	public List<object> LoadRewardInfos(string a_oFilePath) {
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

	//! 보상 정보를 설정한다
	private void SetupRewardInfos(List<STRewardInfo> a_oRewardInfoList, Dictionary<ERewardKinds, STRewardInfo> a_oOutRewardInfoList) {
		CAccess.Assert(a_oRewardInfoList != null && a_oOutRewardInfoList != null);

		for(int i = 0; i < a_oRewardInfoList.Count; ++i) {
			var stRewardInfo = a_oRewardInfoList[i];
			a_oOutRewardInfoList.ExAddVal(stRewardInfo.m_eRewardKinds, stRewardInfo);
		}
	}

	//! 보상 정보를 반환한다
	private STRewardInfo GetRewardInfo(ERewardKinds a_eRewardKinds, Dictionary<ERewardKinds, STRewardInfo> a_oRewardInfoList) {
		bool bIsValid = this.TryGetRewardInfo(a_eRewardKinds, a_oRewardInfoList, out STRewardInfo stRewardInfo);
		CAccess.Assert(bIsValid);

		return stRewardInfo;
	}

	//! 보상 정보를 반환한다
	private bool TryGetRewardInfo(ERewardKinds a_eRewardKinds, Dictionary<ERewardKinds, STRewardInfo> a_oRewardInfoList, out STRewardInfo a_stOutRewardInfo) {
		CAccess.Assert(a_oRewardInfoList != null);
		a_stOutRewardInfo = a_oRewardInfoList.ExGetVal(a_eRewardKinds, KDefine.G_INVALID_REWARD_INFO);

		return a_oRewardInfoList.ContainsKey(a_eRewardKinds);
	}

	//! 보상 정보를 로드한다
	private List<object> DoLoadRewardInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		
		var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr) as SimpleJSON.JSONClass;
		var oFreeRewardInfos = oJSONNode[KDefine.G_KEY_REWARD_IT_FREE];
		var oDailyRewardInfos = oJSONNode[KDefine.G_KEY_REWARD_IT_DAILY];
		var oClearRewardInfos = oJSONNode[KDefine.G_KEY_REWARD_IT_CLEAR];

		this.DoLoadRewardInfos(oFreeRewardInfos, this.FreeRewardInfoList);
		this.DoLoadRewardInfos(oDailyRewardInfos, this.DailyRewardInfoList);
		this.DoLoadRewardInfos(oClearRewardInfos, this.ClearRewardInfoList);

#if UNITY_EDITOR
		this.SetupRewardInfoList(this.FreeRewardInfoList, m_oFreeRewardInfoList);
		this.SetupRewardInfoList(this.DailyRewardInfoList, m_oDailyRewardInfoList);
		this.SetupRewardInfoList(this.ClearRewardInfoList, m_oClearRewardInfoList);
#endif			// #if UNITY_EDITOR

		return new List<object>() {
			this.FreeRewardInfoList, this.DailyRewardInfoList, this.ClearRewardInfoList
		};
	}

	//! 보상 정보를 로드한다
	private Dictionary<ERewardKinds, STRewardInfo> DoLoadRewardInfos(SimpleJSON.JSONNode a_oRewardInfos, Dictionary<ERewardKinds, STRewardInfo> a_oOutRewardInfoList) {
		CAccess.Assert(a_oRewardInfos != null && a_oOutRewardInfoList != null);

		for(int i = 0; i < a_oRewardInfos.Count; ++i) {
			var stRewardInfo = new STRewardInfo(a_oRewardInfos[i]);
			bool bIsReplace = a_oRewardInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT;

			// 보상 정보가 추가 가능 할 경우
			if(bIsReplace || !a_oOutRewardInfoList.ContainsKey(stRewardInfo.m_eRewardKinds)) {
				a_oOutRewardInfoList.ExReplaceVal(stRewardInfo.m_eRewardKinds, stRewardInfo);
			}
		}

		return a_oOutRewardInfoList;
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR
		// 보상 정보를 설정한다
		private void SetupRewardInfoList(Dictionary<ERewardKinds, STRewardInfo> a_oRewardInfoList, List<STRewardInfo> a_oOutRewardInfoList) {
			CAccess.Assert(a_oRewardInfoList != null && a_oOutRewardInfoList != null);
			a_oOutRewardInfoList.Clear();

			foreach(var stKeyVal in a_oRewardInfoList) {
				a_oOutRewardInfoList.ExAddVal(stKeyVal.Value);
			}

			a_oOutRewardInfoList.Sort((a_stLhs, a_stRhs) => (int)a_stLhs.m_eRewardKinds - (int)a_stRhs.m_eRewardKinds);
		}
#endif			// #if UNITY_EDITOR
	#endregion			// 조건부 함수
}
#endif			// #if NEVER_USE_THIS
