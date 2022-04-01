using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 포커스 팝업 */
public partial class CFocusPopup : CSubPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		BLIND_IMG,
		[HideInInspector] MAX_VAL
	}

	/** 콜백 */
	public enum ECallback {
		NONE = -1,
		BEGIN,
		MOVE,
		END,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public List<GameObject> m_oContentsUIsList;
		public Dictionary<ECallback, System.Action<CFocusPopup, PointerEventData>> m_oCallbackDict;
	}

	#region 변수
	private STParams m_stParams;

	/** =====> UI <===== */
	private Dictionary<EKey, Image> m_oImgDict = new Dictionary<EKey, Image>() {
		[EKey.BLIND_IMG] = null
	};
	#endregion			// 변수
	
	#region 프로퍼티
	public override bool IsIgnoreBGAni => true;
	public override EAniType AniType => EAniType.NONE;

	public override Color BGColor => KCDefine.U_COLOR_TRANSPARENT;
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.IsIgnoreAni = true;

		// 이미지를 설정한다
		m_oImgDict[EKey.BLIND_IMG] = this.Contents.ExFindComponent<Image>(KCDefine.U_OBJ_N_BLIND_IMG);
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();
		m_stParams = a_stParams;

		// 터치 전달자를 설정한다
		var oTouchDispatcher = m_oImgDict[EKey.BLIND_IMG]?.GetComponentInChildren<CTouchDispatcher>();
		oTouchDispatcher?.ExSetBeginCallback((a_oSender, a_oEventData) => a_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.BEGIN)?.Invoke(this, a_oEventData), false);
		oTouchDispatcher?.ExSetMoveCallback((a_oSender, a_oEventData) => a_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.MOVE)?.Invoke(this, a_oEventData), false);
		oTouchDispatcher?.ExSetEndCallback((a_oSender, a_oEventData) => a_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.END)?.Invoke(this, a_oEventData), false);
	}

	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();

		// 포커스 UI 가 존재 할 경우
		if(m_stParams.m_oContentsUIsList.ExIsValid()) {
			for(int i = 0; i < m_stParams.m_oContentsUIsList.Count; ++i) {
				m_stParams.m_oContentsUIsList[i].SetActive(true);
				m_stParams.m_oContentsUIsList[i].ExSetParent(this.ContentsUIs);
			}
		}

		this.UpdateUIsState();
	}

	/** UI 상태를 갱신한다 */
	protected new void UpdateUIsState() {
		base.UpdateUIsState();

		// 이미지를 갱신한다 {
		m_oImgDict[EKey.BLIND_IMG]?.ExSetColor<Image>(KCDefine.U_COLOR_POPUP_BG);
		
		var oContentsImg = this.Contents.GetComponentInChildren<Image>();
		oContentsImg?.ExSetColor<Image>(KCDefine.U_COLOR_TRANSPARENT);
		// 이미지를 갱신한다 }
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
