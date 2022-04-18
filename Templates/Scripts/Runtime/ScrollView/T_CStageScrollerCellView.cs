using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 스테이지 스크롤러 셀 뷰 */
public partial class CStageScrollerCellView : CScrollerCellView {
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
			var stIDInfo = CFactory.MakeIDInfo(KCDefine.B_VAL_0_INT, i + m_stParams.m_stBaseParams.m_nID.ExUniqueLevelIDToStageID(), m_stParams.m_stBaseParams.m_nID.ExUniqueLevelIDToChapterID());

			this.UpdateScrollerCellState(this.ScrollerCellList[i], stIDInfo);
			this.ScrollerCellList[i]?.SetActive(stIDInfo.m_nStageID < CLevelInfoTable.Inst.GetNumStageInfos(stIDInfo.m_nChapterID));
		}
	}

	/** 스크롤러 셀 상태를 갱신한다 */
	private void UpdateScrollerCellState(GameObject a_oScrollerCell, STIDInfo a_stIDInfo) {
		int nNumStageClearInfos = CGameInfoStorage.Inst.GetNumStageClearInfos(a_stIDInfo.m_nChapterID);

		// 버튼을 갱신한다 {
		var oSelBtn = a_oScrollerCell.GetComponentInChildren<Button>();
		oSelBtn?.ExAddListener(() => m_stParams.m_stBaseParams.m_oCallbackDict.GetValueOrDefault(ECallback.SEL)?.Invoke(this, CFactory.MakeUniqueLevelID(a_stIDInfo.m_nID, a_stIDInfo.m_nStageID, a_stIDInfo.m_nChapterID)), true, false);

#if PLAY_TEST_ENABLE
		oSelBtn?.ExSetInteractable(true, false);
#else
		oSelBtn?.ExSetInteractable(a_stIDInfo.m_nStageID <= nNumStageClearInfos, false);
#endif			// #if PLAY_TEST_ENABLE
		// 버튼을 갱신한다 }
		
		// 스테이지 정보가 존재 할 경우
		if(a_stIDInfo.m_nStageID < CLevelInfoTable.Inst.GetNumStageInfos(a_stIDInfo.m_nChapterID)) {
			CEpisodeInfoTable.Inst.TryGetStageInfo(a_stIDInfo.m_nStageID, out STStageInfo stStageInfo, a_stIDInfo.m_nChapterID);

			// 텍스트를 갱신한다
			var oStageText = a_oScrollerCell.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_STAGE_TEXT);
			oStageText?.ExSetText($"{a_stIDInfo.m_nStageID + KCDefine.B_VAL_1_INT}", EFontSet._1, false);
		}
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
