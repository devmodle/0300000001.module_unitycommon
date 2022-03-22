using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 효과 */
	public class CFX : CComponent {
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

		#region 추가 변수

		#endregion			// 추가 변수

		#region 추가 프로퍼티

		#endregion			// 추가 프로퍼티

		#region 함수
		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			m_stParams = a_stParams;
		}
		#endregion			// 함수

		#region 추가 함수

		#endregion			// 추가 함수
	}
}
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
