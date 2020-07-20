using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//! 문자열 테이블
public class CStringTable : CSingleton<CStringTable> {
	#region 변수
	private Dictionary<string, string> m_oStringList = new Dictionary<string, string>();
	#endregion			// 변수

	#region 함수
	//! 상태를 리셋한다
	public virtual void Reset() {
		m_oStringList.Clear();
	}

	//! 문자열을 반환한다
	public string GetString(string a_oKey) {
		return m_oStringList.ExGetValue(a_oKey, a_oKey);
	}

	//! 문자열을 추가한다
	public void AddString(string a_oKey, string a_oString) {
		m_oStringList.ExAddValue(a_oKey, a_oString);
	}

	//! 문자열을 제거한다
	public void RemoveString(string a_oKey) {
		m_oStringList.ExRemoveValue(a_oKey);
	}

	//! 문자열을 로드한다
	public void LoadStrings(string a_oCSVString) {
		CBAccess.Assert(a_oCSVString.ExIsValid());
		var oStringInfoList = CSVParser.Parse(a_oCSVString);

		for(int i = 0; i < oStringInfoList.Count; ++i) {
			string oKey = oStringInfoList[i][KUDefine.KEY_STRING_T_ID];
			string oString = oStringInfoList[i][KUDefine.KEY_STRING_T_STRING];

			this.AddString(oKey, oString);
		}
	}

	//! 문자열을 로드한다
	public void LoadStringsFromFile(string a_oFilepath) {
		this.LoadStrings(CBAccess.ReadString(a_oFilepath, System.Text.Encoding.Default));
	}

	//! 문자열을 로드한다
	public void LoadStringsFromRes(string a_oFilepath) {
		var oTextAsset = CResManager.Instance.GetTextAsset(a_oFilepath);
		CBAccess.Assert(oTextAsset.ExIsValid());

		this.LoadStrings(oTextAsset.text);
		CResManager.Instance.RemoveTextAsset(a_oFilepath, true);
	}
	#endregion			// 함수
}
