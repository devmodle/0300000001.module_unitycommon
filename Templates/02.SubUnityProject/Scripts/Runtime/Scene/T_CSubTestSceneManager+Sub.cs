using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
namespace TestScene {
	/** 서브 테스트 씬 관리자 */
	public partial class CSubTestSceneManager : CTestSceneManager {
		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				this.AwakeSetup();
			}
		}

		/** 초기화 */
		public override void Start() {
			base.Start();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				this.StartSetup();
				this.UpdateUIsState();
			}
		}
		#endregion			// 함수
	}

	/** 서브 테스트 씬 관리자 - 서브 */
	public partial class CSubTestSceneManager : CTestSceneManager {
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
		/** 씬을 설정한다 */
		private void AwakeSetup() {
			// Do Something
		}

		/** 씬을 설정한다 */
		private void StartSetup() {
			// Do Something
		}
		
		/** UI 상태를 갱신한다 */
		private void UpdateUIsState() {
			// Do Something
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
