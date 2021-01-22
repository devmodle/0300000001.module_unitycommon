using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif			// #if UNITY_EDITOR

//! 샘플 씬 관리자
public class CSampleSceneManager : CSceneManager {
	#region 프로퍼티
	public override string SceneName => KCDefine.B_SCENE_N_SAMPLE;
	#endregion			// 프로퍼티

	#region 조건부 클래스 함수
#if UNITY_EDITOR
	//! 씬 관리자를 설정한다
	public static void SetupSceneManager(Scene a_stScene, Dictionary<string, System.Type> a_oSceneManagerTypeList) {
		foreach(var stKeyValue in a_oSceneManagerTypeList) {
			// 씬 관리자 타입과 동일 할 경우
			if(a_stScene.name.ExIsEquals(stKeyValue.Key)) {
				var oSceneManager = a_stScene.ExFindChild(KCDefine.U_OBJ_N_SCENE_MANAGER);

				// 씬 관리자 추가가 필요 할 경우
				if(oSceneManager != null && oSceneManager.GetComponentInChildren(stKeyValue.Value) == null) {
					oSceneManager.AddComponent(stKeyValue.Value);
					oSceneManager.ExRemoveComponent<CSampleSceneManager>();
					
					EditorSceneManager.MarkSceneDirty(a_stScene);
				}
			}
		}
	}
#endif			// #if UNITY_EDITOR
	#endregion			// 조건부 클래스 함수
}
