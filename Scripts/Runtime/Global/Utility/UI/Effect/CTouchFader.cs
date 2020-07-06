using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

//! 터치 색상 처리자
public class CTouchFader : CUIComponent,
	IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {
	#region 변수
	private bool m_bIsTouch = false;
	private Tweener[] m_oAnimations = null;

	[SerializeField] private bool m_bIsRecursive = false;
	#endregion			// 변수

	#region 컴포넌트
	private Graphic[] m_oGraphics = null;
	#endregion			// 컴포넌트

	#region 프로퍼티
	public bool IsEnableAnimation { get; set; } = true;

	public Color OriginColor { get; set; } = Color.black;
	public Color NormalColor { get; set; } = Color.black;
	public Color SelectColor { get; set; } = Color.black;
	public Color DisableColor { get; set; } = Color.black;

	private int NumGraphics => Mathf.Min(m_oGraphics.Length, m_bIsRecursive ? m_oGraphics.Length : 1);
	#endregion			// 프로퍼티

	#region 인터페이스
	//! 영역에 들어왔을 경우
	public void OnPointerEnter(PointerEventData a_oEventData) {
		if(m_bIsTouch) {
			this.SetColor(this.OriginColor * this.SelectColor, this.IsEnableAnimation);
		}
	}

	//! 영역에서 벗어났을 경우
	public void OnPointerExit(PointerEventData a_oEventData) {
		if(m_bIsTouch) {
			this.SetColor(this.OriginColor * this.NormalColor, this.IsEnableAnimation);
		}
	}

	//! 터치를 시작했을 경우
	public void OnPointerDown(PointerEventData a_oEventData) {
		m_bIsTouch = true;
		this.SetColor(this.OriginColor * this.SelectColor, this.IsEnableAnimation);
	}

	//! 터치를 종료했을 경우
	public void OnPointerUp(PointerEventData a_oEventData) {
		m_bIsTouch = false;
		this.SetColor(this.OriginColor * this.NormalColor, this.IsEnableAnimation);
	}
	#endregion			// 인터페이스

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		this.NormalColor = KDefine.U_DEF_COLOR_NORMAL;
		this.SelectColor = KDefine.U_DEF_COLOR_SELECT;
		this.DisableColor = KDefine.U_DEF_COLOR_DISABLE;

		// 그래픽을 설정한다 {
		m_oGraphics = this.GetComponentsInChildren<Graphic>();

		if(m_oGraphics.ExIsValid()) {
			m_oAnimations = new Tweener[m_oGraphics.Length];
			this.OriginColor = m_oGraphics.First().color;
		}
		// 그래픽을 설정한다 }

		// 버튼을 설정한다 {
		var oBtn = this.GetComponentInChildren<Button>();

		if(oBtn != null && oBtn.transition == Selectable.Transition.ColorTint) {
			oBtn.transition = Selectable.Transition.None;
		}
		// 버튼을 설정한다 }
	}

	//! 활성화 되었을 경우
	public override void OnEnable() {
		base.OnEnable();
		this.SetColor(this.OriginColor * this.NormalColor, this.IsEnableAnimation);
	}

	//! 비활성화 되었을 경우
	public override void OnDisable() {
		base.OnDisable();

		if(!CSceneManager.IsAppQuit) {
			this.SetColor(this.OriginColor * this.DisableColor, this.IsEnableAnimation);
		}
	}

	//! 제거 되었을 경우
	public override void OnDestroy() {
		base.OnDestroy();
		
		if(!CSceneManager.IsAppQuit) {
			this.ResetAnimation();
		}
	}

	//! 재귀 여부를 변경한다
	public void SetRecursive(bool a_bIsRecursive) {
		m_bIsRecursive = a_bIsRecursive;
	}

	//! 색상을 변경한다
	public void SetColor(Color a_stColor, bool a_bIsAnimation = true) {
		this.ResetAnimation();

		for(int i = 0; i < this.NumGraphics; ++i) {
			if(!this.IsIgnoreAnimation && a_bIsAnimation) {
				m_oAnimations[i] = m_oGraphics[i].DOColor(a_stColor, 
					KDefine.U_DEF_DURATION_ANIMATION).SetUpdate(true);
			} else {
				m_oGraphics[i].color = a_stColor;
			}
		}
	}

	//! 애니메이션을 리셋한다
	private void ResetAnimation() {
		for(int i = 0; i < this.NumGraphics; ++i) {
			m_oAnimations[i]?.Kill();
		}
	}
	#endregion			// 함수
}
