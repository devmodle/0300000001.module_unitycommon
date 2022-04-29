using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 챕터 스크롤러 셀 뷰 */
public partial class CChapterScrollerCellView : CScrollerCellView {
	/** 매개 변수 */
	public new struct STParams {
		public CScrollerCellView.STParams m_stBaseParams;
	}

	#region 변수
	private STParams m_stParams;
	#endregion			// 변수
	
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
	private void UpdateScrollerCellState(GameObject a_oScrollerCell, STIDInfo a_stIDInfo) {
		// 버튼을 갱신한다 {
		var oSelBtn = a_oScrollerCell.GetComponentInChildren<Button>();
		oSelBtn?.ExAddListener(() => m_stParams.m_stBaseParams.m_oCallbackDict.GetValueOrDefault(ECallback.SEL)?.Invoke(this, CFactory.MakeUniqueLevelID(a_stIDInfo.m_nID, a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID)), true, false);

#if PLAY_TEST_ENABLE
		oSelBtn?.ExSetInteractable(true, false);
#else
		oSelBtn?.ExSetInteractable(a_stIDInfo.m_nChapterID <= CGameInfoStorage.Inst.NumChapterClearInfos, false);
#endif			// #if PLAY_TEST_ENABLE
		// 버튼을 갱신한다 }
		
		// 챕터 정보가 존재 할 경우
		if(a_stIDInfo.m_nChapterID < CLevelInfoTable.Inst.NumChapterInfos) {
			CEpisodeInfoTable.Inst.TryGetChapterInfo(a_stIDInfo.m_nChapterID, out STChapterInfo stChapterInfo);

			// 텍스트를 갱신한다
			var oChapterText = a_oScrollerCell.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_CHAPTER_TEXT);
			oChapterText?.ExSetText($"{a_stIDInfo.m_nChapterID + KCDefine.B_VAL_1_INT}", EFontSet._1, false);
		}
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
