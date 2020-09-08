using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 서브 스플래시 씬 관리자
public class CSubSplashSceneManager : CSplashSceneManager {
	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		
		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			// Do Nothing
		}
	}

	//! 스플래시를 출력한다
	protected override void ShowSplash() {
		this.LoadNextScene();
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
