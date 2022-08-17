#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 플레이어 객체 제어자 */
	public partial class CEPlayerObjController : CEObjController {
		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

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

	/** 서브 플레이어 객체 제어자 */
	public partial class CEPlayerObjController : CEObjController {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

		#region 변수

		#endregion			// 변수

		#region 프로퍼티

		#endregion			// 프로퍼티

		#region 함수
		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				// Do Something
			}
		}

		/** 이동을 처리한다 */
		public override void Move(Vector3 a_stDirection, EVecType a_eVecType = EVecType.DIRECTION) {
			base.Move(a_stDirection);

			// 방향 모드 일 경우
			if(a_eVecType == EVecType.DIRECTION) {
				this.SetState((this.State == EState.MOVE && a_stDirection.ExIsEquals(Vector3.zero)) ? EState.IDLE : this.State);
			}
		}

		/** 스킬을 적용한다 */
		public override void ApplySkill(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo) {
			base.ApplySkill(a_stSkillInfo, a_oSkillTargetInfo);
			CSceneManager.GetSceneManager<GameScene.CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME).SetEnableUpdateUIsState(true);
		}

		/** 대기 상태를 처리한다 */
		protected override void HandleIdleState(float a_fDeltaTime) {
			base.HandleIdleState(a_fDeltaTime);
		}

		/** 이동 상태를 처리한다 */
		protected override void HandleMoveState(float a_fDeltaTime) {
			base.HandleMoveState(a_fDeltaTime);
		}

		/** 스킬 상태를 처리한다 */
		protected override void HandleSkillState(float a_fDeltaTime) {
			base.HandleSkillState(a_fDeltaTime);
		}

		/** 스킬을 적용시킨다 */
		protected override void DoApplySkill(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo) {
			base.DoApplySkill(a_stSkillInfo, a_oSkillTargetInfo);
			var oTargetList = CCollectionManager.Inst.SpawnList<CEComponent>();

			try {
				this.SetupApplySkillTargets(a_stSkillInfo, a_oSkillTargetInfo, oTargetList);

				var oSkill = base.Params.m_stBaseParams.m_oEngine.CreateSkill(a_stSkillInfo, a_oSkillTargetInfo, this.GetOwner<CEObj>());
				oSkill.transform.localPosition = this.GetOwner<CEObj>().transform.localPosition;
				oTargetList.ExCopyTo(oSkill.GetController<CESkillController>().TargetList, (a_oTarget) => a_oTarget);

				oSkill.GetController<CESkillController>().Apply();
				base.Params.m_stBaseParams.m_oEngine.SkillList.ExAddVal(oSkill);
			} finally {
				CCollectionManager.Inst.DespawnList(oTargetList);
			}
		}

		/** 제어자를 설정한다 */
		private void SubAwakeSetup() {
			// Do Something
		}

		/** 초기화한다 */
		private void SubInit() {
			this.SetState(EState.IDLE, true);
		}

		/** 적용 스킬 타겟을 설정한다 */
		private void SetupApplySkillTargets(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo, List<CEComponent> a_oOutTargetList) {
			// 단일 타겟 스킬 일 경우
			if(a_stSkillInfo.m_eSkillApplyKinds == ESkillApplyKinds.TARGET_SINGLE) {
				a_oOutTargetList.ExAddVal(base.Params.m_stBaseParams.m_oEngine.FindNearEnemyObj(this.GetOwner<CEObj>()));	
			}
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
