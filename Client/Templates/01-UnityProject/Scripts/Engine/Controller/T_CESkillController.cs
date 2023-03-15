#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 스킬 제어자 */
	public partial class CESkillController : CEController {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			APPLY_TIMES,
			UPDATE_SKIP_TIME,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public new struct STParams {
			public CEController.STParams m_stBaseParams;
		}

		#region 변수
		private Dictionary<EKey, int> m_oIntDict = new Dictionary<EKey, int>() {
			[EKey.APPLY_TIMES] = KCDefine.B_VAL_0_INT
		};

		private Dictionary<EKey, float> m_oRealDict = new Dictionary<EKey, float>() {
			[EKey.UPDATE_SKIP_TIME] = KCDefine.B_VAL_0_REAL
		};
		#endregion // 변수

		#region 프로퍼티
		public new STParams Params { get; private set; }
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

		/** 스킬 정보를 리셋한다 */
		public virtual void ResetSkillInfo(STSkillInfo a_stSkillInfo) {
			// 리셋 가능 할 경우
			if(a_stSkillInfo.m_eSkillKinds != this.GetOwner<CESkill>().Params.m_stSkillInfo.m_eSkillKinds) {
				var stParams = this.GetOwner<CESkill>().Params;
				stParams.m_stSkillInfo = a_stSkillInfo;

				this.GetOwner<CESkill>().Init(stParams);
			}

			this.SubResetSkillInfo(a_stSkillInfo);
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
				CFunc.ShowLogWarning($"CESkillController.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning && this.SubState != ESubState.NONE) {
				switch(this.SubState) {
					case ESubState.APPLY: this.HandleApplySubState(a_fDeltaTime); break;
					case ESubState.COMPLETE: this.HandleCompleteSubState(a_fDeltaTime); break;
				}

				this.SubOnUpdate(a_fDeltaTime);
			}
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
