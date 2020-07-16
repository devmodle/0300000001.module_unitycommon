using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//! 로딩 씬 관리자
public abstract class CLoadingSceneManager : CSceneManager {
	#region 프로퍼티
	public virtual float FadeInAnimationDuration => KDefine.U_DEF_DURATION_SCREEN_FADE_IN_ANIMATION;
	public override string SceneName => KDefine.B_SCENE_NAME_LOADING;

#if UNITY_EDITOR
	public override int ScriptOrder => KDefine.U_SCRIPT_ORDER_LOADING_SCENE_MANAGER;
#endif			// #if UNITY_EDITOR
	#endregion			// 프로퍼티

	#region 클래스 프로퍼티
	public static bool IsAnimation { get; set; } = false;
	public static bool IsStartActivityIndicator { get; set; } = false;

	public static string NextSceneName { get; set; } = KDefine.B_EMPTY_STRING;
	public static LoadSceneMode LoadSceneMode { get; set; } = LoadSceneMode.Single;
	public static System.Action<AsyncOperation, bool> Callback { get; set; } = null;
	#endregion			// 클래스 프로퍼티

	#region 추상 함수
	//! 씬을 비동기 로드 중일 경우
	protected abstract void OnLoadSceneAsync(AsyncOperation a_oAsyncOperation, bool a_bIsComplete);
	#endregion			// 추상 함수

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		
		Func.LateCallFunc(this, (a_oComponent, a_oParams) => {
			// 비동기 로드가 아닐 경우
			if(CLoadingSceneManager.Callback == null) {
				CSceneLoader.Instance.LoadScene(CLoadingSceneManager.NextSceneName,
					false, CLoadingSceneManager.IsAnimation, false, this.FadeInAnimationDuration, CLoadingSceneManager.LoadSceneMode);

				// 씬을 추가했을 경우
				if(CLoadingSceneManager.LoadSceneMode == LoadSceneMode.Additive) {
					CSceneLoader.Instance.UnloadSceneAsync(KDefine.B_SCENE_NAME_LOADING, null);
				}
			} else {
				CSceneLoader.Instance.LoadSceneAsync(CLoadingSceneManager.NextSceneName, (a_oAsyncOperation, a_bIsComplete) => {
					this.OnLoadSceneAsync(a_oAsyncOperation, a_bIsComplete);
					CLoadingSceneManager.Callback?.Invoke(a_oAsyncOperation, a_bIsComplete);

					// 씬을 추가했을 경우
					if(a_bIsComplete && CLoadingSceneManager.LoadSceneMode == LoadSceneMode.Additive) {
						CSceneLoader.Instance.UnloadSceneAsync(KDefine.B_SCENE_NAME_LOADING, null);
					}
				}, 0.0f, false, CLoadingSceneManager.IsAnimation, false, this.FadeInAnimationDuration, CLoadingSceneManager.LoadSceneMode);
			}
		});
	}

	//! 초기화
	public override void Start() {
		base.Start();

		// 루트 씬 일 경우
		if(this.IsRootScene && CLoadingSceneManager.IsStartActivityIndicator) {
			CActivityIndicatorManager.Instance.StartActivityIndicator(true);
		}
	}
	#endregion			// 함수
}
