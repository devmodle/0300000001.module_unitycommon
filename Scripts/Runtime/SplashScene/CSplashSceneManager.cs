using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 스플래시 씬 관리자
public abstract class CSplashSceneManager : CSceneManager {
	#region 프로퍼티
	public override string SceneName => KCDefine.B_SCENE_N_SPLASH;

#if UNITY_EDITOR
	public override int ScriptOrder => KCDefine.U_SCRIPT_O_SPLASH_SCENE_MANAGER;
#endif			// #if UNITY_EDITOR
	#endregion			// 프로퍼티

	#region 추상 함수
	//! 스플래시를 출력한다
	protected abstract void ShowSplash();
	#endregion			// 추상 함수

	#region 함수
	//! 초기화
	public sealed override void Start() {
		base.Start();

		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			StartCoroutine(this.OnStart());
		}
	}

	//! 다음 씬을 로드한다
	protected void LoadNextScene() {
		this.SetupOffsets();
		CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_START, false, false);
	}

	//! 초기화
	private IEnumerator OnStart() {
		yield return CFactory.CreateWaitForSecs(KCDefine.U_DELAY_INIT);
		this.ShowSplash();
	}
	#endregion			// 함수
}
