#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 스킬 제어자 */
	public partial class CESkillController : CEController {
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

	/** 서브 스킬 제어자 */
	public partial class CESkillController : CEController {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			APPLY_TIMES,
			UPDATE_SKIP_TIME,
			[HideInInspector] MAX_VAL
		}

		/** 서브 상태 */
		public enum ESubState {
			NONE = -1,
			APPLY,
			[HideInInspector] MAX_VAL
		}

		#region 변수

		#endregion			// 변수

		#region 프로퍼티
		public ESubState SubState { get; private set; } = ESubState.NONE;

		/** =====> 기타 <===== */
		private Dictionary<ESubKey, int> IntDict { get; } = new Dictionary<ESubKey, int>();
		private Dictionary<ESubKey, float> RealDict { get; }= new Dictionary<ESubKey, float>();
		#endregion			// 프로퍼티

		#region 함수
		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(this.SubState != ESubState.NONE && CSceneManager.IsAppRunning) {
				switch(this.SubState) {
					case ESubState.APPLY: this.HandleApplySubState(a_fDeltaTime); break;
				}
			}
		}

		/** 스킬을 적용한다 */
		public void Apply() {
			this.SetState(EState.IDLE);
			base.Params.m_oOwner.GetOwner<CEObj>().GetController<CEObjController>().ApplySkillTimeDict.ExReplaceVal(this.GetOwner<CESkill>().Params.m_stSkillInfo.m_eSkillKinds, System.DateTime.Now);
		}

		/** 대기 상태를 처리한다 */
		protected override void HandleIdleState(float a_fDeltaTime) {
			base.HandleIdleState(a_fDeltaTime);
			float fUpdateSkipTime = this.RealDict.GetValueOrDefault(ESubKey.UPDATE_SKIP_TIME);

			this.RealDict.ExReplaceVal(ESubKey.UPDATE_SKIP_TIME, fUpdateSkipTime + a_fDeltaTime);

			// 딜레이 시간이 지났을 경우
			if(this.RealDict.GetValueOrDefault(ESubKey.UPDATE_SKIP_TIME).ExIsGreateEquals(this.GetOwner<CESkill>().Params.m_stSkillInfo.m_stTimeInfo.m_fDelay)) {
				this.SetState(EState.SKILL);
				
				this.SubState = ESubState.APPLY;
				this.RealDict.ExReplaceVal(ESubKey.UPDATE_SKIP_TIME, KCDefine.B_VAL_0_REAL);
			}
		}
		
		/** 제어자를 설정한다 */
		private void SubAwakeSetup() {
			// Do Something
		}

		/** 초기화한다 */
		private void SubInit() {
			this.IntDict.Clear();
			this.RealDict.Clear();

			this.SubState = ESubState.NONE;
		}

		/** 적용 서브 상태를 처리한다 */
		private void HandleApplySubState(float a_fDeltaTime) {
			int nApplyTimes = this.IntDict.GetValueOrDefault(ESubKey.APPLY_TIMES);
			float fUpdateSkipTime = this.RealDict.GetValueOrDefault(ESubKey.UPDATE_SKIP_TIME);

			this.RealDict.ExReplaceVal(ESubKey.UPDATE_SKIP_TIME, fUpdateSkipTime + a_fDeltaTime);

			// 적용 간격이 지났을 경우
			if(this.RealDict.GetValueOrDefault(ESubKey.UPDATE_SKIP_TIME).ExIsGreateEquals(this.GetOwner<CESkill>().Params.m_stSkillInfo.m_stTimeInfo.m_fDeltaTime * (nApplyTimes - KCDefine.B_VAL_1_INT))) {
				switch((ESkillApplyType)((int)this.GetOwner<CESkill>().Params.m_stSkillInfo.m_eSkillApplyKinds).ExKindsToType()) {
					case ESkillApplyType.TARGET: this.ApplyTarget(); break;
				}

				this.IntDict.ExReplaceVal(ESubKey.APPLY_TIMES, nApplyTimes + KCDefine.B_VAL_1_INT);
			}

			// 적용 시간이 지났을 경우
			if(this.RealDict.GetValueOrDefault(ESubKey.UPDATE_SKIP_TIME).ExIsGreateEquals(this.GetOwner<CESkill>().Params.m_stSkillInfo.m_stTimeInfo.m_fDuration)) {
				base.Params.m_oOwner.GetOwner<CEObj>().GetController<CEObjController>().SetState(EState.IDLE);
				base.Params.m_oEngine.RemoveSkill(this.GetOwner<CESkill>());
			}
		}

		/** 타겟 스킬을 적용한다 */
		private void ApplyTarget() {
			for(int i = 0; i < this.TargetList.Count; ++i) {
				base.Params.m_oOwner.GetOwner<CEObj>().GetController<CEObjController>().Attack(this.TargetList[i] as CEObj, this.GetOwner<CESkill>());
			}
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
