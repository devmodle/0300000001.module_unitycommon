using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//! 에셋 추출자
public static partial class CAssetExporter {
	#region 클래스 함수
	//! 텍스처 -> PNG 이미지로 추출한다
	[MenuItem("Utility/Export/Texture to PNGImage")]
	public static void ExportTextureToPNGImg() {
		var oTextureList = Selection.objects.ExIsValid() ? Selection.objects.ExToTypes<Texture2D>() : null;

		if(!oTextureList.ExIsValid()) {
			CAssetExporter.ShowExportFailPopup(KCEditorDefine.ALERT_P_EXPORT_TEXTURE_FAIL_MSG);
		} else {
			for(int i = 0; i < oTextureList.Count; ++i) {
				string oFilepath = string.Format(KCEditorDefine.IMG_PATH_FORMAT_TEXTURE_TO_IMG, oTextureList[i].name);
				CAssetExporter.SaveTexture(oFilepath, oTextureList[i]);
			}

			CAssetExporter.ShowExportSuccessPopup(KCEditorDefine.ALERT_P_EXPORT_IMG_SUCCESS_MSG);
		}
	}

	//! 기본 텍스처 -> PNG 이미지로 추출한다
	[MenuItem("Utility/Export/DefTexture to PNGImage")]
	public static void ExportDefTextureToPNGImg() {
		string oFilepath = string.Format(KCEditorDefine.IMG_PATH_FORMAT_TEXTURE_TO_IMG, Texture2D.whiteTexture.name);
		CAssetExporter.SaveTexture(oFilepath, Texture2D.whiteTexture);

		CAssetExporter.ShowExportSuccessPopup(KCEditorDefine.ALERT_P_EXPORT_IMG_SUCCESS_MSG);
	}

	//! 스프라이트 -> PNG 이미지로 추출한다
	[MenuItem("Utility/Export/Sprite to PNGImage")]
	public static void ExportSpriteToPNGImg() {
		var oSpriteList = Selection.objects.ExIsValid() ? Selection.objects.ExToTypes<Sprite>() : null;

		if(!oSpriteList.ExIsValid()) {
			CAssetExporter.ShowExportFailPopup(KCEditorDefine.ALERT_P_EXPORT_SPRITE_FAIL_MSG);
		} else {
			for(int i = 0; i < oSpriteList.Count; ++i) {
				var oTexture = new Texture2D((int)oSpriteList[i].textureRect.width,
					(int)oSpriteList[i].textureRect.height, TextureFormat.RGBA32, 1, true);

				oTexture.SetPixels(oSpriteList[i].texture.GetPixels((int)oSpriteList[i].textureRect.x,
					(int)oSpriteList[i].textureRect.y, (int)oSpriteList[i].textureRect.width, (int)oSpriteList[i].textureRect.height, 0));

				oTexture.Apply();

				var oBytes = oTexture.EncodeToPNG();
				CFunc.WriteBytes(string.Format(KCEditorDefine.IMG_PATH_FORMAT_TEXTURE_TO_IMG, oSpriteList[i].name), oBytes);
			}

			CAssetExporter.ShowExportSuccessPopup(KCEditorDefine.ALERT_P_EXPORT_IMG_SUCCESS_MSG);
		}
	}

	//! 텍스처를 저장한다
	private static void SaveTexture(string a_oFilepath, Texture2D a_oTexture) {
		var oTexture = new Texture2D(a_oTexture.width,
			a_oTexture.height, TextureFormat.RGBA32, 1, true);

		oTexture.SetPixels(a_oTexture.GetPixels(0));
		oTexture.Apply();

		var oBytes = oTexture.EncodeToPNG();
		CFunc.WriteBytes(a_oFilepath, oBytes);		
	}

	//! 추출 성공 팝업을 출력한다
	private static void ShowExportSuccessPopup(string a_oMsg) {
		CEditorFunc.ShowAlertPopup(KCEditorDefine.ALERT_P_TITLE,
			a_oMsg, KCEditorDefine.ALERT_P_OK_BTN_TEXT, string.Empty);
	}

	//! 추출 에러 팝업을 출력한다
	private static void ShowExportFailPopup(string a_oMsg) {
		CEditorFunc.ShowAlertPopup(KCEditorDefine.ALERT_P_TITLE,
			a_oMsg, KCEditorDefine.ALERT_P_OK_BTN_TEXT, string.Empty);
	}
	#endregion			// 클래스 함수
}
