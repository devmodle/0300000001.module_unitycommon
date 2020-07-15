using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if !USE_CUSTOM_PROJECT_OPTION
//! 서브 스플래시 씬 관리자
public class CSubSplashSceneManager : CSplashSceneManager {
	#region 함수
	//! 스플래시를 출력한다
	protected override void ShowSplash() {
		this.LoadNextScene();
	}
	#endregion			// 함수
}
#endif			// #if !USE_CUSTOM_PROJECT_OPTION
