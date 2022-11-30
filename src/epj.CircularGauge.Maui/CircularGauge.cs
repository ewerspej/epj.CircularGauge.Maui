using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace epj.CircularGauge.Maui;

public class CircularGauge : SKCanvasView
{
    #region Default Values

    private const float DefaultStartAngle = 45.0f;
    private const float DefaultSweepAngle = 270.0f;
    private const float DefaultGaugeWidth = 10.0f;
    private const float DefaultRangeStart = 0.0f;
    private const float DefaultRangeEnd = 100.0f;
    private const float DefaultValue = 50.0f;
    private const float DefaultNeedleLength = 128.0f;
    private const float DefaultNeedleWidth = 18.0f;
    private const float DefaultNeedleOffset = 18.0f;
    private const float DefaultBaseWidth = 24.0f;
    private const float DefaultBaseStrokeWidth = 4.0f;

    #endregion

    #region Private Fields

    private SKImageInfo _info;
    private SKSurface _surface;
    private SKCanvas _canvas;
    private SKPoint _center;
    private SKRect _gaugeRect;
    private SKRect _scaleRect;
    private float _adjustedStartAngle;
    private float _internalPadding = 10.0f;
    private int _size;

    //paint for the major units
    private readonly SKPaint _majorUnitsPaint = new SKPaint
    {
        Color = SKColors.Black
    };

    #endregion

    #region Properties

    public float StartAngle { get; set; } = DefaultStartAngle;
    public float SweepAngle { get; set; } = DefaultSweepAngle;
    public float GaugeWidth { get; set; } = DefaultGaugeWidth;
    public Color GaugeColor { get; set; } = Colors.Red;
    public List<Color> GaugeGradientColors { get; set; } = new List<Color>();
    public float RangeStart { get; set; } = DefaultRangeStart;
    public float RangeEnd { get; set; } = DefaultRangeEnd;
    public float Value { get; set; } = DefaultValue;
    public Color NeedleColor { get; set; } = Colors.Black;
    public float NeedleLength { get; set; } = DefaultNeedleLength;
    public float NeedleWidth { get; set; } = DefaultNeedleWidth;
    public float NeedleOffset { get; set; } = DefaultNeedleOffset;
    public Color BaseColor { get; set; } = Colors.Black;
    public float BaseWidth { get; set; } = DefaultBaseWidth;
    public Color BaseStrokeColor { get; set; } = Colors.DimGray;
    public float BaseStrokeWidth { get; set; } = DefaultBaseStrokeWidth;
    public bool DrawBaseStrokeBeforeFill { get; set; } = false;
    public float ScaleLength { get; set; }
    public float ScaleDistance { get; set; }

    #endregion

    public CircularGauge() { }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        base.OnPaintSurface(e);

        _info = e.Info;
        _surface = e.Surface;
        _canvas = _surface.Canvas;
        _canvas.Clear();
        _size = Math.Min(_info.Size.Width, _info.Size.Height);

        //offsets are used to always center the dial inside the canvas and move the stroke inwards only
        var scaleOffset = _internalPadding;
        var dialOffset = GaugeWidth / 2 + _internalPadding + ScaleLength + ScaleDistance;

        //setup the drawing rectangle
        _scaleRect = new SKRect(scaleOffset, scaleOffset, _size - scaleOffset, _size - scaleOffset);

        //setup the drawing rectangle
        _gaugeRect = new SKRect(dialOffset, dialOffset, _size - dialOffset, _size - dialOffset);

        //setup the center
        _center = new SKPoint(_scaleRect.MidX, _scaleRect.MidY);

        //the coordinate system of SkiaSharp starts with 0 degrees at 3 o'clock (polar coordinates),
        //but we want 0 degrees at 6 o'clock, so we rotate everything by 90 degrees.
        _adjustedStartAngle = StartAngle + 90.0f;
    }
}