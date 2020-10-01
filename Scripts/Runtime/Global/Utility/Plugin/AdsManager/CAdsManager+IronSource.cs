using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if IRON_SOURCE_ENABLE
//! 광고 관리자 - 아이언 소스
public partial class CAdsManager : CSingleton<CAdsManager> {
	#region 함수
	//! 아이언 소스 초기화 여부를 검사한다
	private bool IsInitIronSource() {
		return m_stVariable.m_stIronSourceVariable.m_bIsInit;
	}

	//! 아이언 소스 광고 식별자 유효 여부를 검사한다
	private bool IsValidIronSourceAdsID(string a_oKey) {
		bool bIsContains = m_stParameters.m_stIronSourceParameters.m_oAdsIDList.ContainsKey(a_oKey);
		return bIsContains && m_stParameters.m_stIronSourceParameters.m_oAdsIDList[a_oKey].ExIsValid();
	}

	//! 아이언 소스 배너 광고 식별자 유효 여부를 검사한다
	private bool IsValidIronSourceBannerAdsID() {
		return this.IsValidIronSourceAdsID(KDefine.U_KEY_ADS_M_BANNER_ADS_ID);
	}

	//! 아이언 소스 보상 광고 식별자 유효 여부를 검사한다
	private bool IsValidIronSourceRewardAdsID() {
		return this.IsValidIronSourceAdsID(KDefine.U_KEY_ADS_M_REWARD_ADS_ID);
	}

	//! 아이언 소스 전면 광고 식별자 유효 여부를 검사한다
	private bool IsValidIronSourceFullscreenAdsID() {
		return this.IsValidIronSourceAdsID(KDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID);
	}

	//! 아이언 소스 배너 광고 로드 여부를 검사한다
	private bool IsLoadIronSourceBannerAds() {
#if UNITY_IOS || UNITY_ANDROID
		return this.IsValidIronSourceBannerAdsID() && m_stVariable.m_stIronSourceVariable.m_bIsLoadBannerAds;
#else
		return false;
#endif			// #if UNITY_IOS || UNITY_ANDROID
	}

	//! 아이언 소스 보상 광고 로드 여부를 검사한다
	private bool IsLoadIronSourceRewardAds() {
#if UNITY_IOS || UNITY_ANDROID
		return this.IsValidIronSourceRewardAdsID() && IronSource.Agent.isRewardedVideoAvailable();
#else
		return false;
#endif			// #if UNITY_IOS || UNITY_ANDROID
	}

	//! 아이언 소스 전면 광고 로드 여부를 검사한다
	private bool IsLoadIronSourceFullscreenAds() {
#if UNITY_IOS || UNITY_ANDROID
		return this.IsValidIronSourceFullscreenAdsID() && IronSource.Agent.isInterstitialReady();
#else
		return false;
#endif			// #if UNITY_IOS || UNITY_ANDROID
	}

	//! 아이언 소스 배너 광고를 로드한다
	private void LoadIronSourceBannerAds() {
		Func.Assert(this.IsValidIronSourceBannerAdsID());

#if UNITY_IOS || UNITY_ANDROID
		IronSource.Agent.loadBanner(KDefine.U_SIZE_IRON_SOURCE_BANNER, IronSourceBannerPosition.BOTTOM);
#endif			// #if UNITY_IOS || UNITY_ANDROID
	}

	//! 아이언 소스 보상 광고를 로드한다
	private void LoadIronSourceRewardAds() {
		Func.Assert(this.IsValidIronSourceRewardAdsID());
	}

	//! 아이언 소스 전면 광고를 로드한다
	private void LoadIronSourceFullscreenAds() {
		Func.Assert(this.IsValidIronSourceFullscreenAdsID());

#if UNITY_IOS || UNITY_ANDROID
		IronSource.Agent.loadInterstitial();
#endif			// #if UNITY_IOS || UNITY_ANDROID
	}

	//! 아이언 소스 배너 광고를 출력한다
	private void ShowIronSourceBannerAds() {
		Func.Assert(this.IsValidIronSourceBannerAdsID() && m_stParameters.m_eBannerAdsType == EAdsType.IRON_SOURCE);

#if UNITY_IOS || UNITY_ANDROID
		IronSource.Agent.displayBanner();
#endif			// #if UNITY_IOS || UNITY_ANDROID
	}

	//! 아이언 소스 보상 광고를 출력한다
	private void ShowIronSourceRewardAds() {
		Func.Assert(this.IsValidIronSourceRewardAdsID());

#if UNITY_IOS || UNITY_ANDROID
		IronSource.Agent.showRewardedVideo();
#endif			// #if UNITY_IOS || UNITY_ANDROID
	}

	//! 아이언 소스 전면 광고를 출력한다
	private void ShowIronSourceFullscreenAds() {
		Func.Assert(this.IsValidIronSourceFullscreenAdsID());

#if UNITY_IOS || UNITY_ANDROID
		IronSource.Agent.showInterstitial();
#endif			// #if UNITY_IOS || UNITY_ANDROID
	}

