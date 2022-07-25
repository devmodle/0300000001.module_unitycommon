using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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
				this.AwakeSetup();
			}
		}

		/** 씬을 설정한다 */
		private void AwakeSetup() {
			// 객체를 설정한다 {
			CFunc.SetupObjs(new List<(EKey, string, GameObject, GameObject)>() {
				(EKey.LOADING_GAUGE, $"{EKey.LOADING_GAUGE}", this.UIs, CResManager.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_LOADING_GAUGE))
			}, m_oUIsDict, false);

			m_oUIsDict[EKey.LOADING_GAUGE].transform.localPosition = CSubLoadingSceneManager.POS_LOADING_GAUGE;
			// 객체를 설정한다 }

			// 텍스트를 설정한다 {
			CFunc.SetupComponents(new List<(EKey, string, GameObject, GameObject)>() {
				(EKey.LOADING_TEXT, $"{EKey.LOADING_TEXT}", this.UIs, CResManager.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_LOADING_TEXT))
			}, m_oTextDict, false);

			m_oTextDict[EKey.LOADING_TEXT].transform.localPosition = CSubLoadingSceneManager.POS_LOADING_TEXT;
			// 텍스트를 설정한다 }

			// 게이지 처리자를 설정한다
			CFunc.SetupComponents(new List<(EKey, GameObject)>() {
				(EKey.LOADING_GAUGE_HANDLER, m_oUIsDict[EKey.LOADING_GAUGE])
			}, m_oGaugeHandlerDict, false);
		}

		/** 비동기 씬 로딩 상태가 갱신 되었을 경우 */
		protected override void OnUpdateAsyncSceneLoadingState(AsyncOperation a_oAsyncOperation, bool a_bIsComplete) {
			m_oGaugeHandlerDict[EKey.LOADING_GAUGE_HANDLER].SetPercent(a_oAsyncOperation.progress);
		}
		#endregion			// 함수
	}

	/** 서브 로딩 씬 관리자 - 서브 */
	public partial class CSubLoadingSceneManager : CLoadingSceneManager {
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

		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
