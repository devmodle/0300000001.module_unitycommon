using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 서브 인트로 씬 관리자
public class CSubIntroSceneManager : CIntroSceneManager {
	#region 함수
	//! 씬을 설정한다
	protected override void Setup() {
		base.Setup();
		this.LoadNextScene();
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
