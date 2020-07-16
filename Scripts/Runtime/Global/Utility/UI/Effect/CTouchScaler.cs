using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

//! 터치 비율 처리자
public class CTouchScaler : CUIComponent,
	IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {
	#region 변수
	private bool m_bIsTouch = false;
	private Tweener m_oAni = null;

	[SerializeField] private float m_fDuration = KDefine.U_DEF_DURATION_ANI;
	[SerializeField] private float m_fTouchScale = KDefine.U_DEF_SCALE_TOUCH;
	#endregion			// 변수

	#region 프로퍼티
	public bool IsEnableAni { get; set; } = true;
	public Vector3 OriginScale { get; set; } = Vector3.zero;
	#endregion			// 프로퍼티

	#region 인터페이스
	//! 영역에 들어왔을 경우
	public void OnPointerEnter(PointerEventData a_oEventData) {
		if(m_bIsTouch) {
			this.StartEnterAni();
		}
	}

	//! 영역에서 벗어났을 경우
	public void OnPointerExit(PointerEventData a_oEventData) {
		if(m_bIsTouch) {
			this.StartExitAni();
		}
	}

	//! 터치를 시작했을 경우
	public virtual void OnPointerDown(PointerEventData a_oEventData) {
		m_bIsTouch = true;
		this.StartEnterAni();
	}

	//! 터치를 종료했을 경우
	public virtual void OnPointerUp(PointerEventData a_oEventData) {
		m_bIsTouch = false;
		this.StartExitAni();
	}
	#endregion			// 인터페이스

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		this.OriginScale = KDefine.B_SCALE_NORMAL;
	}
	
	//! 제거 되었을 경우
	public override void OnDestroy() {
		base.OnDestroy();

		if(!CSceneManager.IsAppQuit) {
			this.ResetAni();
		}
	}

	//! 비율을 변경한다
	public void SetScale(Vector3 a_stScale, bool a_bIsAni = true) {
		this.ResetAni();

		if(!this.IsIgnoreAni && a_bIsAni) {
			m_oAni = m_oRectTransform.DOScale(a_stScale, this.m_fDuration).SetEase(Ease.Linear).SetUpdate(true);
		} else {
			m_oRectTransform.localScale = a_stScale;
		}
	}

	//! 애니메이션을 리셋한다
	private void ResetAni() {
		m_oAni?.Kill();
	}

	//! 터치 비율을 변경한다
	private void SetTouchScale(float a_fScale) {
		m_fTouchScale = a_fScale;
	}

	//! 진입 애니메이션을 시작한다
	private void StartEnterAni() {
		this.SetScale(this.OriginScale * m_fTouchScale, this.IsEnableAni);
	}

	//! 탈출 애니메이션을 시작한다
	private void StartExitAni() {
		this.SetScale(this.OriginScale, this.IsEnableAni);
	}
	#endregion			// 함수
}
