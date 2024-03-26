using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace OverlayScene
{
	/** 중첩 씬 관리자 */
	public partial class COverlaySceneManager : CSceneManager
	{
		#region 프로퍼티
#if UNITY_EDITOR
		public override int OrderScript => KCDefine.G_SCRIPT_O_MANAGER_SCENE_OVERLAY;
#endif // #if UNITY_EDITOR
		#endregion // 프로퍼티
	}
}
