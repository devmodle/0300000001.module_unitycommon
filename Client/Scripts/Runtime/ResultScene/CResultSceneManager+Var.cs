using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace ResultScene
{
	/** 결과 씬 관리자 - 변수 */
	public partial class CResultSceneManager : CSceneManager
	{
		#region 프로퍼티
#if UNITY_EDITOR
		public override int OrderScript => KCDefine.G_SCRIPT_O_MANAGER_SCENE_RESULT;
#endif // #if UNITY_EDITOR
		#endregion // 프로퍼티
	}
}
