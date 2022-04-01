using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
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
	public struct STParams {
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

		m_oUIsDict[EKey.CLEAR_UIS] = this.Contents.ExFindChild(KCDefine.U_OBJ_N_CLEAR_UIS);
		m_oUIsDict[EKey.CLEAR_FAIL_UIS] = this.Contents.ExFindChild(KCDefine.U_OBJ_N_CLEAR_FAIL_UIS);

		// 텍스트를 설정한다
		m_oTextDict[EKey.RECORD_TEXT] = this.Contents.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_RECORD_TEXT);
		m_oTextDict[EKey.BEST_RECORD_TEXT] = this.Contents.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_BEST_RECORD_TEXT);

		// 버튼을 설정한다
		this.Contents.ExFindComponent<Button>(KCDefine.U_OBJ_N_NEXT_BTN)?.onClick.AddListener(this.OnTouchNextBtn);
		this.Contents.ExFindComponent<Button>(KCDefine.U_OBJ_N_RETRY_BTN)?.onClick.AddListener(this.OnTouchRetryBtn);
		this.Contents.ExFindComponent<Button>(KCDefine.U_OBJ_N_LEAVE_BTN)?.onClick.AddListener(this.OnTouchLeaveBtn);
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

		m_oUIsDict[EKey.CLEAR_UIS]?.SetActive(m_stParams.m_stRecordInfo.m_bIsSuccess);
		m_oUIsDict[EKey.CLEAR_FAIL_UIS]?.SetActive(!m_stParams.m_stRecordInfo.m_bIsSuccess);

		// 텍스트를 갱신한다
		m_oTextDict[EKey.RECORD_TEXT]?.ExSetText($"{m_stParams.m_stRecordInfo.m_nIntRecord}", EFontSet._1, false);
		m_oTextDict[EKey.BEST_RECORD_TEXT]?.ExSetText((m_stParams.m_oClearInfo != null) ? $"{m_stParams.m_oClearInfo.IntBestClearRecord}" : string.Empty, EFontSet._1, false);
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
#endif			// #if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
