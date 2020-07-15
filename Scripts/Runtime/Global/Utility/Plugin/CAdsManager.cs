using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ADS_ENABLE
#if ADMOB_ENABLE
using GoogleMobileAds.Api;
#endif			// #if ADMOB_ENABLE

#if UNITY_ADS_ENABLE
using UnityEngine.Advertisements;
#endif			// #if UNITY_ADS_ENABLE

//! 광고 관리자
public partial class CAdsManager : CSingleton<CAdsManager> {
#if ADMOB_ENABLE
	//! 애드몹 매개 변수
	public struct STAdmobParameters {
		public List<string> m_oTemplateIDList;
		public Dictionary<string, string> m_oAdsIDList;
	}

	//! 애드몹 변수
	private struct STAdmobVariable {
		public bool m_bIsLoadBannerAds;
		public bool m_bIsShowBannerAds;

		public bool m_bIsLoadNativeAds;

		public int m_nBannerAdsLoadTryTimes;
		public int m_nRewardAdsLoadTryTimes;
		public int m_nNativeAdsLoadTryTimes;
		public int m_nFullscreenAdsLoadTryTimes;

		public BannerView m_oBannerAds;
		public RewardedAd m_oRewardAds;
		public InterstitialAd m_oFullscreenAds;

		public AdLoader m_oAdsLoader;
		public AdRequest.Builder m_oRequestBuilder;

		public Dictionary<string, CustomNativeTemplateAd> m_oNativeAdsList;
	}
#endif			// #if ADMOB_ENABLE

#if UNITY_ADS_ENABLE
	//! 유니티 애즈 매개 변수
	public struct STUnityAdsParameters {
		public string m_oGameID;
		public Dictionary<string, string> m_oAdsPlacementList;
	}

	//! 유니티 애즈 변수
	private struct STUnityAdsVariable : IUnityAdsListener {
		public int m_nBannerAdsLoadTryTimes;
		public int m_nRewardAdsLoadTryTimes;
		public int m_nFullscreenAdsLoadTryTimes;

		public List<string> m_oLoadingAdsPlacementList;

		public System.Action<string> m_oLoadCallback;
		public System.Action<string> m_oLoadFailCallback;
		public System.Action<string, ShowResult> m_oCloseCallback;

		#region 인터페이스
		public void OnUnityAdsReady(string a_oPlacement) {
			m_oLoadCallback?.Invoke(a_oPlacement);
		}

		public void OnUnityAdsDidError(string a_oMsg) {
			m_oLoadFailCallback?.Invoke(a_oMsg);
		}

		public void OnUnityAdsDidStart(string a_oPlacement) {
			// Do Nothing
		}

		public void OnUnityAdsDidFinish(string a_oPlacement, ShowResult a_eShowResult) {
			m_oCloseCallback?.Invoke(a_oPlacement, a_eShowResult);
		}
		#endregion			// 인터페이스
	}
#endif			// #if UNITY_ADS_ENABLE

#if IRON_SOURCE_ENABLE
	//! 아이언 소스 매개 변수
	public struct STIronSourceParameters {
		public string m_oAppKey;

		public List<string> m_oAdsUnitList;
		public Dictionary<string, string> m_oAdsPlacementList;
	}

	//! 아이언 소스 변수
	private struct STIronSourceVariable {
		public bool m_bIsLoadBannerAds;

		public int m_nBannerAdsLoadTryTimes;
		public int m_nRewardAdsLoadTryTimes;
		public int m_nFullscreenAdsLoadTryTimes;
	}
#endif			// #if IRON_SOURCE_ENABLE

#if APP_LOVIN_ENABLE
	//! 앱 로빈 매개 변수
	public struct STAppLovinParameters {
		public string m_oSDKKey;
		public Dictionary<string, string> m_oAdsIDList;
	}

	//! 앱 로빈 변수
	public struct STAppLovinVariable {
		public int m_nBannerAdsLoadTryTimes;
		public int m_nRewardAdsLoadTryTimes;
		public int m_nFullscreenAdsLoadTryTimes;
	}
#endif			// #if APP_LOVIN_ENABLE

