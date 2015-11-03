' Interface for publishing and subscribing to a notification system
' Author: Kevin Ross

Namespace ACRemote

    Public Interface INotify
        Sub init(host As String)
        Sub connect()
        Sub notify(key As String, value As String)
        Sub register_keys(keys As String())
        Event KeyNotified As EventHandler(Of KeyValueEventArgs)
    End Interface
    Public Class KeyValueEventArgs
        Inherits EventArgs
        Public Sub New(k As String, v As String)
            Me.key = key
            Me.value = v
        End Sub
        Public Property key() As String
        Public Property value() As String
    End Class
End Namespace

