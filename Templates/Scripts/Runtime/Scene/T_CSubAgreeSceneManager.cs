using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 서브 약관 동의 씬 관리자
public class CSubAgreeSceneManager : CAgreeSceneManager {
	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
#if DEBUG || DEVELOPMENT_BUILD
			CFunc.ShowLog($"Country Code: {CCommonAppInfoStorage.Inst.CountryCode}", KCDefine.B_LOG_COLOR_PLATFORM_INFO);
#endif			// #if DEBUG || DEVELOPMENT_BUILD

			this.SetupAwake();
		}
	}

	//! 일반 약관 동의 팝업을 출력한다
	protected override void ShowNormAgreePopup(string a_oServices, string a_oPrivacy) {
		this.ShowAgreePopup(a_oServices, a_oPrivacy, EAgreePopupType.NORM);
	}

	//! 유럽 연합 약관 동의 팝업을 출력한다
	protected override void ShowEUAgreePopup(string a_oServicesURL, string a_oPrivacyURL) {
		this.ShowAgreePopup(a_oServicesURL, a_oPrivacyURL, EAgreePopupType.EU);
	}

	//! 씬을 설정한다
	private void SetupAwake() {
		Func.SetupLocalizeStrs();
	}

	//! 약관 동의 팝업이 닫혔을 경우
	private void OnCloseAgreePopup(CPopup a_oSender) {
		CAppInfoStorage.Inst.IsCloseAgreePopup = true;
		this.LoadNextScene();
	}

	//! 약관 동의 팝업을 출력한다
	private void ShowAgreePopup(string a_oServices, string a_oPrivacy, EAgreePopupType a_ePopupType) {
		Func.ShowAgreePopup(this.SubPopupUIs, (a_oSender) => {
			var stParams = new CAgreePopup.STParams() {
				m_oServices = a_oServices,
				m_oPrivacy = a_oPrivacy,

				m_ePopupType = a_ePopupType
			};

			var oAgreePopup = a_oSender as CAgreePopup;
			oAgreePopup.Init(stParams);
		}, null, this.OnCloseAgreePopup);
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
