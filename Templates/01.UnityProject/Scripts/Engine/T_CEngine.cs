#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 엔진 */
	public partial class CEngine : CComponent {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			IS_RUNNING,
			SEL_GRID_INFO,
			[HideInInspector] MAX_VAL
		}

		/** 콜백 */
		public enum ECallback {
			NONE = -1,
			CLEAR,
			CLEAR_FAIL,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public struct STParams {
			public CLevelInfo m_oLevelInfo;
			public CClearInfo m_oClearInfo;

			public GameObject m_oItemRoot;
			public GameObject m_oSkillRoot;
			public GameObject m_oObjRoot;
			public GameObject m_oFXRoot;

			public Dictionary<ECallback, System.Action<CEngine>> m_oCallbackDict;
		}

		#region 프로퍼티
		public STParams Params { get; private set; }
		public STRecordInfo RecordInfo { get; private set; }

		public List<CEItem> ItemList { get; } = new List<CEItem>();
		public List<CESkill> SkillList { get; } = new List<CESkill>();
		public List<CEObj> ObjList { get; } = new List<CEObj>();
		public List<CEFX> FXList { get; } = new List<CEFX>();

		public bool IsRunning => this.BoolDict.GetValueOrDefault(EKey.IS_RUNNING);
		public STGridInfo SelGridInfo => this.GridInfoDict.GetValueOrDefault(EKey.SEL_GRID_INFO);

		/** =====> 기타 <===== */
		private List<LineRenderer> GridLineList { get; } = new List<LineRenderer>();
		private Dictionary<EKey, bool> BoolDict { get; } = new Dictionary<EKey, bool>();
		private Dictionary<EKey, STGridInfo> GridInfoDict { get; } = new Dictionary<EKey, STGridInfo>();
		
		/** =====> 객체 <===== */
		private Dictionary<EObjType, List<CEObj>>[,] CellObjDictContainers { get; set; } = null;
		#endregion			// 프로퍼티
		
		#region 함수
		/** 구동 여부를 변경한다 */
		public void SetEnableRunning(bool a_bIsRunning) {
			this.BoolDict.ExReplaceVal(EKey.IS_RUNNING, a_bIsRunning);
		}

		/** 터치 이벤트를 처리한다 */
		public void HandleTouchEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData, ETouchEvent a_eTouchEvent) {
			var stTouchPos = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot);

			// 그리드 영역 일 경우
			if(this.GridInfoDict.GetValueOrDefault(EKey.SEL_GRID_INFO).m_stBounds.Contains(stTouchPos)) {
				switch(a_eTouchEvent) {
					case ETouchEvent.BEGIN: this.HandleTouchBeginEvent(a_oSender, a_oEventData); break;
					case ETouchEvent.MOVE: this.HandleTouchMoveEvent(a_oSender, a_oEventData); break;
					case ETouchEvent.END: this.HandleTouchEndEvent(a_oSender, a_oEventData); break;
				}
			}
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
