using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 서브 초기화 씬 관리자
public class CSubInitSceneManager : CInitSceneManager {
	#region 함수
	//! 씬을 설정한다
	protected override void Setup() {
		base.Setup();

#if MSG_PACK_ENABLE
		// 저장소를 생성한다
		CAppInfoStorage.Create();
		CUserInfoStorage.Create();
		CGameInfoStorage.Create();
		
#if ANALYTICS_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
		CCommonUserInfoStorage.Instance.UserInfo.UserType = EUserType.NONE;
#else
		// 유저 타입 설정이 필요 할 경우
		if(CCommonUserInfoStorage.Instance.UserInfo.UserType <= EUserType.NONE ||
			CCommonUserInfoStorage.Instance.UserInfo.UserType >= EUserType.MAX_VALUE) {
#if AB_TEST_ENABLE
			var eUserType = (EUserType)Random.Range((int)(EUserType.NONE + 1), (int)EUserType.MAX_VALUE);
			CCommonUserInfoStorage.Instance.UserInfo.UserType = eUserType;
#else
			CCommonUserInfoStorage.Instance.UserInfo.UserType = EUserType.USER_A;
#endif			// #if AB_TEST_ENABLE
		}
#endif			// #if ANALYTICS_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
#endif			// #if MSG_PACK_ENABLE
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
