#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using Newtonsoft.Json;

/** 에피소드 정보 */
[System.Serializable]
public partial struct STEpisodeInfo {
	public STCommonInfo m_stCommonInfo;

	public int m_nNumSubEpisodes;
	public int m_nMaxNumEnemyObjs;
	public Vector3 m_stSize;

	public STIDInfo m_stIDInfo;
	public STIDInfo m_stPrevIDInfo;
	public STIDInfo m_stNextIDInfo;

	public EDifficulty m_eDifficulty;
	public EEpisodeKinds m_eEpisodeKinds;
	public ETutorialKinds m_eTutorialKinds;

	public List<ERewardKinds> m_oRewardKindsList;
	public List<STValInfo> m_oRecordValInfoList;

	public Dictionary<ulong, STTargetInfo> m_oClearTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oUnlockTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oDropItemTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oEnemyObjTargetInfoDict;

	#region 상수
	public static STEpisodeInfo INVALID = new STEpisodeInfo() {
		m_stIDInfo = STIDInfo.INVALID, m_stPrevIDInfo = STIDInfo.INVALID, m_stNextIDInfo = STIDInfo.INVALID
	};
	#endregion			// 상수

	#region 프로퍼티
	public ulong ULevelID => CFactory.MakeULevelID(m_stIDInfo.m_nID01, m_stIDInfo.m_nID02, m_stIDInfo.m_nID03);
	public ulong PrevULevelID => CFactory.MakeULevelID(m_stPrevIDInfo.m_nID01, m_stPrevIDInfo.m_nID02, m_stPrevIDInfo.m_nID03);
	public ulong NextULevelID => CFactory.MakeULevelID(m_stNextIDInfo.m_nID01, m_stNextIDInfo.m_nID02, m_stNextIDInfo.m_nID03);

	public ulong UniqueStagelID => CFactory.MakeUniqueStageID(m_stIDInfo.m_nID02, m_stIDInfo.m_nID03);
	public ulong PrevUniqueStageID => CFactory.MakeUniqueStageID(m_stPrevIDInfo.m_nID02, m_stPrevIDInfo.m_nID03);
	public ulong NextUniqueStageID => CFactory.MakeUniqueStageID(m_stNextIDInfo.m_nID02, m_stNextIDInfo.m_nID03);

