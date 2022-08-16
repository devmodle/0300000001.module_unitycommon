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
			APPLY_SKILL_INFO,
			APPLY_SKILL_TARGET_INFO,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public new struct STParams {
			public CEController.STParams m_stBaseParams;
		}

		#region 프로퍼티
		public new STParams Params { get; private set; }
		public bool IsAutoControl { get; set; } = false;
		public Dictionary<ESkillKinds, System.DateTime> ApplySkillTimeDict { get; } = new Dictionary<ESkillKinds, System.DateTime>();
		
		public Vector3 MoveDirection => this.Vec3Dict.GetValueOrDefault(EKey.MOVE_DIRECTION);
		public CSkillTargetInfo ApplySkillTargetInfo => this.SkillTargetInfoDict.GetValueOrDefault(EKey.APPLY_SKILL_TARGET_INFO);

		/** =====> 기타 <===== */
		private Dictionary<EKey, STSkillInfo> SkillInfoDict { get; } = new Dictionary<EKey, STSkillInfo>() {
			[EKey.APPLY_SKILL_INFO] = STSkillInfo.INVALID
		};

		private Dictionary<EKey, Vector3> Vec3Dict { get; } = new Dictionary<EKey, Vector3>();
		private Dictionary<EKey, CSkillTargetInfo> SkillTargetInfoDict { get; } = new Dictionary<EKey, CSkillTargetInfo>();
		#endregion			// 프로퍼티

		#region 함수
		/** 적용 스킬 정보를 리셋한다 */
		public virtual void ResetApplySkillInfo() {
			this.SkillInfoDict.ExReplaceVal(EKey.APPLY_SKILL_INFO, STSkillInfo.INVALID);
			this.SkillTargetInfoDict.ExReplaceVal(EKey.APPLY_SKILL_TARGET_INFO, null);
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
