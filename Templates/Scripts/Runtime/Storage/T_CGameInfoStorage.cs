using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MessagePack;

#if NEVER_USE_THIS
//! 클리어 정보
[MessagePackObject]
[System.Serializable]
public sealed class CClearInfo : CBaseInfo {
	#region 상수
	private const string KEY_ID = "ID";
	private const string KEY_SCORE = "Score";
	private const string KEY_NUM_STARS = "NumStars";
	#endregion			// 상수

	#region 프로퍼티
	[IgnoreMember] public int ID {
		get { return m_oIntList.ExGetVal(CClearInfo.KEY_ID, KCDefine.B_VAL_0_INT); }
		set { m_oIntList.ExReplaceVal(CClearInfo.KEY_ID, value); }
	}

	[IgnoreMember] public int Score {
		get { return m_oIntList.ExGetVal(CClearInfo.KEY_SCORE, KCDefine.B_VAL_0_INT); }
		set { m_oIntList.ExReplaceVal(CClearInfo.KEY_SCORE, value); }
	}

	[IgnoreMember] public int NumStars {
		get { return m_oIntList.ExGetVal(CClearInfo.KEY_NUM_STARS, KCDefine.B_VAL_0_INT); }
		set { m_oIntList.ExReplaceVal(CClearInfo.KEY_NUM_STARS, value); }
	}
	#endregion			// 프로퍼티

	#region 함수
	//! 생성자
	public CClearInfo() : base(KDefine.B_VER_CLEAR_INFO) {
		// Do Nothing
	}
	#endregion			// 함수
}

//! 게임 정보
[MessagePackObject]
[System.Serializable]
public sealed class CGameInfo : CBaseInfo {
	#region 상수
	private const string KEY_DAILY_REWARD_ID = "DailyRewardID";
	private const string KEY_FREE_REWARD_TIMES = "FreeRewardTimes";

	private const string KEY_LAST_DAILY_MISSION_TIME = "LastDailyMissionTime";
	private const string KEY_LAST_FREE_REWARD_TIME = "LastFreeRewardTime";
	private const string KEY_LAST_DAILY_REWARD_TIME = "LastDailyRewardTime";
	#endregion			// 상수

	#region 변수
	[Key(35)] public List<EMissionKinds> m_oCompleteMissionKindsList = new List<EMissionKinds>();
	[Key(36)] public List<EMissionKinds> m_oCompleteDailyMissionKindsList = new List<EMissionKinds>();
	[Key(37)] public List<ETutorialKinds> m_oCompleteTutorialKindsList = new List<ETutorialKinds>();

	[Key(101)] public Dictionary<int, CClearInfo> m_oClearInfoList = new Dictionary<int, CClearInfo>();
	#endregion			// 변수

	#region 프로퍼티
	[IgnoreMember] public System.DateTime LastDailyMissionTime { get; set; } = System.DateTime.Now;
	[IgnoreMember] public System.DateTime LastFreeRewardTime { get; set; } = System.DateTime.Now;
	[IgnoreMember] public System.DateTime LastDailyRewardTime { get; set; } = System.DateTime.Now;

	[IgnoreMember] public int DailyRewardID {
		get { return m_oIntList.ExGetVal(CGameInfo.KEY_DAILY_REWARD_ID, KCDefine.B_VAL_0_INT); }
		set { m_oIntList.ExReplaceVal(CGameInfo.KEY_DAILY_REWARD_ID, value); }
	}

	[IgnoreMember] public int FreeRewardTimes {
		get { return m_oIntList.ExGetVal(CGameInfo.KEY_FREE_REWARD_TIMES, KCDefine.B_VAL_0_INT); }
		set { m_oIntList.ExReplaceVal(CGameInfo.KEY_FREE_REWARD_TIMES, value); }
	}

