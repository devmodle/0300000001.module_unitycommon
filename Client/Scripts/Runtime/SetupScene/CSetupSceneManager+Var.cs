using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using System.Diagnostics;
using DG.Tweening;

namespace SetupScene
{
	/** 설정 씬 관리자 - 변수 */
	public abstract partial class CSetupSceneManager : CSceneManager
	{
		#region 변수
		[Header("=====> Setup Scene Manager - Etc <=====")]
		private Tween m_oGaugeAnim = null;
		private Stopwatch m_oStopwatch = new Stopwatch();

		private System.Text.StringBuilder m_oStrBuilderA = new System.Text.StringBuilder();
		private System.Text.StringBuilder m_oStrBuilderB = new System.Text.StringBuilder();

		[Header("=====> Setup Scene Manager - UIs <=====")]
		private Dictionary<EKey, Text> m_oTextDict = new Dictionary<EKey, Text>();
		private Dictionary<EKey, CGaugeHandler> m_oGaugeHandlerDict = new Dictionary<EKey, CGaugeHandler>();

		[Header("=====> Setup Scene Manager - Game Objects <=====")]
		private Dictionary<EKey, GameObject> m_oUIDict = new Dictionary<EKey, GameObject>();
		#endregion // 변수

		#region 클래스 변수
		private static GameObject m_oPopupUIs = null;
		private static GameObject m_oTopmostUIs = null;
		private static GameObject m_oAbsUIs = null;
		private static GameObject m_oDebugUIs = null;
		private static GameObject m_oTimerManager = null;
		#endregion // 클래스 변수

		#region 프로퍼티
		public virtual bool IsIgnoreLoadingText => false;
		public virtual bool IsIgnoreLoadingGauge => false;

		public virtual Vector3 LoadingTextPos => Vector3.zero;
		public virtual Vector3 LoadingGaugePos => Vector3.zero;

		protected Dictionary<string, int> MaxNumFXSndsDict { get; } = new Dictionary<string, int>();
		protected Dictionary<string, System.Action<string>> DeviceMsgHandlerDict { get; } = new Dictionary<string, System.Action<string>>();

#if UNITY_EDITOR
		public override int OrderScript => KCDefine.B_SCRIPT_O_SETUP_SCENE_MANAGER;
#endif // #if UNITY_EDITOR
		#endregion // 프로퍼티
	}
}
