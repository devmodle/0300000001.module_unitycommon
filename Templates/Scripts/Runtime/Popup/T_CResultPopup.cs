using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 결과 팝업
public class CResultPopup : CSubPopup {
	//! 매개 변수
	public struct STParams {
		public bool m_bIsClear;
		public int m_nScore;

		public CLevelInfo m_oLevelInfo;
		public CClearInfo m_oClearInfo;
	}

	//! 콜백 매개 변수
	public struct STCallbackParams {
		public System.Action<CResultPopup> m_oNextCallback;
		public System.Action<CResultPopup> m_oRetryCallback;
		public System.Action<CResultPopup> m_oLeaveCallback;
	}

	#region 변수
	private STParams m_stParams;
	private STCallbackParams m_stCallbackParams;

	// UI
	private Text m_oScoreText = null;

	// 객체
	private GameObject m_oClearUIs = null;
	private GameObject m_oClearFailUIs = null;
	#endregion			// 변수

	#region 프로퍼티
	public override bool IsIgnoreCloseBtn => true;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		this.IsIgnoreNavStackEvent = true;

		m_oClearUIs = m_oContents.ExFindChild(KCDefine.U_OBJ_N_CLEAR_UIS);
		m_oClearFailUIs = m_oContents.ExFindChild(KCDefine.U_OBJ_N_CLEAR_FAIL_UIS);

		// 텍스트를 설정한다
		m_oScoreText = m_oContents.ExFindComponent<Text>(KCDefine.U_OBJ_N_SCORE_TEXT);

		// 버튼을 설정한다 {
		var oNextBtn = m_oContents.ExFindComponent<Button>(KCDefine.U_OBJ_N_NEXT_BTN);
		oNextBtn?.onClick.AddListener(this.OnTouchNextBtn);

		var oRetryBtn = m_oContents.ExFindComponent<Button>(KCDefine.U_OBJ_N_RETRY_BTN);
		oRetryBtn?.onClick.AddListener(this.OnTouchRetryBtn);

		var oLeaveBtn = m_oContents.ExFindComponent<Button>(KCDefine.U_OBJ_N_LEAVE_BTN);
		oLeaveBtn?.onClick.AddListener(this.OnTouchLeaveBtn);
		// 버튼을 설정한다 }
	}

	//! 초기화
	public virtual void Init(STParams a_stParams, STCallbackParams a_stCallbackParams) {
		base.Init();

		m_stParams = a_stParams;
		m_stCallbackParams = a_stCallbackParams;
	}

	//! 팝업 컨텐츠를 설정한다
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}

	//! 닫기 버튼을 눌렀을 경우
	protected override void OnTouchCloseBtn() {
		base.OnTouchCloseBtn();
		this.OnTouchLeaveBtn();
	}
	
	//! UI 상태를 갱신한다
	private new void UpdateUIsState() {
		base.UpdateUIsState();

		m_oClearUIs?.SetActive(m_stParams.m_bIsClear);
		m_oClearFailUIs?.SetActive(!m_stParams.m_bIsClear);

		// 텍스트를 갱신한다
		m_oScoreText?.ExSetText<Text>(string.Format(KCDefine.B_TEXT_FMT_CURRENCY, m_stParams.m_nScore), false);
	}

	//! 다음 버튼을 눌렀을 경우
	private void OnTouchNextBtn() {
		this.Close();
		m_stCallbackParams.m_oNextCallback?.Invoke(this);
	}

	//! 재시도 버튼을 눌렀을 경우
	private void OnTouchRetryBtn() {
		this.Close();
		m_stCallbackParams.m_oRetryCallback?.Invoke(this);
	}

	//! 나가기 버튼을 눌렀을 경우
	private void OnTouchLeaveBtn() {
		this.Close();
		m_stCallbackParams.m_oLeaveCallback?.Invoke(this);
	}
	#endregion			// 함수

	#region 추가 변수

	#endregion			// 추가 변수

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if NEVER_USE_THIS
