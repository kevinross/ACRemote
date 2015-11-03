' Notifications via MQTT
' Author: Kevin Ross

Imports ACRemote.ACRemote
Imports uPLibrary.Networking.M2Mqtt
Imports uPLibrary.Networking.M2Mqtt.Messages

Namespace Injections.MQTTNotify
    Public Class MQTTNotify
        Implements INotify
        Private mqclient As MqttClient
        Public Event KeyNotified As EventHandler(Of KeyValueEventArgs) Implements INotify.KeyNotified
        ' MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE
        Public Sub New()
        End Sub
        ' initialize module. Can't be in ctor as won't play well with kernel
        Public Sub init(hostname As String) Implements INotify.init
            mqclient = New MqttClient(hostname)
            AddHandler mqclient.MqttMsgPublishReceived, AddressOf client_msgrecvd
        End Sub
        ' connect to notification host
        Public Sub connect() Implements INotify.connect
            mqclient.Connect(Guid.NewGuid().ToString())
        End Sub
        ' send out a notification
        Public Sub notify(key As String, value As String) Implements INotify.notify
            mqclient.Publish(key, System.Text.Encoding.UTF8.GetBytes(value.ToCharArray()))
        End Sub
        ' register keys to listen for in notifications
        Public Sub register_keys(keys As String()) Implements INotify.register_keys
            For Each key As String In keys
                mqclient.Subscribe(New String() {key}, New Byte() {MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE})
            Next
        End Sub
        ' handle notifications of subscribed keys
        Private Sub client_msgrecvd(sender As Object, e As MqttMsgPublishEventArgs)
            RaiseEvent KeyNotified(Me, New KeyValueEventArgs(e.Topic, System.Text.Encoding.UTF8.GetString(e.Message)))
        End Sub
    End Class
End Namespace