	//! 매개 변수
	public struct STParameters {
		public EAdsType m_eBannerAdsType;
		public List<string> m_oDeviceIDList;

#if ADMOB_ENABLE
		public STAdmobParameters m_stAdmobParameters;
#endif			// #if ADMOB_ENABLE

#if UNITY_ADS_ENABLE
		public STUnityAdsParameters m_stUnityAdsParameters;
#endif			// #if UNITY_ADS_ENABLE

#if IRON_SOURCE_ENABLE
		public STIronSourceParameters m_stIronSourceParameters;
#endif			// #if IRON_SOURCE_ENABLE

#if APP_LOVIN_ENABLE
		public STAppLovinParameters m_stAppLovinParameters;
#endif			// #if APP_LOVIN_ENABLE
	}

	//! 변수
	private struct STVariable {
#if ADMOB_ENABLE
		public STAdmobVariable m_stAdmobVariable;
#endif			// #if ADMOB_ENABLE

#if UNITY_ADS_ENABLE
		public STUnityAdsVariable m_stUnityAdsVariable;
#endif			// #if UNITY_ADS_ENABLE

#if IRON_SOURCE_ENABLE
		public STIronSourceVariable m_stIronSourceVariable;
#endif			// #if IRON_SOURCE_ENABLE

#if APP_LOVIN_ENABLE
		public STAppLovinVariable m_stAppLovinVariable;
#endif			// #if APP_LOVIN_ENABLE
	}

	#region 변수
	private STVariable m_stVariable;
	private STParameters m_stParameters;

	private Dictionary<EAdsType, System.Action> m_oBannerAdsLoaderList = new Dictionary<EAdsType, System.Action>();
	private Dictionary<EAdsType, System.Action> m_oRewardAdsLoaderList = new Dictionary<EAdsType, System.Action>();
	private Dictionary<EAdsType, System.Action> m_oNativeAdsLoaderList = new Dictionary<EAdsType, System.Action>();
	private Dictionary<EAdsType, System.Action> m_oFullscreenAdsLoaderList = new Dictionary<EAdsType, System.Action>();

	private Dictionary<EAdsType, System.Func<bool>> m_oBannerAdsCheckerList = new Dictionary<EAdsType, System.Func<bool>>();
	private Dictionary<EAdsType, System.Func<bool>> m_oRewardAdsCheckerList = new Dictionary<EAdsType, System.Func<bool>>();
	private Dictionary<EAdsType, System.Func<bool>> m_oNativeAdsCheckerList = new Dictionary<EAdsType, System.Func<bool>>();
	private Dictionary<EAdsType, System.Func<bool>> m_oFullscreenAdsCheckerList = new Dictionary<EAdsType, System.Func<bool>>();

	private Dictionary<EAdsType, System.Action> m_oBannerAdsShowerList = new Dictionary<EAdsType, System.Action>();
	private Dictionary<EAdsType, System.Action> m_oRewardAdsShowerList = new Dictionary<EAdsType, System.Action>();
	private Dictionary<EAdsType, System.Action> m_oFullscreenAdsShowerList = new Dictionary<EAdsType, System.Action>();

	private Dictionary<EAdsType, System.Action<bool>> m_oBannerAdsCloserList = new Dictionary<EAdsType, System.Action<bool>>();
	private Dictionary<EAdsType, System.Func<string, CustomNativeTemplateAd>> m_oNativeAdsGetterList = new Dictionary<EAdsType, System.Func<string, CustomNativeTemplateAd>>();

	private Dictionary<EAdsType, System.Action<CAdsManager>> m_oRewardAdsCloseCallbackList = new Dictionary<EAdsType, System.Action<CAdsManager>>();
	private Dictionary<EAdsType, System.Action<CAdsManager, STAdsRewardInfo, bool>> m_oRewardAdsCallbackList = new Dictionary<EAdsType, System.Action<CAdsManager, STAdsRewardInfo, bool>>();

	private Dictionary<EAdsType, System.Action<CAdsManager>> m_oFullscreenAdsCloseCallbackList = new Dictionary<EAdsType, System.Action<CAdsManager>>();
	#endregion			// 변수

	#region 프로퍼티
	public bool IsInit { get; private set; } = false;

