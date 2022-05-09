using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IntroScene {
	/** 인트로 씬 관리자 */
	public partial class CIntroSceneManager : CSceneManager {
		#region 프로퍼티
		public override string SceneName => KCDefine.B_SCENE_N_INTRO;
		#endregion			// 프로퍼티

		#region 함수
		/** 초기화 */
		public sealed override void Start() {
			base.Start();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				StartCoroutine(this.OnStart());
			}
		}

		/** 씬을 설정한다 */
		protected virtual void Setup() {
			// Do Something
		}

		/** 다음 씬을 로드한다 */
		protected void LoadNextScene() {
			// 초기화 씬 일 경우
			if(CSceneLoader.Inst.AwakeActiveSceneName.Contains(KCDefine.B_TOKEN_TITLE) || KCDefine.B_INIT_SCENE_NAME_LIST.Contains(CSceneLoader.Inst.AwakeActiveSceneName)) {
#if STUDY_MODULE_ENABLE
				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MENU);
#else
				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_TITLE);
#endif			// #if STUDY_MODULE_ENABLE
			} else {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
				CCommonAppInfoStorage.Inst.IsFirstStart = false;
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE

				CSceneLoader.Inst.LoadScene(CSceneLoader.Inst.AwakeActiveSceneName);
			}
		}

		/** 초기화 */
		private IEnumerator OnStart() {
			yield return CFactory.CreateWaitForSecs(KCDefine.U_DELAY_INIT);
			this.Setup();
		}
		#endregion			// 함수
	}
}
