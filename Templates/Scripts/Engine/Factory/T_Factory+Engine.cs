using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
namespace SampleEngineName {
	//! 엔진 팩토리
	public static partial class Factory {
		#region 클래스 함수
		//! 그리드 정보를 생성한다
		public static STGridInfo MakeGridInfo(CLevelInfo a_oLevelInfo) {
			return new STGridInfo() {
				m_stGridSize = Vector3.zero,
				m_stGridScale = Vector3.zero,
				m_stGridPivotPos = Vector3.zero,
				
				m_stGridBounds = new Bounds(Vector3.zero, Vector3.zero)
			};
		}
		#endregion			// 클래스 함수
	}
}
#endif			// #if NEVER_USE_THIS
