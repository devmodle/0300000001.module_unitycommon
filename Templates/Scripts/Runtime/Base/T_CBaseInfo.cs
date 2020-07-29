using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
#if MSG_PACK_ENABLE
using MessagePack;

//! 기본 정보
[Union(0, typeof(CGameInfo))]
[MessagePackObject]
[System.Serializable]
public abstract class CBaseInfo : IMessagePackSerializationCallbackReceiver {
	#region 변수
	[Key(0)] public STVersionInfo m_stVersionInfo;

	[Key(1)] public Dictionary<string, bool> m_oBoolList = new Dictionary<string, bool>();
	[Key(2)] public Dictionary<string, int> m_oIntList = new Dictionary<string, int>();
	[Key(3)] public Dictionary<string, float> m_oFloatList = new Dictionary<string, float>();
	[Key(4)] public Dictionary<string, string> m_oStringList = new Dictionary<string, string>();
	#endregion			// 변수

	#region 인터페이스
	//! 직렬화 될 경우
	public virtual void OnBeforeSerialize() {
		// Do Nothing
	}

	//! 역직렬화 되었을 경우
	public virtual void OnAfterDeserialize() {
		m_oBoolList = m_oBoolList ?? new Dictionary<string, bool>();
		m_oIntList = m_oIntList ?? new Dictionary<string, int>();
		m_oFloatList = m_oFloatList ?? new Dictionary<string, float>();
		m_oStringList = m_oStringList ?? new Dictionary<string, string>();
	}
	#endregion			// 인터페이스

	#region 함수
	//! 생성자
	public CBaseInfo(string a_oVersion) {
		m_stVersionInfo = CFunc.MakeDefVersionInfo(a_oVersion);
	}
	#endregion			// 함수
}
#endif			// #if MSG_PACK_ENABLE
#endif			// #if NEVER_USE_THIS