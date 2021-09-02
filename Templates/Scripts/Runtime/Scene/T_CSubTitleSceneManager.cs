using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 서브 타이틀 씬 관리자
public partial class CSubTitleSceneManager : CTitleSceneManager {
	#region 변수
	// =====> UI <=====
	private Text m_oVerText = null;
	#endregion			// 변수
	
	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		
		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			this.SetupAwake();
		}
	}
	
	//! 초기화
	public override void Start() {
		base.Start();

		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			this.SetupStart();
			this.UpdateUIsState();

			CSceneLoader.Inst.LoadAdditiveScene(KCDefine.B_SCENE_N_OVERLAY);

			// 최초 시작 일 경우
			if(CCommonAppInfoStorage.Inst.IsFirstStart) {
				LogFunc.SendLaunchLog();
				LogFunc.SendSplashLog();
				
				CCommonAppInfoStorage.Inst.IsFirstStart = false;

#if UNITY_STANDALONE && EDITOR_ENABLE
				// 독립 플랫폼 일 경우
				if(CAccess.IsStandalone) {
					CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_LEVEL_EDITOR);
				}
#endif			// #if UNITY_STANDALONE && EDITOR_ENABLE
			}

			// 최초 플레이 일 경우
			if(CCommonAppInfoStorage.Inst.AppInfo.IsFirstPlay) {
				CCommonAppInfoStorage.Inst.AppInfo.IsFirstPlay = false;
				CCommonAppInfoStorage.Inst.SaveAppInfo();

				this.UpdateFirstPlayState();
			} else {
				// 업데이트가 필요 할 경우
				if(!CAppInfoStorage.Inst.IsIgnoreUpdate && CCommonAppInfoStorage.Inst.IsNeedUpdate()) {
					CAppInfoStorage.Inst.IsIgnoreUpdate = true;
					this.ExLateCallFunc((a_oSender, a_oParams) => Func.ShowUpdatePopup(this.OnReceiveUpdatePopupResult));
				}
				
#if DAILY_MISSION_ENABLE
				// 일일 미션 리셋이 가능 할 경우
				if(CGameInfoStorage.Inst.IsEnableResetDailyMission) {
					CGameInfoStorage.Inst.GameInfo.PrevDailyMissionTime = System.DateTime.Today;
					CGameInfoStorage.Inst.GameInfo.m_oCompleteDailyMissionKindsList.Clear();

					CGameInfoStorage.Inst.SaveGameInfo();
				}
#endif			// #if DAILY_MISSION_ENABLE

#if FREE_REWARD_ENABLE
				// 무료 보상 획득이 가능 할 경우
				if(CGameInfoStorage.Inst.IsEnableGetFreeReward) {
					CGameInfoStorage.Inst.GameInfo.NumAcquireFreeRewards = KCDefine.B_VAL_0_INT;
					CGameInfoStorage.Inst.GameInfo.PrevFreeRewardTime = System.DateTime.Today;
					
					CGameInfoStorage.Inst.SaveGameInfo();
				}
#endif			// #if FREE_REWARD_ENABLE

#if DAILY_REWARD_ENABLE
				// 일일 보상 획득이 가능 할 경우
				if(CGameInfoStorage.Inst.IsEnableGetDailyReward) {
					Func.ShowDailyRewardPopup(this.SubPopupUIs, (a_oSender) => {
						var oDailyRewardPopup = a_oSender as CDailyRewardPopup;
						oDailyRewardPopup.Init();
					});
				}
#endif			// #if DAILY_REWARD_ENABLE
			}
		}
	}

	//! 제거 되었을 경우
	public override void OnDestroy() {
		base.OnDestroy();

		// 앱이 실행 중 일 경우
		if(CSceneManager.IsAwake || CSceneManager.IsAppRunning) {
			// Do Something
		}
	}

	//! 앱이 정지 되었을 경우
	public virtual void OnApplicationPause(bool a_bIsPause) {
		// 재개 되었을 경우
		if(!a_bIsPause && (CSceneManager.IsAwake || CSceneManager.IsAppRunning)) {
#if ADS_MODULE_ENABLE
			// 광고 출력이 가능 할 경우
			if(CAppInfoStorage.Inst.IsEnableShowFullscreenAds && CAdsManager.Inst.IsLoadFullscreenAds(CPluginInfoTable.Inst.DefAdsType)) {
				Func.ShowFullscreenAds(null);
			}
#endif			// #if ADS_MODULE_ENABLE
		}
	}

	//! 내비게이션 스택 이벤트를 수신했을 경우
	public override void OnReceiveNavStackEvent(ENavStackEvent a_eEvent) {
		base.OnReceiveNavStackEvent(a_eEvent);

		// 백 키 눌림 이벤트 일 경우
		if(a_eEvent == ENavStackEvent.BACK_KEY_DOWN) {
			Func.ShowQuitPopup(this.OnReceiveQuitPopupResult);
		}
	}

	//! 씬을 설정한다
	private void SetupAwake() {
		// 텍스트를 설정한다 {
		var oVerText = this.SubUIs.ExFindComponent<Text>(KCDefine.TS_OBJ_N_VER_TEXT);

		m_oVerText = oVerText ?? CFactory.CreateCloneObj<Text>(KCDefine.TS_OBJ_N_VER_TEXT, KCDefine.TS_OBJ_P_VER_TEXT, this.SubUpUIs, KCDefine.TS_POS_VER_TEXT);
		m_oVerText.rectTransform.pivot = KCDefine.B_ANCHOR_UP_LEFT;
		m_oVerText.rectTransform.anchorMin = KCDefine.B_ANCHOR_UP_LEFT;
		m_oVerText.rectTransform.anchorMax = KCDefine.B_ANCHOR_UP_LEFT;
		// 텍스트를 설정한다 }

#if DEBUG || DEVELOPMENT_BUILD
		this.SetupTestUIs();
#endif			// #if DEBUG || DEVELOPMENT_BUILD
	}

	//! 씬을 설정한다
	private void SetupStart() {
		// Do Something
	}

	//! UI 상태를 갱신한다
	private void UpdateUIsState() {
		// 텍스트를 갱신한다
		m_oVerText.text = CAccess.GetVerStr(CProjInfoTable.Inst.ProjInfo.m_stBuildVer.m_oVer, CCommonUserInfoStorage.Inst.UserInfo.UserType);

#if DEBUG || DEVELOPMENT_BUILD
		this.UpdateTestUIsState();
#endif			// #if DEBUG || DEVELOPMENT_BUILD
	}

	//! 최초 플레이 상태를 갱신한다
	private void UpdateFirstPlayState() {
		// 약관 동의 팝업이 닫혔을 경우
		if(CAppInfoStorage.Inst.IsCloseAgreePopup) {
			LogFunc.SendAgreeLog();
		}
	}

	//! 서비스 관리자가 초기화 되었을 경우
	private void OnInitServicesManager(object a_oParams) {
		// Do Something
	}

	//! 종료 팝업 결과를 수신했을 경우
	private void OnReceiveQuitPopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
		// 확인 버튼을 눌렀을 경우
		if(a_bIsOK) {
			this.ExLateCallFunc((a_oSender, a_oParams) => this.QuitApp());
		}
	}

	//! 업데이트 팝업 결과를 수신했을 경우
	private void OnReceiveUpdatePopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
		// 확인 버튼을 눌렀을 경우
		if(a_bIsOK) {
			Application.OpenURL(CProjInfoTable.Inst.ProjInfo.m_oStoreURL);
		}
	}
	#endregion			// 함수

	#region 조건부 함수
