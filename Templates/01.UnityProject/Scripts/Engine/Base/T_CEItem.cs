#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 아이템 */
	public partial class CEItem : CEComponent {
		/** 매개 변수 */
		public new partial struct STParams {
			public CEComponent.STParams m_stBaseParams;
			public STItemInfo m_stItemInfo;
			public CItemTargetInfo m_oItemTargetInfo;
		}

		#region 변수
		private STParams m_stParams;
		#endregion			// 변수

		#region 프로퍼티
		public STItemInfo ItemInfo => m_stParams.m_stItemInfo;
		public CItemTargetInfo ItemTargetInfo => m_stParams.m_oItemTargetInfo;
		public override EComponentType ComponentType => EComponentType.ITEM;
		#endregion			// 프로퍼티

		#region 함수
		/** 어빌리티 값을 설정한다 */
		public override void SetupAbilityVals() {
			base.SetupAbilityVals();
			var oAbilityTargetInfoDict = (m_stParams.m_oItemTargetInfo != null) ? m_stParams.m_oItemTargetInfo.m_oAbilityTargetInfoDict : m_stParams.m_stItemInfo.m_oAbilityTargetInfoDict;

			foreach(var stKeyVal in oAbilityTargetInfoDict) {
				global::Func.SetupAbilityVals(stKeyVal.Value, this.AbilityValDict);
			}
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
