using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 레벨 정보
[System.Serializable]
public struct STLevelInfo {
	public int m_nID;
	public int m_nStageID;
	public int m_nChapterID;

	public string m_oName;
	public string m_oDesc;

	public ELevelMode m_eLevelMode;

	#region 함수
	//! 생성자
	public STLevelInfo(SimpleJSON.JSONNode a_oLevelInfo) {
		m_nID = a_oLevelInfo[KDefine.G_KEY_EPISODE_IT_ID].AsInt;
		m_nStageID = a_oLevelInfo[KDefine.G_KEY_EPISODE_IT_STAGE_ID].AsInt;
		m_nChapterID = a_oLevelInfo[KDefine.G_KEY_EPISODE_IT_CHAPTER_ID].AsInt;
		
		m_oName = a_oLevelInfo[KDefine.G_KEY_EPISODE_IT_NAME];
		m_oDesc = a_oLevelInfo[KDefine.G_KEY_EPISODE_IT_DESC];

		m_eLevelMode = (ELevelMode)a_oLevelInfo[KDefine.G_KEY_EPISODE_IT_LEVEL_MODE].AsInt;
	}
	#endregion			// 함수
}

//! 스테이지 정보
[System.Serializable]
public struct STStageInfo {
	public string m_oName;
	public string m_oDesc;

	public int m_nID;
	public int m_nChapterID;

	public EStageMode m_eStageMode;

	#region 함수
	//! 생성자
	public STStageInfo(SimpleJSON.JSONNode a_oStageInfo) {
		m_oName = a_oStageInfo[KDefine.G_KEY_EPISODE_IT_NAME];
		m_oDesc = a_oStageInfo[KDefine.G_KEY_EPISODE_IT_DESC];

		m_nID = a_oStageInfo[KDefine.G_KEY_EPISODE_IT_ID].AsInt;
		m_nChapterID = a_oStageInfo[KDefine.G_KEY_EPISODE_IT_CHAPTER_ID].AsInt;

		m_eStageMode = (EStageMode)a_oStageInfo[KDefine.G_KEY_EPISODE_IT_STAGE_MODE].AsInt;
	}
	#endregion			// 함수
}

//! 챕터 정보
[System.Serializable]
public struct STChapterInfo {
	public string m_oName;
	public string m_oDesc;

	public int m_nID;
	public EChapterMode m_eChapterMode;

	#region 함수
	//! 생성자
	public STChapterInfo(SimpleJSON.JSONNode a_oChapterInfo) {
		m_oName = a_oChapterInfo[KDefine.G_KEY_EPISODE_IT_NAME];
		m_oDesc = a_oChapterInfo[KDefine.G_KEY_EPISODE_IT_DESC];

		m_nID = a_oChapterInfo[KDefine.G_KEY_EPISODE_IT_ID].AsInt;
		m_eChapterMode = (EChapterMode)a_oChapterInfo[KDefine.G_KEY_EPISODE_IT_CHAPTER_MODE].AsInt;
	}
	#endregion			// 함수
}

//! 에피소드 정보 테이블
public class CEpisodeInfoTable : CScriptableObj<CEpisodeInfoTable> {
	#region 변수
	[Header("Level Info")]
	[SerializeField] private List<STLevelInfo> m_oLevelInfoList = new List<STLevelInfo>();

	[Header("Stage Info")]
	[SerializeField] private List<STStageInfo> m_oStageInfoList = new List<STStageInfo>();

