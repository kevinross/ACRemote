' Provides a base class for DHT11 drivers to implement
' The idea is that this provides most functionality while leaving the door open
' for a different implementation of IDHT11
' Author: Kevin Ross
Imports System.Threading

Namespace ACRemote
    Public MustInherit Class DHT11
        Inherits Defaultable
        Implements IDHT11
        Protected temp_ As Double
        Protected humid_ As Double
        Private last_check As DateTime

        Public Property temperature As Double Implements IDHT11.temperature
            Get
                wait_until_sample_possible()
                update()
                ' value is 1000*actual
                Return Me.temp_ / 1000.0
            End Get
            Set(value As Double)
                Me.temp_ = value
            End Set
        End Property

        Public Property humidity As Double Implements IDHT11.humidity
            Get
                wait_until_sample_possible()
                update()
                ' same as temp
                Return Me.humid_ / 1000.0
            End Get
            Set(value As Double)
                Me.humid_ = value
            End Set
        End Property

        Public Sub New()
            last_check = DateTime.Now
        End Sub

        Private Sub wait_until_sample_possible()
            ' less than 100 milliseconds isn't really worth waiting again
            If (DateTime.Now - last_check) < (New TimeSpan(100 * 10000)) Then
                Return
            End If
            While (DateTime.Now - last_check) < (New TimeSpan(1000 * 10000))
                Thread.Sleep(100)
            End While
            last_check = DateTime.Now
        End Sub
        ' override this method and update .humid_ and .temp_ in it
        Protected MustOverride Sub update()
    End Class
End Namespace

