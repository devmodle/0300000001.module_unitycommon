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

		#region 변수
		private Dictionary<EKey, bool> m_oBoolDict = new Dictionary<EKey, bool>() {
			[EKey.IS_AUTO_CONTROL] = false
		};

		private Dictionary<EKey, Vector3> m_oVec3Dict = new Dictionary<EKey, Vector3>() {
			[EKey.MOVE_POS] = Vector3.zero, [EKey.MOVE_DIRECTION] = Vector3.zero
		};

		private Dictionary<EKey, STSkillInfo> m_oSkillInfoDict = new Dictionary<EKey, STSkillInfo>() {
			[EKey.APPLY_SKILL_INFO] = STSkillInfo.INVALID
		};

		private Dictionary<EKey, CSkillTargetInfo> m_oSkillTargetInfoDict = new Dictionary<EKey, CSkillTargetInfo>() {
			[EKey.APPLY_SKILL_TARGET_INFO] = null
		};
		#endregion // 변수

		#region 프로퍼티
		public new STParams Params { get; private set; }
		public Dictionary<ESkillKinds, System.DateTime> ApplySkillTimeDict { get; } = new Dictionary<ESkillKinds, System.DateTime>();

		public bool IsAutoControl => m_oBoolDict[EKey.IS_AUTO_CONTROL];
		public Vector3 MovePos => m_oVec3Dict[EKey.MOVE_POS];
		public Vector3 MoveDirection => m_oVec3Dict[EKey.MOVE_DIRECTION];
		public CSkillTargetInfo ApplySkillTargetInfo => m_oSkillTargetInfoDict[EKey.APPLY_SKILL_TARGET_INFO];
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

			this.SubInit();
		}

		/** 적용 스킬 정보를 리셋한다 */
		public virtual void ResetApplySkillInfo() {
			m_oSkillInfoDict[EKey.APPLY_SKILL_INFO] = STSkillInfo.INVALID;
		}

		/** 제거 되었을 경우 */
		public override void OnDestroy() {
			base.OnDestroy();

			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsAppRunning) {
					this.SubOnDestroy();
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CEObjController.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(this.IsActive && CSceneManager.IsAppRunning) {
				float fUpdateSkipTime = m_oRealDict.GetValueOrDefault(ESubKey.UPDATE_SKIP_TIME);
				m_oRealDict.ExReplaceVal(ESubKey.UPDATE_SKIP_TIME, fUpdateSkipTime + a_fDeltaTime);

				// 일정 시간이 지났을 경우
				if(m_oRealDict.GetValueOrDefault(ESubKey.UPDATE_SKIP_TIME).ExIsGreateEquals(KCDefine.U_DELAY_DEF)) {
					var oAbilityKindsInfoList = CCollectionManager.Inst.SpawnList<(EAbilityKinds, EAbilityKinds)>();

					try {
						oAbilityKindsInfoList.ExAddVal((EAbilityKinds.STAT_HP_01, EAbilityKinds.STAT_HP_RECOVERY_01));
						oAbilityKindsInfoList.ExAddVal((EAbilityKinds.STAT_MP_01, EAbilityKinds.STAT_MP_RECOVERY_01));
						oAbilityKindsInfoList.ExAddVal((EAbilityKinds.STAT_SP_01, EAbilityKinds.STAT_SP_RECOVERY_01));

						for(int i = 0; i < oAbilityKindsInfoList.Count; ++i) {
							decimal dmVal = this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDict01.ExGetAbilityVal(oAbilityKindsInfoList[i].Item1);
							decimal dmMaxVal = this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDict02.ExGetAbilityVal(oAbilityKindsInfoList[i].Item1);
							decimal dmRecoveryVal = this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDict01.ExGetAbilityVal(oAbilityKindsInfoList[i].Item2);

							this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDict01.ExReplaceVal(oAbilityKindsInfoList[i].Item1, System.Math.Clamp(dmVal + (dmRecoveryVal * (decimal)m_oRealDict.GetValueOrDefault(ESubKey.UPDATE_SKIP_TIME)), KCDefine.B_VAL_0_INT, dmMaxVal));
						}
					} finally {
						CCollectionManager.Inst.DespawnList(oAbilityKindsInfoList);
						m_oRealDict.ExReplaceVal(ESubKey.UPDATE_SKIP_TIME, KCDefine.B_VAL_0_REAL);
					}
				}

				this.SubOnUpdate(a_fDeltaTime);
			}
		}

		/** 자동 제어 여부를 변경한다 */
		public void SetEnableAutoControl(bool a_bIsAutoControl) {
			// 수동 제어 모드 일 경우
			if(!a_bIsAutoControl && this.State == EState.MOVE) {
				this.SetState(EState.IDLE);
			}

			m_oBoolDict[EKey.IS_AUTO_CONTROL] = a_bIsAutoControl;
		}

		/** 이동 위치를 변경한다 */
		public void SetMovePos(Vector3 a_stPos) {
			m_oVec3Dict[EKey.MOVE_POS] = a_stPos;
		}

		/** 이동 방향을 변경한다 */
		public void SetMoveDirection(Vector3 a_stDirection) {
			m_oVec3Dict[EKey.MOVE_DIRECTION] = a_stDirection.normalized;
		}
		#endregion // 함수

		#region 클래스 함수
		/** 매개 변수를 생성한다 */
		public new static STParams MakeParams(CEngine a_oEngine) {
			return new STParams() {
				m_stBaseParams = CEController.MakeParams(a_oEngine)
			};
		}
		#endregion // 클래스 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
