#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
using System.Linq;
using UnityEngine.EventSystems;
using EnhancedUI.EnhancedScroller;

namespace LevelEditorScene {
	/** 서브 레벨 에디터 씬 관리자 */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
#region 함수

#endregion // 함수

#region 조건부 함수
#if UNITY_EDITOR
		/** 기즈모를 그린다 */
		public override void OnDrawGizmos() {
			base.OnDrawGizmos();

			// 메인 카메라가 존재 할 경우
			if(CSceneManager.IsExistsMainCamera) {
				var stPrevColor = Gizmos.color;
				var stMainCameraPos = CSceneManager.ActiveSceneMainCamera.transform.position;
				var stPivotPos = stMainCameraPos + new Vector3(KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL, this.PlaneDistance);

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
				try {
					var oGridPosList = new List<Vector3>() {
						stPivotPos + this.ObjRootPivotPos + new Vector3(NSEngine.Access.MaxGridSize.x / -KCDefine.B_VAL_2_REAL, NSEngine.Access.MaxGridSize.y / -KCDefine.B_VAL_2_REAL, 0.0f) * CAccess.ResolutionUnitScale,
						stPivotPos + this.ObjRootPivotPos + new Vector3(NSEngine.Access.MaxGridSize.x / -KCDefine.B_VAL_2_REAL, NSEngine.Access.MaxGridSize.y / KCDefine.B_VAL_2_REAL, 0.0f) * CAccess.ResolutionUnitScale,
						stPivotPos + this.ObjRootPivotPos + new Vector3(NSEngine.Access.MaxGridSize.x / KCDefine.B_VAL_2_REAL, NSEngine.Access.MaxGridSize.y / KCDefine.B_VAL_2_REAL, 0.0f) * CAccess.ResolutionUnitScale,
						stPivotPos + this.ObjRootPivotPos + new Vector3(NSEngine.Access.MaxGridSize.x / KCDefine.B_VAL_2_REAL, NSEngine.Access.MaxGridSize.y / -KCDefine.B_VAL_2_REAL, 0.0f) * CAccess.ResolutionUnitScale
					};

					for(int i = 0; i < oGridPosList.Count; ++i) {
						int nIdx01 = (i + KCDefine.B_VAL_0_INT) % oGridPosList.Count;
						int nIdx02 = (i + KCDefine.B_VAL_1_INT) % oGridPosList.Count;

						Gizmos.color = Color.magenta;
						Gizmos.DrawLine(oGridPosList[nIdx01], oGridPosList[nIdx02]);
					}
				} finally {
					Gizmos.color = stPrevColor;
				}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			}
		}
#endif // #if UNITY_EDITOR

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endregion // 조건부 함수
	}

	/** 서브 레벨 에디터 씬 관리자 - 서브 */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

#region 변수

#endregion // 변수

#region 프로퍼티

#endregion // 프로퍼티

#region 함수

#endregion // 함수

