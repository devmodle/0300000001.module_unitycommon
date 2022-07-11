using System.Collections;
using System.Collections.Generic;
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
	public string m_oTargets;
	public ETargetKinds m_eTargetKinds;

	#region 상수
	public static readonly STTargetInfo INVALID = new STTargetInfo() {
		m_nKinds = KCDefine.B_IDX_INVALID, m_eTargetKinds = ETargetKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public long IntTargets => long.TryParse(m_oTargets, out long nTargets) ? nTargets : KCDefine.B_VAL_0_INT;
	public double RealTargets => double.TryParse(m_oTargets, out double dblTargets) ? dblTargets : KCDefine.B_VAL_0_REAL;
	public ETargetType TargetType => (ETargetType)((int)m_eTargetKinds).ExKindsToType();
	public ETargetKinds BaseTargetKinds => (ETargetKinds)((int)m_eTargetKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region IEquatable
	public bool Equals(STTargetInfo a_stTargetInfo) {
		return a_stTargetInfo.m_nKinds == m_nKinds && a_stTargetInfo.m_oTargets.Equals(m_oTargets) && a_stTargetInfo.m_eTargetKinds == m_eTargetKinds;
	}
	#endregion			// IEquatable

	#region 함수
	/** 생성자 */
	public STTargetInfo(SimpleJSON.JSONNode a_oTargetInfo, string a_oPrefix, int a_nIdx) {
		string oKindsKey = string.Format(KCDefine.B_TEXT_FMT_2_COMBINE, a_oPrefix, string.Format(KCDefine.U_KEY_FMT_KINDS, a_nIdx + KCDefine.B_VAL_1_INT));
		string oTargetsKey = string.Format(KCDefine.B_TEXT_FMT_2_COMBINE, a_oPrefix, string.Format(KCDefine.U_KEY_FMT_TARGET, a_nIdx + KCDefine.B_VAL_1_INT));
		string oTargetKindsKey = string.Format(KCDefine.B_TEXT_FMT_2_COMBINE, a_oPrefix, string.Format(KCDefine.U_KEY_FMT_TARGET_KINDS, a_nIdx + KCDefine.B_VAL_1_INT));

		m_nKinds = a_oTargetInfo[oKindsKey].ExIsValid() ? a_oTargetInfo[oKindsKey].AsInt : KCDefine.B_IDX_INVALID;
		m_oTargets = a_oTargetInfo[oTargetsKey].ExIsValid() ? a_oTargetInfo[oTargetsKey] : KCDefine.B_STR_0_INT;
		m_eTargetKinds = a_oTargetInfo[oTargetKindsKey].ExIsValid() ? (ETargetKinds)a_oTargetInfo[oTargetKindsKey].AsInt : ETargetKinds.NONE;
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR || UNITY_STANDALONE
	/** 타겟 정보를 생성한다 */
	public void MakeTargetInfo(SimpleJSON.JSONClass a_oOutTargetInfo, string a_oPrefix, int a_nIdx) {
		a_oOutTargetInfo.Add(string.Format(KCDefine.B_TEXT_FMT_2_COMBINE, a_oPrefix, string.Format(KCDefine.U_KEY_FMT_TARGET_KINDS, a_nIdx + KCDefine.B_VAL_1_INT)), $"{(int)m_eTargetKinds}");
		a_oOutTargetInfo.Add(string.Format(KCDefine.B_TEXT_FMT_2_COMBINE, a_oPrefix, string.Format(KCDefine.U_KEY_FMT_KINDS, a_nIdx + KCDefine.B_VAL_1_INT)), $"{m_nKinds}");
		a_oOutTargetInfo.Add(string.Format(KCDefine.B_TEXT_FMT_2_COMBINE, a_oPrefix, string.Format(KCDefine.U_KEY_FMT_TARGET, a_nIdx + KCDefine.B_VAL_1_INT)), m_oTargets);
	}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	#endregion			// 조건부 함수
}

/** 어빌리티 값 정보 */
[MessagePackObject][System.Serializable]
public partial struct STAbilityValInfo : System.IEquatable<STAbilityValInfo> {
	[Key(1)] public long m_nLV;
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
	public bool Equals(STAbilityValInfo a_stAbilityValInfo) {
		return a_stAbilityValInfo.m_nLV == m_nLV && a_stAbilityValInfo.m_eAbilityKinds == m_eAbilityKinds;
	}
	#endregion			// IEquatable

	#region 함수
	/** 생성자 */
	public STAbilityValInfo(SimpleJSON.JSONNode a_oAbilityValInfo, int a_nIdx) {
		string oAbilityLVKey = string.Format(KCDefine.U_KEY_FMT_ABILITY_LV, a_nIdx + KCDefine.B_VAL_1_INT);
		string oAbilityKindsKey = string.Format(KCDefine.U_KEY_FMT_ABILITY_KINDS, a_nIdx + KCDefine.B_VAL_1_INT);

		m_nLV = long.TryParse(a_oAbilityValInfo[oAbilityLVKey], out long nLV) ? nLV : KCDefine.B_VAL_0_INT;
		m_eAbilityKinds = a_oAbilityValInfo[oAbilityKindsKey].ExIsValid() ? (EAbilityKinds)a_oAbilityValInfo[oAbilityKindsKey].AsInt : EAbilityKinds.NONE;
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR || UNITY_STANDALONE
	/** 어빌리티 값 정보를 생성한다 */
	public void MakeAbilityValInfo(SimpleJSON.JSONClass a_oOutAbilityValInfo, int a_nIdx) {
		a_oOutAbilityValInfo.Add(string.Format(KCDefine.U_KEY_FMT_ABILITY_KINDS, a_nIdx + KCDefine.B_VAL_1_INT), $"{(int)m_eAbilityKinds}");
		a_oOutAbilityValInfo.Add(string.Format(KCDefine.U_KEY_FMT_ABILITY_LV, a_nIdx + KCDefine.B_VAL_1_INT), $"{m_nLV}");
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
