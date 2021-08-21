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
	private float m_fMaxPercent = 0.0f;

	private System.Text.StringBuilder m_oStrBuilder = new System.Text.StringBuilder();

	// UI
	private Text m_oLoadingText = null;
	private CGaugeHandler m_oGaugeHandler = null;
	#endregion			// 변수

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			this.SetupAwake();
		}
	}

	//! 씬을 설정한다
	protected override void Setup() {
		base.Setup();
		this.UpdateUIsState();
	}

	//! 상태를 갱신한다
	public override void OnUpdate(float a_fDeltaTime) {
		base.OnUpdate(a_fDeltaTime);

		// 앱이 실행 중 일 경우
		if(CSceneManager.IsAppRunning) {
			m_fSkipTime += Time.deltaTime;

			float fPercent = (KCDefine.B_VAL_1_FLT * a_fDeltaTime) * KCDefine.SS_SCALE_LOADING;
			m_oGaugeHandler.Percent = Mathf.Clamp(m_oGaugeHandler.Percent + fPercent, KCDefine.B_VAL_0_FLT, m_fMaxPercent);
			
			// 텍스트 상태 갱신 주기가 지났을 경우
			if(m_fSkipTime.ExIsGreateEquals(KCDefine.SS_DELTA_T_UPDATE_STATE)) {
				m_nNumDots = (m_nNumDots + KCDefine.B_VAL_1_INT) % KCDefine.SS_MAX_NUM_DOTS;
				m_fSkipTime = KCDefine.B_VAL_0_FLT;

				this.UpdateUIsState();
			}
		}
	}

	//! 시작 씬 이벤트를 수신했을 경우
	protected override void OnReceiveStartSceneEvent(EStartSceneEvent a_eEvent) {
		int nEvent = (int)a_eEvent + KCDefine.B_VAL_1_INT;
		float fPercent = nEvent / (float)((int)EStartSceneEvent.MAX_VAL - KCDefine.B_VAL_1_INT);

		m_fMaxPercent = Mathf.Clamp(fPercent, KCDefine.B_VAL_0_FLT, KCDefine.B_VAL_1_FLT);
	}

	//! 씬을 설정한다
	private void SetupAwake() {
		m_fSkipTime = KCDefine.SS_DELTA_T_UPDATE_STATE;
			
		// 텍스트를 설정한다 {
		var oLoadingText = this.SubUIs.ExFindComponent<Text>(KCDefine.SS_OBJ_N_LOADING_TEXT);

		m_oLoadingText = oLoadingText ?? CFactory.CreateCloneObj<Text>(KCDefine.SS_OBJ_N_LOADING_TEXT, KCDefine.SS_OBJ_P_LOADING_TEXT, this.SubUIs, KDefine.SS_POS_LOADING_TEXT);
		m_oLoadingText.text = KCDefine.SS_TEXT_LOADING;
		// 텍스트를 설정한다 }

		// 게이지 처리자를 설정한다 {
		var oLoadingGauge = this.SubUIs.ExFindChild(KCDefine.SS_OBJ_N_LOADING_GAUGE);
		oLoadingGauge = oLoadingGauge ?? CFactory.CreateCloneObj(KCDefine.SS_OBJ_N_LOADING_GAUGE, KCDefine.SS_OBJ_P_LOADING_GAUGE, this.SubUIs, KDefine.SS_POS_LOADING_IMG_GAUGE);

		m_oGaugeHandler = oLoadingGauge.GetComponentInChildren<CGaugeHandler>();
		m_oGaugeHandler.Percent = KCDefine.B_VAL_0_FLT;
		// 게이지 처리자를 설정한다 }
	}

	//! 텍스트 상태를 갱신한다
	private void UpdateUIsState() {
		string oDotStr = CStrTable.Inst.GetStr(KCDefine.ST_KEY_START_SM_DOT_TEXT);
		string oLoadingStr = CCommonAppInfoStorage.Inst.CountryCode.ExIsValid() ? CStrTable.Inst.GetStr(KCDefine.ST_KEY_START_SM_LOADING_TEXT) : KCDefine.SS_TEXT_LOADING;

		m_oStrBuilder.Clear();
		m_oStrBuilder.Append(oLoadingStr);

		for(int i = 0; i < m_nNumDots + KCDefine.B_VAL_1_INT; ++i) {
			m_oStrBuilder.Append(oDotStr);
		}

		m_oLoadingText.text = m_oStrBuilder.ToString();
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
