using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 기본 접근 확장 클래스
public static partial class AccessExtension {
	#region 클래스 함수
	//! 컴포넌트 상호 작용 여부를 변경한다
	public static void ExSetInteractable(this Button a_oSender, bool a_bIsEnable) {
		CAccess.Assert(a_oSender != null);

		var oTouchInteractable = a_oSender.GetComponentInChildren<CTouchInteractable>();
		oTouchInteractable?.SetInteractable(a_bIsEnable);
	}

	//! 종류 => 기본 종류로 변환한다
	public static int ExToBaseKinds(this int a_nSender) {
		return (a_nSender / KCDefine.B_UNIT_TYPE_TO_KINDS) * KCDefine.B_UNIT_TYPE_TO_KINDS;
	}

	//! 종류 => 종류 타입으로 변환한다
	public static int ExToKindsType(this int a_nSender) {
		int nBaseKinds = a_nSender.ExToBaseKinds();
		int nKindsType = a_nSender % KCDefine.B_UNIT_TYPE_TO_KINDS;
		
		return nBaseKinds + ((nKindsType / KCDefine.B_UNIT_TYPE_TO_KINDS_TYPE) * KCDefine.B_UNIT_TYPE_TO_KINDS_TYPE);
	}

	//! 종류 => 서브 종류 타입으로 변환한다
	public static int ExToSubKindsType(this int a_nSender) {
		int nKindsType = a_nSender.ExToKindsType();
		int nSubKindsType = a_nSender % KCDefine.B_UNIT_TYPE_TO_KINDS_TYPE;

		return nKindsType + ((nSubKindsType / KCDefine.B_UNIT_TYPE_TO_SUB_KINDS_TYPE) * KCDefine.B_UNIT_TYPE_TO_SUB_KINDS_TYPE);
	}
	#endregion			// 클래스 함수
}
#endif			// #if NEVER_USE_THIS
