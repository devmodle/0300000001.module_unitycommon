using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace TestScene
{
	/** 테스트 씬 관리자 - 타입 */
	public abstract partial class CTestSceneManager : CSceneManager
	{
		/** 식별자 */
		private enum EKey
		{
			NONE,
			BACK_BTN,
			[HideInInspector] MAX_VAL
		}
	}
}
