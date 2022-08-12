#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif			// #if PURCHASE_MODULE_ENABLE

/** 상품 교환 정보 */
[System.Serializable]
public partial struct STProductTradeInfo {
	public STCommonInfo m_stCommonInfo;
	public int m_nProductIdx;

	public EProductKinds m_eProductKinds;
	public EProductKinds m_ePrevProductKinds;
	public EProductKinds m_eNextProductKinds;
	public EPurchaseType m_ePurchaseType;

	public Dictionary<ulong, STTargetInfo> m_oPayTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAcquireTargetInfoDict;

	#region 상수
	public static STProductTradeInfo INVALID = new STProductTradeInfo() {
		m_eProductKinds = EProductKinds.NONE, m_ePrevProductKinds = EProductKinds.NONE, m_eNextProductKinds = EProductKinds.NONE, m_ePurchaseType = EPurchaseType.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public EProductType ProductType => (EProductType)((int)m_eProductKinds).ExKindsToType();
	public EProductKinds BaseProductKinds => (EProductKinds)((int)m_eProductKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STProductTradeInfo(SimpleJSON.JSONNode a_oProductTradeInfo) {
		m_stCommonInfo = new STCommonInfo(a_oProductTradeInfo);
		m_nProductIdx = a_oProductTradeInfo[KCDefine.U_KEY_PRODUCT_IDX].AsInt;

		m_eProductKinds = a_oProductTradeInfo[KCDefine.U_KEY_PRODUCT_KINDS].ExIsValid() ? (EProductKinds)a_oProductTradeInfo[KCDefine.U_KEY_PRODUCT_KINDS].AsInt : EProductKinds.NONE;
		m_ePrevProductKinds = a_oProductTradeInfo[KCDefine.U_KEY_PREV_PRODUCT_KINDS].ExIsValid() ? (EProductKinds)a_oProductTradeInfo[KCDefine.U_KEY_PREV_PRODUCT_KINDS].AsInt : EProductKinds.NONE;
		m_eNextProductKinds = a_oProductTradeInfo[KCDefine.U_KEY_NEXT_PRODUCT_KINDS].ExIsValid() ? (EProductKinds)a_oProductTradeInfo[KCDefine.U_KEY_NEXT_PRODUCT_KINDS].AsInt : EProductKinds.NONE;
		m_ePurchaseType = a_oProductTradeInfo[KCDefine.U_KEY_PURCHASE_TYPE].ExIsValid() ? (EPurchaseType)a_oProductTradeInfo[KCDefine.U_KEY_PURCHASE_TYPE].AsInt : EPurchaseType.NONE;

		m_oPayTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oAcquireTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oProductTradeInfo[string.Format(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oPayTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oProductTradeInfo[string.Format(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			if(stTargetInfo.m_eTargetKinds.ExIsValid() && stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) { m_oAcquireTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo); }
		}
	}
	#endregion			// 함수
}

/** 상품 교환 정보 테이블 */
public partial class CProductTradeInfoTable : CSingleton<CProductTradeInfoTable> {
	#region 프로퍼티
	public Dictionary<EProductKinds, STProductTradeInfo> BuyProductTradeInfoDict { get; private set; } = new Dictionary<EProductKinds, STProductTradeInfo>();

	private string ProductTradeInfoTablePath {
		get {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_PRODUCT_INFO;
#else
			return KCDefine.U_TABLE_P_G_PRODUCT_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetProductTradeInfos();
	}

	/** 상품 교환 정보를 리셋한다 */
	public void ResetProductTradeInfos() {
		this.BuyProductTradeInfoDict.Clear();
	}

	/** 상품 교환 정보를 리셋한다 */
	public void ResetProductTradeInfos(string a_oJSONStr) {
		this.ResetProductTradeInfos();
		this.DoLoadProductTradeInfos(a_oJSONStr);
	}

	/** 구입 상품 교환 정보를 반환한다 */
	public STProductTradeInfo GetBuyProductTradeTradeInfo(int a_nProductIdx) {
		bool bIsValid = this.TryGetBuyProductTradeInfo(a_nProductIdx, out STProductTradeInfo stProductTradeInfo);
		CAccess.Assert(bIsValid);

		return stProductTradeInfo;
	}
	
	/** 구입 상품 교환 정보를 반환한다 */
	public STProductTradeInfo GetBuyProductTradeTradeInfo(EProductKinds a_eProductKinds) {
		bool bIsValid = this.TryGetBuyProductTradeInfo(a_eProductKinds, out STProductTradeInfo stProductTradeInfo);
		CAccess.Assert(bIsValid);

		return stProductTradeInfo;
	}

	/** 구입 상품 교환 정보를 반환한다 */
	public bool TryGetBuyProductTradeInfo(int a_nProductIdx, out STProductTradeInfo a_stOutProductTradeInfo) {
		a_stOutProductTradeInfo = this.BuyProductTradeInfoDict.ExGetVal((a_stProductTradeInfo) => a_stProductTradeInfo.m_nProductIdx == a_nProductIdx, STProductTradeInfo.INVALID);
		return !a_stOutProductTradeInfo.Equals(STProductTradeInfo.INVALID);
	}

	/** 구입 상품 교환 정보를 반환한다 */
	public bool TryGetBuyProductTradeInfo(EProductKinds a_eProductKinds, out STProductTradeInfo a_stOutProductTradeInfo) {
		a_stOutProductTradeInfo = this.BuyProductTradeInfoDict.GetValueOrDefault(a_eProductKinds, STProductTradeInfo.INVALID);
		return this.BuyProductTradeInfoDict.ContainsKey(a_eProductKinds);
	}
	
	/** 상품 교환 정보를 로드한다 */
	public Dictionary<EProductKinds, STProductTradeInfo> LoadProductTradeInfos() {
		this.ResetProductTradeInfos();
		return this.LoadProductTradeInfos(this.ProductTradeInfoTablePath);
	}

	/** 상품 교환 정보를 로드한다 */
	private Dictionary<EProductKinds, STProductTradeInfo> LoadProductTradeInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadProductTradeInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			return this.DoLoadProductTradeInfos(CResManager.Inst.GetRes<TextAsset>(a_oFilePath).text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 상품 교환 정보를 로드한다 */
	private Dictionary<EProductKinds, STProductTradeInfo> DoLoadProductTradeInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());

		var oJSONNode = SimpleJSON.JSONNode.Parse(a_oJSONStr);
		var oBuyProductTradeInfosList = new List<SimpleJSON.JSONNode>();

		for(int i = 0; i < KDefine.G_KEY_PRODUCT_TIT_BUY_TRADE_INFOS_LIST.Count; ++i) {
			oBuyProductTradeInfosList.ExAddVal(oJSONNode[KDefine.G_KEY_PRODUCT_TIT_BUY_TRADE_INFOS_LIST[i]]);
		}

		for(int i = 0; i < oBuyProductTradeInfosList.Count; ++i) {
			for(int j = 0; j < oBuyProductTradeInfosList[i].Count; ++j) {
				var stProductTradeInfo = new STProductTradeInfo(oBuyProductTradeInfosList[i][j]);

				// 구입 상품 교환 정보 추가 가능 할 경우
				if(stProductTradeInfo.m_eProductKinds.ExIsValid() && (!this.BuyProductTradeInfoDict.ContainsKey(stProductTradeInfo.m_eProductKinds) || oBuyProductTradeInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.BuyProductTradeInfoDict.ExReplaceVal(stProductTradeInfo.m_eProductKinds, stProductTradeInfo);
				}
			}
		}

		return this.BuyProductTradeInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
