#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif			// #if PURCHASE_MODULE_ENABLE

namespace OverlayScene {
	/** 서브 중첩 씬 관리자 */
	public partial class CSubOverlaySceneManager : COverlaySceneManager {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			PURCHASE_PRODUCT_ID,
			NUM_COINS_TEXT,
			STORE_BTN,
			[HideInInspector] MAX_VAL
		}

		/** 콜백 */
		private enum ECallback {
			NONE = -1,
			PURCHASE,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		private Dictionary<EKey, string> m_oStrDict = new Dictionary<EKey, string>();
		
		/** =====> UI <===== */
		private Dictionary<EKey, TMP_Text> m_oTextDict = new Dictionary<EKey, TMP_Text>();
		private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>();

#if PURCHASE_MODULE_ENABLE
		private Dictionary<ECallback, System.Action<CPurchaseManager, string, bool>> m_oCallbackDict = new Dictionary<ECallback, System.Action<CPurchaseManager, string, bool>>();
#endif			// #if PURCHASE_MODULE_ENABLE
		#endregion			// 변수

		#region 프로퍼티
		public override STSortingOrderInfo UIsCanvasSortingOrderInfo => KDefine.G_SORTING_OI_OVERLAY_SCENE_UIS_CANVAS;
		#endregion			// 프로퍼티

		#region 함수
		/** 상점 팝업을 출력한다 */
		public void ShowStorePopup() {
			Func.ShowStorePopup(CSceneManager.ActiveScenePopupUIs, (a_oSender) => {
				var oBuyProductTradeInfoDict = new Dictionary<EProductKinds, STProductTradeInfo>();

				for(int i = 0; i < KDefine.G_PRODUCT_KINDS_STORE_LIST.Count; ++i) {
					var eProductKinds = KDefine.G_PRODUCT_KINDS_STORE_LIST[i];
					oBuyProductTradeInfoDict.TryAdd(eProductKinds, CProductTradeInfoTable.Inst.GetBuyProductTradeTradeInfo(eProductKinds));
				}

				(a_oSender as CStorePopup).Init(new CStorePopup.STParams() {
					m_oProductTradeInfoList = oBuyProductTradeInfoDict.ExToList(),

#if ADS_MODULE_ENABLE
					m_oAdsCallbackDict = new Dictionary<CStorePopup.ECallback, System.Action<CAdsManager, STAdsRewardInfo, bool>>() {
						[CStorePopup.ECallback.ADS] = (a_oAdsSender, a_stAdsRewardInfo, a_bIsSuccess) => this.UpdateUIsState()
					},
#endif			// #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
					m_oPurchaseCallbackDict01 = new Dictionary<CStorePopup.ECallback, System.Action<CPurchaseManager, string, bool>>() {
						[CStorePopup.ECallback.PURCHASE] = (a_oPurchaseSender, a_oProductID, a_bIsSuccess) => this.UpdateUIsState()
					},

					m_oPurchaseCallbackDict02 = new Dictionary<CStorePopup.ECallback, System.Action<CPurchaseManager, List<Product>, bool>>() {
						[CStorePopup.ECallback.RESTORE] = (a_oRestoreSender, a_oProductList, a_bIsSuccess) => this.UpdateUIsState()
					}
#endif			// #if PURCHASE_MODULE_ENABLE
				});
			});
		}

		/** 상점 버튼을 눌렀을 경우 */
		private void OnTouchStoreBtn() {
			this.ShowStorePopup();
		}
		#endregion			// 함수

		#region 조건부 함수
#if PURCHASE_MODULE_ENABLE
		/** 상품을 결제한다 */
		public void PurchaseProduct(int a_nProductIdx, System.Action<CPurchaseManager, string, bool> a_oCallback) {
			Func.PurchaseProduct(CProductInfoTable.Inst.GetProductInfo(a_nProductIdx).m_oID, a_oCallback);
		}

		/** 상품을 결제한다 */
		public void PurchaseProduct(EProductKinds a_eProductKinds, System.Action<CPurchaseManager, string, bool> a_oCallback) {
			m_oCallbackDict.ExReplaceVal(ECallback.PURCHASE, a_oCallback);
			Func.PurchaseProduct(a_eProductKinds, this.OnPurchaseProduct);
		}

		/** 상품이 결제 되었을 경우 */
		private void OnPurchaseProduct(CPurchaseManager a_oSender, string a_oProductID, bool a_bIsSuccess) {
			// 결제 되었을 경우
			if(a_bIsSuccess) {
				Func.AcquireProduct(a_oProductID);
				m_oStrDict.ExReplaceVal(EKey.PURCHASE_PRODUCT_ID, a_oProductID);

#if FIREBASE_MODULE_ENABLE
				this.ExLateCallFunc((a_oCallFuncSender) => Func.SaveUserInfo(this.OnSaveUserInfo));
#else
				Func.OnPurchaseProduct(a_oSender, a_oProductID, a_bIsSuccess, null);
#endif			// #if FIREBASE_MODULE_ENABLE
			} else {
				Func.OnPurchaseProduct(a_oSender, a_oProductID, a_bIsSuccess, null);
			}

			this.UpdateUIsState();
			m_oCallbackDict.GetValueOrDefault(ECallback.PURCHASE)?.Invoke(a_oSender, a_oProductID, a_bIsSuccess);
		}

#if FIREBASE_MODULE_ENABLE
		/** 유저 정보를 저장했을 경우 */
		private void OnSaveUserInfo(CFirebaseManager a_oSender, bool a_bIsSuccess) {
			Func.OnPurchaseProduct(CPurchaseManager.Inst, m_oStrDict.GetValueOrDefault(EKey.PURCHASE_PRODUCT_ID, string.Empty), true, null);
		}
#endif			// #if FIREBASE_MODULE_ENABLE
#endif			// #if PURCHASE_MODULE_ENABLE
		#endregion			// 조건부 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
