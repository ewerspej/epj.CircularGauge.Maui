using Microsoft.Maui.ApplicationModel;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace epj.CircularGauge.Maui;

public class CircularGauge : SKCanvasView
{
    #region Default Values

    private const float DefaultValue = 50.0f;
    private const float DefaultStartAngle = 45.0f;
    private const float DefaultSweepAngle = 270.0f;
    private const float DefaultGaugeWidth = 10.0f;
    private const float DefaultRangeStart = 0.0f;
    private const float DefaultRangeEnd = 100.0f;
    private const float DefaultNeedleLength = 128.0f;
    private const float DefaultNeedleWidth = 18.0f;
    private const float DefaultNeedleOffset = 18.0f;
    private const float DefaultBaseWidth = 24.0f;
    private const float DefaultBaseStrokeWidth = 4.0f;
    private const float DefaultScaleLength = 10.0f;
    private const float DefaultScaleDistance = 4.0f;
    private const float DefaultScaleThickness = 4.0f;
    private const int DefaultScaleUnits = 5;

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

    #endregion

    #region Properties

    public float Value
    {
        get => (float)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public float StartAngle
    {
        get => (float)GetValue(StartAngleProperty);
        set => SetValue(StartAngleProperty, value);
    }

    public float SweepAngle
    {
        get => (float)GetValue(SweepAngleProperty);
        set => SetValue(SweepAngleProperty, value);
    }

    public float GaugeWidth
    {
        get => (float)GetValue(GaugeWidthProperty);
        set => SetValue(GaugeWidthProperty, value);
    }

    public float RangeStart
    {
        get => (float)GetValue(RangeStartProperty);
        set => SetValue(RangeStartProperty, value);
    }

    public float RangeEnd
    {
        get => (float)GetValue(RangeEndProperty);
        set => SetValue(RangeEndProperty, value);
    }

    public float NeedleLength
    {
        get => (float)GetValue(NeedleLengthProperty);
        set => SetValue(NeedleLengthProperty, value);
    }

    public float NeedleWidth
    {
        get => (float)GetValue(NeedleWidthProperty);
        set => SetValue(NeedleWidthProperty, value);
    }

    public float NeedleOffset
    {
        get => (float)GetValue(NeedleOffsetProperty);
        set => SetValue(NeedleOffsetProperty, value);
    }

    public float BaseWidth
    {
        get => (float)GetValue(BaseWidthProperty);
        set => SetValue(BaseWidthProperty, value);
    }

    public float BaseStrokeWidth
    {
        get => (float)GetValue(BaseStrokeWidthProperty);
        set => SetValue(BaseStrokeWidthProperty, value);
    }

    public bool ShowScale
    {
        get => (bool)GetValue(ShowScaleProperty);
        set => SetValue(ShowScaleProperty, value);
    }

    public float ScaleDistance
    {
        get => (float)GetValue(ScaleDistanceProperty);
        set => SetValue(ScaleDistanceProperty, value);
    }

    public float ScaleLength
    {
        get => (float)GetValue(ScaleLengthProperty);
        set => SetValue(ScaleLengthProperty, value);
    }

    public float ScaleThickness
    {
        get => (float)GetValue(ScaleThicknessProperty);
        set => SetValue(ScaleThicknessProperty, value);
    }

    public int ScaleUnits
    {
        get => (int)GetValue(ScaleUnitsProperty);
        set => SetValue(ScaleUnitsProperty, value);
    }

    public bool DrawBaseStrokeBeforeFill
    {
        get => (bool)GetValue(DrawBaseStrokeBeforeFillProperty);
        set => SetValue(DrawBaseStrokeBeforeFillProperty, value);
    }

    public Color GaugeColor
    {
        get => (Color)GetValue(GaugeColorProperty);
        set => SetValue(GaugeColorProperty, value);
    }

    public Color NeedleColor
    {
        get => (Color)GetValue(NeedleColorProperty);
        set => SetValue(NeedleColorProperty, value);
    }

    public Color BaseColor
    {
        get => (Color)GetValue(BaseColorProperty);
        set => SetValue(BaseColorProperty, value);
    }

    public Color BaseStrokeColor
    {
        get => (Color)GetValue(BaseStrokeColorProperty);
        set => SetValue(BaseStrokeColorProperty, value);
    }

    public Color ScaleColor
    {
        get => (Color)GetValue(ScaleColorProperty);
        set => SetValue(ScaleColorProperty, value);
    }

    public List<Color> GaugeGradientColors { get; set; } = new List<Color>();

    #endregion

    #region BindableProperties

    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(float), typeof(CircularGauge), DefaultValue, propertyChanged: OnBindablePropertyChanged);
    public static readonly BindableProperty StartAngleProperty = BindableProperty.Create(nameof(StartAngle), typeof(float), typeof(CircularGauge), DefaultStartAngle, propertyChanged: OnBindablePropertyChanged);
    public static readonly BindableProperty SweepAngleProperty = BindableProperty.Create(nameof(SweepAngle), typeof(float), typeof(CircularGauge), DefaultSweepAngle, propertyChanged: OnBindablePropertyChanged);
    public static readonly BindableProperty GaugeWidthProperty = BindableProperty.Create(nameof(GaugeWidth), typeof(float), typeof(CircularGauge), DefaultGaugeWidth, propertyChanged: OnBindablePropertyChanged);
    public static readonly BindableProperty RangeStartProperty = BindableProperty.Create(nameof(RangeStart), typeof(float), typeof(CircularGauge), DefaultRangeStart, propertyChanged: OnBindablePropertyChanged);
    public static readonly BindableProperty RangeEndProperty = BindableProperty.Create(nameof(RangeEnd), typeof(float), typeof(CircularGauge), DefaultRangeEnd, propertyChanged: OnBindablePropertyChanged);
    public static readonly BindableProperty NeedleLengthProperty = BindableProperty.Create(nameof(NeedleLength), typeof(float), typeof(CircularGauge), DefaultNeedleLength, propertyChanged: OnBindablePropertyChanged);
    public static readonly BindableProperty NeedleWidthProperty = BindableProperty.Create(nameof(NeedleWidth), typeof(float), typeof(CircularGauge), DefaultNeedleWidth, propertyChanged: OnBindablePropertyChanged);
    public static readonly BindableProperty NeedleOffsetProperty = BindableProperty.Create(nameof(NeedleOffset), typeof(float), typeof(CircularGauge), DefaultNeedleOffset, propertyChanged: OnBindablePropertyChanged);
    public static readonly BindableProperty BaseWidthProperty = BindableProperty.Create(nameof(BaseWidth), typeof(float), typeof(CircularGauge), DefaultBaseWidth, propertyChanged: OnBindablePropertyChanged);
    public static readonly BindableProperty BaseStrokeWidthProperty = BindableProperty.Create(nameof(BaseStrokeWidth), typeof(float), typeof(CircularGauge), DefaultBaseStrokeWidth, propertyChanged: OnBindablePropertyChanged);
    public static readonly BindableProperty ShowScaleProperty = BindableProperty.Create(nameof(ShowScale), typeof(bool), typeof(CircularGauge), true, propertyChanged: OnBindablePropertyChanged);
    public static readonly BindableProperty ScaleDistanceProperty = BindableProperty.Create(nameof(ScaleDistance), typeof(float), typeof(CircularGauge), DefaultScaleDistance, propertyChanged: OnBindablePropertyChanged);
    public static readonly BindableProperty ScaleLengthProperty = BindableProperty.Create(nameof(ScaleLength), typeof(float), typeof(CircularGauge), DefaultScaleLength, propertyChanged: OnBindablePropertyChanged);
    public static readonly BindableProperty ScaleThicknessProperty = BindableProperty.Create(nameof(ScaleThickness), typeof(float), typeof(CircularGauge), DefaultScaleThickness, propertyChanged: OnBindablePropertyChanged);
    public static readonly BindableProperty ScaleUnitsProperty = BindableProperty.Create(nameof(ScaleUnits), typeof(int), typeof(CircularGauge), DefaultScaleUnits, propertyChanged: OnBindablePropertyChanged);
    public static readonly BindableProperty DrawBaseStrokeBeforeFillProperty = BindableProperty.Create(nameof(DrawBaseStrokeBeforeFill), typeof(bool), typeof(CircularGauge), false, propertyChanged: OnBindablePropertyChanged);
    public static readonly BindableProperty GaugeColorProperty = BindableProperty.Create(nameof(GaugeColor), typeof(Color), typeof(CircularGauge), Colors.Red, propertyChanged: OnBindablePropertyChanged);
    public static readonly BindableProperty NeedleColorProperty = BindableProperty.Create(nameof(NeedleColor), typeof(Color), typeof(CircularGauge), Colors.LightGray, propertyChanged: OnBindablePropertyChanged);
    public static readonly BindableProperty BaseColorProperty = BindableProperty.Create(nameof(BaseColor), typeof(Color), typeof(CircularGauge), Colors.LightGray, propertyChanged: OnBindablePropertyChanged);
    public static readonly BindableProperty BaseStrokeColorProperty = BindableProperty.Create(nameof(BaseStrokeColor), typeof(Color), typeof(CircularGauge), Colors.DimGray, propertyChanged: OnBindablePropertyChanged);
    public static readonly BindableProperty ScaleColorProperty = BindableProperty.Create(nameof(ScaleColor), typeof(Color), typeof(CircularGauge), Colors.LightGray, propertyChanged: OnBindablePropertyChanged);

