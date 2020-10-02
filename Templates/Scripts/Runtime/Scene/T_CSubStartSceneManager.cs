using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 서브 시작 씬 관리자
public class CSubStartSceneManager : CStartSceneManager {
	#region 변수
	private int m_nNumDots = 0;
	private float m_fSkipTime = 0.0f;

	private Text m_oStateText = null;
	private System.Text.StringBuilder m_oStringBuilder = new System.Text.StringBuilder();
	#endregion			// 변수
	
	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			m_oStateText = CFactory.CreateCloneObj<Text>(KDefine.SS_OBJ_NAME_STATE_TEXT,
				CResManager.Instance.GetPrefab(KDefine.SS_OBJ_PATH_STATE_TEXT), 
				this.SubUIRoot);

			m_oStateText.text = string.Empty;
			this.UpdateUIState();
		}
	}

	//! 상태를 갱신한다
	public override void OnUpdate(float a_fDeltaTime) {
		base.OnUpdate(a_fDeltaTime);
		m_fSkipTime += Time.deltaTime;

		// 상태 텍스트 갱신 주기가 지났을 경우
		if(m_fSkipTime.ExIsGreateEquals(KDefine.SS_DELTA_TIME_UPDATE_STATE)) {
			m_nNumDots = (m_nNumDots + 1) % KDefine.SS_MAX_NUM_DOTS;
			m_fSkipTime = 0.0f;

			this.UpdateUIState();
		}
	}

	//! 약관 동의 씬 관리자 이벤트를 수신했을 경우
	protected override void OnReceiveAgreeSceneManagerEvent(EAgreeSceneManagerEventType a_eEventType) {
		// Do Nothing
	}

	//! UI 상태를 갱신한다
	private void UpdateUIState() {
#if MSG_PACK_ENABLE
		// 국가 코드가 유효 할 경우
		if(CCommonAppInfoStorage.Instance.CountryCode.ExIsValid()) {
			string oDot = CStringTable.Instance.GetString(KDefine.ST_KEY_START_SM_DOT_TEXT);
			string oLoading = CStringTable.Instance.GetString(KDefine.ST_KEY_START_SM_LOADING_TEXT);

			m_oStringBuilder.Clear();
			m_oStringBuilder.Append(oLoading);
			
			for(int i = 0; i < m_nNumDots + 1; ++i) {
				m_oStringBuilder.Append(oDot);
			}

			m_oStateText.text = m_oStringBuilder.ToString();
		}
#endif			// #if MSG_PACK_ENABLE
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
