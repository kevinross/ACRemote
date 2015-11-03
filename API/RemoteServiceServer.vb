' JSON-RPC service server
' Author: Kevin Ross

Imports AustinHarris.JsonRpc
Imports ACRemote.ACRemote

Namespace ACRemote.API
    Public Class RemoteServiceServer
        Inherits JsonRpcService
        Implements IRemoteService
        Private remote As IRemote = Nothing
        Private temp As IDHT11 = Nothing
        Public Sub New(rmt As IRemote, temp As IDHT11)
            Me.remote = rmt
            Me.temp = temp
        End Sub
        <JsonRpcMethod>
        Public Sub set_power(val As Boolean) Implements IRemoteService.set_power
            remote.power = val
        End Sub
        <JsonRpcMethod>
        Public Function get_power() As Boolean Implements IRemoteService.get_power
            Return remote.power
        End Function
        <JsonRpcMethod>
        Public Sub set_temp(val As Integer) Implements IRemoteService.set_temp
            remote.temp = val
        End Sub
        <JsonRpcMethod>
        Public Function get_temp() As Integer Implements IRemoteService.get_temp
            Return remote.temp
        End Function
        <JsonRpcMethod>
        Public Function get_mode() As String Implements IRemoteService.get_mode
            Return remote.mode.ToString()
        End Function
        <JsonRpcMethod>
        Public Sub set_mode(val As String) Implements IRemoteService.set_mode
            remote.mode = DirectCast(System.Enum.Parse(GetType(modes), val), modes)
        End Sub
        <JsonRpcMethod>
        Public Function get_speed() As String Implements IRemoteService.get_speed
            Return remote.speed.ToString()
        End Function
        <JsonRpcMethod>
        Public Sub set_speed(val As String) Implements IRemoteService.set_speed
            remote.speed = DirectCast(System.Enum.Parse(GetType(speeds), DirectCast(val, [String])), speeds)
        End Sub
        <JsonRpcMethod>
        Public Function actual_temp() As Double Implements IRemoteService.actual_temp
            Return temp.temperature
        End Function
        <JsonRpcMethod>
        Public Function actual_humidity() As Double Implements IRemoteService.actual_humidity
            Return temp.humidity
        End Function
        <JsonRpcMethod>
        Public Sub __set_temp(val As Double) Implements IRemoteService.__set_temp
        End Sub
        <JsonRpcMethod>
        Public Sub __set_humidity(val As Double) Implements IRemoteService.__set_humidity
        End Sub
        <JsonRpcMethod>
        Public Sub reset() Implements IRemoteService.reset
            remote.reset()
        End Sub
    End Class
End Namespace

