using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 서브 지연 설정 씬 관리자
public class CSubLateSetupSceneManager : CLateSetupSceneManager {
	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		
		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			this.IsAutoInitManager = true;
			CLateSetupSceneManager.IsAutoLoadAds = true;
		}
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
