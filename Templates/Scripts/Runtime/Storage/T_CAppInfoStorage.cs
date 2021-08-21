using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using MessagePack;

#if NEVER_USE_THIS
//! 앱 정보
[MessagePackObject]
[System.Serializable]
public class CAppInfo : CBaseInfo {
	#region 상수
#if ADS_MODULE_ENABLE
	private const string KEY_REWARD_ADS_WATCH_TIMES = "RewardAdsWatchTimes";
	private const string KEY_FULLSCREEN_ADS_WATCH_TIMES = "FullscreenAdsWatchTimes";
#endif			// #if ADS_MODULE_ENABLE
	#endregion			// 상수

	#region 프로퍼티
#if ADS_MODULE_ENABLE
	[IgnoreMember] public int RewardAdsWatchTimes {
		get { return m_oIntDict.ExGetVal(CAppInfo.KEY_REWARD_ADS_WATCH_TIMES, KCDefine.B_VAL_0_INT); }
		set { m_oIntDict.ExReplaceVal(CAppInfo.KEY_REWARD_ADS_WATCH_TIMES, value); }
	}

	[IgnoreMember] public int FullscreenAdsWatchTimes {
		get { return m_oIntDict.ExGetVal(CAppInfo.KEY_FULLSCREEN_ADS_WATCH_TIMES, KCDefine.B_VAL_0_INT); }
		set { m_oIntDict.ExReplaceVal(CAppInfo.KEY_FULLSCREEN_ADS_WATCH_TIMES, value); }
	}
#endif			// #if ADS_MODULE_ENABLE
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

//! 앱 정보 저장소
public class CAppInfoStorage : CSingleton<CAppInfoStorage> {
	#region 프로퍼티
	public bool IsIgnoreUpdate { get; set; } = false;
	public bool IsCloseAgreePopup { get; set; } = false;
	
	public CAppInfo AppInfo { get; private set; } = new CAppInfo();

#if ADS_MODULE_ENABLE
	public int AdsSkipTimes { get; set; } = KCDefine.B_VAL_0_INT;

	public System.DateTime PrevAdsTime { get; set; } = System.DateTime.Now;
	public System.DateTime PrevRewardAdsTime { get; set; } = System.DateTime.Now;

	public bool IsEnableShowFullscreenAds {
		get {
			float fAdsDelay = CValTable.Inst.GetFlt(KCDefine.VT_KEY_DELAY_ADS);
			float fAdsDeltaTime = CValTable.Inst.GetFlt(KCDefine.VT_KEY_DELTA_T_ADS);

			double dblDeltaTimeA = System.DateTime.Now.ExGetDeltaTime(CAppInfoStorage.Inst.PrevAdsTime);
			double dblDeltaTimeB = System.DateTime.Now.ExGetDeltaTime(CAppInfoStorage.Inst.PrevRewardAdsTime);

			bool bIsEnable = this.AdsSkipTimes >= KDefine.G_MAX_TIMES_ADS_SKIP && (dblDeltaTimeA.ExIsGreateEquals(fAdsDelay) && dblDeltaTimeB.ExIsGreateEquals(fAdsDeltaTime));
			return bIsEnable && CGameInfoStorage.Inst.GameInfo.m_oClearInfoDict.Count >= KDefine.G_MAX_NUM_ADS_SKIP_CLEAR_INFOS;
		}
	}
	
	public bool IsEnableUpdateAdsSkipTimes => true;
#endif			// #if ADS_MODULE_ENABLE
	#endregion			// 프로퍼티

	#region 함수
	//! 앱 정보를 로드한다
	public CAppInfo LoadAppInfo() {
		return this.LoadAppInfo(KDefine.G_DATA_P_APP_INFO);
	}

	//! 앱 정보를 로드한다
	public CAppInfo LoadAppInfo(string a_oFilePath) {
		// 파일이 존재 할 경우
		if(File.Exists(a_oFilePath)) {
			this.AppInfo = CFunc.ReadMsgPackObj<CAppInfo>(a_oFilePath);
			CAccess.Assert(this.AppInfo != null);
		}

		return this.AppInfo;
	}

	//! 앱 정보를 저장한다
	public void SaveAppInfo() {
		this.SaveAppInfo(KDefine.G_DATA_P_APP_INFO);
	}

	//! 앱 정보를 저장한다
	public void SaveAppInfo(string a_oFilePath) {
		CFunc.WriteMsgPackObj(a_oFilePath, this.AppInfo);
	}
	#endregion			// 함수

	#region 조건부 함수
#if ADS_MODULE_ENABLE
	//! 광고 누적 횟수를 추가한다
	public void AddAdsSkipTimes(int a_nTimes) {
		int nSkipTimes = this.AdsSkipTimes + a_nTimes;
		this.AdsSkipTimes = Mathf.Max(nSkipTimes, KCDefine.B_VAL_0_INT);
	}
	
	//! 보상 광고 시청 횟수를 추가한다
	public void AddRewardAdsWatchTimes(int a_nTimes) {
		int nWatchTimes = this.AppInfo.RewardAdsWatchTimes + a_nTimes;
		this.AppInfo.RewardAdsWatchTimes = Mathf.Max(nWatchTimes, KCDefine.B_VAL_0_INT);
	}

	//! 전면 광고 시청 횟수를 추가한다
	public void AddFullscreenAdsWatchTimes(int a_nTimes) {
		int nWatchTimes = this.AppInfo.FullscreenAdsWatchTimes + a_nTimes;
		this.AppInfo.FullscreenAdsWatchTimes = Mathf.Max(nWatchTimes, KCDefine.B_VAL_0_INT);
	}
#endif			// #if ADS_MODULE_ENABLE
	#endregion			// 조건부 함수

	#region 추가 변수

	#endregion			// 추가 변수

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if NEVER_USE_THIS
