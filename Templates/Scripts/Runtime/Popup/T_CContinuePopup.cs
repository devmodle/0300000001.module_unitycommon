using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 이어하기 팝업
public class CContinuePopup : CSubPopup {
	//! 매개 변수
	public struct STParams {
		public int m_nContinueTimes;
		public CLevelInfo m_oLevelInfo;
	}

	//! 콜백 매개 변수
	public struct STCallbackParams {
		public System.Action<CContinuePopup> m_oRetryCallback;
		public System.Action<CContinuePopup> m_oContinueCallback;
		public System.Action<CContinuePopup> m_oLeaveCallback;
	}

	#region 변수
	private STParams m_stParams;
	private STCallbackParams m_stCallbackParams;

	// =====> UI <=====
	private Text m_oNumText = null;
	private Text m_oPriceText = null;
	#endregion			// 변수

	#region 프로퍼티
	public override bool IsIgnoreCloseBtn => true;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 텍스트를 설정한다
		m_oNumText = m_oContents.ExFindComponent<Text>(KCDefine.U_OBJ_N_NUM_TEXT);
		m_oPriceText = m_oContents.ExFindComponent<Text>(KCDefine.U_OBJ_N_PRICE_TEXT);

		// 버튼을 설정한다 {
		var oRetryBtn = m_oContents.ExFindComponent<Button>(KCDefine.U_OBJ_N_RETRY_BTN);
		oRetryBtn?.onClick.AddListener(this.OnTouchRetryBtn);

		var oContinueBtn = m_oContents.ExFindComponent<Button>(KCDefine.U_OBJ_N_CONTINUE_BTN);
		oContinueBtn?.onClick.AddListener(this.OnTouchContinueBtn);

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
		var stSaleItemInfo = CSaleItemInfoTable.Inst.GetSaleItemInfo(ESaleItemKinds.GAME_ITEM_CONTINUE);

		// 텍스트를 갱신한다
		m_oNumText?.ExSetText<Text>(string.Format(KCDefine.B_TEXT_FMT_1_DIGITS, stSaleItemInfo.m_oItemInfoList[KCDefine.B_VAL_0_INT].m_nNumItems));
		m_oPriceText?.ExSetText<Text>(string.Format(KCDefine.B_TEXT_FMT_1_DIGITS, stSaleItemInfo.IntPrice));
	}
	
	//! 재시도 버튼을 눌렀을 경우
	private void OnTouchRetryBtn() {
		this.Close();
		m_stCallbackParams.m_oRetryCallback?.Invoke(this);
	}

	//! 이어하기 버튼을 눌렀을 경우
	private void OnTouchContinueBtn() {
		var stSaleItemInfo = CSaleItemInfoTable.Inst.GetSaleItemInfo(ESaleItemKinds.GAME_ITEM_CONTINUE);

		// 코인이 부족 할 경우
		if(CUserInfoStorage.Inst.UserInfo.NumCoins < stSaleItemInfo.IntPrice) {
			var oSubOverlaySceneManager = CSceneManager.GetSubSceneManager<CSubOverlaySceneManager>(KCDefine.B_SCENE_N_OVERLAY);
			oSubOverlaySceneManager.ShowStorePopup();
		} else {
			this.Close();
			Func.BuyItem(stSaleItemInfo);
			
			m_stCallbackParams.m_oContinueCallback?.Invoke(this);
		}
	}

	//! 나가기 버튼을 눌렀을 경우
	private void OnTouchLeaveBtn() {
		this.Close();
		m_stCallbackParams.m_oLeaveCallback?.Invoke(this);
	}
	#endregion			// 함수

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if NEVER_USE_THIS
