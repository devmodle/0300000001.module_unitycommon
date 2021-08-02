using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MessagePack;

#if NEVER_USE_THIS
//! 기본 정보
[Union(0, typeof(CAppInfo))]
[Union(1, typeof(CUserInfo))]
[Union(2, typeof(CGameInfo))]
[Union(3, typeof(CClearInfo))]
[Union(4, typeof(CCellInfo))]
[Union(5, typeof(CLevelInfo))]
[MessagePackObject]
[System.Serializable]
public abstract class CBaseInfo : IMessagePackSerializationCallbackReceiver {
	#region 변수
	[Key(0)] public Dictionary<string, bool> m_oBoolDict = new Dictionary<string, bool>();
	[Key(1)] public Dictionary<string, int> m_oIntDict = new Dictionary<string, int>();
	[Key(2)] public Dictionary<string, float> m_oFltDict = new Dictionary<string, float>();
	[Key(3)] public Dictionary<string, string> m_oStrDict = new Dictionary<string, string>();
	#endregion			// 변수

	#region 인터페이스
	//! 직렬화 될 경우
	public virtual void OnBeforeSerialize() {
		// Do Something
	}

	//! 역직렬화 되었을 경우
	public virtual void OnAfterDeserialize() {
		m_oBoolDict = (m_oBoolDict != null) ? m_oBoolDict : new Dictionary<string, bool>();
		m_oIntDict = (m_oIntDict != null) ? m_oIntDict : new Dictionary<string, int>();
		m_oFltDict = (m_oFltDict != null) ? m_oFltDict : new Dictionary<string, float>();
		m_oStrDict = (m_oStrDict != null) ? m_oStrDict : new Dictionary<string, string>();
	}
	#endregion			// 인터페이스
}
#endif			// #if NEVER_USE_THIS
