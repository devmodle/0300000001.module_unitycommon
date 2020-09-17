using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ADS_ENABLE && ADMOB_ENABLE
using GoogleMobileAds.Api;

//! 광고 관리자 - 애드몹
public partial class CAdsManager : CSingleton<CAdsManager> {
	#region 함수
	//! 애드몹이 초기화 되었을 경우
	public void OnInitAdmob(InitializationStatus a_oStatus) {
		var oStatusList = a_oStatus.getAdapterStatusMap();
		string oString = oStatusList.ExToString(KDefine.B_TOKEN_CSV_STRING);

		Func.ShowLog("CAdsManager.OnInitAdmob: {0}", KDefine.B_LOG_COLOR_PLUGIN, oString);
	}

	//! 애드몹 배너 광고를 로드했을 경우
	public void OnLoadAdmobBannerAds(object a_oSender, System.EventArgs a_oEventArgs) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_ADMOB_BANNER_ADS_LOAD_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnLoadAdmobBannerAds", KDefine.B_LOG_COLOR_PLUGIN);

			m_stVariable.m_stAdmobVariable.m_bIsLoadBannerAds = true;
			m_stVariable.m_stAdmobVariable.m_nBannerAdsLoadTryTimes = 0;

			this.HandleLoadAdmobBannerAdsResult();
		});
	}

	//! 애드몹 배너 광고 로드에 실패했을 경우
	public void OnLoadFailAdmobBannerAds(object a_oSender, AdFailedToLoadEventArgs a_oEventArgs) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_ADMOB_BANNER_ADS_LOAD_FAIL_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnLoadFailAdmobBannerAds: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_oEventArgs.Message);
			m_stVariable.m_stAdmobVariable.m_nBannerAdsLoadTryTimes += 1;

			if(m_stVariable.m_stAdmobVariable.m_nBannerAdsLoadTryTimes < KDefine.U_MAX_TIMES_ADS_LOAD_TRY) {
				this.LoadBannerAds(EAdsType.APP_LOVIN);
			}
		});
	}

	//! 애드몹 배너 광고가 닫혔을 경우
	public void OnCloseAdmobBannerAds(object a_oSender, System.EventArgs a_oEventArgs) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_ADMOB_BANNER_ADS_CLOSE_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnCloseAdmobBannerAds: {0}", 
				KDefine.B_LOG_COLOR_PLUGIN, m_stVariable.m_stAdmobVariable.m_bIsShowBannerAds);

			if(!m_stVariable.m_stAdmobVariable.m_bIsShowBannerAds) {
				this.BannerAdsHeight = 0.0f;
			}
		});
	}

	//! 애드몹 보상 광고 로드에 실패했을 경우
	public void OnLoadFailAdmobRewardAds(object a_oSender, AdErrorEventArgs a_oEventArgs) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_ADMOB_REWARD_ADS_LOAD_FAIL_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnLoadFailAdmobRewardAds: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_oEventArgs.Message);
			m_stVariable.m_stAdmobVariable.m_nRewardAdsLoadTryTimes += 1;

			if(m_stVariable.m_stAdmobVariable.m_nRewardAdsLoadTryTimes < KDefine.U_MAX_TIMES_ADS_LOAD_TRY) {
				m_stVariable.m_stAdmobVariable.m_oRewardAds = null;
				this.LoadRewardAds(EAdsType.APP_LOVIN);
			}
		});
	}

	//! 애드몹 보상 광고가 닫혔을 경우
	public void OnCloseAdmobRewardAds(object a_oSender, System.EventArgs a_oEventArgs) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_ADMOB_REWARD_ADS_CLOSE_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnCloseAdmobRewardAds", KDefine.B_LOG_COLOR_PLUGIN);

			m_stVariable.m_stAdmobVariable.m_oRewardAds = null;
			m_stVariable.m_stAdmobVariable.m_nRewardAdsLoadTryTimes = 0;

			this.HandleCloseRewardAdsResult(EAdsType.APP_LOVIN);
			this.LoadRewardAds(EAdsType.APP_LOVIN);
		});
	}

	//! 애드몹 유저 보상을 수신했을 경우
	public void OnReceiveAdmobUserReward(object a_oSender, Reward a_oReward) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_ADMOB_REWARD_ADS_RECEIVE_REWARD_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnReceiveAdmobUserReward: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_oReward);

			this.HandleRewardAdsResult(EAdsType.APP_LOVIN, new STAdsRewardInfo() {
				m_oName = a_oReward.Type,
				m_oValue = a_oReward.Amount.ToString()
			}, true);
		});
	}

	//! 애드몹 전면 광고 로드에 실패했을 경우
	public void OnLoadFailAdmobFullscreenAds(object a_oSender, AdFailedToLoadEventArgs a_oEventArgs) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_ADMOB_FULLSCREEN_ADS_LOAD_FAIL_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnLoadFailAdmobFullscreenAds: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_oEventArgs.Message);
			m_stVariable.m_stAdmobVariable.m_nFullscreenAdsLoadTryTimes += 1;

			if(m_stVariable.m_stAdmobVariable.m_nFullscreenAdsLoadTryTimes < KDefine.U_MAX_TIMES_ADS_LOAD_TRY) {
				this.LoadFullscreenAds(EAdsType.APP_LOVIN);
			}
		});
	}
	
	//! 애드몹 전면 광고가 닫혔을 경우
	public void OnCloseAdmobFullscreenAds(object a_oSender, System.EventArgs a_oEventArgs) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_ADMOB_FULLSCREEN_ADS_CLOSE_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnCloseAdmobFullscreenAds", KDefine.B_LOG_COLOR_PLUGIN);
			m_stVariable.m_stAdmobVariable.m_oFullscreenAds?.Destroy();

			m_stVariable.m_stAdmobVariable.m_oFullscreenAds = null;
			m_stVariable.m_stAdmobVariable.m_nFullscreenAdsLoadTryTimes = 0;

			this.HandleCloseFullscreenAdsResult(EAdsType.APP_LOVIN);
			this.LoadFullscreenAds(EAdsType.APP_LOVIN);
		});
	}

	//! 애드몹 네이티브 광고를 로드했을 경우
	public void OnLoadAdmobNativeAds(object a_oSender, CustomNativeEventArgs a_oEventArgs) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_ADMOB_NATIVE_ADS_LOAD_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnLoadAdmobNativeAds", KDefine.B_LOG_COLOR_PLUGIN);

			var oNativeAds = a_oEventArgs.nativeAd;
			m_stVariable.m_stAdmobVariable.m_oNativeAdsList.ExAddValue(oNativeAds.GetCustomTemplateId(), oNativeAds);

			if(m_stVariable.m_stAdmobVariable.m_oNativeAdsList.Count == m_stParameters.m_stAdmobParameters.m_oTemplateIDList.Count) {
				m_stVariable.m_stAdmobVariable.m_bIsLoadNativeAds = true;
				m_stVariable.m_stAdmobVariable.m_nNativeAdsLoadTryTimes = 0;
			}
		});
	}

	//! 애드몹 네이티브 광고 로드에 실패했을 경우
	public void OnLoadFailAdmobNativeAds(object a_oSender, AdFailedToLoadEventArgs a_oEventArgs) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_ADMOB_NATIVE_ADS_LOAD_FAIL_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnLoadFailAdmobNativeAds: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_oEventArgs.Message);
			m_stVariable.m_stAdmobVariable.m_nNativeAdsLoadTryTimes += 1;

			if(m_stVariable.m_stAdmobVariable.m_nNativeAdsLoadTryTimes < KDefine.U_MAX_TIMES_ADS_LOAD_TRY) {
				this.LoadNativeAds(EAdsType.APP_LOVIN);
			}
		});
	}

	//! 애드몹 네이티브 광고를 반환한다
	public CustomNativeTemplateAd GetAdmobNativeAds(string a_oTemplateID) {
		Func.Assert(a_oTemplateID.ExIsValid());
		return m_stVariable.m_stAdmobVariable.m_oNativeAdsList.ExGetValue(a_oTemplateID, null);
	}

	//! 애드몹 배너 광고 로드 여부를 검사한다
	private bool IsLoadAdmobBannerAds() {
		Func.Assert(m_stParameters.m_stAdmobParameters.m_oAdsIDList.ContainsKey(KDefine.U_KEY_ADS_M_BANNER_ADS_ID));
		bool bIsEnableAds = m_stVariable.m_stAdmobVariable.m_oBannerAds != null;

		return bIsEnableAds && m_stVariable.m_stAdmobVariable.m_bIsLoadBannerAds;
	}

	//! 애드몹 보상 광고 로드 여부를 검사한다
	private bool IsLoadAdmobRewardAds() {
		Func.Assert(m_stParameters.m_stAdmobParameters.m_oAdsIDList.ContainsKey(KDefine.U_KEY_ADS_M_REWARD_ADS_ID));
		bool bIsEnableAds = m_stVariable.m_stAdmobVariable.m_oRewardAds != null;

		return bIsEnableAds && m_stVariable.m_stAdmobVariable.m_oRewardAds.IsLoaded();
	}

	//! 애드몹 전면 광고 로드 여부를 검사한다
	private bool IsLoadAdmobFullscreenAds() {
		Func.Assert(m_stParameters.m_stAdmobParameters.m_oAdsIDList.ContainsKey(KDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID));
		bool bIsEnableAds = m_stVariable.m_stAdmobVariable.m_oFullscreenAds != null;

		return bIsEnableAds && m_stVariable.m_stAdmobVariable.m_oFullscreenAds.IsLoaded();
	}

	//! 애드몹 네이티브 광고 로드 여부를 검사한다
	private bool IsLoadAdmobNativeAds() {
		Func.Assert(m_stParameters.m_stAdmobParameters.m_oAdsIDList.ContainsKey(KDefine.U_KEY_ADS_M_NATIVE_ADS_ID));
		bool bIsEnableAds = m_stVariable.m_stAdmobVariable.m_oAdsLoader != null;

		return bIsEnableAds && m_stVariable.m_stAdmobVariable.m_bIsLoadNativeAds;
	}

	//! 애드몹 배너 광고를 로드한다
	private void LoadAdmobBannerAds() {
		Func.Assert(this.AdmobBannerAds != null);
		this.AdmobBannerAds.LoadAd(this.AdmobRequestBuilder.Build());
	}

	//! 애드몹 보상 광고를 로드한다
	private void LoadAdmobRewardAds() {
		Func.Assert(this.AdmobRewardAds != null);
		this.AdmobRewardAds.LoadAd(this.AdmobRequestBuilder.Build());
	}

	//! 애드몹 전면 광고를 로드한다
	private void LoadAdmobFullscreenAds() {
		Func.Assert(this.AdmobFullscreenAds != null);
		this.AdmobFullscreenAds.LoadAd(this.AdmobRequestBuilder.Build());
	}

	//! 애드몹 네이티브 광고를 로드한다
	private void LoadAdmobNativeAds() {
		Func.Assert(this.AdmobAdsLoader != null);
		this.AdmobAdsLoader.LoadAd(this.AdmobRequestBuilder.Build());
	}

	//! 애드몹 배너 광고를 출력한다
	private void ShowAdmobBannerAds() {
		Func.Assert(this.AdmobBannerAds != null);

		if(m_stParameters.m_eBannerAdsType == EAdsType.APP_LOVIN) {
			this.AdmobBannerAds.Show();
			m_stVariable.m_stAdmobVariable.m_bIsShowBannerAds = true;

			float fScale = Func.GetResolutionScale(Application.isPlaying);
			float fPercent = KDefine.B_SCREEN_HEIGHT / Func.GetDeviceScreenSize().y;

			this.BannerAdsHeight = (this.AdmobBannerAds.GetHeightInPixels() * fPercent) / fScale;
		}
	}

	//! 애드몹 보상 광고를 출력한다
	private void ShowAdmobRewardAds() {
		Func.Assert(this.AdmobRewardAds != null);
		this.AdmobRewardAds.Show();
	}

	//! 애드몹 전면 광고를 출력한다
	private void ShowAdmobFullscreenAds() {
		Func.Assert(this.AdmobFullscreenAds != null);
		this.AdmobFullscreenAds.Show();
	}

	//! 애드몹 배너 광고를 닫는다
	private void CloseAdmobBannerAds(bool a_bIsRemove) {
		this.AdmobBannerAds?.Hide();
		m_stVariable.m_stAdmobVariable.m_bIsShowBannerAds = false;

		if(a_bIsRemove) {
			this.AdmobBannerAds?.Destroy();
			m_stVariable.m_stAdmobVariable.m_oBannerAds = null;
		}
	}

	//! 애드몹 배너 광고 로드 결과를 처리한다
	private void HandleLoadAdmobBannerAdsResult() {
		Func.LateCallFunc(this, (a_oComponent, a_oParams) => {
			this.ShowBannerAds(EAdsType.APP_LOVIN, null);
		});
	}
	#endregion			// 함수
}
#endif			// #if ADS_ENABLE && ADMOB_ENABLE
