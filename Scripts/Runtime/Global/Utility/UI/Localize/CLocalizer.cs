using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//! 지역화
public class CLocalizer : CComponent {
	#region 변수
	private Font m_oPrevFont = null;
	[SerializeField] private string m_oKey = string.Empty;
	#endregion			// 변수

	#region 컴포넌트
	private Text m_oText = null;
	private InputField m_oInputField = null;
	#endregion			// 컴포넌트

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		m_oText = this.GetComponentInChildren<Text>();
		m_oInputField = this.GetComponentInChildren<InputField>();

		// 텍스트가 존재 할 경우
		if(m_oText != null && m_oInputField == null) {
			m_oPrevFont = m_oText.font;
		}
	}

	//! 초기화
	public override void Start() {
		base.Start();
		this.ResetLocalize();
	}

	//! 지역화를 리셋한다
	public void ResetLocalize() {
#if MESSAGE_PACK_ENABLE
		// 태국어 일 경우
		if(CAppInfoStorage.Instance.AppInfo.Language.ExIsEquals(SystemLanguage.Thai.ToString())) {
			m_oText.font = CResManager.Instance.GetFont(KDefine.G_FONT_PATH_THAI);
		} else {
			m_oText.font = m_oPrevFont;
		}
#endif			// #if MESSAGE_PACK_ENABLE

		this.SetString(m_oKey);
	}

	//! 문자열을 변경한다
	public void SetString(string a_oKey) {
		if(m_oText != null && m_oInputField == null) {
			m_oText.text = CStringTable.Instance.GetString(a_oKey);
		}
	}
	#endregion			// 함수
}
