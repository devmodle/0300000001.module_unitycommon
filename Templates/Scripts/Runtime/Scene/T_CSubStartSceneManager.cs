using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

#if SCRIPT_TEMPLATE_ONLY
#if SCENE_TEMPLATES_MODULE_ENABLE
/** 서브 시작 씬 관리자 */
public class CSubStartSceneManager : CStartSceneManager {
	#region 변수
	private Vector3 m_stLoadingTextPos = new Vector3(0.0f, 35.0f, 0.0f);
	private Vector3 m_stLoadingGaugePos = new Vector3(0.0f, -35.0f, 0.0f);

	private Sequence m_oGaugeAni = null;
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
		float fPercent = Mathf.Clamp01((int)(a_eEvent + KCDefine.B_VAL_1_INT) / (float)EStartSceneEvent.MAX_VAL);
		CAccess.AssignVal(ref m_oGaugeAni, m_oGaugeHandler.ExStartGaugeAni((a_fVal) => this.UpdateUIsState(), null, m_oGaugeHandler.Percent, fPercent, KCDefine.U_DURATION_ANI));
	}

	/** 씬을 설정한다 */
	private void SetupAwake() {	
		// 텍스트를 설정한다
		var oLoadingText = this.UIsBase.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_LOADING_TEXT);
		m_oLoadingText = oLoadingText ?? CFactory.CreateCloneObj<TMP_Text>(KCDefine.U_OBJ_N_LOADING_TEXT, KCDefine.SS_OBJ_P_LOADING_TEXT, this.UIs, m_stLoadingTextPos);

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
		m_oStrBuilder.Append(CStrTable.Inst.GetStr(KCDefine.ST_KEY_START_SM_LOADING_TEXT));
		
		CLocalizeInfoTable.Inst.TryGetFontSetInfo(string.Empty, SystemLanguage.English, EFontSet._1, out STFontSetInfo stFontSetInfo);

		string oPercentStr = string.Format(KCDefine.B_TEXT_FMT_1_DIGITS, m_oGaugeHandler.Percent * KCDefine.B_UNIT_NORM_VAL_TO_PERCENT);
		oPercentStr = string.Format(KCDefine.B_TEXT_FMT_BRACKET, string.Format(KCDefine.B_TEXT_FMT_PERCENT, oPercentStr));

		m_oLoadingText.ExSetText(string.Format(KCDefine.B_TEXT_FMT_2_SPACE_COMBINE, m_oStrBuilder.ToString(), oPercentStr), stFontSetInfo);
	}
	#endregion			// 함수
	
	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if SCENE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
