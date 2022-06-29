using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 객체 */
	public partial class CEObj : CEComponent {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			OBJ_SPRITE,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public new partial struct STParams {
			public CEComponent.STParams m_stBaseParams;
			public STObjInfo m_stObjInfo;
		}

		#region 변수
		private STParams m_stParams;
		private Dictionary<EKey, SpriteRenderer> m_oSpriteDict = new Dictionary<EKey, SpriteRenderer>();
		#endregion			// 변수

		#region 프로퍼티
		public Vector3Int Idx { get; set; }
		public STObjInfo ObjInfo => m_stParams.m_stObjInfo;
		#endregion			// 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 스프라이트를 설정한다
			CFunc.SetupSprites(new List<(EKey, string, GameObject)>() {
				(EKey.OBJ_SPRITE, $"{EKey.OBJ_SPRITE}", this.gameObject)
			}, m_oSpriteDict, false);
		}

		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			base.Init(a_stParams.m_stBaseParams);
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
