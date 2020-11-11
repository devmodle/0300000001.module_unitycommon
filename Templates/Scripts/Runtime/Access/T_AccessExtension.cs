using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 기본 접근 확장 클래스
public static partial class AccessExtension {
	#region 클래스 함수
	//! 상호 작용 여부를 변경한다
	public static void SetInteractable(this Button a_oSender, bool a_bIsEnable) {
		CAccess.Assert(a_oSender != null);
		
		a_oSender.interactable = a_bIsEnable;
		a_oSender.gameObject.ExSetEnableComponent<CTouchInteractable>(a_bIsEnable);
	}
	#endregion			// 클래스 함수
}
#endif			// #if NEVER_USE_THIS
