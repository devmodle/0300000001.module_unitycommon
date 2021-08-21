using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 서브 로딩 씬 관리자
public class CSubLoadingSceneManager : CLoadingSceneManager {
	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		
		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			this.SetupAwake();
		}
	}

	//! 씬을 비동기 로드 중일 경우
	protected override void OnLoadSceneAsync(AsyncOperation a_oAsyncOperation, bool a_bIsComplete) {
		// Do Something
	}

	//! 씬을 설정한다
	private void SetupAwake() {
		// Do Something
	}
	#endregion			// 함수

	#region 추가 변수

	#endregion			// 추가 변수
	
	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if NEVER_USE_THIS
