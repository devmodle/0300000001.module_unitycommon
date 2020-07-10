using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//! 터치 이벤트 전달자
public class CTouchDispatcher : CComponent, IDragHandler, IPointerUpHandler, IPointerDownHandler {
	#region 프로퍼티
	public System.Action<CTouchDispatcher, PointerEventData> BeganCallback { get; set; } = null;
	public System.Action<CTouchDispatcher, PointerEventData> MovedCallback { get; set; } = null;
	public System.Action<CTouchDispatcher, PointerEventData> EndedCallback { get; set; } = null;
	#endregion			// 프로퍼티

	#region 인터페이스
	//! 터치를 시작했을 경우
	public void OnPointerDown(PointerEventData a_oEventData) {
		this.BeganCallback?.Invoke(this, a_oEventData);
	}

	//! 터치를 움직였을 경우
	public void OnDrag(PointerEventData a_oEventData) {
		this.MovedCallback?.Invoke(this, a_oEventData);
	}

	//! 터치를 종료했을 경우
	public void OnPointerUp(PointerEventData a_oEventData) {
		this.EndedCallback?.Invoke(this, a_oEventData);
	}
	#endregion			// 인터페이스
}
