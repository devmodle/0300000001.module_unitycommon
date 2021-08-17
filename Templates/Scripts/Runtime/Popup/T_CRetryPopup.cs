using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 재시도 팝업
public class CRetryPopup : CSubPopup {
	//! 매개 변수
	public struct STParams {
		public int m_nRetryTimes;
		public CLevelInfo m_oLevelInfo;
	}

	//! 콜백 매개 변수
	public struct STCallbackParams {
		public System.Action<CRetryPopup> m_oRetryCallback;
		public System.Action<CRetryPopup> m_oLeaveCallback;
	}

	#region 변수
	private STParams m_stParams;
	private STCallbackParams m_stCallbackParams;
	#endregion			// 변수

	#region UI 변수
	private Text m_oPriceText = null;
	#endregion			// UI 변수

	#region 프로퍼티
	public override bool IsIgnoreCloseBtn => true;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 텍스트를 설정한다
		m_oPriceText = m_oContents.ExFindComponent<Text>(KCDefine.U_OBJ_N_PRICE_TEXT);

		// 버튼을 설정한다 {
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
	}
	
	//! 재시도 버튼을 눌렀을 경우
	private void OnTouchRetryBtn() {
		m_stCallbackParams.m_oRetryCallback?.Invoke(this);
	}

	//! 나가기 버튼을 눌렀을 경우
	private void OnTouchLeaveBtn() {
		m_stCallbackParams.m_oLeaveCallback?.Invoke(this);
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
