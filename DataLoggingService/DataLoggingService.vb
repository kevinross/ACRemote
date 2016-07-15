' Polls the temperature sensor regularly and stores the data
' Author: Kevin Ross

Imports System.Data.Entity
Imports ACRemote.API
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

' Create a model class
Public Class EnviroRecord
    <Key>
    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    Public Property ID() As Integer
    Public Property Time() As Date
    Public Property Temp() As Decimal
    Public Property Humidity() As Decimal
End Class
' Create a class for CRUD-ing data
Public Class EnviroContext
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=sqlite")  ' pull the connection string from app.config
    End Sub

    Public Property Records() As DbSet(Of EnviroRecord)
    Protected Overrides Sub OnModelCreating(ByVal modelBuilder As DbModelBuilder)
        ' register the model with the system
        modelBuilder.Entity(Of EnviroRecord)().ToTable("EnviroRecord")
    End Sub
End Class

Public Class DataLoggingService
    ' save one record
    Public Shared Sub CollectRecord(ctx As EnviroContext, cli As RemoteServiceClient)
        Dim rec As EnviroRecord = New EnviroRecord With {.Temp = cli.actual_temp, .Humidity = cli.actual_humidity, .Time = Date.UtcNow()}
        ctx.Records.Add(rec)
        ctx.SaveChanges()
    End Sub
    ' save records in a daemon process
    Public Shared Sub CollectRecords(ctx As EnviroContext, cli As RemoteServiceClient, refreshInterval As Integer)
        While True
            CollectRecord(ctx, cli)
            Threading.Thread.Sleep(refreshInterval * 1000)
        End While
    End Sub
    ' connect to remote and start up the daemon to collect records
    Public Shared Sub Main(args As String())
        Using ctx As New EnviroContext
            Dim json_client As RemoteServiceClient = New RemoteServiceClient(AppSettings.Get(Of String)("sensor_host"))
            Dim interval As Integer = 60
            Try
                interval = AppSettings.Get(Of Integer)("data_interval")
            Catch ex As Exception

            End Try
            If args.Length = 1 Then
                interval = Integer.Parse(args.First)
            End If
            CollectRecords(ctx, json_client, interval)
        End Using
    End Sub
End Class