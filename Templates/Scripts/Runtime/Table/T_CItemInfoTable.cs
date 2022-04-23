using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 아이템 정보 */
[System.Serializable]
public struct STItemInfo {
	public STDescInfo m_stDescInfo;

	public EItemKinds m_eItemKinds;
	public EItemKinds m_ePrevItemKinds;
	public EItemKinds m_eNextItemKinds;

	#region 프로퍼티
	public EItemType ItemType => (EItemType)((int)m_eItemKinds).ExKindsToType();
	public EItemKinds BaseItemKinds => (EItemKinds)((int)m_eItemKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STItemInfo(SimpleJSON.JSONNode a_oItemInfo) {
		m_stDescInfo = new STDescInfo(a_oItemInfo);
		
		m_eItemKinds = a_oItemInfo[KCDefine.U_KEY_ITEM_KINDS].ExIsValid() ? (EItemKinds)a_oItemInfo[KCDefine.U_KEY_ITEM_KINDS].AsInt : EItemKinds.NONE;
		m_ePrevItemKinds = a_oItemInfo[KCDefine.U_KEY_PREV_ITEM_KINDS].ExIsValid() ? (EItemKinds)a_oItemInfo[KCDefine.U_KEY_PREV_ITEM_KINDS].AsInt : EItemKinds.NONE;
		m_eNextItemKinds = a_oItemInfo[KCDefine.U_KEY_NEXT_ITEM_KINDS].ExIsValid() ? (EItemKinds)a_oItemInfo[KCDefine.U_KEY_NEXT_ITEM_KINDS].AsInt : EItemKinds.NONE;
	}
	#endregion			// 함수
}

/** 아이템 정보 테이블 */
public partial class CItemInfoTable : CScriptableObj<CItemInfoTable> {
	#region 변수
	[Header("=====> Item Info <=====")]
	[SerializeField] private List<STItemInfo> m_oItemInfoList = new List<STItemInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<EItemKinds, STItemInfo> ItemInfoDict { get; private set; } = new Dictionary<EItemKinds, STItemInfo>();

	private string ItemInfoTablePath {
		get {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_ITEM_INFO;
#else
			return KCDefine.U_TABLE_P_G_ITEM_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		var oItemInfoList = new List<STItemInfo>(m_oItemInfoList);

		for(int i = 0; i < oItemInfoList.Count; ++i) {
			this.ItemInfoDict.TryAdd(oItemInfoList[i].m_eItemKinds, oItemInfoList[i]);
		}
	}

	/** 효과 정보를 반환한다 */
	public STItemInfo GetItemInfo(EItemKinds a_EItemKinds) {
		bool bIsValid = this.TryGetItemInfo(a_EItemKinds, out STItemInfo stItemInfo);
		CAccess.Assert(bIsValid);

		return stItemInfo;
	}

	/** 효과 정보를 반환한다 */
	public bool TryGetItemInfo(EItemKinds a_EItemKinds, out STItemInfo a_stOutItemInfo) {
		a_stOutItemInfo = this.ItemInfoDict.GetValueOrDefault(a_EItemKinds, default(STItemInfo));
		return this.ItemInfoDict.ContainsKey(a_EItemKinds);
	}

	/** 효과 정보를 로드한다 */
	public Dictionary<EItemKinds, STItemInfo> LoadItemInfos() {
		return this.LoadItemInfos(this.ItemInfoTablePath);
	}

	/** 효과 정보를 로드한다 */
	private Dictionary<EItemKinds, STItemInfo> LoadItemInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadItemInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			return this.DoLoadItemInfos(CResManager.Inst.GetRes<TextAsset>(a_oFilePath).text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 효과 정보를 로드한다 */
	private Dictionary<EItemKinds, STItemInfo> DoLoadItemInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		var oJSONNode = SimpleJSON.JSONNode.Parse(a_oJSONStr);

		var oItemInfosList = new List<SimpleJSON.JSONNode>() {
			oJSONNode[KCDefine.B_KEY_JSON_COMMON_DATA]
		};

		for(int i = 0; i < oItemInfosList.Count; ++i) {
			for(int j = 0; j < oItemInfosList[i].Count; ++j) {
				var stItemInfo = new STItemInfo(oItemInfosList[i][j]);

				// 효과 정보가 추가 가능 할 경우
				if(!this.ItemInfoDict.ContainsKey(stItemInfo.m_eItemKinds) || oItemInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT) {
					this.ItemInfoDict.ExReplaceVal(stItemInfo.m_eItemKinds, stItemInfo);
				}
			}
		}

		return this.ItemInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
