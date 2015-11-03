' Provides access to app settings in a strongly-typed manner
' Author: Kevin Ross

Imports System.Configuration
Imports System.ComponentModel

Namespace ACRemote
    Public Class AppSettingNotFoundException
        Inherits Exception
        Private key As String
        Public Sub New(key As String)
            Me.key = key
        End Sub
        Public Overrides Function ToString() As String
            Return String.Format("App Setting not found: {0}", Me.key)
        End Function
    End Class

    Public Class AppSettings
        Public Shared Function [Get](Of T)(key As String) As T
            Dim appSetting = ConfigurationManager.AppSettings(key)
            If String.IsNullOrWhiteSpace(appSetting) Then
                Throw New AppSettingNotFoundException(key)
            End If
            ' use generic arg to convert to proper type
            Dim converter = TypeDescriptor.GetConverter(GetType(T))
            Return DirectCast((converter.ConvertFromInvariantString(appSetting)), T)
        End Function
    End Class
End Namespace
