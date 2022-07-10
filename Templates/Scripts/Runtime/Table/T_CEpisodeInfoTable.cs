using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
using Newtonsoft.Json;

/** 에피소드 정보 */
[System.Serializable]
public partial struct STEpisodeInfo {
	public STCommonInfo m_stCommonInfo;

	public STIDInfo m_stIDInfo;
	public STIDInfo m_stPrevIDInfo;
	public STIDInfo m_stNextIDInfo;

	public Vector3 m_stSize;
	public EDifficulty m_eDifficulty;
	public EEpisodeKinds m_eEpisodeKinds;
	public ETutorialKinds m_eTutorialKinds;

	public List<string> m_oRecordList;
	public List<ERewardKinds> m_oRewardKindsList;
	public List<STTargetInfo> m_oClearTargetInfoList;
	public List<STTargetInfo> m_oUnlockTargetInfoList;

	#region 상수
	public static STEpisodeInfo INVALID = new STEpisodeInfo() {
		m_stIDInfo = STIDInfo.INVALID, m_stPrevIDInfo = STIDInfo.INVALID, m_stNextIDInfo = STIDInfo.INVALID
	};
	#endregion			// 상수

	#region 함수
	/** 생성자 */
	public STEpisodeInfo(SimpleJSON.JSONNode a_oEpisodeInfo) {
		m_stCommonInfo = new STCommonInfo(a_oEpisodeInfo);

		m_stSize = new Vector3(a_oEpisodeInfo[KCDefine.U_KEY_SIZE_X].AsFloat, a_oEpisodeInfo[KCDefine.U_KEY_SIZE_Y].AsFloat, KCDefine.B_VAL_0_REAL);
		m_eDifficulty = a_oEpisodeInfo[KCDefine.U_KEY_DIFFICULTY].ExIsValid() ? (EDifficulty)a_oEpisodeInfo[KCDefine.U_KEY_DIFFICULTY].AsInt : EDifficulty.NONE;
		m_eEpisodeKinds = a_oEpisodeInfo[KCDefine.U_KEY_EPISODE_KINDS].ExIsValid() ? (EEpisodeKinds)a_oEpisodeInfo[KCDefine.U_KEY_EPISODE_KINDS].AsInt : EEpisodeKinds.NONE;
		m_eTutorialKinds = a_oEpisodeInfo[KCDefine.U_KEY_TUTORIAL_KINDS].ExIsValid() ? (ETutorialKinds)a_oEpisodeInfo[KCDefine.U_KEY_TUTORIAL_KINDS].AsInt : ETutorialKinds.NONE;

		m_oRecordList = new List<string>();
		m_oRewardKindsList = new List<ERewardKinds>();
		m_oClearTargetInfoList = new List<STTargetInfo>();
		m_oUnlockTargetInfoList = new List<STTargetInfo>();

		STEpisodeInfo.SetupIDInfo(KCDefine.U_KEY_FMT_ID, KCDefine.B_VAL_0_INT, a_oEpisodeInfo, out m_stIDInfo);
		STEpisodeInfo.SetupIDInfo(KCDefine.U_KEY_FMT_PREV_ID, KCDefine.B_VAL_0_INT, a_oEpisodeInfo, out m_stPrevIDInfo);
		STEpisodeInfo.SetupIDInfo(KCDefine.U_KEY_FMT_NEXT_ID, KCDefine.B_VAL_0_INT, a_oEpisodeInfo, out m_stNextIDInfo);

		for(int i = 0; i < KDefine.G_MAX_NUM_RECORDS; ++i) {
			string oRecordKey = string.Format(KCDefine.U_KEY_FMT_RECORD, i + KCDefine.B_VAL_1_INT);
			m_oRecordList.ExAddVal(a_oEpisodeInfo[oRecordKey].ExIsValid() ? a_oEpisodeInfo[oRecordKey] : KCDefine.B_STR_0_INT);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_REWARD_KINDS; ++i) {
			string oRewardKindsKey = string.Format(KCDefine.U_KEY_FMT_REWARD_KINDS, i + KCDefine.B_VAL_1_INT);
			m_oRewardKindsList.Add(a_oEpisodeInfo[oRewardKindsKey].ExIsValid() ? (ERewardKinds)a_oEpisodeInfo[oRewardKindsKey].AsInt : ERewardKinds.NONE);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			m_oClearTargetInfoList.Add(new STTargetInfo(a_oEpisodeInfo, KCDefine.U_PREFIX_CLEAR, i));
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			m_oUnlockTargetInfoList.Add(new STTargetInfo(a_oEpisodeInfo, KCDefine.U_PREFIX_UNLOCK, i));
		}
	}

