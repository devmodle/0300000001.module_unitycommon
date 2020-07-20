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
	protected Text m_oMsgText = null;
	#endregion			// 컴포넌트

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		this.IsIgnoreNavigationEvent = true;
		CNavStackManager.Instance.RemoveComponent(this);

		m_oMsgText = this.gameObject.ExFindComponent<Text>(KUDefine.OBJ_NAME_TOAST_P_MSG_TEXT);
		m_oRootTransform.sizeDelta = CSceneManager.CanvasSize;
	}

	//! 초기화
	public virtual void Init(string a_oMsg, float a_fDuration) {
		m_fDuration = a_fDuration;
		m_oMsgText.text = a_oMsg;
	}

	//! 토스트 팝업 출력 애니메이션이 완료 되었을 경우
	public void OnCompleteToastPopupShowAni() {
		Func.LateCallFunc(this, m_fDuration, (a_oComponent, a_oParams) => {
			this.ClosePopup();
		}, true);
	}

	//! 출력 애니메이션을 생성한다
	protected override Sequence CreateShowAni() {
		var oAni = base.CreateShowAni();
		oAni.AppendCallback(this.OnCompleteToastPopupShowAni);

		return oAni;
	}
	#endregion			// 함수

	#region 클래스 제네릭 함수
	//! 토스트 팝업을 생성한다
	public static T CreateToastPopup<T>(string a_oName,
		GameObject a_oOrigin, GameObject a_oParent, string a_oMsg, float a_fDuration) where T : CToastPopup {
		var oToastPopup = CPopup.CreatePopup<T>(a_oName, a_oOrigin, a_oParent, KBDefine.POS_MIDDLE_CENTER);
		oToastPopup?.Init(a_oMsg, a_fDuration);

		return oToastPopup;
	}
	#endregion			// 클래스 제네릭 함수
}
