Public Class OAECtrl
    Dim ScriptFilePath As String = Application.StartupPath + "script\main.ini"
    Dim ScriptI As New OAEScriptEngine
    Dim InitInfo As OAEInitInfo
    Public Sub Init(ByVal ScriptFile As String, ByVal GameForm As Form)
        ScriptI = New OAEScriptEngine(ScriptFilePath)
        InitInfo = ScriptI.GetInitInfo()
        If InitInfo.width > 0 Then
            GameForm.Width = InitInfo.width
        End If
        If InitInfo.height > 0 Then
            GameForm.Height = InitInfo.height
        End If
    End Sub

    Public Sub Main()

    End Sub

End Class