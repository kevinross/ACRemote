' Interface for JSON service
' Author: Kevin Ross

Namespace ACRemote.API
    Public Interface IRemoteService
        Sub set_power(val As Boolean)
        Function get_power() As Boolean
        Sub set_temp(val As Integer)
        Function get_temp() As Integer
        Sub set_mode(mode As String)
        Function get_mode() As String
        Sub set_speed(speed As String)
        Function get_speed() As String
        Function actual_temp() As Double
        Function actual_humidity() As Double
        Sub __set_temp(val As Double)
        Sub __set_humidity(val As Double)
        Sub reset()
    End Interface
End Namespace

