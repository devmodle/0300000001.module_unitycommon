using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif			// #if PURCHASE_MODULE_ENABLE

/** 상품 판매 정보 */
[System.Serializable]
public partial struct STProductSaleInfo {
	public STCommonInfo m_stCommonInfo;
	public int m_nTableIdx;

	public EProductSaleKinds m_eProductSaleKinds;
	public EProductSaleKinds m_ePrevProductSaleKinds;
	public EProductSaleKinds m_eNextProductSaleKinds;
	public EPurchaseType m_ePurchaseType;

	public Dictionary<ulong, STTargetInfo> m_oPayTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAcquireTargetInfoDict;

	#region 상수
	public static STProductSaleInfo INVALID = new STProductSaleInfo() {
		m_eProductSaleKinds = EProductSaleKinds.NONE, m_ePrevProductSaleKinds = EProductSaleKinds.NONE, m_eNextProductSaleKinds = EProductSaleKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public EProductSaleType ProductSaleType => (EProductSaleType)((int)m_eProductSaleKinds).ExKindsToType();
	public EProductSaleKinds BaseProductSaleKinds => (EProductSaleKinds)((int)m_eProductSaleKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STProductSaleInfo(SimpleJSON.JSONNode a_oProductSaleInfo) {
		m_stCommonInfo = new STCommonInfo(a_oProductSaleInfo);
		m_nTableIdx = a_oProductSaleInfo[KCDefine.U_KEY_TABLE_IDX].AsInt;

		m_eProductSaleKinds = a_oProductSaleInfo[KCDefine.U_KEY_PRODUCT_SALE_KINDS].ExIsValid() ? (EProductSaleKinds)a_oProductSaleInfo[KCDefine.U_KEY_PRODUCT_SALE_KINDS].AsInt : EProductSaleKinds.NONE;
		m_ePrevProductSaleKinds = a_oProductSaleInfo[KCDefine.U_KEY_PREV_PRODUCT_SALE_KINDS].ExIsValid() ? (EProductSaleKinds)a_oProductSaleInfo[KCDefine.U_KEY_PREV_PRODUCT_SALE_KINDS].AsInt : EProductSaleKinds.NONE;
		m_eNextProductSaleKinds = a_oProductSaleInfo[KCDefine.U_KEY_NEXT_PRODUCT_SALE_KINDS].ExIsValid() ? (EProductSaleKinds)a_oProductSaleInfo[KCDefine.U_KEY_NEXT_PRODUCT_SALE_KINDS].AsInt : EProductSaleKinds.NONE;
		m_ePurchaseType = a_oProductSaleInfo[KCDefine.U_KEY_PURCHASE_TYPE].ExIsValid() ? (EPurchaseType)a_oProductSaleInfo[KCDefine.U_KEY_PURCHASE_TYPE].AsInt : EPurchaseType.NONE;

		m_oPayTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oAcquireTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oProductSaleInfo[string.Format(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oPayTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_ABILITY_VAL_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oProductSaleInfo[string.Format(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oAcquireTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}
	}
	#endregion			// 함수
}

/** 상품 판매 정보 테이블 */
public partial class CProductSaleInfoTable : CScriptableObj<CProductSaleInfoTable> {
	#region 변수
	[Header("=====> Pkgs Product Sale Info <=====")]
	[SerializeField] private List<STProductSaleInfo> m_oPkgsProductSaleInfoList = new List<STProductSaleInfo>();

	[Header("=====> Single Product Sale Info <=====")]
	[SerializeField] private List<STProductSaleInfo> m_oSingleProductSaleInfoList = new List<STProductSaleInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<EProductSaleKinds, STProductSaleInfo> ProductSaleInfoDict { get; private set; } = new Dictionary<EProductSaleKinds, STProductSaleInfo>();

	private string ProductSaleInfoTablePath {
		get {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_PRODUCT_SALE_INFO;
#else
			return KCDefine.U_TABLE_P_G_PRODUCT_SALE_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetProductSaleInfos();
	}

	/** 상품 판매 정보를 리셋한다 */
	public void ResetProductSaleInfos() {
		this.ProductSaleInfoDict.Clear();

		var oProductSaleInfoList = new List<STProductSaleInfo>(m_oPkgsProductSaleInfoList);
		oProductSaleInfoList.ExAddVals(m_oSingleProductSaleInfoList);

		for(int i = 0; i < oProductSaleInfoList.Count; ++i) {
			this.ProductSaleInfoDict.TryAdd(oProductSaleInfoList[i].m_eProductSaleKinds, oProductSaleInfoList[i]);
		}
	}

	/** 상품 판매 정보를 리셋한다 */
	public void ResetProductSaleInfos(string a_oJSONStr) {
		this.ResetProductSaleInfos();
		this.DoLoadProductSaleInfos(a_oJSONStr);
	}

	/** 상품 판매 정보를 반환한다 */
	public STProductSaleInfo GetProductSaleInfo(int a_nTableIdx) {
		bool bIsValid = this.TryGetProductSaleInfo(a_nTableIdx, out STProductSaleInfo stProductSaleInfo);
		CAccess.Assert(bIsValid);

		return stProductSaleInfo;
	}
	
	/** 상품 판매 정보를 반환한다 */
	public STProductSaleInfo GetProductSaleInfo(EProductSaleKinds a_eProductSaleKinds) {
		bool bIsValid = this.TryGetProductSaleInfo(a_eProductSaleKinds, out STProductSaleInfo stProductSaleInfo);
		CAccess.Assert(bIsValid);

		return stProductSaleInfo;
	}

	/** 지불 타겟 정보를 반환한다 */
	public STTargetInfo GetPayTargetInfo(EProductSaleKinds a_eProductSaleKinds, ETargetKinds a_eTargetKinds, int a_nKinds) {
		bool bIsValid = this.TryGetPayTargetInfo(a_eProductSaleKinds, a_eTargetKinds, a_nKinds, out STTargetInfo stPayTargetInfo);
		CAccess.Assert(bIsValid);

		return stPayTargetInfo;
	}

	/** 획득 타겟 정보를 반환한다 */
	public STTargetInfo GetAcquireTargetInfo(EProductSaleKinds a_eProductSaleKinds, ETargetKinds a_eTargetKinds, int a_nKinds) {
		bool bIsValid = this.TryGetAcquireTargetInfo(a_eProductSaleKinds, a_eTargetKinds, a_nKinds, out STTargetInfo stAcquireTargetInfo);
		CAccess.Assert(bIsValid);

		return stAcquireTargetInfo;
	}

	/** 상품 판매 정보를 반환한다 */
	public bool TryGetProductSaleInfo(int a_nTableIdx, out STProductSaleInfo a_stOutProductSaleInfo) {
		a_stOutProductSaleInfo = this.ProductSaleInfoDict.ExGetVal((a_stProductSaleInfo) => a_stProductSaleInfo.m_nTableIdx == a_nTableIdx, STProductSaleInfo.INVALID);
		return !a_stOutProductSaleInfo.Equals(STProductSaleInfo.INVALID);
	}

	/** 상품 판매 정보를 반환한다 */
	public bool TryGetProductSaleInfo(EProductSaleKinds a_eProductSaleKinds, out STProductSaleInfo a_stOutProductSaleInfo) {
		a_stOutProductSaleInfo = this.ProductSaleInfoDict.GetValueOrDefault(a_eProductSaleKinds, STProductSaleInfo.INVALID);
		return this.ProductSaleInfoDict.ContainsKey(a_eProductSaleKinds);
	}

	/** 지불 타겟 정보를 반환한다 */
	public bool TryGetPayTargetInfo(EProductSaleKinds a_eProductSaleKinds, ETargetKinds a_eTargetKinds, int a_nKinds, out STTargetInfo a_stOutPayTargetInfo) {
		a_stOutPayTargetInfo = this.TryGetProductSaleInfo(a_eProductSaleKinds, out STProductSaleInfo stProductSaleInfo) ? stProductSaleInfo.m_oPayTargetInfoDict.GetValueOrDefault(Factory.MakeUniqueTargetInfoID(a_eTargetKinds, a_nKinds), STTargetInfo.INVALID) : STTargetInfo.INVALID;
		return !a_stOutPayTargetInfo.Equals(STTargetInfo.INVALID);
	}

	/** 획득 타겟 정보를 반환한다 */
	public bool TryGetAcquireTargetInfo(EProductSaleKinds a_eProductSaleKinds, ETargetKinds a_eTargetKinds, int a_nKinds, out STTargetInfo a_stOutAcquireTargetInfo) {
		a_stOutAcquireTargetInfo = this.TryGetProductSaleInfo(a_eProductSaleKinds, out STProductSaleInfo stProductSaleInfo) ? stProductSaleInfo.m_oAcquireTargetInfoDict.GetValueOrDefault(Factory.MakeUniqueTargetInfoID(a_eTargetKinds, a_nKinds), STTargetInfo.INVALID) : STTargetInfo.INVALID;
		return !a_stOutAcquireTargetInfo.Equals(STTargetInfo.INVALID);
	}

	/** 상품 판매 정보를 로드한다 */
	public Dictionary<EProductSaleKinds, STProductSaleInfo> LoadProductSaleInfos() {
		this.ResetProductSaleInfos();
		return this.LoadProductSaleInfos(this.ProductSaleInfoTablePath);
	}

	/** 상품 판매 정보를 로드한다 */
	private Dictionary<EProductSaleKinds, STProductSaleInfo> LoadProductSaleInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadProductSaleInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			return this.DoLoadProductSaleInfos(CResManager.Inst.GetRes<TextAsset>(a_oFilePath).text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 상품 판매 정보를 로드한다 */
	private Dictionary<EProductSaleKinds, STProductSaleInfo> DoLoadProductSaleInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		var oJSONNode = SimpleJSON.JSONNode.Parse(a_oJSONStr);

		var oProductSaleInfosList = new List<SimpleJSON.JSONNode>() {
			oJSONNode[KCDefine.U_KEY_PKGS], oJSONNode[KCDefine.U_KEY_SINGLE]
		};

		for(int i = 0; i < oProductSaleInfosList.Count; ++i) {
			for(int j = 0; j < oProductSaleInfosList[i].Count; ++j) {
				var stProductSaleInfo = new STProductSaleInfo(oProductSaleInfosList[i][j]);

				// 상품 판매 정보가 추가 가능 할 경우
				if(stProductSaleInfo.m_eProductSaleKinds.ExIsValid() && (!this.ProductSaleInfoDict.ContainsKey(stProductSaleInfo.m_eProductSaleKinds) || oProductSaleInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.ProductSaleInfoDict.ExReplaceVal(stProductSaleInfo.m_eProductSaleKinds, stProductSaleInfo);
				}
			}
		}

		return this.ProductSaleInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
