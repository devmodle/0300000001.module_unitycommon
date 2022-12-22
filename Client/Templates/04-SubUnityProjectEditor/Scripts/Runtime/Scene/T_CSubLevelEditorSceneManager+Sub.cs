#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
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
#endregion // 함수

#region 조건부 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		/** UI 상태를 갱신한다 */
		private void SubUpdateUIsState() {
			// Do Something
		}

		/** 터치 시작 이벤트를 처리한다 */
		private void HandleTouchBeginEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var stIdx = a_oEventData.ExGetLocalPos(this.ObjRoot).ExToIdx(m_oGridInfoList[this.SelGridInfoIdx].m_stPivotPos, NSEngine.Access.CellSize);
		}

		/** 터치 이동 이벤트를 처리한다 */
		private void HandleTouchMoveEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var stIdx = a_oEventData.ExGetLocalPos(this.ObjRoot).ExToIdx(m_oGridInfoList[this.SelGridInfoIdx].m_stPivotPos, NSEngine.Access.CellSize);
		}

		/** 터치 종료 이벤트를 처리한다 */
		private void HandleTouchEndEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var stIdx = a_oEventData.ExGetLocalPos(this.ObjRoot).ExToIdx(m_oGridInfoList[this.SelGridInfoIdx].m_stPivotPos, NSEngine.Access.CellSize);
		}
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
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endregion // 조건부 함수
	}

	/** 서브 레벨 에디터 씬 관리자 - 오른쪽 에디터 UI */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
#region 조건부 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		/** 오른쪽 에디터 UI 를 설정한다 */
		private void SubSetupRightEditorUIs() {
			// Do Something
		}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endregion // 조건부 함수
	}
}
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
#endif // #if SCRIPT_TEMPLATE_ONLY
