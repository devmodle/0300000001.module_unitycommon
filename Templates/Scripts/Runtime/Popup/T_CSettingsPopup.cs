using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 설정 팝업
public class CSettingsPopup : CSubPopup {
	#region UI 변수
	private Button m_oBGSndBtn = null;
	private Button m_oFXSndsBtn = null;
	private Button m_oNotiBtn = null;
	#endregion			// UI 변수

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 버튼을 설정한다 {
		m_oBGSndBtn = m_oContents.ExFindComponent<Button>(KDefine.G_OBJ_N_SETTINGS_P_BG_SND_BTN);
		m_oBGSndBtn?.onClick.AddListener(this.OnTouchBGSndBtn);

		m_oFXSndsBtn = m_oContents.ExFindComponent<Button>(KDefine.G_OBJ_N_SETTINGS_P_FX_SNDS_BTN);
		m_oFXSndsBtn?.onClick.AddListener(this.OnTouchFXSndsBtn);

		m_oNotiBtn = m_oContents.ExFindComponent<Button>(KDefine.G_OBJ_N_SETTINGS_P_NOTI_BTN);
		m_oNotiBtn?.onClick.AddListener(this.OnTouchNotiBtn);

		var oReviewBtn = m_oContents.ExFindComponent<Button>(KDefine.G_OBJ_N_SETTINGS_P_REVIEW_BTN);
		oReviewBtn?.onClick.AddListener(this.OnTouchReviewBtn);

		var oSupportsBtn = m_oContents.ExFindComponent<Button>(KDefine.G_OBJ_N_SETTINGS_P_SUPPORTS_BTN);
		oSupportsBtn?.onClick.AddListener(this.OnTouchSupportsBtn);
		// 버튼을 설정한다 }
	}
	
	//! 초기화
	public override void Init() {
		base.Init();		
		this.UpdateUIsState();
	}

	//! UI 상태를 갱신한다
	private void UpdateUIsState() {
		// Do Nothing
	}

	//! 배경음 버튼을 눌렀을 경우
	private void OnTouchBGSndBtn() {
		CCommonGameInfoStorage.Inst.GameInfo.IsMuteBGSnd = !CCommonGameInfoStorage.Inst.GameInfo.IsMuteBGSnd;
		this.UpdateUIsState();
	}

	//! 효과음 버튼을 눌렀을 경우
	private void OnTouchFXSndsBtn() {
		CCommonGameInfoStorage.Inst.GameInfo.IsMuteFXSnds = !CCommonGameInfoStorage.Inst.GameInfo.IsMuteFXSnds;
		this.UpdateUIsState();
	}

	//! 알림 버튼을 눌렀을 경우
	private void OnTouchNotiBtn() {
		CCommonGameInfoStorage.Inst.GameInfo.IsDisableNoti = !CCommonGameInfoStorage.Inst.GameInfo.IsDisableNoti;
		this.UpdateUIsState();
	}

	//! 평가 버튼을 눌렀을 경우
	private void OnTouchReviewBtn() {
		CUnityMsgSender.Inst.SendShowReviewMsg();
	}

	//! 지원 버튼을 눌렀을 경우
	private void OnTouchSupportsBtn() {
		CUnityMsgSender.Inst.SendMailMsg(CProjInfoTable.Inst.ProjInfo.m_oSupportsMail, string.Empty, string.Empty);
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
