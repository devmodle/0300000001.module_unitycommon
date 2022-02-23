using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if RUNTIME_TEMPLATES_MODULE_ENABLE
/** 미션 팝업 */
public abstract class CMissionPopup : CSubPopup {
	/** 매개 변수 */
	public struct STParams {
		public List<STMissionInfo> m_oMissionInfoList;
	}

	#region 변수
	private STParams m_stParams;

	/** =====> 객체 <===== */
	[SerializeField] private List<GameObject> m_oMissionUIsList = new List<GameObject>();
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
	public virtual void Init(STParams a_stParams) {
		base.Init();
		m_stParams = a_stParams;
	}

	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}

	/** UI 상태를 갱신한다 */
	protected new void UpdateUIsState() {
		base.UpdateUIsState();
		
		for(int i = 0; i < m_oMissionUIsList.Count; ++i) {
			var oMissionUIs = m_oMissionUIsList[i];
			this.UpdateMissionUIsState(oMissionUIs, m_stParams.m_oMissionInfoList[i]);
		}
	}

	/** 미션 UI 상태를 갱신한다 */
	private void UpdateMissionUIsState(GameObject a_oMissionUIs, STMissionInfo a_stMissionInfo) {
		// Do Something
	}
	#endregion			// 함수

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
