using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if RUNTIME_TEMPLATES_MODULE_ENABLE
using Newtonsoft.Json;

/** 레벨 정보 */
[System.Serializable]
public struct STLevelInfo {
	public int m_nID;
	public int m_nStageID;
	public int m_nChapterID;

	public ELevelKinds m_eLevelKinds;
	public STEpisodeInfo m_stEpisodeInfo;

	#region 함수
	/** 생성자 */
	public STLevelInfo(SimpleJSON.JSONNode a_oLevelInfo) {
		m_nID = a_oLevelInfo[KCDefine.U_KEY_ID].AsInt;
		m_nStageID = a_oLevelInfo[KCDefine.U_KEY_STAGE_ID].AsInt;
		m_nChapterID = a_oLevelInfo[KCDefine.U_KEY_CHAPTER_ID].AsInt;
		m_eLevelKinds = (ELevelKinds)a_oLevelInfo[KCDefine.U_KEY_LEVEL_KINDS].AsInt;
		m_stEpisodeInfo = new STEpisodeInfo(a_oLevelInfo);
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR || UNITY_STANDALONE
	/** 레벨 정보를 생성한다 */
	public SimpleJSON.JSONClass MakeLevelInfo() {
		var oLevelInfo = new SimpleJSON.JSONClass();
		oLevelInfo.Add(KCDefine.U_KEY_ID, $"{m_nID}");
		oLevelInfo.Add(KCDefine.U_KEY_STAGE_ID, $"{m_nStageID}");
		oLevelInfo.Add(KCDefine.U_KEY_CHAPTER_ID, $"{m_nChapterID}");
		oLevelInfo.Add(KCDefine.U_KEY_LEVEL_KINDS, $"{(int)m_eLevelKinds}");

		m_stEpisodeInfo.MakeCommonEpisodeInfo(oLevelInfo);
		return oLevelInfo;
	}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	#endregion			// 조건부 함수
}

/** 스테이지 정보 */
[System.Serializable]
public struct STStageInfo {
	public int m_nID;
	public int m_nChapterID;

	public EStageKinds m_eStageKinds;
	public STEpisodeInfo m_stEpisodeInfo;

	#region 함수
	/** 생성자 */
	public STStageInfo(SimpleJSON.JSONNode a_oStageInfo) {
		m_nID = a_oStageInfo[KCDefine.U_KEY_ID].AsInt;
		m_nChapterID = a_oStageInfo[KCDefine.U_KEY_CHAPTER_ID].AsInt;
		m_eStageKinds = (EStageKinds)a_oStageInfo[KCDefine.U_KEY_STAGE_KINDS].AsInt;
		m_stEpisodeInfo = new STEpisodeInfo(a_oStageInfo);
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR || UNITY_STANDALONE
	/** 스테이지 정보를 생성한다 */
	public SimpleJSON.JSONClass MakeStageInfo() {
		var oStageInfo = new SimpleJSON.JSONClass();
		oStageInfo.Add(KCDefine.U_KEY_ID, $"{m_nID}");
		oStageInfo.Add(KCDefine.U_KEY_CHAPTER_ID, $"{m_nChapterID}");
		oStageInfo.Add(KCDefine.U_KEY_STAGE_KINDS, $"{(int)m_eStageKinds}");

		m_stEpisodeInfo.MakeCommonEpisodeInfo(oStageInfo);
		return oStageInfo;
	}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	#endregion			// 조건부 함수
}

/** 챕터 정보 */
[System.Serializable]
public struct STChapterInfo {
	public int m_nID;
	public EChapterKinds m_eChapterKinds;
	public STEpisodeInfo m_stEpisodeInfo;

	#region 함수
	/** 생성자 */
	public STChapterInfo(SimpleJSON.JSONNode a_oChapterInfo) {
		m_nID = a_oChapterInfo[KCDefine.U_KEY_ID].AsInt;
		m_eChapterKinds = (EChapterKinds)a_oChapterInfo[KCDefine.U_KEY_CHAPTER_KINDS].AsInt;
		m_stEpisodeInfo = new STEpisodeInfo(a_oChapterInfo);
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR || UNITY_STANDALONE
	/** 챕터 정보를 생성한다 */
	public SimpleJSON.JSONClass MakeChapterInfo() {
		var oChapterInfo = new SimpleJSON.JSONClass();
		oChapterInfo.Add(KCDefine.U_KEY_ID, $"{m_nID}");
		oChapterInfo.Add(KCDefine.U_KEY_CHAPTER_KINDS, $"{(int)m_eChapterKinds}");

		m_stEpisodeInfo.MakeCommonEpisodeInfo(oChapterInfo);
		return oChapterInfo;
	}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	#endregion			// 조건부 함수
}

/** 에피소드 정보 */
[System.Serializable]
public struct STEpisodeInfo {
	public string m_oName;
	public string m_oDesc;

