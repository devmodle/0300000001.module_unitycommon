using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 싱글턴
public abstract class CSingleton<T> : CComponent where T : CSingleton<T> {
	#region 클래스 변수
	private static T m_tInstance = null;
	#endregion			// 클래스 변수

	#region 클래스 프로퍼티
	public static T Instance {
		get {
			if(CSingleton<T>.m_tInstance == null) {
				CSingleton<T>.m_tInstance = CUFactory.CreateObj<T>(typeof(T).ToString(), null);
				CBAccess.Assert(CSingleton<T>.m_tInstance != null);

				DontDestroyOnLoad(CSingleton<T>.m_tInstance.gameObject);
			}

			return CSingleton<T>.m_tInstance;
		}
	}
	#endregion			// 클래스 프로퍼티
	
	#region 클래스 함수
	//! 인스턴스를 생성한다
	public static T Create() {
		return CSingleton<T>.Instance;
	}
	#endregion			// 클래스 함수

	#region 조건부 함수
#if UNITY_EDITOR
	//! 스크립트 순서를 설정한다
	protected override void SetupScriptOrder() {
		CSingleton<T>.m_tInstance.ExSetScriptOrder(KUDefine.SCRIPT_ORDER_SINGLETON);
	}
#endif			// #if UNITY_EDITOR
	#endregion			// 조건부 함수
}
