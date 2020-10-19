using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 서브 시작 씬 관리자
public class CSubStartSceneManager : CStartSceneManager {
	#region 변수
	protected Text m_oLoadingText = null;
	protected Image m_oGaugeImg = null;

	private int m_nNumDots = 0;

	private float m_fSkipTime = 0.0f;
	private float m_fMaxPercent = 0.0f;

	private System.Text.StringBuilder m_oStringBuilder = new System.Text.StringBuilder();
	#endregion			// 변수

	#region 객체
	protected GameObject m_oLoadingImgObj = null;
	#endregion			// 객체
	
	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			// 텍스트를 설정한다 {
			m_oLoadingText = CFactory.CreateCloneObj<Text>(KDefine.SS_OBJ_NAME_LOADING_TEXT,
				CResManager.Instance.GetPrefab(KCDefine.SS_OBJ_PATH_LOADING_TEXT), 
				this.SubUIRoot,
				KDefine.SS_POS_LOADING_TEXT);

			m_oLoadingText.text = string.Empty;
			// 텍스트를 설정한다 }

			// 이미지를 설정한다 {
			m_oLoadingImgObj = CFactory.CreateCloneObj(KDefine.SS_OBJ_NAME_LOADING_IMG_OBJ,
				CResManager.Instance.GetPrefab(KCDefine.SS_OBJ_PATH_LOADING_IMG_OBJ),
				this.SubUIRoot,
				KDefine.SS_POS_LOADING_IMG_OBJ);

			m_oGaugeImg = m_oLoadingImgObj.ExFindComponent<Image>(KDefine.SS_OBJ_NAME_GAUGE_IMG);
			m_oGaugeImg.fillAmount = KCDefine.B_VALUE_FLOAT_0;
			// 이미지를 설정한다 }

			this.UpdateTextState();
		}
	}

	//! 상태를 갱신한다
	public override void OnUpdate(float a_fDeltaTime) {
		base.OnUpdate(a_fDeltaTime);
		m_fSkipTime += Time.deltaTime;

		m_oGaugeImg.fillAmount = Mathf.Clamp(m_oGaugeImg.fillAmount + (KCDefine.B_VALUE_FLOAT_1 * a_fDeltaTime),
			KCDefine.B_VALUE_FLOAT_0, m_fMaxPercent);

		// 상태 텍스트 갱신 주기가 지났을 경우
		if(m_fSkipTime.ExIsGreateEquals(KDefine.SS_DELTA_TIME_UPDATE_STATE)) {
			m_nNumDots = (m_nNumDots + KCDefine.B_VALUE_INT_1) % KDefine.SS_MAX_NUM_DOTS;
			m_fSkipTime = KCDefine.B_VALUE_FLOAT_0;

			this.UpdateTextState();
		}
	}

	//! 시작 씬 이벤트를 수신했을 경우
	protected override void OnReceiveStartSceneEvent(EStartSceneEvent a_eEvent) {
		m_fMaxPercent = Mathf.Clamp((int)a_eEvent / (float)((int)EStartSceneEvent.MAX_VALUE - KCDefine.B_VALUE_INT_1), 
			KCDefine.B_VALUE_FLOAT_0, KCDefine.B_VALUE_FLOAT_1);
	}

	//! 텍스트 상태를 갱신한다
	private void UpdateTextState() {
		// 국가 코드가 유효 할 경우
		if(CCommonAppInfoStorage.Instance.CountryCode.ExIsValid()) {
			string oDot = CStringTable.Instance.GetString(KDefine.ST_KEY_START_SM_DOT_TEXT);
			string oLoading = CStringTable.Instance.GetString(KDefine.ST_KEY_START_SM_LOADING_TEXT);

			m_oStringBuilder.Clear();
			m_oStringBuilder.Append(oLoading);
			
			for(int i = 0; i < m_nNumDots + KCDefine.B_VALUE_INT_1; ++i) {
				m_oStringBuilder.Append(oDot);
			}

			m_oLoadingText.text = m_oStringBuilder.ToString();
		}
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
