using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using MessagePack;
using Newtonsoft.Json;

#if NEVER_USE_THIS
#if RUNTIME_TEMPLATES_MODULE_ENABLE
/** 클리어 정보 */
[MessagePackObject][System.Serializable]
public class CClearInfo : CBaseInfo {
	#region 상수
	private const string KEY_NUM_CLEAR_MARKS = "NumClearMarks";
	private const string KEY_CLEAR_RECORD = "ClearRecord";
	private const string KEY_BEST_CLEAR_RECORD = "BestClearRecord";
	#endregion			// 상수

	#region 변수
	[JsonIgnore][IgnoreMember][System.NonSerialized] public STIDInfo m_stIDInfo;
	#endregion			// 변수

	#region 프로퍼티
	[JsonIgnore][IgnoreMember] public int NumClearMarks {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(CClearInfo.KEY_NUM_CLEAR_MARKS, KCDefine.B_STR_0_INT)); }
		set { m_oStrDict.ExReplaceVal(CClearInfo.KEY_NUM_CLEAR_MARKS, $"{value}"); }
	}

	[JsonIgnore][IgnoreMember] public string ClearRecord {
		get { return m_oStrDict.GetValueOrDefault(CClearInfo.KEY_CLEAR_RECORD, $"{KCDefine.B_VAL_0_INT}"); }
		set { m_oStrDict.ExReplaceVal(CClearInfo.KEY_CLEAR_RECORD, value); }
	}

	[JsonIgnore][IgnoreMember] public string BestClearRecord {
		get { return m_oStrDict.GetValueOrDefault(CClearInfo.KEY_BEST_CLEAR_RECORD, $"{KCDefine.B_VAL_0_INT}"); }
		set { m_oStrDict.ExReplaceVal(CClearInfo.KEY_BEST_CLEAR_RECORD, value); }
	}

	[JsonIgnore][IgnoreMember] public long LevelID => CFactory.MakeUniqueLevelID(m_stIDInfo.m_nID, m_stIDInfo.m_nStageID, m_stIDInfo.m_nChapterID);

	[JsonIgnore][IgnoreMember] public long IntRecord => long.TryParse(this.ClearRecord, out long nClearRecord) ? nClearRecord : KCDefine.B_VAL_0_INT;
	[JsonIgnore][IgnoreMember] public long IntBestClearRecord => long.TryParse(this.BestClearRecord, out long nBestClearRecord) ? nBestClearRecord : KCDefine.B_VAL_0_INT;

	[JsonIgnore][IgnoreMember] public double RealClearRecord => double.TryParse(this.ClearRecord, out double dblClearRecord) ? dblClearRecord : KCDefine.B_VAL_0_DBL;
	[JsonIgnore][IgnoreMember] public double RealBestClearRecord => double.TryParse(this.BestClearRecord, out double dblBestClearRecord) ? dblBestClearRecord : KCDefine.B_VAL_0_DBL;
	#endregion			// 프로퍼티

	#region IMessagePackSerializationCallbackReceiver
	/** 직렬화 될 경우 */
	public override void OnBeforeSerialize() {
		base.OnBeforeSerialize();
	}

	/** 역직렬화 되었을 경우 */
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();

		// 버전이 다를 경우
		if(this.Ver.CompareTo(KDefine.G_VER_CLEAR_INFO) < KCDefine.B_COMPARE_EQUALS) {
			// Do Something
		}
	}
	#endregion			// IMessagePackSerializationCallbackReceiver

	#region 함수
	/** 생성자 */
	public CClearInfo() : base(KDefine.G_VER_CLEAR_INFO) {
		// Do Something
	}
	#endregion			// 함수
}

/** 게임 정보 */
[MessagePackObject][System.Serializable]
public class CGameInfo : CBaseInfo {
	#region 상수
	private const string KEY_DAILY_REWARD_ID = "DailyRewardID";
	private const string KEY_FREE_REWARD_ACQUIRE_TIMES = "FreeRewardAcquireTimes";

