using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if RUNTIME_TEMPLATES_MODULE_ENABLE
/** 블럭 정보 */
[System.Serializable]
public struct STBlockInfo {
	public string m_oName;
	public string m_oDesc;

	public EBlockKinds m_eBlockKinds;
	public EBlockKinds m_eNextBlockKinds;

	#region 함수
	/** 생성자 */
	public STBlockInfo(SimpleJSON.JSONNode a_oBlockInfo) {
		m_oName = a_oBlockInfo[KCDefine.U_KEY_NAME];
		m_oDesc = a_oBlockInfo[KCDefine.U_KEY_DESC];

		m_eBlockKinds = (EBlockKinds)a_oBlockInfo[KCDefine.U_KEY_BLOCK_KINDS].AsInt;
		m_eNextBlockKinds = (EBlockKinds)a_oBlockInfo[KCDefine.U_KEY_NEXT_BLOCK_KINDS].AsInt;
	}
	#endregion			// 함수
}

/** 블럭 정보 테이블 */
public class CBlockInfoTable : CScriptableObj<CBlockInfoTable> {
	#region 변수
	[Header("=====> Block Info <=====")]
	[SerializeField] private List<STBlockInfo> m_oBlockInfoInfoList = new List<STBlockInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<EBlockKinds, STBlockInfo> BlockInfoDict { get; private set; } = new Dictionary<EBlockKinds, STBlockInfo>();

	private string BlockInfoTablePath {
		get {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_BLOCK_INFO;
#else
			return KCDefine.U_TABLE_P_G_BLOCK_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		}
	}
	#endregion			// 프로퍼티

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		for(int i = 0; i < m_oBlockInfoInfoList.Count; ++i) {
			this.BlockInfoDict.TryAdd(m_oBlockInfoInfoList[i].m_eBlockKinds, m_oBlockInfoInfoList[i]);
		}
	}

	/** 블럭 정보를 반환한다 */
	public STBlockInfo GetBlockInfo(EBlockKinds a_eBlockKinds) {
		bool bIsValid = this.TryGetBlockInfo(a_eBlockKinds, out STBlockInfo stBlockInfo);
		CAccess.Assert(bIsValid);

		return stBlockInfo;
	}

	/** 블럭 정보를 반환한다 */
	public bool TryGetBlockInfo(EBlockKinds a_eBlockKinds, out STBlockInfo a_stOutBlockInfo) {
		a_stOutBlockInfo = this.BlockInfoDict.GetValueOrDefault(a_eBlockKinds, KDefine.G_INVALID_BLOCK_INFO);
		return this.BlockInfoDict.ContainsKey(a_eBlockKinds);
	}

	/** 블럭 정보를 로드한다 */
	public Dictionary<EBlockKinds, STBlockInfo> LoadBlockInfos() {
		return this.LoadBlockInfos(this.BlockInfoTablePath);
	}

	/** 블럭 정보를 로드한다 */
	private Dictionary<EBlockKinds, STBlockInfo> LoadBlockInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadBlockInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			var oTextAsset = CResManager.Inst.GetRes<TextAsset>(a_oFilePath);
			return this.DoLoadBlockInfos(oTextAsset.text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 블럭 정보를 로드한다 */
	private Dictionary<EBlockKinds, STBlockInfo> DoLoadBlockInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());

		var oJSONNode = SimpleJSON.JSONNode.Parse(a_oJSONStr);
		var oBlockInfos = oJSONNode[KCDefine.B_KEY_JSON_COMMON_DATA];

		for(int i = 0; i < oBlockInfos.Count; ++i) {
			var stBlockInfo = new STBlockInfo(oBlockInfos[i]);
			bool bIsReplace = oBlockInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT;

			// 블럭 정보가 추가 가능 할 경우
			if(bIsReplace || !this.BlockInfoDict.ContainsKey(stBlockInfo.m_eBlockKinds)) {
				this.BlockInfoDict.ExReplaceVal(stBlockInfo.m_eBlockKinds, stBlockInfo);
			}
		}

		return this.BlockInfoDict;
	}
	#endregion			// 함수

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
