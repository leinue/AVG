Public Class OAECtrl
    Dim ScriptFilePath As String = Application.StartupPath + "config\main.ini"
    Dim ScriptI As New OAEScriptEngine
    Public Function Init(ByVal ScriptFile As String) As Integer

        Return 1
    End Function

End Class