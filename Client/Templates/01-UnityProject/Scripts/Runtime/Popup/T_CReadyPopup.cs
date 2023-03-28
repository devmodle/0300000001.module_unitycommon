#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 준비 팝업 */
public partial class CReadyPopup : CSubPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		TITLE_TEXT,
		[HideInInspector] MAX_VAL
	}

	/** 콜백 */
	public enum ECallback {
		NONE = -1,
		PLAY,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public STIDInfo m_stIDInfo;
		public Dictionary<ECallback, System.Action<CReadyPopup>> m_oCallbackDict;
	}

	#region 변수
	/** =====> UI <===== */
	private Dictionary<EKey, Text> m_oTextDict = new Dictionary<EKey, Text>();
	#endregion // 변수

	#region 프로퍼티
	public STParams Params { get; private set; }
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 텍스트를 설정한다
		CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
			(EKey.TITLE_TEXT, $"{EKey.TITLE_TEXT}", this.gameObject)
		}, m_oTextDict);

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
			(KCDefine.U_OBJ_N_PLAY_BTN, this.gameObject, this.OnTouchPlayBtn)
		});

		this.SubAwake();
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();
		this.Params = a_stParams;

		this.SubInit();
	}

	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		this.SubUpdateUIsState();
	}

	/** 플레이 버튼을 눌렀을 경우 */
	private void OnTouchPlayBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.PLAY)?.Invoke(this);
	}
	#endregion // 함수

	#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(STIDInfo a_stIDInfo, Dictionary<ECallback, System.Action<CReadyPopup>> a_oCallbackDict = null) {
		return new STParams() {
			m_stIDInfo = a_stIDInfo, m_oCallbackDict = a_oCallbackDict ?? new Dictionary<ECallback, System.Action<CReadyPopup>>()
		};
	}
	#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
