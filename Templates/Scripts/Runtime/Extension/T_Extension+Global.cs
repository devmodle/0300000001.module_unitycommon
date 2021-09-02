using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 전역 확장 클래스
public static partial class Extension {
	#region 클래스 함수
	//! 효과를 재생한다
	public static void ExPlay(this ParticleSystem a_oSender, System.Action<CEventDispatcher> a_oCallback, bool a_bIsRemoveChildren = false, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oSender != null);
		var oEventDispatcher = a_oSender?.GetComponentInChildren<CEventDispatcher>();

		// 이벤트 전달자가 존재 할 경우
		if(oEventDispatcher != null) {
			oEventDispatcher.ParticleEventCallback = a_oCallback;
		}

		a_oSender?.ExPlay(a_bIsRemoveChildren, a_bIsEnableAssert);
	}
	#endregion			// 클래스 함수

	#region 추가 클래스 함수

	#endregion			// 추가 클래스 함수
}
#endif			// #if NEVER_USE_THIS
