using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 상호 작용 가능 객체 */
	public partial class CEInteractableObj : CEObj {
		// 매개 변수
		public new partial struct STParams {
			public CEObj.STParams m_stBaseParams;
		}
		
		#region 변수
		private STParams m_stParams;
		private List<STAbilityValInfo> m_oAbilityValInfoList = new List<STAbilityValInfo>();
		#endregion			// 변수

		#region 프로퍼티
		public List<STAbilityValInfo> AbilityValInfoList => m_oAbilityValInfoList;
		#endregion			// 프로퍼티

		#region 함수
		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			base.Init(a_stParams.m_stBaseParams);
			m_stParams = a_stParams;
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
