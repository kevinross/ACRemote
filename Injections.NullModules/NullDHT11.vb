' Null DHT11 module provides get/set of variables, no actual temp/humidity
' Author: Kevin Ross

Imports TinyIoC
Imports ACRemote

Public Class NullDHT11
    Inherits Defaultable
    Implements IDHT11
    Private temp_ As Double, humid_ As Double
    Public Sub New()
    End Sub
    Public Property temperature As Double Implements IDHT11.temperature
        Get
            Console.WriteLine(String.Format("NullDHT11: Get Temp: {0}", temp_))
            Return temp_
        End Get
        Set(value As Double)
            Console.WriteLine(String.Format("NullDHT11: Set Temp: {0}", value))
            temp_ = value
        End Set
    End Property
    Public Property humidity As Double Implements IDHT11.humidity
        Get
            Console.WriteLine(String.Format("NullDHT11: Get Humidity: {0}", humid_))
            Return humid_
        End Get
        Set(value As Double)
            Console.WriteLine(String.Format("NullDHT11: Set Humidity: {0}", value))
            humid_ = value
        End Set
    End Property
End Class