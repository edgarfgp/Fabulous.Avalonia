namespace Fabulous.Avalonia

open System
open System.Runtime.CompilerServices
open Avalonia
open Avalonia.Collections
open Avalonia.Controls
open Avalonia.Layout

open Fabulous

type IFabSlider =
    inherit IFabRangeBase

module Slider =
    let WidgetKey = Widgets.register<Slider> ()

    let Orientation =
        Attributes.defineAvaloniaPropertyWithEquality Slider.OrientationProperty

    let IsDirectionReversed =
        Attributes.defineAvaloniaPropertyWithEquality Slider.IsDirectionReversedProperty

    let IsSnapToTickEnabled =
        Attributes.defineAvaloniaPropertyWithEquality Slider.IsSnapToTickEnabledProperty

    let TickFrequency =
        Attributes.defineAvaloniaPropertyWithEquality Slider.TickFrequencyProperty

    let TickPlacement =
        Attributes.defineAvaloniaPropertyWithEquality Slider.TickPlacementProperty

    let Ticks =
        Attributes.defineSimpleScalarWithEquality<float list> "Slider_Ticks" (fun _ newValueOpt node ->
            let target = node.Target :?> AvaloniaObject

            match newValueOpt with
            | ValueNone -> target.ClearValue(Slider.TicksProperty)
            | ValueSome points ->
                let coll = AvaloniaList<float>()
                points |> List.iter coll.Add
                target.SetValue(Slider.TicksProperty, coll) |> ignore)


    let ValueChanged =
        Attributes.defineAvaloniaPropertyWithChangedEvent' "Slider_ValueChanged" Slider.ValueProperty

[<AutoOpen>]
module SliderBuilders =
    type Fabulous.Avalonia.View with

        static member inline Slider<'msg>(min: float, max: float, value: float, onValueChanged: float -> 'msg) =
            WidgetBuilder<'msg, IFabSlider>(
                Slider.WidgetKey,
                RangeBase.MinimumMaximum.WithValue(min, max),
                RangeBase.Value.WithValue(value),
                Slider.ValueChanged.WithValue(ValueEventData.create value (fun args -> onValueChanged args |> box))
            )

[<Extension>]
type SliderModifiers =
    [<Extension>]
    static member inline orientation(this: WidgetBuilder<'msg, #IFabSlider>, value: Orientation) =
        this.AddScalar(Slider.Orientation.WithValue(value))

    [<Extension>]
    static member inline isDirectionReversed(this: WidgetBuilder<'msg, #IFabSlider>, value: bool) =
        this.AddScalar(Slider.IsDirectionReversed.WithValue(value))

    [<Extension>]
    static member inline isSnapToTickEnabled(this: WidgetBuilder<'msg, #IFabSlider>, value: bool) =
        this.AddScalar(Slider.IsSnapToTickEnabled.WithValue(value))

    [<Extension>]
    static member inline tickFrequency(this: WidgetBuilder<'msg, #IFabSlider>, value: float) =
        this.AddScalar(Slider.TickFrequency.WithValue(value))

    [<Extension>]
    static member inline tickPlacement(this: WidgetBuilder<'msg, #IFabSlider>, value: TickPlacement) =
        this.AddScalar(Slider.TickPlacement.WithValue(value))

    [<Extension>]
    static member inline ticks(this: WidgetBuilder<'msg, #IFabSlider>, value: float list) =
        this.AddScalar(Slider.Ticks.WithValue(value))
