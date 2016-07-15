' Implementation of IRemote running on the RPi
' Author: Kevin Ross

Public Class Remote
    Implements IRemote
    Private temp_sensor As IDHT11
    Private gpio As IPlatformGPIO
    Private notify As INotify
    Private rst As Integer, pwr As Integer, t_down As Integer, t_up As Integer, md As Integer, spd As Integer
    Private power_ As Boolean = False
    Private temp_ As Integer = 76
    Private mode_ As modes = modes.ac
    Private speed_ As speeds = speeds.low
    Public Sub New(settings As IRemoteGpioSettings, temp_sensor As IDHT11, gpio As IPlatformGPIO, notify As INotify)
        Me.gpio = gpio
        Me.temp_sensor = temp_sensor
        Me.notify = notify
        Me.rst = settings.reset
        Me.pwr = settings.power
        Me.t_down = settings.temp_down
        Me.t_up = settings.temp_up
        Me.md = settings.mode
        Me.spd = settings.speed
        setup()
    End Sub
    Private Sub notify_bool(key As String, val As Boolean)
        Me.notify.notify("/ac/state/" + key, If(val, "1", "0"))
    End Sub
    Private Sub notify_str(key As String, val As String)
        Me.notify.notify("/ac/state/" + key, val)
    End Sub
    Private Sub notify_int(key As String, val As Integer)
        Me.notify.notify("/ac/state/" + key, val.ToString())
    End Sub
    Public Property power As Boolean Implements IRemote.power
        Get
            Return power_
        End Get
        Set(value As Boolean)
            Me.go_to_power(value)
            Me.power_ = value
            notify_bool("power", value)
        End Set
    End Property
    ' for all properties != power, always ensure the unit is on when they're written to
    Private Property temp As Integer Implements IRemote.temp
        Get
            Return Me.temp_
        End Get
        Set
            Me.power = True
            Me.go_to_temp(Value)
            Me.temp_ = Value
            notify_int("temp", Value)
        End Set
    End Property
    Public Property mode As modes Implements IRemote.mode
        Get
            Return Me.mode_
        End Get
        Set
            Me.power = True
            Me.go_to_mode(Value)
            Me.mode_ = Value
            notify_str("mode", Value.ToString())
        End Set
    End Property
    Public Property speed As speeds Implements IRemote.speed
        Get
            Return Me.speed_
        End Get
        Set
            Me.power = True
            Me.go_to_speed(Value)
            Me.speed_ = Value
            notify_str("speed", Value.ToString())
        End Set
    End Property
    Public Sub setup()
        For Each p As Integer In New Object() {rst, pwr, t_down, t_up, md, spd}
            Me.gpio.setup(p, direction.out_)
        Next
    End Sub
    Private Sub press(pin As Integer)
        Me.gpio(pin) = True
        System.Threading.Thread.Sleep(250)  ' wait a bit for the remote to catch up
        Me.gpio(pin) = False
    End Sub
    Private Sub press(pin As Integer, times As Integer)
        Dim i As Integer = 0
        While i < times
            press(pin)
            System.Threading.Thread.Sleep(100)
            System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
        End While
    End Sub
    Private Sub go_to_power(power As Boolean)
        If Me.power_ = power Then
            Return
        End If
        press(pwr)
    End Sub
    Private Sub go_to_temp(temp As Integer)
        If temp > Me.temp Then
            press(t_down, temp - Me.temp)   ' one press = one degree change, so abs(desired - actual) gets the number of presses
        ElseIf temp < Me.temp Then
            press(t_up, Me.temp - temp)
        End If
    End Sub
    Private Sub go_to_mode(mode As modes)
        If Me.mode = mode Then
            Return
        End If

        ' ac, dehumid, fan
        ' currently ac, going to...
        If Me.mode = modes.ac Then
            If mode = modes.dehumid Then
                press(md, 1)
            ElseIf mode = modes.fan Then
                press(md, 2)
            End If
        ElseIf Me.mode = modes.dehumid Then
            If mode = modes.fan Then
                press(md, 1)
            ElseIf mode = modes.ac Then
                press(md, 2)
            End If
        ElseIf Me.mode = modes.fan Then
            If mode = modes.ac Then
                press(md, 1)
            ElseIf mode = modes.dehumid Then
                press(md, 2)
            End If
        End If
    End Sub

    Private Sub go_to_speed(speed As speeds)
        If speed = Me.speed Then
            Return
        End If
        ' high, medium, low

        ' currently high, going to...
        If Me.speed = speeds.high Then
            If speed = speeds.medium Then
                press(spd, 1)
            ElseIf speed = speeds.low Then
                press(spd, 2)
            End If
        ElseIf Me.speed = speeds.medium Then
            If speed = speeds.low Then
                press(spd, 1)
            ElseIf speed = speeds.high Then
                press(spd, 2)
            End If
        ElseIf Me.speed = speeds.low Then
            If speed = speeds.high Then
                press(spd, 1)
            ElseIf speed = speeds.medium Then
                press(spd, 2)
            End If
        End If
    End Sub
    ' reset the remote in the event we think it's out of sync with the A/C unit
    ' since the A/C unit gets overridden by the remote's settings, this is only useful
    ' if we think the remote itself is the culprit and not someone pressing buttons on the
    ' A/C unit itself
    Public Sub reset() Implements IRemote.reset
        press(rst, 1)
        power_ = False
        temp_ = 76
        mode_ = modes.ac
        speed_ = speeds.low
        Me.notify_str("reset", "1")
    End Sub
    Public ReadOnly Property actual_temp As Double Implements IRemote.actual_temp
        Get
            Return Me.temp_sensor.temperature
        End Get
    End Property
    Public ReadOnly Property actual_humidity As Double Implements IRemote.actual_humidity
        Get
            Return Me.temp_sensor.humidity
        End Get
    End Property
    Public Sub __set_temp(val As Double) Implements IRemote.__set_temp
        Me.temp_sensor.temperature = val
    End Sub
    Public Sub __set_humidity(val As Double) Implements IRemote.__set_humidity
        Me.temp_sensor.humidity = val
    End Sub
End Class