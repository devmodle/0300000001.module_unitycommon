using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 서브 스플래시 씬 관리자
public class CSubSplashSceneManager : CSplashSceneManager {
	#region UI 변수
	private Image m_oSplashImg = null;
	#endregion			// UI 변수

	#region 프로퍼티
	public override Color ClearColor => KCDefine.SS_COLOR_SPLASH_SM_BG;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		
		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			m_oSplashImg = CFactory.CreateCloneObj<Image>(KCDefine.SS_OBJ_N_SPLASH_SM_LOGO_IMG, KCDefine.U_OBJ_P_IMG, this.SubUIs, KCDefine.SS_POS_SPLASH_SM_LOGO_IMG);
			m_oSplashImg.sprite = CResManager.Inst.GetRes<Sprite>(KCDefine.U_IMG_P_G_SPLASH);
			m_oSplashImg.gameObject.SetActive(false);
		}
	}

	//! 스플래시를 출력한다
	protected override void ShowSplash() {
		m_oSplashImg.SetNativeSize();
		m_oSplashImg.gameObject.SetActive(true);

		this.ExLateCallFunc(KCDefine.SS_DELAY_SPLASH_SM_NEXT_SCENE_LOAD, (a_oSender, a_oParams) => this.LoadNextScene());
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
