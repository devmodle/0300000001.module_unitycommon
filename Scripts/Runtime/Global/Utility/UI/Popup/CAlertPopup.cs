using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//! 알림 팝업
public class CAlertPopup : CPopup {
	#region 변수
	protected System.Action<CAlertPopup, bool> m_oCallback = null;
	#endregion			// 변수

	#region 컴포넌트
	protected Text m_oTitleText = null;
	protected Text m_oMessageText = null;
	protected Text m_oOKButtonText = null;
	protected Text m_oCancelButtonText = null;

	protected Image m_oOKButtonImage = null;
	protected Image m_oCancelButtonImage = null;
	protected Image m_oAlertPopupBGImage = null;
	#endregion			// 컴포넌트

	#region 객체
	[SerializeField] private GameObject m_oTextRoot = null;
	[SerializeField] private GameObject m_oButtonRoot = null;
	#endregion			// 객체

	#region 프로퍼티
	public Vector2 MinBGSize { get; set; } = Vector2.zero;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		this.ShowTimeScale = KDefine.U_DEF_TIME_SCALE;

		// 버튼을 설정한다 {
		var oOKButton = m_oButtonRoot.ExFindComponent<Button>(KDefine.U_OBJ_NAME_ALERT_P_OK_BUTTON);
		oOKButton.onClick.AddListener(this.OnTouchOKButton);

		var oCancelButton = m_oButtonRoot.ExFindComponent<Button>(KDefine.U_OBJ_NAME_ALERT_P_CANCEL_BUTTON);
		oCancelButton.onClick.AddListener(this.OnTouchCancelButton);
		// 버튼을 설정한다 }

		// 텍스트를 설정한다 {
		m_oTitleText = m_oTextRoot.ExFindComponent<Text>(KDefine.U_OBJ_NAME_ALERT_P_TITLE_TEXT);
		m_oMessageText = m_oTextRoot.ExFindComponent<Text>(KDefine.U_OBJ_NAME_ALERT_P_MESSAGE_TEXT);

		m_oOKButtonText = oOKButton.GetComponentInChildren<Text>();
		m_oCancelButtonText = oCancelButton.GetComponentInChildren<Text>();
		// 텍스트를 설정한다 }

		// 이미지를 설정한다 {
		m_oAlertPopupBGImage = m_oContentRoot.ExFindComponent<Image>(KDefine.U_OBJ_NAME_ALERT_P_BG_IMAGE);

