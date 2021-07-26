using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;

#if NEVER_USE_THIS
//! 셀 정보
[MessagePackObject]
[System.Serializable]
public sealed class CCellInfo : CBaseInfo, System.ICloneable {
	#region 변수
	[Key(101)] public HashSet<SampleEngineName.EBlockKinds> m_oBlockKindsList = new HashSet<SampleEngineName.EBlockKinds>();
	#endregion			// 변수

	#region 인터페이스
	//! 역직렬화 되었을 경우
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();
		m_oBlockKindsList = m_oBlockKindsList ?? new HashSet<SampleEngineName.EBlockKinds>();
	}
	#endregion			// 인터페이스

	#region 함수
	//! 생성자
	public CCellInfo() : base(KDefine.G_VER_CELL_INFO) {
		// Do Nothing
	}

	//! 사본 객체를 생성한다
	public object Clone() {
		var oCellInfo = new CCellInfo();
		oCellInfo.OnAfterDeserialize();

		return oCellInfo;
	}
	#endregion			// 함수
}

//! 레벨 정보
[MessagePackObject]
[System.Serializable]
public sealed class CLevelInfo : CBaseInfo, System.ICloneable {
	#region 변수
	[Key(5)] public STIDInfo m_stIDInfo;
	[Key(201)] public Dictionary<int, Dictionary<int, CCellInfo>> m_oCellInfoListContainer = new Dictionary<int, Dictionary<int, CCellInfo>>();
	#endregion			// 변수
	
	#region 프로퍼티
	[IgnoreMember] public Vector3Int NumCells { get; set; } = Vector3Int.zero;
	[IgnoreMember] public long LevelID => CFactory.MakeUniqueLevelID(m_stIDInfo.m_nID, m_stIDInfo.m_nStageID, m_stIDInfo.m_nChapterID);
	#endregion			// 프로퍼티

	#region 인터페이스
	//! 역직렬화 되었을 경우
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();
		m_oCellInfoListContainer = m_oCellInfoListContainer ?? new Dictionary<int, Dictionary<int, CCellInfo>>();

		// 셀 개수를 설정한다 {
		var stNumCells = new Vector3Int(KCDefine.B_VAL_0_INT, m_oCellInfoListContainer.Count, KCDefine.B_VAL_0_INT);

		for(int i = 0; i < m_oCellInfoListContainer.Count; ++i) {
			stNumCells.x = Mathf.Max(stNumCells.x, m_oCellInfoListContainer[i].Count);
		}

		this.NumCells = stNumCells;
		// 셀 개수를 설정한다 }
	}
	#endregion			// 인터페이스

	#region 함수
	//! 생성자
	public CLevelInfo() : base(KDefine.G_VER_LEVEL_INFO) {
		// Do Nothing
	}

	//! 사본 객체를 생성한다
	public object Clone() {
		var oLevelInfo = new CLevelInfo();
		oLevelInfo.m_stIDInfo = m_stIDInfo;

		foreach(var stKeyVal in m_oCellInfoListContainer) {
			var oCellInfoList = new Dictionary<int, CCellInfo>();

			foreach(var stCellInfoKeyVal in oCellInfoList) {
				var oCellInfo = stCellInfoKeyVal.Value.Clone() as CCellInfo;
				oCellInfoList.Add(stCellInfoKeyVal.Key, oCellInfo);
			}

			oLevelInfo.m_oCellInfoListContainer.Add(stKeyVal.Key, oCellInfoList);
		}

		oLevelInfo.OnAfterDeserialize();
		return oLevelInfo;
	}
	#endregion			// 함수
}

//! 레벨 정보 테이블
public class CLevelInfoTable : CSingleton<CLevelInfoTable> {
	#region 프로퍼티
	public Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>> LevelInfoListContainer = new Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>>();
	public int NumChapterInfos => this.LevelInfoListContainer.Count;
	#endregion			// 프로퍼티

	#region 함수
	//! 레벨 정보 개수를 반환한다
	public int GetNumLevelInfos(int a_nID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		CAccess.Assert(this.LevelInfoListContainer.ContainsKey(a_nChapterID) && this.LevelInfoListContainer[a_nChapterID].ContainsKey(a_nID));
		return this.LevelInfoListContainer[a_nChapterID][a_nID].Count;
	}

	//! 스테이지 정보 개수를 반화한다
	public int GetNumStageInfos(int a_nChapterID) {
		CAccess.Assert(this.LevelInfoListContainer.ContainsKey(a_nChapterID));
		return this.LevelInfoListContainer[a_nChapterID].Count;
	}
	
