#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 보상 정보 */
[System.Serializable]
public struct STRewardInfo {
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
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oAcquireTargetInfoDict.TryAdd(Factory.MakeUTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}
	}
	#endregion			// 함수
}

/** 보상 정보 테이블 */
public partial class CRewardInfoTable : CSingleton<CRewardInfoTable> {
	#region 프로퍼티
	public Dictionary<ERewardKinds, STRewardInfo> RewardInfoDict { get; } = new Dictionary<ERewardKinds, STRewardInfo>();
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetRewardInfos();
	}

	/** 보상 정보를 리셋한다 */
	public virtual void ResetRewardInfos() {
		this.RewardInfoDict.Clear();
	}

	/** 보상 정보를 리셋한다 */
	public virtual void ResetRewardInfos(string a_oJSONStr) {
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
		a_stOutAcquireTargetInfo = this.TryGetRewardInfo(a_eRewardKinds, out STRewardInfo stRewardInfo) ? stRewardInfo.m_oAcquireTargetInfoDict.GetValueOrDefault(Factory.MakeUTargetInfoID(a_eTargetKinds, a_nKinds), STTargetInfo.INVALID) : STTargetInfo.INVALID;
		return a_stOutAcquireTargetInfo.m_eTargetKinds != ETargetKinds.NONE;
	}

	/** 보상 정보를 로드한다 */
	public Dictionary<ERewardKinds, STRewardInfo> LoadRewardInfos() {
		this.ResetRewardInfos();
		return this.LoadRewardInfos(Access.RewardInfoTableLoadPath);
	}

	/** 보상 정보를 저장한다 */
	public void SaveRewardInfos(string a_oJSONStr, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oJSONStr != null);

		// JSON 문자열이 존재 할 경우
		if(a_oJSONStr != null) {
			this.ResetRewardInfos(a_oJSONStr);
			
#if UNITY_EDITOR || (UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD))
			CFunc.WriteStr(Access.RewardInfoTableSavePath, a_oJSONStr, false);
#else
			CFunc.WriteStr(Access.RewardInfoTableSavePath, a_oJSONStr, true);
#endif			// #if UNITY_EDITOR || (UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD))
		}
	}

	/** JSON 노드를 설정한다 */
	private void SetupJSONNodes(SimpleJSON.JSONNode a_oJSONNode, out List<SimpleJSON.JSONNode> a_oOutRewardInfosList) {
		a_oOutRewardInfosList = new List<SimpleJSON.JSONNode>();
		var oTableInfoDictContainer = KDefine.G_KEY_TABLE_DICT_CONTAINER[Path.GetFileNameWithoutExtension(Access.RewardInfoTableLoadPath)];

		for(int i = 0; i < oTableInfoDictContainer[this.GetType()][KCDefine.B_KEY_COMMON].Count; ++i) {
			a_oOutRewardInfosList.ExAddVal(a_oJSONNode[oTableInfoDictContainer[this.GetType()][KCDefine.B_KEY_COMMON][i]]);
		}
	}

	/** 보상 정보를 로드한다 */
	private Dictionary<ERewardKinds, STRewardInfo> LoadRewardInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_EDITOR || (UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD))
		return this.DoLoadRewardInfos(File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, false) : CFunc.ReadStrFromRes(a_oFilePath, false));
#else
		return this.DoLoadRewardInfos(File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, true) : CFunc.ReadStrFromRes(a_oFilePath, true));
#endif			// #if UNITY_EDITOR || (UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD))
	}

	/** 보상 정보를 로드한다 */
	private Dictionary<ERewardKinds, STRewardInfo> DoLoadRewardInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		this.SetupJSONNodes(SimpleJSON.JSON.Parse(a_oJSONStr), out List<SimpleJSON.JSONNode> oRewardInfosList);

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
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
