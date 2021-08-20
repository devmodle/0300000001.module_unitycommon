using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
namespace SampleEngineName {
	//! 엔진 팩토리
	public static partial class Factory {
		#region 클래스 함수
		//! 그리드 정보를 생성한다
		public static STGridInfo MakeGridInfo(CLevelInfo a_oLevelInfo) {
			var stGridInfo = new STGridInfo() {
				m_stGridSize = new Vector3(a_oLevelInfo.NumCells.x * KDefine.E_SIZE_CELL.x, a_oLevelInfo.NumCells.y * KDefine.E_SIZE_CELL.y, KCDefine.B_VAL_0_FLT)
			};
			
			stGridInfo.m_stGridBounds = new Bounds(Vector3.zero, stGridInfo.m_stGridSize);

			try {
				float fScaleX = (KDefine.E_MAX_SIZE_GRID.x / stGridInfo.m_stGridBounds.size.x);
				float fScaleY = (KDefine.E_MAX_SIZE_GRID.y / stGridInfo.m_stGridBounds.size.y);

				stGridInfo.m_stGridScale = Vector3.one * Mathf.Min(fScaleX, fScaleY);
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning("Factory.MakeGridInfo Exception: {0}", oException.Message);
				stGridInfo.m_stGridScale = Vector3.one;
			}

			stGridInfo.m_stGridPivotPos = new Vector3(stGridInfo.m_stGridBounds.min.x, stGridInfo.m_stGridBounds.max.y, KCDefine.B_VAL_0_FLT);
			return stGridInfo;
		}
		#endregion			// 클래스 함수

		#region 추가 클래스 변수

		#endregion			// 추가 클래스 변수

		#region 추가 클래스 프로퍼티

		#endregion			// 추가 클래스 프로퍼티

		#region 추가 클래스 함수

		#endregion			// 추가 클래스 함수
	}
}
#endif			// #if NEVER_USE_THIS
