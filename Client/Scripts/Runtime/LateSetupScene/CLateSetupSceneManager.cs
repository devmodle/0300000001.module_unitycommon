using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#elif UNITY_ANDROID
using UnityEngine.Android;
#endif // #if UNITY_IOS

namespace LateSetupScene
{
	/** 지연 설정 씬 관리자 */
	public abstract partial class CLateSetupSceneManager : CSceneManager
	{
		#region 추상
		/** 한국 약관 동의 팝업을 출력한다 */
		protected abstract void ShowKRAgreePopup(string a_oPrivacy, string a_oServices, System.Action<CPopup> a_oCallback);

		/** 유럽 연합 약관 동의 팝업을 출력한다 */
		protected abstract void ShowEUAgreePopup(string a_oPrivacyURL, string a_oServicesURL, System.Action<CPopup> a_oCallback);

		/** 추적 설명 팝업을 출력한다 */
		protected abstract void ShowTrackingDescPopup(System.Action<CPopup> a_oCallback);

#if UNITY_ANDROID
		/** 유저 권한을 요청한다 */
		protected abstract void RequestUserPermission(string a_oPermission, System.Action<string, bool> a_oCallback);
#endif // #if UNITY_ANDROID
		#endregion // 추상

		#region 프로퍼티
		public virtual bool IsAutoInitManager => false;

#if UNITY_EDITOR
		public override int OrderScript => KCDefine.B_SCRIPT_O_LATE_SETUP_SCENE_MANAGER;
#endif // #if UNITY_EDITOR

#if UNITY_ANDROID
		public List<string> UserPermissionList { get; } = new List<string>();
#endif // #if UNITY_ANDROID
		#endregion // 프로퍼티

		#region 클래스 프로퍼티
		public static Dictionary<string, object> LogDataDict { get; private set; } = new Dictionary<string, object>();

#if ADS_MODULE_ENABLE
		public static bool IsPurchaseRemoveAds { get; private set; } = false;
		public static bool IsAutoLoadBannerAds { get; private set; } = false;
		public static bool IsAutoLoadFullscreenAds { get; private set; } = false;
		public static bool IsAutoLoadRewardAds { get; private set; } = false;
#endif // #if ADS_MODULE_ENABLE
		#endregion // 클래스 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake()
		{
			base.Awake();

			// 초기화가 필요 할 경우
			if(!CSceneManager.IsInit)
			{
				return;
			}

			CFunc.BroadcastMsg(KCDefine.SS_FUNC_N_SETUP_SCENE_EVENT,
				global::SetupScene.CSetupSceneManager.ESetupSceneEvent.LOAD_LATE_SETUP_SCENE, false);
		}

		/** 초기화 */
		public sealed override void Start()
		{
			base.Start();

			// 초기화가 필요 할 경우
			if(!CSceneManager.IsInit)
			{
				return;
			}

			StartCoroutine(this.CoStart());
		}

		/** 씬을 설정한다 */
		protected virtual void Setup()
		{
			// Do Something
		}

		/** 약관 동의 팝업이 닫혔을 경우 */
		private void OnCloseAgreePopup(CPopup a_oSender)
		{
			CCommonAppInfoStorage.Inst.AppInfo.IsAgree = true;

			CCommonAppInfoStorage.Inst.SaveAppInfo();
			CCommonAppInfoStorage.Inst.SetCloseAgreePopup(true);

#if ROBO_TEST_ENABLE
			this.OnCloseTrackingConsentView(true);
#else
			// 추적 동의가 필요 할 경우
			if(CAccess.IsNeedsTrackingConsent && CCommonAppInfoStorage.Inst.AppInfo.IsEnableShowTrackingDescPopup)
			{
				this.ShowTrackingDescPopup((a_oPopupSender) => this.ShowTrackingConsentView());
			}
			else
			{
				this.OnCloseTrackingConsentView(true);
			}
#endif // #if ROBO_TEST_ENABLE
		}

