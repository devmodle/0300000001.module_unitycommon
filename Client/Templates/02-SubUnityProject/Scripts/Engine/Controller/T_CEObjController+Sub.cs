#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 서브 객체 제어자 */
	public partial class CEObjController : CEController {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			UPDATE_SKIP_TIME,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		private Dictionary<ESubKey, float> m_oRealDict = new Dictionary<ESubKey, float>();
		#endregion // 변수

		#region 프로퍼티

		#endregion // 프로퍼티

		#region 함수
		/** 공격을 처리한다 */
		public virtual void Attack(CEObj a_oTargetObj, CESkill a_oSkill) {
			var oAbilityValDict = CCollectionManager.Inst.SpawnDict<EAbilityKinds, decimal>();

			try {
				float fPercent = Random.Range(KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_1_REAL);
				float fCriticalRate = (float)a_oTargetObj.AbilityValDictWrapper.m_oDict01.ExGetAbilityVal(EAbilityKinds.STAT_CRITICAL_RATE_01);

				this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDict01.ExCopyTo(oAbilityValDict, (a_dmAbilityVal) => a_dmAbilityVal);
				global::Func.SetupAbilityVals(a_oSkill.Params.m_stSkillInfo, a_oSkill.Params.m_oSkillTargetInfo, oAbilityValDict);

				// 공격을 회피했을 경우
				if(fPercent.ExIsLessEquals((float)a_oTargetObj.AbilityValDictWrapper.m_oDict01.ExGetAbilityVal(EAbilityKinds.STAT_AVOID_RATE_01))) {
					this.GetOwner<CEObj>().Params.m_stBaseParams.m_oCallbackDict.GetValueOrDefault(CEObj.ECallback.ENGINE_OBJ_EVENT)?.Invoke(a_oTargetObj, EEngineObjEvent.AVOID, string.Empty);
				} else {
					decimal dmDamage = System.Math.Clamp(oAbilityValDict.ExGetAbilityVal(EAbilityKinds.STAT_ATK_01) - a_oTargetObj.AbilityValDictWrapper.m_oDict01.ExGetAbilityVal(EAbilityKinds.STAT_DEF_01), KCDefine.B_VAL_0_INT, decimal.MaxValue);
					decimal dmPDamage = System.Math.Clamp(oAbilityValDict.ExGetAbilityVal(EAbilityKinds.STAT_P_ATK_01) - a_oTargetObj.AbilityValDictWrapper.m_oDict01.ExGetAbilityVal(EAbilityKinds.STAT_P_DEF_01), KCDefine.B_VAL_0_INT, decimal.MaxValue);
					decimal dmMDamage = System.Math.Clamp(oAbilityValDict.ExGetAbilityVal(EAbilityKinds.STAT_M_ATK_01) - a_oTargetObj.AbilityValDictWrapper.m_oDict01.ExGetAbilityVal(EAbilityKinds.STAT_M_DEF_01), KCDefine.B_VAL_0_INT, decimal.MaxValue);

					decimal dmTotalDamage = dmDamage + dmPDamage + dmMDamage;
					dmTotalDamage = fPercent.ExIsLessEquals(fCriticalRate) ? dmTotalDamage * KCDefine.B_VAL_2_INT : dmTotalDamage;

					a_oTargetObj.AbilityValDictWrapper.m_oDict01.ExIncrAbilityVal(EAbilityKinds.STAT_HP_01, -dmTotalDamage);
					this.GetOwner<CEObj>().Params.m_stBaseParams.m_oCallbackDict.GetValueOrDefault(CEObj.ECallback.ENGINE_OBJ_EVENT)?.Invoke(a_oTargetObj, fPercent.ExIsLessEquals(fCriticalRate) ? EEngineObjEvent.CRITICAL_DAMAGE : EEngineObjEvent.DAMAGE, $"{dmTotalDamage:0}");
				}
			} finally {
				CCollectionManager.Inst.DespawnDict(oAbilityValDict);
			}
		}

		/** 이동을 처리한다 */
		public virtual void Move(Vector3 a_stVal, EVecType a_eVecType = EVecType.DIRECTION) {
			this.SetState(EState.MOVE);
			this.SetMovePos((a_eVecType == EVecType.POS) ? a_stVal : Vector3.zero);
			this.SetMoveDirection((a_eVecType == EVecType.DIRECTION) ? a_stVal : Vector3.zero);
		}

		/** 스킬을 적용시킨다 */
		public virtual void ApplySkill(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo) {
			// 스킬 적용이 가능 할 경우
			if(this.IsEnableSkillState() && this.IsEnableApplySkill(a_stSkillInfo, a_oSkillTargetInfo)) {
				this.SetState(EState.SKILL);
				this.DoApplySkill(a_stSkillInfo, a_oSkillTargetInfo);
			}
		}

		/** 스킬 적용 가능 여부를 검사한다 */
		protected virtual bool IsEnableApplySkill(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo) {
			// 적용 스킬 타겟 정보가 없을 경우
			if(m_oSkillInfoDict.GetValueOrDefault(EKey.APPLY_SKILL_INFO).m_eSkillKinds == ESkillKinds.NONE) {
				return true;
			}

			var oAbilityValDict = CCollectionManager.Inst.SpawnDict<EAbilityKinds, decimal>();

			try {
				global::Func.SetupAbilityVals(a_stSkillInfo, a_oSkillTargetInfo, oAbilityValDict);
				double dblDeltaTime = System.DateTime.Now.ExGetDeltaTime(this.ApplySkillTimeDict.GetValueOrDefault(a_stSkillInfo.m_eSkillKinds, System.DateTime.Today.AddDays(-KCDefine.B_VAL_1_INT)));

				return (decimal)dblDeltaTime >= this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDict01.ExGetAbilityVal((a_stSkillInfo.SkillType == ESkillType.ACTION) ? EAbilityKinds.STAT_ATK_DELAY_01 : EAbilityKinds.STAT_SKILL_DELAY_01, (a_stSkillInfo.SkillType == ESkillType.ACTION) ? this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDict01.GetValueOrDefault(EAbilityKinds.STAT_ATK_DELAY_01) : oAbilityValDict.GetValueOrDefault(EAbilityKinds.STAT_SKILL_DELAY_01));
			} finally {
				CCollectionManager.Inst.DespawnDict(oAbilityValDict);
			}
		}

		/** 이동 상태를 처리한다 */
		protected override void HandleMoveState(float a_fDeltaTime) {
			base.HandleMoveState(a_fDeltaTime);

			var stNextPos = this.GetOwner<CEObj>().transform.localPosition + ((m_oVec3Dict[EKey.MOVE_DIRECTION] * (float)this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDict01.GetValueOrDefault(EAbilityKinds.STAT_MOVE_SPEED_01)) * a_fDeltaTime);
			stNextPos.x = Mathf.Clamp(stNextPos.x, this.Engine.EpisodeSize.x / -KCDefine.B_VAL_2_REAL, this.Engine.EpisodeSize.x / KCDefine.B_VAL_2_REAL);
			stNextPos.y = Mathf.Clamp(stNextPos.y, (this.Engine.EpisodeSize.y / -KCDefine.B_VAL_2_REAL) + KDefine.E_OFFSET_BOTTOM, this.Engine.EpisodeSize.y / KCDefine.B_VAL_2_REAL);

			this.GetOwner<CEObj>().transform.localPosition = new Vector3(stNextPos.x, stNextPos.y, stNextPos.y / this.Engine.EpisodeSize.y);
		}

		/** 스킬 상태를 처리한다 */
		protected override void HandleSkillState(float a_fDeltaTime) {
			base.HandleSkillState(a_fDeltaTime);
		}

		/** 스킬을 적용시킨다 */
		protected virtual void DoApplySkill(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo) {
			m_oSkillInfoDict.ExReplaceVal(EKey.APPLY_SKILL_INFO, a_stSkillInfo);
			m_oSkillTargetInfoDict.ExReplaceVal(EKey.APPLY_SKILL_TARGET_INFO, a_oSkillTargetInfo);
		}

		/** 초기화 */
		private void SubAwake() {
			// Do Something
		}

		/** 초기화 */
		private void SubInit() {
			this.SetState(EState.APPEAR);
			this.SetMovePos(Vector3.zero);
		}

		/** 제거 되었을 경우 */
		private void SubOnDestroy() {
			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsAppRunning) {
					// Do Something
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CEObjController.SubOnDestroy Exception: {oException.Message}");
			}
		}

		/** 상태를 갱신한다 */
		private void SubOnUpdate(float a_fDeltaTime) {
			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				// Do Something
			}
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
