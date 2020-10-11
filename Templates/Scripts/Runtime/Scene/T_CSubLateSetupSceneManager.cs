using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 서브 지연 설정 씬 관리자
public class CSubLateSetupSceneManager : CLateSetupSceneManager {
	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		
		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			this.IsAutoInitManager = true;

#if ADS_MODULE_ENABLE
			CLateSetupSceneManager.IsAutoLoadAds = true;
#endif			// #if ADS_MODULE_ENABLE

#if ANALYTICS_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
			CCommonUserInfoStorage.Instance.UserInfo.UserType = EUserType.NONE;
#else
			// 유저 타입이 유효하지 않을 경우
			if(!CCommonUserInfoStorage.Instance.UserInfo.UserType.ExIsValid()) {
#if AB_TEST_ENABLE
				int nSumValue = CCommonAppInfoStorage.Instance.AppInfo.DeviceID.ExToSumValue();
			
				CCommonUserInfoStorage.Instance.UserInfo.UserType = (nSumValue % 2 != KCDefine.B_ZERO_VALUE_INT) ? 
					EUserType.USER_A : EUserType.USER_B;
#else
				CCommonUserInfoStorage.Instance.UserInfo.UserType = EUserType.USER_A;
#endif			// #if AB_TEST_ENABLE
			}
#endif			// #if ANALYTICS_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)

			CCommonUserInfoStorage.Instance.SaveUserInfo();
			CCommonAppInfoStorage.Instance.DeviceConfig = CDeviceInfoTable.Instance.DeviceConfig;
		}
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
