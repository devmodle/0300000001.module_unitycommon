using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
using Newtonsoft.Json;

/** 에피소드 정보 */
[System.Serializable]
public struct STEpisodeInfo {
	public STIDInfo m_stIDInfo;
	public STDescInfo m_stDescInfo;

	public int m_nPrevID;
	public int m_nNextID;

	public EDifficulty m_eDifficulty;
	public ERewardKinds m_eRewardKinds;
	public EEpisodeKinds m_eEpisodeKinds;
	public ETutorialKinds m_eTutorialKinds;

	public List<int> m_oRecordList;
	public Dictionary<ETargetKinds, int> m_oNumTargetsDict;
	public Dictionary<ETargetKinds, int> m_oNumUnlockTargetsDict;

	#region 함수
	/** 생성자 */
	public STEpisodeInfo(SimpleJSON.JSONNode a_oEpisodeInfo) {
		m_stIDInfo = new STIDInfo(a_oEpisodeInfo);
		m_stDescInfo = new STDescInfo(a_oEpisodeInfo);

		m_nPrevID = a_oEpisodeInfo[KCDefine.U_KEY_PREV_ID].ExIsValid() ? a_oEpisodeInfo[KCDefine.U_KEY_PREV_ID].AsInt : KCDefine.B_IDX_INVALID;
		m_nNextID = a_oEpisodeInfo[KCDefine.U_KEY_NEXT_ID].ExIsValid() ? a_oEpisodeInfo[KCDefine.U_KEY_NEXT_ID].AsInt : KCDefine.B_IDX_INVALID;

		m_eDifficulty = a_oEpisodeInfo[KCDefine.U_KEY_DIFFICULTY].ExIsValid() ? (EDifficulty)a_oEpisodeInfo[KCDefine.U_KEY_DIFFICULTY].AsInt : EDifficulty.NONE;
		m_eRewardKinds = a_oEpisodeInfo[KCDefine.U_KEY_REWARD_KINDS].ExIsValid() ? (ERewardKinds)a_oEpisodeInfo[KCDefine.U_KEY_REWARD_KINDS].AsInt : ERewardKinds.NONE;
		m_eEpisodeKinds = a_oEpisodeInfo[KCDefine.U_KEY_EPISODE_KINDS].ExIsValid() ? (EEpisodeKinds)a_oEpisodeInfo[KCDefine.U_KEY_EPISODE_KINDS].AsInt : EEpisodeKinds.NONE;
		m_eTutorialKinds = a_oEpisodeInfo[KCDefine.U_KEY_TUTORIAL_KINDS].ExIsValid() ? (ETutorialKinds)a_oEpisodeInfo[KCDefine.U_KEY_TUTORIAL_KINDS].AsInt : ETutorialKinds.NONE;

		m_oRecordList = new List<int>();
		m_oNumTargetsDict = new Dictionary<ETargetKinds, int>();
		m_oNumUnlockTargetsDict = new Dictionary<ETargetKinds, int>();

		for(int i = 0; i < KDefine.G_MAX_NUM_LEVEL_MARKS; ++i) {
			string oRecordKey = string.Format(KCDefine.U_KEY_FMT_RECORD, i + KCDefine.B_VAL_1_INT);
			m_oRecordList.ExAddVal(a_oEpisodeInfo[oRecordKey].AsInt);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_LEVEL_TARGET_KINDS; ++i) {
			string oNumTargetsKey = string.Format(KCDefine.U_KEY_FMT_NUM_TARGETS, i + KCDefine.B_VAL_1_INT);
			string oTargetKindsKey = string.Format(KCDefine.U_KEY_FMT_TARGET_KINDS, i + KCDefine.B_VAL_1_INT);
			
			m_oNumTargetsDict.TryAdd(a_oEpisodeInfo[oTargetKindsKey].ExIsValid() ? (ETargetKinds)a_oEpisodeInfo[oTargetKindsKey].AsInt : ETargetKinds.NONE, a_oEpisodeInfo[oNumTargetsKey].AsInt);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_LEVEL_UNLOCK_TARGET_KINDS; ++i) {
			string oNumUnlockTargetsKey = string.Format(KCDefine.U_KEY_FMT_NUM_UNLOCK_TARGETS, i + KCDefine.B_VAL_1_INT);
			string oUnlockTargetKindsKey = string.Format(KCDefine.U_KEY_FMT_UNLOCK_TARGET_KINDS, i + KCDefine.B_VAL_1_INT);

			m_oNumUnlockTargetsDict.TryAdd(a_oEpisodeInfo[oUnlockTargetKindsKey].ExIsValid() ? (ETargetKinds)a_oEpisodeInfo[oUnlockTargetKindsKey].AsInt : ETargetKinds.NONE, a_oEpisodeInfo[oNumUnlockTargetsKey].AsInt);
		}
	}

