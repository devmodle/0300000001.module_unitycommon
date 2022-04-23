using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 블럭 정보 */
[System.Serializable]
public struct STBlockInfo {
	public STDescInfo m_stDescInfo;

	public EBlockKinds m_eBlockKinds;
	public EBlockKinds m_ePrevBlockKinds;
	public EBlockKinds m_eNextBlockKinds;

	public EResKinds m_eBlockResKinds;
	public Vector3 m_stSize;

	#region 프로퍼티
	public EBlockType BlockType => (EBlockType)((int)m_eBlockKinds).ExKindsToType();
	public EBlockKinds BaseBlockKinds => (EBlockKinds)((int)m_eBlockKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티
	
	#region 함수
	/** 생성자 */
	public STBlockInfo(SimpleJSON.JSONNode a_oBlockInfo) {
		m_stDescInfo = new STDescInfo(a_oBlockInfo);
		
		m_eBlockKinds = a_oBlockInfo[KCDefine.U_KEY_BLOCK_KINDS].ExIsValid() ? (EBlockKinds)a_oBlockInfo[KCDefine.U_KEY_BLOCK_KINDS].AsInt : EBlockKinds.NONE;
		m_ePrevBlockKinds = a_oBlockInfo[KCDefine.U_KEY_PREV_BLOCK_KINDS].ExIsValid() ? (EBlockKinds)a_oBlockInfo[KCDefine.U_KEY_PREV_BLOCK_KINDS].AsInt : EBlockKinds.NONE;
		m_eNextBlockKinds = a_oBlockInfo[KCDefine.U_KEY_NEXT_BLOCK_KINDS].ExIsValid() ? (EBlockKinds)a_oBlockInfo[KCDefine.U_KEY_NEXT_BLOCK_KINDS].AsInt : EBlockKinds.NONE;

		m_eBlockResKinds = a_oBlockInfo[KCDefine.U_KEY_BLOCK_RES_KINDS].ExIsValid() ? (EResKinds)a_oBlockInfo[KCDefine.U_KEY_BLOCK_RES_KINDS].AsInt : EResKinds.NONE;
		m_stSize = new Vector3(a_oBlockInfo[KCDefine.U_KEY_SIZE_X].AsFloat, a_oBlockInfo[KCDefine.U_KEY_SIZE_Y].AsFloat, a_oBlockInfo[KCDefine.U_KEY_SIZE_Z].AsFloat);
	}
	#endregion			// 함수
}

/** 블럭 정보 테이블 */
public partial class CBlockInfoTable : CScriptableObj<CBlockInfoTable> {
	#region 변수
	[Header("=====> BG Block Info <=====")]
	[SerializeField] private List<STBlockInfo> m_oBGBlockInfoList = new List<STBlockInfo>();

	[Header("=====> Norm Block Info <=====")]
	[SerializeField] private List<STBlockInfo> m_oNormBlockInfoList = new List<STBlockInfo>();

	[Header("=====> Overlay Block Info <=====")]
	[SerializeField] private List<STBlockInfo> m_oOverlayBlockInfoList = new List<STBlockInfo>();
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

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		var oBlockInfoList = new List<STBlockInfo>(m_oBGBlockInfoList);
		oBlockInfoList.AddRange(m_oNormBlockInfoList);
		oBlockInfoList.AddRange(m_oOverlayBlockInfoList);

		for(int i = 0; i < oBlockInfoList.Count; ++i) {
			this.BlockInfoDict.TryAdd(oBlockInfoList[i].m_eBlockKinds, oBlockInfoList[i]);
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
		a_stOutBlockInfo = this.BlockInfoDict.GetValueOrDefault(a_eBlockKinds, default(STBlockInfo));
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
			return this.DoLoadBlockInfos(CResManager.Inst.GetRes<TextAsset>(a_oFilePath).text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 블럭 정보를 로드한다 */
	private Dictionary<EBlockKinds, STBlockInfo> DoLoadBlockInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		var oJSONNode = SimpleJSON.JSONNode.Parse(a_oJSONStr);

		var oBlockInfosList = new List<SimpleJSON.JSONNode>() {
			oJSONNode[KCDefine.U_KEY_BG], oJSONNode[KCDefine.U_KEY_NORM], oJSONNode[KCDefine.U_KEY_OVERLAY]
		};

		for(int i = 0; i < oBlockInfosList.Count; ++i) {
			for(int j = 0; j < oBlockInfosList[i].Count; ++j) {
				var stBlockInfo = new STBlockInfo(oBlockInfosList[i][j]);

				// 블럭 정보가 추가 가능 할 경우
				if(!this.BlockInfoDict.ContainsKey(stBlockInfo.m_eBlockKinds) || oBlockInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT) {
					this.BlockInfoDict.ExReplaceVal(stBlockInfo.m_eBlockKinds, stBlockInfo);
				}
			}
		}

		return this.BlockInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
