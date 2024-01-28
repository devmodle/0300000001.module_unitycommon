using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using System.Diagnostics;

using DG.Tweening;

namespace SetupScene {
	/** 설정 씬 관리자 */
	public abstract partial class CSetupSceneManager : CSceneManager {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			LOADING_TEXT,
			SCENE_INFO_TEXT,
			LOADING_GAUGE_HANDLER,

			LOADING_GAUGE,
			[HideInInspector] MAX_VAL
		}

		/** 설정 씬 이벤트 */
		public enum ESetupSceneEvent {
			NONE = -1,
			LOAD_SETUP_SCENE,
			LOAD_LATE_SETUP_SCENE,
			LOAD_NEXT_SCENE,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		private Tween m_oGaugeAni = null;
		private Stopwatch m_oStopwatch = new Stopwatch();

		private System.Text.StringBuilder m_oStrBuilder01 = new System.Text.StringBuilder();
		private System.Text.StringBuilder m_oStrBuilder02 = new System.Text.StringBuilder();
		
		[Header("=====> UIs <=====")]
		private Dictionary<EKey, Text> m_oTextDict = new Dictionary<EKey, Text>();
		private Dictionary<EKey, CGaugeHandler> m_oGaugeHandlerDict = new Dictionary<EKey, CGaugeHandler>();

		[Header("=====> Game Objects <=====")]
		private Dictionary<EKey, GameObject> m_oUIsDict = new Dictionary<EKey, GameObject>();
		#endregion // 변수

		#region 클래스 변수
		[Header("=====> Game Objects <=====")]
		private static GameObject m_oPopupUIs = null;
		private static GameObject m_oTopmostUIs = null;
		private static GameObject m_oAbsUIs = null;
		private static GameObject m_oDebugUIs = null;
		private static GameObject m_oTimerManager = null;
		#endregion // 클래스 변수

		#region 프로퍼티
		public virtual bool IsIgnoreLoadingText => false;
		public virtual bool IsIgnoreLoadingGauge => false;

		public virtual Vector3 LoadingTextPos => Vector3.zero;
		public virtual Vector3 LoadingGaugePos => Vector3.zero;

		protected Dictionary<string, int> MaxNumFXSndsDict { get; } = new Dictionary<string, int>();
		protected Dictionary<string, System.Action<string>> DeviceMsgHandlerDict { get; } = new Dictionary<string, System.Action<string>>();

#if UNITY_EDITOR
		public override int ScriptOrder => KCDefine.U_SCRIPT_O_SETUP_SCENE_MANAGER;
#endif // #if UNITY_EDITOR
		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 초기화가 필요 할 경우
			if(!CSceneManager.IsInit) {
				return;
			}

			var oLoadingText = this.UIs.ExFindChild($"{EKey.LOADING_TEXT}");
			var oLoadingGauge = this.UIs.ExFindChild($"{EKey.LOADING_GAUGE}");
			
			CCommonAppInfoStorage.Inst.IncrAppRunningTimes(KCDefine.B_VAL_1_INT);
			CCommonAppInfoStorage.Inst.SaveAppInfo();

			this.DeviceMsgHandlerDict.TryAdd(KCDefine.B_CMD_GET_DEVICE_ID, this.OnReceiveGetDeviceIDMsg);
			this.DeviceMsgHandlerDict.TryAdd(KCDefine.B_CMD_GET_DEVICE_TYPE, this.OnReceiveGetDeviceTypeMsg);
			this.DeviceMsgHandlerDict.TryAdd(KCDefine.B_CMD_GET_COUNTRY_CODE, this.OnReceiveGetCountryCodeMsg);

			// 객체를 설정한다 {
			CFunc.SetupGameObjs(new List<(EKey, string, GameObject, GameObject)>() {
				(EKey.LOADING_GAUGE, $"{EKey.LOADING_GAUGE}", this.UIs, CResManager.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_LOADING_GAUGE))
			}, m_oUIsDict);

			m_oUIsDict[EKey.LOADING_GAUGE].transform.localPosition = (oLoadingGauge != null) ? 
				m_oUIsDict[EKey.LOADING_GAUGE].transform.localPosition : this.LoadingGaugePos;

			m_oUIsDict[EKey.LOADING_GAUGE].SetActive(!this.IsIgnoreLoadingGauge);
			// 객체를 설정한다 }

			// 텍스트를 설정한다 {
			CFunc.SetupComponents(new List<(EKey, string, GameObject, GameObject)>() {
				(EKey.LOADING_TEXT, $"{EKey.LOADING_TEXT}", this.UIs, CResManager.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_LOADING_TEXT))
			}, m_oTextDict);

			m_oTextDict[EKey.LOADING_TEXT].transform.localPosition = (oLoadingText != null) ? 
				m_oTextDict[EKey.LOADING_TEXT].transform.localPosition : this.LoadingTextPos;

			m_oTextDict[EKey.LOADING_TEXT].gameObject.SetActive(!this.IsIgnoreLoadingText);
			// 텍스트를 설정한다 }

			// 게이지 처리자를 설정한다
			CFunc.SetupComponents(new List<(EKey, GameObject)>() {
				(EKey.LOADING_GAUGE_HANDLER, m_oUIsDict[EKey.LOADING_GAUGE])
			}, m_oGaugeHandlerDict);