	public bool IsEnableBannerAds { get; set; } = true;
	public bool IsEnableRewardAds { get; set; } = true;
	public bool IsEnableNativeAds { get; set; } = true;
	public bool IsEnableFullscreenAds { get; set; } = true;

	public float BannerAdsHeight { get; private set; } = 0.0f;

#if ADMOB_ENABLE
	private BannerView AdmobBannerAds {
		get {
			if(this.IsEnableBannerAds && m_stVariable.m_stAdmobVariable.m_oBannerAds == null) {
				string oAdsID = m_stParameters.m_stAdmobParameters.m_oAdsIDList[KDefine.U_KEY_ADS_M_BANNER_ADS_ID];
				m_stVariable.m_stAdmobVariable.m_oBannerAds = new BannerView(oAdsID, KDefine.U_SIZE_ADMOB_BANNER, AdPosition.Bottom);

				m_stVariable.m_stAdmobVariable.m_oBannerAds.OnAdLoaded -= this.OnLoadAdmobBannerAds;
				m_stVariable.m_stAdmobVariable.m_oBannerAds.OnAdLoaded += this.OnLoadAdmobBannerAds;

				m_stVariable.m_stAdmobVariable.m_oBannerAds.OnAdFailedToLoad -= this.OnLoadFailAdmobBannerAds;
				m_stVariable.m_stAdmobVariable.m_oBannerAds.OnAdFailedToLoad += this.OnLoadFailAdmobBannerAds;

				m_stVariable.m_stAdmobVariable.m_oBannerAds.OnAdClosed -= this.OnCloseAdmobBannerAds;
				m_stVariable.m_stAdmobVariable.m_oBannerAds.OnAdClosed += this.OnCloseAdmobBannerAds;
			}

			return m_stVariable.m_stAdmobVariable.m_oBannerAds;
		}
	}

	private RewardedAd AdmobRewardAds {
		get {
			if(this.IsEnableRewardAds && m_stVariable.m_stAdmobVariable.m_oRewardAds == null) {
				string oAdsID = m_stParameters.m_stAdmobParameters.m_oAdsIDList[KDefine.U_KEY_ADS_M_REWARD_ADS_ID];
				m_stVariable.m_stAdmobVariable.m_oRewardAds = new RewardedAd(oAdsID);

				m_stVariable.m_stAdmobVariable.m_oRewardAds.OnAdFailedToLoad -= this.OnLoadFailAdmobRewardAds;
				m_stVariable.m_stAdmobVariable.m_oRewardAds.OnAdFailedToLoad += this.OnLoadFailAdmobRewardAds;

				m_stVariable.m_stAdmobVariable.m_oRewardAds.OnAdClosed -= this.OnCloseAdmobRewardAds;
				m_stVariable.m_stAdmobVariable.m_oRewardAds.OnAdClosed += this.OnCloseAdmobRewardAds;

				m_stVariable.m_stAdmobVariable.m_oRewardAds.OnUserEarnedReward -= this.OnReceiveAdmobUserReward;
				m_stVariable.m_stAdmobVariable.m_oRewardAds.OnUserEarnedReward += this.OnReceiveAdmobUserReward;
			}

			return m_stVariable.m_stAdmobVariable.m_oRewardAds;
		}
	}

	private InterstitialAd AdmobFullscreenAds {
		get {
			if(this.IsEnableFullscreenAds && m_stVariable.m_stAdmobVariable.m_oFullscreenAds == null) {
				string oAdsID = m_stParameters.m_stAdmobParameters.m_oAdsIDList[KDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID];
				m_stVariable.m_stAdmobVariable.m_oFullscreenAds = new InterstitialAd(oAdsID);

				m_stVariable.m_stAdmobVariable.m_oFullscreenAds.OnAdFailedToLoad -= this.OnLoadFailAdmobFullscreenAds;
				m_stVariable.m_stAdmobVariable.m_oFullscreenAds.OnAdFailedToLoad += this.OnLoadFailAdmobFullscreenAds;
				
				m_stVariable.m_stAdmobVariable.m_oFullscreenAds.OnAdClosed -= this.OnCloseAdmobFullscreenAds;
				m_stVariable.m_stAdmobVariable.m_oFullscreenAds.OnAdClosed += this.OnCloseAdmobFullscreenAds;
			}

			return m_stVariable.m_stAdmobVariable.m_oFullscreenAds;
		}
	}