    private static void OnBindablePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((CircularGauge)bindable).InvalidateSurface();
    }

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

        DrawGauge();
        DrawScale();
        DrawNeedle();
        DrawNeedleBase();
    }

    private void DrawGauge()
    {
        using var path = new SKPath();
        using var paint = new SKPaint();

        path.AddArc(_gaugeRect, _adjustedStartAngle, SweepAngle);

        if (GaugeGradientColors?.Count > 0)
        {
            var colors = GaugeGradientColors.Select(color => color.ToSKColor()).ToArray();

            paint.Shader = SKShader.CreateSweepGradient(_center, colors, SKShaderTileMode.Decal, 0.0f, SweepAngle)
                .WithLocalMatrix(SKMatrix.CreateRotationDegrees(_adjustedStartAngle, _center.X, _center.Y));
        }
        else
        {
            paint.Color = GaugeColor.ToSKColor();
        }

        paint.IsAntialias = true;
        paint.Style = SKPaintStyle.Stroke;
        paint.StrokeWidth = GaugeWidth;
        _canvas.DrawPath(path, paint);
    }

    private void DrawScale()
    {
        if (!ShowScale)
        {
            return;
        }

        //calculate amount and divisor for scale units
        var scaleDivisor = (RangeEnd - RangeStart) / (float)ScaleUnits;
        var elementCount = (int)Math.Floor(scaleDivisor) + 1;

        //we may be able to squeeze in one more scale element
        if ((scaleDivisor - elementCount) * ScaleUnits > 1.0f)
        {
            elementCount += 1;
        }

        //account for scale fractions
        var clipFactor = elementCount / scaleDivisor;
        var clippedAngle = SweepAngle * clipFactor;

        //calculate angles for scale
        var angles = new float[elementCount];
        for (var i = 0; i < elementCount; i++)
        {
            angles[i] = clippedAngle / elementCount * i;
        }

        //rotate canvas before drawing scale element
        _canvas.Save();
        _canvas.RotateDegrees(_adjustedStartAngle, _center.X, _center.Y);

        //draw scale elements for each angle
        foreach (var angle in angles)
        {
            var rad = angle.DegreeToRadian();
            var p0 = rad.ToPointOnCircle(_center, _scaleRect.Width / 2);
            var p1 = rad.ToPointOnCircle(_center, _scaleRect.Width / 2 - ScaleLength);

            using var path = new SKPath();
            path.AddPoly(new[] { p0, p1 }, close: false);

            _canvas.DrawPath(path, new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = ScaleColor.ToSKColor(),
                StrokeWidth = ScaleThickness,
                IsAntialias = true
            });
        }

        //rotate canvas back to original position
        _canvas.Restore();
    }

    private void DrawNeedleBase()
    {
        using var basePath = new SKPath();

        var baseRadius = BaseWidth / 2.0f;
        basePath.AddCircle(_center.X, _center.Y, baseRadius);

        using var basePaint = new SKPaint
        {
            IsAntialias = true,
            Color = BaseColor.ToSKColor(),
            Style = SKPaintStyle.Fill
        };

        using var baseStrokePaint = new SKPaint
        {
            IsAntialias = true,
            Color = BaseStrokeColor.ToSKColor(),
            StrokeWidth = BaseStrokeWidth,
            Style = SKPaintStyle.Stroke
        };

        if (DrawBaseStrokeBeforeFill)
        {
            _canvas.DrawPath(basePath, baseStrokePaint);
            _canvas.DrawPath(basePath, basePaint);
        }
        else
        {
            _canvas.DrawPath(basePath, basePaint);
            _canvas.DrawPath(basePath, baseStrokePaint);
        }
    }

    private void DrawNeedle()
    {
        using var needlePath = new SKPath();

        //first set up needle pointing towards 0 degrees (or 6 o'clock)
        var widthOffset = NeedleWidth / 2.0f;
        var needleOffset = NeedleOffset;
        var needleStart = _center.Y - needleOffset;
        var needleLength = NeedleLength;

        needlePath.MoveTo(_center.X - widthOffset, needleStart);
        needlePath.LineTo(_center.X + widthOffset, needleStart);
        needlePath.LineTo(_center.X, needleStart + needleLength);
        needlePath.LineTo(_center.X - widthOffset, needleStart);
        needlePath.Close();

        //then calculate needle position in degrees
        var needlePosition = StartAngle + ((Value - RangeStart) / (RangeEnd - RangeStart) * SweepAngle);

        //finally rotate needle to actual value
        needlePath.Transform(SKMatrix.CreateRotationDegrees(needlePosition, _center.X, _center.Y));

        using var needlePaint = new SKPaint
        {
            IsAntialias = true,
            Color = NeedleColor.ToSKColor(),
            Style = SKPaintStyle.Fill
        };

        _canvas.DrawPath(needlePath, needlePaint);
    }
}