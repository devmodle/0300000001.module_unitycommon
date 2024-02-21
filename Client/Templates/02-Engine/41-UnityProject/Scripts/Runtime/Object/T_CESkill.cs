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
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public new struct STParams {
			public CEObjComponent.STParams m_stBaseParams;
			public STSkillInfo m_stSkillInfo;
			public CSkillTargetInfo m_oSkillTargetInfo;
		}

		#region 프로퍼티
		public new STParams Params { get; private set; }
		public STSkillInfo OriginSkillInfo { get; private set; } = STSkillInfo.INVALID;
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
			this.TargetSprite?.ExSetPropertyVal<SpriteRenderer>(KCDefine.U_PROPERTY_N_COLOR, 
				Access.GetColor(a_stParams.m_stSkillInfo.m_eSkillKinds));

			this.TargetSprite?.ExSetPropertyVal<SpriteRenderer>(KCDefine.U_PROPERTY_N_SPRITE, 
				Access.GetSprite(a_stParams.m_stSkillInfo.m_eSkillKinds));

			this.TargetSprite?.ExSetSortingOrder(Access.GetSortingOrderInfo(a_stParams.m_stSkillInfo.m_eSkillKinds));

			this.SubInit();
		}

		/** 스킬 정보를 리셋한다 */
		public virtual void ResetSkillInfo(STSkillInfo a_stSkillInfo) {
			// 리셋 가능 할 경우
			if(a_stSkillInfo.m_eSkillKinds != this.Params.m_stSkillInfo.m_eSkillKinds) {
				var stParams = this.Params;
				stParams.m_stSkillInfo = a_stSkillInfo;

				this.Init(stParams);
			}

			this.SubResetSkillInfo(a_stSkillInfo);
		}

		/** 어빌리티 값을 설정한다 */
		protected override void DoSetupAbilityVals(bool a_bIsReset = true) {
			base.DoSetupAbilityVals(a_bIsReset);

			// 스킬 정보가 존재 할 경우
			if(this.Params.m_stSkillInfo.m_eSkillKinds.ExIsValid()) {
				global::Func.SetupAbilityVals(this.Params.m_stSkillInfo, this.Params.m_oSkillTargetInfo, this.AbilityValDictWrapper.m_oDictB);
			}
		}
		#endregion // 함수
	}

	/** 스킬 - 접근 */
	public partial class CESkill : CEObjComponent {
		#region 함수
		/** 원본 스킬 정보를 설정한다 */
		public void SetOriginSkillInfo(STSkillInfo a_stSkillInfo) {
			this.OriginSkillInfo = a_stSkillInfo;
		}
		#endregion // 함수
	}

	/** 스킬 - 팩토리 */
	public partial class CESkill : CEObjComponent {
		#region 클래스 함수
		/** 스킬 매개 변수를 생성한다 */
		public static STParams MakeParams(CEngine a_oEngine, STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo, CEController a_oController = null, string a_oGameObjsPoolKey = KCDefine.B_TEXT_EMPTY) {
			return new STParams() {
				m_stBaseParams = CEObjComponent.MakeParams(a_oEngine, a_oController, a_oGameObjsPoolKey), m_stSkillInfo = a_stSkillInfo, m_oSkillTargetInfo = a_oSkillTargetInfo
			};
		}
		#endregion // 클래스 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
