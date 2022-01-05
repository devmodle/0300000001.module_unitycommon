using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if NEVER_USE_THIS
#if RUNTIME_TEMPLATES_MODULE_ENABLE
/** 판매 코인 획득 팝업 */
public class CSaleCoinsAcquirePopup : CSubPopup {
	/** 매개 변수 */
	public struct STParams {
		public long m_nNumSaleCoins;
	}

	#region 변수
	private STParams m_stParams;
	private long m_nPrevNumSaleCoins = 0;

	/** =====> UI <===== */
	private TMP_Text m_oNumSaleCoinsText = null;

	/** =====> 객체 <===== */
	[SerializeField] private GameObject m_oSaveUIs = null;
	[SerializeField] private GameObject m_oFullUIs = null;
	#endregion			// 변수

	#region 추가 변수

	#endregion			// 추가 변수

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티
	
	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.IsIgnoreNavStackEvent = false;

		// 텍스트를 설정한다
		m_oNumSaleCoinsText = m_oContents.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_NUM_SALE_COINS_TEXT);
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();

		m_stParams = a_stParams;
		m_nPrevNumSaleCoins = CUserInfoStorage.Inst.UserInfo.NumSaleCoins;

		CUserInfoStorage.Inst.AddNumSaleCoins(a_stParams.m_nNumSaleCoins);
		CUserInfoStorage.Inst.SaveUserInfo();
	}

	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}
	
	/** UI 상태를 변경한다 */
	private new void UpdateUIsState() {
		base.UpdateUIsState();

		m_oSaveUIs?.SetActive(m_nPrevNumSaleCoins < KDefine.G_MAX_NUM_SALE_COINS);
		m_oFullUIs?.SetActive(m_nPrevNumSaleCoins >= KDefine.G_MAX_NUM_SALE_COINS);

		// 텍스트를 갱신한다
		m_oNumSaleCoinsText?.ExSetText($"{m_nPrevNumSaleCoins}", EFontSet.A, false);
	}
	#endregion			// 함수

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if NEVER_USE_THIS
