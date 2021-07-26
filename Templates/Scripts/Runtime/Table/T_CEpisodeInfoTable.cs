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
	public ERewardKinds m_eRewardKinds;

	#region 함수
	//! 생성자
	public STLevelInfo(SimpleJSON.JSONNode a_oLevelInfo) {
		m_nID = a_oLevelInfo[KDefine.G_KEY_EPISODE_IT_ID].AsInt;
		m_nStageID = a_oLevelInfo[KDefine.G_KEY_EPISODE_IT_STAGE_ID].AsInt;
		m_nChapterID = a_oLevelInfo[KDefine.G_KEY_EPISODE_IT_CHAPTER_ID].AsInt;
		
		m_oName = a_oLevelInfo[KCDefine.U_KEY_NAME];
		m_oDesc = a_oLevelInfo[KCDefine.U_KEY_DESC];

		m_eLevelMode = (ELevelMode)a_oLevelInfo[KDefine.G_KEY_EPISODE_IT_LEVEL_MODE].AsInt;
		m_eRewardKinds = (ERewardKinds)a_oLevelInfo[KDefine.G_KEY_EPISODE_IT_REWARD_KINDS].AsInt;
	}
	#endregion			// 함수
}

//! 스테이지 정보
[System.Serializable]
public struct STStageInfo {
	public int m_nID;
	public int m_nChapterID;

	public string m_oName;
	public string m_oDesc;

	public EStageMode m_eStageMode;
	public ERewardKinds m_eRewardKinds;

	#region 함수
	//! 생성자
	public STStageInfo(SimpleJSON.JSONNode a_oStageInfo) {
		m_nID = a_oStageInfo[KDefine.G_KEY_EPISODE_IT_ID].AsInt;
		m_nChapterID = a_oStageInfo[KDefine.G_KEY_EPISODE_IT_CHAPTER_ID].AsInt;

		m_oName = a_oStageInfo[KCDefine.U_KEY_NAME];
		m_oDesc = a_oStageInfo[KCDefine.U_KEY_DESC];

		m_eStageMode = (EStageMode)a_oStageInfo[KDefine.G_KEY_EPISODE_IT_STAGE_MODE].AsInt;
		m_eRewardKinds = (ERewardKinds)a_oStageInfo[KDefine.G_KEY_EPISODE_IT_REWARD_KINDS].AsInt;
	}
	#endregion			// 함수
}

//! 챕터 정보
[System.Serializable]
public struct STChapterInfo {
	public int m_nID;

	public string m_oName;
	public string m_oDesc;

	public EChapterMode m_eChapterMode;
	public ERewardKinds m_eRewardKinds;

	#region 함수
	//! 생성자
	public STChapterInfo(SimpleJSON.JSONNode a_oChapterInfo) {
		m_nID = a_oChapterInfo[KDefine.G_KEY_EPISODE_IT_ID].AsInt;

		m_oName = a_oChapterInfo[KCDefine.U_KEY_NAME];
		m_oDesc = a_oChapterInfo[KCDefine.U_KEY_DESC];

		m_eChapterMode = (EChapterMode)a_oChapterInfo[KDefine.G_KEY_EPISODE_IT_CHAPTER_MODE].AsInt;
		m_eRewardKinds = (ERewardKinds)a_oChapterInfo[KDefine.G_KEY_EPISODE_IT_REWARD_KINDS].AsInt;
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
	public Dictionary<long, STLevelInfo> LevelInfoDict { get; private set; } = new Dictionary<long, STLevelInfo>();
	public Dictionary<long, STStageInfo> StageInfoDict { get; private set; } = new Dictionary<long, STStageInfo>();
	public Dictionary<long, STChapterInfo> ChapterInfoDict { get; private set; } = new Dictionary<long, STChapterInfo>();
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		for(int i = 0; i < m_oLevelInfoList.Count; ++i) {
			long nLevelID = CFactory.MakeUniqueLevelID(i, m_oLevelInfoList[i].m_nStageID, m_oLevelInfoList[i].m_nChapterID);
			this.LevelInfoDict.ExAddVal(nLevelID, m_oLevelInfoList[i]);
		}

		for(int i = 0; i < m_oStageInfoList.Count; ++i) {
			long nStageID = CFactory.MakeUniqueStageID(i, m_oStageInfoList[i].m_nChapterID);
			this.StageInfoDict.ExAddVal(nStageID, m_oStageInfoList[i]);
		}

		for(int i = 0; i < m_oChapterInfoList.Count; ++i) {
			long nChapterID = CFactory.MakeUniqueChapterID(i);
			this.ChapterInfoDict.ExAddVal(nChapterID, m_oChapterInfoList[i]);
		}
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
		long nLevelID = CFactory.MakeUniqueLevelID(a_nID, a_nStageID, a_nChapterID);
		a_stOutLevelInfo = this.LevelInfoDict.ExGetVal(nLevelID, KDefine.G_INVALID_LEVEL_INFO);

		return this.LevelInfoDict.ContainsKey(nLevelID);
	}