	public ulong UniqueChapterID => CFactory.MakeUniqueChapterID(m_stIDInfo.m_nID03);
	public ulong PrevUniqueChapterID => CFactory.MakeUniqueChapterID(m_stPrevIDInfo.m_nID03);
	public ulong NextUniqueChapterID => CFactory.MakeUniqueChapterID(m_stNextIDInfo.m_nID03);
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STEpisodeInfo(SimpleJSON.JSONNode a_oEpisodeInfo) {
		m_stCommonInfo = new STCommonInfo(a_oEpisodeInfo);

		m_nNumSubEpisodes = a_oEpisodeInfo[KCDefine.U_KEY_NUM_SUB_EPISODES].ExIsValid() ? a_oEpisodeInfo[KCDefine.U_KEY_NUM_SUB_EPISODES].AsInt : KCDefine.B_VAL_0_INT;
		m_nMaxNumEnemyObjs = a_oEpisodeInfo[KCDefine.U_KEY_MAX_NUM_ENEMY_OBJS].ExIsValid() ? a_oEpisodeInfo[KCDefine.U_KEY_MAX_NUM_ENEMY_OBJS].AsInt : KCDefine.B_VAL_0_INT;
		m_stSize = a_oEpisodeInfo[KCDefine.U_KEY_SIZE].ExIsValid() ? new Vector3(a_oEpisodeInfo[KCDefine.U_KEY_SIZE][KCDefine.B_VAL_0_INT].AsFloat, a_oEpisodeInfo[KCDefine.U_KEY_SIZE][KCDefine.B_VAL_1_INT].AsFloat, a_oEpisodeInfo[KCDefine.U_KEY_SIZE][KCDefine.B_VAL_2_INT].AsFloat) : Vector3.zero;
		
		m_eDifficulty = a_oEpisodeInfo[KCDefine.U_KEY_DIFFICULTY].ExIsValid() ? (EDifficulty)a_oEpisodeInfo[KCDefine.U_KEY_DIFFICULTY].AsInt : EDifficulty.NONE;
		m_eEpisodeKinds = a_oEpisodeInfo[KCDefine.U_KEY_EPISODE_KINDS].ExIsValid() ? (EEpisodeKinds)a_oEpisodeInfo[KCDefine.U_KEY_EPISODE_KINDS].AsInt : EEpisodeKinds.NONE;
		m_eTutorialKinds = a_oEpisodeInfo[KCDefine.U_KEY_TUTORIAL_KINDS].ExIsValid() ? (ETutorialKinds)a_oEpisodeInfo[KCDefine.U_KEY_TUTORIAL_KINDS].AsInt : ETutorialKinds.NONE;

		m_oRewardKindsList = new List<ERewardKinds>();
		m_oRecordValInfoList = new List<STValInfo>();

		m_oClearTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oUnlockTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oDropItemTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oEnemyObjTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		STEpisodeInfo.SetupIDInfo(KCDefine.U_KEY_FMT_ID, a_oEpisodeInfo, out m_stIDInfo);
		STEpisodeInfo.SetupIDInfo(KCDefine.U_KEY_FMT_PREV_ID, a_oEpisodeInfo, out m_stPrevIDInfo);
		STEpisodeInfo.SetupIDInfo(KCDefine.U_KEY_FMT_NEXT_ID, a_oEpisodeInfo, out m_stNextIDInfo);

		for(int i = 0; i < KDefine.G_MAX_NUM_REWARD_KINDS; ++i) {
			string oKey = string.Format(KCDefine.U_KEY_FMT_REWARD_KINDS, i + KCDefine.B_VAL_1_INT);
			if(a_oEpisodeInfo[oKey].ExIsValid()) { m_oRewardKindsList.ExAddVal((ERewardKinds)a_oEpisodeInfo[oKey].AsInt); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_RECORDS; ++i) {
			var stValInfo = new STValInfo(a_oEpisodeInfo[string.Format(KCDefine.U_KEY_FMT_RECORD_VAL_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stValInfo.m_eValType.ExIsValid()) { m_oRecordValInfoList.ExAddVal(stValInfo); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oEpisodeInfo[string.Format(KCDefine.U_KEY_FMT_CLEAR_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oClearTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oEpisodeInfo[string.Format(KCDefine.U_KEY_FMT_UNLOCK_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oUnlockTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oEpisodeInfo[string.Format(KCDefine.U_KEY_FMT_DROP_ITEM_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oDropItemTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oEpisodeInfo[string.Format(KCDefine.U_KEY_FMT_ENEMY_OBJ_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oEnemyObjTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}
	}

	/** 식별자 정보를 설정한다 */
	private static void SetupIDInfo(string a_oIDFmt, SimpleJSON.JSONNode a_oEpisodeInfo, out STIDInfo a_stOutIDInfo) {
		string oID01Key = string.Format(a_oIDFmt, KCDefine.B_VAL_1_INT);
		string oID02Key = string.Format(a_oIDFmt, KCDefine.B_VAL_2_INT);
		string oID03Key = string.Format(a_oIDFmt, KCDefine.B_VAL_3_INT);
		
		a_stOutIDInfo = new STIDInfo() {
			m_nID01 = a_oEpisodeInfo[oID01Key].ExIsValid() ? a_oEpisodeInfo[oID01Key].AsInt : KCDefine.B_IDX_INVALID,
			m_nID02 = a_oEpisodeInfo[oID02Key].ExIsValid() ? a_oEpisodeInfo[oID02Key].AsInt : KCDefine.B_IDX_INVALID,
			m_nID03 = a_oEpisodeInfo[oID03Key].ExIsValid() ? a_oEpisodeInfo[oID03Key].AsInt : KCDefine.B_IDX_INVALID
		};
	}

	/** 식별자 정보를 생성한다 */
	private static void MakeIDInfo(string a_oIDFmt, STIDInfo a_stIDInfo, SimpleJSON.JSONNode a_oOutEpisodeInfo) {
		a_oOutEpisodeInfo.Add(string.Format(a_oIDFmt, KCDefine.B_VAL_1_INT), $"{a_stIDInfo.m_nID01}");
		a_oOutEpisodeInfo.Add(string.Format(a_oIDFmt, KCDefine.B_VAL_2_INT), $"{a_stIDInfo.m_nID02}");
		a_oOutEpisodeInfo.Add(string.Format(a_oIDFmt, KCDefine.B_VAL_3_INT), $"{a_stIDInfo.m_nID03}");
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR || UNITY_STANDALONE
	/** 에피소드 정보를 생성한다 */
	public SimpleJSON.JSONClass MakeEpisodeInfo() {
		var oSizeInfo = new SimpleJSON.JSONArray();
		oSizeInfo.Add($"{m_stSize.x}");
		oSizeInfo.Add($"{m_stSize.y}");
		oSizeInfo.Add($"{m_stSize.z}");

		var oEpisodeInfo = new SimpleJSON.JSONClass();
		var oClearTargetInfoKeyList = m_oClearTargetInfoDict.Keys.ToList();
		var oUnlockTargetInfoKeyList = m_oUnlockTargetInfoDict.Keys.ToList();
		var oDropItemTargetInfoKeyList = m_oDropItemTargetInfoDict.Keys.ToList();
		var oEnemyObjTargetInfoKeyList = m_oEnemyObjTargetInfoDict.Keys.ToList();

		m_stCommonInfo.MakeCommonInfo(oEpisodeInfo);

		STEpisodeInfo.MakeIDInfo(KCDefine.U_KEY_FMT_ID, m_stIDInfo, oEpisodeInfo);
		STEpisodeInfo.MakeIDInfo(KCDefine.U_KEY_FMT_PREV_ID, m_stPrevIDInfo, oEpisodeInfo);
		STEpisodeInfo.MakeIDInfo(KCDefine.U_KEY_FMT_NEXT_ID, m_stNextIDInfo, oEpisodeInfo);

		oEpisodeInfo.Add(KCDefine.U_KEY_DIFFICULTY, $"{(int)m_eDifficulty}");
		oEpisodeInfo.Add(KCDefine.U_KEY_EPISODE_KINDS, $"{(int)m_eEpisodeKinds}");
		oEpisodeInfo.Add(KCDefine.U_KEY_TUTORIAL_KINDS, $"{(int)m_eTutorialKinds}");
		oEpisodeInfo.Add(KCDefine.U_KEY_NUM_SUB_EPISODES, $"{m_nNumSubEpisodes}");
		oEpisodeInfo.Add(KCDefine.U_KEY_MAX_NUM_ENEMY_OBJS, $"{m_nMaxNumEnemyObjs}");
		oEpisodeInfo.Add(KCDefine.U_KEY_SIZE, oSizeInfo);

		for(int i = 0; i < m_oRewardKindsList.Count; ++i) {
			oEpisodeInfo.Add(string.Format(KCDefine.U_KEY_FMT_REWARD_KINDS, i + KCDefine.B_VAL_1_INT), $"{(int)m_oRewardKindsList.ExGetVal(i, ERewardKinds.NONE)}");
		}

		for(int i = 0; i < m_oRecordValInfoList.Count; ++i) {
			m_oRecordValInfoList[i].MakeValInfo(string.Format(KCDefine.U_KEY_FMT_RECORD_VAL_INFO, i + KCDefine.B_VAL_1_INT), oEpisodeInfo);
		}

		for(int i = 0; i < oClearTargetInfoKeyList.Count; ++i) {
			ulong nUniqueTargetInfoID = oClearTargetInfoKeyList[i];
			m_oClearTargetInfoDict[nUniqueTargetInfoID].MakeTargetInfo(string.Format(KCDefine.U_KEY_FMT_CLEAR_TARGET_INFO, i + KCDefine.B_VAL_1_INT), oEpisodeInfo);
		}

		for(int i = 0; i < oUnlockTargetInfoKeyList.Count; ++i) {
			ulong nUniqueTargetInfoID = oUnlockTargetInfoKeyList[i];
			m_oUnlockTargetInfoDict[nUniqueTargetInfoID].MakeTargetInfo(string.Format(KCDefine.U_KEY_FMT_UNLOCK_TARGET_INFO, i + KCDefine.B_VAL_1_INT), oEpisodeInfo);
		}

		for(int i = 0; i < oDropItemTargetInfoKeyList.Count; ++i) {
			ulong nUniqueTargetInfoID = oDropItemTargetInfoKeyList[i];
			m_oDropItemTargetInfoDict[nUniqueTargetInfoID].MakeTargetInfo(string.Format(KCDefine.U_KEY_FMT_DROP_ITEM_TARGET_INFO, i + KCDefine.B_VAL_1_INT), oEpisodeInfo);
		}

		for(int i = 0; i < oEnemyObjTargetInfoKeyList.Count; ++i) {
			ulong nUniqueTargetInfoID = oEnemyObjTargetInfoKeyList[i];
			m_oEnemyObjTargetInfoDict[nUniqueTargetInfoID].MakeTargetInfo(string.Format(KCDefine.U_KEY_FMT_ENEMY_OBJ_TARGET_INFO, i + KCDefine.B_VAL_1_INT), oEpisodeInfo);
		}
		
		return oEpisodeInfo;
	}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	#endregion			// 조건부 함수
}

/** 에피소드 정보 테이블 */
public partial class CEpisodeInfoTable : CSingleton<CEpisodeInfoTable> {
	#region 프로퍼티
	public Dictionary<ulong, STEpisodeInfo> LevelEpisodeInfoDict { get; private set; } = new Dictionary<ulong, STEpisodeInfo>();
	public Dictionary<ulong, STEpisodeInfo> StageEpisodeInfoDict { get; private set; } = new Dictionary<ulong, STEpisodeInfo>();
	public Dictionary<ulong, STEpisodeInfo> ChapterEpisodeInfoDict { get; private set; } = new Dictionary<ulong, STEpisodeInfo>();

	private string EpisodeInfoTablePath {
		get {
#if AB_TEST_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return (CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.A) ? KCDefine.U_RUNTIME_TABLE_P_G_ETC_INFO_SET_A : KCDefine.U_RUNTIME_TABLE_P_G_ETC_INFO_SET_B;
#else
			return (CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.A) ? KCDefine.U_TABLE_P_G_ETC_INFO_SET_A : KCDefine.U_TABLE_P_G_ETC_INFO_SET_B;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
#else
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_ETC_INFO;
#else
			return KCDefine.U_TABLE_P_G_ETC_INFO;
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
	}

	/** 에피소드 정보를 리셋한다 */
	public void ResetEpisodeInfos(string a_oJSONStr) {
		this.ResetEpisodeInfos();
		this.DoLoadEpisodeInfos(a_oJSONStr);
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
		a_stOutLevelEpisodeInfo = this.LevelEpisodeInfoDict.GetValueOrDefault(CFactory.MakeULevelID(a_nLevelID, a_nStageID, a_nChapterID), STEpisodeInfo.INVALID);
		return this.LevelEpisodeInfoDict.ContainsKey(CFactory.MakeULevelID(a_nLevelID, a_nStageID, a_nChapterID));
	}

	/** 스테이지 에피소드 정보를 반환한다 */
	public bool TryGetStageEpisodeInfo(int a_nStageID, out STEpisodeInfo a_stOutStageEpisodeInfo, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		a_stOutStageEpisodeInfo = this.StageEpisodeInfoDict.GetValueOrDefault(CFactory.MakeUniqueStageID(a_nStageID, a_nChapterID), STEpisodeInfo.INVALID);
		return this.StageEpisodeInfoDict.ContainsKey(CFactory.MakeUniqueStageID(a_nStageID, a_nChapterID));
	}

	/** 챕터 에피소드 정보를 반환한다 */
	public bool TryGetChapterEpisodeInfo(int a_nChapterID, out STEpisodeInfo a_stOutChapterEpisodeInfo) {
		a_stOutChapterEpisodeInfo = this.ChapterEpisodeInfoDict.GetValueOrDefault(CFactory.MakeUniqueChapterID(a_nChapterID), STEpisodeInfo.INVALID);
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
				
		var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr);
		var oLevelEpisodeInfosList = new List<SimpleJSON.JSONNode>();
		var oStageEpisodeInfosList = new List<SimpleJSON.JSONNode>();
		var oChapterEpisodeInfosList = new List<SimpleJSON.JSONNode>();

		for(int i = 0; i < KDefine.G_KEY_EPISODE_IT_LEVEL_EPISODE_INFOS_LIST.Count; ++i) {
			oLevelEpisodeInfosList.ExAddVal(oJSONNode[KDefine.G_KEY_EPISODE_IT_LEVEL_EPISODE_INFOS_LIST[i]]);
		}

		for(int i = 0; i < KDefine.G_KEY_EPISODE_IT_STAGE_EPISODE_INFOS_LIST.Count; ++i) {
			oStageEpisodeInfosList.ExAddVal(oJSONNode[KDefine.G_KEY_EPISODE_IT_STAGE_EPISODE_INFOS_LIST[i]]);
		}

		for(int i = 0; i < KDefine.G_KEY_EPISODE_IT_CHAPTER_EPISODE_INFOS_LIST.Count; ++i) {
			oChapterEpisodeInfosList.ExAddVal(oJSONNode[KDefine.G_KEY_EPISODE_IT_CHAPTER_EPISODE_INFOS_LIST[i]]);
		}

		for(int i = 0; i < oLevelEpisodeInfosList.Count; ++i) {
			for(int j = 0; j < oLevelEpisodeInfosList[i].Count; ++j) {
				var stLevelEpisodeInfo = new STEpisodeInfo(oLevelEpisodeInfosList[i][j]);

				// 레벨 에피소드 정보 추가 가능 할 경우
				if(stLevelEpisodeInfo.m_stIDInfo.m_nID01.ExIsValidIdx() && (!this.LevelEpisodeInfoDict.ContainsKey(stLevelEpisodeInfo.m_stIDInfo.UniqueID01) || oLevelEpisodeInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.LevelEpisodeInfoDict.ExReplaceVal(stLevelEpisodeInfo.m_stIDInfo.UniqueID01, stLevelEpisodeInfo);
				}
			}
		}

		for(int i = 0; i < oStageEpisodeInfosList.Count; ++i) {
			for(int j = 0; j < oStageEpisodeInfosList[i].Count; ++j) {
				var stStageEpisodeInfo = new STEpisodeInfo(oStageEpisodeInfosList[i][j]);

				// 스테이지 에피소드 정보 추가 가능 할 경우
				if(stStageEpisodeInfo.m_stIDInfo.m_nID02.ExIsValidIdx() && (!this.StageEpisodeInfoDict.ContainsKey(stStageEpisodeInfo.m_stIDInfo.UniqueID02) || oStageEpisodeInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.StageEpisodeInfoDict.ExReplaceVal(stStageEpisodeInfo.m_stIDInfo.UniqueID02, stStageEpisodeInfo);
				}
			}
		}

		for(int i = 0; i < oChapterEpisodeInfosList.Count; ++i) {
			for(int j = 0; j < oChapterEpisodeInfosList[i].Count; ++j) {
				var stChapterEpisodeInfo = new STEpisodeInfo(oChapterEpisodeInfosList[i][j]);

				// 챕터 에피소드 정보 추가 가능 할 경우
				if(stChapterEpisodeInfo.m_stIDInfo.m_nID03.ExIsValidIdx() && (!this.ChapterEpisodeInfoDict.ContainsKey(stChapterEpisodeInfo.m_stIDInfo.UniqueID03) || oChapterEpisodeInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.ChapterEpisodeInfoDict.ExReplaceVal(stChapterEpisodeInfo.m_stIDInfo.UniqueID03, stChapterEpisodeInfo);
				}
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
		var oJSONNode = SimpleJSON.JSON.Parse(CFunc.ReadStr(this.EpisodeInfoTablePath));
		var oLevelEpisodeInfosList = new List<SimpleJSON.JSONArray>();
		var oStageEpisodeInfosList = new List<SimpleJSON.JSONArray>();
		var oChapterEpisodeInfosList = new List<SimpleJSON.JSONArray>();

		for(int i = 0; i < KDefine.G_KEY_EPISODE_IT_LEVEL_EPISODE_INFOS_LIST.Count; ++i) {
			var oLevelEpisodeInfos = new SimpleJSON.JSONArray();
			oJSONNode.Remove(KDefine.G_KEY_EPISODE_IT_LEVEL_EPISODE_INFOS_LIST[i]);

			foreach(var stKeyVal in this.LevelEpisodeInfoDict) {
				oLevelEpisodeInfos.Add(stKeyVal.Value.MakeEpisodeInfo());
			}

			oJSONNode.Add(KDefine.G_KEY_EPISODE_IT_LEVEL_EPISODE_INFOS_LIST[i], oLevelEpisodeInfos);
		}

		for(int i = 0; i < KDefine.G_KEY_EPISODE_IT_STAGE_EPISODE_INFOS_LIST.Count; ++i) {
			var oStageEpisodeInfos = new SimpleJSON.JSONArray();
			oJSONNode.Remove(KDefine.G_KEY_EPISODE_IT_STAGE_EPISODE_INFOS_LIST[i]);

			foreach(var stKeyVal in this.StageEpisodeInfoDict) {
				oStageEpisodeInfos.Add(stKeyVal.Value.MakeEpisodeInfo());
			}

			oJSONNode.Add(KDefine.G_KEY_EPISODE_IT_STAGE_EPISODE_INFOS_LIST[i], oStageEpisodeInfos);
		}

		for(int i = 0; i < KDefine.G_KEY_EPISODE_IT_CHAPTER_EPISODE_INFOS_LIST.Count; ++i) {
			var oChapterEpisodeInfos = new SimpleJSON.JSONArray();
			oJSONNode.Remove(KDefine.G_KEY_EPISODE_IT_CHAPTER_EPISODE_INFOS_LIST[i]);

			foreach(var stKeyVal in this.ChapterEpisodeInfoDict) {
				oChapterEpisodeInfos.Add(stKeyVal.Value.MakeEpisodeInfo());
			}

			oJSONNode.Add(KDefine.G_KEY_EPISODE_IT_CHAPTER_EPISODE_INFOS_LIST[i], oChapterEpisodeInfos);
		}
		
#if NEWTON_SOFT_JSON_MODULE_ENABLE
		CFunc.WriteStr(this.EpisodeInfoTablePath, JsonConvert.DeserializeObject(oJSONNode.ToString()).ExToJSONStr());
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
	}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion			// 조건부 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