	private AdLoader AdmobAdsLoader {
		get {
			if(m_stVariable.m_stAdmobVariable.m_oAdsLoader == null) {
				string oAdsID = m_stParameters.m_stAdmobParameters.m_oAdsIDList[KDefine.U_KEY_ADS_M_NATIVE_ADS_ID];
				var oBuilder = new AdLoader.Builder(oAdsID);

				for(int i = 0; i < m_stParameters.m_stAdmobParameters.m_oTemplateIDList.Count; ++i) {
					oBuilder.ForCustomNativeAd(m_stParameters.m_stAdmobParameters.m_oTemplateIDList[i]);
				}

				m_stVariable.m_stAdmobVariable.m_oAdsLoader = oBuilder.Build();
			}

			return m_stVariable.m_stAdmobVariable.m_oAdsLoader;
		}
	}

	private AdRequest.Builder AdmobRequestBuilder {
		get {
			if(m_stVariable.m_stAdmobVariable.m_oRequestBuilder == null) {
				m_stVariable.m_stAdmobVariable.m_oRequestBuilder = new AdRequest.Builder();
				m_stVariable.m_stAdmobVariable.m_oRequestBuilder.AddTestDevice(AdRequest.TestDeviceSimulator);
				
				for(int i = 0; i < m_stParameters.m_oDeviceIDList.Count; ++i) {
					m_stVariable.m_stAdmobVariable.m_oRequestBuilder.AddTestDevice(m_stParameters.m_oDeviceIDList[i]);
				}
			}

			return m_stVariable.m_stAdmobVariable.m_oRequestBuilder;
		}
	}
#endif			// #if ADMOB_ENABLE
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

#if ADMOB_ENABLE
		m_oBannerAdsLoaderList.Add(EAdsType.ADMOB, this.LoadAdmobBannerAds);
		m_oRewardAdsLoaderList.Add(EAdsType.ADMOB, this.LoadAdmobRewardAds);
		m_oNativeAdsLoaderList.Add(EAdsType.ADMOB, this.LoadAdmobNativeAds);
		m_oFullscreenAdsLoaderList.Add(EAdsType.ADMOB, this.LoadAdmobFullscreenAds);

		m_oBannerAdsCheckerList.Add(EAdsType.ADMOB, this.IsLoadAdmobBannerAds);
		m_oRewardAdsCheckerList.Add(EAdsType.ADMOB, this.IsLoadAdmobRewardAds);
		m_oNativeAdsCheckerList.Add(EAdsType.ADMOB, this.IsLoadAdmobNativeAds);
		m_oFullscreenAdsCheckerList.Add(EAdsType.ADMOB, this.IsLoadAdmobFullscreenAds);

		m_oBannerAdsShowerList.Add(EAdsType.ADMOB, this.ShowAdmobBannerAds);
		m_oRewardAdsShowerList.Add(EAdsType.ADMOB, this.ShowAdmobRewardAds);
		m_oFullscreenAdsShowerList.Add(EAdsType.ADMOB, this.ShowAdmobFullscreenAds);

		m_oBannerAdsCloserList.Add(EAdsType.ADMOB, this.CloseAdmobBannerAds);
		m_oNativeAdsGetterList.Add(EAdsType.ADMOB, this.GetAdmobNativeAds);
#endif			// #if ADMOB_ENABLE

#if UNITY_ADS_ENABLE
		m_oBannerAdsLoaderList.Add(EAdsType.UNITY_ADS, this.LoadUnityAdsBannerAds);
		m_oRewardAdsLoaderList.Add(EAdsType.UNITY_ADS, this.LoadUnityAdsRewardAds);
		m_oFullscreenAdsLoaderList.Add(EAdsType.UNITY_ADS, this.LoadUnityAdsFullscreenAds);

		m_oBannerAdsCheckerList.Add(EAdsType.UNITY_ADS, this.IsLoadUnityAdsBannerAds);
		m_oRewardAdsCheckerList.Add(EAdsType.UNITY_ADS, this.IsLoadUnityAdsRewardAds);
		m_oFullscreenAdsCheckerList.Add(EAdsType.UNITY_ADS, this.IsLoadUnityAdsFullscreenAds);

