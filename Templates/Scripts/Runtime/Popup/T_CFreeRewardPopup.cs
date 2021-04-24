using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 무료 보상 팝업
public class CFreeRewardPopup : CSubPopup {
	#region UI 변수
	private Button m_oAdsBtn = null;
	#endregion			// UI 변수

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 버튼을 설정한다
		m_oAdsBtn = m_oContents.ExFindComponent<Button>(KDefine.G_OBJ_N_FREE_RP_ADS_BTN);
		m_oAdsBtn.onClick.AddListener(this.OnTouchAdsBtn);
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

	//! 광고 버튼을 눌렀을 경우
	private void OnTouchAdsBtn() {
#if ADS_MODULE_ENABLE
		Func.ShowRewardAds(this.OnCloseRewardAds);
#else
		this.ShowRewardPopup();
#endif			// #if ADS_MODULE_ENABLE
	}

	//! 보상 팝업이 닫혔을 경우
	private void OnCloseRewardPopup(CPopup a_oSender) {
		CGameInfoStorage.Inst.AddFreeRewardTimes(KCDefine.B_VAL_1_INT);

		// 무료 보상을 모두 획득했을 경우
		if(CGameInfoStorage.Inst.GameInfo.FreeRewardTimes >= CRewardInfoTable.Inst.FreeRewardInfoList.Count) {
			CGameInfoStorage.Inst.GameInfo.LastFreeRewardTime = System.DateTime.Today;
		}

		CGameInfoStorage.Inst.SaveGameInfo();
	}

	//! 보상 팝업을 출력한다
	private void ShowRewardPopup() {
		var eRewardKinds = ERewardKinds.FREE_REWARD + (CGameInfoStorage.Inst.GameInfo.FreeRewardTimes + KCDefine.B_VAL_1_INT);
		var stRewardInfo = CRewardInfoTable.Inst.GetFreeRewardInfo(eRewardKinds);

		Func.ShowRewardPopup(this.transform.parent.gameObject, (a_oPopup) => {
			var stParams = new CRewardPopup.STParams() {
				m_eQuality = ERewardQuality.NORM,
				m_ePopupType = ERewardPopupType.FREE,
				m_oItemInfoList = stRewardInfo.m_oItemInfoList
			};

			var oRewardPopup = a_oPopup as CRewardPopup;
			oRewardPopup.Init(stParams);
		}, null, this.OnCloseRewardPopup);
	}
	#endregion			// 함수

	#region 조건부 함수
#if ADS_MODULE_ENABLE
	//! 보상 광고가 닫혔을 경우
	private void OnCloseRewardAds(CAdsManager a_oSender, STAdsRewardItemInfo a_stRewardItemInfo, bool a_bIsSuccess) {
		// 광고를 시청했을 경우
		if(a_bIsSuccess) {
			this.ShowRewardPopup();
		}
	}
#endif			// #if ADS_MODULE_ENABLE
	#endregion			// 조건부 함수
}
#endif			// #if NEVER_USE_THIS
