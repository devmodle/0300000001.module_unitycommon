#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 효과 */
	public partial class CEFX : CEComponent {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			FX_PARTICLE,
			[HideInInspector] MAX_VAL
		}
		
		/** 매개 변수 */
		public new partial struct STParams {
			public CEComponent.STParams m_stBaseParams;
			public STFXInfo m_stTableFXInfo;
		}

		#region 변수
		private STParams m_stParams;
		private Dictionary<EKey, ParticleSystem> m_oParticleDict = new Dictionary<EKey, ParticleSystem>();
		#endregion			// 변수

		#region 프로퍼티
		public STFXInfo TableFXInfo => m_stParams.m_stTableFXInfo;
		public override EComponentType ComponentType => EComponentType.FX;
		#endregion			// 프로퍼티

		#region 함수
		/** 어빌리티 값을 설정한다 */
		public override void SetupAbilityVals() {
			base.SetupAbilityVals();
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
