using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ADS_ENABLE && UNITY_ADS_ENABLE
using UnityEngine.Advertisements;

//! 광고 관리자 - 유니티 애즈
public partial class CAdsManager : CSingleton<CAdsManager> {
	#region 함수
	//! 유니티 애즈 배너 광고를 로드했을 경우
	public void OnLoadUnityAdsBannerAds() {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_UNITY_ADS_BANNER_ADS_LOAD_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnLoadUnityAdsBannerAds", Color.yellow);
			m_stVariable.m_stUnityAdsVariable.m_nBannerAdsLoadTryTimes = 0;

			this.HandleLoadUnityAdsBannerAdsResult();
		});
	}

	//! 유니티 애즈 배너 광고 로드에 실패했을 경우
	public void OnLoadFailUnityAdsBannerAds(string a_oMsg) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_UNITY_ADS_BANNER_ADS_LOAD_FAIL_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnLoadFailUnityAdsBannerAds: {0}", Color.yellow, a_oMsg);
			m_stVariable.m_stUnityAdsVariable.m_nBannerAdsLoadTryTimes += 1;

			if(m_stVariable.m_stUnityAdsVariable.m_nBannerAdsLoadTryTimes < KDefine.U_MAX_TIMES_ADS_LOAD_TRY) {
				this.LoadBannerAds(EAdsType.UNITY_ADS);
			}
		});
	}

	//! 유니티 애즈 광고를 로드했을 경우
	public void OnLoadUnityAds(string a_oPlacement) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_UNITY_ADS_LOAD_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnLoadUnityAds: {0}", Color.yellow, a_oPlacement);
			m_stVariable.m_stUnityAdsVariable.m_oLoadingAdsPlacementList.ExRemoveValue(a_oPlacement);

			var stResult = Func.FindValue<string, string>(m_stParameters.m_stUnityAdsParameters.m_oAdsPlacementList, (a_oUnityAdsPlacement) => {
				return a_oPlacement.ExIsEquals(a_oUnityAdsPlacement);
			});

			if(stResult.Key && stResult.Value.ExIsEquals(KDefine.U_KEY_ADS_M_BANNER_ADS_PLACEMENT)) {
				this.ShowBannerAds(EAdsType.UNITY_ADS, null);
			}
		});
	}

	//! 유니티 애즈 광고 로드에 실패했을 경우
	public void OnLoadFailUnityAds(string a_oMsg) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_UNITY_ADS_LOAD_FAIL_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnLoadFailUnityAds: {0}", Color.yellow, a_oMsg);

			for(int i = 0; i < m_stVariable.m_stUnityAdsVariable.m_oLoadingAdsPlacementList.Count; ++i) {
				string oPlacement = m_stVariable.m_stUnityAdsVariable.m_oLoadingAdsPlacementList[i];

				var stResult = Func.FindValue<string, string>(m_stParameters.m_stUnityAdsParameters.m_oAdsPlacementList, (a_oPlacement) => {
					return oPlacement.ExIsEquals(a_oPlacement);
				});

				if(stResult.Key) {
					m_stVariable.m_stUnityAdsVariable.m_oLoadingAdsPlacementList.ExRemoveValue(m_stParameters.m_stUnityAdsParameters.m_oAdsPlacementList[stResult.Value]);

					if(stResult.Value.ExIsEquals(KDefine.U_KEY_ADS_M_REWARD_ADS_PLACEMENT)) {
						m_stVariable.m_stUnityAdsVariable.m_nRewardAdsLoadTryTimes += 1;

						if(m_stVariable.m_stUnityAdsVariable.m_nRewardAdsLoadTryTimes < KDefine.U_MAX_TIMES_ADS_LOAD_TRY) {
							this.LoadRewardAds(EAdsType.UNITY_ADS);
						}
					} else if(stResult.Value.ExIsEquals(KDefine.U_KEY_ADS_M_FULLSCREEN_ADS_PLACEMENT)) {
						m_stVariable.m_stUnityAdsVariable.m_nFullscreenAdsLoadTryTimes += 1;

						if(m_stVariable.m_stUnityAdsVariable.m_nFullscreenAdsLoadTryTimes < KDefine.U_MAX_TIMES_ADS_LOAD_TRY) {
							this.LoadFullscreenAds(EAdsType.UNITY_ADS);
						}
					}
				}
			}
		});
	}

	//! 유니티 애즈 광고가 닫혔을 경우
	public void OnCloseUnityAds(string a_oPlacement, ShowResult a_eShowResult) {
		CScheduleManager.Instance.AddCallback(KDefine.U_KEY_ADS_M_UNITY_ADS_CLOSE_CALLBACK, () => {
			Func.ShowLog("CAdsManager.OnCloseUnityAds: {0}, {1}", Color.yellow, a_oPlacement, a_eShowResult);

			var stResult = Func.FindValue<string, string>(m_stParameters.m_stUnityAdsParameters.m_oAdsPlacementList, (a_oUnityAdsPlacement) => {
				return a_oPlacement.ExIsEquals(a_oUnityAdsPlacement);
			});

			if(stResult.Key) {
				if(stResult.Value.ExIsEquals(KDefine.U_KEY_ADS_M_REWARD_ADS_PLACEMENT)) {
					this.HandleRewardAdsResult(EAdsType.UNITY_ADS, new STAdsRewardInfo() {
						m_oName = a_oPlacement,
						m_oValue = string.Empty
					}, a_eShowResult == ShowResult.Finished);

					m_stVariable.m_stUnityAdsVariable.m_nRewardAdsLoadTryTimes = 0;

					this.HandleCloseRewardAdsResult(EAdsType.UNITY_ADS);
					this.LoadRewardAds(EAdsType.UNITY_ADS);
				} else if(stResult.Value.ExIsEquals(KDefine.U_KEY_ADS_M_FULLSCREEN_ADS_PLACEMENT)) {
					m_stVariable.m_stUnityAdsVariable.m_nFullscreenAdsLoadTryTimes = 0;

					this.HandleCloseFullscreenAdsResult(EAdsType.UNITY_ADS);
					this.LoadFullscreenAds(EAdsType.UNITY_ADS);
				}
			}
		});
	}

	//! 유니티 애즈 배너 광고 로드 여부를 검사한다
	private bool IsLoadUnityAdsBannerAds() {
		Func.Assert(m_stParameters.m_stUnityAdsParameters.m_oAdsPlacementList.ContainsKey(KDefine.U_KEY_ADS_M_BANNER_ADS_PLACEMENT));
		return Advertisement.IsReady(m_stParameters.m_stUnityAdsParameters.m_oAdsPlacementList[KDefine.U_KEY_ADS_M_BANNER_ADS_PLACEMENT]);
	}

	//! 유니티 애즈 보상 광고 로드 여부를 검사한다
	private bool IsLoadUnityAdsRewardAds() {
		Func.Assert(m_stParameters.m_stUnityAdsParameters.m_oAdsPlacementList.ContainsKey(KDefine.U_KEY_ADS_M_REWARD_ADS_PLACEMENT));
		return Advertisement.IsReady(m_stParameters.m_stUnityAdsParameters.m_oAdsPlacementList[KDefine.U_KEY_ADS_M_REWARD_ADS_PLACEMENT]);
	}

	//! 유니티 애즈 전면 광고 로드 여부를 검사한다
	private bool IsLoadUnityAdsFullscreenAds() {
		Func.Assert(m_stParameters.m_stUnityAdsParameters.m_oAdsPlacementList.ContainsKey(KDefine.U_KEY_ADS_M_FULLSCREEN_ADS_PLACEMENT));
		return Advertisement.IsReady(m_stParameters.m_stUnityAdsParameters.m_oAdsPlacementList[KDefine.U_KEY_ADS_M_FULLSCREEN_ADS_PLACEMENT]);
	}

	//! 유니티 애즈 배너 광고를 로드한다
	private void LoadUnityAdsBannerAds() {
		var oLoadOptions = new BannerLoadOptions() {
			loadCallback = this.OnLoadUnityAdsBannerAds,
			errorCallback = this.OnLoadFailUnityAdsBannerAds
		};

		string oPlacement = m_stParameters.m_stUnityAdsParameters.m_oAdsPlacementList[KDefine.U_KEY_ADS_M_BANNER_ADS_PLACEMENT];
		Advertisement.Banner.Load(oPlacement, oLoadOptions);
	}

	//! 유니티 애즈 보상 광고를 로드한다
	private void LoadUnityAdsRewardAds() {
		string oPlacement = m_stParameters.m_stUnityAdsParameters.m_oAdsPlacementList[KDefine.U_KEY_ADS_M_REWARD_ADS_PLACEMENT];
		m_stVariable.m_stUnityAdsVariable.m_oLoadingAdsPlacementList.ExAddValue(oPlacement);

		Advertisement.Load(oPlacement);
	}

	//! 유니티 애즈 전면 광고를 로드한다
	private void LoadUnityAdsFullscreenAds() {
		string oPlacement = m_stParameters.m_stUnityAdsParameters.m_oAdsPlacementList[KDefine.U_KEY_ADS_M_FULLSCREEN_ADS_PLACEMENT];
		m_stVariable.m_stUnityAdsVariable.m_oLoadingAdsPlacementList.ExAddValue(oPlacement);

		Advertisement.Load(oPlacement);
	}

	//! 유니티 애즈 배너 광고를 출력한다
	private void ShowUnityAdsBannerAds() {
		if(m_stParameters.m_eBannerAdsType == EAdsType.UNITY_ADS) {
			Advertisement.Banner.Show(m_stParameters.m_stUnityAdsParameters.m_oAdsPlacementList[KDefine.U_KEY_ADS_M_BANNER_ADS_PLACEMENT]);
		}
	}

	//! 유니티 애즈 보상 광고를 출력한다
	private void ShowUnityAdsRewardAds() {
		Advertisement.Show(m_stParameters.m_stUnityAdsParameters.m_oAdsPlacementList[KDefine.U_KEY_ADS_M_REWARD_ADS_PLACEMENT]);
	}

	//! 유니티 애즈 전면 광고를 출력한다
	private void ShowUnityAdsFullscreenAds() {
		Advertisement.Show(m_stParameters.m_stUnityAdsParameters.m_oAdsPlacementList[KDefine.U_KEY_ADS_M_FULLSCREEN_ADS_PLACEMENT]);
	}

	//! 유니티 애즈 배너 광고를 닫는다
	private void CloseUnityAdsBannerAds(bool a_bIsRemove) {
		Advertisement.Banner.Hide();
	}

	//! 유니티 애즈 배너 광고 로드 결과를 처리한다
	private void HandleLoadUnityAdsBannerAdsResult() {
		Func.LateCallFunc(this, (a_oComponent, a_oParams) => {
			this.ShowBannerAds(EAdsType.UNITY_ADS, null);
		});
	}
	#endregion			// 함수
}
#endif			// #if ADS_ENABLE && UNITY_ADS_ENABLE
