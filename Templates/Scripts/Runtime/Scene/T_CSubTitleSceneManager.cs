using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
#if RUNTIME_TEMPLATES_MODULE_ENABLE
/** 서브 타이틀 씬 관리자 */
public partial class CSubTitleSceneManager : CTitleSceneManager {
	#region 변수
	// =====> UI <=====
	private Button m_oPlayBtn = null;
	#endregion			// 변수

	#region 추가 변수

	#endregion			// 추가 변수

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티
	
	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		
		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			this.SetupAwake();
		}
	}
	
	/** 초기화 */
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

				this.HandleFirstPlayState();
			} else {
				// 업데이트가 필요 할 경우
				if(!CAppInfoStorage.Inst.IsIgnoreUpdate && CCommonAppInfoStorage.Inst.IsNeedUpdate()) {
					CAppInfoStorage.Inst.IsIgnoreUpdate = true;
					this.ExLateCallFunc((a_oSender, a_oParams) => Func.ShowUpdatePopup(this.OnReceiveUpdatePopupResult));
				}

				// 일일 미션 리셋이 가능 할 경우
				if(CGameInfoStorage.Inst.IsEnableResetDailyMission) {
					CGameInfoStorage.Inst.GameInfo.PrevDailyMissionTime = System.DateTime.Today;
					CGameInfoStorage.Inst.GameInfo.m_oCompleteDailyMissionKindsList.Clear();

					CGameInfoStorage.Inst.SaveGameInfo();
				}

				// 무료 보상 획득이 가능 할 경우
				if(CGameInfoStorage.Inst.IsEnableGetFreeReward) {
					CGameInfoStorage.Inst.GameInfo.NumAcquireFreeRewards = KCDefine.B_VAL_0_INT;
					CGameInfoStorage.Inst.GameInfo.PrevFreeRewardTime = System.DateTime.Today;
					
					CGameInfoStorage.Inst.SaveGameInfo();
				}

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

	/** 제거 되었을 경우 */
	public override void OnDestroy() {
		base.OnDestroy();

		// 앱이 실행 중 일 경우
		if(CSceneManager.IsAwake || CSceneManager.IsAppRunning) {
			// Do Something
		}
	}

	/** 앱이 정지 되었을 경우 */
	public virtual void OnApplicationPause(bool a_bIsPause) {
		// 재개 되었을 경우
		if(!a_bIsPause && (CSceneManager.IsAwake || CSceneManager.IsAppRunning)) {
#if ADS_MODULE_ENABLE
			// 광고 출력이 가능 할 경우
			if(CAppInfoStorage.Inst.IsEnableShowFullscreenAds && CAdsManager.Inst.IsLoadFullscreenAds(CPluginInfoTable.Inst.AdsPlatform)) {
				Func.ShowFullscreenAds(null);
			}
#endif			// #if ADS_MODULE_ENABLE
		}
	}

	/** 내비게이션 스택 이벤트를 수신했을 경우 */
	public override void OnReceiveNavStackEvent(ENavStackEvent a_eEvent) {
		base.OnReceiveNavStackEvent(a_eEvent);

		// 백 키 눌림 이벤트 일 경우
		if(a_eEvent == ENavStackEvent.BACK_KEY_DOWN) {
			Func.ShowQuitPopup(this.OnReceiveQuitPopupResult);
		}
	}

	/** 씬을 설정한다 */
	private void SetupAwake() {
		// 버튼을 설정한다
		m_oPlayBtn = this.SubUIs.ExFindComponent<Button>(KCDefine.U_OBJ_N_PLAY_BTN);
		m_oPlayBtn?.ExAddListener(this.OnTouchPlayBtn, true, false);

#if DEBUG || DEVELOPMENT_BUILD
		this.SetupTestUIs();
#endif			// #if DEBUG || DEVELOPMENT_BUILD
	}

	/** 씬을 설정한다 */
	private void SetupStart() {
		// Do Something
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
#if DEBUG || DEVELOPMENT_BUILD
		this.UpdateTestUIsState();
#endif			// #if DEBUG || DEVELOPMENT_BUILD
	}

	/** 종료 팝업 결과를 수신했을 경우 */
	private void OnReceiveQuitPopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
		// 확인 버튼을 눌렀을 경우
		if(a_bIsOK) {
			a_oSender.IsIgnoreAni = true;
			this.ExLateCallFunc((a_oSender, a_oParams) => this.QuitApp());
		}
	}

	/** 업데이트 팝업 결과를 수신했을 경우 */
	private void OnReceiveUpdatePopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
		// 확인 버튼을 눌렀을 경우
		if(a_bIsOK) {
			Application.OpenURL(Access.StoreURL);
		}
	}

	/** 플레이 버튼을 눌렀을 경우 */
	private void OnTouchPlayBtn() {
		CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME);
	}

	/** 최초 플레이 상태를 처리한다 */
	private void HandleFirstPlayState() {
		// 약관 동의 팝업이 닫혔을 경우
		if(CAppInfoStorage.Inst.IsCloseAgreePopup) {
			LogFunc.SendAgreeLog();
		}
	}
	#endregion			// 함수

	#region 조건부 함수
#if DEBUG || DEVELOPMENT_BUILD
	/** 테스트 UI 를 설정한다 */
	private void SetupTestUIs() {
		// Do Something
	}

	/** 테스트 UI 상태를 갱신한다 */
	private void UpdateTestUIsState() {
		// Do Something
	}
#endif			// #if DEBUG || DEVELOPMENT_BUILD
	#endregion			// 조건부 함수

	#region 추가 함수
#if DEBUG || DEVELOPMENT_BUILD

#endif			// #if DEBUG || DEVELOPMENT_BUILD
	#endregion			// 추가 함수
}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if NEVER_USE_THIS
