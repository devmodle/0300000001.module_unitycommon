using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace InitScene
{
	/** 초기화 씬 관리자 - 변수 */
	public abstract partial class CInitSceneManager : CSceneManager
	{
		#region 클래스 변수
		private static GameObject m_oBlindUIs = null;
		#endregion // 클래스 변수
	}
}
