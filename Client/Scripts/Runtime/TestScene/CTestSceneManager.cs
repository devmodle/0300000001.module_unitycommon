using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace TestScene
{
	/** 테스트 씬 관리자 */
	public abstract partial class CTestSceneManager : CSceneManager
	{
		#region 함수
		/** 초기화 */
		public override void Awake()
		{
			base.Awake();

			// 앱 초기화가 필요 할 경우
			if(!CSceneManager.IsInitApp)
			{
				return;
			}

			// 버튼을 설정한다 {
			CFunc.SetupButtons(new List<(EKey, string, GameObject, GameObject, UnityAction)>()
			{
				(EKey.BACK_BTN, $"{EKey.BACK_BTN}", this.StretchUpUIs, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_G_BACK_BTN), this.OnTouchBackBtn)
			}, m_oBtnDict);

			var oRectTrans = m_oBtnDict[EKey.BACK_BTN].transform as RectTransform;
			oRectTrans.pivot = KCDefine.B_ANCHOR_UP_LEFT;
			oRectTrans.anchorMin = KCDefine.B_ANCHOR_UP_LEFT;
			oRectTrans.anchorMax = KCDefine.B_ANCHOR_UP_LEFT;
			oRectTrans.anchoredPosition = Vector3.zero;
			// 버튼을 설정한다 }
		}

		/** 내비게이션 스택 이벤트를 수신했을 경우 */
		public override void OnReceiveNavStackEvent(ENavStackEvent a_eEvent)
		{
			switch(a_eEvent)
			{
				case ENavStackEvent.BACK_KEY_DOWN:
					break;
			}
		}

		/** 백 버튼을 눌렀을 경우 */
		protected virtual void OnTouchBackBtn()
		{
#if RESEARCH_MODULE_ENABLE && SCENE_TEMPLATES_MODULE_ENABLE
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MENU);
#elif EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_TITLE);
#endif // #if RESEARCH_MODULE_ENABLE && SCENE_TEMPLATES_MODULE_ENABLE
		}
		#endregion // 함수
	}
}
