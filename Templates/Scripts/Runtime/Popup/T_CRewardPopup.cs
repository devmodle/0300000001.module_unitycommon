using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 보상 팝업
public class CRewardPopup : CSubPopup {
	//! 매개 변수
	public struct STParams {
		public ERewardPopupType m_ePopupType;
		public List<STRewardItemInfo> m_oRewardItemInfoList;
	}

	#region 변수
	private STParams m_stParams;
	#endregion			// 변수

	#region 객체
	[SerializeField] private GameObject m_oRewardItemUIs = null;
	private List<GameObject> m_oRewardItemUIList = new List<GameObject>();
	#endregion			// 객체

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
	}
	
	//! 초기화
	public virtual void Init(STParams a_stParams) {
		base.Init();
		m_stParams = a_stParams;
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
