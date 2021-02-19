using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 서브 약관 동의 씬 관리자
public class CSubAgreeSceneManager : CAgreeSceneManager {
	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			Func.SetupLocalizeStrings();
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

	//! 약관 동의 팝업이 닫혔을 경우
	private void OnCloseAgreePopup(CPopup a_oSender) {
		this.LoadNextScene();
	}

	//! 약관 동의 팝업을 출력한다
	private void ShowAgreePopup(string a_oServices, string a_oPrivacy, EAgreePopupType a_ePopupType) {
#if MODE_PORTRAIT_ENABLE
		string oObjPath = KCDefine.AS_OBJ_P_PORTRAIT_AGREE_POPUP;
#else
		string oObjPath = KCDefine.AS_OBJ_P_LANDSCAPE_AGREE_POPUP;
#endif			// #if MODE_PORTRAIT_ENABLE

		var oAgreePopup = CPopup.Create<CAgreePopup>(KCDefine.AS_OBJ_N_AGREE_POPUP, oObjPath, this.SubPopupUIs, KCDefine.B_POS_POPUP);
		oAgreePopup.Init(a_oServices, a_oPrivacy, a_ePopupType);
		oAgreePopup.Show(null, this.OnCloseAgreePopup);
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
