using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 미션 정보
[System.Serializable]
public struct STMissionInfo {
	public string m_oName;
	public string m_oDesc;

	public EMissionKinds m_eMissionKinds;
	public ERewardKinds m_eRewardKinds;

	#region 함수
	//! 생성자
	public STMissionInfo(SimpleJSON.JSONNode a_oMissionInfo) {
		m_oName = a_oMissionInfo[KCDefine.U_KEY_NAME];
		m_oDesc = a_oMissionInfo[KCDefine.U_KEY_DESC];

		m_eMissionKinds = (EMissionKinds)a_oMissionInfo[KCDefine.U_KEY_MISSION_KINDS].AsInt;
		m_eRewardKinds = (ERewardKinds)a_oMissionInfo[KCDefine.U_KEY_REWARD_KINDS].AsInt;
	}
	#endregion			// 함수
}

//! 미션 정보 테이블
public class CMissionInfoTable : CScriptableObj<CMissionInfoTable> {
	#region 변수
	[Header("Free Mission Info")]
	[SerializeField] private List<STMissionInfo> m_oFreeMissionInfoList = new List<STMissionInfo>();

	[Header("Daily Mission Info")]
	[SerializeField] private List<STMissionInfo> m_oDailyMissionInfoList = new List<STMissionInfo>();

	[Header("Event Mission Info")]
	[SerializeField] private List<STMissionInfo> m_oEventMissionInfoList = new List<STMissionInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<EMissionKinds, STMissionInfo> MissionInfoDict { get; private set; } = new Dictionary<EMissionKinds, STMissionInfo>();
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		var oMissionInfoList = new List<STMissionInfo>(m_oFreeMissionInfoList);
		oMissionInfoList.AddRange(m_oDailyMissionInfoList);
		oMissionInfoList.AddRange(m_oEventMissionInfoList);

		for(int i = 0; i < oMissionInfoList.Count; ++i) {
			this.MissionInfoDict.ExAddVal(oMissionInfoList[i].m_eMissionKinds, oMissionInfoList[i]);
		}
	}

	//! 미션 정보를 반환한다
	public STMissionInfo GetMissionInfo(EMissionKinds a_eMissionKinds) {
		bool bIsValid = this.TryGetMissionInfo(a_eMissionKinds, out STMissionInfo stMissionInfo);
		CAccess.Assert(bIsValid);

		return stMissionInfo;
	}

	//! 미션 정보를 반환한다
	public bool TryGetMissionInfo(EMissionKinds a_eMissionKinds, out STMissionInfo a_stOutMissionInfo) {
		a_stOutMissionInfo = this.MissionInfoDict.ExGetVal(a_eMissionKinds, KDefine.G_INVALID_MISSION_INFO);
		return this.MissionInfoDict.ContainsKey(a_eMissionKinds);
	}

	//! 미션 정보를 로드한다
	public Dictionary<EMissionKinds, STMissionInfo> LoadMissionInfos() {
#if UNITY_EDITOR || UNITY_STANDALONE
		return this.LoadMissionInfos(KCDefine.U_RUNTIME_TABLE_P_G_MISSION_INFO);
#else
		return this.LoadMissionInfos(KCDefine.U_TABLE_P_G_MISSION_INFO);
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	}

	//! 미션 정보를 로드한다
	public Dictionary<EMissionKinds, STMissionInfo> LoadMissionInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_EDITOR || UNITY_STANDALONE
		string oJSONStr = CFunc.ReadStr(a_oFilePath);
		return this.DoLoadMissionInfos(oJSONStr);
#else
		try {
			var oTextAsset = CResManager.Inst.GetRes<TextAsset>(a_oFilePath);
			return this.DoLoadMissionInfos(oTextAsset.text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	}

	//! 미션 정보를 로드한다
	private Dictionary<EMissionKinds, STMissionInfo> DoLoadMissionInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr) as SimpleJSON.JSONClass;

		var oMissionInfosList = new List<SimpleJSON.JSONNode>() {
			oJSONNode[KCDefine.U_KEY_FREE],
			oJSONNode[KCDefine.U_KEY_DAILY],
			oJSONNode[KCDefine.U_KEY_EVENT]
		};

		for(int i = 0; i < oMissionInfosList.Count; ++i) {
			for(int j = 0; j < oMissionInfosList[i].Count; ++j) {
				var stMissionInfo = new STMissionInfo(oMissionInfosList[i][j]);
				bool bIsReplace = oMissionInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT;

				// 미션 정보가 추가 가능 할 경우
				if(bIsReplace || !this.MissionInfoDict.ContainsKey(stMissionInfo.m_eMissionKinds)) {
					this.MissionInfoDict.ExReplaceVal(stMissionInfo.m_eMissionKinds, stMissionInfo);
				}
			}
		}
		
		return this.MissionInfoDict;
	}
	#endregion			// 함수

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if NEVER_USE_THIS
