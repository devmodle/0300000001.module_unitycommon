using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 포커스 팝업
public class CFocusPopup : CSubPopup {
	#region 변수
	private System.Action<CFocusPopup, PointerEventData> m_oCallback = null;
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
	public virtual void Init(System.Action<CFocusPopup, PointerEventData> a_oCallback) {
		base.Init();
		m_oCallback = a_oCallback;

		this.UpdateUIsState();
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
