using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 일일 보상 팝업
public class CDailyRewardPopup : CSubPopup {
	#region 객체
	[SerializeField] private List<GameObject> m_oRewardUIsList = new List<GameObject>();
	#endregion			// 객체

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
		for(int i = 0; i < m_oRewardUIsList.Count; ++i) {
			var oRewardUIs = m_oRewardUIsList[i];
			var stDailyRewardInfo = CRewardInfoTable.Inst.GetDailyRewardInfo(ERewardKinds.DAILY_REWARD + (i + KCDefine.B_VALUE_1_INT));

			this.UpdateRewardUIsState(oRewardUIs, stDailyRewardInfo);
		}
	}

	//! 보상 UI 상태를 갱신한다
	private void UpdateRewardUIsState(GameObject a_oRewardUIs, STRewardInfo a_stRewardInfo) {
		// Do Nothing
	}

	//! 광고 버튼을 눌렀을 경우
	private void OnTouchAdsBtn() {
#if ADS_MODULE_ENABLE
		Func.ShowRewardAds(this.OnCloseRewardAds);
#endif			// #if ADS_MODULE_ENABLE
	}
	#endregion			// 함수

	#region 조건부 함수
#if ADS_MODULE_ENABLE
	//! 보상 광고가 닫혔을 경우
	private void OnCloseRewardAds(CAdsManager a_oSender, STAdsRewardItemInfo a_stRewardItemInfo, bool a_bIsSuccess) {
		// 광고를 시청했을 경우
		if(a_bIsSuccess) {
			// Do Nothing
		}
	}
#endif			// #if ADS_MODULE_ENABLE
	#endregion			// 조건부 함수
}
#endif			// #if NEVER_USE_THIS
