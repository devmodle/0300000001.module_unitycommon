#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 엔진 컴포넌트 */
	public abstract partial class CEComponent : CComponent {
		/** 매개 변수 */
		public struct STParams {
			public string m_oObjsPoolKey;
			public CEngine m_oEngine;
		}

		#region 변수
		
		#endregion			// 변수
		
		#region 프로퍼티
		public STParams Params { get; private set; }
		#endregion			// 프로퍼티

		#region 함수

		#endregion			// 함수

		#region 제네릭 함수

		#endregion			// 제네릭 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
