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
	#region 상수
	private const string KEY_SAVE_TIME = "SaveTime";
	#endregion			// 상수

	#region 변수
	[Key(0)] public Dictionary<string, int> m_oIntDict = new Dictionary<string, int>();
	[Key(1)] public Dictionary<string, float> m_oFltDict = new Dictionary<string, float>();
	[Key(2)] public Dictionary<string, string> m_oStrDict = new Dictionary<string, string>();
	#endregion			// 변수

	#region 프로퍼티
	[IgnoreMember] public virtual bool IsIgnoreSaveTime => false;
	[IgnoreMember] public System.DateTime SaveTime => this.SaveTimeStr.ExIsValid() ? this.AdjustSaveTimeStr.ExToTime(KCDefine.B_DATE_T_FMT_SPLASH_YYYY_MM_DD_HH_MM_SS) : System.DateTime.Now;

	[IgnoreMember] private string SaveTimeStr => m_oStrDict.ExGetVal(CBaseInfo.KEY_SAVE_TIME, string.Empty);
	[IgnoreMember] private string AdjustSaveTimeStr => this.SaveTimeStr.Contains(KCDefine.B_TOKEN_SPLASH_STR) ? this.SaveTimeStr : this.SaveTimeStr.ExToTime(KCDefine.B_DATE_T_FMT_YYYY_MM_DD_HH_MM_SS).ExToLongStr();
	#endregion			// 프로퍼티

	#region 인터페이스
	//! 직렬화 될 경우
	public virtual void OnBeforeSerialize() {
		// 저장 시간 무시 모드가 아닐 경우
		if(!this.IsIgnoreSaveTime) {
			m_oStrDict.ExReplaceVal(CBaseInfo.KEY_SAVE_TIME, System.DateTime.Now.ExToLongStr());
		}
	}

	//! 역직렬화 되었을 경우
	public virtual void OnAfterDeserialize() {
		m_oIntDict = m_oIntDict ?? new Dictionary<string, int>();
		m_oFltDict = m_oFltDict ?? new Dictionary<string, float>();
		m_oStrDict = m_oStrDict ?? new Dictionary<string, string>();
	}
	#endregion			// 인터페이스

	#region 추가 변수

	#endregion			// 추가 변수

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if NEVER_USE_THIS
