using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MessagePack;

#if SCRIPT_TEMPLATE_ONLY
#if ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	#region 기본
	/** 그리드 정보 */
	public struct STGridInfo {
		public Vector3 m_stGridSize;
		public Vector3 m_stGridScale;
		public Vector3 m_stGridPivotPos;
		
		public Bounds m_stGridBounds;
	}

	/** 엔진 타입 랩퍼 */
	[MessagePackObject]
	public struct STEngineTypeWrapper {

	}
	#endregion			// 기본

	#region 추가 타입

	#endregion			// 추가 타입
}
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
