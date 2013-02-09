Public Class Form1
    Dim path As String
    Const script_init = "width|height"
    Const script_window = "bgImage|bgMusic|itemList"
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        path = Application.StartupPath + "\config\"
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim a As String
        Dim t As New OAEScriptEngine(path + "main.ini", path + "script.ini")
        'output.init_main(path + "main.ini")
        'a = output.GetAttr("window", "window", "bgImage")
        'Debug.Write(a)
        t.Preproccess()
    End Sub
End Class
