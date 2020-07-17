using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//! 팝업
public class CPopup : CUIComponent {
	#region 변수
	protected Tweener m_oBGAni = null;

	protected Sequence m_oShowAni = null;
	protected Sequence m_oCloseAni = null;

	protected System.Action<CPopup> m_oShowCallback = null;
	protected System.Action<CPopup> m_oCloseCallback = null;
	#endregion			// 변수

	#region 컴포넌트
	protected Image m_oBGImg = null;
	protected RectTransform m_oRootTransform = null;
	#endregion			// 컴포넌트

	#region 객체
	protected GameObject m_oContentRoot = null;
	protected GameObject m_oTouchResponder = null;
	#endregion			// 객체

	#region 프로퍼티
	public bool IsClose { get; private set; } = false;
	public bool IsEnableBGAni { get; set; } = true;

	public float ShowTimeScale { get; set; } = KDefine.U_ZERO_TIME_SCALE;
	public float CloseTimeScale { get; set; } = KDefine.U_DEF_TIME_SCALE;

	public float ShowAniDelay { get; set; } = KDefine.U_DEF_DELAY_POPUP_SHOW_ANI;
	public virtual Color BGColor => KAppDefine.G_DEF_COLOR_POPUP_BG;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		CNavStackManager.Instance.AddComponent(this);

		// 루트를 설정한다
		m_oContentRoot = this.gameObject.ExFindChild(KDefine.U_OBJ_NAME_POPUP_CONTENT_ROOT);
		
		// 터치 응답자를 생성한다
		m_oTouchResponder = this.CreateTouchResponder();
		m_oTouchResponder.transform.SetAsFirstSibling();

		// 컨텐츠 크기를 설정한다
		m_oRootTransform = m_oContentRoot.transform as RectTransform;
		m_oRootTransform.localScale = new Vector3(KDefine.U_MIN_SCALE_POPUP, KDefine.U_MIN_SCALE_POPUP, KDefine.U_MIN_SCALE_POPUP);

		// 이미지를 생성한다
		m_oBGImg = m_oTouchResponder.GetComponentInChildren<Image>();
		m_oBGImg.color = this.BGColor.ExGetAlphaColor(0.0f);

