using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 전역 접근 확장 클래스 */
public static partial class AccessExtension {
	#region 클래스 함수
	/** 가격 정보를 반환한다 */
	public static STPriceInfo ExGetPriceInfo(this List<STPriceInfo> a_oSender, EPriceKinds a_ePriceKinds) {
		bool bIsValid = a_oSender.ExTryGetPriceInfo(a_ePriceKinds, out STPriceInfo stPriceInfo);
		CAccess.Assert(bIsValid);

		return stPriceInfo;
	}

	/** 아이템 개수 정보를 반환한다 */
	public static STNumItemsInfo ExGetNumItemsInfo(this List<STNumItemsInfo> a_oSender, EItemKinds a_eItemKinds) {
		bool bIsValid = a_oSender.ExTryGetNumItemsInfo(a_eItemKinds, out STNumItemsInfo stNumItemsInfo);
		CAccess.Assert(bIsValid);

		return stNumItemsInfo;
	}

	/** 가격 정보를 반환한다 */
	public static bool ExTryGetPriceInfo(this List<STPriceInfo> a_oSender, EPriceKinds a_ePriceKinds, out STPriceInfo a_stOutPriceInfo) {
		int nIdx = a_oSender.FindIndex((a_stPriceInfo) => a_stPriceInfo.m_ePriceKinds == a_ePriceKinds);
		a_stOutPriceInfo = a_oSender.ExIsValidIdx(nIdx) ? a_oSender[nIdx] : default(STPriceInfo);

		return a_oSender.ExIsValidIdx(nIdx);
	}

	/** 아이템 개수 정보를 반환한다 */
	public static bool ExTryGetNumItemsInfo(this List<STNumItemsInfo> a_oSender, EItemKinds a_eItemKinds, out STNumItemsInfo a_stOutNumItemsInfo) {
		int nIdx = a_oSender.FindIndex((a_stNumItemsInfo) => a_stNumItemsInfo.m_eItemKinds == a_eItemKinds);
		a_stOutNumItemsInfo = a_oSender.ExIsValidIdx(nIdx) ? a_oSender[nIdx] : default(STNumItemsInfo);

		return a_oSender.ExIsValidIdx(nIdx);
	}
	#endregion			// 클래스 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
