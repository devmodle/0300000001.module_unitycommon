using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if RUNTIME_TEMPLATES_MODULE_ENABLE
/** 재개 팝업 */
public class CResumePopup : CSubPopup {
	/** 콜백 */
	public enum ECallback {
		NONE = -1,
		RESUME,
		LEAVE,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public Dictionary<ECallback, System.Action<CResumePopup>> m_oCallbackDict;
	}

	#region 변수
	private STParams m_stParams;
	#endregion			// 변수

	#region 함수
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
		base.UpdateUIsState();
	}

	/** 재개 버튼을 눌렀을 경우 */
	private void OnTouchResumeBtn() {
		m_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.RESUME)?.Invoke(this);
	}

	/** 나가기 버튼을 눌렀을 경우 */
	private void OnTouchLeaveBtn() {
		m_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.LEAVE)?.Invoke(this);
	}
	#endregion			// 함수
}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
