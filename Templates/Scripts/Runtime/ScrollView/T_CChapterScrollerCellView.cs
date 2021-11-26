using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
#if RUNTIME_TEMPLATES_MODULE_ENABLE
/** 챕터 스크롤러 셀 뷰 */
public class CChapterScrollerCellView : CScrollerCellView {
	/** 매개 변수 */
	public new struct STParams {
		public CScrollerCellView.STParams m_stBaseParams;
	}

	/** 콜백 매개 변수 */
	public new struct STCallbackParams {
		public CScrollerCellView.STCallbackParams m_stBaseCallbackParams;
	}

	#region 변수
	private STParams m_stParams;
	private STCallbackParams m_stCallbackParams;
	#endregion			// 변수

	#region 추가 변수

	#endregion			// 추가 변수

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
	}
	
	/** 초기화 */
	public virtual void Init(STParams a_stParams, STCallbackParams a_stCallbackParams) {
		base.Init(a_stParams.m_stBaseParams, a_stCallbackParams.m_stBaseCallbackParams);

		m_stParams = a_stParams;
		m_stCallbackParams = a_stCallbackParams;

		this.UpdateUIsState();
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		// Do Something
	}
	#endregion			// 함수

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if NEVER_USE_THIS
