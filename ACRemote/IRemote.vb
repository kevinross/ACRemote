' Interface for an air conditioner remote
' Author: Kevin Ross

Namespace ACRemote
    Public Enum modes
        ac
        dehumid
        fan
    End Enum
    Public Enum speeds
        low
        medium
        high
    End Enum
    Public Interface IRemote
        Property power() As Boolean
        Property temp() As Integer
        Property mode() As modes
        Property speed() As speeds
        Sub reset()
        ReadOnly Property actual_temp() As Double
        ReadOnly Property actual_humidity() As Double
        ' setting temperature and humidity for testing
        Sub __set_temp(val As Double)
        Sub __set_humidity(val As Double)
    End Interface
End Namespace

