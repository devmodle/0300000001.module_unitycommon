using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ADS_ENABLE && IRON_SOURCE_ENABLE
//! 광고 관리자 - 아이언 소스
public partial class CAdsManager : CSingleton<CAdsManager> {
	#region 함수
	//! 아이언 소스 배너 광고를 로드했을 경우
	public void OnLoadIronSourceBannerAds() {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_IRON_SOURCE_BANNER_ADS_LOAD_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnLoadIronSourceBannerAds", Color.yellow);

			m_stVariable.m_stIronSourceVariable.m_bIsLoadBannerAds = true;
			m_stVariable.m_stIronSourceVariable.m_nBannerAdsLoadTryTimes = 0;

			this.HandleLoadIronSourceBannerAdsResult();
		});
	}

	//! 아이언 소스 배너 광고 로드에 실패했을 경우
	public void OnLoadFailIronSourceBannerAds(IronSourceError a_oError) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_IRON_SOURCE_BANNER_ADS_LOAD_FAIL_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnLoadFailIronSourceBannerAds: {0}", Color.yellow, a_oError);
			m_stVariable.m_stIronSourceVariable.m_nBannerAdsLoadTryTimes += 1;

			if(m_stVariable.m_stIronSourceVariable.m_nBannerAdsLoadTryTimes < KDefine.U_MAX_TIMES_ADS_LOAD_TRY) {
				this.LoadBannerAds(EAdsType.IRON_SOURCE);
			}
		});
	}

	//! 아이언 소스 보상 광고가 닫혔을 경우
	public void OnCloseIronSourceRewardAds() {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_IRON_SOURCE_REWARD_ADS_CLOSE_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnCloseIronSourceRewardAds", Color.yellow);
			m_stVariable.m_stIronSourceVariable.m_nRewardAdsLoadTryTimes = 0;

			this.HandleCloseRewardAdsResult(EAdsType.IRON_SOURCE);
			this.LoadRewardAds(EAdsType.IRON_SOURCE);
		});
	}

	//! 아이언 소스 유저 보상을 수신했을 경우
	public void OnReceiveIronSourceUserReward(IronSourcePlacement a_oPlacement) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_IRON_SOURCE_REWARD_ADS_GET_REWARD_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnReceiveIronSourceUserReward: {0}", Color.yellow, a_oPlacement);

			this.HandleRewardAdsResult(EAdsType.IRON_SOURCE, new STAdsRewardInfo() {
				m_oName = a_oPlacement.getRewardName(),
				m_oValue = a_oPlacement.getRewardAmount().ToString()
			}, true);
		});
	}

	//! 아이언 소스 보상 광고 상태가 변경 되었을 경우
	public void OnChangeIronSourceRewardAdsState(bool a_bIsActive) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_IRON_SOURCE_REWARD_ADS_CHANGE_STATE_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnChangeIronSourceRewardAdsState: {0}", Color.yellow, a_bIsActive);

			if(!a_bIsActive) {
				m_stVariable.m_stIronSourceVariable.m_nRewardAdsLoadTryTimes += 1;

				if(m_stVariable.m_stIronSourceVariable.m_nRewardAdsLoadTryTimes < KDefine.U_MAX_TIMES_ADS_LOAD_TRY) {
					this.LoadRewardAds(EAdsType.IRON_SOURCE);
				}
			}
		});
	}

	//! 아이언 소스 전면 광고 로드에 실패했을 경우
	public void OnLoadFailIronSourceFullscreenAds(IronSourceError a_oError) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_IRON_SOURCE_FULLSCREEN_ADS_LOAD_FAIL_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnLoadFailIronSourceFullscreenAds: {0}", Color.yellow, a_oError);
			m_stVariable.m_stIronSourceVariable.m_nFullscreenAdsLoadTryTimes += 1;

			if(m_stVariable.m_stIronSourceVariable.m_nFullscreenAdsLoadTryTimes < KDefine.U_MAX_TIMES_ADS_LOAD_TRY) {
				this.LoadFullscreenAds(EAdsType.IRON_SOURCE);
			}
		});
	}

	//! 아이언 소스 전면 광고가 닫혔을 경우
	public void OnCloseIronSourceFullscreenAds() {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_IRON_SOURCE_FULLSCREEN_ADS_CLOSE_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnCloseIronSourceFullscreenAds", Color.yellow);
			m_stVariable.m_stIronSourceVariable.m_nFullscreenAdsLoadTryTimes = 0;

			this.HandleCloseFullscreenAdsResult(EAdsType.IRON_SOURCE);
			this.LoadFullscreenAds(EAdsType.IRON_SOURCE);
		});
	}

	//! 아이언 소스 배너 광고 로드 여부를 검사한다
	private bool IsLoadIronSourceBannerAds() {
		Func.Assert(m_stParameters.m_stIronSourceParameters.m_oAdsPlacementList.ContainsKey(KDefine.U_KEY_ADS_M_BANNER_ADS_PLACEMENT));
		return m_stVariable.m_stIronSourceVariable.m_bIsLoadBannerAds;
	}

	//! 아이언 소스 보상 광고 로드 여부를 검사한다
	private bool IsLoadIronSourceRewardAds() {
		Func.Assert(m_stParameters.m_stIronSourceParameters.m_oAdsPlacementList.ContainsKey(KDefine.U_KEY_ADS_M_REWARD_ADS_PLACEMENT));
		return IronSource.Agent.isRewardedVideoAvailable();
	}

	//! 아이언 소스 전면 광고 로드 여부를 검사한다
	private bool IsLoadIronSourceFullscreenAds() {
		Func.Assert(m_stParameters.m_stIronSourceParameters.m_oAdsPlacementList.ContainsKey(KDefine.U_KEY_ADS_M_FULLSCREEN_ADS_PLACEMENT));
		return IronSource.Agent.isInterstitialReady();
	}

	//! 아이언 소스 배너 광고를 로드한다
	private void LoadIronSourceBannerAds() {
		string oPlacement = m_stParameters.m_stIronSourceParameters.m_oAdsPlacementList[KDefine.U_KEY_ADS_M_BANNER_ADS_PLACEMENT];
		IronSource.Agent.loadBanner(KDefine.U_SIZE_IRON_SOURCE_BANNER, IronSourceBannerPosition.BOTTOM, oPlacement);
	}

	//! 아이언 소스 보상 광고를 로드한다
	private void LoadIronSourceRewardAds() {
		// Do Nothing
	}

	//! 아이언 소스 전면 광고를 로드한다
	private void LoadIronSourceFullscreenAds() {
		IronSource.Agent.loadInterstitial();
	}

	//! 아이언 소스 배너 광고를 출력한다
	private void ShowIronSourceBannerAds() {
		if(m_stParameters.m_eBannerAdsType == EAdsType.IRON_SOURCE) {
			IronSource.Agent.displayBanner();
		}
	}

	//! 아이언 소스 보상 광고를 출력한다
	private void ShowIronSourceRewardAds() {
		IronSource.Agent.showRewardedVideo();
	}

	//! 아이언 소스 전면 광고를 출력한다
	private void ShowIronSourceFullscreenAds() {
		IronSource.Agent.showInterstitial();
	}

	//! 아이언 소스 배너 광고를 닫는다
	private void CloseIronSourceBannerAds(bool a_bIsRemove) {
		IronSource.Agent.hideBanner();

		if(a_bIsRemove) {
			IronSource.Agent.destroyBanner();
		}
	}

	//! 아이언 소스 배너 광고 로드 결과를 처리한다
	private void HandleLoadIronSourceBannerAdsResult() {
		Func.LateCallFunc(this, (a_oComponent, a_oParams) => {
			this.ShowBannerAds(EAdsType.IRON_SOURCE, null);
		});
	}
	#endregion			// 함수
}
#endif			// #if ADS_ENABLE && IRON_SOURCE_ENABLE
