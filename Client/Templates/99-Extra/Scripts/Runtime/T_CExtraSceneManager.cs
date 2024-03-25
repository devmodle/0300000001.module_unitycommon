#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE
/** 추가 씬 관리자 */
public partial class CSceneManagerExtra : CSceneManager
{
	/** 식별자 */
	private enum EKey
	{
		NONE = -1,
		[HideInInspector] MAX_VAL
	}

	#region 변수

	#endregion // 변수

	#region 프로퍼티
	
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake()
	{
		base.Awake();

		// 앱 초기화가 필요 할 경우
		if(!CSceneManager.IsInitApp)
		{
			return;
		}
	}

	/** 제거되었을 경우 */
	public override void OnDestroy()
	{
		base.OnDestroy();

		try
		{
			// 앱이 종료되었을 경우
			if(!CSceneManager.IsRunningApp)
			{
				return;
			}
		}
		catch(System.Exception oException)
		{
			CFunc.ShowLogWarning($"CSceneManagerExtra.OnDestroy Exception: {oException.Message}");
		}
	}

	/** 상태를 갱신한다 */
	public override void OnUpdate(float a_fTimeDelta)
	{
		base.OnUpdate(a_fTimeDelta);

		// 앱이 종료되었을 경우
		if(!CSceneManager.IsRunningApp)
		{
			return;
		}
	}

	/** 내비게이션 스택 이벤트를 수신했을 경우 */
	public override void OnReceiveEventNavStack(EEventNavStack a_eEvent)
	{
		base.OnReceiveEventNavStack(a_eEvent);

		switch(a_eEvent)
		{
			case EEventNavStack.BACK_KEY_DOWN:
				break;
		}
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
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
