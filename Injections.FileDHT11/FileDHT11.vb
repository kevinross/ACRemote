' Reads temperature and humidity from sysfs file on linux
' Author: Kevin Ross

Imports System.IO
Imports ACRemote

Public Class FileDHT11
    Inherits DHT11
    Public Shared base_path As String = "/sys/bus/iio/devices/iio:device0/"
    Public Shared temp_part As String = "in_temp_input"
    Public Shared humid_part As String = "in_humidityrelative_input"
    Protected Overrides Sub update()
        Me.temp_ = get_part(temp_part)
        Me.humid_ = get_part(humid_part)
    End Sub
    Private Function get_part(part As String) As Integer
        Dim passed As Boolean = False
        While Not passed
            Try
                Dim lines As String = File.ReadAllText(base_path + part)
                passed = True
                Return Int32.Parse(lines.Trim())
            Catch generatedExceptionName As IOException
                System.Threading.Thread.Sleep(10)
            End Try
        End While
        Return 0
    End Function
End Class