using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif			// #if PURCHASE_MODULE_ENABLE

//! 기본 접근자
public static partial class Access {
	#region 클래스 프로퍼티
	public static float BannerAdsHeight {
		get {
#if ADS_MODULE_ENABLE
			// 디바이스 타입이 유효 할 경우
			if(CCommonAppInfoStorage.Inst.DeviceType.ExIsValid()) {
				var stBannerAdsSize = (CCommonAppInfoStorage.Inst.DeviceType == EDeviceType.PHONE) ? KCDefine.U_SIZE_PHONE_BANNER_ADS : KCDefine.U_SIZE_TABLET_BANNER_ADS;
				return CAccess.GetBannerAdsHeight(stBannerAdsSize.y);
			}

			return KCDefine.B_VAL_0_FLT;
#else
			return KCDefine.B_VAL_0_FLT;
#endif			// #if ADS_MODULE_ENABLE
		}
	}
	#endregion			// 클래스 프로퍼티
	
	#region 조건부 클래스 함수
#if PURCHASE_MODULE_ENABLE
	//! 판매 상품 식별자를 반환한다
	public static int GetSaleProductID(ESaleProductKinds a_eSaleProductKinds) {
		int nIdx = KDefine.G_KINDS_SALE_PIT_SALE_PRODUCTS.ExFindVal((a_eCompareSaleProductKinds) => a_eSaleProductKinds == a_eCompareSaleProductKinds);
		CAccess.Assert(KDefine.G_KINDS_SALE_PIT_SALE_PRODUCTS.ExIsValidIdx(nIdx));

		return nIdx;
	}
	
	//! 가격 문자열을 반환한다
	public static string GetPriceStr(int a_nID) {
		var oProduct = Access.GetProduct(a_nID);
		return (oProduct != null) ? CAccess.GetPriceStr(oProduct) : string.Empty;
	}

	//! 상품을 반환한다
	public static Product GetProduct(int a_nID) {
		bool bIsValid = CProductInfoTable.Inst.TryGetProductInfo(a_nID, out STProductInfo stProductInfo);
		CAccess.Assert(bIsValid);
		
		return CPurchaseManager.Inst.GetProduct(stProductInfo.m_oID);
	}
#endif			// #if PURCHASE_MODULE_ENABLE
	#endregion			// 조건부 클래스 함수

	#region 추가 클래스 변수

	#endregion			// 추가 클래스 변수

	#region 추가 클래스 프로퍼티

	#endregion			// 추가 클래스 프로퍼티

	#region 추가 클래스 함수

	#endregion			// 추가 클래스 함수
}
#endif			// #if NEVER_USE_THIS
