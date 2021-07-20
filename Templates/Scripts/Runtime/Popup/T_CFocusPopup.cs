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
		public List<GameObject> m_oFocusUIsList;
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
	#endregion			// 변수

	#region UI 변수
	private Image m_oBlindImg = null;
	#endregion			// UI 변수

	#region 객체
	private GameObject m_oFocusUIs = null;
	#endregion			// 객체

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

		m_oBlindImg = m_oContents.ExFindComponent<Image>(KDefine.G_OBJ_N_FOCUS_P_BLIND_IMG);
		m_oFocusUIs = m_oContents.ExFindChild(KDefine.G_OBJ_N_FOCUS_P_FOCUS_UIS);
	}

	//! 초기화
	public virtual void Init(STParams a_stParams, STCallbackParams a_stCallbackParams) {
		base.Init();

		m_stParams = a_stParams;
		m_stCallbackParams = a_stCallbackParams;

		// 터치 전달자를 설정한다
		var oTouchDispatcher = m_oContents.GetComponentInChildren<CTouchDispatcher>();
		oTouchDispatcher.BeginCallback = (a_oSender, a_oEventData) => a_stCallbackParams.m_oBeginCallback?.Invoke(this, a_oEventData);
		oTouchDispatcher.MoveCallback = (a_oSender, a_oEventData) => a_stCallbackParams.m_oMoveCallback?.Invoke(this, a_oEventData);
		oTouchDispatcher.EndCallback = (a_oSender, a_oEventData) => a_stCallbackParams.m_oEndCallback?.Invoke(this, a_oEventData);

		this.UpdateUIsState();
	}

	//! 팝업 컨텐츠를 설정한다
	protected override void SetupContents() {
		base.SetupContents();

		// 포커스 UI 가 존재 할 경우
		if(m_stParams.m_oFocusUIsList.ExIsValid()) {
			for(int i = 0; i < m_stParams.m_oFocusUIsList.Count; ++i) {
				m_stParams.m_oFocusUIsList[i].SetActive(true);
				m_stParams.m_oFocusUIsList[i].transform.SetParent(m_oFocusUIs.transform, false);
			}
		}
	}

	//! UI 상태를 갱신한다
	private void UpdateUIsState() {
		m_oBlindImg.color = KCDefine.U_COLOR_POPUP_BG;
		
		var oContentsImg = m_oContents.GetComponentInChildren<Image>();
		oContentsImg.color = KCDefine.U_COLOR_TRANSPARENT;
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
