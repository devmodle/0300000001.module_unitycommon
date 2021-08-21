using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 레벨 모드 보정자
public class CLevelModeCorrector : CComponent {
	#region 변수
	[SerializeField] private string m_oBasePath = string.Empty;
	[SerializeField] private ELevelMode m_eLevelMode = ELevelMode.NONE;

	// UI
	private Image m_oImg = null;
	#endregion			// 변수

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		m_oImg = this.GetComponentInChildren<Image>();
	}

	//! 초기화
	public override void Start() {
		base.Start();
		this.SetImg(m_oBasePath);
	}

	//! 레벨 모드를 리셋한다
	public virtual void ResetLevelMode() {
		this.SetImg(m_oBasePath);
	}

	//! 이미지를 변경한다
	public void SetImg(string a_oBasePath) {
		m_oBasePath = a_oBasePath;

		string oLevelModeStr = CStrTable.Inst.GetEnumStr<ELevelMode>(m_eLevelMode);
		string oImgPath = a_oBasePath.ExGetReplaceFileNamePath(oLevelModeStr);

		m_oImg.sprite = CResManager.Inst.GetRes<Sprite>(oImgPath);
	}

	//! 레벨 모드를 변경한다
	public void SetLevelMode(ELevelMode a_eMode) {
		m_eLevelMode = a_eMode;
		this.ResetLevelMode();
	}
	#endregion			// 함수

	#region 추가 변수

	#endregion			// 추가 변수

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if NEVER_USE_THIS
