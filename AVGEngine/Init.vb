Module Init
    Public Sub main()
        Dim MainForm As Form = New Form1()
        Dim GameControl As OAECtrl = New OAECtrl
        GameControl.Init(MainForm)
    End Sub
End Module
