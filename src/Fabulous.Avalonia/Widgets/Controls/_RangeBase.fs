namespace Fabulous.Avalonia

open System
open System.Runtime.CompilerServices
open Avalonia.Controls.Primitives
open Fabulous

type IFabRangeBase =
    inherit IFabTemplatedControl

type RangeBaseValueChangedEventArgs(oldValue: float, newValue: float) =
    inherit EventArgs()
    member val OldValue: float = oldValue
    member val NewValue: float = newValue

module RangeBaseUpdaters =
    let updateSliderMinMax _ (newValueOpt: struct (float * float) voption) (node: IViewNode) =
        let slider = node.Target :?> RangeBase

        match newValueOpt with
        | ValueNone ->
            slider.ClearValue(RangeBase.MinimumProperty)
            slider.ClearValue(RangeBase.MaximumProperty)
        | ValueSome(min, max) ->
            let currMax = slider.GetValue(RangeBase.MaximumProperty)

            if min > currMax then
                slider.SetValue(RangeBase.MaximumProperty, max)
                slider.SetValue(RangeBase.MinimumProperty, min)
            else
                slider.SetValue(RangeBase.MinimumProperty, min)
                slider.SetValue(RangeBase.MaximumProperty, max)

module RangeBase =
    let MinimumMaximum =
        Attributes.defineSimpleScalarWithEquality<struct (float * float)>
            "RangeBase_MinimumMaximum"
            RangeBaseUpdaters.updateSliderMinMax

    let Value = Attributes.defineAvaloniaPropertyWithEquality RangeBase.ValueProperty

    let SmallChange =
        Attributes.defineAvaloniaPropertyWithEquality RangeBase.SmallChangeProperty

    let LargeChange =
        Attributes.defineAvaloniaPropertyWithEquality RangeBase.LargeChangeProperty

[<Extension>]
type RangeBaserModifiers =
    [<Extension>]
    static member inline smallChange(this: WidgetBuilder<'msg, #IFabRangeBase>, value: float) =
        this.AddScalar(RangeBase.SmallChange.WithValue(value))

    [<Extension>]
    static member inline largeChange(this: WidgetBuilder<'msg, #IFabRangeBase>, value: float) =
        this.AddScalar(RangeBase.LargeChange.WithValue(value))
