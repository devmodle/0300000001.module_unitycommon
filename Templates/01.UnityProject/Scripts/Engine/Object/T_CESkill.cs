#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 스킬 */
	public partial class CESkill : CEObjComponent {
		/** 매개 변수 */
		public new struct STParams {
			public CEObjComponent.STParams m_stBaseParams;
			public STSkillInfo m_stSkillInfo;
			public CSkillTargetInfo m_oSkillTargetInfo;
		}

		#region 프로퍼티
		public new STParams Params { get; private set; }
		private List<CEFX> FXList { get; } = new List<CEFX>();
		#endregion			// 프로퍼티

		#region 함수
		/** 어빌리티 값을 설정한다 */
		public override void SetupAbilityVals() {
			base.SetupAbilityVals();
			this.OriginAbilityValDict.ExCopyTo(this.AbilityValDict, (a_dmAbilityVal) => a_dmAbilityVal);
		}

		/** 어빌리티 값을 갱신한다 */
		public override void UpdateAbilityVals() {
			base.UpdateAbilityVals();

			// 스킬 정보가 존재 할 경우
			if(this.Params.m_stSkillInfo.m_eSkillKinds.ExIsValid()) {
				global::Func.SetupAbilityVals(this.Params.m_stSkillInfo, this.Params.m_oSkillTargetInfo, this.OriginAbilityValDict);
			}
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
