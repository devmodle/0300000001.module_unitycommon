#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 */
	public partial class CEngine : CComponent {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			SEL_GRID_INFO,
			SEL_PLAYER_OBJ,
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
		public partial struct STParams {
			public CLevelInfo m_oLevelInfo;
			public CClearInfo m_oClearInfo;

			public GameObject m_oItemRoot;
			public GameObject m_oSkillRoot;
			public GameObject m_oObjRoot;
			public GameObject m_oFXRoot;

			public Dictionary<ECallback, System.Action<CEngine>> m_oCallbackDict;
		}

		#region 변수
		private List<CEItem> m_oItemList = new List<CEItem>();
		private List<CESkill> m_oSkillList = new List<CESkill>();
		private List<CEFX> m_oFXList = new List<CEFX>();
		private List<CEObj> m_oEnemyObjList = new List<CEObj>();
		private List<LineRenderer> m_oGridLineList = new List<LineRenderer>();

		private Dictionary<EKey, STGridInfo> m_oGridInfoDict = new Dictionary<EKey, STGridInfo>();
		private Dictionary<EKey, CEObj> m_oPlayerObjDict = new Dictionary<EKey, CEObj>();
		
		/** =====> 객체 <===== */
		private Dictionary<EObjType, List<CEObj>>[,] m_oObjDictContainers = null;
		#endregion			// 변수

		#region 프로퍼티
		public STParams Params { get; private set; }
		public long IntRecord { get; private set; } = 0;
		public double RealRecord { get; private set; } = 0.0;

		public STGridInfo SelGridInfo => m_oGridInfoDict.GetValueOrDefault(EKey.SEL_GRID_INFO);
		public CEObj SelPlayerObj => m_oPlayerObjDict.GetValueOrDefault(EKey.SEL_PLAYER_OBJ);
		#endregion			// 프로퍼티
		
		#region 함수
		/** 터치 이벤트를 처리한다 */
		public void HandleTouchEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData, ETouchEvent a_eTouchEvent) {
			var stTouchPos = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot);

			// 그리드 영역 일 경우
			if(m_oGridInfoDict.GetValueOrDefault(EKey.SEL_GRID_INFO).m_stBounds.Contains(stTouchPos)) {
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
