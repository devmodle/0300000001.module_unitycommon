#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 효과 */
	public partial class CEFX : CEObjComponent {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public new struct STParams {
			public CEObjComponent.STParams m_stBaseParams;
			public STFXInfo m_stFXInfo;
		}

		#region 변수

		#endregion // 변수

		#region 프로퍼티
		public new STParams Params { get; private set; }
		public STFXInfo OriginFXInfo { get; private set; } = STFXInfo.INVALID;
		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();
			this.SubAwake();
		}

		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			base.Init(a_stParams.m_stBaseParams);
			this.Params = a_stParams;

			// 정렬 순서를 설정한다
			this.TargetSprite?.ExSetSortingOrder(Access.GetSortingOrderInfo(a_stParams.m_stFXInfo.m_eFXKinds));
			this.TargetParticleFX?.ExSetSortingOrder(Access.GetSortingOrderInfo(a_stParams.m_stFXInfo.m_eFXKinds));

			this.SubInit();
		}

		/** 효과 정보를 리셋한다 */
		public virtual void ResetFXInfo(STFXInfo a_stFXInfo) {
			// 리셋 가능 할 경우
			if(a_stFXInfo.m_eFXKinds != this.Params.m_stFXInfo.m_eFXKinds) {
				var stParams = this.Params;
				stParams.m_stFXInfo = a_stFXInfo;

				this.Init(stParams);
			}

			this.SubResetFXInfo(a_stFXInfo);
		}

		/** 어빌리티 값을 설정한다 */
		protected override void DoSetupAbilityVals(bool a_bIsReset = true) {
			base.DoSetupAbilityVals(a_bIsReset);
		}
		#endregion // 함수
	}

	/** 효과 - 접근 */
	public partial class CEFX : CEObjComponent {
		#region 함수
		/** 원본 효과 정보를 설정한다 */
		public void SetOriginFXInfo(STFXInfo a_stFXInfo) {
			this.OriginFXInfo = a_stFXInfo;
		}
		#endregion // 함수
	}

	/** 효과 - 팩토리 */
	public partial class CEFX : CEObjComponent {
		#region 클래스 함수
		/** 효과 매개 변수를 생성한다 */
		public static STParams MakeParams(CEngine a_oEngine, STFXInfo a_stFXInfo, CEController a_oController = null, string a_oObjsPoolKey = KCDefine.B_TEXT_EMPTY) {
			return new STParams() {
				m_stBaseParams = CEObjComponent.MakeParams(a_oEngine, a_oController, a_oObjsPoolKey), m_stFXInfo = a_stFXInfo
			};
		}
		#endregion // 클래스 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
