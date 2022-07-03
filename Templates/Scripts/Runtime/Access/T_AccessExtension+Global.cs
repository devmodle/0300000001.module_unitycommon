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
	/** 타겟 정보를 반환한다 */
	public static STTargetInfo ExGetTargetInfo(this List<STTargetInfo> a_oSender, ETargetKinds a_eTargetKinds, int a_nKinds) {
		bool bIsValid = a_oSender.ExTryGetTargetInfo(a_eTargetKinds, a_nKinds, out STTargetInfo stTargetInfo);
		CAccess.Assert(bIsValid);

		return stTargetInfo;
	}

	/** 타겟 정보를 반환한다 */
	public static bool ExTryGetTargetInfo(this List<STTargetInfo> a_oSender, ETargetKinds a_eTargetKinds, int a_nKinds, out STTargetInfo a_stOutTargetInfo) {
		int nIdx = a_oSender.FindIndex((a_stTargetInfo) => a_stTargetInfo.m_eTargetKinds == a_eTargetKinds && a_stTargetInfo.m_nKinds == a_nKinds);
		a_stOutTargetInfo = a_oSender.ExIsValidIdx(nIdx) ? a_oSender[nIdx] : default(STTargetInfo);

		return a_oSender.ExIsValidIdx(nIdx);
	}
	#endregion			// 클래스 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
