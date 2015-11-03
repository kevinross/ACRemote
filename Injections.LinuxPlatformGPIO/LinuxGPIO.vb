' Uses Raspberry.IO classes and the linux implementation within for GPIO access
' Author: Kevin Ross

Imports Raspberry.IO.GeneralPurpose
Imports ACRemote.ACRemote


Namespace ACRemote
    Public Class LinuxGPIO
        Inherits Defaultable
        Implements IPlatformGPIO
        Private conn As GpioConnection
        Public Sub New()
            Dim settings As New GpioConnectionSettings() With {
                .Driver = GpioConnectionSettings.DefaultDriver
            }
            Me.conn = New GpioConnection(settings, New PinConfiguration() {})
        End Sub
        Public Sub setup(pin As Integer, dir As direction) Implements IPlatformGPIO.setup
            Dim p As ProcessorPin = DirectCast(System.[Enum].Parse(GetType(ProcessorPin), String.Format("Pin{0}", pin)), ProcessorPin)
            Select Case dir
                Case direction.in_
                    Me.conn.Add(New InputPinConfiguration(p))
                    Exit Select
                Case direction.out_
                    Me.conn.Add(New OutputPinConfiguration(p))
                    Exit Select
            End Select
        End Sub
        Default Public Property Item(pin As Integer) As Boolean Implements IPlatformGPIO.Item
            Get
                Dim p As ProcessorPin = DirectCast(System.[Enum].Parse(GetType(ProcessorPin), String.Format("Pin{0}", pin)), ProcessorPin)
                Return Me.conn(p)
            End Get
            Set
                Dim p As ProcessorPin = DirectCast(System.[Enum].Parse(GetType(ProcessorPin), String.Format("Pin{0}", pin)), ProcessorPin)
                Me.conn(p) = Value
            End Set
        End Property
    End Class
End Namespace

