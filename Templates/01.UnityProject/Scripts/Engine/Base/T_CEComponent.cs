#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 컴포넌트 */
	public abstract partial class CEComponent : CComponent {
		/** 매개 변수 */
		public partial struct STParams {
			public string m_oObjsPoolKey;
			public CEngine m_oEngine;
			public CEComponent m_oOwner;
		}

		#region 변수
		private STParams m_stParams;
		private Dictionary<EAbilityKinds, decimal> m_oAbilityValDict = new Dictionary<EAbilityKinds, decimal>();
		#endregion			// 변수
		
		#region 프로퍼티
		public string ObjsPoolKey => m_stParams.m_oObjsPoolKey;
		public CEngine Engine => m_stParams.m_oEngine;
		public CEComponent Owner => m_stParams.m_oOwner;
		public Dictionary<EAbilityKinds, decimal> AbilityValDict => m_oAbilityValDict;

		public abstract EComponentType ComponentType { get; }
		#endregion			// 프로퍼티

		#region 함수
		/** 어빌리티 값을 설정한다 */
		public virtual void SetupAbilityVals() {
			// Do Something
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
