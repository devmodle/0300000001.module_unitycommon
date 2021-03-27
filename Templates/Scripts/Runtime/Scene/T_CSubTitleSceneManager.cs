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

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		
		//! 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			m_oVerText = CFactory.CreateCloneObj<Text>(KCDefine.TS_OBJ_N_VER_TEXT, KCDefine.TS_OBJ_P_VER_TEXT, this.SubTopUIs, KCDefine.TS_POS_VER_TEXT);
			m_oVerText.rectTransform.pivot = KCDefine.B_ANCHOR_TOP_LEFT;
			m_oVerText.rectTransform.anchorMin = KCDefine.B_ANCHOR_TOP_LEFT;
			m_oVerText.rectTransform.anchorMax = KCDefine.B_ANCHOR_TOP_LEFT;
		}
	}
	
	//! 초기화
	public override void Start() {
		base.Start();

		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			CSceneLoader.Inst.LoadAdditiveScene(KCDefine.B_SCENE_N_OVERLAY);
			m_oVerText.text = CAccess.GetVerStr(CProjInfoTable.Inst.ProjInfo.m_stBuildVer.m_oVer, CCommonUserInfoStorage.Inst.UserInfo.UserType);

			var stLastFreeRewardTime = CGameInfoStorage.Inst.GameInfo.LastFreeRewardTime;
			var stLastDailyRewardTime = CGameInfoStorage.Inst.GameInfo.LastDailyRewardTime;

			// 업데이트가 필요 할 경우
			if(!CAppInfoStorage.Inst.IsIgnoreUpdate && CCommonAppInfoStorage.Inst.IsNeedUpdate()) {
				CAppInfoStorage.Inst.IsIgnoreUpdate = true;
				Func.ShowUpdatePopup(this.OnReceiveUpdatePopupResult);
			}

			// 무료 코인 리셋 주기가 지났을 경우
			if(System.DateTime.Now.ExGetDeltaTimePerDays(stLastFreeRewardTime).ExIsGreateEquals(KCDefine.B_VALUE_1_DBL)) {
				CUserInfoStorage.Inst.UserInfo.FreeRewardTimes = KCDefine.B_VALUE_0_INT;
				CUserInfoStorage.Inst.SaveUserInfo();

				CGameInfoStorage.Inst.GameInfo.LastFreeRewardTime = System.DateTime.Today;
				CGameInfoStorage.Inst.SaveGameInfo();
			}

			// 일일 보상 획득 주기가 지났을 경우
			if(System.DateTime.Now.ExGetDeltaTimePerDays(stLastDailyRewardTime).ExIsGreateEquals(KCDefine.B_VALUE_1_DBL)) {
				Func.ShowDailyRewardPopup(this.SubPopupUIs, (a_oPopup) => {
					var oDailyRewardPopup = a_oPopup as CDailyRewardPopup;
					oDailyRewardPopup.Init();
				});
				
				CGameInfoStorage.Inst.GameInfo.LastDailyRewardTime = System.DateTime.Today;
				CGameInfoStorage.Inst.SaveGameInfo();
			}
		}
	}

	//! 업데이트 팝업 결과를 수신했을 경우
	private void OnReceiveUpdatePopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
		// 확인 버튼을 눌렀을 경우
		if(a_bIsOK) {
			CFunc.OpenURL(CProjInfoTable.Inst.ProjInfo.m_oStoreURL);
		}
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS