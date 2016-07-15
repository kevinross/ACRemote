' Interface for publishing and subscribing to a notification system
' Author: Kevin Ross

Public Interface INotify
    ' initialize notifier with a hostname to connect to
    Sub init(host As String)
    ' connect to the notifier
    Sub connect()
    ' send a notification
    Sub notify(key As String, value As String)
    ' register for notifications
    Sub register_keys(keys As String())
    Event KeyNotified As EventHandler(Of KeyValueEventArgs)
End Interface
Public Class KeyValueEventArgs
    Inherits EventArgs
    Public Sub New(k As String, v As String)
        Me.key = k
        Me.value = v
    End Sub
    Public Property key() As String
    Public Property value() As String
End Class