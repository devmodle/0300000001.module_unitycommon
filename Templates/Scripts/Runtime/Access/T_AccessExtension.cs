using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

#if SCRIPT_TEMPLATE_ONLY
#if RUNTIME_TEMPLATES_MODULE_ENABLE
/** 기본 접근 확장 클래스 */
public static partial class AccessExtension {
	#region 클래스 함수
	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EPlayMode a_eSender) {
		return a_eSender > EPlayMode.NONE && a_eSender < EPlayMode.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EDifficulty a_eSender) {
		return a_eSender > EDifficulty.NONE && a_eSender < EDifficulty.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EStageKinds a_eSender) {
		return a_eSender > EStageKinds.NONE && a_eSender < EStageKinds.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EChapterKinds a_eSender) {
		return a_eSender > EChapterKinds.NONE && a_eSender < EChapterKinds.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EPriceType a_eSender) {
		return a_eSender > EPriceType.NONE && a_eSender < EPriceType.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EPriceKinds a_eSender) {
		return a_eSender > EPriceKinds.NONE && a_eSender < EPriceKinds.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EItemType a_eSender) {
		return a_eSender > EItemType.NONE && a_eSender < EItemType.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EItemKinds a_eSender) {
		return a_eSender > EItemKinds.NONE && a_eSender < EItemKinds.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this ESaleItemType a_eSender) {
		return a_eSender > ESaleItemType.NONE && a_eSender < ESaleItemType.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this ESaleItemKinds a_eSender) {
		return a_eSender > ESaleItemKinds.NONE && a_eSender < ESaleItemKinds.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this ESaleProductType a_eSender) {
		return a_eSender > ESaleProductType.NONE && a_eSender < ESaleProductType.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this ESaleProductKinds a_eSender) {
		return a_eSender > ESaleProductKinds.NONE && a_eSender < ESaleProductKinds.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EMissionType a_eSender) {
		return a_eSender > EMissionType.NONE && a_eSender < EMissionType.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EMissionKinds a_eSender) {
		return a_eSender > EMissionKinds.NONE && a_eSender < EMissionKinds.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this ERewardType a_eSender) {
		return a_eSender > ERewardType.NONE && a_eSender < ERewardType.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this ERewardKinds a_eSender) {
		return a_eSender > ERewardKinds.NONE && a_eSender < ERewardKinds.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this ERewardQuality a_eSender) {
		return a_eSender > ERewardQuality.NONE && a_eSender < ERewardQuality.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this ERewardAcquirePopupType a_eSender) {
		return a_eSender > ERewardAcquirePopupType.NONE && a_eSender < ERewardAcquirePopupType.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this ETutorialType a_eSender) {
		return a_eSender > ETutorialType.NONE && a_eSender < ETutorialType.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this ETutorialKinds a_eSender) {
		return a_eSender > ETutorialKinds.NONE && a_eSender < ETutorialKinds.MAX_VAL;
	}
	
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
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
