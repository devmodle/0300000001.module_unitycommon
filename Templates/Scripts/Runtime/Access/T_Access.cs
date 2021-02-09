using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif			// #if PURCHASE_MODULE_ENABLE

//! 기본 접근자
public static partial class Access {
	#region 클래스 함수
	//! 배너 광고 높이를 반환한다
	public static float GetBannerAdsHeight() {
#if ADS_MODULE_ENABLE
		// 디바이스 타입이 유효 할 경우
		if(CCommonAppInfoStorage.Inst.DeviceType.ExIsValid()) {
			var stBannerAdsSize = (CCommonAppInfoStorage.Inst.DeviceType == EDeviceType.PHONE) ? 
				KCDefine.U_SIZE_PHONE_BANNER_ADS : KCDefine.U_SIZE_TABLET_BANNER_ADS;

			return CAccess.GetBannerAdsHeight(stBannerAdsSize.y);
		}

		return KCDefine.B_VALUE_FLT_0;
#else
		return KCDefine.B_VALUE_FLT_0;
#endif			// #if ADS_MODULE_ENABLE
	}
	#endregion			// 클래스 함수

	#region 조건부 클래스 함수
#if PURCHASE_MODULE_ENABLE
	//! 가격 문자열을 반환한다
	public static string GetPriceString(int a_nID) {
		var oProduct = Access.GetProduct(a_nID);
		return (oProduct != null) ? CAccess.GetPriceString(oProduct) : string.Empty;
	}

	//! 상품을 반환한다
	public static Product GetProduct(int a_nID) {
		CAccess.Assert(CProductInfoTable.Inst.TryGetProductInfo(a_nID, out STProductInfo stProductInfo));
		return CPurchaseManager.Inst.GetProduct(stProductInfo.m_oID);
	}
#endif			// #if PURCHASE_MODULE_ENABLE
	#endregion			// 조건부 클래스 함수
}
#endif			// #if NEVER_USE_THIS
