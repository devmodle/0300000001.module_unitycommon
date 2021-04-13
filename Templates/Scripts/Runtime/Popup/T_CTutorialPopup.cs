using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if NEVER_USE_THIS
//! 튜토리얼 팝업
public class CTutorialPopup : CFocusPopup {
	//! 매개 변수
	public new struct STParams {
		public ETutorialKinds m_eTutorialKinds;
		public CFocusPopup.STParams m_stFocusParams;	
	}

	#region 변수
	private STParams m_stParams;
	#endregion			// 변수

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
	}

	//! 초기화
	public virtual void Init(STParams a_stParams, System.Action<CFocusPopup, PointerEventData> a_oBeginCallback, System.Action<CFocusPopup, PointerEventData> a_oMoveCallback, System.Action<CFocusPopup, PointerEventData> a_oEndCallback) {
		base.Init(a_stParams.m_stFocusParams, a_oBeginCallback, a_oMoveCallback, a_oEndCallback);
		m_stParams = a_stParams;

		this.UpdateUIsState();
	}

	//! UI 상태를 갱신한다
	private void UpdateUIsState() {
		// Do Nothing
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
