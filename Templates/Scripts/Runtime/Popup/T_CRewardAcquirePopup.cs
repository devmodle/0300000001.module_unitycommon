using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 보상 획득 팝업
public class CRewardAcquirePopup : CSubPopup {
	//! 매개 변수
	public struct STParams {
		public ERewardQuality m_eQuality;
		public ERewardAcquirePopupType m_ePopupType;
		
		public List<STItemInfo> m_oItemInfoList;
	}

	#region 변수
	private STParams m_stParams;
	private bool m_bIsWatchRewardAds = false;

	// UI
	private Button m_oAdsBtn = null;
	private Button m_oAcquireBtn = null;

	// 객체
	[SerializeField] private GameObject m_oRewardUIs = null;
	[SerializeField] private List<GameObject> m_oItemUIsList = new List<GameObject>();
	#endregion			// 변수

	#region 프로퍼티
	public override bool IsIgnoreAni => true;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		this.IsIgnoreNavStackEvent = true;

		// 버튼을 설정한다 {
		m_oAdsBtn = m_oContents.ExFindComponent<Button>(KCDefine.U_OBJ_N_ADS_BTN);
		m_oAdsBtn?.onClick.AddListener(this.OnTouchAdsBtn);

		m_oAcquireBtn = m_oContents.ExFindComponent<Button>(KCDefine.U_OBJ_N_ACQUIRE_BTN);
		m_oAcquireBtn?.onClick.AddListener(this.OnTouchAcquireBtn);
		// 버튼을 설정한다 }
	}
	
	//! 초기화
	public virtual void Init(STParams a_stParams) {
		base.Init();
		m_stParams = a_stParams;
	}

	//! 팝업 컨텐츠를 설정한다
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}
	
	//! UI 상태를 갱신한다
	private new void UpdateUIsState() {
		// 보상 아이템 UI 상태를 갱신한다
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
		var oNumText = a_oItemUIs.ExFindComponent<Text>(KCDefine.U_OBJ_N_NUM_TEXT);
		oNumText?.ExSetText<Text>(string.Format(KCDefine.B_TEXT_FMT_NUM, a_stItemInfo.m_nNumItems));
	}

	//! 광고 버튼을 눌렀을 경우
	private void OnTouchAdsBtn() {
#if ADS_MODULE_ENABLE
		Func.ShowRewardAds(this.OnCloseRewardAds);
#endif			// #if ADS_MODULE_ENABLE
	}

	//! 획득 버튼을 눌렀을 경우
	private void OnTouchAcquireBtn() {
		this.AcquireItems();
	}

	//! 아이템을 획득한다
	private void AcquireItems() {
		m_oAdsBtn?.ExSetInteractable(false);
		m_oAcquireBtn?.ExSetInteractable(false);

#if ADS_MODULE_ENABLE
		m_oAdsBtn?.gameObject.ExRemoveComponent<CRewardAdsTouchInteractable>();
#endif			// #if ADS_MODULE_ENABLE

		for(int i = 0; i < m_stParams.m_oItemInfoList.Count; ++i) {
			Func.AcquireItem(m_stParams.m_oItemInfoList[i], m_bIsWatchRewardAds ? m_stParams.m_oItemInfoList[i].m_nNumItems : KCDefine.B_VAL_0_INT);
		}

		this.OnTouchCloseBtn();
	}
	#endregion			// 함수

	#region 조건부 함수
#if ADS_MODULE_ENABLE
	//! 보상 광고가 닫혔을 경우
	private void OnCloseRewardAds(CAdsManager a_oSender, STAdsRewardItemInfo a_stRewardItemInfo, bool a_bIsSuccess) {
		// 광고를 시청했을 경우
		if(a_bIsSuccess) {
			m_bIsWatchRewardAds = a_bIsSuccess;
			this.AcquireItems();
		}
	}
#endif			// #if ADS_MODULE_ENABLE
	#endregion			// 조건부 함수

	#region 추가 변수

	#endregion			// 추가 변수

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if NEVER_USE_THIS
