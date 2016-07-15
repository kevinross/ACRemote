Imports System.Text
Imports System.Web
Imports System.Net
Imports System.IO
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class Remote
    ' create some null modules to test with
    Dim dht11 As Injections.NullModules.NullDHT11 = New Injections.NullModules.NullDHT11
    Dim gpio As Injections.NullModules.NullGPIO = New Injections.NullModules.NullGPIO
    Dim gpio_settings As Injections.NullModules.NullGPIOSettings = New Injections.NullModules.NullGPIOSettings
    Dim notify As Injections.NullModules.NullNotify = New Injections.NullModules.NullNotify
    Dim remote As ACRemote.Remote = New ACRemote.Remote(gpio_settings, dht11, gpio, notify)

    ' test whether notifications get processed by the handler
    <TestMethod()> Public Sub TestHandleNotification()
        Dim was_notified As Boolean = False
        Dim test_sub = Sub(sender As Object, args As ACRemote.KeyValueEventArgs)
                           If args.key.Equals("power") And args.value.Equals("ON") Then
                               was_notified = True
                           End If
                       End Sub
        AddHandler notify.KeyNotified, test_sub
        notify.notify("power", "ON")
        RemoveHandler notify.KeyNotified, test_sub
        Assert.IsTrue(was_notified)
    End Sub
    ' test that the remote's property setting works
    <TestMethod()> Public Sub TestRemote()
        remote.reset()
        Assert.IsFalse(remote.power)
        remote.power = True
        Assert.IsTrue(remote.power)

        Assert.AreEqual(remote.mode, ACRemote.modes.ac)
        remote.mode = ACRemote.modes.fan
        Assert.AreEqual(remote.mode, ACRemote.modes.fan)
    End Sub

    ' test that the sensor's property setting works
    <TestMethod()> Public Sub TestSensor()
        Assert.AreEqual(remote.actual_temp, 0.0)
        dht11.temperature = 25
        Assert.AreEqual(remote.actual_temp, 25.0)
    End Sub
    ' start up a local testing server for RPC
    Private Sub start_server()
        Dim svc_class As ACRemote.ServiceClass = New ACRemote.ServiceClass
        ACRemote.ServiceClass.global_gpio = gpio
        ACRemote.ServiceClass.global_gpio_settings = gpio_settings
        ACRemote.ServiceClass.global_notify = notify
        ACRemote.ServiceClass.global_temp = dht11
        svc_class.setup()
        ACRemote.ServiceClass.ws.Run()
    End Sub
    ' stop the testing server and reset the remote
    Private Sub stop_server()
        ACRemote.ServiceClass.global_remote.reset()
        ACRemote.ServiceClass.ws.Stop()
    End Sub

    ' test that the json-rpc server works
    <TestMethod()> Public Sub TestRpc()
        start_server()
        Dim client As ACRemote.API.RemoteServiceClient = New ACRemote.API.RemoteServiceClient(ACRemote.AppSettings.Get(Of String)("api_endpoint").Replace("*", "localhost"))
        ' defaults to false
        Assert.AreEqual(ACRemote.ServiceClass.global_remote.power, False)
        ' set the new value through the client
        client.power = True
        ' check the server-side value
        Assert.AreEqual(ACRemote.ServiceClass.global_remote.power, True)
        ' check the client-side value
        Assert.AreEqual(client.power, True)
        ' set the server-side value
        ACRemote.ServiceClass.global_remote.power = False
        ' check the client-side value
        Assert.AreEqual(client.power, False)
        stop_server()
    End Sub

    <TestMethod()> Public Sub TestRest()
        start_server()
        ' false by default
        Assert.AreEqual(ACRemote.ServiceClass.global_remote.power, False)
        ' set to true via REST
        Dim req As HttpWebRequest = DirectCast(WebRequest.Create(String.Format("{0}power", ACRemote.AppSettings.Get(Of String)("remote_endpoint").Replace("*", "localhost"))), HttpWebRequest)
        req.Method = "POST"
        req.ContentType = "text/plain"
        Using webstream As Stream = req.GetRequestStream()
            Using reqwriter As StreamWriter = New StreamWriter(webstream, System.Text.Encoding.ASCII)
                reqwriter.Write("On")
            End Using
        End Using
        Dim resp As WebResponse = req.GetResponse()
        Using webstream As Stream = resp.GetResponseStream()
            Using respreader As StreamReader = New StreamReader(webstream)
                ' assert value now true
                Assert.AreEqual(respreader.ReadToEnd().Trim(), "True")
            End Using
        End Using
        ' check server-side
        Assert.AreEqual(ACRemote.ServiceClass.global_remote.power, True)
        ' set server-side
        ACRemote.ServiceClass.global_remote.power = False
        ' get current value via REST
        req = DirectCast(WebRequest.Create(String.Format("{0}power", ACRemote.AppSettings.Get(Of String)("remote_endpoint").Replace("*", "localhost"))), HttpWebRequest)
        req.Method = "GET"
        req.ContentType = "text/plain"
        resp = req.GetResponse()
        Using webstream As Stream = resp.GetResponseStream()
            Using respreader As StreamReader = New StreamReader(webstream)
                ' assert value now false
                Assert.AreEqual(respreader.ReadToEnd().Trim(), "False")
            End Using
        End Using
        stop_server()
    End Sub

    <TestMethod()> Public Sub TestDatabase()
        start_server()
        ' add 2 records via rpc
        Using ctx As ACRemote.EnviroContext = New ACRemote.EnviroContext()
            Dim client As ACRemote.API.RemoteServiceClient = New ACRemote.API.RemoteServiceClient(ACRemote.AppSettings.Get(Of String)("api_endpoint"))
            Assert.AreEqual(client.actual_temp, 0.0)
            ACRemote.DataLoggingService.CollectRecord(ctx, client)
            client.__set_temp(25.0)
            Assert.AreEqual(client.actual_temp, 25.0)
            ACRemote.DataLoggingService.CollectRecord(ctx, client)
        End Using
        stop_server()
        ' check records exist
        Using ctx As ACRemote.EnviroContext = New ACRemote.EnviroContext()
            Assert.AreEqual(ctx.Records.First.Temp, 0.0)
            Assert.AreEqual(ctx.Records.Last.Temp, 25.0)
        End Using
    End Sub
End Class