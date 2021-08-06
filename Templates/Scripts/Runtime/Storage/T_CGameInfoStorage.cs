using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using MessagePack;

#if NEVER_USE_THIS
//! 클리어 정보
[MessagePackObject]
[System.Serializable]
public class CClearInfo : CBaseInfo {
	#region 상수
	private const string KEY_SCORE = "Score";
	private const string KEY_NUM_STARS = "NumStars";

	private const string KEY_BEST_SCORE = "BestScore";
	private const string KEY_BEST_TIMES = "BestTimes";
	#endregion			// 상수

	#region 변수
	[Key(3)] public STIDInfo m_stIDInfo;
	#endregion			// 변수

	#region 프로퍼티
	[IgnoreMember] public int Score {
		get { return m_oIntDict.ExGetVal(CClearInfo.KEY_SCORE, KCDefine.B_VAL_0_INT); }
		set { m_oIntDict.ExReplaceVal(CClearInfo.KEY_SCORE, value); }
	}

	[IgnoreMember] public int NumStars {
		get { return m_oIntDict.ExGetVal(CClearInfo.KEY_NUM_STARS, KCDefine.B_VAL_0_INT); }
		set { m_oIntDict.ExReplaceVal(CClearInfo.KEY_NUM_STARS, value); }
	}

	[IgnoreMember] public int BestScore {
		get { return m_oIntDict.ExGetVal(CClearInfo.KEY_BEST_SCORE, KCDefine.B_VAL_0_INT); }
		set { m_oIntDict.ExReplaceVal(CClearInfo.KEY_BEST_SCORE, value); }
	}

	[IgnoreMember] public int BestTimes {
		get { return m_oIntDict.ExGetVal(CClearInfo.KEY_BEST_TIMES, KCDefine.B_VAL_0_INT); }
		set { m_oIntDict.ExReplaceVal(CClearInfo.KEY_BEST_TIMES, value); }
	}

	[IgnoreMember] public long LevelID => CFactory.MakeUniqueLevelID(m_stIDInfo.m_nID, m_stIDInfo.m_nStageID, m_stIDInfo.m_nChapterID);
	#endregion			// 프로퍼티

	#region 인터페이스
	//! 직렬화 될 경우
	public override void OnBeforeSerialize() {
		base.OnBeforeSerialize();
	}

	//! 역직렬화 되었을 경우
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();
	}
	#endregion			// 인터페이스
}

//! 게임 정보
[MessagePackObject]
[System.Serializable]
public class CGameInfo : CBaseInfo {
	#region 상수
	private const string KEY_DAILY_REWARD_ID = "DailyRewardID";
	private const string KEY_NUM_ACQUIRE_FREE_REWARDS = "NumAcquireFreeRewards";

	private const string KEY_LAST_FREE_REWARD_TIME = "LastFreeRewardTime";
	private const string KEY_LAST_DAILY_REWARD_TIME = "LastDailyRewardTime";
	private const string KEY_LAST_DAILY_MISSION_TIME = "LastDailyMissionTime";
	#endregion			// 상수

	#region 변수
	[Key(61)] public List<EMissionKinds> m_oCompleteMissionKindsList = new List<EMissionKinds>();
	[Key(62)] public List<EMissionKinds> m_oCompleteDailyMissionKindsList = new List<EMissionKinds>();
	[Key(63)] public List<ETutorialKinds> m_oCompleteTutorialKindsList = new List<ETutorialKinds>();

	[Key(151)] public Dictionary<long, CClearInfo> m_oClearInfoDict = new Dictionary<long, CClearInfo>();
	#endregion			// 변수

	#region 프로퍼티
	[IgnoreMember] public System.DateTime LastFreeRewardTime { get; set; } = System.DateTime.Now;
	[IgnoreMember] public System.DateTime LastDailyRewardTime { get; set; } = System.DateTime.Now;
	[IgnoreMember] public System.DateTime LastDailyMissionTime { get; set; } = System.DateTime.Now;

	[IgnoreMember] public int DailyRewardID {
		get { return m_oIntDict.ExGetVal(CGameInfo.KEY_DAILY_REWARD_ID, KCDefine.B_VAL_0_INT); }
		set { m_oIntDict.ExReplaceVal(CGameInfo.KEY_DAILY_REWARD_ID, value); }
	}

	[IgnoreMember] public int NumAcquireFreeRewards {
		get { return m_oIntDict.ExGetVal(CGameInfo.KEY_NUM_ACQUIRE_FREE_REWARDS, KCDefine.B_VAL_0_INT); }
		set { m_oIntDict.ExReplaceVal(CGameInfo.KEY_NUM_ACQUIRE_FREE_REWARDS, value); }
	}