	/** 기록을 반환한다 */
	public long GetIntRecord(int a_nIdx) {
		return long.TryParse(m_oRecordList.ExGetVal(a_nIdx, KCDefine.B_STR_0_INT), out long nRecord) ? nRecord : KCDefine.B_VAL_0_INT;
	}

	/** 기록을 반환한다 */
	public double GetRealRecord(int a_nIdx) {
		return double.TryParse(m_oRecordList.ExGetVal(a_nIdx, KCDefine.B_STR_0_INT), out double dblRecord) ? dblRecord : KCDefine.B_VAL_0_REAL;
	}

	/** 클리어 타겟 정보를 반환한다 */
	public STTargetInfo GetClearTargetInfo(ETargetKinds a_eTargetKinds, int a_nKinds) {
		return m_oClearTargetInfoList.ExGetVal((a_stTargetInfo) => a_stTargetInfo.m_eTargetKinds == a_eTargetKinds && a_stTargetInfo.m_nKinds == a_nKinds, default(STTargetInfo));
	}

	/** 잠금 해제 타겟 정보를 반환한다 */
	public STTargetInfo GetUnlockTargetInfo(ETargetKinds a_eTargetKinds, int a_nKinds) {
		return m_oUnlockTargetInfoList.ExGetVal((a_stUnlockTargetInfo) => a_stUnlockTargetInfo.m_eTargetKinds == a_eTargetKinds && a_stUnlockTargetInfo.m_nKinds == a_nKinds, default(STTargetInfo));
	}

	/** 식별자 정보를 설정한다 */
	private static void SetupIDInfo(string a_oIDFmt, int a_nDefID, SimpleJSON.JSONNode a_oEpisodeInfo, out STIDInfo a_stOutIDInfo) {
		string oID01Key = string.Format(a_oIDFmt, KCDefine.B_VAL_1_INT);
		string oID02Key = string.Format(a_oIDFmt, KCDefine.B_VAL_2_INT);
		string oID03Key = string.Format(a_oIDFmt, KCDefine.B_VAL_3_INT);
		
		a_stOutIDInfo = new STIDInfo() {
			m_nID01 = a_oEpisodeInfo[oID01Key].ExIsValid() ? a_oEpisodeInfo[oID01Key].AsInt : a_nDefID,
			m_nID02 = a_oEpisodeInfo[oID02Key].ExIsValid() ? a_oEpisodeInfo[oID02Key].AsInt : a_nDefID,
			m_nID03 = a_oEpisodeInfo[oID03Key].ExIsValid() ? a_oEpisodeInfo[oID03Key].AsInt : a_nDefID
		};
	}

