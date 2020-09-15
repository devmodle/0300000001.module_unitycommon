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
#if LOCALIZE_TEST_ENABLE
			CCommonAppInfoStorage.Instance.AppInfo.Language = m_eLanguage;
			CCommonAppInfoStorage.Instance.SaveAppInfo();
#else

#endif			// #if LOCALIZE_TEST_ENABLE
		}
	}

	//! 약관 동의 팝업을 출력한다
	protected override void ShowAgreePopup(string a_oServiceString, string a_oPersonalString) {
		this.LoadNextScene();
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