		/** 추적 동의 뷰가 닫혔을 경우 */
		private void OnCloseTrackingConsentView(bool a_bIsSuccess)
		{
			CCommonAppInfoStorage.Inst.AppInfo.IsAgreeTracking = a_bIsSuccess;
			CCommonAppInfoStorage.Inst.AppInfo.IsEnableShowTrackingDescPopup = false;

			CCommonAppInfoStorage.Inst.SaveAppInfo();

			// 관리자 자동 초기화 모드 일 경우
			if(this.IsAutoInitManager)
			{
				CServicesManager.Inst.Init(CServicesManager.MakeParams(new Dictionary<CServicesManager.ECallback, System.Action<CServicesManager, bool>>()
				{
					[CServicesManager.ECallback.INIT] = CLateSetupSceneManager.OnInitServicesManager
				}));

#if ADS_MODULE_ENABLE
				var stAdsParams = CAdsManager.MakeParams(CPluginInfoTable.Inst.AdsPlatform, CPluginInfoTable.Inst.BannerAdsPos, new Dictionary<CAdsManager.ECallback, System.Action<CAdsManager, EAdsPlatform, bool>>() {
					[CAdsManager.ECallback.INIT] = CLateSetupSceneManager.OnInitAdsManager
				});

				CUnityMsgSender.Inst.SendSetIsEnableAdsTrackingMsg(true);

#if ADMOB_ADS_ENABLE
				stAdsParams.m_oAdmobTestDeviceIDList.ExAddVals(CDeviceInfoTable.Inst.DeviceInfo.m_oiOSAdmobTestDeviceIDList);
				stAdsParams.m_oAdmobTestDeviceIDList.ExAddVals(CDeviceInfoTable.Inst.DeviceInfo.m_oAndroidAdmobTestDeviceIDList);

				stAdsParams.m_oAdmobAdsIDDict.TryAdd(KCDefine.B_KEY_ADS_M_BANNER_ADS_ID, CPluginInfoTable.Inst.GetBannerAdsID(EAdsPlatform.ADMOB));
				stAdsParams.m_oAdmobAdsIDDict.TryAdd(KCDefine.B_KEY_ADS_M_REWARD_ADS_ID, CPluginInfoTable.Inst.GetRewardAdsID(EAdsPlatform.ADMOB));
				stAdsParams.m_oAdmobAdsIDDict.TryAdd(KCDefine.B_KEY_ADS_M_FULLSCREEN_ADS_ID, CPluginInfoTable.Inst.GetFullscreenAdsID(EAdsPlatform.ADMOB));
#endif // #if ADMOB_ADS_ENABLE

#if IRON_SRC_ADS_ENABLE
				stAdsParams.m_oIronSrcAppKey = CPluginInfoTable.Inst.IronSrcPluginInfo.m_oAppKey;

				stAdsParams.m_oIronSrcAdsIDDict.TryAdd(KCDefine.B_KEY_ADS_M_BANNER_ADS_ID, CPluginInfoTable.Inst.GetBannerAdsID(EAdsPlatform.IRON_SRC));
				stAdsParams.m_oIronSrcAdsIDDict.TryAdd(KCDefine.B_KEY_ADS_M_REWARD_ADS_ID, CPluginInfoTable.Inst.GetRewardAdsID(EAdsPlatform.IRON_SRC));
				stAdsParams.m_oIronSrcAdsIDDict.TryAdd(KCDefine.B_KEY_ADS_M_FULLSCREEN_ADS_ID, CPluginInfoTable.Inst.GetFullscreenAdsID(EAdsPlatform.IRON_SRC));
#endif // #if IRON_SRC_ADS_ENABLE

#if APP_LOVIN_ADS_ENABLE
				stAdsParams.m_oAppLovinSDKKey = CPluginInfoTable.Inst.AppLovinSDKKey;

				stAdsParams.m_oAppLovinAdsIDDict.TryAdd(KCDefine.B_KEY_ADS_M_BANNER_ADS_ID, CPluginInfoTable.Inst.GetBannerAdsID(EAdsPlatform.APP_LOVIN));
				stAdsParams.m_oAppLovinAdsIDDict.TryAdd(KCDefine.B_KEY_ADS_M_REWARD_ADS_ID, CPluginInfoTable.Inst.GetRewardAdsID(EAdsPlatform.APP_LOVIN));
				stAdsParams.m_oAppLovinAdsIDDict.TryAdd(KCDefine.B_KEY_ADS_M_FULLSCREEN_ADS_ID, CPluginInfoTable.Inst.GetFullscreenAdsID(EAdsPlatform.APP_LOVIN));
#endif // #if APP_LOVIN_ADS_ENABLE

				CAdsManager.Inst.Init(stAdsParams);
#endif // #if ADS_MODULE_ENABLE

#if FLURRY_MODULE_ENABLE
				CFlurryManager.Inst.Init(CFlurryManager.MakeParams(CPluginInfoTable.Inst.FlurryAPIKey, new Dictionary<CFlurryManager.ECallback, System.Action<CFlurryManager, bool>>() {
					[CFlurryManager.ECallback.INIT] = CLateSetupSceneManager.OnInitFlurryManager
				}));
#endif // #if FLURRY_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
				CFacebookManager.Inst.Init(CFacebookManager.MakeParams(new Dictionary<CFacebookManager.ECallback, System.Action<CFacebookManager, bool>>() {
					[CFacebookManager.ECallback.INIT] = CLateSetupSceneManager.OnInitFacebookManager
				}));
#endif // #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
				CFirebaseManager.Inst.Init(CFirebaseManager.MakeParams(new Dictionary<CFirebaseManager.ECallback, System.Action<CFirebaseManager, bool>>() {
					[CFirebaseManager.ECallback.INIT] = CLateSetupSceneManager.OnInitFirebaseManager
				}));
#endif // #if FIREBASE_MODULE_ENABLE

#if APPS_FLYER_MODULE_ENABLE
				CAppsFlyerManager.Inst.Init(CAppsFlyerManager.MakeParams(CProjInfoTable.Inst.ProjInfo.m_oMoreAppsID, CPluginInfoTable.Inst.AppsFlyerPluginInfo.m_oDevKey, new Dictionary<CAppsFlyerManager.ECallback, System.Action<CAppsFlyerManager, bool>>() {
					[CAppsFlyerManager.ECallback.INIT] = CLateSetupSceneManager.OnInitAppsFlyerManager
				}));
#endif // #if APPS_FLYER_MODULE_ENABLE

#if GAME_CENTER_MODULE_ENABLE
				CGameCenterManager.Inst.Init(CGameCenterManager.MakeParams(new Dictionary<CGameCenterManager.ECallback, System.Action<CGameCenterManager, bool>>() {
					[CGameCenterManager.ECallback.INIT] = CLateSetupSceneManager.OnInitGameCenterManager
				}));
#endif // #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
				CPurchaseManager.Inst.Init(CPurchaseManager.MakeParams(CProductInfoTable.Inst.ProductInfoList, new Dictionary<CPurchaseManager.ECallback, System.Action<CPurchaseManager, bool>>()
				{
					[CPurchaseManager.ECallback.INIT] = CLateSetupSceneManager.OnInitPurchaseManager
				}));
#endif // #if PURCHASE_MODULE_ENABLE

#if NOTI_MODULE_ENABLE
				CNotiManager.Inst.Init(CNotiManager.MakeParams(new Dictionary<CNotiManager.ECallback, System.Action<CNotiManager, bool>>()
				{
					[CNotiManager.ECallback.INIT] = CLateSetupSceneManager.OnInitNotiManager
				}));
#endif // #if NOTI_MODULE_ENABLE

#if PLAYFAB_MODULE_ENABLE
				CPlayfabManager.Inst.Init(CPlayfabManager.MakeParams(new Dictionary<CPlayfabManager.ECallback, System.Action<CPlayfabManager, bool>>() {
					[CPlayfabManager.ECallback.INIT] = CLateSetupSceneManager.OnInitPlayfabManager
				}));
#endif // #if PLAYFAB_MODULE_ENABLE
			}

			this.Setup();
			this.ApplyUserPermissions();
		}

