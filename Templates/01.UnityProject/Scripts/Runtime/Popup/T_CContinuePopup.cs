#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 이어하기 팝업 */
public partial class CContinuePopup : CSubPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		PRICE_TEXT,
		[HideInInspector] MAX_VAL
	}

	/** 콜백 */
	public enum ECallback {
		NONE = -1,
		RETRY,
		CONTINUE,
		LEAVE,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public partial struct STParams {
		public int m_nContinueTimes;
		public CLevelInfo m_oLevelInfo;
		public Dictionary<ECallback, System.Action<CContinuePopup>> m_oCallbackDict;
	}

	#region 변수
	private STParams m_stParams;

	/** =====> UI <===== */
	private Dictionary<EKey, TMP_Text> m_oTextDict = new Dictionary<EKey, TMP_Text>();
	#endregion			// 변수

	#region 프로퍼티
	public override bool IsIgnoreCloseBtn => true;
	#endregion			// 프로퍼티

	#region 함수
	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}

	/** 닫기 버튼을 눌렀을 경우 */
	protected override void OnTouchCloseBtn() {
		base.OnTouchCloseBtn();
		this.OnTouchLeaveBtn();
	}
	
	/** 재시도 버튼을 눌렀을 경우 */
	private void OnTouchRetryBtn() {
		m_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.RETRY)?.Invoke(this);
	}

	/** 이어하기 버튼을 눌렀을 경우 */
	private void OnTouchContinueBtn() {
		var stPayTargetInfo = CItemInfoTable.Inst.GetBuyItemTradeInfo(EItemKinds.CONSUMABLE_GAME_ITEM_CONTINUE).m_oPayTargetInfoDict.ExGetTargetInfo(ETargetKinds.ITEM_NUMS, (int)EItemKinds.GOODS_COINS);

		// 교환이 불가능 할 경우
		if(Access.IsEnableTrade(CGameInfoStorage.Inst.PlayCharacterID, stPayTargetInfo)) {
			CSceneManager.GetSceneManager<OverlayScene.CSubOverlaySceneManager>(KCDefine.B_SCENE_N_OVERLAY)?.ShowStorePopup();
		} else {
			Func.Acquire(CGameInfoStorage.Inst.PlayCharacterID, CItemInfoTable.Inst.GetBuyItemTradeInfo(EItemKinds.CONSUMABLE_GAME_ITEM_CONTINUE).m_oAcquireTargetInfoDict.ExGetTargetInfo(ETargetKinds.ITEM_NUMS, (int)EItemKinds.GOODS_COINS));
			m_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.CONTINUE)?.Invoke(this);
		}
	}

	/** 나가기 버튼을 눌렀을 경우 */
	private void OnTouchLeaveBtn() {
		m_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.LEAVE)?.Invoke(this);
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
