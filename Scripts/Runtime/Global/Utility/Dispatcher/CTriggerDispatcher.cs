using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 접촉 이벤트 전달자
public class CTriggerDispatcher : CComponent {
	#region 프로퍼티
	public System.Action<CTriggerDispatcher, Collider> EnterCallback { get; set; } = null;
	public System.Action<CTriggerDispatcher, Collider> StayCallback { get; set; } = null;
	public System.Action<CTriggerDispatcher, Collider> ExitCallback { get; set; } = null;
	#endregion			// 프로퍼티

	#region 함수
	//! 접촉을 시작했을 경우
	public void OnTriggerEnter(Collider a_oCollider) {
		this.EnterCallback?.Invoke(this, a_oCollider);
	}

	//! 접촉을 진행 중 일 경우
	public void OnTriggerStay(Collider a_oCollider) {
		this.StayCallback?.Invoke(this, a_oCollider);
	}

	//! 접촉을 종료했을 경우
	public void OnTriggerExit(Collider a_oCollider) {
		this.ExitCallback?.Invoke(this, a_oCollider);
	}
	#endregion			// 함수
}
