using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** 테스트 씬 관리자 */
public abstract class CTestSceneManager : CSceneManager {
	#region 프로퍼티
	public override string SceneName => KCDefine.B_SCENE_N_TEST;
	#endregion			// 프로퍼티
}
