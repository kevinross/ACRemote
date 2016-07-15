' Null GPIO module uses a map to store state
' Author: Kevin Ross

Imports TinyIoC
Imports ACRemote

Public Class NullGPIO
    Inherits Defaultable
    Implements IPlatformGPIO
    Private pins As New System.Collections.Generic.Dictionary(Of Integer, Boolean)()
    Public Sub setup(pin As Integer, dir As direction) Implements IPlatformGPIO.setup
        pins(pin) = False
        Console.WriteLine(String.Format("GPIO: setup pin {0} as {1}", pin, dir.ToString()))
    End Sub
    Default Public Property Item(val As Integer) As Boolean Implements IPlatformGPIO.Item
        Get
            Console.WriteLine(String.Format("GPIO: get pin {0}={1}", val, pins(val)))
            Return pins(val)
        End Get
        Set
            Console.WriteLine(String.Format("GPIO: set pin {0}={1}", val, Value))
            pins(val) = Value
        End Set
    End Property
End Class