using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if INPUT_SYSTEM_MODULE_ENABLE
using UnityEngine.InputSystem;
#endif			// #if INPUT_SYSTEM_MODULE_ENABLE

//! 타이틀 씬 관리자
public class CTitleSceneManager : CSceneManager {
	#region 프로퍼티
	public override string SceneName => KCDefine.B_SCENE_N_TITLE;
	#endregion			// 프로퍼티

	#region 함수
	//! 상태를 갱신한다
	public override void OnUpdate(float a_fDeltaTime) {
		base.OnUpdate(a_fDeltaTime);

#if EDITOR_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE)
#if INPUT_SYSTEM_MODULE_ENABLE
		bool bIsEditorKeyDown = Keyboard.current.leftShiftKey.isPressed && Keyboard.current.eKey.wasPressedThisFrame;
#else
		bool bIsEditorKeyDown = Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.E);
#endif			// #if INPUT_SYSTEM_MODULE_ENABLE

		// 에디터 키를 눌렀을 경우
		if(bIsEditorKeyDown) {
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_LEVEL_EDITOR);
		}
#endif			// #if EDITOR_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE)
	}
	#endregion			// 함수
}