		/** 서비스 관리자가 초기화되었을 경우 */
		private static void OnInitServicesManager(CServicesManager a_oSender, bool a_bIsSuccess)
		{
			CFunc.ShowLog($"CLateSetupSceneManager.OnInitServicesManager: {a_bIsSuccess}");

			// 초기화되었을 경우
			if(a_bIsSuccess)
			{
#if ENABLE_LOGIN_APPLE && UNITY_IOS
				CServicesManager.Inst.UpdateAppleLoginState(CLateSetupSceneManager.OnUpdateAppleLoginState);
#endif // #if ENABLE_LOGIN_APPLE && UNITY_IOS
			}
		}

		/** 다음 씬을 로드한다 */
		private void LoadNextScene()
		{
			CFunc.BroadcastMsg(KCDefine.SS_FUNC_N_SETUP_SCENE_EVENT,
				global::SetupScene.CSetupSceneManager.ESetupSceneEvent.LOAD_NEXT_SCENE, false);

			CSceneManager.SetEnableLateSetup(true);
			CCommonAppInfoStorage.Inst.SetupStoreVer();

#if RESEARCH_MODULE_ENABLE && SCENE_TEMPLATES_MODULE_ENABLE
			// 기본 씬 일 경우
			if(KCDefine.B_DEF_SCENE_NAME_LIST_02.Contains(CSceneLoader.Inst.AwakeActiveSceneName))
			{
				CSceneLoader.Inst.LoadSceneAsync(KCDefine.B_SCENE_N_MENU, this.OnChangeAsyncSceneLoadingPercent);
			}
			else
			{
				CSceneLoader.Inst.LoadSceneAsync(COptsInfoTable.Inst.InfoOptsEtc.m_bIsEnableSceneTitle ? KCDefine.B_SCENE_N_MENU : CSceneLoader.Inst.AwakeActiveSceneName, this.OnChangeAsyncSceneLoadingPercent);
			}
#elif EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			// 기본 씬 일 경우
			if(KCDefine.B_DEF_SCENE_NAME_LIST_01.Contains(CSceneLoader.Inst.AwakeActiveSceneName)) {
				CSceneLoader.Inst.LoadSceneAsync(KCDefine.B_SCENE_N_TITLE, this.OnChangeAsyncSceneLoadingPercent);
			} else {
				CSceneLoader.Inst.LoadSceneAsync(COptsInfoTable.Inst.InfoOptsEtc.m_bIsEnableSceneTitle ? KCDefine.B_SCENE_N_TITLE : CSceneLoader.Inst.AwakeActiveSceneName, this.OnChangeAsyncSceneLoadingPercent);
			}
#endif // #if RESEARCH_MODULE_ENABLE && SCENE_TEMPLATES_MODULE_ENABLE
		}

