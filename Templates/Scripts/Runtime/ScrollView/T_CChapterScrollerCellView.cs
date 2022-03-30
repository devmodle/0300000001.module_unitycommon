using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if RUNTIME_TEMPLATES_MODULE_ENABLE
/** 챕터 스크롤러 셀 뷰 */
public class CChapterScrollerCellView : CScrollerCellView {
	/** 매개 변수 */
	public new struct STParams {
		public CScrollerCellView.STParams m_stBaseParams;
	}

	#region 변수
	private STParams m_stParams;
	#endregion			// 변수

	#region 추가 변수

	#endregion			// 추가 변수

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
	}
	
	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init(a_stParams.m_stBaseParams);
		m_stParams = a_stParams;

		for(int i = 0; i < this.ScrollerCellList.Count; ++i) {
			var stIDInfo = CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, i + m_stParams.m_stBaseParams.m_nID.ExUniqueLevelIDToChapterID());

			this.UpdateScrollerCellState(this.ScrollerCellList[i], stIDInfo);
			this.ScrollerCellList[i]?.SetActive(stIDInfo.m_nChapterID < CLevelInfoTable.Inst.NumChapterInfos);
		}
	}

	/** 스크롤러 셀 상태를 갱신한다 */
	protected virtual void UpdateScrollerCellState(GameObject a_oScrollerCell, STIDInfo a_stIDInfo) {
		// Do Something
	}
	#endregion			// 함수

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
