using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 설정 팝업
public class CSettingsPopup : CSubPopup {
	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
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
