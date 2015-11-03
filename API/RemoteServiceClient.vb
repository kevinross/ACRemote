' JSON-RPC proxy class so we can use the service as if it were a local object
' Author: Kevin Ross

Imports System.Reflection
Imports System.Runtime.Remoting.Proxies
Imports System.Runtime.Remoting.Messaging
Imports ACRemote.ACRemote

Namespace ACRemote.API
    ' alternate implementation to experiment with
    Public Class RemoteServiceProxy
        Inherits RealProxy
        Private client As JsonRpcClient
        Public Shared Function createProxy(endpoint As String) As IRemote
            Return DirectCast(New RemoteServiceProxy(endpoint, GetType(IRemote)).GetTransparentProxy(), IRemote)
        End Function
        Private Sub New(endpoint As String, iface As Type)
            MyBase.New(iface)
            client = New JsonRpcClient(New Uri(endpoint))
        End Sub
        Public Overrides Function Invoke(im As IMessage) As IMessage
            If im.[GetType]() = GetType(IMethodCallMessage) Then
                Dim imcm As IMethodCallMessage = DirectCast(im, IMethodCallMessage)
                Dim meth As MethodInfo = DirectCast(imcm.MethodBase, MethodInfo)
                If meth.ReturnType = GetType(System.Void) Then
                End If
                Dim retval As Object = If(imcm.ArgCount = 0, Me.invoke_(imcm.MethodName), Me.invoke_(imcm.MethodName, imcm.GetArg(0)))
                Return New ReturnMessage(retval, Nothing, 0, imcm.LogicalCallContext, imcm)
            End If
            Return Nothing
        End Function
        Private Function invoke_(method As String, Optional param As Object = Nothing) As Object
            Return client.InvokeMethod(method, param)
        End Function
        Private Sub invoke_void(method As String, Optional param As Object = Nothing)
            client.InvokeMethod(method, param)
        End Sub
    End Class
    ' primary implementation
    Public Class RemoteServiceClient
        Implements IRemote
        Private client As JsonRpcClient
        Private Function invoke(Of T)(method As String) As T
            Return client.InvokeMethod(Of T)(method)
        End Function
        Private Function invoke(Of T)(method As String, param As Object) As T
            Return client.InvokeMethod(Of T)(method, param)
        End Function
        Private Sub invoke(method As String)
            client.InvokeMethod(method)
        End Sub
        Private Sub invoke(method As String, param As Object)
            client.InvokeMethod(method, param)
        End Sub
        Private Function get_val(Of T)(param As String) As T
            Return invoke(Of T)(String.Format("get_{0}", param))
        End Function
        Private Sub set_val(Of T)(param As String, val As T)
            invoke(String.Format("set_{0}", param), val)
        End Sub

        Public Property power As Boolean Implements IRemote.power
            Get
                Return get_val(Of Boolean)("power")
            End Get
            Set
                set_val(Of Boolean)("power", Value)
            End Set
        End Property

        Public Property temp As Integer Implements IRemote.temp
            Get
                Return get_val(Of Integer)("temp")
            End Get
            Set
                set_val(Of Integer)("temp", Value)
            End Set
        End Property
        Public Property mode As modes Implements IRemote.mode
            Get
                Return PropertyUtil.convert_str_to_enum(Of modes)(get_val(Of String)("mode"))
            End Get
            Set
                set_val(Of String)("mode", Value.ToString())
            End Set
        End Property
        Public Property speed As speeds Implements IRemote.speed
            Get
                Return PropertyUtil.convert_str_to_enum(Of speeds)(get_val(Of String)("speed"))
            End Get
            Set
                set_val(Of String)("speed", Value.ToString())
            End Set
        End Property
        Public ReadOnly Property actual_temp As Double Implements IRemote.actual_temp
            Get
                Return get_val(Of Double)("actual_temp")
            End Get
        End Property
        Public ReadOnly Property actual_humidity As Double Implements IRemote.actual_humidity
            Get
                Return get_val(Of Double)("actual_humidity")
            End Get
        End Property
        Public Sub __set_temp(val As Double) Implements IRemote.__set_temp
            invoke(Of Double)("__set_temp", val)
        End Sub
        Public Sub __set_humidity(val As Double) Implements IRemote.__set_humidity
            invoke(Of Double)("__set_humidity", val)
        End Sub
        Public Sub reset() Implements IRemote.reset
            invoke("reset")
        End Sub
        Public Sub New(endpoint As String)
            client = New JsonRpcClient(New Uri(endpoint))
        End Sub
    End Class
End Namespace

