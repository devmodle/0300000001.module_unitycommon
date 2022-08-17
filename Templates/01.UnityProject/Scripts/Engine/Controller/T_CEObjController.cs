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
			IS_AUTO_CONTROL,
			MOVE_POS,
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
		public Dictionary<ESkillKinds, System.DateTime> ApplySkillTimeDict { get; } = new Dictionary<ESkillKinds, System.DateTime>();

		public bool IsAutoControl => this.BoolDict.GetValueOrDefault(EKey.IS_AUTO_CONTROL);
		public Vector3 MovePos => this.Vec3Dict.GetValueOrDefault(EKey.MOVE_POS);
		public Vector3 MoveDirection => this.Vec3Dict.GetValueOrDefault(EKey.MOVE_DIRECTION);
		public CSkillTargetInfo ApplySkillTargetInfo => this.SkillTargetInfoDict.GetValueOrDefault(EKey.APPLY_SKILL_TARGET_INFO);

		/** =====> 기타 <===== */
		private Dictionary<EKey, Vector3> Vec3Dict { get; } = new Dictionary<EKey, Vector3>() {
			[EKey.MOVE_POS] = KCDefine.B_POS_INVALID
		};

		private Dictionary<EKey, STSkillInfo> SkillInfoDict { get; } = new Dictionary<EKey, STSkillInfo>() {
			[EKey.APPLY_SKILL_INFO] = STSkillInfo.INVALID
		};

		private Dictionary<EKey, bool> BoolDict { get; } = new Dictionary<EKey, bool>();
		private Dictionary<EKey, CSkillTargetInfo> SkillTargetInfoDict { get; } = new Dictionary<EKey, CSkillTargetInfo>();
		#endregion			// 프로퍼티

		#region 함수
		/** 적용 스킬 정보를 리셋한다 */
		public virtual void ResetApplySkillInfo() {
			this.SkillInfoDict.ExReplaceVal(EKey.APPLY_SKILL_INFO, STSkillInfo.INVALID);
			this.SkillTargetInfoDict.ExReplaceVal(EKey.APPLY_SKILL_TARGET_INFO, null);
		}

		/** 자동 제어 여부를 변경한다 */
		public void SetIsAutoControl(bool a_bIsAutoControl) {
			this.BoolDict.ExReplaceVal(EKey.IS_AUTO_CONTROL, a_bIsAutoControl);
		}

		/** 이동 위치를 변경한다 */
		public void SetMovePos(Vector3 a_stPos) {
			this.Vec3Dict.ExReplaceVal(EKey.MOVE_POS, a_stPos);
		}

		/** 이동 방향을 변경한다 */
		public void SetMoveDirection(Vector3 a_stDirection) {
			this.Vec3Dict.ExReplaceVal(EKey.MOVE_DIRECTION, a_stDirection.normalized);
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
