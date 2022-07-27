#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 무료 보상 팝업 */
public partial class CFreeRewardPopup : CSubPopup {
	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
			(EKey.ADS_BTN, $"{EKey.ADS_BTN}", this.Contents, this.OnTouchAdsBtn)
		}, m_oBtnDict, false);
	}
	
	/** 초기화 */
	public override void Init() {
		base.Init();
	}

	/** UI 상태를 갱신한다 */
	private new void UpdateUIsState() {
		base.UpdateUIsState();

		// 버튼을 갱신한다
		m_oBtnDict[EKey.ADS_BTN]?.ExSetInteractable(CGameInfoStorage.Inst.IsEnableGetFreeReward);
	}
	#endregion			// 함수
}

/** 서브 무료 보상 팝업 */
public partial class CFreeRewardPopup : CSubPopup {
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
