using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 서브 약관 동의 씬 관리자
public class CSubAgreeSceneManager : CAgreeSceneManager {
	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			Func.SetupLocalizeStrings();
		}
	}

	//! 약관 동의 팝업을 출력한다
	protected override void ShowAgreePopup(string a_oServiceString, string a_oPersonalString) {
		this.LoadNextScene();
	}

	//! 유럽 연합 약관 동의 팝업을 출력한다
	protected override void ShowEuropeanUnionAgreePopup(string a_oServiceURL, string a_oPersonalURL) {
		this.LoadNextScene();
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