	[IgnoreMember] private string LastDailyMissionTimeStr => m_oStrDict.ExGetVal(CGameInfo.KEY_LAST_DAILY_MISSION_TIME, string.Empty);
	[IgnoreMember] private string LastFreeRewardTimeStr => m_oStrDict.ExGetVal(CGameInfo.KEY_LAST_FREE_REWARD_TIME, string.Empty);
	[IgnoreMember] private string LastDailyRewardTimeStr => m_oStrDict.ExGetVal(CGameInfo.KEY_LAST_DAILY_REWARD_TIME, string.Empty);
	#endregion			// 프로퍼티

	#region 인터페이스
	//! 직렬화 될 경우
	public override void OnBeforeSerialize() {
		base.OnBeforeSerialize();

		m_oStrDict.ExReplaceVal(CGameInfo.KEY_LAST_DAILY_MISSION_TIME, this.LastDailyMissionTime.ExToLongStr());
		m_oStrDict.ExReplaceVal(CGameInfo.KEY_LAST_FREE_REWARD_TIME, this.LastFreeRewardTime.ExToLongStr());
		m_oStrDict.ExReplaceVal(CGameInfo.KEY_LAST_DAILY_REWARD_TIME, this.LastDailyRewardTime.ExToLongStr());
	}

	//! 역직렬화 되었을 경우
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();

		this.LastDailyMissionTime = this.LastDailyMissionTimeStr.ExIsValid() ? this.LastDailyMissionTimeStr.ExToTime(KCDefine.B_DATE_T_FMT_YYYY_MM_DD_HH_MM_SS) : System.DateTime.Today.AddDays(-KCDefine.B_VAL_1_INT);
		this.LastFreeRewardTime = this.LastFreeRewardTimeStr.ExIsValid() ? this.LastFreeRewardTimeStr.ExToTime(KCDefine.B_DATE_T_FMT_YYYY_MM_DD_HH_MM_SS) : System.DateTime.Today.AddDays(-KCDefine.B_VAL_1_INT);
		this.LastDailyRewardTime = this.LastDailyRewardTimeStr.ExIsValid() ? this.LastDailyRewardTimeStr.ExToTime(KCDefine.B_DATE_T_FMT_YYYY_MM_DD_HH_MM_SS) : System.DateTime.Today.AddDays(-KCDefine.B_VAL_1_INT);
	}
	#endregion			// 인터페이스
}

//! 게임 정보 저장소
public class CGameInfoStorage : CSingleton<CGameInfoStorage> {
	#region 프로퍼티
	public EPlayMode PlayMode { get; private set; } = EPlayMode.NONE;
	public CLevelInfo PlayLevelInfo { get; private set; } = null;

	public EItemKinds FreeBooster { get; set; } = EItemKinds.NONE;
	public List<EItemKinds> SelBoosterList { get; private set; } = new List<EItemKinds>();

	public CGameInfo GameInfo { get; private set; } = new CGameInfo() {
		LastDailyMissionTime = System.DateTime.Today.AddDays(-KCDefine.B_VAL_1_INT),
		LastFreeRewardTime = System.DateTime.Today.AddDays(-KCDefine.B_VAL_1_INT),
		LastDailyRewardTime = System.DateTime.Today.AddDays(-KCDefine.B_VAL_1_INT)
	};

	public int TotalNumStars {
		get {
			int nNumStars = KCDefine.B_VAL_0_INT;

			foreach(var stKeyVal in this.GameInfo.m_oClearInfoDict) {
				nNumStars += stKeyVal.Value.NumStars;
			}

			return nNumStars;
		}
	}

	public bool IsEnableGetFreeReward => System.DateTime.Now.ExGetDeltaTimePerDays(this.GameInfo.LastFreeRewardTime).ExIsGreateEquals(KCDefine.B_VAL_1_DBL);
	public bool IsEnableGetDailyReward => System.DateTime.Now.ExGetDeltaTimePerDays(this.GameInfo.LastDailyRewardTime).ExIsGreateEquals(KCDefine.B_VAL_1_DBL);
	public bool IsContinueGetDailyReward => System.DateTime.Now.ExGetDeltaTimePerDays(this.GameInfo.LastDailyRewardTime).ExIsLess(KCDefine.B_VAL_2_DBL);

	public bool IsEnableResetDailyMission => System.DateTime.Now.ExGetDeltaTimePerDays(this.GameInfo.LastDailyMissionTime).ExIsGreateEquals(KCDefine.B_VAL_1_DBL);
	public ERewardKinds DailyRewardKinds => KDefine.G_REWARDS_KINDS_REWARD_IT_DAILY[this.GameInfo.DailyRewardID];

#if ADS_MODULE_ENABLE
	public int AdsSkipTimes { get; set; } = KCDefine.B_VAL_0_INT;

	public System.DateTime PrevAdsTime { get; set; } = System.DateTime.Now;
	public System.DateTime PrevRewardAdsTime { get; set; } = System.DateTime.Now;

