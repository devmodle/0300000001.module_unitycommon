#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {
		/** 매개 변수 */
		public new struct STParams {
			public CEObjController.STParams m_stBase;
		}

		#region 프로퍼티
		public new STParams Params { get; private set; }

		public Vector3Int CellIdx { get; private set; } = new Vector3Int(KCDefine.B_IDX_INVALID, KCDefine.B_IDX_INVALID, KCDefine.B_IDX_INVALID);
		public STCellObjInfo CellObjInfo { get; private set; } = STCellObjInfo.INVALID;
		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();
			this.SubAwake();
		}

		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			base.Init(a_stParams.m_stBase);
			this.Params = a_stParams;

			this.SubInit();
		}

		/** 셀 객체 정보를 리셋한다 */
		public virtual void ResetCellObjInfo(STObjInfo a_stObjInfo, STCellObjInfo a_stCellObjInfo) {
			this.GetOwner<CEObj>().ResetObjInfo(a_stObjInfo);
			this.SetCellObjInfo(a_stCellObjInfo);

			this.SubResetObjInfo(a_stObjInfo, a_stCellObjInfo);
		}

		/** 제거되었을 경우 */
		public override void OnDestroy() {
			base.OnDestroy();

			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsRunningApp) {
					this.SubOnDestroy();
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CECellObjController.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fTimeDelta) {
			base.OnUpdate(a_fTimeDelta);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsRunningApp) {
				this.SubOnUpdate(a_fTimeDelta);
			}
		}
		#endregion // 함수
	}

	/** 셀 객체 제어자 - 접근 */
	public partial class CECellObjController : CEObjController {
		#region 함수
		/** 셀 인덱스를 변경한다 */
		public void SetCellIdx(Vector3Int a_stIdx) {
			this.CellIdx = a_stIdx;
		}

		/** 셀 객체 정보를 변경한다 */
		public void SetCellObjInfo(STCellObjInfo a_stCellObjInfo) {
			a_stCellObjInfo.ObjKinds = this.GetOwner<CEObj>().Params.m_stObjInfo.m_eObjKinds;
			this.CellObjInfo = a_stCellObjInfo;
		}
		#endregion // 함수
	}

	/** 셀 객체 제어자 - 팩토리 */
	public partial class CECellObjController : CEObjController {
		#region 클래스 함수
		/** 매개 변수를 생성한다 */
		public new static STParams MakeParams(CEngine a_oEngine) {
			return new STParams() {
				m_stBase = CEObjController.MakeParams(a_oEngine)
			};
		}
		#endregion // 클래스 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
