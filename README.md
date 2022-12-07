# epj.CircularGauge.Maui
![License](https://img.shields.io/github/license/ewerspej/epj.CircularGauge.Maui)

Circular Gauge component for .NET MAUI

## Summary

A simple component that can be used as a circular gauge to indicate some numerical progress or status. The control was developed using *SkiaSharp*.

*Note: The scale of the dial currently only shows dashes and no text labels for the scale units.*

## Platforms

The following platforms are currently supported:

| Platform       | Supported          |
|----------------|--------------------|
| Android        | Yes                |
| iOS            | Yes                |
| Windows        | Yes                |
| MacCatalyst    | Maybe (not tested) |

## Highlights

* Customizable Colors and Sizes for Needle and Gauge
* Color Gradients for Gauge
* Adjustable Scale *(unit labels are currently not featured, but the scale steps can be defined)*
* Show/Hide Scale

## Preview

## Usage

### Important: Register Library

In MauiProgram.cs, add a call to *UseCircularGauge()* on the builder object:

```c#
using epj.CircularGauge.Maui;

namespace CircularGaugeSample;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseCircularGauge() // add this line
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        return builder.Build();
    }
}
```

### XAML

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:maui="clr-namespace:epj.CircularGauge.Maui;assembly=epj.CircularGauge.Maui"
             x:Class="CircularGaugeSample.MainPage">

  <VerticalStackLayout
            Spacing="25"
            Padding="30,0"
            VerticalOptions="Center">

    <maui:CircularGauge
      WidthRequest="250"
      HeightRequest="250"
      Value="{Binding Value, Source={x:Reference ValueSlider}}"
      StartAngle="45.0"
      SweepAngle="270.0"
      GaugeWidth="25.0"
      MinValue="0.0"
      MaxValue="100.0"
      NeedleLength="120.0"
      NeedleWidth="10.0"
      NeedleOffset="20.0"
      BaseWidth="20.0"
      BaseStrokeWidth="4.0"
      ScaleLength="8.0"
      ScaleDistance="4.0"
      ScaleThickness="3.0"
      ScaleUnits="10"
      GaugeColor="Red"
      NeedleColor="LightGray"
      BaseColor="LightGray"
      BaseStrokeColor="DimGray"
      ScaleColor="LightGray" />

    <Slider
      x:Name="ValueSlider"
      WidthRequest="250"
      HorizontalOptions="Center"
      Minimum="0.0"
      Maximum="100.0"
      Value="40.0"/>

  </VerticalStackLayout>

</ContentPage>
```

### Color Gradient

In order to use a color gradient for the gauge, just add the desired colors to the `GaugeGradientColors` list, e.g.:

```xml
<maui:CircularGauge
  WidthRequest="250"
  HeightRequest="250"
  Value="{Binding Value, Source={x:Reference ValueSlider}}">
  <maui:CircularGauge.GaugeGradientColors>
    <Color>DarkGreen</Color>
    <Color>DarkGreen</Color>
    <Color>DarkGreen</Color>
    <Color>YellowGreen</Color>
    <Color>Yellow</Color>
    <Color>Yellow</Color>
    <Color>OrangeRed</Color>
    <Color>DarkRed</Color>
    <Color>DarkRed</Color>
  </maui:CircularGauge.GaugeGradientColors>
</maui:CircularGauge>
```

**Note:** Using a Color Gradient will override the `GaugeColor` property.

## Properties

Most of these properties are bindable for MVVM goodness. If something is missing, please open an issue.

| Type        | Property             | Description                                                             | Default Value |
|-------------|----------------------|-------------------------------------------------------------------------|---------------|
| Float       | Value                | The current value of the gauge                                          | `0.0`         |
| Float       | MinValue             | The minimum value used for the value range and scale                    | `0.0`         |
| Float       | MaxValue             | The maximum value used for the value range and scale                    | `100.0`       |
| Float       | StartAngle           | The angle where the gauge begins, attention: 0° is at the botttom here  | `45.0`        |
| Float       | SweepAngle           | The size of the gauge in degrees beginning at the StartAngle            | `270.0`       |
| Float       | GaugeWidth           | Sets the width of the gauge                                             | `25.0`        |
| Float       | NeedleLength         | Sets the length of the needle                                           | `120.0`       |
| Float       | NeedleWidth          | Sets the width of the needle's end (not the tip)                        | `10.0`        |
| Float       | NeedleOffset         | Use this to offset the needle from the center for visual effects        | `20.0`        |
| Float       | BaseWidth            | Sets the diameter of the needle's base                                  | `20.0`        |
| Float       | BaseStrokeWidth      | Sets the width of the stroke for the needle's base, use 0.0 to disable  | `4.0`         |
| Float       | ScaleLength          | The length of a scale unit dash                                         | `8.0`         |
| Float       | ScaleThickness       | The width of a scale unit dash                                          | `3.0`         |
| Float       | ScaleDistance        | Sets the distance between scale and gauge                               | `4.0`         |
| Integer     | ScaleUnits           | Defines to show a scale unit for every *n* steps, e.g. every ten steps  | `10`          |
| Boolean     | ShowScale            | Set to `true` in order to show the scale, `false` to hide it            | `true`        |
| Color       | GaugeColor           | The main color of the gauge control                                     | `Red`         |
| Color       | NeedleColor          | The main color for the needle                                           | `LightGray`   |
| Color       | BaseColor            | The color of the needle's base                                          | `LightGray`   |
| Color       | BaseStrokeColor      | The color of the base's stroke                                          | `DimGray`     |
| Color       | ScaleColor           | The color of the scale's dashes                                         | `LightGray`   |
| List<Color> | GaugeGradientColors  | List of Colors used for the Gradient (from first to last)               | `<empty>`     |

## Notes
* Uses SkiaSharp for MAUI