	//! 레벨 정보를 반환한다
	public CLevelInfo GetLevelInfo(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetLevelInfo(a_nID, out CLevelInfo oLevelInfo, a_nStageID, a_nChapterID);
		CAccess.Assert(bIsValid);

		return oLevelInfo;
	}
	
	//! 스테이지 레벨 정보를 반환한다
	public Dictionary<int, CLevelInfo> GetStageLevelInfos(int a_nID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetStageLevelInfos(a_nID, out Dictionary<int, CLevelInfo> oLevelInfoList, a_nChapterID);
		CAccess.Assert(bIsValid);

		return oLevelInfoList;
	}

	//! 챕터 레벨 정보를 반환한다
	public Dictionary<int, Dictionary<int, CLevelInfo>> GetChapterLevelInfos(int a_nID) {
		bool bIsValid = this.TryGetChapterLevelInfos(a_nID, out Dictionary<int, Dictionary<int, CLevelInfo>> oLevelInfoList);
		CAccess.Assert(bIsValid);

		return oLevelInfoList;
	}

	//! 레벨 정보를 반환한다
	public bool TryGetLevelInfo(int a_nID, out CLevelInfo a_oOutLevelInfo, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetStageLevelInfos(a_nStageID, out Dictionary<int, CLevelInfo> oStageLevelInfoList, a_nChapterID);
		a_oOutLevelInfo = oStageLevelInfoList?.ExGetVal(a_nID, null);

		return a_oOutLevelInfo != null;
	}

	//! 스테이지 레벨 정보를 반환한다
	public bool TryGetStageLevelInfos(int a_nID, out Dictionary<int, CLevelInfo> a_oOutStageLevelInfoList, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetChapterLevelInfos(a_nChapterID, out Dictionary<int, Dictionary<int, CLevelInfo>> oChapterLevelInfoList);
		a_oOutStageLevelInfoList = oChapterLevelInfoList?.ExGetVal(a_nID, null);

		return a_oOutStageLevelInfoList != null;
	}

	//! 챕터 레벨 정보를 반환한다
	public bool TryGetChapterLevelInfos(int a_nID, out Dictionary<int, Dictionary<int, CLevelInfo>> a_oOutChapterLevelInfoList) {
		a_oOutChapterLevelInfoList = this.LevelInfoListContainer.ExGetVal(a_nID, null);
		return a_oOutChapterLevelInfoList != null;
	}

	//! 레벨 정보를 로드한다
	public Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>> LoadLevelInfos() {
#if UNITY_EDITOR || UNITY_STANDALONE
		return this.LoadLevelInfos(KDefine.G_RUNTIME_TABLE_P_LEVEL_INFO);
#else
		return this.LoadLevelInfos(KCDefine.U_TABLE_P_G_LEVEL_INFO);
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	}

	//! 레벨 정보를 로드한다
	public Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>> LoadLevelInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if UNITY_EDITOR || UNITY_STANDALONE
		string oFilePath = a_oFilePath.ExGetReplaceStr(KCDefine.B_FILE_EXTENSION_BYTES, KCDefine.B_FILE_EXTENSION_JSON);
		var oLevelIDList = CFunc.ReadMsgPackJSONObj<List<long>>(oFilePath, false);

		CAccess.Assert(oLevelIDList != null);

