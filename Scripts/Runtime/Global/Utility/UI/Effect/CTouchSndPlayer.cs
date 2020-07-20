using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//! 터치 사운드 재생자
public class CTouchSndPlayer : CUIComponent, 
	IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {
	#region 변수
	private bool m_bIsTouch = false;
	#endregion			// 변수

	#region 프로퍼티
	public string TouchBeganSndFilepath { get; set; } = string.Empty;
	public string TouchEndedSndFilepath { get; set; } = string.Empty;
	#endregion			// 프로퍼티

	#region 인터페이스
	//! 영역에 들어왔을 경우
	public void OnPointerEnter(PointerEventData a_oEventData) {
		m_bIsTouch = true;
	}

	//! 영역에서 벗어났을 경우
	public void OnPointerExit(PointerEventData a_oEventData) {
		m_bIsTouch = false;
	}

	//! 터치를 시작했을 경우
	public virtual void OnPointerDown(PointerEventData a_oEventData) {
		if(this.TouchBeganSndFilepath.ExIsValid()) {
			CSndManager.Instance.PlayFXSnd(this.TouchBeganSndFilepath);
		}
	}

	//! 터치를 종료했을 경우
	public virtual void OnPointerUp(PointerEventData a_oEventData) {
		if(m_bIsTouch && this.TouchEndedSndFilepath.ExIsValid()) {
			CSndManager.Instance.PlayFXSnd(this.TouchEndedSndFilepath);
		}
	}
	#endregion			// 인터페이스

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		this.TouchBeganSndFilepath = KUDefine.SND_PATH_G_TOUCH_BEGAN;
		this.TouchEndedSndFilepath = KUDefine.SND_PATH_G_TOUCH_ENDED;
	}
	#endregion			// 함수
}
