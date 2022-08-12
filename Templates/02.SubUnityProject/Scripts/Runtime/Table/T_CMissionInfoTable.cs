#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 미션 정보 */
[System.Serializable]
public partial struct STMissionInfo {
	public STCommonInfo m_stCommonInfo;

	public EMissionKinds m_eMissionKinds;
	public EMissionKinds m_ePrevMissionKinds;
	public EMissionKinds m_eNextMissionKinds;

	public List<ERewardKinds> m_oRewardKindsList;

	#region 상수
	public static STMissionInfo INVALID = new STMissionInfo() {
		m_eMissionKinds = EMissionKinds.NONE, m_ePrevMissionKinds = EMissionKinds.NONE, m_eNextMissionKinds = EMissionKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public EMissionType MissionType => (EMissionType)((int)m_eMissionKinds).ExKindsToType();
	public EMissionKinds BaseMissionKinds => (EMissionKinds)((int)m_eMissionKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STMissionInfo(SimpleJSON.JSONNode a_oMissionInfo) {
		m_stCommonInfo = new STCommonInfo(a_oMissionInfo);
		
		m_eMissionKinds = a_oMissionInfo[KCDefine.U_KEY_MISSION_KINDS].ExIsValid() ? (EMissionKinds)a_oMissionInfo[KCDefine.U_KEY_MISSION_KINDS].AsInt : EMissionKinds.NONE;
		m_ePrevMissionKinds = a_oMissionInfo[KCDefine.U_KEY_PREV_MISSION_KINDS].ExIsValid() ? (EMissionKinds)a_oMissionInfo[KCDefine.U_KEY_PREV_MISSION_KINDS].AsInt : EMissionKinds.NONE;
		m_eNextMissionKinds = a_oMissionInfo[KCDefine.U_KEY_NEXT_MISSION_KINDS].ExIsValid() ? (EMissionKinds)a_oMissionInfo[KCDefine.U_KEY_NEXT_MISSION_KINDS].AsInt : EMissionKinds.NONE;

		m_oRewardKindsList = new List<ERewardKinds>();

		for(int i = 0; i < KDefine.G_MAX_NUM_REWARD_KINDS; ++i) {
			string oKey = string.Format(KCDefine.U_KEY_FMT_REWARD_KINDS, i + KCDefine.B_VAL_1_INT);
			if(a_oMissionInfo[oKey].ExIsValid()) { m_oRewardKindsList.ExAddVal((ERewardKinds)a_oMissionInfo[oKey].AsInt); }
		}
	}
	#endregion			// 함수
}

/** 미션 정보 테이블 */
public partial class CMissionInfoTable : CSingleton<CMissionInfoTable> {
	#region 프로퍼티
	public Dictionary<EMissionKinds, STMissionInfo> MissionInfoDict { get; private set; } = new Dictionary<EMissionKinds, STMissionInfo>();

	private string MissionInfoTablePath {
		get {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_MISSION_INFO;
#else
			return KCDefine.U_TABLE_P_G_MISSION_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetMissionInfos();
	}

	/** 미션 정보를 리셋한다 */
	public void ResetMissionInfos() {
		this.MissionInfoDict.Clear();
	}

	/** 미션 정보를 리셋한다 */
	public void ResetMissionInfos(string a_oJSONStr) {
		this.ResetMissionInfos();
		this.DoLoadMissionInfos(a_oJSONStr);
	}

	/** 미션 정보를 반환한다 */
	public STMissionInfo GetMissionInfo(EMissionKinds a_eMissionKinds) {
		bool bIsValid = this.TryGetMissionInfo(a_eMissionKinds, out STMissionInfo stMissionInfo);
		CAccess.Assert(bIsValid);

		return stMissionInfo;
	}

	/** 미션 정보를 반환한다 */
	public bool TryGetMissionInfo(EMissionKinds a_eMissionKinds, out STMissionInfo a_stOutMissionInfo) {
		a_stOutMissionInfo = this.MissionInfoDict.GetValueOrDefault(a_eMissionKinds, STMissionInfo.INVALID);
		return this.MissionInfoDict.ContainsKey(a_eMissionKinds);
	}

	/** 미션 정보를 로드한다 */
	public Dictionary<EMissionKinds, STMissionInfo> LoadMissionInfos() {
		this.ResetMissionInfos();
		return this.LoadMissionInfos(this.MissionInfoTablePath);
	}

	/** 미션 정보를 로드한다 */
	private Dictionary<EMissionKinds, STMissionInfo> LoadMissionInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadMissionInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			return this.DoLoadMissionInfos(CResManager.Inst.GetRes<TextAsset>(a_oFilePath).text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 미션 정보를 로드한다 */
	private Dictionary<EMissionKinds, STMissionInfo> DoLoadMissionInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());

		var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr);
		var oMissionInfosList = new List<SimpleJSON.JSONNode>();

		for(int i = 0; i < KDefine.G_KEY_MISSION_IT_INFOS_LIST.Count; ++i) {
			oMissionInfosList.ExAddVal(oJSONNode[KDefine.G_KEY_MISSION_IT_INFOS_LIST[i]]);
		}
		
		for(int i = 0; i < oMissionInfosList.Count; ++i) {
			for(int j = 0; j < oMissionInfosList[i].Count; ++j) {
				var stMissionInfo = new STMissionInfo(oMissionInfosList[i][j]);

				// 미션 정보 추가 가능 할 경우
				if(stMissionInfo.m_eMissionKinds.ExIsValid() && (!this.MissionInfoDict.ContainsKey(stMissionInfo.m_eMissionKinds) || oMissionInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.MissionInfoDict.ExReplaceVal(stMissionInfo.m_eMissionKinds, stMissionInfo);
				}
			}
		}
		
		return this.MissionInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
