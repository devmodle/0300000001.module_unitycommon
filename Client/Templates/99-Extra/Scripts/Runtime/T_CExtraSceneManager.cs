#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE
/** 추가 씬 관리자 */
public partial class CExtraSceneManager : CSceneManager {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		[HideInInspector] MAX_VAL
	}

	#region 변수

	#endregion // 변수

	#region 프로퍼티
	public override string SceneName => KCDefine.B_SCENE_N_EXTRA;
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 앱 초기화가 필요 할 경우
		if(!CSceneManager.IsAppInit) {
			return;
		}
	}

	/** 초기화 */
	public override void Start() {
		base.Start();

		// 앱 초기화가 필요 할 경우
		if(!CSceneManager.IsAppInit) {
			return;
		}

		this.UpdateUIsState();
	}

	/** 제거 되었을 경우 */
	public override void OnDestroy() {
		base.OnDestroy();

		try {
			// 앱이 종료 되었을 경우
			if(!CSceneManager.IsAppRunning) {
				return;
			}
		} catch(System.Exception oException) {
			CFunc.ShowLogWarning($"CExtraSceneManager.OnDestroy Exception: {oException.Message}");
		}
	}

	/** 상태를 갱신한다 */
	public override void OnUpdate(float a_fDeltaTime) {
		base.OnUpdate(a_fDeltaTime);

		// 앱이 종료 되었을 경우
		if(!CSceneManager.IsAppRunning) {
			return;
		}
	}

	/** 내비게이션 스택 이벤트를 수신했을 경우 */
	public override void OnReceiveNavStackEvent(ENavStackEvent a_eEvent) {
		base.OnReceiveNavStackEvent(a_eEvent);

		switch(a_eEvent) {
			case ENavStackEvent.BACK_KEY_DOWN:

				break;
		}
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		// Do Something
	}
	#endregion // 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