	[Header("Chapter Info")]
	[SerializeField] private List<STChapterInfo> m_oChapterInfoList = new List<STChapterInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<long, STLevelInfo> LevelInfoList { get; private set; } = new Dictionary<long, STLevelInfo>();
	public Dictionary<long, STStageInfo> StageInfoList { get; private set; } = new Dictionary<long, STStageInfo>();
	public Dictionary<long, STChapterInfo> ChapterInfoList { get; private set; } = new Dictionary<long, STChapterInfo>();
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		this.SetupLevelInfos();
		this.SetupStageInfos();
		this.SetupChapterInfos();
	}

	//! 레벨 정보를 반환한다
	public STLevelInfo GetLevelInfo(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetLevelInfo(a_nID, out STLevelInfo stLevelInfo, a_nStageID, a_nChapterID);
		CAccess.Assert(bIsValid);

		return stLevelInfo;
	}

	//! 스테이지 정보를 반환한다
	public STStageInfo GetStageInfo(int a_nID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetStageInfo(a_nID, out STStageInfo stStageInfo, a_nChapterID);
		CAccess.Assert(bIsValid);

		return stStageInfo;
	}

	//! 챕터 정보를 반환한다
	public STChapterInfo GetChapterInfo(int a_nID) {
		bool bIsValid = this.TryGetChapterInfo(a_nID, out STChapterInfo stChapterInfo);
		CAccess.Assert(bIsValid);

		return stChapterInfo;
	}

	//! 레벨 정보를 반환한다
	public bool TryGetLevelInfo(int a_nID, out STLevelInfo a_stOutLevelInfo, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		long nLevelID = Factory.MakeUniqueLevelID(a_nID, a_nStageID, a_nChapterID);
		a_stOutLevelInfo = this.LevelInfoList.ExGetVal(nLevelID, KDefine.G_INVALID_LEVEL_INFO);

		return this.LevelInfoList.ContainsKey(nLevelID);
	}

	//! 스테이지 정보를 반환한다
	public bool TryGetStageInfo(int a_nID, out STStageInfo a_stOutStageInfo, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		long nStageID = Factory.MakeUniqueStageID(a_nID, a_nChapterID);
		a_stOutStageInfo = this.StageInfoList.ExGetVal(nStageID, KDefine.G_INVALID_STAGE_INFO);

		return this.StageInfoList.ContainsKey(nStageID);
	}

	//! 챕터 정보를 반환한다
	public bool TryGetChapterInfo(int a_nID, out STChapterInfo a_stOutChapterInfo) {
		long nChapterID = Factory.MakeUniqueChapterID(a_nID);
		a_stOutChapterInfo = this.ChapterInfoList.ExGetVal(nChapterID, KDefine.G_INVALID_CHAPTER_INFO);

		return this.ChapterInfoList.ContainsKey(nChapterID);
	}

	//! 에피소드 정보를 로드한다	
	public List<object> LoadEpisodeInfos() {
#if UNITY_EDITOR || UNITY_STANDALONE
		return this.LoadEpisodeInfos(KDefine.G_RUNTIME_TABLE_P_EPISODE_INFO);
#else
		return this.LoadEpisodeInfos(KCDefine.U_TABLE_P_G_EPISODE_INFO);
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	}

	//! 에피소드 정보를 로드한다
	public List<object> LoadEpisodeInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_EDITOR || UNITY_STANDALONE
		string oJSONStr = CFunc.ReadStr(a_oFilePath);
		return this.DoLoadEpisodeInfos(oJSONStr);
