' Null notify module that just prints sent notifications and can't receive any
' Author: Kevin Ross

Imports System
Imports ACRemote

Public Class NullNotify
    Inherits Defaultable
    Implements INotify
    Public Sub dummy_handler(e As Object, args As KeyValueEventArgs)
        Console.WriteLine(String.Format("Notification received: {0}={1}", args.key, args.value))
    End Sub
    Public Sub init(hostname As String) Implements INotify.init
        AddHandler KeyNotified, AddressOf dummy_handler
    End Sub
    Public Sub connect() Implements INotify.connect
    End Sub
    Public Sub notify(key As String, value As String) Implements INotify.notify
        Console.WriteLine(String.Format("Notification sent: {0}={1}", key, value))
        RaiseEvent KeyNotified(Me, New KeyValueEventArgs(key, value))
    End Sub
    Public Sub register_keys(keys As String()) Implements INotify.register_keys
    End Sub
    Public Event KeyNotified As EventHandler(Of KeyValueEventArgs) Implements INotify.KeyNotified

End Class