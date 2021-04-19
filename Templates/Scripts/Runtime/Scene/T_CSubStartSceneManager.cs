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
	#endregion			// 변수

	#region UI 변수
	protected Text m_oLoadingText = null;
	protected Image m_oGaugeImg = null;
	#endregion			// UI 변수

	#region 객체
	private GameObject m_oLoadingGauge = null;
	#endregion			// 객체

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			m_fSkipTime = KCDefine.SS_DELTA_T_UPDATE_STATE;
			
			// 텍스트를 설정한다
			m_oLoadingText = CFactory.CreateCloneObj<Text>(KCDefine.SS_OBJ_N_LOADING_TEXT, KCDefine.SS_OBJ_P_LOADING_TEXT, this.SubUIs, KDefine.SS_POS_LOADING_TEXT);
			m_oLoadingText.text = KCDefine.SS_TEXT_LOADING;

			// 이미지를 설정한다 {
			m_oLoadingGauge = CFactory.CreateCloneObj(KCDefine.SS_OBJ_N_LOADING_GAUGE, KCDefine.SS_OBJ_P_LOADING_GAUGE, this.SubUIs, KDefine.SS_POS_LOADING_IMG_GAUGE);

			m_oGaugeImg = m_oLoadingGauge.ExFindComponent<Image>(KCDefine.SS_OBJ_N_GAUGE_IMG);
			m_oGaugeImg.fillAmount = KCDefine.B_VAL_0_FLT;
			// 이미지를 설정한다 }
		}
	}

	//! 씬을 설정한다
	protected override void Setup() {
		base.Setup();
		this.UpdateTextsState();
	}

	//! 상태를 갱신한다
	public override void OnUpdate(float a_fDeltaTime) {
		base.OnUpdate(a_fDeltaTime);
		m_fSkipTime += Time.deltaTime;

		float fPercent = (KCDefine.B_VAL_1_FLT * a_fDeltaTime) * KCDefine.SS_SCALE_LOADING;
		m_oGaugeImg.fillAmount = Mathf.Clamp(m_oGaugeImg.fillAmount + fPercent, KCDefine.B_VAL_0_FLT, m_fMaxPercent);

		// 슬라이스 타입 게이지 일 경우
		if(m_oGaugeImg.type == Image.Type.Sliced) {
			var stSize = m_oGaugeImg.rectTransform.sizeDelta;
			m_oGaugeImg.rectTransform.anchoredPosition = new Vector2(stSize.x * m_oGaugeImg.fillAmount, KCDefine.B_VAL_0_FLT);
		}

		// 텍스트 상태 갱신 주기가 지났을 경우
		if(m_fSkipTime.ExIsGreateEquals(KCDefine.SS_DELTA_T_UPDATE_STATE)) {
			m_nNumDots = (m_nNumDots + KCDefine.B_VAL_1_INT) % KCDefine.SS_MAX_NUM_DOTS;
			m_fSkipTime = KCDefine.B_VAL_0_FLT;

			this.UpdateTextsState();
		}
	}

	//! 시작 씬 이벤트를 수신했을 경우
	protected override void OnReceiveStartSceneEvent(EStartSceneEvent a_eEvent) {
		int nEvent = (int)a_eEvent + KCDefine.B_VAL_1_INT;
		float fPercent = nEvent / (float)((int)EStartSceneEvent.MAX_VAL - KCDefine.B_VAL_1_INT);

		m_fMaxPercent = Mathf.Clamp(fPercent, KCDefine.B_VAL_0_FLT, KCDefine.B_VAL_1_FLT);
	}

	//! 텍스트 상태를 갱신한다
	private void UpdateTextsState() {
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
}
#endif			// #if NEVER_USE_THIS
