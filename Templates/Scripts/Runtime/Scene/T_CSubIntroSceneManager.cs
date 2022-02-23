using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if SCENE_TEMPLATES_MODULE_ENABLE
/** 서브 인트로 씬 관리자 */
public class CSubIntroSceneManager : CIntroSceneManager {
	#region 추가 변수

	#endregion			// 추가 변수

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 함수
	/** 씬을 설정한다 */
	protected override void Setup() {
		base.Setup();
		this.LoadNextScene();
	}
	#endregion			// 함수

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if SCENE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
