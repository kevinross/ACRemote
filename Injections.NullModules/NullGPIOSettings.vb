' Null GPIO settings module provides the currently wired pin numbers, hardcoded
' Author: Kevin Ross

Imports TinyIoC
Imports ACRemote.ACRemote

Namespace Injections.NullModules
  Public Class NullGPIOSettings
    Inherits Defaultable
    Implements IRemoteGpioSettings
        Public ReadOnly Property reset As Integer Implements IRemoteGpioSettings.reset
            Get
                Return 20
            End Get
        End Property
        Public ReadOnly Property power As Integer Implements IRemoteGpioSettings.power
            Get
                Return 24
            End Get
        End Property
        Public ReadOnly Property temp_down As Integer Implements IRemoteGpioSettings.temp_down
            Get
                Return 23
            End Get
        End Property
        Public ReadOnly Property temp_up As Integer Implements IRemoteGpioSettings.temp_up
            Get
                Return 22
            End Get
        End Property
        Public ReadOnly Property mode As Integer Implements IRemoteGpioSettings.mode
            Get
                Return 10
            End Get
        End Property
        Public ReadOnly Property speed As Integer Implements IRemoteGpioSettings.speed
            Get
                Return 9
            End Get
        End Property
        Public Sub New()
        End Sub
    End Class
End Namespace

