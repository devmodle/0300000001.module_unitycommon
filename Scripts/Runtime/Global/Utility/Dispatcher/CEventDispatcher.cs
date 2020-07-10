using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 이벤트 전달자
public class CEventDispatcher : CComponent {
	#region 프로퍼티
	public System.Action<CEventDispatcher> ParticleCallback { get; set; } = null;
	public System.Action<CEventDispatcher, string> AnimationCallback { get; set; } = null;
	#endregion			// 프로퍼티

	#region 함수
	//! 파티클 이벤트를 수신했을 경우
	public void OnReceiveParticleEvent() {
		this.ParticleCallback?.Invoke(this);
	}

	//! 애니메이션 이벤트를 수신했을 경우
	public void OnReceiveAnimationEvent(string a_oEvent) {
		this.AnimationCallback?.Invoke(this, a_oEvent);
	}
	#endregion			// 함수
}
