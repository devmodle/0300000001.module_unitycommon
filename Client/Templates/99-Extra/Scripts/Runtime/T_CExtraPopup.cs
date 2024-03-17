#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE
/** 추가 팝업 */
public partial class CExtraPopup : CSubPopup
{
	/** 식별자 */
	private enum EKey
	{
		NONE = -1,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public record REParams : global::REParams<CExtraPopup, CComponent, CComponent>
	{
		// Do Something
	}

	#region 변수

	#endregion // 변수

	#region 프로퍼티
	public REParams Params { get; private set; } = null;
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake()
	{
		base.Awake();
	}

	/** 초기화 */
	public virtual void Init(REParams a_reParams)
	{
		base.Init();
		this.Params = a_reParams;
	}

	/** 상태를 갱신한다 */
	public override void UpdateState()
	{
		base.UpdateState();
		this.UpdateUIsState();
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState()
	{
		// Do Something
	}
	#endregion // 함수

	#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static REParams MakeParams(CComponent a_oOwner, 
		System.Action<CExtraPopup> a_oCallback, System.Action<CExtraPopup, CComponent> a_oParamsCallback)
	{
		return new REParams()
		{
			m_tOwner = a_oOwner,
			m_oCallback = a_oCallback,
			m_oParamsCallback = a_oParamsCallback
		};
	}
	#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
