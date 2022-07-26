using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 보상 정보 */
[System.Serializable]
public partial struct STRewardInfo {
	public STCommonInfo m_stCommonInfo;

	public ERewardKinds m_eRewardKinds;
	public ERewardKinds m_ePrevRewardKinds;
	public ERewardKinds m_eNextRewardKinds;
	public ERewardQuality m_eRewardQuality;

	public Dictionary<ulong, STTargetInfo> m_oAcquireTargetInfoDict;

	#region 상수
	public static STRewardInfo INVALID = new STRewardInfo() {
		m_eRewardKinds = ERewardKinds.NONE, m_ePrevRewardKinds = ERewardKinds.NONE, m_eNextRewardKinds = ERewardKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public ERewardType RewardType => (ERewardType)((int)m_eRewardKinds).ExKindsToType();
	public ERewardKinds BaseRewardKinds => (ERewardKinds)((int)m_eRewardKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STRewardInfo(SimpleJSON.JSONNode a_oRewardInfo) {
		m_stCommonInfo = new STCommonInfo(a_oRewardInfo);
		
		m_eRewardKinds = a_oRewardInfo[KCDefine.U_KEY_REWARD_KINDS].ExIsValid() ? (ERewardKinds)a_oRewardInfo[KCDefine.U_KEY_REWARD_KINDS].AsInt : ERewardKinds.NONE;
		m_ePrevRewardKinds = a_oRewardInfo[KCDefine.U_KEY_PREV_REWARD_KINDS].ExIsValid() ? (ERewardKinds)a_oRewardInfo[KCDefine.U_KEY_PREV_REWARD_KINDS].AsInt : ERewardKinds.NONE;
		m_eNextRewardKinds = a_oRewardInfo[KCDefine.U_KEY_NEXT_REWARD_KINDS].ExIsValid() ? (ERewardKinds)a_oRewardInfo[KCDefine.U_KEY_NEXT_REWARD_KINDS].AsInt : ERewardKinds.NONE;
		m_eRewardQuality = a_oRewardInfo[KCDefine.U_KEY_REWARD_QUALITY].ExIsValid() ? (ERewardQuality)a_oRewardInfo[KCDefine.U_KEY_REWARD_QUALITY].AsInt : ERewardQuality.NONE;

		m_oAcquireTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oRewardInfo[string.Format(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oAcquireTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}
	}
	#endregion			// 함수
}

/** 보상 정보 테이블 */
public partial class CRewardInfoTable : CSingleton<CRewardInfoTable> {
	#region 변수
	[Header("=====> Free Reward Info <=====")]
	[SerializeField] private List<STRewardInfo> m_oFreeRewardInfoList = new List<STRewardInfo>();

	[Header("=====> Daily Reward Info <=====")]
	[SerializeField] private List<STRewardInfo> m_oDailyRewardInfoList = new List<STRewardInfo>();

	[Header("=====> Event Reward Info <=====")]
	[SerializeField] private List<STRewardInfo> m_oEventRewardInfoList = new List<STRewardInfo>();

	[Header("=====> Clear Reward Info <=====")]
	[SerializeField] private List<STRewardInfo> m_oClearRewardInfoList = new List<STRewardInfo>();

	[Header("=====> Mission Reward Info <=====")]
	[SerializeField] private List<STRewardInfo> m_oMissionRewardInfoList = new List<STRewardInfo>();

	[Header("=====> Tutorial Reward Info <=====")]
	[SerializeField] private List<STRewardInfo> m_oTutorialRewardInfoList = new List<STRewardInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<ERewardKinds, STRewardInfo> RewardInfoDict { get; private set; } = new Dictionary<ERewardKinds, STRewardInfo>();

	private string RewardInfoTablePath {
		get {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_REWARD_INFO;
#else
			return KCDefine.U_TABLE_P_G_REWARD_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetRewardInfos();
	}

	/** 보상 정보를 리셋한다 */
	public void ResetRewardInfos() {
		this.RewardInfoDict.Clear();

		var oRewardInfoList = new List<STRewardInfo>(m_oFreeRewardInfoList);
		oRewardInfoList.AddRange(m_oDailyRewardInfoList);
		oRewardInfoList.AddRange(m_oEventRewardInfoList);
		oRewardInfoList.AddRange(m_oClearRewardInfoList);
		oRewardInfoList.AddRange(m_oMissionRewardInfoList);
		oRewardInfoList.AddRange(m_oTutorialRewardInfoList);

		for(int i = 0; i < oRewardInfoList.Count; ++i) {
			this.RewardInfoDict.TryAdd(oRewardInfoList[i].m_eRewardKinds, oRewardInfoList[i]);
		}
	}

	/** 보상 정보를 리셋한다 */
	public void ResetRewardInfos(string a_oJSONStr) {
		this.ResetRewardInfos();
		this.DoLoadRewardInfos(a_oJSONStr);
	}

	/** 보상 정보를 반환한다 */
	public STRewardInfo GetRewardInfo(ERewardKinds a_eRewardKinds) {
		bool bIsValid = this.TryGetRewardInfo(a_eRewardKinds, out STRewardInfo stRewardInfo);
		CAccess.Assert(bIsValid);

		return stRewardInfo;
	}

	/** 획득 타겟 정보를 반환한다 */
	public STTargetInfo GetAcquireTargetInfo(ERewardKinds a_eRewardKinds, ETargetKinds a_eTargetKinds, int a_nKinds) {
		bool bIsValid = this.TryGetAcquireTargetInfo(a_eRewardKinds, a_eTargetKinds, a_nKinds, out STTargetInfo stAcquireTargetInfo);
		CAccess.Assert(bIsValid);

		return stAcquireTargetInfo;
	}

	/** 보상 정보를 반환한다 */
	public bool TryGetRewardInfo(ERewardKinds a_eRewardKinds, out STRewardInfo a_stOutRewardInfo) {
		a_stOutRewardInfo = this.RewardInfoDict.GetValueOrDefault(a_eRewardKinds, STRewardInfo.INVALID);
		return this.RewardInfoDict.ContainsKey(a_eRewardKinds);
	}

	/** 획득 타겟 정보를 반환한다 */
	public bool TryGetAcquireTargetInfo(ERewardKinds a_eRewardKinds, ETargetKinds a_eTargetKinds, int a_nKinds, out STTargetInfo a_stOutAcquireTargetInfo) {
		a_stOutAcquireTargetInfo = this.TryGetRewardInfo(a_eRewardKinds, out STRewardInfo stRewardInfo) ? stRewardInfo.m_oAcquireTargetInfoDict.GetValueOrDefault(Factory.MakeUniqueTargetInfoID(a_eTargetKinds, a_nKinds), STTargetInfo.INVALID) : STTargetInfo.INVALID;
		return !a_stOutAcquireTargetInfo.Equals(STTargetInfo.INVALID);
	}

	/** 보상 정보를 로드한다 */
	public Dictionary<ERewardKinds, STRewardInfo> LoadRewardInfos() {
		this.ResetRewardInfos();
		return this.LoadRewardInfos(this.RewardInfoTablePath);
	}

	/** 보상 정보를 로드한다 */
	private Dictionary<ERewardKinds, STRewardInfo> LoadRewardInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadRewardInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			return this.DoLoadRewardInfos(CResManager.Inst.GetRes<TextAsset>(a_oFilePath).text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 보상 정보를 로드한다 */
	private Dictionary<ERewardKinds, STRewardInfo> DoLoadRewardInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());

		var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr);
		var oRewardInfosList = new List<SimpleJSON.JSONNode>();

		for(int i = 0; i < KDefine.G_KEY_REWARD_INFOS.Count; ++i) {
			oRewardInfosList.ExAddVal(oJSONNode[KDefine.G_KEY_REWARD_INFOS[i]]);
		}

		for(int i = 0; i < oRewardInfosList.Count; ++i) {
			for(int j = 0; j < oRewardInfosList[i].Count; ++j) {
				var stRewardInfo = new STRewardInfo(oRewardInfosList[i][j]);

				// 보상 정보 추가 가능 할 경우
				if(stRewardInfo.m_eRewardKinds.ExIsValid() && (!this.RewardInfoDict.ContainsKey(stRewardInfo.m_eRewardKinds) || oRewardInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.RewardInfoDict.ExReplaceVal(stRewardInfo.m_eRewardKinds, stRewardInfo);
				}
			}
		}
		
		return this.RewardInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
