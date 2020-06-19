using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

#if SECURITY_ENABLE
#if MESSAGE_PACK_ENABLE
using MessagePack;
#endif			// #if MESSAGE_PACK_ENABLE

#if !MESSAGE_PACK_ENABLE
[System.Obsolete(KDefine.U_MESSAGE_NEED_MESSAGE_PACK, true)]
#endif			// #if !MESSAGE_PACK_ENABLE

//! 보안 변수
public class CSecurityVariable<T> where T : System.IEquatable<T> {
	#region 변수
	private T m_oValue = default(T);

	private byte m_nKeyA = 0;
	private byte m_nKeyB = 0;

	private byte[] m_oBytes = null;
	private byte[] m_oSecurityBytes = null;
	#endregion			// 변수

	#region 프로퍼티
	public T Value {
		get {
			return this.GetValue();
		} set {
			this.SetValue(value);
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	//! 생성자
	public CSecurityVariable(T a_oValue = default(T)) {
		this.SetValue(a_oValue, true);
	}

	//! 메모리 치팅 여부를 검사한다
	private bool IsMemoryCheating() {
		// 길이를 검사한다
		if(m_oBytes.Length != m_oSecurityBytes.Length) {
			return true;
		}

		// 바이트를 검사한다
		for(int i = 0; i < m_oBytes.Length; ++i) {
			if(m_oBytes[i] != (byte)(m_oSecurityBytes[i] ^ m_nKeyB ^ m_nKeyA)) {
				return true;
			}
		}

#if MESSAGE_PACK_ENABLE
		var tValue = MessagePackSerializer.Deserialize<T>(m_oBytes);
		return !m_oValue.Equals(tValue);
#else
		return false;
#endif			// #if MESSAGE_PACK_ENABLE
	}

	//! 값을 반환한다
	private T GetValue(bool a_bIsIgnoreSecurity = false) {
		if(!a_bIsIgnoreSecurity && this.IsMemoryCheating()) {
			throw new System.Exception(string.Empty);
		}

		return m_oValue;
	}

	//! 값을 변경한다
	private void SetValue(T a_oValue, bool a_bIsIgnoreSecurity = false) {
		if(!a_bIsIgnoreSecurity && this.IsMemoryCheating()) {
			throw new System.Exception(string.Empty);
		}

		m_oValue = a_oValue;

#if MESSAGE_PACK_ENABLE
		m_oBytes = MessagePackSerializer.Serialize<T>(a_oValue);

		m_nKeyA = (byte)Random.Range(byte.MinValue, byte.MaxValue);
		m_nKeyB = (byte)Random.Range(byte.MinValue, byte.MaxValue);

		// 보안 바이트를 생성한다 {
		m_oSecurityBytes = new byte[m_oBytes.Length];

		for(int i = 0; i < m_oBytes.Length; ++i) {
			m_oSecurityBytes[i] = (byte)(m_oBytes[i] ^ m_nKeyA ^ m_nKeyB);
		}
		// 보안 바이트를 생성한다 }
#endif			// #if MESSAGE_PACK_ENABLE
	}
	#endregion			// 함수
}
#endif			// #if SECURITY_ENABLE
