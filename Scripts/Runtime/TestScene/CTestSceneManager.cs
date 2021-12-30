using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** 테스트 씬 관리자 */
public abstract class CTestSceneManager : CSceneManager {
	#region 프로퍼티
	public override string SceneName => KCDefine.B_SCENE_N_TEST;
	#endregion			// 프로퍼티

	#region 함수
	/** 내비게이션 스택 이벤트를 수신했을 경우 */
	public override void OnReceiveNavStackEvent(ENavStackEvent a_eEvent) {
		// 백 키 눌림 이벤트 일 경우
		if(a_eEvent == ENavStackEvent.BACK_KEY_DOWN) {
#if STUDY_MODULE_ENABLE
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MENU);
#else
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_TITLE);
#endif			// #if STUDY_MODULE_ENABLE
		}
	}
	#endregion			// 함수
}
