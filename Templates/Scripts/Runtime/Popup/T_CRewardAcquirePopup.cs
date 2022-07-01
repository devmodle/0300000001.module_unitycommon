using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 보상 획득 팝업 */
public partial class CRewardAcquirePopup : CSubPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		ADS_BTN,
		ACQUIRE_BTN,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public partial struct STParams {
		public ERewardQuality m_eQuality;
		public ERewardAcquirePopupType m_eAgreePopup;
		
		public List<STAcquireInfo> m_oAcquireInfoList;
	}
	
	#region 변수
	private STParams m_stParams;

	/** =====> UI <===== */
	private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>();

	/** =====> 객체 <===== */
	[SerializeField] private GameObject m_oRewardUIs = null;
	[SerializeField] private List<GameObject> m_oItemUIsList = new List<GameObject>();
	#endregion			// 변수

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		this.IsIgnoreAni = true;
		this.IsIgnoreNavStackEvent = true;

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
			(EKey.ADS_BTN, $"{EKey.ADS_BTN}", this.Contents, this.OnTouchAdsBtn),
			(EKey.ACQUIRE_BTN, $"{EKey.ACQUIRE_BTN}", this.Contents, this.OnTouchAcquireBtn)
		}, m_oBtnDict, false);
	}
	
	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();
		m_stParams = a_stParams;
	}

	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}
	
	/** UI 상태를 갱신한다 */
	private new void UpdateUIsState() {
		// 보상 아이템 UI 상태를 갱신한다
		for(int i = 0; i < m_oItemUIsList.Count; ++i) {
			var oItemUIs = m_oItemUIsList[i];
			oItemUIs.SetActive(i < m_stParams.m_oAcquireInfoList.Count);
			
			// 보상 정보가 존재 할 경우
			if(i < m_stParams.m_oAcquireInfoList.Count) {
				this.UpdateItemUIsState(oItemUIs, m_stParams.m_oAcquireInfoList[i]);
			}
		}
	}

	/** 보상 아이템 UI 상태를 갱신한다 */
	private void UpdateItemUIsState(GameObject a_oItemUIs, STAcquireInfo a_stAcquireInfo) {
		var oNumText = a_oItemUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_NUM_TEXT);
		oNumText?.ExSetText(string.Format(KCDefine.B_TEXT_FMT_CROSS, a_stAcquireInfo.m_nAcquireVal), EFontSet._1, false);
	}

	/** 광고 버튼을 눌렀을 경우 */
	private void OnTouchAdsBtn() {
#if ADS_MODULE_ENABLE
		Func.ShowRewardAds(this.OnCloseRewardAds);
#endif			// #if ADS_MODULE_ENABLE
	}

	/** 획득 버튼을 눌렀을 경우 */
	private void OnTouchAcquireBtn() {
		this.AcquireRewards(false);
	}

	/** 보상을 획득한다 */
	private void AcquireRewards(bool a_bIsWatchRewardAds) {
		m_oBtnDict[EKey.ADS_BTN]?.ExSetInteractable(false);
		m_oBtnDict[EKey.ACQUIRE_BTN]?.ExSetInteractable(false);

#if ADS_MODULE_ENABLE
		m_oBtnDict[EKey.ADS_BTN]?.gameObject.ExRemoveComponent<CRewardAdsTouchInteractable>();
#endif			// #if ADS_MODULE_ENABLE

		for(int i = 0; i < m_stParams.m_oAcquireInfoList.Count; ++i) {
			Func.Acquire(m_stParams.m_oAcquireInfoList[i], a_bIsWatchRewardAds ? m_stParams.m_oAcquireInfoList[i].m_nAcquireVal : KCDefine.B_VAL_0_INT);
		}

		this.OnTouchCloseBtn();
	}
	#endregion			// 함수

	#region 조건부 함수
#if ADS_MODULE_ENABLE
	/** 보상 광고가 닫혔을 경우 */
	private void OnCloseRewardAds(CAdsManager a_oSender, STAdsRewardInfo a_stAdsRewardInfo, bool a_bIsSuccess) {
		// 광고를 시청했을 경우
		if(a_bIsSuccess) {
			this.AcquireRewards(true);
		}
	}
#endif			// #if ADS_MODULE_ENABLE
	#endregion			// 조건부 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
