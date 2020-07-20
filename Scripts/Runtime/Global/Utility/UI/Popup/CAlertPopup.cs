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
	protected Text m_oMsgText = null;
	protected Text m_oOKBtnText = null;
	protected Text m_oCancelBtnText = null;

	protected Image m_oOKBtnImg = null;
	protected Image m_oCancelBtnImg = null;
	protected Image m_oAlertPopupBGImg = null;
	#endregion			// 컴포넌트

	#region 객체
	[SerializeField] private GameObject m_oTextRoot = null;
	[SerializeField] private GameObject m_oBtnRoot = null;
	#endregion			// 객체

	#region 프로퍼티
	public Vector2 MinBGSize { get; set; } = Vector2.zero;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 버튼을 설정한다 {
		var oOKBtn = m_oBtnRoot.ExFindComponent<Button>(KUDefine.OBJ_NAME_ALERT_P_OK_BTN);
		oOKBtn.onClick.AddListener(this.OnTouchOKBtn);

		var oCancelBtn = m_oBtnRoot.ExFindComponent<Button>(KUDefine.OBJ_NAME_ALERT_P_CANCEL_BTN);
		oCancelBtn.onClick.AddListener(this.OnTouchCancelBtn);
		// 버튼을 설정한다 }

		// 텍스트를 설정한다 {
		m_oTitleText = m_oTextRoot.ExFindComponent<Text>(KUDefine.OBJ_NAME_ALERT_P_TITLE_TEXT);
		m_oMsgText = m_oTextRoot.ExFindComponent<Text>(KUDefine.OBJ_NAME_ALERT_P_MSG_TEXT);

		m_oOKBtnText = oOKBtn.GetComponentInChildren<Text>();
		m_oCancelBtnText = oCancelBtn.GetComponentInChildren<Text>();
		// 텍스트를 설정한다 }

		// 이미지를 설정한다 {
		m_oAlertPopupBGImg = m_oContentRoot.ExFindComponent<Image>(KUDefine.OBJ_NAME_ALERT_P_BG_IMG);

		m_oOKBtnImg = oOKBtn.GetComponentInChildren<Image>();
		m_oCancelBtnImg = oCancelBtn.GetComponentInChildren<Image>();
		// 이미지를 설정한다 }
	}

	//! 초기화
	public virtual void Init(Dictionary<string, string> a_oDataList, System.Action<CAlertPopup, bool> a_oCallback) {
		m_oCallback = a_oCallback;

		// 텍스트를 설정한다 {
		m_oTitleText.text = a_oDataList.ContainsKey(KUDefine.KEY_ALERT_P_TITLE) ? a_oDataList[KUDefine.KEY_ALERT_P_TITLE] 
			: string.Empty;

		m_oMsgText.text = a_oDataList.ContainsKey(KUDefine.KEY_ALERT_P_MSG) ? a_oDataList[KUDefine.KEY_ALERT_P_MSG] 
			: string.Empty;

		m_oOKBtnText.text = a_oDataList.ContainsKey(KUDefine.KEY_ALERT_P_OK_BTN_TEXT) ? a_oDataList[KUDefine.KEY_ALERT_P_OK_BTN_TEXT] 
			: string.Empty;

		m_oCancelBtnText.text = a_oDataList.ContainsKey(KUDefine.KEY_ALERT_P_CANCEL_BTN_TEXT) ? a_oDataList[KUDefine.KEY_ALERT_P_CANCEL_BTN_TEXT] 
			: string.Empty;
		// 텍스트를 설정한다 }
	}

	//! 확인 버튼을 눌렀을 경우
	public virtual void OnTouchOKBtn() {
		m_oCallback?.Invoke(this, true);
		this.ClosePopup();
	}

	//! 취소 버튼을 눌렀을 경우
	public virtual void OnTouchCancelBtn() {
		m_oCallback?.Invoke(this, false);
		this.ClosePopup();
	}

	//! 팝업 컨텐츠를 설정한다
	protected override void SetupPopupContents() {
		var oBGTransform = m_oAlertPopupBGImg.rectTransform;
		oBGTransform.pivot = KBDefine.ANCHOR_MIDDLE_CENTER;

		var oTitleTransform = m_oTitleText.rectTransform;
		oTitleTransform.pivot = KBDefine.ANCHOR_MIDDLE_CENTER;

		var oMsgTransform = m_oMsgText.rectTransform;
		oMsgTransform.pivot = KBDefine.ANCHOR_MIDDLE_CENTER;

		var oOKBtnTransform = m_oOKBtnImg.rectTransform;
		oOKBtnTransform.pivot = KBDefine.ANCHOR_MIDDLE_CENTER;

		var oCancelBtnTransform = m_oCancelBtnImg.rectTransform;
		oCancelBtnTransform.pivot = KBDefine.ANCHOR_MIDDLE_CENTER;

		var stTitleRect = oTitleTransform.rect;
		var stMsgRect = oMsgTransform.rect;
		var stOKBtnRect = oOKBtnTransform.rect;
		var stCancelBtnRect = oCancelBtnTransform.rect;

		float fBGWidth = Mathf.Max(stMsgRect.width, this.MinBGSize.x);
		float fBGHeight = Mathf.Max(stMsgRect.height, this.MinBGSize.y);

		// 텍스트를 설정한다 {
		oMsgTransform.anchoredPosition = Vector2.zero;
		oTitleTransform.anchoredPosition = new Vector2(0.0f, (stMsgRect.height / 2.0f) + (stTitleRect.height / 2.0f) + KAppDefine.G_V_OFFSET_ALERT_P_TITLE);

		fBGWidth = Mathf.Max(fBGWidth, stTitleRect.width);
		fBGHeight = Mathf.Max(fBGHeight, Mathf.Abs(oTitleTransform.anchoredPosition.y) + (stTitleRect.height / 2.0f));
		// 텍스트를 설정한다 }

		// 버튼을 설정한다 {
		if(m_oCancelBtnText.text.Length <= 0) {
			oOKBtnTransform.anchoredPosition = new Vector2(0.0f, -((stMsgRect.height / 2.0f) + (stOKBtnRect.height / 2.0f) + KAppDefine.G_V_OFFSET_ALERT_P_BTN));
			oCancelBtnTransform.anchoredPosition = new Vector2(0.0f, -((stMsgRect.height / 2.0f) + (stOKBtnRect.height / 2.0f) + KAppDefine.G_V_OFFSET_ALERT_P_BTN));

			m_oCancelBtnImg.gameObject.SetActive(false);
		} else {
			float fPosY = -((stMsgRect.height / 2.0f) + (stOKBtnRect.height / 2.0f) + KAppDefine.G_V_OFFSET_ALERT_P_BTN);

			oOKBtnTransform.anchoredPosition = new Vector2(-((stOKBtnRect.width / 2.0f) + (KAppDefine.G_H_OFFSET_ALERT_P_BTN / 2.0f)), fPosY);
			oCancelBtnTransform.anchoredPosition = new Vector2((stOKBtnRect.width / 2.0f) + (KAppDefine.G_H_OFFSET_ALERT_P_BTN / 2.0f), fPosY);
		}

		float fOKBtnDeltaX = Mathf.Abs(oOKBtnTransform.anchoredPosition.x);
		float fCancelBtnDeltaX = Mathf.Abs(oCancelBtnTransform.anchoredPosition.x);

		float fBtnHeight = Mathf.Max(stOKBtnRect.height, stCancelBtnRect.height);

		fBGWidth = Mathf.Max(fBGWidth, (fOKBtnDeltaX + (stOKBtnRect.width / 2.0f)) + (fCancelBtnDeltaX + (stCancelBtnRect.width / 2.0f)));
		fBGHeight = Mathf.Max(fBGHeight, Mathf.Abs(oOKBtnTransform.anchoredPosition.y) + (fBtnHeight / 2.0f));
		// 버튼을 설정한다 }

		// 배경을 설정한다
		oBGTransform.sizeDelta = new Vector2(fBGWidth + KAppDefine.G_H_OFFSET_ALERT_P_BG, (fBGHeight * 2.0f) + KAppDefine.G_V_OFFSET_ALERT_P_BG);

		// 위치를 보정한다 {
		oTitleTransform.anchoredPosition += new Vector2(0.0f, KAppDefine.G_V_EXTRA_OFFSET_ALERT_P_TITLE);
		oMsgTransform.anchoredPosition += new Vector2(0.0f, KAppDefine.G_V_EXTRA_OFFSET_ALERT_P_MSG);

		oOKBtnTransform.anchoredPosition += new Vector2(0.0f, KAppDefine.G_V_EXTRA_OFFSET_ALERT_P_BTN);
		oCancelBtnTransform.anchoredPosition += new Vector2(0.0f, KAppDefine.G_V_EXTRA_OFFSET_ALERT_P_BTN);
		// 위치를 보정한다 }
	}
	#endregion			// 함수

	#region 클래스 제네릭 함수
	//! 알림 팝업을 생성한다
	public static T CreateAlertPopup<T>(string a_oName,
		GameObject a_oOrigin, GameObject a_oParent, Dictionary<string, string> a_oDataList, System.Action<CAlertPopup, bool> a_oCallback) where T : CAlertPopup {
		var oAlertPopup = CPopup.CreatePopup<T>(a_oName, a_oOrigin, a_oParent, KBDefine.POS_MIDDLE_CENTER);
		oAlertPopup?.Init(a_oDataList, a_oCallback);

		return oAlertPopup;
	}
	#endregion			// 클래스 제네릭 함수
}
