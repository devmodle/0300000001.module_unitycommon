#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 객체 제어자 */
	public partial class CEObjController : CEController {
		/** 매개 변수 */
		public new struct STParams {
			public CEController.STParams m_stBase;
		}

		#region 변수
		private float m_fUpdateSkipTime = 0.0f;
		#endregion // 변수

		#region 프로퍼티
		public new STParams Params { get; private set; }
		public Dictionary<ESkillKinds, System.DateTime> ApplySkillTimeDict { get; } = new Dictionary<ESkillKinds, System.DateTime>();

		public bool IsAutoControl { get; private set; } = false;

		public Vector3 MovePos { get; private set; } = Vector3.zero;
		public Vector3 MoveDirection { get; private set; } = Vector3.zero;

		public STFXInfo ApplyFXInfo { get; private set; } = STFXInfo.INVALID;
		public (STSkillInfo, CSkillTargetInfo) ApplySkillInfo { get; private set; } = (STSkillInfo.INVALID, null);
		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();
			this.SubAwake();
		}

		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			base.Init(a_stParams.m_stBase);
			this.Params = a_stParams;

			this.SubInit();
		}

		/** 상태를 리셋한다 */
		public override void Reset() {
			base.Reset();
			this.ResetApplySkillInfo();
		}

		/** 적용 스킬 정보를 리셋한다 */
		public virtual void ResetApplySkillInfo() {
			this.ApplySkillInfo = (STSkillInfo.INVALID, null);
		}

		/** 제거되었을 경우 */
		public override void OnDestroy() {
			base.OnDestroy();

			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsRunningApp) {
					this.SubOnDestroy();
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CEObjController.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fTimeDelta) {
			base.OnUpdate(a_fTimeDelta);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsRunningApp && this.IsActive) {
				m_fUpdateSkipTime += a_fTimeDelta;

				// 일정 시간이 지났을 경우
				if(m_fUpdateSkipTime.ExIsGreatEquals(KCDefine.U_DELAY_DEF)) {
					m_fUpdateSkipTime = KCDefine.B_VAL_0_REAL;
					var oAbilityKindsInfoList = CCollectionPoolManager.Inst.SpawnList<(EAbilityKinds, EAbilityKinds)>();

					try {
						oAbilityKindsInfoList.ExAddVal((EAbilityKinds.STAT_ABILITY_HP_01, EAbilityKinds.STAT_ABILITY_HP_RECOVERY_01));
						oAbilityKindsInfoList.ExAddVal((EAbilityKinds.STAT_ABILITY_MP_01, EAbilityKinds.STAT_ABILITY_MP_RECOVERY_01));
						oAbilityKindsInfoList.ExAddVal((EAbilityKinds.STAT_ABILITY_SP_01, EAbilityKinds.STAT_ABILITY_SP_RECOVERY_01));

						for(int i = 0; i < oAbilityKindsInfoList.Count; ++i) {
							decimal dmVal = this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDictA.ExGetAbilityVal(oAbilityKindsInfoList[i].Item1);
							decimal dmMaxVal = this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDictB.ExGetAbilityVal(oAbilityKindsInfoList[i].Item1);
							decimal dmRecoveryVal = this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDictA.ExGetAbilityVal(oAbilityKindsInfoList[i].Item2);

							this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDictA.ExReplaceVal(oAbilityKindsInfoList[i].Item1, System.Math.Clamp(dmVal + (dmRecoveryVal * (decimal)m_fUpdateSkipTime), KCDefine.B_VAL_0_INT, dmMaxVal));
						}
					} finally {
						CCollectionPoolManager.Inst.DespawnList(oAbilityKindsInfoList);
					}
				}

				this.SubOnUpdate(a_fTimeDelta);
			}
		}

		/** 공격을 처리한다 */
		public virtual void Attack(CEObj a_oTargetObj, CESkill a_oSkill) {
			var oAbilityValDict = CCollectionPoolManager.Inst.SpawnDict<EAbilityKinds, decimal>();

			try {
				float fPercent = Random.Range(KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_1_REAL);
				float fCriticalRate = (float)a_oTargetObj.AbilityValDictWrapper.m_oDictA.ExGetAbilityVal(EAbilityKinds.STAT_ABILITY_CRITICAL_RATE_01);

				this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDictA.ExCopyTo(oAbilityValDict, (_, a_dmAbilityVal) => a_dmAbilityVal);
				global::Func.SetupAbilityVals(a_oSkill.Params.m_stSkillInfo, a_oSkill.Params.m_oSkillTargetInfo, oAbilityValDict);

				// 공격을 회피했을 경우
				if(fPercent.ExIsLessEquals((float)a_oTargetObj.AbilityValDictWrapper.m_oDictA.ExGetAbilityVal(EAbilityKinds.STAT_ABILITY_AVOID_RATE_01))) {
					this.GetOwner<CEObj>().Params.m_stBase.m_oCallbackDict.ExGetVal(CEObj.ECallback.ENGINE_OBJ_EVENT)?.Invoke(a_oTargetObj, EEngineObjEvent.AVOID, string.Empty);
				} else {
					decimal dmDamage = System.Math.Clamp(oAbilityValDict.ExGetAbilityVal(EAbilityKinds.STAT_ABILITY_ATK_01) - a_oTargetObj.AbilityValDictWrapper.m_oDictA.ExGetAbilityVal(EAbilityKinds.STAT_ABILITY_DEF_01), KCDefine.B_VAL_0_INT, decimal.MaxValue);
					decimal dmPDamage = System.Math.Clamp(oAbilityValDict.ExGetAbilityVal(EAbilityKinds.STAT_ABILITY_P_ATK_01) - a_oTargetObj.AbilityValDictWrapper.m_oDictA.ExGetAbilityVal(EAbilityKinds.STAT_ABILITY_P_DEF_01), KCDefine.B_VAL_0_INT, decimal.MaxValue);
					decimal dmMDamage = System.Math.Clamp(oAbilityValDict.ExGetAbilityVal(EAbilityKinds.STAT_ABILITY_M_ATK_01) - a_oTargetObj.AbilityValDictWrapper.m_oDictA.ExGetAbilityVal(EAbilityKinds.STAT_ABILITY_M_DEF_01), KCDefine.B_VAL_0_INT, decimal.MaxValue);

					decimal dmTotalDamage = dmDamage + dmPDamage + dmMDamage;
					dmTotalDamage = fPercent.ExIsLessEquals(fCriticalRate) ? dmTotalDamage * KCDefine.B_VAL_2_INT : dmTotalDamage;

					a_oTargetObj.AbilityValDictWrapper.m_oDictA.ExIncrAbilityVal(EAbilityKinds.STAT_ABILITY_HP_01, -dmTotalDamage);
					this.GetOwner<CEObj>().Params.m_stBase.m_oCallbackDict.ExGetVal(CEObj.ECallback.ENGINE_OBJ_EVENT)?.Invoke(a_oTargetObj, fPercent.ExIsLessEquals(fCriticalRate) ? EEngineObjEvent.CRITICAL_DAMAGE : EEngineObjEvent.DAMAGE, $"{dmTotalDamage:0}");
				}
			} finally {
				CCollectionPoolManager.Inst.DespawnDict(oAbilityValDict);
			}
		}

		/** 이동을 처리한다 */
		public virtual void Move(Vector3 a_stVal, EVecType a_eVecType = EVecType.DIRECTION) {
			this.SetState(EState.MOVE);
			this.SetMovePos((a_eVecType == EVecType.POS) ? a_stVal : Vector3.zero);
			this.SetMoveDirection((a_eVecType == EVecType.DIRECTION) ? a_stVal : Vector3.zero);
		}

		/** 스킬을 적용한다 */
		public virtual void ApplySkill(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo) {
			// 스킬 적용이 가능 할 경우
			if(this.IsEnableSkillState() && this.IsEnableApplySkill(a_stSkillInfo, a_oSkillTargetInfo)) {
				var oTargetList = CCollectionPoolManager.Inst.SpawnList<CEObjComponent>();

				try {
					this.ApplySkillInfo = (a_stSkillInfo, a_oSkillTargetInfo);

					switch(a_stSkillInfo.SkillApplyType) {
						case EApplyType.MULTI: this.SetupMultiSkillTargets(a_stSkillInfo, a_oSkillTargetInfo, oTargetList); break;
						case EApplyType.SINGLE: this.SetupSingleSkillTargets(a_stSkillInfo, a_oSkillTargetInfo, oTargetList); break;
					}

					this.SetState(EState.SKILL);
					this.DoApplySkill(a_stSkillInfo, a_oSkillTargetInfo, oTargetList);
				} finally {
					CCollectionPoolManager.Inst.DespawnList(oTargetList);
				}
			}
		}

		/** 효과를 적용한다 */
		public virtual void ApplyFX(STFXInfo a_stFXInfo) {
			var oTargetList = CCollectionPoolManager.Inst.SpawnList<CEObjComponent>();

			try {
				this.ApplyFXInfo = a_stFXInfo;

				switch(a_stFXInfo.FXApplyType) {
					// Do Something
				}

				this.DoApplyFX(a_stFXInfo, oTargetList);
			} finally {
				CCollectionPoolManager.Inst.DespawnList(oTargetList);
			}
		}

		/** 스킬을 적용한다 */
		private void DoApplySkill(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo, List<CEObjComponent> a_oTargetList) {
			var oSkill = this.CreateSkill(a_stSkillInfo, a_oSkillTargetInfo);
			a_oTargetList.ExCopyTo(oSkill.GetController<CESkillController>().EObjComponentList, (a_oTargetObj) => a_oTargetObj);

			this.Engine.SkillListWrapper.ExAddVal(oSkill);
			oSkill.GetController<CESkillController>().Apply();
		}

		/** 효과를 적용한다 */
		private void DoApplyFX(STFXInfo a_stFXInfo, List<CEObjComponent> a_oTargetList) {
			var oFX = this.CreateFX(a_stFXInfo);
			a_oTargetList.ExCopyTo(oFX.GetController<CEFXController>().EObjComponentList, (a_oTargetObj) => a_oTargetObj);

			this.Engine.FXListWrapper.ExAddVal(oFX);
			oFX.GetController<CEFXController>().Apply();
		}
		#endregion // 함수
	}

	/** 객체 제어자 - 설정 */
	public partial class CEObjController : CEController {
		#region 함수
		/** 다중 스킬 타겟을 설정한다 */
		protected virtual void SetupMultiSkillTargets(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo, List<CEObjComponent> a_oOutTargetList) {
			// Do Something
		}

		/** 단일 스킬 타겟을 설정한다 */
		protected virtual void SetupSingleSkillTargets(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo, List<CEObjComponent> a_oOutTargetList) {
			// Do Something
		}
		#endregion // 함수
	}

	/** 객체 제어자 - 접근 */
	public partial class CEObjController : CEController {
		#region 함수
		/** 자동 제어 여부를 변경한다 */
		public void SetIsAutoControl(bool a_bIsAutoControl) {
			// 수동 제어 모드 일 경우
			if(!a_bIsAutoControl && this.State == EState.MOVE) {
				this.SetState(EState.IDLE);
			}

			this.IsAutoControl = a_bIsAutoControl;
		}

		/** 이동 위치를 변경한다 */
		public void SetMovePos(Vector3 a_stPos) {
			this.MovePos = a_stPos;
		}

		/** 이동 방향을 변경한다 */
		public void SetMoveDirection(Vector3 a_stDirection) {
			this.MoveDirection = a_stDirection.normalized;
		}

		/** 스킬 적용 가능 여부를 검사한다 */
		protected virtual bool IsEnableApplySkill(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo) {
			// 적용 스킬 타겟 정보가 없을 경우
			if(this.ApplySkillInfo.Item1.m_eSkillKinds == ESkillKinds.NONE) {
				return true;
			}

			var oAbilityValDict = CCollectionPoolManager.Inst.SpawnDict<EAbilityKinds, decimal>();

			try {
				global::Func.SetupAbilityVals(a_stSkillInfo, a_oSkillTargetInfo, oAbilityValDict);
				double dblDeltaTime = System.DateTime.Now.ExGetDeltaTime(this.ApplySkillTimeDict.ExGetVal(a_stSkillInfo.m_eSkillKinds, System.DateTime.Today.AddDays(-KCDefine.B_VAL_1_INT)));

				return (decimal)dblDeltaTime >= this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDictA.ExGetAbilityVal((a_stSkillInfo.SkillType == ESkillType.ACTION) ? EAbilityKinds.STAT_ABILITY_ATK_DELAY_01 : EAbilityKinds.STAT_ABILITY_SKILL_DELAY_01, (a_stSkillInfo.SkillType == ESkillType.ACTION) ? this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDictA.GetValueOrDefault(EAbilityKinds.STAT_ABILITY_ATK_DELAY_01) : oAbilityValDict.ExGetVal(EAbilityKinds.STAT_ABILITY_SKILL_DELAY_01));
			} finally {
				CCollectionPoolManager.Inst.DespawnDict(oAbilityValDict);
			}
		}
		#endregion // 함수
	}

	/** 객체 제어자 - 팩토리 */
	public partial class CEObjController : CEController {
		#region 클래스 함수
		/** 매개 변수를 생성한다 */
		public new static STParams MakeParams(CEngine a_oEngine) {
			return new STParams() {
				m_stBase = CEController.MakeParams(a_oEngine)
			};
		}
		#endregion // 클래스 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
