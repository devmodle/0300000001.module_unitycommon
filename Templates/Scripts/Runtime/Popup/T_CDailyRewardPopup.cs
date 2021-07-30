using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 일일 보상 팝업
public class CDailyRewardPopup : CSubPopup {
	#region 변수
	private bool m_bIsWatchRewardAds = false;
	#endregion			// 변수

	#region UI 변수
	private Button m_oAdsBtn = null;
	private Button m_oAcquireBtn = null;
	#endregion			// UI 변수

	#region 객체
	[SerializeField] private List<GameObject> m_oRewardUIsList = new List<GameObject>();
	#endregion			// 객체

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 버튼을 설정한다 {
		m_oAdsBtn = m_oContents.ExFindComponent<Button>(KDefine.G_OBJ_N_DAILY_RP_ADS_BTN);
		m_oAdsBtn?.onClick.AddListener(this.OnTouchAdsBtn);

		m_oAcquireBtn = m_oContents.ExFindComponent<Button>(KDefine.G_OBJ_N_DAILY_RP_ACQUIRE_BTN);
		m_oAcquireBtn?.onClick.AddListener(this.OnTouchAcquireBtn);
		// 버튼을 설정한다 }
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
		// 보상 UI 상태를 갱신한다
		for(int i = 0; i < m_oRewardUIsList.Count; ++i) {
			var oRewardUIs = m_oRewardUIsList[i];
			var stDailyRewardInfo = CRewardInfoTable.Inst.GetRewardInfo(ERewardKinds.DAILY_REWARD + (i + KCDefine.B_VAL_1_INT));

			this.UpdateRewardUIsState(oRewardUIs, stDailyRewardInfo);
		}
	}

	//! 보상 UI 상태를 갱신한다
	private void UpdateRewardUIsState(GameObject a_oRewardUIs, STRewardInfo a_stRewardInfo) {
		// Do Something
	}

	//! 광고 버튼을 눌렀을 경우
	private void OnTouchAdsBtn() {
#if ADS_MODULE_ENABLE
		Func.ShowRewardAds(this.OnCloseRewardAds);
#else
		this.ShowRewardAcquirePopup();
#endif			// #if ADS_MODULE_ENABLE
	}

	//! 획득 버튼을 눌렀을 경우
	private void OnTouchAcquireBtn() {
		this.ShowRewardAcquirePopup();
	}

	//! 보상 획득 팝업이 닫혔을 경우
	private void OnCloseRewardAcquirePopup(CPopup a_oSender) {
		CGameInfoStorage.Inst.SetupNextDailyRewardID();
		CGameInfoStorage.Inst.SaveGameInfo();
	}

	//! 보상 획득 팝업을 출력한다
	private void ShowRewardAcquirePopup() {
		var eRewardKinds = CGameInfoStorage.Inst.DailyRewardKinds;
		var stRewardInfo = CRewardInfoTable.Inst.GetRewardInfo(eRewardKinds);

		// 보상 광고를 시청했을 경우
		if(m_bIsWatchRewardAds) {
			var oItemInfoList = new List<STItemInfo>();

			for(int i = 0; i < stRewardInfo.m_oItemInfoList.Count; ++i) {
				var stItemInfo = new STItemInfo() {
					m_nNumItems = stRewardInfo.m_oItemInfoList[i].m_nNumItems * KCDefine.B_VAL_2_INT,
					m_eItemKinds = stRewardInfo.m_oItemInfoList[i].m_eItemKinds
				};

				oItemInfoList.Add(stItemInfo);
			}

			stRewardInfo.m_oItemInfoList = oItemInfoList;
		}
		
		Func.ShowRewardAcquirePopup(this.transform.parent.gameObject, (a_oSender) => {
			var stParams = new CRewardAcquirePopup.STParams() {
				m_eQuality = stRewardInfo.m_eRewardQuality,
				m_ePopupType = ERewardAcquirePopupType.DAILY,
				m_oItemInfoList = stRewardInfo.m_oItemInfoList
			};

			var oRewardAcquirePopup = a_oSender as CRewardAcquirePopup;
			oRewardAcquirePopup.Init(stParams);
		}, null, this.OnCloseRewardAcquirePopup);
	}
	#endregion			// 함수

	#region 조건부 함수
#if ADS_MODULE_ENABLE
	//! 보상 광고가 닫혔을 경우
	private void OnCloseRewardAds(CAdsManager a_oSender, STAdsRewardItemInfo a_stRewardItemInfo, bool a_bIsSuccess) {
		// 광고를 시청했을 경우
		if(a_bIsSuccess) {
			m_bIsWatchRewardAds = true;
			this.ShowRewardAcquirePopup();
		}
	}
#endif			// #if ADS_MODULE_ENABLE
	#endregion			// 조건부 함수
}
#endif			// #if NEVER_USE_THIS
