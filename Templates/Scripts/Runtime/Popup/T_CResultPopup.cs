using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 결과 팝업 */
public partial class CResultPopup : CSubPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		RECORD_TEXT,
		BEST_RECORD_TEXT,
		CLEAR_UIS,
		CLEAR_FAIL_UIS,
		[HideInInspector] MAX_VAL
	}

	/** 콜백 */
	public enum ECallback {
		NONE = -1,
		NEXT,
		RETRY,
		LEAVE,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public partial struct STParams {
		public STRecordInfo m_stRecordInfo;

		public CLevelInfo m_oLevelInfo;
		public CClearInfo m_oClearInfo;
		public Dictionary<ECallback, System.Action<CResultPopup>> m_oCallbackDict;
	}

	#region 변수
	private STParams m_stParams;

	/** =====> UI <===== */
	private Dictionary<EKey, TMP_Text> m_oTextDict = new Dictionary<EKey, TMP_Text>() {
		[EKey.RECORD_TEXT] = null,
		[EKey.BEST_RECORD_TEXT] = null
	};

	/** =====> 객체 <===== */
	private Dictionary<EKey, GameObject> m_oUIsDict = new Dictionary<EKey, GameObject>() {
		[EKey.CLEAR_UIS] = null,
		[EKey.CLEAR_FAIL_UIS] = null
	};
	#endregion			// 변수

	#region 프로퍼티
	public override bool IsIgnoreCloseBtn => true;
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.IsIgnoreNavStackEvent = true;

		// 객체를 설정한다
		CFunc.SetupObjs(new List<(EKey, string, GameObject)>() {
			(EKey.CLEAR_UIS, $"{EKey.CLEAR_UIS}", this.Contents),
			(EKey.CLEAR_FAIL_UIS, $"{EKey.CLEAR_FAIL_UIS}", this.Contents)
		}, m_oUIsDict, false);

		// 텍스트를 설정한다
		CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
			(EKey.RECORD_TEXT, $"{EKey.RECORD_TEXT}", this.Contents),
			(EKey.BEST_RECORD_TEXT, $"{EKey.BEST_RECORD_TEXT}", this.Contents)
		}, m_oTextDict, false);

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
			(KCDefine.U_OBJ_N_NEXT_BTN, this.Contents, this.OnTouchNextBtn),
			(KCDefine.U_OBJ_N_RETRY_BTN, this.Contents, this.OnTouchRetryBtn),
			(KCDefine.U_OBJ_N_LEAVE_BTN, this.Contents, this.OnTouchLeaveBtn)
		}, false);
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

		// 객체를 갱신한다
		m_oUIsDict[EKey.CLEAR_UIS]?.SetActive(m_stParams.m_stRecordInfo.m_bIsSuccess);
		m_oUIsDict[EKey.CLEAR_FAIL_UIS]?.SetActive(!m_stParams.m_stRecordInfo.m_bIsSuccess);

		// 텍스트를 갱신한다
		m_oTextDict[EKey.RECORD_TEXT]?.ExSetText($"{m_stParams.m_stRecordInfo.m_nIntRecord}", EFontSet._1, false);
		m_oTextDict[EKey.BEST_RECORD_TEXT]?.ExSetText((m_stParams.m_oClearInfo != null) ? $"{m_stParams.m_oClearInfo.BestIntRecord}" : string.Empty, EFontSet._1, false);
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
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
