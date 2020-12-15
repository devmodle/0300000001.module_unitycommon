using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 전역 함수
public static partial class Func {
	#region 클래스 함수
	//! 앱 종료 팝업을 출력한다
	public static void ShowAppQuitPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		var oDataList = new Dictionary<string, string>() {
			[KCDefine.U_KEY_ALERT_P_TITLE] = CStringTable.Inst.GetString(KCDefine.ST_KEY_ALERT_P_TITLE),
			[KCDefine.U_KEY_ALERT_P_MSG] = CStringTable.Inst.GetString(KCDefine.ST_KEY_ALERT_P_MSG),
			[KCDefine.U_KEY_ALERT_P_OK_BTN_TEXT] = CStringTable.Inst.GetString(KCDefine.ST_KEY_ALERT_P_OK_BTN_TEXT),
			[KCDefine.U_KEY_ALERT_P_CANCEL_BTN_TEXT] = CStringTable.Inst.GetString(KCDefine.ST_KEY_ALERT_P_CANCEL_BTN_TEXT)
		};

		var oAlertPopup = CAlertPopup.Create<CAlertPopup>(KCDefine.U_OBJ_NAME_ALERT_POPUP,
			CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_PATH_G_ALERT_POPUP),
			CSceneManager.ScreenPopupUIRoot,
			oDataList,
			a_oCallback);

		oAlertPopup.Show(null, null);
	}
	#endregion			// 클래스 함수
}
#endif			// #if NEVER_USE_THIS
