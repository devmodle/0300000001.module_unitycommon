using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ADS_ENABLE && APP_LOVIN_ENABLE
//! 광고 관리자 - 앱 로빈
public partial class CAdsManager : CSingleton<CAdsManager> {
	#region 함수
	//! 앱 로빈이 초기화 되었을 경우
	public void OnInitAppLovin(MaxSdkBase.SdkConfiguration a_oConfiguration) {
		Func.ShowLog("CAdsManager.OnInitAppLovin: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_oConfiguration);

#if DEBUG || DEVELOPMENT_BUILD
		MaxSdk.ShowMediationDebugger();
#endif			// #if DEBUG || DEVELOPMENT_BUILD
	}

	//! 앱 로빈 배너 광고를 로드했을 경우
	public void OnLoadAppLovinBannerAds(string a_oAdsID) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_APP_LOVIN_BANNER_ADS_LOAD_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnLoadAppLovinBannerAds: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_oAdsID);

			m_stVariable.m_stAppLovinVariable.m_bIsLoadBannerAds = true;
			m_stVariable.m_stAppLovinVariable.m_nBannerAdsLoadTryTimes = 0;

			this.HandleLoadAppLovinBannerAdsResult();
		});
	}

	//! 앱 로빈 배너 광고 로드에 실패했을 경우
	public void OnLoadFailAppLovinBannerAds(string a_oAdsID, int a_nErrorCode) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_APP_LOVIN_BANNER_ADS_LOAD_FAIL_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnLoadFailAppLovinBannerAds: {0}, {1}", KDefine.B_LOG_COLOR_PLUGIN, a_oAdsID, a_nErrorCode);
			m_stVariable.m_stAppLovinVariable.m_nBannerAdsLoadTryTimes += 1;

			// 배너 광고 로드 재시도가 가능 할 경우
			if(m_stVariable.m_stAppLovinVariable.m_nBannerAdsLoadTryTimes < KDefine.U_MAX_TIMES_ADS_LOAD_TRY) {
				this.LoadBannerAds(EAdsType.APP_LOVIN);
			}
		});
	}

	//! 앱 로빈 보상 광고 로드에 실패했을 경우
	public void OnLoadFailAppLovinRewardAds(string a_oAdsID, int a_nErrorCode) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_APP_LOVIN_REWARD_ADS_LOAD_FAIL_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnLoadFailAppLovinRewardAds: {0}, {1}", KDefine.B_LOG_COLOR_PLUGIN, a_oAdsID, a_nErrorCode);
			m_stVariable.m_stAppLovinVariable.m_nRewardAdsLoadTryTimes += 1;

			// 보상 광고 로드 재시도가 가능 할 경우
			if(m_stVariable.m_stAppLovinVariable.m_nRewardAdsLoadTryTimes < KDefine.U_MAX_TIMES_ADS_LOAD_TRY) {
				this.LoadRewardAds(EAdsType.APP_LOVIN);
			}
		});
	}

	//! 앱 로빈 보상 광고가 닫혔을 경우
	public void OnCloseAppLovinRewardAds(string a_oAdsID) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_APP_LOVIN_REWARD_ADS_CLOSE_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnCloseAppLovinRewardAds: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_oAdsID);
			m_stVariable.m_stAppLovinVariable.m_nRewardAdsLoadTryTimes = 0;

			this.HandleCloseRewardAdsResult(EAdsType.APP_LOVIN);
			this.LoadRewardAds(EAdsType.APP_LOVIN);
		});
	}

	//! 앱 로빈 유저 보상을 수신했을 경우
	public void OnReceiveAppLovinUserReward(string a_oAdsID, MaxSdk.Reward a_oReward) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_APP_LOVIN_REWARD_ADS_RECEIVE_REWARD_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnReceiveAppLovinUserReward: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_oAdsID);

			this.HandleRewardAdsResult(EAdsType.APP_LOVIN, new STAdsRewardInfo() {
				m_oName = a_oAdsID, 
				m_oValue = string.Empty
			}, true);
		});
	}

	//! 앱 로빈 전면 광고 로드에 실패했을 경우
	public void OnLoadFailAppLovinFullscreenAds(string a_oAdsID, int a_nErrorCode) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_APP_LOVIN_FULLSCREEN_ADS_LOAD_FAIL_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnLoadFailAppLovinFullscreenAds: {0}, {1}", KDefine.B_LOG_COLOR_PLUGIN, a_oAdsID, a_nErrorCode);
			m_stVariable.m_stAppLovinVariable.m_nFullscreenAdsLoadTryTimes += 1;
			
			// 전면 광고 로드 재시도가 가능 할 경우
			if(m_stVariable.m_stAppLovinVariable.m_nFullscreenAdsLoadTryTimes < KDefine.U_MAX_TIMES_ADS_LOAD_TRY) {
				this.LoadFullscreenAds(EAdsType.APP_LOVIN);
			}
		});
	}

	//! 앱 로빈 전면 광고가 닫혔을 경우
	public void OnCloseAppLovinFullscreenAds(string a_oAdsID) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_APP_LOVIN_FULLSCREEN_ADS_CLOSE_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnCloseAppLovinFullscreenAds: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_oAdsID);
			m_stVariable.m_stAppLovinVariable.m_nFullscreenAdsLoadTryTimes = 0;

			this.HandleCloseFullscreenAdsResult(EAdsType.APP_LOVIN);
			this.LoadFullscreenAds(EAdsType.APP_LOVIN);
		});
	}

	//! 앱 로빈 배너 광고 로드 여부를 검사한다
	private bool IsLoadAppLovinBannerAds() {
		return this.IsContainsAppLovinBannerAdsID && m_stVariable.m_stAppLovinVariable.m_bIsLoadBannerAds;
	}

	//! 앱 로빈 보상 광고 로드 여부를 검사한다
	private bool IsLoadAppLovinRewardAds() {
		return this.IsContainsAppLovinRewardAdsID && 
			MaxSdk.IsRewardedAdReady(m_stParameters.m_stAppLovinParameters.m_oAdsIDList[KDefine.U_KEY_ADS_M_REWARD_ADS_ID]);
	}

	//! 앱 로빈 전면 광고 로드 여부를 검사한다
	private bool IsLoadAppLovinFullscreenAds() {
		return this.IsContainsAppLovinFullscreenAdsID && 
			MaxSdk.IsInterstitialReady(m_stParameters.m_stAppLovinParameters.m_oAdsIDList[KDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID]);
	}

	//! 앱 로빈 배너 광고를 로드한다
	private void LoadAppLovinBannerAds() {
		Func.Assert(this.IsContainsAppLovinBannerAdsID);

		MaxSdk.CreateBanner(m_stParameters.m_stAppLovinParameters.m_oAdsIDList[KDefine.U_KEY_ADS_M_BANNER_ADS_ID],
			MaxSdkBase.BannerPosition.BottomCenter);
		
		MaxSdk.SetBannerBackgroundColor(m_stParameters.m_stAppLovinParameters.m_oAdsIDList[KDefine.U_KEY_ADS_M_BANNER_ADS_ID],
			KDefine.U_COLOR_APP_LOVIN_BANNER_BG);
	}

	//! 앱 로빈 보상 광고를 로드한다
	private void LoadAppLovinRewardAds() {
		Func.Assert(this.IsContainsAppLovinRewardAdsID);
		MaxSdk.LoadRewardedAd(m_stParameters.m_stAppLovinParameters.m_oAdsIDList[KDefine.U_KEY_ADS_M_REWARD_ADS_ID]);
	}

	//! 앱 로빈 전면 광고를 로드한다
	private void LoadAppLovinFullscreenAds() {
		Func.Assert(this.IsContainsAppLovinFullscreenAdsID);
		MaxSdk.LoadInterstitial(m_stParameters.m_stAppLovinParameters.m_oAdsIDList[KDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID]);
	}

	//! 앱 로빈 배너 광고를 출력한다
	private void ShowAppLovinBannerAds() {
		Func.Assert(this.IsContainsAppLovinBannerAdsID && m_stParameters.m_eBannerAdsType == EAdsType.APP_LOVIN);
		MaxSdk.ShowBanner(m_stParameters.m_stAppLovinParameters.m_oAdsIDList[KDefine.U_KEY_ADS_M_BANNER_ADS_ID]);
	}

	//! 앱 로빈 보상 광고를 출력한다
	private void ShowAppLovinRewardAds() {
		Func.Assert(this.IsContainsAppLovinRewardAdsID);
		MaxSdk.ShowRewardedAd(m_stParameters.m_stAppLovinParameters.m_oAdsIDList[KDefine.U_KEY_ADS_M_REWARD_ADS_ID]);
	}

	//! 앱 로빈 전면 광고를 출력한다
	private void ShowAppLovinFullscreenAds() {
		Func.Assert(this.IsContainsAppLovinFullscreenAdsID);
		MaxSdk.ShowInterstitial(m_stParameters.m_stAppLovinParameters.m_oAdsIDList[KDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID]);
	}

	//! 앱 로빈 배너 광고를 닫는다
	private void CloseAppLovinBannerAds(bool a_bIsRemove) {
		Func.Assert(this.IsContainsAppLovinBannerAdsID);
		MaxSdk.HideBanner(m_stParameters.m_stAppLovinParameters.m_oAdsIDList[KDefine.U_KEY_ADS_M_BANNER_ADS_ID]);
	}

	//! 앱 로빈 배너 광고 로드 결과를 처리한다
	private void HandleLoadAppLovinBannerAdsResult() {
		Func.LateCallFunc(this, (a_oComponent, a_oParams) => {
			this.ShowBannerAds(EAdsType.APP_LOVIN, null);
		});
	}
	#endregion			// 함수
}
#endif			// #if ADS_ENABLE && APP_LOVIN_ENABLE
