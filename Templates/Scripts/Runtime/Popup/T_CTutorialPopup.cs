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

	//! 콜백 매개 변수
	public new struct STCallbackParams {
		public CFocusPopup.STCallbackParams m_stFocusCallbackParams;
	}

	#region 변수
	private STParams m_stParams;
	private STCallbackParams m_stCallbackParams;
	#endregion			// 변수

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
	}

	//! 초기화
	public virtual void Init(STParams a_stParams, STCallbackParams a_stCallbackParams) {
		base.Init(a_stParams.m_stFocusParams, a_stCallbackParams.m_stFocusCallbackParams);

		m_stParams = a_stParams;
		m_stCallbackParams = a_stCallbackParams;

		this.UpdateUIsState();
	}

	//! UI 상태를 갱신한다
	private void UpdateUIsState() {
		// Do Nothing
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