	/** 기록을 반환한다 */
	public int GetRecord(int a_nIdx) {
		return m_oRecordList.ExGetVal(a_nIdx, KCDefine.B_VAL_0_INT);
	}

	/** 타겟 개수를 반환한다 */
	public int GetNumTargets(ETargetKinds a_eTargetKinds) {
		return m_oNumTargetsDict.GetValueOrDefault(a_eTargetKinds, KCDefine.B_VAL_0_INT);
	}

	/** 잠금 해제 타겟 개수를 반환한다 */
	public int GetNumUnlockTargets(ETargetKinds a_eTargetKinds) {
		return m_oNumUnlockTargetsDict.GetValueOrDefault(a_eTargetKinds, KCDefine.B_VAL_0_INT);
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR || UNITY_STANDALONE
	/** 레벨 에피소드 정보를 생성한다 */
	public SimpleJSON.JSONClass MakeLevelEpisodeInfo() {
		var oLevelEpisodeInfo = new SimpleJSON.JSONClass();
		oLevelEpisodeInfo.Add(KCDefine.U_KEY_ID, $"{m_stIDInfo.m_nID}");
		oLevelEpisodeInfo.Add(KCDefine.U_KEY_STAGE_ID, $"{m_stIDInfo.m_nStageID}");
		oLevelEpisodeInfo.Add(KCDefine.U_KEY_CHAPTER_ID, $"{m_stIDInfo.m_nChapterID}");

		this.MakeEpisodeInfo(oLevelEpisodeInfo);
		return oLevelEpisodeInfo;
	}

	/** 스테이지 에피소드 정보를 생성한다 */
	public SimpleJSON.JSONClass MakeStageEpisodeInfo() {
		var oStageEpisodeInfo = new SimpleJSON.JSONClass();
		oStageEpisodeInfo.Add(KCDefine.U_KEY_ID, $"{m_stIDInfo.m_nID}");
		oStageEpisodeInfo.Add(KCDefine.U_KEY_CHAPTER_ID, $"{m_stIDInfo.m_nChapterID}");

		this.MakeEpisodeInfo(oStageEpisodeInfo);
		return oStageEpisodeInfo;
	}

	/** 챕터 에피소드 정보를 생성한다 */
	public SimpleJSON.JSONClass MakeChapterEpisodeInfo() {
		var oChapterEpisodeInfo = new SimpleJSON.JSONClass();
		oChapterEpisodeInfo.Add(KCDefine.U_KEY_ID, $"{m_stIDInfo.m_nID}");
		
		this.MakeEpisodeInfo(oChapterEpisodeInfo);
		return oChapterEpisodeInfo;
	}

	/** 에피소드 정보를 생성한다 */
	private void MakeEpisodeInfo(SimpleJSON.JSONClass a_oOutEpisodeInfo) {
		m_stDescInfo.MakeDescInfo(a_oOutEpisodeInfo);

		a_oOutEpisodeInfo.Add(KCDefine.U_KEY_PREV_ID, $"{m_nPrevID}");
		a_oOutEpisodeInfo.Add(KCDefine.U_KEY_NEXT_ID, $"{m_nNextID}");

		a_oOutEpisodeInfo.Add(KCDefine.U_KEY_DIFFICULTY, $"{(int)m_eDifficulty}");
		a_oOutEpisodeInfo.Add(KCDefine.U_KEY_REWARD_KINDS, $"{(int)m_eRewardKinds}");
		a_oOutEpisodeInfo.Add(KCDefine.U_KEY_TUTORIAL_KINDS, $"{(int)m_eTutorialKinds}");

		var oNumTargetsKeyList = m_oNumTargetsDict.Keys.ToList();
		var oNumUnlockTargetsKeyList = m_oNumUnlockTargetsDict.Keys.ToList();

		for(int i = 0; i < m_oRecordList.Count; ++i) {
			a_oOutEpisodeInfo.Add(string.Format(KCDefine.U_KEY_FMT_RECORD, i + KCDefine.B_VAL_1_INT), $"{m_oRecordList.ExGetVal(i, KCDefine.B_VAL_0_INT)}");
		}

		for(int i = 0; i < m_oNumTargetsDict.Count && i < oNumTargetsKeyList.Count; ++i) {
			var eKey = oNumTargetsKeyList.ExGetVal(i, ETargetKinds.NONE);
			int nVal = m_oNumTargetsDict.GetValueOrDefault(eKey, KCDefine.B_VAL_0_INT);

			a_oOutEpisodeInfo.Add(string.Format(KCDefine.U_KEY_FMT_NUM_TARGETS, i + KCDefine.B_VAL_1_INT), $"{nVal}");
			a_oOutEpisodeInfo.Add(string.Format(KCDefine.U_KEY_FMT_TARGET_KINDS, i + KCDefine.B_VAL_1_INT), $"{(int)eKey}");
		}

		for(int i = 0; i < m_oNumUnlockTargetsDict.Count && i < oNumUnlockTargetsKeyList.Count; ++i) {
			var eKey = oNumUnlockTargetsKeyList.ExGetVal(i, ETargetKinds.NONE);
			int nVal = m_oNumUnlockTargetsDict.GetValueOrDefault(eKey, KCDefine.B_VAL_0_INT);

			a_oOutEpisodeInfo.Add(string.Format(KCDefine.U_KEY_FMT_NUM_UNLOCK_TARGETS, i + KCDefine.B_VAL_1_INT), $"{nVal}");
			a_oOutEpisodeInfo.Add(string.Format(KCDefine.U_KEY_FMT_UNLOCK_TARGET_KINDS, i + KCDefine.B_VAL_1_INT), $"{(int)eKey}");
		}
	}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	#endregion			// 조건부 함수
}

/** 에피소드 정보 테이블 */
public partial class CEpisodeInfoTable : CScriptableObj<CEpisodeInfoTable> {
	#region 변수
	[Header("=====> Level Episode Info <=====")]
	[SerializeField] private List<STEpisodeInfo> m_oLevelEpisodeInfoList = new List<STEpisodeInfo>();

