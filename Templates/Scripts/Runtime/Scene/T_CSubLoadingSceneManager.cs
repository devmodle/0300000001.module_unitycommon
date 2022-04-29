using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
namespace LoadingScene {
	/** 서브 로딩 씬 관리자 */
	public partial class CSubLoadingSceneManager : CLoadingSceneManager {
		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();
			
			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				this.SetupAwake();
			}
		}
		
		/** 씬을 설정한다 */
		private void SetupAwake() {
			// Do Something
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
