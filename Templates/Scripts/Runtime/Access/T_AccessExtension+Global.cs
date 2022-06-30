using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 전역 접근 확장 클래스 */
public static partial class AccessExtension {
	#region 클래스 함수
	/** 가격 정보를 반환한다 */
	public static STPriceInfo ExGetPriceInfo(this List<STPriceInfo> a_oSender, EPriceType a_ePriceType, int a_nKinds) {
		bool bIsValid = a_oSender.ExTryGetPriceInfo(a_ePriceType, a_nKinds, out STPriceInfo stPriceInfo);
		CAccess.Assert(bIsValid);

		return stPriceInfo;
	}

	/** 획득 정보를 반환한다 */
	public static STAcquireInfo ExGetAcquireInfo(this List<STAcquireInfo> a_oSender, EItemKinds a_eItemKinds) {
		bool bIsValid = a_oSender.ExTryGetAcquireInfo(a_eItemKinds, out STAcquireInfo stAcquireInfo);
		CAccess.Assert(bIsValid);

		return stAcquireInfo;
	}

	/** 가격 정보를 반환한다 */
	public static bool ExTryGetPriceInfo(this List<STPriceInfo> a_oSender, EPriceType a_ePriceType, int a_nKinds, out STPriceInfo a_stOutPriceInfo) {
		int nIdx = a_oSender.FindIndex((a_stPriceInfo) => a_stPriceInfo.m_ePriceType == a_ePriceType && a_stPriceInfo.m_nKinds == a_nKinds);
		a_stOutPriceInfo = a_oSender.ExIsValidIdx(nIdx) ? a_oSender[nIdx] : default(STPriceInfo);

		return a_oSender.ExIsValidIdx(nIdx);
	}

	/** 획득 정보를 반환한다 */
	public static bool ExTryGetAcquireInfo(this List<STAcquireInfo> a_oSender, EItemKinds a_eItemKinds, out STAcquireInfo a_stOutAcquireInfo) {
		int nIdx = a_oSender.FindIndex((a_stAcquireInfo) => a_stAcquireInfo.m_eItemKinds == a_eItemKinds);
		a_stOutAcquireInfo = a_oSender.ExIsValidIdx(nIdx) ? a_oSender[nIdx] : default(STAcquireInfo);

		return a_oSender.ExIsValidIdx(nIdx);
	}
	#endregion			// 클래스 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
