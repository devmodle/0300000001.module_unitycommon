using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using DG.Tweening;

namespace SetupScene
{
	/** 설정 씬 관리자 */
	public abstract partial class CSetupSceneManager : CSceneManager
	{
		#region 함수
		/** 초기화 */
		public override void Awake()
		{
			base.Awake();

			// 초기화가 필요 할 경우
			if(!CSceneManager.IsInit)
			{
				return;
			}

			var oLoadingText = this.UIs.ExFindChild($"{EKey.LOADING_TEXT}");
			var oLoadingGauge = this.UIs.ExFindChild($"{EKey.LOADING_GAUGE}");

			CStorageInfoAppCommon.Inst.IncrAppRunningTimes(KCDefine.B_VAL_1_INT);
			CStorageInfoAppCommon.Inst.SaveAppInfo();

			this.DeviceMsgHandlerDict.TryAdd(KCDefine.B_CMD_GET_DEVICE_ID, this.OnReceiveGetDeviceIDMsg);
			this.DeviceMsgHandlerDict.TryAdd(KCDefine.B_CMD_GET_DEVICE_TYPE, this.OnReceiveGetDeviceTypeMsg);
			this.DeviceMsgHandlerDict.TryAdd(KCDefine.B_CMD_GET_COUNTRY_CODE, this.OnReceiveGetCountryCodeMsg);

			// 객체를 설정한다 {
			CFunc.SetupGameObjs(new List<(EKey, string, GameObject, GameObject)>()
			{
				(EKey.LOADING_GAUGE, $"{EKey.LOADING_GAUGE}", this.UIs, CManagerRes.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_LOADING_GAUGE))
			}, m_oUIDict);

			m_oUIDict[EKey.LOADING_GAUGE].transform.localPosition = (oLoadingGauge != null) ?
				m_oUIDict[EKey.LOADING_GAUGE].transform.localPosition : this.LoadingGaugePos;

			m_oUIDict[EKey.LOADING_GAUGE].SetActive(!this.IsIgnoreLoadingGauge);
			// 객체를 설정한다 }

			// 텍스트를 설정한다 {
			CFunc.SetupComponents(new List<(EKey, string, GameObject, GameObject)>()
			{
				(EKey.LOADING_TEXT, $"{EKey.LOADING_TEXT}", this.UIs, CManagerRes.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_LOADING_TEXT))
			}, m_oTextDict);

			m_oTextDict[EKey.LOADING_TEXT].transform.localPosition = (oLoadingText != null) ?
				m_oTextDict[EKey.LOADING_TEXT].transform.localPosition : this.LoadingTextPos;

			m_oTextDict[EKey.LOADING_TEXT].gameObject.SetActive(!this.IsIgnoreLoadingText);
			// 텍스트를 설정한다 }

