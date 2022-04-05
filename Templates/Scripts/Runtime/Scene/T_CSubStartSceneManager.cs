using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

#if SCRIPT_TEMPLATE_ONLY
#if SCENE_TEMPLATES_MODULE_ENABLE
namespace StartScene {
	/** 서브 시작 씬 관리자 */
	public partial class CSubStartSceneManager : CStartSceneManager {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			STR_BUILDER_A,
			STR_BUILDER_B,
			LOADING_TEXT,
			SCENE_INFO_TEXT,
			GAUGE_HANDLER,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		private Vector3 m_stLoadingTextPos = new Vector3(0.0f, 35.0f, 0.0f);
		private Vector3 m_stLoadingGaugePos = new Vector3(0.0f, -35.0f, 0.0f);

		private Sequence m_oGaugeAni = null;
		private System.Diagnostics.Stopwatch m_oStopwatch = new System.Diagnostics.Stopwatch();

		private Dictionary<EKey, System.Text.StringBuilder> m_oStrBuilderDict = new Dictionary<EKey, System.Text.StringBuilder>() {
			[EKey.STR_BUILDER_A] = new System.Text.StringBuilder(),
			[EKey.STR_BUILDER_B] = new System.Text.StringBuilder()
		};

		/** =====> UI <===== */
		private Dictionary<EKey, TMP_Text> m_oTextDict = new Dictionary<EKey, TMP_Text>() {
			[EKey.LOADING_TEXT] = null,
			[EKey.SCENE_INFO_TEXT] = null
		};

		private Dictionary<EKey, CGaugeHandler> m_oGaugeHandlerDict = new Dictionary<EKey, CGaugeHandler>() {
			[EKey.GAUGE_HANDLER] = null
		};
		#endregion			// 변수

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
#if DEBUG || DEVELOPMENT
			CLocalizeInfoTable.Inst.TryGetFontSetInfo(string.Empty, SystemLanguage.English, EFontSet._1, out STFontSetInfo stFontSetInfo);

			try {
				m_oStrBuilderDict[EKey.STR_BUILDER_B].AppendLine($"{a_eEvent}: {m_oStopwatch.ElapsedMilliseconds} ms");
				m_oTextDict[EKey.SCENE_INFO_TEXT].ExSetText(m_oStrBuilderDict[EKey.STR_BUILDER_B].ToString(), stFontSetInfo);
			} finally {
				m_oStopwatch.Restart();
			}
#endif			// #if DEBUG || DEVELOPMENT

#if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
			float fPercent = Mathf.Clamp01((int)(a_eEvent + KCDefine.B_VAL_1_INT) / (float)EStartSceneEvent.MAX_VAL);
			CAccess.AssignVal(ref m_oGaugeAni, m_oGaugeHandlerDict[EKey.GAUGE_HANDLER].ExStartGaugeAni((a_fVal) => this.UpdateUIsState(), null, m_oGaugeHandlerDict[EKey.GAUGE_HANDLER].Percent, fPercent, KCDefine.U_DURATION_ANI));
#endif			// #if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
		}

		/** 씬을 설정한다 */
		private void SetupAwake() {	
			// 텍스트를 설정한다
			var oLoadingText = this.UIsBase.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_LOADING_TEXT);
			m_oTextDict[EKey.LOADING_TEXT] = oLoadingText ?? CFactory.CreateCloneObj<TMP_Text>(KCDefine.U_OBJ_N_LOADING_TEXT, CResManager.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_LOADING_TEXT), this.UIs, m_stLoadingTextPos);

			// 게이지 처리자를 설정한다 {
			var oLoadingGauge = this.UIsBase.ExFindChild(KCDefine.SS_OBJ_N_LOADING_GAUGE);
			oLoadingGauge = oLoadingGauge ?? CFactory.CreateCloneObj(KCDefine.SS_OBJ_N_LOADING_GAUGE, CResManager.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_LOADING_GAUGE), this.UIs, m_stLoadingGaugePos);

			m_oGaugeHandlerDict[EKey.GAUGE_HANDLER] = oLoadingGauge.GetComponentInChildren<CGaugeHandler>();
			m_oGaugeHandlerDict[EKey.GAUGE_HANDLER].Percent = KCDefine.B_VAL_0_FLT;
			// 게이지 처리자를 설정한다 }

#if DEBUG || DEVELOPMENT
			// 텍스트를 설정한다
			m_oTextDict[EKey.SCENE_INFO_TEXT] = CFactory.CreateCloneObj<TMP_Text>(KCDefine.SS_OBJ_N_SCENE_INFO_TEXT, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_G_INFO_TEXT), this.UpLeftUIs);
			m_oTextDict[EKey.SCENE_INFO_TEXT].rectTransform.pivot = KCDefine.B_ANCHOR_UP_LEFT;
			m_oTextDict[EKey.SCENE_INFO_TEXT].rectTransform.anchorMin = KCDefine.B_ANCHOR_UP_LEFT;
			m_oTextDict[EKey.SCENE_INFO_TEXT].rectTransform.anchorMax = KCDefine.B_ANCHOR_UP_LEFT;
			m_oTextDict[EKey.SCENE_INFO_TEXT].rectTransform.anchoredPosition = KCDefine.B_ANCHOR_UP_LEFT;

			m_oStopwatch.Start();
			this.OnReceiveStartSceneEvent(EStartSceneEvent.LOAD_START_SCENE);
#endif			// #if DEBUG || DEVELOPMENT
		}

		/** 텍스트 상태를 갱신한다 */
		private void UpdateUIsState() {
			m_oStrBuilderDict[EKey.STR_BUILDER_A].Clear();
			m_oStrBuilderDict[EKey.STR_BUILDER_A].Append(CStrTable.Inst.GetStr(KCDefine.ST_KEY_START_SM_LOADING_TEXT));

			string oPercentStr = string.Format(KCDefine.B_TEXT_FMT_1_DIGITS, m_oGaugeHandlerDict[EKey.GAUGE_HANDLER].Percent * KCDefine.B_UNIT_NORM_VAL_TO_PERCENT);
			oPercentStr = string.Format(KCDefine.B_TEXT_FMT_BRACKET, string.Format(KCDefine.B_TEXT_FMT_PERCENT, oPercentStr));

			CLocalizeInfoTable.Inst.TryGetFontSetInfo(string.Empty, SystemLanguage.English, EFontSet._1, out STFontSetInfo stFontSetInfo);
			m_oTextDict[EKey.LOADING_TEXT].ExSetText(string.Format(KCDefine.B_TEXT_FMT_2_SPACE_COMBINE, m_oStrBuilderDict[EKey.STR_BUILDER_A].ToString(), oPercentStr), stFontSetInfo);
		}
		#endregion			// 함수
	}
}
#endif			// #if SCENE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
