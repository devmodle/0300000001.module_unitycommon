using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if NEVER_USE_THIS
//! 포커스 팝업
public class CFocusPopup : CSubPopup {
	//! 매개 변수
	public struct STParams {
		public List<GameObject> m_oContentsUIsList;
	}

	//! 콜백 매개 변수
	public struct STCallbackParams {
		public System.Action<CFocusPopup, PointerEventData> m_oBeginCallback;
		public System.Action<CFocusPopup, PointerEventData> m_oMoveCallback;
		public System.Action<CFocusPopup, PointerEventData> m_oEndCallback;
	}

	#region 변수
	private STParams m_stParams;
	private STCallbackParams m_stCallbackParams;

	// UI
	private Image m_oBlindImg = null;
	#endregion			// 변수

	#region 프로퍼티
	public override bool IsIgnoreAni => true;
	public override bool IsIgnoreBGAni => true;

	public override Color BGColor => KCDefine.U_COLOR_TRANSPARENT;
	public override EAniType AniType => EAniType.NONE;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		m_oBlindImg = m_oContents.ExFindComponent<Image>(KCDefine.U_OBJ_N_BLIND_IMG);
	}

	//! 초기화
	public virtual void Init(STParams a_stParams, STCallbackParams a_stCallbackParams) {
		base.Init();

		m_stParams = a_stParams;
		m_stCallbackParams = a_stCallbackParams;

		// 터치 전달자를 설정한다
		var oTouchDispatcher = m_oBlindImg?.GetComponentInChildren<CTouchDispatcher>();
		oTouchDispatcher?.ExSetBeginCallback((a_oSender, a_oEventData) => a_stCallbackParams.m_oBeginCallback?.Invoke(this, a_oEventData), false);
		oTouchDispatcher?.ExSetMoveCallback((a_oSender, a_oEventData) => a_stCallbackParams.m_oMoveCallback?.Invoke(this, a_oEventData), false);
		oTouchDispatcher?.ExSetEndCallback((a_oSender, a_oEventData) => a_stCallbackParams.m_oEndCallback?.Invoke(this, a_oEventData), false);
	}

	//! 팝업 컨텐츠를 설정한다
	protected override void SetupContents() {
		base.SetupContents();

		// 포커스 UI 가 존재 할 경우
		if(m_stParams.m_oContentsUIsList.ExIsValid()) {
			for(int i = 0; i < m_stParams.m_oContentsUIsList.Count; ++i) {
				m_stParams.m_oContentsUIsList[i].SetActive(true);
				m_stParams.m_oContentsUIsList[i].transform.SetParent(m_oContentsUIs.transform, false);
			}
		}

		this.UpdateUIsState();
	}

	//! UI 상태를 갱신한다
	protected new void UpdateUIsState() {
		base.UpdateUIsState();
		m_oBlindImg?.ExSetColor<Image>(KCDefine.U_COLOR_POPUP_BG);
		
		var oContentsImg = m_oContents.GetComponentInChildren<Image>();
		oContentsImg?.ExSetColor<Image>(KCDefine.U_COLOR_TRANSPARENT);
	}
	#endregion			// 함수

	#region 추가 변수

	#endregion			// 추가 변수

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if NEVER_USE_THIS
