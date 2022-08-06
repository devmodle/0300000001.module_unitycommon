#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 제어자 */
	public partial class CEController : CComponent {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public partial struct STParams {
			public CEngine m_oEngine;
			public CEComponent m_oOwner;
		}

		#region 변수
		
		#endregion			// 변수

		#region 프로퍼티
		public STParams Params { get; private set; }
		#endregion			// 프로퍼티

		#region 함수
		
		#endregion			// 함수

		#region 제네릭 함수
		/** 소유자를 반환한다 */
		public T GetOwner<T>() where T : CEComponent {
			return this.Params.m_oOwner as T;
		}
		#endregion			// 제네릭 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
