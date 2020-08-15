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
		
		if(CSceneManager.IsInit) {
			
		}
	}

	//! 스플래시를 출력한다
	protected override void ShowSplash() {
		this.LoadNextScene();
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
