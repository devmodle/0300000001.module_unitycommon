using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//! 씬 로더
public class CSceneLoader : CSingleton<CSceneLoader> {
	#region 함수
	//! 씬을 로드한다
	public void LoadScene(string a_oName,
		bool a_bIsStartActivityIndicator = false, 
		bool a_bIsAni = true, 
		bool a_bIsUseLoadingScene = false, 
		float a_fDuration = KDefine.U_DEF_DURATION_SCREEN_FADE_IN_ANI, LoadSceneMode a_eLoadSceneMode = LoadSceneMode.Single) {
		if(a_bIsStartActivityIndicator) {
			CActivityIndicatorManager.Instance.StartActivityIndicator(true);
		}

		if(!a_bIsAni) {
			CLoadingSceneManager.IsAni = false;
			this.DoLoadScene(a_oName, a_bIsStartActivityIndicator, a_bIsUseLoadingScene, a_eLoadSceneMode);
		} else {
			Func.Assert(CSceneManager.RootSceneManager != null);

			CSceneManager.RootSceneManager.StartScreenFadeInAni((a_oTouchResponder) => {
				CLoadingSceneManager.IsAni = true;
				this.DoLoadScene(a_oName, a_bIsStartActivityIndicator, a_bIsUseLoadingScene, a_eLoadSceneMode);
			}, a_fDuration);
		}
	}

	//! 씬을 로드한다
	public void LoadSceneAsync(string a_oName,
		System.Action<AsyncOperation, bool> a_oCallback, 
		float a_fDelay = 0.0f, 
		bool a_bIsStartActivityIndicator = false, 
		bool a_bIsAni = true, 
		bool a_bIsUseLoadingScene = true, 
		float a_fDuration = KDefine.U_DEF_DURATION_SCREEN_FADE_IN_ANI, LoadSceneMode a_eLoadSceneMode = LoadSceneMode.Single) {
		if(a_bIsStartActivityIndicator) {
			CActivityIndicatorManager.Instance.StartActivityIndicator(true);
		}

		StartCoroutine(this.DoLoadSceneAsync(a_oName,
			a_fDelay, a_bIsStartActivityIndicator, a_bIsAni, a_bIsUseLoadingScene, a_fDuration, a_eLoadSceneMode, a_oCallback));
	}

	//! 씬을 제거한다
	public void UnloadSceneAsync(string a_oName,
		System.Action<AsyncOperation, bool> a_oCallback, float a_fDelay = 0.0f, bool a_bIsStartActivityIndicator = false) {
		if(a_bIsStartActivityIndicator) {
			CActivityIndicatorManager.Instance.StartActivityIndicator(true);
		}

		StartCoroutine(this.DoUnloadSceneAsync(a_oName, a_fDelay, a_oCallback));
	}

	//! 로딩 씬을 설정한다
	private void SetupLoadingScene(string a_oName,
		bool a_bIsStartActivityIndicator, LoadSceneMode a_eLoadSceneMode, System.Action<AsyncOperation, bool> a_oCallback) {
		CLoadingSceneManager.IsStartActivityIndicator = a_bIsStartActivityIndicator;
		CLoadingSceneManager.NextSceneName = a_oName;
		CLoadingSceneManager.LoadSceneMode = a_eLoadSceneMode;
		CLoadingSceneManager.Callback = a_oCallback;
	}

	//! 씬을 로드한다
	private void DoLoadScene(string a_oName,
		bool a_bIsStartActivityIndicator, bool a_bIsUseLoadingScene, LoadSceneMode a_eLoadSceneMode) {
		if(!a_bIsUseLoadingScene) {
			SceneManager.LoadScene(a_oName, a_eLoadSceneMode);
		} else {
			this.SetupLoadingScene(a_oName, a_bIsStartActivityIndicator, a_eLoadSceneMode, null);
			SceneManager.LoadScene(KDefine.B_SCENE_NAME_LOADING, a_eLoadSceneMode);
		}
	}

	//! 씬을 로드한다
	private IEnumerator DoLoadSceneAsync(string a_oName,
		float a_fDelay, 
		bool a_bIsStartActivityIndicator, 
		bool a_bIsAni, 
		bool a_bIsUseLoadingScene, 
		float a_fDuration, 
		LoadSceneMode a_eLoadSceneMode, System.Action<AsyncOperation, bool> a_oCallback) {
		yield return Func.CreateWaitForSeconds(a_fDelay);

		if(!a_bIsUseLoadingScene) {
			bool bIsActiveScene = false;

			var oAsyncOperation = SceneManager.LoadSceneAsync(a_oName, a_eLoadSceneMode);
			oAsyncOperation.allowSceneActivation = false;

			yield return Func.WaitAsyncOperation(oAsyncOperation, (a_oAsyncOperation, a_bIsComplete) => {
				a_oCallback?.Invoke(a_oAsyncOperation, a_bIsComplete);

				if(!bIsActiveScene && a_oAsyncOperation.progress.ExIsGreateEquals(KDefine.U_MAX_PERCENT_ASYNC_SCENE_LOAD)) {
					bIsActiveScene = true;

					if(!a_bIsAni) {
						a_oAsyncOperation.allowSceneActivation = true;
					} else {
						Func.Assert(CSceneManager.RootSceneManager != null);

						CSceneManager.RootSceneManager.StartScreenFadeInAni((a_oTouchResponder) => {
							a_oAsyncOperation.allowSceneActivation = true;
						}, a_fDuration);
					}
				}
			});
		} else {
			CLoadingSceneManager.IsAni = a_bIsAni;
			this.SetupLoadingScene(a_oName, a_bIsStartActivityIndicator, a_eLoadSceneMode, a_oCallback);

			if(!a_bIsAni) {
				SceneManager.LoadScene(KDefine.B_SCENE_NAME_LOADING, a_eLoadSceneMode);
			} else {
				Func.Assert(CSceneManager.RootSceneManager != null);

				CSceneManager.RootSceneManager.StartScreenFadeInAni((a_oTouchResponder) => {
					SceneManager.LoadScene(KDefine.B_SCENE_NAME_LOADING, a_eLoadSceneMode);
				}, a_fDuration);
			}
		}
	}

	//! 씬을 제거한다
	private IEnumerator DoUnloadSceneAsync(string a_oName,
		float a_fDelay, System.Action<AsyncOperation, bool> a_oCallback) {
		yield return Func.CreateWaitForSeconds(a_fDelay);
		var oAsyncOperation = SceneManager.UnloadSceneAsync(a_oName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

		yield return Func.WaitAsyncOperation(oAsyncOperation, a_oCallback);
	}
	#endregion			// 함수
}
