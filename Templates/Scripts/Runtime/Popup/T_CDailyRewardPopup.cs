using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
#if RUNTIME_TEMPLATES_MODULE_ENABLE
/** 일일 보상 팝업 */
public class CDailyRewardPopup : CSubPopup {
	#region 변수
	/** =====> UI <===== */
	private Button m_oAcquireBtn = null;
	private Button m_oRewardAdsBtn = null;

	/** =====> 객체 <===== */
	[SerializeField] private List<GameObject> m_oRewardUIsList = new List<GameObject>();
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
		m_oAcquireBtn = m_oContents.ExFindComponent<Button>(KCDefine.U_OBJ_N_ACQUIRE_BTN);
		m_oAcquireBtn?.onClick.AddListener(this.OnTouchAcquireBtn);

		m_oRewardAdsBtn = m_oContents.ExFindComponent<Button>(KCDefine.U_OBJ_N_REWARD_ADS_BTN);
		m_oRewardAdsBtn?.onClick.AddListener(this.OnTouchRewardAdsBtn);
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
		
		// 보상 UI 상태를 갱신한다
		for(int i = 0; i < m_oRewardUIsList.Count; ++i) {
			var oRewardUIs = m_oRewardUIsList[i];
			var stDailyRewardInfo = CRewardInfoTable.Inst.GetRewardInfo(ERewardKinds.DAILY_REWARD_SAMPLE + (i + KCDefine.B_VAL_1_INT));

			this.UpdateRewardUIsState(oRewardUIs, stDailyRewardInfo);
		}
	}

	/** 보상 UI 상태를 갱신한다 */
	private void UpdateRewardUIsState(GameObject a_oRewardUIs, STRewardInfo a_stRewardInfo) {
		// Do Something
	}

	/** 획득 버튼을 눌렀을 경우 */
	private void OnTouchAcquireBtn() {
		this.ShowRewardAcquirePopup(false);
	}

	/** 보상 광고 버튼을 눌렀을 경우 */
	private void OnTouchRewardAdsBtn() {
#if ADS_MODULE_ENABLE
		Func.ShowRewardAds(this.OnCloseRewardAds);
#endif			// #if ADS_MODULE_ENABLE
	}

	/** 보상 획득 팝업이 닫혔을 경우 */
	private void OnCloseRewardAcquirePopup(CPopup a_oSender) {
		CGameInfoStorage.Inst.SetupNextDailyRewardID();
		CGameInfoStorage.Inst.SaveGameInfo();
	}

	/** 보상 획득 팝업을 출력한다 */
	private void ShowRewardAcquirePopup(bool a_bIsWatchRewardAds) {
		var eRewardKinds = CGameInfoStorage.Inst.DailyRewardKinds;
		var stRewardInfo = CRewardInfoTable.Inst.GetRewardInfo(eRewardKinds);

		// 보상 광고 시청 모드 일 경우
		if(a_bIsWatchRewardAds) {
			var oItemInfoList = new List<STItemInfo>();

			for(int i = 0; i < stRewardInfo.m_oItemInfoList.Count; ++i) {
				oItemInfoList.Add(new STItemInfo() {
					m_nNumItems = stRewardInfo.m_oItemInfoList[i].m_nNumItems * KCDefine.B_VAL_2_INT, m_eItemKinds = stRewardInfo.m_oItemInfoList[i].m_eItemKinds
				});
			}

			stRewardInfo.m_oItemInfoList = oItemInfoList;
		}
		
		Func.ShowRewardAcquirePopup(this.transform.parent.gameObject, (a_oSender) => {
			var stParams = new CRewardAcquirePopup.STParams() {
				m_eQuality = stRewardInfo.m_eRewardQuality, m_ePopupType = ERewardAcquirePopupType.DAILY, m_oItemInfoList = stRewardInfo.m_oItemInfoList
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
			this.ShowRewardAcquirePopup(true);
		}
	}
#endif			// #if ADS_MODULE_ENABLE
	#endregion			// 조건부 함수

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if NEVER_USE_THIS
