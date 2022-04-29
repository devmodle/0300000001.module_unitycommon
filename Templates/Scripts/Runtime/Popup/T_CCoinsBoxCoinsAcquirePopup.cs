using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 코인 상자 코인 획득 팝업 */
public partial class CCoinsBoxCoinsAcquirePopup : CSubPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		NUM_COINS_TEXT,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public long m_nNumCoinsBoxCoins;
	}

	#region 변수
	private STParams m_stParams;
	private long m_nPrevNumCoinsBoxCoins = 0;

	/** =====> UI <===== */
	private Dictionary<EKey, TMP_Text> m_oTextDict = new Dictionary<EKey, TMP_Text>() {
		[EKey.NUM_COINS_TEXT] = null
	};

	/** =====> 객체 <===== */
	[SerializeField] private GameObject m_oSaveUIs = null;
	[SerializeField] private GameObject m_oFullUIs = null;
	#endregion			// 변수
	
	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.IsIgnoreNavStackEvent = false;

		// 텍스트를 설정한다
		m_oTextDict[EKey.NUM_COINS_TEXT] = this.Contents.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_NUM_COINS_TEXT);
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();

		m_stParams = a_stParams;
		m_nPrevNumCoinsBoxCoins = CUserInfoStorage.Inst.UserInfo.NumCoinsBoxCoins;

		CUserInfoStorage.Inst.AddNumCoinsBoxCoins(a_stParams.m_nNumCoinsBoxCoins);
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

		m_oSaveUIs?.SetActive(m_nPrevNumCoinsBoxCoins < KDefine.G_MAX_NUM_COINS_BOX_COINS);
		m_oFullUIs?.SetActive(m_nPrevNumCoinsBoxCoins >= KDefine.G_MAX_NUM_COINS_BOX_COINS);
		
		// 텍스트를 갱신한다
		m_oTextDict[EKey.NUM_COINS_TEXT]?.ExSetText($"{m_nPrevNumCoinsBoxCoins}", EFontSet._1, false);
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