	[Header("=====> Stage Episode Info <=====")]
	[SerializeField] private List<STEpisodeInfo> m_oStageEpisodeInfoList = new List<STEpisodeInfo>();

	[Header("=====> Chapter Episode Info <=====")]
	[SerializeField] private List<STEpisodeInfo> m_oChapterEpisodeInfoList = new List<STEpisodeInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<long, STEpisodeInfo> LevelEpisodeInfoDict { get; private set; } = new Dictionary<long, STEpisodeInfo>();
	public Dictionary<long, STEpisodeInfo> StageEpisodeInfoDict { get; private set; } = new Dictionary<long, STEpisodeInfo>();
	public Dictionary<long, STEpisodeInfo> ChapterEpisodeInfoDict { get; private set; } = new Dictionary<long, STEpisodeInfo>();

	private string EpisodeInfoTablePath {
		get {
#if AB_TEST_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return (CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.A) ? KCDefine.U_RUNTIME_TABLE_P_G_EPISODE_INFO_SET_A : KCDefine.U_RUNTIME_TABLE_P_G_EPISODE_INFO_SET_B;
#else
			return (CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.A) ? KCDefine.U_TABLE_P_G_EPISODE_INFO_SET_A : KCDefine.U_TABLE_P_G_EPISODE_INFO_SET_B;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
#else
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_EPISODE_INFO;
#else
			return KCDefine.U_TABLE_P_G_EPISODE_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
#endif			// #if AB_TEST_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		for(int i = 0; i < m_oLevelEpisodeInfoList.Count; ++i) {
			this.LevelEpisodeInfoDict.TryAdd(CFactory.MakeUniqueLevelID(i, m_oLevelEpisodeInfoList[i].m_stIDInfo.m_nStageID, m_oLevelEpisodeInfoList[i].m_stIDInfo.m_nChapterID), m_oLevelEpisodeInfoList[i]);
		}

		for(int i = 0; i < m_oStageEpisodeInfoList.Count; ++i) {
			this.StageEpisodeInfoDict.TryAdd(CFactory.MakeUniqueStageID(i, m_oStageEpisodeInfoList[i].m_stIDInfo.m_nChapterID), m_oStageEpisodeInfoList[i]);
		}

		for(int i = 0; i < m_oChapterEpisodeInfoList.Count; ++i) {
			this.ChapterEpisodeInfoDict.TryAdd(CFactory.MakeUniqueChapterID(i), m_oChapterEpisodeInfoList[i]);
		}
	}

