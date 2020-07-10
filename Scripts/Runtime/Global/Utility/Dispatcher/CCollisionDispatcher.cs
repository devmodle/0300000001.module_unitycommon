using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 충돌 이벤트 전달자
public class CCollisionDispatcher : CComponent {
	#region 프로퍼티
	public System.Action<CCollisionDispatcher, Collision> EnterCallback { get; set; } = null;
	public System.Action<CCollisionDispatcher, Collision> StayCallback { get; set; } = null;
	public System.Action<CCollisionDispatcher, Collision> ExitCallback { get; set; } = null;
	#endregion			// 프로퍼티

	#region 함수
	//! 충돌을 시작했을 경우
	public void OnCollisionEnter(Collision a_oCollision) {
		this.EnterCallback?.Invoke(this, a_oCollision);
	}

	//! 충돌을 진행 중 일 경우
	public void OnCollisionStay(Collision a_oCollision) {
		this.StayCallback?.Invoke(this, a_oCollision);
	}

	//! 충돌을 종료했을 경우
	public void OnCollisionExit(Collision a_oCollision) {
		this.ExitCallback?.Invoke(this, a_oCollision);
	}
	#endregion			// 함수
}