		m_oOKButtonImage = oOKButton.GetComponentInChildren<Image>();
		m_oCancelButtonImage = oCancelButton.GetComponentInChildren<Image>();
		// 이미지를 설정한다 }
	}

	//! 초기화
	public virtual void Init(Dictionary<string, string> a_oDataList, System.Action<CAlertPopup, bool> a_oCallback) {
		m_oCallback = a_oCallback;

		// 텍스트를 설정한다 {
		m_oTitleText.text = a_oDataList.ContainsKey(KDefine.U_KEY_ALERT_P_TITLE) ? a_oDataList[KDefine.U_KEY_ALERT_P_TITLE] 
			: string.Empty;

		m_oMessageText.text = a_oDataList.ContainsKey(KDefine.U_KEY_ALERT_P_MESSAGE) ? a_oDataList[KDefine.U_KEY_ALERT_P_MESSAGE] 
			: string.Empty;

		m_oOKButtonText.text = a_oDataList.ContainsKey(KDefine.U_KEY_ALERT_P_OK_BUTTON_TEXT) ? a_oDataList[KDefine.U_KEY_ALERT_P_OK_BUTTON_TEXT] 
			: string.Empty;

		m_oCancelButtonText.text = a_oDataList.ContainsKey(KDefine.U_KEY_ALERT_P_CANCEL_BUTTON_TEXT) ? a_oDataList[KDefine.U_KEY_ALERT_P_CANCEL_BUTTON_TEXT] 
			: string.Empty;
		// 텍스트를 설정한다 }
	}

	//! 확인 버튼을 눌렀을 경우
	public virtual void OnTouchOKButton() {
		m_oCallback?.Invoke(this, true);
		this.ClosePopup();
	}

	//! 취소 버튼을 눌렀을 경우
	public virtual void OnTouchCancelButton() {
		m_oCallback?.Invoke(this, false);
		this.ClosePopup();
	}

	//! 팝업 컨텐츠를 설정한다
	protected override void SetupPopupContents() {
		var oBGTransform = m_oAlertPopupBGImage.rectTransform;
		oBGTransform.pivot = KDefine.B_ANCHOR_MIDDLE_CENTER;

		var oTitleTransform = m_oTitleText.rectTransform;
		oTitleTransform.pivot = KDefine.B_ANCHOR_MIDDLE_CENTER;

		var oMessageTransform = m_oMessageText.rectTransform;
		oMessageTransform.pivot = KDefine.B_ANCHOR_MIDDLE_CENTER;

		var oOKButtonTransform = m_oOKButtonImage.rectTransform;
		oOKButtonTransform.pivot = KDefine.B_ANCHOR_MIDDLE_CENTER;

		var oCancelButtonTransform = m_oCancelButtonImage.rectTransform;
		oCancelButtonTransform.pivot = KDefine.B_ANCHOR_MIDDLE_CENTER;

		var stTitleRect = oTitleTransform.rect;
		var stMessageRect = oMessageTransform.rect;
		var stOKButtonRect = oOKButtonTransform.rect;
		var stCancelButtonRect = oCancelButtonTransform.rect;

		float fBGWidth = Mathf.Max(stMessageRect.width, this.MinBGSize.x);
		float fBGHeight = Mathf.Max(stMessageRect.height, this.MinBGSize.y);

		// 텍스트를 설정한다 {
		oMessageTransform.anchoredPosition = Vector2.zero;
		oTitleTransform.anchoredPosition = new Vector2(0.0f, (stMessageRect.height / 2.0f) + (stTitleRect.height / 2.0f) + KAppDefine.G_V_OFFSET_ALERT_P_TITLE);

		fBGWidth = Mathf.Max(fBGWidth, stTitleRect.width);
		fBGHeight = Mathf.Max(fBGHeight, Mathf.Abs(oTitleTransform.anchoredPosition.y) + (stTitleRect.height / 2.0f));
		// 텍스트를 설정한다 }

		// 버튼을 설정한다 {
		if(m_oCancelButtonText.text.Length <= 0) {
			oOKButtonTransform.anchoredPosition = new Vector2(0.0f, -((stMessageRect.height / 2.0f) + (stOKButtonRect.height / 2.0f) + KAppDefine.G_V_OFFSET_ALERT_P_BUTTON));
			oCancelButtonTransform.anchoredPosition = new Vector2(0.0f, -((stMessageRect.height / 2.0f) + (stOKButtonRect.height / 2.0f) + KAppDefine.G_V_OFFSET_ALERT_P_BUTTON));

			m_oCancelButtonImage.gameObject.SetActive(false);
		} else {
			float fPosY = -((stMessageRect.height / 2.0f) + (stOKButtonRect.height / 2.0f) + KAppDefine.G_V_OFFSET_ALERT_P_BUTTON);

			oOKButtonTransform.anchoredPosition = new Vector2(-((stOKButtonRect.width / 2.0f) + (KAppDefine.G_H_OFFSET_ALERT_P_BUTTON / 2.0f)), fPosY);
			oCancelButtonTransform.anchoredPosition = new Vector2((stOKButtonRect.width / 2.0f) + (KAppDefine.G_H_OFFSET_ALERT_P_BUTTON / 2.0f), fPosY);
		}

		float fOKButtonDeltaX = Mathf.Abs(oOKButtonTransform.anchoredPosition.x);
		float fCancelButtonDeltaX = Mathf.Abs(oCancelButtonTransform.anchoredPosition.x);

		float fButtonHeight = Mathf.Max(stOKButtonRect.height, stCancelButtonRect.height);

		fBGWidth = Mathf.Max(fBGWidth, (fOKButtonDeltaX + (stOKButtonRect.width / 2.0f)) + (fCancelButtonDeltaX + (stCancelButtonRect.width / 2.0f)));
		fBGHeight = Mathf.Max(fBGHeight, Mathf.Abs(oOKButtonTransform.anchoredPosition.y) + (fButtonHeight / 2.0f));
		// 버튼을 설정한다 }

		// 배경을 설정한다
		oBGTransform.sizeDelta = new Vector2(fBGWidth + KAppDefine.G_H_OFFSET_ALERT_P_BG, (fBGHeight * 2.0f) + KAppDefine.G_V_OFFSET_ALERT_P_BG);

		// 위치를 보정한다 {
		oTitleTransform.anchoredPosition += new Vector2(0.0f, KAppDefine.G_V_EXTRA_OFFSET_ALERT_P_TITLE);
		oMessageTransform.anchoredPosition += new Vector2(0.0f, KAppDefine.G_V_EXTRA_OFFSET_ALERT_P_MESSAGE);

		oOKButtonTransform.anchoredPosition += new Vector2(0.0f, KAppDefine.G_V_EXTRA_OFFSET_ALERT_P_BUTTON);
		oCancelButtonTransform.anchoredPosition += new Vector2(0.0f, KAppDefine.G_V_EXTRA_OFFSET_ALERT_P_BUTTON);
		// 위치를 보정한다 }
	}
	#endregion			// 함수
}
