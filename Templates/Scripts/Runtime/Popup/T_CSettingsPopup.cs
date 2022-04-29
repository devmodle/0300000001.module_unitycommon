using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 설정 팝업 */
public partial class CSettingsPopup : CSubPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		BG_SND_BTN,
		FX_SNDS_BTN,
		VIBRATE_BTN,
		NOTI_BTN,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	/** =====> UI <===== */
	private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>() {
		[EKey.BG_SND_BTN] = null,
		[EKey.FX_SNDS_BTN] = null,
		[EKey.VIBRATE_BTN] = null,
		[EKey.NOTI_BTN] = null
	};
	#endregion			// 변수

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 버튼을 설정한다 {
		m_oBtnDict[EKey.BG_SND_BTN] = this.Contents.ExFindComponent<Button>(KCDefine.U_OBJ_N_BG_SND_BTN);
		m_oBtnDict[EKey.BG_SND_BTN]?.onClick.AddListener(this.OnTouchBGSndBtn);

		m_oBtnDict[EKey.FX_SNDS_BTN] = this.Contents.ExFindComponent<Button>(KCDefine.U_OBJ_N_FX_SNDS_BTN);
		m_oBtnDict[EKey.FX_SNDS_BTN]?.onClick.AddListener(this.OnTouchFXSndsBtn);

		m_oBtnDict[EKey.VIBRATE_BTN] = this.Contents.ExFindComponent<Button>(KCDefine.U_OBJ_N_VIBRATE_BTN);
		m_oBtnDict[EKey.VIBRATE_BTN]?.onClick.AddListener(this.OnTouchVibrateBtn);

		m_oBtnDict[EKey.NOTI_BTN] = this.Contents.ExFindComponent<Button>(KCDefine.U_OBJ_N_NOTI_BTN);
		m_oBtnDict[EKey.NOTI_BTN]?.onClick.AddListener(this.OnTouchNotiBtn);

		this.Contents.ExFindComponent<Button>(KCDefine.U_OBJ_N_REVIEW_BTN)?.onClick.AddListener(this.OnTouchReviewBtn);
		this.Contents.ExFindComponent<Button>(KCDefine.U_OBJ_N_SUPPORTS_BTN)?.onClick.AddListener(this.OnTouchSupportsBtn);
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

#if NEWTON_SOFT_JSON_MODULE_ENABLE
		CSndManager.Inst.IsMuteBGSnd = CCommonGameInfoStorage.Inst.GameInfo.IsMuteBGSnd;
		CSndManager.Inst.IsMuteFXSnds = CCommonGameInfoStorage.Inst.GameInfo.IsMuteFXSnds;

		// 버튼을 갱신한다 {
		string oBGSndImgPath = CCommonGameInfoStorage.Inst.GameInfo.IsMuteBGSnd ? KDefine.G_IMG_P_SETTINGS_P_BG_SND_OFF : KDefine.G_IMG_P_SETTINGS_P_BG_SND_ON;
		m_oBtnDict[EKey.BG_SND_BTN]?.gameObject.ExFindComponent<Image>(KCDefine.U_OBJ_N_ICON_IMG)?.ExSetSprite<Image>(CResManager.Inst.GetRes<Sprite>(oBGSndImgPath));

		string oFXSndsImgPath = CCommonGameInfoStorage.Inst.GameInfo.IsMuteFXSnds ? KDefine.G_IMG_P_SETTINGS_P_FX_SNDS_OFF : KDefine.G_IMG_P_SETTINGS_P_FX_SNDS_ON;
		m_oBtnDict[EKey.FX_SNDS_BTN]?.gameObject.ExFindComponent<Image>(KCDefine.U_OBJ_N_ICON_IMG)?.ExSetSprite<Image>(CResManager.Inst.GetRes<Sprite>(oFXSndsImgPath));

		string oVibrateImgPath = CCommonGameInfoStorage.Inst.GameInfo.IsDisableVibrate ? KDefine.G_IMG_P_SETTINGS_P_VIBRATE_OFF : KDefine.G_IMG_P_SETTINGS_P_VIBRATE_ON;
		m_oBtnDict[EKey.VIBRATE_BTN]?.gameObject.ExFindComponent<Image>(KCDefine.U_OBJ_N_ICON_IMG)?.ExSetSprite<Image>(CResManager.Inst.GetRes<Sprite>(oVibrateImgPath));

		string oNotiImgPath = CCommonGameInfoStorage.Inst.GameInfo.IsDisableNoti ? KDefine.G_IMG_P_SETTINGS_P_NOTI_OFF : KDefine.G_IMG_P_SETTINGS_P_NOTI_ON;
		m_oBtnDict[EKey.NOTI_BTN]?.gameObject.ExFindComponent<Image>(KCDefine.U_OBJ_N_ICON_IMG)?.ExSetSprite<Image>(CResManager.Inst.GetRes<Sprite>(oNotiImgPath));
		// 버튼을 갱신한다 }
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
	}

	/** 배경음 버튼을 눌렀을 경우 */
	private void OnTouchBGSndBtn() {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
		CCommonGameInfoStorage.Inst.GameInfo.IsMuteBGSnd = !CCommonGameInfoStorage.Inst.GameInfo.IsMuteBGSnd;
		CCommonGameInfoStorage.Inst.SaveGameInfo();
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE

		this.UpdateUIsState();
	}

	/** 효과음 버튼을 눌렀을 경우 */
	private void OnTouchFXSndsBtn() {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
		CCommonGameInfoStorage.Inst.GameInfo.IsMuteFXSnds = !CCommonGameInfoStorage.Inst.GameInfo.IsMuteFXSnds;
		CCommonGameInfoStorage.Inst.SaveGameInfo();
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE

		this.UpdateUIsState();
	}

	/** 진동 버튼을 눌렀을 경우 */
	private void OnTouchVibrateBtn() {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
		CCommonGameInfoStorage.Inst.GameInfo.IsDisableVibrate = !CCommonGameInfoStorage.Inst.GameInfo.IsDisableVibrate;
		CCommonGameInfoStorage.Inst.SaveGameInfo();
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE

		this.UpdateUIsState();
		CSndManager.Inst.Vibrate(KCDefine.U_DURATION_HEAVY_VIBRATE);
	}

	/** 알림 버튼을 눌렀을 경우 */
	private void OnTouchNotiBtn() {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
		CCommonGameInfoStorage.Inst.GameInfo.IsDisableNoti = !CCommonGameInfoStorage.Inst.GameInfo.IsDisableNoti;
		CCommonGameInfoStorage.Inst.SaveGameInfo();
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
		
		this.UpdateUIsState();
	}

	/** 평가 버튼을 눌렀을 경우 */
	private void OnTouchReviewBtn() {
		CUnityMsgSender.Inst.SendShowReviewMsg();
	}

	/** 지원 버튼을 눌렀을 경우 */
	private void OnTouchSupportsBtn() {
		CUnityMsgSender.Inst.SendMailMsg(CProjInfoTable.Inst.CompanyInfo.m_oSupportsMail, string.Empty, string.Empty);
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
