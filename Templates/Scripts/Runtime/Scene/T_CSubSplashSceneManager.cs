using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 서브 스플래시 씬 관리자
public class CSubSplashSceneManager : CSplashSceneManager {
	#region 변수
	// =====> UI <=====
	private Image m_oSplashImg = null;
	#endregion			// 변수

	#region 프로퍼티
	public override Color ClearColor => KCDefine.SS_COLOR_CLEAR;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		
		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			this.SetupAwake();
		}
	}

	//! 스플래시를 출력한다
	protected override void ShowSplash() {
		m_oSplashImg.SetNativeSize();
		m_oSplashImg.gameObject.SetActive(true);
		
		this.ExLateCallFunc((a_oSender, a_oParams) => {
			this.LoadNextScene();
		}, KCDefine.SS_DELAY_NEXT_SCENE_LOAD);
	}

	//! 씬을 설정한다
	private void SetupAwake() {
		// 이미지를 설정한다
		m_oSplashImg = CFactory.CreateCloneObj<Image>(KCDefine.U_OBJ_N_SPLASH_IMG, KCDefine.U_OBJ_P_IMG, this.SubUIs, KCDefine.SS_POS_SPLASH_IMG);
		m_oSplashImg.sprite = CResManager.Inst.GetRes<Sprite>(KCDefine.U_IMG_P_G_SPLASH);
		m_oSplashImg.gameObject.SetActive(false);
	}
	#endregion			// 함수

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if NEVER_USE_THIS
