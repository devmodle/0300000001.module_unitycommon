using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if SCENE_TEMPLATES_MODULE_ENABLE
namespace SplashScene {
	/** 서브 스플래시 씬 관리자 */
	public partial class CSubSplashSceneManager : CSplashSceneManager {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			BG_IMG,
			SPLASH_IMG,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		/** =====> UI <===== */
		private Dictionary<EKey, Image> m_oImgDict = new Dictionary<EKey, Image>() {
			[EKey.BG_IMG] = null,
			[EKey.SPLASH_IMG] = null
		};
		#endregion			// 변수

		#region 함수
		/** 씬을 설정한다 */
		protected override void Setup() {
			base.Setup();
			
			// 이미지를 설정한다 {
			m_oImgDict[EKey.BG_IMG] = CFactory.CreateCloneObj<Image>(KCDefine.U_OBJ_N_BG_IMG, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_IMG), this.UIs);
			m_oImgDict[EKey.BG_IMG].color = KCDefine.SS_COLOR_BG_IMG;
			m_oImgDict[EKey.BG_IMG].rectTransform.sizeDelta = CSceneManager.CanvasSize;
			m_oImgDict[EKey.BG_IMG].gameObject.ExAddComponent<CSizeCorrector>().SetSizeRate(Vector3.one);

			m_oImgDict[EKey.SPLASH_IMG] = CFactory.CreateCloneObj<Image>(KCDefine.U_OBJ_N_SPLASH_IMG, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_IMG), this.UIs, KCDefine.SS_POS_SPLASH_IMG);
			m_oImgDict[EKey.SPLASH_IMG].sprite = CResManager.Inst.GetRes<Sprite>(KCDefine.U_IMG_P_SPLASH);
			m_oImgDict[EKey.SPLASH_IMG].gameObject.SetActive(false);
			// 이미지를 설정한다 }
		}

		/** 스플래시를 출력한다 */
		protected override void ShowSplash() {
			m_oImgDict[EKey.SPLASH_IMG].SetNativeSize();
			m_oImgDict[EKey.SPLASH_IMG].gameObject.SetActive(true);

			this.ExLateCallFunc((a_oSender) => this.LoadNextScene(), KCDefine.SS_DELAY_NEXT_SCENE_LOAD);
		}
		#endregion			// 함수
	}
}
#endif			// #if SCENE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