#region 조건부 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		/** 씬을 설정한다 */
		private void SubSetupAwake() {
			this.SubSetupMidEditorUIs();
			this.SubSetupLeftEditorUIs();
			this.SubSetupRightEditorUIs();
		}

		/** 씬을 설정한다 */
		private void SubSetupStart() {
			// Do Something
		}

		/** 상태를 갱신한다 */
		private void SubOnUpdate(float a_fDeltaTime) {
			// 메인 카메라가 존재 할 경우
			if(CSceneManager.IsExistsMainCamera) {
				var stMousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.PlaneDistance);
				var stCursorPos = CSceneManager.ActiveSceneMainCamera.ScreenToWorldPoint(stMousePos).ExToLocal(this.ObjRoot);

				// 그리드 영역 일 경우
				if(m_oGridInfoList.ExIsValidIdx(this.SelGridInfoIdx) && this.SelGridInfo.m_stViewBounds.Contains(stCursorPos)) {
					var stIdx = stCursorPos.ExToIdx(this.SelGridInfo.m_stPivotPos, NSEngine.Access.CellSize);

					// 인덱스가 유효 할 경우
					if(m_oObjSpriteInfoLists.ExIsValidIdx(stIdx)) {
						// Do Something
					}

					// 스크롤이 가능 할 경우
					if(!Input.mouseScrollDelta.ExIsEquals(Vector3.zero)) {
						float fScrollDeltaX = (Input.mouseScrollDelta.x * NSEngine.Access.CellSize.x) * this.EditorObjRoot.transform.localScale.x;
						float fScrollDeltaY = (Input.mouseScrollDelta.y * NSEngine.Access.CellSize.y) * this.EditorObjRoot.transform.localScale.y;

						this.SetMEUIsGridScrollDelta(m_oRealDict.GetValueOrDefault(EKey.GRID_SCROLL_DELTA_X) + fScrollDeltaX, m_oRealDict.GetValueOrDefault(EKey.GRID_SCROLL_DELTA_Y) - fScrollDeltaY);
					}

					bool bIsValid01 = m_oObjSpriteInfoLists.ExIsValidIdx(stIdx) && m_oObjKindsDict.GetValueOrDefault(EKey.SEL_OBJ_KINDS) != EObjKinds.NONE;
					bool bIsValid02 = m_oGridInfoList.ExIsValidIdx(this.SelGridInfoIdx) && this.SelGridInfo.m_stViewBounds.Contains(stCursorPos);

					m_oSpriteDict.GetValueOrDefault(EKey.SEL_OBJ_SPRITE)?.gameObject.SetActive(bIsValid01 && bIsValid02);
					m_oSpriteDict.GetValueOrDefault(EKey.SEL_OBJ_SPRITE)?.gameObject.ExSetLocalPos(this.SelGridInfo.m_stPivotPos + stIdx.ExToPos(NSEngine.Access.CellCenterOffset, NSEngine.Access.CellSize));
				} else {
					m_oSpriteDict.GetValueOrDefault(EKey.SEL_OBJ_SPRITE)?.gameObject.SetActive(false);
				}
			}
		}

		/** UI 상태를 갱신한다 */
		private void SubUpdateUIsState() {
			this.SubUpdateMidEditorUIsState();
			this.SubUpdateLeftEditorUIsState();
			this.SubUpdateRightEditorUIsState();
		}

		/** 객체 스프라이트를 설정한다 */
		private void SubSetupObjSprite(STCellInfo a_stCellInfo, STCellObjInfo a_stCellObjInfo, SpriteRenderer a_oOutObjSprite) {
			// Do Something
		}

		/** 터치 시작 이벤트를 처리한다 */
		private void HandleTouchBeginEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var stIdx = a_oEventData.ExGetLocalPos(this.ObjRoot, this.ScreenSize).ExToIdx(this.SelGridInfo.m_stPivotPos, NSEngine.Access.CellSize);
			var stPrevIdx = m_oVec3IntDict.GetValueOrDefault(EKey.PREV_CELL_IDX);

			// 인덱스가 유효 할 경우
			if(!stIdx.Equals(stPrevIdx) && m_oObjSpriteInfoLists.ExIsValidIdx(stIdx)) {
				// Do Something
			}

			this.HandleTouchMoveEvent(a_oSender, a_oEventData);
		}

		/** 터치 이동 이벤트를 처리한다 */
		private void HandleTouchMoveEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var stIdx = a_oEventData.ExGetLocalPos(this.ObjRoot, this.ScreenSize).ExToIdx(this.SelGridInfo.m_stPivotPos, NSEngine.Access.CellSize);
			var stPrevIdx = m_oVec3IntDict.GetValueOrDefault(EKey.PREV_CELL_IDX);

			// 인덱스가 유효 할 경우
			if(!stIdx.Equals(stPrevIdx) && m_oObjSpriteInfoLists.ExIsValidIdx(stIdx)) {
				var oCellInfo = m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).GetCellInfo(stIdx);
				var eSelObjKinds = m_oObjKindsDict.GetValueOrDefault(EKey.SEL_OBJ_KINDS);

				// 객체 추가가 가능 할 경우
				if(Input.GetMouseButton((int)EMouseBtn.LEFT) && eSelObjKinds != EObjKinds.NONE) {
					var stCellObjInfo = Factory.MakeEditorCellObjInfo(eSelObjKinds);
					oCellInfo.m_oCellObjInfoList.ExAddVal(stCellObjInfo, (a_stCellObjInfo) => a_stCellObjInfo.ObjKinds == eSelObjKinds);
				}
				// 객체 제거가 가능 할 경우
				else if(Input.GetMouseButton((int)EMouseBtn.RIGHT) && oCellInfo.m_oCellObjInfoList.ExIsValid()) {
					oCellInfo.m_oCellObjInfoList.ExRemoveValAt(oCellInfo.m_oCellObjInfoList.Count - KCDefine.B_VAL_1_INT);
				}

				this.UpdateUIsState();
				m_oVec3IntDict.ExReplaceVal(EKey.PREV_CELL_IDX, stIdx);
			}
		}

		/** 터치 종료 이벤트를 처리한다 */
		private void HandleTouchEndEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var stIdx = a_oEventData.ExGetLocalPos(this.ObjRoot, this.ScreenSize).ExToIdx(this.SelGridInfo.m_stPivotPos, NSEngine.Access.CellSize);
			var stPrevIdx = m_oVec3IntDict.GetValueOrDefault(EKey.PREV_CELL_IDX);

			// 인덱스가 유효 할 경우
			if(!stIdx.Equals(stPrevIdx) && m_oObjSpriteInfoLists.ExIsValidIdx(stIdx)) {
				// Do Something
			}

			m_oVec3IntDict.ExReplaceVal(EKey.PREV_CELL_IDX, KCDefine.B_IDX_INVALID_3D);
		}

