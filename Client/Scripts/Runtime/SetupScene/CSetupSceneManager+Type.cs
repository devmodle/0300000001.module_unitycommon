using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace SetupScene
{
	/** 설정 씬 관리자 - 타입 */
	public abstract partial class CSetupSceneManager : CSceneManager
	{
		/** 식별자 */
		private enum EKey
		{
			NONE = -1,
			LOADING_TEXT,
			SCENE_INFO_TEXT,
			LOADING_GAUGE_HANDLER,

			LOADING_GAUGE,
			[HideInInspector] MAX_VAL
		}

		/** 설정 씬 이벤트 */
		public enum ESetupSceneEvent
		{
			NONE = -1,
			LOAD_SETUP_SCENE,
			LOAD_LATE_SETUP_SCENE,
			LOAD_NEXT_SCENE,
			[HideInInspector] MAX_VAL
		}
	}
}