	private const string KEY_PREV_FREE_REWARD_TIME = "PrevFreeRewardTime";
	private const string KEY_PREV_DAILY_REWARD_TIME = "PrevDailyRewardTime";
	private const string KEY_PREV_DAILY_MISSION_TIME = "PrevDailyMissionTime";
	#endregion			// 상수

	#region 변수
	[Key(51)] public List<long> m_oUnlockLevelIDList = new List<long>();
	[Key(52)] public List<long> m_oUnlockStageIDList = new List<long>();
	[Key(53)] public List<long> m_oUnlockChapterIDList = new List<long>();

	[Key(54)] public List<long> m_oAcquireRewardLevelIDList = new List<long>();
	[Key(55)] public List<long> m_oAcquireRewardStageIDList = new List<long>();
	[Key(56)] public List<long> m_oAcquireRewardChapterIDList = new List<long>();

	[Key(61)] public List<EMissionKinds> m_oCompleteMissionKindsList = new List<EMissionKinds>();
	[Key(62)] public List<EMissionKinds> m_oCompleteDailyMissionKindsList = new List<EMissionKinds>();
	[Key(63)] public List<ETutorialKinds> m_oCompleteTutorialKindsList = new List<ETutorialKinds>();

	[Key(151)] public Dictionary<long, CClearInfo> m_oClearInfoDict = new Dictionary<long, CClearInfo>();
	#endregion			// 변수