			// 게이지 처리자를 설정한다
			CFunc.SetupComponents(new List<(EKey, GameObject)>() 
			{
				(EKey.LOADING_GAUGE_HANDLER, m_oUIDict[EKey.LOADING_GAUGE])
			}, m_oGaugeHandlerDict);

#if DEBUG || DEVELOPMENT
			// 텍스트를 설정한다 {
			CFunc.SetupComponents(new List<(EKey, string, GameObject, GameObject)>()
			{
				(EKey.SCENE_INFO_TEXT, $"{EKey.SCENE_INFO_TEXT}", this.StretchUpUIs, CManagerRes.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_G_INFO_TEXT))
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
		public sealed override void Start()
		{
			base.Start();

			// 초기화가 필요 할 경우
			if(!CSceneManager.IsInit)
			{
				return;
			}

			StartCoroutine(this.CoStart());
		}

		/** 애니메이션을 리셋한다 */
		public virtual void ResetAnim()
		{
			CAccess.AssignVal(ref m_oGaugeAnim, null);
		}

		/** 제거되었을 경우 */
		public override void OnDestroy()
		{
			base.OnDestroy();

			try
			{
				// 앱이 종료되었을 경우
				if(!CSceneManager.IsRunningApp)
				{
					return;
				}

				this.ResetAnim();
			}
			catch(System.Exception oException)
			{
				CFunc.ShowLogWarning($"CSetupSceneManager.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 씬을 설정한다 */
		protected virtual void Setup()
		{
			this.SetupPopupUIs();
			this.SetupTopmostUIs();
			this.SetupAbsUIs();
			this.SetupTimerManager();

			foreach(var stKeyVal in this.MaxNumFXSndsDict)
			{
				CManagerSnd.Inst.SetMaxNumFXSnds(stKeyVal.Key, stKeyVal.Value);
			}

#if DEBUG || DEVELOPMENT_BUILD
			this.SetupDebugUIs();
#endif // #if DEBUG || DEVELOPMENT_BUILD

#if PURCHASE_MODULE_ENABLE
			CProductInfoTable.Inst.LoadProductInfos();
#endif // #if PURCHASE_MODULE_ENABLE
		}

		/** 텍스트 상태를 갱신한다 */
		private void UpdateUIsState()
		{
			m_oStrBuilderA.Clear();
			m_oStrBuilderA.Append(CStrTable.Inst.GetStr(KCDefine.G_ST_KEY_SETUP_SM_LOADING_TEXT));

			string oPercentStr = string.Format(KCDefine.B_TEXT_FMT_1_INT, m_oGaugeHandlerDict[EKey.LOADING_GAUGE_HANDLER].Percent * KCDefine.B_UNIT_NORM_VAL_TO_PERCENT);
			oPercentStr = string.Format(KCDefine.B_TEXT_FMT_BRACKET, string.Format(KCDefine.B_TEXT_FMT_PERCENT, oPercentStr));

			CLocalizeInfoTable.Inst.TryGetFontSetInfo(string.Empty, SystemLanguage.English, EFontSet._1, out STFontSetInfo stFontSetInfo);
			m_oTextDict[EKey.LOADING_TEXT].ExSetText(string.Format(KCDefine.B_TEXT_FMT_2_SPACE_COMBINE, m_oStrBuilderA.ToString(), oPercentStr), stFontSetInfo);
		}

		/** 팝업 UI 를 설정한다 */
		private void SetupPopupUIs()
		{
			// UI 설정이 불가능 할 경우
			if(CSetupSceneManager.m_oPopupUIs != null)
			{
				return;
			}

			CSetupSceneManager.m_oPopupUIs = CFactory.CreateCloneGameObj(KCDefine.U_OBJ_N_SCREEN_POPUP_UIS,
				CManagerRes.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_SCREEN_POPUP_UIS), null);

			DontDestroyOnLoad(CSetupSceneManager.m_oPopupUIs);
			CFunc.SetupScreenUIs(CSetupSceneManager.m_oPopupUIs, KCDefine.G_SORTING_O_UIS_POPUP_SCREEN);

			// UI 를 설정한다 {
			var oPopupUIs = CSetupSceneManager.m_oPopupUIs.ExFindChild(KCDefine.U_OBJ_N_SCREEN_POPUP_UIS,
				false);

			CSceneManager.SetScreenPopupUIs(oPopupUIs);
			// UI 를 설정한다 }
		}

		/** 최상위 UI 를 설정한다 */
		private void SetupTopmostUIs()
		{
			// UI 설정이 불가능 할 경우
			if(CSetupSceneManager.m_oTopmostUIs != null)
			{
				return;
			}

			CSetupSceneManager.m_oTopmostUIs = CFactory.CreateCloneGameObj(KCDefine.U_OBJ_N_SCREEN_TOPMOST_UIS,
				CManagerRes.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_SCREEN_TOPMOST_UIS), null);

			DontDestroyOnLoad(CSetupSceneManager.m_oTopmostUIs);
			CFunc.SetupScreenUIs(CSetupSceneManager.m_oTopmostUIs, KCDefine.G_SORTING_O_UIS_TOPMOST_SCREEN);

			// UI 를 설정한다 {
			var oTopmostUIs = CSetupSceneManager.m_oTopmostUIs.ExFindChild(KCDefine.U_OBJ_N_SCREEN_TOPMOST_UIS,
				false);

			CSceneManager.SetScreenTopmostUIs(oTopmostUIs);
			// UI 를 설정한다 }
		}

		/** 절대 UI 를 설정한다 */
		private void SetupAbsUIs()
		{
			// UI 설정이 불가능 할 경우
			if(CSetupSceneManager.m_oAbsUIs != null)
			{
				return;
			}

			CSetupSceneManager.m_oAbsUIs = CFactory.CreateCloneGameObj(KCDefine.U_OBJ_N_SCREEN_ABS_UIS,
				CManagerRes.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_SCREEN_ABS_UIS), null);

			DontDestroyOnLoad(CSetupSceneManager.m_oAbsUIs);
			CFunc.SetupScreenUIs(CSetupSceneManager.m_oAbsUIs, KCDefine.G_SORTING_O_UIS_ABS_SCREEN);

			// UI 를 설정한다 {
			var oAbsUIs = CSetupSceneManager.m_oAbsUIs.ExFindChild(KCDefine.U_OBJ_N_SCREEN_ABS_UIS,
				false);

			CSceneManager.SetScreenAbsUIs(oAbsUIs);
			// UI 를 설정한다 }
		}

		/** 타이머 관리자를 설정한다 */
		private void SetupTimerManager()
		{
			// 관리자 설정이 불가능 할 경우
			if(CSetupSceneManager.m_oTimerManager != null)
			{
				return;
			}

			CSetupSceneManager.m_oTimerManager = CFactory.CreateCloneGameObj(KCDefine.SS_OBJ_N_TIMER_MANAGER,
				CManagerRes.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_TIMER_MANAGER), null);
		}