		m_oBannerAdsShowerList.Add(EAdsType.UNITY_ADS, this.ShowUnityAdsBannerAds);
		m_oRewardAdsShowerList.Add(EAdsType.UNITY_ADS, this.ShowUnityAdsRewardAds);
		m_oFullscreenAdsShowerList.Add(EAdsType.UNITY_ADS, this.ShowUnityAdsFullscreenAds);

		m_oBannerAdsCloserList.Add(EAdsType.UNITY_ADS, this.CloseUnityAdsBannerAds);
#endif			// #if UNITY_ADS_ENABLE

#if IRON_SOURCE_ENABLE
		m_oBannerAdsLoaderList.Add(EAdsType.IRON_SOURCE, this.LoadIronSourceBannerAds);
		m_oRewardAdsLoaderList.Add(EAdsType.IRON_SOURCE, this.LoadIronSourceRewardAds);
		m_oFullscreenAdsLoaderList.Add(EAdsType.IRON_SOURCE, this.LoadIronSourceFullscreenAds);

		m_oBannerAdsCheckerList.Add(EAdsType.IRON_SOURCE, this.IsLoadIronSourceBannerAds);
		m_oRewardAdsCheckerList.Add(EAdsType.IRON_SOURCE, this.IsLoadIronSourceRewardAds);
		m_oFullscreenAdsCheckerList.Add(EAdsType.IRON_SOURCE, this.IsLoadIronSourceFullscreenAds);

		m_oBannerAdsShowerList.Add(EAdsType.IRON_SOURCE, this.ShowIronSourceBannerAds);
		m_oRewardAdsShowerList.Add(EAdsType.IRON_SOURCE, this.ShowIronSourceRewardAds);
		m_oFullscreenAdsShowerList.Add(EAdsType.IRON_SOURCE, this.ShowIronSourceFullscreenAds);

