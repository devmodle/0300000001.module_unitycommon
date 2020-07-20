using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 토스트 팝업 관리자
public class CToastPopupManager : CSingleton<CToastPopupManager> {
	#region 변수
	private bool m_bIsShowToastPopup = false;
	#endregion			// 변수

	#region 컴포넌트
	private Queue<CToastPopup> m_oToastPopupList = new Queue<CToastPopup>();
	#endregion			// 컴포넌트

	#region 프로퍼티
	public System.Func<string, float, CToastPopup> ToastPopupCreator { get; set; } = null;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		this.ToastPopupCreator = this.CreateToastPopup;
	}

	//! 상태를 갱신한다
	public virtual void Update() {
		if(!m_bIsShowToastPopup && m_oToastPopupList.Count >= 1) {
			m_bIsShowToastPopup = true;

			var oToastPopup = m_oToastPopupList.Dequeue();
			oToastPopup.gameObject.SetActive(true);

			oToastPopup.ShowPopup(null, (a_oSender) => {
				m_bIsShowToastPopup = false;
			});
		}
	}

	//! 토스트 팝업을 출력한다
	public void ShowToastPopup(string a_oMsg, float a_fDuration = KUDefine.DEF_DURATION_TOAST_POPUP) {
		CBAccess.Assert(this.ToastPopupCreator != null);

		var oToastPopup = this.ToastPopupCreator(a_oMsg, a_fDuration);
		oToastPopup.gameObject.SetActive(false);

		m_oToastPopupList.Enqueue(oToastPopup);
	}

	//! 토스트 팝업을 생성한다
	private CToastPopup CreateToastPopup(string a_oMsg, float a_fDuration) {
		return CToastPopup.CreateToastPopup<CToastPopup>(KUDefine.OBJ_NAME_TOAST_P_TOAST_POPUP,
			CResManager.Instance.GetPrefab(KUDefine.OBJ_PATH_TOAST_POPUP), CSceneManager.ScreenTopmostUIRoot, a_oMsg, a_fDuration);
	}
	#endregion			// 함수
}