#if DEBUG || DEVELOPMENT_BUILD
	//! 테스트 UI 를 설정한다
	private void SetupTestUIs() {
		// Do Something
	}

	//! 테스트 UI 상태를 갱신한다
	private void UpdateTestUIsState() {
		// Do Something
	}
#endif			// #if DEBUG || DEVELOPMENT_BUILD

#if ADS_MODULE_ENABLE
	//! 광고 관리자가 초기화 되었을 경우
	private void OnInitAdsManager(object a_oParams) {
		// Do Something
	}
#endif			// #if ADS_MODULE_ENABLE

#if FLURRY_MODULE_ENABLE
	//! 플러리 관리자가 초기화 되었을 경우
	private void OnInitFlurryManager(object a_oParams) {
		// Do Something
	}
#endif			// #if FLURRY_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
	//! 페이스 북 관리자가 초기화 되었을 경우
	private void OnInitFacebookManager(object a_oParams) {
		// Do Something
	}
#endif			// #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
	//! 파이어 베이스 관리자가 초기화 되었을 경우
	private void OnInitFirebaseManager(object a_oParams) {
		// Do Something
	}
#endif			// #if FIREBASE_MODULE_ENABLE

#if APPS_FLYER_MODULE_ENABLE
	//! 앱스 플라이어 관리자가 초기화 되었을 경우
	private void OnInitAppsFlyerManager(object a_oParams) {
		// Do Something
	}
#endif			// #if APPS_FLYER_MODULE_ENABLE

#if GAME_ANALYTICS_MODULE_ENABLE
	//! 게임 분석 관리자가 초기화 되었을 경우
	private void OnInitGameAnalyticsManager(object a_oParams) {
		// Do Something
	}
#endif			// #if GAME_ANALYTICS_MODULE_ENABLE

#if SINGULAR_MODULE_ENABLE
	//! 싱귤러 관리자가 초기화 되었을 경우
	private void OnInitSingularManager(object a_oParams) {
		// Do Something
	}
#endif			// #if SINGULAR_MODULE_ENABLE

#if GAME_CENTER_MODULE_ENABLE
	//! 게임 센터 관리자가 초기화 되었을 경우
	private void OnInitGameCenterManager(object a_oParams) {
		// Do Something
	}
#endif			// #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	//! 결제 관리자가 초기화 되었을 경우
	private void OnInitPurchaseManager(object a_oParams) {
		// Do Something
	}
#endif			// #if PURCHASE_MODULE_ENABLE

#if NOTI_MODULE_ENABLE
	//! 알림 관리자가 초기화 되었을 경우
	private void OnInitNotiManager(object a_oParams) {
		// Do Something
	}
#endif			// #if NOTI_MODULE_ENABLE
	#endregion			// 조건부 함수

	#region 추가 함수
#if DEBUG || DEVELOPMENT_BUILD

#endif			// #if DEBUG || DEVELOPMENT_BUILD
	#endregion			// 추가 함수
}
#endif			// #if NEVER_USE_THIS
