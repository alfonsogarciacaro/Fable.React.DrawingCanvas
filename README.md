# Fable React DrawingCanvas

## Demo App

This [demo app](https://davedawkins.github.io/Fable.React.DrawingCanvas/) shows a clock rendered with the DrawingCanvas, updating in real-time

<img src="./demo.png" width="64">

## About

This is a Fable React wrapper for `canvas` that allows you to declare a drawing like this:

```
    div [] [
        drawingcanvas {
            Redraw = Drawing drawing {
                resize 400.00 400.0
                translate 200.0 200.0
                lineWidth 6.0
                beginPath
                arc 0.0 0.0 195.0 0.0 (2.0 * Math.PI) true
                stroke
            }
        }
    ]
```

If you wish, you can supply a list of `DrawCommand` instead:

```
    div [] [
        drawingcanvas {
            Redraw = Drawing [
                Resize (400.0, 400.0)
                Translate (200.0, 200.0)
                LineWidth 6.0
                BeginPath
                Arc (0.0, 0.0, 195.0, 0.0, (2.0 * Math.PI), Some true)
                Stroke
            ]
        }
    ]
```

This demonstrates that `drawing { ... }` is just a builder that generates a `DrawCommand list`. Both approaches present their
challenges when it comes to control structures such as loops and conditionals.

One more option is to pass redraw function from which you may launch missiles if you wish (this is what all presentations about pure functions fear the most):

```
    div [] [
        drawingcanvas {
            Redraw = DrawFunction (fun ctx ->
                ctx.canvas.width <- 400.0
                ctx.canvas.height <- 400.0
                ctx.translate(200.0, 200.0)
                ctx.lineWidth <- 6.0
                ctx.beginPath()
                ctx.arc (0.0, 0.0, 195.0, 0.0, (2.0 * Math.PI), true)
                ctx.stroke()
            )
        }
    ]
```

The demo linked at the top of this page includes code to draw the clock in all three ways. See these files for comparison:

- `./app/ClockUsingBuilder.fs`
- `./app/ClockUsingFunction.fs`
- `./app/ClockUsingList.fs`

<img src="./demo.png" width="400">

## Motivation

This component was inspired by Maxime Mangel's [Elmish.Canvas](https://github.com/MangelMaxime/Elmish.Canvas). I created this component as a learning exercise mainly. I wanted to see if I could derive the React component entirely in Fable, and I also wanted to see how the drawing syntax would look as a Computation Expression. This is my first attempt at a CE, and while it didn't turn out as neatly as I wanted, I'm pleased that it works. I like how the CE variant removes tuple-form arguments, for example. 

I noticed that I would do a lot of `save`/`restore` sub-drawings so I wanted to build a construct in to do that automatically. If you see `Insert` anywhere, this takes a sub-drawing and automatically wraps it with `save`/`restore`. I wondered about doing with the same with `beginPath/fill` and `beginPath/stroke` sequences.

## Issues

- arc() and fillText() take optional arguments, but I haven't found a way to handle this nicely as a CE or DU. As a CE, you can make the member function take an optional argument, but the compiler isn't happy when you omit the value. Worse, you can't then pass "None", you have to pass the wrapped type (eg, bool). This means you can't tell arc() to draw the shortest arc by passing "None", you have to pass "true" or "false". It's better as the DU type "Arc" where you can pass "None", or "Some true|false".

- The CE implementation is a lot of wrapper code around the DU, and I'm not sure how much value it adds. I didn't quite get the `for` and `if/then/else` constructs that I wanted, and from what I can tell, that's down to the use of `CustomExpression`. On the other hand, I'm very new to this, and I might be missing something. There's probably a good reason Maxime didn't go down this route.

- I like that the CE and DU represent pure approaches to specifying the drawing, but I'm not 100% sure I'm following the React rules in executing the drawing. From what I can tell, you can do it during `componentDidMount` and `componentDidUpdate`, provided you have captured a reference to the DOM canvas using `Ref`.

- Incomplete. I have enough of Canvas2D implemented to do the clock, but I need to add the rest of the API

- Documentation. No docs yet

## Availability

I'd be happy to turn this into a Nuget package, add some documentation. I'm definitely going to use it for my own projects. Firstly, I want to rebuild [Wet Frank](http://www.wetfrank.com) in Fable with DrawingCanvas.