#if GOOGLE_SHEET_ENABLE
		/** 구글 시트가 로드 되었을 경우 */
		private void OnLoadGoogleSheet(CServicesManager a_oSender, STGoogleSheetLoadInfo a_stGoogleSheetLoadInfo, Dictionary<string, SimpleJSON.JSONNode> a_oJSONNodeInfoDict, bool a_bIsSuccess) {
			// 로드 되었을 경우
			if(a_bIsSuccess) {
				var oHandlerDict = new Dictionary<string, System.Action>() {
					[KCDefine.U_TABLE_P_G_ETC_INFO.ExGetFileName(false)] = () => CEtcInfoTable.Inst.SaveEtcInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString()),
					[KCDefine.U_TABLE_P_G_MISSION_INFO.ExGetFileName(false)] = () => CMissionInfoTable.Inst.SaveMissionInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString()),
					[KCDefine.U_TABLE_P_G_REWARD_INFO.ExGetFileName(false)] = () => CRewardInfoTable.Inst.SaveRewardInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString()),
					[KCDefine.U_TABLE_P_G_RES_INFO.ExGetFileName(false)] = () => CResInfoTable.Inst.SaveResInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString()),
					[KCDefine.U_TABLE_P_G_ITEM_INFO.ExGetFileName(false)] = () => CItemInfoTable.Inst.SaveItemInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString()),
					[KCDefine.U_TABLE_P_G_SKILL_INFO.ExGetFileName(false)] = () => CSkillInfoTable.Inst.SaveSkillInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString()),
					[KCDefine.U_TABLE_P_G_OBJ_INFO.ExGetFileName(false)] = () => CObjInfoTable.Inst.SaveObjInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString()),
					[KCDefine.U_TABLE_P_G_ABILITY_INFO.ExGetFileName(false)] = () => CAbilityInfoTable.Inst.SaveAbilityInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString()),
					[KCDefine.U_TABLE_P_G_PRODUCT_INFO.ExGetFileName(false)] = () => CProductTradeInfoTable.Inst.SaveProductTradeInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString())
				};

				oHandlerDict.GetValueOrDefault(a_stGoogleSheetLoadInfo.m_oSheetName)?.Invoke();
			}
		}

		/** 버전 정보 구글 시트를 로드했을 경우 */
		private void OnLoadVerInfoGoogleSheet(CServicesManager a_oSender, SimpleJSON.JSONNode a_oVerInfos, Dictionary<string, STLoadGoogleSheetInfo> a_oLoadGoogleSheetInfoDict, bool a_bIsSuccess) {
			// 로드 되었을 경우
			if(a_bIsSuccess) {
				m_oVerInfos = a_oVerInfos;
				Func.LoadGoogleSheets(a_oLoadGoogleSheetInfoDict.Values.ToList(), m_oGoogleSheetLoadHandlerDict, this.OnLoadGoogleSheets);
			} else {
				Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_ON_TABLE_LOAD_FAIL_MSG), null, false);
			}
		}

		/** 구글 시트를 로드했을 경우 */
		private void OnLoadGoogleSheets(CServicesManager a_oSender, bool a_bIsSuccess) {
			// 로드 되었을 경우
			if(a_bIsSuccess) {
				this.UpdateUIsState();
				Func.OnLoadGoogleSheets(m_oVerInfos);

				Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_ON_TABLE_LOAD_MSG), null, false);
			} else {
				Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_ON_TABLE_LOAD_FAIL_MSG), null, false);
			}
		}
#endif // #if GOOGLE_SHEET_ENABLE
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endregion // 조건부 함수
	}

	/** 서브 레벨 에디터 씬 관리자 - 중앙 에디터 UI */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
