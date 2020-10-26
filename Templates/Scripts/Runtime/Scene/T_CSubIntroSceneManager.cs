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

		this.ExLateCallFunc(KCDefine.U_DELAY_NEXT_SCENE_LOAD, (a_oSender, a_oParams) => {
#if STUDY_MODULE_ENABLE
			CSceneLoader.Instance.LoadScene(KCDefine.B_SCENE_NAME_MENU);
#endif			// #if STUDY_MODULE_ENABLE
		});
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
