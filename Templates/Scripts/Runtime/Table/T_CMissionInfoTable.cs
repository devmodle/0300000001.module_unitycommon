using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 미션 정보
[System.Serializable]
public struct STMissionInfo {
	public string m_oName;
	public string m_oDesc;
	
	public EMissionType m_eMissionType;
	public EMissionKinds m_eMissionKinds;

	#region 함수
	//! 생성자
	public STMissionInfo(SimpleJSON.JSONNode a_oMissionInfo) {
		m_oName = a_oMissionInfo[KDefine.G_KEY_MISSION_IT_NAME];
		m_oDesc = a_oMissionInfo[KDefine.G_KEY_MISSION_IT_DESC];

		m_eMissionType = (EMissionType)a_oMissionInfo[KDefine.G_KEY_MISSION_IT_MISSION_TYPE].AsInt;
		m_eMissionKinds = (EMissionKinds)a_oMissionInfo[KDefine.G_KEY_MISSION_IT_MISSION_KINDS].AsInt;
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
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<EMissionKinds, STMissionInfo> FreeMissionInfoList { get; set; } = new Dictionary<EMissionKinds, STMissionInfo>();
	public Dictionary<EMissionKinds, STMissionInfo> DailyMissionInfoList { get; set; } = new Dictionary<EMissionKinds, STMissionInfo>();
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		this.SetupMissionInfos(m_oFreeMissionInfoList, this.FreeMissionInfoList);
		this.SetupMissionInfos(m_oDailyMissionInfoList, this.DailyMissionInfoList);
	}

	//! 자유 미션 정보를 반환한다
	public STMissionInfo GetFreeMissionInfo(EMissionKinds a_eMissionKinds) {
		return this.GetMissionInfo(a_eMissionKinds, this.FreeMissionInfoList);
	}

	//! 일일 미션 정보를 반환한다
	public STMissionInfo GetDailyMissionInfo(EMissionKinds a_eMissionKinds) {
		return this.GetMissionInfo(a_eMissionKinds, this.DailyMissionInfoList);
	}

	//! 자유 미션 정보를 반환한다
	public bool TryGetFreeMissionInfo(EMissionKinds a_eMissionKinds, out STMissionInfo a_stOutFreeMissionInfo) {
		return this.TryGetMissionInfo(a_eMissionKinds, this.FreeMissionInfoList, out a_stOutFreeMissionInfo);
	}

	//! 일일 미션 정보를 반환한다
	public bool TryGetDailyMissionInfo(EMissionKinds a_eMissionKinds, out STMissionInfo a_stOutDailyMissionInfo) {
		return this.TryGetMissionInfo(a_eMissionKinds, this.DailyMissionInfoList, out a_stOutDailyMissionInfo);
	}

	//! 미션 정보를 로드한다
	public List<object> LoadMissionInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		
		var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr) as SimpleJSON.JSONClass;
		var oFreeMissionInfos = oJSONNode[KDefine.G_KEY_MISSION_IT_FREE];
		var oDailyMissionInfos = oJSONNode[KDefine.G_KEY_MISSION_IT_DAILY];

		this.LoadMissionInfos(oFreeMissionInfos, this.FreeMissionInfoList);
		this.LoadMissionInfos(oDailyMissionInfos, this.DailyMissionInfoList);

#if UNITY_EDITOR
		this.SetupMissionInfoList(this.FreeMissionInfoList, m_oFreeMissionInfoList);
		this.SetupMissionInfoList(this.DailyMissionInfoList, m_oDailyMissionInfoList);
#endif			// #if UNITY_EDITOR

		return new List<object>() {
			this.FreeMissionInfoList, this.DailyMissionInfoList
		};
	}

	//! 미션 정보를 로드한다
	public List<object> LoadMissionInfosFromFile(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		string oJSONStr = CFunc.ReadStr(a_oFilePath);

		return this.LoadMissionInfos(oJSONStr);
	}

	//! 미션 정보를 로드한다
	public List<object> LoadMissionInfosFromRes(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

		try {
			var oTextAsset = CResManager.Inst.GetRes<TextAsset>(a_oFilePath);
			return this.LoadMissionInfos(oTextAsset.text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
	}

	//! 미션 정보를 설정한다
	private void SetupMissionInfos(List<STMissionInfo> a_oMissionInfoList, Dictionary<EMissionKinds, STMissionInfo> a_oOutMissionInfoList) {
		CAccess.Assert(a_oMissionInfoList != null && a_oOutMissionInfoList != null);

		for(int i = 0; i < a_oMissionInfoList.Count; ++i) {
			var stMissionInfo = a_oMissionInfoList[i];
			a_oOutMissionInfoList.ExAddVal(stMissionInfo.m_eMissionKinds, stMissionInfo);
		}
	}

	//! 미션 정보를 반환한다
	private STMissionInfo GetMissionInfo(EMissionKinds a_eMissionKinds, Dictionary<EMissionKinds, STMissionInfo> a_oMissionInfoList) {
		bool bIsValid = this.TryGetMissionInfo(a_eMissionKinds, a_oMissionInfoList, out STMissionInfo stMissionInfo);
		CAccess.Assert(bIsValid);

		return stMissionInfo;
	}

	//! 미션 정보를 반환한다
	private bool TryGetMissionInfo(EMissionKinds a_eMissionKinds, Dictionary<EMissionKinds, STMissionInfo> a_oMissionInfoList, out STMissionInfo a_stOutMissionInfo) {
		CAccess.Assert(a_oMissionInfoList != null);
		a_stOutMissionInfo = a_oMissionInfoList.ExGetVal(a_eMissionKinds, KDefine.G_INVALID_MISSION_INFO);

		return a_oMissionInfoList.ContainsKey(a_eMissionKinds);
	}

	//! 미션 정보를 로드한다
	private Dictionary<EMissionKinds, STMissionInfo> LoadMissionInfos(SimpleJSON.JSONNode a_oMissionInfos, Dictionary<EMissionKinds, STMissionInfo> a_oOutMissionInfoList) {
		CAccess.Assert(a_oMissionInfos != null && a_oOutMissionInfoList != null);

		for(int i = 0; i < a_oMissionInfos.Count; ++i) {
			var stMissionInfo = new STMissionInfo(a_oMissionInfos[i]);
			bool bIsReplace = a_oMissionInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT;

			// 미션 정보가 추가 가능 할 경우
			if(bIsReplace || !a_oOutMissionInfoList.ContainsKey(stMissionInfo.m_eMissionKinds)) {
				a_oOutMissionInfoList.ExReplaceVal(stMissionInfo.m_eMissionKinds, stMissionInfo);
			}
		}

		return a_oOutMissionInfoList;
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR
		// 미션 정보를 설정한다
		private void SetupMissionInfoList(Dictionary<EMissionKinds, STMissionInfo> a_oMissionInfoList, List<STMissionInfo> a_oOutMissionInfoList) {
			CAccess.Assert(a_oMissionInfoList != null && a_oOutMissionInfoList != null);
			a_oOutMissionInfoList.Clear();

			foreach(var stKeyVal in a_oMissionInfoList) {
				a_oOutMissionInfoList.ExAddVal(stKeyVal.Value);
			}

			a_oOutMissionInfoList.Sort((a_stLhs, a_stRhs) => (int)a_stLhs.m_eMissionKinds - (int)a_stRhs.m_eMissionKinds);
		}
#endif			// #if UNITY_EDITOR
	#endregion			// 조건부 함수
}
#endif			// #if NEVER_USE_THIS
