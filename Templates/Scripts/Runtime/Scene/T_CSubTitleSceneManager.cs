using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 서브 타이틀 씬 관리자
public partial class CSubTitleSceneManager : CTitleSceneManager {
	#region 함수
	//! 초기화
	public override void Start() {
		base.Start();
		var stLastFreeRewardTime = CAppInfoStorage.Inst.AppInfo.LastFreeRewardTime;

		// 업데이트가 필요 할 경우
		if(CCommonAppInfoStorage.Inst.IsNeedUpdate()) {
			Func.ShowUpdatePopup(this.OnReceiveUpdatePopupResult);
		}

		// 무료 코인 리셋 주기가 지났을 경우
		if(System.DateTime.Now.ExGetDeltaTimePerDays(stLastFreeRewardTime).ExIsGreateEquals(KCDefine.B_VALUE_DBL_1)) {
			CAppInfoStorage.Inst.AppInfo.LastFreeRewardTime = System.DateTime.Today;
			CAppInfoStorage.Inst.SaveAppInfo();

			CUserInfoStorage.Inst.UserInfo.FreeRewardTimes = KCDefine.B_VALUE_INT_0;
			CUserInfoStorage.Inst.SaveUserInfo();
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