	#region 프로퍼티
	[JsonIgnore][IgnoreMember] public int DailyRewardID {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(CGameInfo.KEY_DAILY_REWARD_ID, KCDefine.B_STR_0_INT)); }
		set { m_oStrDict.ExReplaceVal(CGameInfo.KEY_DAILY_REWARD_ID, $"{value}"); }
	}

	[JsonIgnore][IgnoreMember] public int FreeRewardAcquireTimes {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(CGameInfo.KEY_FREE_REWARD_ACQUIRE_TIMES, KCDefine.B_STR_0_INT)); }
		set { m_oStrDict.ExReplaceVal(CGameInfo.KEY_FREE_REWARD_ACQUIRE_TIMES, $"{value}"); }
	}

	[JsonIgnore][IgnoreMember] public System.DateTime PrevDailyMissionTime {
		get { return this.PrevDailyMissionTimeStr.ExIsValid() ? this.CorrectPrevDailyMissionTimeStr.ExToTime(KCDefine.B_DATE_T_FMT_SLASH_YYYY_MM_DD_HH_MM_SS) : System.DateTime.Today.AddDays(-KCDefine.B_VAL_1_INT); }
		set { m_oStrDict.ExReplaceVal(CGameInfo.KEY_PREV_DAILY_MISSION_TIME, value.ExToLongStr()); }
	}

	[JsonIgnore][IgnoreMember] public System.DateTime PrevFreeRewardTime {
		get { return this.PrevFreeRewardTimeStr.ExIsValid() ? this.CorrectPrevFreeRewardTimeStr.ExToTime(KCDefine.B_DATE_T_FMT_SLASH_YYYY_MM_DD_HH_MM_SS) : System.DateTime.Today.AddDays(-KCDefine.B_VAL_1_INT); }
		set { m_oStrDict.ExReplaceVal(CGameInfo.KEY_PREV_FREE_REWARD_TIME, value.ExToLongStr()); }
	}

	[JsonIgnore][IgnoreMember] public System.DateTime PrevDailyRewardTime {
		get { return this.PrevDailyRewardTimeStr.ExIsValid() ? this.CorrectPrevDailyRewardTimeStr.ExToTime(KCDefine.B_DATE_T_FMT_SLASH_YYYY_MM_DD_HH_MM_SS) : System.DateTime.Today.AddDays(-KCDefine.B_VAL_1_INT); }
		set { m_oStrDict.ExReplaceVal(CGameInfo.KEY_PREV_DAILY_REWARD_TIME, value.ExToLongStr()); }
	}

	[JsonIgnore][IgnoreMember] private string PrevDailyMissionTimeStr => m_oStrDict.GetValueOrDefault(CGameInfo.KEY_PREV_DAILY_MISSION_TIME, string.Empty);
	[JsonIgnore][IgnoreMember] private string PrevFreeRewardTimeStr => m_oStrDict.GetValueOrDefault(CGameInfo.KEY_PREV_FREE_REWARD_TIME, string.Empty);
	[JsonIgnore][IgnoreMember] private string PrevDailyRewardTimeStr => m_oStrDict.GetValueOrDefault(CGameInfo.KEY_PREV_DAILY_REWARD_TIME, string.Empty);

	[JsonIgnore][IgnoreMember] private string CorrectPrevDailyMissionTimeStr => this.PrevDailyMissionTimeStr.Contains(KCDefine.B_TOKEN_SPLASH) ? this.PrevDailyMissionTimeStr : this.PrevDailyMissionTimeStr.ExToTime(KCDefine.B_DATE_T_FMT_YYYY_MM_DD_HH_MM_SS).ExToLongStr();
	[JsonIgnore][IgnoreMember] private string CorrectPrevFreeRewardTimeStr => this.PrevFreeRewardTimeStr.Contains(KCDefine.B_TOKEN_SPLASH) ? this.PrevFreeRewardTimeStr : this.PrevFreeRewardTimeStr.ExToTime(KCDefine.B_DATE_T_FMT_YYYY_MM_DD_HH_MM_SS).ExToLongStr();
	[JsonIgnore][IgnoreMember] private string CorrectPrevDailyRewardTimeStr => this.PrevDailyRewardTimeStr.Contains(KCDefine.B_TOKEN_SPLASH) ? this.PrevDailyRewardTimeStr : this.PrevDailyRewardTimeStr.ExToTime(KCDefine.B_DATE_T_FMT_YYYY_MM_DD_HH_MM_SS).ExToLongStr();
	#endregion			// 프로퍼티

	#region IMessagePackSerializationCallbackReceiver
	/** 직렬화 될 경우 */
	public override void OnBeforeSerialize() {
		base.OnBeforeSerialize();
	}

	/** 역직렬화 되었을 경우 */
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();
		
		m_oUnlockLevelIDList = m_oUnlockLevelIDList ?? new List<long>();
		m_oUnlockStageIDList = m_oUnlockStageIDList ?? new List<long>();
		m_oUnlockChapterIDList = m_oUnlockChapterIDList ?? new List<long>();
		
		m_oAcquireRewardLevelIDList = m_oAcquireRewardLevelIDList ?? new List<long>();
		m_oAcquireRewardStageIDList = m_oAcquireRewardStageIDList ?? new List<long>();
		m_oAcquireRewardChapterIDList = m_oAcquireRewardChapterIDList ?? new List<long>();
		
		m_oCompleteMissionKindsList = m_oCompleteMissionKindsList ?? new List<EMissionKinds>();
		m_oCompleteDailyMissionKindsList = m_oCompleteDailyMissionKindsList ?? new List<EMissionKinds>();
		m_oCompleteTutorialKindsList = m_oCompleteTutorialKindsList ?? new List<ETutorialKinds>();
		
		m_oClearInfoDict = m_oClearInfoDict ?? new Dictionary<long, CClearInfo>();

		// 버전이 다를 경우
		if(this.Ver.CompareTo(KDefine.G_VER_GAME_INFO) < KCDefine.B_COMPARE_EQUALS) {
			// Do Something
		}
	}
	#endregion			// IMessagePackSerializationCallbackReceiver

	#region 함수
	/** 생성자 */
	public CGameInfo() : base(KDefine.G_VER_GAME_INFO) {
		// Do Something
	}
	#endregion			// 함수
}

