using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 무료 보상 팝업
public class CFreeRewardPopup : CSubPopup {
	#region 변수
	// =====> UI <=====
	private Button m_oAdsBtn = null;
	#endregion			// 변수

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 버튼을 설정한다
		m_oAdsBtn = m_oContents.ExFindComponent<Button>(KCDefine.U_OBJ_N_ADS_BTN);
		m_oAdsBtn?.onClick.AddListener(this.OnTouchAdsBtn);
	}
	
	//! 초기화
	public override void Init() {
		base.Init();
	}

	//! 팝업 컨텐츠를 설정한다
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}
	
	//! UI 상태를 갱신한다
	private new void UpdateUIsState() {
		base.UpdateUIsState();
	}

	//! 광고 버튼을 눌렀을 경우
	private void OnTouchAdsBtn() {
#if ADS_MODULE_ENABLE
		Func.ShowRewardAds(this.OnCloseRewardAds);
#else
		this.ShowRewardAcquirePopup();
#endif			// #if ADS_MODULE_ENABLE
	}

	//! 보상 획득 팝업이 닫혔을 경우
	private void OnCloseRewardAcquirePopup(CPopup a_oSender) {
		CGameInfoStorage.Inst.AddNumAcquireFreeRewards(KCDefine.B_VAL_1_INT);

		// 무료 보상을 모두 획득했을 경우
		if(CGameInfoStorage.Inst.GameInfo.NumAcquireFreeRewards >= KDefine.G_MAX_NUM_ACQUIRE_FREE_REWARDS) {
			CGameInfoStorage.Inst.GameInfo.PrevFreeRewardTime = System.DateTime.Today;
		}

		CGameInfoStorage.Inst.SaveGameInfo();
	}

	//! 보상 획득 팝업을 출력한다
	private void ShowRewardAcquirePopup() {
		var eRewardKinds = ERewardKinds.FREE_SAMPLE + (CGameInfoStorage.Inst.GameInfo.NumAcquireFreeRewards + KCDefine.B_VAL_1_INT);
		var stRewardInfo = CRewardInfoTable.Inst.GetRewardInfo(eRewardKinds);

		Func.ShowRewardAcquirePopup(this.transform.parent.gameObject, (a_oSender) => {
			var stParams = new CRewardAcquirePopup.STParams() {
				m_eQuality = stRewardInfo.m_eRewardQuality,
				m_ePopupType = ERewardAcquirePopupType.FREE,
				m_oItemInfoList = stRewardInfo.m_oItemInfoList
			};

			(a_oSender as CRewardAcquirePopup).Init(stParams);
		}, null, this.OnCloseRewardAcquirePopup);
	}
	#endregion			// 함수

	#region 조건부 함수
#if ADS_MODULE_ENABLE
	//! 보상 광고가 닫혔을 경우
	private void OnCloseRewardAds(CAdsManager a_oSender, STAdsRewardItemInfo a_stRewardItemInfo, bool a_bIsSuccess) {
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
#endif			// #if NEVER_USE_THIS
