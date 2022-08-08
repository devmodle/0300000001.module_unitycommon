#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 스킬 */
	public partial class CESkill : CEComponent {
		/** 매개 변수 */
		public new partial struct STParams {
			public CEComponent.STParams m_stBaseParams;
			public STSkillInfo m_stSkillInfo;
			public CSkillTargetInfo m_oSkillTargetInfo;
		}

		#region 변수
		private List<CEFX> m_oFXList = new List<CEFX>();
		#endregion			// 변수

		#region 프로퍼티
		public new STParams Params { get; private set; }
		#endregion			// 프로퍼티

		#region 함수
		/** 어빌리티 값을 설정한다 */
		public override void SetupAbilityVals() {
			base.SetupAbilityVals();
			global::Func.SetupAbilityVals(this.Params.m_stSkillInfo, this.Params.m_oSkillTargetInfo, this.AbilityValDict);
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
