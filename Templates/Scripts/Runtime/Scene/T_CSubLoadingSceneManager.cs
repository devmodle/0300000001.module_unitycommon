using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 서브 로딩 씬 관리자
public class CSubLoadingSceneManager : CLoadingSceneManager {
	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		if(CSceneManager.IsInit) {
			
		}
	}

	//! 씬을 비동기 로드 중일 경우
	protected override void OnLoadSceneAsync(AsyncOperation a_oAsyncOperation, bool a_bIsComplete) {
		// Do Nothing
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
