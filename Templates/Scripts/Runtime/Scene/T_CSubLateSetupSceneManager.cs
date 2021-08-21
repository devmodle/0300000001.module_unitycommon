using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 서브 지연 설정 씬 관리자
public class CSubLateSetupSceneManager : CLateSetupSceneManager {
	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		
		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			this.SetupAwake();
		}
	}

	//! 씬을 설정한다
	protected override void Setup() {
		base.Setup();
		
#if ADS_MODULE_ENABLE
		CAdsManager.Inst.IsEnableBannerAds = !CCommonUserInfoStorage.Inst.UserInfo.IsRemoveAds;
		CAdsManager.Inst.IsEnableFullscreenAds = !CCommonUserInfoStorage.Inst.UserInfo.IsRemoveAds;
#endif			// #if ADS_MODULE_ENABLE
	}

	//! 설명 팝업을 출력한다
	protected override void ShowDescPopup() {
		// 설명 팝업 출력이 가능 할 경우
		if(CCommonAppInfoStorage.Inst.AppInfo.IsEnableShowDescPopup) {
			Func.ShowDescPopup(this.SubPopupUIs, (a_oSender) => {
				var oDescPopup = a_oSender as CDescPopup;
				oDescPopup.Init(this.OnReceiveDescPopupResult);
			});
		} else {
			this.OnReceiveDescPopupResult(null);
		}
	}

	//! 씬을 설정한다
	private void SetupAwake() {
		this.IsAutoInitManager = true;

#if ADS_MODULE_ENABLE
		CSubLateSetupSceneManager.IsAutoLoadBannerAds = true;
		CSubLateSetupSceneManager.IsAutoLoadRewardAds = true;
		CSubLateSetupSceneManager.IsAutoLoadFullscreenAds = true;
#endif			// #if ADS_MODULE_ENABLE

#if ANALYTICS_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
		CCommonUserInfoStorage.Inst.UserInfo.UserType = EUserType.NONE;
#else
		// 유저 타입이 유효하지 않을 경우
		if(!CCommonUserInfoStorage.Inst.UserInfo.UserType.ExIsValid()) {
#if AB_TEST_ENABLE
			int nSumVal = CCommonAppInfoStorage.Inst.AppInfo.DeviceID.ExToSumVal();
			CCommonUserInfoStorage.Inst.UserInfo.UserType = (nSumVal % KCDefine.B_VAL_2_INT != KCDefine.B_VAL_0_INT) ? EUserType.USER_A : EUserType.USER_B;
#else
			CCommonUserInfoStorage.Inst.UserInfo.UserType = EUserType.USER_A;
#endif			// #if AB_TEST_ENABLE
		}
#endif			// #if ANALYTICS_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)

		CCommonAppInfoStorage.Inst.DeviceConfig = CDeviceInfoTable.Inst.DeviceConfig;
		CCommonUserInfoStorage.Inst.SaveUserInfo();
	}

	//! 설명 팝업 결과를 수신했을 경우
	private void OnReceiveDescPopupResult(CDescPopup a_oSender) {
		a_oSender?.Close();
		this.ExLateCallFunc((a_oSender, a_oParams) => this.ShowConsentView(), KCDefine.U_DELAY_INIT);
	}
	#endregion			// 함수

	#region 추가 변수

	#endregion			// 추가 변수
	
	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if NEVER_USE_THIS