	public EDifficulty m_eDifficulty;
	public ERewardKinds m_eRewardKinds;
	public ETutorialKinds m_eTutorialKinds;

	public List<int> m_oRecordList;
	public Dictionary<ETargetKinds, int> m_oNumTargetsDict;
	public Dictionary<ETargetKinds, int> m_oNumUnlockTargetsDict;

	#region 함수
	/** 생성자 */
	public STEpisodeInfo(SimpleJSON.JSONNode a_oCommonEpisodeInfo) {
		m_oName = a_oCommonEpisodeInfo[KCDefine.U_KEY_NAME];
		m_oDesc = a_oCommonEpisodeInfo[KCDefine.U_KEY_DESC];

		m_eDifficulty = (EDifficulty)a_oCommonEpisodeInfo[KCDefine.U_KEY_DIFFICULTY].AsInt;
		m_eRewardKinds = (ERewardKinds)a_oCommonEpisodeInfo[KCDefine.U_KEY_REWARD_KINDS].AsInt;
		m_eTutorialKinds = (ETutorialKinds)a_oCommonEpisodeInfo[KCDefine.U_KEY_TUTORIAL_KINDS].AsInt;

		m_oRecordList = new List<int>();
		m_oNumTargetsDict = new Dictionary<ETargetKinds, int>();
		m_oNumUnlockTargetsDict = new Dictionary<ETargetKinds, int>();

		for(int i = 0; i < KDefine.G_MAX_NUM_LEVEL_CLEAR_MARKS; ++i) {
			string oRecordKey = string.Format(KCDefine.U_KEY_FMT_RECORD, i + KCDefine.B_VAL_1_INT);
			m_oRecordList.ExAddVal(a_oCommonEpisodeInfo[oRecordKey].AsInt);	
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_LEVEL_TARGET_KINDS; ++i) {
			string oNumTargetsKey = string.Format(KCDefine.U_KEY_FMT_NUM_TARGETS, i + KCDefine.B_VAL_1_INT);
			string oTargetKindsKey = string.Format(KCDefine.U_KEY_FMT_TARGET_KINDS, i + KCDefine.B_VAL_1_INT);

			// 타겟 개수 정보가 존재 할 경우
			if(a_oCommonEpisodeInfo[oTargetKindsKey] != null && a_oCommonEpisodeInfo[oNumTargetsKey] != null) {
				m_oNumTargetsDict.TryAdd((ETargetKinds)a_oCommonEpisodeInfo[oTargetKindsKey].AsInt, a_oCommonEpisodeInfo[oNumTargetsKey].AsInt);
			}
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_LEVEL_UNLOCK_TARGET_KINDS; ++i) {
			string oNumUnlockTargetsKey = string.Format(KCDefine.U_KEY_FMT_NUM_UNLOCK_TARGETS, i + KCDefine.B_VAL_1_INT);
			string oUnlockTargetKindsKey = string.Format(KCDefine.U_KEY_FMT_UNLOCK_TARGET_KINDS, i + KCDefine.B_VAL_1_INT);

			// 잠금 해제 타겟 개수 정보가 존재 할 경우
			if(a_oCommonEpisodeInfo[oUnlockTargetKindsKey] != null && a_oCommonEpisodeInfo[oNumUnlockTargetsKey] != null) {
				m_oNumUnlockTargetsDict.TryAdd((ETargetKinds)a_oCommonEpisodeInfo[oUnlockTargetKindsKey].AsInt, a_oCommonEpisodeInfo[oNumUnlockTargetsKey].AsInt);
			}
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
	/** 에피소드 정보를 생성한다 */
	public void MakeCommonEpisodeInfo(SimpleJSON.JSONClass a_oOutCommonEpisodeInfo) {
		a_oOutCommonEpisodeInfo.Add(KCDefine.U_KEY_NAME, m_oName ?? string.Empty);
		a_oOutCommonEpisodeInfo.Add(KCDefine.U_KEY_DESC, m_oDesc ?? string.Empty);

		a_oOutCommonEpisodeInfo.Add(KCDefine.U_KEY_DIFFICULTY, $"{(int)m_eDifficulty}");
		a_oOutCommonEpisodeInfo.Add(KCDefine.U_KEY_REWARD_KINDS, $"{(int)m_eRewardKinds}");
		a_oOutCommonEpisodeInfo.Add(KCDefine.U_KEY_TUTORIAL_KINDS, $"{(int)m_eTutorialKinds}");

		var oNumTargetsKeyList = m_oNumTargetsDict.Keys.ToList();
		var oNumUnlockTargetsKeyList = m_oNumUnlockTargetsDict.Keys.ToList();

		for(int i = 0; i < KDefine.G_MAX_NUM_LEVEL_CLEAR_MARKS; ++i) {
			a_oOutCommonEpisodeInfo.Add(string.Format(KCDefine.U_KEY_FMT_RECORD, i + KCDefine.B_VAL_1_INT), $"{m_oRecordList.ExGetVal(i, KCDefine.B_VAL_0_INT)}");
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_LEVEL_TARGET_KINDS; ++i) {
			var eKey = oNumTargetsKeyList.ExGetVal(i, ETargetKinds.NONE);
			int nVal = m_oNumTargetsDict.GetValueOrDefault(eKey, KCDefine.B_VAL_0_INT);

			a_oOutCommonEpisodeInfo.Add(string.Format(KCDefine.U_KEY_FMT_NUM_TARGETS, i + KCDefine.B_VAL_1_INT), $"{nVal}");
			a_oOutCommonEpisodeInfo.Add(string.Format(KCDefine.U_KEY_FMT_TARGET_KINDS, i + KCDefine.B_VAL_1_INT), $"{(int)eKey}");
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_LEVEL_UNLOCK_TARGET_KINDS; ++i) {
			var eKey = oNumUnlockTargetsKeyList.ExGetVal(i, ETargetKinds.NONE);
			int nVal = m_oNumUnlockTargetsDict.GetValueOrDefault(eKey, KCDefine.B_VAL_0_INT);

			a_oOutCommonEpisodeInfo.Add(string.Format(KCDefine.U_KEY_FMT_NUM_UNLOCK_TARGETS, i + KCDefine.B_VAL_1_INT), $"{nVal}");
			a_oOutCommonEpisodeInfo.Add(string.Format(KCDefine.U_KEY_FMT_UNLOCK_TARGET_KINDS, i + KCDefine.B_VAL_1_INT), $"{(int)eKey}");
		}
	}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	#endregion			// 조건부 함수
}

/** 에피소드 정보 테이블 */
public class CEpisodeInfoTable : CScriptableObj<CEpisodeInfoTable> {
	#region 변수
	[Header("=====> Level Info <=====")]
	[SerializeField] private List<STLevelInfo> m_oLevelInfoList = new List<STLevelInfo>();

	[Header("=====> Stage Info <=====")]
	[SerializeField] private List<STStageInfo> m_oStageInfoList = new List<STStageInfo>();

	[Header("=====> Chapter Info <=====")]
	[SerializeField] private List<STChapterInfo> m_oChapterInfoList = new List<STChapterInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<long, STLevelInfo> LevelInfoDict { get; private set; } = new Dictionary<long, STLevelInfo>();
	public Dictionary<long, STStageInfo> StageInfoDict { get; private set; } = new Dictionary<long, STStageInfo>();
	public Dictionary<long, STChapterInfo> ChapterInfoDict { get; private set; } = new Dictionary<long, STChapterInfo>();

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

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		for(int i = 0; i < m_oLevelInfoList.Count; ++i) {
			this.LevelInfoDict.TryAdd(CFactory.MakeUniqueLevelID(i, m_oLevelInfoList[i].m_nStageID, m_oLevelInfoList[i].m_nChapterID), m_oLevelInfoList[i]);
		}

		for(int i = 0; i < m_oStageInfoList.Count; ++i) {
			this.StageInfoDict.TryAdd(CFactory.MakeUniqueStageID(i, m_oStageInfoList[i].m_nChapterID), m_oStageInfoList[i]);
		}

		for(int i = 0; i < m_oChapterInfoList.Count; ++i) {
			this.ChapterInfoDict.TryAdd(CFactory.MakeUniqueChapterID(i), m_oChapterInfoList[i]);
		}
	}

	/** 레벨 기록을 반환한다 */
	public int GetLevelRecord(int a_nID, int a_nIdx, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetLevelInfo(a_nID, out STLevelInfo stLevelInfo, a_nStageID, a_nChapterID);
		return stLevelInfo.m_stEpisodeInfo.GetRecord(a_nIdx);
	}

	/** 레벨 타겟 개수를 반환한다 */
	public int GetLevelNumTargets(int a_nID, ETargetKinds a_eTargetKinds, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetLevelInfo(a_nID, out STLevelInfo stLevelInfo, a_nStageID, a_nChapterID);
		return stLevelInfo.m_stEpisodeInfo.GetNumTargets(a_eTargetKinds);
	}

	/** 레벨 잠금 해제 타겟 개수를 반환한다 */
	public int GetLevelNumUnlockTargets(int a_nID, ETargetKinds a_eTargetKinds, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetLevelInfo(a_nID, out STLevelInfo stLevelInfo, a_nStageID, a_nChapterID);
		return stLevelInfo.m_stEpisodeInfo.GetNumUnlockTargets(a_eTargetKinds);
	}

	/** 스테이지 기록을 반환한다 */
	public int GetStageRecord(int a_nID, int a_nIdx, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetStageInfo(a_nID, out STStageInfo stStageInfo, a_nChapterID);
		return stStageInfo.m_stEpisodeInfo.GetRecord(a_nIdx);
	}

	/** 스테이지 타겟 개수를 반환한다 */
	public int GetStageNumTargets(int a_nID, ETargetKinds a_eTargetKinds, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetStageInfo(a_nID, out STStageInfo stStageInfo, a_nChapterID);
		return stStageInfo.m_stEpisodeInfo.GetNumTargets(a_eTargetKinds);
	}

	/** 스테이지 잠금 해제 타겟 개수를 반환한다 */
	public int GetStageNumUnlockTargets(int a_nID, ETargetKinds a_eTargetKinds, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetStageInfo(a_nID, out STStageInfo stStageInfo, a_nChapterID);
		return stStageInfo.m_stEpisodeInfo.GetNumUnlockTargets(a_eTargetKinds);
	}

	/** 챕터 기록을 반환한다 */
	public int GetChapterRecord(int a_nID, int a_nIdx) {
		this.TryGetChapterInfo(a_nID, out STChapterInfo stChapterInfo);
		return stChapterInfo.m_stEpisodeInfo.GetRecord(a_nIdx);
	}

	/** 챕터 타겟 개수를 반환한다 */
	public int GetChapterNumTargets(int a_nID, ETargetKinds a_eTargetKinds) {
		this.TryGetChapterInfo(a_nID, out STChapterInfo stChapterInfo);
		return stChapterInfo.m_stEpisodeInfo.GetNumTargets(a_eTargetKinds);
	}

	/** 챕터 잠금 해제 타겟 개수를 반환한다 */
	public int GetChapterumUnlockTargets(int a_nID, ETargetKinds a_eTargetKinds) {
		this.TryGetChapterInfo(a_nID, out STChapterInfo stChapterInfo);
		return stChapterInfo.m_stEpisodeInfo.GetNumUnlockTargets(a_eTargetKinds);
	}

	/** 레벨 정보를 반환한다 */
	public STLevelInfo GetLevelInfo(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetLevelInfo(a_nID, out STLevelInfo stLevelInfo, a_nStageID, a_nChapterID);
		CAccess.Assert(bIsValid);

		return stLevelInfo;
	}

	/** 스테이지 정보를 반환한다 */
	public STStageInfo GetStageInfo(int a_nID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetStageInfo(a_nID, out STStageInfo stStageInfo, a_nChapterID);
		CAccess.Assert(bIsValid);

		return stStageInfo;
	}

	/** 챕터 정보를 반환한다 */
	public STChapterInfo GetChapterInfo(int a_nID) {
		bool bIsValid = this.TryGetChapterInfo(a_nID, out STChapterInfo stChapterInfo);
		CAccess.Assert(bIsValid);

		return stChapterInfo;
	}

	/** 레벨 정보를 반환한다 */
	public bool TryGetLevelInfo(int a_nID, out STLevelInfo a_stOutLevelInfo, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		long nUniqueLevelID = CFactory.MakeUniqueLevelID(a_nID, a_nStageID, a_nChapterID);
		a_stOutLevelInfo = this.LevelInfoDict.GetValueOrDefault(nUniqueLevelID, KDefine.G_INVALID_LEVEL_INFO);

		return this.LevelInfoDict.ContainsKey(nUniqueLevelID);
	}

	/** 스테이지 정보를 반환한다 */
	public bool TryGetStageInfo(int a_nID, out STStageInfo a_stOutStageInfo, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		long nUniqueStageID = CFactory.MakeUniqueStageID(a_nID, a_nChapterID);
		a_stOutStageInfo = this.StageInfoDict.GetValueOrDefault(nUniqueStageID, KDefine.G_INVALID_STAGE_INFO);

		return this.StageInfoDict.ContainsKey(nUniqueStageID);
	}

	/** 챕터 정보를 반환한다 */
	public bool TryGetChapterInfo(int a_nID, out STChapterInfo a_stOutChapterInfo) {
		long nUniqueChapterID = CFactory.MakeUniqueChapterID(a_nID);
		a_stOutChapterInfo = this.ChapterInfoDict.GetValueOrDefault(nUniqueChapterID, KDefine.G_INVALID_CHAPTER_INFO);

		return this.ChapterInfoDict.ContainsKey(nUniqueChapterID);
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
			var oTextAsset = CResManager.Inst.GetRes<TextAsset>(a_oFilePath);
			return this.DoLoadEpisodeInfos(oTextAsset.text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 에피소드 정보를 로드한다 */
	private List<object> DoLoadEpisodeInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
				
		var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr) as SimpleJSON.JSONClass;
		var oLevelInfos = oJSONNode[KCDefine.U_KEY_LEVEL];
		var oStageInfos = oJSONNode[KCDefine.U_KEY_STAGE];
		var oChapterInfos = oJSONNode[KCDefine.U_KEY_CHAPTER];

		for(int i = 0; i < oLevelInfos.Count; ++i) {
			var stLevelInfo = new STLevelInfo(oLevelInfos[i]);
			long nUniqueLevelID = CFactory.MakeUniqueLevelID(stLevelInfo.m_nID, stLevelInfo.m_nStageID, stLevelInfo.m_nChapterID);

			// 레벨 정보가 추가 가능 할 경우
			if(!this.LevelInfoDict.ContainsKey(nUniqueLevelID) || oLevelInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT) {
				this.LevelInfoDict.ExReplaceVal(nUniqueLevelID, stLevelInfo);
			}
		}

		for(int i = 0; i < oStageInfos.Count; ++i) {
			var stStageInfo = new STStageInfo(oStageInfos[i]);
			long nUniqueStageID = CFactory.MakeUniqueStageID(stStageInfo.m_nID, stStageInfo.m_nChapterID);

			// 스테이지 정보가 추가 가능 할 경우
			if(!this.StageInfoDict.ContainsKey(nUniqueStageID) || oStageInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT) {
				this.StageInfoDict.ExReplaceVal(nUniqueStageID, stStageInfo);
			}
		}

		for(int i = 0; i < oChapterInfos.Count; ++i) {
			var stChapterInfo = new STChapterInfo(oChapterInfos[i]);
			long nUniqueChapterID = CFactory.MakeUniqueChapterID(stChapterInfo.m_nID);

			// 챕터 정보가 추가 가능 할 경우
			if(!this.ChapterInfoDict.ContainsKey(nUniqueChapterID) || oChapterInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT) {
				this.ChapterInfoDict.ExReplaceVal(nUniqueChapterID, stChapterInfo);
			}
		}

		return new List<object>() {
			this.LevelInfoDict, this.StageInfoDict, this.ChapterInfoDict
		};
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	/** 에피소드 정보를 저장한다 */
	public void SaveEpisodeInfos() {
		var oJSONNode = new SimpleJSON.JSONClass();
		var oLevelInfos = new SimpleJSON.JSONArray();
		var oStageInfos = new SimpleJSON.JSONArray();
		var oChapterInfos = new SimpleJSON.JSONArray();

		foreach(var stKeyVal in this.LevelInfoDict) {
			oLevelInfos.Add(stKeyVal.Value.MakeLevelInfo());
		}

		foreach(var stKeyVal in this.StageInfoDict) {
			oStageInfos.Add(stKeyVal.Value.MakeStageInfo());
		}

		foreach(var stKeyVal in this.ChapterInfoDict) {
			oChapterInfos.Add(stKeyVal.Value.MakeChapterInfo());
		}

		oJSONNode.Add(KCDefine.U_KEY_LEVEL, oLevelInfos);
		oJSONNode.Add(KCDefine.U_KEY_STAGE, oStageInfos);
		oJSONNode.Add(KCDefine.U_KEY_CHAPTER, oChapterInfos);

#if NEWTON_SOFT_JSON_MODULE_ENABLE
		var oEpisodeInfos = JsonConvert.DeserializeObject(oJSONNode.ToString());
		CFunc.WriteStr(this.EpisodeInfoTablePath, oEpisodeInfos.ExToJSONStr(false, true));
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
	}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion			// 조건부 함수

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
