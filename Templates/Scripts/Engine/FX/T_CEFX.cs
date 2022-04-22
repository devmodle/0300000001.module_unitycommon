using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 효과 */
	public partial class CEFX : CComponent {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			FX_PARTICLE,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public struct STParams {
			public STFXInfo m_stFXInfo;
			public CEngine m_oEngine;
		}

		#region 상수
		private static readonly Dictionary<EKey, string> PARTICLE_NAME_DICT = new Dictionary<EKey, string>() {
			[EKey.FX_PARTICLE] = "FXParticle"
		};
		#endregion			// 상수

		#region 변수
		private STParams m_stParams;
		
		private Dictionary<EKey, ParticleSystem> m_oParticleDict = new Dictionary<EKey, ParticleSystem>() {
			[EKey.FX_PARTICLE] = null
		};
		#endregion			// 변수

		#region 프로퍼티
		public STFXInfo FXInfo => m_stParams.m_stFXInfo;
		#endregion			// 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			for(int i = (int)EKey.FX_PARTICLE; i <= (int)EKey.FX_PARTICLE; ++i) {
				m_oParticleDict[(EKey)i] = this.gameObject.ExFindComponent<ParticleSystem>(CEFX.PARTICLE_NAME_DICT[(EKey)i]);
				m_oParticleDict[(EKey)i]?.ExReset(false);
			}
		}

		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			m_stParams = a_stParams;
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
