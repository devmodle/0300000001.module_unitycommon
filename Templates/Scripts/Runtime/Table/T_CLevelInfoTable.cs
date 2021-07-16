using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;

#if NEVER_USE_THIS
//! 셀 정보
[MessagePackObject]
[System.Serializable]
public sealed class CCellInfo : CBaseInfo {
	#region 함수
	//! 생성자
	public CCellInfo() : base(KDefine.G_VER_CELL_INFO) {
		// Do Nothing
	}
	#endregion			// 함수
}

//! 레벨 정보
[MessagePackObject]
[System.Serializable]
public sealed class CLevelInfo : CBaseInfo {
	#region 상수
	private const string KEY_ID = "ID";
	private const string KEY_STAGE_ID = "StageID";
	private const string KEY_CHAPTER_ID = "ChapterID";
	#endregion			// 상수

	#region 변수
	[Key(61)] public List<List<CCellInfo>> m_oCellInfoListContainer = new List<List<CCellInfo>>();
	#endregion			// 변수
	
	#region 프로퍼티
	[IgnoreMember] public Vector3Int NumCells { get; private set; } = Vector3Int.zero;

	[IgnoreMember] public int ID {
		get { return m_oIntList.ExGetVal(CLevelInfo.KEY_ID, KCDefine.B_VAL_0_INT); }
		set { m_oIntList.ExReplaceVal(CLevelInfo.KEY_ID, value); }
	}

	[IgnoreMember] public int StageID {
		get { return m_oIntList.ExGetVal(CLevelInfo.KEY_STAGE_ID, KCDefine.B_VAL_0_INT); }
		set { m_oIntList.ExReplaceVal(CLevelInfo.KEY_STAGE_ID, value); }
	}

	[IgnoreMember] public int ChapterID {
		get { return m_oIntList.ExGetVal(CLevelInfo.KEY_CHAPTER_ID, KCDefine.B_VAL_0_INT); }
		set { m_oIntList.ExReplaceVal(CLevelInfo.KEY_CHAPTER_ID, value); }
	}

	[IgnoreMember] public long LevelID => Factory.MakeUniqueLevelID(this.ID, this.StageID, this.ChapterID);
	#endregion			// 프로퍼티

	#region 인터페이스
	//! 역직렬화 되었을 경우
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();
		m_oCellInfoListContainer = m_oCellInfoListContainer ?? new List<List<CCellInfo>>();

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
	#endregion			// 함수
}

//! 레벨 정보 테이블
public class CLevelInfoTable : CSingleton<CLevelInfoTable> {
	#region 프로퍼티
	public Dictionary<long, CLevelInfo> LevelInfoList { get; private set; } = new Dictionary<long, CLevelInfo>();

