using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 일일 보상 팝업 */
public partial class CDailyRewardPopup : CSubPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		ADS_BTN,
		ACQUIRE_BTN,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	/** =====> UI <===== */
	private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>();

	/** =====> 객체 <===== */
	[SerializeField] private List<GameObject> m_oRewardUIsList = new List<GameObject>();
	#endregion			// 변수

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
			(EKey.ADS_BTN, $"{EKey.ADS_BTN}", this.Contents, this.OnTouchAdsBtn),
			(EKey.ACQUIRE_BTN, $"{EKey.ACQUIRE_BTN}", this.Contents, this.OnTouchAcquireBtn)
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
		m_oBtnDict[EKey.ADS_BTN]?.ExSetInteractable(CGameInfoStorage.Inst.IsEnableGetDailyReward);
		m_oBtnDict[EKey.ACQUIRE_BTN]?.ExSetInteractable(CGameInfoStorage.Inst.IsEnableGetDailyReward);
		
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

	/** 광고 버튼을 눌렀을 경우 */
	private void OnTouchAdsBtn() {
#if ADS_MODULE_ENABLE
		Func.ShowRewardAds(this.OnCloseRewardAds);
#endif			// #if ADS_MODULE_ENABLE
	}

	/** 획득 버튼을 눌렀을 경우 */
	private void OnTouchAcquireBtn() {
		this.ShowRewardAcquirePopup(false);
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
			var oAcquireInfoList = new List<STAcquireInfo>();

			for(int i = 0; i < stRewardInfo.m_oAcquireInfoList.Count; ++i) {
				oAcquireInfoList.Add(new STAcquireInfo() {
					m_nNumItems = stRewardInfo.m_oAcquireInfoList[i].m_nNumItems * KCDefine.B_VAL_2_INT, m_eItemKinds = stRewardInfo.m_oAcquireInfoList[i].m_eItemKinds
				});
			}

			stRewardInfo.m_oAcquireInfoList = oAcquireInfoList;
		}
		
		Func.ShowRewardAcquirePopup(this.transform.parent.gameObject, (a_oSender) => {
			var stParams = new CRewardAcquirePopup.STParams() {
				m_eQuality = stRewardInfo.m_eRewardQuality, m_eAgreePopup = ERewardAcquirePopupType.DAILY, m_oAcquireInfoList = stRewardInfo.m_oAcquireInfoList
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
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
