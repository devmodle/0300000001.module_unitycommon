using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 서브 중첩 씬 관리자
public partial class CSubOverlaySceneManager : COverlaySceneManager {
	#region 프로퍼티
	public override STSortingOrderInfo UIsCanvasSortingOrderInfo => KDefine.G_SORTING_OI_OVERLAY_SCENE_UIS_CANVAS;
	public override STSortingOrderInfo ObjsCanvasSortingOrderInfo => KDefine.G_SORTING_OI_OVERLAY_SCENE_OBJS_CANVAS;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		
		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			this.SetupAwake();
		}
	}

	//! 초기화
	public override void Start() {
		base.Start();

		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			this.SetupStart();
			this.UpdateUIsState();
		}
	}

	//! 상점 팝업을 출력한다
	public void ShowStorePopup() {
		Func.ShowStorePopup(this.SubPopupUIs, (a_oSender) => {
			var oSaleProductInfoList = new List<STSaleProductInfo>();

			for(int i = 0; i < KDefine.G_KINDS_STORE_POPUP_PRODUCTS.Length; ++i) {
				var eSaleProductKinds = KDefine.G_KINDS_STORE_POPUP_PRODUCTS[i];
				oSaleProductInfoList.Add(CSaleProductInfoTable.Inst.GetSaleProductInfo(eSaleProductKinds));
			}

			var stParams = new CStorePopup.STParams() {
				m_oSaleProductInfoList = oSaleProductInfoList
			};

			var stCallbackParams = new CStorePopup.STCallbackParams() {
#if ADS_MODULE_ENABLE
				m_oAdsCallback = (a_oAdsSender, a_stRewardItemInfo, a_bIsSuccess) => this.UpdateUIsState(),
#endif			// #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
				m_oPurchaseCallback = (a_oPurchaseSender, a_oProductID, a_bIsSuccess) => this.UpdateUIsState(),
				m_oRestoreCallback = (a_oRestoreSender, a_oProductList, a_bIsSuccess) => this.UpdateUIsState()
#endif			// #if PURCHASE_MODULE_ENABLE
			};

			var oStorePopup = a_oSender as CStorePopup;
			oStorePopup.Init(stParams, stCallbackParams);
		});
	}

	//! 씬을 설정한다
	private void SetupAwake() {
#if DEBUG || DEVELOPMENT_BUILD
		this.SetupTestUIs();
#endif			// #if DEBUG || DEVELOPMENT_BUILD
	}

	//! 씬을 설정한다
	private void SetupStart() {
		// Do Something
	}

	//! UI 상태를 갱신한다
	private void UpdateUIsState() {
		var oSubTitleSceneManager = CSceneManager.GetSubSceneManager<CSubTitleSceneManager>(KCDefine.B_SCENE_N_TITLE);
		oSubTitleSceneManager?.gameObject.ExSendMsg(KCDefine.U_FUNC_N_UPDATE_UIS_STATE, null);

		var oSubGameSceneManager = CSceneManager.GetSubSceneManager<CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME);
		oSubGameSceneManager?.gameObject.ExSendMsg(KCDefine.U_FUNC_N_UPDATE_UIS_STATE, null);

#if DEBUG || DEVELOPMENT_BUILD
		this.UpdateTestUIsState();
#endif			// #if DEBUG || DEVELOPMENT_BUILD
	}
	#endregion			// 함수

	#region 조건부 함수
#if DEBUG || DEVELOPMENT_BUILD
	//! 테스트 UI 를 설정한다
	private void SetupTestUIs() {
		// Do Something
	}

	//! 테스트 UI 상태를 갱신한다
	private void UpdateTestUIsState() {
		// Do Something
	}
#endif			// #if DEBUG || DEVELOPMENT_BUILD
	#endregion			// 조건부 함수

	#region 추가 변수

	#endregion			// 추가 변수
	
	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if NEVER_USE_THIS
