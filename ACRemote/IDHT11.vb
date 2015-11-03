' Interface for DHT11 temperature sensor
' Author: Kevin Ross

Namespace ACRemote
    Public Interface IDHT11
        Property temperature As Double
        Property humidity As Double
    End Interface
End Namespace

