using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

#if UNITY_IOS
using UnityEngine.iOS;
#endif			// #if UNITY_IOS

//! 액티비티 인디게이터 관리자
public class CActivityIndicatorManager : CSingleton<CActivityIndicatorManager> {
	#region 변수
	private int m_nRefCount = 0;
	#endregion			// 변수

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

#if UNITY_ANDROID
		Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.Large);
#endif			// #if UNITY_ANDROID
	}

	//! 액티비티 인디게이터를 시작한다
	public void StartActivityIndicator(bool a_bIsShowActivityIndicator, bool a_bIsShowBlindUI = true, bool a_bIsResetRefCount = false) {
		m_nRefCount = Mathf.Min(int.MaxValue, m_nRefCount + 1);
		m_nRefCount = a_bIsResetRefCount ? 1 : m_nRefCount;

		if(m_nRefCount >= 1) {
			if(a_bIsShowActivityIndicator) {
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
				CUnityMessageSender.Instance.SendActivityIndicatorMessage(true);
#else
				Function.ShowLog("CActivityIndicatorManager.StartActivityIndicator: {0}", Color.blue, m_nRefCount);
#endif			// #if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
			}

			// 블라인드 UI 를 출력한다
			if(a_bIsShowBlindUI) {
				var oGameObject = CSceneManager.ScreenTopmostUIRoot ?? CSceneManager.TopmostUIRoot;

				CSceneManager.ShowTouchResponder(KDefine.U_OBJ_NAME_ACTIVITY_I_TOUCH_RESPONDER,
					oGameObject, KDefine.U_DEF_COLOR_ACTIVITY_INDICATOR_BG, null, false, true);
			}
		}
	}

	//! 액티비티 인디게이터를 정지한다
	public void StopActivityIndicator(bool a_bIsResetRefCount = false) {
		m_nRefCount = Mathf.Max(0, m_nRefCount - 1);
		m_nRefCount = a_bIsResetRefCount ? 0 : m_nRefCount;

		if(m_nRefCount <= 0) {
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
			CUnityMessageSender.Instance.SendActivityIndicatorMessage(false);
#else
			Function.ShowLog("CActivityIndicatorManager.StopActivityIndicator: {0}", Color.blue, m_nRefCount);
#endif			// #if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)

			CSceneManager.CloseTouchResponder(KDefine.U_OBJ_NAME_ACTIVITY_I_TOUCH_RESPONDER, 
				KDefine.U_DEF_COLOR_TRANSPARENT);
		}
	}
	#endregion			// 함수
}
