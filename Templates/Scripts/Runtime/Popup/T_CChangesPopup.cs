using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 잔돈 팝업
public class CChangesPopup : CSubPopup {
	#region UI 변수
	private Button m_oOKBtn = null;
	private Button m_oPurchaseBtn = null;
	#endregion			// UI 변수

	#region 객체
	[SerializeField] private GameObject m_oSaveUIs = null;
	[SerializeField] private GameObject m_oFullUIs = null;
	#endregion			// 객체

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 버튼을 설정한다 {
		m_oOKBtn = m_oContents.ExFindComponent<Button>(KDefine.G_OBJ_N_CHANGES_P_OK_BTN);
		m_oOKBtn?.onClick.AddListener(this.OnTouchOKBtn);

		m_oPurchaseBtn = m_oContents.ExFindComponent<Button>(KDefine.G_OBJ_N_CHANGES_P_PURCHASE_BTN);
		m_oPurchaseBtn?.onClick.AddListener(this.OnTouchPurchaseBtn);
		// 버튼을 설정한다 }
	}

	//! 초기화
	public override void Init() {
		base.Init();
	}

	//! 팝업 컨텐츠를 설정한다
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}
	
	//! UI 상태를 변경한다
	private new void UpdateUIsState() {
		int nNumChanges = CUserInfoStorage.Inst.UserInfo.NumChanges;

		m_oSaveUIs?.SetActive(nNumChanges < KDefine.G_MAX_NUM_CHANGES);
		m_oFullUIs?.SetActive(nNumChanges >= KDefine.G_MAX_NUM_CHANGES);
	}

	//! 확인 버튼을 눌렀을 경우
	private void OnTouchOKBtn() {
		this.OnTouchCloseBtn();
	}

	//! 결제 버튼을 눌렀을 경우
	private void OnTouchPurchaseBtn() {
#if PURCHASE_MODULE_ENABLE
		Func.PurchaseProduct(KDefine.G_PRODUCT_ID_CHANGES, this.OnPurchaseProduct);
#endif			// #if PURCHASE_MODULE_ENABLE
	}
	#endregion			// 함수

	#region 조건부 함수
#if PURCHASE_MODULE_ENABLE
	//! 상품이 결제 되었을 경우
	private void OnPurchaseProduct(CPurchaseManager a_oSender, string a_oProductID, bool a_bIsSuccess) {
		// 결제 되었을 경우
		if(a_bIsSuccess) {
			Func.AcquireProduct(a_oProductID);
			Func.OnPurchaseProduct(a_oSender, a_oProductID, a_bIsSuccess, null);
		}
	}
#endif			// #if PURCHASE_MODULE_ENABLE
	#endregion			// 조건부 함수
}
#endif			// #if NEVER_USE_THIS
