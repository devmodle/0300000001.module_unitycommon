#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using UnityEngine.EventSystems;

/** 포커스 팝업 */
public partial class CFocusPopup : CSubPopup
{
	/** 식별자 */
	private enum EKey
	{
		NONE = -1,
		FOCUS_BLIND_IMG,
		[HideInInspector] MAX_VAL
	}

	/** 콜백 */
	public enum ECallback
	{
		NONE = -1,
		BEGIN,
		MOVE,
		END,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams
	{
		public List<GameObject> m_oContentsUIList;
		public Dictionary<ECallback, System.Action<CFocusPopup, PointerEventData>> m_oCallbackDict;
	}

	#region 변수
	[Header("=====> UIs <=====")]
	private Dictionary<EKey, Image> m_oImgDict = new Dictionary<EKey, Image>();
	#endregion // 변수

	#region 프로퍼티
	public STParams Params { get; private set; }

	public override EPopupAniType PopupAniType => EPopupAniType.NONE;
	public override Color BlindColor => KCDefine.B_COLOR_TRANSPARENT;
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake()
	{
		base.Awake();
		this.SetIsEnableAnim(false);

		// 이미지를 설정한다
		CFunc.SetupComponents(new List<(EKey, string, GameObject)>()
		{
			(EKey.FOCUS_BLIND_IMG, $"{EKey.FOCUS_BLIND_IMG}", this.ContentsUIs)
		}, m_oImgDict);

		this.SubAwake();
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams)
	{
		base.Init();
		this.Params = a_stParams;

		// 터치 전달자를 설정한다
		Func.SetupTouchDispatchers(new List<(GameObject, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>)>()
		{
			(m_oImgDict[EKey.FOCUS_BLIND_IMG]?.gameObject, (a_oSender, a_oEventData) => a_stParams.m_oCallbackDict?.ExGetVal(ECallback.BEGIN)?.Invoke(this, a_oEventData), (a_oSender, a_oEventData) => a_stParams.m_oCallbackDict?.ExGetVal(ECallback.MOVE)?.Invoke(this, a_oEventData), (a_oSender, a_oEventData) => a_stParams.m_oCallbackDict?.ExGetVal(ECallback.END)?.Invoke(this, a_oEventData))
		});

		this.SubInit();
	}

	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents()
	{
		base.SetupContents();

		for(int i = 0; i < this.Params.m_oContentsUIList.Count; ++i)
		{
			this.Params.m_oContentsUIList[i].SetActive(true);
			this.Params.m_oContentsUIList[i].ExSetParent(this.ContentsUIs);
		}

		this.UpdateUIsState();
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState()
	{
		// 이미지를 갱신한다
		m_oImgDict[EKey.FOCUS_BLIND_IMG]?.ExSetPropertyVal<Image>(KCDefine.U_PROPERTY_N_COLOR, 
			KCDefine.B_COLOR_POPUP_BLIND, a_bIsAssert: false);

		this.SubUpdateUIsState();
	}
	#endregion // 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
