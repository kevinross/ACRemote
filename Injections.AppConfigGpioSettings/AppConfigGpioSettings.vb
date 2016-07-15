' Pulls pin settings from app.config
' Author: Kevin Ross

Imports ACRemote

Public Class AppConfigGpioSettings
    Inherits Defaultable
    Implements IRemoteGpioSettings
    Public ReadOnly Property reset As Integer Implements IRemoteGpioSettings.reset
        Get
            Return AppSettings.Get(Of Integer)("reset_pin")
        End Get
    End Property
    Public ReadOnly Property power As Integer Implements IRemoteGpioSettings.power
        Get
            Return AppSettings.Get(Of Integer)("power_pin")
        End Get
    End Property
    Public ReadOnly Property temp_down As Integer Implements IRemoteGpioSettings.temp_down
        Get
            Return AppSettings.Get(Of Integer)("temp_down_pin")
        End Get
    End Property
    Public ReadOnly Property temp_up As Integer Implements IRemoteGpioSettings.temp_up
        Get
            Return AppSettings.Get(Of Integer)("temp_up_pin")
        End Get
    End Property
    Public ReadOnly Property mode As Integer Implements IRemoteGpioSettings.mode
        Get
            Return AppSettings.Get(Of Integer)("mode_pin")
        End Get
    End Property
    Public ReadOnly Property speed As Integer Implements IRemoteGpioSettings.speed
        Get
            Return AppSettings.Get(Of Integer)("speed_pin")
        End Get
    End Property

    Public Sub New()
    End Sub
End Class