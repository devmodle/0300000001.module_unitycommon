using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif			// #if PURCHASE_MODULE_ENABLE

//! 전역 함수
public static partial class Func {
	#region 클래스 함수
	//! 아이템을 획득한다
	public static void AcquireItem(STItemInfo a_stItemInfo, int a_nExtraNumItems = KCDefine.B_VAL_0_INT) {
		switch(a_stItemInfo.m_eItemKinds) {
			case EItemKinds.GOODS_COINS: {
				CUserInfoStorage.Inst.AddNumCoins(a_stItemInfo.m_nNumItems + a_nExtraNumItems);
			} break;
			case EItemKinds.NON_CONSUMABLE_REMOVE_ADS: {
				CCommonUserInfoStorage.Inst.UserInfo.IsRemoveAds = true;

#if ADS_MODULE_ENABLE
				CAdsManager.Inst.CloseBannerAds(CPluginInfoTable.Inst.DefAdsType, true);

				CAdsManager.Inst.IsEnableBannerAds = false;
				CAdsManager.Inst.IsEnableFullscreenAds = false;
#endif			// #if ADS_MODULE_ENABLE
			} break;
			default: {
				CUserInfoStorage.Inst.AddNumItems(a_stItemInfo.m_eItemKinds, a_stItemInfo.m_nNumItems + a_nExtraNumItems);
			} break;
		}

		CUserInfoStorage.Inst.SaveUserInfo();
		CCommonUserInfoStorage.Inst.SaveUserInfo();
	}

	//! 아이템을 구입한다
	public static void BuyItem(STSaleItemInfo a_stSaleItemInfo, List<int> a_oExtraNumItemsList = null, int a_nExtraPrice = KCDefine.B_VAL_0_INT, bool a_bIsIgnoreAcquire = false) {
		// 아이템 획득이 가능 할 경우
		if(!a_bIsIgnoreAcquire) {
			for(int i = 0; i < a_stSaleItemInfo.m_oItemInfoList.Count; ++i) {
				Func.AcquireItem(a_stSaleItemInfo.m_oItemInfoList[i], a_oExtraNumItemsList.ExIsValid() ? a_oExtraNumItemsList.ExGetVal(i, KCDefine.B_VAL_0_INT) : KCDefine.B_VAL_0_INT);
			}
		}

		// 코인 비용이 존재 할 경우
		if(a_stSaleItemInfo.m_ePriceKinds == EPriceKinds.GOODS_COINS && a_stSaleItemInfo.IntPrice + a_nExtraPrice > KCDefine.B_VAL_0_INT) {
			CUserInfoStorage.Inst.AddNumCoins(-(a_stSaleItemInfo.IntPrice + a_nExtraPrice));
		}

		CUserInfoStorage.Inst.SaveUserInfo();
	}

