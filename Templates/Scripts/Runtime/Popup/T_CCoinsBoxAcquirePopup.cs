using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 코인 상자 획득 팝업 */
public partial class CCoinsBoxAcquirePopup : CSubPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		PREV_NUM_COINS_BOX_COINS,
		NUM_COINS_TEXT,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public partial struct STParams {
		public long m_nNumCoinsBoxCoins;
	}

	#region 변수
	private STParams m_stParams;

	private Dictionary<EKey, long> m_oIntDict = new Dictionary<EKey, long>() {
		[EKey.PREV_NUM_COINS_BOX_COINS] = 0
	};

	/** =====> UI <===== */
	private Dictionary<EKey, TMP_Text> m_oTextDict = new Dictionary<EKey, TMP_Text>();

	/** =====> 객체 <===== */
	[SerializeField] private GameObject m_oSaveUIs = null;
	[SerializeField] private GameObject m_oFullUIs = null;
	#endregion			// 변수
	
	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 텍스트를 설정한다
		CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
			(EKey.NUM_COINS_TEXT, $"{EKey.NUM_COINS_TEXT}", this.Contents)
		}, m_oTextDict, false);
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();

		m_stParams = a_stParams;
		m_oIntDict[EKey.PREV_NUM_COINS_BOX_COINS] = CUserInfoStorage.Inst.NumCoinsBoxCoins;

		Func.Acquire(Factory.MakeTargetInfo(ETargetKinds.ITEM_NUMS, (int)EItemKinds.GOODS_COINS_BOX_COINS, new STValInfo() {
			m_nVal = a_stParams.m_nNumCoinsBoxCoins, m_eValType = EValType.INT
		}), true);
	}

	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}
	
	/** UI 상태를 변경한다 */
	private new void UpdateUIsState() {
		base.UpdateUIsState();

		m_oSaveUIs?.SetActive(m_oIntDict[EKey.PREV_NUM_COINS_BOX_COINS] < KDefine.G_MAX_NUM_COINS_BOX_COINS);
		m_oFullUIs?.SetActive(m_oIntDict[EKey.PREV_NUM_COINS_BOX_COINS] >= KDefine.G_MAX_NUM_COINS_BOX_COINS);
		
		// 텍스트를 갱신한다
		m_oTextDict[EKey.NUM_COINS_TEXT]?.ExSetText($"{m_oIntDict[EKey.PREV_NUM_COINS_BOX_COINS]}", EFontSet._1, false);
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