#if DEBUG || DEVELOPMENT
			// 텍스트를 설정한다 {
			CFunc.SetupComponents(new List<(EKey, string, GameObject, GameObject)>() {
				(EKey.SCENE_INFO_TEXT, $"{EKey.SCENE_INFO_TEXT}", this.StretchUpUIs, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_G_INFO_TEXT))
			}, m_oTextDict);

			m_oTextDict[EKey.SCENE_INFO_TEXT].rectTransform.pivot = KCDefine.B_ANCHOR_UP_LEFT;
			m_oTextDict[EKey.SCENE_INFO_TEXT].rectTransform.anchorMin = KCDefine.B_ANCHOR_UP_LEFT;
			m_oTextDict[EKey.SCENE_INFO_TEXT].rectTransform.anchorMax = KCDefine.B_ANCHOR_UP_LEFT;
			m_oTextDict[EKey.SCENE_INFO_TEXT].rectTransform.anchoredPosition = Vector3.zero;
			// 텍스트를 설정한다 }

			m_oStopwatch.Start();
#endif // #if DEBUG || DEVELOPMENT

			this.OnReceiveSetupSceneEvent(ESetupSceneEvent.LOAD_SETUP_SCENE);
		}

		/** 초기화 */
		public sealed override void Start() {
			base.Start();

			// 초기화되었을 경우
			if(CSceneManager.IsInit) {
				StartCoroutine(this.CoStart());
			}
		}

		/** 애니메이션을 리셋한다 */
		public virtual void ResetAni() {
			m_oGaugeAni?.Kill();
		}

		/** 제거되었을 경우 */
		public override void OnDestroy() {
			base.OnDestroy();

			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsAppRunning) {
					this.ResetAni();
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CSetupSceneManager.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 씬을 설정한다 */
		protected virtual void Setup() {
			this.SetupPopupUIs();
			this.SetupTopmostUIs();
			this.SetupAbsUIs();
			this.SetupTimerManager();

			foreach(var stKeyVal in this.MaxNumFXSndsDict) {
				CSndManager.Inst.SetMaxNumFXSnds(stKeyVal.Key, stKeyVal.Value);
			}

#if DEBUG || DEVELOPMENT_BUILD
			this.SetupDebugUIs();
#endif // #if DEBUG || DEVELOPMENT_BUILD

#if PURCHASE_MODULE_ENABLE
			CProductInfoTable.Inst.LoadProductInfos();
#endif // #if PURCHASE_MODULE_ENABLE
		}

		/** 텍스트 상태를 갱신한다 */
		private void UpdateUIsState() {
			m_oStrBuilder01.Clear();
			m_oStrBuilder01.Append(CStrTable.Inst.GetStr(KCDefine.ST_KEY_SETUP_SM_LOADING_TEXT));

			string oPercentStr = string.Format(KCDefine.B_TEXT_FMT_1_INT, m_oGaugeHandlerDict[EKey.LOADING_GAUGE_HANDLER].Percent * KCDefine.B_UNIT_NORM_VAL_TO_PERCENT);
			oPercentStr = string.Format(KCDefine.B_TEXT_FMT_BRACKET, string.Format(KCDefine.B_TEXT_FMT_PERCENT, oPercentStr));

			CLocalizeInfoTable.Inst.TryGetFontSetInfo(string.Empty, SystemLanguage.English, EFontSet._1, out STFontSetInfo stFontSetInfo);
			m_oTextDict[EKey.LOADING_TEXT].ExSetText(string.Format(KCDefine.B_TEXT_FMT_2_SPACE_COMBINE, m_oStrBuilder01.ToString(), oPercentStr), stFontSetInfo);
		}

		/** 디바이스 메세지를 수신했을 경우 */
		private void OnReceiveDeviceMsg(string a_oCmd, string a_oMsg) {
			this.DeviceMsgHandlerDict.GetValueOrDefault(a_oCmd)?.Invoke(a_oMsg);
		}

		/** 디바이스 식별자 반환 메세지를 수신했을 경우 */
		private void OnReceiveGetDeviceIDMsg(string a_oMsg) {
			bool bIsValidDeviceIDA = CCommonAppInfoStorage.Inst.AppInfo.DeviceID.ExIsValid();
			bool bIsValidDeviceIDB = !CCommonAppInfoStorage.Inst.AppInfo.DeviceID.Equals(KCDefine.B_TEXT_UNKNOWN);

			// 디바이스 식별자 갱신이 필요 할 경우
			if(!bIsValidDeviceIDA || !bIsValidDeviceIDB) {
				CCommonAppInfoStorage.Inst.AppInfo.DeviceID = a_oMsg.ExIsValid() ? a_oMsg : KCDefine.B_TEXT_UNKNOWN;
			}

			CCommonAppInfoStorage.Inst.SaveAppInfo();
			CUnityMsgSender.Inst.SendGetDeviceTypeMsg(this.OnReceiveDeviceMsg);
		}

		/** 디바이스 타입 반환 메세지를 수신했을 경우 */
		private void OnReceiveGetDeviceTypeMsg(string a_oMsg) {
			bool bIsValid = System.Enum.TryParse<EDeviceType>(a_oMsg, out EDeviceType a_eDeviceType);
			CCommonAppInfoStorage.Inst.SetDeviceType((bIsValid && a_eDeviceType.ExIsValid()) ? a_eDeviceType : EDeviceType.UNKNOWN);

			CUnityMsgSender.Inst.SendGetCountryCodeMsg(this.OnReceiveDeviceMsg);
		}

		/** 국가 코드 반환 메세지를 수신했을 경우 */
		private void OnReceiveGetCountryCodeMsg(string a_oMsg) {
#if UNITY_EDITOR
			CCommonAppInfoStorage.Inst.SetCountryCode(a_oMsg.ExIsValid() ? a_oMsg.ToUpper() : KCDefine.B_KOREA_COUNTRY_CODE);
#else
			CCommonAppInfoStorage.Inst.SetCountryCode(a_oMsg.ExIsValid() ? a_oMsg.ToUpper() : KCDefine.B_AMERICA_COUNTRY_CODE);
#endif // #if UNITY_EDITOR

			CSceneManager.SetEnableSetup(true);
			CCommonAppInfoStorage.Inst.SaveAppInfo();

#if SCENE_TEMPLATES_MODULE_ENABLE
			CSceneLoader.Inst.LoadAdditiveScene(KCDefine.B_SCENE_N_LATE_SETUP);
#endif // #if SCENE_TEMPLATES_MODULE_ENABLE
		}

		/** 설정 씬 이벤트를 수신했을 경우 */
		private void OnReceiveSetupSceneEvent(ESetupSceneEvent a_eEvent) {
			float fPercent = Mathf.Clamp01((int)(a_eEvent + KCDefine.B_VAL_1_INT) / (float)ESetupSceneEvent.MAX_VAL);

			CAccess.AssignVal(ref m_oGaugeAni, 
				this.StartLoadingGaugeAni(m_oGaugeHandlerDict[EKey.LOADING_GAUGE_HANDLER], (a_fVal) => this.UpdateUIsState(), null, m_oGaugeHandlerDict[EKey.LOADING_GAUGE_HANDLER].Percent, fPercent, KCDefine.U_DURATION_ANI * KCDefine.B_VAL_2_REAL));

#if DEBUG || DEVELOPMENT
			CLocalizeInfoTable.Inst.TryGetFontSetInfo(string.Empty, SystemLanguage.English, EFontSet._1, out STFontSetInfo stFontSetInfo);

			try {
				m_oTextDict[EKey.SCENE_INFO_TEXT].ExSetText(m_oStrBuilder02.ToString(), stFontSetInfo);
				m_oStrBuilder02.AppendLine($"{a_eEvent}: {m_oStopwatch.ElapsedMilliseconds} ms");
			} finally {
				m_oStopwatch.Restart();
			}
#endif // #if DEBUG || DEVELOPMENT
		}

		/** 로딩 게이지 애니메이션을 시작한다 */
		private Sequence StartLoadingGaugeAni(CGaugeHandler a_oGaugeHandler, System.Action<float> a_oCallback, System.Action<CGaugeHandler, Sequence> a_oCompleteCallback, float a_fStartVal, float a_fEndVal, float a_fDuration, Ease a_eEase = KCDefine.U_EASE_DEF, float a_fDelay = KCDefine.B_VAL_0_REAL, bool a_bIsRealtime = false) {
			CAccess.Assert(a_oGaugeHandler != null);
			return CFactory.MakeSequence(CFactory.MakeAni(() => a_oGaugeHandler.Percent, (a_fVal) => a_oGaugeHandler.SetPercent(a_fVal), () => a_oGaugeHandler.SetPercent(a_fStartVal), a_oCallback, a_fEndVal, a_fDuration, a_eEase, a_bIsRealtime), (a_oAniSender) => CFunc.Invoke(ref a_oCompleteCallback, a_oGaugeHandler, a_oAniSender), a_fDelay, a_bIsRealtime: a_bIsRealtime);
		}
		#endregion // 함수
	}

	/** 설정 씬 관리자 - 코루틴 */
	public abstract partial class CSetupSceneManager : CSceneManager {
		#region 함수
		/** 초기화 */
		private IEnumerator CoStart() {
			yield return CAccess.CoGetWaitForSecs(KCDefine.U_DELAY_INIT);
			this.Setup();

			CUnityMsgSender.Inst.SendGetDeviceIDMsg(this.OnReceiveDeviceMsg);
		}
		#endregion // 함수
	}
}
