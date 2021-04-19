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
	public STRewardInfo(SimpleJSON.JSONNode a_oNode) {
		m_oName = a_oNode[KDefine.G_KEY_REWARD_IT_NAME];
		m_oDesc = a_oNode[KDefine.G_KEY_REWARD_IT_DESC];

		m_eRewardType = (ERewardType)a_oNode[KDefine.G_KEY_REWARD_IT_REWARD_TYPE].AsInt;
		m_eRewardKinds = (ERewardKinds)a_oNode[KDefine.G_KEY_REWARD_IT_REWARD_KINDS].AsInt;

		m_oItemInfoList = new List<STItemInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_REWARD_IT_ITEM_INFOS; ++i) {
			string oNumItemsKey = string.Format(KDefine.G_KEY_FMT_REWARD_IT_NUM_ITEMS, i + KCDefine.B_VAL_1_INT);
			string oItemKindsKey = string.Format(KDefine.G_KEY_FMT_REWARD_IT_ITEM_KINDS, i + KCDefine.B_VAL_1_INT);

			var stItemInfo = new STItemInfo() {
				m_nNumItems = a_oNode[oNumItemsKey].AsInt,
				m_eItemKinds = (EItemKinds)a_oNode[oItemKindsKey].AsInt
			};

			m_oItemInfoList.Add(stItemInfo);
		}
	}
	#endregion			// 함수
}

//! 보상 정보 테이블
public class CRewardInfoTable : CScriptableObj<CRewardInfoTable> {
	#region 변수
	[Header("Clear Reward Info")]
	[SerializeField] private List<STRewardInfo> m_oClearRewardInfoList = new List<STRewardInfo>();

	[Header("Free Reward Info")]
	[SerializeField] private List<STRewardInfo> m_oFreeRewardInfoList = new List<STRewardInfo>();

	[Header("Daily Reward Info")]
	[SerializeField] private List<STRewardInfo> m_oDailyRewardInfoList = new List<STRewardInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public List<STRewardInfo> ClearRewardInfoList => m_oClearRewardInfoList;
	public List<STRewardInfo> FreeRewardInfoList => m_oFreeRewardInfoList;
	public List<STRewardInfo> DailyRewardInfoList => m_oDailyRewardInfoList;
	#endregion			// 프로퍼티

	#region 함수
	//! 보상 정보를 반환한다
	public STRewardInfo GetClearRewardInfo(ERewardKinds a_eRewardKinds) {
		return this.GetRewardInfo(a_eRewardKinds, m_oClearRewardInfoList);
	}

	//! 무료 보상 정보를 반환한다
	public STRewardInfo GetFreeRewardInfo(ERewardKinds a_eRewardKinds) {
		return this.GetRewardInfo(a_eRewardKinds, m_oFreeRewardInfoList);
	}

	//! 일일 보상 정보를 반환한다
	public STRewardInfo GetDailyRewardInfo(ERewardKinds a_eRewardKinds) {
		return this.GetRewardInfo(a_eRewardKinds, m_oDailyRewardInfoList);
	}
	
	//! 보상 정보를 반환한다
	public bool TryGetClearRewardInfo(ERewardKinds a_eRewardKinds, out STRewardInfo a_stOutClearRewardInfo) {
		return this.TryGetRewardInfo(a_eRewardKinds, out a_stOutClearRewardInfo, m_oClearRewardInfoList);
	}

	//! 무료 보상 정보를 반환한다
	public bool TryGetFreeRewardInfo(ERewardKinds a_eRewardKinds, out STRewardInfo a_stOutFreeRewardInfo) {
		return this.TryGetRewardInfo(a_eRewardKinds, out a_stOutFreeRewardInfo, m_oFreeRewardInfoList);
	}

	//! 일일 보상 정보를 반환한다
	public bool TryGetDailyRewardInfo(ERewardKinds a_eRewardKinds, out STRewardInfo a_stOutDailyRewardInfo) {
		return this.TryGetRewardInfo(a_eRewardKinds, out a_stOutDailyRewardInfo, m_oDailyRewardInfoList);
	}

