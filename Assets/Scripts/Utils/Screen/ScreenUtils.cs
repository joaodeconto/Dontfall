using UnityEngine;
using System.Collections;

public class ScreenUtils {

	private ScreenUtils(){}
	//Eles são floats para que não seja necessário convertem de int para float em cada chamada
	private static float screenWidth;
	private static float screenHeight;
	public static float RealWidth { get; private set; }
	public static float RealHeight { get; private set; }
	private static bool wasInitialized = false;
	
	static public void Initialize(int gameTabWidth, int gameTabHeight){
		RealWidth  = gameTabWidth;
		RealHeight = gameTabHeight;
		
		screenWidth  = (float)Screen.width;
		screenHeight = (float)Screen.height;
		
		wasInitialized = true;
	}
	
	static public float ScaleWidth(float width) {
		if (!wasInitialized) {
			Debug.LogError("ScreenUtils wasn't initialized");
			Debug.Break();
			return 0;
		}
		
		screenWidth  = (float)Screen.width;
		return (width*screenWidth)/RealWidth;
	}
	
	static public float ScaleHeight(float height) {
		if (!wasInitialized) {
			Debug.LogError("ScreenUtils wasn't initialized");
			Debug.Break();
			return 0;
		}
		
		screenHeight = (float)Screen.height;
		return (height*screenHeight)/RealHeight;
	}
	
	static public int ScaleHeightInt(int number) {
		int scaledNumber = (int)ScaleHeight(number);		
		return (int)Mathf.Ceil(scaledNumber);
	}
	
	static public Rect ScaledRectInSenseHeight(Rect rect) {
		return ScaledRectInSenseHeight(rect.x, rect.y, rect.width, rect.height);
	}
	
	static public Rect ScaledRectInSenseHeight(float x, float y, Texture2D texture) {
		return ScaledRectInSenseHeight(x, y, texture.width, texture.height);
	}
	
	static public Rect ScaledRectInSenseHeight(float x, float y, float width, float height){
		return new Rect(ScaleHeight(x),ScaleHeight(y),ScaleHeight(width),ScaleHeight(height));
	}
	
	static public float DescaledHeight(float height){
		screenHeight = (float)Screen.height;
		return height*(RealHeight/screenHeight);
	}

	static public bool ScreenSizeChange () {
		if (screenWidth != Screen.width ||
			screenHeight != Screen.height) {
			return true;
		}
		return false;
	}
}
