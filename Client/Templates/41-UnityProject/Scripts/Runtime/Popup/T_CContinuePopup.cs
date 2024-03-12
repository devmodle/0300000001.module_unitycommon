#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using TMPro;

/** 이어하기 팝업 */
public partial class CContinuePopup : CSubPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		PRICE_TEXT,
		ADS_BTN,
		[HideInInspector] MAX_VAL
	}

	/** 콜백 */
	public enum ECallback {
		NONE = -1,
		RETRY,
		CONTINUE,
		FINISH,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public int m_nContinueTimes;
		public int m_nAdsContinueTimes;

		public Dictionary<ECallback, System.Action<CContinuePopup>> m_oCallbackDict;
	}

	#region 변수
	[Header("=====> UIs <=====")]
	private Dictionary<EKey, TMP_Text> m_oTMPTextDict = new Dictionary<EKey, TMP_Text>();
	private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>();
	#endregion // 변수

	#region 프로퍼티
	public STParams Params { get; private set; }
	public bool IsWatchRewardAds { get; private set; } = false;

	public override bool IsEnableCloseBtn => false;
	public EItemKinds ContinueItemKinds => (EItemKinds)Mathf.Min((int)EItemKinds.CONSUMABLE_ITEM_GAME_CONTINUE_MAX_VAL - KCDefine.B_VAL_1_INT, (int)EItemKinds.CONSUMABLE_ITEM_GAME_CONTINUE_01 + this.Params.m_nContinueTimes);
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.SetIsEnableNavStackEvent(false);

		// 텍스트를 설정한다
		CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
			(EKey.PRICE_TEXT, $"{EKey.PRICE_TEXT}", this.ContentsUIs)
		}, m_oTMPTextDict);

		// 버튼을 설정한다 {
		CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
			(KCDefine.U_OBJ_N_RETRY_BTN, this.ContentsUIs, this.OnTouchRetryBtn),
			(KCDefine.U_OBJ_N_CONTINUE_BTN, this.ContentsUIs, this.OnTouchContinueBtn),
			(KCDefine.U_OBJ_N_FINISH_BTN, this.ContentsUIs, this.OnTouchFinishBtn)
		});

		CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
			(EKey.ADS_BTN, $"{EKey.ADS_BTN}", this.ContentsUIs, this.OnTouchAdsBtn)
		}, m_oBtnDict);
		// 버튼을 설정한다 }

		this.SubAwake();
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();
		this.Params = a_stParams;

		this.SubInit();
	}

	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		var stItemTradeInfo = CItemInfoTable.Inst.GetBuyItemTradeInfo(this.ContinueItemKinds);

		// 텍스트를 갱신한다 {
		var oTextKeyInfoList = new List<(EKey, ETargetKinds, EItemKinds)>() {
			(EKey.PRICE_TEXT, ETargetKinds.ITEM_TARGET_NUMS, EItemKinds.GOODS_ITEM_COINS_01)
		};

		for(int i = 0; i < oTextKeyInfoList.Count; ++i) {
			m_oTMPTextDict.ExGetVal(oTextKeyInfoList[i].Item1)?.ExSetText($"{stItemTradeInfo.m_oPayTargetInfoDict.ExGetTargetVal(oTextKeyInfoList[i].Item2, (int)oTextKeyInfoList[i].Item3)}", EFontSet._1, false);
		}
		// 텍스트를 갱신한다 }

		// 버튼을 갱신한다 {
		m_oBtnDict[EKey.ADS_BTN]?.ExSetInteractable(this.Params.m_nAdsContinueTimes < KDefine.PS_MAX_TIMES_ADS_CONTINUE);

#if ADS_MODULE_ENABLE
		// 광고 이어하기가 불가능 할 경우
		if(this.Params.m_nAdsContinueTimes >= KDefine.PS_MAX_TIMES_ADS_CONTINUE) {
			m_oBtnDict[EKey.ADS_BTN]?.gameObject.ExRemoveComponent<CRewardAdsTouchInteractable>();
		}
#endif // #if ADS_MODULE_ENABLE
		// 버튼을 갱신한다 }

		this.SubUpdateUIsState();
	}

	/** 닫기 버튼을 눌렀을 경우 */
	protected override void OnTouchCloseBtn() {
		base.OnTouchCloseBtn();
		this.OnTouchFinishBtn();
	}

	/** 광고 버튼을 눌렀을 경우 */
	private void OnTouchAdsBtn() {
#if ADS_MODULE_ENABLE
		Func.ShowRewardAds(this.OnCloseRewardAds);
#endif // #if ADS_MODULE_ENABLE
	}

	/** 재시도 버튼을 눌렀을 경우 */
	private void OnTouchRetryBtn() {
		this.Params.m_oCallbackDict?.ExGetVal(ECallback.RETRY)?.Invoke(this);
	}

	/** 이어하기 버튼을 눌렀을 경우 */
	private void OnTouchContinueBtn() {
		var stItemTradeInfo = CItemInfoTable.Inst.GetBuyItemTradeInfo(this.ContinueItemKinds);

		// 교환이 불가능 할 경우
		if(!Access.IsEnableTrade(CGameInfoStorage.Inst.PlayCharacterID, stItemTradeInfo.m_oPayTargetInfoDict)) {
			CSceneManager.GetSceneManager<OverlayScene.CSubOverlaySceneManager>()?.ShowStorePopup();
		} else {
			Func.Trade(CGameInfoStorage.Inst.PlayCharacterID, stItemTradeInfo);
			Func.SaveInfoStorages();

			this.Params.m_oCallbackDict?.ExGetVal(ECallback.CONTINUE)?.Invoke(this);
		}
	}

	/** 그만두기 버튼을 눌렀을 경우 */
	private void OnTouchFinishBtn() {
		this.Params.m_oCallbackDict?.ExGetVal(ECallback.FINISH)?.Invoke(this);
	}
	#endregion // 함수

	#region 조건부 함수
#if ADS_MODULE_ENABLE
	/** 보상 광고가 닫혔을 경우 */
	private void OnCloseRewardAds(CAdsManager a_oSender, STAdsRewardInfo a_stAdsRewardInfo, bool a_bIsSuccess) {
		// 광고 시청에 실패했을 경우
		if(!a_bIsSuccess) {
			return;
		}

		this.IsWatchRewardAds = true;
		this.Params.m_oCallbackDict?.ExGetVal(ECallback.CONTINUE)?.Invoke(this);
	}
#endif // #if ADS_MODULE_ENABLE
#endregion // 조건부 함수
}

/** 이어하기 팝업 - 팩토리 */
public partial class CContinuePopup : CSubPopup {
	#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(int a_nContinueTimes, int a_nAdsContinueTimes, Dictionary<ECallback, System.Action<CContinuePopup>> a_oCallbackDict = null) {
		return new STParams() {
			m_nContinueTimes = a_nContinueTimes, 
			m_nAdsContinueTimes = a_nAdsContinueTimes, 
			m_oCallbackDict = a_oCallbackDict ?? new Dictionary<ECallback, System.Action<CContinuePopup>>()
		};
	}
	#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
