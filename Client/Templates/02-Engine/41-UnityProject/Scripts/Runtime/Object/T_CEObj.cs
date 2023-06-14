#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 엔진 객체 */
	public partial class CEObj : CEObjComponent {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public new struct STParams {
			public CEObjComponent.STParams m_stBaseParams;
			public STObjInfo m_stObjInfo;
			public CObjTargetInfo m_oObjTargetInfo;
		}

		#region 변수
		[Header("=====> Game Objects <=====")]
		[SerializeField] private List<GameObject> m_oObjList = new List<GameObject>();
		#endregion // 변수

		#region 프로퍼티
		public new STParams Params { get; private set; }
		public STObjInfo OriginObjInfo { get; private set; } = STObjInfo.INVALID;
		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();
			this.SubAwake();
		}

		/** 객체 정보를 리셋한다 */
		public virtual void ResetObjInfo(STObjInfo a_stObjInfo) {
			// 리셋 가능 할 경우
			if(a_stObjInfo.m_eObjKinds != this.Params.m_stObjInfo.m_eObjKinds) {
				var stParams = this.Params;
				stParams.m_stObjInfo = a_stObjInfo;

				this.Init(stParams);
			}

			this.SubResetObjInfo(a_stObjInfo);
		}

		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			base.Init(a_stParams.m_stBaseParams);
			this.Params = a_stParams;

			// 어빌리티 값을 설정한다
			this.SetupAbilityVals();

			// 객체를 설정한다
			for(int i = 0; i < m_oObjList.Count; ++i) {
				string oStr = CStrTable.Inst.GetEnumStr(typeof(EObjKinds), (int)this.Params.m_stObjInfo.m_eObjKinds);
				m_oObjList[i].SetActive(m_oObjList[i].name.Equals(oStr));
			}

			// 스프라이트를 설정한다 {
			this.TargetSprite?.ExSetColor<SpriteRenderer>(Access.GetColor(a_stParams.m_stObjInfo.m_eObjKinds));
			this.TargetSprite?.ExSetSprite<SpriteRenderer>(Access.GetSprite(a_stParams.m_stObjInfo.m_eObjKinds));
			this.TargetSprite?.ExSetSortingOrder(Access.GetSortingOrderInfo(a_stParams.m_stObjInfo.m_eObjKinds));

			this.TargetSprite?.gameObject.SetActive(a_stParams.m_stObjInfo.m_eObjKinds != EObjKinds.BG_OBJ_PLACEHOLDER_01);
			// 스프라이트를 설정한다 }

			this.SubInit();
		}

		/** 어빌리티 값을 설정한다 */
		protected override void DoSetupAbilityVals(bool a_bIsReset = true) {
			base.DoSetupAbilityVals(a_bIsReset);

			// 객체 정보가 존재 할 경우
			if(this.Params.m_stObjInfo.m_eObjKinds.ExIsValid()) {
				global::Func.SetupAbilityVals(this.Params.m_stObjInfo, this.Params.m_oObjTargetInfo, this.AbilityValDictWrapper.m_oDict02);
			}
		}
		#endregion // 함수
	}

	/** 엔진 객체 - 접근 */
	public partial class CEObj : CEObjComponent {
		#region 함수
		/** 원본 객체 정보를 설정한다 */
		public void SetOriginObjInfo(STObjInfo a_stObjInfo) {
			this.OriginObjInfo = a_stObjInfo;
		}
		#endregion // 함수
	}

	/** 엔진 객체 - 팩토리 */
	public partial class CEObj : CEObjComponent {
		#region 클래스 함수
		/** 매개 변수를 생성한다 */
		public static STParams MakeParams(CEngine a_oEngine, STObjInfo a_stObjInfo, CObjTargetInfo a_oObjTargetInfo, CEController a_oController = null, string a_oGameObjsPoolKey = KCDefine.B_TEXT_EMPTY) {
			return new STParams() {
				m_stBaseParams = CEObjComponent.MakeParams(a_oEngine, a_oController, a_oGameObjsPoolKey), m_stObjInfo = a_stObjInfo, m_oObjTargetInfo = a_oObjTargetInfo
			};
		}
		#endregion // 클래스 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
