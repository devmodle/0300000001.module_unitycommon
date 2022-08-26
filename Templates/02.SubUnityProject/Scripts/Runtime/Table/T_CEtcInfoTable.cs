#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 기타 정보 테이블 */
public partial class CEtcInfoTable : CSingleton<CEtcInfoTable> {
	#region 함수
	/** 기타 정보를 리셋한다 */
	public virtual void ResetEtcInfos() {
		CCalcInfoTable.Inst.ResetCalcInfos();
		CEpisodeInfoTable.Inst.ResetEpisodeInfos();
		CTutorialInfoTable.Inst.ResetTutorialInfos();
		CFXInfoTable.Inst.ResetFXInfos();
	}

	/** 기타 정보를 리셋한다 */
	public virtual void ResetEtcInfos(string a_oJSONStr) {
		CCalcInfoTable.Inst.ResetCalcInfos(a_oJSONStr);
		CEpisodeInfoTable.Inst.ResetEpisodeInfos(a_oJSONStr);
		CTutorialInfoTable.Inst.ResetTutorialInfos(a_oJSONStr);
		CFXInfoTable.Inst.ResetFXInfos(a_oJSONStr);
	}

	/** 기타 정보를 로드한다 */
	public void LoadEtcInfos() {
		// Do Something
	}

	/** 기타 정보를 저장한다 */
	public void SaveEtcInfos(string a_oJSONStr) {
		CCalcInfoTable.Inst.SaveCalcInfos(a_oJSONStr);
		CEpisodeInfoTable.Inst.SaveEpisodeInfos(a_oJSONStr);
		CTutorialInfoTable.Inst.SaveTutorialInfos(a_oJSONStr);
		CFXInfoTable.Inst.SaveFXInfos(a_oJSONStr);
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