	[IgnoreMember] private string LastDailyMissionTimeStr => m_oStrList.ExGetVal(CGameInfo.KEY_LAST_DAILY_MISSION_TIME, string.Empty);
	[IgnoreMember] private string LastFreeRewardTimeStr => m_oStrList.ExGetVal(CGameInfo.KEY_LAST_FREE_REWARD_TIME, string.Empty);
	[IgnoreMember] private string LastDailyRewardTimeStr => m_oStrList.ExGetVal(CGameInfo.KEY_LAST_DAILY_REWARD_TIME, string.Empty);
	#endregion			// 프로퍼티

	#region 인터페이스
	//! 직렬화 될 경우
	public override void OnBeforeSerialize() {
		m_oStrList.ExReplaceVal(CGameInfo.KEY_LAST_DAILY_MISSION_TIME, this.LastDailyMissionTime.ExToLongStr());
		m_oStrList.ExReplaceVal(CGameInfo.KEY_LAST_FREE_REWARD_TIME, this.LastFreeRewardTime.ExToLongStr());
		m_oStrList.ExReplaceVal(CGameInfo.KEY_LAST_DAILY_REWARD_TIME, this.LastDailyRewardTime.ExToLongStr());
	}

	//! 역직렬화 되었을 경우
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();
		
		m_oCompleteMissionKindsList = m_oCompleteMissionKindsList ?? new List<EMissionKinds>();
		m_oCompleteDailyMissionKindsList = m_oCompleteDailyMissionKindsList ?? new List<EMissionKinds>();
		m_oCompleteTutorialKindsList = m_oCompleteTutorialKindsList ?? new List<ETutorialKinds>();

		m_oClearInfoList = m_oClearInfoList ?? new Dictionary<int, CClearInfo>();

		this.LastDailyMissionTime = this.LastDailyMissionTimeStr.ExIsValid() ? this.LastDailyMissionTimeStr.ExToTime(KCDefine.B_DATE_T_FMT_YYYY_MM_DD_HH_MM_SS) : System.DateTime.Today.AddDays(-KCDefine.B_VAL_1_INT);
		this.LastFreeRewardTime = this.LastFreeRewardTimeStr.ExIsValid() ? this.LastFreeRewardTimeStr.ExToTime(KCDefine.B_DATE_T_FMT_YYYY_MM_DD_HH_MM_SS) : System.DateTime.Today.AddDays(-KCDefine.B_VAL_1_INT);
		this.LastDailyRewardTime = this.LastDailyRewardTimeStr.ExIsValid() ? this.LastDailyRewardTimeStr.ExToTime(KCDefine.B_DATE_T_FMT_YYYY_MM_DD_HH_MM_SS) : System.DateTime.Today.AddDays(-KCDefine.B_VAL_1_INT);
	}
	#endregion			// 인터페이스

	#region 함수
	//! 생성자
	public CGameInfo() : base(KDefine.B_VER_GAME_INFO) {
		// Do Nothing
	}
	#endregion			// 함수
}

//! 게임 정보 저장소
public class CGameInfoStorage : CSingleton<CGameInfoStorage> {
	#region 프로퍼티
	public EItemKinds FreeBooster { get; set; } = EItemKinds.NONE;

	public CGameInfo GameInfo { get; private set; } = new CGameInfo() {
		LastDailyMissionTime = System.DateTime.Today.AddDays(-KCDefine.B_VAL_1_INT),
		LastFreeRewardTime = System.DateTime.Today.AddDays(-KCDefine.B_VAL_1_INT),
		LastDailyRewardTime = System.DateTime.Today.AddDays(-KCDefine.B_VAL_1_INT)
	};

	public List<EItemKinds> SelBoosterList { get; private set; } = new List<EItemKinds>();
	public bool IsEnableResetDailyMission => System.DateTime.Now.ExGetDeltaTimePerDays(this.GameInfo.LastDailyMissionTime).ExIsGreateEquals(KCDefine.B_VAL_1_DBL);
	