		/** 유저 권한을 적용한다 */
		private void ApplyUserPermissions()
		{
#if !ROBO_TEST_ENABLE && UNITY_ANDROID
			// 유저 권한이 필요 할 경우
			if(this.UserPermissionList.ExIsValid()) {
				this.RequestUserPermission(this.UserPermissionList[KCDefine.B_VAL_0_INT], this.OnReceiveRequestUserPermissionResult);
			} else {
				this.LoadNextScene();
			}
#else
			this.LoadNextScene();
#endif // #if !ROBO_TEST_ENABLE && UNITY_ANDROID
		}

		/** 추적 동의 뷰를 출력한다 */
		private void ShowTrackingConsentView()
		{
#if UNITY_IOS
			ATTrackingStatusBinding.RequestAuthorizationTracking();

			this.ExRepeatCallFunc((a_oSender, a_bIsComplete) => {
				// 완료되었을 경우
				if(a_bIsComplete) {
					var eStatus = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
					this.OnCloseTrackingConsentView(eStatus != ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED && eStatus != ATTrackingStatusBinding.AuthorizationTrackingStatus.DENIED);
				}
				
				return ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED;
			}, KCDefine.U_DELAY_INIT, KCDefine.B_MAX_DELTA_T_TRACKING_CONSENT_VIEW);
#else
			this.ExLateCallFunc((a_oSender) => this.OnCloseTrackingConsentView(true));
#endif // #if UNITY_IOS
		}
		#endregion // 함수

		#region 접근자 클래스 함수
		/** 로그 데이터를 변경한다 */
		public static void SetLogDataDict(Dictionary<string, object> a_oDataDict)
		{
			CLateSetupSceneManager.LogDataDict = a_oDataDict;
		}
		#endregion         // 접근자 클래스 함수

