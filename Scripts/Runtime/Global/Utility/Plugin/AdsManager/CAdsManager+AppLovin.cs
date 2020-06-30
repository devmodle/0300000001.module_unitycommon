using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ADS_ENABLE && APP_LOVIN_ENABLE
//! 광고 관리자 - 앱 로빈
public partial class CAdsManager : CSingleton<CAdsManager> {
	#region 함수
	//! 앱 로빈 보상 광고 로드에 실패했을 경우
	public void OnLoadFailAppLovinRewardAds(string a_oAdsID, int a_nErrorCode) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_APP_LOVIN_REWARD_ADS_LOAD_FAIL_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnLoadFailAppLovinRewardAds: {0}, {1}", a_oAdsID, a_nErrorCode);
			m_stVariable.m_stAppLovinVariable.m_nRewardAdsLoadTryTimes += 1;

			if(m_stVariable.m_stAppLovinVariable.m_nRewardAdsLoadTryTimes < KDefine.U_MAX_TIMES_ADS_LOAD_TRY) {
				this.LoadRewardAds(EAdsType.APP_LOVIN);
			}
		});
	}

	//! 앱 로빈 보상 광고가 출력 될 경우
	public void OnShowAppLovinRewardAds(string a_oAdsID) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_APP_LOVIN_REWARD_ADS_SHOW_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnShowAppLovinRewardAds: {0}", a_oAdsID);
			m_stVariable.m_stAppLovinVariable.m_nRewardAdsLoadTryTimes = 0;

			this.LoadRewardAds(EAdsType.APP_LOVIN);
		});
	}

	//! 앱 로빈 보상 광고가 닫혔을 경우
	public void OnCloseAppLovinRewardAds(string a_oAdsID) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_APP_LOVIN_REWARD_ADS_CLOSE_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnCloseAppLovinRewardAds: {0}", a_oAdsID);
			this.HandleCloseRewardAdsResult(EAdsType.APP_LOVIN);
		});
	}

	//! 앱 로빈 유저 보상을 수신했을 경우
	public void OnReceiveAppLovinUserReward(string a_oAdsID, MaxSdk.Reward a_oReward) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_APP_LOVIN_REWARD_ADS_RECEIVE_REWARD_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnReceiveAppLovinUserReward: {0}", a_oAdsID);

			this.HandleRewardAdsResult(EAdsType.ADMOB, new STAdsRewardInfo() {
				m_oName = a_oAdsID,
				m_oValue = string.Empty
			}, true);
		});
	}

	//! 앱 로빈 전면 광고 로드에 실패했을 경우
	public void OnLoadFailAppLovinFullscreenAds(string a_oAdsID, int a_nErrorCode) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_APP_LOVIN_FULLSCREEN_ADS_LOAD_FAIL_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnLoadFailAppLovinFullscreenAds: {0}, {1}", a_oAdsID, a_nErrorCode);
			m_stVariable.m_stAppLovinVariable.m_nFullscreenAdsLoadTryTimes += 1;

			if(m_stVariable.m_stAppLovinVariable.m_nFullscreenAdsLoadTryTimes < KDefine.U_MAX_TIMES_ADS_LOAD_TRY) {
				this.LoadFullscreenAds(EAdsType.APP_LOVIN);
			}			
		});
	}

	//! 앱 로빈 전면 광고가 출력 될 경우
	public void OnShowAppLovinFullscreenAds(string a_oAdsID) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_APP_LOVIN_FULLSCREEN_ADS_SHOW_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnShowAppLovinFullscreenAds: {0}", a_oAdsID);
			m_stVariable.m_stAppLovinVariable.m_nFullscreenAdsLoadTryTimes = 0;

			
		});
	}

	//! 앱 로빈 전면 광고가 닫혔을 경우
	public void OnCloseAppLovinFullscreenAds(string a_oAdsID) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_APP_LOVIN_FULLSCREEN_ADS_SHOW_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnCloseAppLovinFullscreenAds: {0}", Color.yellow, a_oAdsID);


		});
	}
	#endregion			// 함수
}
#endif			// #if ADS_ENABLE && APP_LOVIN_ENABLE
