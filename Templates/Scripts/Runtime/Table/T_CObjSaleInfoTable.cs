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
	public STCommonInfo m_stCommonInfo;

	public EObjKinds m_eObjKinds;
	public EObjKinds m_ePrevObjKinds;
	public EObjKinds m_eNextObjKinds;

	public Dictionary<ulong, STTargetInfo> m_oPayTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAcquireTargetInfoDict;

	#region 상수
	public static STObjSaleInfo INVALID = new STObjSaleInfo() {
		m_eObjKinds = EObjKinds.NONE, m_ePrevObjKinds = EObjKinds.NONE, m_eNextObjKinds = EObjKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public EObjType ObjType => (EObjType)((int)m_eObjKinds).ExKindsToType();
	public EObjKinds BaseObjKinds => (EObjKinds)((int)m_eObjKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STObjSaleInfo(SimpleJSON.JSONNode a_oObjSaleInfo) {
		m_stCommonInfo = new STCommonInfo(a_oObjSaleInfo);

		m_eObjKinds = a_oObjSaleInfo[KCDefine.U_KEY_OBJ_SALE_KINDS].ExIsValid() ? (EObjKinds)a_oObjSaleInfo[KCDefine.U_KEY_OBJ_SALE_KINDS].AsInt : EObjKinds.NONE;
		m_ePrevObjKinds = a_oObjSaleInfo[KCDefine.U_KEY_PREV_OBJ_SALE_KINDS].ExIsValid() ? (EObjKinds)a_oObjSaleInfo[KCDefine.U_KEY_PREV_OBJ_SALE_KINDS].AsInt : EObjKinds.NONE;
		m_eNextObjKinds = a_oObjSaleInfo[KCDefine.U_KEY_NEXT_OBJ_SALE_KINDS].ExIsValid() ? (EObjKinds)a_oObjSaleInfo[KCDefine.U_KEY_NEXT_OBJ_SALE_KINDS].AsInt : EObjKinds.NONE;

		m_oPayTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oAcquireTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oObjSaleInfo[string.Format(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oPayTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_ABILITY_VAL_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oObjSaleInfo[string.Format(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oAcquireTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
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
	public Dictionary<EObjKinds, STObjSaleInfo> ObjSaleInfoDict { get; private set; } = new Dictionary<EObjKinds, STObjSaleInfo>();

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
			this.ObjSaleInfoDict.TryAdd(oObjSaleInfoList[i].m_eObjKinds, oObjSaleInfoList[i]);
		}
	}

	/** 객체 판매 정보를 리셋한다 */
	public void ResetObjSaleInfos(string a_oJSONStr) {
		this.ResetObjSaleInfos();
		this.DoLoadObjSaleInfos(a_oJSONStr);
	}

	/** 객체 판매 정보를 반환한다 */
	public STObjSaleInfo GetObjSaleInfo(EObjKinds a_eObjKinds) {
		bool bIsValid = this.TryGetObjSaleInfo(a_eObjKinds, out STObjSaleInfo stObjSaleInfo);
		CAccess.Assert(bIsValid);

		return stObjSaleInfo;
	}

	/** 지불 타겟 정보를 반환한다 */
	public STTargetInfo GetPayTargetInfo(EObjKinds a_eObjKinds, ETargetKinds a_eTargetKinds, int a_nKinds) {
		bool bIsValid = this.TryGetPayTargetInfo(a_eObjKinds, a_eTargetKinds, a_nKinds, out STTargetInfo stPayTargetInfo);
		CAccess.Assert(bIsValid);

		return stPayTargetInfo;
	}

	/** 획득 타겟 정보를 반환한다 */
	public STTargetInfo GetAcquireTargetInfo(EObjKinds a_eObjKinds, ETargetKinds a_eTargetKinds, int a_nKinds) {
		bool bIsValid = this.TryGetAcquireTargetInfo(a_eObjKinds, a_eTargetKinds, a_nKinds, out STTargetInfo stAcquireTargetInfo);
		CAccess.Assert(bIsValid);

		return stAcquireTargetInfo;
	}

	/** 객체 판매 정보를 반환한다 */
	public bool TryGetObjSaleInfo(EObjKinds a_eObjKinds, out STObjSaleInfo a_stOutObjSaleInfo) {
		a_stOutObjSaleInfo = this.ObjSaleInfoDict.GetValueOrDefault(a_eObjKinds, default(STObjSaleInfo));
		return this.ObjSaleInfoDict.ContainsKey(a_eObjKinds);
	}

	/** 지불 타겟 정보를 반환한다 */
	public bool TryGetPayTargetInfo(EObjKinds a_eObjKinds, ETargetKinds a_eTargetKinds, int a_nKinds, out STTargetInfo a_stOutPayTargetInfo) {
		a_stOutPayTargetInfo = this.TryGetObjSaleInfo(a_eObjKinds, out STObjSaleInfo stObjSaleInfo) ? stObjSaleInfo.m_oPayTargetInfoDict.GetValueOrDefault(Factory.MakeUniqueTargetInfoID(a_eTargetKinds, a_nKinds), STTargetInfo.INVALID) : STTargetInfo.INVALID;
		return !a_stOutPayTargetInfo.Equals(STTargetInfo.INVALID);
	}

	/** 획득 타겟 정보를 반환한다 */
	public bool TryGetAcquireTargetInfo(EObjKinds a_eObjKinds, ETargetKinds a_eTargetKinds, int a_nKinds, out STTargetInfo a_stOutAcquireTargetInfo) {
		a_stOutAcquireTargetInfo = this.TryGetObjSaleInfo(a_eObjKinds, out STObjSaleInfo stObjSaleInfo) ? stObjSaleInfo.m_oAcquireTargetInfoDict.GetValueOrDefault(Factory.MakeUniqueTargetInfoID(a_eTargetKinds, a_nKinds), STTargetInfo.INVALID) : STTargetInfo.INVALID;
		return !a_stOutAcquireTargetInfo.Equals(STTargetInfo.INVALID);
	}

	/** 객체 판매 정보를 로드한다 */
	public Dictionary<EObjKinds, STObjSaleInfo> LoadObjSaleInfos() {
		this.ResetObjSaleInfos();
		return this.LoadObjSaleInfos(this.ObjSaleInfoTablePath);
	}

	/** 객체 판매 정보를 로드한다 */
	private Dictionary<EObjKinds, STObjSaleInfo> LoadObjSaleInfos(string a_oFilePath) {
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
	private Dictionary<EObjKinds, STObjSaleInfo> DoLoadObjSaleInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr) as SimpleJSON.JSONClass;

		var oObjSaleInfosList = new List<SimpleJSON.JSONNode>() {
			oJSONNode[KCDefine.U_KEY_BG], oJSONNode[KCDefine.U_KEY_NORM], oJSONNode[KCDefine.U_KEY_OVERLAY], oJSONNode[KCDefine.U_KEY_PLAYABLE], oJSONNode[KCDefine.U_KEY_NON_PLAYABLE], oJSONNode[KCDefine.U_KEY_ENEMY]
		};

		for(int i = 0; i < oObjSaleInfosList.Count; ++i) {
			for(int j = 0; j < oObjSaleInfosList[i].Count; ++j) {
				var stObjSaleInfo = new STObjSaleInfo(oObjSaleInfosList[i][j]);

				// 객체 판매 정보가 추가 가능 할 경우
				if(stObjSaleInfo.m_eObjKinds.ExIsValid() && (!this.ObjSaleInfoDict.ContainsKey(stObjSaleInfo.m_eObjKinds) || oObjSaleInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.ObjSaleInfoDict.ExReplaceVal(stObjSaleInfo.m_eObjKinds, stObjSaleInfo);
				}
			}
		}

		return this.ObjSaleInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