	public int NumChapterInfos {
		get {
			var oChapterIDList = new HashSet<int>();

			foreach(var stKeyVal in this.LevelInfoList) {
				oChapterIDList.Add(stKeyVal.Value.ChapterID);
			}

			return oChapterIDList.Count;
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	//! 스테이지 정보 개수를 반화한다
	public int GetNumStageInfos(int a_nChapterID) {
		var oStageIDList = new HashSet<int>();
		var oLevelInfoList = this.GetChapterLevelInfos(a_nChapterID);

		for(int i = 0; i < oLevelInfoList.Count; ++i) {
			oStageIDList.Add(oLevelInfoList[i].StageID);
		}

		return oStageIDList.Count;
	}
	
	//! 레벨 정보를 반환한다
	public CLevelInfo GetLevelInfo(long a_nID) {
		bool bIsValid = this.TryGetLevelInfo(a_nID, out CLevelInfo oLevelInfo);
		CAccess.Assert(bIsValid);

		return oLevelInfo;
	}
	
	//! 스테이지 레벨 정보를 반환한다
	public List<CLevelInfo> GetStageLevelInfos(int a_nStageID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		var oLevelInfoList = new List<CLevelInfo>();

		foreach(var stKeyVal in this.LevelInfoList) {
			// 스테이지, 챕터가 동일 할 경우
			if(stKeyVal.Value.StageID == a_nStageID && stKeyVal.Value.ChapterID == a_nChapterID) {
				oLevelInfoList.Add(stKeyVal.Value);
			}
		}

		oLevelInfoList.Sort((a_oLhs, a_oRhs) => (int)(a_oLhs.LevelID - a_oRhs.LevelID));
		return oLevelInfoList;
	}

	//! 챕터 레벨 정보를 반환한다
	public List<CLevelInfo> GetChapterLevelInfos(int a_nChapterID) {
		var oLevelInfoList = new List<CLevelInfo>();

		foreach(var stKeyVal in this.LevelInfoList) {
			// 챕터가 동일 할 경우
			if(stKeyVal.Value.ChapterID == a_nChapterID) {
				oLevelInfoList.Add(stKeyVal.Value);
			}
		}

		oLevelInfoList.Sort((a_oLhs, a_oRhs) => (int)(a_oLhs.LevelID - a_oRhs.LevelID));
		return oLevelInfoList;
	}

	//! 레벨 정보를 반환한다
	public bool TryGetLevelInfo(long a_nID, out CLevelInfo a_oOutLevelInfo) {
		a_oOutLevelInfo = this.LevelInfoList.ExGetVal(a_nID, null);
		return this.LevelInfoList.ContainsKey(a_nID);
	}

	//! 레벨 정보를 로드한다
	public Dictionary<long, CLevelInfo> LoadLevelInfos() {
#if UNITY_EDITOR || UNITY_STANDALONE
		return this.LoadLevelInfos(KDefine.G_RUNTIME_TABLE_P_LEVEL_INFO);
#else
		return this.LoadLevelInfos(KCDefine.U_TABLE_P_G_LEVEL_INFO);
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	}

	//! 레벨 정보를 로드한다
	public Dictionary<long, CLevelInfo> LoadLevelInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if UNITY_EDITOR || UNITY_STANDALONE
		string oFilePath = a_oFilePath.ExGetReplaceStr(KCDefine.B_FILE_EXTENSION_BYTES, KCDefine.B_FILE_EXTENSION_TXT);
		string oStr = CFunc.ReadStr(oFilePath);

		bool bIsValid = int.TryParse(oStr, out int nNumLevelInfos);
		CAccess.Assert(bIsValid);

		for(int i = 0; i < nNumLevelInfos; ++i) {
			string oLevelInfoFilePath = string.Format(KDefine.G_RUNTIME_DATA_P_FMT_LEVEL_INFO, i + KCDefine.B_VAL_1_INT);
			CFunc.ShowLog($"CLevelInfoTable.LoadLevelInfos: {oLevelInfoFilePath}");
			
			var oLevelInfo = this.LoadLevelInfo(oLevelInfoFilePath);
			this.LevelInfoList.ExAddVal(oLevelInfo.LevelID, oLevelInfo);
		}
#else
		try {
			this.LevelInfoList = CFunc.ReadMsgPackObjFromRes<Dictionary<long, CLevelInfo>>(a_oFilePath, false);
			CAccess.Assert(this.LevelInfoList != null);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE

		return this.LevelInfoList;
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR || UNITY_STANDALONE
	//! 레벨 정보를 추가한다
	public void AddLevelInfo(CLevelInfo a_oLevelInfo) {
		CAccess.Assert(a_oLevelInfo != null && !this.LevelInfoList.ContainsKey(a_oLevelInfo.LevelID));
		this.LevelInfoList.Add(a_oLevelInfo.LevelID, a_oLevelInfo);
	}

	//! 레벨 정보를 제거한다
	public void RemoveLevelInfo(CLevelInfo a_oLevelInfo) {
		CAccess.Assert(a_oLevelInfo != null && this.LevelInfoList.ContainsKey(a_oLevelInfo.LevelID));
		var oLevelInfoList = this.GetStageLevelInfos(a_oLevelInfo.StageID, a_oLevelInfo.ChapterID);

		for(int i = a_oLevelInfo.ID + KCDefine.B_VAL_1_INT; i < oLevelInfoList.Count; ++i) {
			oLevelInfoList[i].ID = i - KCDefine.B_VAL_1_INT;
			this.LevelInfoList.ExReplaceVal(oLevelInfoList[i].LevelID, oLevelInfoList[i]);
		}

		long nID = Factory.MakeUniqueLevelID(oLevelInfoList.Count - KCDefine.B_VAL_1_INT, a_oLevelInfo.StageID, a_oLevelInfo.ChapterID);
		this.LevelInfoList.Remove(nID);
	}

	//! 레벨 정보를 저장한다
	public void SaveLevelInfo(CLevelInfo a_oLevelInfo) {
		CAccess.Assert(a_oLevelInfo != null);
		string oFilePath = string.Format(KDefine.G_RUNTIME_DATA_P_FMT_LEVEL_INFO, a_oLevelInfo.LevelID + KCDefine.B_VAL_1_INT);

		CFunc.WriteMsgPackObj(oFilePath, a_oLevelInfo, false, false);
	}

	//! 레벨 정보를 저장한다
	public void SaveLevelInfos() {
		string oStr = string.Format(KCDefine.B_TEXT_FMT_1_DIGITS, this.LevelInfoList.Count);		
		string oFilePath = KDefine.G_RUNTIME_TABLE_P_LEVEL_INFO.ExGetReplaceStr(KCDefine.B_FILE_EXTENSION_BYTES, KCDefine.B_FILE_EXTENSION_TXT);

		foreach(var stKeyVal in this.LevelInfoList) {
			this.SaveLevelInfo(stKeyVal.Value);
		}

		CFunc.WriteStr(oFilePath, oStr);
		CFunc.WriteMsgPackObj(KDefine.G_RUNTIME_TABLE_P_LEVEL_INFO, this.LevelInfoList, false, false);
	}

	//! 레벨 정보를 로드한다
	private CLevelInfo LoadLevelInfo(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return CFunc.ReadMsgPackObj<CLevelInfo>(a_oFilePath, false);
	}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	#endregion			// 조건부 함수
}
#endif			// #if NEVER_USE_THIS
