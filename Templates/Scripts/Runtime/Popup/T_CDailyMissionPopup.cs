using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 일일 미션 팝업
public class CDailyMissionPopup : CMissionPopup {
	//! 매개 변수
	public new struct STParams {
		public CMissionPopup.STParams m_stMissionParams;	
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
	public virtual void Init(STParams a_stParams) {
		base.Init(a_stParams.m_stMissionParams);
		m_stParams = a_stParams;

		this.UpdateUIsState();
	}

	//! UI 상태를 갱신한다
	private void UpdateUIsState() {
		// Do Nothing
	}

	//! 미션 버튼을 눌렀을 경우
	private void OnTouchMissionBtn(STMissionInfo a_stMissionInfo) {
		// Do Nothing
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
