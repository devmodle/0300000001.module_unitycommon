using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif // #if UNITY_EDITOR

/** 샘플 씬 관리자 */
public partial class CSampleSceneManager : CSceneManager
{
	#region 클래스 함수
#if UNITY_EDITOR
	/** 씬 관리자를 설정한다 */
	public static void SetupSceneManager(Scene a_stScene, 
		Dictionary<string, System.Type> a_oSceneManagerTypeDict)
	{
		foreach(var stKeyVal in a_oSceneManagerTypeDict)
		{
			// 관리자 설정이 불가능 할 경우
			if(!a_stScene.name.Equals(stKeyVal.Key))
			{
				continue;
			}

			var oSceneManager = a_stScene.ExFindChild(KCDefine.U_OBJ_N_SCENE_MANAGER);

			bool bIsEnableSetup = oSceneManager != null;
			bIsEnableSetup = bIsEnableSetup && oSceneManager.GetComponentInChildren(stKeyVal.Value) == null;

			// 관리자 설정이 불가능 할 경우
			if(!bIsEnableSetup)
			{
				continue;
			}

			EditorSceneManager.MarkSceneDirty(a_stScene);

			oSceneManager.ExRemoveComponent<CSceneManager>(false);
			oSceneManager.AddComponent(stKeyVal.Value);
		}
	}

	/** 내비게이션 스택 이벤트를 수신했을 경우 */
	public override void OnReceiveNavStackEvent(ENavStackEvent a_eEvent)
	{
		base.OnReceiveNavStackEvent(a_eEvent);

		switch(a_eEvent)
		{
			case ENavStackEvent.BACK_KEY_DOWN:
#if RESEARCH_MODULE_ENABLE && SCENE_TEMPLATES_MODULE_ENABLE
				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MENU);
#elif EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_TITLE);
#endif // #if RESEARCH_MODULE_ENABLE && SCENE_TEMPLATES_MODULE_ENABLE

				break;
		}
	}
#endif // #if UNITY_EDITOR
	#endregion // 클래스 함수
}