	//! 추적 설명 팝업을 출력한다
	public static void ShowTrackingDescPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CTrackingDescPopup>(KCDefine.LSS_OBJ_N_TRACKING_DESC_POPUP, KCDefine.LSS_OBJ_P_TRACKING_DESC_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	//! 상점 팝업을 출력한다
	public static void ShowStorePopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CStorePopup>(KDefine.G_OBJ_N_STORE_POPUP, KCDefine.U_OBJ_P_G_STORE_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	//! 설정 팝업을 출력한다
	public static void ShowSettingsPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CSettingsPopup>(KDefine.G_OBJ_N_SETTINGS_POPUP, KCDefine.U_OBJ_P_G_SETTINGS_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	//! 동기화 팝업을 출력한다
	public static void ShowSyncPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CSyncPopup>(KDefine.G_OBJ_N_SYNC_POPUP, KCDefine.U_OBJ_P_G_SYNC_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	//! 일일 미션 팝업을 출력한다
	public static void ShowDailyMissionPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CDailyMissionPopup>(KDefine.G_OBJ_N_DAILY_MISSION_POPUP, KCDefine.U_OBJ_P_G_DAILY_MISSION_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	//! 무료 보상 팝업을 출력한다
	public static void ShowFreeRewardPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CFreeRewardPopup>(KDefine.G_OBJ_N_FREE_REWARD_POPUP, KCDefine.U_OBJ_P_G_FREE_REWARD_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	//! 일일 보상 팝업을 출력한다
	public static void ShowDailyRewardPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CDailyRewardPopup>(KDefine.G_OBJ_N_DAILY_REWARD_POPUP, KCDefine.U_OBJ_P_G_DAILY_REWARD_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	//! 판매 코인 팝업을 출력한다
	public static void ShowSaleCoinsPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CSaleCoinsPopup>(KDefine.G_OBJ_N_SALE_COINS_POPUP, KCDefine.U_OBJ_P_G_CHANGES_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	//! 보상 획득 팝업을 출력한다
	public static void ShowRewardAcquirePopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CRewardAcquirePopup>(KDefine.G_OBJ_N_REWARD_ACQUIRE_POPUP, KCDefine.U_OBJ_P_G_REWARD_ACQUIRE_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	//! 판매 코인 획득 팝업을 출력한다
	public static void ShowSaleCoinsAcquirePopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CSaleCoinsAcquirePopup>(KDefine.G_OBJ_N_SALE_COINS_ACQUIRE_POPUP, KCDefine.U_OBJ_P_G_CHANGES_ACQUIRE_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	//! 이어하기 팝업을 출력한다
	public static void ShowContinuePopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CContinuePopup>(KDefine.G_OBJ_N_CONTINUE_POPUP, KCDefine.U_OBJ_P_G_CONTINUE_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	//! 결과 팝업을 출력한다
	public static void ShowResultPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CResultPopup>(KDefine.G_OBJ_N_RESULT_POPUP, KCDefine.U_OBJ_P_G_RESULT_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	//! 포커스 팝업을 출력한다
	public static void ShowFocusPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CFocusPopup>(KDefine.G_OBJ_N_FOCUS_POPUP, KCDefine.U_OBJ_P_G_FOCUS_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	//! 튜토리얼 팝업을 출력한다
	public static void ShowTutorialPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CTutorialPopup>(KDefine.G_OBJ_N_TUTORIAL_POPUP, KCDefine.U_OBJ_P_G_TUTORIAL_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}
	#endregion			// 클래스 함수

	#region 조건부 클래스 함수
#if FIREBASE_MODULE_ENABLE
	//! 로그인 되었을 경우
	public static void OnLogin(CFirebaseManager a_oSender, bool a_bIsSuccess, System.Action<CAlertPopup, bool> a_oCallback) {
		// 로그아웃 되었을 경우
		if(a_bIsSuccess) {
			Func.ShowLoginSuccessPopup(a_oCallback);
		} else {
			Func.ShowLoginFailPopup(a_oCallback);
		}
	}

	//! 로그아웃 되었을 경우
	public static void OnLogout(CFirebaseManager a_oSender, bool a_bIsSuccess, System.Action<CAlertPopup, bool> a_oCallback) {
		// 로그아웃 되었을 경우
		if(a_bIsSuccess) {
			Func.ShowLogoutSuccessPopup(a_oCallback);
		} else {
			Func.ShowLogoutFailPopup(a_oCallback);
		}
	}

	//! 유저 정보가 로드 되었을 경우
	public static void OnLoadUserInfo(CFirebaseManager a_oSender, string a_oJSONStr, bool a_bIsSuccess, System.Action<CAlertPopup, bool> a_oCallback) {
		// 로드 되었을 경우
		if(a_bIsSuccess) {
			Func.ShowLoadSuccessPopup(a_oCallback);
		} else {
			Func.ShowLoadFailPopup(a_oCallback);
		}
	}

	//! 유저 정보가 저장 되었을 경우
	public static void OnSaveUserInfo(CFirebaseManager a_oSender, bool a_bIsSuccess, System.Action<CAlertPopup, bool> a_oCallback) {
		// 저장 되었을 경우
		if(a_bIsSuccess) {
			Func.ShowSaveSuccessPopup(a_oCallback);
		} else {
			Func.ShowSaveFailPopup(a_oCallback);
		}
	}
#endif			// #if FIREBASE_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	//! 상품이 결제 되었을 경우
	public static void OnPurchaseProduct(CPurchaseManager a_oSender, string a_oProductID, bool a_bIsSuccess, System.Action<CAlertPopup, bool> a_oCallback) {
		// 결제 되었을 경우
		if(a_bIsSuccess) {
			Func.ShowPurchaseSuccessPopup(a_oCallback);
		} else {
			Func.ShowPurchaseFailPopup(a_oCallback);
		}
	}

	//! 상품이 복원 되었을 경우
	public static void OnRestoreProducts(CPurchaseManager a_oSender, List<Product> a_oProductList, bool a_bIsSuccess, System.Action<CAlertPopup, bool> a_oCallback) {
		// 복원 되었을 경우
		if(a_bIsSuccess) {
			Func.ShowRestoreSuccessPopup(a_oCallback);
		} else {
			Func.ShowRestoreFailPopup(a_oCallback);
		}
	}

	//! 상품을 획득한다
	public static void AcquireProduct(string a_oProductID, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oProductID.ExIsValid());

		// 상품이 존재 할 경우
		if(a_oProductID.ExIsValid()) {
			int nIdx = CProductInfoTable.Inst.GetProductInfoIdx(a_oProductID);

			var oProduct = CPurchaseManager.Inst.GetProduct(a_oProductID);
			var eSaleProductKinds = KDefine.G_KINDS_SALE_PIT_SALE_PRODUCTS[nIdx];
			var stSaleProductInfo = CSaleProductInfoTable.Inst.GetSaleProductInfo(eSaleProductKinds);

			for(int i = 0; i < stSaleProductInfo.m_oItemInfoList.Count; ++i) {
				Func.AcquireItem(stSaleProductInfo.m_oItemInfoList[i]);
			}

			// 비소모 상품 일 경우
			if(oProduct != null && oProduct.definition.type == ProductType.NonConsumable && !CCommonUserInfoStorage.Inst.IsRestoreProduct(a_oProductID)) {
				CCommonUserInfoStorage.Inst.AddRestoreProductID(a_oProductID);
				CCommonUserInfoStorage.Inst.SaveUserInfo();
			}
		}
	}

	//! 복원 상품을 획득한다
	public static void AcquireRestoreProducts(List<Product> a_oProductList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oProductList != null);

		// 상품이 존재 할 경우
		if(a_oProductList != null) {
			for(int i = 0; i < a_oProductList.Count; ++i) {
				// 상품 복원이 가능 할 경우
				if(!CCommonUserInfoStorage.Inst.IsRestoreProduct(a_oProductList[i].definition.id)) {
					int nIdx = CProductInfoTable.Inst.GetProductInfoIdx(a_oProductList[i].definition.id);
					var eSaleProductKinds = KDefine.G_KINDS_SALE_PIT_SALE_PRODUCTS[nIdx];
					var stSaleProductInfo = CSaleProductInfoTable.Inst.GetSaleProductInfo(eSaleProductKinds);

					for(int j = 0; j < stSaleProductInfo.m_oItemInfoList.Count; ++j) {
						Func.AcquireItem(stSaleProductInfo.m_oItemInfoList[j]);
					}

					CCommonUserInfoStorage.Inst.AddRestoreProductID(a_oProductList[i].definition.id);
				}				
			}

			CCommonUserInfoStorage.Inst.SaveUserInfo();
		}
	}
#endif			// #if PURCHASE_MODULE_ENABLE
	#endregion			// 조건부 클래스 함수

	#region 추가 클래스 함수

	#endregion			// 추가 클래스 함수
}
#endif			// #if NEVER_USE_THIS
