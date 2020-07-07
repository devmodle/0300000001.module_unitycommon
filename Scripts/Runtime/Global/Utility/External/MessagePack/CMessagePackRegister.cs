using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if MESSAGE_PACK_ENABLE
using MessagePack;
using MessagePack.Resolvers;

//! 메세지 팩 등록자
public static class CMessagePackRegister {
	#region 클래스 변수
	private static bool m_bIsInit = false;
	#endregion			// 클래스 변수

	#region 클래스 함수
	//! 메세지 팩을 등록한다
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void RegisterMessagePack() {
		if(!CMessagePackRegister.m_bIsInit) {
			CMessagePackRegister.m_bIsInit = true;

			StaticCompositeResolver.Instance.Register(MessagePack.Resolvers.GeneratedResolver.Instance,
				MessagePack.Resolvers.StandardResolver.Instance);

			var oOptions = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);
			MessagePackSerializer.DefaultOptions = oOptions;
		}
	}
	#endregion			// 클래스 함수

	#region 조건부 클래스 함수
#if UNITY_EDITOR
	//! 초기화
	[UnityEditor.InitializeOnLoadMethod]
	public static void EditorInitialize() {
		CMessagePackRegister.RegisterMessagePack();
	}
#endif			// #if UNITY_EDITOR
	#endregion			// 조건부 클래스 함수
}
#endif			// #if MESSAGE_PACK_ENABLE
