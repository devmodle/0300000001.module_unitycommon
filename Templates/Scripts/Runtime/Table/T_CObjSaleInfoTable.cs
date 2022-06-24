using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 객체 판매 정보 */
[System.Serializable]
public partial struct STObjSaleInfo {
	public STDescInfo m_stDescInfo;

	public EObjSaleKinds m_eObjSaleKinds;
	public EObjSaleKinds m_ePrevObjSaleKinds;
	public EObjSaleKinds m_eNextObjSaleKinds;

	public List<EObjKinds> m_oObjKindsList;
	public List<STPriceInfo> m_oPriceInfoList;

	#region 프로퍼티
	public EObjSaleType ObjSaleType => (EObjSaleType)((int)m_eObjSaleKinds).ExKindsToType();
	public EObjSaleKinds BaseObjSaleKinds => (EObjSaleKinds)((int)m_eObjSaleKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STObjSaleInfo(SimpleJSON.JSONNode a_oObjSaleInfo) {
		m_stDescInfo = new STDescInfo(a_oObjSaleInfo);

		m_eObjSaleKinds = a_oObjSaleInfo[KCDefine.U_KEY_OBJ_SALE_KINDS].ExIsValid() ? (EObjSaleKinds)a_oObjSaleInfo[KCDefine.U_KEY_OBJ_SALE_KINDS].AsInt : EObjSaleKinds.NONE;
		m_ePrevObjSaleKinds = a_oObjSaleInfo[KCDefine.U_KEY_PREV_OBJ_SALE_KINDS].ExIsValid() ? (EObjSaleKinds)a_oObjSaleInfo[KCDefine.U_KEY_PREV_OBJ_SALE_KINDS].AsInt : EObjSaleKinds.NONE;
		m_eNextObjSaleKinds = a_oObjSaleInfo[KCDefine.U_KEY_NEXT_OBJ_SALE_KINDS].ExIsValid() ? (EObjSaleKinds)a_oObjSaleInfo[KCDefine.U_KEY_NEXT_OBJ_SALE_KINDS].AsInt : EObjSaleKinds.NONE;

		m_oObjKindsList = new List<EObjKinds>();
		m_oPriceInfoList = new List<STPriceInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_OBJ_KINDS; ++i) {
			string oObjKindsKey = string.Format(KCDefine.U_KEY_FMT_OBJ_KINDS, i + KCDefine.B_VAL_1_INT);
			m_oObjKindsList.Add(a_oObjSaleInfo[oObjKindsKey].ExIsValid() ? (EObjKinds)a_oObjSaleInfo[oObjKindsKey].AsInt : EObjKinds.NONE);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_PRICE_INFOS; ++i) {
			m_oPriceInfoList.Add(new STPriceInfo(a_oObjSaleInfo, i));
		}
	}
	#endregion			// 함수
}

/** 객체 판매 정보 테이블 */
public partial class CObjSaleInfoTable : CScriptableObj<CObjSaleInfoTable> {
	#region 변수
	[Header("=====> BG Obj Sale Info <=====")]
	[SerializeField] private List<STObjSaleInfo> m_oBGObjSaleInfoList = new List<STObjSaleInfo>();

	[Header("=====> Norm Obj Sale Info <=====")]
	[SerializeField] private List<STObjSaleInfo> m_oNormObjSaleInfoList = new List<STObjSaleInfo>();

	[Header("=====> Overlay Obj Sale Info <=====")]
	[SerializeField] private List<STObjSaleInfo> m_oOverlayObjSaleInfoList = new List<STObjSaleInfo>();

	[Header("=====> Playable Obj Sale Info <=====")]
	[SerializeField] private List<STObjSaleInfo> m_oPlayableObjSaleInfoList = new List<STObjSaleInfo>();

	[Header("=====> Non Playable Obj Sale Info <=====")]
	[SerializeField] private List<STObjSaleInfo> m_oNonPlayableObjSaleInfoList = new List<STObjSaleInfo>();

	[Header("=====> Enemy Obj Sale Info <=====")]
	[SerializeField] private List<STObjSaleInfo> m_oEnemyObjSaleInfoList = new List<STObjSaleInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<EObjSaleKinds, STObjSaleInfo> ObjSaleInfoDict { get; private set; } = new Dictionary<EObjSaleKinds, STObjSaleInfo>();

