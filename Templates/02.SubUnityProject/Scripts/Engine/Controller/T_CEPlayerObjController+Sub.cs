#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 플레이어 객체 제어자 */
	public partial class CEPlayerObjController : CEController {
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
	public partial class CEPlayerObjController : CEController {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

		#region 변수

		#endregion			// 변수

		#region 프로퍼티
		public bool IsEnableMove => this.IsActive && (this.ControllerState == EControllerState.IDLE || this.ControllerState == EControllerState.MOVE);
		public bool IsEnableApplySkill => this.IsActive && (this.ControllerState == EControllerState.IDLE || this.ControllerState == EControllerState.MOVE);
		#endregion			// 프로퍼티

		#region 함수
		/** 이동을 처리한다 */
		public override void Move(Vector3 a_stDirection) {
			base.Move(a_stDirection);
			this.SetControllerState(this.IsEnableMove ? EControllerState.MOVE : this.ControllerState);
		}

		/** 스킬을 적용한다 */
		public override void ApplySkill(CSkillTargetInfo a_oSkillTargetInfo) {
			base.ApplySkill(a_oSkillTargetInfo);
			this.SetControllerState(this.IsEnableApplySkill ? EControllerState.SKILL : this.ControllerState);
		}

		/** 대기 제어자 상태를 처리한다 */
		protected override void HandleIdleControllerState(float a_fDeltaTime) {
			base.HandleIdleControllerState(a_fDeltaTime);
		}

		/** 이동 제어자 상태를 처리한다 */
		protected override void HandleMoveControllerState(float a_fDeltaTime) {
			base.HandleMoveControllerState(a_fDeltaTime);
			CEpisodeInfoTable.Inst.TryGetLevelEpisodeInfo(this.Params.m_stBaseParams.m_oEngine.Params.m_oLevelInfo.m_stIDInfo.m_nID01, out STEpisodeInfo stLevelEpisodeInfo, this.Params.m_stBaseParams.m_oEngine.Params.m_oLevelInfo.m_stIDInfo.m_nID02, this.Params.m_stBaseParams.m_oEngine.Params.m_oLevelInfo.m_stIDInfo.m_nID03);

			var stEpisodeSize = (stLevelEpisodeInfo.m_stSize * KCDefine.B_UNIT_SCALE) * CAccess.ResolutionScale;
			stEpisodeSize.x = Mathf.Clamp(stEpisodeSize.x, KCDefine.B_VAL_0_REAL, stEpisodeSize.x - ((KCDefine.B_SCREEN_SIZE.x * KCDefine.B_UNIT_SCALE) * CAccess.ResolutionScale));
			stEpisodeSize.y = Mathf.Clamp(stEpisodeSize.y, KCDefine.B_VAL_0_REAL, stEpisodeSize.y - ((KCDefine.B_SCREEN_SIZE.y * KCDefine.B_UNIT_SCALE) * CAccess.ResolutionScale));
			
			var stMainCameraPos = new Vector3(Mathf.Clamp(this.transform.position.x, stEpisodeSize.x / -KCDefine.B_VAL_2_REAL, stEpisodeSize.x / KCDefine.B_VAL_2_REAL), Mathf.Clamp(this.transform.position.y, stEpisodeSize.y / -KCDefine.B_VAL_2_REAL, stEpisodeSize.y / KCDefine.B_VAL_2_REAL), CSceneManager.ActiveSceneMainCamera.transform.position.z);
			CSceneManager.ActiveSceneMainCamera.transform.position = Vector3.Lerp(CSceneManager.ActiveSceneMainCamera.transform.position, stMainCameraPos, a_fDeltaTime * KCDefine.B_VAL_9_REAL);
		}

		/** 효과를 설정한다 */
		private void SubAwakeSetup() {
			// Do Something
		}

		/** 초기화한다 */
		private void SubInit() {
			// Do Something
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