		/** 디바이스 메세지를 수신했을 경우 */
		private void OnReceiveDeviceMsg(string a_oCmd, string a_oMsg)
		{
			this.DeviceMsgHandlerDict.ExGetVal(a_oCmd)?.Invoke(a_oMsg);
		}

		/** 디바이스 식별자 반환 메세지를 수신했을 경우 */
		private void OnReceiveGetDeviceIDMsg(string a_oMsg)
		{
			bool bIsValidDeviceID = CStorageInfoAppCommon.Inst.AppInfo.DeviceID.ExIsValid();
			bIsValidDeviceID = bIsValidDeviceID && !CStorageInfoAppCommon.Inst.AppInfo.DeviceID.Equals(KCDefine.B_TEXT_UNKNOWN);

			// 디바이스 식별자 설정이 필요 할 경우
			if(!bIsValidDeviceID)
			{
				CStorageInfoAppCommon.Inst.AppInfo.DeviceID = a_oMsg.ExIsValid() ? 
					a_oMsg : KCDefine.B_TEXT_UNKNOWN;
			}

			CStorageInfoAppCommon.Inst.SaveAppInfo();
			CUnityMsgSender.Inst.SendGetDeviceTypeMsg(this.OnReceiveDeviceMsg);
		}

		/** 디바이스 타입 반환 메세지를 수신했을 경우 */
		private void OnReceiveGetDeviceTypeMsg(string a_oMsg)
		{
			bool bIsValidDeviceType = System.Enum.TryParse(a_oMsg, out EDeviceType a_eDeviceType);
			bIsValidDeviceType = bIsValidDeviceType && a_eDeviceType.ExIsValid();

			CStorageInfoAppCommon.Inst.SetDeviceType(bIsValidDeviceType ? 
				a_eDeviceType : EDeviceType.UNKNOWN);

			CUnityMsgSender.Inst.SendGetCountryCodeMsg(this.OnReceiveDeviceMsg);
		}

		/** 국가 코드 반환 메세지를 수신했을 경우 */
		private void OnReceiveGetCountryCodeMsg(string a_oMsg)
		{
#if UNITY_EDITOR
			CStorageInfoAppCommon.Inst.SetCountryCode(a_oMsg.ExIsValid() ? 
				a_oMsg.ToUpper() : KCDefine.B_KOREA_COUNTRY_CODE);
#else
			CStorageInfoAppCommon.Inst.SetCountryCode(a_oMsg.ExIsValid() ? 
				a_oMsg.ToUpper() : KCDefine.B_AMERICA_COUNTRY_CODE);
#endif // #if UNITY_EDITOR

			CSceneManager.SetEnableSetup(true);
			CStorageInfoAppCommon.Inst.SaveAppInfo();

#if SCENE_TEMPLATES_MODULE_ENABLE
			CSceneLoader.Inst.LoadAdditionalScene(KCDefine.B_SCENE_N_LATE_SETUP);
#endif // #if SCENE_TEMPLATES_MODULE_ENABLE
		}