		// 버튼을 생성한다
		var oCloseBtn = m_oContentRoot.ExFindComponent<Button>(KDefine.U_OBJ_NAME_POPUP_CLOSE_BTN);
		oCloseBtn?.onClick.AddListener(this.OnTouchCloseBtn);
	}

	//! 제거 되었을 경우
	public override void OnDestroy() {
		base.OnDestroy();

		if(!CSceneManager.IsAppQuit) {
			this.ResetAni();
		}
	}

	//! 닫기 버튼을 눌렀을 경우
	public virtual void OnTouchCloseBtn() {
		this.ClosePopup();
	}

	//! 출력 애니메이션이 완료 되었을 경우
	public void OnCompleteShowAni() {
		this.OnShowPopup();
		m_oShowCallback?.Invoke(this);

		m_oRootTransform.localScale = new Vector3(KDefine.U_DEF_SCALE_POPUP, KDefine.U_DEF_SCALE_POPUP, KDefine.U_DEF_SCALE_POPUP);
	}

	//! 닫기 애니메이션이 완료 되었을 경우
	public void OnCompleteCloseAni() {
		this.OnClosePopup();
		m_oCloseCallback?.Invoke(this);

		Destroy(this.gameObject);
	}

	//! 내비게이션 스택 이벤트를 수신했을 경우
	public override void OnReceiveNavStackEvent(ENavStackEventType a_eEventType) {
		base.OnReceiveNavStackEvent(a_eEventType);

		if(a_eEventType == ENavStackEventType.TOP) {
			Time.timeScale = this.ShowTimeScale;
		} else if(!this.IsIgnoreNavigationEvent && a_eEventType == ENavStackEventType.REMOVE) {
			this.ClosePopup();
		}
	}

	//! 배경 색상을 변경한다
	public void SetBGColor(Color a_stColor, bool a_bIsAni = true) {
		m_oBGAni?.Kill();
		
		if(!a_bIsAni) {
			m_oBGImg.color = a_stColor;
		} else {
			m_oBGAni = m_oBGImg.DOColor(a_stColor, KDefine.U_DEF_DURATION_ANI).SetUpdate(true);
		}
	}

	//! 팝업을 출력한다
	public virtual void ShowPopup(System.Action<CPopup> a_oShowCallback, System.Action<CPopup> a_oCloseCallback) {
		if(!this.IsDestroy) {
			m_oShowCallback = a_oShowCallback;
			m_oCloseCallback = a_oCloseCallback;

			if(this.ShowAniDelay.ExIsLessEquals(KDefine.B_DELTA_TIME_INTERMEDIATE)) {
				Func.LateCallFunc(this, this.DoShowPopup);
			} else {
				Func.LateCallFunc(this, this.ShowAniDelay, this.DoShowPopup, true);
			}
		}
	}

	//! 팝업을 닫는다
	public virtual void ClosePopup() {
		if(!this.IsClose && !this.IsDestroy) {
			this.IsClose = true;
			this.StartCloseAni();

			if(this.NavigationCallback != null) {
				this.NavigationCallback = null;
				CNavStackManager.Instance.RemoveComponent(this);
			}
		}
	}

	//! 출력 되었을 경우
	protected virtual void OnShowPopup() {
		// Do Nothing
	}

	//! 닫혔을 경우
	protected virtual void OnClosePopup() {
		// Do Nothing
	}

	//! 팝업 컨텐츠를 설정한다
	protected virtual void SetupPopupContents() {
		// Do Nothing
	}

	//! 터치 응답자를 생성한다
	protected virtual GameObject CreateTouchResponder() {
		return Func.CreateTouchResponder(string.Format(KDefine.U_OBJ_NAME_FORMAT_POPUP_TOUCH_RESPONDER, this.gameObject.name),
			CResManager.Instance.GetPrefab(KDefine.U_OBJ_PATH_TOUCH_RESPONDER),
			this.gameObject,
			CSceneManager.CanvasSize,
			Vector3.zero.ExToLocal(this.gameObject),
			KDefine.U_DEF_COLOR_TRANSPARENT);
	}

	//! 출력 애니메이션을 생성한다
	protected virtual Sequence CreateShowAni() {
		var oAni = m_oRootTransform.DOScale(KDefine.U_DEF_SCALE_POPUP, KDefine.U_DEF_DURATION_POPUP_ANI);
		return DOTween.Sequence().SetEase(Ease.OutExpo).Append(oAni);
	}

	//! 닫기 애니메이션을 생성한다
	protected virtual Sequence CreateCloseAni() {
		var oAni = m_oRootTransform.DOScale(KDefine.U_MIN_SCALE_POPUP, KDefine.U_DEF_DURATION_POPUP_ANI);
		return DOTween.Sequence().SetEase(Ease.InExpo).Append(oAni);
	}

	//! 팝업을 출력한다
	private void DoShowPopup(CComponent a_oComponent, object[] a_oParams) {
		if(!this.IsClose && !this.IsDestroy) {
			this.SetupPopupContents();
			this.StartShowAni();

			this.SetBGColor(this.BGColor, this.IsEnableBGAni);
		}
	}

	//! 애니메이션을 리셋한다
	private void ResetAni() {
		m_oBGAni?.Kill();

		m_oShowAni?.Kill();
		m_oCloseAni?.Kill();
	}

	//! 출력 애니메이션을 시작한다
	private void StartShowAni() {
		this.ResetAni();
		Time.timeScale = this.ShowTimeScale;

		m_oShowAni = this.CreateShowAni();
		m_oShowAni?.SetAutoKill().SetUpdate(true);
		m_oShowAni?.AppendCallback(this.OnCompleteShowAni);
	}

	//! 닫기 애니메이션을 시작한다
	private void StartCloseAni() {
		this.ResetAni();
		Time.timeScale = this.CloseTimeScale;
		
		m_oCloseAni = this.CreateCloseAni();
		m_oCloseAni?.SetAutoKill().SetUpdate(true);
		m_oCloseAni?.AppendCallback(this.OnCompleteCloseAni);
	}
	#endregion			// 함수
}