	/** 식별자 정보를 생성한다 */
	private static void MakeIDInfo(string a_oIDFmt, STIDInfo a_stIDInfo, SimpleJSON.JSONNode a_oOutEpisodeInfo) {
		string oID01Key = string.Format(a_oIDFmt, KCDefine.B_VAL_1_INT);
		string oID02Key = string.Format(a_oIDFmt, KCDefine.B_VAL_2_INT);
		string oID03Key = string.Format(a_oIDFmt, KCDefine.B_VAL_3_INT);

		a_oOutEpisodeInfo.Add(oID01Key, $"{a_stIDInfo.m_nID01}");
		a_oOutEpisodeInfo.Add(oID02Key, $"{a_stIDInfo.m_nID02}");
		a_oOutEpisodeInfo.Add(oID03Key, $"{a_stIDInfo.m_nID03}");
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR || UNITY_STANDALONE
	/** 에피소드 정보를 생성한다 */
	public SimpleJSON.JSONClass MakeEpisodeInfo() {
		var oEpisodeInfo = new SimpleJSON.JSONClass();
		m_stCommonInfo.MakeCommonInfo(oEpisodeInfo);

		oEpisodeInfo.Add(KCDefine.U_KEY_SIZE_X, $"{m_stSize.x}");
		oEpisodeInfo.Add(KCDefine.U_KEY_SIZE_Y, $"{m_stSize.y}");

		oEpisodeInfo.Add(KCDefine.U_KEY_DIFFICULTY, $"{(int)m_eDifficulty}");
		oEpisodeInfo.Add(KCDefine.U_KEY_EPISODE_KINDS, $"{(int)m_eEpisodeKinds}");
		oEpisodeInfo.Add(KCDefine.U_KEY_TUTORIAL_KINDS, $"{(int)m_eTutorialKinds}");

		STEpisodeInfo.MakeIDInfo(KCDefine.U_KEY_FMT_ID, m_stIDInfo, oEpisodeInfo);
		STEpisodeInfo.MakeIDInfo(KCDefine.U_KEY_FMT_PREV_ID, m_stPrevIDInfo, oEpisodeInfo);
		STEpisodeInfo.MakeIDInfo(KCDefine.U_KEY_FMT_NEXT_ID, m_stNextIDInfo, oEpisodeInfo);

		for(int i = 0; i < m_oRecordList.Count; ++i) {
			oEpisodeInfo.Add(string.Format(KCDefine.U_KEY_FMT_RECORD, i + KCDefine.B_VAL_1_INT), m_oRecordList.ExGetVal(i, KCDefine.B_STR_0_INT));
		}

		for(int i = 0; i < m_oRewardKindsList.Count; ++i) {
			oEpisodeInfo.Add(string.Format(KCDefine.U_KEY_FMT_REWARD_KINDS, i + KCDefine.B_VAL_1_INT), $"{(int)m_oRewardKindsList.ExGetVal(i, ERewardKinds.NONE)}");
		}

		for(int i = 0; i < m_oClearTargetInfoList.Count; ++i) {
			m_oClearTargetInfoList[i].MakeTargetInfo(oEpisodeInfo, KCDefine.U_PREFIX_CLEAR, i);
		}

		for(int i = 0; i < m_oUnlockTargetInfoList.Count; ++i) {
			m_oUnlockTargetInfoList[i].MakeTargetInfo(oEpisodeInfo, KCDefine.U_PREFIX_UNLOCK, i);
		}

		return oEpisodeInfo;
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
		this.ResetEpisodeInfos();
	}

	/** 에피소드 정보를 리셋한다 */
	public void ResetEpisodeInfos() {
		this.LevelEpisodeInfoDict.Clear();
		this.StageEpisodeInfoDict.Clear();
		this.ChapterEpisodeInfoDict.Clear();

		for(int i = 0; i < m_oLevelEpisodeInfoList.Count; ++i) {
			this.LevelEpisodeInfoDict.TryAdd(CFactory.MakeUniqueLevelID(i, m_oLevelEpisodeInfoList[i].m_stIDInfo.m_nID02, m_oLevelEpisodeInfoList[i].m_stIDInfo.m_nID03), m_oLevelEpisodeInfoList[i]);
		}

		for(int i = 0; i < m_oStageEpisodeInfoList.Count; ++i) {
			this.StageEpisodeInfoDict.TryAdd(CFactory.MakeUniqueStageID(i, m_oStageEpisodeInfoList[i].m_stIDInfo.m_nID03), m_oStageEpisodeInfoList[i]);
		}

		for(int i = 0; i < m_oChapterEpisodeInfoList.Count; ++i) {
			this.ChapterEpisodeInfoDict.TryAdd(CFactory.MakeUniqueChapterID(i), m_oChapterEpisodeInfoList[i]);
		}
	}

	/** 에피소드 정보를 리셋한다 */
	public void ResetEpisodeInfos(string a_oJSONStr) {
		this.ResetEpisodeInfos();
		this.DoLoadEpisodeInfos(a_oJSONStr);
	}

	/** 레벨 에피소드 기록을 반환한다 */
	public long GetLevelEpisodeIntRecord(int a_nLevelID, int a_nIdx, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetLevelEpisodeInfo(a_nLevelID, out STEpisodeInfo stLevelEpisodeInfo, a_nStageID, a_nChapterID);
		return stLevelEpisodeInfo.GetIntRecord(a_nIdx);
	}

	/** 레벨 에피소드 기록을 반환한다 */
	public double GetLevelEpisodeRealRecord(int a_nLevelID, int a_nIdx, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetLevelEpisodeInfo(a_nLevelID, out STEpisodeInfo stLevelEpisodeInfo, a_nStageID, a_nChapterID);
		return stLevelEpisodeInfo.GetRealRecord(a_nIdx);
	}

	/** 레벨 에피소드 클리어 타겟 정보를 반환한다 */
	public STTargetInfo GetLevelEpisodeClearTargetInfo(int a_nLevelID, ETargetKinds a_eTargetKinds, int a_nKinds, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetLevelEpisodeInfo(a_nLevelID, out STEpisodeInfo stLevelEpisodeInfo, a_nStageID, a_nChapterID);
		return stLevelEpisodeInfo.GetClearTargetInfo(a_eTargetKinds, a_nKinds);
	}

	/** 레벨 에피소드 잠금 해제 타겟 정보를 반환한다 */
	public STTargetInfo GetLevelEpisodeUnlockTargetInfo(int a_nLevelID, ETargetKinds a_eTargetKinds, int a_nKinds, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetLevelEpisodeInfo(a_nLevelID, out STEpisodeInfo stLevelEpisodeInfo, a_nStageID, a_nChapterID);
		return stLevelEpisodeInfo.GetUnlockTargetInfo(a_eTargetKinds, a_nKinds);
	}

	/** 스테이지 에피소드 기록을 반환한다 */
	public long GetStageEpisodeIntRecord(int a_nStageID, int a_nIdx, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetStageEpisodeInfo(a_nStageID, out STEpisodeInfo stStageEpisodeInfo, a_nChapterID);
		return stStageEpisodeInfo.GetIntRecord(a_nIdx);
	}

	/** 스테이지 에피소드 기록을 반환한다 */
	public double GetStageEpisodeRealRecord(int a_nStageID, int a_nIdx, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetStageEpisodeInfo(a_nStageID, out STEpisodeInfo stStageEpisodeInfo, a_nChapterID);
		return stStageEpisodeInfo.GetRealRecord(a_nIdx);
	}

	/** 스테이지 에피소드 클리어 타겟 정보를 반환한다 */
	public STTargetInfo GetStageEpisodeClearTargetInfo(int a_nStageID, ETargetKinds a_eTargetKinds, int a_nKinds, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetStageEpisodeInfo(a_nStageID, out STEpisodeInfo stStageEpisodeInfo, a_nChapterID);
		return stStageEpisodeInfo.GetClearTargetInfo(a_eTargetKinds, a_nKinds);
	}

	/** 스테이지 에피소드 잠금 해제 타겟 정보를 반환한다 */
	public STTargetInfo GetStageEpisodeUnlockTargetInfo(int a_nStageID, ETargetKinds a_eTargetKinds, int a_nKinds, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetStageEpisodeInfo(a_nStageID, out STEpisodeInfo stStageEpisodeInfo, a_nChapterID);
		return stStageEpisodeInfo.GetUnlockTargetInfo(a_eTargetKinds, a_nKinds);
	}

	/** 챕터 에피소드 기록을 반환한다 */
	public long GetChapterEpisodeIntRecord(int a_nChapterID, int a_nIdx) {
		this.TryGetChapterEpisodeInfo(a_nChapterID, out STEpisodeInfo stChapterEpisodeInfo);
		return stChapterEpisodeInfo.GetIntRecord(a_nIdx);
	}

	/** 챕터 에피소드 기록을 반환한다 */
	public double GetChapterEpisodeRealRecord(int a_nChapterID, int a_nIdx) {
		this.TryGetChapterEpisodeInfo(a_nChapterID, out STEpisodeInfo stChapterEpisodeInfo);
		return stChapterEpisodeInfo.GetRealRecord(a_nIdx);
	}

	/** 챕터 에피소드 클리어 타겟 정보를 반환한다 */
	public STTargetInfo GetChapterEpisodeClearTargetInfo(int a_nChapterID, ETargetKinds a_eTargetKinds, int a_nKinds) {
		this.TryGetChapterEpisodeInfo(a_nChapterID, out STEpisodeInfo stChapterEpisodeInfo);
		return stChapterEpisodeInfo.GetClearTargetInfo(a_eTargetKinds, a_nKinds);
	}

	/** 챕터 에피소드 잠금 해제 타겟 정보를 반환한다 */
	public STTargetInfo GetChapterEpisodeUnlockTargetInfo(int a_nChapterID, ETargetKinds a_eTargetKinds, int a_nKinds) {
		this.TryGetChapterEpisodeInfo(a_nChapterID, out STEpisodeInfo stChapterEpisodeInfo);
		return stChapterEpisodeInfo.GetUnlockTargetInfo(a_eTargetKinds, a_nKinds);
	}

	/** 레벨 에피소드 정보를 반환한다 */
	public STEpisodeInfo GetLevelEpisodeInfo(int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetLevelEpisodeInfo(a_nLevelID, out STEpisodeInfo stLevelEpisodeInfo, a_nStageID, a_nChapterID);
		CAccess.Assert(bIsValid);

		return stLevelEpisodeInfo;
	}

	/** 스테이지 에피소드 정보를 반환한다 */
	public STEpisodeInfo GetStageEpisodeInfo(int a_nStageID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetStageEpisodeInfo(a_nStageID, out STEpisodeInfo stStageEpisodeInfo, a_nChapterID);
		CAccess.Assert(bIsValid);

		return stStageEpisodeInfo;
	}

	/** 챕터 에피소드 정보를 반환한다 */
	public STEpisodeInfo GetChapterEpisodeInfo(int a_nChapterID) {
		bool bIsValid = this.TryGetChapterEpisodeInfo(a_nChapterID, out STEpisodeInfo stChapterEpisodeInfo);
		CAccess.Assert(bIsValid);

		return stChapterEpisodeInfo;
	}

	/** 레벨 에피소드 정보를 반환한다 */
	public bool TryGetLevelEpisodeInfo(int a_nLevelID, out STEpisodeInfo a_stOutLevelEpisodeInfo, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		a_stOutLevelEpisodeInfo = this.LevelEpisodeInfoDict.GetValueOrDefault(CFactory.MakeUniqueLevelID(a_nLevelID, a_nStageID, a_nChapterID), default(STEpisodeInfo));
		return this.LevelEpisodeInfoDict.ContainsKey(CFactory.MakeUniqueLevelID(a_nLevelID, a_nStageID, a_nChapterID));
	}

	/** 스테이지 에피소드 정보를 반환한다 */
	public bool TryGetStageEpisodeInfo(int a_nStageID, out STEpisodeInfo a_stOutStageEpisodeInfo, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		a_stOutStageEpisodeInfo = this.StageEpisodeInfoDict.GetValueOrDefault(CFactory.MakeUniqueStageID(a_nStageID, a_nChapterID), default(STEpisodeInfo));
		return this.StageEpisodeInfoDict.ContainsKey(CFactory.MakeUniqueStageID(a_nStageID, a_nChapterID));
	}

	/** 챕터 에피소드 정보를 반환한다 */
	public bool TryGetChapterEpisodeInfo(int a_nChapterID, out STEpisodeInfo a_stOutChapterEpisodeInfo) {
		a_stOutChapterEpisodeInfo = this.ChapterEpisodeInfoDict.GetValueOrDefault(CFactory.MakeUniqueChapterID(a_nChapterID), default(STEpisodeInfo));
		return this.ChapterEpisodeInfoDict.ContainsKey(CFactory.MakeUniqueChapterID(a_nChapterID));
	}

	/** 에피소드 정보를 로드한다 */
	public List<object> LoadEpisodeInfos() {
		this.ResetEpisodeInfos();
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
		var oLevelEpisodeInfos = oJSONNode[KCDefine.U_KEY_LEVEL];
		var oStageEpisodeInfos = oJSONNode[KCDefine.U_KEY_STAGE];
		var oChapterEpisodeInfos = oJSONNode[KCDefine.U_KEY_CHAPTER];

		for(int i = 0; i < oLevelEpisodeInfos.Count; ++i) {
			var stLevelEpisodeInfo = new STEpisodeInfo(oLevelEpisodeInfos[i]);

			// 레벨 에피소드 정보가 추가 가능 할 경우
			if(stLevelEpisodeInfo.m_stIDInfo.m_nID01.ExIsValidIdx() && (!this.LevelEpisodeInfoDict.ContainsKey(stLevelEpisodeInfo.m_stIDInfo.UniqueID01) || oLevelEpisodeInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
				this.LevelEpisodeInfoDict.ExReplaceVal(stLevelEpisodeInfo.m_stIDInfo.UniqueID01, stLevelEpisodeInfo);
			}
		}

		for(int i = 0; i < oStageEpisodeInfos.Count; ++i) {
			var stStageEpisodeInfo = new STEpisodeInfo(oStageEpisodeInfos[i]);

			// 스테이지 에피소드 정보가 추가 가능 할 경우
			if(stStageEpisodeInfo.m_stIDInfo.m_nID01.ExIsValidIdx() && (!this.StageEpisodeInfoDict.ContainsKey(stStageEpisodeInfo.m_stIDInfo.UniqueID02) || oStageEpisodeInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
				this.StageEpisodeInfoDict.ExReplaceVal(stStageEpisodeInfo.m_stIDInfo.UniqueID02, stStageEpisodeInfo);
			}
		}

		for(int i = 0; i < oChapterEpisodeInfos.Count; ++i) {
			var stChapterEpisodeInfo = new STEpisodeInfo(oChapterEpisodeInfos[i]);

			// 챕터 에피소드 정보가 추가 가능 할 경우
			if(stChapterEpisodeInfo.m_stIDInfo.m_nID01.ExIsValidIdx() && (!this.ChapterEpisodeInfoDict.ContainsKey(stChapterEpisodeInfo.m_stIDInfo.UniqueID03) || oChapterEpisodeInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
				this.ChapterEpisodeInfoDict.ExReplaceVal(stChapterEpisodeInfo.m_stIDInfo.UniqueID03, stChapterEpisodeInfo);
			}
		}

		return new List<object>() {
			this.LevelEpisodeInfoDict, this.StageEpisodeInfoDict, this.ChapterEpisodeInfoDict
		};
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	/** 에피소드 정보를 저장한다 */
	public void SaveEpisodeInfos() {
		var oJSONNode = new SimpleJSON.JSONClass();
		var oLevelEpisodeInfos = new SimpleJSON.JSONArray();
		var oStageEpisodeInfos = new SimpleJSON.JSONArray();
		var oChapterEpisodeInfos = new SimpleJSON.JSONArray();

		foreach(var stKeyVal in this.LevelEpisodeInfoDict) {
			oLevelEpisodeInfos.Add(stKeyVal.Value.MakeEpisodeInfo());
		}

		foreach(var stKeyVal in this.StageEpisodeInfoDict) {
			oStageEpisodeInfos.Add(stKeyVal.Value.MakeEpisodeInfo());
		}

		foreach(var stKeyVal in this.ChapterEpisodeInfoDict) {
			oChapterEpisodeInfos.Add(stKeyVal.Value.MakeEpisodeInfo());
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