#region 조건부 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		/** 중앙 에디터 UI 를 설정한다 */
		private void SubSetupMidEditorUIs() {
			// Do Something
		}

		/** 중앙 에디터 UI 상태를 갱신한다 */
		private void SubUpdateMidEditorUIsState() {
			// Do Something
		}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endregion // 조건부 함수
	}

	/** 서브 레벨 에디터 씬 관리자 - 왼쪽 에디터 UI */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
#region 조건부 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		/** 왼쪽 에디터 UI 를 설정한다 */
		private void SubSetupLeftEditorUIs() {
			// Do Something
		}

		/** 왼쪽 에디터 UI 상태를 갱신한다 */
		private void SubUpdateLeftEditorUIsState() {
			// Do Something
		}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endregion // 조건부 함수
	}

	/** 서브 레벨 에디터 씬 관리자 - 오른쪽 에디터 UI */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
#region 조건부 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		/** 오른쪽 에디터 UI 를 설정한다 */
		private void SubSetupRightEditorUIs() {
			// 페이지 스크롤 스냅이 존재 할 경우
			if(m_oScrollSnapDict.GetValueOrDefault(EKey.RE_UIS_PAGE_SCROLL_SNAP) != null) {
				for(int i = 0; i < m_oScrollSnapDict.GetValueOrDefault(EKey.RE_UIS_PAGE_SCROLL_SNAP).NumberOfPanels; ++i) {
					string oSetupFuncName = string.Format(KDefine.LES_FUNC_N_FMT_SUB_SETUP_RE_UIS_PAGE_UIS, i + KCDefine.B_VAL_1_INT);
					string oUpdateFuncName = string.Format(KDefine.LES_FUNC_N_FMT_SUB_UPDATE_RE_UIS_PAGE_UIS, i + KCDefine.B_VAL_1_INT);

					m_oSubMethodInfoDict.TryAdd(ECallback.SETUP_RE_UIS_PAGE_UIS_01 + i, this.GetType().GetMethod(oSetupFuncName, KCDefine.B_BINDING_F_NON_PUBLIC_INSTANCE));
					m_oSubMethodInfoDict.TryAdd(ECallback.UPDATE_RE_UIS_PAGE_UIS_01 + i, this.GetType().GetMethod(oUpdateFuncName, KCDefine.B_BINDING_F_NON_PUBLIC_INSTANCE));
				}

				for(int i = 0; i < m_oScrollSnapDict.GetValueOrDefault(EKey.RE_UIS_PAGE_SCROLL_SNAP).NumberOfPanels; ++i) {
					m_oSubMethodInfoDict.GetValueOrDefault(ECallback.SETUP_RE_UIS_PAGE_UIS_01 + i)?.Invoke(this, new object[] {
						m_oUIsDict.GetValueOrDefault(EKey.RE_UIS_PAGE_UIS_01 + i)
					});
				}
			}
		}

		/** 오른쪽 에디터 UI 페이지 UI 1 를 설정한다 */
		private void SubSetupREUIsPageUIs01(GameObject a_oPageUIs) {
			// Do Something
		}

		/** 오른쪽 에디터 UI 페이지 UI 2 를 설정한다 */
		private void SubSetupREUIsPageUIs02(GameObject a_oPageUIs) {
			// 탭 UI 를 갱신한다 {
			var oTapUIsHandler = a_oPageUIs.GetComponentInChildren<CTapUIsHandler>();

			// 탭 UI 가 존재 할 경우
			if(oTapUIsHandler != null) {
				for(int i = 0; i < oTapUIsHandler.ContentsUIsList.Count; ++i) {
					this.SetupREUIsPageUIs02TapContentsUIs(oTapUIsHandler.ContentsUIsList[i], i);
				}
			}
			// 탭 UI 를 갱신한다 }
		}

		/** 오른쪽 에디터 UI 페이지 UI 2 탭 컨텐츠 UI 를 설정한다 */
		private void SetupREUIsPageUIs02TapContentsUIs(GameObject a_oContents, int a_nIdx) {
			var oScrollRect = a_oContents?.GetComponentInChildren<ScrollRect>();
			var oScrollViewContents = a_oContents?.ExFindChild(KCDefine.U_OBJ_N_CONTENTS);

			// 스크롤 영역이 존재 할 경우
			if(oScrollRect != null && oScrollViewContents != null) {
				// 스크롤러 셀 뷰가 존재 할 경우
				if(KDefine.LES_OBJ_KINDS_DICT_CONTAINER.TryGetValue(a_nIdx, out List<EObjKinds> oObjKindsList)) {
					int nMaxNumCells = oObjKindsList.Count / KDefine.LES_MAX_NUM_OBJ_KINDS_IN_ROW;
					int nNumExtraCells = (oObjKindsList.Count % KDefine.LES_MAX_NUM_OBJ_KINDS_IN_ROW > KCDefine.B_VAL_0_INT) ? KCDefine.B_VAL_1_INT : KCDefine.B_VAL_0_INT;

					for(int i = 0; i < nMaxNumCells + nNumExtraCells; ++i) {
						var oScrollerCellView = CFactory.CreateCloneObj(KCDefine.LES_OBJ_N_RE_UIS_PAGE_UIS_02_SCROLLER_CELL_VIEW, CResManager.Inst.GetRes<GameObject>(KCDefine.LES_OBJ_P_RE_UIS_PAGE_UIS_02_SCROLLER_CELL_VIEW), oScrollViewContents);
						this.SetupREUIsPageUIs02ScrollerCellView(oScrollerCellView, oObjKindsList, i * KDefine.LES_MAX_NUM_OBJ_KINDS_IN_ROW);
					}
				}
			}
		}

		/** 오른쪽 에디터 UI 페이지 UI 2 스크롤러 셀 뷰를 설정한다 */
		private void SetupREUIsPageUIs02ScrollerCellView(GameObject oScrollerCellView, List<EObjKinds> a_oObjKindsList, int a_nIdx) {
			for(int i = 0; i < KDefine.LES_MAX_NUM_OBJ_KINDS_IN_ROW; ++i) {
				var oBtn = oScrollerCellView.transform.GetChild(i).GetComponentInChildren<Button>();
				var eObjKinds = a_oObjKindsList.ExGetVal(i + a_nIdx, EObjKinds.NONE);

				oBtn?.ExSetInteractable(eObjKinds != EObjKinds.NONE);
				oBtn?.onClick.AddListener(() => this.OnTouchREUIsPageUIs02ScrollerCellViewBtn(eObjKinds));

				// 버튼이 존재 할 경우
				if(oBtn != null) {
					oBtn.image.sprite = Access.GetEditorObjSprite(eObjKinds, KCDefine.B_PREFIX_LEVEL_EDITOR_SCENE);
					oBtn.image.ExSetEnable(eObjKinds != EObjKinds.NONE);
				}
			}
		}

		/** 오른쪽 에디터 UI 상태를 갱신한다 */
		private void SubUpdateRightEditorUIsState() {
			// 스크롤 스냅이 존재 할 경우
			if(m_oScrollSnapDict.GetValueOrDefault(EKey.RE_UIS_PAGE_SCROLL_SNAP) != null) {
				// 페이지 UI 상태를 갱신한다
				for(int i = 0; i < m_oScrollSnapDict.GetValueOrDefault(EKey.RE_UIS_PAGE_SCROLL_SNAP).NumberOfPanels; ++i) {
					m_oSubMethodInfoDict.GetValueOrDefault(ECallback.UPDATE_RE_UIS_PAGE_UIS_01 + i)?.Invoke(this, new object[] {
						m_oUIsDict.GetValueOrDefault(EKey.RE_UIS_PAGE_UIS_01 + i)
					});
				}
			}
		}

		/** 오른쪽 에디터 UI 페이지 UI 1 상태를 갱신한다 */
		private void SubUpdateREUIsPageUIs01(GameObject a_oPageUIs) {
			// Do Something
		}

		/** 오른쪽 에디터 UI 페이지 UI 2 상태를 갱신한다 */
		private void SubUpdateREUIsPageUIs02(GameObject a_oPageUIs) {
			// Do Something
		}

		/** 오른쪽 에디터 UI 페이지 UI 2 스크롤러 셀 뷰 버튼을 눌렀을 경우 */
		private void OnTouchREUIsPageUIs02ScrollerCellViewBtn(EObjKinds a_eObjKinds) {
			m_oImgDict.GetValueOrDefault(EKey.ME_UIS_SEL_OBJ_IMG)?.ExSetSprite<Image>(Access.GetEditorObjSprite(a_eObjKinds, KCDefine.B_PREFIX_LEVEL_EDITOR_SCENE));
			m_oSpriteDict.GetValueOrDefault(EKey.SEL_OBJ_SPRITE)?.ExSetSprite<SpriteRenderer>(Access.GetEditorObjSprite(a_eObjKinds, KCDefine.B_PREFIX_LEVEL_EDITOR_SCENE));

			m_oObjKindsDict.ExReplaceVal(EKey.SEL_OBJ_KINDS, a_eObjKinds);
			this.UpdateUIsState();
		}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endregion // 조건부 함수
	}
}
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
#endif // #if SCRIPT_TEMPLATE_ONLY