	//! 스테이지 정보를 반환한다
	public bool TryGetStageInfo(int a_nID, out STStageInfo a_stOutStageInfo, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		long nStageID = CFactory.MakeUniqueStageID(a_nID, a_nChapterID);
		a_stOutStageInfo = this.StageInfoDict.ExGetVal(nStageID, KDefine.G_INVALID_STAGE_INFO);

		return this.StageInfoDict.ContainsKey(nStageID);
	}

	//! 챕터 정보를 반환한다
	public bool TryGetChapterInfo(int a_nID, out STChapterInfo a_stOutChapterInfo) {
		long nChapterID = CFactory.MakeUniqueChapterID(a_nID);
		a_stOutChapterInfo = this.ChapterInfoDict.ExGetVal(nChapterID, KDefine.G_INVALID_CHAPTER_INFO);

		return this.ChapterInfoDict.ContainsKey(nChapterID);
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

	//! 에피소드 정보를 로드한다
	private List<object> DoLoadEpisodeInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		
		var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr) as SimpleJSON.JSONClass;
		var oLevelInfos = oJSONNode[KDefine.G_KEY_EPISODE_IT_LEVEL];
		var oStageInfos = oJSONNode[KDefine.G_KEY_EPISODE_IT_STAGE];
		var oChapterInfos = oJSONNode[KDefine.G_KEY_EPISODE_IT_CHAPTER];

		for(int i = 0; i < oLevelInfos.Count; ++i) {
			var stLevelInfo = new STLevelInfo(oLevelInfos[i]);
			long nLevelID = CFactory.MakeUniqueLevelID(i, stLevelInfo.m_nStageID, stLevelInfo.m_nChapterID);

			// 레벨 정보가 추가 가능 할 경우
			if(!this.LevelInfoDict.ContainsKey(nLevelID) || oLevelInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT) {
				this.LevelInfoDict.ExReplaceVal(nLevelID, stLevelInfo);
			}
		}

		for(int i = 0; i < oStageInfos.Count; ++i) {
			var stStageInfo = new STStageInfo(oStageInfos[i]);
			long nStageID = CFactory.MakeUniqueStageID(i, stStageInfo.m_nChapterID);

			// 스테이지 정보가 추가 가능 할 경우
			if(!this.StageInfoDict.ContainsKey(nStageID) || oStageInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT) {
				this.StageInfoDict.ExReplaceVal(nStageID, stStageInfo);
			}
		}

		for(int i = 0; i < oChapterInfos.Count; ++i) {
			var stChapterInfo = new STChapterInfo(oChapterInfos[i]);
			long nChapterID = CFactory.MakeUniqueChapterID(i);

			// 챕터 정보가 추가 가능 할 경우
			if(!this.ChapterInfoDict.ContainsKey(nChapterID) || oChapterInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT) {
				this.ChapterInfoDict.ExReplaceVal(nChapterID, stChapterInfo);
			}
		}
		
		return new List<object>() {
			this.LevelInfoDict, this.StageInfoDict, this.ChapterInfoDict
		};
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
