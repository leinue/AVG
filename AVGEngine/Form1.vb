Public Class Form1
    Dim path As String
    Const script_init = "width|height"
    Const script_window = "bgImage|bgMusic|itemList"
    'GetAttr("item","item1","locX")
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        path = Application.StartupPath + "config\script.ini"
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

    End Sub
End Class