	//! 아이언 소스 배너 광고를 닫는다
	private void CloseIronSourceBannerAds(bool a_bIsRemove) {
		Func.Assert(this.IsValidIronSourceBannerAdsID());

#if UNITY_IOS || UNITY_ANDROID
		IronSource.Agent.hideBanner();

		this.BannerAdsHeight = 0.0f;
		m_stVariable.m_stIronSourceVariable.m_bIsLoadBannerAds = false;
		
		// 제거 모드 일 경우
		if(a_bIsRemove) {
			IronSource.Agent.destroyBanner();
		}
#endif			// #if UNITY_IOS || UNITY_ANDROID
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_IOS || UNITY_ANDROID
	//! 아이언 소스이 초기화 되었을 경우
	public void OnInitIronSource() {
		Func.ShowLog("CAdsManager.OnInitIronSource", KDefine.B_LOG_COLOR_PLUGIN);
		m_stVariable.m_stIronSourceVariable.m_bIsInit = true;
	}

	//! 아이언 소스 배너 광고를 로드했을 경우
	public void OnLoadIronSourceBannerAds() {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_IRON_SOURCE_BANNER_ADS_LOAD_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnLoadIronSourceBannerAds", KDefine.B_LOG_COLOR_PLUGIN);
			m_stVariable.m_stIronSourceVariable.m_bIsLoadBannerAds = true;

			this.HandleLoadIronSourceBannerAdsResult();
		});
	}

	//! 아이언 소스 배너 광고 로드에 실패했을 경우
	public void OnLoadFailIronSourceBannerAds(IronSourceError a_oError) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_IRON_SOURCE_BANNER_ADS_LOAD_FAIL_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnLoadFailIronSourceBannerAds: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_oError);
			this.AddLoadFailBannerAdsInfo(EAdsType.IRON_SOURCE, this.LoadBannerAds);
		});
	}

	//! 아이언 소스 보상 광고가 닫혔을 경우
	public void OnCloseIronSourceRewardAds() {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_IRON_SOURCE_REWARD_ADS_CLOSE_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnCloseIronSourceRewardAds", KDefine.B_LOG_COLOR_PLUGIN);

			this.HandleCloseRewardAdsResult(EAdsType.IRON_SOURCE);
			this.LoadRewardAds(EAdsType.IRON_SOURCE);
		});
	}

	//! 아이언 소스 유저 보상을 수신했을 경우
	public void OnReceiveIronSourceUserReward(IronSourcePlacement a_oPlacement) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_IRON_SOURCE_REWARD_ADS_RECEIVE_REWARD_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnReceiveIronSourceUserReward: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_oPlacement);

			this.HandleRewardAdsResult(EAdsType.IRON_SOURCE, new STAdsRewardInfo() {
				m_oName = a_oPlacement.getRewardName(),
				m_oValue = a_oPlacement.getRewardAmount().ToString()
			}, true);
		});
	}

	//! 아이언 소스 보상 광고 상태가 변경 되었을 경우
	public void OnChangeIronSourceRewardAdsState(bool a_bIsActive) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_IRON_SOURCE_REWARD_ADS_CHANGE_STATE_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnChangeIronSourceRewardAdsState: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_bIsActive);

			// 비활성 상태 일 경우
			if(!a_bIsActive) {
				this.AddLoadFailRewardAdsInfo(EAdsType.IRON_SOURCE, this.LoadRewardAds);
			}
		});
	}

	//! 아이언 소스 전면 광고 로드에 실패했을 경우
	public void OnLoadFailIronSourceFullscreenAds(IronSourceError a_oError) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_IRON_SOURCE_FULLSCREEN_ADS_LOAD_FAIL_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnLoadFailIronSourceFullscreenAds: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_oError);
			this.AddLoadFailFullscreenAdsInfo(EAdsType.IRON_SOURCE, this.LoadFullscreenAds);
		});
	}

	//! 아이언 소스 전면 광고가 닫혔을 경우
	public void OnCloseIronSourceFullscreenAds() {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_IRON_SOURCE_FULLSCREEN_ADS_CLOSE_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnCloseIronSourceFullscreenAds", KDefine.B_LOG_COLOR_PLUGIN);

			this.HandleCloseFullscreenAdsResult(EAdsType.IRON_SOURCE);
			this.LoadFullscreenAds(EAdsType.IRON_SOURCE);
		});
	}

	//! 아이언 소스 배너 광고 로드 결과를 처리한다
	private void HandleLoadIronSourceBannerAdsResult() {
		Func.LateCallFunc(this, (a_oComponent, a_oParams) => {
			this.ShowBannerAds(EAdsType.IRON_SOURCE, null);
		});
	}
#endif			// #if UNITY_IOS || UNITY_ANDROID
	#endregion			// 조건부 함수
}
#endif			// #if ADS_MODULE_ENABLE && IRON_SOURCE_ENABLE
