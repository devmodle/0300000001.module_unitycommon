using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 객체 */
	public partial class CEObj : CComponent {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			OBJ_SPRITE,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public struct STParams {
			public STObjInfo m_stObjInfo;
			public CEngine m_oEngine;
		}

		#region 상수
		private static readonly Dictionary<EKey, string> SPRITE_NAME_DICT = new Dictionary<EKey, string>() {
			[EKey.OBJ_SPRITE] = "ObjSprite"
		};
		#endregion			// 상수

		#region 변수
		private STParams m_stParams;

		private Dictionary<EKey, SpriteRenderer> m_oSpriteDict = new Dictionary<EKey, SpriteRenderer>() {
			[EKey.OBJ_SPRITE] = null
		};
		#endregion			// 변수

		#region 프로퍼티
		public Vector3Int Idx { get; set; }
		public STObjInfo ObjInfo => m_stParams.m_stObjInfo;
		#endregion			// 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();
			
			for(int i = (int)EKey.OBJ_SPRITE; i <= (int)EKey.OBJ_SPRITE; ++i) {
				m_oSpriteDict[(EKey)i] = this.gameObject.ExFindComponent<SpriteRenderer>(CEObj.SPRITE_NAME_DICT[(EKey)i]);
			}
		}

		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			m_stParams = a_stParams;

			// 객체 스프라이트가 존재 할 경우
			if(m_oSpriteDict[EKey.OBJ_SPRITE] != null) {
				m_oSpriteDict[EKey.OBJ_SPRITE].sprite = Access.GetObjSprite(a_stParams.m_stObjInfo.m_eObjKinds);
				m_oSpriteDict[EKey.OBJ_SPRITE].ExSetSortingOrder(Access.GetSortingOrderInfo(a_stParams.m_stObjInfo.m_eObjKinds));
			}
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