		#region 조건부 함수
#if UNITY_ANDROID
		/** 유저 권한 요청 결과를 수신했을 경우 */
		private void OnReceiveRequestUserPermissionResult(string a_oPermission, bool a_bIsSuccess) {
			this.UserPermissionList.ExRemoveVal(a_oPermission);
			this.ExLateCallFunc((a_oSender) => this.ApplyUserPermissions());
		}
#endif // #if UNITY_ANDROID
		#endregion // 조건부 함수

		#region 조건부 클래스 함수
#if ENABLE_LOGIN_APPLE && UNITY_IOS
		/** 애플 로그인 상태가 갱신되었을 경우 */
		private static void OnUpdateAppleLoginState(CServicesManager a_oSender, bool a_bIsSuccess) {
			CFunc.ShowLog($"CLateSetupSceneManager.OnUpdateAppleLoginState: {a_bIsSuccess}");
		}
#endif // #if ENABLE_LOGIN_APPLE && UNITY_IOS

#if ADS_MODULE_ENABLE
		/** 광고 관리자가 초기화되었을 경우 */
		private static void OnInitAdsManager(CAdsManager a_oSender, EAdsPlatform a_eAdsPlatform, bool a_bIsSuccess) {
			CFunc.ShowLog($"CLateSetupSceneManager.OnInitAdsManager: {a_eAdsPlatform}, {a_bIsSuccess}");

			// 초기화에 실패했을 경우
			if(!a_bIsSuccess) {
				return;
			}

			bool bIsEnableLoadBannerAds = !CLateSetupSceneManager.IsPurchaseRemoveAds &&
				CPluginInfoTable.Inst.GetBannerAdsID(a_eAdsPlatform).ExIsValid() && CPluginInfoTable.Inst.AdsPlatform == a_eAdsPlatform;

			bool bIsEnableLoadRewardAds = CPluginInfoTable.Inst.GetRewardAdsID(a_eAdsPlatform).ExIsValid();

			bool bIsEnableLoadFullscreenAds = !CLateSetupSceneManager.IsPurchaseRemoveAds &&
				CPluginInfoTable.Inst.GetFullscreenAdsID(a_eAdsPlatform).ExIsValid();

			// 배너 광고 로드가 가능 할 경우
			if(bIsEnableLoadBannerAds && CLateSetupSceneManager.IsAutoLoadBannerAds) {
				CAdsManager.Inst.LoadBannerAds(a_eAdsPlatform);
			}

			// 보상 광고 로드가 가능 할 경우
			if(bIsEnableLoadRewardAds && CLateSetupSceneManager.IsAutoLoadRewardAds) {
				CAdsManager.Inst.LoadRewardAds(a_eAdsPlatform);
			}

			// 전면 광고 로드가 가능 할 경우
			if(bIsEnableLoadFullscreenAds && CLateSetupSceneManager.IsAutoLoadFullscreenAds) {
				CAdsManager.Inst.LoadFullscreenAds(a_eAdsPlatform);
			}
		}
#endif // #if ADS_MODULE_ENABLE

#if FLURRY_MODULE_ENABLE
		/** 플러리 관리자가 초기화되었을 경우 */
		private static void OnInitFlurryManager(CFlurryManager a_oSender, bool a_bIsSuccess) {
			CFunc.ShowLog($"CLateSetupSceneManager.OnInitFlurryManager: {a_bIsSuccess}");

			// 초기화되었을 경우
			if(a_bIsSuccess) {
				CFlurryManager.Inst.SetAnalyticsUserID(CCommonAppInfoStorage.Inst.AppInfo.DeviceID);

				// 약관 동의 팝업이 닫혔을 경우
				if(CCommonAppInfoStorage.Inst.IsCloseAgreePopup) {
					CFlurryManager.Inst.SendLog(KCDefine.G_LOG_N_AGREE, CLateSetupSceneManager.LogDataDict.ExToTypes<string, object, string, string>());
				}
			}
		}
#endif // #if FLURRY_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
		/** 페이스 북 관리자가 초기화되었을 경우 */
		private static void OnInitFacebookManager(CFacebookManager a_oSender, bool a_bIsSuccess) {
			CFunc.ShowLog($"CLateSetupSceneManager.OnInitFacebookManager: {a_bIsSuccess}");
		}
#endif // #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
		/** 파이어 베이스 관리자가 초기화되었을 경우 */
		private static void OnInitFirebaseManager(CFirebaseManager a_oSender, bool a_bIsSuccess) {
			CFunc.ShowLog($"CLateSetupSceneManager.OnInitFirebaseManager: {a_bIsSuccess}");

			// 초기화되었을 경우
			if(a_bIsSuccess) {
				CFirebaseManager.Inst.SetAnalyticsUserID(CCommonAppInfoStorage.Inst.AppInfo.DeviceID);
				CFirebaseManager.Inst.SetCrashlyticsUserID(CCommonAppInfoStorage.Inst.AppInfo.DeviceID);

				CFirebaseManager.Inst.SetCrashlyticsDatas(new Dictionary<string, string>() {
					[KCDefine.G_LOG_KEY_COUNTRY_CODE] = CCommonAppInfoStorage.Inst.CountryCode
				});

				CFirebaseManager.Inst.LoadMsgToken(CLateSetupSceneManager.OnLoadFirebaseMsgToken);

				// 약관 동의 팝업이 닫혔을 경우
				if(CCommonAppInfoStorage.Inst.IsCloseAgreePopup) {
					CFirebaseManager.Inst.SendLog(KCDefine.G_LOG_N_AGREE, CLateSetupSceneManager.LogDataDict.ExToTypes<string, object, string, string>());
				}
			}
		}

