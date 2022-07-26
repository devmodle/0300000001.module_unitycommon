using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 이어하기 팝업 */
public partial class CContinuePopup : CSubPopup {
	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 텍스트를 설정한다
		CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
			(EKey.PRICE_TEXT, $"{EKey.PRICE_TEXT}", this.Contents)
		}, m_oTextDict, false);

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
			(KCDefine.U_OBJ_N_RETRY_BTN, this.Contents, this.OnTouchRetryBtn),
			(KCDefine.U_OBJ_N_CONTINUE_BTN, this.Contents, this.OnTouchContinueBtn),
			(KCDefine.U_OBJ_N_LEAVE_BTN, this.Contents, this.OnTouchLeaveBtn)
		}, false);
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();
		m_stParams = a_stParams;
	}

	/** UI 상태를 갱신한다 */
	private new void UpdateUIsState() {
		base.UpdateUIsState();
		var stPayTargetInfo = CItemInfoTable.Inst.GetItemSaleInfo(EItemKinds.CONSUMABLE_GAME_ITEM_CONTINUE).m_oPayTargetInfoDict.ExGetTargetInfo(ETargetKinds.ITEM_NUMS, (int)EItemKinds.GOODS_COINS);
		
		// 텍스트를 갱신한다
		m_oTextDict[EKey.PRICE_TEXT]?.ExSetText($"{stPayTargetInfo.m_stValInfo01.m_nVal}", EFontSet._1, false);
	}
	#endregion			// 함수
}

/** 서브 이어하기 팝업 */
public partial class CContinuePopup : CSubPopup {
	/** 서브 식별자 */
	private enum ESubKey {
		NONE = -1,
		[HideInInspector] MAX_VAL
	}

	#region 변수

	#endregion			// 변수

	#region 프로퍼티

	#endregion			// 프로퍼티

	#region 함수
	
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