	/** 레벨 에피소드 기록을 반환한다 */
	public int GetLevelEpisodeRecord(int a_nID, int a_nIdx, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetLevelEpisodeInfo(a_nID, out STEpisodeInfo stLevelEpisodeInfo, a_nStageID, a_nChapterID);
		return stLevelEpisodeInfo.GetRecord(a_nIdx);
	}

	/** 레벨 에피소드 타겟 개수를 반환한다 */
	public int GetLevelEpisodeNumTargets(int a_nID, ETargetKinds a_eTargetKinds, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetLevelEpisodeInfo(a_nID, out STEpisodeInfo stLevelEpisodeInfo, a_nStageID, a_nChapterID);
		return stLevelEpisodeInfo.GetNumTargets(a_eTargetKinds);
	}

	/** 레벨 에피소드 잠금 해제 타겟 개수를 반환한다 */
	public int GetLevelEpisodeNumUnlockTargets(int a_nID, ETargetKinds a_eTargetKinds, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetLevelEpisodeInfo(a_nID, out STEpisodeInfo stLevelEpisodeInfo, a_nStageID, a_nChapterID);
		return stLevelEpisodeInfo.GetNumUnlockTargets(a_eTargetKinds);
	}

	/** 스테이지 에피소드 기록을 반환한다 */
	public int GetStageEpisodeRecord(int a_nID, int a_nIdx, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetStageEpisodeInfo(a_nID, out STEpisodeInfo stStageEpisodeInfo, a_nChapterID);
		return stStageEpisodeInfo.GetRecord(a_nIdx);
	}

	/** 스테이지 에피소드 타겟 개수를 반환한다 */
	public int GetStageEpisodeNumTargets(int a_nID, ETargetKinds a_eTargetKinds, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetStageEpisodeInfo(a_nID, out STEpisodeInfo stStageEpisodeInfo, a_nChapterID);
		return stStageEpisodeInfo.GetNumTargets(a_eTargetKinds);
	}

	/** 스테이지 에피소드 잠금 해제 타겟 개수를 반환한다 */
	public int GetStageEpisodeNumUnlockTargets(int a_nID, ETargetKinds a_eTargetKinds, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetStageEpisodeInfo(a_nID, out STEpisodeInfo stStageEpisodeInfo, a_nChapterID);
		return stStageEpisodeInfo.GetNumUnlockTargets(a_eTargetKinds);
	}

	/** 챕터 에피소드 기록을 반환한다 */
	public int GetChapterEpisodeRecord(int a_nID, int a_nIdx) {
		this.TryGetChapterEpisodeInfo(a_nID, out STEpisodeInfo stChapterEpisodeInfo);
		return stChapterEpisodeInfo.GetRecord(a_nIdx);
	}

	/** 챕터 에피소드 타겟 개수를 반환한다 */
	public int GetChapterEpisodeNumTargets(int a_nID, ETargetKinds a_eTargetKinds) {
		this.TryGetChapterEpisodeInfo(a_nID, out STEpisodeInfo stChapterEpisodeInfo);
		return stChapterEpisodeInfo.GetNumTargets(a_eTargetKinds);
	}

