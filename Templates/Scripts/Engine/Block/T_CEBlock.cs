using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 블럭 */
	public partial class CEBlock : CComponent {
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

		#region 상수
		private static readonly Dictionary<EKey, string> SPRITE_NAME_DICT = new Dictionary<EKey, string>() {
			[EKey.BLOCK_SPRITE] = "BlockSprite"
		};
		#endregion			// 상수

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

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();
			
			for(int i = (int)EKey.BLOCK_SPRITE; i <= (int)EKey.BLOCK_SPRITE; ++i) {
				m_oSpriteDict[(EKey)i] = this.gameObject.ExFindComponent<SpriteRenderer>(CEBlock.SPRITE_NAME_DICT[(EKey)i]);
			}
		}

		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			m_stParams = a_stParams;

			// 블럭 스프라이트가 존재 할 경우
			if(m_oSpriteDict[EKey.BLOCK_SPRITE] != null) {
				m_oSpriteDict[EKey.BLOCK_SPRITE].sprite = Access.GetBlockSprite(a_stParams.m_stBlockInfo.m_eBlockKinds);
				m_oSpriteDict[EKey.BLOCK_SPRITE].ExSetSortingOrder(Access.GetSortingOrderInfo(a_stParams.m_stBlockInfo.m_eBlockKinds));
			}
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
