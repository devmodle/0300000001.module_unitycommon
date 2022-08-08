#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 객체 */
	public partial class CEObj : CEComponent {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			OBJ_SPRITE,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public new partial struct STParams {
			public CEComponent.STParams m_stBaseParams;
			public STObjInfo m_stObjInfo;
			public CObjTargetInfo m_oObjTargetInfo;
		}

		#region 변수
		private Dictionary<EKey, SpriteRenderer> m_oSpriteDict = new Dictionary<EKey, SpriteRenderer>();
		#endregion			// 변수

		#region 프로퍼티
		public new STParams Params { get; private set; }
		public Vector3Int Idx { get; set; } = Vector3Int.zero;
		#endregion			// 프로퍼티

		#region 함수
		/** 어빌리티 값을 설정한다 */
		public override void SetupAbilityVals() {
			base.SetupAbilityVals();
			global::Func.SetupAbilityVals(this.Params.m_stObjInfo, this.Params.m_oObjTargetInfo, this.AbilityValDict);

			// 스킬 타겟 정보가 존재 할 경우
			if(this.Params.m_oObjTargetInfo != null && this.Params.m_oObjTargetInfo.m_oTargetInfoDictContainer.TryGetValue(ETargetType.SKILL, out List<CTargetInfo> oTargetInfoList)) {
				for(int i = 0; i < oTargetInfoList.Count; ++i) {
					// 패시브 스킬 일 경우
					if((ESkillType)oTargetInfoList[i].Kinds.ExKindsToType() == ESkillType.PASSIVE) {
						global::Func.SetupAbilityVals(oTargetInfoList[i].m_oAbilityTargetInfoDict, this.AbilityValDict);
					}
				}
			}
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
