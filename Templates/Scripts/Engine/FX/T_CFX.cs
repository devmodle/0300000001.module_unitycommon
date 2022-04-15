using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 효과 */
	public partial class CFX : CComponent {
		/** 매개 변수 */
		public struct STParams {
			public STFXInfo m_stFXInfo;
			public CEngine m_oEngine;
		}

		#region 변수
		private STParams m_stParams;
		#endregion			// 변수

		#region 프로퍼티
		public STFXInfo FXInfo => m_stParams.m_stFXInfo;
		#endregion			// 프로퍼티

		#region 함수
		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			m_stParams = a_stParams;
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