	/** 챕터 에피소드 잠금 해제 타겟 개수를 반환한다 */
	public int GetChapterEpisodeNumUnlockTargets(int a_nID, ETargetKinds a_eTargetKinds) {
		this.TryGetChapterEpisodeInfo(a_nID, out STEpisodeInfo stChapterEpisodeInfo);
		return stChapterEpisodeInfo.GetNumUnlockTargets(a_eTargetKinds);
	}

	/** 레벨 에피소드 정보를 반환한다 */
	public STEpisodeInfo GetLevelEpisodeInfo(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetLevelEpisodeInfo(a_nID, out STEpisodeInfo stLevelEpisodeInfo, a_nStageID, a_nChapterID);
		CAccess.Assert(bIsValid);

		return stLevelEpisodeInfo;
	}

	/** 스테이지 에피소드 정보를 반환한다 */
	public STEpisodeInfo GetStageEpisodeInfo(int a_nID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetStageEpisodeInfo(a_nID, out STEpisodeInfo stStageEpisodeInfo, a_nChapterID);
		CAccess.Assert(bIsValid);

		return stStageEpisodeInfo;
	}

	/** 챕터 에피소드 정보를 반환한다 */
	public STEpisodeInfo GetChapterEpisodeInfo(int a_nID) {
		bool bIsValid = this.TryGetChapterEpisodeInfo(a_nID, out STEpisodeInfo stChapterEpisodeInfo);
		CAccess.Assert(bIsValid);

		return stChapterEpisodeInfo;
	}

	/** 레벨 에피소드 정보를 반환한다 */
	public bool TryGetLevelEpisodeInfo(int a_nID, out STEpisodeInfo a_stOutLevelEpisodeInfo, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		long nUniqueLevelID = CFactory.MakeUniqueLevelID(a_nID, a_nStageID, a_nChapterID);
		a_stOutLevelEpisodeInfo = this.LevelEpisodeInfoDict.GetValueOrDefault(nUniqueLevelID, default(STEpisodeInfo));

		return this.LevelEpisodeInfoDict.ContainsKey(nUniqueLevelID);
	}

	/** 스테이지 에피소드 정보를 반환한다 */
	public bool TryGetStageEpisodeInfo(int a_nID, out STEpisodeInfo a_stOutStageEpisodeInfo, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		long nUniqueStageID = CFactory.MakeUniqueStageID(a_nID, a_nChapterID);
		a_stOutStageEpisodeInfo = this.StageEpisodeInfoDict.GetValueOrDefault(nUniqueStageID, default(STEpisodeInfo));

		return this.StageEpisodeInfoDict.ContainsKey(nUniqueStageID);
	}

	/** 챕터 에피소드 정보를 반환한다 */
	public bool TryGetChapterEpisodeInfo(int a_nID, out STEpisodeInfo a_stOutChapterEpisodeInfo) {
		long nUniqueChapterID = CFactory.MakeUniqueChapterID(a_nID);
		a_stOutChapterEpisodeInfo = this.ChapterEpisodeInfoDict.GetValueOrDefault(nUniqueChapterID, default(STEpisodeInfo));

		return this.ChapterEpisodeInfoDict.ContainsKey(nUniqueChapterID);
	}

	/** 에피소드 정보를 로드한다 */
	public List<object> LoadEpisodeInfos() {
		return this.LoadEpisodeInfos(this.EpisodeInfoTablePath);
	}

