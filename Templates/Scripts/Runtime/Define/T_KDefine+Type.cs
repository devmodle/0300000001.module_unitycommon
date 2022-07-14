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
public partial struct STTargetInfo {
	public int m_nKinds;
	public string m_oTarget01;
	public string m_oTarget02;
	public string m_oTarget03;
	public ETargetKinds m_eTargetKinds;

	#region 상수
	public static readonly STTargetInfo INVALID = new STTargetInfo() {
		m_nKinds = KCDefine.B_IDX_INVALID, m_eTargetKinds = ETargetKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public long IntTarget01 => long.TryParse(m_oTarget01, NumberStyles.Any, null, out long nTarget01) ? nTarget01 : KCDefine.B_VAL_0_INT;
	public long IntTarget02 => long.TryParse(m_oTarget02, NumberStyles.Any, null, out long nTarget02) ? nTarget02 : KCDefine.B_VAL_0_INT;
	public long IntTarget03 => long.TryParse(m_oTarget03, NumberStyles.Any, null, out long nTarget03) ? nTarget03 : KCDefine.B_VAL_0_INT;

	public double RealTarget01 => double.TryParse(m_oTarget01, NumberStyles.Any, null, out double dblTarget01) ? dblTarget01 : KCDefine.B_VAL_0_REAL;
	public double RealTarget02 => double.TryParse(m_oTarget02, NumberStyles.Any, null, out double dblTarget02) ? dblTarget02 : KCDefine.B_VAL_0_REAL;
	public double RealTarget03 => double.TryParse(m_oTarget03, NumberStyles.Any, null, out double dblTarget03) ? dblTarget03 : KCDefine.B_VAL_0_REAL;

	public ETargetType TargetType => (ETargetType)((int)m_eTargetKinds).ExKindsToType();
	public ETargetKinds BaseTargetKinds => (ETargetKinds)((int)m_eTargetKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STTargetInfo(SimpleJSON.JSONNode a_oTargetInfo) {
		m_nKinds = a_oTargetInfo[KCDefine.B_VAL_1_INT].ExIsValid() ? a_oTargetInfo[KCDefine.B_VAL_1_INT].AsInt : KCDefine.B_IDX_INVALID;
		m_oTarget01 = a_oTargetInfo[KCDefine.B_VAL_2_INT].ExIsValid() ? a_oTargetInfo[KCDefine.B_VAL_2_INT] : KCDefine.B_STR_0_INT;
		m_oTarget02 = a_oTargetInfo[KCDefine.B_VAL_3_INT].ExIsValid() ? a_oTargetInfo[KCDefine.B_VAL_3_INT] : KCDefine.B_STR_0_INT;
		m_oTarget03 = a_oTargetInfo[KCDefine.B_VAL_4_INT].ExIsValid() ? a_oTargetInfo[KCDefine.B_VAL_4_INT] : KCDefine.B_STR_0_INT;
		m_eTargetKinds = a_oTargetInfo[KCDefine.B_VAL_0_INT].ExIsValid() ? (ETargetKinds)a_oTargetInfo[KCDefine.B_VAL_0_INT].AsInt : ETargetKinds.NONE;
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR || UNITY_STANDALONE
	/** 타겟 정보를 생성한다 */
	public void MakeTargetInfo(string a_oKey, SimpleJSON.JSONClass a_oOutTargetInfo) {
		var oJSONArray = new SimpleJSON.JSONArray();
		oJSONArray.Add($"{(int)m_eTargetKinds}");
		oJSONArray.Add($"{m_nKinds}");
		oJSONArray.Add(m_oTarget01);
		oJSONArray.Add(m_oTarget02);
		oJSONArray.Add(m_oTarget03);

		a_oOutTargetInfo.Add(a_oKey, oJSONArray);
	}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	#endregion			// 조건부 함수
}

/** 어빌리티 값 정보 */
[MessagePackObject][System.Serializable]
public partial struct STAbilityValInfo {
	[Key(31)] public long m_nVal;
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

	#region 함수
	/** 생성자 */
	public STAbilityValInfo(SimpleJSON.JSONNode a_oAbilityValInfo) {
		m_nVal = long.TryParse(a_oAbilityValInfo[KCDefine.B_VAL_1_INT], NumberStyles.Any, null, out long nVal) ? nVal : KCDefine.B_VAL_0_INT;
		m_eAbilityKinds = a_oAbilityValInfo[KCDefine.B_VAL_0_INT].ExIsValid() ? (EAbilityKinds)a_oAbilityValInfo[KCDefine.B_VAL_0_INT].AsInt : EAbilityKinds.NONE;
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
