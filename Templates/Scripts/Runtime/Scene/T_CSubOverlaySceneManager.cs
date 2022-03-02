using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if SCRIPT_TEMPLATE_ONLY
#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif			// #if PURCHASE_MODULE_ENABLE

#if RUNTIME_TEMPLATES_MODULE_ENABLE
/** 서브 중첩 씬 관리자 */
public partial class CSubOverlaySceneManager : COverlaySceneManager {
#if DEBUG || DEVELOPMENT_BUILD
	/** 테스트 UI */
	[System.Serializable]
	private struct STTestUIs {
		// Do Something
	}
#endif			// #if DEBUG || DEVELOPMENT_BUILD

	#region 변수
	/** =====> UI <===== */
#if DEBUG || DEVELOPMENT_BUILD
	[SerializeField] private STTestUIs m_stTestUIs;
#endif			// #if DEBUG || DEVELOPMENT_BUILD
	#endregion			// 변수

	#region 추가 변수

	#endregion			// 추가 변수
	
	#region 프로퍼티
	public TMP_Text NumCoinsText { get; private set; } = null;
	public Button StoreBtn { get; private set; } = null;

	public override STSortingOrderInfo UIsCanvasSortingOrderInfo => KDefine.G_SORTING_OI_OVERLAY_SCENE_UIS_CANVAS;
	#endregion			// 프로퍼티

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		
		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			this.SetupAwake();
		}
	}

	/** 초기화 */
	public override void Start() {
		base.Start();

		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			this.SetupStart();
			this.UpdateUIsState();
		}
	}

	/** 상점 팝업을 출력한다 */
	public void ShowStorePopup() {
#if RUNTIME_TEMPLATES_MODULE_ENABLE
		Func.ShowStorePopup(this.PopupUIs, (a_oSender) => {
			var oSaleProductInfoList = new List<STSaleProductInfo>();

			for(int i = 0; i < KDefine.G_SALE_PRODUCT_KINDS_PRODUCT_LIST.Count; ++i) {
				var eSaleProductKinds = KDefine.G_SALE_PRODUCT_KINDS_PRODUCT_LIST[i];
				oSaleProductInfoList.Add(CSaleProductInfoTable.Inst.GetSaleProductInfo(eSaleProductKinds));
			}

			(a_oSender as CStorePopup).Init(new CStorePopup.STParams() {
				m_oSaleProductInfoList = oSaleProductInfoList,

#if ADS_MODULE_ENABLE
				m_oAdsCallbackDictA = new Dictionary<CStorePopup.ECallback, System.Action<CAdsManager, STAdsRewardInfo, bool>>() {
					[CStorePopup.ECallback.ADS] = (a_oAdsSender, a_stAdsRewardInfo, a_bIsSuccess) => this.UpdateUIsState()
				},
#endif			// #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
				m_oPurchaseCallbackDictA = new Dictionary<CStorePopup.ECallback, System.Action<CPurchaseManager, string, bool>>() {
					[CStorePopup.ECallback.PURCHASE] = (a_oPurchaseSender, a_oProductID, a_bIsSuccess) => this.UpdateUIsState()
				},

				m_oPurchaseCallbackDictB = new Dictionary<CStorePopup.ECallback, System.Action<CPurchaseManager, List<Product>, bool>>() {
					[CStorePopup.ECallback.RESTORE] = (a_oRestoreSender, a_oProductList, a_bIsSuccess) => this.UpdateUIsState()
				}
#endif			// #if PURCHASE_MODULE_ENABLE
			});
		});
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
	}

	/** 씬을 설정한다 */
	private void SetupAwake() {
		// 텍스트를 설정한다
		this.NumCoinsText = this.UIsBase.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_NUM_COINS_TEXT);

		// 버튼을 설정한다
		this.StoreBtn = this.UIsBase.ExFindComponent<Button>(KCDefine.U_OBJ_N_STORE_BTN);
		this.StoreBtn?.onClick.AddListener(this.OnTouchStoreBtn);

#if DEBUG || DEVELOPMENT_BUILD
		this.SetupTestUIs();
#endif			// #if DEBUG || DEVELOPMENT_BUILD
	}

	/** 씬을 설정한다 */
	private void SetupStart() {
		// Do Something
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		var oSubTitleSceneManager = CSceneManager.GetSceneManager<CSubTitleSceneManager>(KCDefine.B_SCENE_N_TITLE);
		oSubTitleSceneManager?.gameObject.ExSendMsg(KCDefine.U_FUNC_N_UPDATE_UIS_STATE, null, false);

		var oSubMainSceneManager = CSceneManager.GetSceneManager<CSubMainSceneManager>(KCDefine.B_SCENE_N_MAIN);
		oSubMainSceneManager?.gameObject.ExSendMsg(KCDefine.U_FUNC_N_UPDATE_UIS_STATE, null, false);

		var oSubGameSceneManager = CSceneManager.GetSceneManager<CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME);
		oSubGameSceneManager?.gameObject.ExSendMsg(KCDefine.U_FUNC_N_UPDATE_UIS_STATE, null, false);

		// 텍스트를 갱신한다
		this.NumCoinsText.ExSetText($"{CUserInfoStorage.Inst.UserInfo.NumCoins}", EFontSet.A, false);

#if DEBUG || DEVELOPMENT_BUILD
		this.UpdateTestUIsState();
#endif			// #if DEBUG || DEVELOPMENT_BUILD
	}

	/** 상점 버튼을 눌렀을 경우 */
	private void OnTouchStoreBtn() {
		this.ShowStorePopup();
	}
	#endregion			// 함수

	#region 조건부 함수
#if DEBUG || DEVELOPMENT_BUILD
	/** 테스트 UI 를 설정한다 */
	private void SetupTestUIs() {
		// Do Something
	}

	/** 테스트 UI 상태를 갱신한다 */
	private void UpdateTestUIsState() {
		// Do Something
	}
#endif			// #if DEBUG || DEVELOPMENT_BUILD
	#endregion			// 조건부 함수

	#region 추가 함수
#if DEBUG || DEVELOPMENT_BUILD

#endif			// #if DEBUG || DEVELOPMENT_BUILD
	#endregion			// 추가 함수
}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