	/** 에피소드 정보를 로드한다 */
	private List<object> LoadEpisodeInfos(string a_oFilePath) {
		CFunc.ShowLog($"CEpisodeInfoTable.LoadEpisodeInfos: {a_oFilePath}");
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadEpisodeInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			return this.DoLoadEpisodeInfos(CResManager.Inst.GetRes<TextAsset>(a_oFilePath).text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 에피소드 정보를 로드한다 */
	private List<object> DoLoadEpisodeInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
				
		var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr) as SimpleJSON.JSONClass;
		var oLevelEpisodeInfos = oJSONNode[KCDefine.U_KEY_LEVEL];
		var oStageEpisodeInfos = oJSONNode[KCDefine.U_KEY_STAGE];
		var oChapterEpisodeInfos = oJSONNode[KCDefine.U_KEY_CHAPTER];

		for(int i = 0; i < oLevelEpisodeInfos.Count; ++i) {
			var stLevelEpisodeInfo = new STEpisodeInfo(oLevelEpisodeInfos[i]);
			long nUniqueLevelID = CFactory.MakeUniqueLevelID(stLevelEpisodeInfo.m_stIDInfo.m_nID, stLevelEpisodeInfo.m_stIDInfo.m_nStageID, stLevelEpisodeInfo.m_stIDInfo.m_nChapterID);

			// 레벨 에피소드 정보가 추가 가능 할 경우
			if(!this.LevelEpisodeInfoDict.ContainsKey(nUniqueLevelID) || oLevelEpisodeInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT) {
				this.LevelEpisodeInfoDict.ExReplaceVal(nUniqueLevelID, stLevelEpisodeInfo);
			}
		}

		for(int i = 0; i < oStageEpisodeInfos.Count; ++i) {
			var stStageEpisodeInfo = new STEpisodeInfo(oStageEpisodeInfos[i]);
			long nUniqueStageID = CFactory.MakeUniqueStageID(stStageEpisodeInfo.m_stIDInfo.m_nID, stStageEpisodeInfo.m_stIDInfo.m_nChapterID);

			// 스테이지 에피소드 정보가 추가 가능 할 경우
			if(!this.StageEpisodeInfoDict.ContainsKey(nUniqueStageID) || oStageEpisodeInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT) {
				this.StageEpisodeInfoDict.ExReplaceVal(nUniqueStageID, stStageEpisodeInfo);
			}
		}

		for(int i = 0; i < oChapterEpisodeInfos.Count; ++i) {
			var stChapterEpisodeInfo = new STEpisodeInfo(oChapterEpisodeInfos[i]);
			long nUniqueChapterID = CFactory.MakeUniqueChapterID(stChapterEpisodeInfo.m_stIDInfo.m_nID);

			// 챕터 에피소드 정보가 추가 가능 할 경우
			if(!this.ChapterEpisodeInfoDict.ContainsKey(nUniqueChapterID) || oChapterEpisodeInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT) {
				this.ChapterEpisodeInfoDict.ExReplaceVal(nUniqueChapterID, stChapterEpisodeInfo);
			}
		}

		return new List<object>() {
			this.LevelEpisodeInfoDict, this.StageEpisodeInfoDict, this.ChapterEpisodeInfoDict
		};
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	/** 에피소드 정보를 리셋한다 */
	public void ResetEpisodeInfos(string a_oJSONStr) {
		this.LevelEpisodeInfoDict.Clear();
		this.StageEpisodeInfoDict.Clear();
		this.ChapterEpisodeInfoDict.Clear();

		this.DoLoadEpisodeInfos(a_oJSONStr);
	}

	/** 에피소드 정보를 저장한다 */
	public void SaveEpisodeInfos() {
		var oJSONNode = new SimpleJSON.JSONClass();
		var oLevelEpisodeInfos = new SimpleJSON.JSONArray();
		var oStageEpisodeInfos = new SimpleJSON.JSONArray();
		var oChapterEpisodeInfos = new SimpleJSON.JSONArray();

		foreach(var stKeyVal in this.LevelEpisodeInfoDict) {
			oLevelEpisodeInfos.Add(stKeyVal.Value.MakeLevelEpisodeInfo());
		}

		foreach(var stKeyVal in this.StageEpisodeInfoDict) {
			oStageEpisodeInfos.Add(stKeyVal.Value.MakeStageEpisodeInfo());
		}

		foreach(var stKeyVal in this.ChapterEpisodeInfoDict) {
			oChapterEpisodeInfos.Add(stKeyVal.Value.MakeChapterEpisodeInfo());
		}

		oJSONNode.Add(KCDefine.U_KEY_LEVEL, oLevelEpisodeInfos);
		oJSONNode.Add(KCDefine.U_KEY_STAGE, oStageEpisodeInfos);
		oJSONNode.Add(KCDefine.U_KEY_CHAPTER, oChapterEpisodeInfos);

#if NEWTON_SOFT_JSON_MODULE_ENABLE
		CFunc.WriteStr(this.EpisodeInfoTablePath, JsonConvert.DeserializeObject(oJSONNode.ToString()).ExToJSONStr(false, true));
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
	}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion			// 조건부 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
