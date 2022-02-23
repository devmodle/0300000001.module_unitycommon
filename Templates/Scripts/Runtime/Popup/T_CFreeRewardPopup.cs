using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if RUNTIME_TEMPLATES_MODULE_ENABLE
/** 무료 보상 팝업 */
public class CFreeRewardPopup : CSubPopup {
	#region 변수
	/** =====> UI <===== */
	private Button m_oRewardAdsBtn = null;
	#endregion			// 변수

	#region 추가 변수

	#endregion			// 추가 변수

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 버튼을 설정한다
		m_oRewardAdsBtn = m_oContents.ExFindComponent<Button>(KCDefine.U_OBJ_N_REWARD_ADS_BTN);
		m_oRewardAdsBtn?.onClick.AddListener(this.OnTouchRewardAdsBtn);
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
	}

	/** 보상 광고 버튼을 눌렀을 경우 */
	private void OnTouchRewardAdsBtn() {
#if ADS_MODULE_ENABLE
		Func.ShowRewardAds(this.OnCloseRewardAds);
#else
		this.ShowRewardAcquirePopup();
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
				m_eQuality = stRewardInfo.m_eRewardQuality, m_ePopupType = ERewardAcquirePopupType.FREE, m_oItemInfoList = stRewardInfo.m_oItemInfoList
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

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
