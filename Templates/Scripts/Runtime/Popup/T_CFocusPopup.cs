using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 포커스 팝업
public class CFocusPopup : CSubPopup {
	//! 매개 변수
	public struct STParams {
		public List<GameObject> m_oFocusList;
	}

	#region 변수
	private STParams m_stParams;
	#endregion			// 변수

	#region UI 변수
	private Image m_oBlindImg = null;
	#endregion			// UI 변수

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
	}

	//! 초기화
	public virtual void Init(STParams a_stParams, System.Action<CFocusPopup, PointerEventData> a_oBeginCallback, System.Action<CFocusPopup, PointerEventData> a_oMoveCallback, System.Action<CFocusPopup, PointerEventData> a_oEndCallback) {
		base.Init();
		m_stParams = a_stParams;

		// 터치 전달자를 설정한다
		var oTouchDispatcher = m_oContents.GetComponentInChildren<CTouchDispatcher>();
		oTouchDispatcher.BeginCallback = (a_oSender, a_oEventData) => a_oBeginCallback?.Invoke(this, a_oEventData);
		oTouchDispatcher.MoveCallback = (a_oSender, a_oEventData) => a_oMoveCallback?.Invoke(this, a_oEventData);
		oTouchDispatcher.EndCallback = (a_oSender, a_oEventData) => a_oEndCallback?.Invoke(this, a_oEventData);

		this.UpdateUIsState();
	}

	//! 팝업 컨텐츠를 설정한다
	protected override void SetupContents() {
		base.SetupContents();

		for(int i = 0; i < m_stParams.m_oFocusList.Count; ++i) {
			m_stParams.m_oFocusList[i].SetActive(true);
			m_stParams.m_oFocusList[i].transform.SetParent(m_oContentsTrans, false);
		}
	}

	//! UI 상태를 갱신한다
	private void UpdateUIsState() {
		// 이미지를 갱신한다 {
		m_oBlindImg.color = KCDefine.U_DEF_COLOR_POPUP_BG;
		
		var oContentsImg = m_oContents.GetComponentInChildren<Image>();
		oContentsImg.color = KCDefine.U_COLOR_TRANSPARENT;
		// 이미지를 갱신한다 }
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
