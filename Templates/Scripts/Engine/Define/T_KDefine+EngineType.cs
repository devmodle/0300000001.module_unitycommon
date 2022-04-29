using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MessagePack;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	#region 기본
	/** 그리드 정보 */
	public struct STGridInfo {
		public Vector3 m_stSize;
		public Vector3 m_stScale;
		public Vector3 m_stPivotPos;
		
		public Bounds m_stBounds;
	}

	/** 엔진 타입 랩퍼 */
	[MessagePackObject]
	public struct STEngineTypeWrapper {
		// Do Something
	}
	#endregion			// 기본
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
