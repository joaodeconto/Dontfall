//========================================================================================================================
// Edy Vehicle Physics - (c) Angel Garcia "Edy" - Oviedo, Spain
// Live demo: http://www.edy.es/unity/offroader.html
// 
// Terms & Conditions:
//  - Use for unlimited time, any number of projects, royalty-free.
//  - Keep the copyright notices on top of the source files.
//  - Resale or redistribute as anything except a final product to the end user (asset / library / engine / middleware / etc.) is not allowed.
//  - Put me (Angel Garcia "Edy") in your game's credits as author of the vehicle physics.
//
// Bug reports, improvements to the code, suggestions on further developments, etc are always welcome.
// Unity forum user: Edy
//========================================================================================================================
//
// GUICanvas
//
// Standalone class (not derived from MonoBehavior) for common GUI drawing operations.
//
//========================================================================================================================


class GUICanvas
	{
	private var m_texture : Texture2D;
	private var m_pixelsWd : float;
	private var m_pixelsHt : float;
	private var m_canvasWd : float;
	private var m_canvasHt : float;
	private var m_scaleX : float;
	private var m_scaleY : float;
	
	private var m_moveX : float;
	private var m_moveY : float;
	
	private var m_pixels : Color[];
	private var m_buffer : Color[];
	
	private var m_alpha = -1.0;
	private var m_changed = false;
	
	// Constructor e información
	
	function GUICanvas (PixelsWd, PixelsHt : int, CanvasWd, CanvasHt : float)
		{
		m_texture = new Texture2D(PixelsWd, PixelsHt, TextureFormat.ARGB32, false);
		m_scaleX = (PixelsWd*1.0)/CanvasWd;		// Pasar los ints a float antes de hacer la division
		m_scaleY = (PixelsHt*1.0)/CanvasHt;
		m_pixelsWd = PixelsWd;
		m_pixelsHt = PixelsHt;
		m_canvasWd = CanvasWd;
		m_canvasHt = CanvasHt;
		
		m_moveX = 0.0;
		m_moveY = 0.0;
		
		m_pixels = new Color[PixelsWd * PixelsHt];
		}
		
	function CanvasWidth () : float { return m_canvasWd; }
	function CanvasHeight () : float { return m_canvasHt; }
	function PixelsWidth () : float { return m_pixelsWd; }
	function PixelsHeight () : float { return m_pixelsHt; }
	function ScaleX () : float	{ return m_scaleX; }
	function ScaleY () : float	{ return m_scaleY; }
		
	// Establecer automáticamente la transparencia. Indicar -1 para que se use siempre la del color indicado.
	
	function SetAlpha (alpha : float)
		{
		m_alpha = alpha;
		}
		
	// Dibujar Lineas
	
	function Line (x0, y0, x1, y1 : float, col : Color)
		{
		MoveTo(x0, y0);
		LineTo(x1, y1, col);
		}
		
	function MoveTo (x0, y0 : float)
		{
		m_moveX = x0;
		m_moveY = y0;
		}
		
	function LineTo (xn, yn : float, col : Color)
		{
		if (m_alpha >= 0) col.a = m_alpha;
		
		var x0 = m_moveX;
		var y0 = m_moveY;
		var x1 = xn;
		var y1 = yn;
		
		m_moveX = xn;
		m_moveY = yn;		
		
		// Asegurar que x0 <= x1. Así organizamos mejor el crop.
		
		if (x0 > x1)
			{
			var tmp = x0; x0 = x1; x1 = tmp;
			tmp = y0; y0 = y1; y1 = tmp;
			}
			
		// Cropear por izquierda y derecha
		
		var sl = (y1-y0)/(x1-x0);
		
		if (x0 < 0.0) { y0 = y0 - x0 * sl; x0 = 0.0; }
		if (x1 > m_canvasWd) { y1 = y1 - (x1-m_canvasWd) * sl; x1 = m_canvasWd; }
		
		// Ya podemos descartar las lineas que no cruzarán el cuadro
			
		if (x0 > m_canvasWd || x1 < 0.0 || 
			(y0 < 0.0 && y1 < 0.0) || (y0 > m_canvasHt && y1 > m_canvasHt))
			return;
			
		// Si llega aquí la linea cruza el cuadro necesariamente.
		// Ajustar las coordenadas "Y" que pudieran estar fuera.
		
		if (y0 < 0.0) { x0 = x0 - y0 * sl; y0 = 0.0; }
		if (y0 > m_canvasHt) { x0 = x0 - (y0-m_canvasHt) / sl; y0 = m_canvasHt; }

		if (y1 < 0.0) { x1 = x1 - y1 * sl; y1 = 0.0; }
		if (y1 > m_canvasHt) { x1 = x1 - (y1-m_canvasHt) / sl; y1 = m_canvasHt; }

		// Dibujar la linea

		TexLine(m_texture, x0*m_scaleX, y0*m_scaleY, x1*m_scaleX, y1*m_scaleY, col);
		m_changed = true;
		}
	
	// Lineas con Vector2
		
	function Line (P0, P1 : Vector2, col : Color)
		{
		MoveTo(P0.x, P0.y);
		LineTo(P1.x, P1.y, col);
		}
		
	function MoveTo (P0 : Vector2)
		{
		MoveTo(P0.x, P0.y);
		}
		
	function LineTo (P1 : Vector2, col : Color)
		{
		LineTo(P1.x, P1.y, col);
		}

	// Lineas horizontales / verticales
	
	function LineX (y : float, col : Color)
		{
		Line (0, y, m_canvasWd, y, col);
		}
		
	function LineY (x : float, col : Color)
		{
		Line (x, 0, x, m_canvasHt, col);
		}
		
	// Dibujar círculo. El radio se mide en la escala X.
	
	function Circle (x, y, radius : float, col : Color)
		{
		if (m_alpha >= 0) col.a = m_alpha;
		TexCircle(m_texture, x*m_scaleX, y*m_scaleY, radius*m_scaleX, col);
		m_changed = true;
		}
		
	// Rellenar bloques
	
	function Clear (col : Color)
		{
		var count = m_pixelsWd * m_pixelsHt;
		
		if (m_alpha >= 0) col.a = m_alpha;
		for (var i=0; i<count; i++)
			m_pixels[i] = col;
			
		m_texture.SetPixels(m_pixels);
		m_changed = true;
		}
		
	function Fill (x, y, width, height : float, col : Color)
		{
		var xi : int = x*m_scaleX;
		var yi : int = y*m_scaleY;
		var wdi : int = width*m_scaleX;
		var hti : int = height*m_scaleY;

		var count = wdi * hti;
		
		if (m_alpha >= 0) col.a = m_alpha;
		for (var i=0; i<count; i++)
			m_pixels[i] = col;
			
		m_texture.SetPixels(xi, yi, wdi, hti, m_pixels);
		m_changed = true;
		}
		
	// Dibujar spline
	
	function SpLine (P0, T0, P1, T1 : Vector2, col : Color)
		{
		var Steps = 20;
		
		var s : float;
		var s2 : float;
		var s3 : float;
		var h1 : float;
		var h2 : float;
		var h3 : float;
		var h4 : float;
		var P : Vector2;
		
		MoveTo(P0);
		for (var t=0; t<=Steps; t++)
			{
			s = t;
			s /= Steps;
			s2 = s*s;
			s3 = s2*s;
			
			// Valores de las funciones de Hermite
			
			h1 =  2*s3 - 3*s2 + 1;
			h2 = -2*s3 + 3*s2;
			h3 =    s3 - 2*s2 + s;
			h4 =    s3 - s2;
			
			/* Estas son las ecuaciones para curvas de Bezier - yo no he notado absolutamente ninguna diferencia.
			h1 =   -s3 + 3*s2 - 3*s + 1;
			h2 =  3*s3 - 6*s2 + 3*s;
			h3 = -3*s3 + 3*s2;
			h4 =    s3;
			*/
			
			// Punto interpolado
			
			P = h1*P0 + h2*P1 + h3*T0 + h4*T1;
			LineTo(P, col);
			}
		}
			
	// Especializadas
	// -----------------------------------------------------------

	// Dibujar una rejilla a intervalos dados
	
	function Grid (stepX, stepY : float, col : Color)
		{
		var f : float;
		
		for (f=0.0; f<=m_canvasWd; f+=stepX) LineY(f, col);
		for (f=0.0; f<=m_canvasHt; f+=stepY) LineX(f, col);
		}

	
	// Dibujar una curva de fricción de WheelCollider
	
	function FrictionCurve (Slip0, Value0, Slip1, Value1, Slope : float, col : Color)
		{
		var P0 = new Vector2(Slip0, Value0);
		var P1 = new Vector2(Slip1, Value1);
		var sl0 = Value0/Slip0;
		var sl1 = Value1/Slip1;
		
		SpLine(Vector2.zero, Vector2(Slip0, 0), P0, P0,	col);		
		SpLine(P0, 
				Vector2(Slip1-Slip0, sl0*Slip1 - Value0) * (1 + (Slip1-Slip0) / (Slip1-Slip0+1)) * CarWheelFriction.MCt0,
				P1, 
				Vector2(Slip1-Slip0, 0) * CarWheelFriction.MCt1,			
				col);
		Line(Slip1, Value1, m_canvasWd, Value1 + (m_canvasWd-Slip1)*Slope, col);
		}
		
	// Dibujar una curva progresiva (Bias) entre dos puntos con el coeficiente dado
	
	function BiasCurve (P0, P1 : Vector2, bias : float, col : Color)
		{
		var Steps = 20;
		
		var s : float;
		var c : float;
		
		var dX = P1.x - P0.x;
		var dY = P1.y - P0.y;
		
		MoveTo(P0);
		for (var t=0; t<=Steps; t++)
			{
			s = t;
			s /= Steps;			
			c = Bias(s, bias);
			
			LineTo(P0.x + dX*s, P0.y + dY*c, col);
			}
		}
		
	
	// Gráfica de lineas con los valores dados.
	// - Para una sola gráfica Values debe llevar pares consecutivos X,Y con ValueSize=2
	// - Para varias gráficas indicar valores consecutivos X,Y1,Y2,Y3..Yn con ValueSize=n+1
	//   Se dibujarán las gráficas (X,Y1), (X,Y2), (X,Y3) .. (X,Yn)
	
	private var COLORS = [Color.green, Color.yellow, Color.cyan, Color.magenta, Color.white, Color.blue, Color.red, Color.gray];
	
	function LineGraph (Values : Array, ValueSize : int)
		{
		var X : float;
		var Y : float;
		
		if (ValueSize < 2) return;
		if (Values.Count < 2*ValueSize) return;

		for (var i=1; i<ValueSize; i++)
			{
			MoveTo(Values[0], Values[i]);
			
			for (var v=1; v<Values.Count/ValueSize; v++)
				LineTo(Values[v*ValueSize], Values[v*ValueSize + i], COLORS[(i-1) % 8]);
			}
		}
	
	// Guardar / Restaurar
	// -----------------------------------------------------------

	function Save ()
		{
		m_buffer = m_texture.GetPixels();
		}
		
	function Restore ()
		{
		if (m_buffer)
			{
			m_texture.SetPixels(m_buffer);
			m_changed = true;
			}
		}
	
	// Dibujar en el GUI. Invocar sólo desde función OnGUI
	// -----------------------------------------------------------
	
	function ApplyChanges()
		{
		if (m_changed)
			{
			m_texture.Apply(false);
			m_changed = false;
			}
		}
	
	function GUIDraw (posX, posY : int)
		{
		ApplyChanges();
		GUI.DrawTexture(Rect(posX, posY, m_pixelsWd, m_pixelsHt), m_texture);
		}
		
	function GUIStretchDraw(posX, posY, width, height : int)
		{
		ApplyChanges();
		GUI.DrawTexture(Rect(posX, posY, width, height), m_texture);
		}
	}


