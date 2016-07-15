' Provides access to app settings in a strongly-typed manner
' Author: Kevin Ross

Imports System.Configuration
Imports System.ComponentModel
Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq


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

Public Class FromJsonClass
    Public conf As JObject
    Public Sub New(path As String)
        Try
            Using sr As StreamReader = New StreamReader(path)
                conf = JObject.Parse(sr.ReadToEnd)
            End Using
        Catch e As Exception
            conf = JObject.Parse("{}")
        End Try
    End Sub
End Class

Public Class AppSettings
    ' pull settings from a JSON file
    Public Shared config As JObject = New FromJsonClass(AppSettings.Get(Of String)("json_settings")).conf
    ' pull a setting from app.config, type-safe
    Public Shared Function [Get](Of T)(key As String) As T
        Dim appSetting = ConfigurationManager.AppSettings(key)
        If String.IsNullOrWhiteSpace(appSetting) Then
            If key.Equals("json_settings") Then
                Return Nothing
            End If
            Dim token As JToken = Nothing
            If AppSettings.config.TryGetValue(key, token) Then
                Return DirectCast(token.ToObject(GetType(T)), T)
            Else
                Throw New AppSettingNotFoundException(key)
            End If
        End If

        ' use generic arg to convert to proper type
        Dim converter = TypeDescriptor.GetConverter(GetType(T))
        Return DirectCast((converter.ConvertFromInvariantString(appSetting)), T)
    End Function
End Class