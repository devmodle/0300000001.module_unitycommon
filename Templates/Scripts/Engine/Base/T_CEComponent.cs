using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 컴포넌트 */
	public partial class CEComponent : CComponent {
		/** 매개 변수 */
		public partial struct STParams {
			public string m_oObjsPoolKey;
			public CEngine m_oEngine;
		}

		#region 변수
		private STParams m_stParams;
		private Dictionary<ulong, STAbilityValInfo> m_oAbilityValInfoDict = new Dictionary<ulong, STAbilityValInfo>();
		#endregion			// 변수

		#region 프로퍼티
		public string ObjsPoolKey => m_stParams.m_oObjsPoolKey;
		public CEngine Engine => m_stParams.m_oEngine;
		public Dictionary<ulong, STAbilityValInfo> AbilityValInfoDict => m_oAbilityValInfoDict;
		#endregion			// 프로퍼티

		#region 초기화
		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			m_stParams = a_stParams;
		}
		#endregion			// 초기화
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
