﻿module Test.DrawingCanvas.Turtle

open Util
open Fable.Mocha
open Fable.React.DrawingCanvas
open Fable.React.DrawingCanvas.Turtle

let tests = testList "DrawingCanvas.Turtle" [

    testCase "Turtle basic" <| fun () ->
        let d = turtle {
            penDown
            forward 1.0
        }

        let expected = [
            BeginPath
            MoveTo (0.0,0.0)
            LineTo (1.0,0.0)
            Translate (1.0,0.0)
            Stroke
        ]
        expectDrawingsEqual expected d

    testCase "Turtle nested" <| fun () ->
        let d = turtle {
            penDown
            sub (turtle {
                turn 180.0
            })
            forward 1.0
        }

        let expected = [
            Rotate (System.Math.PI)
            BeginPath
            MoveTo (0.0,0.0)
            LineTo (1.0,0.0)
            Translate (1.0,0.0)
            Stroke
        ]
        expectDrawingsEqual expected d

    testCase "Turtle repeat" <| fun () ->
        let d = turtle {
            penDown
            repeat [1..2] (fun i -> (turtle {
                turn (float i * 180.0)
            }))
            forward 1.0
        }

        let expected = [
            Rotate (System.Math.PI)
            Rotate (System.Math.PI * 2.0)
            BeginPath
            MoveTo (0.0,0.0)
            LineTo (1.0,0.0)
            Translate (1.0,0.0)
            Stroke
        ]
        expectDrawingsEqual expected d

    testCase "Turtle repeat sub" <| fun () ->
        let d0 i = turtle {
            turn (float i * 180.0)
        }

        let d = turtle {
            penDown
            repeat [1..2] d0
            forward 1.0
        }

        let expected = [
            Rotate (System.Math.PI)
            Rotate (System.Math.PI * 2.0)
            BeginPath
            MoveTo (0.0,0.0)
            LineTo (1.0,0.0)
            Translate (1.0,0.0)
            Stroke
        ]
        expectDrawingsEqual expected d


    testCase "Turtle ifThen" <| fun () ->
        let d = turtle {
            penDown
            ifThen true (turtle { turn 180. })
        }

        let expected = [
            Rotate (System.Math.PI)
        ]

        expectDrawingsEqual expected d


    testCase "Turtle ifThenFn" <| fun () ->
        let tfn() = turtle {
            ifThen true ( turtle { turn 180. })
        }

        let d = turtle {
            penDown
            sub (tfn())
        }

        let expected = [
            Rotate (System.Math.PI)
        ]

        expectDrawingsEqual expected d

    testCase "Turtle pen Up Down" <| fun () ->
        let d = turtle {
            penUp
            forward 1.
            penDown
            forward 2.
            penUp
            forward 3.
        }

        let expected = [
            BeginPath
            MoveTo (0.,0.)

            MoveTo (1.,0.)
            Translate (1.,0.)

            LineTo (2.,0.)
            Translate (2.,0.)

            MoveTo (3.,0.)
            Translate (3.,0.)

            Stroke
        ]

        expectDrawingsEqual expected d

    testCase "Turtle pen Up Down Fn" <| fun () ->
        let f = turtle {
            forward 2.
        }
        let d = turtle {

            penUp
            forward 1.
            penDown
            sub f
            penUp
            forward 3.
        }

        let expected = [
            BeginPath
            MoveTo (0.,0.)

            MoveTo (1.,0.)
            Translate (1.,0.)

            LineTo (2.,0.)
            Translate (2.,0.)

            MoveTo (3.,0.)
            Translate (3.,0.)

            Stroke
        ]

        let turtle = { IsPenDown = false; LineCount = 0 }

        for cmd in (d() |> translate turtle |> Seq.toList) do
            System.Console.WriteLine("{0}", cmd)
        expectDrawingsEqual expected d

]