		m_oBannerAdsCloserList.Add(EAdsType.IRON_SOURCE, this.CloseIronSourceBannerAds);
#endif			// #if IRON_SOURCE_ENABLE
	}

	//! 초기화
	public virtual void Init(STParameters a_stParameters, System.Action<CAdsManager, bool> a_oCallback) {
		Func.ShowLog("CAdsManager.Init: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_stParameters.m_eBannerAdsType);

		if(!this.IsInit && Func.IsMobilePlatform()) {
#if UNITY_ADS_ENABLE
			Func.Assert(a_stParameters.m_stUnityAdsParameters.m_oGameID.ExIsValid());
#endif			// #if UNITY_ADS_ENABLE

#if IRON_SOURCE_ENABLE
			Func.Assert(a_stParameters.m_stIronSourceParameters.m_oAppKey.ExIsValid());
#endif			// #if IRON_SOURCE_ENABLE

#if APP_LOVIN_ENABLE
			Func.Assert(a_stParameters.m_stAppLovinParameters.m_oSDKKey.ExIsValid());
#endif			// #if APP_LOVIN_ENABLE

			this.IsInit = true;
			m_stParameters = a_stParameters;

#if ADMOB_ENABLE
			MobileAds.Initialize(this.OnInitAdmob);
#endif			// #if ADMOB_ENABLE

#if UNITY_ADS_ENABLE
			m_stVariable.m_stUnityAdsVariable.m_oLoadingAdsPlacementList = new List<string>();

			m_stVariable.m_stUnityAdsVariable.m_oLoadCallback = this.OnLoadUnityAds;
			m_stVariable.m_stUnityAdsVariable.m_oLoadFailCallback = this.OnLoadFailUnityAds;
			m_stVariable.m_stUnityAdsVariable.m_oCloseCallback = this.OnCloseUnityAds;

			Advertisement.AddListener(m_stVariable.m_stUnityAdsVariable);
			Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);

#if ADS_TEST_ENABLE
			Advertisement.Initialize(a_stParameters.m_stUnityAdsParameters.m_oGameID, true, true);
#else
			Advertisement.Initialize(a_stParameters.m_stUnityAdsParameters.m_oGameID, false, true);
#endif			// #if ADS_TEST_ENABLE
#endif			// #if UNITY_ADS_ENABLE

#if IRON_SOURCE_ENABLE
			if(a_stParameters.m_stIronSourceParameters.m_oAdsUnitList.ExIsValid()) {
				var oUnitAdsList = new List<string>();

				for(int i = 0; i < a_stParameters.m_stIronSourceParameters.m_oAdsUnitList.Count; ++i) {
					if(a_stParameters.m_stIronSourceParameters.m_oAdsUnitList[i].ExIsValid()) {
						oUnitAdsList.Add(a_stParameters.m_stIronSourceParameters.m_oAdsUnitList[i]);
					}
				}

				IronSourceEvents.onBannerAdLoadedEvent -= this.OnLoadIronSourceBannerAds;
				IronSourceEvents.onBannerAdLoadedEvent += this.OnLoadIronSourceBannerAds;

				IronSourceEvents.onBannerAdLoadFailedEvent -= this.OnLoadFailIronSourceBannerAds;
				IronSourceEvents.onBannerAdLoadFailedEvent += this.OnLoadFailIronSourceBannerAds;

				IronSourceEvents.onRewardedVideoAdClosedEvent -= this.OnCloseIronSourceRewardAds;
				IronSourceEvents.onRewardedVideoAdClosedEvent += this.OnCloseIronSourceRewardAds;

				IronSourceEvents.onRewardedVideoAdRewardedEvent -= this.OnReceiveIronSourceUserReward;
				IronSourceEvents.onRewardedVideoAdRewardedEvent += this.OnReceiveIronSourceUserReward;

				IronSourceEvents.onRewardedVideoAvailabilityChangedEvent -= this.OnChangeIronSourceRewardAdsState;
				IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += this.OnChangeIronSourceRewardAdsState;

				IronSourceEvents.onInterstitialAdLoadFailedEvent -= this.OnLoadFailIronSourceFullscreenAds;
				IronSourceEvents.onInterstitialAdLoadFailedEvent += this.OnLoadFailIronSourceFullscreenAds;

				IronSourceEvents.onInterstitialAdClosedEvent -= this.OnCloseIronSourceFullscreenAds;
				IronSourceEvents.onInterstitialAdClosedEvent += this.OnCloseIronSourceFullscreenAds;

				IronSource.Agent.init(a_stParameters.m_stIronSourceParameters.m_oAppKey, oUnitAdsList.ToArray());
				IronSource.Agent.validateIntegration();
				IronSource.Agent.shouldTrackNetworkState(true);
			}
#endif			// #if IRON_SOURCE_ENABLE

#if APP_LOVIN_ENABLE

#endif			// #if APP_LOVIN_ENABLE
		}

		a_oCallback?.Invoke(this, this.IsInit);
	}

	//! 제거 되었을 경우
	public override void OnDestroy() {
		base.OnDestroy();

		if(this.IsInit && !CSceneManager.IsAppQuit) {
#if ADMOB_ENABLE
			m_stVariable.m_stAdmobVariable.m_oBannerAds?.Destroy();
			m_stVariable.m_stAdmobVariable.m_oFullscreenAds?.Destroy();
#endif			// #if ADMOB_ENABLE
		}
	}

	//! 어플리케이션이 정지 되었을 경우
	public void OnApplicationPause(bool a_bIsPause) {
		if(this.IsInit) {
#if IRON_SOURCE_ENABLE
			IronSource.Agent.onApplicationPause(a_bIsPause);
#endif			// #if IRON_SOURCE_ENABLE
		}
	}

	//! 배너 광고 로드 여부를 검사한다
	public bool IsLoadBannerAds(EAdsType a_eAdsType) {
		Func.Assert(m_oBannerAdsCheckerList.ContainsKey(a_eAdsType));
		bool bIsEnableAds = this.IsInit && this.IsEnableBannerAds && Func.IsMobilePlatform();

		return bIsEnableAds && m_oBannerAdsCheckerList[a_eAdsType]();
	}

	//! 보상 광고 로드 여부를 검사한다
	public bool IsLoadRewardAds(EAdsType a_eAdsType) {
		Func.Assert(m_oRewardAdsCheckerList.ContainsKey(a_eAdsType));
		bool bIsEnableAds = this.IsInit && this.IsEnableRewardAds && Func.IsMobilePlatform();

		return bIsEnableAds && m_oRewardAdsCheckerList[a_eAdsType]();
	}

	//! 전면 광고 로드 여부를 검사한다
	public bool IsLoadFullscreenAds(EAdsType a_eAdsType) {
		Func.Assert(m_oFullscreenAdsCheckerList.ContainsKey(a_eAdsType));
		bool bIsEnableAds = this.IsInit && this.IsEnableFullscreenAds && Func.IsMobilePlatform();

		return bIsEnableAds && m_oFullscreenAdsCheckerList[a_eAdsType]();
	}

	//! 네이티브 광고 로드 여부를 검사한다
	public bool IsLoadNativeAds(EAdsType a_eAdsType) {
		Func.Assert(m_oNativeAdsCheckerList.ContainsKey(a_eAdsType));
		bool bIsEnableAds = this.IsInit && this.IsEnableNativeAds && Func.IsMobilePlatform();

		return bIsEnableAds && m_oNativeAdsCheckerList[a_eAdsType]();
	}

	//! 배너 광고를 로드한다
	public void LoadBannerAds(EAdsType a_eAdsType) {
		Func.ShowLog("CAdsManager.LoadBannerAds: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_eAdsType);
		Func.Assert(m_oBannerAdsLoaderList.ContainsKey(a_eAdsType));

		bool bIsEnableLoad = this.IsInit && this.IsEnableBannerAds;

		if(bIsEnableLoad && !this.IsLoadBannerAds(a_eAdsType)) {
			m_oBannerAdsLoaderList[a_eAdsType]();
		}
	}

	//! 보상 광고를 로드한다
	public void LoadRewardAds(EAdsType a_eAdsType) {
		Func.ShowLog("CAdsManager.LoadRewardAds: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_eAdsType);
		Func.Assert(m_oRewardAdsLoaderList.ContainsKey(a_eAdsType));

		bool bIsEnableLoad = this.IsInit && this.IsEnableRewardAds;

		if(bIsEnableLoad && !this.IsLoadRewardAds(a_eAdsType)) {
			m_oRewardAdsLoaderList[a_eAdsType]();
		}
	}

	//! 전면 광고를 로드한다
	public void LoadFullscreenAds(EAdsType a_eAdsType) {
		Func.ShowLog("CAdsManager.LoadFullscreenAds: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_eAdsType);
		Func.Assert(m_oFullscreenAdsLoaderList.ContainsKey(a_eAdsType));

		bool bIsEnableLoad = this.IsInit && this.IsEnableFullscreenAds;

		if(bIsEnableLoad && !this.IsLoadFullscreenAds(a_eAdsType)) {
			m_oFullscreenAdsLoaderList[a_eAdsType]();
		}
	}

	//! 네이티브 광고를 로드한다
	public void LoadNativeAds(EAdsType a_eAdsType) {
		Func.ShowLog("CAdsManager.LoadNativeAds: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_eAdsType);
		Func.Assert(m_oNativeAdsLoaderList.ContainsKey(a_eAdsType));

		bool bIsEnableLoad = this.IsInit && this.IsEnableNativeAds;

		if(bIsEnableLoad && !this.IsLoadNativeAds(a_eAdsType)) {
			m_oNativeAdsLoaderList[a_eAdsType]();
		}
	}

	//! 배너 광고를 출력한다
	public void ShowBannerAds(EAdsType a_eAdsType, System.Action<CAdsManager, bool> a_oCallback) {
		Func.ShowLog("CAdsManager.ShowBannerAds: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_eAdsType);
		Func.Assert(m_oBannerAdsShowerList.ContainsKey(a_eAdsType));

		bool bIsSuccess = false;

		if(this.IsLoadBannerAds(a_eAdsType)) {
			bIsSuccess = true;
			m_oBannerAdsShowerList[a_eAdsType]();
		}

		a_oCallback?.Invoke(this, bIsSuccess);
	}

	//! 보상 광고를 출력한다
	public void ShowRewardAds(EAdsType a_eAdsType, 
		System.Action<CAdsManager, STAdsRewardInfo, bool> a_oCallback, System.Action<CAdsManager> a_oCloseCallback = null) {
		Func.ShowLog("CAdsManager.ShowRewardAds: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_eAdsType);
		Func.Assert(m_oRewardAdsShowerList.ContainsKey(a_eAdsType));

		if(this.IsLoadRewardAds(a_eAdsType)) {
			m_oRewardAdsCallbackList.ExAddValue(a_eAdsType, a_oCallback);
			m_oRewardAdsCloseCallbackList.ExAddValue(a_eAdsType, a_oCloseCallback);

			m_oRewardAdsShowerList[a_eAdsType]();
		} else {
			a_oCallback?.Invoke(this, default(STAdsRewardInfo), false);
		}
	}

	//! 전면 광고를 출력한다
	public void ShowFullscreenAds(EAdsType a_eAdsType, 
		System.Action<CAdsManager, bool> a_oCallback, System.Action<CAdsManager> a_oCloseCallback = null) {
		Func.ShowLog("CAdsManager.ShowFullscreenAds: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_eAdsType);
		Func.Assert(m_oFullscreenAdsShowerList.ContainsKey(a_eAdsType));

		bool bIsSuccess = false;

		if(this.IsLoadFullscreenAds(a_eAdsType)) {
			bIsSuccess = true;
			m_oFullscreenAdsCloseCallbackList.ExAddValue(a_eAdsType, a_oCloseCallback);

			m_oFullscreenAdsShowerList[a_eAdsType]();
		}

		a_oCallback?.Invoke(this, bIsSuccess);
	}

	//! 배너 광고를 닫는다
	public void CloseBannerAds(EAdsType a_eAdsType, bool a_bIsRemove = false) {
		Func.ShowLog("CAdsManager.CloseBannerAds: {0}, {1}", KDefine.B_LOG_COLOR_PLUGIN, a_eAdsType, a_bIsRemove);
		Func.Assert(m_oBannerAdsCloserList.ContainsKey(a_eAdsType));

		bool bIsEnable = this.IsInit && !this.IsEnableBannerAds;

		if(bIsEnable || this.IsLoadBannerAds(a_eAdsType)) {
			this.BannerAdsHeight = 0.0f;
			m_oBannerAdsCloserList[a_eAdsType](a_bIsRemove);
		}
	}

	//! 보상 광고 닫힘 결과를 처리한다
	private void HandleCloseRewardAdsResult(EAdsType a_eAdsType) {
		if(m_oRewardAdsCloseCallbackList.ContainsKey(a_eAdsType)) {
			var oCallback = m_oRewardAdsCloseCallbackList[a_eAdsType];

			m_oRewardAdsCallbackList.Remove(a_eAdsType);
			m_oRewardAdsCloseCallbackList.Remove(a_eAdsType);

			oCallback?.Invoke(this);
		}
	}

	//! 보상 광고 결과를 처리한다
	private void HandleRewardAdsResult(EAdsType a_eAdsType, STAdsRewardInfo a_stRewardInfo, bool a_bIsSuccess) {
		Func.ShowLog("CAdsManager.HandleRewardAdsResult: {0}, {1}, {2}", 
			KDefine.B_LOG_COLOR_PLUGIN, a_eAdsType, a_stRewardInfo, a_bIsSuccess);

		if(m_oRewardAdsCallbackList.ContainsKey(a_eAdsType)) {
			var oCallback = m_oRewardAdsCallbackList[a_eAdsType];
			m_oRewardAdsCallbackList.Remove(a_eAdsType);

			oCallback?.Invoke(this, a_stRewardInfo, a_bIsSuccess);
		}
	}

	//! 전면 광고 닫힘 결과를 처리한다
	private void HandleCloseFullscreenAdsResult(EAdsType a_eAdsType) {
		Func.ShowLog("CAdsManager.HandleCloseFullscreenAdsResult: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_eAdsType);

		if(m_oFullscreenAdsCloseCallbackList.ContainsKey(a_eAdsType)) {
			var oCallback = m_oFullscreenAdsCloseCallbackList[a_eAdsType];
			m_oFullscreenAdsCloseCallbackList.Remove(a_eAdsType);

			oCallback?.Invoke(this);
		}
	}
	#endregion			// 함수
}
#endif			// #if ADS_ENABLE
