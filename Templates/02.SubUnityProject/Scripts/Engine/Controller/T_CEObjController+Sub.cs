#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 아이템 제어자 */
	public partial class CEObjController : CEController {
		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();
			this.Vec3Dict.ExReplaceVal(EKey.MOVE_POS, KCDefine.B_POS_INVALID);

			#region 추가
			this.SubAwakeSetup();
			#endregion			// 추가
		}

		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			base.Init(a_stParams.m_stBaseParams);
			this.Params = a_stParams;

			#region 추가
			this.SubInit();
			#endregion			// 추가
		}
		#endregion			// 함수
	}

	/** 서브 객체 제어자 */
	public partial class CEObjController : CEController {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			UPDATE_SKIP_TIME,
			[HideInInspector] MAX_VAL
		}

		#region 변수

		#endregion			// 변수

		#region 프로퍼티
		/** =====> 기타 <===== */
		private Dictionary<ESubKey, float> RealDict { get; } = new Dictionary<ESubKey, float>();
		#endregion			// 프로퍼티

		#region 함수
		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(this.IsActive && CSceneManager.IsAppRunning) {
				float fUpdateSkipTime = this.RealDict.GetValueOrDefault(ESubKey.UPDATE_SKIP_TIME);
				this.RealDict.ExReplaceVal(ESubKey.UPDATE_SKIP_TIME, fUpdateSkipTime + a_fDeltaTime);

				// 일정 시간이 지났을 경우
				if(this.RealDict.GetValueOrDefault(ESubKey.UPDATE_SKIP_TIME).ExIsGreateEquals(KCDefine.U_DELAY_DEF)) {
					var oAbilityKindsInfoList = CCollectionManager.Inst.SpawnList<(EAbilityKinds, EAbilityKinds)>();

					try {
						oAbilityKindsInfoList.ExAddVal((EAbilityKinds.STAT_HP_01, EAbilityKinds.STAT_HP_RECOVERY_01));
						oAbilityKindsInfoList.ExAddVal((EAbilityKinds.STAT_MP_01, EAbilityKinds.STAT_MP_RECOVERY_01));
						oAbilityKindsInfoList.ExAddVal((EAbilityKinds.STAT_SP_01, EAbilityKinds.STAT_SP_RECOVERY_01));

						for(int i = 0; i < oAbilityKindsInfoList.Count; ++i) {
							decimal dmVal = this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDict01.ExGetAbilityVal(oAbilityKindsInfoList[i].Item1);
							decimal dmMaxVal = this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDict02.ExGetAbilityVal(oAbilityKindsInfoList[i].Item1);
							decimal dmRecoveryVal = this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDict01.ExGetAbilityVal(oAbilityKindsInfoList[i].Item2);

							this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDict01.ExReplaceVal(oAbilityKindsInfoList[i].Item1, System.Math.Clamp(dmVal + (dmRecoveryVal * (decimal)this.RealDict.GetValueOrDefault(ESubKey.UPDATE_SKIP_TIME)), KCDefine.B_VAL_0_INT, dmMaxVal));
						}
					} finally {
						CCollectionManager.Inst.DespawnList(oAbilityKindsInfoList);
						this.RealDict.ExReplaceVal(ESubKey.UPDATE_SKIP_TIME, KCDefine.B_VAL_0_REAL);
					}
				}
			}
		}

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
					// Do Something
				} else {
					decimal dmDamage = oAbilityValDict.ExGetAbilityVal(EAbilityKinds.STAT_ATK_01) - a_oTargetObj.AbilityValDictWrapper.m_oDict01.ExGetAbilityVal(EAbilityKinds.STAT_DEF_01);
					decimal dmPDamage = oAbilityValDict.ExGetAbilityVal(EAbilityKinds.STAT_P_ATK_01) - a_oTargetObj.AbilityValDictWrapper.m_oDict01.ExGetAbilityVal(EAbilityKinds.STAT_P_DEF_01);
					decimal dmMDamage = oAbilityValDict.ExGetAbilityVal(EAbilityKinds.STAT_M_ATK_01) - a_oTargetObj.AbilityValDictWrapper.m_oDict01.ExGetAbilityVal(EAbilityKinds.STAT_M_DEF_01);

					decimal dmTotalDamage = dmDamage + dmPDamage + dmMDamage;
					a_oTargetObj.AbilityValDictWrapper.m_oDict01.ExIncrAbilityVal(EAbilityKinds.STAT_HP_01, System.Math.Clamp(fPercent.ExIsLessEquals(fCriticalRate) ? dmTotalDamage * KCDefine.B_VAL_2_INT : dmTotalDamage, KCDefine.B_VAL_0_INT, decimal.MaxValue));

					// 크리티컬 공격 일 경우
					if(fPercent.ExIsLessEquals(fCriticalRate)) {
						// Do Something
					} else {
						// Do Something
					}
				}
			} finally {
				CCollectionManager.Inst.DespawnDict(oAbilityValDict);
			}
		}

		/** 이동을 처리한다 */
		public virtual void Move(Vector3 a_stVal, EVecType a_eVecType = EVecType.DIRECTION) {
			this.SetState(EState.MOVE);
			this.SetMovePos((a_eVecType == EVecType.POS) ? a_stVal : KCDefine.B_POS_INVALID);
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
			if(this.SkillInfoDict.GetValueOrDefault(EKey.APPLY_SKILL_INFO).m_eSkillKinds == ESkillKinds.NONE || this.SkillTargetInfoDict.GetValueOrDefault(EKey.APPLY_SKILL_TARGET_INFO) == null) {
				return true;
			}

			var stApplySkillTime = this.ApplySkillTimeDict.GetValueOrDefault(a_stSkillInfo.m_eSkillKinds, System.DateTime.Today.AddDays(-KCDefine.B_VAL_1_INT));
			return System.DateTime.Now.ExGetDeltaTime(stApplySkillTime).ExIsGreateEquals(a_stSkillInfo.m_fDelay);
		}

		/** 이동 상태를 처리한다 */
		protected override void HandleMoveState(float a_fDeltaTime) {
			base.HandleMoveState(a_fDeltaTime);

			var stNextPos = this.GetOwner<CEObj>().transform.localPosition + ((this.Vec3Dict.GetValueOrDefault(EKey.MOVE_DIRECTION) * (float)this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDict01.GetValueOrDefault(EAbilityKinds.STAT_MOVE_SPEED_01)) * a_fDeltaTime);
			stNextPos.x = Mathf.Clamp(stNextPos.x, base.Params.m_stBaseParams.m_oEngine.EpisodeSize.x / -KCDefine.B_VAL_2_REAL, base.Params.m_stBaseParams.m_oEngine.EpisodeSize.x / KCDefine.B_VAL_2_REAL);
			stNextPos.y = Mathf.Clamp(stNextPos.y, (base.Params.m_stBaseParams.m_oEngine.EpisodeSize.y / -KCDefine.B_VAL_2_REAL) + KDefine.E_OFFSET_BOTTOM, base.Params.m_stBaseParams.m_oEngine.EpisodeSize.y / KCDefine.B_VAL_2_REAL);

			this.GetOwner<CEObj>().transform.localPosition = new Vector3(stNextPos.x, stNextPos.y, stNextPos.y / base.Params.m_stBaseParams.m_oEngine.EpisodeSize.y);
		}

		/** 스킬 상태를 처리한다 */
		protected override void HandleSkillState(float a_fDeltaTime) {
			base.HandleSkillState(a_fDeltaTime);
		}

		/** 스킬을 적용시킨다 */
		protected virtual void DoApplySkill(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo) {
			this.SkillInfoDict.ExReplaceVal(EKey.APPLY_SKILL_INFO, a_stSkillInfo);
			this.SkillTargetInfoDict.ExReplaceVal(EKey.APPLY_SKILL_TARGET_INFO, a_oSkillTargetInfo);
		}
		
		/** 제어자를 설정한다 */
		private void SubAwakeSetup() {
			// Do Something
		}

		/** 초기화한다 */
		private void SubInit() {
			this.SetState(EState.APPEAR);
			this.SetMovePos(KCDefine.B_POS_INVALID);
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
