using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 인트로 씬 관리자
public class CIntroSceneManager : CSceneManager {
	#region 프로퍼티
	public override string SceneName => KCDefine.B_SCENE_N_INTRO;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public sealed override void Start() {
		base.Start();

		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			StartCoroutine(this.OnStart());
		}
	}

	//! 씬을 설정한다
	protected virtual void Setup() {
		// Do Nothing
	}

	//! 초기화
	private IEnumerator OnStart() {
		yield return CFactory.CreateWaitForSecs(KCDefine.U_DELAY_INIT);
		CSceneLoader.Inst.UnloadSceneAsync(KCDefine.B_SCENE_N_PERMISSION, null);

		this.Setup();
	}
	#endregion			// 함수
}
