#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.Runtime.Serialization;
using MessagePack;

#if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
using Newtonsoft.Json;
#endif // #if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE

/** 기본 정보 */
[MessagePackObject]
[System.Serializable]
public partial struct STBaseInfo : System.ICloneable, IMessagePackSerializationCallbackReceiver {
	#region 변수
	[Key(0)] public Dictionary<string, string> m_oDictStr;
	#endregion // 변수

	#region ICloneable
	/** 사본 객체를 생성한다 */
	public object Clone() {
		var stBaseInfo = new STBaseInfo(null);
		this.SetupCloneInst(ref stBaseInfo);

		stBaseInfo.OnAfterDeserialize();
		return stBaseInfo;
	}
	#endregion // ICloneable

	#region IMessagePackSerializationCallbackReceiver
	/** 직렬화 될 경우 */
	public void OnBeforeSerialize() {
		// Do Something
	}

	/** 역직렬화되었을 경우 */
	public void OnAfterDeserialize() {
		m_oDictStr = m_oDictStr ?? new Dictionary<string, string>();
	}
	#endregion // IMessagePackSerializationCallbackReceiver

	#region 함수
	/** 생성자 */
	public STBaseInfo(Dictionary<string, string> a_oStrDict) {
		m_oDictStr = a_oStrDict ?? new Dictionary<string, string>();
	}

	/** 사본 객체를 설정한다 */
	private void SetupCloneInst(ref STBaseInfo a_stOutBaseInfo) {
		m_oDictStr.ExCopyTo(a_stOutBaseInfo.m_oDictStr, (_, a_oStr) => a_oStr);
	}
	#endregion // 함수

	#region 조건부 함수
#if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
	/** 직렬화 될 경우 */
	[OnSerializing]
	private void OnSerializing(StreamingContext a_oContext) {
		this.OnBeforeSerialize();
	}

	/** 역직렬화되었을 경우 */
	[OnDeserialized]
	private void OnDeserialized(StreamingContext a_oContext) {
		this.OnAfterDeserialize();
	}
#endif // #if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
#endregion // 조건부 함수
}

/** 기본 정보 */
[Union(0, typeof(CAppInfo))]
[Union(1, typeof(CUserInfo))]
[Union(2, typeof(CGameInfo))]
[Union(3, typeof(CClearInfo))]
[Union(4, typeof(CTargetInfo))]
[MessagePackObject]
[System.Serializable]
public abstract partial class CBaseInfo : IMessagePackSerializationCallbackReceiver {
	#region 변수
	[Key(0)] public Dictionary<string, string> m_oDictStr = new Dictionary<string, string>();
	#endregion // 변수

	#region 상수
	private const string KEY_VER = "Ver";
	#endregion // 상수

	#region 프로퍼티
#if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
	[JsonIgnore]
	[IgnoreMember]
	public System.Version Ver {
		get { return System.Version.Parse(m_oDictStr.ExGetVal(KEY_VER, KCDefine.G_VER_STR_DEF)); }
		set { m_oDictStr.ExReplaceVal(KEY_VER, value.ToString(KCDefine.B_VAL_3_INT)); }
	}

	[JsonIgnore][IgnoreMember] public virtual bool IsEnableVer => false;
#else
	[IgnoreMember]
	public System.Version Ver {
		get { return System.Version.Parse(m_oDictStr.ExGetVal(KEY_VER, KCDefine.G_VER_STR_DEF)); }
		set { m_oDictStr.ExReplaceVal(KEY_VER, value.ToString(KCDefine.B_VAL_3_INT)); }
	}

	[IgnoreMember] public virtual bool IsEnableVer => false;
#endif // #if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
#endregion // 프로퍼티

	#region IMessagePackSerializationCallbackReceiver
	/** 직렬화 될 경우 */
	public virtual void OnBeforeSerialize() {
		// 버전 유지 모드 일 경우
		if(this.IsEnableVer) {
			return;
		}

		m_oDictStr.ExRemoveVal(KEY_VER);
	}

	/** 역직렬화되었을 경우 */
	public virtual void OnAfterDeserialize() {
		m_oDictStr = m_oDictStr ?? new Dictionary<string, string>();
	}
	#endregion // IMessagePackSerializationCallbackReceiver

	#region 함수
	/** 생성자 */
	public CBaseInfo(System.Version a_stVer) {
		this.Ver = a_stVer;
	}
	#endregion // 함수

	#region 조건부 함수
#if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
	/** 직렬화 될 경우 */
	[OnSerializing]
	private void OnSerializing(StreamingContext a_oContext) {
		this.OnBeforeSerialize();
	}

	/** 역직렬화되었을 경우 */
	[OnDeserialized]
	private void OnDeserialized(StreamingContext a_oContext) {
		this.OnAfterDeserialize();
	}
#endif // #if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
#endregion // 조건부 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