		/** 설정 씬 이벤트를 수신했을 경우 */
		private void OnReceiveSetupSceneEvent(ESetupSceneEvent a_eEvent)
		{
			int nVal = (int)(a_eEvent + KCDefine.B_VAL_1_INT);
			float fPercent = Mathf.Clamp01(nVal / (float)ESetupSceneEvent.MAX_VAL);

			CAccess.AssignVal(ref m_oGaugeAnim,
				this.StartLoadingGaugeAnim(m_oGaugeHandlerDict[EKey.LOADING_GAUGE_HANDLER], (a_fVal) => this.UpdateUIsState(), null, m_oGaugeHandlerDict[EKey.LOADING_GAUGE_HANDLER].Percent, fPercent, KCDefine.U_DURATION_ANI * KCDefine.B_VAL_2_REAL));

#if DEBUG || DEVELOPMENT
			CLocalizeInfoTable.Inst.TryGetFontSetInfo(string.Empty, 
				SystemLanguage.English, EFontSet._1, out STFontSetInfo stFontSetInfo);

			try
			{
				m_oTextDict[EKey.SCENE_INFO_TEXT].ExSetText(m_oStrBuilderB.ToString(), stFontSetInfo);
				m_oStrBuilderB.AppendLine($"{a_eEvent}: {m_oStopwatch.ElapsedMilliseconds} ms");
			}
			finally
			{
				m_oStopwatch.Restart();
			}
#endif // #if DEBUG || DEVELOPMENT
		}

		/** 로딩 게이지 애니메이션을 시작한다 */
		private Sequence StartLoadingGaugeAnim(CGaugeHandler a_oGaugeHandler, 
			System.Action<float> a_oCallback, System.Action<CGaugeHandler, Sequence> a_oCompleteCallback, float a_fStartVal, float a_fEndVal, float a_fDuration, Ease a_eEase = KCDefine.U_EASE_DEF, float a_fDelay = KCDefine.B_VAL_0_REAL, bool a_bIsRealtime = false)
		{
			CFunc.Assert(a_oGaugeHandler != null);
			return CFactory.MakeSequence(CFactory.MakeAnim(() => a_oGaugeHandler.Percent, (a_fVal) => a_oGaugeHandler.SetPercent(a_fVal), () => a_oGaugeHandler.SetPercent(a_fStartVal), a_oCallback, a_fEndVal, a_fDuration, a_eEase, a_bIsRealtime), (a_oAnimSender) => CFunc.Invoke(ref a_oCompleteCallback, a_oGaugeHandler, a_oAnimSender), a_fDelay, a_bIsRealtime: a_bIsRealtime);
		}

#if DEBUG || DEVELOPMENT_BUILD
		/** 디버그 UI 를 설정한다 */
		private void SetupDebugUIs()
		{
			// UI 설정이 불가능 할 경우
			if(CSetupSceneManager.m_oDebugUIs != null)
			{
				return;
			}

			CSetupSceneManager.m_oDebugUIs = CFactory.CreateCloneGameObj(KCDefine.U_OBJ_N_SCREEN_DEBUG_UIS,
				CManagerRes.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_SCREEN_DEBUG_UIS), null);

			DontDestroyOnLoad(CSetupSceneManager.m_oDebugUIs);
			CFunc.SetupScreenUIs(CSetupSceneManager.m_oDebugUIs, KCDefine.G_SORTING_O_UIS_DEBUG_SCREEN);

			// UI 를 설정한다 {
			var oDebugUIs = CSetupSceneManager.m_oDebugUIs.ExFindChild(KCDefine.U_OBJ_N_SCREEN_DEBUG_UIS,
				false);

			var oFPSInfoUIs = CSetupSceneManager.m_oDebugUIs.ExFindChild(KCDefine.U_OBJ_N_SCREEN_FPS_INFO_UIS);
			oFPSInfoUIs.SetActive(false);

			var oDebugInfoUIs = CSetupSceneManager.m_oDebugUIs.ExFindChild(KCDefine.U_OBJ_N_SCREEN_DEBUG_INFO_UIS);
			oDebugInfoUIs.SetActive(false);

			CSceneManager.SetScreenDebugUIs(oDebugUIs);
			CSceneManager.SetScreenFPSInfoUIs(oFPSInfoUIs);
			CSceneManager.SetScreenDebugInfoUIs(oDebugInfoUIs);
			// UI 를 설정한다 }
		}
#endif // #if DEBUG || DEVELOPMENT_BUILD
		#endregion // 함수
	}

	/** 설정 씬 관리자 - 코루틴 */
	public abstract partial class CSetupSceneManager : CSceneManager
	{
		#region 함수
		/** 초기화 */
		private IEnumerator CoStart()
		{
			yield return CAccess.CoGetWaitForSecs(KCDefine.U_DELAY_INIT);
			this.Setup();

			CUnityMsgSender.Inst.SendGetDeviceIDMsg(this.OnReceiveDeviceMsg);
		}
		#endregion // 함수
	}
}
