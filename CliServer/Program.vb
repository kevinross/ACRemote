' Main server. Provides several interfaces: JSON-RPC, REST methods, MQTT.
' Author: Kevin Ross

Imports System.Collections.Specialized
Imports System.Threading
Imports AustinHarris.JsonRpc
Imports System.Net
Imports ACRemote.API
Imports SimpleWebServer

Public Class ServiceClass
    Public Shared global_temp As IDHT11
    Public Shared global_gpio As IPlatformGPIO
    Public Shared global_gpio_settings As IRemoteGpioSettings
    Public Shared global_notify As INotify
    Public Shared global_remote As IRemote
    Public Shared service As IRemoteService = Nothing
    Public Shared ws As WebServer
    ' a set of keys we're interested in hearing about
    Private Shared pubsub_topics As String() = {"/ac/state/power", "/ac/state/mode", "/ac/state/speed", "/ac/state/temp"}
    Public Sub run()
        ' setup globals
        resolve_impls()
        ' setup listeners
        setup()
        ' run web server
        ws.Run()
        ' connect to notification server
        global_notify.connect()
        Dim temp_thread As New Thread(New ThreadStart(AddressOf report_sensors))
        temp_thread.Start()
        ' goal is to have this as a window's service so until then, wait for key press
        ' when service is implemented, this will just spin
        If Environment.UserInteractive Then
            Console.ReadKey()
        Else
            While True
                Thread.Sleep(100)
            End While
        End If
        ' stop everything
        ws.Stop()
        temp_thread.Abort()
    End Sub
    Private Sub report_sensors()
        While True
            global_notify.notify("/ac/sensor/temp", global_temp.temperature.ToString())
            global_notify.notify("/ac/sensor/humidity", global_temp.humidity.ToString())
            Thread.Sleep(3000)
        End While
    End Sub
    Private Sub resolve_impls()
        ' resolve all instances
        global_temp = KernelInstance.Resolve(Of IDHT11)()
        global_gpio = KernelInstance.Resolve(Of IPlatformGPIO)()
        global_gpio_settings = KernelInstance.Resolve(Of IRemoteGpioSettings)()
        global_notify = KernelInstance.Resolve(Of INotify)()
    End Sub
    Public Sub setup()
        ' create a remote
        global_remote = New Remote(global_gpio_settings, global_temp, global_gpio, global_notify)
        ' create the JSON-RPC server
        If service Is Nothing Then
            service = New RemoteServiceServer(global_remote, global_temp)
        End If
        Console.WriteLine(String.Format("Listening on {0} for JSON-RPC and {1} for basic GET/POST", AppSettings.Get(Of String)("api_endpoint"), AppSettings.Get(Of String)("remote_endpoint")))
        ' create the web server
        ws = New WebServer(New String() {AppSettings.Get(Of String)("api_endpoint"), AppSettings.Get(Of String)("remote_endpoint")}, AddressOf SendResponse)
        ' subscribe to notifications
        global_notify.init(AppSettings.Get(Of String)("mqtt_broker"))
        global_notify.register_keys(pubsub_topics)
        AddHandler global_notify.KeyNotified, AddressOf handle_change
    End Sub
    Private Sub handle_change(e As Object, msg As KeyValueEventArgs)
        PropertyUtil.set_prop(global_remote, msg.key, msg.value)
    End Sub
    ' respond to HTTP requests, either REST or JSON-RPC
    Private Function SendResponse(req As HttpListenerRequest) As String
        Dim data As String = GetRequestPostData(req)
        If data IsNot Nothing AndAlso data.Contains("json") Then
            Dim tsk As System.Threading.Tasks.Task(Of String) = JsonRpcProcessor.Process(data, DirectCast(Nothing, Object))
            Return tsk.Result
        Else
            ' grab the last segment: /remote/{segment} (such as power, mode)
            Dim lastelem As String = req.Url.Segments(req.Url.Segments.Length - 1)
            If data.Trim() = "" Then
                data = req.QueryString("val")
            End If
            Select Case req.HttpMethod
                Case "GET"
                    Return PropertyUtil.get_prop(global_remote, lastelem).ToString()
                Case "POST"
                    PropertyUtil.set_prop(global_remote, lastelem, data.Trim())
                    Return PropertyUtil.get_prop(global_remote, lastelem).ToString()
                Case "DELETE"
                    global_remote.reset()
                    Exit Select
            End Select
            Return ""
        End If
    End Function
    ' get the data from the post
    Private Function GetRequestPostData(request As HttpListenerRequest) As String
        If Not request.HasEntityBody Then
            Return ""
        End If
        Using body As System.IO.Stream = request.InputStream
            ' here we have data

            Using reader As New System.IO.StreamReader(body, request.ContentEncoding)
                Return reader.ReadToEnd()
            End Using
        End Using
    End Function
    ' useful for debugging: print the parsed query string
    Public Shared Function ConstructQueryString(parameters As NameValueCollection) As String
        Dim items As List(Of [String]) = New List(Of String)()

        For Each name As String In parameters
            items.Add(String.Concat(name, "=", System.Web.HttpUtility.UrlEncode(parameters(name))))
        Next

        Return String.Join("&", items.ToArray())
    End Function
End Class
Public Class MainClass
    Public Shared Sub Main(args As String())
        Dim sc = New ServiceClass
        sc.run()
    End Sub
End Class