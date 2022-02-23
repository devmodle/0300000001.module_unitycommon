using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if RUNTIME_TEMPLATES_MODULE_ENABLE
/** 난이도 보정자 */
public class CDifficultyCorrector : CComponent {
	#region 변수
	[SerializeField] private string m_oBasePath = string.Empty;
	[SerializeField] private EDifficulty m_eDifficulty = EDifficulty.NONE;
	#endregion			// 변수

	#region 추가 변수

	#endregion			// 추가 변수

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

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
		// Do Something
	}
	#endregion			// 함수

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
