#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 일일 미션 팝업 */
public partial class CDailyMissionPopup : CMissionPopup {
	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init(a_stParams.m_stBaseParams);
		m_stParams = a_stParams;
	}

	/** UI 상태를 갱신한다 */
	private new void UpdateUIsState() {
		base.UpdateUIsState();
	}
	#endregion			// 함수
}

/** 서브 일일 미션 팝업 */
public partial class CDailyMissionPopup : CMissionPopup {
	/** 서브 식별자 */
	private enum ESubKey {
		NONE = -1,
		[HideInInspector] MAX_VAL
	}

	#region 변수

	#endregion			// 변수

	#region 프로퍼티

	#endregion			// 프로퍼티

	#region 함수
	
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
