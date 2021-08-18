using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if INPUT_SYSTEM_MODULE_ENABLE
using UnityEngine.InputSystem;
#endif			// #if INPUT_SYSTEM_MODULE_ENABLE

//! 타이틀 씬 관리자
public class CTitleSceneManager : CSceneManager {
	#region 프로퍼티
	public override bool IsRealtimeFadeInAni => true;
	public override bool IsRealtimeFadeOutAni => true;
	
	public override string SceneName => KCDefine.B_SCENE_N_TITLE;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			Time.timeScale = KCDefine.B_VAL_1_FLT;
		}
	}
	
	//! 상태를 갱신한다
	public override void OnUpdate(float a_fDeltaTime) {
		base.OnUpdate(a_fDeltaTime);

		// 앱이 실행 중 일 경우
		if(CSceneManager.IsAppRunning) {
#if EDITOR_ENABLE && UNITY_STANDALONE
#if INPUT_SYSTEM_MODULE_ENABLE
			bool bIsEditorKeyDown = Keyboard.current.leftShiftKey.isPressed && Keyboard.current.eKey.wasPressedThisFrame;
#else
			bool bIsEditorKeyDown = Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.E);
#endif			// #if INPUT_SYSTEM_MODULE_ENABLE

			// 에디터 키를 눌렀을 경우
			if(bIsEditorKeyDown) {
				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_LEVEL_EDITOR);
			}
#endif			// #if EDITOR_ENABLE && UNITY_STANDALONE
		}
	}
	#endregion			// 함수
}
