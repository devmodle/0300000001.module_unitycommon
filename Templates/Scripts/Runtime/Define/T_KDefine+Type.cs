using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
using MessagePack;
using Newtonsoft.Json;

#region 기본
/** 타겟 정보 */
[System.Serializable]
public partial struct STTargetInfo : System.IEquatable<STTargetInfo> {
	public int m_nKinds;
	public ETargetKinds m_eTargetKinds;
	public EKindsGroupType m_eKindsGroupType;
	public STValInfo m_stValInfo;

	#region 상수
	public static readonly STTargetInfo INVALID = new STTargetInfo() {
		m_nKinds = KCDefine.B_IDX_INVALID, m_eTargetKinds = ETargetKinds.NONE, m_eKindsGroupType = EKindsGroupType.NONE, m_stValInfo = STValInfo.INVALID
	};
	#endregion			// 상수

	#region 프로퍼티
	public int Kinds {
		get {
			switch(m_eKindsGroupType) {
				case EKindsGroupType.SUB_TYPE: return m_nKinds.ExKindsToSubType();
				case EKindsGroupType.KINDS_TYPE: return m_nKinds.ExKindsToKindsType();
				case EKindsGroupType.SUB_KINDS_TYPE: return m_nKinds.ExKindsToSubKindsType();
			}
			
			return m_nKinds;
		}
	}

	public ETargetType TargetType => (ETargetType)((int)m_eTargetKinds).ExKindsToType();
	public ETargetKinds BaseTargetKinds => (ETargetKinds)((int)m_eTargetKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region IEquatable
	/** 동일 여부를 검사한다 */
	public bool Equals(STTargetInfo a_stTargetInfo) {
		return m_nKinds == a_stTargetInfo.m_nKinds && m_eTargetKinds == a_stTargetInfo.m_eTargetKinds && m_eKindsGroupType == a_stTargetInfo.m_eKindsGroupType && m_stValInfo.Equals(a_stTargetInfo.m_stValInfo);
	}
	#endregion			// IEquatable

	#region 함수
	/** 생성자 */
	public STTargetInfo(SimpleJSON.JSONNode a_oTargetInfo, int a_nSrcIdx = KCDefine.B_VAL_0_INT) {
		m_nKinds = a_oTargetInfo[a_nSrcIdx + KCDefine.B_VAL_2_INT].ExIsValid() ? a_oTargetInfo[a_nSrcIdx + KCDefine.B_VAL_2_INT].AsInt : KCDefine.B_IDX_INVALID;
		m_eTargetKinds = a_oTargetInfo[a_nSrcIdx + KCDefine.B_VAL_0_INT].ExIsValid() ? (ETargetKinds)a_oTargetInfo[a_nSrcIdx + KCDefine.B_VAL_0_INT].AsInt : ETargetKinds.NONE;
		m_eKindsGroupType = a_oTargetInfo[a_nSrcIdx + KCDefine.B_VAL_1_INT].ExIsValid() ? (EKindsGroupType)a_oTargetInfo[a_nSrcIdx + KCDefine.B_VAL_1_INT].AsInt : EKindsGroupType.NONE;
		m_stValInfo = new STValInfo(a_oTargetInfo, a_nSrcIdx + KCDefine.B_VAL_3_INT);
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR || UNITY_STANDALONE
	/** 타겟 정보를 생성한다 */
	public void MakeTargetInfo(string a_oKey, SimpleJSON.JSONClass a_oOutTargetInfo) {
		var oJSONArray = new SimpleJSON.JSONArray();
		oJSONArray.Add($"{(int)m_eTargetKinds}");
		oJSONArray.Add($"{(int)m_eKindsGroupType}");
		oJSONArray.Add($"{m_nKinds}");
		oJSONArray.Add($"{(int)m_stValInfo.m_eValType}");
		oJSONArray.Add(m_stValInfo.m_oVal);

		a_oOutTargetInfo.Add(a_oKey, oJSONArray);
	}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	#endregion			// 조건부 함수
}

/** 어빌리티 값 정보 */
[MessagePackObject][System.Serializable]
public partial struct STAbilityValInfo : System.IEquatable<STAbilityValInfo> {
	[Key(1)] public long m_nVal;
	[Key(11)] public EAbilityKinds m_eAbilityKinds;

	#region 상수
	public static readonly STAbilityValInfo INVALID = new STAbilityValInfo() {
		m_eAbilityKinds = EAbilityKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	[JsonIgnore][IgnoreMember] public EAbilityType AbilityType => (EAbilityType)((int)m_eAbilityKinds).ExKindsToType();
	[JsonIgnore][IgnoreMember] public EAbilityKinds BaseAbilityKinds => (EAbilityKinds)((int)m_eAbilityKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region IEquatable
	/** 동일 여부를 검사한다 */
	public bool Equals(STAbilityValInfo a_stAbilityValInfo) {
		return m_nVal == a_stAbilityValInfo.m_nVal && m_eAbilityKinds == a_stAbilityValInfo.m_eAbilityKinds;
	}
	#endregion			// IEquatable

	#region 함수
	/** 생성자 */
	public STAbilityValInfo(SimpleJSON.JSONNode a_oAbilityValInfo, int a_nSrcIdx = KCDefine.B_VAL_0_INT) {
		m_nVal = long.TryParse(a_oAbilityValInfo[a_nSrcIdx + KCDefine.B_VAL_1_INT], NumberStyles.Any, null, out long nLV) ? nLV : KCDefine.B_VAL_0_INT;
		m_eAbilityKinds = a_oAbilityValInfo[a_nSrcIdx + KCDefine.B_VAL_0_INT].ExIsValid() ? (EAbilityKinds)a_oAbilityValInfo[a_nSrcIdx + KCDefine.B_VAL_0_INT].AsInt : EAbilityKinds.NONE;
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR || UNITY_STANDALONE
	/** 어빌리티 값 정보를 생성한다 */
	public void MakeAbilityValInfo(string a_oKey, SimpleJSON.JSONClass a_oOutAbilityValInfo) {
		var oJSONArray = new SimpleJSON.JSONArray();
		oJSONArray.Add($"{(int)m_eAbilityKinds}");
		oJSONArray.Add($"{m_nVal}");

		a_oOutAbilityValInfo.Add(a_oKey, oJSONArray);
	}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	#endregion			// 조건부 함수
}

/** 타입 랩퍼 */
[MessagePackObject]
public partial struct STTypeWrapper {
	[Key(51)] public List<long> m_oUniqueLevelIDList;

	[Key(161)] public Dictionary<int, Dictionary<int, int>> m_oNumLevelInfosDictContainer;
	[Key(162)] public Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>> m_oLevelInfoDictContainer;
}
#endregion			// 기본
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
