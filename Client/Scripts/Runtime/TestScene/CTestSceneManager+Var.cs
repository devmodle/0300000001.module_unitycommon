using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace TestScene
{
	/** 테스트 씬 관리자 - 변수 */
	public abstract partial class CTestSceneManager : CSceneManager
	{
		#region 변수
		[Header("=====> Test Scene Manager - UIs <=====")]
		private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>();
		#endregion // 변수

		#region 프로퍼티
		public override bool IsEnableTestUIs => true;
		public override bool IsEnableOverlayScene => true;
		public override bool IsEnableBGTouchResponder => true;
		#endregion // 프로퍼티
	}
}
