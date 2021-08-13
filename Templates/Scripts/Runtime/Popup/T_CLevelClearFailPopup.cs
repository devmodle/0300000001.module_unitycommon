using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 레벨 클리어 실패 팝업
public class CLevelClearFailPopup : CLevelClearPopup {
	//! 매개 변수
	public struct STParams {
		public CLevelClearPopup.STParams m_stBaseParams;
	}

	//! 콜백 매개 변수
	public struct STCallbackParams {
		public CLevelClearPopup.STCallbackParams m_stBaseCallbackParams;
		public System.Action<CLevelClearFailPopup> m_oRetryCallback;
	}

	#region 변수
	private STParams m_stParams;
	private STCallbackParams m_stCallbackParams;
	#endregion			// 변수

	#region UI 변수
	private Text m_oScoreText = null;
	#endregion			// UI 변수

	#region 프로퍼티
	public override bool IsIgnoreCloseBtn => true;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 버튼을 설정한다
		var oRetryBtn = m_oContents.ExFindComponent<Button>(KCDefine.U_OBJ_N_RETRY_BTN);
		oRetryBtn?.onClick.AddListener(this.OnTouchRetryBtn);
	}

	//! 초기화
	public virtual void Init(STParams a_stParams, STCallbackParams a_stCallbackParams) {
		base.Init(a_stParams.m_stBaseParams, a_stCallbackParams.m_stBaseCallbackParams);

		m_stParams = a_stParams;
		m_stCallbackParams = a_stCallbackParams;
	}

	//! 팝업 컨텐츠를 설정한다
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}

	//! UI 상태를 갱신한다
	private new void UpdateUIsState() {
		base.UpdateUIsState();
	}

	//! 재시도 버튼을 눌렀을 경우
	private void OnTouchRetryBtn() {
		m_stCallbackParams.m_oRetryCallback?.Invoke(this);
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