		for(int i = 0; i < oLevelIDList.Count; ++i) {
			string oLevelInfoFilePath = string.Format(KDefine.G_RUNTIME_DATA_P_FMT_LEVEL_INFO, oLevelIDList[i] + KCDefine.B_VAL_1_INT);			
			var oLevelInfo = this.LoadLevelInfo(oLevelInfoFilePath);

			this.AddLevelInfo(oLevelInfo);
		}
#else
		try {
			this.LevelInfoListContainer = CFunc.ReadMsgPackObjFromRes<Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>>>(a_oFilePath, false);
			CAccess.Assert(this.LevelInfoListContainer != null);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE

		return this.LevelInfoListContainer;
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR || UNITY_STANDALONE
	//! 레벨 정보를 추가한다
	public void AddLevelInfo(CLevelInfo a_oLevelInfo) {
		CAccess.Assert(a_oLevelInfo != null);

		var oChapterLevelInfoList = this.LevelInfoListContainer.ExGetVal(a_oLevelInfo.m_stIDInfo.m_nChapterID, null);
		oChapterLevelInfoList = oChapterLevelInfoList ?? new Dictionary<int, Dictionary<int, CLevelInfo>>();

		var oStageLevelInfoList = oChapterLevelInfoList?.ExGetVal(a_oLevelInfo.m_stIDInfo.m_nStageID, null);
		oStageLevelInfoList = oStageLevelInfoList ?? new Dictionary<int, CLevelInfo>();

		oStageLevelInfoList.ExReplaceVal(a_oLevelInfo.m_stIDInfo.m_nID, a_oLevelInfo);
		oChapterLevelInfoList.ExReplaceVal(a_oLevelInfo.m_stIDInfo.m_nStageID, oStageLevelInfoList);
		this.LevelInfoListContainer.ExReplaceVal(a_oLevelInfo.m_stIDInfo.m_nChapterID, oChapterLevelInfoList);
	}

	//! 레벨 정보를 제거한다
	public void RemoveLevelInfo(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetStageLevelInfos(a_nStageID, out Dictionary<int, CLevelInfo> oStageLevelInfoList, a_nChapterID);
		CAccess.Assert(bIsValid && oStageLevelInfoList.ExIsValid());

		for(int i = a_nID + KCDefine.B_VAL_1_INT; i < oStageLevelInfoList.Count; ++i) {
			oStageLevelInfoList[i].m_stIDInfo.m_nID -= KCDefine.B_VAL_1_INT;
			oStageLevelInfoList.ExReplaceVal(i - KCDefine.B_VAL_1_INT, oStageLevelInfoList[i]);
		}

		oStageLevelInfoList.Remove(oStageLevelInfoList.Count - KCDefine.B_VAL_1_INT);
	}

	//! 스테이지 레벨 정보를 제거한다
	public void RemoveStageLevelInfos(int a_nID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetChapterLevelInfos(a_nChapterID, out Dictionary<int, Dictionary<int, CLevelInfo>> oChapterLevelInfoList);
		CAccess.Assert(bIsValid && oChapterLevelInfoList.ExIsValid());

		for(int i = a_nID + KCDefine.B_VAL_1_INT; i < oChapterLevelInfoList.Count; ++i) {
			foreach(var stKeyVal in oChapterLevelInfoList[i]) {
				stKeyVal.Value.m_stIDInfo.m_nStageID -= KCDefine.B_VAL_1_INT;
			}

			oChapterLevelInfoList.ExReplaceVal(i - KCDefine.B_VAL_1_INT, oChapterLevelInfoList[i]);
		}

		oChapterLevelInfoList.Remove(oChapterLevelInfoList.Count - KCDefine.B_VAL_1_INT);
	}

	//! 챕터 레벨 정보를 제거한다
	public void RemoveChapterLevelInfos(int a_nID) {
		CAccess.Assert(this.LevelInfoListContainer.ContainsKey(a_nID));

		for(int i = a_nID + KCDefine.B_VAL_1_INT; i < this.LevelInfoListContainer.Count; ++i) {
			foreach(var stKeyVal in this.LevelInfoListContainer[i]) {
				foreach(var stLevelKeyVal in stKeyVal.Value) {
					stLevelKeyVal.Value.m_stIDInfo.m_nChapterID -= KCDefine.B_VAL_1_INT;
				}
			}

			this.LevelInfoListContainer.ExReplaceVal(i - KCDefine.B_VAL_1_INT, this.LevelInfoListContainer[i]);
		}

		this.LevelInfoListContainer.Remove(this.LevelInfoListContainer.Count - KCDefine.B_VAL_1_INT);
	}

	//! 레벨 정보를 이동한다
	public void MoveLevelInfo(int a_nFromID, int a_nToID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetStageLevelInfos(a_nStageID, out Dictionary<int, CLevelInfo> oStageLevelInfoList, a_nChapterID);

		CAccess.Assert(bIsValid && oStageLevelInfoList.ExIsValid());
		CAccess.Assert(oStageLevelInfoList.ContainsKey(a_nFromID) && oStageLevelInfoList.ContainsKey(a_nToID));

		var oFromLevelInfo = oStageLevelInfoList[a_nFromID];
		int nOffset = (a_nFromID <= a_nToID) ? KCDefine.B_VAL_1_INT : -KCDefine.B_VAL_1_INT;

		oStageLevelInfoList.Remove(a_nFromID);

		for(int i = a_nFromID + nOffset; i != a_nToID + nOffset; i += nOffset) {
			oStageLevelInfoList[i].m_stIDInfo.m_nID -= nOffset;
			oStageLevelInfoList.ExReplaceVal(i - nOffset, oStageLevelInfoList[i]);
		}

		oFromLevelInfo.m_stIDInfo.m_nID = a_nToID;
		oStageLevelInfoList.ExReplaceVal(a_nToID, oFromLevelInfo);
	}

	//! 스테이지 레벨 정보를 이동한다
	public void MoveStageLevelInfos(int a_nFromID, int a_nToID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetChapterLevelInfos(a_nChapterID, out Dictionary<int, Dictionary<int, CLevelInfo>> oChapterLevelInfoList);

		CAccess.Assert(bIsValid && oChapterLevelInfoList.ExIsValid());
		CAccess.Assert(oChapterLevelInfoList.ContainsKey(a_nFromID) && oChapterLevelInfoList.ContainsKey(a_nToID));

		var oFromStageLevelInfoList = oChapterLevelInfoList[a_nFromID];
		int nOffset = (a_nFromID <= a_nToID) ? KCDefine.B_VAL_1_INT : -KCDefine.B_VAL_1_INT;

		oChapterLevelInfoList.Remove(a_nFromID);

		for(int i = a_nFromID + nOffset; i != a_nToID + nOffset; i += nOffset) {
			foreach(var stKeyVal in oChapterLevelInfoList[i]) {
				stKeyVal.Value.m_stIDInfo.m_nStageID -= nOffset;
			}

			oChapterLevelInfoList.ExReplaceVal(i - nOffset, oChapterLevelInfoList[i]);
		}

		foreach(var stKeyVal in oFromStageLevelInfoList) {
			stKeyVal.Value.m_stIDInfo.m_nStageID = a_nToID;
		}

		oChapterLevelInfoList.ExReplaceVal(a_nToID, oFromStageLevelInfoList);
	}

	//! 챕터 레벨 정보를 이동한다
	public void MoveChapterLevelInfos(int a_nFromID, int a_nToID) {
		CAccess.Assert(this.LevelInfoListContainer.ContainsKey(a_nFromID) && this.LevelInfoListContainer.ContainsKey(a_nToID));

		var oFromChapterLevelInfoList = this.LevelInfoListContainer[a_nFromID];
		int nOffset = (a_nFromID <= a_nToID) ? KCDefine.B_VAL_1_INT : -KCDefine.B_VAL_1_INT;

		for(int i = a_nFromID + nOffset; i != a_nToID + nOffset; i += nOffset) {
			foreach(var stKeyVal in this.LevelInfoListContainer[i]) {
				foreach(var stLevelKeyVal in stKeyVal.Value) {
					stLevelKeyVal.Value.m_stIDInfo.m_nChapterID -= nOffset;
				}
			}

			this.LevelInfoListContainer.ExReplaceVal(i - nOffset, this.LevelInfoListContainer[i]);
		}

		foreach(var stKeyVal in oFromChapterLevelInfoList) {
			foreach(var stLevelKeyVal in stKeyVal.Value) {
				stLevelKeyVal.Value.m_stIDInfo.m_nChapterID = a_nToID;
			}
		}

		this.LevelInfoListContainer.ExReplaceVal(a_nToID, oFromChapterLevelInfoList);
	}

	//! 레벨 정보를 저장한다
	public void SaveLevelInfos() {
		var oLevelIDList = new List<long>();
		string oFilePath = KDefine.G_RUNTIME_TABLE_P_LEVEL_INFO.ExGetReplaceStr(KCDefine.B_FILE_EXTENSION_BYTES, KCDefine.B_FILE_EXTENSION_JSON);

		foreach(var stKeyVal in this.LevelInfoListContainer) {
			foreach(var stStageKeyVal in stKeyVal.Value) {
				foreach(var stLevelKeyVal in stStageKeyVal.Value) {
					this.SaveLevelInfo(stLevelKeyVal.Value, oLevelIDList);
				}
			}
		}

		CFunc.WriteMsgPackJSONObj(oFilePath, oLevelIDList, false, false);
		CFunc.WriteMsgPackObj(KDefine.G_RUNTIME_TABLE_P_LEVEL_INFO, this.LevelInfoListContainer, false, false);
	}

	//! 레벨 정보를 저장한다
	private void SaveLevelInfo(CLevelInfo a_oLevelInfo, List<long> a_oOutLevelIDList) {
		CAccess.Assert(a_oLevelInfo != null);
		string oFilePath = string.Format(KDefine.G_RUNTIME_DATA_P_FMT_LEVEL_INFO, a_oLevelInfo.LevelID + KCDefine.B_VAL_1_INT);

		a_oOutLevelIDList.Add(a_oLevelInfo.LevelID);
		CFunc.WriteMsgPackObj(oFilePath, a_oLevelInfo, false, false);
	}

	//! 레벨 정보를 로드한다
	private CLevelInfo LoadLevelInfo(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		CFunc.ShowLog($"CLevelInfoTable.LoadLevelInfo: {a_oFilePath}");

		return CFunc.ReadMsgPackObj<CLevelInfo>(a_oFilePath, false);
	}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	#endregion			// 조건부 함수
}
#endif			// #if NEVER_USE_THIS