	public bool IsEnableGetFreeReward => System.DateTime.Now.ExGetDeltaTimePerDays(this.GameInfo.LastFreeRewardTime).ExIsGreateEquals(KCDefine.B_VAL_1_DBL);
	public bool IsEnableGetDailyReward => System.DateTime.Now.ExGetDeltaTimePerDays(this.GameInfo.LastDailyRewardTime).ExIsGreateEquals(KCDefine.B_VAL_1_DBL);
	public bool IsContinueGetDailyReward => System.DateTime.Now.ExGetDeltaTimePerDays(this.GameInfo.LastDailyRewardTime).ExIsLess(KCDefine.B_VAL_2_DBL);

	public ERewardKinds DailyRewardKinds => KDefine.G_KINDS_REWARD_IT_DAILY_REWARDS[this.GameInfo.DailyRewardID];

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

	public bool IsEnableShowResumeAds => this.IsEnableShowFullscreenAds;
	public bool IsEnableUpdateAdsSkipTimes => true;
#endif			// #if ADS_MODULE_ENABLE
	#endregion			// 프로퍼티

	#region 함수
	//! 선택 된 부스터 상태를 리셋한다
	public void ResetSelBoostersState(bool a_bIsResetFreeBooster = true) {
		// 무료 부스터 리셋 모드 일 경우
		if(a_bIsResetFreeBooster) {
			this.FreeBooster = EItemKinds.NONE;
		}

		this.SelBoosterList.Clear();
	}

	//! 다음 일일 보상 식별자를 설정한다
	public void SetupNextDailyRewardID(bool a_bIsResetDailyRewardTime = true) {
		// 일일 보상 시간 리셋 모드 일 경우
		if(a_bIsResetDailyRewardTime) {
			this.GameInfo.LastDailyRewardTime = System.DateTime.Today;
		}

		int nNextDailyRewardID = (this.GameInfo.DailyRewardID + KCDefine.B_VAL_1_INT) % KDefine.G_KINDS_REWARD_IT_DAILY_REWARDS.Length;
		this.SetDailyRewardID(nNextDailyRewardID);
	}

	//! 무료 부스터 여부를 검사한다
	public bool IsFreeBooster(EItemKinds a_eBooster) {
		return this.FreeBooster == a_eBooster;
	}

	//! 부스터 포함 여부를 검사한다
	public bool IsContainsBooster(EItemKinds a_eBooster) {
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

	//! 일일 보상 종류를 변경한다
	public void SetDailyRewardID(int a_nID) {
		CAccess.Assert(KDefine.G_KINDS_REWARD_IT_DAILY_REWARDS.ExIsValidIdx(a_nID));
		this.GameInfo.DailyRewardID = a_nID;
	}

	//! 선택 된 부스터를 추가한다
	public void AddSelBooster(EItemKinds a_eBooster) {
		this.SelBoosterList.ExAddVal(a_eBooster);
	}

	//! 무료 보상 횟수를 추가한다
	public void AddFreeRewardTimes(int a_nRewardTimes) {
		int nFreeRewardTimes = this.GameInfo.FreeRewardTimes + a_nRewardTimes;
		this.GameInfo.FreeRewardTimes = Mathf.Clamp(nFreeRewardTimes, KCDefine.B_VAL_0_INT, CRewardInfoTable.Inst.FreeRewardInfoList.Count);
	}

	//! 완료 튜토리얼을 추가한다
	public void AddCompleteTutorial(ETutorialKinds a_eTutorialKinds) {
		this.GameInfo.m_oCompleteTutorialKindsList.ExAddVal(a_eTutorialKinds);
	}

	//! 게임 정보를 저장한다
	public void SaveGameInfo() {
		this.SaveGameInfo(KDefine.B_DATA_P_GAME_INFO);
	}

	//! 게임 정보를 저장한다
	public void SaveGameInfo(string a_oFilePath) {
		CFunc.WriteMsgPackObj(a_oFilePath, this.GameInfo);
	}

	//! 게임 정보를 로드한다
	public CGameInfo LoadGameInfo() {
		return this.LoadGameInfo(KDefine.B_DATA_P_GAME_INFO);
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
