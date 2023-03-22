#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 엔진 팩토리 */
	public static partial class Factory {
		#region 클래스 함수
		/** 그리드 정보를 생성한다 */
		public static STGridInfo MakeGridInfo(Vector3 a_stPivot, Vector3 a_stPos, Vector3 a_stOffset, Vector3Int a_stNumCells, int a_nIdx = KCDefine.B_VAL_0_INT, bool a_bIsEnableOverflow = false) {
			return Factory.MakeGridInfo(a_stPivot, a_stPos, a_stOffset, Access.CellSize, Access.MaxGridSize, a_stNumCells, a_nIdx, a_bIsEnableOverflow);
		}

		/** 그리드 정보를 생성한다 */
		public static STGridInfo MakeGridInfo(Vector3 a_stPivot, Vector3 a_stPos, Vector3 a_stOffset, Vector3 a_stCellSize, Vector3 a_stMaxGridSize, Vector3Int a_stNumCells, int a_nIdx = KCDefine.B_VAL_0_INT, bool a_bIsEnableOverflow = false) {
			var stGridInfo = new STGridInfo() {
				m_nIdx = a_nIdx, m_stBounds = new Bounds(Vector3.zero, new Vector3(a_stNumCells.x * a_stCellSize.x, a_stNumCells.y * a_stCellSize.y, KCDefine.B_VAL_0_REAL))
			};

			var stPos = a_stPos + a_stOffset;
			var stBoundsPos = stPos.ExGetPivotPos(KCDefine.B_ANCHOR_MID_CENTER, a_stPivot, stGridInfo.m_stBounds.size);
			var stViewBoundsPos = a_stPos.ExGetPivotPos(KCDefine.B_ANCHOR_MID_CENTER, a_stPivot, a_bIsEnableOverflow ? new Vector3(stGridInfo.m_stBounds.size.x, stGridInfo.m_stBounds.size.x * (a_stMaxGridSize.y / a_stMaxGridSize.x), KCDefine.B_VAL_0_REAL) : new Vector3(Mathf.Max(stGridInfo.m_stBounds.size.x, stGridInfo.m_stBounds.size.y), Mathf.Max(stGridInfo.m_stBounds.size.x, stGridInfo.m_stBounds.size.y), KCDefine.B_VAL_0_REAL));

			try {
				stGridInfo.m_stBounds = new Bounds(stBoundsPos, stGridInfo.m_stBounds.size);
				stGridInfo.m_stViewBounds = new Bounds(stViewBoundsPos, a_bIsEnableOverflow ? new Vector3(stGridInfo.m_stBounds.size.x, stGridInfo.m_stBounds.size.x * (a_stMaxGridSize.y / a_stMaxGridSize.x), KCDefine.B_VAL_0_REAL) : new Vector3(Mathf.Max(stGridInfo.m_stBounds.size.x, stGridInfo.m_stBounds.size.y), Mathf.Max(stGridInfo.m_stBounds.size.x, stGridInfo.m_stBounds.size.y), KCDefine.B_VAL_0_REAL));

				stGridInfo.m_stPivotPos = new Vector3(stGridInfo.m_stBounds.min.x, stGridInfo.m_stBounds.max.y, KCDefine.B_VAL_0_REAL);
				stGridInfo.m_stViewPivotPos = new Vector3(stGridInfo.m_stViewBounds.min.x, stGridInfo.m_stViewBounds.max.y, KCDefine.B_VAL_0_REAL);

				// 오버 플로우 모드 일 경우
				if(a_bIsEnableOverflow) {
					stGridInfo.m_stScale = Vector3.one * (a_stMaxGridSize.x / stGridInfo.m_stBounds.size.x);
				} else {
					stGridInfo.m_stScale = Vector3.one * Mathf.Min(a_stMaxGridSize.x / stGridInfo.m_stBounds.size.x, a_stMaxGridSize.y / stGridInfo.m_stBounds.size.y);
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"Factory.MakeGridInfo Exception: {oException.Message}");
				stGridInfo.m_stScale = Vector3.one;
			}

			return stGridInfo;
		}
		
		/** 그리드 정보를 생성한다 */
		public static void MakeGridInfos(CLevelInfo a_oLevelInfo, List<STGridInfo> a_oOutGridInfoList, float a_fDeltaX = KCDefine.B_VAL_0_REAL, float a_fDeltaY = KCDefine.B_VAL_0_REAL) {
				Factory.MakeGridInfos(a_oLevelInfo, Access.CellSize, Access.MaxGridSize, a_oOutGridInfoList, a_fDeltaX, a_fDeltaY);
		}

		/** 그리드 정보를 생성한다 */
		public static void MakeGridInfos(CLevelInfo a_oLevelInfo, Vector3 a_stCellSize, Vector3 a_stMaxGridSize, List<STGridInfo> a_oOutGridInfoList, float a_fDeltaX = KCDefine.B_VAL_0_REAL, float a_fDeltaY = KCDefine.B_VAL_0_REAL) {
			a_oOutGridInfoList.Clear();

			// TODO: 다중 그리드 및 수평 스크롤 처리 구현 예정
			for(int i = 0; i < KCDefine.B_VAL_1_INT; ++i) {
				switch(a_oLevelInfo.GridType) {
					case EGridType.SCROLL_H: {
						break;
					}
					case EGridType.SCROLL_V: {
						var stGridInfo = Factory.MakeGridInfo(KCDefine.B_ANCHOR_DOWN_CENTER, Vector3.zero, Vector3.zero, a_oLevelInfo.NumCells, i, true);
						stGridInfo.m_stScale = stGridInfo.m_stScale.ExIsValid() ? stGridInfo.m_stScale : Vector3.one;

						a_oOutGridInfoList.ExAddVal(Factory.MakeGridInfo(KCDefine.B_ANCHOR_DOWN_CENTER,
							new Vector3(KCDefine.B_VAL_0_REAL, (a_stMaxGridSize.y / -KCDefine.B_VAL_2_REAL) / stGridInfo.m_stScale.y, KCDefine.B_VAL_0_REAL),
							new Vector3(KCDefine.B_VAL_0_REAL, a_fDeltaY, KCDefine.B_VAL_0_REAL),
							a_stCellSize,
							a_stMaxGridSize,
							a_oLevelInfo.NumCells,
							i,
							true));

						break;
					}
					default: {
						var stGridInfo = Factory.MakeGridInfo(KCDefine.B_ANCHOR_MID_CENTER, Vector3.zero, Vector3.zero, a_stCellSize, a_stMaxGridSize, a_oLevelInfo.NumCells);
						stGridInfo.m_stScale = (a_oLevelInfo.GridType == EGridType.NONE) ? Vector3.one : stGridInfo.m_stScale;

						a_oOutGridInfoList.ExAddVal(stGridInfo);
						break;
					}
				}
			}
		}
		#endregion // 클래스 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
