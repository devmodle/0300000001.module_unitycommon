using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

#if NEVER_USE_THIS
#if SCENE_TEMPLATES_MODULE_ENABLE
/** 서브 시작 씬 관리자 */
public class CSubStartSceneManager : CStartSceneManager {
	#region 변수
	private int m_nNumDots = 0;
	private Vector3 m_stLoadingTextPos = new Vector3(0.0f, 40.0f, 0.0f);
	private Vector3 m_stLoadingGaugePos = new Vector3(0.0f, -40.0f, 0.0f);

	private Tween m_oGaugeAni = null;
	private System.Text.StringBuilder m_oStrBuilder = new System.Text.StringBuilder();

	/** =====> UI <===== */
	private TMP_Text m_oLoadingText = null;
	private CGaugeHandler m_oGaugeHandler = null;
	#endregion			// 변수

	#region 추가 변수

	#endregion			// 추가 변수

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			this.SetupAwake();
		}
	}

	/** 제거 되었을 경우 */
	public override void OnDestroy() {
		base.OnDestroy();

		try {
			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAwake || CSceneManager.IsAppRunning) {
				m_oGaugeAni?.Kill();
				CScheduleManager.Inst.RemoveTimer(this);
			}
		} catch(System.Exception oException) {
			CFunc.ShowLogWarning($"CSubStartSceneManager.OnDestroy Exception: {oException.Message}");
		}
	}

	/** 씬을 설정한다 */
	protected override void Setup() {
		base.Setup();
		this.UpdateUIsState();
	}

	/** 시작 씬 이벤트를 수신했을 경우 */
	protected override void OnReceiveStartSceneEvent(EStartSceneEvent a_eEvent) {
		CAccess.AssignVal(ref m_oGaugeAni, m_oGaugeHandler.ExStartGaugeAni(m_oGaugeHandler.Percent, Mathf.Clamp01((int)(a_eEvent + KCDefine.B_VAL_1_INT) / (float)((int)EStartSceneEvent.MAX_VAL)), KCDefine.U_DURATION_ANI));
	}

	/** 씬을 설정한다 */
	private void SetupAwake() {
		CScheduleManager.Inst.AddRepeatTimer(this, KCDefine.SS_DELTA_T_UPDATE_STATE, () => { m_nNumDots = (m_nNumDots + KCDefine.B_VAL_1_INT) % KCDefine.SS_MAX_NUM_DOTS; this.UpdateUIsState(); });
		CLocalizeInfoTable.Inst.TryGetFontSetInfo(string.Empty, SystemLanguage.English, EFontSet.A, out STFontSetInfo stFontSetInfo);
			
		// 텍스트를 설정한다 {
		var oLoadingText = this.UIsBase.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_LOADING_TEXT);

		m_oLoadingText = oLoadingText ?? CFactory.CreateCloneObj<TMP_Text>(KCDefine.U_OBJ_N_LOADING_TEXT, KCDefine.SS_OBJ_P_LOADING_TEXT, this.UIs, m_stLoadingTextPos);
		m_oLoadingText.ExSetText(KCDefine.SS_TEXT_LOADING, stFontSetInfo);
		// 텍스트를 설정한다 }

		// 게이지 처리자를 설정한다 {
		var oLoadingGauge = this.UIsBase.ExFindChild(KCDefine.SS_OBJ_N_LOADING_GAUGE);
		oLoadingGauge = oLoadingGauge ?? CFactory.CreateCloneObj(KCDefine.SS_OBJ_N_LOADING_GAUGE, KCDefine.SS_OBJ_P_LOADING_GAUGE, this.UIs, m_stLoadingGaugePos);

		m_oGaugeHandler = oLoadingGauge.GetComponentInChildren<CGaugeHandler>();
		m_oGaugeHandler.Percent = KCDefine.B_VAL_0_FLT;
		// 게이지 처리자를 설정한다 }
	}

	/** 텍스트 상태를 갱신한다 */
	private void UpdateUIsState() {
		m_oStrBuilder.Clear();
		m_oStrBuilder.Append(KCDefine.SS_TEXT_LOADING);
		
		CLocalizeInfoTable.Inst.TryGetFontSetInfo(string.Empty, SystemLanguage.English, EFontSet.A, out STFontSetInfo stFontSetInfo);

		for(int i = 0; i < m_nNumDots + KCDefine.B_VAL_1_INT; ++i) {
			m_oStrBuilder.Append(CStrTable.Inst.GetStr(KCDefine.ST_KEY_START_SM_DOT_TEXT));
		}
		
		m_oLoadingText.ExSetText(m_oStrBuilder.ToString(), stFontSetInfo);
	}
	#endregion			// 함수
	
	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if SCENE_TEMPLATES_MODULE_ENABLE
#endif			// #if NEVER_USE_THIS
