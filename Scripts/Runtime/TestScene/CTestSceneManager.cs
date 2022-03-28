using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** 테스트 씬 관리자 */
public abstract class CTestSceneManager : CSceneManager {
	#region 변수
	/** =====> UI <===== */
	protected Button m_oBackBtn = null;
	#endregion			// 변수

	#region 프로퍼티
	public override string SceneName => KCDefine.B_SCENE_N_TEST;
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 앱이 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			this.SetupAwake();
		}
	}

	/** 내비게이션 스택 이벤트를 수신했을 경우 */
	public override void OnReceiveNavStackEvent(ENavStackEvent a_eEvent) {
		// 백 키 눌림 이벤트 일 경우
		if(a_eEvent == ENavStackEvent.BACK_KEY_DOWN) {
			this.OnTouchBackBtn();
		}
	}

	/** 백 버튼을 눌렀을 경우 */
	protected virtual void OnTouchBackBtn() {
#if STUDY_MODULE_ENABLE
		CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MENU);
#else
		CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_TITLE);
#endif			// #if STUDY_MODULE_ENABLE
	}

	/** 씬을 설정한다 */
	private void SetupAwake() {
		// 버튼을 설정한다 {
		m_oBackBtn = CFactory.CreateCloneObj<Button>(KCDefine.U_OBJ_N_BACK_BTN, KCDefine.U_OBJ_P_G_BACK_BTN, this.UpLeftUIs);
		m_oBackBtn.onClick.AddListener(this.OnTouchBackBtn);

		(m_oBackBtn.transform as RectTransform).pivot = KCDefine.B_ANCHOR_UP_LEFT;
		(m_oBackBtn.transform as RectTransform).anchorMin = KCDefine.B_ANCHOR_UP_LEFT;
		(m_oBackBtn.transform as RectTransform).anchorMax = KCDefine.B_ANCHOR_UP_LEFT;
		(m_oBackBtn.transform as RectTransform).anchoredPosition = Vector3.zero;
		// 버튼을 설정한다 }
	}
	#endregion			// 함수
}