		/** 파이어 베이스 메세지 토큰을 수신했을 경우 */
		private static void OnLoadFirebaseMsgToken(CFirebaseManager a_oSender, string a_oMsgToken, bool a_bIsSuccess) {
			CFunc.ShowLog($"CLateSetupSceneManager.OnLoadFirebaseMsgToken: {a_oMsgToken}, {a_bIsSuccess}");
		}

		/** 파이어 베이스 기본 속성을 설정했을 경우 */
		private static void OnSetupFirebaseDefConfigs(CFirebaseManager a_oSender, bool a_bIsSuccess) {
			CFunc.ShowLog($"CLateSetupSceneManager.OnSetupFirebaseDefConfigs: {a_bIsSuccess}");
		}
#endif // #if FIREBASE_MODULE_ENABLE

#if APPS_FLYER_MODULE_ENABLE
		/** 앱스 플라이어 관리자가 초기화되었을 경우 */
		private static void OnInitAppsFlyerManager(CAppsFlyerManager a_oSender, bool a_bIsSuccess) {
			CFunc.ShowLog($"CLateSetupSceneManager.OnInitAppsFlyerManager: {a_bIsSuccess}");
			
			// 초기화되었을 경우
			if(a_bIsSuccess) {
				CAppsFlyerManager.Inst.SetAnalyticsUserID(CCommonAppInfoStorage.Inst.AppInfo.DeviceID);

				// 약관 동의 팝업이 닫혔을 경우
				if(CCommonAppInfoStorage.Inst.IsCloseAgreePopup) {
					CAppsFlyerManager.Inst.SendLog(KCDefine.G_LOG_N_AGREE, CLateSetupSceneManager.LogDataDict.ExToTypes<string, object, string, string>());
				}
			}
		}
#endif // #if APPS_FLYER_MODULE_ENABLE

#if GAME_CENTER_MODULE_ENABLE
		/** 게임 센터 관리자가 초기화되었을 경우 */
		private static void OnInitGameCenterManager(CGameCenterManager a_oSender, bool a_bIsSuccess) {
			CFunc.ShowLog($"CLateSetupSceneManager.OnInitGameCenterManager: {a_bIsSuccess}");
		}
#endif // #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
		/** 결제 관리자가 초기화되었을 경우 */
		private static void OnInitPurchaseManager(CPurchaseManager a_oSender, bool a_bIsSuccess)
		{
			CFunc.ShowLog($"CLateSetupSceneManager.OnInitPurchaseManager: {a_bIsSuccess}");
		}
#endif // #if PURCHASE_MODULE_ENABLE

#if NOTI_MODULE_ENABLE
		/** 알림 관리자가 초기화되었을 경우 */
		private static void OnInitNotiManager(CNotiManager a_oSender, bool a_bIsSuccess)
		{
			CFunc.ShowLog($"CLateSetupSceneManager.OnInitNotiManager: {a_bIsSuccess}");
		}
#endif // #if NOTI_MODULE_ENABLE

#if PLAYFAB_MODULE_ENABLE
		/** 플레이 팹 관리자가 초기화되었을 경우 */
		private static void OnInitPlayfabManager(CPlayfabManager a_oSender, bool a_bIsSuccess) {
			CFunc.ShowLog($"CLateSetupSceneManager.OnInitPlayfabManager: {a_bIsSuccess}");

			// 초기화되었을 경우
			if(a_bIsSuccess && CCommonAppInfoStorage.Inst.IsCloseAgreePopup) {
				CPlayfabManager.Inst.SendLog(KCDefine.G_LOG_N_AGREE, CLateSetupSceneManager.LogDataDict);
			}
		}
#endif // #if PLAYFAB_MODULE_ENABLE
		#endregion // 조건부 클래스 함수

