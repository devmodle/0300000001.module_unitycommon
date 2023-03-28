#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 난이도 UI 처리자 */
public partial class CDifficultyUIsHandler : CComponent {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	[SerializeField] private string m_oBasePath = string.Empty;
	[SerializeField] private EDifficulty m_eDifficulty = EDifficulty.NONE;
	#endregion // 변수

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
	}

	/** 초기화 */
	public override void Start() {
		base.Start();
		this.SetupDifficulty();
	}

	/** 난이도를 리셋한다 */
	public virtual void ResetDifficulty() {
		this.SetupDifficulty();
	}

	/** 이미지를 변경한다 */
	public void SetImg(string a_oBasePath) {
		m_oBasePath = a_oBasePath;
		this.SetupDifficulty();
	}

	/** 난이도를 변경한다 */
	public void SetDifficulty(EDifficulty a_eMode) {
		m_eDifficulty = a_eMode;
		this.SetupDifficulty();
	}

	/** 난이도를 설정한다 */
	private void SetupDifficulty() {
		// 이미지가 존재 할 경우
		if(m_eDifficulty != EDifficulty.NONE && this.TryGetComponent(out Image oImg)) {
			oImg.sprite = CResManager.Inst.GetRes<Sprite>(string.Format(KCDefine.B_TEXT_FMT_2_UNDER_SCORE_COMBINE, m_oBasePath, $"{m_eDifficulty}"));
			oImg.gameObject.SetActive(oImg.sprite != null);
		}
	}
	#endregion // 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
