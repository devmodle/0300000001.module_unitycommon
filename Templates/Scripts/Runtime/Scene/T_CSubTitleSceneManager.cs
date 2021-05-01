using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 서브 타이틀 씬 관리자
public partial class CSubTitleSceneManager : CTitleSceneManager {
	#region UI 변수
	private Text m_oVerText = null;
	#endregion			// UI 변수

	#region 프로퍼티
	public override bool IsRealtimeFadeOutAni => CCommonAppInfoStorage.Inst.AppInfo.IsFirstPlay;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		
		//! 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			m_oVerText = CFactory.CreateCloneObj<Text>(KCDefine.TS_OBJ_N_VER_TEXT, KCDefine.TS_OBJ_P_VER_TEXT, this.SubUpUIs, KCDefine.TS_POS_VER_TEXT);
			m_oVerText.rectTransform.pivot = KCDefine.B_ANCHOR_UP_LEFT;
			m_oVerText.rectTransform.anchorMin = KCDefine.B_ANCHOR_UP_LEFT;
			m_oVerText.rectTransform.anchorMax = KCDefine.B_ANCHOR_UP_LEFT;
		}
	}
	
	//! 초기화
	public override void Start() {
		base.Start();
		this.UpdateUIsState();

		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			CSceneLoader.Inst.LoadAdditiveScene(KCDefine.B_SCENE_N_OVERLAY);
			m_oVerText.text = CAccess.GetVerStr(CProjInfoTable.Inst.ProjInfo.m_stBuildVer.m_oVer, CCommonUserInfoStorage.Inst.UserInfo.UserType);

			// 최초 시작 일 경우
			if(CCommonAppInfoStorage.Inst.IsFirstStart) {
				LogFunc.SendLaunchLog();
				LogFunc.SendSplashLog();

				CCommonAppInfoStorage.Inst.IsFirstStart = true;
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
				
#if DAILY_MISSION_ENABLE
				// 일일 미션 리셋이 가능 할 경우
				if(CGameInfoStorage.Inst.IsEnableResetDailyMission) {
					CGameInfoStorage.Inst.GameInfo.LastDailyMissionTime = System.DateTime.Today;
					CGameInfoStorage.Inst.GameInfo.m_oCompleteDailyMissionKindsList.Clear();

					CGameInfoStorage.Inst.SaveGameInfo();
				}
#endif			// #if DAILY_MISSION_ENABLE

#if FREE_REWARD_ENABLE
				// 무료 보상 획득이 가능 할 경우
				if(CGameInfoStorage.Inst.IsEnableGetFreeReward) {
					CGameInfoStorage.Inst.GameInfo.FreeRewardTimes = KCDefine.B_VAL_0_INT;
					CGameInfoStorage.Inst.GameInfo.LastFreeRewardTime = System.DateTime.Today;
					
					CGameInfoStorage.Inst.SaveGameInfo();
				}
#endif			// #if FREE_REWARD_ENABLE

#if DAILY_REWARD_ENABLE
				// 일일 보상 획득이 가능 할 경우
				if(CGameInfoStorage.Inst.IsEnableGetDailyReward) {
					Func.ShowDailyRewardPopup(this.SubPopupUIs, (a_oPopup) => {
						var oDailyRewardPopup = a_oPopup as CDailyRewardPopup;
						oDailyRewardPopup.Init();
					});
				}
#endif			// #if DAILY_REWARD_ENABLE
			}
		}
	}

	//! UI 상태를 갱신한다
	private void UpdateUIsState() {
		// Do Nothing
	}

	//! 업데이트 팝업 결과를 수신했을 경우
	private void OnReceiveUpdatePopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
		// 확인 버튼을 눌렀을 경우
		if(a_bIsOK) {
			CFunc.OpenURL(CProjInfoTable.Inst.ProjInfo.m_oStoreURL);
		}
	}

	//! 최초 플레이 상태를 처리한다
	private void HandleFirstPlayState() {
		// 약관 동의 팝업이 닫혔을 경우
		if(CAppInfoStorage.Inst.IsCloseAgreePopup) {
			LogFunc.SendAgreeLog();
		}
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS