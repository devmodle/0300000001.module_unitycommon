#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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
		private Dictionary<EKey, Image> m_oImgDict = new Dictionary<EKey, Image>();
		#endregion			// 변수

		#region 상수
		private static readonly Vector3 POS_SPLASH_IMG = new Vector3(0.0f, 25.0f, 0.0f);
		#endregion			// 상수

		#region 함수
		/** 씬을 설정한다 */
		protected override void Setup() {
			base.Setup();
			
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
			// 테이블을 생성한다 {
			CLevelInfoTable.Create();

			CEtcInfoTable.Create();
			CMissionInfoTable.Create();
			CRewardInfoTable.Create();
			CEpisodeInfoTable.Create();
			CTutorialInfoTable.Create();
			CResInfoTable.Create();

			CItemInfoTable.Create();
			CSkillInfoTable.Create();
			CObjInfoTable.Create();
			CFXInfoTable.Create();
			CAbilityInfoTable.Create();
			CProductSaleInfoTable.Create();
			// 테이블을 생성한다 }
			
			// 저장소를 생성한다
			CAppInfoStorage.Create();
			CUserInfoStorage.Create();
			CGameInfoStorage.Create();
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
			
			// 이미지를 설정한다 {
			CFunc.SetupComponents(new List<(EKey, string, GameObject, GameObject)>() {
				(EKey.BG_IMG, $"{EKey.BG_IMG}", this.UIs, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_IMG)),
				(EKey.SPLASH_IMG, $"{EKey.SPLASH_IMG}", this.UIs, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_IMG))
			}, m_oImgDict, false);

			m_oImgDict[EKey.BG_IMG].color = KCDefine.SS_COLOR_BG_IMG;
			m_oImgDict[EKey.BG_IMG].rectTransform.sizeDelta = CSceneManager.CanvasSize;
			m_oImgDict[EKey.BG_IMG].gameObject.ExAddComponent<CSizeCorrector>().SetSizeRate(Vector3.one);

			m_oImgDict[EKey.SPLASH_IMG].sprite = CResManager.Inst.GetRes<Sprite>(KCDefine.U_IMG_P_SPLASH);
			m_oImgDict[EKey.SPLASH_IMG].transform.localPosition = POS_SPLASH_IMG;
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
