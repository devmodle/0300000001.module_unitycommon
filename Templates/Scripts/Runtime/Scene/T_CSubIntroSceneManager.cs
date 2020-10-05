using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 서브 인트로 씬 관리자
public class CSubIntroSceneManager : CIntroSceneManager {
	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		
		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			this.ExLateCallFunc(KCDefine.U_DELAY_NEXT_SCENE_LOAD, (a_oSender, a_oParams) => {
				// Do Nothing
			});
		}
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
