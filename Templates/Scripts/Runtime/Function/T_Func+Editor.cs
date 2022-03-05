using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if UNITY_STANDALONE && EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
/** 에디터 함수 */
public static partial class Func {
	#region 클래스 함수
	
	#endregion			// 클래스 함수

	#region 조건부 클래스 함수
#if RUNTIME_TEMPLATES_MODULE_ENABLE
	/** 에디터 종료 팝업을 출력한다 */
	public static void ShowEditorQuitPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_EDITOR_QUIT_P_MSG), a_oCallback);
	}

	/** 에디터 리셋 팝업을 출력한다 */
	public static void ShowEditorResetPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_EDITOR_RESET_P_MSG), a_oCallback);
	}

	/** 에디터 A 세트 팝업을 출력한다 */
	public static void ShowEditorASetPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_EDITOR_A_SET_P_MSG), a_oCallback);
	}

	/** 에디터 B 세트 팝업을 출력한다 */
	public static void ShowEditorBSetPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_EDITOR_B_SET_P_MSG), a_oCallback);
	}

	/** 에디터 테이블 다시 로드 팝업을 출력한다 */
	public static void ShowEditorTableReloadPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_EDITOR_TABLE_RL_MSG), a_oCallback);
	}
	
	/** 에디터 레벨 제거 팝업을 출력한다 */
	public static void ShowEditorLevelRemovePopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_EDITOR_REMOVE_LP_MSG), a_oCallback);
	}

	/** 에디터 스테이지 제거 팝업을 출력한다 */
	public static void ShowEditorStageRemovePopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_EDITOR_REMOVE_SP_MSG), a_oCallback);
	}

	/** 에디터 챕터 제거 팝업을 출력한다 */
	public static void ShowEditorChapterRemovePopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_EDITOR_REMOVE_CP_MSG), a_oCallback);
	}

	/** 에디터 입력 팝업을 출력한다 */
	public static void ShowEditorInputPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CEditorInputPopup>(KCDefine.E_OBJ_N_EDITOR_INPUT_POPUP, KCDefine.E_OBJ_P_EDITOR_INPUT_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 에디터 레벨 생성 팝업을 출력한다 */
	public static void ShowEditorLevelCreatePopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CEditorLevelCreatePopup>(KCDefine.E_OBJ_N_EDITOR_LEVEL_CREATE_POPUP, KCDefine.E_OBJ_P_EDITOR_LEVEL_CREATE_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}
	
#if ENGINE_TEMPLATES_MODULE_ENABLE
	/** 에디터 레벨 정보를 설정한다 */
	public static void SetupEditorLevelInfo(CLevelInfo a_oLevelInfo, CEditorLevelCreateInfo a_oCreateInfo) {
		int nNumCellsX = Random.Range(a_oCreateInfo.m_stMinNumCells.x, a_oCreateInfo.m_stMaxNumCells.x + KCDefine.B_VAL_1_INT);
		nNumCellsX = Mathf.Clamp(nNumCellsX, SampleEngineName.KDefine.E_MIN_NUM_CELLS.x, SampleEngineName.KDefine.E_MAX_NUM_CELLS.x);

		int nNumCellsY = Random.Range(a_oCreateInfo.m_stMinNumCells.y, a_oCreateInfo.m_stMaxNumCells.y + KCDefine.B_VAL_1_INT);
		nNumCellsY = Mathf.Clamp(nNumCellsY, SampleEngineName.KDefine.E_MIN_NUM_CELLS.y, SampleEngineName.KDefine.E_MAX_NUM_CELLS.y);

		a_oLevelInfo.m_oCellInfoDictContainer.Clear();

		for(int i = 0; i < nNumCellsY; ++i) {
			var oCellInfoDict = new Dictionary<int, CCellInfo>();

			for(int j = 0; j < nNumCellsX; ++j) {
				var stIdx = new Vector3Int(j, i, KCDefine.B_IDX_INVALID);
				oCellInfoDict.TryAdd(j, Factory.MakeCellInfo(stIdx));
			}

			a_oLevelInfo.m_oCellInfoDictContainer.TryAdd(i, oCellInfoDict);
		}

		a_oLevelInfo.OnAfterDeserialize();
		Func.SetupEditorCellInfos(a_oLevelInfo, a_oCreateInfo);
	}

	/** 에디터 셀 정보 설정 완료 여부를 검사한다 */
	private static bool IsSetupEditorCellInfos(CLevelInfo a_oLevelInfo, CEditorLevelCreateInfo a_oCreateInfo) {
		return true;
	}

	/** 에디터 셀 정보를 설정한다 */
	private static void SetupEditorCellInfos(CLevelInfo a_oLevelInfo, CEditorLevelCreateInfo a_oCreateInfo) {
		int nTryTimes = KCDefine.B_VAL_0_INT;
		
		var oIdxVDictContainer = new Dictionary<int, List<Vector3Int>>();
		var oIdxHDictContainer = new Dictionary<int, List<Vector3Int>>();

		do {
			oIdxVDictContainer.Clear();
			oIdxHDictContainer.Clear();

			for(int i = 0; i < a_oLevelInfo.m_oCellInfoDictContainer.Count; ++i) {
				for(int j = 0; j < a_oLevelInfo.m_oCellInfoDictContainer[i].Count; ++j) {
					var oIdxVList = oIdxVDictContainer.ContainsKey(j) ? oIdxVDictContainer[j] : new List<Vector3Int>();
					var oIdxHList = oIdxHDictContainer.ContainsKey(i) ? oIdxHDictContainer[i] : new List<Vector3Int>();

					oIdxVList.Add(a_oLevelInfo.m_oCellInfoDictContainer[i][j].m_stIdx);
					oIdxHList.Add(a_oLevelInfo.m_oCellInfoDictContainer[i][j].m_stIdx);

					oIdxVDictContainer.TryAdd(j, oIdxVList);
					oIdxHDictContainer.TryAdd(i, oIdxHList);

					a_oLevelInfo.m_oCellInfoDictContainer[i][j].m_oBlockKindsList.Clear();
					a_oLevelInfo.m_oCellInfoDictContainer[i][j].m_oBlockKindsList.Add(EBlockKinds.BG_EMPTY);
				}
			}

			for(int i = 0; i < oIdxVDictContainer.Count; ++i) {
				oIdxVDictContainer.ExSwap(i, Random.Range(KCDefine.B_VAL_0_INT, oIdxVDictContainer.Count));
			}

			for(int i = 0; i < oIdxHDictContainer.Count; ++i) {
				oIdxHDictContainer.ExSwap(i, Random.Range(KCDefine.B_VAL_0_INT, oIdxHDictContainer.Count));
			}
			
			Func.SetupEditorCellInfos(a_oLevelInfo, a_oCreateInfo, oIdxVDictContainer, oIdxHDictContainer);
		} while(nTryTimes++ < KDefine.LES_MAX_TRY_TIMES_SETUP_CELL_INFOS && !Func.IsSetupEditorCellInfos(a_oLevelInfo, a_oCreateInfo));
		
		a_oLevelInfo.OnAfterDeserialize();
	}

	/** 에디터 셀 정보를 설정한다 */
	private static void SetupEditorCellInfos(CLevelInfo a_oLevelInfo, CEditorLevelCreateInfo a_oCreateInfo, Dictionary<int, List<Vector3Int>> a_oIdxVDictContainer, Dictionary<int, List<Vector3Int>> a_oIdxHDictContainer) {
		// Do Something
	}
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
	#endregion			// 조건부 클래스 함수

	#region 추가 클래스 함수

	#endregion			// 추가 클래스 함수
}
#endif			// #if UNITY_STANDALONE && EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
#endif			// #if SCRIPT_TEMPLATE_ONLY
