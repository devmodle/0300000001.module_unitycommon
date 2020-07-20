using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//! 값 테이블
public class CValueTable : CSingleton<CValueTable> {
	#region 변수
	private Dictionary<string, bool> m_oBoolList = new Dictionary<string, bool>();
	private Dictionary<string, int> m_oIntList = new Dictionary<string, int>();
	private Dictionary<string, float> m_oFloatList = new Dictionary<string, float>();
	private Dictionary<string, string> m_oStringList = new Dictionary<string, string>();
	#endregion			// 변수

	#region 함수
	//! 상태를 리셋한다
	public virtual void Reset() {
		m_oBoolList.Clear();
		m_oIntList.Clear();
		m_oFloatList.Clear();
		m_oStringList.Clear();
	}

	//! 논리를 반환한다
	public bool GetBool(string a_oKey, bool a_bIsDefValue = false) {
		return m_oBoolList.ExGetValue(a_oKey, a_bIsDefValue);
	}

	//! 정수를 반환한다
	public int GetInt(string a_oKey, int a_nDefValue = 0) {
		return m_oIntList.ExGetValue(a_oKey, a_nDefValue);
	}

	//! 실수를 반환한다
	public float GetFloat(string a_oKey, float a_fDefValue = 0.0f) {
		return m_oFloatList.ExGetValue(a_oKey, a_fDefValue);
	}

	//! 문자열을 반환한다
	public string GetString(string a_oKey, string a_oDefValue = KBDefine.EMPTY_STRING) {
		return m_oStringList.ExGetValue(a_oKey, a_oDefValue);
	}

	//! 논리를 추가한다
	public void AddBool(string a_oKey, bool a_bIsValue) {
		m_oBoolList.ExAddValue(a_oKey, a_bIsValue);
	}

	//! 정수를 추가한다
	public void AddInt(string a_oKey, int a_nValue) {
		m_oIntList.ExAddValue(a_oKey, a_nValue);
	}

	//! 실수를 추가한다
	public void AddFloat(string a_oKey, float a_fValue) {
		m_oFloatList.ExAddValue(a_oKey, a_fValue);
	}

	//! 문자열을 추가한다
	public void AddString(string a_oKey, string a_oValue) {
		m_oStringList.ExAddValue(a_oKey, a_oValue);
	}

	//! 논리를 제거한다
	public void RemoveBool(string a_oKey) {
		m_oBoolList.ExRemoveValue(a_oKey);
	}

	//! 정수를 제거한다
	public void RemoveInt(string a_oKey) {
		m_oIntList.ExRemoveValue(a_oKey);
	}

	//! 실수를 제거한다
	public void RemoveFloat(string a_oKey) {
		m_oFloatList.ExRemoveValue(a_oKey);
	}

	//! 문자열을 제거한다
	public void RemoveString(string a_oKey) {
		m_oStringList.ExRemoveValue(a_oKey);
	}

	//! 값을 로드한다
	public void LoadValues(string a_oCSVString) {
		CBAccess.Assert(a_oCSVString.ExIsValid());
		var oStringInfoList = CSVParser.Parse(a_oCSVString);

		for(int i = 0; i < oStringInfoList.Count; ++i) {
			string oKey = oStringInfoList[i][KUDefine.KEY_VALUE_T_ID];
			string oValue = oStringInfoList[i][KUDefine.KEY_VALUE_T_VALUE];

			var eValueType = (EValueType)int.Parse(oStringInfoList[i][KUDefine.KEY_VALUE_T_VALUE_TYPE]);

			switch(eValueType) {
			case EValueType.BOOL: this.AddBool(oKey, bool.Parse(oValue)); break;
			case EValueType.INT: this.AddInt(oKey, int.Parse(oValue)); break;
			case EValueType.FLOAT: this.AddFloat(oKey, float.Parse(oValue)); break;
			case EValueType.STRING: this.AddString(oKey, oValue); break;
			}
		}
	}

	//! 값을 로드한다
	public void LoadValuesFromFile(string a_oFilepath) {
		this.LoadValues(CBAccess.ReadString(a_oFilepath, System.Text.Encoding.Default));
	}

	//! 값을 로드한다
	public void LoadValuesFromRes(string a_oFilepath) {
		var oTextAsset = CResManager.Instance.GetTextAsset(a_oFilepath);
		CBAccess.Assert(oTextAsset.ExIsValid());

		this.LoadValues(oTextAsset.text);
		CResManager.Instance.RemoveTextAsset(a_oFilepath, true);
	}
	#endregion			// 함수
}
