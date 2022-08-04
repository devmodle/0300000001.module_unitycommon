#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 기본 접근자 확장 클래스 */
public static partial class AccessExtension {
	#region 클래스 함수
	/** 컴포넌트 상호 작용 여부를 변경한다 */
	public static void ExSetInteractable(this Button a_oSender, bool a_bIsEnable, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oSender != null);

		var oTouchInteractable = a_oSender?.GetComponentInChildren<CTouchInteractable>();
		oTouchInteractable?.SetInteractable(a_bIsEnable);
	}

	/** 텍스트를 변경한다 */
	public static void ExSetText(this Text a_oSender, string a_oStr, EFontSet a_eFontSet = EFontSet._1, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oSender != null && a_eFontSet.ExIsValid()));

		// 텍스트가 존재 할 경우
		if(a_oSender != null && a_eFontSet.ExIsValid()) {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
			a_oSender.ExSetText(a_oStr, CLocalizeInfoTable.Inst.GetFontSetInfo(a_eFontSet), a_bIsEnableAssert);
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
		}
	}

	/** 텍스트를 변경한다 */
	public static void ExSetText(this TMP_Text a_oSender, string a_oStr, EFontSet a_eFontSet = EFontSet._1, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oSender != null && a_eFontSet.ExIsValid()));

		// 텍스트가 존재 할 경우
		if(a_oSender != null && a_eFontSet.ExIsValid()) {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
			a_oSender.ExSetText(a_oStr, CLocalizeInfoTable.Inst.GetFontSetInfo(a_eFontSet), a_bIsEnableAssert);
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
		}
	}

	/** 텍스트를 변경한다 */
	public static void ExSetText(this InputField a_oSender, string a_oStr, EFontSet a_eFontSet = EFontSet._1, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oSender != null && a_eFontSet.ExIsValid()));

		// 텍스트가 존재 할 경우
		if(a_oSender != null && a_eFontSet.ExIsValid()) {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
			a_oSender.ExSetText(a_oStr, CLocalizeInfoTable.Inst.GetFontSetInfo(a_eFontSet), a_bIsEnableAssert);
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
		}
	}

	/** 텍스트를 변경한다 */
	public static void ExSetText(this TMP_InputField a_oSender, string a_oStr, EFontSet a_eFontSet = EFontSet._1, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oSender != null && a_eFontSet.ExIsValid()));

		// 텍스트가 존재 할 경우
		if(a_oSender != null && a_eFontSet.ExIsValid()) {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
			a_oSender.ExSetText(a_oStr, CLocalizeInfoTable.Inst.GetFontSetInfo(a_eFontSet), a_bIsEnableAssert);
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
		}
	}

	/** 시작 콜백을 변경한다 */
	public static void ExSetBeginCallback(this CTouchDispatcher a_oSender, System.Action<CTouchDispatcher, PointerEventData> a_oCallback, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oSender != null);

		// 터치 전달자가 존재 할 경우
		if(a_oSender != null) {
			a_oSender.BeginCallback = a_oCallback;
		}
	}

	/** 이동 콜백을 변경한다 */
	public static void ExSetMoveCallback(this CTouchDispatcher a_oSender, System.Action<CTouchDispatcher, PointerEventData> a_oCallback, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oSender != null);

		// 터치 전달자가 존재 할 경우
		if(a_oSender != null) {
			a_oSender.MoveCallback = a_oCallback;
		}
	}

	/** 종료 콜백을 변경한다 */
	public static void ExSetEndCallback(this CTouchDispatcher a_oSender, System.Action<CTouchDispatcher, PointerEventData> a_oCallback, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oSender != null);

		// 터치 전달자가 존재 할 경우
		if(a_oSender != null) {
			a_oSender.EndCallback = a_oCallback;
		}
	}
	#endregion			// 클래스 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
