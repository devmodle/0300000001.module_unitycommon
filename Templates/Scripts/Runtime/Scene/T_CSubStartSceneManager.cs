using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 서브 시작 씬 관리자
public class CSubStartSceneManager : CStartSceneManager {
	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		
		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			
		}
	}

	//! 약관 동의 씬 관리자 이벤트를 수신했을 경우
	public override void OnReceiveAgreeSceneManagerEvent(EAgreeSceneManagerEventType a_eEventType) {
		// Do Nothing
	}
	#endregion			// 함수	
}
#endif			// #if NEVER_USE_THIS
