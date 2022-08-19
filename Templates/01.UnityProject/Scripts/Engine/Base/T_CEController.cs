#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 제어자 */
	public abstract partial class CEController : CEComponent {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public new struct STParams {
			public CEComponent.STParams m_stBaseParams;
			public CEObjComponent m_oOwner;
		}

		#region 변수
		
		#endregion			// 변수

		#region 프로퍼티
		public new STParams Params { get; private set; }
		public List<CEObjComponent> TargetObjList { get; } = new List<CEObjComponent>();
		#endregion			// 프로퍼티

		#region 함수

		#endregion			// 함수

		#region 제네릭 함수
		/** 소유자를 반환한다 */
		public T GetOwner<T>() where T : CEObjComponent {
			return this.Params.m_oOwner as T;
		}

		/** 타겟을 반환한다 */
		public T GetTarget<T>(int a_nIdx) where T : CEObjComponent {
			return this.TargetObjList.ExGetVal(a_nIdx, null) as T;
		}
		#endregion			// 제네릭 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
