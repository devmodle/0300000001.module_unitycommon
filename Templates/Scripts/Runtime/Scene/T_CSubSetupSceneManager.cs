using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 서브 설정 씬 관리자
public class CSubSetupSceneManager : CSetupSceneManager {
	#region 변수
#if LOCALIZE_TEST_ENABLE
	[SerializeField] private SystemLanguage m_eLanguage = SystemLanguage.Unknown;
#endif			// #if LOCALIZE_TEST_ENABLE
	#endregion			// 변수

	#region 함수
	//! 씬을 설정한다
	protected override void Setup() {
		base.Setup();

		// 저장소를 로드한다
		CAppInfoStorage.Instance.LoadAppInfo();
		CUserInfoStorage.Instance.LoadUserInfo();
		CGameInfoStorage.Instance.LoadGameInfo();

#if LOCALIZE_TEST_ENABLE
		CCommonAppInfoStorage.Instance.AppInfo.Language = m_eLanguage;
#endif			// #if LOCALIZE_TEST_ENABLE

		// 언어가 유효하지 않을 경우
		if(!CCommonAppInfoStorage.Instance.AppInfo.Language.ExIsValid()) {
			CCommonAppInfoStorage.Instance.AppInfo.Language = Application.systemLanguage;
		}

		CCommonAppInfoStorage.Instance.SaveAppInfo();
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
