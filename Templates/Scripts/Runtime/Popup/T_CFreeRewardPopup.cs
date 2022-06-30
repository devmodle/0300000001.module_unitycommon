using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 무료 보상 팝업 */
public partial class CFreeRewardPopup : CSubPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		ADS_BTN,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	/** =====> UI <===== */
	private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>();
	#endregion			// 변수

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
			(EKey.ADS_BTN, $"{EKey.ADS_BTN}", this.Contents, this.OnTouchAdsBtn)
		}, m_oBtnDict, false);
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

		// 버튼을 갱신한다
		m_oBtnDict[EKey.ADS_BTN]?.ExSetInteractable(CGameInfoStorage.Inst.IsEnableGetFreeReward);
	}

	/** 광고 버튼을 눌렀을 경우 */
	private void OnTouchAdsBtn() {
#if ADS_MODULE_ENABLE
		Func.ShowRewardAds(this.OnCloseRewardAds);
#endif			// #if ADS_MODULE_ENABLE
	}

	/** 보상 획득 팝업이 닫혔을 경우 */
	private void OnCloseRewardAcquirePopup(CPopup a_oSender) {
		CGameInfoStorage.Inst.AddFreeRewardAcquireTimes(KCDefine.B_VAL_1_INT);

		// 무료 보상을 모두 획득했을 경우
		if(CGameInfoStorage.Inst.GameInfo.FreeRewardAcquireTimes >= KDefine.G_MAX_TIMES_ACQUIRE_FREE_REWARDS) {
			CGameInfoStorage.Inst.GameInfo.PrevFreeRewardTime = System.DateTime.Today;
		}
		
		CGameInfoStorage.Inst.SaveGameInfo();
	}

	/** 보상 획득 팝업을 출력한다 */
	private void ShowRewardAcquirePopup() {
		var eRewardKinds = ERewardKinds.FREE_COINS + (CGameInfoStorage.Inst.GameInfo.FreeRewardAcquireTimes + KCDefine.B_VAL_1_INT);
		var stRewardInfo = CRewardInfoTable.Inst.GetRewardInfo(eRewardKinds);

		Func.ShowRewardAcquirePopup(this.transform.parent.gameObject, (a_oSender) => {
			var stParams = new CRewardAcquirePopup.STParams() {
				m_eQuality = stRewardInfo.m_eRewardQuality, m_eAgreePopup = ERewardAcquirePopupType.FREE, m_oAcquireInfoList = stRewardInfo.m_oAcquireInfoList
			};

			(a_oSender as CRewardAcquirePopup).Init(stParams);
		}, null, this.OnCloseRewardAcquirePopup);
	}
	#endregion			// 함수

	#region 조건부 함수
#if ADS_MODULE_ENABLE
	/** 보상 광고가 닫혔을 경우 */
	private void OnCloseRewardAds(CAdsManager a_oSender, STAdsRewardInfo a_stAdsRewardInfo, bool a_bIsSuccess) {
		// 광고를 시청했을 경우
		if(a_bIsSuccess) {
			this.ShowRewardAcquirePopup();
		}
	}
#endif			// #if ADS_MODULE_ENABLE
	#endregion			// 조건부 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
