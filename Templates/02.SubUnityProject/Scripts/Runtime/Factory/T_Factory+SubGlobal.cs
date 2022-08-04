#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE
/** 서브 전역 팩토리 */
public static partial class Factory {
	#region 클래스 함수
	
	#endregion			// 클래스 함수
}

/** 서브 타이틀 씬 팩토리 */
public static partial class Factory {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 서브 메인 씬 팩토리 */
public static partial class Factory {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 서브 게임 씬 팩토리 */
public static partial class Factory {
	#region 클래스 함수

	#endregion			// 클래스 함수

	#region 조건부 클래스 함수
#if UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
	/** 게임 컴포넌트 매개 변수를 생성한다 */
	public static GameScene.CGSComponent.STParams MakeGSComponentParams(SampleEngineName.CEngine a_oEngine) {
		return new GameScene.CGSComponent.STParams() {
			m_oEngine = a_oEngine
		};
	}
#endif			// #if UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
	#endregion			// 조건부 클래스 함수
}

/** 서브 로딩 씬 팩토리 */
public static partial class Factory {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 서브 중첩 씬 팩토리 */
public static partial class Factory {
	#region 클래스 함수

	#endregion			// 클래스 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
