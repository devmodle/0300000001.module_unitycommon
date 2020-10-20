using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 기본 접근자
public static partial class Access {
	#region 클래스 함수
	//! 배너 광고 높이를 반환한다
	public static float GetBannerAdsHeight() {
		// 디바이스 타입이 유효 할 경우
		if(CCommonAppInfoStorage.Instance.DeviceType.ExIsValid()) {
			var stBannerAdsSize = (CCommonAppInfoStorage.Instance.DeviceType == EDeviceType.PHONE) ? 
				KCDefine.U_SIZE_PHONE_BANNER_ADS : KCDefine.U_SIZE_TABLET_BANNER_ADS;

			return CAccess.GetBannerAdsHeight(stBannerAdsSize.y);
		}

		return KCDefine.B_VALUE_FLOAT_0;
	}
	#endregion			// 클래스 함수
}
#endif			// #if NEVER_USE_THIS