		#region 조건부 접근자 클래스 함수
#if ADS_MODULE_ENABLE
		/** 광고 제거 결제 여부를 변경한다 */
		public static void SetIsPurchaseRemoveAds(bool a_bIsPurchase) {
			CLateSetupSceneManager.IsPurchaseRemoveAds = a_bIsPurchase;
		}

		/** 배너 광고 자동 로드 여부를 변경한다 */
		public static void SetIsAutoLoadBannerAds(bool a_bIsAutoLoad) {
			CLateSetupSceneManager.IsAutoLoadBannerAds = a_bIsAutoLoad;
		}

		/** 전면 광고 자동 로드 여부를 변경한다 */
		public static void SetIsAutoLoadFullscreenAds(bool a_bIsAutoLoad) {
			CLateSetupSceneManager.IsAutoLoadFullscreenAds = a_bIsAutoLoad;
		}

		/** 보상 광고 자동 로드 여부를 변경한다 */
		public static void SetIsAutoLoadRewardAds(bool a_bIsAutoLoad) {
			CLateSetupSceneManager.IsAutoLoadRewardAds = a_bIsAutoLoad;
		}
#endif // #if ADS_MODULE_ENABLE
		#endregion // 조건부 접근자 클래스 함수
	}

	/** 지연 설정 씬 관리자 - 코루틴 */
	public abstract partial class CLateSetupSceneManager : CSceneManager
	{
		#region 함수
		/** 초기화 */
		private IEnumerator CoStart()
		{
			yield return CAccess.CoGetWaitForSecs(KCDefine.U_DELAY_INIT);
			this.SetupActiveScene();

#if ROBO_TEST_ENABLE
			this.OnCloseAgreePopup(null);
#else
			// 약관 동의 상태 일 경우
			if(CCommonAppInfoStorage.Inst.AppInfo.IsAgree || !CCommonAppInfoStorage.Inst.IsNeedsAgree(CCommonAppInfoStorage.Inst.CountryCode))
			{
				this.OnCloseAgreePopup(null);
			}
			else
			{
				// 한국 일 경우
				if(CCommonAppInfoStorage.Inst.CountryCode.Equals(KCDefine.B_KOREA_COUNTRY_CODE))
				{
					var oPrivacy = CResManager.Inst.GetRes<TextAsset>(KCDefine.LSS_DATA_P_PRIVACY);
					var oServices = CResManager.Inst.GetRes<TextAsset>(KCDefine.LSS_DATA_P_SERVICES);

					try
					{
						this.ShowKRAgreePopup(oPrivacy.text, oServices.text, this.OnCloseAgreePopup);
					}
					finally
					{
						CResManager.Inst.RemoveRes<TextAsset>(KCDefine.LSS_DATA_P_PRIVACY, true);
						CResManager.Inst.RemoveRes<TextAsset>(KCDefine.LSS_DATA_P_SERVICES, true);
					}
				}
				else
				{
					this.ShowEUAgreePopup(CProjInfoTable.Inst.CompanyInfo.m_oPrivacyURL, CProjInfoTable.Inst.CompanyInfo.m_oServicesURL, this.OnCloseAgreePopup);
				}
			}
#endif // #if ROBO_TEST_ENABLE
		}
		#endregion // 함수
	}
}
