#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 어빌리티 정보 */
[System.Serializable]
public partial struct STAbilityInfo {
	public STCommonInfo m_stCommonInfo;
	public STValInfo m_stValInfo;

	public EAbilityKinds m_eAbilityKinds;
	public EAbilityKinds m_ePrevAbilityKinds;
	public EAbilityKinds m_eNextAbilityKinds;

	public Dictionary<ulong, STTargetInfo> m_oExtraAbilityTargetInfoDict;

	#region 상수
	public static STAbilityInfo INVALID = new STAbilityInfo() {
		m_eAbilityKinds = EAbilityKinds.NONE, m_ePrevAbilityKinds = EAbilityKinds.NONE, m_eNextAbilityKinds = EAbilityKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public EAbilityType AbilityType => (EAbilityType)((int)m_eAbilityKinds).ExKindsToType();
	public EAbilityKinds BaseAbilityKinds => (EAbilityKinds)((int)m_eAbilityKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STAbilityInfo(SimpleJSON.JSONNode a_oAbilityInfo) {
		m_stCommonInfo = new STCommonInfo(a_oAbilityInfo);
		m_stValInfo = new STValInfo(a_oAbilityInfo[KCDefine.U_KEY_VAL_INFO]);

		m_eAbilityKinds = a_oAbilityInfo[KCDefine.U_KEY_ABILITY_KINDS].ExIsValid() ? (EAbilityKinds)a_oAbilityInfo[KCDefine.U_KEY_ABILITY_KINDS].AsInt : EAbilityKinds.NONE;
		m_ePrevAbilityKinds = a_oAbilityInfo[KCDefine.U_KEY_PREV_ABILITY_KINDS].ExIsValid() ? (EAbilityKinds)a_oAbilityInfo[KCDefine.U_KEY_PREV_ABILITY_KINDS].AsInt : EAbilityKinds.NONE;
		m_eNextAbilityKinds = a_oAbilityInfo[KCDefine.U_KEY_NEXT_ABILITY_KINDS].ExIsValid() ? (EAbilityKinds)a_oAbilityInfo[KCDefine.U_KEY_NEXT_ABILITY_KINDS].AsInt : EAbilityKinds.NONE;

		m_oExtraAbilityTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oAbilityInfo[string.Format(KCDefine.U_KEY_FMT_EXTRA_ABILITY_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oExtraAbilityTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}
	}
	#endregion			// 함수
}

/** 어빌리티 강화 정보 */
[System.Serializable]
public partial struct STAbilityEnhanceInfo {
	public STCommonInfo m_stCommonInfo;

	public EAbilityKinds m_eAbilityKinds;
	public EAbilityKinds m_ePrevAbilityKinds;
	public EAbilityKinds m_eNextAbilityKinds;
	
	public Dictionary<ulong, STTargetInfo> m_oPayTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAcquireTargetInfoDict;

	#region 상수
	public static STAbilityEnhanceInfo INVALID = new STAbilityEnhanceInfo() {
		m_eAbilityKinds = EAbilityKinds.NONE, m_ePrevAbilityKinds = EAbilityKinds.NONE, m_eNextAbilityKinds = EAbilityKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public EAbilityType AbilityType => (EAbilityType)((int)m_eAbilityKinds).ExKindsToType();
	public EAbilityKinds BaseAbilityKinds => (EAbilityKinds)((int)m_eAbilityKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STAbilityEnhanceInfo(SimpleJSON.JSONNode a_oAbilityInfo) {
		m_stCommonInfo = new STCommonInfo(a_oAbilityInfo);

		m_eAbilityKinds = a_oAbilityInfo[KCDefine.U_KEY_ABILITY_KINDS].ExIsValid() ? (EAbilityKinds)a_oAbilityInfo[KCDefine.U_KEY_ABILITY_KINDS].AsInt : EAbilityKinds.NONE;
		m_ePrevAbilityKinds = a_oAbilityInfo[KCDefine.U_KEY_PREV_ABILITY_KINDS].ExIsValid() ? (EAbilityKinds)a_oAbilityInfo[KCDefine.U_KEY_PREV_ABILITY_KINDS].AsInt : EAbilityKinds.NONE;
		m_eNextAbilityKinds = a_oAbilityInfo[KCDefine.U_KEY_NEXT_ABILITY_KINDS].ExIsValid() ? (EAbilityKinds)a_oAbilityInfo[KCDefine.U_KEY_NEXT_ABILITY_KINDS].AsInt : EAbilityKinds.NONE;

		m_oPayTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oAcquireTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oAbilityInfo[string.Format(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oPayTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oAbilityInfo[string.Format(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oAcquireTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}
	}
	#endregion			// 함수
}

/** 어빌리티 정보 테이블 */
public partial class CAbilityInfoTable : CSingleton<CAbilityInfoTable> {
	#region 프로퍼티
	public Dictionary<EAbilityKinds, STAbilityInfo> AbilityInfoDict { get; private set; } = new Dictionary<EAbilityKinds, STAbilityInfo>();
	public Dictionary<EAbilityKinds, STAbilityEnhanceInfo> AbilityEnhanceInfoDict { get; private set; } = new Dictionary<EAbilityKinds, STAbilityEnhanceInfo>();

	private string AbilityInfoTablePath {
		get {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_ABILITY_INFO;
#else
			return KCDefine.U_TABLE_P_G_ABILITY_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetAbilityInfos();
	}

	/** 어빌리티 정보를 리셋한다 */
	public void ResetAbilityInfos() {
		this.AbilityInfoDict.Clear();
		this.AbilityEnhanceInfoDict.Clear();
	}

	/** 어빌리티 정보를 리셋한다 */
	public void ResetAbilityInfos(string a_oJSONStr) {
		this.ResetAbilityInfos();
		this.DoLoadAbilityInfos(a_oJSONStr);
	}

	/** 어빌리티 정보를 반환한다 */
	public STAbilityInfo GetAbilityInfo(EAbilityKinds a_eAbilityKinds) {
		bool bIsValid = this.TryGetAbilityInfo(a_eAbilityKinds, out STAbilityInfo stAbilityInfo);
		CAccess.Assert(bIsValid);

		return stAbilityInfo;
	}

	/** 어빌리티 강화 정보를 반환한다 */
	public STAbilityEnhanceInfo GetAbilityEnhanceInfo(EAbilityKinds a_eAbilityKinds) {
		bool bIsValid = this.TryGetAbilityEnhanceInfo(a_eAbilityKinds, out STAbilityEnhanceInfo stAbilityEnhanceInfo);
		CAccess.Assert(bIsValid);

		return stAbilityEnhanceInfo;
	}

	/** 어빌리티 정보를 반환한다 */
	public bool TryGetAbilityInfo(EAbilityKinds a_eAbilityKinds, out STAbilityInfo a_stOutAbilityInfo) {
		a_stOutAbilityInfo = this.AbilityInfoDict.GetValueOrDefault(a_eAbilityKinds, STAbilityInfo.INVALID);
		return this.AbilityInfoDict.ContainsKey(a_eAbilityKinds);
	}

	/** 어빌리티 강화 정보를 반환한다 */
	public bool TryGetAbilityEnhanceInfo(EAbilityKinds a_eAbilityKinds, out STAbilityEnhanceInfo a_stOutAbilityEnhanceInfo) {
		a_stOutAbilityEnhanceInfo = this.AbilityEnhanceInfoDict.GetValueOrDefault(a_eAbilityKinds, STAbilityEnhanceInfo.INVALID);
		return this.AbilityEnhanceInfoDict.ContainsKey(a_eAbilityKinds);
	}

	/** 어빌리티 정보를 로드한다 */
	public Dictionary<EAbilityKinds, STAbilityInfo> LoadAbilityInfos() {
		this.ResetAbilityInfos();
		return this.LoadAbilityInfos(this.AbilityInfoTablePath);
	}

	/** 어빌리티 정보를 로드한다 */
	private Dictionary<EAbilityKinds, STAbilityInfo> LoadAbilityInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadAbilityInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			return this.DoLoadAbilityInfos(CResManager.Inst.GetRes<TextAsset>(a_oFilePath).text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 어빌리티 정보를 로드한다 */
	private Dictionary<EAbilityKinds, STAbilityInfo> DoLoadAbilityInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());

		var oJSONNode = SimpleJSON.JSONNode.Parse(a_oJSONStr);
		var oAbilityInfosList = new List<SimpleJSON.JSONNode>();
		var oAbilityEnhanceInfosList = new List<SimpleJSON.JSONNode>();

		for(int i = 0; i < KDefine.G_KEY_ABILITY_IT_INFOS_LIST.Count; ++i) {
			oAbilityInfosList.ExAddVal(oJSONNode[KDefine.G_KEY_ABILITY_IT_INFOS_LIST[i]]);
		}

		for(int i = 0; i < KDefine.G_KEY_ABILITY_IT_ENHANCE_INFOS_LIST.Count; ++i) {
			oAbilityEnhanceInfosList.ExAddVal(oJSONNode[KDefine.G_KEY_ABILITY_IT_ENHANCE_INFOS_LIST[i]]);
		}
		
		for(int i = 0; i < oAbilityInfosList.Count; ++i) {
			for(int j = 0; j < oAbilityInfosList[i].Count; ++j) {
				var stAbilityInfo = new STAbilityInfo(oAbilityInfosList[i][j]);

				// 어빌리티 정보 추가 가능 할 경우
				if(stAbilityInfo.m_eAbilityKinds.ExIsValid() && (!this.AbilityInfoDict.ContainsKey(stAbilityInfo.m_eAbilityKinds) || oAbilityInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.AbilityInfoDict.ExReplaceVal(stAbilityInfo.m_eAbilityKinds, stAbilityInfo);
				}
			}
		}

		for(int i = 0; i < oAbilityEnhanceInfosList.Count; ++i) {
			for(int j = 0; j < oAbilityEnhanceInfosList[i].Count; ++j) {
				var stAbilityEnhanceInfo = new STAbilityEnhanceInfo(oAbilityEnhanceInfosList[i][j]);

				// 어빌리티 강화 정보 추가 가능 할 경우
				if(stAbilityEnhanceInfo.m_eAbilityKinds.ExIsValid() && (!this.AbilityEnhanceInfoDict.ContainsKey(stAbilityEnhanceInfo.m_eAbilityKinds) || oAbilityEnhanceInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.AbilityEnhanceInfoDict.ExReplaceVal(stAbilityEnhanceInfo.m_eAbilityKinds, stAbilityEnhanceInfo);
				}
			}
		}

		return this.AbilityInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
