using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if SCENE_TEMPLATES_MODULE_ENABLE
/** 서브 스플래시 씬 관리자 */
public class CSubSplashSceneManager : CSplashSceneManager {
	#region 변수
	/** =====> UI <===== */
	private Image m_oBGImg = null;
	private Image m_oSplashImg = null;
	#endregion			// 변수

	#region 추가 변수

	#endregion			// 추가 변수

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		
		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			this.SetupAwake();
		}
	}

	/** 스플래시를 출력한다 */
	protected override void ShowSplash() {
		m_oSplashImg.SetNativeSize();
		m_oSplashImg.gameObject.SetActive(true);

		this.ExLateCallFunc((a_oSender) => this.LoadNextScene(), KCDefine.SS_DELAY_NEXT_SCENE_LOAD);
	}

	/** 씬을 설정한다 */
	private void SetupAwake() {
		// 이미지를 설정한다 {
		m_oBGImg = CFactory.CreateCloneObj<Image>(KCDefine.U_OBJ_N_BG_IMG, KCDefine.U_OBJ_P_IMG, this.UIs);
		m_oBGImg.color = KCDefine.SS_COLOR_BG_IMG;
		m_oBGImg.rectTransform.sizeDelta = Vector2.zero;
		m_oBGImg.rectTransform.anchorMin = KCDefine.B_ANCHOR_DOWN_LEFT;
		m_oBGImg.rectTransform.anchorMax = KCDefine.B_ANCHOR_UP_RIGHT;

		m_oSplashImg = CFactory.CreateCloneObj<Image>(KCDefine.U_OBJ_N_SPLASH_IMG, KCDefine.U_OBJ_P_IMG, this.UIs, KCDefine.SS_POS_SPLASH_IMG);
		m_oSplashImg.sprite = CResManager.Inst.GetRes<Sprite>(KCDefine.U_IMG_P_G_SPLASH);
		m_oSplashImg.gameObject.SetActive(false);
		// 이미지를 설정한다 }
	}
	#endregion			// 함수

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if SCENE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
