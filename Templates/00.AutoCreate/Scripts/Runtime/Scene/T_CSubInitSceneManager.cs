#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCENE_TEMPLATES_MODULE_ENABLE
namespace InitScene {
	/** 서브 초기화 씬 관리자 */
	public partial class CSubInitSceneManager : CInitSceneManager {
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
		public static readonly Color COLOR_BG_IMG = new Color(0x29 / (float)KCDefine.B_UNIT_NORM_VAL_TO_BYTE, 0x4c / (float)KCDefine.B_UNIT_NORM_VAL_TO_BYTE, 0x94 / (float)KCDefine.B_UNIT_NORM_VAL_TO_BYTE, 1.0f);
		private static readonly Vector3 POS_SPLASH_IMG = new Vector3(0.0f, 25.0f, 0.0f);
		#endregion			// 상수

		#region 함수
		/** 씬을 설정한다 */
		protected override void Setup() {
			base.Setup();

			// 이미지를 설정한다 {
			CFunc.SetupComponents(new List<(EKey, string, GameObject, GameObject)>() {
				(EKey.BG_IMG, $"{EKey.BG_IMG}", this.UIs, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_IMG)),
				(EKey.SPLASH_IMG, $"{EKey.SPLASH_IMG}", this.UIs, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_IMG))
			}, m_oImgDict, false);

			m_oImgDict.GetValueOrDefault(EKey.BG_IMG).color = COLOR_BG_IMG;
			m_oImgDict.GetValueOrDefault(EKey.BG_IMG).rectTransform.sizeDelta = CSceneManager.CanvasSize;
			m_oImgDict.GetValueOrDefault(EKey.BG_IMG).gameObject.ExAddComponent<CSizeCorrector>().SetSizeRate(Vector3.one);

			m_oImgDict.GetValueOrDefault(EKey.SPLASH_IMG).sprite = CResManager.Inst.GetRes<Sprite>(KCDefine.U_IMG_P_SPLASH);
			m_oImgDict.GetValueOrDefault(EKey.SPLASH_IMG).transform.localPosition = POS_SPLASH_IMG;
			m_oImgDict.GetValueOrDefault(EKey.SPLASH_IMG).gameObject.SetActive(false);
			// 이미지를 설정한다 }

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
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
			CProductTradeInfoTable.Create();
			// 테이블을 생성한다 }
			
			// 저장소를 생성한다
			CAppInfoStorage.Create();
			CUserInfoStorage.Create();
			CGameInfoStorage.Create();
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		}

		/** 스플래시를 출력한다 */
		protected override void ShowSplash() {
			m_oImgDict.GetValueOrDefault(EKey.SPLASH_IMG).SetNativeSize();
			m_oImgDict.GetValueOrDefault(EKey.SPLASH_IMG).gameObject.SetActive(true);

			this.ExLateCallFunc((a_oSender) => this.LoadNextScene(), KCDefine.IS_DELAY_NEXT_SCENE_LOAD);
		}
		#endregion			// 함수
	}
}
#endif			// #if SCENE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
