using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 블럭 */
	public partial class CBlock : CComponent {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			BLOCK_SPRITE,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public struct STParams {
			public STBlockInfo m_stBlockInfo;
			public CEngine m_oEngine;
		}

		#region 변수
		private STParams m_stParams;

		private Dictionary<EKey, SpriteRenderer> m_oSpriteDict = new Dictionary<EKey, SpriteRenderer>() {
			[EKey.BLOCK_SPRITE] = null
		};
		#endregion			// 변수

		#region 프로퍼티
		public Vector3Int Idx { get; set; }
		public STBlockInfo BlockInfo => m_stParams.m_stBlockInfo;
		#endregion			// 프로퍼티

		#region 추가 변수

		#endregion			// 추가 변수

		#region 추가 프로퍼티

		#endregion			// 추가 프로퍼티

		#region 함수
		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			m_stParams = a_stParams;
			m_oSpriteDict[EKey.BLOCK_SPRITE] = this.gameObject.ExFindComponent<SpriteRenderer>(KDefine.E_OBJ_N_BLOCK_SPRITE);

			// 블럭 스프라이트가 존재 할 경우
			if(m_oSpriteDict[EKey.BLOCK_SPRITE] != null) {
				m_oSpriteDict[EKey.BLOCK_SPRITE].sprite = Access.GetBlockSprite(a_stParams.m_stBlockInfo.m_eBlockKinds);
				m_oSpriteDict[EKey.BLOCK_SPRITE].ExSetSortingOrder(Access.GetSortingOrder(a_stParams.m_stBlockInfo.m_eBlockKinds));
			}
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