#else
		try {
			var oTextAsset = CResManager.Inst.GetRes<TextAsset>(a_oFilePath);
			return this.DoLoadEpisodeInfos(oTextAsset.text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	}

	//! 레벨 정보를 설정한다
	private void SetupLevelInfos() {
		CAccess.Assert(m_oLevelInfoList != null && this.LevelInfoList != null);

		for(int i = 0; i < m_oLevelInfoList.Count; ++i) {
			long nLevelID = Factory.MakeUniqueLevelID(i, m_oLevelInfoList[i].m_nStageID, m_oLevelInfoList[i].m_nChapterID);
			this.LevelInfoList.ExAddVal(nLevelID, m_oLevelInfoList[i]);
		}
	}

	//! 스테이지 정보를 설정한다
	private void SetupStageInfos() {
		CAccess.Assert(m_oStageInfoList != null && this.StageInfoList != null);

		for(int i = 0; i < m_oStageInfoList.Count; ++i) {
			long nStageID = Factory.MakeUniqueStageID(i, m_oStageInfoList[i].m_nChapterID);
			this.StageInfoList.ExAddVal(nStageID, m_oStageInfoList[i]);
		}
	}

	//! 챕터 정보를 설정한다
	private void SetupChapterInfos() {
		CAccess.Assert(m_oChapterInfoList != null && this.ChapterInfoList != null);

		for(int i = 0; i < m_oChapterInfoList.Count; ++i) {
			long nChapterID = Factory.MakeUniqueChapterID(i);
			this.ChapterInfoList.ExAddVal(nChapterID, m_oChapterInfoList[i]);
		}
	}

	//! 에피소드 정보를 로드한다
	private List<object> DoLoadEpisodeInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		
		var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr) as SimpleJSON.JSONClass;
		var oLevelInfos = oJSONNode[KDefine.G_KEY_EPISODE_IT_LEVEL];
		var oStageInfos = oJSONNode[KDefine.G_KEY_EPISODE_IT_STAGE];
		var oChapterInfos = oJSONNode[KDefine.G_KEY_EPISODE_IT_CHAPTER];

		for(int i = 0; i < oLevelInfos.Count; ++i) {
			var stLevelInfo = new STLevelInfo(oLevelInfos[i]);
			long nLevelID = Factory.MakeUniqueLevelID(i, stLevelInfo.m_nStageID, stLevelInfo.m_nChapterID);

			// 레벨 정보가 추가 가능 할 경우
			if(!this.LevelInfoList.ContainsKey(nLevelID) || oLevelInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT) {
				this.LevelInfoList.ExReplaceVal(nLevelID, stLevelInfo);
			}
		}

		for(int i = 0; i < oStageInfos.Count; ++i) {
			var stStageInfo = new STStageInfo(oStageInfos[i]);
			long nStageID = Factory.MakeUniqueStageID(i, stStageInfo.m_nChapterID);

			// 스테이지 정보가 추가 가능 할 경우
			if(!this.StageInfoList.ContainsKey(nStageID) || oStageInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT) {
				this.StageInfoList.ExReplaceVal(nStageID, stStageInfo);
			}
		}

		for(int i = 0; i < oChapterInfos.Count; ++i) {
			var stChapterInfo = new STChapterInfo(oChapterInfos[i]);
			long nChapterID = Factory.MakeUniqueChapterID(i);

			// 챕터 정보가 추가 가능 할 경우
			if(!this.ChapterInfoList.ContainsKey(nChapterID) || oChapterInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT) {
				this.ChapterInfoList.ExReplaceVal(nChapterID, stChapterInfo);
			}
		}

#if UNITY_EDITOR
		this.SetupLevelInfoList();
		this.SetupStageInfoList();
		this.SetupChapterInfoList();
#endif			// #if UNITY_EDITOR

		return new List<object>() {
			this.LevelInfoList, this.StageInfoList, this.ChapterInfoList
		};
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR
	//! 레벨 정보를 설정한다
	private void SetupLevelInfoList() {
		m_oLevelInfoList.Clear();

		foreach(var stKeyVal in this.LevelInfoList) {
			m_oLevelInfoList.ExAddVal(stKeyVal.Value);
		}

		m_oLevelInfoList.Sort((a_stLhs, a_stRhs) => {
			long nLevelID = Factory.MakeUniqueLevelID(a_stLhs.m_nID, a_stLhs.m_nStageID, a_stLhs.m_nChapterID);
			return (int)(nLevelID - Factory.MakeUniqueLevelID(a_stRhs.m_nID, a_stRhs.m_nStageID, a_stRhs.m_nChapterID));
		});
	}

	//! 스테이지 정보를 설정한다
	private void SetupStageInfoList() {
		m_oStageInfoList.Clear();

		foreach(var stKeyVal in this.StageInfoList) {
			m_oStageInfoList.ExAddVal(stKeyVal.Value);
		}

		m_oStageInfoList.Sort((a_stLhs, a_stRhs) => {
			long nStageID = Factory.MakeUniqueStageID(a_stLhs.m_nID, a_stLhs.m_nChapterID);
			return (int)(nStageID - Factory.MakeUniqueStageID(a_stRhs.m_nID, a_stRhs.m_nChapterID));
		});
	}

	//! 챕터 정보를 설정한다
	private void SetupChapterInfoList() {
		m_oChapterInfoList.Clear();

		foreach(var stKeyVal in this.ChapterInfoList) {
			m_oChapterInfoList.ExAddVal(stKeyVal.Value);
		}
		
		m_oChapterInfoList.Sort((a_stLhs, a_stRhs) => {
			long nChapterID = Factory.MakeUniqueChapterID(a_stLhs.m_nID);
			return (int)(nChapterID - Factory.MakeUniqueChapterID(a_stRhs.m_nID));
		});
	}
#endif			// #if UNITY_EDITOR
	#endregion			// 조건부 함수
}
#endif			// #if NEVER_USE_THIS
