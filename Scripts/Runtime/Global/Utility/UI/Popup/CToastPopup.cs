using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//! 토스트 팝업
public class CToastPopup : CPopup {
	#region 변수
	protected float m_fDuration = 0.0f;
	#endregion			// 변수

	#region 컴포넌트
	protected Text m_oMessageText = null;
	#endregion			// 컴포넌트

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		this.IsIgnoreNavigationEvent = true;
		CNavigationManager.Instance.RemoveComponent(this);

		m_oMessageText = this.gameObject.ExFindComponent<Text>(KDefine.U_OBJ_NAME_TOAST_P_MESSAGE_TEXT);
		m_oRootTransform.sizeDelta = CSceneManager.CanvasSize;
	}

	//! 초기화
	public virtual void Init(string a_oMessage, float a_fDuration) {
		m_fDuration = a_fDuration;
		m_oMessageText.text = a_oMessage;
	}

	//! 토스트 팝업 출력 애니메이션이 완료 되었을 경우
	public void OnCompleteToastPopupShowAnimation() {
		Func.LateCallFunc(this, m_fDuration, (a_oComponent, a_oParams) => {
			this.ClosePopup();
		}, true);
	}

	//! 출력 애니메이션을 생성한다
	protected override Sequence CreateShowAnimation() {
		var oAnimation = base.CreateShowAnimation();
		oAnimation.AppendCallback(this.OnCompleteToastPopupShowAnimation);

		return oAnimation;
	}
	#endregion			// 함수
}
