using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 보상 획득 팝업
public class CRewardAcquirePopup : CSubPopup {
	//! 매개 변수
	public struct STParams {
		public ERewardQuality m_eQuality;
		public ERewardAcquirePopupType m_ePopupType;
		
		public List<STItemInfo> m_oItemInfoList;
	}

	#region 변수
	private STParams m_stParams;
	#endregion			// 변수

	#region UI 변수
	private Button m_oAcquireBtn = null;
	#endregion			// UI 변수

	#region 객체
	[SerializeField] private List<GameObject> m_oItemUIsList = new List<GameObject>();
	#endregion			// 객체

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 버튼을 설정한다
		m_oAcquireBtn = m_oContents.ExFindComponent<Button>(KDefine.G_OBJ_N_REWARD_AP_ACQUIRE_BTN);
		m_oAcquireBtn.onClick.AddListener(this.OnTouchAcquireBtn);
	}
	
	//! 초기화
	public virtual void Init(STParams a_stParams) {
		base.Init();
		m_stParams = a_stParams;

		this.UpdateUIsState();
	}

	//! UI 상태를 갱신한다
	private void UpdateUIsState() {
		// 보상 아이템 UI 상태를 갱신한다
		for(int i = 0; i < m_oItemUIsList.Count; ++i) {
			var oItemUIs = m_oItemUIsList[i];
			oItemUIs.SetActive(i < m_stParams.m_oItemInfoList.Count);
			
			// 보상 정보가 존재 할 경우
			if(i < m_stParams.m_oItemInfoList.Count) {
				this.UpdateItemUIsState(oItemUIs, m_stParams.m_oItemInfoList[i]);
			}
		}
	}

	//! 보상 아이템 UI 상태를 갱신한다
	private void UpdateItemUIsState(GameObject a_oItemUIs, STItemInfo a_stItemInfo) {
		// Do Nothing
	}

	//! 획득 버튼을 눌렀을 경우
	private void OnTouchAcquireBtn() {
		for(int i = 0; i < m_stParams.m_oItemInfoList.Count; ++i) {
			Func.AcquireItem(m_stParams.m_oItemInfoList[i]);
		}
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
