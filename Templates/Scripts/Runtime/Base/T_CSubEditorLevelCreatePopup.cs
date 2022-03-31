using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if UNITY_STANDALONE && EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
/** 서브 에디터 레벨 생성 팝업 */
public partial class CSubEditorLevelCreatePopup : CEditorLevelCreatePopup {
	/** 매개 변수 */
	public new struct STParams {
		public CEditorLevelCreatePopup.STParams m_stBaseParams;
	}

	#region 변수
	private STParams m_stParams;
	#endregion			// 변수

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init(a_stParams.m_stBaseParams);
		m_stParams = a_stParams;
	}

	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}

	/** UI 상태를 갱신한다 */
	protected new void UpdateUIsState() {
		base.UpdateUIsState();
	}

	/** 에디터 레벨 생성 정보를 생성한다 */
	protected override CEditorLevelCreateInfo CreateEditorLevelCreateInfo() {
		var oCreateInfo = base.CreateEditorLevelCreateInfo();

		return new CSubEditorLevelCreateInfo() {
			m_nNumLevels = oCreateInfo.m_nNumLevels, m_stMinNumCells = oCreateInfo.m_stMinNumCells, m_stMaxNumCells = oCreateInfo.m_stMaxNumCells
		};
	}
	#endregion			// 함수
}
#endif			// #if UNITY_STANDALONE && EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
#endif			// #if SCRIPT_TEMPLATE_ONLY
