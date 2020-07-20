using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//! 에셋 추가자
public partial class CAssetImporter : AssetPostprocessor {
	#region 함수
	//! 사운드를 추가했을 경우
	public void OnPreprocessAudio() {
		var oAudioImporter = (AudioImporter)this.assetImporter;
		oAudioImporter.ambisonic = false;
		oAudioImporter.forceToMono = true;
		oAudioImporter.preloadAudioData = true;
		oAudioImporter.loadInBackground = true;
		
		// 샘플링 옵션을 설정한다 {
		var stSettings = oAudioImporter.defaultSampleSettings;
		stSettings.loadType = AudioClipLoadType.DecompressOnLoad;
		stSettings.compressionFormat = AudioCompressionFormat.Vorbis;
		stSettings.sampleRateSetting = AudioSampleRateSetting.PreserveSampleRate;

		oAudioImporter.defaultSampleSettings = stSettings;
		// 샘플링 옵션을 설정한다 }
	}

	//! 텍스처를 추가했을 경우
	public void OnPreprocessTexture() {
		var oTextureImporter = (TextureImporter)this.assetImporter;
		oTextureImporter.filterMode = FilterMode.Bilinear;
		oTextureImporter.alphaIsTransparency = true;

		if(oTextureImporter.textureType == TextureImporterType.Sprite) {
			oTextureImporter.wrapMode = TextureWrapMode.Repeat;
			oTextureImporter.sRGBTexture = true;
			oTextureImporter.mipmapEnabled = false;
			oTextureImporter.spritePixelsPerUnit = KBDefine.REF_PIXELS_UNIT;
		}
	}
	#endregion			// 함수
}