	//! 보상 정보를 로드한다
	public List<STRewardInfo> LoadClearRewardInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		return this.LoadRewardInfos(a_oJSONStr, m_oClearRewardInfoList);
	}

	//! 무료 보상 정보를 로드한다
	public List<STRewardInfo> LoadFreeRewardInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		return this.LoadRewardInfos(a_oJSONStr, m_oFreeRewardInfoList);
	}

	//! 일일 보상 정보를 로드한다
	public List<STRewardInfo> LoadDailyRewardInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		return this.LoadRewardInfos(a_oJSONStr, m_oDailyRewardInfoList);
	}

	//! 보상 정보를 로드한다
	public List<STRewardInfo> LoadClearRewardInfosFromFile(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return this.LoadRewardInfosFromFile(a_oFilePath, m_oClearRewardInfoList);
	}

	//! 무료 보상 정보를 로드한다
	public List<STRewardInfo> LoadFreeRewardInfosFromFile(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return this.LoadRewardInfosFromFile(a_oFilePath, m_oFreeRewardInfoList);
	}

	//! 일일 보상 정보를 로드한다
	public List<STRewardInfo> LoadDailyRewardInfosFromFile(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return this.LoadRewardInfosFromFile(a_oFilePath, m_oDailyRewardInfoList);
	}

	//! 보상 정보를 로드한다
	public List<STRewardInfo> LoadClearRewardInfosFromRes(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return this.LoadRewardInfosFromRes(a_oFilePath, m_oClearRewardInfoList);
	}

	//! 무료 보상 정보를 로드한다
	public List<STRewardInfo> LoadFreeRewardInfosFromRes(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return this.LoadRewardInfosFromRes(a_oFilePath, m_oFreeRewardInfoList);
	}

	//! 일일 보상 정보를 로드한다
	public List<STRewardInfo> LoadDailyRewardInfosFromRes(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return this.LoadRewardInfosFromRes(a_oFilePath, m_oDailyRewardInfoList);
	}

	//! 보상 정보를 반환한다
	private STRewardInfo GetRewardInfo(ERewardKinds a_eRewardKinds, List<STRewardInfo> a_oRewardInfoList) {
		bool bIsValid = this.TryGetRewardInfo(a_eRewardKinds, out STRewardInfo stRewardInfo, a_oRewardInfoList);
		CAccess.Assert(bIsValid);

		return stRewardInfo;
	}

	//! 보상 정보를 반환한다
	private bool TryGetRewardInfo(ERewardKinds a_eRewardKinds, out STRewardInfo a_stOutRewardInfo, List<STRewardInfo> a_oRewardInfoList) {
		int nIdx = a_oRewardInfoList.ExFindValue((a_stRewardInfo) => a_stRewardInfo.m_eRewardKinds == a_eRewardKinds);
		a_stOutRewardInfo = a_oRewardInfoList.ExIsValidIdx(nIdx) ? a_oRewardInfoList[nIdx] : KDefine.G_INVALID_REWARD_INFO;

		return a_oRewardInfoList.ExIsValidIdx(nIdx);
	}

	//! 보상 정보를 로드한다
	private List<STRewardInfo> LoadRewardInfos(string a_oJSONStr, List<STRewardInfo> a_oOutRewardInfoList) {
		CAccess.Assert(a_oJSONStr.ExIsValid() && a_oOutRewardInfoList != null);

		var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr);
		var oRewardInfos = oJSONNode[KCDefine.B_KEY_JSON_COMMON_DATA];

		for(int i = 0; i < oRewardInfos.Count; ++i) {
			var stRewardInfo = new STRewardInfo(oRewardInfos[i]);
			a_oOutRewardInfoList.Add(stRewardInfo);
		}

		return a_oOutRewardInfoList;
	}

	//! 보상 정보를 로드한다
	private List<STRewardInfo> LoadRewardInfosFromFile(string a_oFilePath, List<STRewardInfo> a_oOutRewardInfoList) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		string oJSONStr = CFunc.ReadStr(a_oFilePath);

		return this.LoadRewardInfos(oJSONStr, a_oOutRewardInfoList);
	}

	//! 보상 정보를 로드한다
	private List<STRewardInfo> LoadRewardInfosFromRes(string a_oFilePath, List<STRewardInfo> a_oOutRewardInfoList) {
		CAccess.Assert(a_oFilePath.ExIsValid());

		try {
			var oTextAsset = CResManager.Inst.GetRes<TextAsset>(a_oFilePath);
			return this.LoadRewardInfos(oTextAsset.text, a_oOutRewardInfoList);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