	private string ObjSaleInfoTablePath {
		get {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_OBJ_SALE_INFO;
#else
			return KCDefine.U_TABLE_P_G_OBJ_SALE_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetObjSaleInfos();
	}

	/** 객체 판매 정보를 리셋한다 */
	public void ResetObjSaleInfos() {
		this.ObjSaleInfoDict.Clear();

		var oObjSaleInfoList = new List<STObjSaleInfo>(m_oBGObjSaleInfoList);
		oObjSaleInfoList.ExAddVals(m_oNormObjSaleInfoList);
		oObjSaleInfoList.ExAddVals(m_oOverlayObjSaleInfoList);
		oObjSaleInfoList.ExAddVals(m_oPlayableObjSaleInfoList);
		oObjSaleInfoList.ExAddVals(m_oNonPlayableObjSaleInfoList);
		oObjSaleInfoList.ExAddVals(m_oEnemyObjSaleInfoList);

		for(int i = 0; i < oObjSaleInfoList.Count; ++i) {
			this.ObjSaleInfoDict.TryAdd(oObjSaleInfoList[i].m_eObjSaleKinds, oObjSaleInfoList[i]);
		}
	}

	/** 객체 판매 정보를 리셋한다 */
	public void ResetObjSaleInfos(string a_oJSONStr) {
		this.ResetObjSaleInfos();
		this.DoLoadObjSaleInfos(a_oJSONStr);
	}

	/** 객체 판매 정보를 반환한다 */
	public STObjSaleInfo GetObjSaleInfo(EObjSaleKinds a_eObjSaleKinds) {
		bool bIsValid = this.TryGetObjSaleInfo(a_eObjSaleKinds, out STObjSaleInfo stObjSaleInfo);
		CAccess.Assert(bIsValid);

		return stObjSaleInfo;
	}

	/** 가격 정보를 반환한다 */
	public STPriceInfo GetPriceInfo(EObjSaleKinds a_eObjSaleKinds, EPriceType a_ePriceType, int a_nKinds) {
		bool bIsValid = this.TryGetPriceInfo(a_eObjSaleKinds, a_ePriceType, a_nKinds, out STPriceInfo stPriceInfo);
		CAccess.Assert(bIsValid);

		return stPriceInfo;
	}

	/** 객체 판매 정보를 반환한다 */
	public bool TryGetObjSaleInfo(EObjSaleKinds a_eObjSaleKinds, out STObjSaleInfo a_stOutObjSaleInfo) {
		a_stOutObjSaleInfo = this.ObjSaleInfoDict.GetValueOrDefault(a_eObjSaleKinds, default(STObjSaleInfo));
		return this.ObjSaleInfoDict.ContainsKey(a_eObjSaleKinds);
	}

	/** 가격 정보를 반환한다 */
	public bool TryGetPriceInfo(EObjSaleKinds a_eObjSaleKinds, EPriceType a_ePriceType, int a_nKinds, out STPriceInfo a_stOutPriceInfo) {
		// 아이템 판매 정보가 존재 할 경우
		if(this.TryGetObjSaleInfo(a_eObjSaleKinds, out STObjSaleInfo stObjSaleInfo)) {
			return stObjSaleInfo.m_oPriceInfoList.ExTryGetPriceInfo(a_ePriceType, a_nKinds, out a_stOutPriceInfo);
		}

		a_stOutPriceInfo = default(STPriceInfo);
		return false;
	}

	/** 객체 판매 정보를 로드한다 */
	public Dictionary<EObjSaleKinds, STObjSaleInfo> LoadObjSaleInfos() {
		this.ResetObjSaleInfos();
		return this.LoadObjSaleInfos(this.ObjSaleInfoTablePath);
	}

	/** 객체 판매 정보를 로드한다 */
	private Dictionary<EObjSaleKinds, STObjSaleInfo> LoadObjSaleInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadObjSaleInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			return this.DoLoadObjSaleInfos(CResManager.Inst.GetRes<TextAsset>(a_oFilePath).text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 객체 판매 정보를 로드한다 */
	private Dictionary<EObjSaleKinds, STObjSaleInfo> DoLoadObjSaleInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr) as SimpleJSON.JSONClass;

		var oObjSaleInfosList = new List<SimpleJSON.JSONNode>() {
			oJSONNode[KCDefine.U_KEY_BG], oJSONNode[KCDefine.U_KEY_NORM], oJSONNode[KCDefine.U_KEY_OVERLAY], oJSONNode[KCDefine.U_KEY_PLAYABLE], oJSONNode[KCDefine.U_KEY_NON_PLAYABLE], oJSONNode[KCDefine.U_KEY_ENEMY]
		};

		for(int i = 0; i < oObjSaleInfosList.Count; ++i) {
			for(int j = 0; j < oObjSaleInfosList[i].Count; ++j) {
				var stObjSaleInfo = new STObjSaleInfo(oObjSaleInfosList[i][j]);

				// 객체 판매 정보가 추가 가능 할 경우
				if(stObjSaleInfo.m_eObjSaleKinds.ExIsValid() && (!this.ObjSaleInfoDict.ContainsKey(stObjSaleInfo.m_eObjSaleKinds) || oObjSaleInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.ObjSaleInfoDict.ExReplaceVal(stObjSaleInfo.m_eObjSaleKinds, stObjSaleInfo);
				}
			}
		}

		return this.ObjSaleInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