/** 게임 정보 저장소 */
public class CGameInfoStorage : CSingleton<CGameInfoStorage> {
	#region 프로퍼티
	public EPlayMode PlayMode { get; private set; } = EPlayMode.NONE;
	public EItemKinds FreeBooster { get; set; } = EItemKinds.NONE;

	public CLevelInfo PlayLevelInfo { get; private set; } = null;
	public List<EItemKinds> SelBoosterList { get; private set; } = new List<EItemKinds>();

	public CGameInfo GameInfo { get; private set; } = new CGameInfo() {
		PrevDailyMissionTime = System.DateTime.Today.AddDays(-KCDefine.B_VAL_1_DBL), PrevFreeRewardTime = System.DateTime.Today.AddDays(-KCDefine.B_VAL_1_DBL), PrevDailyRewardTime = System.DateTime.Today.AddDays(-KCDefine.B_VAL_1_DBL)
	};

	public int TotalNumClearMarks {
		get {
			int nNumClearMarks = KCDefine.B_VAL_0_INT;

			foreach(var stKeyVal in this.GameInfo.m_oClearInfoDict) {
				nNumClearMarks += stKeyVal.Value.NumClearMarks;
			}

			return nNumClearMarks;
		}
	}

	public bool IsEnableGetFreeReward => System.DateTime.Now.ExGetDeltaTimePerDays(this.GameInfo.PrevFreeRewardTime).ExIsGreateEquals(KCDefine.B_VAL_1_DBL);
	public bool IsEnableGetDailyReward => System.DateTime.Now.ExGetDeltaTimePerDays(this.GameInfo.PrevDailyRewardTime).ExIsGreateEquals(KCDefine.B_VAL_1_DBL);
	public bool IsContinueGetDailyReward => System.DateTime.Now.ExGetDeltaTimePerDays(this.GameInfo.PrevDailyRewardTime).ExIsLess(KCDefine.B_VAL_2_DBL);

	public bool IsEnableResetDailyMission => System.DateTime.Now.ExGetDeltaTimePerDays(this.GameInfo.PrevDailyMissionTime).ExIsGreateEquals(KCDefine.B_VAL_1_DBL);
	public ERewardKinds DailyRewardKinds => KDefine.G_REWARDS_KINDS_DAILY_REWARD_LIST[this.GameInfo.DailyRewardID];
	#endregion			// 프로퍼티

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 함수
	/** 게임 정보를 리셋한다 */
	public virtual void ResetGameInfo(string a_oBase64Str) {
		CFunc.ShowLog($"CGameInfoStorage.ResetGameInfo: {a_oBase64Str}");
		CAccess.Assert(a_oBase64Str.ExIsValid());

		this.GameInfo = a_oBase64Str.ExMsgPackBase64StrToObj<CGameInfo>();
		CAccess.Assert(this.GameInfo != null);
	}

	/** 부스터를 리셋한다 */
	public virtual void ResetBoosters() {
		this.FreeBooster = EItemKinds.NONE;
		this.SelBoosterList.Clear();
	}

	/** 다음 일일 보상 식별자를 설정한다 */
	public void SetupNextDailyRewardID(bool a_bIsResetDailyRewardTime = true) {
		// 일일 보상 시간 리셋 모드 일 경우
		if(a_bIsResetDailyRewardTime) {
			this.GameInfo.PrevDailyRewardTime = System.DateTime.Today;
		}

		int nNextDailyRewardID = (this.GameInfo.DailyRewardID + KCDefine.B_VAL_1_INT) % KDefine.G_REWARDS_KINDS_DAILY_REWARD_LIST.Count;
		this.SetDailyRewardID(nNextDailyRewardID);
	}

