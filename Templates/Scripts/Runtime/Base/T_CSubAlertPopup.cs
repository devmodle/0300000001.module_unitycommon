using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 서브 경고 팝업
public class CSubAlertPopup : CAlertPopup {
	#region 프로퍼티
	public override EAniType AniType => EAniType.DROPDOWN;
	#endregion			// 프로퍼티
	
	#region 함수
	//! 초기화
	public override void Init(STParams a_stParams, System.Action<CAlertPopup, bool> a_oCallback) {
		base.Init(a_stParams, a_oCallback);
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
