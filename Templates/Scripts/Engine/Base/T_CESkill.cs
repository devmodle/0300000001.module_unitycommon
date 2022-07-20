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
		}

		#region 변수
		private STParams m_stParams;
		private List<CEFX> m_oFXList = new List<CEFX>();
		#endregion			// 변수

		#region 프로퍼티
		public STSkillInfo SkillInfo => m_stParams.m_stSkillInfo;
		#endregion			// 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();
		}

		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			base.Init(a_stParams.m_stBaseParams);
			m_stParams = a_stParams;
		}
		#endregion			// 함수
	}

	/** 서브 스킬 */
	public partial class CESkill : CEComponent {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

		#region 추가 변수

		#endregion			// 추가 변수

		#region 추가 함수
		/** 어빌리티 값을 설정한다 */
		public override void SetupAbilityVals() {
			base.SetupAbilityVals();

			foreach(var stKeyVal in m_stParams.m_stSkillInfo.m_oAbilityTargetInfoDict) {
				global::Func.SetupAbilityVals(stKeyVal.Value, this.IntAbilityValDict, this.RealAbilityValDict);
			}
		}
		#endregion			// 추가 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
