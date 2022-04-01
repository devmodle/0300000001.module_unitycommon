using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if SCRIPT_TEMPLATE_ONLY
#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif			// #if PURCHASE_MODULE_ENABLE

#if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
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
		private Dictionary<EKey, string> m_oStrDict = new Dictionary<EKey, string>() {
			[EKey.PURCHASE_PRODUCT_ID] = string.Empty
		};

		/** =====> UI <===== */
		private Dictionary<EKey, TMP_Text> m_oTextDict = new Dictionary<EKey, TMP_Text>() {
			[EKey.NUM_COINS_TEXT] = null
		};

		private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>() {
			[EKey.STORE_BTN] = null
		};

#if PURCHASE_MODULE_ENABLE
		private Dictionary<ECallback, System.Action<CPurchaseManager, string, bool>> m_oCallbackDict = new Dictionary<ECallback, System.Action<CPurchaseManager, string, bool>>();
#endif			// #if PURCHASE_MODULE_ENABLE
		#endregion			// 변수

		#region 프로퍼티
		public override STSortingOrderInfo UIsCanvasSortingOrderInfo => KDefine.G_SORTING_OI_OVERLAY_SCENE_UIS_CANVAS;
		#endregion			// 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();
			
			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				this.SetupAwake();
			}
		}

		/** 초기화 */
		public override void Start() {
			base.Start();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				this.SetupStart();
				this.UpdateUIsState();
			}
		}

		/** 상점 팝업을 출력한다 */
		public void ShowStorePopup() {
#if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
			Func.ShowStorePopup(CSceneManager.ActiveScenePopupUIs, (a_oSender) => {
				var oSaleProductInfoList = new List<STSaleProductInfo>();

				for(int i = 0; i < KDefine.G_SALE_PRODUCT_KINDS_PRODUCT_LIST.Count; ++i) {
					var eSaleProductKinds = KDefine.G_SALE_PRODUCT_KINDS_PRODUCT_LIST[i];
					oSaleProductInfoList.Add(CSaleProductInfoTable.Inst.GetSaleProductInfo(eSaleProductKinds));
				}

				(a_oSender as CStorePopup).Init(new CStorePopup.STParams() {
					m_oSaleProductInfoList = oSaleProductInfoList,

#if ADS_MODULE_ENABLE
					m_oAdsCallbackDict = new Dictionary<CStorePopup.ECallback, System.Action<CAdsManager, STAdsRewardInfo, bool>>() {
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
#endif			// #if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
		}

		/** 씬을 설정한다 */
		private void SetupAwake() {
			// 텍스트를 설정한다
			m_oTextDict[EKey.NUM_COINS_TEXT] = this.UIsBase.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_NUM_COINS_TEXT);

			// 버튼을 설정한다
			m_oBtnDict[EKey.STORE_BTN] = this.UIsBase.ExFindComponent<Button>(KCDefine.U_OBJ_N_STORE_BTN);
			m_oBtnDict[EKey.STORE_BTN]?.onClick.AddListener(this.OnTouchStoreBtn);

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
			var oSubTitleSceneManager = CSceneManager.GetSceneManager<TitleScene.CSubTitleSceneManager>(KCDefine.B_SCENE_N_TITLE);
			oSubTitleSceneManager?.gameObject.ExSendMsg(KCDefine.U_FUNC_N_UPDATE_UIS_STATE, null, false);

			var oSubMainSceneManager = CSceneManager.GetSceneManager<MainScene.CSubMainSceneManager>(KCDefine.B_SCENE_N_MAIN);
			oSubMainSceneManager?.gameObject.ExSendMsg(KCDefine.U_FUNC_N_UPDATE_UIS_STATE, null, false);

			var oSubGameSceneManager = CSceneManager.GetSceneManager<GameScene.CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME);
			oSubGameSceneManager?.gameObject.ExSendMsg(KCDefine.U_FUNC_N_UPDATE_UIS_STATE, null, false);

			// 텍스트를 갱신한다
			m_oTextDict[EKey.NUM_COINS_TEXT]?.ExSetText($"{CUserInfoStorage.Inst.UserInfo.NumCoins}", EFontSet._1, false);

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

#if PURCHASE_MODULE_ENABLE
		/** 상품을 결제한다 */
		public void PurchaseProduct(int a_nID, System.Action<CPurchaseManager, string, bool> a_oCallback) {
			Func.PurchaseProduct(CProductInfoTable.Inst.GetProductInfo(a_nID).m_oID, a_oCallback);
		}

		/** 상품을 결제한다 */
		public void PurchaseProduct(ESaleProductKinds a_eSaleProductKinds, System.Action<CPurchaseManager, string, bool> a_oCallback) {
			m_oCallbackDict.ExReplaceVal(ECallback.PURCHASE, a_oCallback);
			Func.PurchaseProduct(a_eSaleProductKinds, this.OnPurchaseProduct);
		}

		/** 상품이 결제 되었을 경우 */
		private void OnPurchaseProduct(CPurchaseManager a_oSender, string a_oProductID, bool a_bIsSuccess) {
			// 결제 되었을 경우
			if(a_bIsSuccess) {
				Func.AcquireProduct(a_oProductID);
				m_oStrDict[EKey.PURCHASE_PRODUCT_ID] = a_oProductID;

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
			Func.OnPurchaseProduct(CPurchaseManager.Inst, m_oStrDict[EKey.PURCHASE_PRODUCT_ID], true, null);
		}
#endif			// #if FIREBASE_MODULE_ENABLE
#endif			// #if PURCHASE_MODULE_ENABLE
		#endregion			// 조건부 함수
	}
}
#endif			// #if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
