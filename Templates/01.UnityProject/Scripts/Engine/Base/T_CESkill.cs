using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 스킬 */
	public partial class CESkill : CEComponent {
		/** 매개 변수 */
		public new partial struct STParams {
			public CEComponent.STParams m_stBaseParams;
			public STSkillInfo m_stSkillInfo;
			public CSkillInfo m_oSkillInfo;
		}

		#region 변수
		private STParams m_stParams;
		private List<CEFX> m_oFXList = new List<CEFX>();
		#endregion			// 변수

		#region 프로퍼티
		public STSkillInfo SkillInfo => m_stParams.m_stSkillInfo;
		#endregion			// 프로퍼티

		#region 함수
		/** 어빌리티 값을 설정한다 */
		public override void SetupAbilityVals() {
			base.SetupAbilityVals();
			var oAbilityTargetInfoDict = (m_stParams.m_oSkillInfo != null) ? m_stParams.m_oSkillInfo.m_oAbilityTargetInfoDict : m_stParams.m_stSkillInfo.m_oAbilityTargetInfoDict;

			foreach(var stKeyVal in oAbilityTargetInfoDict) {
				global::Func.SetupAbilityVals(stKeyVal.Value, this.AbilityValDict);
			}
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
