using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if !USE_CUSTOM_PROJECT_OPTION
//! 서브 약관 동의 씬 관리자
public class CSubAgreeSceneManager : CAgreeSceneManager {
	#region 함수
	//! 약관 동의 팝업을 출력한다
	protected override void ShowAgreePopup(string a_oServiceString, string a_oPersonalString) {
		this.LoadNextScene();
	}
	#endregion			// 함수
}
#endif			// #if !USE_CUSTOM_PROJECT_OPTION