	public bool IsEnableShowFullscreenAds {
		get {
			float fAdsDelay = CValTable.Inst.GetFlt(KCDefine.VT_KEY_DELAY_ADS);
			float fAdsDeltaTime = CValTable.Inst.GetFlt(KCDefine.VT_KEY_DELTA_T_ADS);

			double dblDeltaTimeA = System.DateTime.Now.ExGetDeltaTime(CGameInfoStorage.Inst.PrevAdsTime);
			double dblDeltaTimeB = System.DateTime.Now.ExGetDeltaTime(CGameInfoStorage.Inst.PrevRewardAdsTime);

			bool bIsEnableShow = dblDeltaTimeA.ExIsGreateEquals(fAdsDelay) && dblDeltaTimeB.ExIsGreateEquals(fAdsDeltaTime);
			return bIsEnableShow && CGameInfoStorage.Inst.AdsSkipTimes >= KDefine.G_MAX_TIMES_ADS_SKIP;
		}
	}
	
	public bool IsEnableUpdateAdsSkipTimes => true;
#endif			// #if ADS_MODULE_ENABLE
	#endregion			// 프로퍼티

	#region 함수
	//! 부스터를 리셋한다
	public virtual void ResetBoosters() {
		this.FreeBooster = EItemKinds.NONE;
		this.SelBoosterList.Clear();
	}

	//! 다음 일일 보상 식별자를 설정한다
	public void SetupNextDailyRewardID(bool a_bIsResetDailyRewardTime = true) {
		// 일일 보상 시간 리셋 모드 일 경우
		if(a_bIsResetDailyRewardTime) {
			this.GameInfo.LastDailyRewardTime = System.DateTime.Today;
		}

		int nNextDailyRewardID = (this.GameInfo.DailyRewardID + KCDefine.B_VAL_1_INT) % KDefine.G_REWARDS_KINDS_REWARD_IT_DAILY.Length;
		this.SetDailyRewardID(nNextDailyRewardID);
	}

	//! 플레이 레벨 정보를 설정한다
	public void SetupPlayLevelInfo(int a_nID, EPlayMode a_ePlayMode, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.PlayMode = a_ePlayMode;
		this.PlayLevelInfo = CLevelInfoTable.Inst.LoadLevelInfo(a_nID, a_nStageID, a_nChapterID);
	}

	//! 무료 부스터 여부를 검사한다
	public bool IsFreeBooster(EItemKinds a_eBooster) {
		return this.FreeBooster == a_eBooster;
	}

	//! 부스터 선택 여부를 검사한다
	public bool IsSelBooster(EItemKinds a_eBooster) {
		return this.SelBoosterList.Contains(a_eBooster);
	}

	//! 미션 완료 여부를 검사한다
	public bool IsCompleteMission(EMissionKinds a_eMissionKinds) {
		return this.GameInfo.m_oCompleteMissionKindsList.Contains(a_eMissionKinds);
	}

	//! 일일 미션 완료 여부를 검사한다
	public bool IsCompleteDailyMission(EMissionKinds a_eMissionKinds) {
		return this.GameInfo.m_oCompleteDailyMissionKindsList.Contains(a_eMissionKinds);
	}

	//! 튜토리얼 완료 여부를 검사한다
	public bool IsCompleteTutorial(ETutorialKinds a_eTutorialKinds) {
		return this.GameInfo.m_oCompleteTutorialKindsList.Contains(a_eTutorialKinds);
	}

	//! 클리어 여부를 검사한다
	public bool IsClear(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		long nLevelID = CFactory.MakeUniqueLevelID(a_nID, a_nStageID, a_nChapterID);
		return this.GameInfo.m_oClearInfoDict.ContainsKey(nLevelID);
	}

	//! 스테이지 별 개수를 반환한다
	public int GetNumStageStars(int a_nStageID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		int nNumStars = KCDefine.B_VAL_0_INT;
		int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(a_nStageID, a_nChapterID);

		for(int i = 0; i < nNumLevelInfos; ++i) {
			this.TryGetClearInfo(i, out CClearInfo oClearInfo, a_nStageID, a_nChapterID);
			nNumStars += (oClearInfo != null) ? oClearInfo.NumStars : KCDefine.B_VAL_0_INT;
		}

		return nNumStars;
	}

	//! 챕터 별 개수를 반환한다
	public int GetNumChapterStars(int a_nChapterID) {
		int nNumStars = KCDefine.B_VAL_0_INT;
		int nNumStageInfos = CLevelInfoTable.Inst.GetNumStageInfos(a_nChapterID);

		for(int i = 0; i < nNumStageInfos; ++i) {
			nNumStars += this.GetNumStageStars(i, a_nChapterID);
		}
		
		return nNumStars;
	}

