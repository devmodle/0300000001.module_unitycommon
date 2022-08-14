#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 객체 제어자 */
	public partial class CEObjController : CEController {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			MOVE_DIRECTION,
			APPLY_SKILL_TARGET_INFO,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public new struct STParams {
			public CEController.STParams m_stBaseParams;
		}

		#region 변수
		private Dictionary<EKey, Vector3> m_oVec3Dict = new Dictionary<EKey, Vector3>() {
			[EKey.MOVE_DIRECTION] = Vector3.zero
		};

		private Dictionary<EKey, CSkillTargetInfo> m_oSkillTargetInfoDict = new Dictionary<EKey, CSkillTargetInfo>() {
			[EKey.APPLY_SKILL_TARGET_INFO] = null
		};
		#endregion			// 변수

		#region 프로퍼티
		public new STParams Params { get; private set; }
		public bool IsAutoControl { get; set; } = false;
		
		public Vector3 MoveDirection => m_oVec3Dict[EKey.MOVE_DIRECTION];
		public CSkillTargetInfo ApplySkillTargetInfo => m_oSkillTargetInfoDict[EKey.APPLY_SKILL_TARGET_INFO];
		#endregion			// 프로퍼티

		#region 함수
		
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