	/** 플레이 레벨 정보를 설정한다 */
	public void SetupPlayLevelInfo(int a_nID, EPlayMode a_ePlayMode, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.PlayMode = a_ePlayMode;

#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		this.PlayLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(a_nID, a_nStageID, a_nChapterID);
#else
		this.PlayLevelInfo = CLevelInfoTable.Inst.LoadLevelInfo(a_nID, a_nStageID, a_nChapterID);
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 무료 부스터 여부를 검사한다 */
	public bool IsFreeBooster(EItemKinds a_eBooster) {
		return this.FreeBooster == a_eBooster;
	}

	/** 부스터 선택 여부를 검사한다 */
	public bool IsSelBooster(EItemKinds a_eBooster) {
		return this.SelBoosterList.Contains(a_eBooster);
	}

	/** 미션 완료 여부를 검사한다 */
	public bool IsCompleteMission(EMissionKinds a_eMissionKinds) {
		return this.GameInfo.m_oCompleteMissionKindsList.Contains(a_eMissionKinds);
	}

	/** 일일 미션 완료 여부를 검사한다 */
	public bool IsCompleteDailyMission(EMissionKinds a_eMissionKinds) {
		return this.GameInfo.m_oCompleteDailyMissionKindsList.Contains(a_eMissionKinds);
	}

	/** 튜토리얼 완료 여부를 검사한다 */
	public bool IsCompleteTutorial(ETutorialKinds a_eTutorialKinds) {
		return this.GameInfo.m_oCompleteTutorialKindsList.Contains(a_eTutorialKinds);
	}

	/** 레벨 클리어 여부를 검사한다 */
	public bool IsClearLevel(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		return this.GameInfo.m_oClearInfoDict.ContainsKey(CFactory.MakeUniqueLevelID(a_nID, a_nStageID, a_nChapterID));
	}

	/** 레벨 잠금 해제 여부를 검사한다 */
	public bool IsUnlockLevel(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		return this.GameInfo.m_oUnlockLevelIDList.Contains(CFactory.MakeUniqueLevelID(a_nID, a_nStageID, a_nChapterID));
	}

	/** 스테이지 잠금 해제 여부를 검사한다 */
	public bool IsUnlockStage(int a_nID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		return this.GameInfo.m_oUnlockStageIDList.Contains(CFactory.MakeUniqueStageID(a_nID));
	}

	/** 챕터 잠금 해제 여부를 검사한다 */
	public bool IsUnlockChapter(int a_nID) {
		return this.GameInfo.m_oUnlockChapterIDList.Contains(CFactory.MakeUniqueChapterID(a_nID));
	}

	/** 레벨 보상 획득 여부를 검사한다 */
	public bool IsAcquireRewardLevel(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		return this.GameInfo.m_oAcquireRewardLevelIDList.Contains(CFactory.MakeUniqueLevelID(a_nID, a_nStageID, a_nChapterID));
	}

	/** 스테이지 보상 획득 여부를 검사한다 */
	public bool IsAcquireRewardStage(int a_nID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		return this.GameInfo.m_oAcquireRewardStageIDList.Contains(CFactory.MakeUniqueStageID(a_nID, a_nChapterID));
	}

	/** 챕터 보상 획득 여부를 검사한다 */
	public bool IsAcquireRewardChapter(int a_nID) {
		return this.GameInfo.m_oAcquireRewardChapterIDList.Contains(CFactory.MakeUniqueChapterID(a_nID));
	}

	/** 스테이지 클리어 마크 개수를 반환한다 */
	public int GetNumStageClearMarks(int a_nID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		int nNumClearMarks = KCDefine.B_VAL_0_INT;
		int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(a_nID, a_nChapterID);

		for(int i = 0; i < nNumLevelInfos; ++i) {
			this.TryGetClearInfo(i, out CClearInfo oClearInfo, a_nID, a_nChapterID);
			nNumClearMarks += (oClearInfo != null) ? oClearInfo.NumClearMarks : KCDefine.B_VAL_0_INT;
		}

		return nNumClearMarks;
	}

	/** 챕터 클리어 마크 개수를 반환한다 */
	public int GetNumChapterClearMarks(int a_nChapterID) {
		int nNumClearMarks = KCDefine.B_VAL_0_INT;
		int nNumStageInfos = CLevelInfoTable.Inst.GetNumStageInfos(a_nChapterID);

		for(int i = 0; i < nNumStageInfos; ++i) {
			nNumClearMarks += this.GetNumStageClearMarks(i, a_nChapterID);
		}
		
		return nNumClearMarks;
	}

	/** 클리어 정보 개수를 반환한다 */
	public int GetNumClearInfos(int a_nID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(a_nID, a_nChapterID);

		for(int i = 0; i < nNumLevelInfos; ++i) {
			// 클리어 정보가 없을 경우
			if(!this.TryGetClearInfo(i, out CClearInfo oClearInfo, a_nID, a_nChapterID)) {
				return i;
			}
		}

		return nNumLevelInfos;
	}

	/** 클리어 정보를 반환한다 */
	public CClearInfo GetClearInfo(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetClearInfo(a_nID, out CClearInfo oClearInfo, a_nStageID, a_nChapterID);
		CAccess.Assert(bIsValid);

		return oClearInfo;
	}

	/** 클리어 정보를 반환한다 */
	public bool TryGetClearInfo(int a_nID, out CClearInfo a_oOutClearInfo, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		a_oOutClearInfo = this.GameInfo.m_oClearInfoDict.GetValueOrDefault(CFactory.MakeUniqueLevelID(a_nID, a_nStageID, a_nChapterID), null);
		return a_oOutClearInfo != null;
	}

	/** 일일 보상 종류를 변경한다 */
	public void SetDailyRewardID(int a_nID) {
		CAccess.Assert(KDefine.G_REWARDS_KINDS_DAILY_REWARD_LIST.ExIsValidIdx(a_nID));
		this.GameInfo.DailyRewardID = a_nID;
	}

	/** 선택 부스터를 추가한다 */
	public void AddSelBooster(EItemKinds a_eBooster) {
		CAccess.Assert(!this.IsSelBooster(a_eBooster));
		this.SelBoosterList.Add(a_eBooster);
	}

	/** 무료 보상 획득 횟수를 추가한다 */
	public void AddFreeRewardAcquireTimes(int a_nRewardTimes) {
		this.GameInfo.FreeRewardAcquireTimes = Mathf.Clamp(this.GameInfo.FreeRewardAcquireTimes + a_nRewardTimes, KCDefine.B_VAL_0_INT, KDefine.G_MAX_TIMES_ACQUIRE_FREE_REWARDS);
	}

	/** 완료 미션을 추가한다 */
	public void AddCompleteMission(EMissionKinds a_eMissionKinds) {
		CAccess.Assert(!this.IsCompleteMission(a_eMissionKinds));
		this.GameInfo.m_oCompleteMissionKindsList.Add(a_eMissionKinds);
	}

	/** 완료 일일 미션을 추가한다 */
	public void AddCompleteDailyMission(EMissionKinds a_eMissionKinds) {
		CAccess.Assert(!this.IsCompleteDailyMission(a_eMissionKinds));
		this.GameInfo.m_oCompleteDailyMissionKindsList.Add(a_eMissionKinds);
	}

	/** 완료 튜토리얼을 추가한다 */
	public void AddCompleteTutorial(ETutorialKinds a_eTutorialKinds) {
		CAccess.Assert(!this.IsCompleteTutorial(a_eTutorialKinds));
		this.GameInfo.m_oCompleteTutorialKindsList.Add(a_eTutorialKinds);
	}

	/** 클리어 정보를 추가한다 */
	public void AddClearInfo(CClearInfo a_oClearInfo) {
		CAccess.Assert(!this.IsClearLevel(a_oClearInfo.m_stIDInfo.m_nID, a_oClearInfo.m_stIDInfo.m_nStageID, a_oClearInfo.m_stIDInfo.m_nChapterID));
		this.GameInfo.m_oClearInfoDict.TryAdd(a_oClearInfo.LevelID, a_oClearInfo);
	}

	/** 잠금 해제 레벨을 추가한다 */
	public void AddUnlockLevel(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		CAccess.Assert(!this.IsUnlockLevel(a_nID, a_nStageID, a_nChapterID));
		this.GameInfo.m_oUnlockLevelIDList.Add(CFactory.MakeUniqueLevelID(a_nID, a_nStageID, a_nChapterID));
	}

	/** 잠금 해제 스테이지를 추가한다 */
	public void AddUnlockStage(int a_nID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		CAccess.Assert(!this.IsUnlockStage(a_nID, a_nChapterID));
		this.GameInfo.m_oUnlockStageIDList.Add(CFactory.MakeUniqueStageID(a_nID, a_nChapterID));
	}

	/** 잠금 해제 챕터를 추가한다 */
	public void AddUnlockChapter(int a_nID) {
		CAccess.Assert(!this.IsUnlockChapter(a_nID));
		this.GameInfo.m_oUnlockChapterIDList.Add(CFactory.MakeUniqueChapterID(a_nID));
	}

	/** 보상 획득 레벨을 추가한다 */
	public void AddAcquireRewardLevel(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		CAccess.Assert(!this.IsAcquireRewardLevel(a_nID, a_nStageID, a_nChapterID));
		this.GameInfo.m_oAcquireRewardLevelIDList.Add(CFactory.MakeUniqueLevelID(a_nID, a_nStageID, a_nChapterID));
	}

	/** 보상 획득 스테이지를 추가한다 */
	public void AddAcquireRewardStage(int a_nID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		CAccess.Assert(!this.IsUnlockStage(a_nID, a_nChapterID));
		this.GameInfo.m_oAcquireRewardStageIDList.Add(CFactory.MakeUniqueStageID(a_nID, a_nChapterID));
	}

	/** 보상 획득 챕터를 추가한다 */
	public void AddAcquireRewardChapter(int a_nID) {
		CAccess.Assert(!this.IsUnlockChapter(a_nID));
		this.GameInfo.m_oAcquireRewardChapterIDList.Add(CFactory.MakeUniqueChapterID(a_nID));
	}

	/** 게임 정보를 로드한다 */
	public CGameInfo LoadGameInfo() {
		return this.LoadGameInfo(KDefine.G_DATA_P_GAME_INFO);
	}

	/** 게임 정보를 로드한다 */
	public CGameInfo LoadGameInfo(string a_oFilePath) {
		// 파일이 존재 할 경우
		if(File.Exists(a_oFilePath)) {
#if MSG_PACK_ENABLE
			this.GameInfo = CFunc.ReadMsgPackObj<CGameInfo>(a_oFilePath);
#else
			this.GameInfo = CFunc.ReadJSONObj<CGameInfo>(a_oFilePath);
#endif			// #if MSG_PACK_ENABLE

			CAccess.Assert(this.GameInfo != null);

			foreach(var stKeyVal in this.GameInfo.m_oClearInfoDict) {
				stKeyVal.Value.m_stIDInfo = CFactory.MakeIDInfo(stKeyVal.Key.ExUniqueLevelIDToID(), stKeyVal.Key.ExUniqueLevelIDToStageID(), stKeyVal.Key.ExUniqueLevelIDToChapterID());
			}
		}

		return this.GameInfo;
	}

	/** 게임 정보를 저장한다 */
	public void SaveGameInfo() {
		this.SaveGameInfo(KDefine.G_DATA_P_GAME_INFO);
	}

	/** 게임 정보를 저장한다 */
	public void SaveGameInfo(string a_oFilePath) {
#if MSG_PACK_ENABLE
		CFunc.WriteMsgPackObj(a_oFilePath, this.GameInfo);
#else
		CFunc.WriteJSONObj(a_oFilePath, this.GameInfo);
#endif			// #if MSG_PACK_ENABLE
	}
	#endregion			// 함수

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if NEVER_USE_THIS