	//! 클리어 정보를 반환한다
	public CClearInfo GetClearInfo(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetClearInfo(a_nID, out CClearInfo oClearInfo, a_nStageID, a_nChapterID);
		CAccess.Assert(bIsValid);

		return oClearInfo;
	}

	//! 클리어 정보를 반환한다
	public bool TryGetClearInfo(int a_nID, out CClearInfo a_oOutClearInfo, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		long nLevelID = CFactory.MakeUniqueLevelID(a_nID, a_nStageID, a_nChapterID);
		a_oOutClearInfo = this.GameInfo.m_oClearInfoDict.ExGetVal(nLevelID, null);

		return a_oOutClearInfo != null;
	}

	//! 일일 보상 종류를 변경한다
	public void SetDailyRewardID(int a_nID) {
		CAccess.Assert(KDefine.G_REWARDS_KINDS_REWARD_IT_DAILY.ExIsValidIdx(a_nID));
		this.GameInfo.DailyRewardID = a_nID;
	}

	//! 선택 부스터를 추가한다
	public void AddSelBooster(EItemKinds a_eBooster) {
		CAccess.Assert(!this.IsSelBooster(a_eBooster));
		this.SelBoosterList.Add(a_eBooster);
	}

	//! 무료 보상 획득 횟수를 추가한다
	public void AddNumAcquireFreeRewards(int a_nRewardTimes) {
		int nNumAcquireFreeRewards = this.GameInfo.NumAcquireFreeRewards + a_nRewardTimes;
		this.GameInfo.NumAcquireFreeRewards = Mathf.Clamp(nNumAcquireFreeRewards, KCDefine.B_VAL_0_INT, KDefine.G_MAX_NUM_ACQUIRE_FREE_REWARDS);
	}

	//! 완료 미션을 추가한다
	public void AddCompleteMission(EMissionKinds a_eMissionKinds) {
		CAccess.Assert(!this.IsCompleteMission(a_eMissionKinds));
		this.GameInfo.m_oCompleteMissionKindsList.Add(a_eMissionKinds);
	}

	//! 완료 일일 미션을 추가한다
	public void AddCompleteDailyMission(EMissionKinds a_eMissionKinds) {
		CAccess.Assert(!this.IsCompleteDailyMission(a_eMissionKinds));
		this.GameInfo.m_oCompleteDailyMissionKindsList.Add(a_eMissionKinds);
	}

	//! 완료 튜토리얼을 추가한다
	public void AddCompleteTutorial(ETutorialKinds a_eTutorialKinds) {
		CAccess.Assert(!this.IsCompleteTutorial(a_eTutorialKinds));
		this.GameInfo.m_oCompleteTutorialKindsList.Add(a_eTutorialKinds);
	}

	//! 클리어 정보를 추가한다
	public void AddClearInfo(CClearInfo a_oClearInfo) {
		CAccess.Assert(!this.IsClear(a_oClearInfo.m_stIDInfo.m_nID, a_oClearInfo.m_stIDInfo.m_nStageID, a_oClearInfo.m_stIDInfo.m_nChapterID));
		this.GameInfo.m_oClearInfoDict.Add(a_oClearInfo.LevelID, a_oClearInfo);
	}

	//! 게임 정보를 로드한다
	public CGameInfo LoadGameInfo() {
		return this.LoadGameInfo(KDefine.G_DATA_P_GAME_INFO);
	}

	//! 게임 정보를 로드한다
	public CGameInfo LoadGameInfo(string a_oFilePath) {
		// 파일이 존재 할 경우
		if(File.Exists(a_oFilePath)) {
			this.GameInfo = CFunc.ReadMsgPackObj<CGameInfo>(a_oFilePath);
			CAccess.Assert(this.GameInfo != null);
		}

		return this.GameInfo;
	}

	//! 게임 정보를 저장한다
	public void SaveGameInfo() {
		this.SaveGameInfo(KDefine.G_DATA_P_GAME_INFO);
	}

	//! 게임 정보를 저장한다
	public void SaveGameInfo(string a_oFilePath) {
		CFunc.WriteMsgPackObj(a_oFilePath, this.GameInfo);
	}
	#endregion			// 함수

	#region 조건부 함수
#if ADS_MODULE_ENABLE
	//! 광고 누적 횟수를 추가한다
	public void AddAdsSkipTimes(int a_nTimes) {
		int nSkipTimes = this.AdsSkipTimes + a_nTimes;
		this.AdsSkipTimes = Mathf.Clamp(nSkipTimes, KCDefine.B_VAL_0_INT, KDefine.G_MAX_TIMES_ADS_SKIP);
	}
#endif			// #if ADS_MODULE_ENABLE
	#endregion			// 조건부 함수
}
#endif			// #if NEVER_USE_THIS
