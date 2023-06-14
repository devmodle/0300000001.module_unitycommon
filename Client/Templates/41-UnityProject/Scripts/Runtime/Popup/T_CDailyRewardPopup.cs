#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
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
	[Header("=====> UIs <=====")]
	private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>();

	[Header("=====> Game Objects <=====")]
	[SerializeField] private List<GameObject> m_oRewardUIsList = new List<GameObject>();
	#endregion // 변수

	#region 프로퍼티
	public override bool IsIgnoreCloseBtn => true;
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.SetIgnoreNavStackEvent(true);

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
			(EKey.ADS_BTN, $"{EKey.ADS_BTN}", this.ContentsUIs, this.OnTouchAdsBtn),
			(EKey.ACQUIRE_BTN, $"{EKey.ACQUIRE_BTN}", this.ContentsUIs, this.OnTouchAcquireBtn)
		}, m_oBtnDict);

		this.SubAwake();
	}

	/** 초기화 */
	public override void Init() {
		base.Init();
		this.SubInit();
	}

	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		// 버튼을 갱신한다
		m_oBtnDict[EKey.ADS_BTN]?.ExSetInteractable(Access.IsEnableGetDailyReward(CGameInfoStorage.Inst.PlayCharacterID));
		m_oBtnDict[EKey.ACQUIRE_BTN]?.ExSetInteractable(Access.IsEnableGetDailyReward(CGameInfoStorage.Inst.PlayCharacterID));

		// 보상 UI 상태를 갱신한다
		for(int i = 0; i < m_oRewardUIsList.Count; ++i) {
			var oRewardUIs = m_oRewardUIsList[i];
			var stDailyRewardInfo = CRewardInfoTable.Inst.GetRewardInfo(ERewardKinds.DAILY_REWARD_ATTENDANCE_01 + i);

			this.UpdateRewardUIsState(oRewardUIs, stDailyRewardInfo);
		}

		this.SubUpdateUIsState();
	}

	/** 광고 버튼을 눌렀을 경우 */
	private void OnTouchAdsBtn() {
#if ADS_MODULE_ENABLE
		Func.ShowRewardAds(this.OnCloseRewardAds);
#endif // #if ADS_MODULE_ENABLE
	}

	/** 획득 버튼을 눌렀을 경우 */
	private void OnTouchAcquireBtn() {
		this.ShowRewardAcquirePopup(false);
	}

	/** 보상 획득 팝업이 닫혔을 경우 */
	private void OnCloseRewardAcquirePopup(CPopup a_oSender) {
		Func.SetupNextDailyRewardID(CGameInfoStorage.Inst.PlayCharacterID);
		CGameInfoStorage.Inst.SaveGameInfo();

		this.Close();
	}

	/** 보상 획득 팝업을 출력한다 */
	private void ShowRewardAcquirePopup(bool a_bIsWatchRewardAds) {
		Func.ShowRewardAcquirePopup(this.transform.parent.gameObject, (a_oSender) => {
			var oTargetInfoDict = CCollectionPoolManager.Inst.SpawnDict<ulong, STTargetInfo>();

			try {
				var eRewardKinds = Access.GetDailyRewardKinds(CGameInfoStorage.Inst.PlayCharacterID);
				var stRewardInfo = CRewardInfoTable.Inst.GetRewardInfo(eRewardKinds);

				foreach(var stKeyVal in stRewardInfo.m_oAcquireTargetInfoDict) {
					var stValInfo = new STValInfo(stKeyVal.Value.m_stValInfo01.m_eValType, a_bIsWatchRewardAds ? stKeyVal.Value.m_stValInfo01.m_dmVal * KCDefine.B_VAL_2_INT : stKeyVal.Value.m_stValInfo01.m_dmVal);
					oTargetInfoDict.TryAdd(stKeyVal.Key, new STTargetInfo(stKeyVal.Value.m_eTargetKinds, stKeyVal.Value.m_nKinds, stValInfo));
				}
				
				oTargetInfoDict.ExCopyTo(stRewardInfo.m_oAcquireTargetInfoDict, (a_stTargetInfo) => a_stTargetInfo);
				(a_oSender as CRewardAcquirePopup).Init(CRewardAcquirePopup.MakeParams(stRewardInfo.m_eRewardQuality, ERewardAcquirePopupType.DAILY, stRewardInfo.m_oAcquireTargetInfoDict));
			} finally {
				CCollectionPoolManager.Inst.DespawnDict(oTargetInfoDict);
			}
		}, null, this.OnCloseRewardAcquirePopup);
	}
	#endregion // 함수

	#region 조건부 함수
#if ADS_MODULE_ENABLE
	/** 보상 광고가 닫혔을 경우 */
	private void OnCloseRewardAds(CAdsManager a_oSender, STAdsRewardInfo a_stAdsRewardInfo, bool a_bIsSuccess) {
		// 광고를 시청했을 경우
		if(a_bIsSuccess) {
			this.ShowRewardAcquirePopup(true);
		}
	}
#endif // #if ADS_MODULE_ENABLE
	#endregion // 조건부 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
