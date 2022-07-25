using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 객체 정보 */
[System.Serializable]
public partial struct STObjInfo {
	public STCommonInfo m_stCommonInfo;
	public Vector3 m_stSize;

	public EObjKinds m_eObjKinds;
	public EObjKinds m_ePrevObjKinds;
	public EObjKinds m_eNextObjKinds;

	public List<EResKinds> m_oResKindsList;

	public Dictionary<ulong, STTargetInfo> m_oDropItemTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oEquipItemTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oSkillTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAbilityTargetInfoDict;

	#region 상수
	public static STObjInfo INVALID = new STObjInfo() {
		m_eObjKinds = EObjKinds.NONE, m_ePrevObjKinds = EObjKinds.NONE, m_eNextObjKinds = EObjKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public EObjType ObjType => (EObjType)((int)m_eObjKinds).ExKindsToType();
	public EObjKinds BaseObjKinds => (EObjKinds)((int)m_eObjKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티
	
	#region 함수
	/** 생성자 */
	public STObjInfo(SimpleJSON.JSONNode a_oObjInfo) {
		m_stCommonInfo = new STCommonInfo(a_oObjInfo);
		m_stSize = new Vector3(a_oObjInfo[KCDefine.U_KEY_SIZE][KCDefine.B_VAL_0_INT].AsFloat, a_oObjInfo[KCDefine.U_KEY_SIZE][KCDefine.B_VAL_1_INT].AsFloat, a_oObjInfo[KCDefine.U_KEY_SIZE][KCDefine.B_VAL_2_INT].AsFloat);

		m_eObjKinds = a_oObjInfo[KCDefine.U_KEY_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjInfo[KCDefine.U_KEY_OBJ_KINDS].AsInt : EObjKinds.NONE;
		m_ePrevObjKinds = a_oObjInfo[KCDefine.U_KEY_PREV_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjInfo[KCDefine.U_KEY_PREV_OBJ_KINDS].AsInt : EObjKinds.NONE;
		m_eNextObjKinds = a_oObjInfo[KCDefine.U_KEY_NEXT_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjInfo[KCDefine.U_KEY_NEXT_OBJ_KINDS].AsInt : EObjKinds.NONE;

		m_oResKindsList = new List<EResKinds>();

		m_oDropItemTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oEquipItemTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oSkillTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oAbilityTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_RES_KINDS; ++i) {
			string oKey = string.Format(KCDefine.U_KEY_FMT_RES_KINDS, i + KCDefine.B_VAL_1_INT);
			m_oResKindsList.Add(a_oObjInfo[oKey].ExIsValid() ? (EResKinds)a_oObjInfo[oKey].AsInt : EResKinds.NONE);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oObjInfo[string.Format(KCDefine.U_KEY_FMT_DROP_ITEM_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oDropItemTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oObjInfo[string.Format(KCDefine.U_KEY_FMT_EQUIP_ITEM_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oEquipItemTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oObjInfo[string.Format(KCDefine.U_KEY_FMT_SKILL_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oSkillTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oObjInfo[string.Format(KCDefine.U_KEY_FMT_ABILITY_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oAbilityTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}
	}
	#endregion			// 함수
}

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

		m_eObjKinds = a_oObjSaleInfo[KCDefine.U_KEY_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjSaleInfo[KCDefine.U_KEY_OBJ_KINDS].AsInt : EObjKinds.NONE;
		m_ePrevObjKinds = a_oObjSaleInfo[KCDefine.U_KEY_PREV_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjSaleInfo[KCDefine.U_KEY_PREV_OBJ_KINDS].AsInt : EObjKinds.NONE;
		m_eNextObjKinds = a_oObjSaleInfo[KCDefine.U_KEY_NEXT_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjSaleInfo[KCDefine.U_KEY_NEXT_OBJ_KINDS].AsInt : EObjKinds.NONE;

		m_oPayTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oAcquireTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oObjSaleInfo[string.Format(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oPayTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oObjSaleInfo[string.Format(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oAcquireTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}
	}
	#endregion			// 함수
}

/** 객체 업그레이드 정보 */
[System.Serializable]
public partial struct STObjEnhanceInfo {
	public STCommonInfo m_stCommonInfo;

	public EObjKinds m_eObjKinds;
	public EObjKinds m_ePrevObjKinds;
	public EObjKinds m_eNextObjKinds;

	public Dictionary<ulong, STTargetInfo> m_oPayTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAcquireTargetInfoDict;

	#region 상수
	public static STObjEnhanceInfo INVALID = new STObjEnhanceInfo() {
		m_eObjKinds = EObjKinds.NONE, m_ePrevObjKinds = EObjKinds.NONE, m_eNextObjKinds = EObjKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public EObjType ObjType => (EObjType)((int)m_eObjKinds).ExKindsToType();
	public EObjKinds BaseObjKinds => (EObjKinds)((int)m_eObjKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STObjEnhanceInfo(SimpleJSON.JSONNode a_oObjSaleInfo) {
		m_stCommonInfo = new STCommonInfo(a_oObjSaleInfo);

		m_eObjKinds = a_oObjSaleInfo[KCDefine.U_KEY_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjSaleInfo[KCDefine.U_KEY_OBJ_KINDS].AsInt : EObjKinds.NONE;
		m_ePrevObjKinds = a_oObjSaleInfo[KCDefine.U_KEY_PREV_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjSaleInfo[KCDefine.U_KEY_PREV_OBJ_KINDS].AsInt : EObjKinds.NONE;
		m_eNextObjKinds = a_oObjSaleInfo[KCDefine.U_KEY_NEXT_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjSaleInfo[KCDefine.U_KEY_NEXT_OBJ_KINDS].AsInt : EObjKinds.NONE;

		m_oPayTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oAcquireTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oObjSaleInfo[string.Format(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oPayTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oObjSaleInfo[string.Format(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oAcquireTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}
	}
	#endregion			// 함수
}

/** 객체 정보 테이블 */
public partial class CObjInfoTable : CSingleton<CObjInfoTable> {
	#region 변수
	[Header("=====> BG Obj Info <=====")]
	[SerializeField] private List<STObjInfo> m_oBGObjInfoList = new List<STObjInfo>();
	[SerializeField] private List<STObjSaleInfo> m_oBGObjSaleInfoList = new List<STObjSaleInfo>();
	[SerializeField] private List<STObjEnhanceInfo> m_oBGObjEnhanceInfoList = new List<STObjEnhanceInfo>();

	[Header("=====> Norm Obj Info <=====")]
	[SerializeField] private List<STObjInfo> m_oNormObjInfoList = new List<STObjInfo>();
	[SerializeField] private List<STObjSaleInfo> m_oNormObjSaleInfoList = new List<STObjSaleInfo>();
	[SerializeField] private List<STObjEnhanceInfo> m_oNormObjEnhanceInfoList = new List<STObjEnhanceInfo>();

	[Header("=====> Overlay Obj Info <=====")]
	[SerializeField] private List<STObjInfo> m_oOverlayObjInfoList = new List<STObjInfo>();
	[SerializeField] private List<STObjSaleInfo> m_oOverlayObjSaleInfoList = new List<STObjSaleInfo>();
	[SerializeField] private List<STObjEnhanceInfo> m_oOverlayObjEnhanceInfoList = new List<STObjEnhanceInfo>();

	[Header("=====> Playable Obj Info <=====")]
	[SerializeField] private List<STObjInfo> m_oPlayableObjInfoList = new List<STObjInfo>();
	[SerializeField] private List<STObjSaleInfo> m_oPlayableObjSaleInfoList = new List<STObjSaleInfo>();
	[SerializeField] private List<STObjEnhanceInfo> m_oPlayableObjEnhanceInfoList = new List<STObjEnhanceInfo>();

	[Header("=====> Non Playable Obj Info <=====")]
	[SerializeField] private List<STObjInfo> m_oNonPlayableObjInfoList = new List<STObjInfo>();
	[SerializeField] private List<STObjSaleInfo> m_oNonPlayableObjSaleInfoList = new List<STObjSaleInfo>();
	[SerializeField] private List<STObjEnhanceInfo> m_oNonPlayableObjEnhanceInfoList = new List<STObjEnhanceInfo>();

	[Header("=====> Enemy Obj Info <=====")]
	[SerializeField] private List<STObjInfo> m_oEnemyObjInfoList = new List<STObjInfo>();
	[SerializeField] private List<STObjSaleInfo> m_oEnemyObjSaleInfoList = new List<STObjSaleInfo>();
	[SerializeField] private List<STObjEnhanceInfo> m_oEnemyObjEnhanceInfoList = new List<STObjEnhanceInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<EObjKinds, STObjInfo> ObjInfoDict { get; private set; } = new Dictionary<EObjKinds, STObjInfo>();
	public Dictionary<EObjKinds, STObjSaleInfo> ObjSaleInfoDict { get; private set; } = new Dictionary<EObjKinds, STObjSaleInfo>();
	public Dictionary<EObjKinds, STObjEnhanceInfo> ObjEnhanceInfoDict { get; private set; } = new Dictionary<EObjKinds, STObjEnhanceInfo>();

	private string ObjInfoTablePath {
		get {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_OBJ_INFO;
#else
			return KCDefine.U_TABLE_P_G_OBJ_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetObjInfos();
	}

	/** 객체 정보를 리셋한다 */
	public void ResetObjInfos() {
		this.ObjInfoDict.Clear();
		this.ObjSaleInfoDict.Clear();
		this.ObjEnhanceInfoDict.Clear();

		var oObjInfoList = new List<STObjInfo>(m_oBGObjInfoList);
		oObjInfoList.AddRange(m_oNormObjInfoList);
		oObjInfoList.AddRange(m_oOverlayObjInfoList);
		oObjInfoList.AddRange(m_oPlayableObjInfoList);
		oObjInfoList.AddRange(m_oNonPlayableObjInfoList);
		oObjInfoList.AddRange(m_oEnemyObjInfoList);

		var oObjSaleInfoList = new List<STObjSaleInfo>(m_oBGObjSaleInfoList);
		oObjSaleInfoList.ExAddVals(m_oNormObjSaleInfoList);
		oObjSaleInfoList.ExAddVals(m_oOverlayObjSaleInfoList);
		oObjSaleInfoList.ExAddVals(m_oPlayableObjSaleInfoList);
		oObjSaleInfoList.ExAddVals(m_oNonPlayableObjSaleInfoList);
		oObjSaleInfoList.ExAddVals(m_oEnemyObjSaleInfoList);

		var oObjEnhanceInfoList = new List<STObjEnhanceInfo>(m_oBGObjEnhanceInfoList);
		oObjEnhanceInfoList.ExAddVals(m_oNormObjEnhanceInfoList);
		oObjEnhanceInfoList.ExAddVals(m_oOverlayObjEnhanceInfoList);
		oObjEnhanceInfoList.ExAddVals(m_oPlayableObjEnhanceInfoList);
		oObjEnhanceInfoList.ExAddVals(m_oNonPlayableObjEnhanceInfoList);
		oObjEnhanceInfoList.ExAddVals(m_oEnemyObjEnhanceInfoList);

		for(int i = 0; i < oObjInfoList.Count; ++i) {
			this.ObjInfoDict.TryAdd(oObjInfoList[i].m_eObjKinds, oObjInfoList[i]);
		}

		for(int i = 0; i < oObjSaleInfoList.Count; ++i) {
			this.ObjSaleInfoDict.TryAdd(oObjSaleInfoList[i].m_eObjKinds, oObjSaleInfoList[i]);
		}

		for(int i = 0; i < oObjEnhanceInfoList.Count; ++i) {
			this.ObjEnhanceInfoDict.TryAdd(oObjEnhanceInfoList[i].m_eObjKinds, oObjEnhanceInfoList[i]);
		}
	}

	/** 객체 정보를 리셋한다 */
	public void ResetObjInfos(string a_oJSONStr) {
		this.ResetObjInfos();
		this.DoLoadObjInfos(a_oJSONStr);
	}

	/** 객체 정보를 반환한다 */
	public STObjInfo GetObjInfo(EObjKinds a_eObjKinds) {
		bool bIsValid = this.TryGetObjInfo(a_eObjKinds, out STObjInfo stObjInfo);
		CAccess.Assert(bIsValid);

		return stObjInfo;
	}

	/** 객체 판매 정보를 반환한다 */
	public STObjSaleInfo GetObjSaleInfo(EObjKinds a_eObjKinds) {
		bool bIsValid = this.TryGetObjSaleInfo(a_eObjKinds, out STObjSaleInfo stObjSaleInfo);
		CAccess.Assert(bIsValid);

		return stObjSaleInfo;
	}

	/** 객체 업그레이드 정보를 반환한다 */
	public STObjEnhanceInfo GetObjEnhanceInfo(EObjKinds a_eObjKinds) {
		bool bIsValid = this.TryGetObjEnhanceInfo(a_eObjKinds, out STObjEnhanceInfo stObjEnhanceInfo);
		CAccess.Assert(bIsValid);

		return stObjEnhanceInfo;
	}

	/** 객체 정보를 반환한다 */
	public bool TryGetObjInfo(EObjKinds a_eObjKinds, out STObjInfo a_stOutObjInfo) {
		a_stOutObjInfo = this.ObjInfoDict.GetValueOrDefault(a_eObjKinds, STObjInfo.INVALID);
		return this.ObjInfoDict.ContainsKey(a_eObjKinds);
	}

	/** 객체 판매 정보를 반환한다 */
	public bool TryGetObjSaleInfo(EObjKinds a_eObjKinds, out STObjSaleInfo a_stOutObjSaleInfo) {
		a_stOutObjSaleInfo = this.ObjSaleInfoDict.GetValueOrDefault(a_eObjKinds, STObjSaleInfo.INVALID);
		return this.ObjSaleInfoDict.ContainsKey(a_eObjKinds);
	}

	/** 객체 업그레이드 정보를 반환한다 */
	public bool TryGetObjEnhanceInfo(EObjKinds a_eObjKinds, out STObjEnhanceInfo a_stOutObjEnhanceInfo) {
		a_stOutObjEnhanceInfo = this.ObjEnhanceInfoDict.GetValueOrDefault(a_eObjKinds, STObjEnhanceInfo.INVALID);
		return this.ObjEnhanceInfoDict.ContainsKey(a_eObjKinds);
	}

	/** 객체 정보를 로드한다 */
	public (Dictionary<EObjKinds, STObjInfo>, Dictionary<EObjKinds, STObjSaleInfo>, Dictionary<EObjKinds, STObjEnhanceInfo>) LoadObjInfos() {
		this.ResetObjInfos();
		return this.LoadObjInfos(this.ObjInfoTablePath);
	}

	/** 객체 정보를 로드한다 */
	private (Dictionary<EObjKinds, STObjInfo>, Dictionary<EObjKinds, STObjSaleInfo>, Dictionary<EObjKinds, STObjEnhanceInfo>) LoadObjInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadObjInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			return this.DoLoadObjInfos(CResManager.Inst.GetRes<TextAsset>(a_oFilePath).text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 객체 정보를 로드한다 */
	private (Dictionary<EObjKinds, STObjInfo>, Dictionary<EObjKinds, STObjSaleInfo>, Dictionary<EObjKinds, STObjEnhanceInfo>) DoLoadObjInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		var oJSONNode = SimpleJSON.JSONNode.Parse(a_oJSONStr);

		var oObjInfosList = new List<SimpleJSON.JSONNode>() {
			oJSONNode[KCDefine.U_KEY_BG], oJSONNode[KCDefine.U_KEY_NORM], oJSONNode[KCDefine.U_KEY_OVERLAY], oJSONNode[KCDefine.U_KEY_PLAYABLE], oJSONNode[KCDefine.U_KEY_NON_PLAYABLE], oJSONNode[KCDefine.U_KEY_ENEMY]
		};

		var oObjSaleInfosList = new List<SimpleJSON.JSONNode>() {
			oJSONNode[KCDefine.U_KEY_BG_SALE], oJSONNode[KCDefine.U_KEY_NORM_SALE], oJSONNode[KCDefine.U_KEY_OVERLAY_SALE], oJSONNode[KCDefine.U_KEY_PLAYABLE_SALE], oJSONNode[KCDefine.U_KEY_NON_PLAYABLE_SALE], oJSONNode[KCDefine.U_KEY_ENEMY_SALE]
		};

		var oObjEnhanceInfosList = new List<SimpleJSON.JSONNode>() {
			oJSONNode[KCDefine.U_KEY_BG_ENHANCE], oJSONNode[KCDefine.U_KEY_NORM_ENHANCE], oJSONNode[KCDefine.U_KEY_OVERLAY_ENHANCE], oJSONNode[KCDefine.U_KEY_PLAYABLE_ENHANCE], oJSONNode[KCDefine.U_KEY_NON_PLAYABLE_ENHANCE], oJSONNode[KCDefine.U_KEY_ENEMY_ENHANCE]
		};

		for(int i = 0; i < oObjInfosList.Count; ++i) {
			for(int j = 0; j < oObjInfosList[i].Count; ++j) {
				var stObjInfo = new STObjInfo(oObjInfosList[i][j]);

				// 객체 정보 추가 가능 할 경우
				if(stObjInfo.m_eObjKinds.ExIsValid() && (!this.ObjInfoDict.ContainsKey(stObjInfo.m_eObjKinds) || oObjInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.ObjInfoDict.ExReplaceVal(stObjInfo.m_eObjKinds, stObjInfo);
				}
			}
		}

		for(int i = 0; i < oObjSaleInfosList.Count; ++i) {
			for(int j = 0; j < oObjSaleInfosList[i].Count; ++j) {
				var stObjSaleInfo = new STObjSaleInfo(oObjSaleInfosList[i][j]);

				// 객체 판매 정보 추가 가능 할 경우
				if(stObjSaleInfo.m_eObjKinds.ExIsValid() && (!this.ObjSaleInfoDict.ContainsKey(stObjSaleInfo.m_eObjKinds) || oObjSaleInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.ObjSaleInfoDict.ExReplaceVal(stObjSaleInfo.m_eObjKinds, stObjSaleInfo);
				}
			}
		}

		for(int i = 0; i < oObjEnhanceInfosList.Count; ++i) {
			for(int j = 0; j < oObjEnhanceInfosList[i].Count; ++j) {
				var stObjEnhanceInfo = new STObjEnhanceInfo(oObjEnhanceInfosList[i][j]);

				// 객체 업그레이드 정보 추가 가능 할 경우
				if(stObjEnhanceInfo.m_eObjKinds.ExIsValid() && (!this.ObjSaleInfoDict.ContainsKey(stObjEnhanceInfo.m_eObjKinds) || oObjEnhanceInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.ObjEnhanceInfoDict.ExReplaceVal(stObjEnhanceInfo.m_eObjKinds, stObjEnhanceInfo);
				}
			}
		}

		return (this.ObjInfoDict, this.ObjSaleInfoDict, this.ObjEnhanceInfoDict);
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
