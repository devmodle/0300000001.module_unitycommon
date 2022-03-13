using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if SCRIPT_TEMPLATE_ONLY
#if RUNTIME_TEMPLATES_MODULE_ENABLE
/** 결과 팝업 */
public class CResultPopup : CSubPopup {
	/** 콜백 */
	public enum ECallback {
		NONE = -1,
		NEXT,
		RETRY,
		LEAVE,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public STRecordInfo m_stRecordInfo;

		public CLevelInfo m_oLevelInfo;
		public CClearInfo m_oClearInfo;
		public Dictionary<ECallback, System.Action<CResultPopup>> m_oCallbackDict;
	}

	#region 변수
	private STParams m_stParams;

	/** =====> UI <===== */
	private TMP_Text m_oRecordText = null;
	private TMP_Text m_oBestRecordText = null;

	/** =====> 객체 <===== */
	private GameObject m_oClearUIs = null;
	private GameObject m_oClearFailUIs = null;
	#endregion			// 변수

	#region 프로퍼티
	public override bool IsIgnoreCloseBtn => true;
	#endregion			// 프로퍼티

	#region 추가 변수

	#endregion			// 추가 변수

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.IsIgnoreNavStackEvent = true;

		m_oClearUIs = m_oContents.ExFindChild(KCDefine.U_OBJ_N_CLEAR_UIS);
		m_oClearFailUIs = m_oContents.ExFindChild(KCDefine.U_OBJ_N_CLEAR_FAIL_UIS);

		// 텍스트를 설정한다
		m_oRecordText = m_oContents.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_RECORD_TEXT);
		m_oBestRecordText = m_oContents.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_BEST_RECORD_TEXT);

		// 버튼을 설정한다
		m_oContents.ExFindComponent<Button>(KCDefine.U_OBJ_N_NEXT_BTN)?.onClick.AddListener(this.OnTouchNextBtn);
		m_oContents.ExFindComponent<Button>(KCDefine.U_OBJ_N_RETRY_BTN)?.onClick.AddListener(this.OnTouchRetryBtn);
		m_oContents.ExFindComponent<Button>(KCDefine.U_OBJ_N_LEAVE_BTN)?.onClick.AddListener(this.OnTouchLeaveBtn);
	}

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

	/** 닫기 버튼을 눌렀을 경우 */
	protected override void OnTouchCloseBtn() {
		base.OnTouchCloseBtn();
		this.OnTouchLeaveBtn();
	}
	
	/** UI 상태를 갱신한다 */
	private new void UpdateUIsState() {
		base.UpdateUIsState();

		m_oClearUIs?.SetActive(m_stParams.m_stRecordInfo.m_bIsSuccess);
		m_oClearFailUIs?.SetActive(!m_stParams.m_stRecordInfo.m_bIsSuccess);

		// 텍스트를 갱신한다
		m_oRecordText?.ExSetText($"{m_stParams.m_stRecordInfo.m_nIntRecord}", EFontSet._1, false);
		m_oBestRecordText?.ExSetText((m_stParams.m_oClearInfo != null) ? $"{m_stParams.m_oClearInfo.IntBestClearRecord}" : string.Empty, EFontSet._1, false);
	}

	/** 다음 버튼을 눌렀을 경우 */
	private void OnTouchNextBtn() {
		m_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.NEXT)?.Invoke(this);
	}

	/** 재시도 버튼을 눌렀을 경우 */
	private void OnTouchRetryBtn() {
		m_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.RETRY)?.Invoke(this);
	}

	/** 나가기 버튼을 눌렀을 경우 */
	private void OnTouchLeaveBtn() {
		m_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.LEAVE)?.Invoke(this);
	}
	#endregion			// 함수
	
	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
