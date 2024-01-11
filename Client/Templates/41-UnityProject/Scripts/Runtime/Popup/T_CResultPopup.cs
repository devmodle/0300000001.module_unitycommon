#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.Linq;
using TMPro;

/** 결과 팝업 */
public partial class CResultPopup : CSubPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		TITLE_TEXT,
		RECORD_TEXT,
		BEST_RECORD_TEXT,

		ADS_BTN,
		ACQUIRE_BTN,

		CLEAR_UIS,
		REWARD_UIS,
		CLEAR_FAIL_UIS,
		[HideInInspector] MAX_VAL
	}

	/** 콜백 */
	public enum ECallback {
		NONE = -1,
		NEXT,
		RETRY,
		LEAVE,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public STIDInfo m_stIDInfo;
		public STRecordInfo m_stRecordInfo;

		public Dictionary<ECallback, System.Action<CResultPopup>> m_oCallbackDict;
	}

	#region 변수
	[Header("=====> UIs <=====")]
	private Dictionary<EKey, Text> m_oTextDict = new Dictionary<EKey, Text>();
	private Dictionary<EKey, TMP_Text> m_oTMPTextDict = new Dictionary<EKey, TMP_Text>();
	private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>();

	[Header("=====> Game Objects <=====")]
	private Dictionary<EKey, GameObject> m_oUIsDict = new Dictionary<EKey, GameObject>();
	#endregion // 변수

	#region 프로퍼티
	public STParams Params { get; private set; }
	public override bool IsIgnoreCloseBtn => true;
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.SetIsIgnoreNavStackEvent(true);

		// 객체를 설정한다
		CFunc.SetupGameObjs(new List<(EKey, string, GameObject)>() {
			(EKey.CLEAR_UIS, $"{EKey.CLEAR_UIS}", this.ContentsUIs),
			(EKey.REWARD_UIS, $"{EKey.REWARD_UIS}", this.ContentsUIs),
			(EKey.CLEAR_FAIL_UIS, $"{EKey.CLEAR_FAIL_UIS}", this.ContentsUIs)
		}, m_oUIsDict);

		// 텍스트를 설정한다 {
		CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
			(EKey.TITLE_TEXT, $"{EKey.TITLE_TEXT}", this.ContentsUIs),
		}, m_oTextDict);

		CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
			(EKey.RECORD_TEXT, $"{EKey.RECORD_TEXT}", this.ContentsUIs),
			(EKey.BEST_RECORD_TEXT, $"{EKey.BEST_RECORD_TEXT}", this.ContentsUIs)
		}, m_oTMPTextDict);
		// 텍스트를 설정한다 }

		// 버튼을 설정한다 {
		CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
			(KCDefine.U_OBJ_N_NEXT_BTN, this.ContentsUIs, this.OnTouchNextBtn),
			(KCDefine.U_OBJ_N_RETRY_BTN, this.ContentsUIs, this.OnTouchRetryBtn),
			(KCDefine.U_OBJ_N_LEAVE_BTN, this.ContentsUIs, this.OnTouchLeaveBtn)
		});

		CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
			(EKey.ADS_BTN, $"{EKey.ADS_BTN}", this.ContentsUIs, this.OnTouchAdsBtn),
			(EKey.ACQUIRE_BTN, $"{EKey.ACQUIRE_BTN}", this.ContentsUIs, this.OnTouchAcquireBtn),
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
		
		Func.UpdateSingleSceneUIsState();
	}

	/** 닫기 버튼을 눌렀을 경우 */
	protected override void OnTouchCloseBtn() {
		base.OnTouchCloseBtn();
		this.OnTouchLeaveBtn();
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		int nNumMarks = 0;
		var oLevelClearInfo = Access.GetLevelClearInfo(CGameInfoStorage.Inst.PlayCharacterID, this.Params.m_stIDInfo.m_nID01, this.Params.m_stIDInfo.m_nID02, this.Params.m_stIDInfo.m_nID03);

		// 클리어 정보가 존재 할 경우
		if(oLevelClearInfo != null) {
			for(int i = 0; i < CGameInfoStorage.Inst.PlayEpisodeInfo.m_oRecordValInfoList.Count; ++i) {
				nNumMarks = (CGameInfoStorage.Inst.PlayEpisodeInfo.m_oRecordValInfoList[i].m_eValType.ExIsValid() && this.Params.m_stRecordInfo.m_nIntRecord >= CGameInfoStorage.Inst.PlayEpisodeInfo.m_oRecordValInfoList[i].m_dmVal) ? i + KCDefine.B_VAL_1_INT : nNumMarks;
			}

			oLevelClearInfo.NumMarks = Mathf.Max(nNumMarks, oLevelClearInfo.NumMarks);
			CGameInfoStorage.Inst.SaveGameInfo();
		}

		// 객체를 갱신한다 {
		bool bIsSuccess = this.Params.m_stRecordInfo.m_bIsSuccess;
		bool bIsAcquireReward = Access.IsAcquireLevelReward(CGameInfoStorage.Inst.PlayCharacterID, this.Params.m_stIDInfo.m_nID01, this.Params.m_stIDInfo.m_nID02, this.Params.m_stIDInfo.m_nID03);

		m_oUIsDict[EKey.CLEAR_UIS]?.SetActive(bIsSuccess && (bIsAcquireReward || !CGameInfoStorage.Inst.PlayEpisodeInfo.IsExistsReward));
		m_oUIsDict[EKey.REWARD_UIS]?.SetActive(bIsSuccess && (!bIsAcquireReward && CGameInfoStorage.Inst.PlayEpisodeInfo.IsExistsReward));
		m_oUIsDict[EKey.CLEAR_FAIL_UIS]?.SetActive(!bIsSuccess);
		// 객체를 갱신한다 }

		// 텍스트를 갱신한다
		m_oTMPTextDict[EKey.RECORD_TEXT]?.ExSetText($"{this.Params.m_stRecordInfo.m_nIntRecord}", a_bIsEnableAssert: false);
		m_oTMPTextDict[EKey.BEST_RECORD_TEXT]?.ExSetText((oLevelClearInfo != null) ? $"{oLevelClearInfo.m_stBestRecordInfo.m_nIntRecord}" : string.Empty, a_bIsEnableAssert: false);

		this.SubUpdateUIsState();
	}

	/** 다음 버튼을 눌렀을 경우 */
	private void OnTouchNextBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.NEXT)?.Invoke(this);
	}

	/** 재시도 버튼을 눌렀을 경우 */
	private void OnTouchRetryBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.RETRY)?.Invoke(this);
	}

	/** 나가기 버튼을 눌렀을 경우 */
	private void OnTouchLeaveBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.LEAVE)?.Invoke(this);
	}

	/** 광고 버튼을 눌렀을 경우 */
	private void OnTouchAdsBtn() {
#if ADS_MODULE_ENABLE
		Func.ShowRewardAds(this.OnCloseRewardAds);
#endif // #if ADS_MODULE_ENABLE
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
#endif // #if ADS_MODULE_ENABLE

		var oRewardTargetInfoDict = CCollectionPoolManager.Inst.SpawnDict<ulong, STTargetInfo>();

		try {
			var stEpisodeInfo = CEpisodeInfoTable.Inst.GetLevelEpisodeInfo(this.Params.m_stIDInfo.m_nID01, this.Params.m_stIDInfo.m_nID02, this.Params.m_stIDInfo.m_nID03);

			for(int i = 0; i < stEpisodeInfo.m_oRewardKindsList.Count; ++i) {
				// 보상 종류가 유효 할 경우
				if(stEpisodeInfo.m_oRewardKindsList[i] != ERewardKinds.NONE) {
					var stRewardInfo = CRewardInfoTable.Inst.GetRewardInfo(stEpisodeInfo.m_oRewardKindsList[i]);

					foreach(var stKeyVal in stRewardInfo.m_oAcquireTargetInfoDict) {
						var stValInfo = new STValInfo(stKeyVal.Value.m_stValInfo01.m_eValType, a_bIsWatchRewardAds ? stKeyVal.Value.m_stValInfo01.m_dmVal * KCDefine.B_VAL_2_INT : stKeyVal.Value.m_stValInfo01.m_dmVal);
						oRewardTargetInfoDict.TryAdd(stKeyVal.Key, new STTargetInfo(stKeyVal.Value.m_eTargetKinds, stKeyVal.Value.m_nKinds, stValInfo));
					}
				}
			}

			// 캐릭터 게임 정보가 존재 할 경우
			if(CGameInfoStorage.Inst.TryGetCharacterGameInfo(CGameInfoStorage.Inst.PlayCharacterID, out CCharacterGameInfo oCharacterGameInfo)) {
				oCharacterGameInfo.m_oAcquireRewardULevelIDList.ExAddVal(this.Params.m_stIDInfo.UniqueID01);
			}

			Func.Acquire(CGameInfoStorage.Inst.PlayCharacterID, oRewardTargetInfoDict, true);
			this.UpdateUIsState();
		} finally {
			CCollectionPoolManager.Inst.DespawnDict(oRewardTargetInfoDict);
		}
	}
	#endregion // 함수

	#region 조건부 함수
#if ADS_MODULE_ENABLE
	/** 보상 광고가 닫혔을 경우 */
	private void OnCloseRewardAds(CAdsManager a_oSender, STAdsRewardInfo a_stAdsRewardInfo, bool a_bIsSuccess) {
		// 광고를 시청했을 경우
		if(a_bIsSuccess) {
			this.AcquireRewards(true);
		}
	}
#endif // #if ADS_MODULE_ENABLE
	#endregion // 조건부 함수
}

/** 결과 팝업 - 팩토리 */
public partial class CResultPopup : CSubPopup {
	#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(STIDInfo a_stIDInfo, STRecordInfo a_stRecordInfo, Dictionary<ECallback, System.Action<CResultPopup>> a_oCallbackDict = null) {
		return new STParams() {
			m_stIDInfo = a_stIDInfo, m_stRecordInfo = a_stRecordInfo, m_oCallbackDict = a_oCallbackDict ?? new Dictionary<ECallback, System.Action<CResultPopup>>()
		};
	}
	#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
