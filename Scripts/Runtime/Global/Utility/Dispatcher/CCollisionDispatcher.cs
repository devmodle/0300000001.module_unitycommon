using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 충돌 이벤트 전달자
public class CCollisionDispatcher : CComponent {
	#region 프로퍼티
	public System.Action<CCollisionDispatcher, Collision> CollisionEnterCallback { get; set; } = null;
	public System.Action<CCollisionDispatcher, Collision> CollisionStayCallback { get; set; } = null;
	public System.Action<CCollisionDispatcher, Collision> CollisionExitCallback { get; set; } = null;
	#endregion			// 프로퍼티

	#region 함수
	//! 충돌을 시작했을 경우
	public void OnCollisionEnter(Collision a_oCollision) {
		this.CollisionEnterCallback?.Invoke(this, a_oCollision);
	}

	//! 충돌을 진행 중 일 경우
	public void OnCollisionStay(Collision a_oCollision) {
		this.CollisionStayCallback?.Invoke(this, a_oCollision);
	}

	//! 충돌을 종료했을 경우
	public void OnCollisionExit(Collision a_oCollision) {
		this.CollisionExitCallback?.Invoke(this, a_oCollision);
	}
	#endregion			// 함수
}
