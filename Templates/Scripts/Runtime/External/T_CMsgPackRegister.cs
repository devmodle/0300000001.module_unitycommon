using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
#if MSG_PACK_ENABLE
using MessagePack;
using MessagePack.Resolvers;

#if UNITY_EDITOR
using UnityEditor;
#endif			// #if UNITY_EDITOR

//! 메세지 팩 등록자
public static class CMsgPackRegister {
	#region 클래스 변수
	private static bool m_bIsInit = false;
	#endregion			// 클래스 변수

	#region 클래스 함수
	//! 메세지 팩을 등록한다
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void RegisterMsgPack() {
		// 초기화가 필요 할 경우
		if(!CMsgPackRegister.m_bIsInit) {
			CMsgPackRegister.m_bIsInit = true;

			StaticCompositeResolver.Instance.Register(MessagePack.Resolvers.GeneratedResolver.Instance,
				MessagePack.Resolvers.StandardResolver.Instance);

			var oOpts = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);
			MessagePackSerializer.DefaultOptions = oOpts;
		}
	}
	#endregion			// 클래스 함수

	#region 조건부 클래스 함수
#if UNITY_EDITOR
	//! 초기화
	[InitializeOnLoadMethod]
	public static void EditorInitialize() {
		CMsgPackRegister.RegisterMsgPack();
	}
#endif			// #if UNITY_EDITOR
	#endregion			// 조건부 클래스 함수
}
#endif			// #if MSG_PACK_ENABLE
#endif			// #if NEVER_USE_THIS
