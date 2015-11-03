' Interface for GPIO pin number settings
' Author: Kevin Ross

Namespace ACRemote
    Public Interface IRemoteGpioSettings
        ReadOnly Property reset() As Integer
        ReadOnly Property power() As Integer
        ReadOnly Property temp_down() As Integer
        ReadOnly Property temp_up() As Integer
        ReadOnly Property mode() As Integer
        ReadOnly Property speed() As Integer
    End Interface
End Namespace

