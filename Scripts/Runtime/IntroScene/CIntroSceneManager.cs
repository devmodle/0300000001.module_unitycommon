using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 인트로 씬 관리자
public class CIntroSceneManager : CSceneManager {
	#region 변수
	private List<string> m_oSceneNameList = new List<string>() {
		KCDefine.B_SCENE_N_INIT,
		KCDefine.B_SCENE_N_SPLASH,
		KCDefine.B_SCENE_N_START,
		KCDefine.B_SCENE_N_SETUP,
		KCDefine.B_SCENE_N_AGREE,
		KCDefine.B_SCENE_N_LATE_SETUP,
		KCDefine.B_SCENE_N_PERMISSION,
		KCDefine.B_SCENE_N_INTRO
	};
	#endregion			// 변수

	#region 프로퍼티
	public override string SceneName => KCDefine.B_SCENE_N_INTRO;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public sealed override void Start() {
		base.Start();

		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			StartCoroutine(this.OnStart());
		}
	}

	//! 씬을 설정한다
	protected virtual void Setup() {
		// Do Nothing
	}

	//! 다음 씬을 로드한다
	protected void LoadNextScene() {
		int nIdx = m_oSceneNameList.ExFindVal((a_oSceneName) => CSceneManager.AwakeSceneName.ExIsEquals(a_oSceneName));

		this.ExLateCallFunc((a_oSender, a_oParams) => {
			// 초기화 씬 일 경우
			if(m_oSceneNameList.ExIsValidIdx(nIdx)) {
#if STUDY_MODULE_ENABLE
				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MENU);
#else
				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_TITLE);
#endif			// #if STUDY_MODULE_ENABLE
			} else {
				CCommonAppInfoStorage.Inst.IsFirstStart = false;
				CSceneLoader.Inst.LoadScene(CSceneManager.AwakeSceneName, false, false);
			}
		}, KCDefine.U_DELAY_NEXT_SCENE_LOAD);
	}

	//! 초기화
	private IEnumerator OnStart() {
		yield return CFactory.CreateWaitForSecs(KCDefine.U_DELAY_INIT);
		CSceneLoader.Inst.UnloadSceneAsync(KCDefine.B_SCENE_N_PERMISSION, null);

		this.Setup();
	}
	#endregion			// 함수
}
