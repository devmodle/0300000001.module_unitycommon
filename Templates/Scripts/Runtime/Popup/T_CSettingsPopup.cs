using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if RUNTIME_TEMPLATES_MODULE_ENABLE
/** 설정 팝업 */
public class CSettingsPopup : CSubPopup {
	#region 변수
	/** =====> UI <===== */
	private Button m_oBGSndBtn = null;
	private Button m_oFXSndsBtn = null;
	private Button m_oNotiBtn = null;
	#endregion			// 변수

	#region 추가 변수

	#endregion			// 추가 변수

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 버튼을 설정한다 {
		m_oBGSndBtn = m_oContents.ExFindComponent<Button>(KCDefine.U_OBJ_N_BG_SND_BTN);
		m_oBGSndBtn?.onClick.AddListener(this.OnTouchBGSndBtn);

		m_oFXSndsBtn = m_oContents.ExFindComponent<Button>(KCDefine.U_OBJ_N_FX_SNDS_BTN);
		m_oFXSndsBtn?.onClick.AddListener(this.OnTouchFXSndsBtn);

		m_oNotiBtn = m_oContents.ExFindComponent<Button>(KCDefine.U_OBJ_N_NOTI_BTN);
		m_oNotiBtn?.onClick.AddListener(this.OnTouchNotiBtn);

		m_oContents.ExFindComponent<Button>(KCDefine.U_OBJ_N_REVIEW_BTN)?.onClick.AddListener(this.OnTouchReviewBtn);
		m_oContents.ExFindComponent<Button>(KCDefine.U_OBJ_N_SUPPORTS_BTN)?.onClick.AddListener(this.OnTouchSupportsBtn);
		// 버튼을 설정한다 }
	}
	
	/** 초기화 */
	public override void Init() {
		base.Init();
	}

	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}
	
	/** UI 상태를 갱신한다 */
	private new void UpdateUIsState() {
		base.UpdateUIsState();

		CSndManager.Inst.IsMuteBGSnd = CCommonGameInfoStorage.Inst.GameInfo.IsMuteBGSnd;
		CSndManager.Inst.IsMuteFXSnds = CCommonGameInfoStorage.Inst.GameInfo.IsMuteFXSnds;

		// 버튼을 갱신한다 {
		string oBGSndImgPath = CCommonGameInfoStorage.Inst.GameInfo.IsMuteBGSnd ? KDefine.G_IMG_P_SETTINGS_P_BG_SND_OFF : KDefine.G_IMG_P_SETTINGS_P_BG_SND_ON;
		m_oBGSndBtn?.gameObject.ExFindComponent<Image>(KCDefine.U_OBJ_N_ICON_IMG)?.ExSetSprite<Image>(CResManager.Inst.GetRes<Sprite>(oBGSndImgPath));

		string oFXSndsImgPath = CCommonGameInfoStorage.Inst.GameInfo.IsMuteFXSnds ? KDefine.G_IMG_P_SETTINGS_P_FX_SNDS_OFF : KDefine.G_IMG_P_SETTINGS_P_FX_SNDS_ON;
		m_oFXSndsBtn?.gameObject.ExFindComponent<Image>(KCDefine.U_OBJ_N_ICON_IMG)?.ExSetSprite<Image>(CResManager.Inst.GetRes<Sprite>(oFXSndsImgPath));

		string oNotiImgPath = CCommonGameInfoStorage.Inst.GameInfo.IsDisableNoti ? KDefine.G_IMG_P_SETTINGS_P_NOTI_OFF : KDefine.G_IMG_P_SETTINGS_P_NOTI_ON;
		m_oNotiBtn?.gameObject.ExFindComponent<Image>(KCDefine.U_OBJ_N_ICON_IMG)?.ExSetSprite<Image>(CResManager.Inst.GetRes<Sprite>(oNotiImgPath));
		// 버튼을 갱신한다 }
	}

	/** 배경음 버튼을 눌렀을 경우 */
	private void OnTouchBGSndBtn() {
		CCommonGameInfoStorage.Inst.GameInfo.IsMuteBGSnd = !CCommonGameInfoStorage.Inst.GameInfo.IsMuteBGSnd;
		CCommonGameInfoStorage.Inst.SaveGameInfo();

		this.UpdateUIsState();
	}

	/** 효과음 버튼을 눌렀을 경우 */
	private void OnTouchFXSndsBtn() {
		CCommonGameInfoStorage.Inst.GameInfo.IsMuteFXSnds = !CCommonGameInfoStorage.Inst.GameInfo.IsMuteFXSnds;
		CCommonGameInfoStorage.Inst.SaveGameInfo();

		this.UpdateUIsState();
	}

	/** 알림 버튼을 눌렀을 경우 */
	private void OnTouchNotiBtn() {
		CCommonGameInfoStorage.Inst.GameInfo.IsDisableNoti = !CCommonGameInfoStorage.Inst.GameInfo.IsDisableNoti;
		CCommonGameInfoStorage.Inst.SaveGameInfo();
		
		this.UpdateUIsState();
	}

	/** 평가 버튼을 눌렀을 경우 */
	private void OnTouchReviewBtn() {
		CUnityMsgSender.Inst.SendShowReviewMsg();
	}

	/** 지원 버튼을 눌렀을 경우 */
	private void OnTouchSupportsBtn() {
		CUnityMsgSender.Inst.SendMailMsg(CProjInfoTable.Inst.ProjInfo.m_oSupportsMail, string.Empty, string.Empty);
	}
	#endregion			// 함수

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