// ----------- función de curva progresiva (Bias)

private var m_lastExponent = 0.0;
private var m_lastBias = -1.0;

private function BiasRaw(x : float, fBias : float) : float
	{
	if (x <= 0.0) return 0.0;
	if (x >= 1.0) return 1.0;

	if (fBias != m_lastBias)
		{
		if (fBias <= 0.0) return x >= 1.0? 1.0 : 0.0;
		else if (fBias >= 1.0) return x > 0.0? 1.0 : 0.0;
		else if (fBias == 0.5) return x;

		m_lastExponent = Mathf.Log(fBias) * -1.4427;
		m_lastBias = fBias;
		}

	return Mathf.Pow(x, m_lastExponent);
	}

	
// Bias simétrico usando sólo la curva inferior (fBias < 0.5)
// Admite rango -1, 1 aplicando efecto simétrico desde 0 hacia +1 y -1.

private function Bias(x : float, fBias : float) : float
	{
	var fResult : float;
		
	fResult = fBias <= 0.5? BiasRaw(Mathf.Abs(x), fBias) : 1.0 - BiasRaw(1.0 - Mathf.Abs(x), 1.0 - fBias);
	
	return x<0.0? -fResult : fResult;
	}

	

// ----------- funciones de UnifyWiki	
	
private function TexLine (tex : Texture2D, x0 : int, y0 : int, x1 : int, y1 : int, col : Color) {
    var dy = y1-y0;
    var dx = x1-x0;
    
    if (dy < 0) {dy = -dy; var stepy = -1;}
    else {stepy = 1;}
    if (dx < 0) {dx = -dx; var stepx = -1;}
    else {stepx = 1;}
    dy <<= 1;
    dx <<= 1;
	
    tex.SetPixel(x0, y0, col);
    if (dx > dy) {
        var fraction = dy - (dx >> 1);
        while (x0 != x1) {
            if (fraction >= 0) {
                y0 += stepy;
                fraction -= dx;
            }
            x0 += stepx;
            fraction += dy;
            tex.SetPixel(x0, y0, col);
        }
    }
    else {
        fraction = dx - (dy >> 1);
        while (y0 != y1) {
            if (fraction >= 0) {
                x0 += stepx;
                fraction -= dy;
            }
            y0 += stepy;
            fraction += dx;
            tex.SetPixel(x0, y0, col);
        }
    }
}


private function TexCircle (tex : Texture2D, cx : int, cy : int, r : int, col : Color) {
    var y = r;
    var d = 1/4 - r;
    var end = Mathf.Ceil(r/Mathf.Sqrt(2));
    
    for (x = 0; x <= end; x++) {
        tex.SetPixel(cx+x, cy+y, col);
        tex.SetPixel(cx+x, cy-y, col);
        tex.SetPixel(cx-x, cy+y, col);
        tex.SetPixel(cx-x, cy-y, col);
        tex.SetPixel(cx+y, cy+x, col);
        tex.SetPixel(cx-y, cy+x, col);
        tex.SetPixel(cx+y, cy-x, col);
        tex.SetPixel(cx-y, cy-x, col);
        
        d += 2*x+1;
        if (d > 0) {
            d += 2 - 2*y--;
        }
    }
}