using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 컴포넌트
public class CComponent : MonoBehaviour {
	#region 컴포넌트
	[HideInInspector] public RectTransform m_oRectTransform = null;

	[HideInInspector] public Rigidbody m_oRigidbody = null;
	[HideInInspector] public Rigidbody2D m_oRigidbody2D = null;
	#endregion			// 컴포넌트

	#region 프로퍼티
	public bool IsDestroy { get; private set; } = false;
	public bool IsIgnoreAnimation { get; set; } = false;
	public bool IsIgnoreNavigationEvent { get; set; } = false;

	public System.Action<CComponent> DestroyCallback { get; set; } = null;
	public System.Action<CComponent> ScheduleCallback { get; set; } = null;
	public System.Action<CComponent> NavigationCallback { get; set; } = null;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public virtual void Awake() {
		m_oRectTransform = this.transform as RectTransform;

		m_oRigidbody = this.GetComponentInChildren<Rigidbody>();
		m_oRigidbody2D = this.GetComponentInChildren<Rigidbody2D>();
	}

	//! 초기화
	public virtual void Start() {
#if UNITY_EDITOR
		this.SetupScriptOrder();
#endif			// #if UNITY_EDITOR
	}

	//! 상태를 갱신한다
	public virtual void OnUpdate(float a_fDeltaTime) {
		// Do Nothing
	}

	//! 제거 되었을 경우
	public virtual void OnDestroy() {
		this.IsDestroy = true;

		if(!CSceneManager.IsAppQuit) {
			this.DestroyCallback?.Invoke(this);
			this.ScheduleCallback?.Invoke(this);
			this.NavigationCallback?.Invoke(this);
		}
	}

	//! 내비게이션 이벤트를 수신했을 경우
	public virtual void OnReceiveNavigationEvent(ENavigationEventType a_eEventType) {
		if(!this.IsIgnoreNavigationEvent && a_eEventType == ENavigationEventType.BACK_KEY_DOWN) {
			CNavigationManager.Instance.RemoveComponent(this);
		}
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR
	//! 스크립트 순서를 설정한다
	protected virtual void SetupScriptOrder() {
		// Do Nothing
	}
#endif			// #if UNITY_EDITOR
	#endregion			// 조건부 함수
}
