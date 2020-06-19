using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 스크립트 객체
public class CScriptableObject<T> : ScriptableObject where T : CScriptableObject<T> {
	#region 변수
	private static T m_tInstance = null;

	public static bool m_bIsRuntime = false;
	private static string m_oFilepath = string.Empty;
	#endregion			// 변수

	#region 클래스 프로퍼티
	public static T Instance {
		get {
			if(m_tInstance == null) {
				CScriptableObject<T>.m_tInstance = CResourceManager.Instance.GetScriptableObject<T>(CScriptableObject<T>.m_oFilepath);
				Function.Assert(CScriptableObject<T>.m_tInstance != null);
				
				CScriptableObject<T>.m_tInstance.Awake();
			}

			return CScriptableObject<T>.m_tInstance;
		}
	}
	#endregion			// 클래스 프로퍼티

	#region 함수
	//! 초기화
	public virtual void Awake() {
		// Do Nothing
	}
	#endregion			// 함수

	#region 클래스 함수
	//! 인스턴스를 생성한다
	public static T Create(string a_oFilepath) {
		Function.Assert(a_oFilepath.ExIsValid());
		CScriptableObject<T>.m_oFilepath = a_oFilepath;

		return CScriptableObject<T>.Instance;
	}
	#endregion			// 클래스 함수
}
