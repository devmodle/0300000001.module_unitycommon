#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 아이템 */
	public partial class CEItem : CEObjComponent {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public new struct STParams {
			public CEObjComponent.STParams m_stBaseParams;
			public STItemInfo m_stItemInfo;
			public CItemTargetInfo m_oItemTargetInfo;
		}

		#region 프로퍼티
		public new STParams Params { get; private set; }
		public STItemInfo OriginItemInfo { get; private set; } = STItemInfo.INVALID;
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

			// 어빌리티 값을 설정한다
			this.SetupAbilityVals();

			// 스프라이트를 설정한다
			this.TargetSprite?.ExSetPropertyVal<SpriteRenderer>(KCDefine.U_PROPERTY_N_COLOR, Access.GetColor(a_stParams.m_stItemInfo.m_eItemKinds));
			this.TargetSprite?.ExSetPropertyVal<SpriteRenderer>(KCDefine.U_PROPERTY_N_SPRITE, Access.GetSprite(a_stParams.m_stItemInfo.m_eItemKinds));
			this.TargetSprite?.ExSetSortingOrder(Access.GetSortingOrderInfo(a_stParams.m_stItemInfo.m_eItemKinds));

			this.SubInit();
		}

		/** 아이템 정보를 리셋한다 */
		public virtual void ResetItemInfo(STItemInfo a_stItemInfo) {
			// 리셋 가능 할 경우
			if(a_stItemInfo.m_eItemKinds != this.Params.m_stItemInfo.m_eItemKinds) {
				var stParams = this.Params;
				stParams.m_stItemInfo = a_stItemInfo;

				this.Init(stParams);
			}

			this.SubResetItemInfo(a_stItemInfo);
		}

		/** 어빌리티 값을 설정한다 */
		protected override void DoSetupAbilityVals(bool a_bIsReset = true) {
			base.DoSetupAbilityVals(a_bIsReset);

			// 아이템 정보가 존재 할 경우
			if(this.Params.m_stItemInfo.m_eItemKinds.ExIsValid()) {
				global::Func.SetupAbilityVals(this.Params.m_stItemInfo, this.Params.m_oItemTargetInfo, this.AbilityValDictWrapper.m_oDictB);
			}
		}
		#endregion // 함수
	}

	/** 아이템 - 접근 */
	public partial class CEItem : CEObjComponent {
		#region 함수
		/** 원본 아이템 정보를 설정한다 */
		public void SetOriginItemInfo(STItemInfo a_stItemInfo) {
			this.OriginItemInfo = a_stItemInfo;
		}
		#endregion // 함수
	}

	/** 아이템 - 팩토리 */
	public partial class CEItem : CEObjComponent {
		#region 클래스 함수
		/** 매개 변수를 생성한다 */
		public static STParams MakeParams(CEngine a_oEngine, STItemInfo a_stItemInfo, CItemTargetInfo a_oItemTargetInfo, CEController a_oController = null, string a_oGameObjsPoolKey = KCDefine.B_TEXT_EMPTY) {
			return new STParams() {
				m_stBaseParams = CEObjComponent.MakeParams(a_oEngine, a_oController, a_oGameObjsPoolKey), m_stItemInfo = a_stItemInfo, m_oItemTargetInfo = a_oItemTargetInfo
			};
		}
		#endregion // 클래스 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
