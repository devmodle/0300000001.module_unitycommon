#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 포커스 팝업 */
public partial class CFocusPopup : CSubPopup {
	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.IsIgnoreAni = true;

		// 이미지를 설정한다
		CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
			(EKey.FOCUS_BLIND_IMG, $"{EKey.FOCUS_BLIND_IMG}", this.Contents)
		}, m_oImgDict, false);
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();
		m_stParams = a_stParams;

		// 터치 전달자를 설정한다
		Func.SetupTouchDispatchers(new List<(GameObject, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>)>() {
			(m_oImgDict[EKey.FOCUS_BLIND_IMG]?.gameObject, (a_oSender, a_oEventData) => a_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.BEGIN)?.Invoke(this, a_oEventData), (a_oSender, a_oEventData) => a_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.MOVE)?.Invoke(this, a_oEventData), (a_oSender, a_oEventData) => a_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.END)?.Invoke(this, a_oEventData))
		}, false);
	}

	/** UI 상태를 갱신한다 */
	protected new void UpdateUIsState() {
		base.UpdateUIsState();
		
		// 이미지를 갱신한다
		m_oImgDict[EKey.FOCUS_BLIND_IMG]?.ExSetColor<Image>(KCDefine.U_COLOR_POPUP_BLIND, false);
	}
	#endregion			// 함수
}

/** 서브 포커스 팝업 */
public partial class CFocusPopup : CSubPopup {
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
