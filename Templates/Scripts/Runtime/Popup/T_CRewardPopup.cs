using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 보상 팝업
public class CRewardPopup : CSubPopup {
	//! 매개 변수
	public struct STParams {
		public ERewardPopupType m_ePopupType;
		public List<STItemInfo> m_oItemInfoList;
	}

	#region 변수
	private STParams m_stParams;
	#endregion			// 변수

	#region 객체
	[SerializeField] private List<GameObject> m_oItemUIsList = new List<GameObject>();
	#endregion			// 객체

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
	}
	
	//! 초기화
	public virtual void Init(STParams a_stParams) {
		base.Init();
		m_stParams = a_stParams;

		this.UpdateUIsState();
	}

	//! UI 상태를 갱신한다
	private void UpdateUIsState() {
		for(int i = 0; i < m_oItemUIsList.Count; ++i) {
			var oItemUIs = m_oItemUIsList[i];
			oItemUIs.SetActive(i < m_stParams.m_oItemInfoList.Count);

			// 보상 정보가 존재 할 경우
			if(i < m_stParams.m_oItemInfoList.Count) {
				this.UpdateItemUIsState(oItemUIs, m_stParams.m_oItemInfoList[i]);
			}
		}
	}

	//! 보상 아이템 UI 상태를 갱신한다
	private void UpdateItemUIsState(GameObject a_oItemUIs, STItemInfo a_stItemInfo) {
		// Do Nothing
	}

	//! 획득 버튼을 눌렀을 경우
	private void OnTouchGetBtn() {
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
